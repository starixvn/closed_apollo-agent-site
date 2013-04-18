/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var AGENTLIST = function ($) {
    return {
        AgentId: null,
        IsSuper: false,
        init: function () {
            AGENTLIST.AgentId = $("#gridId").attr("agentid");
            AGENTLIST.loadNavigation();
            AGENTLIST.loadAgentList();
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

                    var $quickLinkEditSetting = $("#quickLinkEditSetting");
                    for (var i = 0; i < rowLength; i++) {
                        if (i == rowLength - 1) {
                            template += "<span>" + result[i].Username + "</span>";
                            $("#currUsername").html(result[i].Username);
                            AGENTLIST.IsSuper = (result[i].Level == 0);

                            $quickLinkEditSetting.attr("agentid", result[i].Id);
                            $quickLinkEditSetting.attr("agentlevel", result[i].Level);
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
            $('[name="viewAgent"]').unbind('click').bind('click', function () {
                AGENTLIST.AgentId = $(this).attr('agentid');
                AGENTLIST.loadAgentList();
                return false;
            });
        },
        bindingViewMember: function () {
            $('[name="viewMember"]').unbind('click').bind('click', function () {
                AGENTLIST.AgentId = $(this).attr('agentid');

                location.href = APOLLO.resolveUrl("Account/MemberList?agentId=" + AGENTLIST.AgentId + "&username=" + $(this).attr('username') + "&agentlevel=" + $(this).attr('agentlevel'));
                return false;
            });
        },
        bindingNavigation: function () {
            $("[name='navigation']").unbind('click').bind('click', function () {
                AGENTLIST.AgentId = $(this).attr('agentid');
                AGENTLIST.loadAgentList();
                return false;
            });
        },
        bindingQuickLink: function () {
            $("#quickLinkCustomerList").unbind('click').bind('click', function () {
                location.href = APOLLO.resolveUrl("Account/MemberList?agentid=" + AGENTLIST.AgentId);
                return false;
            });
        },
        bindingSearch: function () {
            $("#Search").unbind('click').bind('click', function () {
                AGENTLIST.loadAgentList(AGENTLIST.AgentId);
            });
        },
        initMenuPopup: function (els) {
            var menuEdit = $("#menuedit");

            var commission = $("#commission");
            var betsetting = $("#betsetting");
            var sharesetting = $("#sharesetting");
            els.unbind('click').bind('click', function () {
                AGENTLIST.editAgentSetting($(this));
            });

            els.mouseover(function () {
                var $this = $(this);
                var pos = $this.offset();
                var eWidth = $this.outerWidth();

                var mWidth = menuEdit.outerWidth();
                var left = (pos.left + eWidth) + "px";
                var top = 3 + pos.top + "px";

                var agentid = $this.attr('agentid');
                var agentlevel = $this.attr('agentlevel');

                commission.attr("agentId", agentid);
                betsetting.attr("agentId", agentid);
                sharesetting.attr("agentId", agentid);

                commission.attr("agentlevel", agentlevel);
                betsetting.attr("agentlevel", agentlevel);
                sharesetting.attr("agentlevel", agentlevel);

                menuEdit.css({
                    position: 'absolute',
                    zIndex: 5000,
                    left: left,
                    top: top
                });
                menuEdit.show();
            }).mouseout(function () {
                menuEdit.hide();
            });
            commission.unbind('click').bind('click', function () {
                var $this = $(this);
                var url = APOLLO.resolveUrl("CustomerSetting/EditCommissionSetting?custId=" + $this.attr("agentid")
                + "&agentLevel=" + $this.attr("agentlevel"));
                AGENTLIST.currentPopUp = APOLLO.openDialogFrame(url, '', {
                    width: 1015,
                    height: 300,
                    position: { my: "top+50", at: "center top", of: window },
                    close: function (event, ui) {
                    }
                });
            });
            betsetting.unbind('click').bind('click', function () {
                var $this = $(this);
                var url = APOLLO.resolveUrl("CustomerSetting/EditBetSetting?custId=" + $this.attr("agentid")
                + "&agentLevel=" + $this.attr("agentlevel"));
                AGENTLIST.currentPopUp = APOLLO.openDialogFrame(url, '', {
                    width: 1015,
                    height: 600,
                    position: { my: "top+50", at: "center top", of: window },
                    close: function (event, ui) {
                    }
                });
            });
            sharesetting.unbind('click').bind('click', function () {
                var $this = $(this);
                var url = APOLLO.resolveUrl("CustomerSetting/EditPositionTaking?custId=" + $this.attr("agentid")
                + "&agentLevel=" + $this.attr("agentlevel"));
                AGENTLIST.currentPopUp = APOLLO.openDialogFrame(url, '', {
                    width: 1020,
                    height: 800,
                    position: { my: "top+10", at: "center top", of: window },
                    close: function (event, ui) {
                    }
                });
            });
            menuEdit.mouseover(function () {
                $(this).show();
            }).mouseout(function () {
                $(this).hide();
            });
        },
        editAgentSetting: function ($this) {
            var url = APOLLO.resolveUrl("CustomerSetting/EditAllSetting?custId=" + $this.attr("agentid")
                + "&agentLevel=" + $this.attr("agentlevel"));
            AGENTLIST.currentPopUp = APOLLO.openDialogFrame(url, '', {
                width: 1020,
                height: 800,
                position: { my: "top+10", at: "center top", of: window },
                close: function (event, ui) {
                }
            });
        },
        bindingEdit: function () {
            $("[name='Edit']").unbind('click').bind('click', function () {
                // Edit Agent
                AGENTLIST.editAgentSetting($(this));
            });
            $("#quickLinkEditSetting").unbind('click').bind('click', function () {
                // Edit Uplilne Agent
                AGENTLIST.editAgentSetting($(this));
            });
            // AGENTLIST.initMenuPopup( $( "[name='Edit']" ) );
            // AGENTLIST.initMenuPopup( $( "#quickLinkEditSetting" ) );
        },
        loadAgentList: function () {
            $.ajax({
                type: "POST",
                url: APOLLO.resolveUrl("Account/GetAgentList"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify({ AgentId: AGENTLIST.AgentId, Username: $("#txtUserName").val() == BASELANGUAGES.EnterUsername ? "" : $("#txtUserName").val() }),
                timeout: 120000
            })
                .done(function (result) {
                    AGENTLIST.AgentId = result.CurrentAgentId;
                    AGENTLIST.loadNavigation();
                    AGENTLIST.initAgentList(result);

                    AGENTLIST.bindingQuickLink();
                    AGENTLIST.bindingSearch();
                })
                .fail(function (event, jqXHR, ajaxSettings) {
                    APOLLO.handleAjaxError(event, jqXHR, ajaxSettings);
                });
            if (AGENTLIST.currentPopUp != null) {
                    APOLLO.closeDialog(AGENTLIST.currentPopUp);
                }
        },

        initAgentList: function (agentList) {
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
                            { Name: "Status", Value: current.Status },
                            { Name: "BetStatus", Value: current.BetStatus },
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
                currentHeaderColumns[0].Columns.push({ HeaderText: LANGUAGES.SoccerDeadballHDP, Name: "HandicapGivenShare", MergeRowNumber: 2 });
            }
            else {
                currentHeaderColumns[0].Columns.push({ HeaderText: LANGUAGES.SoccerDeadballHDP, Name: "HandicapGivenShare", MergeColNumber: 2 });
            }

            var currentContentColumns = [
                         { HeaderText: "#", Width: 30, Name: "No", HeaderAlign: "center", CellAlign: "center" },
                         { HeaderText: LANGUAGES.Edit, Width: 30, Name: "Edit", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return "<a href='#' class='edit-setting icon-pencil' name='Edit' agentid=" + cell.Value.AgentId + " agentlevel=" + cell.Value.AgentLevel + " ></a>";
                             }
                         },
                         { HeaderText: LANGUAGES.Username, Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left" },
                         { HeaderText: LANGUAGES.Status, Width: 100, Name: "Status", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return (cell.Value.Status == 1 ? LANGUAGES.Active : (cell.Value.Status == 2 ? LANGUAGES.Stopped : LANGUAGES.Locked));
                             }
                         },
                          { HeaderText: LANGUAGES.BetStatus, Width: 100, Name: "BetStatus", HeaderAlign: "center", CellAlign: "center",
                              CustomCellRender: function (cell) {
                                  return (cell.Value.BetStatus == 1 ? LANGUAGES.Active : (cell.Value.BetStatus == 2 ? LANGUAGES.Stopped : LANGUAGES.Locked));
                              }
                          },
                         { HeaderText: LANGUAGES.NumberOfAgent, Width: 70, Name: "NumberOfAgent", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return (cell.Value.NumberOfAgent != 0 ? "<a name='viewAgent' href='#' agentId=" + cell.Value.AgentId + ">" + cell.Value.NumberOfAgent + "</a>" : cell.Value.NumberOfAgent);
                             }
                         },
                         { HeaderText: LANGUAGES.NumberOfMember, Width: 70, Name: "NumberOfMember", HeaderAlign: "center", CellAlign: "center",
                             CustomCellRender: function (cell) {
                                 return (cell.Value.NumberOfMember != 0 ? "<a href='#' name='viewMember' agentlevel=" + cell.Value.AgentLevel + " agentId=" + cell.Value.AgentId + " username=" + cell.Value.Username + ">" + cell.Value.NumberOfMember + "</a>" : cell.Value.NumberOfMember);
                             }
                         },
                         { HeaderText: LANGUAGES.GroupA, Width: 100, Name: "GroupA", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.GroupB, Width: 100, Name: "GroupB", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.GroupC, Width: 100, Name: "GroupC", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.GroupD, Width: 100, Name: "GroupD", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.OneXTwo, Width: 100, Name: "OneXTwo", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                         { HeaderText: LANGUAGES.Other, Width: 100, Name: "Other", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" }
                        ];
            if (AGENTLIST.IsSuper) {
                currentContentColumns.push({ HeaderText: LANGUAGES.GivenShare, Width: 100, Name: "HandicapGivenShare", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" });
            }
            else {
                currentContentColumns.push({ HeaderText: LANGUAGES.GivenShare, Width: 100, Name: "HandicapGivenShare", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" });
                currentContentColumns.push({ HeaderText: LANGUAGES.TakenShare, Width: 100, Name: "HandicapTakenShare", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" });
            }
            var gridOption = {
                customizeHeader: true,
                groupHeaderRows: currentHeaderColumns,
                columns: currentContentColumns,
                bodyRows: bodyData,
                showFooter: false,
                fullHeight: true,
                gridExpandHeight: 250,
                numberOfFrozenColumn: 3,
                marginRight: 0,
                showPager: true,
                customPager: true,
                showColumnsShowHide: !AGENTLIST.IsSuper,
                columnsNeedHide: [6],
                pagerOption: {
                    itemsPerPage: agentList.ItemsPerPage,
                    currentPage: agentList.CurrentPage,
                    totalItem: agentList.TotalItem,
                    itemsPerPageArray: [10, 20, 50, 100, 200, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                },
                bodyRowHeight: 28
            };
            function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
                $.ajax({
                    type: "POST",
                    url: APOLLO.resolveUrl("Account/GetAgentList"),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
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

            AGENTLIST.bindingViewAgent();
            AGENTLIST.bindingViewMember();
            AGENTLIST.bindingEdit();
        }
    };
} (jQuery);

$( document ).ready( function () {
    AGENTLIST.init();
} ).ajaxStart( $.blockUI ).ajaxStop( $.unblockUI );