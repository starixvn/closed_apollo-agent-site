/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var TOTALBETS = function ($) {
    return {
        initViewForeCastHandicap: function () {
            $(".tblRpt").on("click", "input.view-forecast", function () {
                parent.RefreshCountDown.stopTimeout();

                var url = APOLLO.resolveUrl('Forecast/ForecastByMatch?'
                    + 'matchId=' + $(this).attr("match-id")
                    + '&betType=' + $(this).attr("bet-type")
                    + '&leagueName=' + escape($(this).attr("league-name")));
                if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                    url += "&mainShareHolderId=" + DEFAULT_LAYOUT.currentMainShareHolderId;
                }

                //window.open(url, '_blank');
                APOLLO.openDialogFrame(url, DEFAULT_LAYOUT.forecastString, {
                    width: 665,
                    height: 250,
                    position: { my: "top+20", at: "center top", of: window },
                    close: function (event, ui) {
                        parent.RefreshCountDown.startTimeout();
                    }
                });
            });
        },

        initViewBetList: function () {
            $(".tblRpt").on("click", "a.view-bet-list", function () {
                parent.RefreshCountDown.stopTimeout();
                var url = APOLLO.resolveUrl('Betlist/Forecast?matchId=' + $(this).attr("match-id")
                + '&betType=' + $(this).attr("bet-type")
                + '&type=' + $(this).attr("betlist-type")
                + '&leagueName=' + $(this).attr("league-name"));
                if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                    url += "&mainShareHolderId=" + DEFAULT_LAYOUT.currentMainShareHolderId + "&isForecast=true";
                }
                // resolution for screens
                var resolution = APOLLO.resolutionForScreens(1000, 720);
                
                APOLLO.openDialogFrame(url, DEFAULT_LAYOUT.betListString, {
                    width: resolution.width,
                    height: resolution.height,
                    position: { my: "top+20", at: "center top", of: window },
                    close: function (event, ui) {
                        parent.RefreshCountDown.startTimeout();
                    }
                });
            });
        },

        initViewBetListGame: function () {
            $(".tblRpt").on("click", "a.view-bet-list", function () {
                parent.RefreshCountDown.stopTimeout();
                var url = APOLLO.resolveUrl('Betlist/Forecast?matchId=' + $(this).attr("match-id")
                + '&betType=' + $(this).attr("bet-type")
                + '&type=' + $(this).attr("betlist-type")
                + '&islive=' + $(this).attr("is-live"));
                var title = $(this).attr("league-name") + "-" + $(this).attr("bet-type-name");

                if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                    url += "&mainShareHolderId=" + DEFAULT_LAYOUT.currentMainShareHolderId + "&isForecast=true";
                }

                APOLLO.openDialogFrame(url, title, {
                    width: 1000,
                    height: 770,
                    position: { my: "top+40", at: "center top", of: window },
                    close: function (event, ui) {
                        parent.RefreshCountDown.startTimeout();
                    }
                });
            });
        },

        initViewBetListNewGame: function () {
            $(".tblRpt").on("click", "a.view-bet-list-new-game", function () {
                parent.RefreshCountDown.stopTimeout();
                var url = APOLLO.resolveUrl('Betlist/Forecast?matchId=' + $(this).attr("match-id")
                + '&betType=' + $(this).attr("bet-type")
                + '&type=' + $(this).attr("betlist-type")
                + '&islive=' + $(this).attr("is-live")
                + '&betteam=' + $(this).attr("bet-team"));

                if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                    url += "&mainShareHolderId=" + DEFAULT_LAYOUT.currentMainShareHolderId + "&isForecast=true";
                }

                APOLLO.openDialogFrame(url, DEFAULT_LAYOUT.betListString, {
                    width: 1000,
                    height: 770,
                    position: { my: "top+40", at: "center top", of: window },
                    close: function (event, ui) {
                        parent.RefreshCountDown.startTimeout();
                    }
                });
            });
        },

        initViewForecast1x2: function () {
            $(".tblRpt").on("click", "input.view-forecast", function () {
                parent.RefreshCountDown.stopTimeout();
                var url = APOLLO.resolveUrl('Forecast/FC1X2?'
                + 'matchId=' + $(this).attr("match-id")
                + '&betType=' + $(this).attr("bet-type")
                + '&leagueName=' + escape($(this).attr("league-name")));
                if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                    url += "&mainShareHolderId=" + DEFAULT_LAYOUT.currentMainShareHolderId;
                }
                //window.open(url, '_blank');

                // resolution for screens
                var resolution = APOLLO.resolutionForScreens(600, 630);
                APOLLO.openDialogFrame(url, DEFAULT_LAYOUT.forecastString, {
                    width: resolution.width,
                    height: resolution.height,
                    position: { my: "top+20", at: "center top", of: window },
                    close: function (event, ui) {
                        parent.RefreshCountDown.startTimeout();
                    }
                });
            });
        },
        reloadPage: function () {
            window.location.href = window.location.href;
        }
    };
} (jQuery);

$(function () {
    $.blockUI();
    setTimeout('$.unblockUI()', 300);

	if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
		$("#ddlMainShareHolder").change(function () {
			var $this = $(this);
			window.location.href = DEFAULT_LAYOUT.currentActionLink + "?mainShareHolderId=" + $this.val();
		});
	}
});