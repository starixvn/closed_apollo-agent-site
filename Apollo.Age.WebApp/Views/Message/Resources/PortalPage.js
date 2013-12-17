/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var MessagePortalPage = function ($) {

    return {

        timerID: null,

        moreIsLoading: false,

        imagePath: null,

        publicMessageId: null,

        privateMessageId: null,

        privateMsgContent: "",

        publicMsgContent: "",

        isFirstTime: false,

        init: function () {
            MessagePortalPage.imagePath = APOLLO.resolveUrl('~/Views/Message/Images/loading_2.gif');
            MessagePortalPage.announcement = $("#announcement").val();

            MessagePortalPage.publicMessageId = 0;
            MessagePortalPage.privateMessageId = 0;
            MessagePortalPage.privateMsgContent = "";
            MessagePortalPage.publicMsgContent = "";
            MessagePortalPage.isFirstTime = false;

            MessagePortalPage.getMessages();
            MessagePortalPage.getLevelMessages();

        },

        loadAnnouncement: function () {

            var lastPostTime = getCookie("lastPostTime");

            var url = APOLLO.resolveUrl("~/Message/Announcement?lastPostTime=" + lastPostTime);

            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'HTML',
                success: function (res) {
                    var divMore = $('#divMore');
                    if (divMore.length > 0) {
                        $('.divMoreRemove').remove();
                        $('#divAnnouncement').append(res);
                    }
                    else {
                        $('#announcement').append(res);
                    }

                    var divAnnouncement = document.getElementById("divAnnouncement");
                    if (divAnnouncement != undefined) {
                        divAnnouncement.scrollTop = divAnnouncement.scrollHeight;
                    }

                    var moreIsVisible = getCookie('moreIsVisible');
                    if (moreIsVisible == 'False') {
                        // var divMore = $('#divMore');
                        if (divMore.length > 0) {
                            divMore.hide();
                        }
                    }
                },
                error: function (xhr, status, err) {
                }
            });
        },

        openMsg: function () {
            //var baseURL = parent.location.href.substring(0, parent.location.href.lastIndexOf('/') + 1);
            window.location.href = APOLLO.resolveUrl('~/Message/NewMessage?pageQuery=normal');
        },

        toggleAnnouncementContent: function (elementId) {
            var e = $('#' + elementId);

            if (e.length > 0) {
                var icon = e.find('.toogleIcon');
                var smallContent = e.find('.small');
                var fullContent = e.find('.full');

                if (fullContent.hasClass('text_exposed_show')) {
                    if (e.hasClass('expanding')) {
                        e.removeClass('expanding');
                    };
                    smallContent.attr('class', 'text_exposed_show small');
                    fullContent.attr('class', 'text_exposed_hide full');

                    icon.attr('style', 'background-position: 0px 0px;');
                }
                else {
                    e.addClass('expanding');
                    smallContent.attr('class', 'text_exposed_hide small');
                    fullContent.attr('class', 'text_exposed_show full');
                    icon.attr('style', 'background-position: 0px -9px;');
                    var isRead = e.attr('isRead');
                    if (isRead == '0') {
                        MessagePortalPage.setReadPriMsg(elementId);
                    }
                }
            }
        },

        setReadPriMsg: function (messageId) {

            var url = APOLLO.resolveUrl("~/Message/SetReadPriMsg?messageId=") + messageId;

            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'JSON',
                success: function (result) {
                    var e = $('#' + messageId);
                    e.attr('style', 'font-weight : normal');
                    e.attr('isRead', '1');
                },
                error: function (xhr, status, err) {
                }
            });
        },

        showMoreAnnouncements: function () {
            if (!MessagePortalPage.moreIsLoading) {
                $('#divMore').addClass('divMoreRemove');
                $('#divMore').append("<img src='" + MessagePortalPage.imagePath + "' id='imgLoading'/>");

                setTimeout(MessagePortalPage.loadAnnouncement, 1000);
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
                    $("#topnewsbylevel").html("<marquee scrolldelay='200'>" + result.public_message_content + "</marquee>");
                    setTimeout(MessagePortalPage.getLevelMessages, (60 * 1000));
                },
                error: function (xhr, status, err) {
                }
            });
        },

        getMessages: function () {


            function completedGetMessages(results) {

                try {
                    var publicMessage = (results.public_message != null && results.public_message != 'undefined') ? results.public_message : 0;
                    var privateMessage = (results.private_message != null && results.private_message != 'undefined') ? results.private_message : 0;
                    var unreadPriMsgCount = (results.unread_private_messages != null && results.unread_private_messages != 'undefined') ? results.unread_private_messages : 0;

                    var MSGID = 0,
                        MSGCONTENT = 1,
                        MSGTYPE = 2;

                    // Process Public Message: show new public message and its link to message form
                    var url = APOLLO.resolveUrl("~/Message/NewMessage?pageQuery=");

                    url += publicMessage[MSGTYPE];

                    if (publicMessage[MSGID] != null && MessagePortalPage.publicMessageId != publicMessage[MSGID]) {

                        MessagePortalPage.publicMsgContent = publicMessage[MSGCONTENT];

                        MessagePortalPage.publicMessageId = publicMessage[MSGID];

                    }

                    // Process Private Message: show new private message
                    if (privateMessage[MSGID] != null && MessagePortalPage.privateMessageId != privateMessage[MSGID]) {

                        MessagePortalPage.privateMsgContent = privateMessage[MSGCONTENT];

                        MessagePortalPage.privateMessageId = privateMessage[MSGID];

                    }

                    if (!MessagePortalPage.isFirstTime) {
                        document.getElementById('topnews').innerHTML = "<marquee scrolldelay='200'>" + (MessagePortalPage.publicMsgContent != "" ? MessagePortalPage.publicMsgContent + ";" : "") + " " + MessagePortalPage.privateMsgContent + "</marquee>";
                        MessagePortalPage.isFirstTime = true;
                    }
                    else {
                        if ((publicMessage[MSGID] != null && MessagePortalPage.publicMessageId != publicMessage[MSGID]) || (privateMessage[MSGID] != null && MessagePortalPage.privateMessageId != privateMessage[MSGID])) {
                            document.getElementById('topnews').innerHTML = "<marquee scrolldelay='200'>" + (MessagePortalPage.publicMsgContent != "" ? MessagePortalPage.publicMsgContent + ";" : "") + " " + MessagePortalPage.privateMsgContent + "</marquee>";
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
                error: function (xhr, status, err) {
                }
            });
        }

    }
} (jQuery);

$(document).ready(function () {
    MessagePortalPage.init();
    APOLLO.initFreeHeader();

    $('.pagequery').click(function () {
        var value = $(this).attr('query');
        MessagePortalPage.showMsg(value);
    });

    $(document).delegate('#divMore', 'click', function () {
        MessagePortalPage.showMoreAnnouncements();
    });

    $(document).delegate('.message', 'click', function () {
        var messageId = $(this).attr('id');
        MessagePortalPage.toggleAnnouncementContent(messageId);
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