/* File Created: March 13, 2013 */
/*global APOLLO*/
/*global DEFAULT_LAYOUT*/
/*global Language*/
'use strict';
if (typeof jQuery !== 'undefined') {
    var EDITPOSITIONTAKING = (function ($) {
        return {
            settingContext: null,
            positionTakingElement: null,
            positionTakingSetting: null,
            currentIsSimpleMode: true,
            initOthers: function (isSimpleMode) {
                EDITPOSITIONTAKING.currentIsSimpleMode = isSimpleMode;
                settingContext.IsSimpleMode = isSimpleMode;
                EDITPOSITIONTAKING.settingContext = settingContext;
                Fanex.CustSetting.CURRENT_CONTEXT = new Fanex.CustSetting.Context(settingContext);
                EDITPOSITIONTAKING.positionTakingSetting = positionTakingSetting;
                EDITPOSITIONTAKING.positionTakingElement = new Fanex.CustSetting.PTGrid(positionTakingSetting);
                var positionTakingContainer = document.getElementById('ptContainer');
                positionTakingContainer.innerHTML = "";
                EDITPOSITIONTAKING.positionTakingElement.Render(positionTakingContainer);

                if (isSimpleMode) {
                    var simpleModeWidthClass = settingContext.TargetRoleId === 2 ? "simple-mode-share-setting-preset-width" : "simple-mode-share-setting-width";
                    $("#page_main").removeClass("advanced-mode-share-setting-width").addClass(simpleModeWidthClass);
                    $("#simple-mode").hide();
                    $("#advanced-mode").show();
                    if (DEFAULT_LAYOUT.isMaster) {
                        $('#edit-warning').show();
                    }
                }
                else {
                    $("#page_main").addClass("advanced-mode-share-setting-width").removeClass("simple-mode-share-setting-width");
                    $("#simple-mode").show();
                    $('#edit-warning').hide();
                    $("#advanced-mode").hide();
                }

                APOLLO.initPageInfo();
            },
            init: function (settingContext, positionTakingSetting) {
                //$('#page_main').addClass("simple-mode-share-setting-width");

                EDITPOSITIONTAKING.initOthers(settingContext.IsSimpleMode);

                //// Register Event
                $("#btnSubmitFooter").unbind('click').click(function () {

                    // remove the messages when open the other message
                    $('#noty_top_layout_container').remove();
                    ////DEFAULT_LAYOUT
                    if (!DEFAULT_LAYOUT.allowEdit) {
                        APOLLO.showMessage(Language.NOT_ALLOW_EDIT_MESSAGE, 'error');
                        return;
                    }

                    if (EDITPOSITIONTAKING.positionTakingElement.Validate()) {

                        var postionTakingValue = EDITPOSITIONTAKING.positionTakingElement.Save();
                        if (postionTakingValue == null) {
                            APOLLO.showMessage(Language.OUTRANGE_SIMPLE_MODE_ERROR, 'error');
                            return;
                        }

                        if (!EDITPOSITIONTAKING.positionTakingElement.CheckChangeData(postionTakingValue.SportTypes)) {
                            APOLLO.showMessage(DEFAULT_LAYOUT.noChangeMessage, 'success');
                            return;
                        }

                        if (settingContext.TargetRoleId === Fanex.CustSetting.RoleType.Member) {
                            var confirmMember = confirm(Language.CONFIRM_WRITE_MEMBER);
                            if (confirmMember === true) {
                                EDITPOSITIONTAKING.updatePositionTakingSetting(postionTakingValue);
                            }
                        }
                        else {
                            var confirmAgent = confirm(Language.CONFIRM_WRITE_AGENT);
                            if (confirmAgent === true) {
                                EDITPOSITIONTAKING.updatePositionTakingSetting(postionTakingValue);
                            }
                            //                            $.ajax({
                            //                                url: APOLLO.resolveUrl('CustomerSetting/CalculateUpdateAgentPositionTakingTime'),
                            //                                type: 'POST',
                            //                                contentType: "application/json; charset=utf-8",
                            //                                dataType: "json",
                            //                                data: JSON.stringify({
                            //                                    settingContext: EDITPOSITIONTAKING.settingContext,
                            //                                    positionTakingSetting: postionTakingValue
                            //                                }),
                            //                                cache: false
                            //                            }).done(function (data) {
                            //                                try { $.unblockUI(); } catch (e) { };
                            //                                if (data != '') {
                            //                                    var confirmEstimate = confirm(data);
                            //                                    if (confirmEstimate == true) {
                            //                                        EDITPOSITIONTAKING.updatePositionTakingSetting(postionTakingValue);
                            //                                    }
                            //                                }
                            //                            }).fail(function (jqXHR, textStatus, errorThrown) {
                            //                                try { $.unblockUI(); } catch (e) { };
                            //                            });
                        }
                    }

                });

                $("#btnCancelFooter").unbind('click').click(function () {
                    if (DEFAULT_LAYOUT.fromLeftMenu === true) {
                        window.location.href = $('#backUrl').val();
                    }
                    else {
                        history.go(-1);
                    }
                    return false;
                });

                $('#back-link').unbind('click').bind('click', function () {
                    history.go(-1);
                    return false;
                });

                $("#simple-mode").unbind("click").click(function () {

                    EDITPOSITIONTAKING.initOthers(true);
                });

                $("#advanced-mode").unbind("click").click(function () {
                    EDITPOSITIONTAKING.initOthers(false);
                });
                if (DEFAULT_LAYOUT.isMaster) {
                    $('#tbl-edit').show();
                }
            },
            updatePositionTakingSetting: function (postionTakingValue) {

                // remove the messages when open the other message
                $('#noty_top_layout_container').remove();

                $.ajax({
                    url: APOLLO.resolveUrl('CustomerSetting/UpdatePositionTaking'),
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        settingContext: EDITPOSITIONTAKING.settingContext,
                        positionTakingSetting: postionTakingValue
                    }),
                    cache: false,
                    headers: APOLLO.addAntiForgeryToken({})
                }).done(function (data) {
                    if (data.ErrCode === 0) {
                        //noty({ text: data.ErrMsg, type: 'success' });
                        EDITPOSITIONTAKING.completeUpdate();
                    } else {
                        try { $.unblockUI(); } catch (e) { }
                        var mainMessage = data.ErrMsg;
                        if (Fanex.CustSetting.CURRENT_CONTEXT.FormId === Fanex.CustSetting.FormTypes.EDIT_MULTIPLE) {
                            var arrayMessage = data.ErrMsg.split('##'),
                                    errorMessage = '',
                                    successMessage = '';
                            for (var i = 0; i < arrayMessage.length; i++) {
                                var innerMessage = arrayMessage[i];
                                if (i === 0) {
                                    $.each(innerMessage.split(';'), function (i, val) {
                                        if (val !== '') {
                                            errorMessage += ' + ' + val + '<br/>';
                                        }
                                    });
                                }
                                else {
                                    $.each(innerMessage.split(';'), function (i, val) {
                                        if (val !== '') {
                                            successMessage += ' + ' + val + '<br/>';
                                        }
                                    });
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
                });
            },
            completeUpdate: function () {
                var backLink = $('#backUrl').val();

                //setTimeout(function () {
                try { $.unblockUI(); } catch (e) { }
                if (DEFAULT_LAYOUT.reload) {
                    var url = window.location.href;
                    if (url.indexOf("&isSimpleMode=") > 0) {
                        url = url.substring(0, url.indexOf("&isSimpleMode="));
                    }
                    window.location.href = url + '&isSimpleMode=' + EDITPOSITIONTAKING.currentIsSimpleMode;
                }
                else {
                    window.location.href = backLink;
                }
                //}, 4000);

            }
        };
    } (jQuery));

    $(function () {
        if (positionTakingSetting.SportTypes && positionTakingSetting.SportTypes.length > 0) {
            settingContext.IsSimpleMode = DEFAULT_LAYOUT.isSimpleMode;
            EDITPOSITIONTAKING.init(settingContext, positionTakingSetting);
        }
        else {
            $('#ptContainer, #modeMark').hide();
            $('#noInfoDiv').show();
        }
    }).ajaxStart($.blockUI);

}
