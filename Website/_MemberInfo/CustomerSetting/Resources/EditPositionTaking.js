/* File Created: March 13, 2013 */


var editPositionTaking =
{
    SettingContext: null,
    PositionTakingElement: null,
    PositionTakingSetting: null,
    Init: function (settingContext, positionTakingSetting) {

        editPositionTaking.SettingContext = settingContext;
        Fanex.CustSetting.CURRENT_CONTEXT = new Fanex.CustSetting.Context(settingContext);
        editPositionTaking.PositionTakingSetting = positionTakingSetting;
        editPositionTaking.PositionTakingElement = new Fanex.CustSetting.PTGrid(positionTakingSetting);
        var positionTakingContainer = document.getElementById('ptContainer');
        positionTakingContainer.innerHTML = "";
        editPositionTaking.PositionTakingElement.Render(positionTakingContainer);
    },
    UpdatePositionTakingSetting: function () {
            var params = "settingContext=" + JSON.stringify(editPositionTaking.SettingContext) +
                         "&positionTakingSetting=" + JSON.stringify(editPositionTaking.PositionTakingElement.Save());
            var submitUrl = "../../CustomerSetting/Handlers/UpdatePositionTakingSetting.ashx";
            var onCompleteCallback = null;
            age.ReloadPage(600000);
            ajax.PostJSON(
        				submitUrl,
        				params,
        				editPositionTaking.CompleteUpdate
        			);
    },
    CompleteUpdate: function (result) {
        var errMsg = result.errMsg;
        if (result.errCode == 0) {
            age.UnLock();
            AddPopupHeight(50);
            ageMsg.Show(errMsg, 1);
            window.scroll(0, 0);
            setTimeout(function () {
                AddPopupHeight(0);
                location.reload();
            }, 3000);

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
editPositionTaking.Init(settingContext, positionTakingSetting);
age.UnLock();

