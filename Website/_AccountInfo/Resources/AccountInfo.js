function ClearCurrentLeftMenu() { //do nothing here!
}
var oldClassName = 'b_left';
var newClassName = 'b_left_hover';

function checkAndSubmitUI() {
    if (document.getElementById('txtUsername').value == '') {
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
            var givencredit = replaceAll(document.getElementById(textboxCredit).value, ",", "");
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
            document.getElementById("spantxtGivenCredit").innerHTML = result.amount.toString();
            changeCredit(false, textboxCredit);
        }
        else alert(result.errMsg);
    }
}

function changeCredit(ischanging, textboxCredit) {
    try {
        var spanGC = document.getElementById('span' + textboxCredit);
        var txtGC = document.getElementById(textboxCredit);
        if (typeof ischanging != 'undefined' && ischanging) {
            if (document.getElementById('tdMemMaxCredit')) document.getElementById('tdMemMaxCredit').innerHTML = _page.lblMemberMax; //for nice GUI
            if (document.getElementById('tdAgOutstand')) document.getElementById('tdAgOutstand').innerHTML = _page.lblAgOutst; //for nice GUI
            txtGC.value = replaceAll(spanGC.innerHTML, ",", "");
            spanGC.style.display = 'none';
            txtGC.style.display = ''
            txtGC.focus();
        }
        else {
            if (document.getElementById('tdMemMaxCredit')) document.getElementById('tdMemMaxCredit').innerHTML = _page.lblMemberMaxCredit;
            if (document.getElementById('tdAgOutstand')) document.getElementById('tdAgOutstand').innerHTML = _page.lblAgentOutstanding;
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
    var isSuspended = document.getElementById('chkSuspend').checked ? false : true;
    document.getElementById('hdnTypeStatus').value = 'NO_POP'; //type popup: No popup
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
            document.getElementById('chkSuspend').checked = isSuspended;
        }
        else {
            alert(result.errMsg);
        }
    }
}

function changeClosedStatus(evt, objDiv) {
    if (objDiv.childNodes[1].disabled) return _stopEvent(evt);
    var isClosed = document.getElementById('chkClosed').checked ? false : true;
    var isClearAllCredit = false;
    if (isClosed) { //do close
        showClosedPopup();
        return _stopEvent(evt);
    }
    else {
        if (!confirm(_page.confirmOpenMem)) {
            return;
        }
        document.getElementById('hdnTypeStatus').value = 'NO_POP'; //type popup
        updateCloseStatus(isClosed, false);
    }

    function showClosedPopup() {
        document.getElementById('agtype').innerHTML = _page.closedStatus;
        document.getElementById('chkEnabled').checked = true;
        document.getElementById('chkEnabled').disabled = true;
        document.getElementById('hdnTypeStatus').value = 'Closed'; //type popup
        document.getElementById('lblEnableStatus').innerHTML = _page.titleclose; //title popup
        document.getElementById('lbldownline').innerHTML = _page.lblclearallcredit;
        document.getElementById('chkEnabledDL').disabled = true; // False
        document.getElementById('chkEnabledDL').checked = false; // True
        document.getElementById('btnUpdStatus').disabled = false;
        document.getElementById('btnUpdStatus').onclick = function() {
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
                document.getElementById('chkClosed').checked = isClosed;
                if (isClearAllCredit) resetGivenCredit();
            }
            else alert(result.errMsg);
            closeDivPopup();
        }
    }
}

function changeOutrightStatus(evt, objDiv) {
    if (objDiv.childNodes[1].disabled) return _stopEvent(evt);
    var isDisabled = document.getElementById('chkOutright').checked;
    document.getElementById('hdnTypeStatus').value = 'NO_POP'; //type popup: No popup
    var confirmMessage = isDisabled ? _page.confirmDisableOutright_Member : _page.confirmAllowOutright;
    if (!confirm(confirmMessage)) return _stopEvent(evt);
    ajax.PostJSON('Handlers/UpdateOutrightStatus.ashx', 'custid=' + _page.UserId + '&isdisabled=' + isDisabled, onComplete);

    function onComplete(result) {
        if (result.errCode == 0) {
            document.getElementById('chkOutright').checked = !isDisabled;
        }
        else alert(result.errMsg);
    }
}

function changeAutoMPTStatus(evt, objDiv) {
    if (objDiv.childNodes[1].disabled) return _stopEvent(evt);
    var isDisabled = document.getElementById('chkMAutoPT').checked;
    document.getElementById('hdnTypeStatus').value = 'NO_POP'; //No popup
    if (!confirm((isDisabled) ? _page.confirmDisableMAutoPT : _page.confirmEnableMAutoPT)) {
        return _stopEvent(evt);
    }
    ajax.PostJSON('Handlers/UpdateAutoMasterPTStatus.ashx', 'custid=' + _page.UserId + '&isdisabled=' + isDisabled, onComplete);

    function onComplete(result) {
        if (result.errCode == 0) {
            document.getElementById('chkMAutoPT').checked = !isDisabled;
        }
        else alert(result.errMsg);
    }
}

function allowSubmit(chkDisablePop) {
    //check agent status has changed to enable button submit
    if (!chkDisablePop.checked) {
        document.getElementById('btnUpdStatus').disabled = true;
        document.getElementById('btnUpdStatus').className = "btn2";
    }
    else {
        document.getElementById('btnUpdStatus').disabled = false;
        document.getElementById('btnUpdStatus').className = "btn";
    }
    //control downline status when change agent status
    if (chkDisablePop.checked == false) { //disable
        document.getElementById('chkEnabledDL').checked = false;
        document.getElementById('chkEnabledDL').disabled = true;
        document.getElementById('spanChkEnabledDL').style.display = 'none';
    }
    else { //enable
        document.getElementById('chkEnabledDL').checked = false;
        document.getElementById('chkEnabledDL').disabled = false;
        document.getElementById('spanChkEnabledDL').style.display = '';
    }
}

function resetGivenCredit() {
    document.getElementById('spantxtGivenCredit').innerHTML = '0.00';
}

function assignDefaultPopupValue() {
    document.getElementById('btnUpdStatus').className = "btn2";
    document.getElementById('agtype').innerHTML = _page.agenttype;
    document.getElementById('btnUpdStatus').disabled = true;
    document.getElementById('chkEnabled').checked = false;
    document.getElementById('chkEnabled').disabled = false;
    document.getElementById('chkEnabledDL').checked = false;
    document.getElementById('chkEnabledDL').disabled = true;
    document.getElementById('spanChkEnabledDL').style.display = 'none';
}

function showDivPopup(idCheckBox) {
    var obj = document.getElementById(idCheckBox);
    var curtop = 0;
    if (obj.offsetParent) {
        do {
            curtop += obj.offsetTop;
        } while (obj = obj.offsetParent);
    }
    curtop += 16;
    document.getElementById('disabledStatusPop').style.top = curtop + 'px';
    document.getElementById('disabledStatusPop').style.display = 'block'; //show popup
    document.getElementById('overlayBg').style.display = 'block'; //show overlay
}

function closeDivPopup() {
    if (document.getElementById('disabledStatusPop')) document.getElementById('disabledStatusPop').style.display = 'none';
    document.getElementById('overlayBg').style.display = 'none';
}

function toggleCheckBox(chkId) {
    document.getElementById(chkId).checked = !document.getElementById(chkId).checked;
}

function loadCustDownlineList(downlineList) {
    document.getElementById("tabdownline").innerHTML = downlineList;
}

function showEditPage(isShow) {
    document.getElementById('spPage').style.display = (isShow == 1) ? 'none' : '';
    document.getElementById('txtPage').style.display = (isShow == 1) ? '' : 'none';
    if (isShow) {
        document.getElementById('txtPage').value = document.getElementById('spPage').innerHTML;
        document.getElementById('txtPage').focus();
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
                var pageIdx = parseInt(document.getElementById('txtPage').value);
                var totalPage = parseInt(document.getElementById('hdnTotalPage').value);
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
    document.getElementById('ploading').style.display = (isShow == 1) ? "" : "none";
}

function loadCustOnlineList(onlineList) {
    document.getElementById("tbl_onlinelist").innerHTML = onlineList;
}

function showDownlineList(isShow) {
    if (isShow == 1) {
        document.getElementById('tabbalance').style.display = 'none';
        document.getElementById('tabdownline').style.display = '';
        //document.getElementById('balinfo').className = "";
        //document.getElementById('userdownline').className = "current";
    }
    else {
        document.getElementById('tabbalance').style.display = '';
        document.getElementById('tabdownline').style.display = 'none';
        document.getElementById('balinfo').className = "current";
        document.getElementById('userdownline').className = "";
    }
}
//Show/hide customer settings tab

function showSection(sectionID, agentId) {   
    switch (sectionID) {
        case "custsettings":
            document.getElementById("agentsTab").className = "";
            document.getElementById("membersTab").className = "";
            document.getElementById("tbl_agentsTab").style.display = "none";
            document.getElementById("tbl_membersTab").style.display = "none";
            break;
        case "agentsTab":
            document.getElementById("custsettings").className = "";
            document.getElementById("membersTab").className = "";
            document.getElementById("tbl_custsettings").style.display = "none";
            document.getElementById("tbl_membersTab").style.display = "none";
            document.getElementById("tbl_agentsTab").style.display = "block";           
            break;
        case "membersTab":
            document.getElementById("custsettings").className = "";
            document.getElementById("agentsTab").className = "";
            document.getElementById("tbl_agentsTab").style.display = "none";
            document.getElementById("tbl_custsettings").style.display = "none";
            document.getElementById("tbl_membersTab").style.display = "block";
            bindMembersList(agentId);

            /*
            ajax.Request('OnlineList.aspx', {
            asynchronous: true,
            parameters: 'custId=' + _page.UserId,
            onComplete: function(data) {
            document.getElementById("tbl_onlinelist").innerHTML = data.responseText;
            }
            });
            */
            break;
    }
    
    document.getElementById(sectionID).className = "current";
    document.getElementById("tbl_" + sectionID).style.display = "block";
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
    if (document.getElementById('txtUsername').value == username) {
        document.getElementById('txtUsername').className = 'text_f';
        document.getElementById('txtUsername').value = '';
    }
}

function onblurUser(username) {
    if (document.getElementById('txtUsername').value == '') {
        document.getElementById('txtUsername').className = 'text_f1';
        document.getElementById('txtUsername').value = username;
    }
}
RegisterStartUp('initHeader()');

function initHeader() {
    initAutoComplete('txtUsername', 'Handlers/QueryUserName.ashx');
    document.getElementById('leftpanewrapper').style.display = 'block';
 
}
// Menu2 & Acc Info (Common.js)----------------------
var currentItem = 0;
var oldClassName = 'b_left';
var newClassName = 'b_left_hover';

function menu_select(order) {
    clearTopMenu();
    document.getElementById('menuitem' + order).className = newClassName;
    if (currentItem != order) {
        var item = document.getElementById('menuitem' + currentItem);
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
    //    var div = document.getElementById(divId);
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
    var isEnabled = document.getElementById('chkLiveCasino').checked ? true : false;
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
            document.getElementById('chkLiveCasino').checked = isEnabled;
        }
        else {
            document.getElementById('chkLiveCasino').checked = !isEnabled;
            alert(_page.updateFailed);            
        }
    }
}

function bindAgentDownLine(agentID) {
    
//    var contentId = document.getElementById("agent-downline");
//    var container = document.getElementById("agent-downline-list");
//    if (container.style.display == "none") {
//        ajax.PostJSON('../_AccountInfo/Handlers/GetAgentPathList.ashx', 'root=' + agentID, function (result) {
//            var data = result.Agents;
//            var text = "";
//            document.getElementById("agent-downline").innerHTML = "";
//            document.getElementById("agent-downline").innerHTML = document.getElementById("agent-downline").innerHTML + "<span class='root-agent'>" + data[0][1] + "</span> </br>";
//            for (var i = 1; i < data.length; ++i) {
//                text += "<li><a href='../_AccountInfo/AccountInfo.aspx?IsMember=0&custid=" + data[i][0] + "&txtUsername=" + data[i][1] + "'>\\ " + data[i][1] + "</a></li>";
//            }

//            document.getElementById("agent-downline").innerHTML = document.getElementById("agent-downline").innerHTML + text;
//            container.style.display = "block";
//        });
//        
//    }
//    else {
//        container.style.display = "none";
    //    }

//    var contentId = $("#agent-downline");

//    $("#agent-downline-list").toggle('fast', function() {
//        ajax.PostJSON('../_AccountInfo/Handlers/GetAgentPathList.ashx', 'root=' + agentID, function (result) {
//            var data = result.Agents;
//            var text = "";
//            contentId.html("");
//            contentId.html(contentId.html() + "<span class='root-agent'>" + data[0][1] + "</span> </br>");
//            for (var i = 1; i < data.length; ++i) {
//                text += "<li><a href='../_AccountInfo/AccountInfo.aspx?IsMember=0&custid=" + data[i][0] + "&txtUsername=" + data[i][1] + "'>\\ " + data[i][1] + "</a></li>";
//            }
//            contentId.html(contentId.html() + text);
//        });
//    });  
}

function bindMembersList(agentID) {
    var link = "../_AccountInfo/DownlineMemberList.aspx?custid=" + agentID;
    var iframe = document.createElement('iframe');
    iframe.frameBorder = 0;
    iframe.width = "100%";
    iframe.height = "100%";
    iframe.id = "randomid";
    iframe.setAttribute("src", link);

    var contentId = document.getElementById("iframe-content");
    contentId.innerHTML = "";
    contentId.appendChild(iframe);

    /*
    ajax.PostJSON('../_AccountInfo/Handlers/GetMemberList.ashx', 'root=' + agentID + "&pagesize=20&pageindex=1", function (result) {
        var data = result.Members;
        var text = "";
        for (var i = 0; i < data.length; ++i) {
            text += "<li><a href='../_AccountInfo/AccountInfo.aspx?IsMember=1&custid=" + data[i][0] + "&txtUsername=" + data[i][1] + "'> " + data[i][1] + "</a></li>";
        }

        var contentId = document.getElementById("members-list");
        text = "";
        if (text.length > 0) {
            contentId.innerHTML = text;
            //$("#paging-member").show();
        }
        else {
            // $("#paging-member").hide();
            var link = "../_AccountInfo/DownLineTreeView.aspx?root=2";
            var iframe = document.createElement('iframe');
            iframe.frameBorder = 0;
            iframe.width = "300px";
            iframe.height = "250px";
            iframe.id = "randomid";
            iframe.setAttribute("src", link);
            document.getElementById("iframe-content").appendChild(iframe);
        }

        $('#members-list').slimscroll({
            width: '161px',
            height: '546px',
            railVisible: true,
            allowPageScroll: true,
            touchScrollStep: 1000,
            railDraggable: true
        }).parent().css({
            "width": "179px",
            "height": "554px"
        });

        }           
        contentId.style.display = "block";
    });
    */
}

if (typeof jQuery != 'undefined') {
    $(function () {
        $("div .view-agent-downline").click(function (event) {
            var $this = $(this);
            agentID = $this.attr('aid');
            contentId = $("#agent-downline");
            $("div #agentDownlineLink").find("img").attr("src", "../_AccountInfo/Resources/Images/icon-view-info-hover.png");
            $("div .agent-tooltip").show('fast', function () {
                ajax.PostJSON('../_AccountInfo/Handlers/GetAgentPathList.ashx', 'agentId=' + agentID, function (result) {
                    var data = result.Agents;
                    var text = "";
                    contentId.html("");
                    contentId.html(contentId.html() + "<span class='root-agent'>" + data[0][1] + "</span> </br>");
                    for (var i = 1; i < data.length; ++i) {
                        text += "<li><a href='../_AccountInfo/AccountInfo.aspx?IsMember=0&custid=" + data[i][0] + "&txtUsername=" + data[i][1] + "'>\\ " + data[i][1] + "</a></li>";
                    }
                    contentId.html(contentId.html() + text);
                });
            });
            event.stopPropagation();
        });

        $('body, html').on('click', function () {
            if ($("div .view-agent-downline") != undefined) {
                $("div .agent-tooltip").hide();
                $("div #agentDownlineLink").find("img").attr("src", "../_AccountInfo/Resources/Images/icon-view-info.png");
            }
        });
    });
}
