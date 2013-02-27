function changeNumberGameStatus(evt, objDiv, hasDownLine) {

    if (objDiv.childNodes[1].disabled) return _stopEvent(evt);

    var isOpenPopup = false;
    var isDisabled = $('chkNumberGame').checked;
    if (isDisabled) //enable number game currently, wanna disable number game
    {
        if (typeof hasDownLine != 'undefined' && hasDownLine) { //for agent
            if (!confirm(_page.strConfirmDisableNumber)) {
                return _stopEvent(evt);
            }
            $('chkEnabled').checked = false;
            $('chkEnabledDL').checked = false;
            updateNumberGameStatus(true, true);
        } else { //for member
            if (!confirm(_page.lblAlertDisableNumberMember)) {
                return _stopEvent(evt);
            }
            updateNumberGameStatus(true, false);
        }
    } else //disable number game currently, wanna enable number game
    {
        if (typeof hasDownLine != 'undefined' && hasDownLine) { //for agent
            showNumberGamePopup();
            return _stopEvent(evt);
        } else { //for member
            if (!confirm(_page.strConfirmEnableNumber)) {
                return _stopEvent(evt);
            }
            updateNumberGameStatus(false, false);
        }
    }

    function showNumberGamePopup() {
        // fill data for popup
        assignDefaultPopupValue();
        $('hdnTypeStatus').value = 'Number'; //type popup
        $('lblEnableStatus').innerHTML = _page.lblEnableNumber; //title popup
        $('lbldownline').innerHTML = _page.Numberdownlines;
        $('btnUpdStatus').onclick = function() {
            var isDisabled = !$('chkEnabled').checked; // chkEnabled = true => isDisabled    =false;
            var isDisableDownline = !$('chkEnabledDL').checked; //chkEnabledDL = true => isDisableDownline =false
            updateNumberGameStatus(isDisabled, isDisableDownline);
        };
        isOpenPopup = true;
        showDivPopup('chkNumberGame');
    }

    function updateNumberGameStatus(isDisabled, isDisableDownline) {
        ajax.PostJSON('Handlers/UpdateNumberStatus.ashx', 'custid=' + _page.UserId + '&isdisabled=' + isDisabled + '&disabledownline=' + isDisableDownline, onComplete);

        function onComplete(result) {
            if (result.errCode == 0) {
                $('chkNumberGame').checked = !isDisabled;
            } else alert(result.errMsg);
            if (isOpenPopup) closeDivPopup();
        }
    }
}