//MemberCommission.aspx
function getData_MemberCommission_Agent()
{     
    var url='MemberCommission.aspx?idmaster=' + $('selmaster').value + '&idagent=' + $('selmaster').value;            
    location.href=url;
}
function show_data_com()
{
    if($('selmaster') && $('selmaster') != 'undefined') 
    {
        var url = 'MemberCommission.aspx?idmaster=' + $('selmaster').value + '&idagent=' + $('selAg').value;  
        location.href=url;
    }
    else
    {
        var url = 'MemberCommission.aspx?idagent=' + $('selAg').value;        
        location.href=url;
    }     
}