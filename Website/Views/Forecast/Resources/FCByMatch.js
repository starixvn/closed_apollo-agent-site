/* File Created: April 4, 2013 */
var forecastByMatch =
{
    MatchId: matchId,
    BetType: betType,
    CurrentScore: currentScore,
    ContentLink: window.rootUrl + 'Forecast/ForecastByMatchContent',
    Init: function () {
        forecastByMatch.LoadContentView();
    },
    UpdateValue: function () {

    },
    LoadContentView: function () {
        var url = forecastByMatch.ContentLink + '?matchId=' + forecastByMatch.MatchId
        + '&betType=' + forecastByMatch.BetType
        + '&currentScore=' + forecastByMatch.CurrentScore;
        $.get(url, function (contentView) {
            $("#forecast-body").html(contentView);
            forecastByMatch.RegisterEvents();
        });
    },
    RegisterEvents: function () {
        //// Register View BestList
        $('#forecast-body .view-bet-list').click(function () {
            var $this = $(this);
            if (countDownTimer != null) clearTimeout(countDownTimer);
            var url = window.rootUrl + '_BetList/BetList.aspx?matchId=' + $this.attr("match-id")
            + '&betType=' + $this.attr("bet-type")
            + '&type=BettypeMatch';

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

        //// Register View Others BetType
        $('#forecast-body .toggle-link').click(function () {
            var $this = $(this);
            var url = forecastByMatch.ContentLink + '?matchId=' + forecastByMatch.MatchId
            + '&betType=' + $this.attr('bet-type')
            + '&currentScore=0';
            $("#forecast-body").load(url);
            forecastByMatch.RegisterEvents();
        });

        //// Register View Others Score
        $('#forecast-body')
        .off("click")
        .on("click", ".view-other-forecast", function () {
            var $this = $(this);
            var url = forecastByMatch.ContentLink + '?matchId=' + forecastByMatch.MatchId
            + '&betType=' + forecastByMatch.BetType
            + '&currentScore=' + $this.attr('forecast-point');
            $("#forecast-body").load(url);
            forecastByMatch.RegisterEvents();
        });
    }
};

$(function () {
    forecastByMatch.Init();
});

function UpdateForecastValue() {
    forecastByMatch.LoadContentView();
}