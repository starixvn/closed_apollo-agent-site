var REPORTS = function ($) {
    return {
        init: function () {
            // Agent WinLoss Report
            $("#btnSubmit").bind('click', function () {

                console.log($("#FromDate").val());
                console.log($("#ToDate").val());
            });
        }
        
    };
} (jQuery);

$(document).ready(function () {
    REPORTS.init();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);