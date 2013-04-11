var REPORTS = function ($) {
    return {
        init: function () {
            $("#MemberOutstanding").unbind('click').bind('click', function () {
                $.ajax({
                    type: "POST",
                    url: APOLLO.resolveUrl("Reports/MemberOutstanding"),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ 'agentId': $("#MemberOutstanding").attr("agentid") }),
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

            $("[name='navigation']").unbind('click').bind('click', function () {
                location.href = APOLLO.resolveUrl("Reports/Outstanding?agentId=" + $(this).attr("agentid"));
            });

            $("[name='viewagent']").unbind('click').bind('click', function () {
                location.href = APOLLO.resolveUrl("Reports/Outstanding?agentId=" + $(this).attr("agentid"));
            });

            $("#gridId").on("click", ".act-view-betlist", REPORTS.viewBetList);
        },
        // View betlist page from report Outstanding
        viewBetList: function () {
            var username = $(this).closest('tr').children('td:first').text();
            location.href = APOLLO.resolveUrl("_Reports/BetList/OutstandingDetail.aspx?custId=" + $(this).attr("data-custid") + "&username=" + username);
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
                                            { HeaderText: LANGUAGES.Member, Name: "Member", MergeRowNumber: 2 },
                                            { HeaderText: LANGUAGES.SportBook, Name: "Outstanding", MergeColNumber: 2 },
                                            { HeaderText: LANGUAGES.NumberGame, Name: "OutstandingGame", MergeColNumber: 2 }
                                        ]
                                 }
                        ],
                columns: [

                         { HeaderText: LANGUAGES.Member, Width: 230, Name: "Member", HeaderAlign: "center", CellAlign: "left" },
                        { HeaderText: LANGUAGES.Outstanding, Width: 193, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00",
                            CustomCellRender: function (cell) {
                                return (cell.Value.Outstanding != 0 ? "<a href='#' class='act-view-betlist' data-custid='" + cell.Value.UserId + "'>" + numeral(cell.Value.Outstanding).format("0,0.00") + "</a>" : numeral(cell.Value.Outstanding).format("0,0.00"));
                            }
                        },
                        { HeaderText: LANGUAGES.ShareAmount, Width: 197, Name: "ShareAmount", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                        { HeaderText: LANGUAGES.Outstanding, Width: 195, Name: "OutstandingGame", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00",
                            CustomCellRender: function (cell) {
                                return (cell.Value.GameOutstanding != 0 ? "<a href=''>" + numeral(cell.Value.GameOutstanding).format("0,0.00") + "</a>" : numeral(cell.Value.GameOutstanding).format("0,0.00"));
                            }
                        },
                        { HeaderText: LANGUAGES.ShareAmount, Width: 180, Name: "ShareAmountGame", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" }
                        ],
                bodyRows: bodyData,
                showFooter: true,
                footerRows: footerData,
                fullHeight: true,
                gridExpandHeight: 250,
                marginRight: 0,
                showColumnsShowHide: false,
                columnsNeedHide: [3, 4]
            };

            $("#gridId").fanexGrid(gridOption);
        }
    };
} (jQuery);

$(document).ready(function () {
    REPORTS.init();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);