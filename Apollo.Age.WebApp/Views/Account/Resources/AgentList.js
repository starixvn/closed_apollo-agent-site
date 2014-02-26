/// <reference path="../../../_GlobalResources/Js/jquery.js" />
/*global DEFAULTS*/
/*global APOLLO*/
/*global BASELANGUAGES*/
/*global LANGUAGES*/
/*global ACCOUNTLIST*/

'use strict';
var AGENTLIST = (function ($) {
    return {
        AgentId: null,
        Username: null,
        AgentLevel: null,
        IsSuper: false,
        IsMainShareHolder: false,
        IsShareHolder: false,
        init: function () {
            AGENTLIST.AgentId = DEFAULTS.CurrentAgentId;
            AGENTLIST.loadNavigation();
        },
        initQuickLinkEditSetting: function (agent, hideSetting) {
            var $quickLinkEditSetting = $("#quickLinkEditSetting");

            if (DEFAULTS.IsMaster) {
                $quickLinkEditSetting.removeClass("edit-setting ").addClass("edit-setting icon-pencil");
            }
            else {
                $quickLinkEditSetting.removeClass("edit-setting icon-pencil").addClass("edit-setting icon-th-large");
            }

            $quickLinkEditSetting.attr("agentId", agent.Id);
            $quickLinkEditSetting.attr("agentLevel", agent.Level);
            $quickLinkEditSetting.attr("username", agent.Username);
            if (hideSetting) {
                $quickLinkEditSetting.css('display', 'none');
            }
            else {
                $quickLinkEditSetting.css('display', '');
            }
        },
        loadNavigation: function () {
            $.ajax({
                type: "POST",
                url: APOLLO.resolveUrl("Account/GetAgentDownlinePath"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ agentId: AGENTLIST.AgentId }),
                async: false,
                timeout: 120000
            })
                .done(function (result) {
                    var rowLength = result.length;
                    var template = '';

                    for (var i = 0; i < rowLength; i++) {
                        if (i === rowLength - 1) {
                            AGENTLIST.IsSuper = (result[i].Level === 0);
                            AGENTLIST.IsMainShareHolder = (result[i].Level === 1);
                            AGENTLIST.IsShareHolder = (result[i].Level === 2);

                            AGENTLIST.Username = result[i].Username;
                            AGENTLIST.AgentLevel = result[i].Level;
                            AGENTLIST.AgentId = result[i].Id;

                            template += "<li class='currentUser'><span>" + AGENTLIST.Username + "</span></li>";
                            $("#currUsername").html(AGENTLIST.Username);
                            AGENTLIST.initQuickLinkEditSetting(result[i], AGENTLIST.AgentId === result[0].Id);

                        }
                        else {
                            template += "<li><a href='#' name='navigation' agentId='" + result[i].Id + "' agentlevel='" + result[i].Level + "' >" + result[i].Username + "</a></li>";
                        }
                    }
                    $("#divnavigation").html(template);

                    AGENTLIST.bindingNavigation();
                    AGENTLIST.loadAgentList();
                    AGENTLIST.bindingQuickLink();
                    AGENTLIST.bindingSearch();
                })
                .fail(function (event, jqXHR, ajaxSettings) {
                    APOLLO.handleAjaxError(event, jqXHR, ajaxSettings);
                });
        },
        bindingViewAgent: function () {
            $('[name="viewAgent"]').on('click', function () {
                AGENTLIST.AgentId = $(this).attr('agentId');

                $("#txtUserName").val(BASELANGUAGES.EnterUsername);
                AGENTLIST.loadNavigation();
                return false;
            });
        },
        bindingViewMember: function () {
            $('[name="viewMember"]').on('click', function () {
                AGENTLIST.AgentId = $(this).attr('agentid');

                location.href = APOLLO.resolveUrl("Account/MemberList?agentId=" + AGENTLIST.AgentId);
                return false;
            });
        },
        bindingNavigation: function () {
            $("[name='navigation']").on('click', function () {
                AGENTLIST.AgentId = $(this).attr('agentid');
                $("#txtUserName").val('');

                AGENTLIST.loadNavigation();
                return false;
            });
        },
        bindingQuickLink: function () {
            $("#quickLinkCustomerList").on('click', function () {
                location.href = APOLLO.resolveUrl("Account/MemberList?agentId=" + AGENTLIST.AgentId);
                return false;
            });
        },
        bindingSearch: function () {
            $("#Search").on('click', function () {
                AGENTLIST.loadAgentList(AGENTLIST.AgentId);
            });
        },
        editAgentSetting: function ($this) {
            var url = APOLLO.resolveUrl("CustomerSetting/EditAgentSetting?agentId=" + $this.attr("agentId")
                + "&agentLevel=" + $this.attr("agentLevel"));
            AGENTLIST.currentPopUp = APOLLO.openDialogFrame(url, LANGUAGES.PopUpTitle + $this.attr('username'), {
                width: 980,
                height: 470,
                position: { my: "top+25%", at: "center top", of: window }
            });
        },
        bindingEdit: function () {
            $("[name='Edit']").on('click', function () {
                // Edit Agent
                AGENTLIST.editAgentSetting($(this));
            });
            $("#quickLinkEditSetting").on('click', function () {
                // Edit Uplilne Agent
                AGENTLIST.editAgentSetting($(this));
            });
        },
        loadAgentList: function () {
            $.ajax({
                type: "POST",
                url: APOLLO.resolveUrl("Account/GetAgentList"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify({
                    agentId: AGENTLIST.AgentId,
                    Username: APOLLO.htmlDecode($("#txtUserName").val().trim()) === APOLLO.htmlDecode(BASELANGUAGES.EnterUsername) ? "" : APOLLO.htmlDecode($("#txtUserName").val().trim())
                }),
                timeout: 120000
            })
                .done(function (result) {
                    AGENTLIST.AgentId = result.CurrentAgentId;

                    AGENTLIST.initAgentList(result);
                })
                .fail(function (event, jqXHR, ajaxSettings) {
                    APOLLO.handleAjaxError(event, jqXHR, ajaxSettings);
                });
            if (AGENTLIST.currentPopUp != null) {
                APOLLO.closeDialog(AGENTLIST.currentPopUp);
            }
        },

        initAgentList: function (agentList) {
            var icon = "icon-th-large";
            var tooltip = LANGUAGES.ViewSetting;
            if (DEFAULTS.IsMaster) {
                icon = "icon-pencil";
                tooltip = LANGUAGES.EditSetting;
            }

            var bodyData = [];

            var rowLength = agentList.ListData.length;
            var counter = (agentList.CurrentPage * agentList.ItemsPerPage);
            for (var i = 0; i < rowLength; i++) {
                var current = agentList.ListData[i];
                counter++;
                var currentRow = {
                    Columns:
                        [
                            { Name: "No", Value: counter },
                            { Name: "Edit", Value: current },
                            { Name: "Username", Value: current.Username },
                            { Name: "Status", Value: current },
                            { Name: "BetStatus", Value: current },
                            { Name: "NumberOfAgent", Value: current },
                            { Name: "NumberOfMember", Value: current },
                            { Name: "GroupA", Value: current.GroupA * 100 },
                            { Name: "GroupB", Value: current.GroupB * 100 },
                            { Name: "GroupC", Value: current.GroupC * 100 },
                            { Name: "GroupD", Value: current.GroupD * 100 },
                            { Name: "OneXTwo", Value: current.OneXTwo * 100 },
                            { Name: "Other", Value: current.Other * 100 }
                        ]
                };

                if (AGENTLIST.IsSuper) {
                    currentRow.Columns.push({ Name: "HandicapGivenShare", Value: current.HandicapGivenShare / 100 });
                }
                else {
                    currentRow.Columns.push({ Name: "HandicapTakenShare", Value: current.HandicapTakenShare / 100 });
                    currentRow.Columns.push({ Name: "HandicapGivenShare", Value: current.HandicapGivenShare / 100 });
                }

                bodyData.push(currentRow);
            }

            var currentHeaderColumns = [{
                Columns: [
                            { HeaderText: "#", Name: "No", MergeRowNumber: 2 },
                            { HeaderText: LANGUAGES.Edit, Name: "Edit", MergeRowNumber: 2 },
                            { HeaderText: LANGUAGES.Username, Name: "Username", MergeRowNumber: 2 },
                            { HeaderText: LANGUAGES.Status, Name: "Status", MergeRowNumber: 2 },
                            { HeaderText: LANGUAGES.BetStatus, Name: "BetStatus", MergeRowNumber: 2 },
                            { HeaderText: LANGUAGES.NumberOfAgent, Name: "NumberOfAgent", MergeRowNumber: 2 },
                            { HeaderText: LANGUAGES.NumberOfMember, Name: "NumberOfMember", MergeRowNumber: 2 },
                            { HeaderText: LANGUAGES.Commission, Name: "GroupA", MergeColNumber: 6 }

                        ]
            }];
            if (AGENTLIST.IsSuper) {
                currentHeaderColumns[0].Columns.push({ HeaderText: LANGUAGES.SoccerDeadballHDPGivenShare, Name: "HandicapGivenShare", MergeRowNumber: 2 });
            }
            else {
                currentHeaderColumns[0].Columns.push({ HeaderText: LANGUAGES.SoccerDeadballHDP, Name: "HandicapTakenShare", MergeColNumber: 2 });
            }

            var currentContentColumns = [
                         { HeaderText: "#", Width: 30, Name: "No", HeaderAlign: "center", CellAlign: "center" },
                         { HeaderText: LANGUAGES.Edit, Width: 30, Name: "Edit", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return "<a href='#' class='edit-setting " + icon + "' title='" + tooltip + "' name='Edit' username=" + cell.Value.Username + " agentid=" + cell.Value.AgentId + " agentlevel=" + cell.Value.AgentLevel + " ></a>";
                             }
                         },
                         { HeaderText: LANGUAGES.Username, Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left" },
                         { HeaderText: LANGUAGES.Status, Width: 80, Name: "Status", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return ACCOUNTLIST.initStatus(cell.Value.Status, cell.Value.UplineStatus);
                             }
                         },
                          { HeaderText: LANGUAGES.BetStatus, Width: 80, Name: "BetStatus", HeaderAlign: "center", CellAlign: "center",
                              CustomCellRender: function (cell) {
                                  return ACCOUNTLIST.initStatus(cell.Value.BetStatus, cell.Value.UplineBetStatus);
                              }
                          },
                         { HeaderText: LANGUAGES.NumberOfAgent, Width: 60, Name: "NumberOfAgent", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return (cell.Value.NumberOfAgent !== 0 ? "<a name='viewAgent' href='#' title='" + LANGUAGES.ViewAgentDownline + "' agentlevel=" + cell.Value.AgentLevel + " agentId=" + cell.Value.AgentId + " username=" + cell.Value.Username + ">" + cell.Value.NumberOfAgent + "</a>" : cell.Value.NumberOfAgent);
                             }
                         },
                         { HeaderText: LANGUAGES.NumberOfMember, Width: 70, Name: "NumberOfMember", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return (cell.Value.NumberOfMember !== 0 ? "<a href='#' title='" + LANGUAGES.ViewMember + "' name='viewMember' agentId=" + cell.Value.AgentId + ">" + cell.Value.NumberOfMember + "</a>" : cell.Value.NumberOfMember);
                             }
                         },
                         { HeaderText: LANGUAGES.GroupA, Width: 80, Name: "GroupA", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.GroupB, Width: 80, Name: "GroupB", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.GroupC, Width: 80, Name: "GroupC", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.GroupD, Width: 80, Name: "GroupD", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.OneXTwo, Width: 80, Name: "OneXTwo", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.Other, Width: 80, Name: "Other", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" }
                        ];
            if (AGENTLIST.IsSuper) {
                currentContentColumns.push({ HeaderText: LANGUAGES.SoccerDeadballHDPGivenShare, Width: 130, Name: "HandicapGivenShare", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" });
            }
            else {
                currentContentColumns.push({ HeaderText: LANGUAGES.TakenShare, Width: 100, Name: "HandicapTakenShare", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" });
                currentContentColumns.push({ HeaderText: LANGUAGES.GivenShare, Width: 100, Name: "HandicapGivenShare", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" });
            }
            var gridOption = {
                customizeHeader: true,
                groupHeaderRows: currentHeaderColumns,
                columns: currentContentColumns,
                bodyRows: bodyData,
                showFooter: false,
                fullHeight: true,
                gridExpandHeight: 190,
                numberOfFrozenColumn: 3,
                marginRight: 0,
                showPager: true,
                customPager: true,
                showColumnsShowHide: (!AGENTLIST.IsSuper && !AGENTLIST.IsMainShareHolder),
                columnsNeedHide: [6],
                showDistinctRow: true,
                pagerOption: {
                    itemsPerPage: agentList.ItemsPerPage,
                    currentPage: agentList.CurrentPage,
                    totalItem: agentList.TotalItem,
                    itemsPerPageArray: [10, 20, 50, 100, 200, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                },
                bodyRowHeight: 15,
                headerColumnHeight: 15,
                showTitle: true,
                titleInfo:
                {
                    text: '<ul><li id="current"><a href="#">' + LANGUAGES.AgentList + '</a></li><li><a href="#" id="quickLinkCustomerList">' + LANGUAGES.MemberList + '</a></li></ul>',
                    cssClass: 'agent-list'
                }
            };

            function ReRenderAfterPaged(pageIndex, itemsPerPage) {
                $.ajax({
                    type: "POST",
                    url: APOLLO.resolveUrl("Account/GetAgentList"),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: JSON.stringify({ agentId: AGENTLIST.AgentId, pageIndex: pageIndex, pageSize: itemsPerPage }),
                    timeout: 120000
                })
                    .done(function (result) {
                        gridOption.pagerOption.itemsPerPage = itemsPerPage;
                        gridOption.pagerOption.currentPage = pageIndex;
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

            //// Show hide quicklink
            if (AGENTLIST.IsSuper || AGENTLIST.IsMainShareHolder || AGENTLIST.IsShareHolder) {
                $("#quickLinkCustomerList").hide();
            }
            else {
                $("#quickLinkCustomerList").show();
            }

            AGENTLIST.bindingViewAgent();
            AGENTLIST.bindingViewMember();
            AGENTLIST.bindingEdit();
        }
    };
} (jQuery));

$(document).ready(function () {
    AGENTLIST.init();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);