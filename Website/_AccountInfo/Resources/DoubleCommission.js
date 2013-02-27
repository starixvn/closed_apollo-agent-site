function changeDoubleCommStatus(evt, hasDownLine) {
    if ($('chkDoubleComm').disabled) return _stopEvent(evt);

    var isOpenPopup = false;
    var isDisabled = $('chkDoubleComm').checked;
    if (isDisabled) {
        if (typeof hasDownLine != 'undefined' && hasDownLine) { //for agent
            if (!confirm(_page.strConfirmDisableDoubleComm)) {
                return _stopEvent(evt);
            }
            updateDoubleCommStatus(false);
        } else { //for member
            if (!confirm(_page.lblAlertDisableDoubleCommMember)) {
                return _stopEvent(evt);
            }
            updateDoubleCommStatus(false);
        }
    } else {
        if (typeof hasDownLine != 'undefined' && hasDownLine) { //for agent
            if (!confirm(_page.strConfirmEnableDoubleComm)) {
                return _stopEvent(evt);
            }
            updateDoubleCommStatus(true);
        } else { //for member
            if (!confirm(_page.strConfirmEnableDoubleCommMember)) {
                return _stopEvent(evt);
            }
            updateDoubleCommStatus(true);
        }
    }

    function updateDoubleCommStatus(doubleCommLosing) {
        ajax.PostJSON('Handlers/UpdateDoubleCommStatus.ashx', 'custid=' + _page.UserId + '&doubleCommLosing=' + doubleCommLosing, onComplete);

        function onComplete(result) {
            if (result.errCode == 0) {
                $('chkDoubleComm').checked = doubleCommLosing;
            } else alert(result.errMsg);
            if (isOpenPopup) closeDivPopup();
        }
    }
}

// Split url custID list and set double commission on each one

function MultiUpdateDComm() {
    var newStatus = null;

    if ($('dcomAllow').checked == true) {
        newStatus = 1;
    }
    else if ($('dcomDisallow').checked == true) {
        newStatus = 0;
    }

    if (newStatus != null) {
        var lstCustID = GetUrlParm(window.location.href, 'arCID');
        if (lstCustID != null) {
            var arrCustID = lstCustID.split('^').slice(0, -1);
            age.ShowMaskDiv(true);
            UpdateDoubleCommStatus(arrCustID, newStatus);            
        }
    }
}

// Ajax

function UpdateDoubleCommStatus(arrCustID, status) {
    if (arrCustID != null && arrCustID.length > 0) {
        var custid = arrCustID.shift();
        ajax.PostJSON(age.GetBaseUrl() + '_AccountInfo/Handlers/UpdateDoubleCommStatus.ashx', 'custid=' + custid + '&doubleCommLosing=' + status, function (result) {
            if (result.errCode != 0) {
                alert(result.errMsg);
                age.ReloadParentPage(parent.location.pathname + parent.location.search, 4000);
                return;
            }
            UpdateDoubleCommStatus(arrCustID, status);
        });
    }
    else {
        age.ReloadParentPage(parent.location.pathname + parent.location.search, 4000);
    }
}

// Get url parameter by name

function GetUrlParm(url, parm) {
    var re = new RegExp(".*[?&]" + parm + "=([^&]+)(&|$)");
    var match = url.match(re);
    return (match ? match[1] : "");
}