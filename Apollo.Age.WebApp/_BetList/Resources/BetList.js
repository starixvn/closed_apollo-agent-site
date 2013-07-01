function ViewCommissionDetail(ibcrefno, currency, tableId, tableName, tournamentId, tournamentName, gameType, betType) {
	var url = '../P2P/CommissionDetail.aspx?ibcrefno=' + ibcrefno + '&currency=' + currency + '&tableid=' + tableId + '&tablename=' + tableName + '&tournamentid=' + tournamentId + '&tournamentname=' + tournamentName + '&gametype=' + gameType + '&bettype=' + betType;
	window.location.href = url;
}

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
			ajax.Request(APOLLO.resolveUrl("~/_BetList/MixParlay/MixParlay.aspx"), { method: 'get', parameters: 'transid=' + transid,
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
	var width = 750,
		height = 350,
		params = '?refno=' + refno + '&wldate=' + winlostdate + '&transid=' + transid + '&betid=' + betid + '&custid=' + custid,
		detailTitle = $(target).siblings('.combinationLink').find('#hidden0').text() + " - " + _page.refNo + ": " + transid;
	APOLLO.openDialogFrame(APOLLO.resolveUrl("~/_BetList/MixParlay/CombinationMixParlayDetail.aspx" + params),
		detailTitle, { width: width, height: height });
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
    if (parent.APOLLO.currentOpenDialog) {
        var heightPopup = 630;
	    if (APOLLO.browser.msie) {
            heightPopup = 615;
            if(parseInt(APOLLO.browser.version, 10) == 7) {
	            $('#content_main').css({ 'padding-bottom': '130px' });
	        }
        }
	    parent.APOLLO.resizeDialog(900, heightPopup);
	}
	
	if (document.getElementById("exporttoexcel") != null) {
		document.getElementById("exporttoexcel").style.display = "none";
	}
	InitExcelFunction();
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

			init: function () {
				BETLIST.dialogDetails = $('#dialog-details');
			}
		};
	} (jQuery);

	$(document).ready(function () {
	    BETLIST.init();

	    var viewDetailsLinks = $(".view-detail");
	    var submit = $(".act-submit");

	    viewDetailsLinks.click(function () {
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

	        var title = _page.titleViewComm + " - " + _page.refNo + ": " + transId;                

	        APOLLO.openDialogFrame(APOLLO.resolveUrl("~/BetList/ViewDetail" + params),
				title, { width: 450, height: 450, modal: false });

	        return false;
	    });

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

	    $(".navigation").bind('click', function () {
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

	        if (type == "report") {
	            var url = APOLLO.resolveUrl('Reports/AgentWinLoss?id=' + $(this).attr("agentid") + "&name=" + $(this).html()
					+ '&fromDate=' + fromDate + "&toDate=" + toDate);
	            location.href = url;
	        }
	        else {
	            location.href = APOLLO.resolveUrl("Reports/Outstanding?agentId=" + $(this).attr("agentid"));
	        }
	    });
	});
}