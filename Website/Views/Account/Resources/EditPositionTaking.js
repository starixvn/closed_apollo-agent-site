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
                EDITPOSITIONTAKING.init(settingContext, positionTakingSetting);
            },
            init: function (settingContext, positionTakingSetting, isUpdateAllSetting) {

                EDITPOSITIONTAKING.settingContext = settingContext;
                Fanex.CustSetting.CURRENT_CONTEXT = new Fanex.CustSetting.Context(settingContext);
                EDITPOSITIONTAKING.positionTakingSetting = positionTakingSetting;
                EDITPOSITIONTAKING.positionTakingElement = new Fanex.CustSetting.PTGrid(positionTakingSetting);
                var positionTakingContainer = document.getElementById('ptContainer');
                positionTakingContainer.innerHTML = "";
                EDITPOSITIONTAKING.positionTakingElement.Render(positionTakingContainer);

                if (isUpdateAllSetting) return;

                //// Register Event
                $("#btnSubmitFooter").click(function () {
                    EDITPOSITIONTAKING.updatePositionTakingSetting();
                });

                $(".setting-mode #simple-mode").unbind("click").click(function () {
                    if (EDITPOSITIONTAKING.currentIsSimpleMode) return;
                    $(this).removeClass("active-mode").addClass("deactive-mode");
                    $(".setting-mode #advanced-mode").removeClass("deactive-mode").addClass("active-mode");
                    EDITPOSITIONTAKING.currentIsSimpleMode = true;
                    EDITPOSITIONTAKING.initOthers(true);
                });

                $(".setting-mode #advanced-mode").unbind("click").click(function () {
                    if (EDITPOSITIONTAKING.currentIsSimpleMode == false) return;
                    $(this).removeClass("active-mode").addClass("deactive-mode");
                    $(".setting-mode #simple-mode").removeClass("deactive-mode").addClass("active-mode");
                    EDITPOSITIONTAKING.currentIsSimpleMode = false;
                    EDITPOSITIONTAKING.initOthers(false);
                });
            },
            updatePositionTakingSetting: function () {
                $.ajax({
                    url: APOLLO.resolveUrl('CustomerSetting/UpdatePositionTaking'),
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        settingContext: EDITPOSITIONTAKING.settingContext,
                        positionTakingSetting: EDITPOSITIONTAKING.positionTakingElement.Save()
                    })
                }).done(function (data) {
                    if (data.ErrCode == 0) {
                        noty({ text: data.ErrMsg, type: 'success' });
                        EDITPOSITIONTAKING.completeUpdate();
                    } else {
                        noty({ text: APOLLO.generalErrorMsg, type: 'error' });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    APOLLO.handleAjaxError(jqXHR, textStatus, errorThrown);
                });
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
        }
    } (jQuery);

    $(function () {
        settingContext.IsSimpleMode = true;
        EDITPOSITIONTAKING.init(settingContext, positionTakingSetting);
    }).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    
}
