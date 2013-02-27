<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditMinPT.aspx.vb" Inherits="EditMinPT" %>
<%@ OutputCache Location="None" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title></title>
		<link href="../../App_Themes/AgencyV2/newagent.css?<%= AppSettings.JsVersion %>" type="text/css" rel="Stylesheet" />
		<link href="../../App_Themes/AgencyV2/popup.css?<%= AppSettings.JsVersion %>" type="text/css" rel="Stylesheet" />
        <style type="text/css">
			.right { TEXT-ALIGN: right }
			SPAN.mp { FONT-WEIGHT: bold; FONT-SIZE: 8pt; COLOR: #ffffff }
			table.tblRpt tr{height:21px;}
		</style>
		<script type="text/javascript" src="../../Js/opt/jx.js?<%= AppSettings.JsVersion %>"></script>
        <script type="text/javascript" src="../../Js/opt/Core.js?<%= AppSettings.JsVersion %>"></script>
        <script type="text/javascript" src="../../Js/Common.js?<%= AppSettings.JsVersion %>"></script>
		<script type="text/javascript" src="../../Js/Customer.js?<%= AppSettings.JsVersion %>"></script>
		<script type="text/javascript" src="../../Js/MasterEditMember.js?<%= AppSettings.JsVersion %>"></script>
        <script src="../_GlobalResources/Js/jx.js?<%= AppSettings.JsVersion %>" type="text/javascript"></script>
         <script type="text/javascript">
             var flag = false;
             function BtnOnClick() {

                 $("btnSubmit").disabled = true;
                 if (!flag && $("isAllowUpdPT").value == "1") {
                     if (parent.ageWnd) {
                         $("showPopup").value = "1";
                         flag = true;
                     }
                     else {
                         $("btnSubmit").disabled = false;
                     }
                     DelayReloadPage(null, 300000);
                     enabledBtn(false);
                     document.form.submit();
                 }
             }
		</script>
	</head>
	<body>
	    <link id="lnkNobackground" runat="server" href='../../App_Themes/AgencyV2/userinfo.css' type='text/css' rel='stylesheet' />
		<iframe id="frUpdate" style="DISPLAY: none" name="frUpdate"></iframe>
		<input type="hidden" id="roleId" name="roleId" value="3" />
		<input type="hidden" id="hidEdit" name="hidEdit" value="1" />
		<form id="form" name="form" method="post" action="UpdateEditMinPT.aspx" target="frUpdate">
			<input id="isAllowUpdPT" name="isAllowUpdPT" type="hidden" value="1" runat="server"/>
			<input id="saveCustid" type="hidden" runat="server" name="saveCustid"/>
			<input id="saveUsername" type="hidden" runat="server" name="saveUsername"/>
            <input id="showPopup" type="hidden" runat="server" name="showPopup" value="0"/>
            <input id="currentUser" type="hidden" runat="server" name="currentUser"/>
			<asp:Literal id="htmlEditMinPT" runat="server"></asp:Literal>
		</form>
		<%=disabledBtn%>
		<%=script%>
	</body>
</html>