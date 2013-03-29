/*
* Created 20100203@Lee - Header page functions
* Revision:
* Don@20100616 - Refactor user message function: use JSON format for processing ajax response
*/
// Scroller message _page.section ------------------
AGE.prototype.ApplyScroller = function () {
    if (typeof this.scroller_left != 'undefined') return; // Apply first time only
    this.scroller_left = -100;
    this.scroller_width = 0;

    var text_news = document.getElementById('text_news');

    this.scroller = document.getElementById('scroller');
    this.topnews = document.getElementById('topnews');

    this.topnews.onmouseover = function () {
        this.scrollerPaused = true;
    };
    this.topnews.onmouseout = function () {
        this.scrollerPaused = false;
    };

    this.maskWidth = parseInt(text_news.offsetWidth);

    this.Scrolling();
}

var isTimeout = false;

// age=this is the object of AGE instance
AGE.prototype.Scrolling = function () {
    try {
        if (top.main.location.href.indexOf(_page.getBaseURL) == -1) {
            setCookie(_page.CookieCurrentURLKey, '', -1);
            top.location = _page.getBaseURL;
            isTimeout = true;
            return;
        }
    } catch (e) { // Permission is denied because of cross domain issue
        setCookie(_page.CookieCurrentURLKey, '', -1);
        top.location = _page.getBaseURL;
        isTimeout = true;
        return;
    }

    if (age.scrollerPaused) return;

    if (age.scroller_left + age.scroller_width < -5) {
        age.scroller_left = age.maskWidth + 5;
        age.scroller_width = parseInt(age.topnews.offsetWidth);
    }

    age.scroller.style.left = (age.scroller_left--) + 'px';
    setTimeout(age.Scrolling, 20);

    if (isTimeout) return;
}

function ChangeLang(lang) {
    function OnComplete(result) {
        var errCode = result.errCode;
        if (errCode == 0) {
            // Remove www so that subdomain can override LANGUAGE value
            var domain = window.location.hostname.replace(/([a-zA-Z0-9]+.)/, '').replace('www.', '');
            setCookie('LANGUAGE', lang, 30, domain);
            age.Invalidate();
        } else {
            if (errCode == ACCESS_DENIED) {
                age.SignOut();
            }
        }
    }

    // Post with return result in JSON format
    ajax.PostJSON('Handlers/SetLanguage.ashx', 'lang=' + lang, OnComplete);
}

function Change2Menu(url) {
    if (top.menu && top.menu.ClearCurrentLeftMenu) {
        top.menu.ClearCurrentLeftMenu();
    }

    top.main.location = _page.getBaseURL + url;
    SelectTopMenu(url);
}

function SetCurrentLanguage() {
    document.getElementById('selLanguage').value = _page.Language;
}

function ClearActiveTab() {
    if (currentTab) currentTab.className = "";
}

// For keeping top menu selected correctly
var currentTab = null;

function SelectTopMenu(url) {
    var mainUrl = '';
    if (url) {
        mainUrl = url;
    } else if (top.main) {
        mainUrl = top.main.location.href;
    }

    var start = mainUrl.lastIndexOf('/');
    var end = mainUrl.lastIndexOf('?');
    if (end == -1) end = mainUrl.length;
    var name = mainUrl.substring(start + 1, end);

    if (currentTab) currentTab.className = "";

    switch (name) {
        case 'PortalPage.aspx':
            currentTab = document.getElementById("balance");
            break;
        case 'MemberList.aspx':
            currentTab = document.getElementById("transfer");
            break;
        case 'AgentList.aspx':
            currentTab = document.getElementById("transfer");
            break;
        case 'MasterList.aspx':
            currentTab = document.getElementById("transfer");
            break;
        case 'ChangePassword.aspx':
            currentTab = document.getElementById("changepass");
            break;
        case 'LogCustomerSetting.aspx':
            currentTab = document.getElementById("viewlog");
            break;
        default:
            currentTab = null;
            break;
    }

    if (currentTab) currentTab.className = "active";
    else ClearActiveTab();
}

function isDate(TDY, TDM, TDD) // input 3 string
{
    var BegDate = new Date(parseFloat(TDY) + "/" + TDM + "/" + TDD);
    if (BegDate.getYear() < 200) var TBegYear = BegDate.getYear() + 1900
    else var TBegYear = BegDate.getYear()
    if (TBegYear != parseFloat(TDY) || BegDate.getMonth() + 1 != parseFloat(TDM) || BegDate.getDate() != parseFloat(TDD)) return false;
    else return true;
}

function ClockTick() {
    _page.sec++;
    if (_page.sec == 60) {
        _page.sec = 0;
        _page.min++;
    }
    if (_page.min == 60) {
        _page.min = 0;
        _page.hrs++;
    }
    if (_page.hrs == 24) {
        _page.hrs = 0;
        _page.sec = 0;
        _page.min = 0;
        _page.day++;
    }
    if (_page.min < 10) {
        strmin = "0" + _page.min;
    } else {
        strmin = _page.min;
    }
    if (_page.sec < 10) {
        strsec = "0" + _page.sec;
    } else {
        strsec = _page.sec;
    }

    if (!isDate(_page.year, _page.month, _page.day)) //to next _page.month
    {
        _page.month++;
        _page.day = 1;
    }
    if (!isDate(_page.year, _page.month, _page.day)) //to next _page.year
    {
        _page.year++;
        _page.month = 1;
        _page.day = 1;
    }

    if (_page.hrs >= 12) {
        stra = _page.PM;
    } else {
        stra = _page.AM;
    }
    if (_page.hrs >= 12) {
        strhrs = _page.hrs - 12;
    } else {
        strhrs = _page.hrs;
    }

    var months = eval(_page.Months);

    var str = strhrs + ":" + strmin + ":" + strsec + " " + stra + " " + months[_page.month - 1] + " " + _page.day + ", " + _page.year + " GMT-4";
    document.getElementById("clock").innerHTML = str;

    setTimeout(ClockTick, 1000);
}

// Balloon tip and message functions
var timeOutId;

function PrivateMsgLink(url) {
    Change2Menu(url);
    var open = "(",
        close = ")";
    var countText = document.getElementById('privateMsgLnk').innerHTML;
    var countPrivateMsg = countText.substring(countText.indexOf(open) + 1, countText.indexOf(close));
    if (parseInt(countPrivateMsg) > 0) {
        UpdateCountPrivateMsg();
    }
}

function UpdateCountPrivateMsg() {
    function completeCountPrivateMsg(data) {
        if (data.errCode == 0) {
            var open = "(",
                noMsg = "0";
            var countText = document.getElementById('privateMsgLnk').innerHTML;
            document.getElementById('privateMsgLnk').innerHTML = countText.substring(0, countText.indexOf(open) + 1) + noMsg + ")";
        }
    }

    ajax.PostJSON(
     _page.getBaseURL + '_UserMessage/Handlers/UpdatePrivateMessageCount.ashx', '', completeCountPrivateMsg);
}

var publicMessageId = 0; // public message
var privateMessageId = 0; // private message
function GetMessages() {
    function completedGetMessages(results) {
        //Result Format:
        //{
        // public_message:{'id','2 test message here','normal'},
        // private_message:{'id','2 test message here'},
        // unread_private_messages:'0'
        //}
        try {
            var publicMessage = (results.public_message != null && results.public_message != 'undefined') ? results.public_message : 0;
            var privateMessage = (results.private_message != null && results.private_message != 'undefined') ? results.private_message : 0;
            var unreadPriMsgCount = (results.unread_private_messages != null && results.unread_private_messages != 'undefined') ? results.unread_private_messages : 0;

            //Update count of private messages
            var countText = document.getElementById('privateMsgLnk').innerHTML;
            document.getElementById('privateMsgLnk').innerHTML = countText.substring(0, countText.indexOf("(") + 1) + unreadPriMsgCount + ")";

            var MSGID = 0,
                MSGCONTENT = 1,
                MSGTYPE = 2;
            var objDiv = age.createElement('div');

            //Process Public Message: show new public message and its link to message form
            var url = "Info/NewMessageForm.aspx?pageQuery=";
            var balloonPopupContent = "";
            url += publicMessage[MSGTYPE];

            if ((publicMessage[MSGID] != null) && (publicMessageId != publicMessage[MSGID])) {
                document.getElementById('topnews').innerHTML = publicMessage[MSGCONTENT];
                document.getElementById('text_news_out').onclick = new Function("Change2Menu('" + url + "')"); //Work with FF, IE6-7
                if (publicMessageId > 0) {
                    balloonPopupContent = publicMessage[MSGCONTENT];
                }
                publicMessageId = publicMessage[MSGID];
            }

            //Process Private Message: show new private message
            if ((privateMessage[MSGID] != null) && (privateMessageId != privateMessage[MSGID])) {
                document.getElementById('topnews').innerHTML += ". <font color=\"green\">Private message:" + privateMessage[MSGCONTENT] + "</font>";
                if (privateMessageId > 0) {
                    balloonPopupContent = privateMessage[MSGCONTENT];
                }
                privateMessageId = privateMessage[MSGID];
            }

            //Process Balloon Pop-up: show new public and private messages if existed
            if (balloonPopupContent != "") {
                SetContentPopupTop(balloonPopupContent);
                ShowPopupTop(0);
                timeOutId = setTimeout("ShowPopupTop(1)", (30 * 1000));
            }

            age.ApplyScroller();
        } catch (e) { }

        setTimeout(GetMessages, (60 * 1000));
    }

    ajax.PostJSON(_page.getBaseURL + '_UserMessage/Handlers/GetMessages.ashx', '', completedGetMessages);
}

function ReplaceAll(source, stringToFind, stringToReplace) {
    var temp = source;

    var index = temp.indexOf(stringToFind);

    while (index != -1) {
        temp = temp.replace(stringToFind, stringToReplace);
        index = temp.indexOf(stringToFind);
    }

    return temp;
}

var msgIndex = 0;
function ShowHotMsg(msgs) {
    if (msgs.length == 0) {
        document.getElementById("Popup_top").style.display = 'none';

        return;
    }

    var Popup_contents1 = document.getElementById('Popup_contents');
    var Popup_contents2 = document.getElementById('Popup_contents2');

    Popup_contents1.innerHTML = msgs[msgIndex];
    if (msgs.length > 1) {
        Popup_contents2.innerHTML = msgs[msgIndex + 1];
    }

    ShowPopupTop(true);

    document.getElementById("Popup_top").style.height = Popup_contents1.clientHeight + 'px';

    var height = 0;
    var marginTop = 0;
    var timeOutSwap = 0;
    function swapMessage() {
        marginTop -= 5;
        if (Math.abs(marginTop) >= height) {
            marginTop = 0;
            var parentNode = Popup_contents1.parentNode;
            parentNode.removeChild(Popup_contents1);
            parentNode.appendChild(Popup_contents1);
            var node2 = Popup_contents2;
            Popup_contents2 = Popup_contents1;
            Popup_contents1 = node2;

            Popup_contents1.style.marginTop = marginTop + "px";
            Popup_contents2.style.marginTop = marginTop + "px";

            document.getElementById("Popup_top").style.height = Popup_contents1.clientHeight + 'px';

            return;
        }

        Popup_contents1.style.marginTop = marginTop + "px";
        setTimeout(swapMessage, 50);
    }

    function startSwap() {
        if (msgIndex >= msgs.length - 1) {
            msgIndex = -1;
        }
        height = Popup_contents1.clientHeight + 20;
        Popup_contents2.innerHTML = msgs[++msgIndex];
        timeOutSwap = setTimeout(swapMessage, 50);
    }

    if (msgs.length > 1) {
        setInterval(startSwap, 10000);
    }
}

function ShowPopupTop(show) {
    if (!show) {
        document.getElementById("Popup_top").style.display = "none";
    } else {
        document.getElementById("Popup_top").style.display = "block";
    }
    return false;
}

function SetContentPopupTop(content) {
    document.getElementById("Popup_contents").innerHTML = content;
}

// Hide Tooltip after 5s
function HideTooltipIntroNickname() {
    if (document.getElementById('tooltip_box') != null) setTimeout("document.getElementById('tooltip_box').style.display ='none'", 10000);
}

// Start up
function InitHeader() {
    // SetCurrentLanguage();
    //ShowHotMsg(_page.msgList);
    timeoutId1 = setTimeout(GetMessages, 100);
    //ClockTick();
    SelectTopMenu();
    HideTooltipIntroNickname();
}

RegisterStartUp(InitHeader);