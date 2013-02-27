function getData_MemberCommission_Agent()
{     
    var url='MemberCommissionP2P.aspx?idMaster=' + $('selmaster').value + '&idAgent=' + $('selmaster').value;
    age.DelayReloadPage(url, 1);
}

function show_data_com()
{
    if($('selmaster') && $('selmaster') != 'undefined') 
    {
        var url = 'MemberCommissionP2P.aspx?idMaster=' + $('selmaster').value + '&idAgent=' + $('selAg').value;  
    }
    else
    {
        var url = 'MemberCommissionP2P.aspx?idAgent=' + $('selAg').value;        
    }
    age.DelayReloadPage(url, 1);
}