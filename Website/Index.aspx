<%@ Page Language="C#" AutoEventWireup="false" EnableSessionState="ReadOnly" CodeFile="Index.aspx.cs" Inherits="Index" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <link href="<%= Page.ResolveUrl("~/_GlobalResources/Css/Index.less")%>" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        // For caching in child frames
        window.cachePage = new Array();
        window.cacheData = new Array();

        //window.baseURL = '<%=Page.ResolveUrl("~/")%>';
    </script>
</head>
<body>
    <form id="agentSite" runat="server">
        <div class="header">
            <iframe src="_Header/Header.aspx" id="frHeader" name="frHeader"
                width="100%" height="100%" frameborder="0" scrolling="no"></iframe>
        </div>
        <div class="menu">
            <iframe src="Account/Menu" id="menu" name="menu"
                width="100%" height="100%" frameborder="0" scrolling="no"></iframe>
        </div>
        <div class="mainpage">
            <iframe src="<%=mainPage%>" id="main" name="main"
                width="100%" height="100%" frameborder="0"></iframe>
        </div>
    </form>
    <script src="<%= Page.ResolveUrl("_GlobalResources/Js/jquery.js")%>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("_GlobalResources/Js/Core.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            setInterval(APOLLO.reCheckIn, 45000);
        });
    </script>
</body>
</html>