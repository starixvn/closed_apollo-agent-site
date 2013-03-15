function getQuerystring(key, default_) {
    if (default_ == null) default_ = "";
    key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
    var qs = regex.exec(window.location.href);
    if (qs == null) return default_;
    else return qs[1];
}

function slideToggle(id) {
    var obj = null;
    if(typeof(id) == 'object')
    {
        var rowIndex = id.rowIndex;
        var p = id.parentNode ? id.parentNode : id.parentElement;
        var table = p.parentNode ? p.parentNode : p.parentElement;
        var cell = table.rows[rowIndex + 1].cells[0];
        obj = cell.firstElementChild ? cell.firstElementChild : cell.firstChild ;
    }
    else
    {
        obj = $(id);
    }
    
    if (obj != null) {
        if (obj.style.display == 'none') obj.style.display = 'block';
        else obj.style.display = 'none';
    }
}

function setShowOldValueCookie(obj)
{
    setCookie('showOldValue', obj.checked);
    DoSearch(true);
}

function Initialize() {
    // set default datetime
    var issubacc = 0;
    var ismember = 0;
    var fromDate = getQuerystring('fromdate');

    if (getQuerystring('custname') == null || getQuerystring('custname') == '') {
        $('txtUserName').value = enterusername;
        $('txtUserName').className = 'text_f1';
    } else {
        $('txtUserName').value = getQuerystring('custname');
        $('txtUserName').className = 'text_f';
    }
    $('RequiredUserName').style.display = 'none';

    var tabId = getQuerystring('tabid');
    if (tabId.length == 0) tabId = 'CustomerSetting';
    AddClassName(tabId, 'current');

    if (tabId.toLowerCase() == 'customersetting') {
        $('ddlLogActionType').style.display = "inline";
        $('divShowOldValue').style.display = "block";
        $('chk_showOldValue').checked = (getCookie('showOldValue') != 'false');
        $(ddlActionType).value = GetActionType();
    } else if(tabId.toLowerCase() == 'status'){
        $('divShowOldValue').style.display = "block";
        $('chk_showOldValue').checked = getCookie('showOldValue') == 'true';
    } else if (tabId.toLowerCase() == 'login') {
        if (role != '2') {
            $('divMessageWarning').style.display = "block";
        }
        issubacc = 1;
        ismember = 1;
    } else {
        $('ddlLogActionType').style.display = "none";
        $('divShowOldValue').style.display = "none";
    }
    
    $('dSubmit').disabled = false;
    
    SetActiveTab();

    var url = '../_GlobalResources/Handlers/QueryUserName.ashx';
    url = SetParameterValue("issub", issubacc, url);
    url = SetParameterValue("ismember", ismember, url);

    // auto complete
    initAutoComplete('txtUserName', url);
}

function SearchByDate() {
    DoSearch();
}

function SearchOneDay(value) {
    DoSearch();
}

function DoSearch(keepPageIndex) {

    $('txtUserName').focus();

    var custName = $('txtUserName').value.trim();
    // Allow default username for log login at agent
    if (role == '2' && getQuerystring('tabid').toLowerCase() == 'login' && custName.toLowerCase() == enterusername.toLowerCase()) {
        custName = '';
    }
    if ((custName.length > 0 && custName.toLowerCase() != enterusername.toLowerCase()) || role == 2) {
        var ArrDate = $('lblSelectedMonth').title.split('/');
        var fromDate = ArrDate[0] + '/' + $('ddlDateFrom').value + '/' + ArrDate[2];
        var toDate = ArrDate[0] + '/' + $('ddlDateTo').value + '/' + ArrDate[2];
        
        var url = location.href;
        url = SetParameterValue("custname", custName, url);
        url = SetParameterValue("fromdate", fromDate, url);
        url = SetParameterValue("todate", toDate, url);
        url = SetParameterValue("actiontype", $(ddlActionType).value, url);
        if(!keepPageIndex)
        {
            url = SetParameterValue("pageIndex", 1, url);
        }
        
        location.href = url;
    } else {
        $('RequiredUserName').style.display = 'inline';
        $('txtUserName').focus();
        $('txtUserName').value = '';
        $('txtUserName').className = 'text_f';
        $('dSubmit').disabled = false;
        $('loading').style.display = 'none';
        return false;
    }
}

function GetActionType() {
    if (getQuerystring('actiontype') == '') {
        return $(ddlActionType).value;
    }
    return getQuerystring('actiontype');
}

function GetPageByTabId(tabId) {
    var url = "LogCustomerSetting.aspx";
    tabId = tabId.toLocaleLowerCase();
    switch (tabId) {
    case "status":
        url = "LogStatus.aspx";
        break;
    case "credit":
        url = "LogCredit.aspx";
        break;
    case "login":
        url = "LogLogin.aspx";
        break;
    }
    return url;
}

function SetPageUrlValue(newPage, url)
{
    if (!url.contains("?")) {
        return newPage;
    }
    // Already contain ?
    var separator = "?";

    var i1 = url.indexOf(separator);
    var tmp = url.substr(i1);
    url = newPage + tmp;
    return url;
}

function MenuGoTo(tabId) {
    var url = location.href;
    url = SetPageUrlValue(GetPageByTabId(tabId), url);
    url = SetParameterValue("tabid", tabId, url);
    url = SetParameterValue("pageIndex", 1, url);
    location.href = url;
}

function ResetDate(lowerYear, resetDate, objInput) {
    var selectedDate = new Date(objInput.value);
    if (selectedDate.getFullYear() < lowerYear) {
        objInput.value = resetDate;
    }
}

function SetActiveTab() {
    if (top.frHeader && top.frHeader.SelectTopMenu) {
        top.frHeader.SelectTopMenu('LogCustomerSetting.aspx');
    }
}

function KeyPress(e) {
    if (!e) e = window.event;
    var key = (e.keyCode) ? e.keyCode : e.which;
    if (key == 13) {
        DoSearch();
        return age.stopEvent(e);
    }

}

function onclickUser(username) {
    if ($('txtUserName').value == username) {
        $('txtUserName').className = 'text_f';
        $('txtUserName').value = '';
    }
}

function onblurUser(username) {
    if ($('txtUserName').value == '') {
        $('txtUserName').className = 'text_f1';
        $('txtUserName').value = username;
    }
}

function setupCalendar(triggerId, textBoxId, minDate, maxDate) {

    minDate = parseInt(minDate);
    maxDate = parseInt(maxDate);
    Calendar.setup({
        animation: false,
        trigger: triggerId,
        inputField: textBoxId,
        min: minDate,
        max: maxDate,
        dateFormat: "%m/%d/%Y",
        weekNumbers: true,
        onSelect: function () {
            this.hide();
        }
    });
}