


function doViewBetList(matchId, betType, type) {
    var url = age.GetBaseUrl() + '_BetList/BetList.aspx?matchId=' + matchId + '&betType=' + betType + '&type=' + type;
    window.location.href = url;
}

function doViewBetListRacing(matchId, betType, type) {
    var url = age.GetBaseUrl() + '_BetList/BetList.aspx?matchId=' + matchId + '&betType=' + betType + '&type=' + type + '&racing=1';
    window.location.href = url;
}

function doViewBetListGame(matchId, betType, type, islive) {
    var url = age.GetBaseUrl() + '_BetList/BetList.aspx?matchId=' + matchId + '&betType=' + betType + '&type=' + type + '&islive=' + islive;
    window.location.href = url;
}

function doViewBetListNewGames(matchId, betType, type, islive, betteam) {
    var url = age.GetBaseUrl() + '_BetList/BetList.aspx?matchId=' + matchId + '&betType=' + betType + '&type=' + type + '&islive=' + islive + '&betteam=' + betteam;
    window.location.href = url;
}

function openForecast(matchId, betType) {
    isStopCoundown = true;
    var url = age.GetBaseUrl() + '_Forecast/FCAH.aspx?matchId=' + matchId + '&b=' + betType;
    //ageWnd.open(url, '', 70, 50, 600, 210);
    ageWnd.openDialog(url, '', {
        width: 600,
        height: 210,
        position: { my: "top+50", at: "center top", of: window },
        close: function (event, ui) {
            isStopCoundown = false;
        }
    });
}

function openForecast1X2(matchId, betType) {
    isStopCoundown = true;
    var url = age.GetBaseUrl() + '_Forecast/FC1X2.aspx?is_popup=1&matchId=' + matchId + '&betType=' + betType;
    //ageWnd.open(url, '', 70, 50, 600, 210);
    ageWnd.openDialog(url, '', {
        width: 600,
        height: 210,
        position: { my: "top+50", at: "center top", of: window },
        close: function (event, ui) {
            isStopCoundown = false;
        }
    });
}

var totalBets =
{
    InitViewForeCastHandicap: function () {
        $(".tblRpt").on("click", "a.view-forecast", function () {
            if (countDownTimer != null) clearTimeout(countDownTimer);
            var url = window.rootUrl + '_Forecast/FCAH.aspx?'
            + 'matchId=' + $(this).attr("match-id")
            + '&b=' + $(this).attr("bet-type");

            ageWnd.openDialog(url, 'ForeCast', {
                width: 650,
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

            var url = window.rootUrl + '_BetList/BetList.aspx?matchId=' + $(this).attr("match-id")
            + '&betType=' + $(this).attr("bet-type")
            + '&type=' + $(this).attr("betlist-type");

            //location.href = url;
            ageWnd.openDialog(url, 'Bet List', {
                width: 800,
                height: 800,
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
            ageWnd.openDialog(url, 'ForeCast', {
                width: 650,
                height: 470,
                position: { my: "top+50", at: "center top", of: window },
                close: function (event, ui) {
                    countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                }
            });
        });
    }
};

