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
            WINLOSS.hdFromDate = $("#hdFromDate");
            WINLOSS.hdToDate = $("#hdToDate");
            WINLOSS.hdSportType = $("#hdSportType");
            WINLOSS.hdBettype = $("#hdBettype");
            WINLOSS.Yesterday = $("#yesterday");
            WINLOSS.Tomorrow = $("#tomorrow");
            WINLOSS.FromThisWeek = $("#thisWeekFrom");
            WINLOSS.ToThisWeek = $("#thisWeekTo");
            WINLOSS.FromLastWeek = $("#lastWeekFrom");
            WINLOSS.ToLastWeek = $("#lastWeekTo");
            WINLOSS.FromThisPeriod = $("#thisPeriodFrom");
            WINLOSS.ToThisPeriod = $("#thisPeriodTo");
            WINLOSS.FromLastPeriod = $("#lastPeriodFrom");
            WINLOSS.ToLastPeriod = $("#lastPeriodTo");
            WINLOSS.SportName = $("#SportName");
            WINLOSS.BettypeName = $("#BettypeName");
            WINLOSS.AgentId = $("#agentId");
            WINLOSS.AgentName = $("#agentName");


            WINLOSS.ddlSport.bind('change', function () {
                var $this = $(this);
                var valueChange = $this.val();
                var url = APOLLO.resolveUrl('Reports/CreateBetType?isIncludeParlay=false');

                if (valueChange == "-99") {
                    url = APOLLO.resolveUrl('Reports/CreateParlayBettype');
                }
                else if (valueChange == "0") {
                    url = APOLLO.resolveUrl('Reports/CreateBetType?isIncludeParlay=true');
                }
                else {
                    url = APOLLO.resolveUrl('Reports/CreateBetType?isIncludeParlay=false');
                }

                $.getJSON(url)
                        .done(function (data) {
                            WINLOSS.ddlBettype.empty();
                            $.each(data, function (i, item) {
                                WINLOSS.ddlBettype.append($('<option>', {
                                    value: item.Id,
                                    text: item.Name
                                }));
                            });
                        });

            });

            // Agent WinLoss Report
            $("#btnSubmit").bind('click', function () {
                if (!APOLLO.IsValidDate(WINLOSS.FromDate.val()) || !APOLLO.IsValidDate(WINLOSS.ToDate.val())) {
                    alert("Invalid date");
                    return;
                }

                if (WINLOSS.FromDate.val() < WINLOSS.FromLastPeriod.val() || WINLOSS.ToDate.val() < WINLOSS.FromLastPeriod.val()) {
                    alert(LANGUAGES.MessageValidateDateRange + " " + WINLOSS.FromLastPeriod.val());
                }
                else {
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
                    + WINLOSS.hdFromDate.val() + "&ToDate=" + WINLOSS.hdToDate.val()
                    + "&AgentId=" + $(this).attr("data-agentid")
                    + "&AgentName=" + $(this).attr("data-agentname")
                    + "&Sporttypes=" + WINLOSS.hdSportType.val()
                    + "&Bettypes=" + WINLOSS.hdBettype.val());
                location.href = url;
            });

            $(".view-memberwinloss").on("click", function () {
                $.blockUI();
                var url = APOLLO.resolveUrl("Reports/MemberWinLoss?FromDate="
                    + WINLOSS.hdFromDate.val() + "&ToDate=" + WINLOSS.hdToDate.val()
                    + "&AgentId=" + $(this).attr("data-agentid")
                    + "&AgentName=" + $(this).attr("data-agentname")
                    + "&Sporttypes=" + WINLOSS.hdSportType.val()
                    + "&Bettypes=" + WINLOSS.hdBettype.val());
                location.href = url;
            });

            $(".navigation").on('click', function () {
                $.blockUI();
                var url = APOLLO.resolveUrl('Reports/AgentWinLoss?AgentId=' + $(this).attr("data-agentid") + "&AgentName=" + $(this).html()
                    + '&FromDate=' + WINLOSS.hdFromDate.val() + "&ToDate=" + WINLOSS.hdToDate.val()
                    + "&Sporttypes=" + WINLOSS.hdSportType.val()
                    + "&Bettypes=" + WINLOSS.hdBettype.val());
                location.href = url;
            });

            $(".view-betlist").on("click", function () {
                WINLOSS.sportTypeName = WINLOSS.SportName.val();
                WINLOSS.betTypeName = WINLOSS.BettypeName.val();
                var url = APOLLO.resolveUrl("Betlist/Winloss?fromDate="
                    + WINLOSS.hdFromDate.val() + "&toDate=" + WINLOSS.hdToDate.val()
                    + "&custId=" + $(this).attr("data-memberid")
                    + "&sportType=" + WINLOSS.hdSportType.val()
                    + "&betType=" + WINLOSS.hdBettype.val()
                    + "&type=report");
                // resolution for screens
                var resolution = APOLLO.resolutionForScreens(1030, 630);
                APOLLO.openDialogFrame(url, LANGUAGES.BetListString, {
                    width: resolution.width,
                    height: resolution.height,
                    position: { my: "top+20", at: "center top", of: window }
                });
            });

            $("#btnExcelAgentWinLoss").on("click", function () {
                $.blockUI();
                var url = APOLLO.resolveUrl('Reports/ExportAgentWinLoss?AgentId=' + WINLOSS.AgentId.val()
                    + "&AgentName=" + WINLOSS.AgentName.val()
                    + "&FromDate=" + WINLOSS.hdFromDate.val() + "&ToDate=" + WINLOSS.hdToDate.val()
                    + "&Sporttypes=" + WINLOSS.hdSportType.val()
                    + "&Bettypes=" + WINLOSS.hdBettype.val());
                location.href = url;
                $.unblockUI();
            });

            $("#btnExcelMemberWinLoss").on("click", function () {
                $.blockUI();
                var url = APOLLO.resolveUrl('Reports/ExportMemberWinLoss?AgentId=' + WINLOSS.AgentId.val()
                    + "&AgentName=" + WINLOSS.AgentName.val()
                    + "&FromDate=" + WINLOSS.hdFromDate.val() + "&ToDate=" + WINLOSS.hdToDate.val()
                    + "&Sporttypes=" + WINLOSS.hdSportType.val()
                    + "&Bettypes=" + WINLOSS.hdBettype.val());
                location.href = url;
                $.unblockUI();
            });

            $(".tblWinLoss tbody tr td:not(:first-child)").on("click", function () {
                var $this = $(this);

                if ($this.parent().hasClass("rowactivecolor")) {
                    $this.parent().removeClass("rowactivecolor");
                }
                else {
                    $this.parent().addClass("rowactivecolor");
                }
            });

            $(".dateRange").on('click', function () {
                /* Range type value
                + 1 : Today
                + 2 : Yesterday
                + 3 : Tomorrow
                + 4 : This Week
                + 5 : Last Week
                */
                var rangeType = $(this).attr('data-range');

                switch (rangeType) {
                    case "1":
                        WINLOSS.FromDate.val(WINLOSS.Today.val());
                        WINLOSS.ToDate.val(WINLOSS.Today.val());
                        break;
                    case "2":
                        WINLOSS.FromDate.val(WINLOSS.Yesterday.val());
                        WINLOSS.ToDate.val(WINLOSS.Yesterday.val());
                        break;
                    case "4":
                        WINLOSS.FromDate.val(WINLOSS.FromThisWeek.val());
                        WINLOSS.ToDate.val(WINLOSS.ToThisWeek.val());
                        break;
                    case "5":
                        WINLOSS.FromDate.val(WINLOSS.FromLastWeek.val());
                        WINLOSS.ToDate.val(WINLOSS.ToLastWeek.val());
                        break;
                    case "6":
                        WINLOSS.FromDate.val(WINLOSS.FromThisPeriod.val());
                        WINLOSS.ToDate.val(WINLOSS.ToThisPeriod.val());
                        break;
                    case "7":
                        WINLOSS.FromDate.val(WINLOSS.FromLastPeriod.val());
                        WINLOSS.ToDate.val(WINLOSS.ToLastPeriod.val());
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
    // $.blockUI();
    WINLOSS.init();
    APOLLO.initPageInfo();
    // $.unblockUI();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);
