<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddNewAgent.aspx.vb" Inherits="AddNewAgent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href='../../App_Themes/AgencyV2/main.css' type='text/css' rel='stylesheet' />
    <link href='../../App_Themes/AgencyV2/newagent.css' type='text/css' rel='stylesheet' />
    <style type="text/css">
        SPAN.mp { FONT-WEIGHT: bold; FONT-SIZE: 8pt; COLOR: #ffffff }
        body
		{
			margin : 0px 0px 0px 0px;
            padding: 0px 0px 0px 0px;
		}
	</style>
	
    <script type="text/javascript" src="../../Js/opt/Core.js?<%= AppSettings.JsVersion %>"></script>
    <script type="text/javascript" src="../../Js/Common.js?<%= AppSettings.JsVersion %>"></script>
    <script type="text/javascript" src="../../Js/Customer.js?<%= AppSettings.JsVersion %>"></script>
    <script type="text/javascript" src="../../Js/AddNewAgent.js?<%= AppSettings.JsVersion %>"></script>

    <script type="text/javascript">
		
		function BtnAddCSClick()
		{
		    $('hidCS').value = 'true';
		    var isOK = BtnOnClick();

		    if(isOK)
		    {
		        var btnAddCS = $("btnAddCS");
		        btnAddCS.style.color="#cccccc";					
		        btnAddCS.disabled=true;	
		        btnAddCS.onclick = ";";
		    }
		}

		_FocusEl = 'number1';
    </script>
    
    <%VariableOfScript()%>
</head>
<body onload="<%=strDirect%>">    
    <iframe id="frInsert" name="frInsert" style="display: none"></iframe>
    <form id="form" name="form" method="post" action="UpdateNewAgent.aspx" target="frInsert">
        <input type="hidden" name="roleId" id="roleId" value="3" />
        <input type="hidden" id="isAllowUpd" name="isAllowUpd" value="1" runat="server" />
        <input type="hidden" id="dirMaster" name="dirMaster" runat="server" />
        <asp:Literal ID="htmlAddAgentContent" runat="server"></asp:Literal>
    </form>
    <%=script %>
</body>
</html>
