var WINLOSS = function ($) {
    return {
        FromDate: null,
        ToDate: null,

        init: function () {
            WINLOSS.FromDate = $("#FromDate");
            WINLOSS.ToDate = $("#ToDate");
            // Agent WinLoss Report
            $("#btnSubmit").bind('click', function () {
                var fromDate = new Date(WINLOSS.FromDate.val());
                var toDate = new Date(WINLOSS.ToDate.val());

                if (toDate < fromDate) {
                    noty({ text: LANGUAGES.MessageValidateDate, type: 'warning' });
                }
                else {
                    var url = APOLLO.resolveUrl('Reports/AgentWinLoss?fromDate=' + WINLOSS.FromDate.val() + "&toDate=" + WINLOSS.ToDate.val());
                    location.href = url;
                }
            });

            $(".navigation").bind('click', function () {
                var url = APOLLO.resolveUrl('Reports/AgentWinLoss?id=' + $(this).attr("agentid") + "&name=" + $(this).html() 
                    + '&fromDate=' + WINLOSS.FromDate.val() + "&toDate=" + WINLOSS.ToDate.val());
                location.href = url;
            });
        }

    };
} (jQuery);

$(document).ready(function () {
    WINLOSS.init();
    APOLLO.initPageInfo();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);
