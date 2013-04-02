/// <reference path="../../_GlobalResources/Js/jquery-1.9.1.min.js" />

var MENU = {};

if (typeof jQuery != 'undefined') {
    MENU = function ($) {
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
            } else {
                parent.addClass('active').siblings('.menu-item').removeClass('active').children('.submenu').slideUp('fast');
                subMenu.slideDown('fast');
            }
        }).find('.active .submenu').show();
    });
}