function UpdCredit() {
    ajax.PostJSON(
        '../../_AccountInfo/Handlers/UpdateGivenCredit.ashx',
        'custid=' + $('custid').value + '&custRoleId=' + $('roleId').value + '&amount=' + $('txtamout').value,
        onComplete
    );
    
    function onComplete(result) {
        if (result.errCode == 0) {
            parent.DelayReloadPage();
            parent.ageWnd.close();
        }
        else {
            $('txtamout').focus();
            ageMsg.Show(result.errMsg);
            AddPopupHeight(50);
        }
    }
}

function OnkeyUpAmt(e) {
    var isok = true;
    if (!e) e = window.event;
    var key = (e.keyCode) ? e.keyCode : e.which;

    if ((key < 48 || key > 57) && key != 8 && key != 13 && key != 0 && (key < 37 || key > 40)) {
        return false;
    }
    else {
        if (key == 13) {
            UpdCredit();
            return false;
        }
    }
}