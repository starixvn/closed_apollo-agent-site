function ShowResultWithPicture(showpic) {
	var url = location.href;
	location.href = SetParameterValue("showpic", showpic, url);
}

function ViewBallNo(id) {
	// Hide others
	for (var i = 0; i < 100; i++) {
		if ('ballNo' + i == id) {
			continue;
		}

		if (document.getElementById('ballNo' + i)) {
			document.getElementById('ballNo' + i).style.display = 'none';
		} else {
			break;
		}
	}

	var display = document.getElementById(id).style.display == 'none';
	document.getElementById(id).style.left = '190px';
	document.getElementById(id).style.display = display ? 'block' : 'none';
}

function ViewHandHistory(custid, ibcrefno, currency, tableId, tableName, tournamentId, tournamentName, gameType, betType, date) {
	var url = '../P2P/HandHistory.aspx?custid=' + custid + '&ibcrefno=' + ibcrefno + '&currency=' + currency + '&tableid=' + tableId + '&tablename=' + tableName + '&tournamentid=' + tournamentId + '&tournamentname=' + tournamentName + '&gametype=' + gameType + '&bettype=' + betType + '&Date=' + date;
	window.location.href = url;
}

function ViewHandHistoryDetail(custid, handNo, handType, currency) {
	var url = '../P2P/HandHistoryDetail.aspx?custid=' + custid + '&handno=' + handNo + '&handtype=' + handType + '&currency=' + currency;
	window.location.href = url;
}

function ViewCommissionDetail(ibcrefno, currency, tableId, tableName, tournamentId, tournamentName, gameType, betType) {
	var url = '../P2P/CommissionDetail.aspx?ibcrefno=' + ibcrefno + '&currency=' + currency + '&tableid=' + tableId + '&tablename=' + tableName + '&tournamentid=' + tournamentId + '&tournamentname=' + tournamentName + '&gametype=' + gameType + '&bettype=' + betType;
	window.location.href = url;
}

function ViewResult(matchid, race, bettype, sporttype, refno, username, winlostdate, refno_mixparlay, league, is_outright) {
	if (bettype == 71) {
		ViewCasinoResult(refno, bettype, username);
	} else if (bettype == 9 || bettype == 29) {//view mix parlay & combination mix parlay
		ViewMixParlayResult(refno_mixparlay, winlostdate, bettype);
	} else if (bettype == 1000 || bettype == 1001 || bettype == 1002) {  // P2P Game
		ViewP2PResult(refno, bettype, username);
	} else {
		ViewSportBookResult(matchid, bettype, race, sporttype, league, is_outright, refno);
	}
}

function ViewMixParlayResult(refno, winlostdate, bettype) {
	var width = 730;
	var height = 450;
	var params = '?refnum=' + refno + '&wldate=' + winlostdate + "&bettype=" + bettype + '&matchid=1';
	if (ageWnd.loaded) {
		ageWnd.close();
	}
	ageWnd.open(APOLLO.resolveUrl("~/_Reports/Result/Result.aspx" + params), '', 50, 100, width, height);
}

function ViewSportBookResult(matchid, bettype, race, sporttype, league, is_outright, refNo) {
	var width = 730;
	var height = 250;
	var params = '?matchid=' + matchid + '&bettype=' + bettype + '&race=' + race + '&ddlSport=' + sporttype + '&ddlLeague=' + league + '&is_outright=' + is_outright + '&refnum=' + refNo;

	if (parent.ageWnd) {
		parent.ageWnd.open(APOLLO.resolveUrl("~/_Reports/Result/Result.aspx" + params), '', 50, 100, width, height);
	}
	else {
		ageWnd.open(APOLLO.resolveUrl("~/_Reports/Result/Result.aspx" + params), '', 50, 100, width, height);
	}
}

function showMP(transid) {
	var divEvent = document.getElementById('divEvent_' + transid);
	if (divEvent.style.display == 'none') {
		if (divEvent.innerHTML == "") {
			ajax.Request(APOLLO.resolveUrl("~/_BetList/MixParlay/MixParlay.aspx"), { method: 'get', parameters: 'transid=' + transid,
				onComplete: function (data) {
					divEvent.innerHTML = data.responseText;
					divEvent.style.display = '';
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

function showCombMP(transid, orderID) {
	var divEvent = document.getElementById('divEvent_' + transid + orderID);
	if (divEvent.style.display == 'none') {
		if (divEvent.innerHTML == "") {
			ajax.Request(APOLLO.resolveUrl("~/_BetList/MixParlay/CombinationMixParlay.aspx"), { method: 'get', parameters: 'transid=' + transid,
				onComplete: function (data) {
					divEvent.innerHTML = data.responseText;
					divEvent.style.display = '';
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

function showCombMPDetail(refno, winlostdate, transid, betid, custid) {
	var width = 750;
	var height = 350;
	var params = '?refno=\'' + refno + '\'&wldate=' + winlostdate + '&transid=' + transid + '&betid=' + betid + '&custid=' + custid;

	if (parent.ageWnd) {
		parent.ageWnd.open(APOLLO.resolveUrl("~/_BetList/MixParlay/CombinationMixParlayDetail.aspx" + params), '', 50, 100, width, height);
	}
	else {
		ageWnd.open(APOLLO.resolveUrl("~/_BetList/MixParlay/CombinationMixParlayDetail.aspx" + params), '', 50, 100, width, height);
	}
}

function InitExcelFunction() {
	var exportExcelImg = document.getElementById("exporttoexcel");
	if (exportExcelImg) {
		function exportExcelImg_OnClick(ev) {
			var url = location.href;
			url = SetParameterValue('exporttoexcel.x', 4, url);
			url = SetParameterValue('exporttoexcel.y', 8, url);
			location.href = url;

			if (IE) {
				window.event.cancelBubble = true;
				window.event.returnValue = false;
			} else {
				ev.preventDefault();
				ev.stopPropagation();
			}
		}

		age.addEvent(exportExcelImg, 'click', exportExcelImg_OnClick);

		exportExcelImg.onblur = function () {
			document.getElementById("exporttoexcel").focused = false;
		}
		exportExcelImg.onfocus = function () {
			document.getElementById("exporttoexcel").focused = true;
		}
	}
}

function InitBetList() {
	var ageWnd = parent.ageWnd;

	if (typeof ageWnd != 'undefined' && ageWnd.loaded) {
		ageWnd.setRect(ageWnd.left, 50, 900, 720);  // resize parent dialog
	}

	if (document.getElementById("exporttoexcel") != null) {
		document.getElementById("exporttoexcel").style.display = "none";
	}
	InitExcelFunction();
}

function OpenIPInfo(ip) {
	ageWnd.open(APOLLO.resolveUrl("~/_IPInfo/IpInfo.aspx?ip=" + ip), '', 300, 100, 400, 170);
}

function OpenScoreMap(matchIds, bettype, liveindicator) {
	var wnd = parent.ageWnd ? parent.ageWnd : ageWnd;
	var url = APOLLO.resolveUrl("~/Forecast/ScoreMap?matchIds=" + matchIds +
            (liveindicator == 1
                ? "&liveBetTypes=" + bettype
                : "&deadBallBetTypes=" + bettype));
	wnd.open(url, '', 0, 20, 900, 480);
}

RegisterStartUp(InitBetList);

var BETLIST = {};

if (typeof jQuery != "undefined") {
	BETLIST = function ($) {
		return {
			dialogDetails: null,

			init: function () {
				BETLIST.dialogDetails = $('#dialog-details');
			}
		};
	} (jQuery);

	$(document).ready(function () {
		BETLIST.init();

		var viewDetailsLinks = $(".view-detail");
		var submit = $(".act-submit");

		if (viewDetailsLinks.length > 0) {
			viewDetailsLinks.click(function () {
				var $this = $(this),
                    transId = $this.attr("tid"),
					fromDate = $("#fromDate").val(),
					toDate = $("#toDate").val(),
					type = $("#type").val(),
                    url = APOLLO.resolveUrl("~/_BetList/ViewDetail.aspx"),
                    ajaxParams = "transid=" + transId +
                        "&fromdate=" + fromDate +
                        "&toDate=" + toDate +
						"&type=" + type;
				ajax.Request(url, {
					asynchronous: true,
					parameters: ajaxParams,
					onComplete: function (data) {
						BETLIST.dialogDetails.html(data.responseText);
					}
				});
				APOLLO.openDialog(BETLIST.dialogDetails, "Details", { modal: false, width: 450, height: 500 });
			});
		}

		if (submit.length > 0) {
			submit.click(function () {
				$("#frmBetList").submit();
			});
		}

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
	});
}