<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConfirmChangeAutoPT.aspx.cs" Inherits="Super_ConfirmChangeAutoPT" %>
<%@ OutputCache Location="None"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title></title>
    <style type="text/css">
        input{width:40px;}
    </style>
    <link href="../../App_Themes/AgencyV2/main.css" type="text/css" rel="Stylesheet" />    
    <link href="../../App_Themes/AgencyV2/newagent.css" type="text/css" rel="Stylesheet" />
    <style type="text/css">
        .Hide{visibility:hidden;}
    </style>
    
    <script type="text/javascript" src="../../Js/opt/Core.js?<%= AppSettings.JsVersion %>"></script>
    <script type="text/javascript" src="../../Js/Common.js?<%= AppSettings.JsVersion %>"></script>    
    
    <script type="text/javascript">
        function GetMemberPT()
        {        
            var maxMember=Math.round(parseFloat(parent.$("maxMemberPT").value)*10);                      
            $("ddlMemberPT").options.length=0;
            while(maxMember>=0)
            {
                var val=round(maxMember/10);              
                $("ddlMemberPT").options.add(new Option(val,val));
                maxMember=maxMember-1;               
            }
        }
        function ButtonClick(str)
        {			
            parent.$("memberPT").value=$("ddlMemberPT").value;
            parent.$("isUpdateMember").value=str;  
            parent.flag=false;          
            parent.ageWnd.close();
            DisableControls();      
            parent.DelayReloadPage(null, 300000);   
            parent.document.form.submit();            
        }     
        function DisableControls()
        {   		
            var disableC=parent.$("saveControlDisabled").value.split(",");                      
            parent.flag=false;                    
            for(var i=0;i<parent.document.form.elements.length;i++)
            {
                if(disableC[i]=="1")
                {
                    parent.document.form.elements[i].disabled=false;
                }
            }
            if (typeof parent.showLoading == 'function') {
                parent.showLoading(false);
            }
        }
    </script>  
  </head>
  <body onload="GetMemberPT();" onunload="DisableControls();">
	<form id="fConfirmEditMaster">
	<input type="text" style="width:0px; height:0px; border: 0px;" />
    <table border="0" width="100%">
        <tr>
            <td class="c">
                <%=Resources.Lang.lblConfirmEditMaster %>
            </td>
        </tr>
        <tr class="c">
            <td>
                <select id="ddlMemberPT" name="ddlMemberPT">
                </select>
            </td>
        </tr>
        <tr>
            <td class="c" style="padding: 3px 0px 3px 0px">
                <input type="button" id="btnYes" onclick="ButtonClick(1);" name="btnYes" value="Yes"/>
                &nbsp;
                <input type="button" id="btnNo" onclick="ButtonClick(0);" name="btnNo" value="No"/>
            </td>
        </tr>
    </table>
    </form>
  </body>