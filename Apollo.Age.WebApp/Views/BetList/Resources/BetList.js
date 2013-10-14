function ViewResult(matchid, race, bettype, sporttype, refno, username, winlostdate, refno_mixparlay, league, is_outright) {
	if (bettype == 9 || bettype == 29) {//view mix parlay & combination mix parlay
		ViewMixParlayResult(refno_mixparlay, refno);
	} else {
		ViewSportBookResult(matchid, sporttype, refno);
	}
}

function ViewMixParlayResult(refno_mixparlay, refno) {
	var width = 760,
		height = 450,
		params = '?refNo=' + refno_mixparlay,
		detailTitle = $('#' + refno).find('a:first').text();

	APOLLO.openDialogFrame(APOLLO.resolveUrl("~/Results/ParlayResult" + params),
		detailTitle, { width: width, height: height });
}

function ViewSportBookResult(matchId, sportId, refno) {
	var width = 730,
		height = 250,
		params = '?matchId= ' + matchId + '&sportId=' + sportId,
		detailTitle = $('#' + refno).find('.league').html().replace(/<\/?[^>]+>/gi, "").replace("&nbsp;", " - ");

	APOLLO.openDialogFrame(APOLLO.resolveUrl("~/Results/ResultByMatch" + params),
		detailTitle, { width: width, height: height });
}

function showMP(transid) {
	var divEvent = document.getElementById('divEvent_' + transid);
	if (divEvent.style.display == 'none') {
		if (divEvent.innerHTML == "") {
			ajax.Request(APOLLO.resolveUrl("~/BetList/MixParlay"), { method: 'get', parameters: 'transid=' + transid,
				onSuccess: function (data) {
					if (data.errCode == 500) {
						divEvent.innerHTML = data.errMsg;
						divEvent.style.display = '';
					}
					else {
						divEvent.innerHTML = data.responseText;
						divEvent.style.display = '';
					}
				}
			})
		}
		else {
			divEvent.style.display = '';
		}
	}
	else {
		divEvent.style.display = 'none';
	}
}

function showCombMPDetail(refno, winlostdate, transid, betid, custid, target) {
	if (parent.RefreshCountDown) parent.RefreshCountDown.stopTimeout();
	var width = 750,
		height = 350,
		params = '?refno=' + refno + '&wldate=' + winlostdate + '&transid=' + transid + '&betid=' + betid + '&custid=' + custid,
		detailTitle = $(target).siblings('.combinationLink').find('#hidden0').text() + " - " + ApoRes.RefNo + ": " + transid;
	APOLLO.openDialogFrame(APOLLO.resolveUrl("~/_BetList/MixParlay/CombinationMixParlayDetail.aspx" + params),
		detailTitle, { width: width, height: height,
			close: function (event, ui) {
				if (parent.RefreshCountDown) parent.RefreshCountDown.startTimeout();
			} 
		});
}

function InitBetList() {
	if (parent.APOLLO && parent.APOLLO.currentOpenDialog) {
		var heightPopup = 630;
		if (APOLLO.browser.msie) {
			heightPopup = 615;
			if (parseInt(APOLLO.browser.version, 10) == 7) {
				$('#content_main').css({ 'padding-bottom': '130px' });
			}
		}

		if (parent.APOLLO.isResizeDialog) {	
			// resolution for screens
			if (window.parent.$("#page_main").width() >= 1024) {
				parent.APOLLO.resizeDialog(1000, heightPopup);
            } else {
                // check IE
				if (APOLLO.browser.msie) {
					parent.APOLLO.resizeDialog(960, 500);
				} else {
					parent.APOLLO.resizeDialog(960, 510);
				}
			}
		}
	}

	if (document.getElementById("exporttoexcel") != null) {
		document.getElementById("exporttoexcel").style.display = "none";
	}
}

function OpenScoreMap(matchIds, bettype, liveindicator, custId) {
	var mainShareHolderId = 0;
	var url = APOLLO.resolveUrl("~/Forecast/ScoreMap?matchIds=" + matchIds +
			(liveindicator == 1 ? "&liveBetTypes=" + bettype : "&deadBallBetTypes=" + bettype));

	if (typeof ($("#MainShareHolderId").val()) !== 'undefined' && $("#MainShareHolderId").val() != "") {
		mainShareHolderId = $("#MainShareHolderId").val();
		if (mainShareHolderId > 0) {
			url += "&mainShareHolderId=" + mainShareHolderId;
		}
	};

	var mainShareHolderUsername = $("#MainShareHolderUsername").val();
	if (typeof (mainShareHolderUsername) !== 'undefined' && mainShareHolderUsername != "") {
		mainShareHolderUsername = " - " + mainShareHolderUsername;
	}
	url += "&custId=" + custId;

	var titleScoreMap = _page.titleScoreMap + mainShareHolderUsername;
	var wnd = parent.IsApoRoot ? window : parent;
	var singleFrame = false; // allow multiple instances of dialog to keep BetList opened
	// Check IE7
	if (APOLLO.browser.msie && (parseInt(APOLLO.browser.version, 10) == 7
		|| parseInt(APOLLO.browser.version, 10) == 8)) {
		wnd.APOLLO.openDialogFrame(url, titleScoreMap, { width: 1000, height: 610, position: { my: "center", at: "center", of: window} }, singleFrame);
	} else {
		wnd.APOLLO.openDialogFrame(url, titleScoreMap, { width: 1000, height: 625 }, singleFrame);
	}
}

RegisterStartUp(InitBetList);

var BETLIST = {};

if (typeof jQuery != "undefined") {
	BETLIST = function ($) {
		return {
			dialogDetails: null,
			pageIndex: 0,
			init: function () {
				BETLIST.BalanceUpDay = $("#balanceUpDay")
				BETLIST.FromDate = $("#FromDate");
				BETLIST.ToDate = $("#ToDate");
				BETLIST.Today = $("#today");
				BETLIST.Yesterday = $("#yesterday");
				BETLIST.Tomorrow = $("#tomorrow");
				BETLIST.FromThisWeek = $("#thisWeekFrom");
				BETLIST.ToThisWeek = $("#thisWeekTo");
				BETLIST.FromLastWeek = $("#lastWeekFrom");
				BETLIST.ToLastWeek = $("#lastWeekTo");
				BETLIST.ddlSport = $("#SportTypes");
				BETLIST.ddlBettype = $("#BetTypes");
				BETLIST.dialogDetails = $('#dialog-details');
				// Event click submit on ticket cancel
				$(".act-ticket-cancel").click(function () {
					BETLIST.ticketCancel();
				});

				$('#backLink').click(function () {
					history.go(-1);
					return false;
				});
				$(".dateRange").bind('click', function () {
					/* Range type value
					+ 1 : Today
					+ 2 : Yesterday
					+ 3 : Tomorrow
					+ 4 : This Week
					+ 5 : Last Week
					*/
					var rangeType = $(this).attr('range');

					switch (rangeType) {
						case "1":
							BETLIST.FromDate.val(BETLIST.Today.val());
							BETLIST.ToDate.val(BETLIST.Today.val());
							break;
						case "2":
							BETLIST.FromDate.val(BETLIST.Yesterday.val());
							BETLIST.ToDate.val(BETLIST.Yesterday.val());
							break;
						case "3":
							BETLIST.FromDate.val(BETLIST.Tomorrow.val());
							BETLIST.ToDate.val(BETLIST.Tomorrow.val());
							break;
						case "4":
							BETLIST.FromDate.val(BETLIST.FromThisWeek.val());
							BETLIST.ToDate.val(BETLIST.ToThisWeek.val());
							break;
						case "5":
							BETLIST.FromDate.val(BETLIST.FromLastWeek.val());
							BETLIST.ToDate.val(BETLIST.ToLastWeek.val());
							break;
						default:
							BETLIST.FromDate.val(BETLIST.Today.val());
							BETLIST.ToDate.val(BETLIST.Today.val());
							return;
					}
				});

				if ($('#IsTransactionsBetList').val() === '1') {
					BETLIST.initPager(BETLIST.pageIndex);
				}

				BETLIST.initEditLink();
			},

			getBetListData: function () {
				$.blockUI();
				$.ajax({
					url: APOLLO.resolveUrl("~/Betlist/TransactionsPaging"),
					data: {
						fromDate: BETLIST.FromDate.val(),
						toDate: BETLIST.ToDate.val(),
						sportTypes: BETLIST.ddlSport.val(),
						betTypes: BETLIST.ddlBettype.val(),
						username: $("#Username").val(),
						userType: $("#UserType").val(),
						pageIndex: BETLIST.pageIndex
					},
					success: function (data) {
						$('#container').html(data);
						BETLIST.initEditLink();
					},
					complete: function () {
						$.unblockUI();
					}
				});
			},

			initPager: function (pageIndex) {
				$('#pager').sbPager({
					currentPage: pageIndex,
					itemPerPage: 40,
					totalItem: $('#TotalItems').val(),
					callback: function (currentPage) {
						BETLIST.pageIndex = currentPage;
						BETLIST.getBetListData();
					}
				});
			},

			ticketCancel: function () {
				if (BETLIST.FromDate.val() < BETLIST.BalanceUpDay.val() || BETLIST.ToDate.val() < BETLIST.BalanceUpDay.val()) {
					alert(LANGUAGES.MessageValidateWithArchivedDate + " " + BETLIST.BalanceUpDay.val());
				}
				else {
					$.blockUI();
					$("form:first").submit();
				}
			},
			initEditLink: function () {
				$(".view-detail").click(function () {
					var fromDate = toDate = type = "";

					if (typeof ($("#fromDate").val()) !== 'undefined') {
						fromDate = $("#fromDate").val();
					}
					if (typeof ($("#toDate").val()) !== 'undefined') {
						toDate = $("#toDate").val();
					}
					if (typeof ($("#type").val()) !== 'undefined') {
						type = $("#type").val();
					}

					var $this = $(this),
					transId = $this.attr("tid"),
					fromDate = fromDate,
					toDate = toDate,
					type = type,
					params = "?transid=" + transId +
						"&fromdate=" + fromDate +
						"&toDate=" + toDate +
						"&type=" + type;

					var title = ApoRes.BetListViewDetail + " - " + ApoRes.RefNo + ": " + transId;
					APOLLO.isResizeDialog = false;

					// resolution for screens                   
					if (($(window).width() >= 1024)) {
						widthDialog = 400;
						heightDialog = 500;                        
					} else {
						widthDialog = 400;
						heightDialog = 380;
					}
					APOLLO.openDialogFrame(APOLLO.resolveUrl("~/BetList/ViewDetail" + params),
								title, { width: widthDialog, height: heightDialog, modal: false });

					return false;
				});
			}
		};
	} (jQuery);

	$(document).ready(function () {
		$.blockUI();
		setTimeout('$.unblockUI()', 300);

		BETLIST.init();

		$("#btnCancel").on('click', function () {
			$.blockUI();
			history.back();
			return false;
		});

		var submit = $(".act-submit");

		
			
		

		if (submit.length > 0) {
			submit.click(function () {
				$("#frmBetList").submit();
			});
		}
		if (typeof ($("#username").val()) !== 'undefined') {
			$("#username").autocomplete({
				source: function (request, response) {
					var url = "";
					if ($("#userType").val() == "0") {
						url = "Account/GetMemberByUsername?username=" + request.term;
					}
					else {
						url = "Account/GetAgentByUsername?username=" + request.term;
					}

					$.ajax({
						url: APOLLO.resolveUrl(url),
						success: function (data) {
							var found = $.map(data, function (item) {
								return {
									value: item.Username
								}
							});
							return response(found);
						}
					});
				},
				position: { my: "right top", at: "right bottom" }
			});
		}

		if (typeof ($("#ddlMainShareHolder").val()) !== 'undefined') {
			$("#ddlMainShareHolder").change(function () {
				var $this = $(this);
				var markIndex = document.location.href.indexOf('?');
				var link = document.location.href;
				if (markIndex > 0) {
					var markIndexShareHolderId = document.location.href.indexOf("&mainShareHolderId=");
					if (markIndexShareHolderId > 0) {
						link = document.location.href.substring(0, markIndexShareHolderId);
					}
					link = link + "&mainShareHolderId=" + $this.val();
				}
				document.location.href = link;
			});
		}

		// change page with navigation
		$(".navigation").bind('click', function () {
			var obType = $("#type"),
				type = null;
			if (obType.length) {
				type = obType.val();
			}

			if (type != null) {
				var fromDate = null,
				 toDate = null,
					sportType = betType = 0,
					obFromDate = $('#fromDate'),
					obToDate = $('#toDate');
				obSportType = $('#sportTypes'),
					obBetType = $('#betTypes');

				if (obFromDate.length) {
					fromDate = $("#fromDate").val();
				}

				if (obToDate.length) {
					toDate = obToDate.val();
				}

				if (obSportType.length) {
					sportType = obSportType.val();
				}

				if (obBetType.length) {
					betType = obBetType.val();
				}

				$.blockUI();
				if (type == "report") {
					var url = APOLLO.resolveUrl('Reports/AgentWinLoss?AgentId=' + $(this).attr("data-agentid")
													+ "&AgentName=" + $(this).html()
													+ '&FromDate=' + fromDate
													+ '&ToDate=' + toDate
													+ '&Sporttypes=' + sportType
													+ '&Bettypes=' + betType);
					location.href = url;
				} else if (type == "outstanding") {
					var url = APOLLO.resolveUrl('Reports/Outstanding?agentId=' + $(this).attr("data-agentid")
													+ '&Sporttypes=' + sportType
													+ '&Bettypes=' + betType);
					location.href = url;
				}
			}
		});
	});
}