function changeVirtualSportsStatus(evt, hasDownLine) {
    if (document.getElementById('chkVirtualSports').disabled) return _stopEvent(evt);

    var isOpenPopup = false;
    var isDisabled = document.getElementById('chkVirtualSports').checked;
    if (isDisabled) {
        if (typeof hasDownLine != 'undefined' && hasDownLine) { //for agent
            if (!confirm(_page.strConfirmDisableVirtualSports)) {
                return _stopEvent(evt);
            }
            document.getElementById('chkEnabled').checked = false;
            document.getElementById('chkEnabledDL').checked = false;
            updateVirtualSportsStatus(true, true);
        } else { //for member
            if (!confirm(_page.lblAlertDisableVirtualSportsMember)) {
                return _stopEvent(evt);
            }
            updateVirtualSportsStatus(true, false);
        }
    } else {
        if (typeof hasDownLine != 'undefined' && hasDownLine) { //for agent
            showVirtualSportsPopup();
            return _stopEvent(evt);
        } else { //for member
            if (!confirm(_page.strConfirmEnableVirtualSports)) {
                return _stopEvent(evt);
            }
            updateVirtualSportsStatus(false, false);
        }
    }

    function showVirtualSportsPopup() {
        // fill data for popup
        assignDefaultPopupValue();
        document.getElementById('hdnTypeStatus').value = 'VirtualSports'; //type popup
        document.getElementById('lblEnableStatus').innerHTML = _page.lblEnableVirtualSports; //title popup
        document.getElementById('lbldownline').innerHTML = _page.VirtualSportsDownlines;
        document.getElementById('btnUpdStatus').onclick = function () {
            var isDisabled = !document.getElementById('chkEnabled').checked; // chkEnabled = true => isDisabled    =false;
            var isDisableDownline = !document.getElementById('chkEnabledDL').checked; //chkEnabledDL = true => isDisableDownline =false
            updateVirtualSportsStatus(isDisabled, isDisableDownline);
        };
        isOpenPopup = true;
        showDivPopup('chkVirtualSports');
    }

    function updateVirtualSportsStatus(isDisabled, isDisableDownline) {
        ajax.PostJSON('Handlers/UpdateVirtualSportsStatus.ashx', 'custid=' + _page.UserId + '&isdisabled=' + isDisabled + '&disabledownline=' + isDisableDownline, onComplete);

        function onComplete(result) {
            if (result.errCode == 0) {
                document.getElementById('chkVirtualSports').checked = !isDisabled;
            } else alert(result.errMsg);
            if (isOpenPopup) closeDivPopup();
        }
    }
}