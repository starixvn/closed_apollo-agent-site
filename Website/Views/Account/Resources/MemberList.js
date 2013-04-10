/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var MEMBERLIST = function ($) {
    return {
        AgentId: null,
        init: function () {
            MEMBERLIST.AgentId = $("#gridId").attr("agentid");
            MEMBERLIST.loadMemberList();
            $("[name='navigation']").bind('click', function () {
                location.href = APOLLO.resolveUrl("/Account/Agentlist?agentid=" + $(this).attr('agentid'));
                return false;
            });
        },
        loadMemberList: function () {
            $.ajax({
                type: "POST",
                url: APOLLO.resolveUrl("/Account/GetCustomerList"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ AgentId: MEMBERLIST.AgentId, Username: $("#txtUserName").val() }),
                timeout: 120000,
                success: function (result) {
                    MEMBERLIST.AgentId = result.CurrentAgentId;
                    MEMBERLIST.initMemberList(result);

                    $("#quickLinkAgentList").bind('click', function () {
                        location.href = APOLLO.resolveUrl("/Account/AgentList?agentid=" + MEMBERLIST.AgentId);
                    });

                    $("#Search").bind('click', function () {
                        MEMBERLIST.loadMemberList(MEMBERLIST.AgentId);
                    });
                },
                error: function (event, jqXHR, ajaxSettings) {
                },
                complete: function () {
                }
            });
        },
        initMemberList: function (memberList) {
            var bodyData = [];

            var rowLength = memberList.ListData.length;
            var counter = (memberList.CurrentPage * memberList.ItemsPerPage);
            for (var i = 0; i < rowLength; i++) {
                var current = memberList.ListData[i];
                counter++;
                bodyData.push({
                    Columns:
                        [
                            { Name: "No", Value: counter },
                            { Name: "Edit", Value: current },
                            { Name: "Username", Value: current.Username },
                            { Name: "Status", Value: current },
                            { Name: "UserGroup", Value: current.UserGroup },
                            { Name: "AgentGroup", Value: current.AgentGroup },
                            { Name: "AgentOneXTwo", Value: current.AgentDiscountOneXTwo },
                            { Name: "AgentOther", Value: current.AgentDiscountOther },
                            { Name: "AgentNumberGame", Value: current.AgentDiscountNumberGame },
                            { Name: "CustomerGroup", Value: current.CustomerGroup },
                            { Name: "CustomerOneXTwo", Value: current.CustomerDiscountOneXTwo },
                            { Name: "CustomerOther", Value: current.CustomerDiscountOther },
                            { Name: "CustomerNumberGame", Value: current.CustDiscountNumberGame },
                            { Name: "Handicap", Value: current.Handicap },
                            { Name: "OverUnder", Value: current.OverUnder },
                            { Name: "HandicapLive", Value: current.HandicapLive },
                            { Name: "OverUnderLive", Value: current.OverUnderLive }
                        ]
                });
            }

            var gridOption = {
                customizeHeader: true,
                groupHeaderRows: [
                                 {
                                     Columns: [
                                            { HeaderText: "#", Name: "No", MergeRowNumber: 2 },
                                            { HeaderText: "Edit", Name: "Edit", MergeRowNumber: 2 },
                                            { HeaderText: "Username", Name: "Username", MergeRowNumber: 2 },
                                            { HeaderText: "Status", Name: "Status", MergeRowNumber: 2 },
                                            { HeaderText: "UserGroup", Name: "UserGroup", MergeRowNumber: 2 },

                                            { HeaderText: "Agent Commission(%)", Name: "AgentGroup", MergeColNumber: 4 },
                                            { HeaderText: "Member Commission(%)", Name: "CustomerGroup", MergeColNumber: 4 },
                                            { HeaderText: "Soccer Share(%)", Name: "Handicap", MergeColNumber: 4 }

                                        ]
                                 }
                        ],
                columns: [
                         { HeaderText: "#", Width: 50, Name: "No", HeaderAlign: "center", CellAlign: "center" },
                         { HeaderText: "Edit", Width: 50, Name: "Edit", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return "Edit";
                             }
                         },
                         { HeaderText: "User", Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left" },
                         { HeaderText: "Status", Width: 150, Name: "Status", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return (cell.Value.Status == 0 ? "Active" : (cell.Value.Status == 1 ? "Suspended" : "Closed"));
                             }
                         },
                         { HeaderText: "Group", Width: 150, Name: "UserGroup", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },

                         { HeaderText: "Group", Width: 150, Name: "AgentGroup", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: "1x2", Width: 150, Name: "AgentOneXTwo", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: "Other", Width: 150, Name: "AgentOther", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: "Number Game", Width: 150, Name: "AgentNumberGame", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },

                         { HeaderText: "Group", Width: 150, Name: "CustomerGroup", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: "1x2", Width: 150, Name: "CustomerOneXTwo", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: "Other", Width: 150, Name: "CustomerOther", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: "Number Game", Width: 150, Name: "CustomerNumberGame", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },

                         { HeaderText: "HDP", Width: 150, Name: "Handicap", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: "O/U", Width: 150, Name: "OverUnder", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: "HDP Live", Width: 150, Name: "HandicapLive", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: "O/U Live", Width: 150, Name: "OverUnderLive", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" }

                        ],
                bodyRows: bodyData,
                showFooter: false,
                fullHeight: true,
                gridExpandHeight: 250,
                marginRight: 0,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: memberList.ItemsPerPage,
                    currentPage: memberList.CurrentPage,
                    totalItem: memberList.TotalItem,
                    itemsPerPageArray: [50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };
            function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
                $.ajax({
                    type: "POST",
                    url: APOLLO.resolveUrl("/Account/GetCustomerList"),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ agentId: MEMBERLIST.AgentId, pageIndex: page_index, pageSize: itemsPerPage }),
                    timeout: 120000,
                    success: function (result) {
                        gridOption.pagerOption.itemsPerPage = itemsPerPage;
                        gridOption.pagerOption.currentPage = page_index;
                        gridOption.pagerOption.totalItem = result.TotalItem;
                        MEMBERLIST.AgentId = result.CurrentAgentId;

                        MEMBERLIST.initMemberList(result);
                    },
                    error: function (event, jqXHR, ajaxSettings) {
                    },
                    complete: function () {
                    }
                });
                return false;
            }
            $("#gridId").fanexGrid(gridOption);

            $('[name="viewMember"]').bind('click', function () {
                MEMBERLIST.AgentId = $(this).attr('agentid');
                MEMBERLIST.loadAgentList();
            });
        }
    };
} (jQuery);

$(document).ready(function () {
    MEMBERLIST.init();
});