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
                    $("#settingContainer .edit-setting-warning").show();
                    EDITSETTING.currentIsSimpleMode = true;
                    EDITSETTING.initOthers(true);
                    EDITSETTING.setDialogHeight(true);
                });

                $(".setting-mode #advanced-mode").unbind("click").click(function () {
                    if (EDITSETTING.currentIsSimpleMode == false) return;
                    $(this).removeClass("active-mode").addClass("deactive-mode");
                    $(".setting-mode #simple-mode").removeClass("deactive-mode").addClass("active-mode");
                    $("#settingContainer .edit-setting-warning").hide();
                    EDITSETTING.currentIsSimpleMode = false;
                    EDITSETTING.initOthers(false);
                    EDITSETTING.setDialogHeight(false);
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
                            noty({ text: data.ErrMsg, type: 'success', timeout: 5000 });
                            EDITSETTING.completeUpdate();
                        } else {
                            noty({ text: data.ErrMsg, type: 'error', timeout: 5000 });
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
            },
            setDialogHeight: function (isSimpleMode) {
                var currentList = null;
                if (typeof (parent.MEMBERLIST) != 'undefined') {
                    currentList = parent.MEMBERLIST;
                }
                else if (typeof (parent.AGENTLIST) != 'undefined') {
                    currentList = parent.AGENTLIST;
                }
                if (currentList != null) {
                    if (isSimpleMode == true) {
                        currentList.currentPopUp.dialog("option", {
                            height: 450,
                            position: { my: "top+25%", at: "center top", of: parent.window }
                        });
                    }
                    else {
                        currentList.currentPopUp.dialog("option", {
                            height: 750,
                            position: { my: "top+10", at: "center top", of: parent.window }
                        });
                    }
                }
            }
        }
    } (jQuery);

    $(function () {
        EDITSETTING.init();
        $("#btnSubmitFooter").show();
    }).ajaxStart($.blockUI).ajaxStop($.unblockUI);
}