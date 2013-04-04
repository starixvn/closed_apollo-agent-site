/// <reference path="../../../_GlobalResources/Js/jquery.js" />
/// <reference path="../../../_GlobalResources/Js/jquery.cookie.js" />

var MENU = function ($) {
    return {
        menu: null,

        init: function () {
            MENU.menu = $('#menu');
        }
    };
} (jQuery);

$(document).ready(function () {
    MENU.init();

    MENU.menu.on('click', '.menu-item-title', function () {
        var $this = $(this),
            subMenu = $this.next('.submenu'),
            parent = $this.parent();

        if (parent.hasClass('active')) {
            parent.removeClass('active');
            subMenu.slideUp('fast');
            $.removeCookie('age-active-menuitem', { path: '/' });
        } else {
            parent.addClass('active').siblings('.menu-item').removeClass('active').children('.submenu').slideUp('fast');
            subMenu.slideDown('fast');
            $.cookie('age-active-menuitem', parent.index(), { path: '/' });
        }
    })

    // activate selected menu item
    var activeMenuItemIdx = $.cookie('age-active-menuitem');

    if (activeMenuItemIdx) {
        MENU.menu.children('.menu-item').each(function (idx) {
            if (idx == activeMenuItemIdx) $(this).addClass('active');
            else $(this).removeClass('active');
        });
    }

    MENU.menu.find('.active .submenu').show();
});