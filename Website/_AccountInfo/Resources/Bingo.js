function changeBingoStatus(evt, objDiv, hasDownLine) {
    if (objDiv.childNodes[1].disabled) return _stopEvent(evt);
    var isOpenPopup = false;
    var isDisabled = $('chkBingo').checked;
    if (isDisabled) //enable bingo currently, wanna disable bingo
    {
        if (typeof hasDownLine != 'undefined' && hasDownLine) { //for agent
            if (!confirm(_page.strConfirmDisableBingo)) {
                return _stopEvent(evt);
            }
            $('chkEnabled').checked = false;
            $('chkEnabledDL').checked = false;
            updateBingoStatus(true, true);
        }
        else { //for member
            if (!confirm(_page.lblAlertDisableBingoMember)) {
                return _stopEvent(evt);
            }
            updateBingoStatus(true, false);
        }
    }
    else //disable bingo currently, wanna enable bingo
    {
        if (typeof hasDownLine != 'undefined' && hasDownLine) { //for agent
            showBingoPopup();
            return _stopEvent(evt);
        }
        else { //for member
            if (!confirm(_page.strConfirmEnableBingo)) {
                return _stopEvent(evt);
            }
            updateBingoStatus(false, false);
        }
    }

    function showBingoPopup() {
        // fill data for popup
        assignDefaultPopupValue();
        $('hdnTypeStatus').value = 'Bingo'; //type popup
        $('lblEnableStatus').innerHTML = _page.lblEnableBingo; //title popup
        $('lbldownline').innerHTML = _page.casinodownlines;
        $('btnUpdStatus').onclick = function() {
            var isDisabled = !$('chkEnabled').checked;
            var isEnableDownline = $('chkEnabledDL').checked; // chkEnabledDL = true => isEnableDownline =true
            updateBingoStatus(isDisabled, isEnableDownline);
        };
        isOpenPopup = true;
        showDivPopup('chkBingo');
    }

    function updateBingoStatus(isDisabled, isEnableDownline) {
        ajax.PostJSON('Handlers/UpdateBingoStatus.ashx', 'custid=' + _page.UserId + '&isdisabled=' + isDisabled + '&issyncbingo=' + _page.isSyncBingo + '&enabledownline=' + isEnableDownline, onComplete);

        function onComplete(result) {
            if (result.errCode == 0) {
                $('chkBingo').checked = !isDisabled;
            }
            else alert(result.errMsg);
            if (isOpenPopup) closeDivPopup();
        }
    }
}