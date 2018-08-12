<%@ Page Language="C#" %>

<%@ Import Namespace="System.Web.Security" %>

<!-- Simple Forms Authentication for now. Later, if we get a lot of users, I'll implement the 
ASP.NET MEMBERSHIP application service for role-based authentication. BW - 9/15/2011 -->

<script runat="server">
  void Logon_Click(object sender, EventArgs e)
  //added 2/13/2012
  {
      if ((UserID.Text == "AAAAAAA") && (UserPass.Text == "xxxxxxx") ||
          (UserID.Text == "BBBBBBB") && (UserPass.Text == "xxxxxxx") ||
          (UserID.Text == "CCCCCCC") && (UserPass.Text == "xxxxxxx") ||
          (UserID.Text == "DDDDDDD") && (UserPass.Text == "xxxxxxx") ||
          (UserID.Text == "EEEEEEE") && (UserPass.Text == "xxxxxxx") ||
          (UserID.Text == "FFFFFFF") && (UserPass.Text == "xxxxxxx") ||
          (UserID.Text == "GGGGGGG") && (UserPass.Text == "xxxxxxx") ||
          (UserID.Text == "HHHHHHH") && (UserPass.Text == "xxxxxxx") ||
          (UserID.Text == "IIIIIII") && (UserPass.Text == "xxxxxxx") ||
          (UserID.Text == "JJJJJJJ") && (UserPass.Text == "xxxxxxx") ||
          (UserID.Text == "KKKKKKK") && (UserPass.Text == "xxxxxxx")) //my creds
      {
          FormsAuthentication.RedirectFromLoginPage
             (UserID.Text, Persist.Checked);
      }
      else
      {
          Msg.Text = "Invalid credentials. Please try again.";
      }
  }
  
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  <title>Provider Visualization Tool</title>

</head>
<body> 
  <form id="form2" defaultfocus="UserID" runat="server">
  <div style="padding-top: 0px; padding-right: 0px;">
    <img alt="KP" class="style1" src="PVTCommon/images/kp_logo_thrive.jpg" 
          height="46px" width="250px"/>&nbsp;
  </div>
    <table>
        <tr>
            <td style="font-family: Lucida Sans Unicode, Lucida Grande, Sans-Serif; font-size: 12px;"
                align="right">
                UserID:
            </td>
            <td>
                <asp:TextBox ID="UserID" runat="server" Width="150px" />
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="UserID"
                    Display="Dynamic" ErrorMessage="Cannot be empty." runat="server" />
            </td>
        </tr>
        <tr>
            <td style="font-family: Lucida Sans Unicode, Lucida Grande, Sans-Serif; font-size: 12px;"
                align="right">
                Password:
            </td>
            <td>
                <asp:TextBox ID="UserPass" TextMode="Password" runat="server" Width="150px" />
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="UserPass"
                    ErrorMessage="Cannot be empty." runat="server" />
            </td>
        </tr>
        <tr>
            <td style="font-family: Lucida Sans Unicode, Lucida Grande, Sans-Serif; font-size: 12px;">
                Remember me?
            </td>
            <td>
                <asp:CheckBox ID="Persist" runat="server" />
            </td>
        </tr>
    </table>
    <asp:Button ID="Submit1" OnClick="Logon_Click" Text="Log On" runat="server" />
    <p>
        <asp:Label ID="Msg" ForeColor="red" runat="server" />
    </p>
    <div style="padding-top: 4px; float: left; text-align: left; font-size: xx-small;
        font-family: Arial, Helvetica, sans-serif;">
        <% Response.WriteFile("Version.inc"); %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </div>
    <p>
        &nbsp;</p>
    </form>
</body>
</html>
