using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class ManageThresholdUsers : System.Web.UI.Page
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

            int edit_filter_id = 0;
            if (Request["edit_filter_id"] != null && int.TryParse(Request["edit_filter_id"].ToString(), out edit_filter_id))
            {
                Edit(edit_filter_id);
            }

            if (Request["redirect"] != null)
            {
                ViewState["redirect"] = Request["redirect"];
            }
        }
        ltMessage.Text = string.Empty;
    }

    protected void gvThresholds_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvThresholds.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void gvThresholds_RowEditing(object sender, GridViewEditEventArgs e)
    {
        int filter_id = (int)gvThresholds.DataKeys[e.NewEditIndex].Value;
        Edit(filter_id);
        e.Cancel = true;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            
            if (ViewState["edit_filter_id"] != null && (int)ViewState["edit_filter_id"] > 0)
            {
                DataRow dr = ProfileMetric.GetThreshold((int)ViewState["edit_filter_id"]);

                if (dr != null)
                {
                    if (dr != null)
                    {
                        if (txtfilter_ytd.Text == string.Empty)
                        {
                            dr["threshold_filter_ytd"] = System.DBNull.Value;
                        }
                        else
                        {
                            dr["threshold_filter_ytd"] = decimal.Parse(txtfilter_ytd.Text);
                        }

                        if (txtfilter_yoy.Text == string.Empty)
                        {
                            dr["threshold_filter_yoy"] = System.DBNull.Value;
                        }
                        else
                        {
                            dr["threshold_filter_yoy"] = decimal.Parse(txtfilter_yoy.Text);
                        }
                        
                        dr["threshold_filter_ytd_operator"] = int.Parse(ddlfilter_ytd_operator.SelectedValue);
                        dr["threshold_filter_yoy_operator"] = int.Parse(ddlfilter_yoy_operator.SelectedValue);
                    }

                    dr.AcceptChanges();
                }

                ltMessage.Text = "Threshold update successful";

            }
            else
            {
                ltMessage.Text = "Error occurred while saving changes";
            }

            if (ViewState["redirect"] != null)
            {
                Response.Redirect(ViewState["redirect"].ToString());
            }
            else
            {
                Clear();
                BindGrid();
            }
        }
    }
    #endregion

    #region Private Methods
    private void Edit(int filter_id)
    {
        DataRow dr = ProfileMetric.GetThreshold(filter_id);

        if (dr != null)
        {

            ltfilter_displayname.Text = dr["threshold_filter_displayname"].ToString();

            txtfilter_ytd.Text = dr["threshold_filter_ytd"].ToString();
            txtfilter_yoy.Text = dr["threshold_filter_yoy"].ToString();

            if (dr["threshold_filter_ytd_operator"] != System.DBNull.Value &&
                                        ddlfilter_ytd_operator.Items.FindByValue(dr["threshold_filter_ytd_operator"].ToString()) != null)
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
        pnlAddEdit.Visible = true;
    }
    private void Clear()
    {
        ViewState["edit_filter_id"] = null;
        txtfilter_ytd.Text = string.Empty;
        txtfilter_yoy.Text = string.Empty;
        ddlfilter_ytd_operator.SelectedIndex = 0;
        ddlfilter_yoy_operator.SelectedIndex = 0;

        pnlAddEdit.Visible = false;
    }
    private void BindGrid()
    {
        gvThresholds.DataSource = ProfileMetric.GetThresholdData(true); //sets values to current user session or database
        gvThresholds.DataBind();
    }

    #endregion
}