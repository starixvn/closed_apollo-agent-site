/// <reference path="../../../../../_GlobalResources/Js/jquery.js" />

var Soccer = function ($) {
    return {
        init: function () {
            Soccer.bindSubMenu();
        },
        bindSubMenu: function () {

            $("[name=wagers-submenu]").on('click', function () {
                var $this = $(this);
                //location.href = APOLLO.resolveUrl("~/wagers/Soccer") + "?bettype=" + $this.attr("data-bettype");
            });
        },              

    }
} (jQuery);

$(function () {
    Soccer.init();
}); /* File Created: June 28, 2013 */