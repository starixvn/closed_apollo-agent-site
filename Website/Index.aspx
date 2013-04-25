<%@ Page Language="C#" AutoEventWireup="false" EnableSessionState="ReadOnly" CodeFile="Index.aspx.cs" Inherits="Index" %>

<!DOCTYPE html>
<!--[if IE 7 ]> <html class="ie7 ie"> <![endif]-->
<!--[if IE 8 ]> <html class="ie8 ie"> <![endif]-->
<!--[if IE 9 ]> <html class="ie9 ie"> <![endif]-->
<!--[if (gt IE 9)]><!--> <html class="ie10 ie"> <!--<![endif]-->
<!--[if !(IE)]> <html><![endif]-->
<head>
    <title></title>
    <link href="<%= Page.ResolveUrl("~/_GlobalResources/Css/Index.less")%>" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        // For caching in child frames
        window.cachePage = new Array();
        window.cacheData = new Array();
    </script>
</head>
<body>
    <form id="agentSite" runat="server">
        <div class="header">
            <iframe src="Account/TopBar" id="frHeader" name="frHeader"
                width="100%" height="100%" frameborder="0" scrolling="no"></iframe>
        </div>
        <div class="menu">
            <iframe src="Account/Menu" id="menu" name="menu"
                width="100%" height="99%" frameborder="0" scrolling="no"></iframe>
        </div>
        <div class="mainpage">
            <iframe src="<%=mainPage%>" id="main" name="main"
                width="100%" height="100%" frameborder="0"></iframe>
        </div>
    </form>
    <input type="hidden" id="ApoRoot" value="<%= Page.ResolveUrl("~/")%>" />
    <script src="<%= Page.ResolveUrl("_GlobalResources/Js/jquery.js")%>" type="text/javascript"></script>
    <script src="<%= Page.ResolveUrl("_GlobalResources/Js/Core.js")%>" type="text/javascript"></script>
    
</body>
</html>