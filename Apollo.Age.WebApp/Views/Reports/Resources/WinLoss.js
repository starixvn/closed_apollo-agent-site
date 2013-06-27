var WINLOSS = function ($) {
    return {
        FromDate: null,
        ToDate: null,

        init: function () {
            WINLOSS.BalanceUpDay = $("#balanceUpDay")
            WINLOSS.FromDate = $("#FromDate");
            WINLOSS.ToDate = $("#ToDate");
            WINLOSS.Today = $("#today");
            WINLOSS.Yesterday = $("#yesterday");
            WINLOSS.Tomorrow = $("#tomorrow");
            WINLOSS.FromThisWeek = $("#thisWeekFrom");
            WINLOSS.ToThisWeek = $("#thisWeekTo");
            WINLOSS.FromLastWeek = $("#lastWeekFrom");
            WINLOSS.ToLastWeek = $("#lastWeekTo");
            // Agent WinLoss Report
            $("#btnSubmit").bind('click', function () {
                var fromDate = new Date(WINLOSS.FromDate.val());
                var toDate = new Date(WINLOSS.ToDate.val());
                var balanceUpDate = new Date(WINLOSS.BalanceUpDay.val());

                if (fromDate < balanceUpDate || toDate < balanceUpDate) {
                    alert(LANGUAGES.MessageValidateWithArchivedDate);
                }
                else {
                    var url = APOLLO.resolveUrl('Reports/AgentWinLoss?fromDate=' + WINLOSS.FromDate.val() + "&toDate=" + WINLOSS.ToDate.val());
                    location.href = url;
                }
            });

            $("#btnCancel").bind('click', function () {
                var url = APOLLO.resolveUrl("_betList/BetListTransaction.aspx?type=cancel");
                location.href = url;
            });

            $(".navigation").bind('click', function () {
                var url = APOLLO.resolveUrl('Reports/AgentWinLoss?id=' + $(this).attr("agentid") + "&name=" + $(this).html()
                    + '&fromDate=' + WINLOSS.FromDate.val() + "&toDate=" + WINLOSS.ToDate.val());
                location.href = url;
            });

            $(".dateRange").bind('click', function () {
                /* Range type value
                + 1 : Today
                + 2 : Yesterday
                + 3 : Tomorrow
                + 4 : This Week
                + 5 : Last Week
                */
                var rangeType = $(this).attr('range');

                switch (rangeType) {
                    case "1":
                        WINLOSS.FromDate.val(WINLOSS.Today.val());
                        WINLOSS.ToDate.val(WINLOSS.Today.val());
                        break;
                    case "2":
                        WINLOSS.FromDate.val(WINLOSS.Yesterday.val());
                        WINLOSS.ToDate.val(WINLOSS.Yesterday.val());
                        break;
                    case "3":
                        WINLOSS.FromDate.val(WINLOSS.Tomorrow.val());
                        WINLOSS.ToDate.val(WINLOSS.Tomorrow.val());
                        break;
                    case "4":
                        WINLOSS.FromDate.val(WINLOSS.FromThisWeek.val());
                        WINLOSS.ToDate.val(WINLOSS.ToThisWeek.val());
                        break;
                    case "5":
                        WINLOSS.FromDate.val(WINLOSS.FromLastWeek.val());
                        WINLOSS.ToDate.val(WINLOSS.ToLastWeek.val());
                        break;
                    default:
                        WINLOSS.FromDate.val(WINLOSS.Today.val());
                        WINLOSS.ToDate.val(WINLOSS.Today.val());
                        return;
                }

            });
        }

    };
} (jQuery);

$(document).ready(function () {
    WINLOSS.init();
    APOLLO.initPageInfo();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);
