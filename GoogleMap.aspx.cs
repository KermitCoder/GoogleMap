using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Text;

public partial class GoogleMap : System.Web.UI.Page
{
    private class LatLong
    {
        #region LatLong Private Variables
        string _lat = string.Empty;
        string _lng = string.Empty;
        string _name = string.Empty;
        string _address = string.Empty;
        string _phone = string.Empty;
        string _tin = string.Empty;
        bool _contracted = false;
        string _suffix = string.Empty;
        string _msa = string.Empty;
        string _icon = string.Empty;
        string _currentyear = string.Empty;
        string _previousyear = string.Empty;

        List<ProfileMetric> _lstpm = null;
        #endregion

        #region LatLong Constructors
        private LatLong()
        {
        }

        public LatLong(string tin, string suffix, string lat, string lng, string name, string address, string phone, string msa, string icon, bool contracted, string currentyear, string previousyear)
        {
            _lat = lat;
            _lng = lng;
            _name = name;
            _address = address;
            _phone = phone;
            _tin = tin;
            _suffix = suffix;
            _msa = msa;
            _icon = icon;
            _contracted = contracted;
            _currentyear = currentyear;
            _previousyear = previousyear;
        }
        #endregion

        #region LatLong Public Properties
        public List<ProfileMetric> MetricList
        {
            get { return _lstpm; }
            set { _lstpm = value; }
        }
        public string TIN
        {
            get { return _tin; }
        }

        public string SUFFIX
        {
            get { return _suffix; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Address
        {
            get { return _address; }
        }

        public string Phone
        {
            get { return _phone; }
        }

        public string MSA
        {
            get { return _msa; }
        }

        public string Lat
        {
            get { return _lat; }
        }

        public string Lng
        {
            get { return _lng; }
        }

        public string MarkerIcon
        {
            get { return _icon; }
        }

        public bool Contracted
        {
            get { return _contracted; }
        }

        public string CurrentYear
        {
            get { return _currentyear; }
        }

        public string PreviousYear
        {
            get { return _previousyear; }
        }
        #endregion
    }

    #region Private Variables
    //Change connection string at web.config or create a seperate class for data access
    private static string _connectionString = (ConfigurationManager.ConnectionStrings["PVTDB"] != null) ?
        ConfigurationManager.ConnectionStrings["PVTDB"].ToString() : string.Empty;

    private static string _googleGeoAPI = "http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor={1}";

    //public static string _iconGreen = "http://www.google.com/intl/en_us/mapfiles/ms/micons/green-dot.png";
    //public static string _iconRed = "http://www.google.com/intl/en_us/mapfiles/ms/micons/red-dot.png";
    public static string _iconGreen = "PVTCommon/images/green-bldg.png";
    public static string _iconRed = "PVTCommon/images/red-bldg.png";
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Based on the requirements, this could be converted to a control
            GenerateMap();
            ddlMSA.Attributes.Add("onchange", "ReDraw('" + ddlMSA.ClientID + "')");
            InitMetricFilters();
        }
    }
    #endregion

    #region Private Methods
    private void GenerateMap()
    {
        DataSet ds = GetTopProviders();

        if (ds != null && ds.Tables.Count > 0)
        {

            List<LatLong> lst = new List<LatLong>();

            foreach (DataRowView dr in ds.Tables[0].DefaultView)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append((dr["Address1"] != null) ? Server.UrlEncode(dr["Address1"].ToString().ToUpper()) : string.Empty);
                //sb.Append((dr["Address2"] != null) ? Server.UrlEncode(dr["Address2"].ToString().ToUpper()) : string.Empty);
                sb.Append((dr["City"] != null && dr["City"].ToString() != string.Empty) ? ",+" + Server.UrlEncode(dr["City"].ToString().ToUpper()) : string.Empty);
                sb.Append((dr["State"] != null && dr["State"].ToString() != string.Empty) ? ",+" + Server.UrlEncode(dr["State"].ToString().ToUpper()) : string.Empty);

                string name = (dr["Provider"] != null) ? dr["Provider"].ToString().ToUpper() : string.Empty;
                string phone = (dr["Phone"] != null) ? dr["Phone"].ToString() : string.Empty;
                string tin = (dr["TIN9"] != null) ? dr["TIN9"].ToString() : string.Empty;
                string suffix = (dr["SUFFIX"] != null) ? dr["SUFFIX"].ToString() : string.Empty;
                string icon = _iconGreen;
                string msa = (dr["MSA"] != null) ? dr["MSA"].ToString() : string.Empty;
                string currentyear = (dr["Current_Year"] != null) ? dr["Current_Year"].ToString() : string.Empty;
                string previousyear = (dr["Previous_Year"] != null) ? dr["Previous_Year"].ToString() : string.Empty;
                string lat = (dr["lat"] != null) ? dr["lat"].ToString() : string.Empty;
                string lng = (dr["long"] != null) ? dr["long"].ToString() : string.Empty;

                bool contracted = false;
                if (dr["Contracted"] != System.DBNull.Value)
                {
                    bool.TryParse(dr["Contracted"].ToString(), out contracted);
                }

                //Currently Thresholds are hardcoded into ProfileMetric.cs class under App_Code
                //If they are coming from class, it could be set here
                //If anything not in control, icon is red

                //Thresholds:
                List<ProfileMetric> metriclist = ProfileMetric.InitMetricList();
                ProfileMetric.UpdateMetricWithData(metriclist, ds.Tables[0], dr.Row);

                //Not needed as per the new business logic
                /*foreach (ProfileMetric pm in metriclist)
                {
                    //If any of the Metrics is not in control, icon is red
                    if (pm.incontrol.HasValue && pm.incontrol.Value == false)
                    {
                        icon = _iconRed;
                    }
                }*/

                LatLong latlng = new LatLong(tin, suffix, lat, lng, name, Server.UrlDecode(sb.ToString()), phone, msa, icon, contracted, currentyear, previousyear);

                if (latlng.Lat != string.Empty && latlng.Lng != string.Empty)
                {
                    latlng.MetricList = metriclist;
                    lst.Add(latlng);
                }
            }

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJSON = oSerializer.Serialize(lst);


            if (lst.Count > 0)
            {
                ltinitmap.Text = "<script language=\"javascript\" type=\"text/javascript\">var latlngarr = " + sJSON + ";</script>" + Environment.NewLine;
                ltinitmap.Text += string.Format("<script language=\"javascript\" type=\"text/javascript\">var map = initialize(latlngarr);</script>");
            }
            else
            {
                //daily query limit can trigger this else condition
                ltinitmap.Text = "<script language=\"javascript\" type=\"text/javascript\">alert('No Providers Found');</script>";
            }
        }
    }

    private DataSet GetTopProviders()
    {
        if (_connectionString != string.Empty)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            cmd.CommandTimeout = 90;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.udsp_PVT_Select_YoY_Differential_By_Year";

            da.SelectCommand = cmd;
            da.Fill(ds);

            return ds;
        }
        else
        {
            throw new Exception("Add PVTDB Connection string at the web.config");
        }
    }

    private LatLong GetLatLong(string tin, string suffix, string address, string name, string phone, string msa, string icon, bool contracted, string currentyear, string previousyear)
    {
        try
        {
            string apiurl = string.Format(_googleGeoAPI, address, "false");
            string respstring = GetResponse(apiurl);
            StringReader sdr = new StringReader(respstring);

            DataSet ds = new DataSet();

            ds.ReadXml(sdr);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables["location"] != null && ds.Tables["location"].Rows.Count > 0
                && ds.Tables["location"].Rows[0]["lat"] != null
                && ds.Tables["location"].Rows[0]["lng"] != null)
            {
                return new LatLong(tin, suffix, ds.Tables["location"].Rows[0]["lat"].ToString(), ds.Tables["location"].Rows[0]["lng"].ToString(), name, Server.UrlDecode(address), phone, msa, icon, contracted, currentyear, previousyear);
            }
            else
            {
                //Some error with GEO coding API for this address
                //Add handler for this based on business rules
                return new LatLong("", "", "", "", "", "", "", "", "", false, "", "");
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            return new LatLong("", "", "", "", "", "", "", "", "", false, "", "");
        }

    }

    private string GetResponse(string url)
    {
        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);

        HttpWebResponse response = (HttpWebResponse)req.GetResponse();
        Stream receiveStream = response.GetResponseStream();
        StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

        string outstr = readStream.ReadToEnd();

        response.Close();
        readStream.Close();

        return outstr;
    }

    private void InitMetricFilters()
    {
        //added checkboxes to treeview - BW, 4/15/2012.
        Literal list;
        string listItem = "";

        List<ProfileMetric> metriclist = ProfileMetric.InitMetricList();
        var listOfUls = new List<string>();

        foreach (ProfileMetric pm in metriclist)
        {
            foreach (ProfileMetric.MetricThreshold mt in pm.thresholds)
            {
                /*CheckBox chk = new CheckBox();
                chk.ID = "chk" + pm.columnname + ((int)mt.ThresholdType).ToString();
                chk.Attributes.Add("threshold", mt.ToString());
                chk.Attributes.Add("onclick", "ReDraw('" + ddlMSA.ClientID + "')");
                //chk.Style.Add("margin", "5px");
                
                chk.Text = "";*/
                string chkbox = "<span threshold=\"" + mt.ToString() + "\"><input type='checkbox' onclick=\"ReDraw('" + ddlMSA.ClientID + "')\"  id=\"ContentPlaceHolder1_chk" + pm.columnname + ((int)mt.ThresholdType).ToString() + "\" name=\"ContentPlaceHolder1_chk" + pm.columnname + ((int)mt.ThresholdType).ToString() + "\"></span>";

                var lt = new Literal();
                var thrstr = (mt.ThresholdType == ProfileMetric.MetricThreshold.ThresholdTypes.InControl) ? string.Empty :
                                                        "(" + mt.Threshold.ToString("0.00") + ")";
                var editlnk = (mt.ThresholdType == ProfileMetric.MetricThreshold.ThresholdTypes.InControl) ? string.Empty :
                    "<a href=\"ManageThresholdUsers.aspx?redirect=GoogleMap.aspx&edit_filter_id=" + pm.thresholdid + "\">Edit</a>";

                lt.Text = "<span style=\"font-size:12px;\">" + pm.displayname + " (" + mt.ThresholdType.ToString() + ") " + thrstr + " " +
                    editlnk + "</span>";

                //Change this line for Master Page

                listItem += "<li><span>" + chkbox + " " + lt.Text + "</span></li>";

                //Following was the original code
                //dvDropDowns.Controls.Add(chk);
                //dvDropDowns.Controls.Add(lt);
                //dvDropDowns.Controls.Add(new System.Web.UI.HtmlControls.HtmlGenericControl("br"));
            }

            listOfUls.Add("<li class='expandable'><span style=\"font-size:12px;\">" + pm.columnname + "</span><ul>" + listItem + "</ul></li>");
            listItem = "";
        }

        string liText = "";

        foreach (var listOfUl in listOfUls)
        {
            //liText += "<li class='collapsable'> " + listOfUl + " </li>";
            liText += listOfUl;
        }

        list = new Literal { Text = "<ul id='browser' class='treeview'>" + liText + "</ul><br/>" };
        dvDropDowns.Controls.Add(list);        
    }
    #endregion
}