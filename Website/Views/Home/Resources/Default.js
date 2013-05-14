var MINI_MENU = function ($) {
    return {
        collapseMenu: function () {
            $('#btn-mini-menu').addClass('expand').removeClass('collapse');
            $('#btn-mini-menu').attr('title', 'Resource.Expand');
            // change size of menu navigation
            $('.menu').addClass('mini-menu');
            $('.mainpage').addClass('max-mainpage');
            $('.menu iframe').hide();
            $('ul.tab-menu-icon').show();
        },
        expandMenu: function () {
            $('#btn-mini-menu').addClass('collapse').removeClass('expand');
            $('#btn-mini-menu').attr('title', 'Resource.Minimize');
            // change size of menu navigation
            $('.menu').removeClass('mini-menu');
            $('.mainpage').removeClass('max-mainpage');
            $('.menu iframe').show();
            $('ul.tab-menu-icon').hide();
        }
    }
} (jQuery);

$(document).ready(function () {
    window.IsApoRoot = true;

    // Check IE7
    if (APOLLO.browser.msie && parseInt(APOLLO.browser.version, 10) == 7) {
        var windowHeight = $(window).height(),
        headerHeight = $('.header').height(),
        contentHeight = windowHeight - headerHeight + 1;
        $('.menu').height(contentHeight - 10);
        $('.mainpage').height(contentHeight - 20);
    }

    // Collapse or expand menu
    $('#btn-mini-menu').click(function () {
        if ($('#btn-mini-menu').hasClass('collapse')) {
            MINI_MENU.collapseMenu();
        } else {
            $('.menu iframe').attr('src', APOLLO.resolveUrl('Account/Menu'));
            MINI_MENU.expandMenu();
        }
    });

    // Click on "main menu" icon and change to "main menu" page   
    $('.main-menu').click(function (e) {
        if ($('ul.tab-menu-icon li:hover > ul').css('display') == 'none') {
            $('ul.tab-menu-icon li:hover > ul').show();
            $('.main-menu').addClass('active');
        } else {
            $('ul.tab-menu-icon li:hover > ul').hide();
            $('.main-menu').removeClass('active');
        }
        e.stopPropagation();
    });

    // Click on "account" icon and change to treeview page
    $('.account').click(function (e) {
        $('.menu iframe').attr('src', APOLLO.resolveUrl('Account/TreeViewList'));
        MINI_MENU.expandMenu();
        e.stopPropagation();
    });

    // Click on "search" icon and change to search page
    $('.search').click(function (e) {
        $('.menu iframe').attr('src', APOLLO.resolveUrl('Account/AccountInfo'));
        MINI_MENU.expandMenu();
        e.stopPropagation();
    });

    $('.menu').click(function () {
        $('.tab-menu-icon li.main-menu > ul').hide();
        $('.main-menu').removeClass('active');
    });

});