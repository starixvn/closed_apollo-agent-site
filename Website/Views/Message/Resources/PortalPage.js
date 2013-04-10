/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var MessagePortalPage = function ($) {

    return {

        timerID: null,

        moreIsLoading: false,

        imagePath: null,

        init: function () {
            MessagePortalPage.imagePath = APOLLO.resolveUrl('Views/Message/Images/loading_2.gif');
        },

        loadAnnouncement: function () {

            var lastPostTime = getCookie("lastPostTime");

            var url = APOLLO.resolveUrl("Message/Announcement?lastPostTime=" + lastPostTime);

            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'HTML',
                success: function (res) {
                    var divMore = $('#divMore');
                    if (divMore != null) {
                        $('.divMoreRemove').remove();
                        $('#divAnnouncement').append(res);
                    }
                    else {
                        $('#announcement').append(res);
                    }

                    var moreIsVisible = getCookie('moreIsVisible');
                    if (moreIsVisible == 'False') {
                        var divMore = $('#divMore');
                        if (divMore != null) {
                            divMore.hide();
                        }
                    }
                },
                error: function (xhr, status, err) {
                }
            });
        },
       
        openMsg: function () {
            var baseURL = parent.location.href.substring(0, parent.location.href.lastIndexOf('/') + 1);
            window.location.href = APOLLO.resolveUrl('Message/NewMessage?pageQuery=normal');
        },

        toggleAnnouncementContent: function (elementId) {
            var e = $('#' + elementId);
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
        },

        setReadPriMsg: function (messageId) {

            var url = APOLLO.resolveUrl("Message/SetReadPriMsg?messageId=") + messageId;

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
        }
    }
} (jQuery);

$(document).ready(function () {
    MessagePortalPage.init();

    $('.pagequery').click(function () {
        var value = $(this).attr('query');
        MessagePortalPage.showMsg(value);
    });
});