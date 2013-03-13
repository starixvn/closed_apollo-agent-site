function ClearCurrentLeftMenu() { //do nothing here!
}
var oldClassName = 'b_left';
var newClassName = 'b_left_hover';

function checkAndSubmitUI() {
    if ($('txtUsername').value == '') {
        alert(_page.usernamerequired);
        return;
    }
    document.forms.frmAgent.submit();
}

function checkKeyPress(e, textboxCredit) {
    if (checkAllowNumber(e) == false) return false;
    try {
        if (typeof e == 'undefined') e = window.event;
        var key = (typeof e.keyCode == 'undefined') ? e.which : e.keyCode;
        if (key == 27) {
            changeCredit(false, textboxCredit);
            return false;
        }
        if (key == 13) {
            var givencredit = replaceAll($(textboxCredit).value, ",", "");
            if (!isNumeric(givencredit)) {
                alert(_page.invalidvalue);
                return false;
            }
            ajax.PostJSON('Handlers/UpdateGivenCredit.ashx', 'custid=' + _page.UserId + '&custRoleId=' + _page.CustRoleId + '&amount=' + givencredit, onComplete);
        }
    }
    catch (ex) {
        alert(ex.message);
    }

    function onComplete(result) {
        if (result.errCode == 0) {
            $("spantxtGivenCredit").innerHTML = result.amount.toString();
            changeCredit(false, textboxCredit);
        }
        else alert(result.errMsg);
    }
}

function changeCredit(ischanging, textboxCredit) {
    try {
        var spanGC = $('span' + textboxCredit);
        var txtGC = $(textboxCredit);
        if (typeof ischanging != 'undefined' && ischanging) {
            if ($('tdMemMaxCredit')) $('tdMemMaxCredit').innerHTML = _page.lblMemberMax; //for nice GUI
            if ($('tdAgOutstand')) $('tdAgOutstand').innerHTML = _page.lblAgOutst; //for nice GUI
            txtGC.value = replaceAll(spanGC.innerHTML, ",", "");
            spanGC.style.display = 'none';
            txtGC.style.display = ''
            txtGC.focus();
        }
        else {
            if ($('tdMemMaxCredit')) $('tdMemMaxCredit').innerHTML = _page.lblMemberMaxCredit;
            if ($('tdAgOutstand')) $('tdAgOutstand').innerHTML = _page.lblAgentOutstanding;
            spanGC.style.display = '';
            txtGC.style.display = 'none';
        }
    }
    catch (e)
    { }
}

function reloadMenuUsrInfo() {
    var delay2 = 2000;
    setTimeout("window.location.href = window.location.href", delay2);
}

function changeSuspendedStatus(evt, objDiv) {
    if (objDiv.childNodes[1].disabled) {
        return _stopEvent(evt);
    }
    var isSuspended = $('chkSuspend').checked ? false : true;
    $('hdnTypeStatus').value = 'NO_POP'; //type popup: No popup
    if (isSuspended) {
        if (!confirm(_page.confirmSuspendDownline)) {
            return _stopEvent(evt);
        }
    }
    else if (!confirm(_page.confirmSuspendMem)) {
        return _stopEvent(evt);
    }
    ajax.PostJSON('Handlers/UpdateSuspendedStatus.ashx', 'custid=' + _page.UserId + '&issuspended=' + isSuspended + '&issynccs=' + _page.isSyncCasino, onComplete);

    function onComplete(result) {
        if (result.errCode == 0) {
            $('chkSuspend').checked = isSuspended;
        }
        else {
            alert(result.errMsg);
        }
    }
}

function changeClosedStatus(evt, objDiv) {
    if (objDiv.childNodes[1].disabled) return _stopEvent(evt);
    var isClosed = $('chkClosed').checked ? false : true;
    var isClearAllCredit = false;
    if (isClosed) { //do close
        showClosedPopup();
        return _stopEvent(evt);
    }
    else {
        if (!confirm(_page.confirmOpenMem)) {
            return;
        }
        $('hdnTypeStatus').value = 'NO_POP'; //type popup
        updateCloseStatus(isClosed, false);
    }

    function showClosedPopup() {
        $('agtype').innerHTML = _page.closedStatus;
        $('chkEnabled').checked = true;
        $('chkEnabled').disabled = true;
        $('hdnTypeStatus').value = 'Closed'; //type popup
        $('lblEnableStatus').innerHTML = _page.titleclose; //title popup
        $('lbldownline').innerHTML = _page.lblclearallcredit;
        $('chkEnabledDL').disabled = true; // False
        $('chkEnabledDL').checked = false; // True
        $('btnUpdStatus').disabled = false;
        $('btnUpdStatus').onclick = function() {
            var confirmMsg = _page.CustRoleId == 1 ? _page.confirmCloseMem : _page.alertCloseCustDownline;
            if (!confirm(confirmMsg)) {
                closeDivPopup();
                return;
            }
            isClearAllCredit = false;
            updateCloseStatus(isClosed, isClearAllCredit);
        };
        showDivPopup('chkClosed');
    }

    function updateCloseStatus(isClosed, clearAllCredit) {
        isClearAllCredit = clearAllCredit;
        ajax.PostJSON('Handlers/UpdateClosedStatus.ashx', 'custid=' + _page.UserId + '&isclosed=' + isClosed + '&issynccs=' + _page.isSyncCasino + '&clearallcredit=' + clearAllCredit, onComplete);

        function onComplete(result) {
            if (result.errCode == 0) {
                $('chkClosed').checked = isClosed;
                if (isClearAllCredit) resetGivenCredit();
            }
            else alert(result.errMsg);
            closeDivPopup();
        }
    }
}

function changeOutrightStatus(evt, objDiv) {
    if (objDiv.childNodes[1].disabled) return _stopEvent(evt);
    var isDisabled = $('chkOutright').checked;
    $('hdnTypeStatus').value = 'NO_POP'; //type popup: No popup
    var confirmMessage = isDisabled ? _page.confirmDisableOutright_Member : _page.confirmAllowOutright;
    if (!confirm(confirmMessage)) return _stopEvent(evt);
    ajax.PostJSON('Handlers/UpdateOutrightStatus.ashx', 'custid=' + _page.UserId + '&isdisabled=' + isDisabled, onComplete);

    function onComplete(result) {
        if (result.errCode == 0) {
            $('chkOutright').checked = !isDisabled;
        }
        else alert(result.errMsg);
    }
}

function changeAutoMPTStatus(evt, objDiv) {
    if (objDiv.childNodes[1].disabled) return _stopEvent(evt);
    var isDisabled = $('chkMAutoPT').checked;
    $('hdnTypeStatus').value = 'NO_POP'; //No popup
    if (!confirm((isDisabled) ? _page.confirmDisableMAutoPT : _page.confirmEnableMAutoPT)) {
        return _stopEvent(evt);
    }
    ajax.PostJSON('Handlers/UpdateAutoMasterPTStatus.ashx', 'custid=' + _page.UserId + '&isdisabled=' + isDisabled, onComplete);

    function onComplete(result) {
        if (result.errCode == 0) {
            $('chkMAutoPT').checked = !isDisabled;
        }
        else alert(result.errMsg);
    }
}

function allowSubmit(chkDisablePop) {
    //check agent status has changed to enable button submit
    if (!chkDisablePop.checked) {
        $('btnUpdStatus').disabled = true;
        $('btnUpdStatus').className = "btn2";
    }
    else {
        $('btnUpdStatus').disabled = false;
        $('btnUpdStatus').className = "btn";
    }
    //control downline status when change agent status
    if (chkDisablePop.checked == false) { //disable
        $('chkEnabledDL').checked = false;
        $('chkEnabledDL').disabled = true;
        $('spanChkEnabledDL').style.display = 'none';
    }
    else { //enable
        $('chkEnabledDL').checked = false;
        $('chkEnabledDL').disabled = false;
        $('spanChkEnabledDL').style.display = '';
    }
}

function resetGivenCredit() {
    $('spantxtGivenCredit').innerHTML = '0.00';
}

function assignDefaultPopupValue() {
    $('btnUpdStatus').className = "btn2";
    $('agtype').innerHTML = _page.agenttype;
    $('btnUpdStatus').disabled = true;
    $('chkEnabled').checked = false;
    $('chkEnabled').disabled = false;
    $('chkEnabledDL').checked = false;
    $('chkEnabledDL').disabled = true;
    $('spanChkEnabledDL').style.display = 'none';
}

function showDivPopup(idCheckBox) {
    var obj = $(idCheckBox);
    var curtop = 0;
    if (obj.offsetParent) {
        do {
            curtop += obj.offsetTop;
        } while (obj = obj.offsetParent);
    }
    curtop += 16;
    $('disabledStatusPop').style.top = curtop + 'px';
    $('disabledStatusPop').style.display = 'block'; //show popup
    $('overlayBg').style.display = 'block'; //show overlay
}

function closeDivPopup() {
    if ($('disabledStatusPop')) $('disabledStatusPop').style.display = 'none';
    $('overlayBg').style.display = 'none';
}

function toggleCheckBox(chkId) {
    $(chkId).checked = !$(chkId).checked;
}

function loadCustDownlineList(downlineList) {
    $("tabdownline").innerHTML = downlineList;
}

function showEditPage(isShow) {
    $('spPage').style.display = (isShow == 1) ? 'none' : '';
    $('txtPage').style.display = (isShow == 1) ? '' : 'none';
    if (isShow) {
        $('txtPage').value = $('spPage').innerHTML;
        $('txtPage').focus();
    }
}

function checkDownlinePageKeyPress(e, textbox) {
    if (checkAllowNumber(e) == false) return false;
    try {
        if (typeof e == 'undefined') e = window.event;
        var key = (typeof e.keyCode == 'undefined') ? e.which : e.keyCode;
        if (key == 27) {
            showEditPage(0);
            return false;
        }
        if (key == 13) {
            var fr = self.frames['frmDownlineList'];
            if (typeof fr != 'undefined' && fr) {
                var pageIdx = parseInt($('txtPage').value);
                var totalPage = parseInt($('hdnTotalPage').value);
                if (pageIdx < 1 || pageIdx > totalPage) return false;
                showLoadingDownline(1);
                fr.location.href = 'DownlineListInfo.aspx?custid=' + _page.UserId + '&pageidx=' + pageIdx + '&totalpages=' + totalPage;
                return true;
            }
        }
    }
    catch (ex) {
        showLoadingDownline(0);
        alert(ex.message);
    }
}

function showLoadingDownline(isShow) {
    $('ploading').style.display = (isShow == 1) ? "" : "none";
}

function loadCustOnlineList(onlineList) {
    $("tbl_onlinelist").innerHTML = onlineList;
}

function showDownlineList(isShow) {
    if (isShow == 1) {
        $('tabbalance').style.display = 'none';
        $('tabdownline').style.display = '';
        //$('balinfo').className = "";
        //$('userdownline').className = "current";
    }
    else {
        $('tabbalance').style.display = '';
        $('tabdownline').style.display = 'none';
        $('balinfo').className = "current";
        $('userdownline').className = "";
    }
}
//Show/hide customer settings tab

function showSection(sectionID) {   
    switch (sectionID) {
        case "custsettings":
            $("agentsTab").className = "";
            $("membersTab").className = "";
            $("tbl_agentsTab").style.display = "none";
            $("tbl_membersTab").style.display = "none";
            break;
        case "agentsTab":
            $("custsettings").className = "";
            $("membersTab").className = "";
            $("tbl_custsettings").style.display = "none";
            $("tbl_membersTab").style.display = "none";
            $("tbl_agentsTab").style.display = "block";
            break;
        case "membersTab":
            $("custsettings").className = "";
            $("agentsTab").className = "";
            $("tbl_agentsTab").style.display = "none";
            $("tbl_custsettings").style.display = "none";
            $("tbl_membersTab").style.display = "none";
            /*
            ajax.Request('OnlineList.aspx', {
                asynchronous: true,
                parameters: 'custId=' + _page.UserId,
                onComplete: function(data) {
                    $("tbl_onlinelist").innerHTML = data.responseText;
                }
            });
            */
            break;
    }
    $(sectionID).className = "current";
    $("tbl_" + sectionID).style.display = "block";
}

function replaceAll(text, strA, strB) {
    while (text.indexOf(strA) != -1) {
        text = text.replace(strA, strB);
    }
    return text;
}

function isNumeric(x) {
    var RegExp = /^(-)?(\d*)(\.?)(\d*)$/; // Note: this WILL allow a number that ends in a decimal: -452.
    return x.match(RegExp);
}
//Check allow number only (except ESC, ENTER, ...)

function checkAllowNumber(e) {
    var code = (e.which) ? e.which : e.keyCode;
    if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 27 && code != 0 && code != 46 && (code < 37 || code > 40)) {
        return false;
    }
    return true;
}

function doClearAllCredit(custId, roleId) {
    var msgConfirm = _page.confirmClearCredit;
    if (roleId == 1) msgConfirm = _page.confirmClearCreditMem;
    if (!window.confirm(msgConfirm)) return false;
    ajax.PostJSON('Handlers/ClearAllCredit.ashx?custid=' + custId, '', onComplete);

    function onComplete(result) {
        if (result.errCode == 0) {
            resetGivenCredit();
        }
        else alert(result.errMsg);
    }
}

function OpenIPInfo(ip) {
    ipInfoWnd = window.open("../_IPInfo/IpInfo.aspx?ip=" + ip, "ipinfo", "location=0, width=370, height=140");
    ipInfoWnd.moveTo(500, 250);
}

function onclickUser(username) {
    if ($('txtUsername').value == username) {
        $('txtUsername').className = 'text_f';
        $('txtUsername').value = '';
    }
}

function onblurUser(username) {
    if ($('txtUsername').value == '') {
        $('txtUsername').className = 'text_f1';
        $('txtUsername').value = username;
    }
}
RegisterStartUp('initHeader()');

function initHeader() {
    initAutoComplete('txtUsername', 'Handlers/QueryUserName.ashx');
    $('leftpanewrapper').style.display = 'block';
 
}
// Menu2 & Acc Info (Common.js)----------------------
var currentItem = 0;
var oldClassName = 'b_left';
var newClassName = 'b_left_hover';

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

function _stopEvent(ev) {
    if (IE) {
        window.event.cancelBubble = true;
        window.event.returnValue = false;
    }
    else {
        ev.preventDefault();
        ev.stopPropagation();
    }
    return false;
};

function updateCustomerInfo(custId) {
    //    custid, firstName, lastName, phone, mobilePhone, email, fax
    //    var div = $(divId);
    var firstName = document.getElementById('txtFName').value;
    var lastName = document.getElementById('txtLName').value;
    var phone = document.getElementById('txtPhone').value;
    var mobile = document.getElementById('txtMobile').value;
    var email = document.getElementById('txtEmail').value;
    var fax = document.getElementById('txtFax').value;

    var params = new Array('custId=' + custId, 'firstName=' + firstName, 'lastName=' + lastName, 'phone=' + phone, 'mobile=' + mobile, 'email=' + email, 'fax=' + fax);

    // Post with return result in JSON format

    function OnComplete(result) {
        age.UnLock();
        var errCode = result.errCode;
        if (errCode == 0) {
            ageMsg.Hide();
            age.DelayReloadPage();
        } else {
            ageMsg.Show(result.errMsg);
        }
    }

    age.Lock(false);
    ajax.PostJSON(
    age.GetBaseUrl() + '_AccountInfo/Handlers/UpdateMemberinfo.ashx', params.join('&'), OnComplete);
}

function changeLiveCasinoStatus(evt, objDiv) {
    var isEnabled = $('chkLiveCasino').checked ? true : false;
    if (isEnabled) {
        if (!confirm(_page.confirmEnableLiveCasino)) {
            return _stopEvent(evt);
        }
    }
    else if (!confirm(_page.confirmDisableLiveCasino)) {
        return _stopEvent(evt);
    }

    ajax.PostJSON('../_MemberInfo/LiveCasino/Handlers/Agent/UpdateEnableStatus.ashx', 'custid=' + _page.UserId + '&enable=' + isEnabled + "&CustRoleId=" + _page.CustRoleId, onComplete);

    function onComplete(result) {
        if (result.errCode == 0) {
            $('chkLiveCasino').checked = isEnabled;
        }
        else {
            $('chkLiveCasino').checked = !isEnabled;
            alert(_page.updateFailed);            
        }
    }
}

function bindAgentDownLine(agentID) {
    ajax.PostJSON('../_AccountInfo/Handlers/GetAgentPathList.ashx', 'root=' + agentID, function (result) {
        alert(result.Agent.length);
        var data = result.Agent;
        $("agent-downline").innerHTML = "";
        for (var i = 0; i < data.length; ++i) {
            $("agent-downline").innerHTML = $("agent-downline").innerHTML + "<a href='#'> " + data[i][1] + "</a> </br>";
        }
    });
}


