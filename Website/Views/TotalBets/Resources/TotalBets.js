/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var TOTALBETS = function ($) {
    return {
        initViewForeCastHandicap: function () {
            $(".tblRpt").on("click", "a.view-forecast", function () {
                if (countDownTimer != null) clearTimeout(countDownTimer);

                var url = APOLLO.resolveUrl('Forecast/ForecastByMatch?'
                + 'matchId=' + $(this).attr("match-id")
                + '&betType=' + $(this).attr("bet-type")
                 + '&leagueName=' + $(this).attr("league-name"));
                if (currentMainShareHolderId > 0) {
                    url += "&mainShareHolderId=" + currentMainShareHolderId;
                }
                ageWnd.openDialog(url, 'FORECAST', {
                    width: 665,
                    height: 350,
                    position: { my: "top+40", at: "center top", of: window },
                    close: function (event, ui) {
                        countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                    }
                });
            });
        },

        initViewBetList: function () {
            $(".tblRpt").on("click", "a.view-bet-list", function () {
                if (countDownTimer != null) clearTimeout(countDownTimer);
                var url = APOLLO.resolveUrl('_BetList/BetList.aspx?matchId=' + $(this).attr("match-id")
                + '&betType=' + $(this).attr("bet-type")
                + '&type=' + $(this).attr("betlist-type")
                + '&leagueName=' + $(this).attr("league-name"));
                if (currentMainShareHolderId > 0) {
                    url += "&mainShareHolderId=" + currentMainShareHolderId + "&isForecast=true";
                }
                ageWnd.openDialog(url, "BET LIST", {
                    width: 930,
                    height: 720,
                    position: { my: "top+40", at: "center top", of: window },
                    close: function (event, ui) {
                        countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                    }
                });
            });
        },

        initViewBetListGame: function () {
            $(".tblRpt").on("click", "a.view-bet-list", function () {
                if (countDownTimer != null) clearTimeout(countDownTimer);
                var url = APOLLO.resolveUrl('_BetList/BetList.aspx?matchId=' + $(this).attr("match-id")
                + '&betType=' + $(this).attr("bet-type")
                + '&type=' + $(this).attr("betlist-type")
                + '&islive=' + $(this).attr("is-live"));
                var title = $(this).attr("league-name") + "-" + $(this).attr("bet-type-name");

                if (currentMainShareHolderId > 0) {
                    url += "&mainShareHolderId=" + currentMainShareHolderId + "&isForecast=true";
                }
                ageWnd.openDialog(url, title, {
                    width: 930,
                    height: 770,
                    position: { my: "top+40", at: "center top", of: window },
                    close: function (event, ui) {
                        countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                    }
                });
            });
        },

        initViewBetListNewGame: function () {
            $(".tblRpt").on("click", "a.view-bet-list-new-game", function () {
                if (countDownTimer != null) clearTimeout(countDownTimer);
                var url = APOLLO.resolveUrl('_BetList/BetList.aspx?matchId=' + $(this).attr("match-id")
                + '&betType=' + $(this).attr("bet-type")
                + '&type=' + $(this).attr("betlist-type")
                + '&islive=' + $(this).attr("is-live")
                + '&betteam=' + $(this).attr("bet-team"));

                if (currentMainShareHolderId > 0) {
                    url += "&mainShareHolderId=" + currentMainShareHolderId + "&isForecast=true";
                }
                ageWnd.openDialog(url, 'Bet List', {
                    width: 930,
                    height: 770,
                    position: { my: "top+40", at: "center top", of: window },
                    close: function (event, ui) {
                        countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                    }
                });
            });
        },

        initViewForecast1x2: function () {
            $(".tblRpt").on("click", "a.view-forecast", function () {
                if (countDownTimer != null) clearTimeout(countDownTimer);
                var url = APOLLO.resolveUrl('Forecast/FC1X2?'
                + 'matchId=' + $(this).attr("match-id")
                + '&betType=' + $(this).attr("bet-type")
                + '&leagueName=' + $(this).attr("league-name"));
                if (currentMainShareHolderId > 0) {
                    url += "&mainShareHolderId=" + currentMainShareHolderId;
                }
                ageWnd.openDialog(url, 'FORECAST', {
                    width: 600,
                    height: 600,
                    position: { my: "top+40", at: "center top", of: window },
                    close: function (event, ui) {
                        countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                    }
                });
            });
        },
        innitFreeHeader: function () {
            //APOLLO.browser
            if (APOLLO.browser.msie && parseInt(APOLLO.browser.version, 10) < 9) {
                return;
            }
            var clonedHeaderRow;
            $(".persist-area").each(function () {
                clonedHeaderRow = $(".persist-header", this);
                clonedHeaderRow
                     .before(clonedHeaderRow.clone())
                     .css({ "width": clonedHeaderRow.width() })
                     .addClass("floatingHeader")
                     .removeClass("persist-header");
            });

            $(window)
                .scroll(TOTALBETS.updateTableHeaders)
                .trigger("scroll");
            $(window).resize(function () {
                TOTALBETS.updateTableHeaders();
            });

        },

        updateTableHeaders: function () {
            if ($(".persist-area").val() != undefined) {
                var widthspace = $(".persist-area").width();

                $('.floatingHeader').width(widthspace);
                var left = $('table thead tr:first').position()["left"];
                $(".persist-area").each(function () {
                    var el = $(this),
                        offset = el.offset(),
                        scrollTop = $(window).scrollTop(),
                        floatingHeader = $(".floatingHeader", this);
                    if ((scrollTop > offset.top) && (scrollTop < offset.top + el.height())) {
                        floatingHeader.css({
                            "visibility": "visible",
                            "left": (left - $(window).scrollLeft()),
                            "z-index": "34"
                        });
                    } else {
                        floatingHeader.css({
                            "visibility": "hidden"
                        });
                    };
                });

                var i = 0;
                $("table .persist-header td").each(function () {
                    var x = $(this).width();
                    $($("table .floatingHeader td")[i]).width(x);
                    i++;
                });
            }
        }
    };
} (jQuery);

$(function () {
	if (currentMainShareHolderId > 0) {
		$("#ddlMainShareHolder").change(function () {
			var $this = $(this);
			window.location.href = currentActionLink + "?mainShareHolderId=" + $this.val();
		});
	}
});

function ReloadPage() {
    window.location.href = window.location.href;
}