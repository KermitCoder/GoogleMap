using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Globalization;

public class ProfileMetric
{
    #region Private variables
    private static string _connectionString = (ConfigurationManager.ConnectionStrings["PVTDB"] != null) ?
        ConfigurationManager.ConnectionStrings["PVTDB"].ToString() : string.Empty;
    #endregion

    #region Threshold Class
    public class MetricThreshold
    {
        #region Enum
        public enum ThresholdTypes
        {
            YTD = 1,
            YoY = 2,
            InControl = 3
        }

        public enum ThresholdOperatorTypes
        {
            LTE = 1,
            GTE = 2
        }
        #endregion

        #region Public Properties
        public decimal Threshold
        {
            get;
            set;
        }

        public ThresholdTypes ThresholdType
        {
            get;
            set;
        }

        public ThresholdOperatorTypes ThresholdOperatorType
        {
            get;
            set;
        }

        public bool InControl
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public MetricThreshold(decimal thresh, ThresholdTypes threshtype, ThresholdOperatorTypes threshotype)
        {
            this.Threshold = thresh;
            this.ThresholdType = threshtype;
            this.ThresholdOperatorType = threshotype;
        }
        #endregion
    }
    #endregion

    #region MetricValue
    public class MetricValue
    {
        public string displayvalue { get; set; }
        public object value { get; set; }
        public bool? incontrol { get; set; } 
        public MetricThreshold threshold { get; set; }

        public MetricValue(object val, MetricThreshold thresh, bool perc, string displayformatstring)
        {
            this.value = val;

            if (displayformatstring != null && displayformatstring.Trim() != string.Empty)
            {
                try
                {
                    //this.displayvalue = val.ToString(); //NOTE: uncomment this line for quick load
                    this.displayvalue = (perc && val != System.DBNull.Value) ? string.Format(displayformatstring, val) + "%" : string.Format(displayformatstring, (decimal)val);                                        
                    
                    //NOTE: The above line, specifically the decimal cast, is causing many firstchance exceptions which is causing a longer load time. 
                    //The displayformatstring is what gives us a currency value, which is assigned in the PVT_Threshold_Filters table i.e. {0:C}.
                    //Basically, what the float values coming through are giving us a problem with being casted to strings.
                    //TEST: float.TryParse("0.58", NumberStyles.Any, CultureInfo.InvariantCulture, out f);
                }
                catch { this.displayvalue = (perc && val != System.DBNull.Value) ? val.ToString() + "%" : val.ToString(); }
                //NOTE: We're taking in large float types, so this exception will be thrown but then handled.
            }
            else
            {
                this.displayvalue = (perc && val != System.DBNull.Value) ? val.ToString() + "%" : val.ToString();
            }

            this.threshold = thresh;

            if (this.threshold != null)
            {
                decimal tempval = 0.0m;

                decimal.TryParse(val.ToString(), out tempval);

                if (tempval < 0)
                {
                    tempval = -1 * tempval;
                }

                if (threshold.ThresholdOperatorType == MetricThreshold.ThresholdOperatorTypes.GTE)
                {
                    this.incontrol = (tempval >= threshold.Threshold);
                }
                else if (threshold.ThresholdOperatorType == MetricThreshold.ThresholdOperatorTypes.LTE)
                {
                    this.incontrol = (tempval <= threshold.Threshold);
                }
            }
        }
    }
    #endregion

    #region Properties
    //static values initialized/defines values to display
    public int thresholdid { get; set; }
    public string columnname { get; set; }
    public string displayname { get; set; }
    public string displayformatstring { get; set; }
    //public string charttype { get; set; }
    public bool percentage { get; set; }
    public List<MetricThreshold> thresholds;

    //dynamic values
    public MetricValue currentvalue { get; set; }
    public MetricValue previousvalue { get; set; }
    public MetricValue difference { get; set; }
    public bool incontrolrule1 { get; set; }
    public bool incontrolrule2 { get; set; }
    public bool incontrolrule3 { get; set; }
    public bool incontrolrule { get; set; }
    #endregion

    #region Constructors
    public ProfileMetric(int id, string colname, string dispname, bool perc)
    {
        thresholdid = id;
        columnname = colname;
        displayname = dispname;
        //charttype = ctype;
        percentage = perc;
        thresholds = new List<MetricThreshold>();
    }
    #endregion

    #region Static methods
    public static void DeleteThreshold(int filter_id)
    {
        if (_connectionString != string.Empty)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            cmd.Connection = conn;
            cmd.CommandTimeout = 90; 
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.udsp_PVT_Delete_Threshold_Filters";
            cmd.Parameters.Add("@filter_id", SqlDbType.Int, 4).Value = filter_id;

            cmd.ExecuteNonQuery();
        }
        else
        {
            throw new System.Exception("Add PVTDB Connection string at the web.config");
        }
    }

    public static DataRow GetThreshold(int filter_id)
    {
        DataSet ds = ProfileMetric.GetThresholdData(true);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows.Find(filter_id);

            return dr;
        }

        return null;
    }

    public static void InsertThreshold(string filter_desc 
                                    ,string filter_displayname
                                    ,decimal? filter_ytd
                                    ,decimal? filter_yoy 
                                    ,bool filter_percentage
                                    ,string filter_formatstring
                                    ,int filter_ytd_operator
                                    ,int filter_yoy_operator)
    {
        if (_connectionString != string.Empty)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.udsp_PVT_Save_Threshold_Filters";
            cmd.Parameters.Add("@threshold_filter_desc", SqlDbType.VarChar, 50).Value = filter_desc;
            cmd.Parameters.Add("@threshold_filter_displayname", SqlDbType.VarChar, 50).Value = filter_displayname;
            cmd.Parameters.Add("@threshold_filter_ytd_operator", SqlDbType.Int, 4).Value = filter_ytd_operator;
            cmd.Parameters.Add("@threshold_filter_yoy_operator", SqlDbType.Int, 4).Value = filter_yoy_operator;
            cmd.Parameters.Add("@threshold_filter_ytd", SqlDbType.Decimal).Value = filter_ytd;
            cmd.Parameters.Add("@threshold_filter_yoy", SqlDbType.Decimal).Value = filter_yoy;
            cmd.Parameters.Add("@threshold_filter_percentage", SqlDbType.Bit).Value = filter_percentage;
            cmd.Parameters.Add("@threshold_filter_formatstring", SqlDbType.VarChar, 50).Value = filter_formatstring;

            cmd.ExecuteNonQuery();
        }
        else
        {
            throw new System.Exception("Add PVTDB Connection string at the web.config");
        }
    }

    public static void UpdateThreshold(
                                    int filter_id
                                    , string filter_desc
                                    , string filter_displayname
                                    , decimal? filter_ytd
                                    , decimal? filter_yoy
                                    , bool filter_percentage
                                    , string filter_formatstring
                                    , int filter_ytd_operator
                                    , int filter_yoy_operator)
    {
        if (_connectionString != string.Empty)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.udsp_PVT_Save_Threshold_Filters";
            cmd.Parameters.Add("@threshold_filter_id", SqlDbType.Int, 4).Value = filter_id;
            cmd.Parameters.Add("@threshold_filter_desc", SqlDbType.VarChar, 50).Value = filter_desc;
            cmd.Parameters.Add("@threshold_filter_displayname", SqlDbType.VarChar, 50).Value = filter_displayname;
            cmd.Parameters.Add("@threshold_filter_ytd_operator", SqlDbType.Int, 4).Value = filter_ytd_operator;
            cmd.Parameters.Add("@threshold_filter_yoy_operator", SqlDbType.Int, 4).Value = filter_yoy_operator;
            cmd.Parameters.Add("@threshold_filter_ytd", SqlDbType.Decimal).Value = filter_ytd;
            cmd.Parameters.Add("@threshold_filter_yoy", SqlDbType.Decimal).Value = filter_yoy;
            cmd.Parameters.Add("@threshold_filter_percentage", SqlDbType.Bit).Value = filter_percentage;
            cmd.Parameters.Add("@threshold_filter_formatstring", SqlDbType.VarChar, 50).Value = filter_formatstring;

            cmd.ExecuteNonQuery();
        }
        else
        {
            throw new System.Exception("Add PVTDB Connection string at the web.config");
        }
    }

    public static DataSet GetThresholdData(bool cached)

    //If the current session has data cached, grab session values set by the user and pass
    //those into the DataSet. Otherwise, if we use a database connection (connectionString
    //is not empty), then we grab the values from the database. This is used in the 
    //BindGrid() for the ManageThresholdUsers and ManageThresholds code-behind classes.
    {
        if (cached && HttpContext.Current.Session["ThresholdData"] != null)
        {
            return (DataSet)HttpContext.Current.Session["ThresholdData"];
        }
        else
        {
            if (_connectionString != string.Empty)
            {
                SqlConnection conn = new SqlConnection(_connectionString);
                SqlCommand cmd = new SqlCommand();
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();

                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.udsp_PVT_Threshold_Filters";

                da.SelectCommand = cmd;
                da.Fill(ds);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataColumn dc = ds.Tables[0].Columns["threshold_filter_id"];
                    DataColumn[] keys = new DataColumn[1];
                    keys[0] = dc;
                    ds.Tables[0].PrimaryKey = keys;
                }

                HttpContext.Current.Session["ThresholdData"] = ds;
                return ds;
            }
            else
            {
                throw new System.Exception("Add PVTDB Connection string at the web.config");
            }
        }
    }
    public static List<ProfileMetric> InitMetricList()
    {
        List<ProfileMetric> lst = new List<ProfileMetric>();
        DataSet ds = GetThresholdData(true);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string colname = (dr["threshold_filter_desc"] == System.DBNull.Value) ?
                    "Filter" + dr["threshold_filter_id"].ToString() : dr["threshold_filter_desc"].ToString();
                string dispname = (dr["threshold_filter_displayname"] == System.DBNull.Value) ?
                    "Filter " + dr["threshold_filter_id"].ToString() : dr["threshold_filter_displayname"].ToString();
                bool perc = (bool)dr["threshold_filter_percentage"];

                ProfileMetric pm = new ProfileMetric((int)dr["threshold_filter_id"], colname, dispname, perc);

                pm.displayformatstring = dr["threshold_filter_formatstring"].ToString().Trim();

                if (dr["threshold_filter_ytd"] != System.DBNull.Value)
                {
                    pm.thresholds.Add(new MetricThreshold(decimal.Parse(dr["threshold_filter_ytd"].ToString())
                        , MetricThreshold.ThresholdTypes.YTD
                        , (int.Parse(dr["threshold_filter_ytd_operator"].ToString()) == 1) ?
                            MetricThreshold.ThresholdOperatorTypes.GTE : MetricThreshold.ThresholdOperatorTypes.LTE));
                }

                if (dr["threshold_filter_yoy"] != System.DBNull.Value)
                {
                    pm.thresholds.Add(new MetricThreshold(decimal.Parse(dr["threshold_filter_yoy"].ToString())
                        , MetricThreshold.ThresholdTypes.YoY
                        , (int.Parse(dr["threshold_filter_yoy_operator"].ToString()) == 1) ?
                            MetricThreshold.ThresholdOperatorTypes.GTE : MetricThreshold.ThresholdOperatorTypes.LTE));
                }
                // InControl is last in the loop for ProfileMetric pm list
                pm.thresholds.Add(new MetricThreshold(0.0m
                        , MetricThreshold.ThresholdTypes.InControl
                        , MetricThreshold.ThresholdOperatorTypes.LTE));

                lst.Add(pm);
            }
        }        
        return lst;
    }

    public static void UpdateMetricWithData(List<ProfileMetric> lst, DataTable dt, DataRow dr)
    {
        foreach (ProfileMetric pm in lst)
        {
            if (dt.Columns.Contains(pm.columnname + "_Current")
                && dt.Columns.Contains(pm.columnname + "_Previous")
                && dt.Columns.Contains(pm.columnname + "_Difference"))
            {
                //If YTD Threshold is defined
                MetricThreshold mt = pm.thresholds.Find(mtemp => mtemp.ThresholdType == MetricThreshold.ThresholdTypes.YTD);
                if (mt != null)
                {
                    pm.currentvalue = new MetricValue(dr[pm.columnname + "_Current"], mt, pm.percentage,pm.displayformatstring);
                    pm.previousvalue = new MetricValue(dr[pm.columnname + "_Previous"], mt, pm.percentage, pm.displayformatstring);

                    /*mt.InControl = (!pm.currentvalue.incontrol.HasValue || pm.currentvalue.incontrol.Value) &&
                                (!pm.previousvalue.incontrol.HasValue || pm.previousvalue.incontrol.Value);*/
                    mt.InControl = (!pm.currentvalue.incontrol.HasValue || pm.currentvalue.incontrol.Value);
                }
                else
                {
                    pm.currentvalue = new MetricValue(dr[pm.columnname + "_Current"], null, pm.percentage, pm.displayformatstring);
                    pm.previousvalue = new MetricValue(dr[pm.columnname + "_Previous"], null, pm.percentage, pm.displayformatstring);
                }

                //If YoY Threshold is defined
                mt = pm.thresholds.Find(mtemp => mtemp.ThresholdType == MetricThreshold.ThresholdTypes.YoY);
                pm.difference = new MetricValue(dr[pm.columnname + "_Difference"], mt, true, string.Empty);
                if (mt != null)
                {
                    mt.InControl = (!pm.difference.incontrol.HasValue || pm.difference.incontrol.Value);
                }

                mt = pm.thresholds.Find(mtemp => mtemp.ThresholdType == MetricThreshold.ThresholdTypes.InControl);
                if (mt != null)
                {
                    mt.InControl = (dr[pm.columnname + "_In_Control"] == System.DBNull.Value) ? false : (bool)dr[pm.columnname + "_In_Control"];
                }

                //InControl Threshold is defined
                //reverse logic: s/b == true but bit values in database are switched for true/false
                pm.incontrolrule1 = (dt.Columns.Contains(pm.columnname + "ControlRule1") &&
                    (dr[pm.columnname + "ControlRule1"] != System.DBNull.Value
                     && bool.Parse(dr[pm.columnname + "ControlRule1"].ToString()) == false)
                    );

                pm.incontrolrule2 = (dt.Columns.Contains(pm.columnname + "ControlRule2") &&
                    (dr[pm.columnname + "ControlRule2"] != System.DBNull.Value
                     && bool.Parse(dr[pm.columnname + "ControlRule2"].ToString()) == false)
                    );

                pm.incontrolrule3 = (dt.Columns.Contains(pm.columnname + "ControlRule3") &&
                    (dr[pm.columnname + "ControlRule3"] != System.DBNull.Value
                     && bool.Parse(dr[pm.columnname + "ControlRule3"].ToString()) == false)
                    );

                pm.incontrolrule = pm.incontrolrule1 && pm.incontrolrule2 && pm.incontrolrule3;
            }
        }
    }
    #endregion
}