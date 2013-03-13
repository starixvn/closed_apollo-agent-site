/* File Created: March 13, 2013 */


var memberBetLimit =
{
    SettingContext: null,
    BetLimitElement: null,
    BetLimitItems: null,
    Init: function (settingContext, betLimits) {
        memberBetLimit.SettingContext = settingContext;
        Fanex.CustSetting.CURRENT_CONTEXT = new Fanex.CustSetting.Context(settingContext);
        memberBetLimit.BetLimitItems = betLimits;
        memberBetLimit.BetLimitElement = new Fanex.CustSetting.BettingLimit(betLimits);
        var bettingLimitContainer = document.getElementById('bettingLimitContainer');
        bettingLimitContainer.innerHTML = "";
        memberBetLimit.BetLimitElement.Render(bettingLimitContainer);
    },
    UpdateMemberBetLimit: function () {
        if (memberBetLimit.BetLimitElement.Validate()) {
            var params = "settingContext=" + JSON.stringify(memberBetLimit.SettingContext) +
                         "&betLimits=" + JSON.stringify(memberBetLimit.BetLimitElement.Save());
            var submitUrl = "../../CustomerSetting/Handlers/UpdateBetLimit.ashx";
            var onCompleteCallback = null;
            age.ReloadPage(3000);
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
};

////
memberBetLimit.Init(settingContext, betLimits);

