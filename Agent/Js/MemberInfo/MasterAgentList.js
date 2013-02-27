function showReset(id, usn, isSync) {
    var popH = 150, popW = 370;
    var url = '../Popup/ResetPass.aspx?custid=' + id + '&usn=' + usn + '&issync=' + isSync;
    //url, title, left, top, width, height
    ageWnd.open(url, '', 150, 100, popW, popH);
}

function showTransferCondition(id) {
    var popH = 140, popW = 550;
    //url, title, left, top, width, height
    ageWnd.open('../_MemberInfo/TransferCondition/EditTransferCondition.aspx?custid=' + id, '', 200, 100, popW, popH);
}

function HoverPen(e, isover) {
    if (isover == 1) {
        e.className = 'btnEdithover';
    }
    else {
        e.className = 'btnEdit';
    }
}

function UpdateCustomerStatus(custid, isClosed, isSuspended, isAllowAutoMPT, isOutright, isSync, isAllowNumber) {
    //url, title, left, top, width, height
    var isClearCredit = false;
    var isDownLineNumberGame = false;
    if (isClosed) {
        isClearCredit = $('cbxCredit1').checked;
    }
    if (isClosed == 1) {
        if (!confirm(confirmCloseMem)) return;
    }
    else if (isClosed == 0) {
        if (!confirm(confirmOpenMem)) return;
    }
    else if (isSuspended == 1) {
        if (!confirm(confirmSuspendDownline)) return;
    }
    else if (isSuspended == 0)
        if (!confirm(confirmSuspendMem)) return;

    if (isAllowNumber == 0) {
        if (!confirm(confirmDisableNumberGameDownline)) return;
    }
    else if (isAllowNumber == 1) {
        isDownLineNumberGame = $('cbxCredit5').checked;
    }

    ageWnd.open('../Handlers/DownlineList.ashx?custid=' + custid + '&isClosed=' + isClosed + '&isSuspended=' + isSuspended + '&isAllowAutoMPT=' + isAllowAutoMPT + '&isOutright=' + isOutright + '&isClearCredit=' + isClearCredit + '&issync=' + isSync + '&isAllowNumberGame=' + isAllowNumber + "&isDownLineNumberGame=" + isDownLineNumberGame,
                '', 0, 0, 0, 0);

    // stop all event
    document.body.onmousedown = function(evt) {
        if (window.attachEvent) {
            window.event.cancelBubble = true;
            window.event.returnValue = false;
        } else {
            evt.preventDefault();
            evt.stopPropagation();
        }
        return false;
    }
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
    return { offsetLeft: left, offsetTop: top };
}

// currentStatus: Open, Closed, Suspended
function ShowFrmUpdStatus(el, custid, isClosed, isSuspended, isAllowNumber, isAllowAutoMPT, isOutright, username) {
    var div = $('divUpd');
    if (!div) return false;

    var td1 = $('td1');
    var td2 = $('td2');
    var td4 = $('td4');

    var cbxSt1 = $('cbxSt1');
    var cbxSt2 = $('cbxSt2');
    var cbxSt4 = $('cbxSt4');
    var spCredit1 = $('spCredit1');

    td1.innerHTML = $('txtClosed').innerHTML;
    td2.innerHTML = $('txtSuspended').innerHTML;
    td4.innerHTML = $('txtOutright').innerHTML;
    // Old status
    div.custid = custid;
    div.isClosed = div.isClosed1 = isClosed;
    div.isSuspended = div.isSuspended1 = isSuspended;
    div.isOutright = div.isOutright1 = isOutright;
    div.strUsername = username;
    div.isCancelSubmit = false;

    //Get sync Casino status
    var isSync = el.parentNode.parentNode.cells[3].firstChild.getAttribute('statuscs');
    if (isSync == null)
        isSync = el.parentNode.parentNode.getAttribute('statuscs');
    div.isSync = isSync;
    //

    cbxSt1.checked = isClosed ? 'checked' : null;
    cbxSt2.disabled = null;
    cbxSt2.checked = isSuspended ? 'checked' : null;
    cbxSt2.disabled = (callerIsSuspended) ? 'disabled' : null;
    cbxSt4.disabled = null;
    cbxSt4.checked = isOutright ? 'checked' : null;
    cbxSt4.disabled = (callerIsAllowOutright) ? 'disabled' : null;
    spCredit1.style.display = 'none';

    el.style.position = 'relative';
    var pos = GetPosition(el);
    div.style.top = (pos.offsetTop - 35) + 'px';  //nqminh - 22Oct08 - added 'px' to compatible with DTD XHTML
    div.style.left = (pos.offsetLeft + 12) + 'px';

    div.style.display = 'block';

    clearTimeout(div.timerID); div.timerID = 0;
    div.timerID = setTimeout(HideFrmUpdStatus, 2000);
    AGE.addEvent(div, "mouseover", function() { clearTimeout(div.timerID); div.timerID = 0; }, true);
    AGE.addEvent(cbxSt1, "click", function() { div.isClosed = cbxSt1.checked ? true : false; }, true);
    AGE.addEvent(cbxSt2, "click", function() { div.isSuspended = cbxSt2.checked ? true : false; }, true);
    AGE.addEvent(cbxSt4, "click", function() { div.isOutright = cbxSt4.checked ? true : false; }, true);

    if (($('roleid') && $('roleid').value == 4) && ($('hdlevel') && $('hdlevel').value == 'Super')) {
        var td3 = $('td3');
        var cbxSt3 = $('cbxSt3');
        td3.innerHTML = $('txtAllowAutoPT').innerHTML;
        div.isAllowAutoMPT = div.isAllowAutoMPT1 = isAllowAutoMPT;
        cbxSt3.checked = isAllowAutoMPT ? 'checked' : null;
        AGE.addEvent(cbxSt3, "click", function() { div.isAllowAutoMPT = cbxSt3.checked ? true : false; }, true);
    }

    //NumberGame
    if ($('isDisableNumberGameStatus').value == 'false') {
        var td5 = $('td5');
        var spCredit5 = $('spCredit5');
        var cbxSt5 = $('cbxSt5');
        div.isAllowNumber = div.isAllowNumber5 = isAllowNumber;

        td5.innerHTML = $('txtAllowNumberGame').innerHTML;
        cbxSt5.checked = isAllowNumber ? 'checked' : null;
        spCredit5.style.display = 'none';
        AGE.addEvent(cbxSt5, "click", function() { div.isAllowNumber = cbxSt5.checked ? true : false; }, true);
    }

    AGE.addEvent(div, "mouseout", function() { clearTimeout(div.timerID); div.timerID = 0; div.timerID = setTimeout(HideFrmUpdStatus, 100); }, true);
}

function HideFrmUpdStatus() {
    var div = $('divUpd');
    if (!div) return false;
    div.style.display = 'none';

    // Is changing status

    var isClosed = (div.isClosed != div.isClosed1) ? div.isClosed : '-1';

    var isSuspended = (div.isSuspended != div.isSuspended1) ? div.isSuspended : '-1';
    var isAllowAutoMPT = (div.isAllowAutoMPT != div.isAllowAutoMPT1) ? div.isAllowAutoMPT : '-1';
    var isOutright = (div.isOutright != div.isOutright1) ? div.isOutright : '-1';
    var isAllowNumber = (div.isAllowNumber != div.isAllowNumber5) ? div.isAllowNumber : '-1';
    var isSync = div.isSync;
    // Call saving
    if ((isClosed == '-1') && (isSuspended == '-1') && (isAllowAutoMPT == '-1') && (isOutright == '-1') && (isAllowNumber == '-1')) return;

    if (div.isCancelSubmit) {
        return;
    }

    //    if ($('cbxSt1').checked && $('cbxCredit1').checked) {
    //        var strConfirm = lblConfirmClearCredit.replace("{username}", div.strUsername);
    //        if (!window.confirm(strConfirm)) {
    //            return;
    //        }
    //    }

    UpdateCustomerStatus(div.custid, isClosed, isSuspended, isAllowAutoMPT, isOutright, isSync, isAllowNumber);
}

function CheckStatusClick(number) {
    if (number == 1) { //closed click!
        var div = $('divUpd');
        if (!div) return false;
        //var isClosed = (div.isClosed != div.isClosed1) ? div.isClosed : '-1';
        if (!div.isClosed1) { //if original 'close' status is false
            var chkStatus = $("cbxSt" + number);
            var spCredit = $("spCredit" + number);
            var chkCredit = $("cbxCredit" + number);
            chkCredit.checked = false; //default is unchecked clear credit
            //spCredit.style.display = (chkStatus.checked) ? "" : "none";
            spCredit.style.display = "none";
        }
    }

    if (number == 5) { //Disbale NumberGame click!
        var div = $('divUpd');
        if (!div) return false;
        if (!div.isAllowNumber5) { //if original 'allow number game' status is false
            var chkStatus = $("cbxSt" + number);
            var spCredit = $("spCredit" + number);
            var chkCredit = $("cbxCredit" + number);
            chkCredit.checked = false; //default is unchecked clear credit
            spCredit.style.display = (chkStatus.checked && droleid != 1) ? "" : "none";
        }
    }
}

function OnClearCredit() {
    var div = $('divUpd');
    if (!div) return false;
    var strConfirm = lblConfirmClearCredit.replace("{username}", div.strUsername);
    if (!window.confirm(strConfirm)) {
        div.isCancelSubmit = true;
    } else div.isCancelSubmit = false;
}

function OnAllowDownline() {
    var div = $('divUpd');
    if (!div) return false;
    var strConfirm = lblConfirmAllowDownline.replace("{username}", div.strUsername);
    if (!window.confirm(strConfirm)) {
        div.isCancelSubmit = true;
    } else div.isCancelSubmit = false;
}

function GenerateTRStatus(number) {
    var txtCurChk = "cbxSt" + number;
    var txtSpCredit = "spCredit" + number;
    var txtChkCredit = "cbxCredit" + number;
    var ckbCredit = ' <span id="' + txtSpCredit + '" style="display:none; padding-left:10px;">' + '<input id="' + txtChkCredit + '" name="' + txtChkCredit + '" type="checkbox" onclick="OnClearCredit();" /> ' + lblClearAllCredit + '</span>';
    var ckbStatus = '<tr><td onclick="CheckStatusClick(\'' + number + '\');">'
        + '<input id="cbxSt' + number + '" name="cbxSt' + number + '" type="checkbox" />'
        + '</td>'
        + '<td align="left">'
        + '<label id="td' + number + '" for="cbxSt' + number + '"></label>';
    var rest =
          '</td>'
        + '</tr>';

    if (number > 1) ckbCredit = '';

    if (number == 5) {
        ckbCredit = ' <span id="' + txtSpCredit + '" style="display:none; padding-left:10px;">' + '<input id="' + txtChkCredit + '" name="' + txtChkCredit + '" type="checkbox" onclick="OnAllowDownline();" /> ' + lblClearAllDownlines + '</span>';
    }

    return ckbStatus + ckbCredit + rest;
}

function InitFrmUpdStatus() {
    var div = $('divUpd');
    var disableAutoMPT = "";
    if (div) return false;
    var strDivHeader = '<div id="divUpd" style="display:none; border: solid 1px #ccdddd; position: absolute; background-color: #eeeeee; white-space: nowrap; padding-right: 5px; font-weight:normal; color:#171717">'
               + '<table border="0" cellspacing="2" cellpadding="1">';
    var strDivFooter = '</table></div>';
    var strDivContent = '';
    strDivContent += GenerateTRStatus(1); //closed
    strDivContent += GenerateTRStatus(2); //suspend
    if (($('roleid') && $('roleid').value == 4) && ($('hdlevel') && $('hdlevel').value == 'Super')) {
        strDivContent += GenerateTRStatus(3); //autoMPT
    }
    strDivContent += GenerateTRStatus(4); //outRight

    if ($('isDisableNumberGameStatus').value == 'false') {
        strDivContent += GenerateTRStatus(5); //NumberGame
    }
    $('FrmUpd').innerHTML += strDivHeader + strDivContent + strDivFooter;
    return true;
}

function ShowErrMsg(msg) {
    clearTimeout(window.timerErr); window.timerErr = 0;

    window.diverr = $('diverr');
    if (!window.diverr) return false;

    window.err = $('err');
    if (!window.err) return false;

    window.err.innerHTML = msg;
    window.diverr.style.display = 'block';

    window.timerErr = setTimeout('location = location.href;', 5000);
}

// Init floating div
function InitFrmUpdMult() {
    var div = $('divUpdMult');
    if (div) return false;
    var strDiv = '<div id="divUpdMult" style="display:none; border: solid 1px #ccdddd; position: absolute; background-color: #eeeeee; white-space:nowrap; padding-right: 5px; font-weight:normal;">'
    strDiv += '<table border="0" cellspacing="2" cellpadding="1"><tr><td></td></tr>'
    strDiv += '<tr>'
    strDiv += '<td>'
    strDiv += '<a class="b_left" id="updMultSpBook" href="javascript:;"></a>'
    strDiv += '</td></tr>'
    if ($('isDisableFI').value != 1) {
        strDiv += '<tr>'
        strDiv += '<td>'
        strDiv += '<a class="b_left" id="updMultFI" href="javascript:;"></a>'
        strDiv += '</td>'
        strDiv += '</tr>'
    }
    if ($('isDisableHR').value != 1) {
        strDiv += '<tr>'
        strDiv += '<td>'
        strDiv += '<a class="b_left" id="updMultHR" href="javascript:;"></a>'
        strDiv += '</td>'
        strDiv += '</tr>'
    }

    if ($('isDisableCS').value != 1) {
        strDiv += '<tr id="tr_updMultCS">'
        strDiv += '<td>'
        strDiv += '<a class="b_left" id="updMultCS" href="javascript:;"></a>'
        strDiv += '</td></tr>'
    }

    //For P2P
    if ($('isDisableP2P').value != 1) {
        strDiv += '<tr id="tr_updMultP2P">'
        strDiv += '<td>'
        strDiv += '<a class="b_left" id="updMultP2P" href="javascript:;"></a>'
        strDiv += '</td></tr>'
    }

    // Hide bingo
//    $('isDisableBG').value = 1;

    if ($('isDisableBG').value != 1) {
        strDiv += '<tr id="tr_updMultBG">'
        strDiv += '<td>'
        strDiv += '<a class="b_left" id="updMultBG" href="javascript:;"></a>'
        strDiv += '</td></tr>'
    }

    strDiv += '<tr><td></td></tr></table>'
    strDiv += '</div>';

    $('FrmUpd').innerHTML += strDiv;
    return true;
}
var username = '';
function ShowFrmUpdMult(el, custLevel, upline_id, masterid) {
    var ArrChkId = document.getElementsByName("chkid");
    var ischecked = false;
    var counter = 0;
    var counterCS = 0;
    var counterBG = 0;
    var counterP2P = 0;
    var custid = 0;
    var custidCS = 0;
    var custidBG = 0;
    var custidP2P = 0;
    var status = ''; //  For casino
    var statusBingo = ''; //  For Bingo
    var statusP2P = ''; //  For P2P
    var isCasino = false;
    var isBingo = false;
    var isP2P = false;

    for (var j = 0; j < ArrChkId.length; j++) {
        if (ArrChkId[j].checked) {
            if ($('isDisableCS').value != 1) {
                var strStatus = ArrChkId[j].getAttribute('statuscs');
                status += strStatus + ';';

                if (!isCasino) {
                    if (strStatus == "0" || strStatus == "1") {
                        isCasino = true;
                    }
                    else counterCS--;
                }
            }

            if ($('isDisableBG').value != 1) {
                var strStatus = ArrChkId[j].getAttribute('statusbg');
                statusBingo += strStatus + ';';

                if (!isBingo) {
                    if (strStatus == "0" || strStatus == "1") {
                        isBingo = true;
                    }
                    else counterBG--;
                }
            }

            if ($('isDisableP2P').value != 1) {
                var strStatus = ArrChkId[j].getAttribute('statusp2p');
                statusP2P += strStatus + ';';

                if (!isP2P) {
                    if (strStatus == "0" || strStatus == "1") {
                        isP2P = true;
                    }
                    else counterP2P--;
                }
            }

            ischecked = true;
            counter++;
            counterCS++;
            counterBG++;
            counterP2P++;
            if (counter > 1 && (counterCS > 1 || counterBG > 1 || counterP2P > 1)) {
                break;
            }
            if (counter == 1) { // First other custid
                custid = ArrChkId[j].value.split(';')[0];
                username = ArrChkId[j].value.split(';')[1];
            }
            if (counterCS == 1) { // First casino custid
                custidCS = ArrChkId[j].value.split(';')[0];
            }
            if (counterBG == 1) { // First bingo custid
                custidBG = ArrChkId[j].value.split(';')[0];
            }
            if (counterP2P == 1) { // First p2p custid
                custidP2P = ArrChkId[j].value.split(';')[0];
            }
        }
    }

    if (ischecked) {
        var div = $('divUpdMult');
        if (!div) return false;

        el.style.position = 'relative';
        var pos = GetPosition(el);
        div.style.top = (pos.offsetTop - 15) + 'px';  //Ryan - 22Oct08 - added 'px' to compatible with DTD XHTML
        div.style.left = (pos.offsetLeft + 20) + 'px';

        clearTimeout(div.timerID); div.timerID = 0;
        div.timerID = setTimeout(HideFrmUpdMult, 2000);
        AGE.addEvent(div, "mouseover", function() { clearTimeout(div.timerID); div.timerID = 0; }, true);

        var td1 = $('updMultSpBook');
        td1.innerHTML = $("txtSpBookMult").innerHTML;
        if ($("roleid").value == "4") {
            if (custLevel == "Super" || custLevel == "Master")
                td1.href = 'javascript:EditMember_Multi(' + upline_id + ',"' + custLevel + '",' + masterid + ')';
            else
                td1.href = 'javascript:EditMaster_Multi(' + upline_id + ')';
        }
        else if ($("roleid").value == "3") {
            if (custLevel == "Super" || custLevel == "Master")
                td1.href = 'javascript:EditMember_Multi(' + upline_id + ',"' + custLevel + '",' + masterid + ')';
            else
                td1.href = 'javascript:EditAgent_Multi(' + upline_id + ')';
        }
        else
            td1.href = 'javascript:EditMember_Multi(' + upline_id + ',"' + custLevel + '",' + masterid + ')';

        if ($('isDisableFI').value != 1) {
            var td3 = $('updMultFI');
            td3.innerHTML = $("txtFIMult").innerHTML;
            if (counter > 1)
                td3.href = 'javascript:OpenEditFinance("","' + custLevel + '",' + upline_id + ',1)';
            else
                td3.href = 'javascript:OpenEditFinance(' + custid + ',"' + custLevel + '",' + upline_id + ',0)';
        }
        if ($('isDisableHR').value != 1) {
            var td4 = $('updMultHR');
            td4.innerHTML = $("txtHRMult").innerHTML;
            var ismult = 0;
            if (counter > 1)
                ismult = 1;

            if ($("roleid").value == "4") {
                if (custLevel == "Super") {
                    td4.href = 'javascript:CustomerEditHR("' + custid + '",4,"Member",' + ismult + ')';
                }
                else {
                    td4.href = 'javascript:CustomerEditHR("' + custid + '",4,"Master",' + ismult + ')';
                }
            }
            else if ($("roleid").value == "3") {
                if (custLevel == "Master") {
                    td4.href = 'javascript:CustomerEditHR("' + custid + '",3,"Member",' + ismult + ')';
                }
                else {
                    td4.href = 'javascript:CustomerEditHR("' + custid + '",3,"Agent",' + ismult + ')';
                }
            }
            else {
                td4.href = 'javascript:CustomerEditHR("' + custid + '",2,"Member",' + ismult + ')';
            }
        }

        // For casino
        if ($('isDisableCS').value != 1) {
            var trUpdCS = $('tr_updMultCS');
            if (!isCasino) trUpdCS.style.display = 'none';
            else trUpdCS.style.display = '';

            var updCS = $('updMultCS');
            updCS.innerHTML = $("txtCSMultEdit").innerHTML;

            if (counterCS > 1)
                updCS.href = 'javascript:EditMulti("","' + status + '",1,1)';
            else
                updCS.href = 'javascript:EditMulti(' + custidCS + ',0,"' + status + '",1)';
        }

        // For Bingo
        if ($('isDisableBG').value != 1) {
            var trUpdBG = $('tr_updMultBG');
            if (!isBingo) trUpdBG.style.display = 'none';
            else trUpdBG.style.display = '';
            var updBG = $('updMultBG');
            updBG.innerHTML = $("txtBGMultEdit").innerHTML;

            if (counterBG > 1)
                updBG.href = 'javascript:EditMulti("","' + statusBingo + '",1,2)';
            else
                updBG.href = 'javascript:EditMulti(' + custidBG + ',0,"' + statusBingo + '",2)';
        }

        // For P2P
        if ($('isDisableP2P').value != 1) {
            var trUpdP2P = $('tr_updMultP2P');
            if (!isP2P) trUpdP2P.style.display = 'none';
            else trUpdP2P.style.display = '';

            var updP2P = $('updMultP2P');
            updP2P.innerHTML = $("txtP2PMultEdit").innerHTML;

            if (counterP2P > 1)
                updP2P.href = 'javascript:EditMulti("","' + statusP2P + '",1,3)';
            else
                updP2P.href = 'javascript:EditMulti(' + custidP2P + ',0,"' + statusP2P + '",3)';
        }

        div.style.display = 'block';
        AGE.addEvent(div, "mouseout", function() { clearTimeout(div.timerID); div.timerID = 0; div.timerID = setTimeout(HideFrmUpdMult, 100); }, true);
    }
}
function HideFrmUpdMult() {
    var div = $('divUpdMult');
    if (!div) return false;
    div.style.display = 'none';
}

function InitFrmUpdOthersFunction() {
    var div = $('divUpdOthers');
    if (div) return false;
    var strDiv = '<div id="divUpdOthers" style="display:none; border: solid 1px #ccdddd; position: absolute; background-color: #eeeeee; white-space: nowrap; padding-right: 5px; font-weight:normal;">'
    strDiv += '<table cellspacing="2" cellpadding="1">'
    strDiv += '<tr>'
    strDiv += '<td>'
    strDiv += '<a class="b_left" id="updMinPT" href="javascript:;"></a>'
    strDiv += '</td></tr>'

    if ($("isTransfer") != null && $('isDisableFI').value != 1) {
        strDiv += '<tr>'
        strDiv += '<td>'
        strDiv += '<a class="b_left" id="updFIMinPT" href="javascript:;"></a>'
        strDiv += '</td></tr>'
    }
    if ($('isDisableHR').value != 1) {
        strDiv += '<tr>'
        strDiv += '<td>'
        strDiv += '<a class="b_left" id="updHRMinPT" href="javascript:;"></a>'
        strDiv += '</td></tr>'
    }
    if ($("isTransfer") != null && $("isTransfer").value == "1") {
        strDiv += '<tr><td align="left"><a class="b_left" id="updTransferCon" href="javascript:;"></a>'
        strDiv += '</td>'
        strDiv += '</tr>'
    }

    strDiv += '<tr>'
    strDiv += '<td>'
    strDiv += '<a class="b_left" id="updPwd" href="javascript:;"></a>'
    strDiv += '</td>'
    strDiv += '</tr>'
    strDiv += '</table>'
    strDiv += '</div>';

    $('FrmUpd').innerHTML += strDiv;
    return true;
}
function ShowFrmUpdOthers(el, custid, username) {
    var div = $('divUpdOthers');
    if (!div) return false;

    var td1 = $('updMinPT');
    td1.innerHTML = $("txtMinPT").innerHTML;
    var td3 = $('updPwd');
    td3.innerHTML = $("txtPassword").innerHTML;
    // Get isSync for Casino
    var isSync = el.parentNode.parentNode.cells[3].firstChild.getAttribute('statuscs');
    //--
    el.style.position = 'relative';
    var pos = GetPosition(el);
    div.style.top = (pos.offsetTop - 35) + 'px';  //Ryan - 22Oct08 - added 'px' to compatible with DTD XHTML
    div.style.left = (pos.offsetLeft + 12) + 'px';

    div.style.display = 'block';
    clearTimeout(div.timerID); div.timerID = 0;
    div.timerID = setTimeout(HideFrmUpdOthers, 2000);
    AGE.addEvent(div, "mouseover", function() { clearTimeout(div.timerID); div.timerID = 0; }, true);
    td1.href = 'javascript:OpenEditMinMax(' + custid + ',"' + username + '")';
    td3.href = 'javascript:showReset(' + custid + ',"' + username + '",' + isSync + ')';
    AGE.addEvent(div, "mouseout", function() { clearTimeout(div.timerID); div.timerID = 0; div.timerID = setTimeout(HideFrmUpdOthers, 100); }, true);

    if ($("isTransfer") != null && $("isTransfer").value == "1") {
        var td2 = $('updTransferCon');
        td2.innerHTML = $("txtTransfer").innerHTML;
        td2.href = 'javascript:showTransferCondition(' + custid + ')';
    }
    if ($("isTransfer") != null && $('isDisableFI').value != 1) {
        var td4 = $('updFIMinPT');
        td4.innerHTML = $("txtFIMinPT").innerHTML;
        td4.href = 'javascript:OpenEditMinMaxFI(' + custid + ',"' + username + '")';
    }
    if ($('isDisableHR').value != 1) {
        var td5 = $('updHRMinPT');
        td5.innerHTML = $("txtHRMinPT").innerHTML;
        td5.href = 'javascript:OpenEditMinMaxHR(' + custid + ',"' + username + '")';
    }
}
function HideFrmUpdOthers() {
    var div = $('divUpdOthers');
    if (!div) return false;
    div.style.display = 'none';
}

function viewCustOutSt(username, custid) {
    window.location.href = "../_Reports/BetList/DailyStatement.aspx?username=" + username + "&option=1&custid=" + custid + "&page=1&chk_ShowSB=on&chk_ShowCasino=on&chk_ShowRB=on&chk_showng=off&chk_ShowBI=off";
    //window.location.href = "../Report/OutstandingAgentDetail.aspx?username=" + username + "&option=1&custid=" + custid + "&page=1&ShowCasino=1&ShowSB=1&ShowRB=1&ShowFI=1";
}

function viewStatement(custid, type) {
    var roleid = 0;
    if (type == "Master") {
        roleid = 3;
    }
    else if (type == "Agent") {
        roleid = 2;
    }
    window.location.href = "../_Reports/Common/Statement.aspx?custid=" + custid + (roleid > 0 ? "&roleid=" + roleid : "");
    //    window.location.href = "../Report/Statement.aspx?custid=" + custid + "&site=" + type;
}

function viewDownlineList(act, custId, masterId) {
    //DownlineList.aspx?act=AgentList&custid={custid}&masterid={custid}
    var masterIdParam = "";
    if (typeof masterId != 'undefined' && masterId)
        masterIdParam = "&masterid=" + masterId;
    window.location.href = "DownlineList.aspx?act=" + act + "&custid=" + custId + masterIdParam;
}

function EditMulti(custid, status, isMult, page) {
    var URL = "";
    var popH = 600, popW = 980;
    var SelStatus = "";
    var bHasCheckall = true;
    var bAddToList = false;

    switch (page) {
        case 1:
            URL = "../MemberInfo/PositionTakingCS.aspx?";
            break;
        case 2:
            URL = "../MemberInfo/PositionTakingBingo.aspx?";
            break;
        case 3:
            URL = "../_MemberInfo/P2P/P2P_Page.aspx?";
            break;
    }

    var arCID = document.getElementsByName("chkid");
    var custname = arCID[0].value.substring(arCID[0].value.indexOf(";") + 1)

    if (isMult == 1) {
        if (!arCID) return;
        var SelCID = "";
        var SelUser = "";
        var firstSynCustId = 0;
        var firstNotSynCustId = 0;
        var intCount = 0;
        for (var i = 0; i < arCID.length; i++) {
            if (arCID[i].checked) {
                //Casino
                if (page == 1) {
                    if (arCID[i].getAttribute("statusCS") == "1") {//Already synchronized
                        if (firstSynCustId == 0) firstSynCustId = arCID[i].value.substring(0, arCID[i].value.indexOf(";"));
                        bAddToList = true;
                    }
                    else if (arCID[i].getAttribute("statusCS") == "0") {
                        if (firstNotSynCustId == 0) firstNotSynCustId = arCID[i].value.substring(0, arCID[i].value.indexOf(";"));
                        bAddToList = true;
                    }
                    else {
                        arCID[i].checked = null;
                        bHasCheckall = false;
                        bAddToList = false;
                    }

                    if (bAddToList) {
                        SelCID += arCID[i].value.substring(0, arCID[i].value.indexOf(";")) + "^";
                        SelUser += arCID[i].value.substring(arCID[i].value.indexOf(";") + 1) + "^";
                        SelStatus += arCID[i].getAttribute("statusCS") + "^";
                        intCount += 1;
                    }
                }
                //Bingo
                else if (page == 2) {
                    if (arCID[i].getAttribute("statusbg") == "1") {//Already synchronized
                        if (firstSynCustId == 0) firstSynCustId = arCID[i].value.substring(0, arCID[i].value.indexOf(";"));
                        bAddToList = true;
                    }
                    else if (arCID[i].getAttribute("statusbg") == "0") {
                        if (firstNotSynCustId == 0) firstNotSynCustId = arCID[i].value.substring(0, arCID[i].value.indexOf(";"));
                        bAddToList = true;
                    }
                    else {
                        arCID[i].checked = null;
                        bHasCheckall = false;
                        bAddToList = false;
                    }
                    if (bAddToList) {
                        SelCID += arCID[i].value.substring(0, arCID[i].value.indexOf(";")) + "^";
                        SelUser += arCID[i].value.substring(arCID[i].value.indexOf(";") + 1) + "^";
                        SelStatus += arCID[i].getAttribute("statusbg") + "^";
                        intCount += 1;
                    }
                }
                //P2P
                if (page == 3) {
                    if (arCID[i].getAttribute("statusP2P") == "1") {//Already synchronized
                        if (firstSynCustId == 0) firstSynCustId = arCID[i].value.substring(0, arCID[i].value.indexOf(";"));
                        bAddToList = true;
                    }
                    else if (arCID[i].getAttribute("statusP2P") == "0") {
                        if (firstNotSynCustId == 0) firstNotSynCustId = arCID[i].value.substring(0, arCID[i].value.indexOf(";"));
                        bAddToList = true;
                    }
                    else {
                        arCID[i].checked = null;
                        bHasCheckall = false;
                        bAddToList = false;
                    }

                    if (bAddToList) {
                        SelCID += arCID[i].value.substring(0, arCID[i].value.indexOf(";")) + "^";
                        SelUser += arCID[i].value.substring(arCID[i].value.indexOf(";") + 1) + "^";
                        SelStatus += arCID[i].getAttribute("statusP2P") + "^";
                        intCount += 1;
                    }
                }
            }
        }

        if (!bHasCheckall) checkall.checked = null;

        if (firstSynCustId == 0) {
            firstSynCustId = firstNotSynCustId;
        }

        if (intCount == 0) return;
        if (intCount == 1) {
            $("arrayCustID").value = "";
            $("arrayUserName").value = "";
            if (page == 3)
                URL += "custids=" + SelCID + "&custnames=" + SelUser + "&isMult=0&roleid=" + droleid;
            else
                URL += "custid=" + firstSynCustId + "&isMult=0";
        }
        else if (intCount > 1) {
            $("arrayCustID").value = SelCID;
            $("arrayUserName").value = SelUser;
            $("arrayStatus").value = SelStatus;
            if (page == 3)
                URL += "custids=" + SelCID + "&custnames=" + SelUser + "&isMult=1&roleid=" + droleid;
            else
                URL += "custid=" + firstSynCustId + "&isMult=1";
        }
    }
    else {
        $("arrayCustID").value = "";
        $("arrayUserName").value = "";
        $("arrayStatus").value = "";
        if (page == 3)
            URL += "custids=" + custid + "&username=" + username + "&isMult=0&roleid=" + droleid;
        else
            URL += "custid=" + custid + "&isMult=0&status=" + SelStatus.substring(0, SelStatus.indexOf("^"));
    }
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

function OpenEditFinance(custid, custLevel, upline_id, isMult) {
    var URL = "../MemberInfo/AddEditFinancial.aspx?";
    var popH = 550, popW = 950;
    if (isMult == 1) {
        var arCID = document.getElementsByName("chkid");
        if (!arCID) return;
        var SelCID = "";
        var SelUser = "";
        var intCount = 0;
        var mycustid = "";
        for (var i = 0; i < arCID.length; i++) {
            if (arCID[i].checked) {
                SelCID += arCID[i].value.substring(0, arCID[i].value.indexOf(";")) + "^";
                SelUser += arCID[i].value.substring(arCID[i].value.indexOf(";") + 1) + "^";
                intCount += 1;
            }
        }
        URL += "custid=" + SelCID.substring(0, SelCID.indexOf("^")) + "&isMult=1";

        if (intCount == 0) return;
        if (intCount == 1) {
            $("arrayCustID").value = "";
            $("arrayUserName").value = "";
        }
        else if (intCount > 1) {
            $("arrayCustID").value = SelCID;
            $("arrayUserName").value = SelUser;
        }
        if (custLevel == "Super" || custLevel == "Master") {
            popH = 550;
            popW = 800;
            URL = "../MemberInfo/EditMemberFI.aspx?custId=" + SelCID.substring(0, SelCID.indexOf("^")) + "&agentid=" + upline_id + "&isMult=1";
        }
    }
    else {
        $("arrayCustID").value = "";
        $("arrayUserName").value = "";
        URL += "custId=" + custid + "&isMult=0";

        if (custLevel == "Super" || custLevel == "Master") {
            popH = 550;
            popW = 800;
            URL = "../MemberInfo/EditMemberFI.aspx?custId=" + custid + "&agentid=" + upline_id + "&isMult=0";
        }
    }
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

function EditP2P(custid, roleid, username) {
    var URL = '../_MemberInfo/P2P/P2P_Page.aspx?custids=' + custid + '&roleid=' + roleid + '&username=' + username + '&disable=' + $('isDisableP2P').value;
    var popH = 550, popW = 950;
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

function GetCustID(CheckedID) {
    var arCID = document.getElementsByName(CheckedID);
    if (!arCID) return;
    var SelCID = "";
    var SelUser = "";
    var intCount = 0;
    var mycustid = "";
    for (var i = 0; i < arCID.length; i++) {
        if (arCID[i].checked) {
            SelCID += arCID[i].value.substring(0, arCID[i].value.indexOf(";")) + "^";
            SelUser += arCID[i].value.substring(arCID[i].value.indexOf(";") + 1) + "^";
            intCount += 1;
        }
    }

    if (intCount == 0) return;
    if (intCount == 1) {
        $("arrayCustID").value = "";
        $("arrayUserName").value = "";
    }
    else if (intCount > 1) {
        $("arrayCustID").value = SelCID;
        $("arrayUserName").value = SelUser;
    }
    return SelCID.substring(0, SelCID.indexOf("^"));
}

function CustomerEditHR(custid, roleid, actionOn, isMult) {
    var url = "";
    var popH = 900, popW = 1000;
    switch (roleid) {
        case 4: // Super
            url += "../_MemberInfo/RaceBook/Super/";
            if (isMult == 0) {
                if (actionOn == 'ViewAgent') {
                    url += "ViewAgent.aspx?custid=" + custid;
                }
                else if (actionOn == "Member") {
                    url += "EditMember.aspx?custid=" + custid;
                }
                else {
                    url += "EditMaster.aspx?custid=" + custid;
                }
                $("arrayCustID").value = custid;
                $("arrayUserName").value = "";
            }
            else {
                custid = GetCustID("chkid");
                if (actionOn == "Member") {
                    url += "EditMultMember.aspx?custid=" + custid;
                }
                else {
                    url += "EditMultMaster.aspx?custid=" + custid;
                }
            }
            break;
        case 3: // Master
            url += "../_MemberInfo/RaceBook/Master/";
            if (isMult == 0) {
                if (actionOn == "Member") {
                    url += "EditMember.aspx?custid=" + custid;
                }
                else {
                    url += "EditAgent.aspx?custid=" + custid;
                }
                $("arrayCustID").value = custid;
                $("arrayUserName").value = "";
            }
            else {
                custid = GetCustID("chkid");
                if (actionOn == "Member") {
                    url += "EditMultMember.aspx?custid=" + custid;
                }
                else {
                    url += "EditMultAgent.aspx?custid=" + custid;
                }
            }
            break;
        case 2: // Agent
            url += "../_MemberInfo/RaceBook/Agent/";
            if (isMult == 0) {
                url += "EditMember.aspx?custid=" + custid;
                $("arrayCustID").value = custid;
                $("arrayUserName").value = "";
            }
            else {
                custid = GetCustID("chkid");
                url += "EditMultMember.aspx?custid=" + custid;
            }
            break;
    }
    ageWnd.open(url, '', 0, 10, popW, popH);
}

//Open Horseracing edit setting window
function EditHR(custid, custLevel, upline_id, isMult) {
    var URL = "../MemberInfo/AddEditHorseRacing.aspx?";
    var popH = 900, popW = 950;
    if (isMult == 1) {
        var arCID = document.getElementsByName("chkid");
        if (!arCID) return;
        var SelCID = "";
        var SelUser = "";
        var intCount = 0;
        var mycustid = "";
        for (var i = 0; i < arCID.length; i++) {
            if (arCID[i].checked) {
                SelCID += arCID[i].value.substring(0, arCID[i].value.indexOf(";")) + "^";
                SelUser += arCID[i].value.substring(arCID[i].value.indexOf(";") + 1) + "^";
                intCount += 1;
            }
        }
        URL += "custid=" + SelCID.substring(0, SelCID.indexOf("^")) + "&isMult=1";

        if (intCount == 0) return;
        if (intCount == 1) {
            $("arrayCustID").value = "";
            $("arrayUserName").value = "";
        }
        else if (intCount > 1) {
            $("arrayCustID").value = SelCID;
            $("arrayUserName").value = SelUser;
        }
        if (custLevel == "Super" || custLevel == "Master") {
            popH = 750;
            popW = 800;
            URL = "../MemberInfo/EditMemberHR.aspx?custId=" + SelCID.substring(0, SelCID.indexOf("^")) + "&agentid=" + upline_id + "&isMult=1";
        }
    }
    else {
        $("arrayCustID").value = "";
        $("arrayUserName").value = "";
        URL += "custId=" + custid + "&isMult=0";

        if (custLevel == "Super") {
            popH = 700;
            popW = 800;
            URL = "../_MemberInfo/RaceBook/Super/EditMember.aspx?custId=" + custid;
        }
        else if (custLevel == "Master") {
            popH = 700;
            popW = 800;
            URL = "../_MemberInfo/RaceBook/Master/EditMember.aspx?custId=" + custid;
        }
        if (custLevel == "ViewAgent") {
            popH = 700;
            popW = 800;
            URL = "../_MemberInfo/RaceBook/Super/ViewAgent.aspx?custId=" + custid;
        }
    }
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

function EditMaster_Single(cid, cname, superid) {
    $("arrayCustID").value = "";
    $("arrayUserName").value = "";
    var URL = "../../_MemberInfo/PositionTaking/Super/EditMaster.aspx?"; //V1

    URL += "m=0";
    var popH = 950, popW = 1024;
    URL += "&custid=" + cid;
    URL += "&USER=" + cname;
    URL += '&superid=' + superid;
    var w = screen.availWidth;
    var h = screen.availHeight;
    var leftPos = (w - popW) / 2, topPos = (h - popH) / 2;

    //url, title, left, top, width, height
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

function EditMaster_Multi(superid) {
    var arCID = document.getElementsByName("chkid");
    if (!arCID) return;
    var SelCID = "";
    var SelUser = "";
    var intCount = 0;
    for (var i = 0; i < arCID.length; i++) {
        if (arCID[i].checked) {
            SelCID += arCID[i].value.substring(0, arCID[i].value.indexOf(";")) + "^";
            SelUser += arCID[i].value.substring(arCID[i].value.indexOf(";") + 1) + "^";
            intCount += 1;
        }
    }
    
    if (intCount == 0) return;
    if (intCount == 1) {
        var URL = "../../_MemberInfo/PositionTaking/Super/EditMaster.aspx?";
        URL += "m=0";
        var popH = 950, popW = 1024;
        URL += "&custid=" + SelCID.replace("^", "");
        URL += "&USER=" + SelUser.replace("^", "");
        $("arrayCustID").value = "";
        $("arrayUserName").value = "";
    } else if (intCount > 1) {
        var URL = "../UserSuper/EditMaster.aspx?";
        URL += "m=1";
        var popH = 950, popW = 1024;
        $("arrayCustID").value = SelCID;
        $("arrayUserName").value = SelUser;
    }

    URL += '&superid=' + superid;
    //url, title, left, top, width, height
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

//this function is used when master edit agent that from agent list -- edit single
function EditAgent_Single(id, us, masterid) {
    $("arrayCustID").value = "";
    $("arrayUserName").value = "";
    var url = "../../_MemberInfo/PositionTaking/Master/EditAgent.aspx.aspx?m=0&custid=" + id + "&USER=" + us + '&masterid=' + masterid; //V1
    var popH = 950, popW = 1024;

    //url, title, left, top, width, height
    ageWnd.open(url, '', 0, 10, popW, popH);
}

//this function is used when master edit agent that from agent list -- edit multi
function EditAgent_Multi(masterid) {
    var arCID = document.getElementsByName("chkid");
    if (!arCID) return;
    var SelCID = "";
    var SelUser = "";
    var intCount = 0;
    for (var i = 0; i < arCID.length; i++) {
        if (arCID[i].checked) {
            SelCID += arCID[i].value.substring(0, arCID[i].value.indexOf(";")) + "^";
            SelUser += arCID[i].value.substring(arCID[i].value.indexOf(";") + 1) + "^";
            intCount += 1;
        }
    }

    if (intCount == 0) return;
    if (intCount == 1) {
        var URL = "../../_MemberInfo/PositionTaking/Master/EditAgent.aspx.aspx?";
        URL += "m=0";
        var popH = 950, popW = 1024;
        URL += "&custid=" + SelCID.replace("^", "");
        URL += "&USER=" + SelUser.replace("^", "");
        $("arrayCustID").value = "";
        $("arrayUserName").value = "";
    } else if (intCount > 1) {
        var URL = "../UserMaster/EditAgent.aspx?";
        URL += "m=1";
        var popH = 950, popW = 1024;
        $("arrayCustID").value = SelCID;
        $("arrayUserName").value = SelUser;
    }

    URL += '&masterid=' + masterid;

    //url, title, left, top, width, height
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

function EditMember_Multi(agentid, Customer, masterid) {
    var arCID = document.getElementsByName("chkid");
    if (!arCID) return;
    var SelCID = "";
    var SelUser = "";
    var intCount = 0;
    for (var i = 0; i < arCID.length; i++) {
        if (arCID[i].checked) {
            SelCID += arCID[i].value.substring(0, arCID[i].value.indexOf(";")) + "^";
            SelUser += arCID[i].value.substring(arCID[i].value.indexOf(";") + 1) + "^";
            intCount += 1;
        }
    }

    if (intCount == 0) return;
    if (intCount == 1) {
        var URL = "../../_MemberInfo/PositionTaking/Agent/EditMember.aspx?";
        if (Customer == 'Super') URL = "../UserSuper/EditMember.aspx?";
        if (Customer == 'Master') URL = "../UserMaster/EditMember.aspx?";
        URL += "m=0";
        URL += "&custid=" + SelCID.replace("^", "");
        URL += "&USER=" + SelUser.replace("^", "");
        var popH = 950, popW = 1024;
        $("arrayCustID").value = "";
        $("arrayUserName").value = "";
    } else if (intCount > 1) {
        var URL = "../UserAgent/EditMember.aspx?";
        if (Customer == 'Super') URL = "../UserSuper/EditMember.aspx?";
        if (Customer == 'Master') URL = "../UserMaster/EditMember.aspx?";
        URL += "m=1";
        $("arrayCustID").value = SelCID;
        $("arrayUserName").value = SelUser;
        var popH = 950, popW = 1024;
    }

    URL += '&agentid=' + agentid + '&masterid=' + masterid;

    //url, title, left, top, width, height
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

function OpenEditMinMax(custid, username) {
    var url = "../UserSuper/EditMinPT.aspx?custid=" + custid + "&username=" + username;
    var popW = 1024, popH = 950;

    //url, title, left, top, width, height
    ageWnd.open(url, '', 0, 10, popW, popH);
}
function OpenEditMinMaxFI(custid, username) {
    var url = "../MemberInfo/EditMinFI.aspx?custid=" + custid + "&username=" + username;
    var popW = 900, popH = 400;

    //url, title, left, top, width, height
    ageWnd.open(url, '', 0, 10, popW, popH);
}
function OpenEditMinMaxHR(custid, username) {
    // var url = "../MemberInfo/EditMinHR.aspx?custid=" + custid + "&username=" + username;
    var url = '';
    if ($('roleid').value == 4) {
        url = '../_MemberInfo/RaceBook/Super/EditMinPT.aspx';
    }
    else {
        url = '../_MemberInfo/RaceBook/Master/EditMinPT.aspx';
    }

    url += '?custid=' + custid;
    $("arrayCustID").value = custid;
    var popW = 800, popH = 900;

    //url, title, left, top, width, height
    ageWnd.open(url, '', 0, 10, popW, popH);
}

function ChangeIcon(id, isDis) {
    var custid = id.split('^');

    for (var i = 0; i < custid.length - 1; i++) {
        if (isDis == "0") {
            $(custid[i]).className = "divEdit";
        }
        else {
            $(custid[i]).className = "divEditDisabled";
        }
    }
    HideLoadingDiv();
}
function DelayReloadIcon(Id, isDis, time) {
    if (!window.divMaskLoading) return;
    if (typeof ageWnd != 'undefined') if (ageWnd.loaded) ageWnd.close(true);

    window.divMaskLoading.style.width = (window.viewport.width - (IE ? 22 : 2)) + 'px';
    window.divMaskLoading.style.height = (window.viewport.height) + 'px';

    window.divMaskLoading.firstChild.style.left = Math.floor(window.viewport.width / 2) + 'px';
    window.divMaskLoading.firstChild.style.top = document.documentElement.scrollTop + 'px';
    window.divMaskLoading.style.display = 'block';
    var delay = time ? time : 2000;
    window.loadingT = setTimeout("ChangeIcon('" + Id + "','" + isDis + "')", delay);
}
function SetChangeICon(id, isDis) {
    $('HdCustid').value = id;
    $('HdStatus').value = isDis;
}

// User in multiple update case
// Return 2 arrays: data.custids, data.usernames
function GetSelectedCustomer() {
    var custids = new Array();
    var usernames = new Array();

    var arCID = document.getElementsByName("chkid");
    var c = arCID.length;

    for (var i = 0; i < c; i++) {
        if (!arCID[i].checked) continue;

        var custid = parseInt(arCID[i].value.split(';')[0]);
        custids.push(custid);
        var username = arCID[i].value.split(';')[1];
        usernames.push(username);
    }

    if (custids.length == 0) return null;

    return { custids: custids, usernames: usernames };
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