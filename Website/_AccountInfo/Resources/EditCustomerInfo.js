function updateCustomerInfo(custId) {
    //    custid, firstName, lastName, phone, mobilePhone, email, fax
    //    var div = document.getElementById(divId);
    var txtFName = document.getElementById('txtFName');
    var txtLName = document.getElementById('txtLName');
    var txtPhone = document.getElementById('txtPhone');
    var txtMobile = document.getElementById('txtMobile');
    var txtEmail = document.getElementById('txtEmail');
    var txtFax = document.getElementById('txtFax');
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
    document.getElementById('txtFName').value = '';
    document.getElementById('txtLName').value = '';
    document.getElementById('txtPhone').value = '';
    document.getElementById('txtMobile').value = '';
    document.getElementById('txtEmail').value = '';
    document.getElementById('txtFax').value = '';
}

function CustomerInfoOnLoad() {
    if ((parent.ageWnd != null) && (typeof (document.getElementById('title_header')) != 'undefined')) {
        document.getElementById('title_header').style.display = 'none';
    }
}

RegisterStartUp('CustomerInfoOnLoad()');