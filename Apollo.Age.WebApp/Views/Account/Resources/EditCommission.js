/* File Created: March 13, 2013 */
/*global APOLLO*/
'use strict';
if (typeof jQuery !== 'undefined') {
    var EDITCOMMISSION = (function ($) {
        return {
            currentIsSimpleMode: true,
            initOthers: function (isSimpleMode) {
                settingContext.IsSimpleMode = isSimpleMode;
                EDITCOMMISSION.init(settingContext, commissionSetting);
            },
            settingContext: null,
            commissionSettingElement: null,
            commissionSetting: null,
            init: function (settingContext, customerCommission, isUpdateAllSetting) {
                EDITCOMMISSION.settingContext = settingContext;
                Fanex.CustSetting.CURRENT_CONTEXT = new Fanex.CustSetting.Context(settingContext);
                EDITCOMMISSION.commissionSetting = customerCommission;
                EDITCOMMISSION.commissionSettingElement = new Fanex.CustSetting.CommissionSetting(customerCommission);
                var commissionSettingContainer = document.getElementById('commissionSettingContainer');
                commissionSettingContainer.innerHTML = "";
                EDITCOMMISSION.commissionSettingElement.Render(commissionSettingContainer);

                if (isUpdateAllSetting) { return; }

                //// Register Event
                $("#btnSubmitFooter").click(function () {
                    EDITCOMMISSION.updatecommissionSetting();
                });

                $(".setting-mode #simple-mode").unbind("click").click(function () {
                    if (EDITCOMMISSION.currentIsSimpleMode) { return; }
                    $(this).removeClass("active-mode").addClass("deactive-mode");
                    $(".setting-mode #advanced-mode").removeClass("deactive-mode").addClass("active-mode");
                    EDITCOMMISSION.currentIsSimpleMode = true;
                    EDITCOMMISSION.initOthers(true);
                });

                $(".setting-mode #advanced-mode").unbind("click").click(function () {
                    if (EDITCOMMISSION.currentIsSimpleMode === false) { return; }
                    $(this).removeClass("active-mode").addClass("deactive-mode");
                    $(".setting-mode #simple-mode").removeClass("deactive-mode").addClass("active-mode");
                    EDITCOMMISSION.currentIsSimpleMode = false;
                    EDITCOMMISSION.initOthers(false);
                });
            },
            updatecommissionSetting: function () {
                $.ajax({
                    url: APOLLO.resolveUrl('CustomerSetting/UpdateCommissionSetting'),
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        settingContext: EDITCOMMISSION.settingContext,
                        commissionSetting: EDITCOMMISSION.commissionSettingElement.Save()
                    })
                }).done(function (data) {
                    if (data.ErrCode === 0) {
                        noty({ text: data.ErrMsg, type: 'success' });
                        EDITCOMMISSION.completeUpdate();
                    } else {
                        noty({ text: APOLLO.generalErrorMsg, type: 'error' });
                        APOLLO.console.error(data.ErrMsg);
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    APOLLO.handleAjaxError(jqXHR, textStatus, errorThrown);
                });
            },
            completeUpdate: function () {
                setTimeout(function () {
                    if (typeof (parent.MEMBERLIST) !== 'undefined') {
                        parent.MEMBERLIST.loadMemberList();
                    }
                    else if (typeof (parent.AGENTLIST) !== 'undefined') {
                        parent.AGENTLIST.loadAgentList();
                    }
                }, 3000);
            }
        };
    } (jQuery));

    $(function () {
        settingContext.IsSimpleMode = isSimpleMode;
        EDITCOMMISSION.init(settingContext, commissionSetting);
    }).ajaxStart($.blockUI).ajaxStop($.unblockUI);
}
