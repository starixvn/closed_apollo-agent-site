function ShowPopupTransferMember(url, custId) {
    var popH = 352, popW = 452;
    url += '?custid=' + custId;
    ageWnd.open(url, '', 350, 100, popW, popH);
}

function TransferMulti() {
    var warning = "";
    var checkID = document.getElementsByName('chkitem');
    var custIdList = "";
    var custidSync = "";
    var counter = 0;
    var amt = "";
    var isSync = false;
    var counterSync = 0;
    var custid = "";
    if (!checkID) return;

    for (var i = 0; i < checkID.length; i++) {
        if (checkID[i].checked == true) {
            counter++;
            var arrPara = checkID[i].value.split('_');
            amt = arrPara[0];
            if (counter == 1) {
                custid = arrPara[1];
                if(arrPara[2] == 1 && arrPara[3] == "False")
                {
                    isSync = true;
                }
            }

            if (arrPara[2] == 1 && arrPara[3] == "False") {
                custidSync += arrPara[1] + ",";
            }
            else {
                custIdList += arrPara[1] + ",";
            }
        }
    }
    if (custidSync != "") {
        warning = _page.wrnResetMaxTransfer;
    }
    else {
        warning = _page.wrnTransfer;
    }
    if (custIdList.length > 0 || custidSync.length > 0) {
        if (custIdList.length > 0) {
            custIdList = custIdList.substring(0, custIdList.length - 1);
        }
        if (custidSync.length > 0) {
            custidSync = custidSync.substring(0, custidSync.length - 1);
        }
        var params = ajax.CreateParams();

        function OnComplete(result) {
            age.UnLock();
            var errCode = result.errCode;
            if (errCode == 0) {
                if (_page.CallBack) {
                    eval(_page.CallBack);
                }
            }
            else if (errCode == '106') {
                showErrMsg(_page.lblconfirmclosesubacc, 0);
            }
            else if (errCode == '105') // kick out
            {
                alert(_page.lblConfirmClosed);
                location.href = '../Authorization/Logout.ashx';
            }
            else {
                //ageMsg.Show(result.errMsg);
                if (result.errMsg.search('_') == -1) {
                    ageMsg.Show(result.errMsg);
                }
                else {
                    var msg_arr = result.errMsg.split('_');
                    if (msg_arr.length < 3) {
                        if (msg_arr[1] == 'true') {
                            top.errCodes.push(msg_arr[0]);

                            if (_page.CallBack) {
                                eval(_page.CallBack);
                            }
                        }
                        else {
                            ageMsg.Show(msg_arr[0]);
                        }
                    }
                    else {
                        if (msg_arr[1] == 'true' || msg_arr[3] == 'true') {
                            top.errCodes.push(msg_arr[0] + ' - ' + msg_arr[2]);

                            if (_page.CallBack) {
                                eval(_page.CallBack);
                            }
                        }
                        else {
                            ageMsg.Show(msg_arr[0]);
                        }
                    }
                }
            }
        }

        // Post with return result in JSON format
        if (confirm(warning)) {
            age.DelayReloadPage(null, 1000000);
            if (counter == 1) {
                params += '&custid=' + custid;
                params += '&isclose=' + _page.status;
                params += '&oldamount=' + amt;
                params += '&issynccasino=' + isSync;
                params += '&disablecasino=0';
                params += '&txtamount=' + amt;

                ajax.PostJSON(
                    '../../_Transfer/Handler/SingleTransfer_Member.ashx',
                    params,
                    OnComplete
                );
            }
            else {
                params += '&ischeck=false';
                params += '&custid=' + custIdList;
                params += '&custidSync=' + custidSync;
                params += '&status=' + _page.status;
                params += '&totalPage=' + _page.totalPage;
                params += '&pagesize=' + _page.pageSize;
                params += '&username=' + _page.userName;
                params += '&isYesterdayBalance=' + _page.isYesterdayBalance;
                ajax.PostJSON(
                    '../../_Transfer/Handler/MultipleTransfer_Member.ashx',
                    params,
                    OnComplete
                );
            }
        }
        custIdList = '';
        custidSync = '';
        $("chkall").checked = false;
        for (var i = 0; i < checkID.length; i++) {
            if (checkID[i].checked == true) {
                checkID[i].checked = false;
            }
        }
    }
}

function TransferAll() {
    var warning = "";
    var checkID = document.getElementsByName('chkitem');
    var custidSync = "";
    var counter = 0;
    if (!checkID) return;

    var txtname = $('txtUserName').value;
    if (txtname == _page.userNameDefault) {
        txtname = '';
    }
    var params = ajax.CreateParams();

    params += '&ischeck=true';
    params += '&custid=0';
    params += '&custidSync=' + _page.cusidSyncAll;
    params += '&status=' + _page.status;
    params += '&totalPage=' + _page.totalPage;
    params += '&pagesize=' + _page.pageSize;
    params += '&username=' + txtname;
    params += '&isYesterdayBalance=' + _page.isYesterdayBalance;

    function OnComplete(result) {
        var errCode = result.errCode;
        if (errCode == 0) {
            if (_page.CallBack) {
                eval(_page.CallBack);
            }
        }
        else if (errCode == '106') {
            showErrMsg(_page.lblconfirmclosesubacc, 0);
        }
        else if (errCode == '105') // kick out
        {
            alert(_page.lblConfirmClosed);
            location.href = '../Authorization/Logout.ashx';
        }
        else {
            if (result.errMsg.search('_') == -1) {
                ageMsg.Show(result.errMsg);
            }
            else {
                var msg_arr = result.errMsg.split('_');
                if (msg_arr[1] == 'true') {
                    top.errCodes.push(msg_arr[0]);

                    if (_page.CallBack) {
                        eval(_page.CallBack);
                    }
                }
                else {
                    ageMsg.Show(msg_arr[0]);
                }
            }
        }
    }
    for (var i = 0; i < checkID.length; i++) {
        var arrPara = checkID[i].value.split('_');

        if (arrPara[2] == 1 && arrPara[3] == "False") {
            custidSync += arrPara[1] + ",";
        }
    }

    if (custidSync != "") {
        custidSync = custidSync.substring(0, custidSync.length - 1);
        warning = _page.wrnResetMaxTransfer;
    }
    else {
        warning = _page.wrnTransfer;
    }
    // Post with return result in JSON format
    if (confirm(warning)) {
        age.DelayReloadPage(null, 1000000);
        ajax.PostJSON(
                    '../../_Transfer/Handler/MultipleTransfer_Member.ashx',
                    params,
                    OnComplete
                );
    }

    $("chkall").checked = false;
    for (var i = 0; i < checkID.length; i++) {
        if (checkID[i].checked == true) {
            checkID[i].checked = false;
        }
    }
}