<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="_Authorization_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="//netdna.bootstrapcdn.com/twitter-bootstrap/2.3.1/css/bootstrap-combined.min.css"
        rel="stylesheet">
</head>
<body>
    <div id="header">
        <div class="navbar">
            <div class="navbar-inner">
                <div class="container">
                    <a class="brand" href="#">Sport Agent Site</a>
                </div>
            </div>
        </div>
    </div>
    <div id="main">
        <div class="container">
            <div class="site_login">
                <h3>
                    Agent Login</h3>
                <h5>
                </h5>
                <form id="form1" runat="server">
                <div class="control-group">
                    <label for="username" class="control-label">
                        username</label>
                    <div class="controls">
                        <asp:TextBox ID="username" runat="server" placeholder="username"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label for="password" class="control-label">
                        password</label>
                    <div class="controls">
                        <asp:TextBox ID="password" runat="server" placeholder="password"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    
                    <asp:Button ID="Button1"
                        runat="server" Text="Login"  class="btn btn-primary" 
                        onclick="Button1_Click" />
                </div>
                </form>
            </div>
        </div>
        <!-- /container -->
    </div>
</body>
</html>

<%--[4:16:19 PM] Suncity - Sams: <script src="http://demo.acsdev.net/partner/Content/tgp_auth-0.0.2.js" type="text/javascript"></script>
<script type="text/javascript">

    TGP.authorize_endpoint = "http://demo.acsdev.net/scoauth/oauth/authorize";

    TGP.authorize({
      client_id: "[your_app_id]",  // = app_id
      redirect_uri: "[your_url]",
      state: "",
    });
</script>
[4:16:31 PM] Suncity - Sams: need jquery--%>
