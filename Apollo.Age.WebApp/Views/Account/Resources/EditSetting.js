/* File Created: March 14, 2013 */
/*global DEFAULT_OBJECT*/
/*global Language*/
/*global DEFAULT_LAYOUT*/
/*global APOLLO*/
'use strict';
if (typeof jQuery !== 'undefined') {
    var EDITSETTING = (function ($) {
        return {
            currentIsSimpleMode: true,
            settingContext: null,
            commissionSettingElement: null,
            betLimitElement: null,
            maxBetLimit: 0,

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
            init: function () {
                $('#page_main').addClass('setting-width');
                EDITSETTING.currentIsSimpleMode = false;
                EDITSETTING.initOthers(false);
                $("#btnSubmitFooter, #btnSubmitHeader").unbind("click").click(function () {
                    // remove the messages when open the other message
                    $('#noty_top_layout_container').remove();
                    EDITSETTING.saveSetting();
                });

                $("#btnCancelFooter").unbind('click').click(function () {
                    history.go(-1);
                    return false;
                });

                if (DEFAULT_LAYOUT.isMaster) {
                    $('#tbl-edit').show();
                }
                $('#btnCancelFooter').show();

                EDITSETTING.maxBetLimit = maxBetLimit;
                if (EDITSETTING.maxBetLimit === 0) {
                    $("#betSettingInformationMessage").hide();
                }
                else {
                    $("#betSettingInformationMessage").show();
                    // $("#maxbetlimit").html(EDITSETTING.maxBetLimit);
                }
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

                    if (Fanex.CustSetting.CURRENT_CONTEXT.FormId !== Fanex.CustSetting.FormTypes.EDIT_NON_DIRECT
                    && dataObject.betLimits.length === 0
                    && commissionSettingValue.CommissionItems.length === 0 && typeof (commissionSettingValue.UserGroup) === 'undefined') {
                        APOLLO.showMessage(DEFAULT_LAYOUT.noChangeMessage, 'success');
                        return;
                    }

                    var stringConfirm = "";
                    if (settingContext.TargetRoleId === Fanex.CustSetting.RoleType.Member) {
                        stringConfirm = Language.CONFIRM_WRITE_MEMBER;
                    }
                    else {
                        stringConfirm = Language.CONFIRM_WRITE_AGENT;
                    }
                    var c = confirm(stringConfirm);
                    if (c === false) {
                        return;
                    }

                    var url = APOLLO.resolveUrl('CustomerSetting/');
                    if (typeof (DEFAULT_OBJECT.memberId) !== 'undefined') {
                        dataObject.memberId = DEFAULT_OBJECT.memberId;
                        url += "UpdateMemberSetting";
                    }
                    else if (typeof (DEFAULT_OBJECT.agentId) !== 'undefined') {
                        dataObject.agentId = DEFAULT_OBJECT.agentId;
                        url += "UpdateAgentSetting";
                    }

                    $.ajax({
                        url: url,
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(dataObject),
                        cache: false,
                        headers: APOLLO.addAntiForgeryToken({})
                    }).done(function (data) {
                        if (data.ErrCode === 0) {
                            EDITSETTING.completeUpdate();
                        } else {
                            try { $.unblockUI(); } catch (e) { }
                            var mainMessage = data.ErrMsg;
                            if (Fanex.CustSetting.CURRENT_CONTEXT.FormId === Fanex.CustSetting.FormTypes.EDIT_MULTIPLE) {
                                //
                                var arrayMessage = data.ErrMsg.split('^^'),
                                    errorMessage = '',
                                    successMessage = '';
                                for (var i = 0; i < arrayMessage.length; i++) {
                                    var innerMessage = arrayMessage[i].split('##');
                                    for (var j = 0; j < innerMessage.length; j++) {
                                        if (j === 0) {
                                            $.each(innerMessage[j].split(';'), function (i, val) {
                                                if (val !== '') {
                                                    errorMessage += ' + ' + val + '<br/>';
                                                }
                                            });
                                        }
                                        else {
                                            $.each(innerMessage[j].split(';'), function (i, val) {
                                                if (val !== '') {
                                                    successMessage += ' + ' + val + '<br/>';
                                                }
                                            });
                                        }
                                    }
                                }
                                mainMessage = Language.UPDATE_ERROR + '<br/>' + errorMessage;
                                if (successMessage !== '') {
                                    mainMessage += '<br/><div class="successMessage">' + Language.UPDATE_MULTIPLE_SUCCESS + '<br/>' + successMessage + '</div>';
                                }

                                APOLLO.showMessage(mainMessage, 'error', null, true);
                            }
                            else {
                                APOLLO.showMessage(mainMessage, 'error');
                            }
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        if (jqXHR.status === 403) {
                            location.href = APOLLO.resolveUrl("~/Error/AccessDenied");
                        }
                        else {
                            if (jqXHR.responseText) {
                                APOLLO.showMessage(jqXHR.responseText, 'error');
                            }
                            else {
                                APOLLO.handleAjaxError(jqXHR, textStatus, errorThrown);
                            }
                        }
                        try { $.unblockUI(); } catch (e) { }
                    }).always(function () {
                    });
                }
            },
            completeUpdate: function (result) {
                var backLink = $('#backUrl').val();
                //setTimeout(function () {
                try { $.unblockUI(); } catch (e) { }
                window.location.href = backLink;
                //}, 4000);
            }
        };
    } (jQuery));

    $(function () {
        EDITSETTING.init();
    }).ajaxStart($.blockUI);
}