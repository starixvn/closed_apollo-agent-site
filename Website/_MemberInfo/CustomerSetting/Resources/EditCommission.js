/* File Created: March 13, 2013 */


var editCommission =
{
    SettingContext: null,
    CommissionSettingElement: null,
    CommissionSetting: null,
    Init: function (settingContext, customerCommission) {

        editCommission.SettingContext = settingContext;
        Fanex.CustSetting.CURRENT_CONTEXT = new Fanex.CustSetting.Context(settingContext);
        editCommission.CommissionSetting = customerCommission;
        editCommission.CommissionSettingElement = new Fanex.CustSetting.CommissionSetting(customerCommission);
        var commissionSettingContainer = document.getElementById('commissionSettingContainer');
        commissionSettingContainer.innerHTML = "";
        editCommission.CommissionSettingElement.Render(commissionSettingContainer);
    },
    UpdateCommissionSetting: function () {
            var params = "settingContext=" + JSON.stringify(editCommission.SettingContext) +
                         "&commissionSetting=" + JSON.stringify(editCommission.CommissionSettingElement.Save());
            var submitUrl = "../../CustomerSetting/Handlers/UpdateCommissionSetting.ashx";
            var onCompleteCallback = null;
            age.ReloadPage(600000);
            ajax.PostJSON(
        				submitUrl,
        				params,
        				editCommission.CompleteUpdate
        			);
    },
    CompleteUpdate: function (result) {
        var errMsg = result.errMsg;
        if (result.errCode == 0) {
            age.UnLock();
//            AddPopupHeight(50);
            ageMsg.Show(errMsg, 1);
            window.scroll(0, 0);
            setTimeout(function () {
                if (isReloadParent > 0) {
                    parent.window.location.reload();
                }
                else {
                    window.location.reload();
                }
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
};

////
editCommission.Init(settingContext, customerCommission);

