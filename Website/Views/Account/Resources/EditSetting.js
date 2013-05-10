/* File Created: March 14, 2013 */
if (typeof jQuery != 'undefined') {
    var EDITSETTING = function ($) {
        return {
            currentIsSimpleMode: true,
            settingContext: null,
            commissionSettingElement: null,
            betLimitElement: null,
            positionTakingElement: null,

            initOthers: function (isSimpleMode) {
                settingContext.IsSimpleMode = isSimpleMode;

                //// Init Commission Setting
                EDITSETTING.settingContext = settingContext;
                Fanex.CustSetting.CURRENT_CONTEXT = new Fanex.CustSetting.Context(settingContext);
                EDITSETTING.commissionSettingElement = new Fanex.CustSetting.CommissionSetting(commissionSetting);
                var commissionSettingContainer = document.getElementById('commissionSettingContainer');
                commissionSettingContainer.innerHTML = "";
                EDITSETTING.commissionSettingElement.Render(commissionSettingContainer);

                //// Init Bet Limit Setting
                EDITSETTING.betLimitElement = new Fanex.CustSetting.BettingLimit(betLimits);
                var bettingLimitContainer = document.getElementById('bettingLimitContainer');
                bettingLimitContainer.innerHTML = "";
                EDITSETTING.betLimitElement.Render(bettingLimitContainer);

                //// Position Taking
                EDITSETTING.positionTakingElement = new Fanex.CustSetting.PTGrid(positionTakingSetting);
                var positionTakingContainer = document.getElementById('ptContainer');
                positionTakingContainer.innerHTML = "";
                EDITSETTING.positionTakingElement.Render(positionTakingContainer);

                APOLLO.initPageInfo();
            },
            initForMode: function () {
                if (EDITSETTING.currentIsSimpleMode) {
                    $("setting-mode #simple-mode").removeClass("active-mode").addClass("deactive-mode");
                    $(".setting-mode #advanced-mode").removeClass("deactive-mode").addClass("active-mode");
                    $("#settingContainer .edit-setting-warning").show();
                }
                else {
                    $(".setting-mode #advanced-mode").removeClass("active-mode").addClass("deactive-mode");
                    $(".setting-mode #simple-mode").removeClass("deactive-mode").addClass("active-mode");
                    $("#settingContainer .edit-setting-warning").hide();
                }
            },
            init: function () {
                EDITSETTING.currentIsSimpleMode = DEFAULT_LAYOUT.isSimpleMode;
                EDITSETTING.initOthers(DEFAULT_LAYOUT.isSimpleMode);
                EDITSETTING.initForMode();
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
                if (EDITSETTING.commissionSettingElement.Validate()
                && EDITSETTING.betLimitElement.Validate()
                && EDITSETTING.positionTakingElement.Validate()) {
                    var commissionSettingValue = EDITSETTING.commissionSettingElement.Save();
                    if (commissionSettingValue == null && Fanex.CustSetting.CURRENT_CONTEXT.FormId != Fanex.CustSetting.FormTypes.EDIT_NON_DIRECT) {
                        noty({ text: Language.INVALID_COMMISSION_MESSAGE, type: 'error', timeout: 5000 });
                        return;
                    }

                    var postionTakingValue = EDITSETTING.positionTakingElement.Save();
                    if (postionTakingValue == null) {
                        noty({ text: Language.OUTRANGE_SIMPLE_MODE_ERROR, type: 'error', timeout: 5000 });
                        return;
                    }

                    var dataObject = {
                        settingContext: EDITSETTING.settingContext,
                        positionTakingSetting: postionTakingValue,
                        commissionSetting: commissionSettingValue,
                        betLimits: EDITSETTING.betLimitElement.Save()
                    };

                    if (Fanex.CustSetting.CURRENT_CONTEXT.FormId == Fanex.CustSetting.FormTypes.EDIT_NON_DIRECT
                    && !EDITSETTING.positionTakingElement.CheckChangeData(postionTakingValue.SportTypes)) {
                        noty({ text: DEFAULT_LAYOUT.noChangeMessage, type: 'success', timeout: 5000 });
                        return;
                    }
                    else if (Fanex.CustSetting.CURRENT_CONTEXT.FormId != Fanex.CustSetting.FormTypes.EDIT_NON_DIRECT
                    && dataObject.betLimits.length == 0
                    && commissionSettingValue.CommissionItems.length == 0 && typeof (commissionSettingValue.UserGroup) == 'undefined'
                    && !EDITSETTING.positionTakingElement.CheckChangeData(postionTakingValue.SportTypes)) {
                        noty({ text: DEFAULT_LAYOUT.noChangeMessage, type: 'success', timeout: 5000 });
                        return;
                    }

                    var url = APOLLO.resolveUrl('CustomerSetting/');
                    if (typeof (DEFAULT_OBJECT.memberId) != 'undefined') {
                        dataObject.memberId = DEFAULT_OBJECT.memberId;
                        url += "UpdateMemberSetting";
                    }
                    else if (typeof (DEFAULT_OBJECT.agentId) != 'undefined') {
                        dataObject.agentId = DEFAULT_OBJECT.agentId;
                        url += "UpdateAgentSetting";
                    }

                    $("#btnSubmitFooter").attr("disabled", "disabled");
                    $.ajax({
                        url: url,
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(dataObject)
                    }).done(function (data) {
                        if (data.ErrCode == 0) {
                            noty({ text: data.ErrMsg, type: 'success', timeout: 5000 });
                            EDITSETTING.completeUpdate();
                        } else {
                            noty({ text: data.ErrMsg, type: 'error', timeout: 5000 });
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        APOLLO.handleAjaxError(jqXHR, textStatus, errorThrown);
                    }).always(function () {
                        $("#btnSubmitFooter").removeAttr("disabled");
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
                    else {
                        var url = window.location.href;
                        if (url.indexOf("&isSimpleMode=") > 0) {
                            url = url.substring(0, url.indexOf("&isSimpleMode="));
                        }
                        window.location.href = url + '&isSimpleMode=' + EDITSETTING.currentIsSimpleMode;
                    }
                }, 4500);
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
                            height: 470,
                            position: { my: "top+25%", at: "center top", of: parent.window }
                        });
                    }
                    else {
                        currentList.currentPopUp.dialog("option", {
                            height: 720,
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