// Set or unset double commission for a user
// newStatus = 0 => unset double commission
// newStatus = 1 => set double commission
function SetDoubleCommission(custid, el, hasDownline) {

    var newStatus = 2;
    var confirmMessage = "";

    if (el.parentNode.className == "bkgDcommAllowed") { // Currently allowed
        newStatus = 0;
        if (typeof hasDownline == 'undefined') { // Member
            confirmMessage = _page.lblAlertDisableDoubleCommMember;
        }
        else { // Super, Master, Agent
            confirmMessage = _page.strConfirmDisableDoubleComm;
        }
    }
    else if (el.parentNode.className == "bkgDcommDisallowed") { // Currently not allowed
        newStatus = 1;
        if (typeof hasDownline == 'undefined') { // Member
            confirmMessage = _page.strConfirmEnableDoubleCommMember;
        }
        else { // Super, Master, Agent
            confirmMessage = _page.strConfirmEnableDoubleComm;
        }
    }

    if (newStatus != 2 && confirmMessage != "") {
        if (confirm(confirmMessage)) {
            UpdateDoubleCommStatus(custid, newStatus, el);
        }
    }
}

function UpdateDoubleCommStatus(custid, status, el) {
    ajax.PostJSON(age.GetBaseUrl() + '_AccountInfo/Handlers/UpdateDoubleCommStatus.ashx', 'custid=' + custid + '&doubleCommLosing=' + status, onComplete);

    function onComplete(result) {
        if (result.errCode == 0) {
            age.DelayReloadPage(window.location.href, 2000);
        } else {
            alert(result.errMsg);
            age.DelayReloadPage(window.location.href, 0);
        }        
    }
}