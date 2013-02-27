<%@ Page Language="C#" AutoEventWireup="false" EnableSessionState="ReadOnly" EnableViewState="false" CodeFile="PositionTakingCS.aspx.cs" Inherits="PositionTakingCS" %>
<%@ OutputCache Location="None" %>
<html>
<head>
    <title><%= Resources.Lang.updateCS %></title>
    <link href="../../App_Themes/AgencyV2/casino.css?<%= AppSettings.JsVersion %>" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/AgencyV2/main.css?<%= AppSettings.JsVersion %>" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/AgencyV2/button.css?<%= AppSettings.JsVersion %>" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/AgencyV2/PopUpMessage.css?<%= AppSettings.JsVersion %>" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/AgencyV2/common.css?<%= AppSettings.JsVersion %>" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/AgencyV2/header.css?<%= AppSettings.JsVersion %>" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/AgencyV2/newagent.css?<%= AppSettings.JsVersion %>" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/AgencyV2/popup.css?<%= AppSettings.JsVersion %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Js/opt/Core.js?<%= AppSettings.JsVersion %>"></script>
    <script type="text/javascript" src="../../Js/Common.js?<%= AppSettings.JsVersion %>"></script>
    <script type="text/javascript" src="../../Js/opt/jx.js?<%= AppSettings.JsVersion %>"></script>
    <script type="text/javascript" src="../../Js/jQuery/jquery.js?<%= AppSettings.JsVersion %>"></script>
    <script type="text/javascript" src="../../Js/jshashtable.js?<%= AppSettings.JsVersion %>"></script>
    <script type="text/javascript" src="../../Js/Casino.js?<%= AppSettings.JsVersion %>&pack=1"></script>
    <script type="text/javascript" src="../../Js/MemberInfo/CasinoPT.js?<%= AppSettings.JsVersion %>"></script>
</head>
<body>
    <iframe name="InsertData" id="InsertData" style="display: none"></iframe>
    <form id="frmUpdatePT" action="../Handlers/SynCasino.ashx?custid=<%= (Request["cscustid"] + Request["custid"]) %>&insert=<%= isInserting.ToString().ToLower() %>" method="post" target="InsertData">
        <asp:Literal ID="ctrHTML" EnableViewState="false" runat="server"></asp:Literal>
        <input type="hidden" id="bIsMult" name="bIsMult" value="<%= Request["isMult"] %>" />
    </form>
</body>
</html>