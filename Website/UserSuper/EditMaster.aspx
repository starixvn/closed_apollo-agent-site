<%@ Page Language="VB" AutoEventWireup="TRUE" CodeFile="EditMaster.aspx.vb" Inherits="EditMaster" %>
<%@ OutputCache Location="None" %>
<link href="../../App_Themes/AgencyV2/newagent.css?<%= AppSettings.JsVersion %>" type="text/css" rel="Stylesheet" />  
<style type="text/css">		    
	.right { TEXT-ALIGN: right }
	SPAN.mp { FONT-WEIGHT: bold; FONT-SIZE: 8pt; COLOR: #ffffff }
</style>
<%=VariableOfScript()%>
<script src="../../Js/Customer.js?<%=AppSettings.JsVersion %>" type="text/javascript"></script>
<script src="../../Js/EditMaster.js?<%=AppSettings.JsVersion %>" type="text/javascript"></script>
<script src="../../_Components/AGEWnd/AGEWnd.js?<%=AppSettings.JsVersion %>" type="text/javascript"></script>