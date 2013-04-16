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
                        })
                    }).done(function (data) {
                        if (data.ErrCode == 0) {
                            noty({ text: data.ErrMsg, type: 'success' });
                            BETLIMIT.completeUpdate();
                        } else {
                            noty({ text: APOLLO.generalErrorMsg, type: 'error' });
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        APOLLO.handleAjaxError(jqXHR, textStatus, errorThrown);
                    });

                }
            },
            completeUpdate: function (result) {
                setTimeout(function () {
                    if (typeof (parent.MEMBERLIST) != 'undefined') {
                        parent.MEMBERLIST.loadMemberList();
                    }
                    else if (typeof (parent.AGENTLIST) != 'undefined') {
                        parent.AGENTLIST.loadAgentList();
                    }
                }, 3000);
            }
        };
    } (jQuery);

    $(function () {
        BETLIMIT.init(settingContext, betLimits);
    }).ajaxStart($.blockUI).ajaxStop($.unblockUI);
};


