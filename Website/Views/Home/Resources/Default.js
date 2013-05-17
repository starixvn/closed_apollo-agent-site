var MINI_MENU = function ($) {
    return {
        collapseMenu: function () {
            $('#btn-mini-menu').addClass('expand').removeClass('collapse');
            $('#btn-mini-menu').attr('title', LANGUAGES.Expand);
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
                         $('.mainpage').animate({ left: '31px' }, 100).addClass('max-mainpage');
                     }
                 }
            );
        },
        expandMenu: function () {
            $('#btn-mini-menu').addClass('collapse').removeClass('expand');
            $('#btn-mini-menu').attr('title', LANGUAGES.Collapse);
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

    // check collapse or expand 
    if ($.cookie('miniMenu') == 1) {
        MINI_MENU.collapseMenu();
    }

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
            $.cookie('miniMenu', 1);
        } else {
            MINI_MENU.expandMenu();
            $.cookie('miniMenu', 0);
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
    });

    // Click on "account" icon and change to treeview page
    $('.account').click(function (e) {
        $('.menu iframe').attr('src', APOLLO.resolveUrl('Account/TreeViewList'));
        MINI_MENU.expandMenu();
    });

    // Click on "search" icon and change to search page
    $('.search').click(function (e) {

        var custId = $.cookie('searching-id');
        var username = $.cookie('searching-name');
        var isMember = $.cookie('searching-ismember');

        var url = "Account/AccountInfo";
        if ((custId != undefined || username != undefined) && isMember != undefined) {
            if ($.cookie('searching-id') != "-1" || username.length > 0) {
                if (isMember.toBoolean()) {
                    url = 'Account/MemberInfo?custid=' + custId + '&username=' + username
                }
                else {
                    url = 'Account/AgentInfo?agentid=' + custId + '&username=' + username
                }
            }
        }

        $('.menu iframe').attr('src', APOLLO.resolveUrl(url));
        MINI_MENU.expandMenu();
    });

    // Bind the mouseleave for mini menu.
    $(".tab-menu-icon li.main-menu > ul").bind("mouseleave", function (event) {
        setTimeout(function () {
            $('.tab-menu-icon li.main-menu > ul').fadeOut(50);
            $('.main-menu').removeClass('active');
        }, 1000);
    });
});