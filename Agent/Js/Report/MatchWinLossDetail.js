function SearchByDate()
{
    var url='MatchWinLossDetail.aspx?fdate=' + $('fdate').value +'&tdate=' + $('tdate').value;        
    _submit();
    location.href=url;
}

function SearchOneDay(value)
{
    var url ='MatchWinLossDetail.aspx?isdate=' + value;
   _submit();
   location.href=url;
}

function ViewBetList(leagueId,matchId, rBettype, type, race) {
    //<a href='../BetList/BetList.aspx?matchid={rMatchID}&bettype={rBettype}&type={type}'>{rTypeName}</a>
    var url = "../BetList/BetList.aspx?";
    if (parseInt(race) > 0) {
        url += 'leagueid=' + leagueId + '&bettype=' + rBettype + '&type=' + type;
        url += '&fdate=' + $('fdate').value;
        url += '&tdate=' + $('tdate').value;
        url += '&race=' + race;
    }
    else {
        url += 'matchid=' + matchId + '&bettype=' + rBettype + '&type=' + type;
        if (rBettype == 10) { // Outright bet type
            url += '&fdate=' + $('fdate').value;
            url += '&tdate=' + $('tdate').value;
        }
    }
    window.location.href = url;
}
