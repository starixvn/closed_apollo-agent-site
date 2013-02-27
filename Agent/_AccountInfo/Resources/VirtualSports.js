function changeVirtualSportsStatus(evt, hasDownLine) {
    if ($('chkVirtualSports').disabled) return _stopEvent(evt);

    var isOpenPopup = false;
    var isDisabled = $('chkVirtualSports').checked;
    if (isDisabled) {
        if (typeof hasDownLine != 'undefined' && hasDownLine) { //for agent
            if (!confirm(_page.strConfirmDisableVirtualSports)) {
                return _stopEvent(evt);
            }
            $('chkEnabled').checked = false;
            $('chkEnabledDL').checked = false;
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
        $('hdnTypeStatus').value = 'VirtualSports'; //type popup
        $('lblEnableStatus').innerHTML = _page.lblEnableVirtualSports; //title popup
        $('lbldownline').innerHTML = _page.VirtualSportsDownlines;
        $('btnUpdStatus').onclick = function () {
            var isDisabled = !$('chkEnabled').checked; // chkEnabled = true => isDisabled    =false;
            var isDisableDownline = !$('chkEnabledDL').checked; //chkEnabledDL = true => isDisableDownline =false
            updateVirtualSportsStatus(isDisabled, isDisableDownline);
        };
        isOpenPopup = true;
        showDivPopup('chkVirtualSports');
    }

    function updateVirtualSportsStatus(isDisabled, isDisableDownline) {
        ajax.PostJSON('Handlers/UpdateVirtualSportsStatus.ashx', 'custid=' + _page.UserId + '&isdisabled=' + isDisabled + '&disabledownline=' + isDisableDownline, onComplete);

        function onComplete(result) {
            if (result.errCode == 0) {
                $('chkVirtualSports').checked = !isDisabled;
            } else alert(result.errMsg);
            if (isOpenPopup) closeDivPopup();
        }
    }
}