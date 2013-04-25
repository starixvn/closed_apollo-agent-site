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
                    position: { my: "top+50", at: "center top", of: window },
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

                ageWnd.openDialog(url, "BET LIST", {
                    width: 930,
                    height: 770,
                    position: { my: "top+50", at: "center top", of: window },
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

                ageWnd.openDialog(url, title, {
                    width: 930,
                    height: 770,
                    position: { my: "top+50", at: "center top", of: window },
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
                //location.href = url;
                ageWnd.openDialog(url, 'Bet List', {
                    width: 930,
                    height: 770,
                    position: { my: "top+50", at: "center top", of: window },
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
                    width: 350,
                    height: 600,
                    position: { my: "top+50", at: "center top", of: window },
                    close: function (event, ui) {
                        countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                    }
                });
            });
        },

        innitFreeHeader: function () {
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
            var markIndex = document.location.href.indexOf('?');
            var link = document.location.href;
            if (markIndex > 0) {
                link = document.location.href.substring(0, markIndex);
            }

            document.location.href = link + "?mainShareHolderId=" + $this.val();
        });
    }
});

function ReloadPage() {
    location.reload();
//    if (currentMainShareHolderId > 0) {
//        document.location.href = APOLLO.resolveUrl("Forecast/FC1X2?leagueId=" + FC1X2.currentLeague); 
//    }
//    else {
//        //
//        location.reload();
//    }
}