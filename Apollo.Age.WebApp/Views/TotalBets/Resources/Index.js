'use strict';
var TotalBetsIndex = (function () {
    return {
        menu: null,
        moreLink: null,
        moreMenu: null,
        moreMenuTimer: null,

        reloadChild: function () {
            document.getElementById('submain').contentWindow.location.href =
            document.getElementById('submain').contentWindow.location.href;
        }
    };
})();

$(function () {
    TotalBetsIndex.menu = $('.menu');
    TotalBetsIndex.moreLink = $('#more-link');
    TotalBetsIndex.moreMenu = $('#more-link .sub-menu');

    TotalBetsIndex.menu.on('click', 'li', function () {
        var $this = $(this);
        if ($this.attr('id') !== 'more-link') {
            $this.parents('.menu').find('li').removeClass('active');
            $this.addClass('active');
        }
    });

    TotalBetsIndex.moreLink
    .on('click', '>a', function () {
        $(this).siblings('.sub-menu').toggle();
    })
    .on('click', 'ul a', function () {
        $(this).parents('.sub-menu').toggle();
    })
    .on('mouseleave', function () {
        TotalBetsIndex.moreMenuTimer = setTimeout(function () { TotalBetsIndex.moreMenu.hide(); }, 500);
    })
    .on('mouseenter', '.sub-menu', function () {
        if (TotalBetsIndex.moreMenuTimer) {
            clearTimeout(TotalBetsIndex.moreMenuTimer);
        }
    });
});