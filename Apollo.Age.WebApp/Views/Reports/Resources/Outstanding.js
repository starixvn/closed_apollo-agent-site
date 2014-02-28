/*global APOLLO*/
/*global LANGUAGES*/
'use strict';
var OUTSTANDING = (function ($) {
    return {
        ddlSport: null,
        ddlBettype: null,
        hdSportType: null,
        hdBettype: null,
        SportName: null,
        BettypeName: null,
        isAgentOutStanding: true,

        init: function () {
            OUTSTANDING.ddlSport = $("#ddlSport");
            OUTSTANDING.ddlBettype = $("#ddlBettype");
            OUTSTANDING.hdSportType = $("#hdSportType");
            OUTSTANDING.hdBettype = $("#hdBettype");
            OUTSTANDING.SportName = $("#SportName");
            OUTSTANDING.BettypeName = $("#BettypeName");

            OUTSTANDING.ddlSport.bind('change', function () {
                var $this = $(this);
                var valueChange = $this.val();
                var url = APOLLO.resolveUrl('Reports/CreateBetType?isIncludeParlay=false');

                if (valueChange === "-99") {
                    url = APOLLO.resolveUrl('Reports/CreateParlayBettype');
                }
                else if (valueChange === "0") {
                    url = APOLLO.resolveUrl('Reports/CreateBetType?isIncludeParlay=true');
                }
                else {
                    url = APOLLO.resolveUrl('Reports/CreateBetType?isIncludeParlay=false');
                }

                $.getJSON(url)
                        .done(function (data) {
                            OUTSTANDING.ddlBettype.empty();
                            $.each(data, function (i, item) {
                                OUTSTANDING.ddlBettype.append($('<option>', {
                                    value: item.Id,
                                    text: item.Name
                                }));
                            });
                        });

            });

            $("#MemberOutstanding").on('click', function () {
                var $this = $(this);

                $.ajax({
                    type: "POST",
                    url: APOLLO.resolveUrl("Reports/MemberOutstanding"),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        'agentId': $this.attr("data-agentid"),
                        'Sporttypes': OUTSTANDING.hdSportType.val(),
                        'Bettypes': OUTSTANDING.hdBettype.val(),
                        'mainShareHolderId': $this.attr("data-mainshareholderid"),
                        'mainShareHolderUsername': $this.attr("data-mainshareholderusername")
                    }),
                    timeout: 120000
                })
                .done(function (result) {
                    if (result.HasPermissionDownlineDetail) {
                        OUTSTANDING.initGrid(result);

                        OUTSTANDING.ddlSport.val(OUTSTANDING.hdSportType.val());
                        OUTSTANDING.ddlBettype.val(OUTSTANDING.hdBettype.val());
                        OUTSTANDING.isAgentOutStanding = false;
                    }
                    else {
                        alert(APOLLO.htmlDecode(LANGUAGES.DonwlinePermission));
                    }
                })
                .fail(function (event, jqXHR, ajaxSettings) {
                    APOLLO.handleAjaxError(event, jqXHR, ajaxSettings);
                });
                return false;
            });

            $(".navigation").on('click', function () {
                $.blockUI();
                location.href = APOLLO.resolveUrl("Reports/Outstanding?agentId=" + $(this).attr("data-agentid")
				+ "&Sporttypes=" + OUTSTANDING.hdSportType.val()
                    + "&Bettypes=" + OUTSTANDING.hdBettype.val());
            });

            $(".viewoutstading").on('click', function () {
                $.blockUI();
                location.href = APOLLO.resolveUrl("Reports/Outstanding?agentId=" + $(this).attr("data-agentid")
				+ "&Sporttypes=" + OUTSTANDING.hdSportType.val()
                    + "&Bettypes=" + OUTSTANDING.hdBettype.val()
					);
            });

            $("#btnSubmit").on('click', function () {
                $.blockUI();

                var url = APOLLO.resolveUrl('Reports/Outstanding?Sporttypes=' + OUTSTANDING.ddlSport.val()
                    + "&Bettypes=" + OUTSTANDING.ddlBettype.val());
                location.href = url;
            });


            $("#gridId").on("click", ".act-view-betlist", OUTSTANDING.viewBetList);

            $('#export-excel').bind('click', function () {
                OUTSTANDING.sportTypeName = OUTSTANDING.SportName.val();
                OUTSTANDING.betTypeName = OUTSTANDING.BettypeName.val();
                var parameters = $.param({
                    AgentId: $('#CurrentCustId').val(),
                    AgentUsername: $('.boxNavigation .currentUser span').text(),
                    SportName: escape(OUTSTANDING.sportTypeName),
                    BetTypeName: escape(OUTSTANDING.betTypeName),
                    Sporttypes: OUTSTANDING.ddlSport.val(),
                    Bettypes: OUTSTANDING.ddlBettype.val(),
                    IsAgentOutStanding: OUTSTANDING.isAgentOutStanding
                });
                var url = APOLLO.resolveUrl('Reports/ExportOutstandingReport?' + parameters);
                $('#exportExcelFrame').attr('src', url);

            });
        },

        viewBetList: function () {
            OUTSTANDING.sportTypeName = OUTSTANDING.SportName.val();
            OUTSTANDING.betTypeName = OUTSTANDING.BettypeName.val();
            var url = APOLLO.resolveUrl("BetList/Outstanding?custId=" + $(this).attr("data-custid")
            + "&sporttype=" + OUTSTANDING.hdSportType.val()
            + "&username=" + $(this).attr("data-username")
            + "&bettype=" + OUTSTANDING.hdBettype.val());
            // resolution for screens
            var resolution = APOLLO.resolutionForScreens(1030, 630);
            APOLLO.openDialogFrame(url, LANGUAGES.BetListString, {
                width: resolution.width,
                height: resolution.height,
                position: { my: "top+20", at: "center top", of: window }
            });
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
            var rowLength = outstandingData.ListOutstanding != null ? outstandingData.ListOutstanding.length : 0;
            for (var i = 0; i < rowLength; i++) {
                var current = outstandingData.ListOutstanding[i];
                bodyData.push({
                    Columns:
                        [
                            { Name: "Member", Value: current },
                            { Name: "Outstanding", Value: current.Outstanding },
                            { Name: "ShareAmount", Value: current.ShareAmount },
                            { Name: "OutstandingGame", Value: current.GameOutstanding },
                            { Name: "ShareAmountGame", Value: current.GameShareAmount }
                        ]
                });
            }
            var gridOption = {
                columns: [
                         { HeaderText: "", Width: 200, Name: "Member", HeaderAlign: "center", CellAlign: "left",
                             CustomCellRender: function (cell) {
                                 return (cell.Value.Outstanding !== 0 ? "<a href='javascript:;' class='act-view-betlist' mainShareHolderId='" + outstandingData.MainShareHolderId + "'  mainShareHolderUsername='" + outstandingData.MainShareHolderUsername + "' data-custid='" + cell.Value.UserId + "' title='" + LANGUAGES.ViewBetlist + "' data-username='" + cell.Value.UserName + "'>" + cell.Value.UserName + "</a>" : cell.Value.UserName);
                             }
                         },
                        { HeaderText: LANGUAGES.Outstanding, Width: 289, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                        { HeaderText: LANGUAGES.ShareAmount, Width: 289, Name: "ShareAmount", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                        { HeaderText: LANGUAGES.Outstanding, Width: 200, Name: "OutstandingGame", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
                        { HeaderText: LANGUAGES.ShareAmount, Width: 200, Name: "ShareAmountGame", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" }
                        ],
                bodyRows: bodyData,
                showFooter: bodyData.length > 0,
                footerRows: footerData,
                fullHeight: true,
                gridExpandHeight: 100,
                marginRight: 0,
                showColumnsShowHide: false,
                hideYBackGroundScroller: true,
                columnsNeedHide: [3, 4],
                bodyRowHeight: 15,
                headerColumnHeight: 15
            };
            $("#agentOutstanding").hide();
            $("#gridId").fanexGrid(gridOption);
            $("#navigationAgent").hide();
            $("#navigationMember").show();
        }
    };
} (jQuery));

$(document).ready(function () {
    //$.blockUI();
    OUTSTANDING.init();
    APOLLO.initPageInfo();
    //$.unblockUI();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);
