/* File Created: April 3, 2013 */

var fcHandicapOu =
{
    CurrentLeague: initLeague,
    Init: function () {
        if (fcHandicapOu.CurrentLeague != 0) {
            $('#ddlLeague').on("change", function () {
                var $this = $(this);
                fcHandicapOu.CurrentLeague = $this.val();
                if (fcHandicapOu.CurrentLeague != 0) {
                    fcHandicapOu.ChangeLeague();
                }
            });
            fcHandicapOu.ChangeLeague();
        }
    },
    RegisterEvents: function () {
        $('#forecast-body')
        .off("click")
        .on("click", ".view-forecast-detail", function () {
            var $this = $(this);
            if (countDownTimer != null) clearTimeout(countDownTimer);
            var url = window.rootUrl + '_Forecast/FCAH.aspx?matchId=' + $this.attr("match-id")
            + '&b=' + $this.attr("bet-type");
            //'_Forecast/FCAH.aspx?matchid=' + matchId + '&b=' + betType, '', 70, 50, 600, 222);
            //location.href = url;
            ageWnd.openDialog(url, '', {
                width: 680,
                height: 300,
                position: { my: "top+50", at: "center top", of: window },
                close: function (event, ui) {
                    countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                }
            });
        });
    },
    ChangeLeague: function () {
        var url = window.rootUrl + 'Forecast/FCHandicapOverUnderMatches?leagueId=' + fcHandicapOu.CurrentLeague;
        $("#forecast-body").load(url);
        fcHandicapOu.RegisterEvents();
    }
};

$(function () {
    fcHandicapOu.Init();
});

function ReloadMatches() {
    RefreshCountDown_Init();
    $('#ddlLeague').trigger("change");
}