<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreditBalance.aspx.cs" Inherits="MemberInfo_creditbalance" %>
<%@ OutputCache Location="None"%>

<script type="text/javascript" src="../../_Components/AGEWnd/AGEWnd.js?<%= AppSettings.JsVersion %>"></script>
<script type="text/javascript" src="../../Js/MemberInfo/CreditBalance.js?<%= AppSettings.JsVersion %>"></script>
<script type="text/javascript" src="../../Js/Paging.js?<%= AppSettings.JsVersion %>"></script>
<script type="text/javascript" src="../_Components/AutoComplete/autocomplete.js?<%= AppSettings.JsVersion %>"></script>
<script type="text/javascript">

    function viewOutSt(custId, userName)
    {
        window.location = '../_Reports/BetList/OutstandingDetail.aspx?custid=' + custId + '&username=' + userName + '&type=RunByDate_Mem' + '&chkgetall=on'+ '&chk_all=on' +'&chk_showCasino=on' + '&chk_showSB=on' + '&chk_showRB=on';
    }

    function viewBal(act, custId) {
        window.location = "CreditBalance.aspx?act=" + act + "&custid=" + custId;
    }

    //paging
    function NavigatePage(mypage, strNavigatePage, site) {
        NavigatePage2(mypage, strNavigatePage, "act", site);
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
            url = url + '&search=' + $('hdsearch').value;
            url = url + '&username=' + ($('hdsearch').value == 'yes' ? conditionValue : '');
            url = url + '&status=' + $('statusFilter').value;
            url = url + '&total=' + $('hdtotalpages').value;
            NavigatePage(1, url, site);
        }
        $('dSubmit').disabled = true;
        $('dSubmit').className = 'btn2';
    }

    AGE.addEvent(window, "load", InitTagSuggestion);

    function InitTagSuggestion() {
        initAutoComplete('txtUserName', '../_GlobalResources/Handlers/QueryUserName.ashx?custid=' + $('hdcus').value +'&isdir=1');
    }
</script>