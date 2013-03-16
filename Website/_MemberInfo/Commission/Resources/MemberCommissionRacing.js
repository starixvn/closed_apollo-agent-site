function getData_MemberCommission_Agent()
{     
    var url='MemberCommissionRacing.aspx?idMaster=' + document.getElementById('selmaster').value + '&idAgent=' + document.getElementById('selmaster').value;
    age.DelayReloadPage(url, 1);
}

function show_data_com()
{
    if(document.getElementById('selmaster') && document.getElementById('selmaster') != 'undefined') 
    {
        var url = 'MemberCommissionRacing.aspx?idMaster=' + document.getElementById('selmaster').value + '&idAgent=' + document.getElementById('selAg').value;  
    }
    else
    {
        var url = 'MemberCommissionRacing.aspx?idAgent=' + document.getElementById('selAg').value;        
    }
    age.DelayReloadPage(url, 1);
}