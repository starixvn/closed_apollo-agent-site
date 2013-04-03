/* File Created: April 3, 2013 */

var fc1x2 =
{
    CurrentLeague: initLeague,
    Init: function () {
        if (fc1x2.CurrentLeague != 0) {
            $('#ddlLeague').on("change", function () {
                var $this = $(this);
                fc1x2.CurrentLeague = $this.val();
                if (fc1x2.CurrentLeague != 0) {
                    fc1x2.ChangeLeague();
                }
            });
            fc1x2.ChangeLeague();
        }
    },
    RegisterEvents: function () {
        $('#forecast-body')
        .off("click")
        .on("click", ".view-bet-list", function () {
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
    },
    ChangeLeague: function () {
        var url = window.rootUrl + 'Forecast/FC1X2Matches?leagueId=' + fc1x2.CurrentLeague;
        $("#forecast-body").load(url);
        fc1x2.RegisterEvents();
    }
};

$(function () {
    fc1x2.Init();
});

function ReloadMatches() {
    RefreshCountDown_Init();
    $('#ddlLeague').trigger("change");
}