/// <reference path="../../../_GlobalResources/Js/jquery.js" />
/// <reference path="../../../_GlobalResources/Js/jquery.cookie.js" />
/*global APOLLO*/

var MENU = (function ($) {
    'use strict';
    return {
        menu: null,
        tabmenu: null,
        apoRoot: null,
        downlineDetailsPolicy: null,

        init: function () {
            MENU.menu = $('#menu');
            MENU.tabmenu = $('#tab-menu');
            MENU.apoRoot = $('#ApoRoot').val();
            MENU.downlineDetailsPolicy = $('#DownlineDetailsPolicy').val();

            MENU.tabmenu.on('click', '#lk-menu-tab', function () {
                location.href = APOLLO.resolveUrl("Account/Menu");
            });

            MENU.tabmenu.on('click', '#lk-tree-tab', function () {
                location.href = APOLLO.resolveUrl("Account/TreeViewList");
            });

            MENU.tabmenu.on('click', '#lk-acc-tab', function () {
                var custId = $.cookie('searching-id');
                var username = $.cookie('searching-name');
                var isMember = $.cookie('searching-ismember');

                var url = "Account/AccountInfo";
                if ((custId !== undefined || username !== undefined) && isMember !== undefined) {
                    if ($.cookie('searching-id') !== "-1" || username.length > 0) {
                        if (isMember.toBoolean()) {
                            url = 'Account/MemberInfo?custid=' + custId + '&username=' + username;
                        }
                        else {
                            url = 'Account/AgentInfo?agentid=' + custId + '&username=' + username;
                        }
                    }
                }

                location.href = APOLLO.resolveUrl(url);
            });

            $('#lklRefresh').bind('click', function () {
                location.reload();
            });

            // add loading image
            $('.submenu a[target=main]').on('click', function () {
                parent.document.getElementById('main').contentWindow.APOLLO.showBlockLoading();
            });

            MENU.loadAgentList();
        },

        loadAgentList: function () {

            var html = MENU.downlineDetailsPolicy === "1" ? "a" : "span";

            var agentList = $("#agent-list");
            if (agentList.length > 0) {
                var agentId = agentList.attr("data-agentid");
                agentList.treeview({
                    url: MENU.apoRoot + "Account/GetDataByNode",
                    link: "<" + html + " href='" + MENU.apoRoot + "Account/IndirectAgentList?parentAgentId={3}&agentid={0}' target='main'>{1}</" + html + ">",
                    linkdirectAgents: "<a href='" + MENU.apoRoot + "Account/AgentList' target='main'>{1}</a>",
                    agentId: agentId,
                    showRoot: true,
                    rootLink: MENU.apoRoot + "Account/GetRootNode"
                });
            }
        }
    };
} (jQuery));

$(document).ready(function () {
    'use strict';
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
    });

    // activate selected menu item
    var activeMenuItemIdx = $.cookie('age-active-menuitem');

    if (activeMenuItemIdx) {
        MENU.menu.children('.menu-item').each(function (idx) {
            if (idx === activeMenuItemIdx) {
                $(this).addClass('active');
            }
            else {
                $(this).removeClass('active');
            }
        });
    }

    MENU.menu.find('.active .submenu').show();
});