/* File Created: March 13, 2013 */

if (typeof jQuery != 'undefined') {
    var EDITCOMMISSION = function ($) {
        return {
            settingContext: null,
            commissionSettingElement: null,
            commissionSetting: null,
            init: function (settingContext, customerCommission) {

                EDITCOMMISSION.settingContext = settingContext;
                Fanex.CustSetting.CURRENT_CONTEXT = new Fanex.CustSetting.Context(settingContext);
                EDITCOMMISSION.commissionSetting = customerCommission;
                EDITCOMMISSION.commissionSettingElement = new Fanex.CustSetting.CommissionSetting(customerCommission);
                var commissionSettingContainer = document.getElementById('commissionSettingContainer');
                commissionSettingContainer.innerHTML = "";
                EDITCOMMISSION.commissionSettingElement.Render(commissionSettingContainer);

                //// Register Event
                $("#btnSubmitFooter").click(function () {
                    EDITCOMMISSION.updatecommissionSetting();
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
                    if (data.ErrCode == 0) {
                        noty({ text: data.ErrMsg, type: 'success' });
                    } else {
                        noty({ text: data.ErrMsg, type: 'error' });
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    APOLLO.handleAjaxError(jqXHR, textStatus, errorThrown);
                });
            },
            completeUpdate: function (result) {
                //                var errMsg = result.errMsg;
                //                if (result.errCode == 0) {
                //                    age.UnLock();
                //                    //            AddPopupHeight(50);
                //                    ageMsg.Show(errMsg, 1);
                //                    window.scroll(0, 0);
                //                    setTimeout(function () {
                //                        if (isReloadParent > 0) {
                //                            parent.window.location.reload();
                //                        }
                //                        else {
                //                            window.location.reload();
                //                        }
                //                    }, 1000);

                //                    return;
                //                }
                //                else {
                //                    age.UnLock();
                //                    //            AddPopupHeight(50);
                //                    ageMsg.Show(errMsg);
                //                    window.scroll(0, 0);
                //                    return;
                //                }
            }
        }
    } (jQuery);

    $(function () {
        EDITCOMMISSION.init(settingContext, commissionSetting);
    }).ajaxStart($.blockUI).ajaxStop($.unblockUI);

}

