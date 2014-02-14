/*global APOLLO*/
/*global LANGUAGES*/

var MiniMenu = function ($) {
    return {
        btnToggle: null,
        menu: null,
        menuTab: null,
        mainPage: null,
        tabMainMenu: null,

        init: function () {
            MiniMenu.btnToggle = $('#btn-mini-menu');
            MiniMenu.menu = $('.menu');
            MiniMenu.menuTab = $('.tab-menu-icon');
            MiniMenu.mainPage = $('.mainpage');
            MiniMenu.tabMainMenu = MiniMenu.menuTab.find('.main-menu');
            MiniMenu.tabAccount = MiniMenu.menuTab.find('.account');
            MiniMenu.tabSearch = MiniMenu.menuTab.find('.search');

            //if ($.cookie('miniMenu') == 1) MiniMenu.collapse(); // check collapse or expand

            MiniMenu.btnToggle.click(function () {
                var $this = $(this);

                if ($this.hasClass('collapse')) {
                    MiniMenu.collapse();
                    //$.cookie('miniMenu', 1);
                } else {
                    MiniMenu.expand();
                    //$.cookie('miniMenu', 0);
                }
            });

            MiniMenu.tabMainMenu.click(function () {
                if (MiniMenu.tabMainMenu.children('ul').is(':hidden')) {
                    MiniMenu.tabMainMenu.find('.parent').fadeIn(300);
                    MiniMenu.mainPage.addClass('active');
                }
            }).on('click', 'a[target=main]', function () {
                MiniMenu.tabMainMenu.find('.parent').fadeOut(300);
                MiniMenu.mainPage.removeClass('active');
            });

            MiniMenu.tabMainMenu.children('ul').mouseleave(function () {
                var $this = $(this);

                setTimeout(function () {
                    $this.fadeOut(50, function () { MiniMenu.mainPage.removeClass('active'); });
                }, 300);
            });

            MiniMenu.tabAccount.click(function () {
                MiniMenu.menu.find('iframe').attr('src', APOLLO.resolveUrl('Account/TreeViewList'));
                MiniMenu.expand();
            });

            MiniMenu.tabSearch.click(function () {
                var custId = $.cookie('searching-id');
                var username = $.cookie('searching-name');
                var isMember = $.cookie('searching-ismember');
                var url = "Account/AccountInfo";

                if ((custId != undefined || username != undefined) && isMember != undefined) {
                    if ($.cookie('searching-id') != "-1" || username.length > 0) {
                        if (isMember.toBoolean()) {
                            url = 'Account/MemberInfo?custid=' + custId + '&username=' + username;
                        }
                        else {
                            url = 'Account/AgentInfo?agentid=' + custId + '&username=' + username;
                        }
                    }
                }

                MiniMenu.menu.find('iframe').attr('src', APOLLO.resolveUrl(url));
                MiniMenu.expand();
            });
        },

        collapse: function () {
            MiniMenu.btnToggle
                .addClass('expand')
                .removeClass('collapse')
                .attr('title', LANGUAGES.Expand);

            // changes size of menu
            MiniMenu.menu.animate(
                 { width: '22px' },
                 {
                     duration: 50,
                     specialEasing: {
                         width: 'linear',
                         height: 'easeOutBounce'
                     },
                     complete: function () {
                         var $this = $(this);

                         $this.addClass('mini-menu').find('iframe').hide();
                         MiniMenu.menuTab.show();
                         MiniMenu.mainPage.animate({ left: '22px' }, 100).addClass('max-mainpage');
                     }
                 }
            );
        },

        expand: function () {
            MiniMenu.btnToggle
                .addClass('collapse')
                .removeClass('expand')
                .attr('title', LANGUAGES.Collapse);

            // changes size of menu
            MiniMenu.menu.animate(
                 { width: "220px" },
                 {
                     duration: 10,
                     specialEasing: {
                         width: 'linear',
                         height: 'easeOutBounce'
                     },
                     complete: function () {
                         var $this = $(this);

                         $this.removeClass('mini-menu').find('iframe').show();
                         MiniMenu.menuTab.hide();
                         MiniMenu.mainPage.animate({ left: '221px' }, 50).removeClass('max-mainpage');

                         if (MiniMenu.tabMainMenu.hasClass('active')) {
                             MiniMenu.tabMainMenu.removeClass('active');
                             MiniMenu.tabMainMenu.children('ul').hide();
                         }
                     }
                 }
            );

        }
    };
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

    MiniMenu.init();

    $('#main').attr('src', APOLLO.resolveUrl('message/newmessage'));

    if ($('.mainpage').length > 0) {        
       $('#btn-mini-menu').hide();
    }
});