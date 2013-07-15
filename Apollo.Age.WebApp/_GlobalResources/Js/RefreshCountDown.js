var RefreshCountDown = function ($) {
    return {
        interval: 0,
        timeoutCountDown: null,
        onRefresh: null,
        refreshNow: null,
        countDownRefresh: null,
        countDownMenu: null,
        inActive: null,

        stopTimeout: function () {
            if (RefreshCountDown.timeoutCountDown != null) {
                clearTimeout(RefreshCountDown.timeoutCountDown);
            }
        },

        startTimeout: function () {
            RefreshCountDown.refreshCountDownTick(RefreshCountDown.interval);
        },

        setCurrentTimeCookie: function () {
            var refreshId = $('#refreshCountDownId').attr('refresh-id');
            if (RefreshCountDown.interval > 0) {
                $.cookie('ApoRefreshTimeout-' + refreshId, RefreshCountDown.interval);
            } else {
                RefreshCountDown.stopTimeout();
                $.removeCookie('ApoRefreshTimeout-' + refreshId);
            }
        },

        showMenuOption: function () {
            $("#btndown").on('click', function () {
                var outerHeight = $(this).outerHeight() - 1;
                if (RefreshCountDown.countDownMenu.css('display') == 'none') {
                    RefreshCountDown.countDownMenu.css('top', outerHeight + 'px');
                    RefreshCountDown.countDownMenu.show();
                }
                else {
                    RefreshCountDown.countDownMenu.hide();
                }
                RefreshCountDown.bindEventSubMenuOption();
            });
        },

        refreshCountDownTick: function (timeTick) {
            var intervalTime = timeTick;
            intervalTime--;
            if (intervalTime <= 0) {
                intervalTime = RefreshCountDown.interval;
                eval(RefreshCountDown.onRefresh);
            }

            RefreshCountDown.countDownRefresh.html(intervalTime);
            RefreshCountDown.timeoutCountDown = setTimeout(function () {
                RefreshCountDown.refreshCountDownTick(intervalTime);
            }, 1000);
        },

        bindEventSubMenuOption: function () {
            var countdown = $('#countdownmenu li');
            countdown.on('click', function () {
                var $this = $(this);
                var itemCheckedClass = 'menuitemcheck';
                RefreshCountDown.interval = $this.attr('data-intervaltime');

                //Set value for menu time
                RefreshCountDown.countDownRefresh.html(RefreshCountDown.interval);

                //Show/Hide submenu option
                countdown.each(function () {
                    if ($(this).hasClass(itemCheckedClass)) {
                        $(this).removeClass(itemCheckedClass);
                        return false;
                    }
                });
                $this.addClass(itemCheckedClass);

                if (RefreshCountDown.countDownMenu.css('display') != 'none') {
                    RefreshCountDown.countDownMenu.hide();
                }
                //--------------

                //Save Cookie current time
                RefreshCountDown.setCurrentTimeCookie();


                //Start refresh count down tick
                if (RefreshCountDown.timeoutCountDown != null) {
                    clearTimeout(RefreshCountDown.timeoutCountDown);
                }

                if (RefreshCountDown.interval <= 0) {
                    RefreshCountDown.stopTimeout();
                    RefreshCountDown.countDownRefresh.html(RefreshCountDown.inActive);
                }
                else {
                    RefreshCountDown.refreshCountDownTick(RefreshCountDown.interval);
                }
            });
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
        },

        bindRefreshNow: function () {
            RefreshCountDown.refreshNow.on('click', function () {
                $this = $(this);
                var actived = $this.attr('actived');
                if (actived == 'true') {
                    RefreshCountDown.disabledRefreshCountDown(true);

                    eval(RefreshCountDown.onRefresh);

                    setTimeout(function () {
                        RefreshCountDown.disabledRefreshCountDown(false);
                    }, 15 * 1000);
                }
            });
        }

    };
} (jQuery);

$(document).ready(function () {
    RefreshCountDown.onRefresh = $('#apo-on-refresh').val();
    RefreshCountDown.refreshNow = $('#btnrefresh');
    RefreshCountDown.countDownRefresh = $('#countdownrefresh');
    RefreshCountDown.countDownMenu = $('#countdownmenu');
    RefreshCountDown.inActive = $('#inactive').val();

    // set refresh timeout by using cookie
    var refreshId = $('#refreshCountDownId').attr('refresh-id');
    var cookieValue = $.cookie('ApoRefreshTimeout-' + refreshId);
    if (typeof cookieValue !== 'undefined') {
        RefreshCountDown.interval = cookieValue;
        RefreshCountDown.refreshCountDownTick(RefreshCountDown.interval);
        RefreshCountDown.countDownRefresh.html(RefreshCountDown.interval);
    }
    else {
        RefreshCountDown.countDownRefresh.html(RefreshCountDown.inActive);
    }

    RefreshCountDown.disabledRefreshCountDown(false);
    RefreshCountDown.showMenuOption();
    RefreshCountDown.bindRefreshNow();

});