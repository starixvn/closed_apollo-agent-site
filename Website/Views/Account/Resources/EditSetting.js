/* File Created: March 14, 2013 */
if (typeof jQuery != 'undefined') {
    var EDITSETTING = function ($) {
        return {
            currentIsSimpleMode: true,
            initOthers: function (isSimpleMode) {
                settingContext.IsSimpleMode = isSimpleMode;
                BETLIMIT.init(settingContext, betLimits, true);
                EDITCOMMISSION.init(settingContext, commissionSetting, true);
                EDITPOSITIONTAKING.init(settingContext, positionTakingSetting, true);
            },
            init: function () {
                $("#btnSubmitFooter").unbind("click").click(function () {
                    EDITSETTING.saveSetting();
                });

                $(".setting-mode #simple-mode").unbind("click").click(function () {
                    if (EDITSETTING.currentIsSimpleMode) return;
                    $(this).removeClass("active-mode").addClass("deactive-mode");
                    $(".setting-mode #advanced-mode").removeClass("deactive-mode").addClass("active-mode");
                    EDITSETTING.currentIsSimpleMode = true;
                    EDITSETTING.initOthers(true);
                });

                $(".setting-mode #advanced-mode").unbind("click").click(function () {
                    if (EDITSETTING.currentIsSimpleMode == false) return;
                    $(this).removeClass("active-mode").addClass("deactive-mode");
                    $(".setting-mode #simple-mode").removeClass("deactive-mode").addClass("active-mode");
                    EDITSETTING.currentIsSimpleMode = false;
                    EDITSETTING.initOthers(false);
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
                        })
                    }).done(function (data) {
                        if (data.ErrCode == 0) {
                            noty({ text: data.ErrMsg, type: 'success' });
                            EDITSETTING.completeUpdate();
                        } else {
                            noty({ text: APOLLO.generalErrorMsg, type: 'error' });
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        APOLLO.handleAjaxError(jqXHR, textStatus, errorThrown);
                    });
                }
            },
            CompleteUpdate: function (result) {
                setTimeout(function () {
                    if (typeof (parent.MEMBERLIST) != 'undefined') {
                        parent.MEMBERLIST.loadMemberList();
                    }
                    else if (typeof (parent.AGENTLIST) != 'undefined') {
                        parent.AGENTLIST.loadAgentList();
                    }
                }, 3000);
            }
        }
    } (jQuery);

    $(function () {
        EDITSETTING.init();
    }).ajaxStart($.blockUI).ajaxStop($.unblockUI);
}