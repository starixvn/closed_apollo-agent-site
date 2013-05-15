var MINI_MENU = function ($) {
    return {
        collapseMenu: function () {
            $('#btn-mini-menu').addClass('expand').removeClass('collapse');
            $('#btn-mini-menu').attr('title', 'Resource.Expand');
            // change size of menu navigation
            $('.menu').animate(
                 { "width": "30px" },
                 {
                     duration: 50,
                     specialEasing: {
                         width: 'linear',
                         height: 'easeOutBounce'
                     },
                     complete: function () {
                         $(this).addClass('mini-menu');
                         $('.menu iframe').hide();
                         $('ul.tab-menu-icon').show();
                         $('.mainpage').animate({left: '31px'}, 100).addClass('max-mainpage');
                     }
                 }
            );
        },
        expandMenu: function () {
            $('#btn-mini-menu').addClass('collapse').removeClass('expand');
            $('#btn-mini-menu').attr('title', 'Resource.Minimize');
            // change size of menu navigation
            // $('.menu').removeClass('mini-menu').width(220);
            $('.menu').animate(
                 { "width": "220px" },
                 {
                     duration: 10,
                     specialEasing: {
                         width: 'linear',
                         height: 'easeOutBounce'
                     },
                     complete: function () {
                         $(this).removeClass('mini-menu');
                         $('.menu iframe').show();
                         $('ul.tab-menu-icon').hide();                        
                         $('.mainpage').animate({ left: '221px' }, 50).removeClass('max-mainpage');
                         if ($('li.main-menu').hasClass('active')) {
                             $('li.main-menu').removeClass('active');
                             $('ul.tab-menu-icon li.main-menu > ul').hide();
                         } 
                     }
                 }
            );

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
        if ($('ul.tab-menu-icon li > ul').css('display') == 'none') {
            $('ul.tab-menu-icon li.main-menu ul.parent').fadeIn(300);
            $('.main-menu').addClass('active');
        } else {
            $('ul.tab-menu-icon li.main-menu ul.parent').fadeOut(300);
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