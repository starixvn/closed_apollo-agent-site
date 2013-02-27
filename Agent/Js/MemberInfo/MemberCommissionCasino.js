//MemberCommissionCasino.aspx
function getData_MemberCommission_Agent()
{     
    var url='MemberCommissionCasino.aspx?idmaster=' + $('selmaster').value;            
    location.href=url;
      
}
function show_data_com()
{       
    if($('selmaster') && $('selmaster') != 'undefined') 
    {
        var url = 'MemberCommissionCasino.aspx?idmaster=' + $('selmaster').value + '&idagent=' + $('selAg').value;  
        location.href=url;
    }
    else
    {
        var url = 'MemberCommissionCasino.aspx?idagent=' + $('selAg').value;        
        location.href=url;
    }     
}
