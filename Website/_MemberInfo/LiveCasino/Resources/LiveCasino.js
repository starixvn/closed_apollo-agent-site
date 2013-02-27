//========================================================================================================================
/*
Created by: Andy
Description: General updating information
*/
var finish1Task = 0;
var userNames = '[]';
var timerID = null;
var taskCount = 2;
var submit = false;
var flag = false;
var success = false;

function checkMaxPT() {
    if ($('pt_3') && $('pt_4')) {
        PT_RenderCombo2($("pt_4"), _page.MinPT_Min, $('pt_3').value, 0.01);
        if (parseFloat($('pt_3').value) < parseFloat($('pt_4').value)) {
            $('pt_4').value = $('pt_3').value;
        }
    }

    return true;
}

function checkMinMaxPT() {
    if ($('pt_3') && $('pt_4')) {
        $('pt_3').onchange = checkMaxPT;
    }
}

function initData() {
    if (_page.disabled) {
        alert(_page.noAccess);
        return false;
    }

    age.ShowMaskDiv(true);

    top.errCodes = new Array();
    top.errMsgs = new Array();
    top.userNames = new Array();

    var userStr = $('custnames').innerHTML == '' ? _page.userName : $('custnames').innerHTML;

    if (userStr.indexOf(',') > 0)
        userNames = userStr.split(',');
    else
        userNames = new Array(userStr);
}

function checkPT(el) {
    if (el.checked) {
        if ($('pt_1'))
            $('pt_1').disabled = true;
        if ($('pt_2'))
            $('pt_2').disabled = false;

        var pt3 = $("pt_3");
        var pt4 = $("pt_4");

        if (pt3)
            PT_RenderCombo(pt3, _page.MaxPT_Min, _page.MaxPT_Max, 0.01, pt3.options[pt3.selectedIndex].value);
        if (pt4)
            PT_RenderCombo(pt4, _page.MinPT_Min, pt3.options[pt3.selectedIndex].value, 0.01, pt4.options[pt4.selectedIndex].value);
    }
    else {
        if ($('pt_1')) {
            $('pt_1').disabled = false;
            PTChanging($('pt_1'));
        }
        if ($('pt_2'))
            $('pt_2').disabled = true;
    }
}

function btnSubmitClick() {
    submit = true;
    finish1Task = 0;
    initData();                                             //Inits data and mask div

    timerID = setTimeout('showErrMsg()', 100);

    if ((_page.roleId == 4 && _page.custRoleId == 3 ||
        _page.roleId == 3 && _page.custRoleId == 2) &&
        flag &&
        $('chkAutoPT') &&
        !$('chkAutoPT').checked ||
        ((_page.roleId == 4 && _page.custRoleId == 3 ||
        _page.roleId == 3 && _page.custRoleId == 2) &&
        _page.isMult) &&
        $('chkAutoPT') &&
        !$('chkAutoPT').checked) {
        showConfirm();
        return false;
    }
    else
        submitBtnSubmitClick();
}

function submitBtnSubmitClick() {
    //    if (!_page.sync) {
    //        updatePT();
    //    }
    //    else {
    if (_page.roleId == 4 && _page.custRoleId == 3)         //Super edits master
        editMasterPTComm2();
    else if (_page.roleId == 4 && _page.custRoleId == 1)    //Super edits member
        editMemberBySuper();
    else if (_page.roleId == 3 && _page.custRoleId == 2)    //Master edits agent
        editAgentPTComm2();
    else if (_page.roleId == 3 && _page.custRoleId == 1)    //Master edits member
        editMemberByMaster();
    else if (_page.roleId == 2 && _page.custRoleId == 1)    //Agents edits member
        editMemberPTComm2();
    else                                                    //Invalid request
        alert(_page.noAccess);

    saveMemberWinLimit();

    //    }
}

function savePTComm() {
    //    if (!_page.sync) {
    //        btnSubmitClick();
    //    }
    //    else {
    initData();                                         //Inits data and mask div

    if ((_page.roleId == 4 && _page.custRoleId == 3 ||
        _page.roleId == 3 && _page.custRoleId == 2) &&
        flag &&
        $('chkAutoPT') &&
        !$('chkAutoPT').checked ||
        ((_page.roleId == 4 && _page.custRoleId == 3 ||
        _page.roleId == 3 && _page.custRoleId == 2) &&
        _page.isMult) &&
        $('chkAutoPT') &&
        !$('chkAutoPT').checked) {
        showConfirm();
        return false;
    }
    else
        submitPTComm();
    //    }
}

function submitPTComm() {
    if (_page.roleId == 4 && _page.custRoleId == 3)         //Super edits master
        editMasterPTComm();
    else if (_page.roleId == 4 && _page.custRoleId == 1)    //Super edits member
        editMemberPTCommBySuper();
    else if (_page.roleId == 3 && _page.custRoleId == 2)    //Master edits agent
        editAgentPTComm();
    else if (_page.roleId == 3 && _page.custRoleId == 1)    //Master edits member
        editMemberPTCommByMaster();
    else if (_page.roleId == 2 && _page.custRoleId == 1)    //Agents edits member
        editMemberPTComm();
    else                                                    //Invalid request
        alert(_page.noAccess);
}

function saveGameSetting() {
    //    if (!_page.sync) {
    //        btnSubmitClick();
    //    }
    //    else {
    initData();                                         //Inits data and mask div

    editGameSetting();
    //    }
}

function GenerateMessage(ArrCodes, ArrMsgs, ArrUserName, actualUsers, isCombined, hasMemberWinLimit) {
    var isSucc = true;
    isSucc = isSucc & ageMsg.CheckMessageIsSucc(ArrCodes);

    var myTable = '<table id="tblMessage" cellpadding="0" cellspacing="0" border="0" style="width:100%;">';
    var myTDTemp = '';
    var myEndTable = ' </table>';
    var tmp = false;

    if (isCombined == false) {
        for (var j = 0; j < ArrUserName.length; j++) {
            if (ArrUserName[j] == undefined)
                continue;

            myTDTemp += '<tr><td class="EnrichUserName">';
            myTDTemp += ArrUserName[j];
            myTDTemp += "</td></tr>";

            if (ArrMsgs[j] != true && ArrMsgs[j] != undefined) {
                tmp = true;
                myTDTemp += '<tr><td>';
                myTDTemp += ArrMsgs[j];
                myTDTemp += "</td></tr>";
            }
        }
    }
    else {
        for (var j = 0; j < actualUsers.length; j++) {
            if (actualUsers[j] == undefined)
                continue;

            myTDTemp += '<tr><td class="EnrichUserName">';
            myTDTemp += actualUsers[j];
            myTDTemp += "</td></tr>";

            if (ArrMsgs[j] != true && ArrMsgs[j] != undefined) {
                tmp = true;
                //First message
                myTDTemp += '<tr><td>';
                myTDTemp += ArrMsgs[j];
                myTDTemp += "</td></tr>";
            }
            if (ArrMsgs[1 * actualUsers.length + j] != true && ArrMsgs[1 * actualUsers.length + j] != undefined) {
                tmp = true;
                //Second message
                myTDTemp += '<tr><td>';
                myTDTemp += ArrMsgs[1 * actualUsers.length + j];
                myTDTemp += "</td></tr>";
            }
            //Third message
            if (hasMemberWinLimit && ArrMsgs[2 * actualUsers.length + j] != true && ArrMsgs[2 * actualUsers.length + j] != undefined) {
                tmp = true;
                myTDTemp += '<tr><td>';
                myTDTemp += ArrMsgs[2 * actualUsers.length + j];
                myTDTemp += "</td></tr>";
            }
        }
    }

    myTable += myTDTemp + myEndTable;

    if (tmp) {
        ageMsg.ShowMsgs(myTable, isSucc);
    }
}

function showErrMsg() {
    if (submit == true && finish1Task >= taskCount) {
        if ($('pt_3') && $('pt_4')) {
            if (success)
                flag = $('chkAutoPT').checked;
        }

        submit = false;
        finish1Task = 0;

        clearTimeout(timerID);
        timerID = null;

        GenerateMessage(top.errCodes, top.errMsgs, top.userNames, userNames, true, _page.showWinLimit);
        age.HideMaskDiv();

        top.errCodes = new Array();
        top.errMsgs = new Array();
        top.userNames = new Array();

        if (userNames.length > 4 && $('spmsgerr').innerHTML != '') {
            $('errMsg').style.height = '220px';
            $('errMsg').style.overflow = 'auto';
        }

        if ($('spmsgerr').innerHTML == '')
            $('errMsg').style.display = 'none';
        else
            $('errMsg').style.display = 'block';

        $('warningMsg').style.display = 'none';

        return false;
    }
    else if (submit == false) {
        if ($('pt_3') && $('pt_4')) {
            if (success)
                flag = $('chkAutoPT').checked;
        }

        GenerateMessage(top.errCodes, top.errMsgs, top.userNames, userNames, false);
        age.HideMaskDiv();

        top.errCodes = new Array();
        top.errMsgs = new Array();
        top.userNames = new Array();

        if (userNames.length > 4 && $('spmsgerr').innerHTML != '') {
            $('errMsg').style.height = '220px';
            $('errMsg').style.overflow = 'auto';
        }

        if ($('spmsgerr').innerHTML == '')
            $('errMsg').style.display = 'none';
        else
            $('errMsg').style.display = 'block';

        $('warningMsg').style.display = 'none';

        return false;
    }
    else if (submit == true && finish1Task < taskCount) {
        //Reset timer
        clearTimeout(timerID);
        timerID = null;

        timerID = setTimeout('showErrMsg()', 100);
    }

    return true;
}

function initQueryString() {
    var URL = this.parent.parent.frames["main"].fullURL;
    if (URL) {
        var custids = URL.split('&')[0].split('=')[1];
        var custnames = URL.split('&')[1].split('=')[1];

        while (custnames.indexOf('^') > -1) {
            custnames = custnames.replace('^', ', ');
        }

        if (custnames.indexOf(',') > -1) {
            $('custids').innerHTML = custids.substring(0, custids.length - 1);
            $('custnames').innerHTML = custnames.substring(0, custnames.length - 2);
        }
    }
}

function editGameSetting() {
    if (_page.gameRows == 0) {
        if (!_page.isMult)
            top.userNames.push(_page.userName);
        else if (submit == false)
            top.userNames = userNames;

        for (var i = 0; i < userNames.length; i++) {
            top.errCodes.push(-1);
            top.errMsgs.push(true);
        }

        if (submit == true) {
            finish1Task++;
        }
        else
            showErrMsg();

        return false;
    }

    var url = '../Handlers/GameSetting.ashx';
    var pars = getGameSetting();

    url = SetParameterValue('custids', _page.isMult ? $('custids').innerHTML : _page.custId, url);
    url = SetParameterValue('roleid', _page.custRoleId, url);
    url = SetParameterValue('submit', submit, url);

    if (_page.roleId == 2 && _page.custRoleId == 1) {
        url = SetParameterValue('enable', $('bettype_chkEnableLiveCS').checked, url);
        url = SetParameterValue('disable', $('bettype_chkEnableLiveCS').disabled, url);
    }

    if (_page.roleId == 4 && _page.custRoleId == 3) {
        var upPreset = $('pt_1');
        var upAutoPreset = $('pt_2');
        var max = $('pt_3');
        var min = $('pt_4');
        var comm = $('comm_5');
        var isAuto = $('chkAutoPT') && $('chkAutoPT').checked;

        url = SetParameterValue('presetPT', isAuto ? upPreset.options[upPreset.options.length - 1].value : upPreset.options[upPreset.selectedIndex].value, url);
        url = SetParameterValue('autoPresetPT', isAuto ? upAutoPreset.options[upAutoPreset.selectedIndex].value : 0.0, url);
        url = SetParameterValue('minPT', min.options[min.selectedIndex].value, url);
        url = SetParameterValue('maxPT', max.options[max.selectedIndex].value, url);
        url = SetParameterValue('comm', comm.options[comm.selectedIndex].value, url);
        url = SetParameterValue('isAutoPresetPT', isAuto, url);
        url = SetParameterValue('isUpdateAllMember', $('isUpdateAllMember').innerHTML, url);
        url = SetParameterValue('newMemberPT', $('newMemberPT').innerHTML, url);
    }
    else if (_page.roleId == 4 && _page.custRoleId == 1) {
        var upPreset = $('pt_1');
        var upAutoPreset = $('pt_2');
        var isAuto = $('chkAutoPT') && $('chkAutoPT').checked;

        if (upPreset != null)
            url = SetParameterValue('presetPT', isAuto ? upPreset.options[upPreset.options.length - 1].value : upPreset.options[upPreset.selectedIndex].value, url);
        else
            url = SetParameterValue('presetPT', $('presetPT').innerHTML, url);

        if (upAutoPreset)
            url = SetParameterValue('autoPresetPT', isAuto ? upAutoPreset.options[upAutoPreset.selectedIndex].value : 0.0, url);
        if ($('chkAutoPT'))
            url = SetParameterValue('isAutoPresetPT', isAuto, url);
    }
    else if (_page.roleId == 3 && _page.custRoleId == 2) {
        var upPreset = $('pt_1');
        var upAutoPreset = $('pt_2');
        var max = $('pt_3');
        var min = $('pt_4');
        var comm = $('comm_5');
        var isAuto = $('chkAutoPT') && $('chkAutoPT').checked;

        url = SetParameterValue('presetPT', isAuto ? upPreset.options[upPreset.options.length - 1].value : upPreset.options[upPreset.selectedIndex].value, url);
        url = SetParameterValue('autoPresetPT', isAuto ? upAutoPreset.options[upAutoPreset.selectedIndex].value : 0.0, url);
        url = SetParameterValue('minPT', min.options[min.selectedIndex].value, url);
        url = SetParameterValue('maxPT', max.options[max.selectedIndex].value, url);
        url = SetParameterValue('comm', comm.options[comm.selectedIndex].value, url);
        url = SetParameterValue('isAutoPresetPT', isAuto, url);
        url = SetParameterValue('isUpdateAllMember', $('isUpdateAllMember').innerHTML, url);
        url = SetParameterValue('newMemberPT', $('newMemberPT').innerHTML, url);
    }
    else if (_page.roleId == 3 && _page.custRoleId == 1) {
        var upPreset = $('pt_1');
        var upAutoPreset = $('pt_2');
        var isAuto = $('chkAutoPT') && $('chkAutoPT').checked;

        if (upPreset != null)
            url = SetParameterValue('presetPT', isAuto ? upPreset.options[upPreset.options.length - 1].value : upPreset.options[upPreset.selectedIndex].value, url);
        else
            url = SetParameterValue('presetPT', $('presetPT').innerHTML, url);

        if (upAutoPreset)
            url = SetParameterValue('autoPresetPT', isAuto ? upAutoPreset.options[upAutoPreset.selectedIndex].value : 0.0, url);
        if ($('chkAutoPT'))
            url = SetParameterValue('isAutoPresetPT', isAuto, url);
    }
    else if (_page.roleId == 2 && _page.custRoleId == 1) {
        var upPreset = $('pt_1');
        var comm = $('comm_2');

        url = SetParameterValue('presetPT', upPreset.options[upPreset.selectedIndex].value, url);
        url = SetParameterValue('comm', comm.options[comm.selectedIndex].value, url);
    }
    else {
        alert(_page.noAccess);

        return false;
    }

    url = SetParameterValue('pars', pars, url);

    jx.request(
    url,
    function (res) {
        try {
            var ret = eval('(' + res.responseText + ')');
            ret = ret.errMsg;
            var users = userNames;
            var codes = ret.split('|')[0].split('$');
            var msgs = ret.split('|')[1].split('$');

            for (var i = 0; i < codes.length; i++) {
                top.userNames.push(users[i]);
                top.errCodes.push(codes[i] == 0 ? -1 : 1);
                top.errMsgs.push(msgs[i]);
            }
        } catch (ex) { }

        if (submit == true) {
            finish1Task++;
        }
        else {
            showErrMsg();
        }
    },
    'post',
    '',
    true
    );
}

function showConfirm() {
    ageWnd.displayBtnClose = false;

    if (_page.roleId == 4 && _page.custRoleId == 3) {
        ageWnd.open('ConfirmMasterAutoPT.aspx', '', 200, 200, 350, 150);
    }
    else if (_page.roleId == 3 && _page.custRoleId == 2) {
        ageWnd.open('ConfirmAgentAutoPT.aspx', '', 200, 200, 350, 150);
    }
}

RegisterStartUp('initQueryString();checkMinMaxPT();');
//========================================================================================================================
//========================================================================================================================
/*
Super edits master
*/
function editMasterPTComm() {
    var upPreset = $('pt_1');
    var upAutoPreset = $('pt_2');
    var max = $('pt_3');
    var min = $('pt_4');
    var comm = $('comm_5');
    var isAuto = $('chkAutoPT') && $('chkAutoPT').checked;

    var url = '../Handlers/Super/MasterPTComm.ashx';

    url = SetParameterValue('custids', _page.isMult ? $('custids').innerHTML : _page.custId, url);
    url = SetParameterValue('roleid', _page.custRoleId, url);
    url = SetParameterValue('submit', submit, url);

    url = SetParameterValue('presetPT', isAuto ? upPreset.options[upPreset.options.length - 1].value : upPreset.options[upPreset.selectedIndex].value, url);
    url = SetParameterValue('autoPresetPT', isAuto ? upAutoPreset.options[upAutoPreset.selectedIndex].value : 0.0, url);
    url = SetParameterValue('minPT', min.options[min.selectedIndex].value, url);
    url = SetParameterValue('maxPT', max.options[max.selectedIndex].value, url);
    url = SetParameterValue('comm', comm.options[comm.selectedIndex].value, url);
    url = SetParameterValue('isAutoPresetPT', isAuto, url);
    url = SetParameterValue('isUpdateAllMember', $('isUpdateAllMember').innerHTML, url);
    url = SetParameterValue('newMemberPT', $('newMemberPT').innerHTML, url);

    var pars = getGameSetting();

    url = SetParameterValue('pars', pars, url);

    jx.request(
    url,
    function (res) {
        try {
            var ret = eval('(' + res.responseText + ')');
            ret = ret.errMsg;
            var users = userNames;
            var codes = ret.split('|')[0].split('$');
            var msgs = ret.split('|')[1].split('$');

            if (codes[0] == '0') success = true;

            for (var i = 0; i < codes.length; i++) {
                top.userNames.push(users[i]);
                top.errCodes.push(codes[i] == 0 ? -1 : 1);
                top.errMsgs.push(msgs[i]);
            }
        } catch (ex) { }

        if (submit == true) {
            finish1Task++;
        }
        else {
            showErrMsg();
        }
    },
    'post',
    '',
    true
    );
}

function editMaster() {
    editMasterPTComm();
    editGameSetting();
}
//========================================================================================================================
//========================================================================================================================
/*
Super edits member
*/
function editMemberPTCommBySuper() {
    var upPreset = $('pt_1');
    var upAutoPreset = $('pt_2');
    var isAuto = $('chkAutoPT') && $('chkAutoPT').checked;

    var url = '../Handlers/Super/MemberPTComm.ashx';

    url = SetParameterValue('custids', _page.isMult ? $('custids').innerHTML : _page.custId, url);
    url = SetParameterValue('roleid', _page.custRoleId, url);
    url = SetParameterValue('submit', submit, url);

    if (upPreset != null)
        url = SetParameterValue('presetPT', isAuto ? upPreset.options[upPreset.options.length - 1].value : upPreset.options[upPreset.selectedIndex].value, url);
    else
        url = SetParameterValue('presetPT', $('presetPT').innerHTML, url);

    if (upAutoPreset)
        url = SetParameterValue('autoPresetPT', isAuto ? upAutoPreset.options[upAutoPreset.selectedIndex].value : 0.0, url);
    if ($('chkAutoPT'))
        url = SetParameterValue('isAutoPresetPT', isAuto, url);

    var pars = getGameSetting();

    url = SetParameterValue('pars', pars, url);

    jx.request(
    url,
    function (res) {
        try {
            var ret = eval('(' + res.responseText + ')');
            ret = ret.errMsg;
            var users = userNames;
            var codes = ret.split('|')[0].split('$');
            var msgs = ret.split('|')[1].split('$');

            for (var i = 0; i < codes.length; i++) {
                top.userNames.push(users[i]);
                top.errCodes.push(codes[i] == 0 ? -1 : 1);
                top.errMsgs.push(msgs[i]);
            }
        } catch (ex) { }

        if (submit == true) {
            finish1Task++;
        }
        else {
            showErrMsg();
        }
    },
    'post',
    '',
    true
    );
}

function editMemberBySuper() {
    editMemberPTCommBySuper();
    editGameSetting();
}
//========================================================================================================================
//========================================================================================================================
/*
Master edits agent
*/
function editAgentPTComm() {
    var upPreset = $('pt_1');
    var upAutoPreset = $('pt_2');
    var max = $('pt_3');
    var min = $('pt_4');
    var comm = $('comm_5');
    var isAuto = $('chkAutoPT') && $('chkAutoPT').checked;

    var url = '../Handlers/Master/AgentPTComm.ashx';

    url = SetParameterValue('custids', _page.isMult ? $('custids').innerHTML : _page.custId, url);
    url = SetParameterValue('roleid', _page.custRoleId, url);
    url = SetParameterValue('submit', submit, url);

    url = SetParameterValue('presetPT', isAuto ? upPreset.options[upPreset.options.length - 1].value : upPreset.options[upPreset.selectedIndex].value, url);
    url = SetParameterValue('autoPresetPT', isAuto ? upAutoPreset.options[upAutoPreset.selectedIndex].value : 0.0, url);
    url = SetParameterValue('minPT', min.options[min.selectedIndex].value, url);
    url = SetParameterValue('maxPT', max.options[max.selectedIndex].value, url);
    url = SetParameterValue('comm', comm.options[comm.selectedIndex].value, url);
    url = SetParameterValue('isAutoPresetPT', isAuto, url);
    url = SetParameterValue('isUpdateAllMember', $('isUpdateAllMember').innerHTML, url);
    url = SetParameterValue('newMemberPT', $('newMemberPT').innerHTML, url);

    var pars = getGameSetting();

    url = SetParameterValue('pars', pars, url);

    jx.request(
    url,
    function (res) {
        try {
            var ret = eval('(' + res.responseText + ')');
            ret = ret.errMsg;
            var users = userNames;
            var codes = ret.split('|')[0].split('$');
            var msgs = ret.split('|')[1].split('$');

            if (codes[0] == '0') success = true;

            for (var i = 0; i < codes.length; i++) {
                top.userNames.push(users[i]);
                top.errCodes.push(codes[i] == 0 ? -1 : 1);
                top.errMsgs.push(msgs[i]);
            }
        } catch (ex) { }

        if (submit == true) {
            finish1Task++;
        }
        else {
            showErrMsg();
        }
    },
    'post',
    '',
    true
    );
}

function editAgent() {
    editAgentPTComm();
    editGameSetting();
}
//========================================================================================================================
//========================================================================================================================
/*
Master edits member
*/
function editMemberPTCommByMaster() {
    var upPreset = $('pt_1');
    var upAutoPreset = $('pt_2');
    var isAuto = $('chkAutoPT') && $('chkAutoPT').checked;

    var url = '../Handlers/Master/MemberPTComm.ashx';

    url = SetParameterValue('custids', _page.isMult ? $('custids').innerHTML : _page.custId, url);
    url = SetParameterValue('roleid', _page.custRoleId, url);
    url = SetParameterValue('submit', submit, url);

    if (upPreset != null)
        url = SetParameterValue('presetPT', isAuto ? upPreset.options[upPreset.options.length - 1].value : upPreset.options[upPreset.selectedIndex].value, url);
    else
        url = SetParameterValue('presetPT', $('presetPT').innerHTML, url);

    if (upAutoPreset)
        url = SetParameterValue('autoPresetPT', isAuto ? upAutoPreset.options[upAutoPreset.selectedIndex].value : 0.0, url);
    if ($('chkAutoPT'))
        url = SetParameterValue('isAutoPresetPT', isAuto, url);

    var pars = getGameSetting();

    url = SetParameterValue('pars', pars, url);

    jx.request(
    url,
    function (res) {
        try {
            var ret = eval('(' + res.responseText + ')');
            ret = ret.errMsg;
            var users = userNames;
            var codes = ret.split('|')[0].split('$');
            var msgs = ret.split('|')[1].split('$');

            for (var i = 0; i < codes.length; i++) {
                top.userNames.push(users[i]);
                top.errCodes.push(codes[i] == 0 ? -1 : 1);
                top.errMsgs.push(msgs[i]);
            }
        } catch (ex) { }

        if (submit == true) {
            finish1Task++;
        }
        else {
            showErrMsg();
        }
    },
    'post',
    '',
    true
    );
}

function editMemberByMaster() {
    editMemberPTCommByMaster();
    editGameSetting();
}
//========================================================================================================================
//========================================================================================================================
/*
Agent edits member
*/
function editMemberPTComm() {
    var upPreset = $('pt_1');
    var comm = $('comm_2');

    var url = '../Handlers/Agent/MemberPTComm.ashx';

    url = SetParameterValue('custids', _page.isMult ? $('custids').innerHTML : _page.custId, url);
    url = SetParameterValue('roleid', _page.custRoleId, url);
    url = SetParameterValue('submit', submit, url);

    url = SetParameterValue('presetPT', upPreset.options[upPreset.selectedIndex].value, url);
    url = SetParameterValue('comm', comm.options[comm.selectedIndex].value, url);
    url = SetParameterValue('enable', $('bettype_chkEnableLiveCS').checked, url);
    url = SetParameterValue('disable', $('bettype_chkEnableLiveCS').disabled, url);

    var pars = getGameSetting();

    url = SetParameterValue('pars', pars, url);

    jx.request(
    url,
    function (res) {
        try {
            var ret = eval('(' + res.responseText + ')');
            ret = ret.errMsg;
            var users = userNames;
            var codes = ret.split('|')[0].split('$');
            var msgs = ret.split('|')[1].split('$');

            for (var i = 0; i < codes.length; i++) {
                top.userNames.push(users[i]);
                top.errCodes.push(codes[i] == 0 ? -1 : 1);
                top.errMsgs.push(msgs[i]);
            }
        } catch (ex) { }

        if (submit == true) {
            finish1Task++;
        }
        else {
            showErrMsg();
        }
    },
    'post',
    '',
    true
    );
}

function editMember() {
    editMemberPTComm();
    editGameSetting();
}
//========================================================================================================================

function PTChanging(el) {
    var sel = el.value;
    if ($("pt_3")) {
        var cur = $("pt_3").value;
        PT_RenderCombo($("pt_3"), _page.MaxPT_Min, _page.MaxPT_Max - sel, 0.01, cur);
    }
    if ($("pt_4")) {
        PT_RenderCombo2($("pt_4"), _page.MinPT_Min, _page.MinPT_Max - sel, 0.01);
    }
}
//=========================================================================================================================
window.onload = function () {
    flag = _page.oldAuto;
    setEventForCheckbox();

    if (_page.uncheck && _page.custRoleId == 1) {
        $('warningMsg').innerHTML = _page.uncheckMsg;
        $('warningMsg').style.display = 'block';
    }

    if (_page.showWinLimit) {
        taskCount += 1;
    }
}
function PT_RenderCombo2(combo, min, max, step) {
    var index = 0;
    var itemIndex = 0;
    var selectedValue = combo.value;

    if (!combo) return;
    combo.options.length = 0;

    if (combo.id == 'pt_4')
        max = $('pt_3').value;

    var c = Math.round((max * 100 - min * 100) / (step * 100));

    if (c <= 0) {
        var opt = document.createElement("option");
        opt.text = opt.value = Math.round(min * 100) / 100;
        combo.options.add(opt);
        return;
    }

    for (var i = c; i >= 0; i--) {
        var f = Math.round(min * 100 + (i * step * 100)) / 100;
        var opt = document.createElement("option");

        if (parseFloat(selectedValue) == parseFloat(f))
            index = itemIndex;

        opt.text = opt.value = f;
        combo.options.add(opt);
        itemIndex++;
    }

    if (document.all)
        combo.options[index].selected = true;
    else
        combo.value = selectedValue;
}

function onchangeCheckbox() {
    for (var i = 0; i < _page.gameRows; i++) {
        var uncheckCount = 0;
        var items = document.getElementsByName('gameRow' + i);
        if (items[0].type == 'checkbox') {
            for (var j = 0; j < items.length; j++) {
                if (!items[j].checked)
                    uncheckCount++;
            }
        }

        if (uncheckCount == items.length) {
            $('warningMsg').innerHTML = _page.warningMsg;
            $('warningMsg').style.display = 'block';
            break;
        }
        else {
            $('warningMsg').style.display = 'none';
        }
    }
}

function setEventForCheckbox() {
    for (var i = 0; i < _page.gameRows; i++) {
        var items = document.getElementsByName('gameRow' + i);
        if (items[0].type == 'checkbox') {
            for (var j = 0; j < items.length; j++) {
                items[j].onclick = onchangeCheckbox;
            }
        }
    }
}

function updatePT() {
    if (_page.roleId == 4 && _page.custRoleId == 3)         //Super edits master
        editMasterPTComm2();
    else if (_page.roleId == 4 && _page.custRoleId == 1)    //Super edits member
        editMemberPTCommBySuper();
    else if (_page.roleId == 3 && _page.custRoleId == 2)    //Master edits agent
        editAgentPTComm2();
    else if (_page.roleId == 3 && _page.custRoleId == 1)    //Master edits member
        editMemberPTCommByMaster();
    else if (_page.roleId == 2 && _page.custRoleId == 1)    //Agents edits member
        editMemberPTComm2();
    else                                                    //Invalid request
        alert(_page.noAccess);
}

function editMasterPTComm2() {
    var upPreset = $('pt_1');
    var upAutoPreset = $('pt_2');
    var max = $('pt_3');
    var min = $('pt_4');
    var comm = $('comm_5');
    var isAuto = $('chkAutoPT') && $('chkAutoPT').checked;

    var url = '../Handlers/Super/MasterPTComm.ashx';

    url = SetParameterValue('custids', _page.isMult ? $('custids').innerHTML : _page.custId, url);
    url = SetParameterValue('roleid', _page.custRoleId, url);
    url = SetParameterValue('submit', submit, url);

    url = SetParameterValue('presetPT', isAuto ? upPreset.options[upPreset.options.length - 1].value : upPreset.options[upPreset.selectedIndex].value, url);
    url = SetParameterValue('autoPresetPT', isAuto ? upAutoPreset.options[upAutoPreset.selectedIndex].value : 0.0, url);
    url = SetParameterValue('minPT', min.options[min.selectedIndex].value, url);
    url = SetParameterValue('maxPT', max.options[max.selectedIndex].value, url);
    url = SetParameterValue('comm', comm.options[comm.selectedIndex].value, url);
    url = SetParameterValue('isAutoPresetPT', isAuto, url);
    url = SetParameterValue('isUpdateAllMember', $('isUpdateAllMember').innerHTML, url);
    url = SetParameterValue('newMemberPT', $('newMemberPT').innerHTML, url);

    var pars = getGameSetting();

    url = SetParameterValue('pars', pars, url);

    jx.request(
    url,
    function (res) {
        try {
            var ret = eval('(' + res.responseText + ')');
            ret = ret.errMsg;
            var users = userNames;
            var codes = ret.split('|')[0].split('$');
            var msgs = ret.split('|')[1].split('$');

            if (codes[0] == '0') success = true;

            for (var i = 0; i < codes.length; i++) {
                top.userNames.push(users[i]);
                top.errCodes.push(codes[i] == 0 ? -1 : 1);
                top.errMsgs.push(msgs[i]);
            }

            setTimeout('editGameSetting()', 2000);
        } catch (ex) { }

        if (submit == true) {
            finish1Task++;
        }
        else {
            showErrMsg();
        }
    },
    'post',
    '',
    true
    );
}

function editAgentPTComm2() {
    var upPreset = $('pt_1');
    var upAutoPreset = $('pt_2');
    var max = $('pt_3');
    var min = $('pt_4');
    var comm = $('comm_5');
    var isAuto = $('chkAutoPT') && $('chkAutoPT').checked;

    var url = '../Handlers/Master/AgentPTComm.ashx';

    url = SetParameterValue('custids', _page.isMult ? $('custids').innerHTML : _page.custId, url);
    url = SetParameterValue('roleid', _page.custRoleId, url);
    url = SetParameterValue('submit', submit, url);

    url = SetParameterValue('presetPT', isAuto ? upPreset.options[upPreset.options.length - 1].value : upPreset.options[upPreset.selectedIndex].value, url);
    url = SetParameterValue('autoPresetPT', isAuto ? upAutoPreset.options[upAutoPreset.selectedIndex].value : 0.0, url);
    url = SetParameterValue('minPT', min.options[min.selectedIndex].value, url);
    url = SetParameterValue('maxPT', max.options[max.selectedIndex].value, url);
    url = SetParameterValue('comm', comm.options[comm.selectedIndex].value, url);
    url = SetParameterValue('isAutoPresetPT', isAuto, url);
    url = SetParameterValue('isUpdateAllMember', $('isUpdateAllMember').innerHTML, url);
    url = SetParameterValue('newMemberPT', $('newMemberPT').innerHTML, url);

    var pars = getGameSetting();

    url = SetParameterValue('pars', pars, url);

    jx.request(
    url,
    function (res) {
        try {
            var ret = eval('(' + res.responseText + ')');
            ret = ret.errMsg;
            var users = userNames;
            var codes = ret.split('|')[0].split('$');
            var msgs = ret.split('|')[1].split('$');

            if (codes[0] == '0') success = true;

            for (var i = 0; i < codes.length; i++) {
                top.userNames.push(users[i]);
                top.errCodes.push(codes[i] == 0 ? -1 : 1);
                top.errMsgs.push(msgs[i]);
            }

            setTimeout('editGameSetting()', 2000);
        } catch (ex) { }

        if (submit == true) {
            finish1Task++;
        }
        else {
            showErrMsg();
        }
    },
    'post',
    '',
    true
    );
}

function editMemberPTComm2() {
    var upPreset = $('pt_1');
    var comm = $('comm_2');

    var url = '../Handlers/Agent/MemberPTComm.ashx';

    url = SetParameterValue('custids', _page.isMult ? $('custids').innerHTML : _page.custId, url);
    url = SetParameterValue('roleid', _page.custRoleId, url);
    url = SetParameterValue('submit', submit, url);

    url = SetParameterValue('presetPT', upPreset.options[upPreset.selectedIndex].value, url);
    url = SetParameterValue('comm', comm.options[comm.selectedIndex].value, url);
    url = SetParameterValue('enable', $('bettype_chkEnableLiveCS').checked, url);
    url = SetParameterValue('disable', $('bettype_chkEnableLiveCS').disabled, url);

    var pars = getGameSetting();

    url = SetParameterValue('pars', pars, url);

    jx.request(
    url,
    function (res) {
        try {
            var ret = eval('(' + res.responseText + ')');
            ret = ret.errMsg;
            var users = userNames;
            var codes = ret.split('|')[0].split('$');
            var msgs = ret.split('|')[1].split('$');

            for (var i = 0; i < codes.length; i++) {
                top.userNames.push(users[i]);
                top.errCodes.push(codes[i] == 0 ? -1 : 1);
                top.errMsgs.push(msgs[i]);
            }

            setTimeout('editGameSetting()', 2000);
        } catch (ex) { }

        if (submit == true) {
            finish1Task++;
        }
        else {
            showErrMsg();
        }
    },
    'post',
    '',
    true
    );
}

function getGameSetting() {
    var pars = '';

    //pars = [{1^1101, 'Baccarat', (1^ '10-100'^ true$ 2^ '20-200'^ false)}; {2^1102, 'Sicbo', (3^ '20-200'^ false$ 4^ '20-200'^ false)}]
    for (var i = 0; i < _page.gameRows; i++) {
        var betting = '';
        var betID = $('betTypeID' + i).innerHTML;
        var gameTypeID = $('gameTypeID' + i).innerHTML;
        var gameTypeName = $('gameType' + i).innerHTML;
        var bets = document.getElementsByName('gameRow' + i);

        for (var j = 0; j < bets.length; j++) {
            var id = bets[j].id.split('_')[1];
            var name = document.getElementById('innerText' + id).innerHTML;
            var checked = bets[j].checked;

            betting += id + "^" + name + "^" + checked + '$';
        }

        betting = betting.substring(0, betting.length - 1);

        pars += '{' + gameTypeID + '|' + betID + "|" + gameTypeName + "|(" + betting + ')};';
    }

    pars = '[' + pars.substring(0, pars.length - 1) + ']';

    return pars;
}

function saveMemberWinLimit() {
    initData();

    var url = '../Handlers/MemberWinLimitHandler.ashx';

    url = SetParameterValue('custids', _page.isMult ? $('custids').innerHTML : _page.custId, url);
    url = SetParameterValue('roleid', _page.custRoleId, url);
    url = SetParameterValue('submit', submit, url);
    url = SetParameterValue('isDailyReset', $('chkAutoReset').checked, url);
    url = SetParameterValue('maxWinLimit', $('comm_0').value, url);

    jx.request(
    url,
    function (res) {
        try {
            var ret = eval('(' + res.responseText + ')');
            ret = ret.errMsg;
            var users = userNames;
            var codes = ret.split('|')[0].split('$');
            var msgs = ret.split('|')[1].split('$');

            for (var i = 0; i < codes.length; i++) {
                top.userNames.push(users[i]);
                top.errCodes.push(codes[i] == 0 ? -1 : 1);
                top.errMsgs.push(msgs[i]);
            }
        } catch (ex) { }

        if (submit == true) {
            finish1Task++;
        }
        else {
            showErrMsg();
        }
    },
    'post',
    '',
    true
    );
}

function viewCurrentWinLimit() {
    initData();

    var url = '../Handlers/CurrentMemberWinLimitHandler.ashx';

    url = SetParameterValue('custids', _page.isMult ? $('custids').innerHTML : _page.custId, url);
    url = SetParameterValue('roleid', _page.custRoleId, url);
    url = SetParameterValue('submit', submit, url);

    jx.request(
    url,
    function (res) {
        try {
            var ret = eval('(' + res.responseText + ')');
            ret = ret.errMsg;

            $('winLimitTitle').innerHTML = _page.currentWinLimit;
            $('currentWinLimit').innerHTML = '';
            $('winLimitValue').innerHTML = ret;
        } catch (ex) { }

        if (submit == true) {
            finish1Task++;
        }
        else {
            showErrMsg();
        }
    },
    'post',
    '',
    true
    );
}