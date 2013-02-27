
function GetFCAH(matchId, score, bettype) {
    ajax.Request("FCAH.aspx", {
        asynchronous: true,
        method: 'post',
        parameters: 'matchid=' + matchId + '&score=' + score + "&b=" + bettype + "&full_mode=0",
        onComplete: function(rs) {
            $('sContent').innerHTML = rs.responseText;
        }
    });
}
function openForecast(matchId, betType) {
    if (betType == 1) {
        location.href = age.GetBaseUrl() + '_BetList/BetList.aspx?matchid=' + matchId + '&bettype=5&type=BettypeMatch';
    }
    else {
        location.href = age.GetBaseUrl() + '_BetList/BetList.aspx?matchid=' + matchId + '&bettype=15&type=BettypeMatch';
    }
}
function openForecastPopup(matchId, betType) {
    ageWnd.open(age.GetBaseUrl() + '_Forecast/FCAH.aspx?matchid=' + matchId + '&b=' + betType, '', 70, 50, 600, 210);
}

function RefreshFC() {
    if (countDownTimer != null) clearTimeout(countDownTimer);
    document.getElementById('CountDownRefresh').innerHTML = loadingTxt;

    
    var leagueid = $('ddlLeague') ? $('ddlLeague').value : null;
    // Get href

    var href = window.location.href;
    // Viewing bettype
    var betTypeInput = document.getElementById('bettype_vl');
    if (betTypeInput) {
        var betType = betTypeInput.value;
        var regExp = new RegExp("b=\\d", "g");
        href = href.replace(regExp, "b=" + betType);
    }
    // Viewing score
    var scoreInput = document.getElementById('score_vl');
    if (scoreInput) {
        var score = scoreInput.value;
        href += "&score=" + score;
    }
    
    var ajaxParameters = leagueid != null ? 'full_mode=0&leagueid=' + leagueid : 'full_mode=0';
    ajax.Request(href, { asynchronous: true, parameters: ajaxParameters
                            , onComplete: function(data) {
                                $('sContent').innerHTML = data.responseText;
                                RefreshCountDown_Init();
                            }
    });

    return true;
}

function GetForecastByLeague() {
    var leagueid = null;
    if ($('ddlLeague')) {
        leagueid = $('ddlLeague').value;
    }
    if (leagueid) {
        ajax.Request(location.href, {
            asynchronous: true,
            method: 'post',
            parameters: 'full_mode=0&leagueid=' + leagueid,
            onComplete: function(rs) {
                $('sContent').innerHTML = rs.responseText;
            }
        });
    }
}


function initFC() {

    var ageWnd = parent.ageWnd;
    if (typeof ageWnd == 'undefined') {
        return;
    }
    var h = 210;
    var w = 600;

    if (ageWnd.loaded) {
        ageWnd.setRect(ageWnd.left, ageWnd.top, w, h);
    }

}

RegisterStartUp('initFC()');