/* File Created: March 13, 2013 */

if (typeof jQuery != 'undefined') {
    var BETLIMIT = function ($) {
        return {
            settingContext: null,
            betLimitElement: null,
            betLimitItems: null,
            init: function (settingContext, betLimits) {
                BETLIMIT.settingContext = settingContext;
                Fanex.CustSetting.CURRENT_CONTEXT = new Fanex.CustSetting.Context(settingContext);
                BETLIMIT.betLimitItems = betLimits;
                BETLIMIT.betLimitElement = new Fanex.CustSetting.BettingLimit(betLimits);
                var bettingLimitContainer = document.getElementById('bettingLimitContainer');
                bettingLimitContainer.innerHTML = "";
                BETLIMIT.betLimitElement.Render(bettingLimitContainer);

                //// Register Event
                $("#btnSubmitFooter").click(function () {
                    BETLIMIT.updateBetLimit();
                });
            },
            updateBetLimit: function () {
                if (BETLIMIT.betLimitElement.Validate()) {
                    $.ajax({
                        url: APOLLO.resolveUrl('CustomerSetting/UpdateBetLimit'),
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({
                            settingContext: BETLIMIT.settingContext,
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
            completeUpdate: function (result) {
                //                var errMsg = result.errMsg;
                //                if (result.errCode == 0) {
                //                    age.UnLock();
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
                //                    ageMsg.Show(errMsg);
                //                    window.scroll(0, 0);
                //                    return;
                //                }
            }
        };
    } (jQuery);

    $(function () {
        BETLIMIT.init(settingContext, betLimits);
    }).ajaxStart($.blockUI).ajaxStop($.unblockUI);
};


