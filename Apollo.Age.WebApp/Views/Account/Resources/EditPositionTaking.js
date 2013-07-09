/* File Created: March 13, 2013 */

if (typeof jQuery != 'undefined') {
    var EDITPOSITIONTAKING = function ($) {
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
                    $("#page_main").removeClass("advanced-mode-share-setting-width").addClass("simple-mode-share-setting-width");
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
            init: function (settingContext, positionTakingSetting, isUpdateAllSetting) {
                $('#page_main').addClass("simple-mode-share-setting-width");

                EDITPOSITIONTAKING.initOthers(settingContext.IsSimpleMode);

                //// Register Event
                $("#btnSubmitFooter").unbind('click').click(function () {

                    if (EDITPOSITIONTAKING.positionTakingElement.Validate()) {

                        var postionTakingValue = EDITPOSITIONTAKING.positionTakingElement.Save();
                        if (postionTakingValue == null) {
                            noty({ text: Language.OUTRANGE_SIMPLE_MODE_ERROR, type: 'error', timeout: 5000 });
                            return;
                        }

                        if (!EDITPOSITIONTAKING.positionTakingElement.CheckChangeData(postionTakingValue.SportTypes)) {
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
                        if (c == true) {
                            EDITPOSITIONTAKING.updatePositionTakingSetting(postionTakingValue);
                        }
                    }

                });

                $("#btnCancelFooter").unbind('click').click(function () {
                    if (DEFAULT_LAYOUT.fromLeftMenu == true) {
                        window.location.href = $('#backUrl').val();
                    }
                    else {
                        history.go(-1);
                    }
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
                

                $.ajax({
                    url: APOLLO.resolveUrl('CustomerSetting/UpdatePositionTaking'),
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        settingContext: EDITPOSITIONTAKING.settingContext,
                        positionTakingSetting: postionTakingValue
                    }),
                    headers: APOLLO.addAntiForgeryToken({})
                }).done(function (data) {
                    if (data.ErrCode == 0) {
                        //noty({ text: data.ErrMsg, type: 'success' });
                        EDITPOSITIONTAKING.completeUpdate();
                    } else {
                        noty({ text: data.ErrMsg, type: 'error', timeout: 5000 });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    APOLLO.handleAjaxError(jqXHR, textStatus, errorThrown);
                    try { $.unblockUI(); } catch (e) { };
                });
            },
            completeUpdate: function (result) {
                var backLink = $('#backUrl').val();
                setTimeout(function () {
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
                }, 4000);

            }
        }
    } (jQuery);

    $(function () {
        settingContext.IsSimpleMode = DEFAULT_LAYOUT.isSimpleMode;
        EDITPOSITIONTAKING.init(settingContext, positionTakingSetting);
    }).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    
}
