<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="GoogleMap.aspx.cs" Inherits="GoogleMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <a name="profile"></a><a href="#map" id="agmap" style="display: none;">View Map</a><br />
    <div id="divProfile" style="display: none;">
        <h1>
            <label id="lblName"></label>
        </h1>
            <label id="lblAddress"></label>
        <br />
            <label id="lblPhone"></label>
        <br />
            <label id="lblTIN"></label>-<label id="lblSUFFIX"></label>
        <br />
        <label id="lblMSA"></label>
        <br />
        <label id="lblContracted"></label>
        <br />
        <center>
            <table id="tblProfile" cellpadding="10px" cellspacing="0" border="1" width="900px;">
            </table>
        </center>
    </div>
    <br />
    <div style="text-align: center; width: 900px;">
        <a name="map"></a>
        <div style="text-align: left; font-family: Lucida Sans Unicode, Lucida Grande, Sans-Serif;
            font-size: 12px;">
            <input type="checkbox" id="chkUseCluster" checked="checked" onclick="ReDraw('<%=ddlMSA.ClientID%>');" />
            Cluster View
        </div>
        <div id="map_canvas" style="width: 590px; height: 600px; float: left; display: inline-block;">
        </div>
        <asp:Literal runat="server" ID="ltinitmap"></asp:Literal>
        <div runat="server" id="dvDropDowns" align="right" style="float: left; display: inline-block;
            margin-left: 10px; text-align: left; font-family: Lucida Sans Unicode, Lucida Grande, Sans-Serif;
            font-size: 10px; height: 600px; width: 300px; overflow: auto;">
            <asp:DropDownList runat="server" ID="ddlMSA" Style="margin: 5px;" DataSourceID="dsMSA"
                DataTextField="MSA_Display_Name" DataValueField="MSA_Database_Name">
            </asp:DropDownList>
            <asp:SqlDataSource ID="dsMSA" runat="server" ConnectionString="<%$ ConnectionStrings:PVTDB %>"
                SelectCommand="SELECT [MSA_Display_Name], [MSA_Database_Name] FROM [PVT_MSA_List] WHERE [active] = 'True'">
            </asp:SqlDataSource>
            <br />
            <br />
            
            <%--<ul id="browser" class="filetree">
                <li><span>Folder 1</span>
                    <ul>
                        <li><span>Item 1.1</span></li>
                    </ul>
                </li>
                <li><span class="folder">Folder 2</span>
                    <ul>
                        <li><span class="folder">Subfolder 2.1</span>
                            <ul id="folder21">
                                <li><span class="file">File 2.1.1</span></li>
                                <li><span class="file">File 2.1.2</span></li>
                            </ul>
                        </li>
                        <li><span class="file">File 2.2</span></li>
                    </ul>
                </li>
                <li class="closed"><span class="folder">Folder 3 (closed at start)</span>
                    <ul>
                        <li><span class="file">File 3.1</span></li>
                    </ul>
                </li>
                <li><span class="file">File 4</span></li>
            </ul>--%>
        </div>
    </div>
    <div style="padding-top: 4px; padding-left: 0px; float: left; text-align: left; font-size: x-small;
        font-family: Arial, Helvetica, sans-serif; width: 778px; position: relative;
        top: 0px; left: 0px;">
        <asp:HyperLink ID="HyperLink1" runat="server" Font-Size="x-small" NavigateUrl="~/GoogleMap.aspx"
            Style="text-align: left" Target="_blank">Home</asp:HyperLink>
        &nbsp;&nbsp;
        <asp:HyperLink ID="HyperLink2" runat="server" Font-Size="x-small" NavigateUrl="~/ManageThresholds.aspx"
            Style="text-align: left" Target="_blank">Manage Thresholds (admin)</asp:HyperLink>
        &nbsp;&nbsp;
        <asp:HyperLink ID="HyperLink3" runat="server" Font-Size="x-small" NavigateUrl="~/ManageThresholdUsers.aspx"
            Style="text-align: left" Target="_blank">Manage Thresholds (user)</asp:HyperLink>
        &nbsp;<br />
        <br />
        <% Response.WriteFile("Version.inc"); %>
    </div>
</asp:Content>
