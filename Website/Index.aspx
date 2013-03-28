<%@ Page Language="C#" AutoEventWireup="false" EnableSessionState="ReadOnly" CodeFile="Index.aspx.cs" Inherits="Index" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <link href="<%= Page.ResolveUrl("~/_GlobalResources/Css/Index.less")%>" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function $(id) {
            return document.getElementById(id);
        }

        var url = window.location.href;
        var start = url.indexOf('//');
        var end = url.indexOf('/(');

        window.document.title = url.substr(start + 2, end - start - 2);

        // For caching in child frames
        window.cachePage = new Array();
        window.cacheData = new Array();

        // For showing message in child frames
        window.errCodes = new Array();
        window.errMsgs = new Array();
        window.userNames = new Array();

        window.baseURL = "<%=baseURL %>";
    </script>
</head>
<body>
    <form id="agentSite" runat="server">
        <div class="header">
            <iframe src="_Header/Header.aspx" frameborder="0" scrolling="no" width="100%" height="100%"
                id="frHeader" name="frHeader"></iframe>
        </div>
        <div class="menu">
            <iframe src="_Menu/Menu.aspx" frameborder="0" id="menu" name="menu" width="100%"
                height="100%" marginwidth="0" marginheight="0" allowtransparency="yes"></iframe>
        </div>
        <div class="mainpage">
            <iframe src="<%=mainPage%>" onload="setURL();" frameborder="0" id="main" scrolling="auto"
                name="main" width="100%" height="100%" marginwidth="0" marginheight="0" allowtransparency="yes">
            </iframe>
        </div>
        <div class="footer">
            <iframe src="Footer.html" frameborder="0" width="100%" scrolling="no" height="100%"
                id="frFooter" name="frFooter"></iframe>
        </div>
    </form>
    <script src="<%= Page.ResolveUrl("_GlobalResources/Js/Core.js")%>" type="text/javascript"></script>

    <script type="text/javascript">

        AGE.PERMISSIONS = '<%= (string)(Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess) %>';

        function setURL() {
            var url = top.main.location;
            var start = url.toString().lastIndexOf(')/') + 2;
            var currentURL = url.toString().substring(start);

            if (currentURL == null) return;
            setCookie('<%=CookieCurrentURLKey %>', currentURL, 1);
        }

        AGE.prototype._SignOut = AGE.prototype.SignOut;
        // SignOut
        AGE.prototype.SignOut = function () {
            setCookie('<%=CookieCurrentURLKey %>', '', -1);
            this._SignOut();
        }
    </script>
</body>
</html>