var WINLOSS = function ($) {
    return {
        FromDate: null,
        ToDate: null,

        init: function () {
            FromDate = $("#FromDate");
            ToDate = $("#ToDate");
            // Agent WinLoss Report
            $("#btnSubmit").bind('click', function () {
                var fromDate = new Date(FromDate.val());
                var toDate = new Date(ToDate.val());

                if (toDate < fromDate) {
                    noty({ text: LANGUAGES.MessageValidateDate, type: 'warning' });
                }
                else {
                    var url = APOLLO.resolveUrl('Reports/AgentWinLoss?fromDate=' + FromDate.val() + "&toDate=" + ToDate.val());
                    location.href = url;
                }
            });

            $(".navigation").bind('click', function () {
                var url = APOLLO.resolveUrl('Reports/AgentWinLoss?id=' + $(this).attr("agentid") + "&name=" + $(this).html() + '&fromDate=' + FromDate.val() + "&toDate=" + ToDate.val());
                location.href = url;
            });
        }

    };
} (jQuery);

$(document).ready(function () {
    WINLOSS.init();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);
