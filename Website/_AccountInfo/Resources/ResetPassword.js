/*
* Created 20120727@Terry, Lee - Javascript functions for reset password
* Revision ?@? - ... 
* 
*/

function ChangePassword() {
    // Validate password
    var txtPwd = $('password');
    var txtConfirmPwd = $('confirmPassword');

    var errMsg = null;

    if (txtPwd.value.length == 0) {
        errMsg = _page.errPasswordEmpty;
        AddPopupHeight(50);
        txtPwd.focus();
    }
    else if (txtPwd.value != txtConfirmPwd.value) {
        errMsg = _page.errConfirm;
        AddPopupHeight(50);
        txtConfirmPwd.focus();
    }
    else {
        errMsg = ValidatePassword(txtPwd.value);
        txtPwd.focus();
    }

    if (null != errMsg) {
        ageMsg.Show(errMsg);
        AddPopupHeight(50);
        return;
    }

    var params = ajax.CreateParams();
    params += '&password=' + txtPwd.value;
    params += '&custId=' + _page.custId;
    params += '&isSyncCasino=' + _page.isSyncCasino;

    function OnComplete(result) {
        age.UnLock();
        var errCode = result.errCode;
        AddPopupHeight(50);
        if (errCode == 0) {
            ageMsg.Show(_page.infoSuccess, true);
            age.ReloadPage(500);
        }
        else {           
            txtPwd.focus();
            ageMsg.Show(result.errMsg);
        }
    }

    // Post with return result in JSON format
    age.Lock(false);
    ajax.PostJSON(
        'Handlers/ResetPassword.ashx',
        params,
        OnComplete
    );
}

function Reset() {
    $('password').value = '';
    $('confirmPassword').value = '';
     $('password').focus();
}


// Check pass call in keyup event
function CheckPass(pwdElement, iconElementId) {
    var isPwd = IsPassword(pwdElement.value);
    var iconElement = $(iconElementId);

    if (!iconElement) return false;

    if (isPwd) {
        iconElement.className = 'PwdSuccess';
    }
    else {
        iconElement.className = 'PwdError';
    }

    return isPwd;
}

function ValidatePassword(str) {
    if (str.length == 0) return _page.errPasswordEmpty;
    else if (!IsPassword(str)) return _page.errPasswordRequire;

    return null;
}


function ChangePasswordOnLoad() {
    if (_page.isPopup == true) {
        $('header_main').style.display = 'none';
    }
}

RegisterStartUp('ChangePasswordOnLoad()');