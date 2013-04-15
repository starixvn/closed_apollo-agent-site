/* File Created: March 13, 2013 */

if (typeof jQuery != 'undefined') {
    var EDITPOSITIONTAKING = function ($) {
        return {
            settingContext: null,
            positionTakingElement: null,
            positionTakingSetting: null,
            init: function (settingContext, positionTakingSetting) {

                EDITPOSITIONTAKING.settingContext = settingContext;
                Fanex.CustSetting.CURRENT_CONTEXT = new Fanex.CustSetting.Context(settingContext);
                EDITPOSITIONTAKING.positionTakingSetting = positionTakingSetting;
                EDITPOSITIONTAKING.positionTakingElement = new Fanex.CustSetting.PTGrid(positionTakingSetting);
                var positionTakingContainer = document.getElementById('ptContainer');
                positionTakingContainer.innerHTML = "";
                EDITPOSITIONTAKING.positionTakingElement.Render(positionTakingContainer);

                //// Register Event
                $("#btnSubmitFooter").click(function () {
                    EDITPOSITIONTAKING.updatePositionTakingSetting();
                });
            },
            updatePositionTakingSetting: function () {
                $.ajax({
                    url: APOLLO.resolveUrl('CustomerSetting/UpdatePositionTaking'),
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        settingContext: EDITPOSITIONTAKING.settingContext,
                        positionTakingSetting: EDITPOSITIONTAKING.positionTakingElement.Save()
                    }),
                    success: function (data) {
                        ageMsg.ShowMsgs(data.ErrMsg, 1);
                    },
                    error: function () {
                        ageMsg.ShowMsgs(data.ErrMsg);
                    }
                });
            },
            completeUpdate: function (result) {
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
                    //            AddPopupHeight(50);
                    ageMsg.Show(errMsg);
                    window.scroll(0, 0);
                    return;
                }
            }
        }
    } (jQuery);

    $(function () {
        EDITPOSITIONTAKING.init(settingContext, positionTakingSetting);
    }).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    
}
