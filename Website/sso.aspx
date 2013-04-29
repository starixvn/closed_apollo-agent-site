<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sso.aspx.cs" Inherits="sso"
    EnableViewState="false" EnableTheming="false" EnableViewStateMac="false" %>

<%@ Import Namespace=" Apollo.Age.Integration.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sports</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        sdfasfasfasdfds
    </div>
    </form>
    <script src="_GlobalResources/Js/jquery.js" type="text/javascript"></script>
      <script src="<%=OAuthSettings.Settings.TgpScriptSrc %>" type="text/javascript"></script>
    <script type="text/javascript">
        TGP.authorize_endpoint = "<%=OAuthSettings.Settings.AuthorizeUrl%>";

        TGP.authorize({
            client_id: "<%=OAuthSettings.Settings.AppId %>",
            redirect_uri: "<%=@OAuthSettings.Settings.RedirectUri %>",
            state: "active"
        });
    </script>    
    
</body>
</html>
