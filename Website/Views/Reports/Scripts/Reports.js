var REPORTS = {}; // Global namespace for common functions

if (typeof jQuery != 'undefined') {
    // Functions in this namespace require jquery and jquery-ui
    REPORTS = function ($) {
        return {
            init: function () {
                $("#AgentOutstanding").bind('click', function () {
                    $.get('agentoutstanding', { 'agentId': $("#AgentOutstanding").attr("agentid") })
                        .done(function (data) {
                            $('#divDetail').html(data);
                        });
                });

                $("#MemberOutstanding").bind('click', function () {
                    //                    $.get('memberoutstanding', { 'agentId': $("#MemberOutstanding").attr("agentid") })
                    //                        .done(function (data) {
                    //                            $('#divDetail').html(data);
                    //                        });
                    $.ajax({
                        type: "POST",
                        url: "memberoutstanding",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: { 'agentId': $("#MemberOutstanding").attr("agentid") },
                        timeout: 120000,
                        success: function (result) {
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
            ViewAgent: function (agentId) {
                location.href = "outstanding?agentid=" + agentId;
            },
            initGrid: function (outstandingData) {
                var bodyData = [];
                var rowLength = outstandingData.length;
                for (var i = 0; i < rowLength; i++) {
                    var current = outstandingData[i];
                    bodyData.push({
                        Columns:
                        [
                            { Name: "Agent", Value: current.UserName },
                            { Name: "Outstanding", Value: current.Outstanding },
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
                         { HeaderText: "Agent", Width: 160, Name: "Agent", HeaderAlign: "center", CellAlign: "center" },
                        { HeaderText: "Outstanding", Width: 70, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right" },
                        { HeaderText: "Share Amount ", Width: 100, Name: "ShareAmount", HeaderAlign: "center", CellAlign: "right" },
                        { HeaderText: "Outstanding", Width: 100, Name: "OutstandingGame", HeaderAlign: "center", CellAlign: "right" },
                        { HeaderText: "Share Amount ", Width: 160, Name: "ShareAmountGame", HeaderAlign: "center", CellAlign: "right" }
                        ],
                    bodyRows: bodyData,
                    fullHeight: false,
                    gridHeight: 400
                };

                $("#gridId").fanexGrid(gridOption);
            }
        };
    } (jQuery);
}
$(document).ready(function () {
    REPORTS.init();
});