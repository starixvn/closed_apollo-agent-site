/*
* Created 20100310@Lee - Javascript functions for credit balance information page
* Revision ?@? - ... 
* 
*/


function SetActiveTab()
{
    if(top.frHeader && top.frHeader.SelectTopMenu)
    {
        top.frHeader.SelectTopMenu('PortalPage.aspx');
    }
}

//Start Load Ad
function SetAdCookie(adName, isShow, userId, subId) {
    var cookieName = 'cookie20110712_' + adName + userId + '' + subId;
    setCookie(cookieName, isShow ? 1 : 0, 17);
}

function LoadAd() {
    var cookieName = 'cookie20110712_' + 'LoadAd' + _page.userid + '' + _page.subid;
    var isLoaded = getCookie(cookieName);
    if ((isLoaded == null || isLoaded == 0) && $('divAd') != null) {
        $('divAd').style.display = 'block';
    }
    else {
        NextPopup();
    }
}
// ...
//End Load Ad

//Start Load Introduction
function LoadIntroduction() {
    var cookieName = 'cookie20110712_' + 'LoadIntroduction' + _page.userid + '' + _page.subid;
    var isLoaded = getCookie(cookieName);
    if ((isLoaded == null || isLoaded == 0) && $('divIntroduction') != null) {
        var CountCookie = getCookie(cookieName + 'Count');
        if (CountCookie == null || CountCookie == 0) {
            CountCookie = 1;
        }
        if (parseInt(CountCookie) < 6) {
            setCookie(cookieName + 'Count', 1 + parseInt(CountCookie), 17);
            $('divIntroduction').style.display = 'block';
            setTimeout("$('divIntroduction').style.display = ''", 30000);
        }
    }
    else {
        NextPopup();
    }
}
// ...
//End Load Ad

var Popups = [LoadIntroduction];

function NextPopup() {    
    if (Popups.length == 0 || parent.loadedAd == true) {
        parent.loadedAd = true;
        return;
    }
    if (Popups.length == 0 || parent.LoadIntroduction == true) {
        parent.LoadIntroduction = true;
        return;
    }
    setTimeout(Popups.pop(), 100);
}

RegisterStartUp('SetActiveTab(); setTimeout(NextPopup, 500);');
