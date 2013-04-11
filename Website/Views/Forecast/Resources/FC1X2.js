/* File Created: April 3, 2013 */
if (typeof jQuery != 'undefined') {
    var FC1X2 = function ($) {
        return {
            currentLeague: 0,
            matchId: matchId,
            betType: betType,
            link: APOLLO.resolveUrl('Forecast/FC1X2Matches'),
            init: function () {
                if (FC1X2.matchId > 0) {
                    FC1X2.loadMatch();
                }
                else {
                    FC1X2.currentLeague = $('#ddlLeague').val();
                    if (FC1X2.currentLeague) {
                        $('#ddlLeague').on("change", function () {
                            var $this = $(this);
                            FC1X2.currentLeague = $this.val();
                            if (FC1X2.currentLeague) {
                                FC1X2.changeLeague();
                            }
                        });
                        FC1X2.changeLeague();
                    }
                }
            },
            registerEvents: function (isLoadNewPopup) {

                $('#forecast-body')
        .off("click")
        .on("click", ".view-bet-list", function () {
            var $this = $(this);
            if (isLoadNewPopup) {
                if (countDownTimer != null) clearTimeout(countDownTimer);
                var url = APOLLO.resolveUrl('_BetList/BetList.aspx?matchId=' + $this.attr("match-id")
            + '&betType=' + $this.attr("bet-type")
            + '&type=betTypeMatch');

                //location.href = url;
                ageWnd.openDialog(url, '', {
                    width: 930,
                    height: 770,
                    position: { my: "top+50", at: "center top", of: window },
                    close: function (event, ui) {
                        countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);
                    }
                });
            }
            else {
                var url = APOLLO.resolveUrl('_BetList/BetList.aspx?matchId=' + FC1X2.matchId
                + '&betType=' + FC1X2.betType
                + '&type=betTypeMatch');
                location.href = url;
            }
        });

            },
            changeLeague: function () {
                var url = FC1X2.link + '?leagueId=' + FC1X2.currentLeague;
                $("#forecast-body").load(url);
                FC1X2.registerEvents(true);
            },
            loadMatch: function () {
                var url = FC1X2.link + '?matchId=' + FC1X2.matchId + '&betType=' + FC1X2.betType;
                $("#forecast-body").load(url);
                FC1X2.registerEvents(false);
            }
        };
    } (jQuery);

    $(function () {
        FC1X2.init();
    }).ajaxStart($.blockUI).ajaxStop($.unblockUI);
};
