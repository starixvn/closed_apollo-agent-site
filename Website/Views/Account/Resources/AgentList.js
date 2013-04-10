/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var AGENTLIST = function ($) {
    return {
        AgentId: null,
        init: function () {
            AGENTLIST.AgentId = $("#gridId").attr("agentid");
            AGENTLIST.loadAgentList();
        },
        loadNavigation: function () {
            $.ajax({
                type: "POST",
                url: APOLLO.resolveUrl("Account/GetAgentDownlinePath"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ AgentId: AGENTLIST.AgentId }),
                timeout: 120000
            })
                .done(function (result) {
                    var rowLength = result.length;
                    var template = '';

                    for (var i = 0; i < rowLength; i++) {
                        if (i == rowLength - 1) {
                            template += "<span>" + result[i].Username + "</span>";
                        }
                        else {
                            template += "<a href='#' name='navigation' agentid='" + result[i].Id + "' >" + result[i].Username + "</a>";
                            template += "<img alt='' height='12' width='12' src= '" + APOLLO.resolveUrl("../_MemberInfo/CustomerList/Resources/Images/breadcrumb.gif") + "'/>";
                        }
                    }
                    $("#divnavigation").html(template);
                    AGENTLIST.bindingNavigation();
                })
                .fail(function (event, jqXHR, ajaxSettings) {
                    APOLLO.handleAjaxError(event, jqXHR, ajaxSettings);
                });
        },
        bindingViewAgent: function () {
            $('[name="viewAgent"]').bind('click', function () {
                AGENTLIST.AgentId = $(this).attr('agentid');
                AGENTLIST.loadAgentList();
                return false;
            });
        },
        bindingViewMember: function () {
            $('[name="viewMember"]').bind('click', function () {
                AGENTLIST.AgentId = $(this).attr('agentid');
                location.href = APOLLO.resolveUrl("Account/CustomerList?agentId=" + AGENTLIST.AgentId);
                return false;
            });
        },
        bindingNavigation: function () {
            $("[name='navigation']").bind('click', function () {
                AGENTLIST.AgentId = $(this).attr('agentid');
                AGENTLIST.loadAgentList();
                return false;
            });
        },
        bindingQuickLink: function () {
            $("#quickLinkCustomerList").bind('click', function () {
                location.href = APOLLO.resolveUrl("Account/CustomerList?agentid=" + AGENTLIST.AgentId);
                return false;
            });
        },
        bindingSearch: function () {
            $("#Search").bind('click', function () {
                AGENTLIST.loadAgentList(AGENTLIST.AgentId);
            });
        },
        bindingEdit: function () {
            $("[name='Edit']").mouseover(function () {
                var pos = $(this).offset();
                var eWidth = $(this).outerWidth();
                var mWidth = $("#menuedit").outerWidth();
                var left = (pos.left + eWidth) + "px";
                var top = 3 + pos.top + "px";
                $("#commission").attr("agentId", $(this).attr('agentid'));
                $("#betsetting").attr("agentId", $(this).attr('agentid'));

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
                // Code here
            });
            $("#betsetting").unbind('click').bind('click', function () {
                // Code here
            });
            $("#menuedit").mouseover(function () {
                $(this).show();
            }).mouseout(function () {
                $(this).hide();
            });
        },
        loadAgentList: function () {
            $.ajax({
                type: "POST",
                url: APOLLO.resolveUrl("Account/GetAgentList"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ AgentId: AGENTLIST.AgentId, Username: $("#txtUserName").val() }),
                timeout: 120000
            })
                .done(function (result) {
                    AGENTLIST.AgentId = result.CurrentAgentId;
                    AGENTLIST.initAgentList(result);
                    AGENTLIST.loadNavigation();

                    AGENTLIST.bindingViewAgent();
                    AGENTLIST.bindingViewMember();
                    AGENTLIST.bindingEdit();

                    AGENTLIST.bindingQuickLink();
                    AGENTLIST.bindingSearch();
                })
                .fail(function (event, jqXHR, ajaxSettings) {
                    APOLLO.handleAjaxError(event, jqXHR, ajaxSettings);
                });
        },

        initAgentList: function (agentList) {
            var bodyData = [];

            var rowLength = agentList.ListData.length;
            var counter = (agentList.CurrentPage * agentList.ItemsPerPage);
            for (var i = 0; i < rowLength; i++) {
                var current = agentList.ListData[i];
                counter++;
                bodyData.push({
                    Columns:
                        [
                            { Name: "No", Value: counter },
                            { Name: "Edit", Value: current },
                            { Name: "Username", Value: current.Username },
                            { Name: "Status", Value: current },
                            { Name: "NumberOfAgent", Value: current },
                            { Name: "NumberOfMember", Value: current },
                            { Name: "GroupA", Value: current.GroupA },
                            { Name: "GroupB", Value: current.GroupB },
                            { Name: "GroupC", Value: current.GroupC },
                            { Name: "GroupD", Value: current.GroupD },
                            { Name: "OneXTwo", Value: current.OneXTwo },
                            { Name: "Other", Value: current.Other },
                            { Name: "NumberGame", Value: current.NumberGame },
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
                                            { HeaderText: "NumberOfAgent", Name: "NumberOfAgent", MergeRowNumber: 2 },
                                            { HeaderText: "NumberOfMember", Name: "NumberOfMember", MergeRowNumber: 2 },
                                            { HeaderText: "Commission(%)", Name: "GroupA", MergeColNumber: 7 },
                                            { HeaderText: "Soccer Share(%)", Name: "Handicap", MergeColNumber: 4 }

                                        ]
                                 }
                        ],
                columns: [
                         { HeaderText: "#", Width: 50, Name: "No", HeaderAlign: "center", CellAlign: "center" },
                         { HeaderText: "Edit", Width: 50, Name: "Edit", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return "<a href='#' name='Edit' agentid=" + cell.Value.AgentId + " >Edit</a>";
                             }
                         },
                         { HeaderText: "User", Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left" },
                         { HeaderText: "Status", Width: 150, Name: "Status", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return (cell.Value.Status == 0 ? "Active" : (cell.Value.Status == 1 ? "Suspended" : "Closed"));
                             }
                         },
                         { HeaderText: "Agents", Width: 150, Name: "NumberOfAgent", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return (cell.Value.NumberOfAgent != 0 ? "<a name='viewAgent' href='#' agentId=" + cell.Value.AgentId + ">" + cell.Value.NumberOfAgent + "</a>" : cell.Value.NumberOfAgent);
                             }
                         },
                         { HeaderText: "Members", Width: 150, Name: "NumberOfMember", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return (cell.Value.NumberOfMember != 0 ? "<a href='#' name='viewMember' agentId=" + cell.Value.AgentId + ">" + cell.Value.NumberOfMember + "</a>" : cell.Value.NumberOfMember);
                             }
                         },
                         { HeaderText: "GroupA", Width: 150, Name: "GroupA", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: "GroupB", Width: 150, Name: "GroupB", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: "GroupC", Width: 150, Name: "GroupC", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: "GroupD", Width: 150, Name: "GroupD", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: "1X2", Width: 150, Name: "OneXTwo", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: "Other", Width: 150, Name: "Other", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: "NumberGame", Width: 150, Name: "NumberGame", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
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
                    itemsPerPage: agentList.ItemsPerPage,
                    currentPage: agentList.CurrentPage,
                    totalItem: agentList.TotalItem,
                    itemsPerPageArray: [50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                },
                showColumnsShowHide: false,
                columnsNeedHide: [12]
            };
            function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
                $.ajax({
                    type: "POST",
                    url: APOLLO.resolveUrl("Account/GetAgentList"),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ agentId: AGENTLIST.AgentId, pageIndex: page_index, pageSize: itemsPerPage }),
                    timeout: 120000
                })
                    .done(function (result) {
                        gridOption.pagerOption.itemsPerPage = itemsPerPage;
                        gridOption.pagerOption.currentPage = page_index;
                        gridOption.pagerOption.totalItem = result.TotalItem;
                        AGENTLIST.AgentId = result.CurrentAgentId;
                        AGENTLIST.initAgentList(result);
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
    AGENTLIST.init();
});