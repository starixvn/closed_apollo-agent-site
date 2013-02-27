//MemberCommission.aspx
function getData_MemberCommission_Agent()
{     
    var url='MemberCommissionRacing.aspx?idmaster=' + $('selmaster').value + '&idagent=' + $('selmaster').value;            
    location.href=url;
}
function show_data_com()
{
    if($('selmaster') && $('selmaster') != 'undefined') 
    {
        var url = 'MemberCommissionRacing.aspx?idmaster=' + $('selmaster').value + '&idagent=' + $('selAg').value;  
        location.href=url;
    }
    else
    {
        var url = 'MemberCommissionRacing.aspx?idagent=' + $('selAg').value;        
        location.href=url;
    }     
}