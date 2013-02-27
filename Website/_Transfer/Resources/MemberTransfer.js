function OnkeyUpAmtforMember(e) {
    var isok = true;
    if (!e) e = window.event;
    var key = (e.keyCode) ? e.keyCode : e.which;

    if ((key < 48 || key > 57) && key != 8 && key != 13 && key != 45 && key != 46 && key != 0 && (key < 37 || key > 40)) {
        return false;
    }
    else {
        if (key == 13) {
            UpdYesBalanceMember();
            return false;
        }
    }
}

function UpdYesBalanceMember() {
    var warning = "";
    var params = ajax.CreateParams();
    arrAmount = document.getElementsByName('txtamount');

    if (!isNumeric(arrAmount[0].value)) {
        ageMsg.Show(_page.invalidValue);
        AddPopupHeight(50);
        return false;
    }
    params += '&custid=' + _page.custId;
    //params += '&isclose=' + _page.isClose;
    params += '&oldamount=' + _page.amt;
    params += '&issynccasino=' + _page.isSyncCasino;
    params += '&disablecasino=' + _page.disableCasino;
    params += '&txtamount=' + arrAmount[0].value;
    function OnComplete(result) {
        age.UnLock();
        var errCode = result.errCode;
        if (errCode == 0) {
            if (_page.CallBack != 'false') {
                eval(_page.CallBack);
            }
            else {
                alert(_page.wrntransfersuccessful);
                age.DelayReloadPage();
                top.menu.checkAndSubmitUI();
            }
        }
        else {
            ageMsg.Show(result.errMsg);
            AddPopupHeight(50);
        }
    }
    if (_page.isSyncCasino && _page.amt == arrAmount[0].value) {
        warning = _page.wrnResetMaxTransfer;
    }
    else {
        warning = _page.wrnTransfer;
    }
    // Post with return result in JSON format
    if (confirm(warning)) {
        age.Lock(false);
        ajax.PostJSON(
            '../../_Transfer/Handler/SingleTransfer_Member.ashx',
            params,
            OnComplete
        );
    }
    age.UnLock();
}

function InitMemberTransfer() {
    if (parent.ageWnd) { // In pop up
    }
    else {
        // Collapse layout
        $('page_popup_agent').style.width = '400px';
        document.body.style.backgroundImage = 'url("../../_GlobalResources/Images/bg_conts.jpg")';
    }
}

RegisterStartUp('InitMemberTransfer()');