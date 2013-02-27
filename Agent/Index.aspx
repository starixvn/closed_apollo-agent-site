<%@ Page Language="C#" AutoEventWireup="false" EnableSessionState="ReadOnly" CodeFile="Index.aspx.cs" Inherits="Index" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
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

    </script>

    <link href="../_MemberInfo/SetNickName/Resources/SetNickName.css?<%=TplCore.TplSettings.UrlVersion %>" rel="stylesheet" type="text/css" />
    <link href="../_Announcement/Resources/Announcement.css?<%=TplCore.TplSettings.UrlVersion %>" rel="stylesheet" type="text/css" />
    <link href="../_GlobalResources/css/tooltip.css?<%=TplCore.TplSettings.UrlVersion %>" rel="stylesheet" type="text/css" />
</head>
<body scroll='no'>
    <form id="agentSite" runat="server" style="height: 100%; width: 100%;">
    <table border="0" cellpadding="0" cellspacing="0" class="content">
        <tr>
            <td colspan="2" class="header">
                <iframe src="_Header/Header.aspx" frameborder="0" scrolling="no" width="100%" height="100%" id="frHeader" name="frHeader"></iframe>
            </td>
        </tr>
        <tr>
            <td class="menu" valign="top">
                <iframe src="_Menu/Menu.aspx" frameborder="0" id="menu" name="menu" width="100%" height="100%" marginwidth="0" marginheight="0" allowtransparency="yes"></iframe>
            </td>
            <td class="mainpage" valign="top">
                <iframe src="<%=mainPage%>" onload="setURL();" frameborder="0" id="main" scrolling="auto" name="main" width="100%" height="100%" marginwidth="0" marginheight="0" allowtransparency="yes"></iframe>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="footer">
                <iframe src="../Footer.html" frameborder="0" width="100%" scrolling="no" height="100%" id="frFooter" name="frFooter"></iframe>
            </td>
        </tr>
    </table>
    </form>
</body>

<script src="../_GlobalResources/Js/Core.js?<%=TplCore.TplSettings.UrlVersion %>" type="text/javascript"></script>

<script src="../_MemberInfo/SetNickName/Resources/SetNickName.js?<%=TplCore.TplSettings.UrlVersion %>" type="text/javascript"></script>

<script src="../_Announcement/Resources/Announcement.js?<%=TplCore.TplSettings.UrlVersion %>" type="text/javascript"></script>

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
    AGE.prototype.SignOut = function() {
        setCookie('<%=CookieCurrentURLKey %>', '', -1);
        this._SignOut();
    }
</script>

</html>