

var TransferConditionModel = function () {
    // Check Transfer condition
    displayTransferCondition = true;

    this.saveCustomer = function () {

        if (!Fanex.Customer.ValidationTransferCondition()) {
            return false;
        }

        if (confirm(Language.ALERT_TRANSFER_CONDITION_DOWNLINE)) {
            var transferOption = '';
            if (transfer && transfer.length > 0) {
                if ($('rdWeekly').checked == true) {
                    transferOption = Fanex.Customer.TransferOption(transfer.split(""));
                }
                else {
                    transferOption = '00000000';
                }
            }
            
            var params = ajax.CreateParams();
            params += '&transferOption=' + transferOption;

            function OnComplete(result) {
                var errMsg = result.errMsg;
                $('w20px').style.display = 'none';
                if (result.errCode == 0) {
                    age.UnLock();
                    ageMsg.Show(errMsg, 1);
                    window.scroll(0, 0);
                    age.ReloadPage(500);
                    return;
                }
                else {
                    age.UnLock();
                    ageMsg.Show(errMsg);
                    window.scroll(0, 0);
                    return;
                }
            }

            age.ReloadPage(30000);
            var submitUrl = "Handlers/UpdateTransferCondition.ashx?targetCustId=" + context.targetCustId;

            ajax.PostJSON(
				submitUrl,
				params,
				OnComplete
			);
        }
    }
}

RegisterStartUp(function () {
    if (transfer && transfer.length > 0) {
        var arrTransferOption = transfer.split("");
        var isDaily = false;

        var arrWeekly = new Array("chkSun", "chkMon", "chkTue", "chkWed", "chkThurs", "chkFri", "chkSat");

        for (var i = 0; i < arrTransferOption.length - 1; i++) {
            if (arrTransferOption[i] == 1) {
                document.getElementById(arrWeekly[i]).checked = true;
                if (isDaily == false) {
                    isDaily = true;
                }
            }
        }

        if (isDaily == true) {
            $("rdWeekly").checked = true;
            for (var i = 0; i < arrWeekly.length; i++) {
                document.getElementById(arrWeekly[i]).disabled = false;
            }
        }
    };

    if ((parent.ageWnd == null) && (typeof ($('header_main')) != 'undefined')) {
        $('w20px').style.display = 'none';
    }

    Fanex.Customer.HideTitle();
});

ko.applyBindings(new TransferConditionModel());