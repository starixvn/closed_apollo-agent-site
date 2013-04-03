var REPORTS = {}; // Global namespace for common functions

if (typeof jQuery != 'undefined') {
    // Functions in this namespace require jquery and jquery-ui
    REPORTS = function ($) {
        return {
            init: function () {
                $("#AgentOutstanding").bind('click', function () {
                    $.get(parent.baseURL + '/Reports/AgentOutstanding', { 'agentId': $("#AgentOutstanding").attr("agentid") })
                        .done(function (data) {
                            $("#gridId").hide();
                            $('#divDetail').html(data);
                        });
                    return false;
                });

                $("#MemberOutstanding").bind('click', function () {
                    $.ajax({
                        type: "POST",
                        url: parent.baseURL + "/Reports/MemberOutstanding",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ 'agentId': $("#MemberOutstanding").attr("agentid") }),
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
                location.href = parent.baseURL + "/Reports/Outstanding?agentId=" + agentId;
            },
            initGrid: function (outstandingData) {
                var bodyData = [];
                var footerData = [];
                footerData.push({
                    Columns: [
                        { Name: "Member", Value: "Total" },
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
                            { Name: "Member", Value: current.UserName },
                            { Name: "Outstanding", Value: current },
                            { Name: "ShareAmount", Value: current.ShareAmount },
                            { Name: "OutstandingGame", Value: current },
                            { Name: "ShareAmountGame", Value: current.GameShareAmount }
                        ]
                    });
                }
                var gridOption = {
                    customizeHeader: true,
                    groupHeaderRows: [
                                 {
                                     Columns: [
                                            { HeaderText: "Member", Name: "Member", MergeRowNumber: 2 },
                                            { HeaderText: "Sportsbook", Name: "Outstanding", MergeColNumber: 2 },
                                            { HeaderText: "Number Game ", Name: "OutstandingGame", MergeColNumber: 2 }
                                        ]
                                 }
                        ],
                    columns: [

                         { HeaderText: "Member", Width: 230, Name: "Member", HeaderAlign: "center", CellAlign: "left" },
                        { HeaderText: "Outstanding", Width: 193, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00",
                            CustomCellRender: function (cell) {
                                return (cell.Value.Outstanding != 0 ? "<a href=''>" + numeral(cell.Value.Outstanding).format("0,0.00") + "</a>" : numeral(cell.Value.Outstanding).format("0,0.00"));
                            }
                        },
                        { HeaderText: "Share Amount ", Width: 197, Name: "ShareAmount", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                        { HeaderText: "Outstanding", Width: 195, Name: "OutstandingGame", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00",
                            CustomCellRender: function (cell) {
                                return (cell.Value.GameOutstanding != 0 ? "<a href=''>" + numeral(cell.Value.GameOutstanding).format("0,0.00") + "</a>" : numeral(cell.Value.GameOutstanding).format("0,0.00"));
                            }
                        },
                        { HeaderText: "Share Amount ", Width: 180, Name: "ShareAmountGame", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" }
                        ],
                    bodyRows: bodyData,
                    showFooter: true,
                    footerRows: footerData,
                    fullHeight: true,
                    gridExpandHeight: 250,
                    marginRight: 0
                };

                $("#gridId").fanexGrid(gridOption);
            }
        };
    } (jQuery);
}
$(document).ready(function () {
    REPORTS.init();
});