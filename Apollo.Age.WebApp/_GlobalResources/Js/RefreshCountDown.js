/*var curCountDown = 0;
var ckiName = null;
var countDownTimer = null;
var isStopCoundown = true;

function CountDownRefresh_Tick() {
document.getElementById('CountDownRefresh').innerHTML = curCountDown;
curCountDown--;
if (countDownTimer != null) clearTimeout(countDownTimer);
countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);

if (curCountDown < 0) {
DoRefreshFunction();
}
}
function DoRefreshFunction() {
document.getElementById('CountDownRefresh').innerHTML = loadingTxt;
eval(countDownRefreshFunc);
curCountDown = initCountDown;
};

function CountDownRefresh() {
if (countDownTimer != null) clearTimeout(countDownTimer);
document.getElementById('CountDownRefresh').innerHTML = loadingTxt;

ajax.Request(window.location.href,
{
asynchronous: true,
parameters: 'full_mode=0',
onComplete: function (data) {
document.getElementById('sContent').innerHTML = data.responseText;
RefreshCountDown_Init();
}
});

return true;
}

function CountDownRefresh_SaveOption(idNewOption) {
document.getElementById('CountDownMenu').style.display = 'none';
var txtNewOption = document.getElementById(idNewOption).innerHTML;
initCountDown = parseInt(txtNewOption);
CountDownRefresh_DoCheckMenu();
setCookie(ckiName, initCountDown, 35);
CountDownRefresh_SetCountDown(initCountDown); //reset
}

function CountDownRefresh_DoCheckMenu() {
var menus = document.getElementById('CountDownMenu').getElementsByTagName("div");
for (i = 0; i < menus.length; i++) {
if (parseInt(menus[i].innerHTML) == initCountDown) {
AddClassName(menus[i].id, "menuitemcheck");
}
else
RemoveClassName(menus[i].id, "menuitemcheck");
}
}

function CountDownRefresh_Refresh() {
var pnlDown = document.getElementById('CountDownMenu');
if (pnlDown.style.display == '')
pnlDown.style.display = 'none';
DoRefreshFunction();
}

function CountDownRefresh_DoOpenOption(btnDown) {
var pnlDown = document.getElementById('CountDownMenu');
var arrBtnPos = FindPosition(btnDown.parentNode);
if (pnlDown.style.display == 'none') {
pnlDown.style.left = arrBtnPos[0] + 'px';
pnlDown.style.top = (arrBtnPos[1] + 20) + 'px';
pnlDown.style.display = '';
}
else pnlDown.style.display = 'none';
}

function CountDownRefresh_SetCountDown(newCountDown) {
initCountDown = newCountDown;
curCountDown = initCountDown;
}

function FindPosition(oElement) {
if (typeof (oElement.offsetParent) != 'undefined') {
for (var posX = 0, posY = 0; oElement; oElement = oElement.offsetParent) {
posX += oElement.offsetLeft;
posY += oElement.offsetTop;
}
return [posX, posY];
} else {
return [oElement.x, oElement.y];
}
}


// Start up
function RefreshCountDown_Init() {
if (typeof initCountDown !== 'undefined') {
curCountDown = initCountDown;
ckiName = "ibcCountDown" + refreshCountDownId;

//read countdown from cookie
var countDownFromCookie = getCookie(ckiName);
if (countDownFromCookie && countDownFromCookie != 0) {
CountDownRefresh_SetCountDown(countDownFromCookie);
}
CountDownRefresh_DoCheckMenu();
//do count down
CountDownRefresh_Tick();
}
}

// utility functions
function RefreshCountDown_AddEvent(el, evname, func) {
if (el.attachEvent) { // IE
el.attachEvent("on" + evname, func);
} else if (el.addEventListener) { // Gecko / W3C
el.addEventListener(evname, func, true);
} else { // Opera (or old browsers)
el["on" + evname] = func;
}
}

RefreshCountDown_AddEvent(window, "load", RefreshCountDown_Init);*/

var RefreshCountDown = function ($) {
    return {
        timeout: null,
        ddlRefresh: null,
        onRefresh: null,
        refreshNow: null,
        refreshNowTimeout: null,

        setRefreshTimeout: function (timeout) {
            if (RefreshCountDown.onRefresh) {
                clearTimeout(RefreshCountDown.timeout);
                RefreshCountDown.timeout = setTimeout(function () {
                    eval(RefreshCountDown.onRefresh);
                    RefreshCountDown.setCurrentTimeout();
                }, timeout * 1000);
            }
        },

        clearTimeout: function () {
            if (RefreshCountDown.timeout != null) clearTimeout(RefreshCountDown.timeout);
        },

        setCurrentTimeout: function () {
            var refreshId = $('#refreshCountDownId').attr('refresh-id');
            var timeout = parseInt(RefreshCountDown.ddlRefresh.val());
            if (timeout > 0) {
                RefreshCountDown.setRefreshTimeout(timeout);
                $.cookie('ApoRefreshTimeout-' + refreshId, timeout);
            } else {
                RefreshCountDown.clearTimeout();
                $.removeCookie('ApoRefreshTimeout-' + refreshId);
            }
        },

        disabledRefreshCountDown: function (isDisabled) {
            if (isDisabled) {
                RefreshCountDown.refreshNow.attr('class', 'refresh disabled');
                RefreshCountDown.refreshNow.attr('actived', false);
            }
            else {
                RefreshCountDown.refreshNow.attr('class', 'refresh');
                RefreshCountDown.refreshNow.attr('actived', true);
            }
        }
    };
} (jQuery);

$(document).ready(function () {
    RefreshCountDown.ddlRefresh = $('.btnCountDown select');
    RefreshCountDown.onRefresh = $('#apo-on-refresh').val();
    RefreshCountDown.refreshNow = $('#btnRefresh');

    // set refresh timeout by using cookie
    var refreshId = $('#refreshCountDownId').attr('refresh-id');
    var timeout = $.cookie('ApoRefreshTimeout-' + refreshId);
    if (typeof timeout !== 'undefined') {
        RefreshCountDown.setRefreshTimeout(timeout);
        RefreshCountDown.ddlRefresh.val(timeout);

        RefreshCountDown.disabledRefreshCountDown(false);

    }
    else {
        RefreshCountDown.disabledRefreshCountDown(true);
    }

    RefreshCountDown.ddlRefresh.on('change', function () {
        RefreshCountDown.setCurrentTimeout();
        var $this = $(this);
        if ($this.val() == 0) {
            clearTimeout(refreshNowTimeout);
        }
        RefreshCountDown.disabledRefreshCountDown($this.val() == 0);

    });

    RefreshCountDown.refreshNow.on('click', function () {
        $this = $(this);
        var actived = $this.attr('actived');
        if (actived == 'true') {
            RefreshCountDown.disabledRefreshCountDown(true);

            eval(RefreshCountDown.onRefresh);

            refreshNowTimeout = setTimeout(function () {
                RefreshCountDown.disabledRefreshCountDown(false);
            }, 15 * 1000);
        }
    });
});