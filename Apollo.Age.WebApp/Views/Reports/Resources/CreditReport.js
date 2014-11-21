/*global APOLLO*/
'use strict';
var CREDITREPORT = (function ($) {
    return {
        FromDate: null,
        ToDate: null,
        BalanceUpDay: null,
        ddlSport: null,
        ddlBettype: null,

        init: function () {
            CREDITREPORT.BalanceUpDay = $("#balanceUpDay");
            CREDITREPORT.FromDate = $("#FromDate");
            CREDITREPORT.Today = $("#today");
            CREDITREPORT.Yesterday = $("#yesterday");
            CREDITREPORT.hdFromDate = $("#hdFromDate");
            CREDITREPORT.FromThisWeek = $("#thisWeekFrom");
            CREDITREPORT.ToThisWeek = $("#thisWeekTo");
            CREDITREPORT.FromLastWeek = $("#lastWeekFrom");
            CREDITREPORT.ToLastWeek = $("#lastWeekTo");
            CREDITREPORT.ToDate = $("#ToDate");
            CREDITREPORT.hdToDate = $("#hdToDate");

            CREDITREPORT.ddlSport = $("#ddlSport");
            CREDITREPORT.ddlBettype = $("#ddlBettype");
            CREDITREPORT.hdSportType = $("#hdSportType");
            CREDITREPORT.hdBettype = $("#hdBettype");
            CREDITREPORT.SportName = $("#SportName");
            CREDITREPORT.BettypeName = $("#BettypeName");
            CREDITREPORT.AgentId = $("#agentId");
            CREDITREPORT.AgentName = $("#agentName");
            CREDITREPORT.IsHistoryReport = $("#isHistoryReport");

            // Agent WinLoss Report
            $("#btnSubmit").bind('click', function () {
                if (!APOLLO.IsValidDate(CREDITREPORT.FromDate.val()) || !APOLLO.IsValidDate(CREDITREPORT.ToDate.val())) {
                    alert("Invalid date");
                    return;
                }

                $.blockUI();
                var url = APOLLO.resolveUrl('Reports/CreditReport?FromDate='
                + CREDITREPORT.FromDate.val() + "&ToDate=" + CREDITREPORT.ToDate.val()
                + "&username=" + $.trim($('#username').val())
                + "&isagent=" + $('#IsAgent').val());

                location.href = url;
            });

            $(".tblWinLoss tbody tr").on("click", function () {
                var $this = $(this);

                if ($this.hasClass("rowactivecolor")) {
                    $this.removeClass("rowactivecolor");
                }
                else {
                    $this.addClass("rowactivecolor");
                }
            });

            $(".dateRange").on('click', function () {
                /* Range type value
                + 1 : Today
                + 2 : Yesterday
                + 4 : This Week
                + 5 : Last Week
                */
                var rangeType = $(this).attr('data-range');

                switch (rangeType) {
                    case "1":
                        CREDITREPORT.FromDate.val(CREDITREPORT.Today.val());
                        CREDITREPORT.ToDate.val(CREDITREPORT.Today.val());
                        break;
                    case "2":
                        CREDITREPORT.FromDate.val(CREDITREPORT.Yesterday.val());
                        CREDITREPORT.ToDate.val(CREDITREPORT.Yesterday.val());
                        break;
                    case "4":
                        CREDITREPORT.FromDate.val(CREDITREPORT.FromThisWeek.val());
                        CREDITREPORT.ToDate.val(CREDITREPORT.ToThisWeek.val());
                        break;
                    case "5":
                        CREDITREPORT.FromDate.val(CREDITREPORT.FromLastWeek.val());
                        CREDITREPORT.ToDate.val(CREDITREPORT.ToLastWeek.val());
                        break;
                    default:
                        CREDITREPORT.FromDate.val(CREDITREPORT.Today.val());
                        CREDITREPORT.ToDate.val(CREDITREPORT.Today.val());
                        return;
                }
            });

            $('#export-excel').bind('click', function () {
                var parameters = $.param({
                    FromDate: $('#FromDate').val(),
                    ToDate: $('#ToDate').val(),
                    Username: $.trim($('#username').val()),
                    IsAgent: $('#IsAgent').val()
                });
                var url = APOLLO.resolveUrl('Reports/ExportCreditReport?' + parameters);
                $('#exportExcelFrame').attr('src', url);
            });
        }
    };
} (jQuery));

$(document).ready(function () {
    CREDITREPORT.init();
    APOLLO.initPageInfo();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);