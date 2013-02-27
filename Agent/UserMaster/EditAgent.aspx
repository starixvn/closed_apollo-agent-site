<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditAgent.aspx.vb" Inherits="EditAgent" %>
<%@ OutputCache Location="None" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
	<head>
		<title></title>
		<link href='../../App_Themes/AgencyV2/popup.css?<%= AppSettings.JsVersion %>' type='text/css' rel='stylesheet' />
        <link href='../../App_Themes/AgencyV2/newagent.css?<%= AppSettings.JsVersion %>' type='text/css' rel='stylesheet' />
        <link href='../../App_Themes/AgencyV2/main.css?<%= AppSettings.JsVersion %>' type='text/css' rel='stylesheet' />
		<style type="text/css">
		    form#form { width: 100%; height: 100%;}
			.balloon { BORDER-RIGHT: aqua 8px solid; BORDER-TOP: aqua 8px solid; VISIBILITY: hidden; BORDER-LEFT: aqua 8px solid; BORDER-BOTTOM: aqua 8px solid; POSITION: absolute; BACKGROUND-COLOR: aqua }
			SPAN.mp { FONT-WEIGHT: bold; FONT-SIZE: 8pt; COLOR: #ffffff }
		</style>

        <script type="text/javascript" src="../../Js/opt/Core.js?<%= AppSettings.JsVersion %>"></script>
        <script type="text/javascript" src="../../Js/Common.js?<%= AppSettings.JsVersion %>"></script>
		<script src="../../_Components/AGEWnd/AGEWnd.js?<%= AppSettings.JsVersion %>" type="text/javascript"></script>
		<script src="../../Js/Customer.js?<%= AppSettings.JsVersion %>" type="text/javascript"></script>
		<script src="../../Js/EditMaster.js?<%= AppSettings.JsVersion %>" type="text/javascript"></script>
		<script src="../../Js/AddNewAgent.js?<%= AppSettings.JsVersion %>" type="text/javascript"></script>

		<%VariableOfScript()%>
        <script type="text/javascript">
            AGE.addEvent(window, 'load', GetCustID);
        </script>
	</head>
	<body>
	    <link id="lnkNobackground" runat="server" href='../../App_Themes/AgencyV2/userinfo.css' type='text/css' rel='stylesheet' />
		<iframe id="frUpdate" name="frUpdate" style="DISPLAY:none"></iframe>
		<input type="hidden" name="roleId" id="roleId" value="3">
		<input type="hidden" id="hidEdit" name="hidEdit" value="1">
        <input type="hidden" id="saveControlDisabled" name="saveControlDisabled">
        <input type="hidden" id="maxMemberPT" name="maxMemberPT" value="0" />
		<form id="form" name="form" method="post" action="UpdateEditAgent.aspx" target="frUpdate">
			 <input type="hidden" id="saveCustid" name="saveCustid" runat="server">
			 <input type="hidden" id="saveUsername" name="saveUsername" runat="server">
			 <input type="hidden" id="isAllowUpd" name="isAllowUpd" runat="server">
			 <input type="hidden" id="isMult" name="isMult" value="0" runat="server">
			 <input type="hidden" id="isUpdateMember" name="isUpdateMember" value="0" />
			 <input type="hidden" id="memberPT" name="memberPT" />
			<asp:Literal id="htmlEditAgentContent" runat="server"></asp:Literal>
		</form>
		<%=disabledBtn%>
		<%=script %>
	</body>
</html>