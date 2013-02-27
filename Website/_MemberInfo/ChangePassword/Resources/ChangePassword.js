/*
* Created 20100226@Terry, Lee - Javascript functions for change password
* Revision ?@? - ... 
* 
*/


function ChangePassword() {
    // Validate password
    var txtOldPwd = $('txtOldPwd');
    var txtNewPwd = $('txtNewPwd');
    var txtConfirmPwd = $('txtConfirmPwd');

    var errMsg = null;

    if (txtOldPwd.value.length == 0) {
        errMsg = _page.alertInputOldPwd;
        txtOldPwd.focus();
    }
    else if (txtNewPwd.value != txtConfirmPwd.value) {
        errMsg = _page.alertErrorConfirm;
        txtNewPwd.focus();
    }
    else {
        errMsg = ValidatePassword(txtNewPwd.value);
        txtNewPwd.focus();
    }

    if (null != errMsg) {
        ageMsg.Show(errMsg);
        return;
    }

    var params = ajax.CreateParams('txtNewPwd', 'txtOldPwd', 'txtConfirmPwd');

    function OnComplete(result) {
        age.UnLock();
        var errCode = result.errCode;
        if (errCode == 0) {
            ageMsg.Show(_page.alertPasswordSucc, true);
            if (_page.firstLogged) {
                setTimeout('age.Refresh()', 2000);
            }
        }
        else {
            txtOldPwd.focus();
            ageMsg.Show(result.errMsg);
        }
    }

    // Post with return result in JSON format
    age.Lock(false);
    ajax.PostJSON(
        'Handlers/ChangePassword.ashx',
        params,
        OnComplete
    );
}

function Reset() {
    $('txtOldPwd').value = '';
    $('txtNewPwd').value = '';
    $('txtConfirmPwd').value = '';
}


// Check pass call in keyup event
function CheckPass(pwdElement, iconElementId)
{
    var isPwd = IsPassword(pwdElement.value);
    var iconElement = $(iconElementId);
    
    if(!iconElement) return false;
    
    if(isPwd)
    {
        iconElement.className = 'PwdSuccess';
    }
    else
    {
        iconElement.className = 'PwdError';
    }
    
    return isPwd;
}

function ValidatePassword(str) {
    if (str.length == 0) return _page.alertPasswordEmpty;
    else if (!IsPassword(str)) return _page.alertPasswordRequire;

    return null;
}


function ChangePasswordOnLoad()
{
    // Set active tab
    if(top.frHeader && top.frHeader.SelectTopMenu)
    {
        top.frHeader.SelectTopMenu('ChangePassword.aspx');
    }
    
    // Show first logged message
    if(_page.firstLogged)
    {
        ageMsg.Show(_page.forcechangepwd);
    }
}

RegisterStartUp('ChangePasswordOnLoad()');