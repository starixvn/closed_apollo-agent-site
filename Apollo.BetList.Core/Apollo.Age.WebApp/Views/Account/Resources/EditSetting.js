/* File Created: March 14, 2013 */
if (typeof jQuery != 'undefined') {
    var EDITSETTING = function ($) {
        return {
            currentIsSimpleMode: true,
            settingContext: null,
            commissionSettingElement: null,
            betLimitElement: null,

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

                APOLLO.initPageInfo();
            },
            initForMode: function () {
                //                if (EDITSETTING.currentIsSimpleMode) {
                //                    $("#btnSubmitHeader").hide();
                //                }
                //                else {
                //                    $("#btnSubmitHeader").show();
                //                }
            },
            init: function () {
                $('#page_main').addClass('setting-width');
                EDITSETTING.currentIsSimpleMode = false;
                EDITSETTING.initOthers(false);
                EDITSETTING.initForMode();
                $("#btnSubmitFooter, #btnSubmitHeader").unbind("click").click(function () {
                    EDITSETTING.saveSetting();
                });
                //                $("#simple-mode").unbind("click").click(function () {

                //                    $(this).hide();
                //                    $("#advanced-mode").show();
                //                    $('#edit-warning').show();
                //                    EDITSETTING.currentIsSimpleMode = true;
                //                    EDITSETTING.initOthers(true);
                //                });

                $("#btnCancelFooter").unbind('click').click(function () {
                    history.go(-1);
                    return false;
                });

                //                $("#advanced-mode").unbind("click").click(function () {

                //                    $(this).hide();
                //                    $("#simple-mode").show();
                //                    $('#edit-warning').hide();

                //                    EDITSETTING.currentIsSimpleMode = false;
                //                    EDITSETTING.initOthers(false);
                //                });
                if (DEFAULT_LAYOUT.isMaster) {
                    $('#tbl-edit').show();
                }
                $('#btnCancelFooter').show();
            },
            saveSetting: function () {
                if (EDITSETTING.commissionSettingElement.Validate()
                && EDITSETTING.betLimitElement.Validate()) {
                    var commissionSettingValue = EDITSETTING.commissionSettingElement.Save();

                    var dataObject = {
                        settingContext: EDITSETTING.settingContext,
                        commissionSetting: commissionSettingValue,
                        betLimits: EDITSETTING.betLimitElement.Save()
                    };

                    if (Fanex.CustSetting.CURRENT_CONTEXT.FormId != Fanex.CustSetting.FormTypes.EDIT_NON_DIRECT
                    && dataObject.betLimits.length == 0
                    && commissionSettingValue.CommissionItems.length == 0 && typeof (commissionSettingValue.UserGroup) == 'undefined') {
                        noty({ text: DEFAULT_LAYOUT.noChangeMessage, type: 'success', timeout: 5000 });
                        return;
                    }

                    var stringConfirm = "";
                    if (settingContext.TargetRoleId == Fanex.CustSetting.RoleType.Member) {
                        stringConfirm = Language.CONFIRM_WRITE_MEMBER;
                    }
                    else {
                        stringConfirm = Language.CONFIRM_WRITE_AGENT;
                    }
                    var c = confirm(stringConfirm);
                    if (c == false) {
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


                    $.ajax({
                        url: url,
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(dataObject),
                        headers: APOLLO.addAntiForgeryToken({})
                    }).done(function (data) {
                        if (data.ErrCode == 0) {
                            EDITSETTING.completeUpdate();
                        } else {
                            noty({ text: data.ErrMsg, type: 'error', timeout: 5000 });
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        APOLLO.handleAjaxError(jqXHR, textStatus, errorThrown);
                        try { $.unblockUI(); } catch (e) { };
                    }).always(function () {
                    });
                }
            },
            completeUpdate: function (result) {
                var backLink = $('#backUrl').val();
                setTimeout(function () {
                    window.location.href = backLink;
                }, 2000);

            }
        }
    } (jQuery);

    $(function () {
        EDITSETTING.init();
    }).ajaxStart($.blockUI).ajaxStop($.unblockUI);
}