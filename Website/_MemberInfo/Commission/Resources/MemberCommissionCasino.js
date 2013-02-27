function getData_MemberCommission_Agent()
{     
    var url='MemberCommissionCasino.aspx?idMaster=' + $('selmaster').value + '&idAgent=' + $('selmaster').value;
    age.DelayReloadPage(url, 1);
}

function show_data_com()
{
    if($('selmaster') && $('selmaster') != 'undefined') 
    {
        var url = 'MemberCommissionCasino.aspx?idMaster=' + $('selmaster').value + '&idAgent=' + $('selAg').value;  
    }
    else
    {
        var url = 'MemberCommissionCasino.aspx?idAgent=' + $('selAg').value;        
    }
    age.DelayReloadPage(url, 1);
}