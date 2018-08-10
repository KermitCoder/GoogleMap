using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;

public partial class ChartMetric : System.Web.UI.Page
{
    #region Private Property
    private string TIN
    {
        get
        {
            string _tin = string.Empty;

            if (Request["TIN"] != null)
            {
                _tin = Request["TIN"];
            }

            return _tin;
        }
    }

    private string MSA
    {
        get
        {
            string _msa = string.Empty;

            if (Request["MSA"] != null)
            {
                _msa = Request["MSA"];
            }

            return _msa;
        }
    }

    private string Suffix
    {
        get
        {
            if (Request["Suffix"] != null) 
            { 
                return Request["Suffix"]; 
            }
            else 
            { 
                return string.Empty; }
        }
    }
   
    private string ChartType
    {
        get
        {
            string _charttype = string.Empty;

            if (Request["charttype"] != null)
            {
                _charttype = Request["charttype"];
            }

            return _charttype;
        }
    }
    #endregion

    #region Private Variables
    private static string _connectionString = (ConfigurationManager.ConnectionStrings["PVTDB"] != null) ?
        ConfigurationManager.ConnectionStrings["PVTDB"].ToString() : string.Empty;
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Populate();
            MetricChart.ChartAreas["MainChartArea"].AxisY.IsStartedFromZero = false;
            //MetricChart.ChartAreas["MainChartArea"].RecalculateAxesScale();

            /*
            // set logarithmic scale
            metricchart.chartareas["mainchartarea"].axisy.islogarithmic = logarithmic.checked;

            if (logarithmic.checked)
            {
                autoscale.enabled = false;
                autoscale.checked = true;
            }
            else
            {
                autoscale.enabled = true;
            }
            // set auto scale.
            if (autoscale.checked)
            {
                // set auto minimum and maximum values.
                metricchart.chartareas["mainchartarea"].axisy.minimum = double.nan;
                metricchart.chartareas["mainchartarea"].axisy.maximum = double.nan;
                maximum.enabled = false;
                minimum.enabled = false;
            }
            else
            {
                // set manual minimum and maximum values.
                metricchart.chartareas["mainchartarea"].axisy.minimum = double.parse(minimum.selecteditem.value);
                metricchart.chartareas["mainchartarea"].axisy.maximum = double.parse(maximum.selecteditem.value);
                maximum.enabled = true;
                minimum.enabled = true;
            }
            */
        }
    }

    protected void MetricChart_DataBound(object sender, EventArgs e)
    {
        /* Begin Default Data points */
      
        //if (MetricChart.Series["Series2"].Points.Count > 0)
        //{
        //    int ct = MetricChart.Series["Series2"].Points.Count - 1;
        //    string val = MetricChart.Series["Series2"].Points[ct].YValues[0].ToString("0.00");
        //    MetricChart.Series["Series2"].Points[ct].Label = "Center line = " + val;
        //    MetricChart.Series["Series2"].Points[ct].LabelBackColor = System.Drawing.Color.White;
        //    MetricChart.Series["Series2"].Points[ct].LabelForeColor = System.Drawing.Color.Black;
        //}
        //if (MetricChart.Series["Series3"].Points.Count > 0)
        //{
        //    int ct = MetricChart.Series["Series3"].Points.Count - 1;
        //    string val = MetricChart.Series["Series3"].Points[ct].YValues[0].ToString("0.00");
        //    MetricChart.Series["Series3"].Points[ct].Label = "UCL = " + val;
        //    MetricChart.Series["Series3"].Points[ct].LabelBackColor = System.Drawing.Color.White;
        //    MetricChart.Series["Series3"].Points[ct].LabelForeColor = System.Drawing.Color.Black;
        //}
        //if (MetricChart.Series["Series4"].Points.Count > 0)
        //{
        //    int ct = MetricChart.Series["Series4"].Points.Count - 1;
        //    string val = MetricChart.Series["Series4"].Points[ct].YValues[0].ToString("0.00");
        //    MetricChart.Series["Series4"].Points[ct].Label = "LCL = " + val;
        //    MetricChart.Series["Series4"].Points[ct].LabelBackColor = System.Drawing.Color.White;
        //    MetricChart.Series["Series4"].Points[ct].LabelForeColor = System.Drawing.Color.Black;
        //}
        
        /* End Default Data points */
    }
    
    #endregion

    #region Private Methods
    private void Populate()
    {
        ltMetric.Text = GetDisplayNameForMetric();
        DataSet ds = GetData();

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ltProvider.Text = (ds.Tables[0].Rows[0]["Provider"] != null) ? ds.Tables[0].Rows[0]["Provider"].ToString() : string.Empty;

            MetricChart.Series["Series1"].XValueMember = "Current_MonthName";
            MetricChart.Series["Series1"].YValueMembers = GetColumnNameForMetric();
            

            if (ds.Tables[0].Columns.Contains(ChartType + "SL_avg"))
            {
                MetricChart.Series["Series2"].XValueMember = "Current_MonthName";
                MetricChart.Series["Series2"].YValueMembers = ChartType + "SL_avg";                
            }

            if (ds.Tables[0].Columns.Contains(ChartType + "USL"))
            {
                MetricChart.Series["Series3"].XValueMember = "Current_MonthName";
                MetricChart.Series["Series3"].YValueMembers = ChartType + "USL";
            }

            if (ds.Tables[0].Columns.Contains(ChartType + "LSL"))
            {
                MetricChart.Series["Series4"].XValueMember = "Current_MonthName";
                MetricChart.Series["Series4"].YValueMembers = ChartType + "LSL";
            }

            MetricChart.DataSource = ds;
            MetricChart.DataBind();

            /* Begin fake data points for right side label extension */
            int max = 2;

            for (int i = 0; i < max; i++)
            {
                if (MetricChart.Series["Series2"].Points.Count > 0)
                {
                    int ct = MetricChart.Series["Series2"].Points.Count;
                    string val = MetricChart.Series["Series2"].Points[ct - 1].YValues[0].ToString("0.00");

                    MetricChart.Series["Series2"].Points.Insert(ct, new DataPoint(0.00, val));

                    MetricChart.Series["Series2"].Points[ct].Color = System.Drawing.Color.White;
                    if (i == max - 1)
                    if (i == 1)
                    {
                        MetricChart.Series["Series2"].Points[ct].Label = "Center line = " + val;
                        MetricChart.Series["Series2"].Points[ct].LabelBackColor = System.Drawing.Color.White;
                        MetricChart.Series["Series2"].Points[ct].LabelForeColor = System.Drawing.Color.Black;
                    }
                }
                if (MetricChart.Series["Series3"].Points.Count > 0)
                {
                    int ct = MetricChart.Series["Series3"].Points.Count;
                    string val = MetricChart.Series["Series3"].Points[ct - 1].YValues[0].ToString("0.00");

                    MetricChart.Series["Series3"].Points.Insert(ct,
                            new DataPoint(0.00, val));

                    MetricChart.Series["Series3"].Points[ct].Color = System.Drawing.Color.White;
                    if (i == 1)
                    {
                        MetricChart.Series["Series3"].Points[ct].Label = "UCL = " + val;
                        MetricChart.Series["Series3"].Points[ct].LabelBackColor = System.Drawing.Color.White;
                        MetricChart.Series["Series3"].Points[ct].LabelForeColor = System.Drawing.Color.Black;
                        MetricChart.ChartAreas["MainChartArea"].AxisX.MajorGrid.Enabled = false;
                        MetricChart.ChartAreas["MainChartArea"].AxisY.MajorGrid.Enabled = false;                      
                    }
                }
                if (MetricChart.Series["Series4"].Points.Count > 0)
                {
                    int ct = MetricChart.Series["Series4"].Points.Count;
                    string val = MetricChart.Series["Series4"].Points[ct - 1].YValues[0].ToString("0.00");

                    MetricChart.Series["Series4"].Points.Insert(ct,
                            new DataPoint(0.00, val));

                    MetricChart.Series["Series4"].Points[ct].Color = System.Drawing.Color.White;
                    if (i == 1)
                    {
                        MetricChart.Series["Series4"].Points[ct].Label = "LCL = " + val;
                        MetricChart.Series["Series4"].Points[ct].LabelBackColor = System.Drawing.Color.White;
                        MetricChart.Series["Series4"].Points[ct].LabelForeColor = System.Drawing.Color.Black;
                    }
                }
            }           
            /* End fake data points for label extension */
        }
        else
        {
            Response.Write("No Chart Data found");
            //Response.Redirect("GoogleMap.aspx");
        }
    }

    private DataSet GetData()
    {
        if (_connectionString != string.Empty)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.udsp_PVT_Select_Provider_Profile_Metrics_Line_Chart";
            cmd.Parameters.Add("@tin", SqlDbType.NVarChar, 9).Value = TIN;
            cmd.Parameters.Add("@msa", SqlDbType.NVarChar, 50).Value = MSA;
            //cmd.Parameters.Add("@suffix", SqlDbType.NVarChar, 5).Value = Suffix;

            da.SelectCommand = cmd;
            da.Fill(ds);

            return ds;
        }
        else
        {
            throw new Exception("Add PVTDB Connection string at the web.config");
        }
    }

    private string GetColumnNameForMetric()
    {
        return ChartType + "_Current";
    }

    private string GetDisplayNameForMetric()
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
            ds.Tables[0].DefaultView.RowFilter = "threshold_filter_desc = '" + ChartType + "'";

            if (ds.Tables[0].DefaultView.Count > 0)
            {
                return ds.Tables[0].DefaultView[0]["threshold_filter_displayname"].ToString();
            }
        }

        return string.Empty;
    }
    #endregion
}