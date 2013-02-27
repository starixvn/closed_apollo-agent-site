function InitPage() {
    initAutoComplete('txtUserName', '../../_GlobalResources/Handlers/QueryUserName.ashx?custid=' + _page.CustId + '&isdir=1');
}

RegisterStartUp('InitPage()');

function viewOutSt(custId, userName) {
    window.location = '../../_Reports/BetList/OutstandingDetail.aspx?custid=' + custId + '&username=' + userName + '&type=RunByDate_Mem' + '&chkgetall=on' + '&chk_all=on' + '&chk_showCasino=on' + '&chk_showSB=on' + '&chk_showRB=on';
}

function viewBal(action, custId) {
    window.location = "CreditBalanceList.aspx?action=" + action + "&custid=" + custId + "&pageIndex=1&pageSize=" + _page.PageSize;
}

function searchByUsername(url, site) {
    var txtname = $('txtUserName').value;
    if (txtname == _page.UserNameDefault) {
        txtname = '';
    }

    url += '?' + 'custid=' + _page.CustId;
    url += '&username=' + encodeURIComponent(txtname.escapeHtml());
    url += '&status=' + $('statusFilter').value;
    url += "&action=" + site;
    url += '&pageIndex=1&pageSize=' + _page.PageSize;
    age.DelayReloadPage(url, 1);
}

function openTransfer(custId, amt, userName, roleId) {
    var popH = 115, popW = 330;
    ageWnd.open('CreditTransfer.aspx?custid=' + custId + '&amt=' + amt + '&username=' + userName + '&roleId=' + roleId, '', 50, 100, popW, popH);
}
function showTransYesBalance(custId) {
    var isyes = 1;
    var popH = 390, popW = 450;
    ageWnd.open('../../_Transfer/Agent/MemberTransfer.aspx?custid=' + custId, '', 50, 100, popW, popH);
}

/* Overwright Transfer.js */
function DelayReloadPage() {
    age.DelayReloadPage(); // Reload page after 2s
}
function OnPopupComplete(reload) {
    ageWnd.close(); // Close popup
    alert(_page.wrnTransferSuccessful);
    DelayReloadPage();
}

String.prototype.escapeHtml = function () { // Replace all ampersands with &amp; and all <’s and >’s with &lt; and &gt;, respectively:
    var result = '';
    for (var i = 0; i < this.length; i++) {
        if (this.charAt(i) == "&" && this.length - i - 1 >= 4 && this.substr(i, 4) != "&amp;") {
            result = result + "&amp;";
        } else if (this.charAt(i) == "<") {
            result = result + "&lt;";
        } else if (this.charAt(i) == ">") {
            result = result + "&gt;";
        } else {
            result = result + this.charAt(i);
        }
    }
    return result;
};