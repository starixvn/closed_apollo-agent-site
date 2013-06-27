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
            },
            init: function (settingContext, positionTakingSetting, isUpdateAllSetting) {

                EDITPOSITIONTAKING.initOthers(settingContext.IsSimpleMode);

                //// Register Event
                $("#btnSubmitFooter").click(function () {
                    EDITPOSITIONTAKING.updatePositionTakingSetting();
                });

                $("#simple-mode").unbind("click").click(function () {
                    $(this).hide();
                    $("#page_main").removeClass("advanced-mode-width").addClass("simple-mode-width");
                    $("#advanced-mode").show();

                    EDITPOSITIONTAKING.currentIsSimpleMode = true;
                    EDITPOSITIONTAKING.initOthers(true);
                });

                $("#advanced-mode").unbind("click").click(function () {
                    $(this).hide();
                    $("#page_main").addClass("advanced-mode-width").removeClass("simple-mode-width");
                    $("#simple-mode").show();
                    
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
        settingContext.IsSimpleMode = DEFAULT_LAYOUT.isSimpleMode;
        EDITPOSITIONTAKING.init(settingContext, positionTakingSetting);
    });
    
}
