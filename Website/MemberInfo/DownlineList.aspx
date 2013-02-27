<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DownlineList.aspx.cs" Inherits="MemberInfo_downlineList" %>
<%@ OutputCache Location="None"%>
<script type="text/javascript" src="../../_Components/AGEWnd/AGEWnd.js?<%= AppSettings.JsVersion%>"></script>
<script type="text/javascript" src="../_GlobalResources/Js/jx.js?<%= AppSettings.JsVersion %>"></script>
<script type="text/javascript" src="../../Js/MemberInfo/MasterAgentList.js?<%= AppSettings.JsVersion %>"></script>
<script type="text/javascript" src="../../Js/Paging.js?<%= AppSettings.JsVersion %>"></script>
<script type="text/javascript" src="../_Components/AutoComplete/autocomplete.js?<%= AppSettings.JsVersion %>"></script>
<script type="text/javascript">
    //paging
    function NavigatePage(page, url, site) {
        NavigatePage2(page, url, "act", site);
    }
    
    function GotoPageNext(strNavigatePage, site) {
        GotoPageNext2(strNavigatePage, "act", site);
    }
    
    function searchByUsername(strNavigatePage, site) {
        var ctrlCondition = $('txtUserName');
        var conditionValue = "";
        if (ctrlCondition != null) {
            $('hdtotalpages').value = 0;
            conditionValue = ctrlCondition.value;
            $('hdsearch').value = (conditionValue.length > 0 ? 'yes' : '');
            var url = strNavigatePage + '?' + 'custid=' + $('hdcus').value;

            url = url + '&masterid=' + masterid;

            url = url + '&search=' + $('hdsearch').value;
            url = url + '&username=' + ($('hdsearch').value == 'yes' ? conditionValue : '');
            url = url + '&status=' + $('statusFilter').value;
            url = url + '&total=' + $('hdtotalpages').value;
            NavigatePage(1, url, site);
        }
        var dSubmit = $('dSubmit');
        dSubmit.disabled = true;
        dSubmit.className = 'btn2';
    }

    function InitTagSuggestion() {
        initAutoComplete('txtUserName', '../_GlobalResources/Handlers/QueryUserName.ashx?custid='+ $('hdcus').value + '&isdir=1');       
    }
    
</script>