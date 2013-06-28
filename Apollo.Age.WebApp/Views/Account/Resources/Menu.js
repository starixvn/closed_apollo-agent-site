/// <reference path="../../../_GlobalResources/Js/jquery.js" />
/// <reference path="../../../_GlobalResources/Js/jquery.cookie.js" />

var MENU = function ($) {
	return {
		menu: null,
		tabmenu: null,
		apoRoot: null,
		isAgentListLoaded: false,

		init: function () {
			MENU.menu = $('#menu');
			MENU.tabmenu = $('#tab-menu');
			MENU.apoRoot = $('#ApoRoot').val();

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

				location.href = APOLLO.resolveUrl(url);
			});

			// add loading image
			$('.submenu a[target=main]').on('click', function () {
				parent.document.getElementById('main').contentWindow.APOLLO.showBlockLoading();
			});
		},

		loadAgentList: function () {
			if (!MENU.isAgentListLoaded) {
				var agentList = $("#agent-list");
				var agentId = agentList.attr("agent-id");

				agentList.treeview({
					url: MENU.apoRoot + "Account/GetDataByNode",
					link: "<a href='" + MENU.apoRoot + "Account/AgentList?agentid={0}' target='main'>{1}</a>",
					agentId: agentId
				});

				MENU.isAgentListLoaded = true;
			}
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