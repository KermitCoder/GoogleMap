<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ManageThresholdUsers.aspx.cs" Inherits="ManageThresholdUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br /><br />
    <div style="font-family:Lucida Sans Unicode, Lucida Grande, Sans-Serif;font-size:14px;color:#003399;text-align:center"><b>Manage Thresholds (user)</b></div>
    <asp:Panel runat="server" ID="pnlAddEdit" Visible="false">
        <table id="tblManageThresholdUsers">
            <tr>
                <td colspan="2">
                    <asp:ValidationSummary runat="server" ID="VSAddEdit" ValidationGroup="vgAddEdit" ForeColor="Red" DisplayMode="List" />

                    <font color="red"><asp:Literal runat="server" ID="ltMessage"></asp:Literal></font>
                </td>
            </tr>
            <tr>
                <td>
                    Filter Name
                </td>
                <td>
                    <asp:Literal runat="server" ID="ltfilter_displayname" ></asp:Literal>
                </td>
            </tr>

            <tr>
                <td>
                    YTD Threshold
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtfilter_ytd" Columns="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="CPfilter_ytd" Display="Dynamic" ControlToValidate="txtfilter_ytd" ValidationGroup="vgAddEdit" 
                    ErrorMessage="Please enter a valid Number for YTD Threshold" Text="*" Operator="DataTypeCheck" Type="Double" />
                </td>
            </tr>

            <tr>
                <td>
                    YoY Threshold
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtfilter_yoy" Columns="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="CPfilter_yoy" Display="Dynamic" ControlToValidate="txtfilter_yoy" ValidationGroup="vgAddEdit" 
                    ErrorMessage="Please enter a valid Number for YoY Threshold" Text="*" Operator="DataTypeCheck" Type="Double" />
                </td>
            </tr>

            <tr>
                <td>
                    YTD Operator
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlfilter_ytd_operator">
                        <asp:ListItem Value="1" Text="<="></asp:ListItem>
                        <asp:ListItem Value="2" Text=">="></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td>
                    YoY Operator
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlfilter_yoy_operator">
                        <asp:ListItem Value="1" Text="<="></asp:ListItem>
                        <asp:ListItem Value="2" Text=">="></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnSave" Text="Save Threshold" ValidationGroup="vgAddEdit" OnClick="btnSave_Click" />
                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <div id="divManageThresholdUsers">
    <asp:Panel runat="server" ID="pnlThresholds">
        <asp:GridView runat="server" ID="gvThresholds" OnPageIndexChanging="gvThresholds_PageIndexChanging" 
        PageSize="20" AutoGenerateColumns="false" CellPadding="5" AllowPaging="true" DataKeyNames="threshold_filter_id"
        OnRowEditing="gvThresholds_RowEditing" HorizontalAlign="Center" HeaderStyle-ForeColor="#003399">
            <HeaderStyle/>
            <AlternatingRowStyle BackColor="LightGray" />
            <Columns>
                <asp:BoundField ReadOnly="true" HeaderText="Threshold Name" DataField="threshold_filter_displayname" />
                <asp:BoundField ReadOnly="true" HeaderText="YTD Threshold" DataField="threshold_filter_ytd" />
                <asp:BoundField ReadOnly="true" HeaderText="YTD Operator" DataField="threshold_filter_ytd_operator" />               
                <asp:BoundField ReadOnly="true" HeaderText="YoY Threshold" DataField="threshold_filter_yoy" />
                <asp:BoundField ReadOnly="true" HeaderText="YoY Operator" DataField="threshold_filter_yoy_operator" />
                <asp:CommandField ShowEditButton="true" />
            </Columns>
        </asp:GridView>
    </asp:Panel><br />
    <div style="padding-top: 4px; padding-left: 0px; float: left; text-align: left; font-size: x-small; font-family: Arial, Helvetica, sans-serif; width: 778px; position: relative; top: 0px; left: 0px;">
        <asp:HyperLink ID="HyperLink1" runat="server" Font-Size="x-small"
            NavigateUrl="~/GoogleMap.aspx" style="text-align: left" Target="_blank">Home</asp:HyperLink>
        &nbsp;&nbsp;
        <asp:HyperLink ID="HyperLink2" runat="server" Font-Size="x-small"
            NavigateUrl="~/ManageThresholds.aspx" style="text-align: left" Target="_blank">Manage Thresholds (admin)</asp:HyperLink>
        &nbsp;&nbsp;
        <asp:HyperLink ID="HyperLink3" runat="server" Font-Size="x-small"
            NavigateUrl="~/ManageThresholdUsers.aspx" style="text-align: left" Target="_blank">Manage Thresholds (user)</asp:HyperLink>         
        &nbsp;<br /><br />
        <% Response.WriteFile("Version.inc"); %>
    </div> 
    </div>
</asp:Content>