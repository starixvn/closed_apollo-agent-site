<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConfirmEditAgent.aspx.cs" Inherits="Super_ConfirmEditAgent" %>
<%@ OutputCache Location="None"%>

    <style type="text/css">
        input
        {
            width: 40px;
        }
        .Hide
        {
            visibility: hidden;
        }
    </style>

    <link href="../../App_Themes/<%=Sunplus.Agent.Model.Entities.UserSession.Current.SiteName %>/newagent.css" type="text/css" rel="Stylesheet" />
    <link href="../../App_Themes/<%=Sunplus.Agent.Model.Entities.UserSession.Current.SiteName %>/main.css" type="text/css" rel="Stylesheet" />

    <script type="text/javascript" src="../../Js/opt/Core.js?<%= AppSettings.JsVersion %>"></script>
    <script type="text/javascript" src="../../Js/Common.js?<%= AppSettings.JsVersion %>"></script>

    <script type="text/javascript">
        function GetMemberPT()
        {
            var maxMember=Math.round(parseFloat(parent.$("maxMemberPT").value)*100);
            $("ddlMemberPT").options.length=0;
            while(maxMember>=0)
            {
                var val=round(maxMember/100);
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
        }
        window.onload = function() {
            GetMemberPT();
        }
        window.onunload = function() {
            DisableControls();
        }
    </script>