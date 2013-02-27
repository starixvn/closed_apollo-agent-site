function updateCustomerInfo(custId) {
    //    custid, firstName, lastName, phone, mobilePhone, email, fax
    //    var div = $(divId);
    var txtFName = $('txtFName');
    var txtLName = $('txtLName');
    var txtPhone = $('txtPhone');
    var txtMobile = $('txtMobile');
    var txtEmail = $('txtEmail');
    var txtFax = $('txtFax');
    var isSyncCS = _page.isSync;
    var errMsg = CheckValidateEmail(txtEmail.value);

    if (null != errMsg) {
        ageMsg.Show(errMsg);
        AddPopupHeight(50);
        return;
    }

//    var params = new Array('custId=' + custId, 'firstName=' + firstName, 'lastName=' + lastName, 'phone=' + phone, 'mobile=' + mobile, 'email=' + email, 'fax=' + fax, 'isSyncCS=' + isSyncCS);

    var params = ajax.CreateParams('txtFName', 'txtLName', 'txtPhone', 'txtMobile', 'txtEmail', 'txtFax');
    params += '&custId=' + custId + '&isSyncCS=' + isSyncCS;
    // Post with return result in JSON format

    function OnComplete(result) {
        age.UnLock();
        var errCode = result.errCode;
        AddPopupHeight(50);
        if (errCode == 0) {
          ageMsg.Hide();
          ageMsg.ShowMsgs(_page.updatesuccessfull, 1);
          return;
        } else {
            ageMsg.Show(result.errMsg);
        }
    }

    ajax.PostJSON(age.GetBaseUrl() + '_AccountInfo/Handlers/UpdateMemberinfo.ashx', params, OnComplete);
}

function CheckValidateEmail(email) {
    if (email != '' && !validateEmail(email)) {
        return _page.emailnotvalid;
        return false;
    }
}

function validateEmail(email) {
    var emailRegEx = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return email.match(emailRegEx);
}

function Reset() {
    $('txtFName').value = '';
    $('txtLName').value = '';
    $('txtPhone').value = '';
    $('txtMobile').value = '';
    $('txtEmail').value = '';
    $('txtFax').value = '';
}

function CustomerInfoOnLoad() {
    if ((parent.ageWnd != null) && (typeof ($('title_header')) != 'undefined')) {
        $('title_header').style.display = 'none';
    }
}

RegisterStartUp('CustomerInfoOnLoad()');