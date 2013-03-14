/* File Created: March 14, 2013 */

var editSetting =
{
    SaveSetting: function () {
        if (memberBetLimit.BetLimitElement.Validate()) {
            var params = "settingContext=" + JSON.stringify(memberBetLimit.SettingContext) +
                         "&betLimits=" + JSON.stringify(memberBetLimit.BetLimitElement.Save()) +
                         "&commissionSetting=" + JSON.stringify(editCommission.CommissionSettingElement.Save()) +
                         "&positionTakingSetting=" + JSON.stringify(editPositionTaking.PositionTakingElement.Save());
            var submitUrl = "../../CustomerSetting/Handlers/UpdateSetting.ashx";
            var onCompleteCallback = null;
            age.ReloadPage(600000);
            ajax.PostJSON(
        				submitUrl,
        				params,
        				memberBetLimit.CompleteUpdate
        			);
        }
    },
    CompleteUpdate: function (result) {
        var errMsg = result.errMsg;
        if (result.errCode == 0) {
            age.UnLock();
            AddPopupHeight(50);
            ageMsg.Show(errMsg, 1);
            window.scroll(0, 0);
            setTimeout(function () {
                parent.window.location.reload();
            }, 1000);

            return;
        }
        else {
            age.UnLock();
            AddPopupHeight(50);
            ageMsg.Show(errMsg);
            window.scroll(0, 0);
            return;
        }
    }
}