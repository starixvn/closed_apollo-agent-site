/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var MEMBERLIST = function ($) {
    return {
        AgentId: null,
        init: function () {
            MEMBERLIST.AgentId = $("#gridId").attr("agentid");
            $("#currusername").html($("#gridId").attr("username"));
            MEMBERLIST.loadMemberList();
            MEMBERLIST.bindingNavigation();
        },
        bindingNavigation: function () {
            $("[name='navigation']").unbind('click').bind('click', function () {
                location.href = APOLLO.resolveUrl("Account/Agentlist?agentid=" + $(this).attr('agentid'));
                return false;
            });
        },
        bindingQuickLink: function () {
            $("#quickLinkAgentList").unbind('click').bind('click', function () {
                location.href = APOLLO.resolveUrl("Account/AgentList?agentid=" + MEMBERLIST.AgentId);
            });
        },
        bindingSearch: function () {
            $("#Search").unbind('click').bind('click', function () {
                MEMBERLIST.loadMemberList(MEMBERLIST.AgentId);
            });
        },
        bindingEdit: function () {
            $("[name='Edit']").mouseover(function () {
                var pos = $(this).offset();
                var eWidth = $(this).outerWidth();
                var mWidth = $("#menuedit").outerWidth();
                var left = (pos.left + eWidth) + "px";
                var top = 3 + pos.top + "px";
                $("#commission").attr("custid", $(this).attr('custid'));
                $("#betsetting").attr("custid", $(this).attr('custid'));

                $("#menuedit").css({
                    position: 'absolute',
                    zIndex: 5000,
                    left: left,
                    top: top
                });
                $("#menuedit").show();
            }).mouseout(function () {
                $("#menuedit").hide();
            });
            $("#commission").unbind('click').bind('click', function () {
                console.log($(this).attr("custid"));
                // Code here
            });
            $("#betsetting").unbind('click').bind('click', function () {
                console.log($(this).attr("custid"));
                // Code here
            });
            $("#menuedit").mouseover(function () {
                $(this).show();
            }).mouseout(function () {
                $(this).hide();
            });
        },
        loadMemberList: function () {
            $.ajax({
                type: "POST",
                url: APOLLO.resolveUrl("Account/GetMemberList"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ AgentId: MEMBERLIST.AgentId, Username: $("#txtUserName").val() }),
                timeout: 120000
            })
                .done(function (result) {
                    MEMBERLIST.AgentId = result.CurrentAgentId;
                    MEMBERLIST.initMemberList(result);
                    MEMBERLIST.bindingEdit();
                    MEMBERLIST.bindingQuickLink();
                    MEMBERLIST.bindingSearch();
                })
                .fail(function (event, jqXHR, ajaxSettings) {
                    APOLLO.handleAjaxError(event, jqXHR, ajaxSettings);
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
                                            { HeaderText: LANGUAGES.Edit, Name: "Edit", MergeRowNumber: 2 },
                                            { HeaderText: LANGUAGES.Username, Name: "Username", MergeRowNumber: 2 },
                                            { HeaderText: LANGUAGES.Status, Name: "Status", MergeRowNumber: 2 },
                                            { HeaderText: LANGUAGES.Group, Name: "UserGroup", MergeRowNumber: 2 },

                                            { HeaderText: LANGUAGES.AgentCommission, Name: "AgentGroup", MergeColNumber: 4 },
                                            { HeaderText: LANGUAGES.MemberCommission, Name: "CustomerGroup", MergeColNumber: 4 },
                                            { HeaderText: LANGUAGES.SoccerShare, Name: "Handicap", MergeColNumber: 4 }

                                        ]
                                 }
                        ],
                columns: [
                         { HeaderText: "#", Width: 30, Name: "No", HeaderAlign: "center", CellAlign: "center" },
                         { HeaderText: LANGUAGES.Edit, Width: 30, Name: "Edit", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return "<a href='#' class='edit-setting' name='Edit' custid=" + cell.Value.CustId + " ></a>";
                             }
                         },
                         { HeaderText: LANGUAGES.Username, Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left" },
                         { HeaderText: LANGUAGES.Status, Width: 50, Name: "Status", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return (cell.Value.Status == 0 ? "Active" : (cell.Value.Status == 1 ? "Suspended" : "Closed"));
                             }
                         },
                         { HeaderText: LANGUAGES.Group, Width: 150, Name: "UserGroup", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },

                         { HeaderText: LANGUAGES.Group, Width: 150, Name: "AgentGroup", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.OneXTwo, Width: 150, Name: "AgentOneXTwo", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.Other, Width: 150, Name: "AgentOther", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.NumberGame, Width: 150, Name: "AgentNumberGame", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },

                         { HeaderText: LANGUAGES.Group, Width: 150, Name: "CustomerGroup", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.OneXTwo, Width: 150, Name: "CustomerOneXTwo", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.Other, Width: 150, Name: "CustomerOther", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.NumberGame, Width: 150, Name: "CustomerNumberGame", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },

                         { HeaderText: LANGUAGES.Handicap, Width: 150, Name: "Handicap", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.OverUnder, Width: 150, Name: "OverUnder", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.HandicapLive, Width: 150, Name: "HandicapLive", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.OverUnderLive, Width: 150, Name: "OverUnderLive", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" }

                        ],
                bodyRows: bodyData,
                showFooter: false,
                fullHeight: true,
                gridExpandHeight: 250,
                numberOfFrozenColumn: 3,
                marginRight: 0,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: memberList.ItemsPerPage,
                    currentPage: memberList.CurrentPage,
                    totalItem: memberList.TotalItem,
                    itemsPerPageArray: [50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                },
                showColumnsShowHide: false,
                columnsNeedHide: [8, 12]
            };
            function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
                $.ajax({
                    type: "POST",
                    url: APOLLO.resolveUrl("Account/GetMemberList"),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ agentId: MEMBERLIST.AgentId, pageIndex: page_index, pageSize: itemsPerPage }),
                    timeout: 120000
                })
                    .done(function (result) {
                        gridOption.pagerOption.itemsPerPage = itemsPerPage;
                        gridOption.pagerOption.currentPage = page_index;
                        gridOption.pagerOption.totalItem = result.TotalItem;
                        MEMBERLIST.AgentId = result.CurrentAgentId;

                        MEMBERLIST.initMemberList(result);
                    })
                    .fail(function (event, jqXHR, ajaxSettings) {
                        APOLLO.handleAjaxError(event, jqXHR, ajaxSettings);
                    });
                return false;
            }
            $("#gridId").fanexGrid(gridOption);
        }
    };
} (jQuery);

$(document).ready(function () {
    MEMBERLIST.init();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);