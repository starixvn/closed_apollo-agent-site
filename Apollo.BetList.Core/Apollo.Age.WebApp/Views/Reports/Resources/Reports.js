var REPORTS = function ($) {
	return {
		init: function () {
			$("#MemberOutstanding").on('click', function () {
				var $this = $(this);

				$.ajax({
					type: "POST",
					url: APOLLO.resolveUrl("Reports/MemberOutstanding"),
					contentType: "application/json; charset=utf-8",
					dataType: "json",
					data: JSON.stringify({ 'agentId': $this.attr("agentid"), 'Sporttypes': $("#ddlSport").val(),
						'Bettypes': $("#ddlBettype").val(), 'mainShareHolderId': $this.attr("mainshareholderid"), 'mainShareHolderUsername': $this.attr("mainshareholderusername")
					}),
					timeout: 120000
				})
                .done(function (result) {
                	REPORTS.initGrid(result);
                })
                .fail(function (event, jqXHR, ajaxSettings) {
                	APOLLO.handleAjaxError(event, jqXHR, ajaxSettings);
                });
				return false;
			});

			$("[name='navigation']").on('click', function () {
				$.blockUI();
				location.href = APOLLO.resolveUrl("Reports/Outstanding?agentId=" + $(this).attr("agentid")
				+ "&Sporttypes=" + $("#ddlSport").val()
                    + "&Bettypes=" + $("#ddlBettype").val());
			});

			$("[name='viewagent']").on('click', function () {
				$.blockUI();
				location.href = APOLLO.resolveUrl("Reports/Outstanding?agentId=" + $(this).attr("agentid")
				+ "&Sporttypes=" + $("#ddlSport").val()
                    + "&Bettypes=" + $("#ddlBettype").val()
					);
			});

			$("#gridId").on("click", ".act-view-betlist", REPORTS.viewBetList);
		},

		// View betlist page from report Outstanding
		viewBetList: function () {
			$.blockUI();
			var username = $(this).closest('tr').children('td:first').text();
			location.href = APOLLO.resolveUrl("BetList/Outstanding?custId=" + $(this).attr("data-custid") + "&username=" + username
			+ "&sporttype=" + $("#ddlSport").val() + "&bettype=" + $("#ddlBettype").val()
			+ "&mainShareHolderId=" + $(this).attr("mainShareHolderId") + "&mainShareHolderUsername=" + $(this).attr("mainShareHolderUsername"));
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
			var rowLength =  outstandingData.ListOutstanding != null ? outstandingData.ListOutstanding.length : 0;
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
                         		return (cell.Value.Outstanding != 0 ? "<a href='javascript:;' class='act-view-betlist' mainShareHolderId='" + outstandingData.MainShareHolderId + "'  mainShareHolderUsername='" + outstandingData.MainShareHolderUsername + "' data-custid='" + cell.Value.UserId + "' title='" + LANGUAGES.ViewBetlist + "' >" + cell.Value.UserName + "</a>" : cell.Value.UserName);
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
} (jQuery);

$(document).ready(function () {
	REPORTS.init();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);