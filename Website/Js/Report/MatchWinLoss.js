
function SearchByDate()
{
    var url='MatchWinLossAgent.aspx?fdate=' + $('fdate').value +'&tdate=' + $('tdate').value;        
    _submit();
    location.href=url;
}
function SearchOneDay(value)
{
    var url ='MatchWinLossAgent.aspx?isdate=' + value;
    _submit();
    location.href=url;
}

function ViewBetListAG(matchId, bettype, type)
{    
    var url = '../BetList/BetList.aspx?matchid=' + matchId + '&bettype=' + bettype + '&type=' + type;
    window.location.href = url;
}

