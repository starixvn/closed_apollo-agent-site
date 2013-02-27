<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditMember.aspx.vb" Inherits="MasterEditMember" %>
<%@ OutputCache Location="None" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
	<HEAD>
		<title></title>
		<link href="../../App_Themes/AgencyV2/newagent.css?<%= AppSettings.JsVersion %>" type="text/css" rel="Stylesheet" />
		<link href="../../App_Themes/AgencyV2/popup.css?<%= AppSettings.JsVersion %>" type="text/css" rel="Stylesheet" />  		
		
		<style type="text/css">
			.right { TEXT-ALIGN: right }
			SPAN.mp { FONT-WEIGHT: bold; FONT-SIZE: 8pt; COLOR: #ffffff }
			table.tblRpt tr{height:21px;}
		</style>
		
        <script type="text/javascript" src="../../Js/opt/Core.js?<%= AppSettings.JsVersion %>"></script>
        <script type="text/javascript" src="../../Js/Common.js?<%= AppSettings.JsVersion %>"></script>
		<script type="text/javascript" src="../../Js/Customer.js?<%= AppSettings.JsVersion %>"></script>		
		<script type="text/javascript" src="../../Js/MasterEditMemberAuto.js?<%= AppSettings.JsVersion %>"></script>
		<script type="text/javascript" src="../../Js/ArrangeCheckBox_EditMultipleMember.js?<%=AppSettings.JsVersion %>"></script>

		<script type="text/javascript">
		    var flag = false;
		    function BtnOnClick() {

		        if ($("isAllowUpdPT").value == "0" || flag) {
		            $("btnSubmit").disabled = true;
		            $("btnSubmit").style.color = "#cccccc";
		        }
		        else {
		            flag = true;
		            DelayReloadPage(null, 300000);
		            enabledBtn(false);
		            document.form.submit();
		        }
		    }			
		</script>		
	</HEAD>
	<body onload="GetCustID();InitDisableControl();">
	    <link id="lnkNobackground" runat="server" href='../../App_Themes/AgencyV2/userinfo.css' type='text/css' rel='stylesheet' />        
		<iframe name="frUpdate" id="frUpdate" style="DISPLAY:none"></iframe>
		<input type="hidden" id="roleId" name="roleId" value="3"/>			
		<form id="form" name="form" method="post" action="UpdateEditMember.aspx" target="frUpdate">
			<input id="isAllowUpdPT" name="isAllowUpdPT" type="hidden" runat="server"/> 
			<input id="isMult" name="isMult" type="hidden" runat="server"/>
			<input id="saveCustid" type="hidden" runat="server" NAME="saveCustid"/> 
			<input id="saveUsername" type="hidden" runat="server" NAME="saveUsername"/>
			<input id="saveAgentid" type="hidden" runat="server" NAME="saveAgentid"/>
			<asp:Literal id="htmlSuperEditMember" runat="server"></asp:Literal>			
		</form>		
		<%=script %>
	</body>
</html>
