<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ManageThresholds.aspx.cs" Inherits="ManageThresholds" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br /><br />
    <div style="font-family:Lucida Sans Unicode, Lucida Grande, Sans-Serif;font-size:14px;color:#003399;text-align:center";><b>Manage Thresholds (admin)</b></div>
    <asp:Panel runat="server" ID="pnlAddEdit">
        <table id="tblManageThresholds">
            <tr>
                <td colspan="4">
                    <asp:ValidationSummary runat="server" ID="VSAddEdit" ValidationGroup="vgAddEdit" ForeColor="Red" DisplayMode="List" />

                    <font color="red"><asp:Literal runat="server" ID="ltMessage"></asp:Literal></font>
                </td>
            </tr>
            <tr>
                <td>
                    Filter
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtfilter_desc" Columns="40"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RVfilter_desc" Display="Dynamic" ControlToValidate="txtfilter_desc" ValidationGroup="vgAddEdit" 
                    ErrorMessage="Please enter Filter" Text="*" />
                    <asp:RegularExpressionValidator runat="server" ID="RGfilter_desc" Display="Dynamic" ControlToValidate="txtfilter_desc" ValidationGroup="vgAddEdit" 
                    ErrorMessage="Please enter AlphaNumeric filter without empty space" Text="*" 
                    ValidationExpression="^[a-zA-Z0-9_]*$" />
                    (underscore spaces)</td>
            </tr>

            <tr>
                <td>
                    Display Name
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtfilter_displayname" Columns="40"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RVfilter_displayname" Display="Dynamic" ControlToValidate="txtfilter_displayname" ValidationGroup="vgAddEdit" 
                    ErrorMessage="Please enter Filter Display Name" Text="*" />
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
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;Percentage
                    <asp:CheckBox runat="server" ID="chkfilter_percentage" />
                </td>                
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;Format String
                    <asp:DropDownList runat="server" ID="ddlfilter_formatstring">
                        <asp:ListItem Text="Select" Value=""></asp:ListItem>
                        <asp:ListItem Text="Currency" Value="{0:C}"></asp:ListItem>
                        <asp:ListItem Text="Decimal" Value="{0.00}"></asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;(for integer, no format required)
                </td>
            </tr>

            <tr>
                <td>
                    YoY Threshold
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtfilter_yoy" Columns="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="CPfilter_yoy" Display="Dynamic" ControlToValidate="txtfilter_yoy" ValidationGroup="vgAddEdit" 
                    ErrorMessage="Please enter a valid Number for YoY Threshold" Text="*" Operator="DataTypeCheck" Type="Double" />
                </td>
            </tr>

            <tr>
                <td>
                    YTD Operator
                </td>
                <td colspan="3">
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
                <td colspan="3">
                    <asp:DropDownList runat="server" ID="ddlfilter_yoy_operator">
                        <asp:ListItem Value="1" Text="<="></asp:ListItem>
                        <asp:ListItem Value="2" Text=">="></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button runat="server" ID="btnSave" Text="Add Threshold" ValidationGroup="vgAddEdit" OnClick="btnSave_Click" />
                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <hr /><br />
    <div id="divManageThresholds">
    <asp:Panel runat="server" ID="pnlThresholds">
        <asp:GridView runat="server" ID="gvThresholds" OnPageIndexChanging="gvThresholds_PageIndexChanging" 
        PageSize="20" AutoGenerateColumns="false" CellPadding="5" AllowPaging="true" 
            OnRowDeleting="gvThresholds_RowDeleting" DataKeyNames="threshold_filter_id"
        OnRowEditing="gvThresholds_RowEditing" HorizontalAlign="Center" HeaderStyle-ForeColor="#003399">
            <AlternatingRowStyle BackColor="LightGray" />
            <Columns>
                <asp:BoundField ReadOnly="true" HeaderText="Threshold Name" DataField="threshold_filter_displayname" />
                <asp:BoundField ReadOnly="true" HeaderText="YTD Threshold" DataField="threshold_filter_ytd" />
                <asp:BoundField ReadOnly="true" HeaderText="YTD Operator" DataField="threshold_filter_ytd_operator" />
                <asp:BoundField ReadOnly="true" HeaderText="YTD Format String" DataField="threshold_filter_formatstring" />
                <asp:BoundField ReadOnly="true" HeaderText="YTD Percentage" DataField="threshold_filter_percentage" />
                <asp:BoundField ReadOnly="true" HeaderText="YoY Threshold" DataField="threshold_filter_yoy" />
                <asp:BoundField ReadOnly="true" HeaderText="YoY Operator" DataField="threshold_filter_yoy_operator" />
                <asp:CommandField EditText="Edit" DeleteText="Delete" ShowDeleteButton="true" ShowEditButton="true"/>
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