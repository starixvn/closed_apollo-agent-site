/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var MessagePortalPage = function ($) {

    return {

        timerID: null,

        moreIsLoading: false,

        imagePath: null,

        Init: function () {
            MessagePortalPage.imagePath = APOLLO.resolveUrl('Views/Message/Images/loading_2.gif');
        },

        LoadAnnouncement: function () {

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

        ToogleDiv: function (divTitle, divContent, imgId, style, div2Closed, div3Closed, imgId2, imgId3, div4Closed, imgId4) {
            if (document.getElementById("newmemH").style.display == "block")
                document.getElementById("newmemH").style.display = "none";

            var state = document.getElementById(divContent).style.display;
            if (style != null)
                state = style;

            if (div2Closed != null) {
                document.getElementById((div2Closed + 'T')).className = "sectionClose";
                document.getElementById(imgId2).src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/collapse.png";
                document.getElementById(div2Closed).style.display = "none";
            }
            if (div3Closed != null) {
                document.getElementById((div3Closed + 'T')).className = "sectionClose";
                document.getElementById(imgId3).src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/collapse.png";
                document.getElementById(div3Closed).style.display = "none";
            }
            if (div4Closed != null) {
                document.getElementById((div4Closed + 'T')).className = "sectionClose";
                document.getElementById(imgId4).src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/collapse.png";
                document.getElementById(div4Closed).style.display = "none";
            }

            if (state != null && state == "none") {
                document.getElementById(divTitle).className = "sectionOpen";
                document.getElementById(divContent).style.display = "block";

                if ((divTitle == "newmemT" ||
            divTitle == "top10T" ||
            divTitle == "highestT") && document.getElementById(divContent).innerHTML == "") {
                    document.getElementById(imgId).src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/loading.gif";
                }

                if (divContent == "top10") {
                    if (document.getElementById('top10').innerHTML == "")
                        loadStatistic_Top10();
                    else
                        document.getElementById('imgTop10').src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/expand.png";
                }
                else if (divContent == "highest") {
                    if (document.getElementById('highest').innerHTML == "")
                        loadStatistic_Highest();
                    else
                        document.getElementById('imgHighest').src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/expand.png";
                }
                else if (divContent == "newmem") {
                    document.getElementById("newmemH").style.display = "block";
                    if (document.getElementById('newmem').innerHTML == "" && document.getElementById('newmemVal').innerHTML != "0")
                        loadStatistic_Newmem();
                    else
                        document.getElementById('imgNewmem').src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/expand.png";
                }
                else {
                    document.getElementById('imgACS').src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/expand.png";
                }
            }
            else {
                document.getElementById(divTitle).className = "sectionClose";
                document.getElementById(imgId).src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/collapse.png";

                document.getElementById(divContent).style.display = "none";

                if (divContent == "newmem") {
                    document.getElementById("newmemH").style.display = "none";
                }
            }

            document.getElementById(divContent).style.position = "absolute";
        },

        CollapseAll: function (state) {
            MessagePortalPageToogleDiv('ACST', 'ACS', 'imgACS', state);
            MessagePortalPage.ToogleDiv('top10T', 'top10', 'imgTop10', state);
            MessagePortalPage.ToogleDiv('highestT', 'highest', 'imgHighest', state);
            MessagePortalPage.ToogleDiv('newmemT', 'newmem', 'imgNewmem', state);
        },

        OpenMsg: function () {
            var baseURL = parent.location.href.substring(0, parent.location.href.lastIndexOf('/') + 1);
            window.location.href = APOLLO.resolveUrl('Message/NewMessage?pageQuery=normal');
        },

        HideDiv: function () {
            clearTimeout(MessagePortalPage.timerID);
            MessagePortalPage.CollapseAll('block');
        },

        DivOnMouseOut: function () {
            MessagePortalPage.timerID = setTimeout(MessagePortalPage.HideDiv, 10000);
        },

        DivOnMouseOver: function () {
            clearTimeout(MessagePortalPage.timerID);
            MessagePortalPage.timerID = null;
        },

        ToggleAnnouncementContent: function (elementId) {
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
                    MessagePortalPage.SetReadPriMsg(elementId);
                }
            }
        },

        SetReadPriMsg: function (messageId) {

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

        ShowMoreAnnouncements: function () {
            if (!MessagePortalPage.moreIsLoading) {
                $('#divMore').addClass('divMoreRemove');
                $('#divMore').append("<img src='" + MessagePortalPage.imagePath + "' id='imgLoading'/>");

                setTimeout(MessagePortalPage.LoadAnnouncement, 1000);
            }
        },

        showMsg: function (value) {
            $('#PageQuery').val(value);
            $('form').submit();
        }
    }
} (jQuery);

$(document).ready(function () {
    MessagePortalPage.Init();

    $('.pagequery').click(function () {
        var value = $(this).attr('query');
        MessagePortalPage.showMsg(value);
    });
});