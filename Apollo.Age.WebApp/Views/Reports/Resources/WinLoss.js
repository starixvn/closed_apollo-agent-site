var WINLOSS = function ($) {
    return {
        FromDate: null,
        ToDate: null,
        BalanceUpDay: null,
        ddlSport: null,
        ddlBettype: null,

        init: function () {
            WINLOSS.BalanceUpDay = $("#balanceUpDay")
            WINLOSS.FromDate = $("#FromDate");
            WINLOSS.ToDate = $("#ToDate");
            WINLOSS.Today = $("#today");
            WINLOSS.ddlSport = $("#ddlSport");
            WINLOSS.ddlBettype = $("#ddlBettype");
            WINLOSS.Yesterday = $("#yesterday");
            WINLOSS.Tomorrow = $("#tomorrow");
            WINLOSS.FromThisWeek = $("#thisWeekFrom");
            WINLOSS.ToThisWeek = $("#thisWeekTo");
            WINLOSS.FromLastWeek = $("#lastWeekFrom");
            WINLOSS.ToLastWeek = $("#lastWeekTo");
            // Agent WinLoss Report
            $("#btnSubmit").bind('click', function () {

                if (WINLOSS.FromDate.val() < WINLOSS.BalanceUpDay.val() || WINLOSS.ToDate.val() < WINLOSS.BalanceUpDay.val()) {
                    alert(LANGUAGES.MessageValidateWithArchivedDate + " " + WINLOSS.BalanceUpDay.val());
                }
                else 
                {
                    $.blockUI();
                    var url = APOLLO.resolveUrl('Reports/AgentWinLoss?FromDate='
                    + WINLOSS.FromDate.val() + "&ToDate=" + WINLOSS.ToDate.val()
                    + "&Sporttypes=" + WINLOSS.ddlSport.val()
                    + "&Bettypes=" + WINLOSS.ddlBettype.val());
                    location.href = url;
                }
            });

            $(".view-agentwinloss").on("click", function () {
                $.blockUI();
                var url = APOLLO.resolveUrl("Reports/AgentWinLoss?FromDate="
                    + WINLOSS.FromDate.val() + "&ToDate=" + WINLOSS.ToDate.val()
                    + "&AgentId=" + $(this).attr("agentid")
                    + "&AgentName=" + $(this).attr("agentname")
                    + "&Sporttypes=" + WINLOSS.ddlSport.val()
                    + "&Bettypes=" + WINLOSS.ddlBettype.val());
                location.href = url;
            });

            $(".view-memberwinloss").on("click", function () {
                $.blockUI();
                var url = APOLLO.resolveUrl("Reports/MemberWinLoss?FromDate="
                    + WINLOSS.FromDate.val() + "&ToDate=" + WINLOSS.ToDate.val()
                    + "&AgentId=" + $(this).attr("agentid")
                    + "&AgentName=" + $(this).attr("agentname")
                    + "&Sporttypes=" + WINLOSS.ddlSport.val()
                    + "&Bettypes=" + WINLOSS.ddlBettype.val());
                location.href = url;
            });

            $(".navigation").on('click', function () {
                $.blockUI();
                var url = APOLLO.resolveUrl('Reports/AgentWinLoss?AgentId=' + $(this).attr("agentid") + "&AgentName=" + $(this).html()
                    + '&FromDate=' + WINLOSS.FromDate.val() + "&ToDate=" + WINLOSS.ToDate.val()
                    + "&Sporttypes=" + WINLOSS.ddlSport.val()
                    + "&Bettypes=" + WINLOSS.ddlBettype.val());
                location.href = url;
            });

            $(".dateRange").on('click', function () {
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
    $.blockUI();
    WINLOSS.init();
    APOLLO.initPageInfo();
    $.unblockUI();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);
