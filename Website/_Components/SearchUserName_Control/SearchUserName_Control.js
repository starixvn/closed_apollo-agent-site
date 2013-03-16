function onclickUser(username) {
    if (document.getElementById('txtUserName').value == username) {
        document.getElementById('txtUserName').className = 'text_normal';
        document.getElementById('txtUserName').value = '';
    }
}
function onblurUser(username) {
    if (document.getElementById('txtUserName').value == '') {
        document.getElementById('txtUserName').className = 'text_italic';
        document.getElementById('txtUserName').value = username;
    }
}

function onKeyPressUser(submitID,event) {
    if (!event) event = window.event;
    var key = (event.keyCode) ? event.keyCode : event.which;
    if (key == 13) {
        document.getElementById(submitID).click();
        return false;
    }
}

function InitTagSuggestion() {
    initAutoComplete('txtUserName', '../../../_GlobalResources/Handlers/QueryUserName.ashx?custid=' + _page.custid + '&isdir=1');
}