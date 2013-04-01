/* File Created: April 1, 2013 */

var handicapTotalBet = {
    Init: function () {
        $(".tblRpt").on("click", "a.view-forecast", function () {
            if (countDownTimer != null) clearTimeout(countDownTimer);
            var url = window.rootUrl + '_Forecast/FCAH.aspx?'
            + 'matchId=' + $(this).attr("match-id")
            + '&b=' + $(this).attr("bet-type");

            ageWnd.openDialog(url, '', {
                width: 650,
                height: 320,
                position: { my: "top+50", at: "center top", of: window },
                close: function (event, ui) {
                    countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                }
            });
        });
        $(".tblRpt").on("click", "a.view-bet-list", function () {

            var url = window.rootUrl + '_BetList/BetList.aspx?matchId=' + $(this).attr("match-id")
            + '&betType=' + $(this).attr("bet-type")
            + '&type=' + $(this).attr("betlist-type");

            location.href = url;
        });

    }
};

$(function () {
    handicapTotalBet.Init();
});