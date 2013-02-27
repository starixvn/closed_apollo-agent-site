/*
* Created 20110707@Simon - Javascript functions for popup announcement.
* Revision ?@? - ...
*
*/

var ShowedMessageIds = new Array();
var timeoutAnnouncement = 0, timeoutAnnouncementNext = 0;

function GetAnnoucementCookieName() {
    try {
        if (!$("popUpId")) return null;
        if (parseInt($("popUpId").value) > 0) return null;

        return $("popUpId").value + $("announcementCookiesId").value;
    }
    catch (e) {
        return null;
    }
}

function DoNotShowAnnoucement(hide) {
    setCookie(GetAnnoucementCookieName(), hide ? 5 : 1, 30); // count 5 times already
}

function GetAnnoucementShowCount() {
    var cookieName = GetAnnoucementCookieName();
    if (cookieName == null) return 0;

    var value = getCookie(cookieName);
    var count = parseInt(value, 10);
    if (isNaN(count)) {
        count = 0;
    }

    return count;
}

function IncreaseAnnoucementShowTimes() {
    var count = GetAnnoucementShowCount();
    setCookie(GetAnnoucementCookieName(), ++count, 30);
}

function LoadAnnouncement() {
    jx.request('_Announcement/Announcement.aspx', ProcessAnnouncement, 'post', 'id=' + ShowedMessageIds.join(','), true);

    timeoutAnnouncement = setTimeout(RemoveAnnouncement, 85000);
}

function LoadAnnouncementNext(timeout) {
    jx.request('_Announcement/Announcement.aspx', ProcessAnnouncement, 'post', 'id=' + ShowedMessageIds.join(',') + '&AnnounmentNext=1', true);

    if (timeout) {
        timeoutAnnouncementNext = setTimeout(RemoveAnnouncement, timeout);
    }
    else {
        timeoutAnnouncementNext = setTimeout(RemoveAnnouncement, 85000);
    }
}

function ProcessAnnouncement(result) {
    var text = result.responseText;
    if (text.length < 10) {
        return;
    }

    RemoveAnnouncement();

    var div = document.createElement('DIV');
    div.innerHTML = text;
    div.style.display = 'none';
    div.setAttribute('id', 'AnnouncementId');
    document.body.appendChild(div);

    var id = $('popUpId').value;

    if (!CheckShowedMessage(id)) {
        ShowedMessageIds.push(id);
    }

    IncreaseAnnoucementShowTimes();
    var count = GetAnnoucementShowCount(); // 5 times
    if (count >= 6) {
        setTimeout(RemoveAnnouncement, 1000); // Remove DIV (setTimeout If sources have iframe)
        return;
    }

    if (id > 0 && $("popup_panel")) {
        $("popup_panel").style.display = 'none';
    }

    if ($('popup_container_text') == null) {
        div.style.top = $('popup_container').style.top;
        div.style.left = $('popup_container').style.left;
    }
    else {
        div.style.top = $('popup_container_text').style.top;
        div.style.left = $('popup_container_text').style.left;
    }

    setTimeout(function () { $('AnnouncementId').style.display = 'block'; }, 2000);
}

function RemoveAnnouncement(timeout) {
    var AnnouncementId = $('AnnouncementId');
    if (AnnouncementId != null) {
        AnnouncementId.parentNode.removeChild(AnnouncementId);
        clearTimeout(timeoutAnnouncement);
        LoadAnnouncementNext(timeout);
    }
}

function CheckShowedMessage(id) {
    var count = ShowedMessageIds.length;
    for (var i = 0; i < count; i++) {
        if (id == ShowedMessageIds[i]) return true;
    }

    return false;
}

RegisterStartUp(LoadAnnouncement);