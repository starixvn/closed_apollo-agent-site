/* File Created: March 13, 2013 */

if (typeof jQuery != 'undefined') {
    var EDITPOSITIONTAKING = function ($) {
        return {
            settingContext: null,
            positionTakingElement: null,
            positionTakingSetting: null,
            currentIsSimpleMode: true,
            initOthers: function (isSimpleMode) {
                settingContext.IsSimpleMode = isSimpleMode;
                EDITPOSITIONTAKING.settingContext = settingContext;
                Fanex.CustSetting.CURRENT_CONTEXT = new Fanex.CustSetting.Context(settingContext);
                EDITPOSITIONTAKING.positionTakingSetting = positionTakingSetting;
                EDITPOSITIONTAKING.positionTakingElement = new Fanex.CustSetting.PTGrid(positionTakingSetting);
                var positionTakingContainer = document.getElementById('ptContainer');
                positionTakingContainer.innerHTML = "";
                EDITPOSITIONTAKING.positionTakingElement.Render(positionTakingContainer);

                APOLLO.initPageInfo();
            },
            init: function (settingContext, positionTakingSetting, isUpdateAllSetting) {
                $('#page_main').addClass("simple-mode-share-setting-width");

                EDITPOSITIONTAKING.initOthers(settingContext.IsSimpleMode);

                //// Register Event
                $("#btnSubmitFooter").unbind('click').click(function () {
                    var stringConfirm = "";
                    if (settingContext.TargetRoleId == Fanex.CustSetting.RoleType.Member) {
                        stringConfirm = Language.CONFIRM_WRITE_MEMBER;
                    }
                    else {
                        stringConfirm = Language.CONFIRM_WRITE_AGENT;
                    }
                    var c = confirm(stringConfirm);
                    if (c == true
                    && EDITPOSITIONTAKING.positionTakingElement.Validate()) {
                        
                        EDITPOSITIONTAKING.updatePositionTakingSetting();
                    }

                });

                $("#btnCancelFooter").unbind('click').click(function () {
                    history.go(-1);
                    return false;
                });

                $("#simple-mode").unbind("click").click(function () {
                    $(this).hide();
                    $("#page_main").removeClass("advanced-mode-share-setting-width").addClass("simple-mode-share-setting-width");
                    $("#advanced-mode").show();

                    EDITPOSITIONTAKING.currentIsSimpleMode = true;
                    EDITPOSITIONTAKING.initOthers(true);
                });

                $("#advanced-mode").unbind("click").click(function () {
                    $(this).hide();
                    $("#page_main").addClass("advanced-mode-share-setting-width").removeClass("simple-mode-share-setting-width");
                    $("#simple-mode").show();

                    EDITPOSITIONTAKING.currentIsSimpleMode = false;
                    EDITPOSITIONTAKING.initOthers(false);
                });
            },
            updatePositionTakingSetting: function () {
                var postionTakingValue = EDITPOSITIONTAKING.positionTakingElement.Save();
                if (postionTakingValue == null) {
                    noty({ text: Language.OUTRANGE_SIMPLE_MODE_ERROR, type: 'error', timeout: 5000 });
                    return;
                }

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
                });
            },
            completeUpdate: function (result) {
                window.location.href = $('#backUrl').val();
                //                setTimeout(function () {
                //                    if (typeof (parent.MEMBERLIST) != 'undefined') {
                //                        parent.MEMBERLIST.loadMemberList();
                //                    }
                //                    else if (typeof (parent.AGENTLIST) != 'undefined') {
                //                        parent.AGENTLIST.loadAgentList();
                //                    }
                //                }, 3000);
            }
        }
    } (jQuery);

    $(function () {
        settingContext.IsSimpleMode = DEFAULT_LAYOUT.isSimpleMode;
        EDITPOSITIONTAKING.init(settingContext, positionTakingSetting);
    });
    
}
