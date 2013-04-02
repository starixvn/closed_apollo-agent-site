var REPORTS = {}; // Global namespace for common functions

if (typeof jQuery != 'undefined') {
    // Functions in this namespace require jquery and jquery-ui
    REPORTS = function ($) {
        return {
            init: function () {
                $("#AgentOutstanding").bind('click', function () {
                    $.get(parent.baseURL + '/reports/agentoutstanding', { 'agentId': $("#AgentOutstanding").attr("agentid") })
                        .done(function (data) {
                            $("#gridId").hide();
                            $('#divDetail').html(data);
                        });
                });

                $("#MemberOutstanding").bind('click', function () {
                    $.ajax({
                        type: "POST",
                        url: parent.baseURL + "/reports/memberoutstanding",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: { 'agentId': $("#MemberOutstanding").attr("agentid") },
                        timeout: 120000,
                        success: function (result) {
                            $("#divDetail").html(null);
                            REPORTS.initGrid(result);
                        },
                        error: function (event, jqXHR, ajaxSettings) {
                        },
                        complete: function () {
                        }
                    });
                    return false;
                });
            },
            viewAgent: function (agentId) {
                location.href = parent.baseURL + "/reports/outstanding?agentid=" + agentId;
            },
            initGrid: function (outstandingData) {
                var bodyData = [];
                var footerData = [];
                footerData.push({
                    Columns: [
                        { Name: "Agent", Value: "Total" },
                        { Name: "Outstanding", Value: outstandingData.Total.Outstanding },
                        { Name: "ShareAmount", Value: outstandingData.Total.ShareAmount },
                        { Name: "OutstandingGame", Value: outstandingData.Total.GameOutstanding },
                        { Name: "ShareAmountGame", Value: outstandingData.Total.GameShareAmount },
                    ]
                });
                var rowLength = outstandingData.ListOutstanding.length;
                for (var i = 0; i < rowLength; i++) {
                    var current = outstandingData.ListOutstanding[i];
                    bodyData.push({
                        Columns:
                        [
                            { Name: "Agent", Value: current.UserName },
                            { Name: "Outstanding", Value: current },
                            { Name: "ShareAmount", Value: current.ShareAmount },
                            { Name: "OutstandingGame", Value: current.GameOutstanding },
                            { Name: "ShareAmountGame", Value: current.GameShareAmount }
                        ]
                    });
                }
                var gridOption = {
                    customizeHeader: true,
                    groupHeaderRows: [
                                 {
                                     Columns: [
                                            { HeaderText: "Agent", Name: "Agent", MergeRowNumber: 2 },
                                            { HeaderText: "Sportsbook", Name: "Outstanding", MergeColNumber: 2 },
                                            { HeaderText: "Number Game ", Name: "OutstandingGame", MergeColNumber: 2 }
                                        ]
                                 }
                        ],
                    columns: [
                         { HeaderText: "Agent", Width: 200, Name: "Agent", HeaderAlign: "center", CellAlign: "center" },
                        { HeaderText: "Outstanding", Width: 150, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right",
                            CustomCellRender: function (cell) {
                                return "<a href=''>" + numeral(cell.Value.Outstanding).format("0,0.00") + "</a>";
                            }
                        },
                        { HeaderText: "Share Amount ", Width: 150, Name: "ShareAmount", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                        { HeaderText: "Outstanding", Width: 150, Name: "OutstandingGame", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                        { HeaderText: "Share Amount ", Width: 150, Name: "ShareAmountGame", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" }
                        ],
                    bodyRows: bodyData,
                    showFooter: true,
                    footerRows: footerData,
                    fullHeight: false,
                    gridHeight: 550
                };

                $("#gridId").fanexGrid(gridOption);
            }
        };
    } (jQuery);
}
$(document).ready(function () {
    REPORTS.init();
});