var url = 'P2P_Page.aspx';
var urlScreenName = 'Handlers/GetScreenName.ashx';
var urlBetSetting = 'Handlers/UpdateBetSetting.ashx';
var urlGameSetting = 'Handlers/UpdateGameSetting.ashx';
var urlCommission = 'Handlers/UpdateCommission.ashx';

var submit = 'false';
var finish1Task = 0;
var userNames = '[]';
var fnIndex = '1';
var timerID = null;
var taskCount = 3;

function initData() {
    age.ShowMaskDiv('../../../_GlobalResources/Images/ajax-loader.gif');

    top.errCodes = new Array();
    top.errMsgs = new Array();
    top.userNames = new Array();

    var userStr = $('custnames').innerHTML == '' ? _page.userName : $('custnames').innerHTML;

    if (userStr.indexOf(',') > 0)
        userNames = userStr.split(',');
    else
        userNames = new Array(userStr);
}

function singleSubmit(_fnIndex, _submit) {
    if (_page.disabled) {
        alert(_page.disabledMsg);
        return;
    }

    fnIndex = _fnIndex;
    submit = _submit;

    var min = parseFloat(_page.minTransfer);
    var max = parseFloat(_page.maxTransfer);
    var part = $('betSetting').innerHTML;
    var maxCredit = $('maxTranCredit').value;

    while (maxCredit.indexOf(',') > 0) {
        maxCredit = maxCredit.replace(',', '');
    }

    var val = parseFloat(maxCredit);

    if ((maxCredit == '' || val < min || val > max) && fnIndex == '1') {
        alert(part + ': ' + _page.invalidValue);
        $('maxTranCredit').focus();
    }
    else {
        initData();

        switch (fnIndex) {
            case '1':
                saveMaxTransfer(submit, $('custids').innerHTML);
                break;
            case '2':
                saveGameSetting(submit, $('custids').innerHTML);
                break;
            case '3':
                saveCommission(submit, $('custids').innerHTML);
                break;
        }
    }
}

function btnSubmit_OnClick() {
    if (_page.disabled) {
        alert(_page.disabledMsg);
        return;
    }

    submit = 'true';
    finish1Task = 0;

    var min = parseFloat(_page.minTransfer);
    var max = parseFloat(_page.maxTransfer);
    var part = $('betSetting').innerHTML;

    initData();

    var maxCredit = $('maxTranCredit').value;

    while (maxCredit.indexOf(',') > 0) {
        maxCredit = maxCredit.replace(',', '');
    }

    var val = parseFloat(maxCredit);

    if (maxCredit == '' || val < min || val > max) {
        taskCount = 2;
        var users = userNames;

        for (var i = 0; i < users.length; i++) {
            top.userNames.push(users[i]);
            top.errCodes.push(1);
            top.errMsgs.push($('betSetting').innerHTML + ": " + _page.invalidValue);
        }
    }
    else {
        taskCount = 3;
        saveMaxTransfer(submit, $('custids').innerHTML);
    }

    saveCommission(submit, $('custids').innerHTML);
    saveGameSetting(submit, $('custids').innerHTML);
    timerID = setTimeout('showErrMsg()', 100);
}

function GenerateMessage(ArrCodes, ArrMsgs, ArrUserName, actualUsers, isCombined) {
    var isSucc = true;
    isSucc = isSucc & ageMsg.CheckMessageIsSucc(ArrCodes);

    var myTable = '<table id="tblMessage" cellpadding="0" cellspacing="0" border="0" style="width:100%;">';
    var myTDTemp = '';
    var myEndTable = ' </table>';

    if (isCombined == false) {
        for (var j = 0; j < ArrUserName.length; j++) {
            myTDTemp += '<tr><td class="EnrichUserName">';
            myTDTemp += ArrUserName[j];
            myTDTemp += "</td></tr>";

            if (ArrCodes[j] < 0) {
                myTDTemp += '<tr><td class="EnrichSuccmsg">';
                myTDTemp += ArrMsgs[j];
                myTDTemp += "</td></tr>";
            }
            else {
                myTDTemp += '<tr><td class="EnrichErrmsg">';
                myTDTemp += ArrMsgs[j];
                myTDTemp += "</td></tr>";
            }
        }
    }
    else {
        for (var j = 0; j < actualUsers.length; j++) {
            myTDTemp += '<tr><td class="EnrichUserName">';
            myTDTemp += actualUsers[j];
            myTDTemp += "</td></tr>";

            //First message
            if (ArrCodes[j] < 0) {
                myTDTemp += '<tr><td class="EnrichSuccmsg">';
                myTDTemp += ArrMsgs[j];
                myTDTemp += "</td></tr>";
            }
            else {
                myTDTemp += '<tr><td class="EnrichErrmsg">';
                myTDTemp += ArrMsgs[j];
                myTDTemp += "</td></tr>";
            }
            //Second message
            if (ArrCodes[1 * actualUsers.length + j] < 0) {
                myTDTemp += '<tr><td class="EnrichSuccmsg">';
                myTDTemp += ArrMsgs[1 * actualUsers.length + j];
                myTDTemp += "</td></tr>";
            }
            else {
                myTDTemp += '<tr><td class="EnrichErrmsg">';
                myTDTemp += ArrMsgs[1 * actualUsers.length + j];
                myTDTemp += "</td></tr>";
            }
            //Third message
            if (ArrCodes[2 * actualUsers.length + j] < 0) {
                myTDTemp += '<tr><td class="EnrichSuccmsg">';
                myTDTemp += ArrMsgs[2 * actualUsers.length + j];
                myTDTemp += "</td></tr>";
            }
            else {
                myTDTemp += '<tr><td class="EnrichErrmsg">';
                myTDTemp += ArrMsgs[2 * actualUsers.length + j];
                myTDTemp += "</td></tr>";
            }
        }
    }

    myTable += myTDTemp + myEndTable;

    ageMsg.ShowMsgs(myTable, isSucc);
}

function showErrMsg() {
    if (submit == 'true' && finish1Task >= taskCount) {
        submit = 'false';
        finish1Task = 0;

        clearTimeout(timerID);
        timerID = null;

        GenerateMessage(top.errCodes, top.errMsgs, top.userNames, userNames, true);
        age.HideMaskDiv();

        top.errCodes = new Array();
        top.errMsgs = new Array();
        top.userNames = new Array();

        if (userNames.length > 4) {
            $('errMsg').style.height = '220px';
            $('errMsg').style.overflow = 'auto';
        }

        return false;
    }
    else if (submit == 'false') {
        GenerateMessage(top.errCodes, top.errMsgs, top.userNames, userNames, false);
        age.HideMaskDiv();

        top.errCodes = new Array();
        top.errMsgs = new Array();
        top.userNames = new Array();

        if (userNames.length > 4) {
            $('errMsg').style.height = '220px';
            $('errMsg').style.overflow = 'auto';
        }

        return false;
    }
    else if (submit == 'true' && finish1Task < 3) {
        //Reset timer
        clearTimeout(timerID);
        timerID = null;

        timerID = setTimeout('showErrMsg()', 100);
    }

    return true;
}

function saveCommission(_submit, custid) {
    submit = _submit;

    try {
        var comm1 = $('Comm1');
        var discount_1 = comm1.options[comm1.selectedIndex].value;

        var comm2 = $('Comm2');
        var discount_2 = comm2.options[comm2.selectedIndex].value;

        var url1 = SetParameterValue('roleid', _page.custRoleId, urlCommission);
        url1 = SetParameterValue('custids', custid, url1);

        url1 = SetParameterValue('discount_1', discount_1, url1);
        url1 = SetParameterValue('discount_2', discount_2, url1);
    }
    catch (err) {
        for (var i = 0; i < userNames.length; i++) {
            top.userNames.push(userNames[i]);
            top.errCodes.push(1);
            top.errMsgs.push($('commission').innerHTML + ": " + _page.updateError);
        }

        if (submit == 'true') {
            finish1Task++;
        }
        else {
            showErrMsg();
        }
    }

    jx.request(
    url1,
    function (res) {
        try {
            var ret = eval('(' + res.responseText + ')');
            ret = ret.errMsg;
            var users = userNames;
            var codes = ret.split(':')[0].split(',');
            var msgs = ret.split(':')[1].split(',');

            for (var i = 0; i < codes.length; i++) {
                top.userNames.push(users[i]);
                top.errCodes.push(codes[i] == 0 ? -1 : 1);
                top.errMsgs.push($('commission').innerHTML + ": " + msgs[i]);
            }
        } catch (ex) { }

        if (submit == 'true') {
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

function saveGameSetting(_submit, custid) {
    submit = _submit;

    if (_page.gameRows > 0) {
        var stake1_1 = false;
        var stake1_2 = false;
        var stake1_3 = false;
        var stake1_4 = false;
        var stake1_5 = false;

        var url1 = SetParameterValue('roleid', _page.custRoleId, urlGameSetting);
        url1 = SetParameterValue('custids', custid, url1);

        var pars = '';

        for (var i = 1; i <= _page.gameRows; i++) {
            stake1_1 = $('chkFun' + i.toString()).value;
            stake1_2 = $('chkLow' + i.toString()).checked;
            stake1_3 = $('chkMed' + i.toString()).checked;
            stake1_4 = $('chkHigh' + i.toString()).checked;
            stake1_5 = $('chkVip' + i.toString()).checked;

            pars += '{' +
                        stake1_1 + ',' +
                        stake1_2 + ',' +
                        stake1_3 + ',' +
                        stake1_4 + ',' +
                        stake1_5 +
                    '};';

            url1 = SetParameterValue('bettype' + i.toString(), $('bettypeID' + i.toString()).innerHTML, url1);
        }

        if (pars.length > 0)
            pars = pars.substring(0, pars.length - 1);

        url1 = SetParameterValue('pars', '[' + pars + ']', url1);

        jx.request(
            url1,
            function (res) {
                try {
                    var ret = eval('(' + res.responseText + ')');
                    ret = ret.errMsg;
                    var users = userNames;
                    var codes = ret.split(':')[0].split(',');
                    var msgs = ret.split(':')[1].split(',');

                    for (var i = 0; i < codes.length; i++) {
                        top.userNames.push(users[i]);
                        top.errCodes.push(codes[i] == 0 ? -1 : 1);
                        top.errMsgs.push($('gameSetting').innerHTML + ": " + msgs[i]);
                    }
                } catch (ex) { }

                if (submit == 'true') {
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
    else {
        for (var j = 0; j < userNames.length; j++) {
            top.userNames.push(userNames[j]);
            top.errCodes.push(1);
            top.errMsgs.push($('gameSetting').innerHTML + ": " + _page.updateError);
        }

        if (submit == 'true') {
            finish1Task++;
        }
        else {
            showErrMsg();
        }
    }
}

function saveMaxTransfer(_submit, custid) {
    submit = _submit;

    try {
        var maxTranCredit = $('maxTranCredit').value;

        while (maxTranCredit.indexOf(',') > 0) {
            maxTranCredit = maxTranCredit.replace(',', '');
        }

        var chkResetTranLimit = $('chkResetTranLimit').checked;

        var url1 = SetParameterValue('roleid', _page.custRoleId, urlBetSetting);
        url1 = SetParameterValue('custids', custid, url1);

        url1 = SetParameterValue('maxTranCredit', maxTranCredit, url1);
        url1 = SetParameterValue('chkResetTranLimit', chkResetTranLimit, url1);

        jx.request(
        url1,
        function (res) {
            try {
                var ret = eval('(' + res.responseText + ')');
                ret = ret.errMsg;
                var users = userNames;
                var codes = ret.split(':')[0].split(',');
                var msgs = ret.split(':')[1].split(',');

                for (var i = 0; i < codes.length; i++) {
                    top.userNames.push(users[i]);
                    top.errCodes.push(codes[i] == 0 ? -1 : 1);
                    top.errMsgs.push($('betSetting').innerHTML + ": " + msgs[i]);
                }
            } catch (ex) { }

            if (submit == 'true') {
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
    } catch (err) {
        for (var i = 0; i < userNames.length; i++) {
            top.userNames.push(userNames[i]);
            top.errCodes.push(1);
            top.errMsgs.push($('betSetting').innerHTML + ": " + _page.updateError);
        }

        if (submit == 'true') {
            finish1Task++;
        }
        else {
            showErrMsg();
        }
    }
}

function getScreenName() {
    if (_page.loadScreenName == true) {
        top.errCodes = new Array();
        top.errMsgs = new Array();
        top.userNames = new Array();
        top.userNames.push(_page.userName.toUpperCase());

        $('screenName').innerHTML = '<img src="Resources/Images/loading.gif" />';

        var url1 = SetParameterValue('custids', _page.custId, urlScreenName);

        jx.request(
        url1,
        function (res) {
            try {
                var resText = eval('(' + res.responseText + ')');

                $('screenName').innerHTML = resText.errMsg;
            } catch (ex) { }
        },
        'post',
        '',
        true
    );
    }
}

function numberOnly(ev) {
    if (ev.keyCode == 35 || ev.keyCode == 36 || ev.keyCode == 37 || ev.keyCode == 39) {
        return true;
    }

    if ((ev.keyCode < 48 || (ev.keyCode > 57 && ev.keyCode < 96) || ev.keyCode > 105) &&
        ev.keyCode != 8 && ev.keyCode != 46 && ev.keyCode != 13) {
        if (window.event) { // IE
            window.event.cancelBubble = true;
            window.event.returnValue = false;
        } else {
            ev.preventDefault();
            ev.stopPropagation();
        }
        return false;
    }

    formatNumber();

    return true;
}

function calculateComm(upValue, rowId) {
    var obj = $('divCommUpline' + rowId);
    var comm = $('Comm' + rowId);
    var commVal = comm.options[comm.selectedIndex].value;

    var val = upValue - commVal;
    obj.innerHTML = val.toFixed(2);
};

function format(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

function formatNumber(ev) {
    if (ev.keyCode == 35 || ev.keyCode == 36 || ev.keyCode == 37 || ev.keyCode == 39) {
        return false;
    }

    var obj = $('maxTranCredit');
    var value = obj.value;

    if (value.length > 1 && value.indexOf('0') == 0) {
        value = value.replace('0', '');
    }

    while (value.indexOf(',') > 0) {
        value = value.replace(',', '');
    }

    var tmp = value.toString();

    value = tmp.length > 10 ? tmp.substring(0, 11) : value;

    var formatValue = format(value);

    if (_page.custRoleId == 1) {
        var ata = format(parseFloat(value) + _page.todayWinlost - _page.lobbyBalance);
        $('ata').innerHTML = (ata == (NaN || 'NaN') ? '0' : ata);
    }

    obj.value = formatValue;
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

RegisterStartUp('getScreenName(); initQueryString();');