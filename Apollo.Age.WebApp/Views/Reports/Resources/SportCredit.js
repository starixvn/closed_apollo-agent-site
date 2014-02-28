/*global APOLLO*/
'use strict';
var SPORTCREDIT = (function ($) {
    return {
        init: function () {
            $("#btnSubmit").on("click", function () {
                $.blockUI();
                var url = APOLLO.resolveUrl("Reports/SportCredit?username=" + $("#Username").val().trim());
                location.href = url;
            });

            $("#Username").on("keypress", function (e) {
                var code = e.which;
                if (code === 13) {
                    $.blockUI();
                    var url = APOLLO.resolveUrl("Reports/SportCredit?username=" + $("#Username").val().trim());
                    location.href = url;
                }
            });
            $('#export-excel').on('click', function () {
                $.blockUI();
                var url = APOLLO.resolveUrl('Reports/ExportSportCredit?username=' + $("#Username").val().trim());
                location.href = url;
                $.unblockUI();
            });
        }
    };
} (jQuery));

$(document).ready(function () {
    SPORTCREDIT.init();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);