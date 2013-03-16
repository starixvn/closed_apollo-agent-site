function changeNumberGameStatus(evt, objDiv, hasDownLine) {

    if (objDiv.childNodes[1].disabled) return _stopEvent(evt);

    var isOpenPopup = false;
    var isDisabled = document.getElementById('chkNumberGame').checked;
    if (isDisabled) //enable number game currently, wanna disable number game
    {
        if (typeof hasDownLine != 'undefined' && hasDownLine) { //for agent
            if (!confirm(_page.strConfirmDisableNumber)) {
                return _stopEvent(evt);
            }
            document.getElementById('chkEnabled').checked = false;
            document.getElementById('chkEnabledDL').checked = false;
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
        document.getElementById('hdnTypeStatus').value = 'Number'; //type popup
        document.getElementById('lblEnableStatus').innerHTML = _page.lblEnableNumber; //title popup
        document.getElementById('lbldownline').innerHTML = _page.Numberdownlines;
        document.getElementById('btnUpdStatus').onclick = function() {
            var isDisabled = !document.getElementById('chkEnabled').checked; // chkEnabled = true => isDisabled    =false;
            var isDisableDownline = !document.getElementById('chkEnabledDL').checked; //chkEnabledDL = true => isDisableDownline =false
            updateNumberGameStatus(isDisabled, isDisableDownline);
        };
        isOpenPopup = true;
        showDivPopup('chkNumberGame');
    }

    function updateNumberGameStatus(isDisabled, isDisableDownline) {
        ajax.PostJSON('Handlers/UpdateNumberStatus.ashx', 'custid=' + _page.UserId + '&isdisabled=' + isDisabled + '&disabledownline=' + isDisableDownline, onComplete);

        function onComplete(result) {
            if (result.errCode == 0) {
                document.getElementById('chkNumberGame').checked = !isDisabled;
            } else alert(result.errMsg);
            if (isOpenPopup) closeDivPopup();
        }
    }
}