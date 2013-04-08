/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var AGENTLIST = function ($) {
    return {
        init: function () {
            $.ajax({
                type: "POST",
                url: parent.baseURL + "/UserList/AgentList",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ 'agentId': null }),
                timeout: 120000,
                success: function (result) {
                    AGENTLIST.initGrid(result);
                },
                error: function (event, jqXHR, ajaxSettings) {
                },
                complete: function () {
                }
            });
        },
        initGrid: function (agentList) {
            var bodyData = [];

            var rowLength = agentList.ListData.length;
            for (var i = 0; i < rowLength; i++) {
                var current = agentList.ListData[i];
                bodyData.push({
                    Columns:
                        [
                            { Name: "Username", Value: current.UserName },
                            { Name: "Status", Value: current },
                            { Name: "NumberOfAgent", Value: current.ShareAmount },
                            { Name: "NumberOfMember", Value: current },
                            { Name: "GroupA", Value: current.GameShareAmount },
                            { Name: "GroupB", Value: current.GameShareAmount },
                            { Name: "GroupC", Value: current.GameShareAmount },
                            { Name: "GroupD", Value: current.GameShareAmount },
                            { Name: "OneXTwo", Value: current.GameShareAmount },
                            { Name: "Other", Value: current.GameShareAmount },
                            { Name: "NumberGame", Value: current.GameShareAmount },
                            { Name: "Handicap", Value: current.GameShareAmount },
                            { Name: "OverUnder", Value: current.GameShareAmount },
                            { Name: "HandicapLive", Value: current.GameShareAmount },
                            { Name: "OverUnderLive", Value: current.GameShareAmount }
                        ]
                });
            }

            var gridOption = {
                customizeHeader: true,
                groupHeaderRows: [
                                 {
                                     Columns: [
                                            { HeaderText: "Username", Name: "Username", MergeRowNumber: 2 },
                                            { HeaderText: "Status", Name: "Status", MergeRowNumber: 2 },
                                            { HeaderText: "NumberOfAgent", Name: "NumberOfAgent", MergeRowNumber: 2 },
                                            { HeaderText: "NumberOfMember", Name: "NumberOfMember", MergeRowNumber: 2 },
                                            { HeaderText: "Commission(%)", Name: "GroupA", MergeColNumber: 7 },
                                            { HeaderText: "Soccer Share(%)", Name: "Handicap", MergeColNumber: 4 }

                                        ]
                                 }
                        ],
                columns: [

                         { HeaderText: "User", Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left" },
                         { HeaderText: "Status", Width: 150, Name: "Status", HeaderAlign: "center", CellAlign: "left" },
                         { HeaderText: "Agents", Width: 150, Name: "NumberOfAgent", HeaderAlign: "center", CellAlign: "left",
                             CustomCellRender: function (cell) {
                                 return (cell.Value.Outstanding != 0 ? "<a href=''>" + cell.Value.Outstanding + "</a>" : cell.Value.Outstanding);
                             }
                         },
                         { HeaderText: "Members", Width: 150, Name: "NumberOfMember", HeaderAlign: "center", CellAlign: "left",
                             CustomCellRender: function (cell) {
                                 return (cell.Value.Outstanding != 0 ? "<a href=''>" + cell.Value.Outstanding + "</a>" : cell.Value.Outstanding);
                             }
                         },
                         { HeaderText: "GroupA", Width: 150, Name: "GroupA", HeaderAlign: "center", CellAlign: "left", FormatNumber: "0,0.00" },
                         { HeaderText: "GroupB", Width: 150, Name: "GroupB", HeaderAlign: "center", CellAlign: "left", FormatNumber: "0,0.00" },
                         { HeaderText: "GroupC", Width: 150, Name: "GroupC", HeaderAlign: "center", CellAlign: "left", FormatNumber: "0,0.00" },
                         { HeaderText: "GroupD", Width: 150, Name: "GroupD", HeaderAlign: "center", CellAlign: "left", FormatNumber: "0,0.00" },
                         { HeaderText: "1X2", Width: 150, Name: "OneXTwo", HeaderAlign: "center", CellAlign: "left", FormatNumber: "0,0.00" },
                         { HeaderText: "Other", Width: 150, Name: "Other", HeaderAlign: "center", CellAlign: "left", FormatNumber: "0,0.00" },
                         { HeaderText: "NumberGame", Width: 150, Name: "NumberGame", HeaderAlign: "center", CellAlign: "left", FormatNumber: "0,0.00" },
                         { HeaderText: "HDP", Width: 50, Name: "Handicap", HeaderAlign: "center", CellAlign: "left", FormatNumber: "0,0.00" },
                         { HeaderText: "O/U", Width: 50, Name: "OverUnder", HeaderAlign: "center", CellAlign: "left", FormatNumber: "0,0.00" },
                         { HeaderText: "HDP Live", Width: 50, Name: "HandicapLive", HeaderAlign: "center", CellAlign: "left", FormatNumber: "0,0.00" },
                         { HeaderText: "O/U Live", Width: 50, Name: "OverUnderLive", HeaderAlign: "center", CellAlign: "left", FormatNumber: "0,0.00" }

                        ],
                bodyRows: bodyData,
                showFooter: false,
                fullHeight: true,
                gridExpandHeight: 250,
                marginRight: 0
            };

            $("#gridId").fanexGrid(gridOption);
        }
    };
} (jQuery);

$(document).ready(function () {
    AGENTLIST.init();
});