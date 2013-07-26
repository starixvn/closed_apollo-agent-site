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
    //    try {
    //        if (parent.main.location.href.indexOf(_page.getBaseURL) == -1) {
    //            setCookie(_page.CookieCurrentURLKey, '', -1);
    //            parent.location = _page.getBaseURL;
    //            isTimeout = true;
    //            return;
    //        }
    //    } catch (e) { // Permission is denied because of cross domain issue
    //        setCookie(_page.CookieCurrentURLKey, '', -1);
    //        parent.location = _page.getBaseURL;
    //        isTimeout = true;
    //        return;
    //    }

    if (age.scrollerPaused) return;

    if (age.scroller_left + age.scroller_width < -5) {
        age.scroller_left = age.maskWidth - 90;
        age.scroller_width = parseInt(age.topnews.offsetWidth);
    }

    age.scroller.style.left = (age.scroller_left--) + 'px';
    setTimeout(age.Scrolling, 20);

    if (isTimeout) return;
}

// Balloon tip and message functions
var timeOutId;

function PrivateMsgLink(url) {
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

    var url = APOLLO.resolveUrl("~/Message/UpdatePriMsg");
    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'JSON',
        success: completeCountPrivateMsg,
        error: function (xhr, status, err) {
        }
    });
}

//var publicMessageId = 0; // public message
//var privateMessageId = 0; // private message
//function GetMessages() {
//    function completedGetMessages(results) {
//        //Result Format:
//        //{
//        // public_message:{'id','2 test message here','normal'},
//        // private_message:{'id','2 test message here'},
//        // unread_private_messages:'0'
//        //}
//        try {
//            var publicMessage = (results.public_message != null && results.public_message != 'undefined') ? results.public_message : 0;
//            var privateMessage = (results.private_message != null && results.private_message != 'undefined') ? results.private_message : 0;
//            var unreadPriMsgCount = (results.unread_private_messages != null && results.unread_private_messages != 'undefined') ? results.unread_private_messages : 0;

//            //Update count of private messages
//            var countText = document.getElementById('privateMsgLnk').innerHTML;
//            document.getElementById('privateMsgLnk').innerHTML = countText.substring(0, countText.indexOf("(") + 1) + unreadPriMsgCount + ")";

//            var MSGID = 0,
//                MSGCONTENT = 1,
//                MSGTYPE = 2;
//            var objDiv = age.createElement('div');

//            //Process Public Message: show new public message and its link to message form
//            var url = APOLLO.resolveUrl("~/Message/NewMessage?pageQuery=");
//            var balloonPopupContent = "";
//            url += publicMessage[MSGTYPE];

//            if ((publicMessage[MSGID] != null) && (publicMessageId != publicMessage[MSGID])) {
//                document.getElementById('topnews').innerHTML = publicMessage[MSGCONTENT];

//                if (publicMessageId > 0) {
//                    balloonPopupContent = publicMessage[MSGCONTENT];
//                }
//                publicMessageId = publicMessage[MSGID];
//            }

//            //Process Private Message: show new private message
//            if ((privateMessage[MSGID] != null) && (privateMessageId != privateMessage[MSGID])) {
//                document.getElementById('topnews').innerHTML += ". <span style=\"color:green\">Private message:" + privateMessage[MSGCONTENT] + "</span>";
//                if (privateMessageId > 0) {
//                    balloonPopupContent = privateMessage[MSGCONTENT];
//                }
//                privateMessageId = privateMessage[MSGID];
//            }

//            //Process Balloon Pop-up: show new public and private messages if existed
//            if (balloonPopupContent != "") {
//                SetContentPopupTop(balloonPopupContent);
//                ShowPopupTop(0);
//                timeOutId = setTimeout("ShowPopupTop(1)", (30 * 1000));
//            }

//            age.ApplyScroller();
//        } catch (e) { }

//        setTimeout(GetMessages, (60 * 1000));
//    }

//    $.ajax({
//        url: APOLLO.resolveUrl('~/Message/GetMessage'),
//        type: 'POST',
//        dataType: 'JSON',
//        success: completedGetMessages,
//        error: function (xhr, status, err) {
//        }
//    });
//}

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
    setTimeout(function () { HEADER.init(); }, 100);
}

// Change to message page
function Change2Menu(url) {
    if (parent.menu && parent.menu.ClearCurrentLeftMenu) {
        parent.menu.ClearCurrentLeftMenu();
    }

    //parent.main.location = _page.getBaseURL + url;
    parent.main.location = APOLLO.resolveUrl(url);
    SelectTopMenu(url);
}

// Clear active tab
function ClearActiveTab() {
    if (currentTab) currentTab.className = "";
}

function SelectTopMenu(url) {
    var mainUrl = '';
    if (url) {
        mainUrl = url;
    } else if (parent.main) {
        mainUrl = parent.main.location.href;
    }

    var start = mainUrl.lastIndexOf('/');
    var end = mainUrl.lastIndexOf('?');
    if (end == -1) end = mainUrl.length;
    var name = mainUrl.substring(start + 1, end);

    //if (currentTab) currentTab.className = "";

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

RegisterStartUp(InitHeader);

var HEADER = function ($) {
    return {
        IsLoadTreeView: false,

        init: function () {
            if (DEFAULTS.IsInternalLogin && !DEFAULTS.IsAllowMultipleSignIn) {
                setTimeout(function () { HEADER.reCheckIn(); }, 1000);
            }
            else {
                setTimeout(function () { HEADER.reCheckUM(); }, 1000);
            }
            HEADER.signOut();

            // show sub-menu
            $('.menu-list').click(function () {
                var $this = $(this);
                if ($this.next('.sub-menu').is(':visible')) {
                    $('.sub-menu').hide();
                } else {
                    $('.sub-menu').hide();
                    $this.next('.sub-menu').show();
                }
            });

            // show or hide left navigation
            $('.toolbar').on('click', 'li', function (event) {
                if ($(this).attr('id') == 'account-link') {
                    if (!DEFAULTS.IsMaster) {
                        var url = "/CustomerSetting/MySetting";
                        parent.document.getElementById('main').setAttribute('src', url);
                    } else {
                        var url = "/Account/AgentList";
                        HEADER.oldUrl = url;
                        parent.document.getElementById('main').setAttribute('src', url);
                    }
                    HEADER.showAccountTab();
                }
                else {
                    HEADER.hideAccountTab();
                    parent.$('#btn-mini-menu').hide();
                }
                event.stopPropagation();
            });

            // show contact us page
            $('#contact-us').click(function () {
                var contactWindow = window.open(APOLLO.resolveUrl('~/Account/ContactUs'), '_blank', 'scrollbars=1,width=800,height=428');
                contactWindow.focus();
            });

            HEADER.activeMenu();
        },

        signOut: function () {
            $("#signout").on("click", function () {
                APOLLO.signOut();
            });
        },

        reCheckIn: function () {
            $.ajax({
                type: "POST",
                url: APOLLO.resolveUrl("~/Internal/ReCheckIn"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                timeout: 120000
            })
                .done(function (result) {
                    if (DEFAULTS.IsInternalLogin && !DEFAULTS.IsAllowMultipleSignIn) {
                        if (result.isDowntime) {
                            parent.location.href = APOLLO.resolveUrl(result.urlReturn);
                        }
                        else {
                            if (result.isKickout) {
                                alert(LANGUAGES.AlerSignonMessage);
                                APOLLO.signOut();
                            }
                            else {
                                setTimeout(function () { HEADER.reCheckIn(); }, 30000);
                            }
                        }
                    }
                })
                .fail(function (event, jqXHR, ajaxSettings) {
                    APOLLO.handleAjaxError(event, jqXHR, ajaxSettings);
                });
        },

        reCheckUM: function () {
            $.ajax({
                type: "POST",
                url: APOLLO.resolveUrl("~/Internal/ReCheckUMService"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                timeout: 120000
            })
                .done(function (result) {
                    if (result.isDownTime) {
                        parent.location.href = APOLLO.resolveUrl(result.urlReturn);
                    }
                    else {
                        setTimeout(function () { HEADER.reCheckUM(); }, 30000);
                    }
                })
                .fail(function (event, jqXHR, ajaxSettings) {
                    APOLLO.handleAjaxError(event, jqXHR, ajaxSettings);
                });
        },

        showAccountTab: function () {
            var leftMenu = parent.$('.menu');
            var mainPage = parent.$('.mainpage');
            // hide left navigation
            if (leftMenu.is(':visible')) {
                HEADER.hideLeftNavigation(leftMenu, mainPage);
                parent.$('#btn-mini-menu').hide();
            } else { //show left navigation
                HEADER.showLeftNavigation(leftMenu, mainPage);

                var leftNav = parent.$('.menu'),
                    menuIcon = parent.$('.tab-menu-icon'),
                    btnMiniMenu = parent.$('#btn-mini-menu'),
                    iframeLeft = parent.$('#menu');

                if (leftNav.hasClass('mini-menu')) {
                    leftNav.removeClass('mini-menu');
                    menuIcon.hide();
                    if (btnMiniMenu.hasClass('collapse')) {
                        btnMiniMenu.removeClass('collapse').addClass('expand').show();
                    } else {
                        btnMiniMenu.removeClass('expand').addClass('collapse').show();
                    }
                    iframeLeft.show();
                } else {
                    if (btnMiniMenu.hasClass('expand')) {
                        btnMiniMenu.removeClass('expand');
                    }
                    btnMiniMenu.addClass('collapse').show();
                }
            }
        },

        hideAccountTab: function () {
            HEADER.hideLeftNavigation(parent.$('.menu'), parent.$('.mainpage'));
        },

        hideLeftNavigation: function (leftMenu, mainPage) {
            leftMenu.hide().animate(
                { width: "0" },
                {
                    duration: 10,
                    specialEasing: {
                        width: 'linear',
                        height: 'easeOutBounce'
                    },
                    complete: function () {
                        mainPage.animate({ left: '0' }, 50);
                    }
                }
            );
        },

        showLeftNavigation: function (leftMenu, mainPage) {
            leftMenu.show().animate(
                { width: "220px" },
                {
                    duration: 10,
                    specialEasing: {
                        width: 'linear',
                        height: 'easeOutBounce'
                    },
                    complete: function () {
                        mainPage.animate({ left: '221px' }, 50);
                    }
                }
            );

            if (!HEADER.IsLoadTreeView) {
                parent.document.getElementById('menu').contentWindow.location = APOLLO.resolveUrl("Account/TreeViewList");
                HEADER.IsLoadTreeView = true;
            }
            //parent.document.getElementById('menu').contentWindow.MENU.loadAgentList();
        },

        oldUrl: null,

        activeMenu: function () {
            var toolbarMenu = $('.toolbar ul li a');
            toolbarMenu.on('click', function () {
                $this = $(this);
                $iframe = parent.$('.mainpage iframe');

                var currentSubUrl = $this.attr('url');

                // Ignore link to check for click on menu many times
                if (HEADER.oldUrl == currentSubUrl &&
                currentSubUrl != "/Reports/matchentered" &&
                currentSubUrl != "/Account/AgentList") {
                    return;
                }

                HEADER.oldUrl = currentSubUrl;

                $iframe.attr('src', currentSubUrl);
            });
        }
    };
} (jQuery);