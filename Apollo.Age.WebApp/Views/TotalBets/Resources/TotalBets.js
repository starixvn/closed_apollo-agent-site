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
                if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                    url += "&mainShareHolderId=" + DEFAULT_LAYOUT.currentMainShareHolderId;
                }

                //                ageWnd.openDialog(url, DEFAULT_LAYOUT.forecastString, {
                //                    width: 665,
                //                    height: 350,
                //                    position: { my: "top+40", at: "center top", of: window },
                //                    close: function (event, ui) {
                //                        countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                //                    }
                //                });
                
                APOLLO.openDialogFrame(url, DEFAULT_LAYOUT.forecastString, {
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
                if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                    url += "&mainShareHolderId=" + DEFAULT_LAYOUT.currentMainShareHolderId + "&isForecast=true";
                }

                //                ageWnd.openDialog(url, DEFAULT_LAYOUT.betListString, {
                //                    width: 930,
                //                    height: 720,
                //                    position: { my: "top+40", at: "center top", of: window },
                //                    close: function (event, ui) {
                //                        countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                //                    }
                //                });

                APOLLO.openDialogFrame(url, DEFAULT_LAYOUT.betListString, {
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

                if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                    url += "&mainShareHolderId=" + DEFAULT_LAYOUT.currentMainShareHolderId + "&isForecast=true";
                }

                //                ageWnd.openDialog(url, title, {
                //                    width: 930,
                //                    height: 770,
                //                    position: { my: "top+40", at: "center top", of: window },
                //                    close: function (event, ui) {
                //                        countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                //                    }
                //                });

                APOLLO.openDialogFrame(url, title, {
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

                if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                    url += "&mainShareHolderId=" + DEFAULT_LAYOUT.currentMainShareHolderId + "&isForecast=true";
                }

                //                ageWnd.openDialog(url, DEFAULT_LAYOUT.betListString, {
                //                    width: 930,
                //                    height: 770,
                //                    position: { my: "top+40", at: "center top", of: window },
                //                    close: function (event, ui) {
                //                        countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                //                    }
                //                });

                APOLLO.openDialogFrame(url, DEFAULT_LAYOUT.betListString, {
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
                if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                    url += "&mainShareHolderId=" + DEFAULT_LAYOUT.currentMainShareHolderId;
                }

                //                ageWnd.openDialog(url, DEFAULT_LAYOUT.forecastString, {
                //                    width: 600,
                //                    height: 600,
                //                    position: { my: "top+40", at: "center top", of: window },
                //                    close: function (event, ui) {
                //                        countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                //                    }
                //                });
                
                APOLLO.openDialogFrame(url, DEFAULT_LAYOUT.forecastString, {
                    width: 600,
                    height: 600,
                    position: { my: "top+40", at: "center top", of: window },
                    close: function (event, ui) {
                        countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                    }
                });
            });
        }        
    };
} (jQuery);

$(function () {
    if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
        $("#ddlMainShareHolder").change(function () {
            var $this = $(this);
            window.location.href = DEFAULT_LAYOUT.currentActionLink + "?mainShareHolderId=" + $this.val();
        });
    }
});

function ReloadPage() {
    window.location.href = window.location.href;
}