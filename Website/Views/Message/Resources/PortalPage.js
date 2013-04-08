/// <reference path="../../../_GlobalResources/Js/jquery.js" />


var PortalPage_Paging = null;
var PortalPage_Paging_OL = null;
var count = 0;

function displayBlocks(style) {

    document.getElementById('balanceInfo').style.display = style;
    document.getElementById('statistic').style.display = style;
    document.getElementById('announcement').style.display = style;

    //    document.getElementById('balanceInfo').style.display = style;
    //    document.getElementById('statistic').style.display = style;

    //    if (_page.RemoveOnlineList == false) {
    //        document.getElementById('onlineList').style.display = style;
    //    }

    //    document.getElementById('announcement').style.display = style;

}

function loadBalanceInfo() {
    jx.request(
        'BalanceInfo.aspx',
        function (res) {
            count++;
            document.getElementById('balanceInfo').innerHTML = res.responseText;
            if (_page.RemoveOnlineList == true && count == 3) {
                count = 0;
                displayBlocks('block');
            }
            else if (count == 4) {
                count = 0;
                displayBlocks('block');
            }
        },
        'post',
        '',
        true
    );
}

function loadStatistic() {
    jx.request(
        'Statistic.aspx?' + _page.key,
        function (res) {
            count++;
            document.getElementById('statistic').innerHTML = res.responseText;
            if (_page.RemoveOnlineList == true && count == 3) {
                count = 0;
                displayBlocks('block');
            }
            else if (count == 4) {
                count = 0;
                displayBlocks('block');
            }
            collapseAll('block');
        },
        'post',
        '',
        true
    );
}

function loadStatistic_Top10() {
    jx.request(
        'Statistic.aspx?type=1&' + _page.key,
        function (res) {
            document.getElementById('imgTop10').src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/expand.png";
            document.getElementById('top10').innerHTML = res.responseText;
        },
        'post',
        '',
        true
    );
}

function loadStatistic_Highest() {
    jx.request(
        'Statistic.aspx?type=2&' + _page.key,
        function (res) {
            document.getElementById('imgHighest').src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/expand.png";
            document.getElementById('highest').innerHTML = res.responseText;
        },
        'post',
        '',
        true
    );
}

function loadStatistic_Newmem() {
    jx.request(
        'Statistic.aspx?type=3&' + _page.key,
        function (res) {
            document.getElementById('imgNewmem').src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/expand.png";
            document.getElementById('newmem').innerHTML = res.responseText;
        },
        'post',
        '',
        true
    );
}

function loadAnnouncement(url, firstRun) {
    if (firstRun == true) {
        setCookie('lastPostTime', '');
        setCookie('moreIsVisible', '');
    }
    jx.request(
        url,
        function (res) {
            count++;
            if (firstRun == true) {
                document.getElementById('announcement').innerHTML = res.responseText;
            }
            else {
                var divMore = document.getElementById('divMore');
                if (divMore != null) {

                    $('.divMoreRemove').remove();
                    $('#divAnnouncement').append(res.responseText);

                }
                else {
                    document.getElementById('announcement').innerHTML = res.responseText;
                }
            }

            var moreIsVisible = getCookie('moreIsVisible');
            if (moreIsVisible == 'False') {
                var divMore = document.getElementById('divMore');
                if (divMore != null) {
                    divMore.style.display = 'none';
                }
            }

            if (_page.RemoveOnlineList == true && count == 3) {
                count = 0;
                displayBlocks('block');
            }
            else if (count == 4) {
                count = 0;
                displayBlocks('block');
            }
            if (document.getElementById('PortalPage_Paging'))
                PortalPage_Paging = new Paging('PortalPage_Paging');
        },
        'post',
        '',
        true
    );
}

function loadAnnouncement_Paging(url) {
    jx.request(
        url,
        function (res) {
            document.getElementById('announcement').innerHTML = res.responseText;
            if (document.getElementById('PortalPage_Paging'))
                PortalPage_Paging = new Paging('PortalPage_Paging');
        },
        'get',
        '',
        true
    );
}

function loadOnlineList(url) {
    jx.request(
        url,
        function (res) {
            count++;
            document.getElementById('onlineList').innerHTML = res.responseText;
            if (_page.RemoveOnlineList == true && count == 3) {
                count = 0;
                displayBlocks('block');
            }
            else if (count == 4) {
                count = 0;
                displayBlocks('block');
            }
            if (document.getElementById('PortalPage_Paging_OL'))
                PortalPage_Paging_OL = new Paging('PortalPage_Paging_OL');
        },
        'get',
        '',
        true
    );
}

function loadOnlineList_Paging(url) {
    jx.request(
        url,
        function (res) {
            document.getElementById('onlineList').innerHTML = res.responseText;
            if (document.getElementById('PortalPage_Paging_OL'))
                PortalPage_Paging_OL = new Paging('PortalPage_Paging_OL');
        },
        'get',
        '',
        true
    );
}

function toogleDiv(divTitle, divContent, imgId, style, div2Closed, div3Closed, imgId2, imgId3, div4Closed, imgId4) {
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
}

function collapseAll(state) {
    toogleDiv('ACST', 'ACS', 'imgACS', state);
    toogleDiv('top10T', 'top10', 'imgTop10', state);
    toogleDiv('highestT', 'highest', 'imgHighest', state);
    toogleDiv('newmemT', 'newmem', 'imgNewmem', state);
}

function openMsg() {
    var baseURL = parent.location.href.substring(0, parent.location.href.lastIndexOf('/') + 1);
    window.location.href = baseURL + 'Info/NewMessageForm.aspx?pageQuery=normal';
}

var timerID = null;

function hideDiv() {
    clearTimeout(timerID);
    collapseAll('block');
}

function divOnMouseOut() {
    timerID = setTimeout(hideDiv, 10000);
}

function divOnMouseOver() {
    clearTimeout(timerID);
    timerID = null;
}

//Paging content
Paging.prototype.Go = function (index) {
    if (this.rowcount <= 0) return;

    if (!index) { // Get from textbox
        index = document.getElementById('txt' + this.id).value;
    }

    if (index <= 0) index = 1;

    if (index > 0 && index <= this.pagecount) {
        this.currentindex = index;
        var tmp = location.href;
        if (tmp.indexOf('?') >= 0)
            tmp = tmp.substring(0, tmp.indexOf('?'));

        tmp += '?pageIndex=' + this.currentindex + '&pageSize=' + this.pagesize + '&' + _page.key;

        if (this.id == 'PortalPage_Paging') {
            tmp = tmp.replace('PortalPage.aspx', 'Announcement.aspx');
            loadAnnouncement_Paging(tmp);
        }
        else if (this.id == 'PortalPage_Paging_OL') {
            tmp = tmp.replace('PortalPage.aspx', 'OnlineList.aspx');
            loadOnlineList_Paging(tmp);
        }
    }
}

function verifyOnlineList() {
    if (_page.RemoveOnlineList == false) {
        loadOnlineList('OnlineList.aspx?' + _page.key);
    }
}

function ToggleAnnouncementContent(elementId) {
    var e = $('#' + elementId);
    var icon = e.find('.toogleIcon');
    var smallContent = e.find('.small');
    var fullContent = e.find('.full');
    console.log(fullContent.attr('class'));
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
            SetReadPriMsg(elementId);
        }
    }
}

function SetReadPriMsg(messageId) {

    var url = "../Message/SetReadPriMsg";
    url += '?messageId=' + messageId;

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

}

var moreIsLoading = false;

function continueExecution() {
    //finish doing things after the pause
    var lastPostTime = getCookie("lastPostTime");

    var url = "../Message/Announcement";
    url += '?lastPostTime=' + lastPostTime;

    loadAnnouncement(url, false);

    moreIsLoading = false;
}

function ShowMoreAnnouncements() {
    if (!moreIsLoading) {
        moreIsLoading = true;
        $('#divMore').attr('class', 'divMoreRemove');
        document.getElementById('divMore').innerHTML = "<img src='../Views/Message/Images/loading_2.gif' id='imgLoading'/>";

        setTimeout(continueExecution, 1000);

    }
}

//Load all infos

//RegisterStartUp("displayBlocks('none');loadBalanceInfo();loadStatistic();verifyOnlineList();loadAnnouncement('Announcement.aspx?' + _page.key, true);");
RegisterStartUp("displayBlocks('block');verifyOnlineList();loadAnnouncement('Announcement.aspx?' + _page.key, true);");