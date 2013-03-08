/*
* Created: David
* Task: Customer List
* Revisions:
*    - 20111228@Terry: Fixbug for Close function - UpdateCustomerStatus
*    - 20121010@Marc: Add double commission feature
*/

// Local variable
var arrCusId = "";
var selectCusId = "";
var selectUser = "";
var Count = 0;

// count the checkbox is checked
function CheckedCount(id) {
    arrCusId = document.getElementsByName(id);
    if (!arrCusId) return;
    for (var i = 0; i < arrCusId.length; i++) {
        if (arrCusId[i].checked) {
            selectCusId += arrCusId[i].id.split('_')[1] + "^";
            selectUser += arrCusId[i].value + "^";
            Count += 1;
        }
    }
}

function showTransferCondition(id, username) {
    var popH = 230,
		popW = 565;
    //url, title, left, top, width, height
    ageWnd.open('../../../_MemberInfo/TransferCondition/EditTransferCondition.aspx?custid=' + id + '&username=' + username, '', 200, 300, popW, popH);
}

// View Outstanding of username
function viewCustOutSt(username, custid) {
    window.location.href = "../../../_Reports/BetList/DailyStatement.aspx?username=" + username + "&option=1&custid=" + custid + "&page=1&chk_ShowSB=on&chk_ShowCasino=on&chk_ShowRB=on&chk_showng=off&chk_ShowBI=off";
}

// View detail Ip infomation
function OpenIPInfo(ip) {
    ageWnd.open('../../../_IPInfo/IPInfo.aspx?ip=' + ip, '', 270, 300, 400, 150);
}

// Show Popup Edit Password
function showReset(id, usn, isSync) {
    var popH = 145,
		popW = 375;
    var url = '../../../_AccountInfo/ResetPassword.aspx?custid=' + id + '&usn=' + usn + '&isSyncCasino=' + isSync;
    //url, title, left, top, width, height
    ageWnd.open(url, '', 270, 300, popW, popH);
}

// View Statement on Usename
function viewStatement(custid, type) {
    var roleid = 0;
    if (type == "Master") {
        roleid = 3;
    } else if (type == "Agent") {
        roleid = 2;
    }
    window.location.href = "../../../_Reports/Common/Statement.aspx?custid=" + custid + (roleid > 0 ? "&roleid=" + roleid : "");
}

function GetPosition(element) {
    var left = 0;
    var top = 0;
    if (element.offsetParent) {
        while (element) {
            left += element.offsetLeft;
            top += element.offsetTop;
            element = element.offsetParent;
        }
    }
    return {
        offsetLeft: left,
        offsetTop: top
    };
}

function ViewDownLine(url) {
    window.location.href = url;
    return;
}

function GetCustomer(url) {
    var txtname = $('txtUserName').value;
    if (txtname == _page.usernamedefault) {
        txtname = '';
    }

    url = SetParameterValue("custid", _page.custid, url);
    url = SetParameterValue("username", txtname, url);
    url = SetParameterValue("status", $('statusFilter').value, url);

    //    url = SetParameterValue("pageIndex", $('txt_PagingTop') ? $('txt_PagingTop').value : 1, url);
    url = SetParameterValue("pageSize", $('sel_PagingTop').value, url);

    if (!$('dcommFilter') == false) {
        url = SetParameterValue("dComStatus", $('dcommFilter').value, url);
    }
    else {
        url = SetParameterValue("dComStatus", '2', url);
    }

    location.href = url;
}

function CheckAll_onClick(els) {
    var arrChk = document.getElementsByName('chkid');
    for (var i = 0; i < arrChk.length; i++) {
        if (els.checked && !arrChk[i].checked) {
            arrChk[i].checked = true;
        }
        else if (!els.checked && arrChk[i].checked) {
            arrChk[i].checked = false;
        }
    }
}

function ItemCheck_OnClick(els) {
    var arrChk = document.getElementsByName('chkid');
    if (!els.checked) { // Uncheck
        $("chkAll").checked = false;
    }
    else { // Checked
        var isChecked = true;
        for (var i = 0; i < arrChk.length; i++) {
            if (!arrChk[i].checked) {
                isChecked = false;
                break;
            }
        }
        if (isChecked) {
            $("chkAll").checked = true;
        }
    }
}

function ChangeAtribute(id, attDestinationName, attSourceName) {
    if ($(id)) {
        var sourceName = $(id).getAttribute(attSourceName);
        $(id).setAttribute(attDestinationName, sourceName);
        $(id).removeAttribute(attSourceName);
    }
}

// Render Popup Status
function RenderStatus(el, divID, custId, closed, suspended, allowAutoPT, outright, id) {
    var div = $(divID);
    var chk_closed = $('chk_closed');
    var chk_suspended = $('chk_suspended');
    var chk_autoPT = $('chk_allowAutoPT');
    var chk_outright = $('chk_outright');

    if (chk_closed.value == '1') {
        chk_closed.parentNode.style.display = 'none';
    }
    if (chk_suspended.value == '1') {
        chk_suspended.parentNode.style.display = 'none';
    }
    if (chk_autoPT.value == '1') {
        chk_autoPT.parentNode.style.display = 'none';
    }
    if (chk_outright.value == '1') {
        chk_outright.parentNode.style.display = 'none';
    }

    //Old status
    div.custid = custId;
    div.isClosed = div.isClosed1 = closed;
    div.isSuspended = div.isSuspended1 = suspended;
    div.isAutoPT = div.isAutoPT1 = allowAutoPT;
    div.isOutright = div.isOutright1 = outright;
    div.isCancelSubmit = false;

    div.isSync = $(id).getAttribute('statuscs');

    chk_closed.checked = closed ? 'checked' : null;

    chk_suspended.disabled = null;
    chk_suspended.checked = suspended ? 'checked' : null;
    if ($('isDisableSuspendedStatus').value == 'true') {
        chk_suspended.disabled = 'disable';
    } else chk_suspended.disabled = null;

    chk_autoPT.checked = allowAutoPT ? 'checked' : null;

    chk_outright.disabled = null;
    chk_outright.checked = outright ? 'checked' : null;
    if ($('isDisableAllowOutrightStatus').value == 'true') {
        chk_outright.disabled = 'disable';
    } else chk_outright.disabled = null;

    //display popup
    el.style.position = 'relative';
    var pos = GetPosition(el);
    div.style.top = (pos.offsetTop - 35) + 'px';
    div.style.left = (pos.offsetLeft + 12) + 'px';
    div.style.display = 'block';

    clearTimeout(div.timerID);
    div.timerID = 0;
    div.timerID = setTimeout("HidePopupStatus('" + divID + "');", 2000);
    age.addEvent(div, "mouseover", function () {
        clearTimeout(div.timerID);
        div.timerID = 0;
    }, true);

    age.addEvent(chk_closed, "click", function () {
        div.isClosed = chk_closed.checked ? true : false;
    }, true);
    age.addEvent(chk_suspended, "click", function () {
        div.isSuspended = chk_suspended.checked ? true : false;
    }, true);

    age.addEvent(chk_autoPT, "click", function () {
        div.isAutoPT = chk_autoPT.checked ? true : false;
    }, true);

    age.addEvent(chk_outright, "click", function () {
        div.isOutright = chk_outright.checked ? true : false;
    }, true);

    age.addEvent(div, "mouseout", function () {
        clearTimeout(div.timerID);
        div.timerID = 0;
        div.timerID = setTimeout("HidePopupStatus('" + divID + "');", 100);
    }, true);
}

function HidePopupStatus(divID) {
    var div = $(divID);
    if (!div) return false;
    div.style.display = 'none';

    var isClosed = (div.isClosed != div.isClosed1) ? div.isClosed : '-1';
    var isSuspended = (div.isSuspended != div.isSuspended1) ? div.isSuspended : '-1';
    var isAllowAutoMPT = (div.isAutoPT != div.isAutoPT1) ? div.isAutoPT : '-1';
    var isOutright = (div.isOutright != div.isOutright1) ? div.isOutright : '-1';
    var isSyncCasino = div.isSync;
    // Call saving
    if ((isClosed == '-1') && (isSuspended == '-1') && (isAllowAutoMPT == '-1') && (isOutright == '-1')) return;

    if (div.isCancelSubmit) {
        return;
    }
    UpdateCustomerStatus(div.custid, isClosed, isSuspended, isAllowAutoMPT, isOutright, isSyncCasino);
}

//Update customer on PopupSatus
function UpdateCustomerStatus(custId, isClosed, isSuspended, isAllowAutoMPT, isOutright, isSyncCasino) {
    var isClearCredit = '-1';
    if (isClosed == 1) {
        if (_page.isAgentLevel == 1 && !confirm(_page.confirmCloseMem)) return;
        if (_page.isAgentLevel == 0 && !confirm(_page.confirmCloseMemDownline)) return;
    }
    else if (isClosed == 0) {
        if (!confirm(_page.confirmCloseMem)) return;
    }
    else if (isSuspended == 1) {
        {
            if (_page.isAgentLevel == 1) {
                if (!confirm(_page.confirmSuspendMem)) return;
            }
            else {
                if (!confirm(_page.confirmSuspendDownline)) return;
            }
        }
    } else if (isSuspended == 0) {
        if (_page.isAgentLevel == 1) {
            if (!confirm(_page.confirmUnSuspendMem)) return;
        }
        else {
            if (!confirm(_page.confirmUnSuspendDownline)) return;
        }
    }

    var params = new Array('custId=' + custId, 'isClosed=' + isClosed, 'isSuspended=' + isSuspended, 'isAllowAutoMPT=' + isAllowAutoMPT, 'isOutright=' + isOutright, 'isSyncCasino=' + isSyncCasino, 'isClearCredit=' + isClearCredit);

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
	age.GetBaseUrl() + '_MemberInfo/CustomerList/Handler/CustomerList.ashx', params.join('&'), OnComplete);
}

function RenderMulti(el, divID, custLevel, upline_id, masterid) {
    var div = $(divID);
    var ArrChkId = document.getElementsByName("chkid");
    var ischecked = false;

    var counter = 0;
    var counterCS = 0;
    var counterBG = 0;
    var counterP2P = 0;
    var counterLiveCS = 0;
    var counterVirtualSports = 0;

    var custid = 0;
    var username = '';

    var custidCS = 0;
    var custidBG = 0;
    var custidP2P = 0;
    var custidLiveCS = 0;
    var custidVirtualSports = 0;

    var status = '';
    var statusBingo = ''; //  For Bingo
    var statusP2P = ''; //  For P2P
    var statusLiveCS = '';
    var statusVirtualSports = ''; //  For Virtual Sports
    var isCasino = false;
    var isBingo = false;
    var isP2P = false;
    var isLiveCS = false;
    var isVirtualSports = false;

    var racing = $('racing');
    var casino = $('casino');
    var bingo = $('bingo');
    var p2p = $('P2P');
    var liveCasino = $('livecasino');
    var virtualSports = $('virtualsports');
    var doubleComm = $('doublecomm');

    for (var j = 0; j < ArrChkId.length; j++) {
        if (ArrChkId[j].checked) {
            if (casino.value != 1) {
                var strStatus = ArrChkId[j].getAttribute('statusCS');
                status += strStatus + ';';
                if (!isCasino) {
                    if (strStatus == "0" || strStatus == "1") {
                        isCasino = true;
                    } else counterCS--;
                }
            }
            if (bingo.value != 1) {
                var strStatus = ArrChkId[j].getAttribute('statusBingo');
                statusBingo += strStatus + ';';
                if (!isBingo) {
                    if (strStatus == "0" || strStatus == "1") {
                        isBingo = true;
                    } else counterBG--;
                }
            }
            if (p2p.value != 1) {
                var strStatus = ArrChkId[j].getAttribute('statusP2P');
                statusP2P += strStatus + ';';
                if (!isP2P) {
                    if (strStatus == "0" || strStatus == "1") {
                        isP2P = true;
                    } else counterP2P--;
                }
            }
            if (liveCasino.value != 1) {
                var strStatus = ArrChkId[j].getAttribute('statusLiveCasino');
                statusLiveCS += statusLiveCS + ';';
                if (!isLiveCS) {
                    if (strStatus == "0" || strStatus == "1") {
                        isLiveCS = true;
                    } else counterLiveCS--;
                }
            }
            if (virtualSports.value != 1) {
                var strStatus = ArrChkId[j].getAttribute('statusvirtualsport');
                statusVirtualSports += strStatus + ';';
                if (!isVirtualSports) {
                    if (strStatus == "0" || strStatus == "1") {
                        isVirtualSports = true;
                    } else counterVirtualSports--;
                }
            }

            ischecked = true;
            counter++;
            counterCS++;
            counterBG++;
            counterP2P++;
            counterLiveCS++;
            counterVirtualSports++;

            if (counter > 1 && (counterCS > 1 || counterBG > 1 || counterP2P > 1 || counterLiveCS > 1 || counterVirtualSports > 1)) {
                break;
            }
            if (counter == 1) { // First other custid
                custid = ArrChkId[j].id.split('_')[1];
                username = ArrChkId[j].value;
            }
            if (counterCS == 1) { // First casino custid
                custidCS = ArrChkId[j].id.split('_')[1];
            }
            if (counterBG == 1) { // First bingo custid
                custidBG = ArrChkId[j].id.split('_')[1];
            }
            if (counterP2P == 1) { // First p2p custid
                custidP2P = ArrChkId[j].id.split('_')[1];
            }
            if (counterLiveCS == 1) { // First liveCasino custid
                custidLiveCS = ArrChkId[j].id.split('_')[1];
                username = ArrChkId[j].value;
            }
            if (counterVirtualSports == 1) { // First VirtualSports custid
                custidVirtualSports = ArrChkId[j].id.split('_')[1];
            }
        }
    }

    if (ischecked) {
        el.style.position = 'relative';
        var pos = GetPosition(el);
        div.style.top = (pos.offsetTop - 15) + 'px';
        div.style.left = (pos.offsetLeft + 20) + 'px';

        clearTimeout(div.timerID);
        div.timerID = 0;
        div.timerID = setTimeout("HidePopupGeneral('" + divID + "');", 2000);
        age.addEvent(div, "mouseover", function () {
            clearTimeout(div.timerID);
            div.timerID = 0;
        }, true);

        // Link to edit SportBook
        var hrefSport = $('sportBook');
        if (_page.roleid == 4) {
            if (custLevel == "Super" || custLevel == "Master") hrefSport.href = 'javascript:EditMember_Multi(' + upline_id + ',"' + custLevel + '",' + masterid + ')';
            else hrefSport.href = 'javascript:EditMaster_Multi(' + upline_id + ')';
        } else if (_page.roleid == 3) {
            if (custLevel == "Super" || custLevel == "Master") hrefSport.href = 'javascript:EditMember_Multi(' + upline_id + ',"' + custLevel + '",' + masterid + ')';
            else hrefSport.href = 'javascript:EditAgent_Multi(' + upline_id + ')';
        } else hrefSport.href = 'javascript:EditMember_Multi(' + upline_id + ',"' + custLevel + '",' + masterid + ')';

        // Link to edit Racing
        if (racing.getAttribute('value') != '0') {
            $('tr_racing').style.display = 'none';
        } else {
            racing.style.display = '';
            var ismult = 0;
            if (counter > 1) ismult = 1;

            if (_page.roleid == 4) {
                if (custLevel == "Super") {
                    racing.href = 'javascript:CustomerEditHR("' + custid + '","Member",' + ismult + ')';
                } else {
                    racing.href = 'javascript:CustomerEditHR("' + custid + '","Master",' + ismult + ')';
                }
            } else if (_page.roleid == 3) {
                if (custLevel == "Master") {
                    racing.href = 'javascript:CustomerEditHR("' + custid + '","Member",' + ismult + ')';
                } else {
                    racing.href = 'javascript:CustomerEditHR("' + custid + '","Agent",' + ismult + ')';
                }
            } else {
                racing.href = 'javascript:CustomerEditHR("' + custid + '","Member",' + ismult + ')';
            }
        }

        //Link to edit Casino
        if (casino.getAttribute('value') != '0') {
            $('tr_casino').style.display = 'none';
        } else {
            if (!isCasino) // RemoveAttribute("MultiPopup", "tr_casino");
                $('tr_casino').style.display = 'none';
            else casino.style.display = '';
            if (counterCS > 1) casino.href = 'javascript:EditMulti("","' + status + '",1,1)';
            else casino.href = 'javascript:EditMulti(' + custidCS + ',0,"' + status + '",1)';
        }

        //Link to edit Bingo
        if (bingo.getAttribute('value') != '0') {
            $('tr_bingo').style.display = 'none';
        } else {
            if (!isBingo) // RemoveAttribute("MultiPopup", "tr_casino");
                $('tr_bingo').style.display = 'none';
            else bingo.style.display = '';
            if (counterBG > 1) bingo.href = 'javascript:EditMulti("","' + statusBingo + '",1,2)';
            else bingo.href = 'javascript:EditMulti(' + custidBG + ',0,"' + statusBingo + '",2)';
        }

        //Link to edit P2P
        if (p2p.getAttribute('value') != '0') {
            $('tr_P2P').style.display = 'none';
        } else {
            if (!isP2P) // RemoveAttribute("MultiPopup", "tr_casino");
                $('tr_P2P').style.display = 'none';
            else p2p.style.display = '';
            if (counterP2P > 1) p2p.href = 'javascript:EditMulti("","' + statusP2P + '",1,3)';
            else p2p.href = 'javascript:EditMulti(' + custidP2P + ',0,"' + statusP2P + '",3)';
        }

        //Link to edit liveCasino
        if (liveCasino.getAttribute('value') != '0') {
            $('tr_livecasino').style.display = 'none';
        } else {
            if (!isLiveCS) // RemoveAttribute("MultiPopup", "tr_livecasino");
                $('tr_livecasino').style.display = 'none';
            else liveCasino.style.display = '';
            if (counterLiveCS > 1) {
                if (_page.roleid == 4 && _page.roleidLiveCasino == 3) {
                    liveCasino.href = 'javascript:EditMultiLiveCasino("","' + statusLiveCS + '",1,1)';
                }
                else if (_page.roleid == 4 && _page.roleidLiveCasino == 1) {
                    liveCasino.href = 'javascript:EditMultiLiveCasino("","' + statusLiveCS + '",1,2)';
                }
                else if (_page.roleid == 3 && _page.roleidLiveCasino == 2) {
                    liveCasino.href = 'javascript:EditMultiLiveCasino("","' + statusLiveCS + '",1,3)';
                }
                else if (_page.roleid == 3 && _page.roleidLiveCasino == 1) {
                    liveCasino.href = 'javascript:EditMultiLiveCasino("","' + statusLiveCS + '",1,4)';
                }
                else if (_page.roleid == 2) {
                    liveCasino.href = 'javascript:EditMultiLiveCasino("","' + statusLiveCS + '",1,5)';
                }
            }
            else {
                liveCasino.href = 'javascript:EditLiveCS("' + custidLiveCS + '", "' + _page.roleid + '", "' + username + '")';
            }
        }

        //Link to edit VirtualSports
        if (virtualSports.getAttribute('value') != '0') {
            $('tr_virtualsports').style.display = 'none';
        }
        else {
            if (!isVirtualSports) {
                $('tr_virtualsports').style.display = 'none';
            }
            else {
                virtualSports.style.display = '';
            }
            virtualSports.href = 'javascript:EditMultiVirtualSports()';
        }

        //Link to edit DoubleComm
        var isShowDCom = $('EdiMulti').parentNode.attributes.getNamedItem('isshowdcom');
        var isDirectDownline = $('EdiMulti').parentNode.attributes.getNamedItem('isdirectdownline');

        // Check permission first
        if (!isShowDCom == false && isShowDCom.value == 'True') {
            // Then check editing context: allow edit on direct downline only
            if (!isDirectDownline == false && isDirectDownline.value == '1') {
                doubleComm.href = 'javascript:MultiEditDComm()';
            }
            else {
                doubleComm.style.display = 'none';
            }
        }
        else {
            doubleComm.style.display = 'none';
        }

        div.style.display = 'block';
        age.addEvent(div, "mouseout", function () {
            clearTimeout(div.timerID);
            div.timerID = 0;
            div.timerID = setTimeout("HidePopupGeneral('" + divID + "');", 100);
        }, true);
    }
}

// using hide popup of editMulti and Other
function HidePopupGeneral(divID) {
    var div = $(divID);
    if (!div) return false;
    div.style.display = 'none';
}

// Get custId when checkboxs is checked
function GetCustID(CheckedID) {
    arrCusId = "";
    selectCusId = "";
    selectUser = "";
    Count = 0;
    CheckedCount(CheckedID);

    if (Count == 0) return;
    if (Count == 1) {
        $("arrayCustID").value = "";
        $("arrayUserName").value = "";
    } else if (Count > 1) {
        $("arrayCustID").value = selectCusId;
        $("arrayUserName").value = selectUser;
    }
    return selectCusId.substring(0, selectCusId.indexOf("^"));
}

function SetAttributeURL(Id, url) {
    var elm = $(Id);
    if (elm) elm.href = 'javascript:' + url;
}

function ChangedIcon(Id, Icon) {
    if (Icon == "0") {
        $("tr_" + Id).style.display = 'none';
        return;
    }
    var className = "LinkPopup";
    var viewOnly = top.AGE.PERMISSIONS.indexOf('[B]') == -1;
    if (viewOnly) {
        className = "View";
    }
    else {
        switch (Icon) {
            case "1":
                className = "Enable";
                break;
            case "2":
                className = "Disable_edit";
                break;
            case "3":
                className = "Synchronize";
                break;
            case "4":
                className = "View";
                break;
        }
    }
    if (document.getElementById(Id)) {
        document.getElementById(Id).className = className;
    }
}

function EditMenuControl(element, popupId, custId, userName) {
    var div = $(popupId);
    if (!div) return false;

    element.style.position = 'relative';
    var pos = GetPosition(element);
    div.style.top = (pos.offsetTop - 35) + 'px';
    div.style.left = (pos.offsetLeft + 17) + 'px';

    div.style.display = 'block';
    clearTimeout(div.timerID);
    div.timerID = 0;
    div.timerID = setTimeout("HidePopupGeneral('" + popupId + "');", 2000);
    age.addEvent(div, "mouseover", function () {
        clearTimeout(div.timerID);
        div.timerID = 0;
    }, true);

    age.addEvent(div, "mouseout", function () {
        clearTimeout(div.timerID);
        div.timerID = 0;
        div.timerID = setTimeout("HidePopupGeneral('" + popupId + "');", 100);
    }, true);
}

function ShowFrmUpdOthers(el, divID, custid, username, actionOn) {
    var div = $(divID);
    if (!div) return false;

    var isEdit = "";

    ChangedIcon('editInfo', isEdit);
    ChangedIcon('betSetting', isEdit);
    ChangedIcon('commission', isEdit);

    if (document.getElementById('sportBookMinPT') != null) {
        ChangedIcon('sportBookMinPT', "1");
    }
    if (document.getElementById('racingMinPT') != null) {
        ChangedIcon('racingMinPT', "1");
    }
    if (document.getElementById('transfer') != null) {
        ChangedIcon('transfer', "1");
    }

    var urlEditMinPT_SB = 'OpenEditMinMax(' + custid + ',"' + username + '")';
    SetAttributeURL('sportBookMinPT', urlEditMinPT_SB);

    var urlEditMinPT_Racing = 'OpenEditMinMaxHR(' + custid + ',"' + username + '")';
    SetAttributeURL('racingMinPT', urlEditMinPT_Racing);

    if (actionOn == 'Master' || actionOn == 'Agent') {
        if (_page.isHideTransfer == '0') {
            var urlTransfer = 'showTransferCondition(' + custid + ',"' + username + '")';
            SetAttributeURL('transfer', urlTransfer);
        }
        else {
            $("tr_transfer").style.display = 'none';
        }
    }

    var urlPass = '';  //'showReset(' + custid + ',"' + username + '",' + isCasino + ')';
    SetAttributeURL('password', urlPass);

    var role = _page.roleid - 1;

    var urlEditInfo = '';  //"EditInfo(" + custid + ",'" + username + "'," + isCasino + "," + isBingo + ")";
    if (document.getElementById('editInfo') != null) {
        SetAttributeURL('editInfo', urlEditInfo);
    }

    var urlEditBetSetting = "EditBetSetting(" + custid + ",'" + username + "')";
    if (document.getElementById('betSetting') != null) {
        SetAttributeURL('betSetting', urlEditBetSetting);
    }

    var urlEditCommission = "EditCommission(" + custid + ",'" + username + "'," + role + ")";
    if (document.getElementById('commission') != null) {
        SetAttributeURL('commission', urlEditCommission);
    }

    el.style.position = 'relative';
    var pos = GetPosition(el);
    div.style.top = (pos.offsetTop - 35) + 'px';
    div.style.left = (pos.offsetLeft + 17) + 'px';

    div.style.display = 'block';
    clearTimeout(div.timerID);
    div.timerID = 0;
    div.timerID = setTimeout("HidePopupGeneral('" + divID + "');", 2000);
    age.addEvent(div, "mouseover", function () {
        clearTimeout(div.timerID);
        div.timerID = 0;
    }, true);

    age.addEvent(div, "mouseout", function () {
        clearTimeout(div.timerID);
        div.timerID = 0;
        div.timerID = setTimeout("HidePopupGeneral('" + divID + "');", 100);
    }, true);
}

function DelayPage(time) {
    var delay = time ? time : 2000;
    location.nextUrl = age.RemoveBookmarksInUrl(location.href);
    setTimeout("location = location.nextUrl", delay);
}

function DelayReloadPage(time) {
    DelayPage(time);
}

function getPrint(print_area) {
    var printContent = $(print_area);
    var printWindow = window.open('', '', 'left=500,top=400,width=200,height=5');
    printWindow.document.write("<html>");
    printWindow.document.write("<head>");
    printWindow.document.write("</head>");
    printWindow.document.write("<body style='margin-top: 100px'>");
    printWindow.document.write(printContent.innerHTML);
    printWindow.document.write("</body></html>");
    printWindow.document.close();
    printWindow.focus();
    printWindow.print();
    printWindow.close();
}

function EditInfo(custid, username, syncCasino, syncBingo) {
    var URL = '../../../_AccountInfo/EditCustomerInfo.aspx?custId=' + custid + '&username=' + username + '&isSyncCasino=' + syncCasino + '&isSyncBingo=' + syncBingo;
    var popH = 170, popW = 560;
    ageWnd.open(URL, '', 170, 300, popW, popH);
}

function EditBetSetting(custid, username) {
    var URL = '../../../_MemberInfo/BetSetting/EditBetSetting.aspx?custId=' + custid + '&username=' + username;
    var popH = 470, popW = 870;
    ageWnd.open(URL, '', 50, 200, popW, popH);
}

function EditCommission(custid, username, roleid) {
    var popH = 240, popW = 1015;
    var URL = '../../EditCommission/EditCommission.aspx?custId=' + custid + '&username=' + username + "&roleId=" + roleid;

    if (roleid == 1) {
        URL = '../../EditCommission/EditMemberCommission.aspx?custId=' + custid + '&username=' + username;
        popW = 870;
        popH = 250;
    }
    else {
        popW = 886;
        popH = 170;
    }

    ageWnd.open(URL, '', 50, 200, popW, popH);
}

function MultiEditDComm() {
    var arCID = document.getElementsByName("chkid");
    if (!arCID) return;

    var selCID = "";
    for (var i = 0; i < arCID.length; i++) {
        if (arCID[i].checked) {
            selCID += arCID[i].id.split('_')[1] + "^";
        }
    }

    var URL = '../../../_AccountInfo/ComfirmMultiEditDoubleComm.aspx?arCID=' + selCID;
    var popH = 125, popW = 500;
    ageWnd.open(URL, '', 300, 300, popW, popH);
}

RegisterStartUp("InitTagSuggestion()");