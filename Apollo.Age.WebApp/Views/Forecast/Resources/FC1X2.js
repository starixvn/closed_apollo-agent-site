/* File Created: April 3, 2013 */
/*global DEFAULT_OBJECT*/
/*global DEFAULT_LAYOUT*/
/*global APOLLO*/
'use strict';
if (typeof jQuery !== 'undefined') {
    var FC1X2 = (function ($) {
        return {
            currentLeague: 0,
            matchId: DEFAULT_OBJECT.matchId,
            betType: DEFAULT_OBJECT.betType,
            cuurentLeagueName: DEFAULT_OBJECT.initLeagueName,
            link: APOLLO.resolveUrl('Forecast/FC1X2Matches'),
            init: function () {
                parent.$(".ui-dialog .ui-dialog-title").text(DEFAULT_LAYOUT.forecastString);
                if (FC1X2.matchId > 0) {
                    if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                        $("#main-shareholder-box").hide();
                    }
                    FC1X2.loadMatch();
                }
                else {
                    FC1X2.cuurentLeagueName = $('#ddlLeague option:selected').text();
                    FC1X2.currentLeague = $('#ddlLeague').val();
                    if (FC1X2.currentLeague) {
                        $('#ddlLeague').on("change", function () {
                            var $this = $(this);
                            FC1X2.currentLeague = $this.val();
                            FC1X2.cuurentLeagueName = $('#ddlLeague option:selected').text();
                            if (FC1X2.currentLeague) {
                                FC1X2.changeLeague();
                            }
                        });
                        FC1X2.registerEvents(true);
                    }
                }
            },
            registerEvents: function (isLoadNewPopup) {
                var url = '';
                var countDownTimer = null;
                $('#forecast-body')
                .off("click")
                .on("click", ".view-bet-list", function () {
                    var $this = $(this);
                    if (isLoadNewPopup) {
                        if (countDownTimer != null) { clearTimeout(countDownTimer); }
                        url = APOLLO.resolveUrl('Betlist/Forecast?matchId=' + $this.attr("match-id")
                            + '&betType=' + $this.attr("bet-type")
                            + '&type=BettypeMatch'
                            + '&leagueName=' + FC1X2.cuurentLeagueName);

                        if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                            url += "&mainShareHolderId=" + DEFAULT_LAYOUT.currentMainShareHolderId + "&isForecast=true";
                        }

                        APOLLO.openDialogFrame(url, DEFAULT_LAYOUT.betListString, {
                            width: 930,
                            height: 700,
                            position: { my: "top+30", at: "center top", of: window },
                            close: function (event) {
                                countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                            }
                        });

                    } else {
                        url = APOLLO.resolveUrl('Betlist/Forecast?matchId=' + FC1X2.matchId
                            + '&betType=' + FC1X2.betType
                            + '&type=BettypeMatch'
                            + '&leagueName=' + FC1X2.cuurentLeagueName);

                        if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                            url += "&mainShareHolderId=" + DEFAULT_LAYOUT.currentMainShareHolderId + "&isForecast=true";
                        }

                        parent.$(".ui-dialog .ui-dialog-title").text(DEFAULT_LAYOUT.betListString);
                        location.href = url;
                    }
                });
            },
            changeLeague: function () {
                var url = FC1X2.link + '?leagueId=' + FC1X2.currentLeague
                + '&mainShareHolderId=' + DEFAULT_LAYOUT.currentMainShareHolderId;
                $("#forecast-body").load(url);
                FC1X2.registerEvents(true);
            },
            loadMatch: function () {
                var url = FC1X2.link + '?matchId=' + FC1X2.matchId + '&betType=' + FC1X2.betType;
                if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                    url += '&mainShareHolderId=' + DEFAULT_LAYOUT.currentMainShareHolderId;
                }
                $("#forecast-body").load(url);
                FC1X2.registerEvents(false);
            }
        };
    } (jQuery));

	$(function () {
		if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
			$("#ddlMainShareHolder").change(function () {
				var $this = $(this);
				window.location.href = DEFAULT_LAYOUT.currentActionLink + "?mainShareHolderId=" + $this.val();
			});

			$(".selectLeague label").width($("#main-shareholder-box label").width());
		}
		FC1X2.init();
	}).ajaxStart($.blockUI).ajaxStop($.unblockUI);
}

function UpdateForecast1x2Value() {
	if (FC1X2.matchId > 0) {
		window.location.href = window.location.href;
	}
	else {
		var url = APOLLO.resolveUrl("Forecast/FC1X2?leagueId=" + FC1X2.currentLeague);
		if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
			url += '&mainShareHolderId=' + DEFAULT_LAYOUT.currentMainShareHolderId;
		}
		window.location.href = url;
	}
}