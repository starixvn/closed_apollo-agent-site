var totalBets =
{
    InitViewForeCastHandicap: function () {
        $(".tblRpt").on("click", "a.view-forecast", function () {
            if (countDownTimer != null) clearTimeout(countDownTimer);
            var url = window.rootUrl + '_Forecast/FCAH.aspx?'
            + 'matchId=' + $(this).attr("match-id")
            + '&b=' + $(this).attr("bet-type");

            ageWnd.openDialog(url, '', {
                width: 665,
                height: 320,
                position: { my: "top+50", at: "center top", of: window },
                close: function (event, ui) {
                    countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                }
            });
        });
    },
    InitViewBetList: function () {
        $(".tblRpt").on("click", "a.view-bet-list", function () {
            if (countDownTimer != null) clearTimeout(countDownTimer);
            var url = window.rootUrl + '_BetList/BetList.aspx?matchId=' + $(this).attr("match-id")
            + '&betType=' + $(this).attr("bet-type")
            + '&type=' + $(this).attr("betlist-type");

            //location.href = url;
            ageWnd.openDialog(url, '', {
                width: 930,
                height: 770,
                position: { my: "top+50", at: "center top", of: window },
                close: function (event, ui) {
                    countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                }
            });
        });
    },
    InitViewBetListGame: function () {
        $(".tblRpt").on("click", "a.view-bet-list", function () {
            if (countDownTimer != null) clearTimeout(countDownTimer);
            var url = window.rootUrl + '_BetList/BetList.aspx?matchId=' + $(this).attr("match-id")
            + '&betType=' + $(this).attr("bet-type")
            + '&type=' + $(this).attr("betlist-type")
            + '&islive=' + $(this).attr("is-live");
            //location.href = url;
            ageWnd.openDialog(url, '', {
                width: 930,
                height: 770,
                position: { my: "top+50", at: "center top", of: window },
                close: function (event, ui) {
                    countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                }
            });
        });
    },
    InitViewBetListNewGame: function () {
        $(".tblRpt").on("click", "a.view-bet-list-new-game", function () {
            if (countDownTimer != null) clearTimeout(countDownTimer);
            var url = window.rootUrl + '_BetList/BetList.aspx?matchId=' + $(this).attr("match-id")
            + '&betType=' + $(this).attr("bet-type")
            + '&type=' + $(this).attr("betlist-type")
            + '&islive=' + $(this).attr("is-live")
            + '&betteam=' + $(this).attr("bet-team");
            //location.href = url;
            ageWnd.openDialog(url, '', {
                width: 930,
                height: 770,
                position: { my: "top+50", at: "center top", of: window },
                close: function (event, ui) {
                    countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                }
            });
        });
    },
    InitViewForecast1x2: function () {
        $(".tblRpt").on("click", "a.view-forecast", function () {
            if (countDownTimer != null) clearTimeout(countDownTimer);
            var url = window.rootUrl + '_Forecast/FC1X2.aspx?'
            + 'matchId=' + $(this).attr("match-id")
            + '&betType=' + $(this).attr("bet-type");
            ageWnd.openDialog(url, '', {
                width: 350,
                height: 520,
                position: { my: "top+50", at: "center top", of: window },
                close: function (event, ui) {
                    countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                }
            });
        });
    },
    InnitFreeHeader: function () {
        var clonedHeaderRow;
        $(".persist-area").each(function () {
            clonedHeaderRow = $(".persist-header", this);
            clonedHeaderRow
	             .before(clonedHeaderRow.clone())
	             .css("width", clonedHeaderRow.width())
	             .addClass("floatingHeader")
	             .removeClass("persist-header");

        });

        $(window)
	        .scroll(this.updateTableHeaders)
	        .trigger("scroll");
        $(window).resize(function () {
            updateTableHeaders();
        });
    },
    updateTableHeaders: function () {
        if ($(".persist-area").val() != undefined) {
            var widthspace = $(".persist-area").width() + 18;

            $('.floatingHeader').width(widthspace);
            var left = $('table thead tr:first').position()["left"];
            $(".persist-area").each(function () {
                var el = $(this),
                    offset = el.offset(),
                    scrollTop = $(window).scrollTop(),
                    floatingHeader = $(".floatingHeader", this)
                if ((scrollTop > offset.top - 60) && (scrollTop < offset.top + el.height())) {
                    floatingHeader.css({
                        "visibility": "visible",
                        "left": (left - $(window).scrollLeft())
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

