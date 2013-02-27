function changeCasinoStatus(evt, objDiv, hasDownLine) {
    if (objDiv.childNodes[1].disabled) return _stopEvent(evt);
    var isOpenPopup = false;
    var isDisabled = $('chkCasino').checked;
    if (isDisabled) //enableCS currently, wanna disableCS
    {
        if (typeof hasDownLine != 'undefined' && hasDownLine) { //for agent
            if (!confirm(_page.strConfirmDisableCasino)) {
                return _stopEvent(evt);
            }
            $('chkEnabled').checked = false;
            $('chkEnabledDL').checked = false;
            updateCustomerCasinoStatus(true, true);
        }
        else { //for member
            if (!confirm(_page.lblAlertDisableCasinoMember)) {
                return _stopEvent(evt);
            }
            updateCustomerCasinoStatus(true, false);
        }
    }
    else //disableCS currently, wanna enableCS
    {
        if (typeof hasDownLine != 'undefined' && hasDownLine) { //for agent
            showCasinoPopup();
            return _stopEvent(evt);
        }
        else { //for member
            if (!confirm(_page.strConfirmEnableCS)) {
                return _stopEvent(evt);
            }
            updateCustomerCasinoStatus(false, false);
        }
    }

    function showCasinoPopup() {
        // fill data for popup
        assignDefaultPopupValue();
        $('hdnTypeStatus').value = 'CS'; //type popup
        $('lblEnableStatus').innerHTML = _page.lblEnableCS; //title popup
        $('lbldownline').innerHTML = _page.casinodownlines;
        $('btnUpdStatus').onclick = function() {
            var isDisabled = !$('chkEnabled').checked;
            var isUpdateDownline = $('chkEnabledDL').checked; // chkEnabledDL = true => isUpdateDownline = true
            updateCustomerCasinoStatus(isDisabled, isUpdateDownline);
        };
        isOpenPopup = true;
        showDivPopup('chkCasino');
    }

    function updateCustomerCasinoStatus(isDisabled, isUpdateDownline) {
        ajax.PostJSON('Handlers/UpdateCasinoStatus.ashx', 'custid=' + _page.UserId + '&isdisabled=' + isDisabled + '&issynccs=' + _page.isSyncCasino + '&isupdatedownline=' + isUpdateDownline, onComplete);

        function onComplete(result) {
            if (result.errCode == 0) {
                $('chkCasino').checked = !isDisabled;
            }
            else alert(result.errMsg);
            if (isOpenPopup) closeDivPopup();
        }
    }
}