using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class ManageThresholds : System.Web.UI.Page
{
    #region Private Variables
    private static string _connectionString = (ConfigurationManager.ConnectionStrings["PVTDB"] != null) ?
        ConfigurationManager.ConnectionStrings["PVTDB"].ToString() : string.Empty;
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gvThresholds.PageIndex = 0;
            BindGrid();
        }

        if (ViewState["edit_filter_id"] != null && (int)ViewState["edit_filter_id"] > 0)
        {
            btnSave.Text = "Save Threshold";
        }
        else
        {
            //if (txtfilter_desc.Text != string.Empty)
            //{ 
            //    btnSave.Attributes.Add("onclick", "javascript:alert('Data for new metric should be loaded before adding new threshold!')"); 
            //}
            btnSave.Text = "Add Threshold";
        }
        ltMessage.Text = string.Empty;
    }

    protected void gvThresholds_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvThresholds.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void gvThresholds_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (e.Keys.Count > 0)
        {
            int filter_id = (int)e.Keys[0];
            ProfileMetric.DeleteThreshold(filter_id);
            BindGrid();

            ltMessage.Text = "Threshold delete successful";
        }
    }

    protected void gvThresholds_RowEditing(object sender, GridViewEditEventArgs e)
    {
        int filter_id = (int)gvThresholds.DataKeys[e.NewEditIndex].Value;

        DataRow dr = ProfileMetric.GetThreshold(filter_id);

        if (dr != null)
        {
            txtfilter_desc.Text = dr["threshold_filter_desc"].ToString();
            txtfilter_displayname.Text = dr["threshold_filter_displayname"].ToString();
            txtfilter_ytd.Text = dr["threshold_filter_ytd"].ToString();
            txtfilter_yoy.Text = dr["threshold_filter_yoy"].ToString();
            if (ddlfilter_formatstring.Items.FindByValue(dr["threshold_filter_formatstring"].ToString()) !=null)
            {
                ddlfilter_formatstring.SelectedValue = dr["threshold_filter_formatstring"].ToString();
            }

            chkfilter_percentage.Checked = (dr["threshold_filter_percentage"] != System.DBNull.Value &&
                (bool)dr["threshold_filter_percentage"] == true);

            if (dr["threshold_filter_ytd_operator"] != System.DBNull.Value &&
                                        ddlfilter_ytd_operator.Items.FindByValue(dr["threshold_filter_ytd_operator"].ToString()) !=null)
            {
                ddlfilter_ytd_operator.SelectedValue = dr["threshold_filter_ytd_operator"].ToString();
            }

            if (dr["threshold_filter_yoy_operator"] != System.DBNull.Value &&
                                        ddlfilter_yoy_operator.Items.FindByValue(dr["threshold_filter_yoy_operator"].ToString()) != null)
            {
                ddlfilter_yoy_operator.SelectedValue = dr["threshold_filter_yoy_operator"].ToString();
            }
        }

        ViewState["edit_filter_id"] = filter_id;
        e.Cancel = true;

        btnSave.Text = "Save Threshold";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            decimal? threshold_filter_ytd = null;
            decimal? threshold_filter_yoy = null;

            if (txtfilter_ytd.Text != string.Empty)
            {
                threshold_filter_ytd = decimal.Parse(txtfilter_ytd.Text);
            }

            if (txtfilter_yoy.Text != string.Empty)
            {
                threshold_filter_yoy = decimal.Parse(txtfilter_yoy.Text);
            }

            if (ViewState["edit_filter_id"] == null)
            {
                ProfileMetric.InsertThreshold(txtfilter_desc.Text
                                            , txtfilter_displayname.Text
                                            , threshold_filter_ytd
                                            , threshold_filter_yoy
                                            , chkfilter_percentage.Checked
                                            , ddlfilter_formatstring.SelectedValue
                                            , int.Parse(ddlfilter_ytd_operator.SelectedValue)
                                            , int.Parse(ddlfilter_yoy_operator.SelectedValue));

                ltMessage.Text = "Threshold addition successful";

            }
            else if (ViewState["edit_filter_id"] != null && (int)ViewState["edit_filter_id"] > 0)
            {
                ProfileMetric.UpdateThreshold((int)ViewState["edit_filter_id"]
                                            , txtfilter_desc.Text
                                            , txtfilter_displayname.Text
                                            , threshold_filter_ytd
                                            , threshold_filter_yoy
                                            , chkfilter_percentage.Checked
                                            , ddlfilter_formatstring.SelectedValue
                                            , int.Parse(ddlfilter_ytd_operator.SelectedValue)
                                            , int.Parse(ddlfilter_yoy_operator.SelectedValue));

                ltMessage.Text = "Threshold update successful";

            }
            else
            {
                ltMessage.Text = "Error Occurred Saving changes";
            }
            Clear();
            BindGrid();
        }
    }
    #endregion

    #region Private Methods
    private void Clear()
    {
        ViewState["edit_filter_id"] = null;
        txtfilter_desc.Text = string.Empty;
        txtfilter_displayname.Text = string.Empty;
        txtfilter_ytd.Text = string.Empty;
        txtfilter_yoy.Text = string.Empty;
        ddlfilter_formatstring.SelectedIndex = 0;
        chkfilter_percentage.Checked = false;
        ddlfilter_ytd_operator.SelectedIndex = 0;
        ddlfilter_yoy_operator.SelectedIndex = 0;

        btnSave.Text = "Add Threshold";
    }
    private void BindGrid()
    {
        gvThresholds.DataSource = ProfileMetric.GetThresholdData(false); //sets values to current user session or database
        gvThresholds.DataBind();
    }
    #endregion
}