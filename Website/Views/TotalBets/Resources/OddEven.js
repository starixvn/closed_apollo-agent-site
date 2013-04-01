/* File Created: April 1, 2013 */

var oddEvenTotalBet = {
    Init: function () {
        $(".tblRpt .fcbtn").on("click", "a", function () {
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
    }
};

$(function () {
    oddEvenTotalBet.Init();
});