/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var MEMBERLIST = function ($) {
    return {
        AgentId: null,
        currentPopUp: null,
        AgentLevel: null,
        Username: null,
        IsShowEditSetting: true,
        init: function () {
            MEMBERLIST.AgentId = $("#gridId").attr("agentId");
            MEMBERLIST.AgentLevel = $("#gridId").attr("agentLevel");
            MEMBERLIST.Username = $("#gridId").attr("username");
            $("#currUsername").html(MEMBERLIST.Username);
            MEMBERLIST.loadMemberList();
            MEMBERLIST.bindingNavigation();
            MEMBERLIST.bindingQuickLinkEditSetting(MEMBERLIST.AgentId, MEMBERLIST.AgentLevel, MEMBERLIST.Username);
        },
        bindingQuickLinkEditSetting: function (agentId, agentLevel, username) {
            var $quickLinkEditSetting = $("#quickLinkEditSetting");
            if ($("#gridId").attr("agentId") == $("#gridId").attr("firstAgentId")) {
                $quickLinkEditSetting.css('display', 'none');
            }
            else {
                if (DEFAULTS.IsMaster) {
                    $quickLinkEditSetting.removeClass("edit-setting ").addClass("edit-setting icon-pencil");
                }
                else {
                    $quickLinkEditSetting.removeClass("edit-setting icon-pencil").addClass("edit-setting icon-th-large");
                }
            }

            $quickLinkEditSetting.attr("agentId", agentId);
            $quickLinkEditSetting.attr("agentLevel", agentLevel);
            $quickLinkEditSetting.attr("username", username);
        },
        bindingNavigation: function () {
            $("[name='navigation']").on('click', function () {
                location.href = APOLLO.resolveUrl("Account/Agentlist?agentid=" + $(this).attr('agentId'));
                return false;
            });
        },
        bindingQuickLink: function () {
            $("#quickLinkAgentList").on('click', function () {
                location.href = APOLLO.resolveUrl("Account/AgentList?agentid=" + MEMBERLIST.AgentId);
            });
        },
        bindingSearch: function () {
            $("#Search").on('click', function () {
                MEMBERLIST.loadMemberList(MEMBERLIST.AgentId);
            });
        },
        editCustomerSetting: function ($this) {
            var url = APOLLO.resolveUrl("CustomerSetting/EditMemberSetting?memberId=" + $this.attr("memberId")
                                + "&userName=" + $this.attr("username")
                                + "&userGroup=" + $this.attr("userGroup")
                                + "&agentId=" + $("#gridId").attr("agentId"));
            MEMBERLIST.currentPopUp = APOLLO.openDialogFrame(url, LANGUAGES.PopUpTitle + $this.attr("username"), {
                width: 980,
                height: 470,
                position: { my: "top+25%", at: "center top", of: window },
                close: function (event, ui) {
                }
            });
        },
        bindingEdit: function () {
            $("[name='Edit']").on('click', function () {
                MEMBERLIST.editCustomerSetting($(this));
            });
            $("#quickLinkEditSetting").on('click', function () {
                // Edit Agent
                var $this = $(this);
                var url = APOLLO.resolveUrl("CustomerSetting/EditAgentSetting?agentId=" + $this.attr("agentId")
                + "&agentLevel=" + $this.attr("agentLevel"));
                MEMBERLIST.currentPopUp = APOLLO.openDialogFrame(url, LANGUAGES.PopUpAgentTitle + $this.attr('username'), {
                    width: 980,
                    height: 470,
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
                data: JSON.stringify({ AgentId: MEMBERLIST.AgentId, Username: APOLLO.htmlDecode($("#txtUserName").val().trim()) == APOLLO.htmlDecode(BASELANGUAGES.EnterUsername) ? "" : APOLLO.htmlDecode($("#txtUserName").val().trim()) }),
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
            var icon = "icon-th-large";
            var tooltip = LANGUAGES.ViewSetting;
            if (DEFAULTS.IsMaster) {
                icon = "icon-pencil";
                tooltip = LANGUAGES.EditSetting;
            }

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
                            { Name: "BetStatus", Value: current },
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
                                 return "<a href='#' title='" + tooltip + "' class='edit-setting " + icon + "' name='Edit' userGroup=" + cell.Value.UserGroup + " username=" + cell.Value.Username + " memberId=" + cell.Value.MemberId + " ></a>";
                             }
                         },
                         { HeaderText: LANGUAGES.Username, Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left" },
                         { HeaderText: LANGUAGES.Status, Width: 50, Name: "Status", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return ACCOUNTLIST.initStatus(cell.Value.Status, cell.Value.UplineStatus);
                             }
                         },
                         { HeaderText: LANGUAGES.BetStatus, Width: 100, Name: "BetStatus", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return ACCOUNTLIST.initStatus(cell.Value.BetStatus, cell.Value.UplineBetStatus);
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
                },
                bodyRowHeight: 15,
                headerColumnHeight: 15,
                showTitle: true,
                titleInfo:
                {
                    text: '<ul><li><a href="#" id="quickLinkAgentList">' + LANGUAGES.AgentList + '</a></li><li id="current"><a href="#">' + LANGUAGES.MemberList + '</a></li></ul>',
                    cssClass: 'member-list'
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