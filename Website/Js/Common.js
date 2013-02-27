// Menu2 & Acc Info ----------------------
var currentItem = 0;
var oldClassName = 'Bleft_Sub';
var newClassName = 'Bleft_Sub_2';

function menu_select(order) {
    clearTopMenu();

    $('menuitem' + order).className = newClassName;
    if (currentItem != order) {
        var item = $('menuitem' + currentItem);
        if (item) item.className = oldClassName;
    }
    currentItem = order;
}

function clearTopMenu() {
    if (top.frHeader) {
        top.frHeader.ClearActiveTab();
    }
}

function ClearCurrentLeftMenu() {
    var item = $('menuitem' + currentItem);
    if (item) item.className = oldClassName;
}

//Start - Account Info Blink
var blinkAccInfo;

function doBlinḳAccInfoTab() { }

function stopBlickAccInfoTab() { }
//End - Account Info Blink
function OpenIPInfo(ip) {
    ageWnd.open('../_IPInfo/IpInfo.aspx?ip=' + ip, '', 500, 250, 400, 150);
}

function slide_down() { }

function menu_toggle(menu_id) {
    if ($('menu-' + menu_id).className == 'Bleft_Parent') {
        $('menu-' + menu_id).className = 'Bleft_ParentAc';
        $('div-' + menu_id).style.display = '';
    } else {
        $('menu-' + menu_id).className = 'Bleft_Parent';
        $('div-' + menu_id).style.display = 'none';
    }
}

function OpenPage(_url, width, height) {
    var _w = window.open(_url, 'flashuserguide', 'height=' + height + ',width=' + width + ',menubar=no,status=no,scrollbars=yes,resizable=yes');
    if (typeof window.focus != 'undefined') _w.focus();
    return false;
}
// ----------------------
var checkall = false; // object in memory (to add custom att)
var checkID = false;

function CheckAll() {
    if (!checkID) {
        checkID = document.getElementsByName('chkid');
    }
    var isDisableCS = $("isDisableCS");
    if (!checkall) {
        checkall = $("chkall");
    }

    //checkall.
    checkall.No = checkall.checked ? checkID.length : 0; // Check all or not
    if (!checkID) return;
    for (var i = 0; i < checkID.length; i++) {
        checkID[i].checked = checkall.checked;
    }
}

function chkChecked(t) {
    if (!checkID) {
        checkID = document.getElementsByName('chkid');
    }

    if (!checkall) {
        checkall = $('chkall');
    }

    if (!checkall.No) {
        checkall.No = 0;
    }

    checkall.No += (t.checked ? 1 : -1);
    checkall.checked = (checkall.No == checkID.length ? true : false);
}

function RedirectHRFICS(username, custid, isFI, isCS, isHR, isBG, isLiveCS, roleid) {
    if (isFI == 0 || isCS == 0 || isHR == 0 || isBG == 0) {
        url = "../MemberInfo/RedirectPT.aspx?username= " + username + "&custid=" + custid + "&isfi=" + isFI + "&iscs=" + isCS + "&ishr=" + isHR + "&isbg=" + isBG + "&islivecs=" + isLiveCS + "&isedit=false" + "&insert=true";
    }

    ReloadParentPage(url, 5000);
}

function Redir_GetData(url, param) {
    Ajax.Request(url, {
        method: 'get',
        parameters: param,
        onComplete: View_Data
    });
}

function View_Data(data) { }

//Cancelled Bets report
function GenDataCancelBet(custid, roleid) {
    var param = '?custid=' + custid + '&roleid=' + roleid
    Ajax.Request("../Handlers/GetBetlist_CancelBet.ashx", {
        asynchronous: true,
        method: 'get',
        parameters: param,
        onComplete: function (data) {
            $('spData').innerHTML = data.responseText;
            if ($('tbl2')) $('tbl2').style.display = window.attachEvent ? 'block' : 'table';
            var loading = $("loading");
            if (loading) loading.className = "loading";
        }
    });
}

function selected_Option_master(e) {
    var loading = $("loading");
    if (loading) loading.className = "loading1";
    var param = '?custid=' + e.value + '&roleid=2'
    Ajax.Request("../Handlers/GetOption_CancelBet.ashx", {
        asynchronous: true,
        method: 'get',
        parameters: param,
        onComplete: function (data) {
            $('optmember').className = "hidden";
            $('optagent').innerHTML = data.responseText;
            GenDataCancelBet(e.value, 3);
        }
    });
}

function selected_Option_agent(e) {
    var loading = $("loading");
    if (loading) loading.className = "loading1";
    var param = "";
    if (e.value == "-1") {
        $('optmember').className = "hidden";
        GenDataCancelBet($('selmaster').value, 3);
    } else {
        param = '?custid=' + e.value + '&roleid=1'
        Ajax.Request("../Handlers/GetOption_CancelBet.ashx", {
            asynchronous: true,
            method: 'get',
            parameters: param,
            onComplete: function (data) {
                $('optmember').className = "";
                $('optmember').innerHTML = data.responseText;
                GenDataCancelBet(e.value, 2);
            }
        });
    }
}

function select_Option_member(e) {
    var loading = $("loading");
    if (loading) loading.className = "loading1";
    if (e.value == "-1") {
        GenDataCancelBet($('selagent').value, 2);
    } else {
        GenDataCancelBet(e.value, 1);
    }
}

function ShowBalloonMsg(id, strMsg) {
    var _e1 = $(id);
    _e1.style.postion = "relative";

    var pathTooltip = $('pathTooltip') ? $('pathTooltip').value : '';

    var x = 0;
    var y = 0;
    var obj = _e1;

    if (obj.offsetParent) {
        do {
            x += obj.offsetLeft;
            y += obj.offsetTop;
        }
        while (obj = obj.offsetParent);
    }

    x += _e1.offsetWidth - 130;
    y -= _e1.offsetHeight + 48;
    _e1.ontip = function () {
        Tip('<div class="msgerr">' + strMsg + '</div>', BALLOON, true, ABOVE, true, BALLOONIMGPATH, pathTooltip + "../Js/wz_tooltip/tip_balloon/", WIDTH, -350, FIX, [x, y]);
    }
    _e1.ontip();
    _e1.focus();
    window.scrollBy(0, -60);
    _e1.onblur = UnTip;
}

function UnShowBalloonMsg() {
    UnTip();
}

function ShowBalloonMsg_ForLogin(id, strMsg, txtWidth, dir) {
    var e1 = $(id);
    e1.style.postion = "relative";

    var x = 0;
    var y = 0;
    var obj = e1;
    if (obj.offsetParent) {
        do {
            x += obj.offsetLeft;
            y += obj.offsetTop;
        }
        while (obj = obj.offsetParent);
    }
    x += e1.offsetWidth - txtWidth - 20;
    y -= e1.offsetHeight + 36;
    if (!dir) dir = "";
    else y -= 10;
    e1.ontip = function () {
        Tip('<div class="msgerr">' + strMsg + '</div>', BALLOON, true, ABOVE, true, BALLOONIMGPATH, dir + "Js/wz_tooltip/tip_balloon/", WIDTH, -380, FIX, [x, y]);
    }
    e1.ontip();
    e1.focus();
    window.scrollBy(0, -60);
    e1.onblur = UnTip;
}

//this function is used display error message that according sporttype is contained in span
var arrPlaceHolder = ["err", "error1", "error2", "error3", "error5", "error8", "error10", "error11", "error201", "error161", "error99", "error151", "error99_M"];
var arrPanel = ["", "panelSoccer", "panelBasket", "panelFootball", "panelTennis", "panelBaseball", "panelGolf", "panelMoto", "panelFinancial", "panelNumberGame", "panelOtherSports", "panelHorseracing", "panelMixSports"];

function ShowError(errMsg, placeHolder) {
    flag = false;
    var isEdit = ($("hidEdit") != null ? true : false);

    for (var i = 0; i < arrPlaceHolder.length; i++) {
        if (placeHolder == arrPlaceHolder[i]) {
            if (i != 0) {
                if (arrPanel[i] != null && arrPanel[i] != "") {
                    if ($(arrPanel[i]) != null) $(arrPanel[i]).style.display = "";
                }
            } else {
                if (null != $(arrPlaceHolder[i])) {
                    alert_error(errMsg, placeHolder);
                    if ($('div' + placeHolder)) $('div' + placeHolder).style.display = "";
                }
                continue;
            }
        } else {
            if ($(arrPlaceHolder[i])) $(arrPlaceHolder[i]).innerHTML = "";
            if ($('div' + arrPlaceHolder[i])) $('div' + arrPlaceHolder[i]).style.display = "none";
        }
    }
    if (isEdit) {
        if ($("btnSubmit")) $("btnSubmit").disabled = false;
    } else {
        if ($("btnAdd")) $("btnAdd").disabled = false;
    }
}

function ClearAllMsg() {
    if ($('diverr')) {
        $('diverr').style.display = 'none';
    }
    for (var i = 0; i < arrPlaceHolder.length; i++) {
        if (null != $(arrPlaceHolder[i])) $(arrPlaceHolder[i]).innerHTML = "";
    }
}

function check(str) {
    numbers = new String("0123456789");
    var i = 0;
    var nCount = 0;
    while (i < str.length) {
        (numbers.indexOf(str.substring(i, i + 1)) >= 0) ? nCount++ : false;
        i++;
    }
    return ((nCount > (str.length - 2)) || nCount < 2) ? false : true;
}

function createCheck(id) {
    t = document.createElement("img");
    t.setAttribute("id", id);
    t.border = 0;
    t.style.marginBottom = '-4px';
    t.src = "../../App_Themes/x5.gif";
    return t;
}

function ResetCheckPass(tdid) {
    var td = $(tdid);
    var img = $("pass_" + tdid);
    td.removeChild(img);
}

function checkpass(obj, tdid) {
    if (!tdid) tdid = 'td_pass';
    var bol = false;
    var pass = obj.value;
    var t = $("pass_" + tdid);
    var td = $(tdid);
    if (pass.length < 6) {
        if (!t) {
            t = createCheck("pass_" + tdid);
            td.appendChild(t);
        } else {
            t.src = "../../App_Themes/x5.gif";
        }
        if (typeof (lblPasswordRequire) != 'undefined' && typeof (err) != 'undefined') {
            ShowError(lblPasswordRequire, "err");
            $('diverr').style.display = '';
        }
    } else {
        bol = check(pass);
        if (bol) {
            if (!t) {
                td = $(tdid);
                t = createCheck("pass_" + tdid);
                td.appendChild(t);
            }
            t.src = "../../App_Themes/t1.gif";

            if ($('err')) {
                $('err').innerHTML = '';
                $('diverr').style.display = 'none';
            }
        }
    }
    return bol;
}

//check Numeric input field by id , return value
function isNumericSpan(id) {
    if (!$(id)) {
        alert(id + ' does not exists.');
        return;
    }
    return parseFloat(ReplaceAll($(id).innerHTML, ',', ''));
}

function CheckTime() {
    var strConfirm = true;
    if (typeof (lblConfirm) != 'undefined') {
        strConfirm = confirm(lblConfirm);
    }
    if (strConfirm) {
        var chkCount = 0;
        var isSelect = false;
        if ($("rdWeekly").checked) {
            var elements = $("alldays").getElementsByTagName("input");
            for (var i = 1; i < elements.length; i++) {
                if (elements[i].checked) {
                    isSelect = true;
                    chkCount++;
                }
            }
            if (!isSelect) {
                ShowBalloonMsg("rdWeekly", lblChooseOneDay);
                window.scroll(0, 0);
                parent.scroll(0, 0);
                return false;
            }
        }
        return true;
    }
    return false;
}

function ChooseDate() {
    var chkCount = 0;
    if ($("rdWeekly").checked) {
        var elements = $("alldays").getElementsByTagName("input");
        for (var i = 1; i < elements.length; i++) {
            if (elements[i].checked) {
                isSelect = true;
                chkCount++;
                if (chkCount == 7) {
                    ShowError(lblChooseDaily, "err");
                    $('diverr').style.display = '';
                    window.scroll(0, 0);
                    parent.scroll(0, 0);
                    return false;
                }
            }
        }
    }
    return true;
}

function togglePTmenu(id) {
    $(id).className = $(id).className == 'md2' ? 'md2_on' : 'md2';
}

function UpdateErr(strErr) {
    ShowError(strErr, 'err');
    HideLoadingDiv();
    enabledBtn(true);
    window.scroll(0, 0);
}

function enabledBtn(enable) {
    if ($('tblBtn')) $('tblBtn').onmouseover = enable ? 'this.className=\'mainBtnHover\'' : '';
    if ($('tblBtn')) $('tblBtn').onmouseout = enable ? 'this.className=\'BtnHover\'' : '';
    if ($('tblBtn')) $('tblBtn').className = enable ? 'mainBtn' : 'mainBtnDisabled';
}

function boldTable(isBold) {
    var bold = isBold ? '1' : '0';
    if (isBold) {
        addClassName('tbl-rpt', 'boldrow');
    } else {
        removeClassName('tbl-rpt', 'boldrow');
    }
    setCookie('rpt_bold', bold, 30);
}

function ViewHistory(RoleID) {
    var custid = $('hdcus').value;
    var fdate = '',
        tdate = '';
    var month = ($('month')) ? $('month').value : '';
    var IsHistoryReport = $('IsHistoryReport').value;
    IsHistoryReport = (IsHistoryReport == '1') ? '0' : '1';

    if ($('fdate')) {
        fdate = ($('fdate').value.split('/').length > 1) ? $('fdate').value.split('/')[1] : $('fdate').value;
        tdate = ($('tdate').value.split('/').length > 1) ? $('tdate').value.split('/')[1] : $('tdate').value;
    }

    var url = '';
    var current_url = window.location.href;

    if (current_url.indexOf('Detail') > 0) {
        if (current_url.indexOf('DetailSuper') > 0) url = 'WinlossDetailSuper.aspx';
        else if (current_url.indexOf('DetailMaster') > 0) url = 'WinlossDetailMaster.aspx';
        else if (current_url.indexOf('DetailAgent') > 0) url = 'WinlossDetailAgent.aspx';
    } else {
        switch (RoleID) {
            case 4:
                url = 'WinlossSuper.aspx';
                break;
            case 3:
                url = 'WinlossMaster.aspx';
                break;
            case 2:
                url = 'WinlossAgent.aspx';
                break;
        }
    }

    url += '?ddlMonth=' + month;
    url += '&fdate=' + fdate;
    url += '&tdate=' + tdate;

    url += '&IsHistoryReport=' + IsHistoryReport;
    url += '&IsSwitch=1';

    if (IsNumeric(custid)) url += '&custid=' + custid;

    window.location.href = url;
}

function ChangeMonth(month) {
    var param = '?month=' + month;
    param += '&fdate=' + $('fdate').value;
    param += '&tdate=' + $('tdate').value;
    Ajax.Request("../Handlers/GetData.ashx?mode=2", {
        asynchronous: true,
        method: 'get',
        parameters: param,
        onComplete: function (data) {
            var str = data.responseText.split('|');
            var fdate = str[0];
            var tdate = str[1];

            $('spfdate').innerHTML = fdate;
            $('sptdate').innerHTML = tdate;
        }
    });
}

function HideWLData() {
    $('divContent').style.display = 'none';
}

//do not allow enter text on numberic field
function AllowNumber(e) {
    return CheckNumber(e, BtnOnClick);
}

function DisableHR(idchkHR, ischkHRDownline) {
    var idUplines = idchkHR.replace('chk', 'hd');
    var idDownlines = ischkHRDownline.replace('chk', 'hd');
    var idUplinesOld = idchkHR.replace('chk', 'hd') + '_Old';
    var idDownlinesOld = ischkHRDownline.replace('chk', 'hd') + '_Old';

    if ($(idUplinesOld).value == '0' && $(idDownlinesOld).value == '0') {
        if ($(ischkHRDownline)) $(ischkHRDownline).checked = $(idchkHR).checked;
    } else if ($(idUplinesOld).value == '1' && $(idDownlinesOld).value == '1') {
        if ($(ischkHRDownline)) {
            $(ischkHRDownline).disabled = !$(idchkHR).checked;
            if ($(idchkHR).checked == false) $(ischkHRDownline).checked = false;
        }
    } else if ($(idUplinesOld).value == '0' && $(idDownlinesOld).value == '1') {
        if ($(ischkHRDownline)) $(ischkHRDownline).disabled = true;
    } else {
        if ($(ischkHRDownline)) $(ischkHRDownline).checked = $(idchkHR).checked;
    }
    $(idUplines).value = $(idchkHR).checked ? '0' : '1';
    if ($(ischkHRDownline)) {
        $(idDownlines).value = $(ischkHRDownline).checked ? '0' : '1';
    }
}

function DisableFI(idchkFI, ischkFIDownline) {
    if ($('hdDisabledFI_Old').value == '0' && $('hdDisabledFIDownline_Old').value == '0') {
        if ($(ischkFIDownline)) $(ischkFIDownline).checked = $(idchkFI).checked;
    } else if ($('hdDisabledFI_Old').value == '1' && $('hdDisabledFIDownline_Old').value == '1') {
        if ($(ischkFIDownline)) {
            $(ischkFIDownline).disabled = !$(idchkFI).checked;
            if ($(idchkFI).checked == false) $(ischkFIDownline).checked = false;
        }
    } else if ($('hdDisabledFI_Old').value == '0' && $('hdDisabledFIDownline_Old').value == '1') {
        if ($(ischkFIDownline)) $(ischkFIDownline).disabled = true;
    } else {
        if ($(ischkFIDownline)) $(ischkFIDownline).checked = $(idchkFI).checked;
    }

    $('hdDisabledFI').value = $(idchkFI).checked ? '0' : '1';
    if ($(ischkFIDownline)) {
        $('hdDisabledFIDownline').value = $(ischkFIDownline).checked ? '0' : '1';
    }
}

function showEnrichMsg(msg, type) {
    if (typeof type == 'undefined') type = 0;
    $('diverr').style.display = window.attachEvent ? 'block' : 'table';
    $('err').innerHTML = msg;
    $('err').className = (type == 1) ? "EnrichScc" : "Enrich";
    var cname = (type == 1) ? "succ" : "emsg";
    $('1_1').className = cname + "_1_1";
    $('1_2').className = cname + "_1_2";
    $('1_3').className = cname + "_1_3";
    $('2_1').className = cname + "_2_1";
    $('2_2').className = cname + "_2_2";
    $('3_1').className = cname + "_3_1";
    $('3_2').className = cname + "_3_2";
    $('3_3').className = cname + "_3_3";
    HideLoadingDiv();
}
//nqminh - 16Feb09 - show error/info message from error template App_Templates/ErrorMsg.html
//msg: message to display
//type = 0 : error message
//     = 1 : info message
function showErrMsg(msg, type) {
    if (typeof type == 'undefined') type = 0;
    var objMsgDiv = $('diverrmsg');
    var objMsgText = $('spmsgerr');

    if (objMsgDiv == null) { //is Enrich Msg
        objMsgDiv = $('diverr');
        objMsgText = $('err');
        objMsgText.className = 'msgerr';
    }
    objMsgDiv.style.display = window.attachEvent ? 'block' : 'table';
    objMsgText.innerHTML = msg;
    objMsgText.className = (type == 1) ? 'msgscc' : 'msgerr';
    var cname = (type == 1) ? "succ" : "emsg";
    $('1_1').className = cname + "_1_1";
    $('1_2').className = cname + "_1_2";
    $('1_3').className = cname + "_1_3";
    $('2_1').className = cname + "_2_1";
    $('2_2').className = cname + "_2_2";
    $('3_1').className = cname + "_3_1";
    $('3_2').className = cname + "_3_2";
    $('3_3').className = cname + "_3_3";
    HideLoadingDiv();
}

//not being used now
function showFlashAccInfo(isAgentSite, isDisableUsrInfo, userID, subID) {
    if (isAgentSite == 'True' && isDisableUsrInfo == 'False') {
        var cookieAccInfoName = 'ibc_cooki_showaccinfo' + userID + subID;
        var hasCookie = readCookie(cookieAccInfoName);
        if (!hasCookie || hasCookie == 0) {
            setTimeout("showDivPopup()", 1500);
        }
    }
}

function showIntroLetter(canDisplay, divContentId) {
    if (canDisplay == 'True') {
        if (!hasCookieName(divContentId)) {
            setTimeout("showDivPopup('" + divContentId + "')", 1500);
            return 1;
        }
    }
    return 0;
}

function showDivPopup(popupName) {
    if (top.main) {
        if (top.main.location.href.toLowerCase().indexOf('info/creditbalance.aspx') >= 0) {
            if (typeof top.main.alertDivPopup == 'function') {
                if (!top.main.$('accinfomas') || top.main.$('accinfomas').style.display == 'none') top.main.alertDivPopup(popupName);
            }
        }
    }
}

function alertDivPopup(popupName) {
    if ($(popupName)) {
        if ($('container') == null || $('container').style.visibility == 'hidden') { //if have no MinPT popup!
            $(popupName).style.display = '';
        }
    }
}

function getUserID() {
    if (top.frHeader && top.frHeader.loaded) {
        return top.frHeader.$('userid').value;
    }
    return 0;
}

function getSubAccID() {
    if (top.frHeader && top.frHeader.loaded) {
        return top.frHeader.$('subaccid').value;
    }
    return 0;
}

function getRoleID() {
    if (top.frHeader && top.frHeader.loaded) {
        return top.frHeader.$('roleid').value;
    }
    return 0;
}

function hasCookieName(flashName, userID, subID) {
    if (typeof userID == 'undefined') {
        userID = getUserID();
    }
    if (typeof subID == 'undefined') subID = getSubAccID();

    var cookieAccInfoName = 'ibc_cooki_' + flashName + userID + '' + subID;
    var hasCookie = readCookie(cookieAccInfoName);
    return (hasCookie && hasCookie != 0);
}

function setFlashCookie(flashName, isShow, userId, subId) {
    var cookieAccInfoName = 'ibc_cooki_' + flashName + userId + '' + subId;
    createCookie(cookieAccInfoName, isShow ? 1 : 0, 17);
}

//add popup ads to main window
//Note:
//1. You must fill data for popup in Header.aspx.cs first!
//2. In this function, parameter 'divContentId' must be the FLASH NAME (without .swf extension)
//3. If you want to customize css of flash, pls refer to: Common.css -> divPopupAd class
function addDivPopup(divContentId) {
    if (!hasCookieName(divContentId)) {
        //check other popup existed?
        if (top.main.location.href.toLowerCase().indexOf('info/creditbalance.aspx') >= 0) {
            if (top.main.$('divPopupLetter')) top.main.$('divPopupLetter').style.display = 'none';
        }
        //check this popup existed?
        if (!top.main || !top.main.loaded) return;
        var divPopupExist = top.main.$(divContentId);
        if (!top.frHeader || !top.frHeader.loaded) return;
        var hdnShowFlashAccInfo = top.frHeader.$("showFlashAccInfo");
        if (!divPopupExist && hdnShowFlashAccInfo.value == "1") {
            hdnShowFlashAccInfo.value = "0";
            var divPopup = top.main.document.createElement("div");
            divPopup.id = divContentId;
            divPopup.className = 'divPopupAd';
            var divTemplate = top.frHeader.$(divContentId);
            divPopup.style.width = divTemplate.style.width;
            divPopup.style.height = divTemplate.style.height;
            divPopup.innerHTML = divTemplate.innerHTML;
            top.main.document.body.appendChild(divPopup);
        } else { }
    }
}

function showFlashAccInfoMaster() {
    var roleId = getRoleID();
    if (roleId == 4) {
        addDivPopup('accinfosup');
    }
}

function doViewBetList(matchId, bettype, type) {
    var url = '../BetList/BetList.aspx?matchid=' + matchId + '&bettype=' + bettype + '&type=' + type;
    window.location.href = url;
}

function viewMixParlayResult(refno, winlostdate) {
    var width = 750;
    var height = 250;
    var params = '?refnum=' + refno + '&wldate=' + winlostdate;
    if (ageWnd.loaded) {
        ageWnd.close();
    }
    ageWnd.open('../Popup/ViewMixParlayResult.aspx' + params, '', 50, 100, width, height);
}

function viewFinanceResult(transid) {
    var width = 330;
    var height = 110;
    var params = '?transid=' + transid;
    ageWnd.open('../Popup/ViewFinanceResult.aspx' + params, '', 300, 100, width, height);
}

function EditMember_Single(cid, user, agentid, Customer) {
    var URL = "../_MemberInfo/PositionTaking/Agent/EditMember.aspx?"; //V1
    if (Customer == 'Super') URL = "../UserSuper/EditMember.aspx?";
    if (Customer == 'Master') URL = "../UserMaster/EditMember.aspx?";

    URL += "m=0";
    URL += "&custid=" + cid;
    URL += "&USER=" + user;
    URL += '&agentid=' + agentid;
    var popH = 950,
        popW = 1024;

    if ($("arrayCustID")) {
        $("arrayCustID").value = "";
        $("arrayUserName").value = "";
    }

    //url, title, left, top, width, height
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

function EditCS(cid) {
    var URL = "../MemberInfo/PositionTakingCS.aspx?custid=" + cid + "&isupdate=false";

    var popH = 600,
        popW = 980;
    //url, title, left, top, width, height
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

function EditBG(cid) {
    var URL = "../MemberInfo/PositionTakingBingo.aspx?custid=" + cid + "&isupdate=false";

    var popH = 600,
        popW = 980;
    //url, title, left, top, width, height
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

// Super/Master/Agent edit downline
//declare array that contains id of
var arr1Super1 = ["listS1OUD", "listS11stHdpD", "listS11stOUD", "listS1OE"];
var arr2Super1 = ["listS1CS", "listS1TG", "listS1MP", "listS1OR"];
var arr3Super1 = ["listS1OUL", "listS11stHdpL", "listS11stOUL"];

var arr1Master1 = ["listM1OUD", "listM11stHdpD", "listM11stOUD", "listM1OE"];
var arr2Master1 = ["listM1CS", "listM1TG", "listM1MP", "listM1OR"];
var arr3Master1 = ["listM1OUL", "listM11stHdpL", "listM11stOUL"];

var arrSuper2 = ["listS2OU", "listS2OE", "listS2MP", "listS2ML"]; // basketball
var arrSuper3 = ["listS3OU", "listS3OE", "listS3ML"]; // football
var arrSuper5 = ["listS5OU", "listS5OE", "listS5ML"]; // tennis
var arrSuper8 = ["listS8OU", "listS8ML"]; // baseball
var arrSuper10 = ["listS10OU", "listS10OE", "listS10ML"]; // golf
var arrSuper11 = ["listS11ML"]; // motosports
var arr161Super81 = ["listS1611stOED", "listS161OED", "listS161WRD"];
var arr161Super85 = ["listS161OEL", "listS161HLL", "listS161NCL", "listS161NGL"];

var arrSuper99 = ["listS99OU", "listS99OE", "listS99ML"]; // othersports
var arrSuper201 = ["listS201OE"]; // financial
var arrSuper151TO = ["listSTOPlace"]; // Tote
var arrSuper151FO = ["listSFOPlace"]; // Fixed Odds
var arrSuper99_M = ["listS99_M"];

var arrMaster2 = ["listM2OU", "listM2OE", "listM2MP", "listM2ML"]; // basketball
var arrMaster3 = ["listM3OU", "listM3OE", "listM3ML"]; // football
var arrMaster5 = ["listM5OU", "listM5OE", "listM5ML"]; // tennis
var arrMaster8 = ["listM8OU", "listM8ML"]; // baseball
var arrMaster10 = ["listM10OU", "listM10OE", "listM10ML"]; // golf
var arrMaster11 = ["listM11ML"]; // motosports
var arr161Master81 = ["listM1611stOED", "listM161OED", "listM161WRD"];
var arr161Master85 = ["listM161OEL", "listM161HLL", "listM161NCL", "listM161NGL"];

var arrMaster99 = ["listM99OU", "listM99OE", "listM99ML"]; // othersports
var arrMaster201 = ["listM201OE"]; // financial
var arrMaster151TO = ["listMTOPlace"]; //
var arrMaster151FO = ["listMFOPlace"]; //
var arrMaster99_M = ["listM99_M"];
//declare array that contain id of Master Preset Position dropdown to use for CheckAutoPreset function
var arrDdl1 = ["listS1HdpD", "listS1OUD", "listS11stHdpD", "listS11stOUD", "listS1OE", "listS11x2", "listS1CS", "listS1TG", "listS1MP", "listS1OR", "listS1HdpL", "listS1OUL", "listS11stHdpL", "listS11stOUL"];
var arrDdl2 = ["listS2Hdp", "listS2OU", "listS2OE", "listS2MP", "listS2ML"]; // basketball
var arrDdl3 = ["listS3Hdp", "listS3OU", "listS3OE", "listS3ML"]; // football
var arrDdl5 = ["listS5Hdp", "listS5OU", "listS5OE", "listS5ML"]; // tennis
var arrDdl8 = ["listS8Hdp", "listS8OU", "listS8ML"]; // baseball
var arrDdl10 = ["listS10Hdp", "listS10OU", "listS10OE", "listS10ML"]; // golf
var arrDdl11 = ["listS11Hdp", "listS11ML"]; // motosports
var arrDdl201 = ["listS201OU", "listS201OE"]; // financial
var arrDdl161 = ["listS1611stOUD", "listS1611stOED", "listS161OUD", "listS161OED", "listS161OUL", "listS161OEL", "listS161HLL"];
var arrDdl99 = ["listS99Hdp", "listS99OU", "listS99OE", "listS99ML"]; // othersports
var arrDdl99_M = ["listS99_M"];