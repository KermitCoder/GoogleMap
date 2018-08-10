<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
  <title>Provider Visualization Tool</title>
  <link rel="stylesheet" type="text/css" href="PVTCommon/css/Master1.css" />
</head>

<script runat="server">

    void Page_Load(object sender, EventArgs e)
    {
        Welcome.Text = "Hello, " + Context.User.Identity.Name;
    }

    void Signout_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
        Response.Redirect("Logon.aspx");
    }
</script>
<body>
  <asp:Label ID="Welcome" runat="server" />
  <form id="Form1" runat="server">
	<asp:HyperLink ID="HyperLink1" runat="server" Font-Size="x-small"
		NavigateUrl="~/GoogleMap.aspx" style="text-align: left" Target="_self" 
		Font-Names="Verdana">Provider Visualization Tool</asp:HyperLink><p/>

    <asp:Button ID="Submit1" OnClick="Signout_Click" 
       Text="Sign Out" runat="server" /><p/>

    <div style="padding-top: 4px; float: left; text-align: left; font-size: x-small; font-family: Arial, Helvetica, sans-serif;">
               <% Response.WriteFile("Version.inc"); %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </div>

    <p>
        &nbsp;</p>
    </form>
</body>
</html>
