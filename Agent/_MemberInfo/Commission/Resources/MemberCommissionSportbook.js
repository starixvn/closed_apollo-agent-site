function getData_MemberCommission_Agent()
{     
    var url='MemberCommissionSportbook.aspx?idMaster=' + $('selmaster').value + '&idAgent=' + $('selmaster').value;
    age.DelayReloadPage(url, 1);
}

function show_data_com()
{
    if($('selmaster') && $('selmaster') != 'undefined') 
    {
        var url = 'MemberCommissionSportbook.aspx?idMaster=' + $('selmaster').value + '&idAgent=' + $('selAg').value;
    }
    else
    {
        var url = 'MemberCommissionSportbook.aspx?idAgent=' + $('selAg').value;
    }
    age.DelayReloadPage(url, 1);
}