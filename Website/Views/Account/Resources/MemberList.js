/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var MEMBERLIST = function ($) {
    return {
        AgentId: null,
        currentPopUp: null,
        AgentLevel: null,
        Username: null,
        init: function () {
            MEMBERLIST.AgentId = $("#gridId").attr("agentId");
            MEMBERLIST.AgentLevel = $("#gridId").attr("agentLevel");
            MEMBERLIST.Username = $("#gridId").attr("username");
            $("#currUsername").html(MEMBERLIST.Username);
            MEMBERLIST.loadMemberList();
            MEMBERLIST.bindingNavigation();
            MEMBERLIST.bindingQuickiLinkEditSetting(MEMBERLIST.AgentId, MEMBERLIST.AgentLevel, MEMBERLIST.Username);
        },
        bindingQuickiLinkEditSetting: function (agentId, agentLevel, username) {
            var $quickLinkEditSetting = $("#quickLinkEditSetting");
            $quickLinkEditSetting.attr("agentId", agentId);
            $quickLinkEditSetting.attr("agentLevel", agentLevel);
            $quickLinkEditSetting.attr("username", username);
        },
        bindingNavigation: function () {
            $("[name='navigation']").unbind('click').bind('click', function () {
                location.href = APOLLO.resolveUrl("Account/Agentlist?agentid=" + $(this).attr('agentId'));
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
        editCustomerSetting: function ($this) {
            var url = APOLLO.resolveUrl("CustomerSetting/EditMemberSetting?memberId=" + $this.attr("memberId")
                                + "&userName=" + $this.attr("username")
                                + "&userGroup=" + $this.attr("userGroup")
                                + "&agentId=" + $("#gridId").attr("agentId"));
            MEMBERLIST.currentPopUp = APOLLO.openDialogFrame(url, 'Edit Member Setting - ' + $this.attr("username"), {
                width: 970,
                height: 450,
                position: { my: "top+25%", at: "center top", of: window },
                close: function (event, ui) {
                }
            });
        },
        bindingEdit: function () {
            $("[name='Edit']").unbind('click').bind('click', function () {
                MEMBERLIST.editCustomerSetting($(this));
            });
            $("#quickLinkEditSetting").unbind('click').bind('click', function () {
                // Edit Agent
                var $this = $(this);
                var url = APOLLO.resolveUrl("CustomerSetting/EditAgentSetting?agentId=" + $this.attr("agentId")
                + "&agentLevel=" + $this.attr("agentLevel"));
                MEMBERLIST.currentPopUp = APOLLO.openDialogFrame(url, 'Edit Agent Setting - ' + $this.attr('username'), {
                    width: 970,
                    height: 450,
                    position: { my: "top+25%", at: "center top", of: window },
                    close: function (event, ui) {
                    }
                });
            });
        },
        loadMemberList: function () {
            $.ajax({
                type: "POST",
                url: APOLLO.resolveUrl("Account/GetMemberList"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ AgentId: MEMBERLIST.AgentId, Username: $("#txtUserName").val().trim() == BASELANGUAGES.EnterUsername ? "" : $("#txtUserName").val().trim() }),
                timeout: 120000
            })
                .done(function (result) {
                    MEMBERLIST.AgentId = result.CurrentAgentId;
                    MEMBERLIST.initMemberList(result);

                    MEMBERLIST.bindingQuickLink();
                    MEMBERLIST.bindingSearch();
                })
                .fail(function (event, jqXHR, ajaxSettings) {
                    APOLLO.handleAjaxError(event, jqXHR, ajaxSettings);
                });
            if (MEMBERLIST.currentPopUp != null) {
                APOLLO.closeDialog(MEMBERLIST.currentPopUp);
            }
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
                            { Name: "Status", Value: current.Status },
                            { Name: "BetStatus", Value: current.BetStatus },
                            { Name: "UserGroup", Value: current.UserGroup },
                            { Name: "AgentGroup", Value: current.AgentGroup * 100 },
                            { Name: "AgentOneXTwo", Value: current.AgentDiscountOneXTwo * 100 },
                            { Name: "AgentOther", Value: current.AgentDiscountOther * 100 },
                            { Name: "MemberGroup", Value: current.MemberGroup * 100 },
                            { Name: "MemberOneXTwo", Value: current.MemberDiscountOneXTwo * 100 },
                            { Name: "MemberOther", Value: current.MemberDiscountOther * 100 },
                            { Name: "HandicapTakenShare", Value: current.HandicapTakenShare / 100 },
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
                                            { HeaderText: LANGUAGES.BetStatus, Name: "BetStatus", MergeRowNumber: 2 },
                                            { HeaderText: LANGUAGES.Group, Name: "UserGroup", MergeRowNumber: 2 },
                                            { HeaderText: LANGUAGES.AgentCommission, Name: "AgentGroup", MergeColNumber: 3 },
                                            { HeaderText: LANGUAGES.MemberCommission, Name: "MemberGroup", MergeColNumber: 3 },
                                            { HeaderText: LANGUAGES.SoccerDeadballHDPTakenShare, Name: "HandicapTakenShare", MergeRowNumber: 2 },

                                        ]
                                 }
                        ],
                columns: [
                         { HeaderText: "#", Width: 30, Name: "No", HeaderAlign: "center", CellAlign: "center" },
                         { HeaderText: LANGUAGES.Edit, Width: 30, Name: "Edit", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 var icon = "icon-pencil";
                                 var tooltip = LANGUAGES.EditSetting;
                                 if (!DEFAULTS.IsMaster) {
                                     icon = "icon-th-large";
                                     tooltip = LANGUAGES.ViewSetting;
                                 }
                                 return "<a href='#' title='" + tooltip + "' class='edit-setting " + icon + "' name='Edit' userGroup=" + cell.Value.UserGroup + " username=" + cell.Value.Username + " memberId=" + cell.Value.MemberId + " ></a>";
                             }
                         },
                         { HeaderText: LANGUAGES.Username, Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left" },
                         { HeaderText: LANGUAGES.Status, Width: 50, Name: "Status", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return (cell.Value == 1 ? "<span class='active'>" + LANGUAGES.Active + "</span>" : (cell.Value == 2 ? "<span class='stop'>" + LANGUAGES.Stopped + "</span>" : "<span class='lock'>" + LANGUAGES.Locked + "</span>"));
                             }
                         },
                         { HeaderText: LANGUAGES.BetStatus, Width: 100, Name: "BetStatus", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return (cell.Value == 1 ? "<span class='active'>" + LANGUAGES.Active + "</span>" : (cell.Value == 2 ? "<span class='stop'>" + LANGUAGES.Stopped + "</span>" : "<span class='lock'>" + LANGUAGES.Locked + "</span>"));
                             }
                         },
                         { HeaderText: LANGUAGES.Group, Width: 100, Name: "UserGroup", HeaderAlign: "center", CellAlign: "center" },

                         { HeaderText: LANGUAGES.Group, Width: 100, Name: "AgentGroup", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.OneXTwo, Width: 100, Name: "AgentOneXTwo", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.Other, Width: 100, Name: "AgentOther", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },

                         { HeaderText: LANGUAGES.Group, Width: 100, Name: "MemberGroup", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.OneXTwo, Width: 100, Name: "MemberOneXTwo", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.Other, Width: 100, Name: "MemberOther", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },

                         { HeaderText: LANGUAGES.SoccerDeadballHDPTakenShare, Width: 150, Name: "HandicapTakenShare", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },

                        ],
                bodyRows: bodyData,
                showFooter: false,
                fullHeight: true,
                gridExpandHeight: 190,
                numberOfFrozenColumn: 3,
                marginRight: 0,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: memberList.ItemsPerPage,
                    currentPage: memberList.CurrentPage,
                    totalItem: memberList.TotalItem,
                    itemsPerPageArray: [10, 20, 50, 100, 200, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                }
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

            MEMBERLIST.bindingEdit();
        }
    };
} (jQuery);

$(document).ready(function () {
    MEMBERLIST.init();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);