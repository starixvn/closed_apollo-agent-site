// JScript File
function openTransfer(custid,amt,usn,site)
{
    var popH = 115, popW = 330;
    ageWnd.open('../Popup/CreditTransfer.aspx?custid=' + custid + '&amt=' + amt + '&usn=' + usn + '&sitetype=' + site, '', 50, 100, popW, popH);
}
function showTransYesBalance(custid)
{
    var isyes=1;
    var popH = 390, popW = 450;
    ageWnd.open('../_Transfer/Agent/MemberTransfer.aspx?custid=' + custid + '&disablecasino=0' + '&status=-1', '', 50, 100, popW, popH);
}
function onclickUser(username) {    
    if ($('txtUserName').value == username)
    { 
        $('txtUserName').className = 'text_f'; 
        $('txtUserName').value = ''; 
    }
}
function onblurUser(username) {
    if ($('txtUserName').value == '') 
    {
        $('txtUserName').className = 'text_f1'; 
        $('txtUserName').value = username; 
    }
}