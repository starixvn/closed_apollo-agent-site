/* File Created: March 14, 2013 */
if (typeof jQuery != 'undefined') {
    var EDITSETTING = function ($) {
        return {
            init: function() {
                $("#btnSubmitFooter").unbind("click").click(function () {
                    EDITSETTING.saveSetting();
                });
            },
            saveSetting: function () {
                if (BETLIMIT.betLimitElement.Validate()) {
                    $.ajax({
                        url: APOLLO.resolveUrl('CustomerSetting/UpdateAllSetting'),
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({
                            settingContext: EDITPOSITIONTAKING.settingContext,
                            positionTakingSetting: EDITPOSITIONTAKING.positionTakingElement.Save(),
                            commissionSetting: EDITCOMMISSION.commissionSettingElement.Save(),
                            betLimits: BETLIMIT.betLimitElement.Save()
                        }),
                        success: function (data) {
                            ageMsg.ShowMsgs(data.ErrMsg, 1);
                        },
                        error: function () {
                            ageMsg.ShowMsgs(data.ErrMsg);
                        }
                    });
                }
            },
            CompleteUpdate: function (result) {
//                var errMsg = result.errMsg;
//                if (result.errCode == 0) {
//                    age.UnLock();
//                    //            AddPopupHeight(50);
//                    ageMsg.Show(errMsg, 1);
//                    window.scroll(0, 0);
//                    setTimeout(function () {
//                        if (isReloadParent > 0) {
//                            parent.window.location.reload();
//                        }
//                        else {
//                            window.location.reload();
//                        }
//                    }, 1000);

//                    return;
//                }
//                else {
//                    age.UnLock();
//                    //            AddPopupHeight(50);
//                    ageMsg.Show(errMsg);
//                    window.scroll(0, 0);
//                    return;
//                }
            }
        }
    } (jQuery);

    $(function () {
        EDITSETTING.init();
    }).ajaxStart($.blockUI).ajaxStop($.unblockUI);
}