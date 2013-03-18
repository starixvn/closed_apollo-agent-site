var _noFocusStartUp = true;

//function viewOutStById(custId, userName, fDate, tDate) {
//    window.location = "../Report/OutstandingAgentDetail.aspx?custid=" + custId + "&username=" + userName + "&type=RunByDate_Mem&fdate=" + fDate + "&tdate=" + tDate + "&ShowCasino=1&ShowSB=1";
//}

function viewOutStById(custId, userName, fDate, tDate) {
    window.location = "../_Reports/BetList/OutstandingDetail.aspx?custid=" + custId + "&username=" + userName + "&type=RunByDate_Mem&fdate=" + fDate + "&tdate=" + tDate + "&chkgetall=on&chk_all=on&chk_ShowCasino=on&chk_ShowSB=on";
}

function viewOutStByUsrname(userName, custId) {
    window.location = "../_Reports/BetList/DailyStatement.aspx?username=" + userName + "&option=1&custid=" + custId + "&page=1";
    //window.location = "../Report/OutstandingAgentDetail.aspx?username=" + userName + "&option=1&custid=" + custId + "&page=1";
}

function filterAcc(value) {
    window.location = "MultipleTransfer.aspx?status=" + value;
}
function CheckAll() {
    var checkID = document.getElementsByName('chkitem');

    if (!checkID) return;
    for (var i = 0; i < checkID.length; i++) {
        var arrAmt = checkID[i].value.split('_');

        if (arrAmt[0] != 0) {
            checkID[i].checked = document.getElementById("chkall").checked;
        }
    }
}
function ShowTransYesBalance(amt, custid, username) {
    var popH = 120, popW = 400;
    ageWnd.open('../Popup/TransYesBalance.aspx?amt=' + amt + '&custid=' + custid + '&usn=' + username + '&status=' + document.getElementById('statusFilter').value, '', 350, 100, popW, popH);
}
function showTransYesBalanceForAg(custid) {
    var popH = 420, popW = 450;
    ageWnd.open('../Popup/TransYesBalanceForAgent.aspx?custid=' + custid + '&status=' + document.getElementById('statusFilter').value, '', 50, 100, popW, popH);
}
function TransMulti() {
    if (document.getElementById('hdisInternalSite').value == 'True') {
        alert(document.getElementById('hdNotAuthorized').value);
        return;
    }

    var checkID = document.getElementsByName('chkitem');
    var custidmulti = "";
    var counter = 0;
    var isok = false;
    var issucc = false;
    var mulError = '';
    if (!checkID) return;
    for (var i = 0; i < checkID.length; i++) {
        if (checkID[i].checked == true) {
            counter++;
            var amtarr = checkID[i].value.split('_');
            if (counter >= 400) {
                custidmulti += amtarr[1] + "_";
                counter = 0;
            }
            else {
                custidmulti += amtarr[1] + ",";
            }
        }
    }

    if (custidmulti.length > 0) {
        document.getElementById('btntransfer2').disabled = true;
        document.getElementById('btntransfer').disabled = true;
        custidmulti = custidmulti.substring(0, custidmulti.length - 1);
        var _arr = custidmulti.split('_');

        var isSelectedUserName = false;
        if (document.getElementById('txtUserName').value != null && document.getElementById('txtUserName').value != 'Username or First/Last Name')
            isSelectedUserName = true;

        if (!document.getElementById("chkall").checked || isSelectedUserName) {
            if (confirm(document.getElementById('wrntransfer').value)) {
                DelayPage();
                for (var i = 0; i < _arr.length; i++) {
                    ajax.Request('../Handlers/UpdateMultiTransferYesBalance.ashx', {
                        method: 'post',
                        parameters: 'custid=' + _arr[i] + '&amount=0&ischeck=' + (document.getElementById("chkall").checked && !isSelectedUserName) + '&isstatus=' + document.getElementById('statusFilter').value + '&arrId=' + _arr,
                        asynchronous: false,
                        onSuccess: function (res) {
                            if (res.responseText == 'Ok') {
                                issucc = true;
                            }
                            else if (res.responseText == '106') {
                                document.getElementById('btntransfer2').disabled = false;
                                document.getElementById('btntransfer').disabled = false;
                                showErrMsg(document.getElementById('hdConfirmCloseSubdAcc').value, 0);
                            }
                            else if (res.responseText == '105') // kick out
                            {
                                alert(document.getElementById('hdConfirmClosedAcc').value);
                                location.href = '../Authorization/Logout.ashx';
                            }
                            else {
                                var errarr = res.responseText.split('_');

                                if (errarr[1] == 'true') {
                                    issucc = true;
                                }
                                mulError += '- ' + errarr[0] + '\n';
                            }
                            custidmulti = '';
                            document.getElementById("chkall").checked = false;
                            for (var i = 0; i < checkID.length; i++) {
                                if (checkID[i].checked == true) {
                                    checkID[i].checked = false;
                                }
                            }
                        }
                    });
                }

                if (mulError != '') {
                    document.getElementById('btntransfer2').disabled = false;
                    document.getElementById('btntransfer').disabled = false;
                    alert(mulError);
                }
                if (issucc == true) {
                    setTimeout("location = location.href", 2000);
                }
            }
            else {
                document.getElementById('btntransfer2').disabled = false;
                document.getElementById('btntransfer').disabled = false;
            }
        }
        else {
            if (confirm(document.getElementById('wrntransfer').value)) {
                DelayPage();
                ajax.Request('../Handlers/UpdateMultiTransferYesBalance.ashx', {
                    method: 'post',
                    parameters: 'custid=0&amount=0&ischeck=' + (document.getElementById("chkall").checked && !isSelectedUserName) + '&isstatus=' + document.getElementById('statusFilter').value + '&arrId=' + _arr,
                    asynchronous: true,
                    onComplete: function (res) {
                        if (res.responseText == 'Ok') {
                            setTimeout("location = location.href", 2000);
                        }
                        else if (res.responseText == '106') {
                            document.getElementById('btntransfer2').disabled = false;
                            document.getElementById('btntransfer').disabled = false;
                            showErrMsg(document.getElementById('hdConfirmCloseSubdAcc').value, 0);
                        }
                        else if (res.responseText == '105') // kick out
                        {
                            alert(document.getElementById('hdConfirmClosedAcc').value);
                            location.href = '../Authorization/Logout.ashx';
                        }
                        else {
                            var errarr = res.responseText.split('_');

                            if (errarr[1] == 'true') {
                                setTimeout("location = location.href", 2000);
                            }
                            else {
                                document.getElementById('btntransfer2').disabled = false;
                                document.getElementById('btntransfer').disabled = false;
                            }
                        }

                        custidmulti = "";
                        document.getElementById("chkall").checked = false;
                        for (var i = 0; i < checkID.length; i++) {
                            if (checkID[i].checked == true) {
                                checkID[i].checked = false;
                            }
                        }
                    }
                });
            }
            else {
                document.getElementById('btntransfer2').disabled = false;
                document.getElementById('btntransfer').disabled = false;
            }
        }
    }
    else {
        Err(document.getElementById('errchooseitem').value);
    }
}

function Err(msg) {
    document.getElementById('spmsgerr').innerHTML = msg;
    document.getElementById('diverrmsg').style.display = window.attachEvent ? 'block' : 'table';
}

function IsCheck() {
    var checkID = document.getElementsByName('chkitem');
    var ischeck = false;
    var isuncheck = false;
    if (!checkID) return;
    for (var j = 0; j < checkID.length; j++) {
        var arrAmt = checkID[j].value.split('_');
        if (arrAmt[0] != 0) {
            if (checkID[j].checked == false) {
                isuncheck = true;
                break;
            }
        }
    }

    if (isuncheck == true) {
        document.getElementById('chkall').checked = false;
    }
    else {
        document.getElementById('chkall').checked = true;
    }
}

//Paging
function NavigatePage(page, url, site) {
    NavigatePage2(page, url, "sitelist", site);
}

function GotoPageNext(url, site) {
    GotoPageNext2(url, "sitelist", site);
}

function searchByUsername(requestedPage, site, custId) {
    var status = "", username = "", allYesBal = "no";
    status = document.getElementById('statusFilter').value;
    username = document.getElementById('txtUserName').value;
    allYesBal = (document.getElementById('chkAllYesBal').checked ? "yes" : "no");
    var url = requestedPage + "?status=" + status + "&username=" + username + "&custId=" + custId + "&allYesBal=" + allYesBal;
    NavigatePage(1, url, site);
}
AGE.addEvent(window, "load", InitTagSuggestion);
function InitTagSuggestion() {
    initAutoComplete('txtUserName', '../_GlobalResources/Handlers/QueryUserName.ashx?isdir=1');
}
function onclickUser(username) {
    if (document.getElementById('txtUserName').value == username) {
        document.getElementById('txtUserName').className = 'text_f';
        document.getElementById('txtUserName').value = '';
    }
}
function onblurUser(username) {
    if (document.getElementById('txtUserName').value == '') {
        document.getElementById('txtUserName').className = 'text_f1';
        document.getElementById('txtUserName').value = username;
    }
}

function GetParameterValue(name, url) {
    if (null == url || null == name || 0 == name.length) return null;
    var urlToCompare = url.toLowerCase();
    name = name.toLowerCase();
    var separator = urlToCompare.contains("?" + name + "=") ? "?" : "&";
    var parameter = separator + name + "=";
    var i1 = urlToCompare.indexOf(parameter);
    if (i1 == -1) return null;
    var tmp = url.substr(i1);
    var i2 = tmp.indexOf("&", 1);
    var value = i2 >= 0 ? tmp.substr(parameter.length, i2 - parameter.length) : tmp.substr(parameter.length);
    return value;
}
String.prototype.contains = function (sub) {
    return this.indexOf(sub, 0) != -1;
}