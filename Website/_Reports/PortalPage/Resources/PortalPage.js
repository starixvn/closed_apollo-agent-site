/*
Author: Andy.Nguyen
Revision: 2011-05-31
Description: Adds new functions for Portal Page requirements
*/

var PortalPage_Paging = null;
var PortalPage_Paging_OL = null;
var count = 0;

function displayBlocks(style) {
    $('balanceInfo').style.display = style;
    $('statistic').style.display = style;
    if (_page.RemoveOnlineList == false) $('onlineList').style.display = style;
    $('announcement').style.display = style;
}

function loadBalanceInfo() {
    jx.request(
        'BalanceInfo.aspx',
        function (res) {
            count++;
            $('balanceInfo').innerHTML = res.responseText;
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
            $('statistic').innerHTML = res.responseText;
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
            $('imgTop10').src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/expand.png";
            $('top10').innerHTML = res.responseText;
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
            $('imgHighest').src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/expand.png";
            $('highest').innerHTML = res.responseText;
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
            $('imgNewmem').src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/expand.png";
            $('newmem').innerHTML = res.responseText;
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
                $('announcement').innerHTML = res.responseText;
            }
            else {
                var divMore = $('divMore');
                if (divMore != null) {
                    var div = document.createElement('div');
                    div.innerHTML = res.responseText;
                    var elements = div.childNodes;
                    var root = elements[0];
                    while (root.childNodes.length > 0) {
                        $('divAnnouncement').insertBefore(root.childNodes[0], $('divAnnouncement').lastChild);
                    }
                }
                else {
                    $('announcement').innerHTML = res.responseText;
                }
            }

            var moreIsVisible = getCookie('moreIsVisible');
            if (moreIsVisible == 'False') {
                var divMore = $('divMore');
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
            if ($('PortalPage_Paging'))
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
            $('announcement').innerHTML = res.responseText;
            if ($('PortalPage_Paging'))
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
            $('onlineList').innerHTML = res.responseText;
            if (_page.RemoveOnlineList == true && count == 3) {
                count = 0;
                displayBlocks('block');
            }
            else if (count == 4) {
                count = 0;
                displayBlocks('block');
            }
            if ($('PortalPage_Paging_OL'))
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
            $('onlineList').innerHTML = res.responseText;
            if ($('PortalPage_Paging_OL'))
                PortalPage_Paging_OL = new Paging('PortalPage_Paging_OL');
        },
        'get',
        '',
        true
    );
}


function toogleDiv(divTitle, divContent, imgId, style, div2Closed, div3Closed, imgId2, imgId3, div4Closed, imgId4) {
    if ($("newmemH").style.display == "block")
        $("newmemH").style.display = "none";

    var state = $(divContent).style.display;
    if (style != null)
        state = style;

    if (div2Closed != null) {
        $((div2Closed + 'T')).className = "sectionClose";
        $(imgId2).src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/collapse.png";
        $(div2Closed).style.display = "none";
    }
    if (div3Closed != null) {
        $((div3Closed + 'T')).className = "sectionClose";
        $(imgId3).src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/collapse.png";
        $(div3Closed).style.display = "none";
    }
    if (div4Closed != null) {
        $((div4Closed + 'T')).className = "sectionClose";
        $(imgId4).src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/collapse.png";
        $(div4Closed).style.display = "none";
    }

    if (state != null && state == "none") {
        $(divTitle).className = "sectionOpen";
        $(divContent).style.display = "block";

        if ((divTitle == "newmemT" ||
            divTitle == "top10T" ||
            divTitle == "highestT") && $(divContent).innerHTML == "") {
            $(imgId).src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/loading.gif";
        }

        if (divContent == "top10") {
            if ($('top10').innerHTML == "")
                loadStatistic_Top10();
            else
                $('imgTop10').src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/expand.png";
        }
        else if (divContent == "highest") {
            if ($('highest').innerHTML == "")
                loadStatistic_Highest();
            else
                $('imgHighest').src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/expand.png";
        }
        else if (divContent == "newmem") {
            $("newmemH").style.display = "block";
            if ($('newmem').innerHTML == "" && $('newmemVal').innerHTML != "0")
                loadStatistic_Newmem();
            else
                $('imgNewmem').src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/expand.png";
        }
        else {
            $('imgACS').src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/expand.png";
        }
    }
    else {
        $(divTitle).className = "sectionClose";
        $(imgId).src = age.GetBaseUrl().substring(0, age.GetBaseUrl().lastIndexOf('(') - 2) + "_Reports/PortalPage/Images/collapse.png";

        $(divContent).style.display = "none";

        if (divContent == "newmem") {
            $("newmemH").style.display = "none";
        }
    }

    $(divContent).style.position = "absolute";
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
    var e = $(elementId);
    var icon = e.childNodes[0].childNodes[0];
    var smallContent = e.childNodes[1].childNodes[0];
    var fullContent = e.childNodes[1].childNodes[1];
    if (fullContent.className == 'text_exposed_show') {
        if (HasClassName(e, 'expanding')) {
            RemoveClassName(e, 'expanding');
        };
        smallContent.className = 'text_exposed_show';
        fullContent.className = 'text_exposed_hide';

        icon.style.backgroundPosition = '0px 0px';
    }
    else {
        AddClassName(e, 'expanding');
        smallContent.className = 'text_exposed_hide';
        fullContent.className = "text_exposed_show";
        icon.style.backgroundPosition = '0px -9px';
        var isRead = e.getAttribute('isRead');
        if (isRead == '0') {
            SetReadPriMsg(elementId);
        }
    }
}

function SetReadPriMsg(messageId) {
    var params = ajax.CreateParams();
    function OnComplete(result) {
        var errCode = result.errCode;
        if (errCode == 0) {
            var e = $(messageId);
            e.style.fontWeight = 'normal';
            e.setAttribute('isRead', '1');
        }
    }
    params += 'messageId=' + messageId;
    ajax.PostJSON('SetReadPriMsg.ashx', params, OnComplete, true);
}
var moreIsLoading = false;

function continueExecution() {
    //finish doing things after the pause
    var lastPostTime = getCookie("lastPostTime");
    var url = location.href;
    url += '?lastPostTime=' + lastPostTime + '&' + _page.key;
    url = url.replace('PortalPage.aspx', 'Announcement.aspx');
    loadAnnouncement(url, false);
    $('divMore').innerHTML = _page.more;
    moreIsLoading = false;
}


function ShowMoreAnnouncements() {
    if (!moreIsLoading) {
        moreIsLoading = true;
        $('divMore').innerHTML = "<img src='../PortalPage/Images/loading_2.gif' />";
        //age.removeEvent($('divMore'), 'click', 'ShowMoreAnnouncements');
        setTimeout(continueExecution, 1000);
    }
}

//Load all infos

RegisterStartUp("displayBlocks('none');loadBalanceInfo();loadStatistic();verifyOnlineList();loadAnnouncement('Announcement.aspx?' + _page.key, true);");