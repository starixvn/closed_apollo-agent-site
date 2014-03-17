/*global LANGUAGES*/
/*global DEFAULTS*/
/*global APOLLO*/
'use strict';
// Start up

var HEADER = (function ($) {
    return {
        IsLoadTreeView: false,

        init: function () {
            if (DEFAULTS.IsInternalLogin && !DEFAULTS.IsAllowMultipleSignIn) {
                setTimeout(function () { HEADER.reCheckIn(); }, 1000);
            }

            HEADER.signOut();

            // show sub-menu
            $('.menu-list').click(function () {
                var $this = $(this);
                if ($this.next('.sub-menu').is(':visible')) {
                    $('.sub-menu').hide();
                } else {
                    $('.sub-menu').hide();
                    $this.next('.sub-menu').show();
                }
            });

            // show or hide left navigation
            $('.toolbar').on('click', 'li', function (event) {
                if ($(this).attr('id') === 'account-link') {
                    var url = '';
                    if (!DEFAULTS.IsMaster) {
                        url = "/CustomerSetting/MySetting";
                    }
                    else {
                        url = "/Account/AgentList";
                    }

                    HEADER.oldUrl = url;
                    parent.document.getElementById('main').setAttribute('src', url);
                    HEADER.showAccountTab();
                }
                else {
                    HEADER.hideAccountTab();
                    parent.$('#btn-mini-menu').hide();
                }
                event.stopPropagation();
            });

            // show contact us page
            $('#contact-us').click(function () {
                var contactWindow = window.open(APOLLO.resolveUrl('~/Account/ContactUs'), '_blank', 'scrollbars=1,width=800,height=428');
                contactWindow.focus();
            });

            HEADER.activeMenu();
        },

        signOut: function () {
            $("#signout").on("click", function () {
                APOLLO.signOut();
            });
        },

        reCheckIn: function () {
            $.ajax({
                type: "POST",
                url: APOLLO.resolveUrl("~/Internal/ReCheckIn"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                timeout: 120000
            })
                .done(function (result) {
                    if (DEFAULTS.IsInternalLogin && !DEFAULTS.IsAllowMultipleSignIn) {
                        if (result.isKickout) {
                            alert(LANGUAGES.AlerSignonMessage);
                            APOLLO.signOut();
                        }
                        else {
                            setTimeout(function () { HEADER.reCheckIn(); }, 30000);
                        }
                    }
                })
                .fail(function (event, jqXHR, ajaxSettings) {
                    APOLLO.handleAjaxError(event, jqXHR, ajaxSettings);
                });
        },

        showAccountTab: function () {
            var leftMenu = parent.$('.menu');
            var mainPage = parent.$('.mainpage');
            // hide left navigation
            if (leftMenu.is(':visible')) {
                HEADER.hideLeftNavigation(leftMenu, mainPage);
                parent.$('#btn-mini-menu').hide();
            } else { //show left navigation
                HEADER.showLeftNavigation(leftMenu, mainPage);

                var leftNav = parent.$('.menu'),
                    menuIcon = parent.$('.tab-menu-icon'),
                    btnMiniMenu = parent.$('#btn-mini-menu'),
                    iframeLeft = parent.$('#menu');

                if (leftNav.hasClass('mini-menu')) {
                    leftNav.removeClass('mini-menu');
                    menuIcon.hide();
                    if (btnMiniMenu.hasClass('collapse')) {
                        btnMiniMenu.removeClass('collapse').addClass('expand').show();
                    } else {
                        btnMiniMenu.removeClass('expand').addClass('collapse').show();
                    }
                    iframeLeft.show();
                } else {
                    if (btnMiniMenu.hasClass('expand')) {
                        btnMiniMenu.removeClass('expand');
                    }
                    btnMiniMenu.addClass('collapse').show();
                }
            }
        },

        hideAccountTab: function () {
            HEADER.hideLeftNavigation(parent.$('.menu'), parent.$('.mainpage'));
        },

        hideLeftNavigation: function (leftMenu, mainPage) {
            leftMenu.hide().animate(
                { width: "0" },
                {
                    duration: 10,
                    specialEasing: {
                        width: 'linear',
                        height: 'easeOutBounce'
                    },
                    complete: function () {
                        mainPage.animate({ left: '0' }, 50);
                    }
                }
            );
        },

        showLeftNavigation: function (leftMenu, mainPage) {
            leftMenu.show().animate(
                { width: "220px" },
                {
                    duration: 10,
                    specialEasing: {
                        width: 'linear',
                        height: 'easeOutBounce'
                    },
                    complete: function () {
                        mainPage.animate({ left: '221px' }, 50);
                    }
                }
            );

            if (!HEADER.IsLoadTreeView) {
                parent.document.getElementById('menu').contentWindow.location = APOLLO.resolveUrl("Account/TreeViewList");
                HEADER.IsLoadTreeView = true;
            }
        },

        oldUrl: null,

        activeMenu: function () {
            var toolbarMenu = $('.toolbar ul li a');
            toolbarMenu.on('click', function () {
                var $this = $(this);
                var $iframe = parent.$('.mainpage iframe');

                var currentSubUrl = $this.attr('url');

                // Ignore link to check for click on menu many times
                if (HEADER.oldUrl === currentSubUrl &&
                currentSubUrl !== "/Reports/matchentered" &&
                currentSubUrl !== "/Account/AgentList") {
                    return;
                }

                HEADER.oldUrl = currentSubUrl;

                $iframe.attr('src', currentSubUrl);
            });
        }
    };
} (jQuery));

$(document).ready(function () {
    setTimeout(function () { HEADER.init(); }, 100);
});
