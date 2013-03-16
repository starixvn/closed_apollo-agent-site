//MemberCommission.aspx
function getData_MemberCommission_Agent()
{     
    var url='MemberCommissionRacing.aspx?idmaster=' + document.getElementById('selmaster').value + '&idagent=' + document.getElementById('selmaster').value;            
    location.href=url;
}
function show_data_com()
{
    if(document.getElementById('selmaster') && document.getElementById('selmaster') != 'undefined') 
    {
        var url = 'MemberCommissionRacing.aspx?idmaster=' + document.getElementById('selmaster').value + '&idagent=' + document.getElementById('selAg').value;  
        location.href=url;
    }
    else
    {
        var url = 'MemberCommissionRacing.aspx?idagent=' + document.getElementById('selAg').value;        
        location.href=url;
    }     
}