var WINLOSS = function ($) {
    return {
        FromDate: null,
        ToDate: null,

        init: function () {
            console.log($("#FromDate").val());
            FromDate = $("#FromDate");
            ToDate = $("#ToDate");
            // Agent WinLoss Report
            $("#btnSubmit").bind('click', function () {
                var url = APOLLO.resolveUrl('Reports/AgentWinLoss?fromDate=' + FromDate.val() + "&toDate=" + ToDate.val());
                location.href = url;
            });

            $(".navigation").bind('click', function () {
                var url = APOLLO.resolveUrl('Reports/AgentWinLoss?agentId=' + $(this).attr("agentid") + '&fromDate=' + FromDate.val() + "&toDate=" + ToDate.val());
                location.href = url;
            });
        }

    };
} (jQuery);

$(document).ready(function () {
    WINLOSS.init();
});