/// <reference path="../../../_GlobalResources/Js/jquery.js" />
/*global BASELANGUAGES*/
/*global APOLLO*/
'use strict';
var FUNDOUT = function ($) {
    return {
        memberId: 0,
        oldSportCredit: 0,
        newSportCredit: 0,
        init: function () {

            FUNDOUT.oldSportCredit = $('#SportCredit').val();
            FUNDOUT.memberId = $('#MemberId').val();
            FUNDOUT.bindingSubmitButton();
        },
        bindingSubmitButton: function () {
            $('#btnSubmit').on('click', function () {

                FUNDOUT.newSportCredit = $('#txtSportCredit').val();
                if (FUNDOUT.newSportCredit === '' || FUNDOUT.newSportCredit == null ||
                    FUNDOUT.newSportCredit > FUNDOUT.oldSportCredit || 
                    FUNDOUT.newSportCredit <= 0) {
                    alert(BASELANGUAGES.fundoutamountmessage.replace('{0}', FUNDOUT.oldSportCredit));
                    return;
                }
               

                $.ajax({
                    url: APOLLO.resolveUrl('/Account/PostFundOut'),
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        sportCredit: FUNDOUT.newSportCredit,
                        memberId: FUNDOUT.memberId
                    })
                }).done(function (data) {
                    if (data.ErrCode === 0) {
                        parent.location.reload();                    
                    } else {                      
                        alert(data.ErrMsg);
                    }
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    APOLLO.handleAjaxError(jqXHR, textStatus, errorThrown);
                });
            });
        }
    };
} (jQuery);

$(document).ready(function () {
    FUNDOUT.init();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);