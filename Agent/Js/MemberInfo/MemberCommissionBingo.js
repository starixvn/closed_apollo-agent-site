//MemberCommissionBingo.aspx
function getData_MemberCommission_Agent() {
    var url = 'MemberCommissionBingo.aspx?idmaster=' + $('selmaster').value;
    location.href = url;

}
function show_data_com() {
    if ($('selmaster') && $('selmaster') != 'undefined') {
        var url = 'MemberCommissionBingo.aspx?idmaster=' + $('selmaster').value + '&idagent=' + $('selAg').value;
        location.href = url;
    }
    else {
        var url = 'MemberCommissionBingo.aspx?idagent=' + $('selAg').value;
        location.href = url;
    }
}
