/// <reference path="../../../_GlobalResources/Js/jquery.js" />
/*global APOLLO*/
'use strict';
/* jshint maxlen:false */
var MessagePortalPage = (function ($) {
    return {
        timerID: null,
        moreIsLoading: false,
        imagePath: null,
        publicMessageId: null,
        privateMessageId: null,
        privateMsgContent: "",
        publicMsgContent: "",
        isFirstTime: false,
        isShowLevelMessage: false,
        publicLevelMessageId: 0,
        publicLevelMessageContent: "",

        init: function () {
            MessagePortalPage.imagePath = APOLLO.resolveUrl('~/Views/Message/Images/loading_2.gif');
            MessagePortalPage.announcement = $("#announcement").val();

            MessagePortalPage.publicMessageId = 0;
            MessagePortalPage.privateMessageId = 0;
            MessagePortalPage.privateMsgContent = "";
            MessagePortalPage.publicMsgContent = "";
            MessagePortalPage.isFirstTime = false;

            MessagePortalPage.isShowLevelMessage = $("#isShowMessageLevel").val();

            MessagePortalPage.getMessages();
            if (MessagePortalPage.isShowLevelMessage) {
                MessagePortalPage.getLevelMessages();
            }
        },

        showMsg: function (value) {
            $('#PageQuery').val(value);
            $('form').submit();
        },

        getLevelMessages: function () {
            $.ajax({
                url: APOLLO.resolveUrl('~/Message/GetLevelMessage'),
                type: 'POST',
                dataType: 'JSON',
                success: function (result) {
                    if (result != null) {
                        if (MessagePortalPage.publicLevelMessageId === 0 || MessagePortalPage.publicLevelMessageId !== result.publicLevelMessageId) {
                            MessagePortalPage.publicLevelMessageId = result.publicLevelMessageId;
                            MessagePortalPage.publicLevelMessageContent = result.publicLevelMessageContent;

                            $("#topnewsbylevel").html("<marquee scrolldelay='200'>" + MessagePortalPage.publicLevelMessageContent + "</marquee>");
                        }

                        setTimeout(MessagePortalPage.getLevelMessages, (60 * 1000));
                    }
                },
                statusCode: {
                    999: function () {
                        parent.location.href = APOLLO.resolveUrl("~/Error/RedirectToUM");
                    }
                },
                error: function () {
                }
            });
        },

        getMessages: function () {
            function completedGetMessages(results) {
                try {
                    var publicMessage = (results.publicMessage != null && results.publicMessage !== 'undefined') ? results.publicMessage : 0;
                    var privateMessage = (results.privateMessages != null && results.privateMessages !== 'undefined') ? results.privateMessages : 0;
                    var finalMessage = null;
                    var MSGID = 0,
						MSGCONTENT = 1,
						MSGTYPE = 2;

                    // Process Public Message: show new public message and its link to message form
                    var url = APOLLO.resolveUrl("~/Message/NewMessage?pageQuery=");

                    url += publicMessage[MSGTYPE];

                    if (publicMessage[MSGID] != null && MessagePortalPage.publicMessageId !== publicMessage[MSGID]) {
                        MessagePortalPage.publicMsgContent = publicMessage[MSGCONTENT];
                        MessagePortalPage.publicMessageId = publicMessage[MSGID];
                    }

                    // Process Private Message: show new private message
                    if (privateMessage[MSGID] != null && MessagePortalPage.privateMessageId !== privateMessage[MSGID]) {
                        MessagePortalPage.privateMsgContent = privateMessage[MSGCONTENT];
                        MessagePortalPage.privateMessageId = privateMessage[MSGID];
                    }

                    if (MessagePortalPage.publicMsgContent !== "" && MessagePortalPage.privateMsgContent !== "") {
                        finalMessage = MessagePortalPage.publicMsgContent + "; " + MessagePortalPage.privateMsgContent;
                    }
                    else {
                        if (MessagePortalPage.publicMsgContent === "") {
                            finalMessage = MessagePortalPage.privateMsgContent;
                        }
                        else {
                            finalMessage = MessagePortalPage.publicMsgContent;
                        }
                    }

                    if (!MessagePortalPage.isFirstTime) {
                        document.getElementById('topnews').innerHTML = "<marquee scrolldelay='200'>" + finalMessage + "</marquee>";
                        MessagePortalPage.isFirstTime = true;
                    }
                    else {
                        if ((publicMessage[MSGID] != null && MessagePortalPage.publicMessageId !== publicMessage[MSGID]) || (privateMessage[MSGID] !== null && MessagePortalPage.privateMessageId !== privateMessage[MSGID])) {
                            document.getElementById('topnews').innerHTML = "<marquee scrolldelay='200'>" + finalMessage + "</marquee>";
                        }
                    }
                } catch (e) {
                    console.log(e);
                }

                setTimeout(MessagePortalPage.getMessages, (60 * 1000));
            }

            $.ajax({
                url: APOLLO.resolveUrl('~/Message/GetMessage'),
                type: 'POST',
                dataType: 'JSON',
                success: completedGetMessages,
                error: function () {
                },
                statusCode: {
                    999: function() {
                        parent.location.href = APOLLO.resolveUrl("~/Error/RedirectToUM");
                    }
                }
            });
        }
    };
} (jQuery));

$(document).ready(function () {
    MessagePortalPage.init();
    APOLLO.initFreeHeader();

    $('.pagequery').click(function () {
        var value = $(this).attr('query');
        MessagePortalPage.showMsg(value);
    });

    $(document).delegate('#announcement-link', 'click', function () {
        var myWindow = window.open(APOLLO.resolveUrl('~/Message/Announcement'), '_blank', 'scrollbars=1,width=520,height=800');
        myWindow.focus();
    });

    $(document).delegate('#announcementLevel-link', 'click', function () {
        var myWindow = window.open(APOLLO.resolveUrl('~/Message/Announcement?isLevelAnnouncement=true'), '_blank', 'scrollbars=1,width=520,height=800');
        myWindow.focus();
    });
});