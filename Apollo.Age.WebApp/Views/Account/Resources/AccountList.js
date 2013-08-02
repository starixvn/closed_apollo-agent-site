/// <reference path="../../../_GlobalResources/Js/jquery.js" />
var ACCOUNTLIST = function ($) {
    return {
        agentListSettingTime: null,

        init: function () {
            var txtUsername = $('#txtUserName');
            txtUsername.unbind('click').bind('click', function () {
                if (APOLLO.htmlDecode(txtUsername.val()) == APOLLO.htmlDecode(BASELANGUAGES.EnterUsername)) {
                    txtUsername.val('');
                }
            }),
            txtUsername.blur(function () {
                if (txtUsername.val() == '') {
                    txtUsername.val(APOLLO.htmlDecode(BASELANGUAGES.EnterUsername));
                }
            }),
            $("#fundout").on('click', function () {
                APOLLO.openDialogFrame(APOLLO.resolveUrl("~/Account/FundOut?memberId=" + $(this).attr('data-memberid') + "&sportCredit=" + $(this).attr('data-sportcredit')), BASELANGUAGES.fundout + " - " + $(this).attr('data-membername'), {
                    width: 300,
                    height: 80,
                    position: { my: "top+50", at: "center top", of: window },
                    close: function (event, ui) {
                        //                        countDownTimer = setTimeout(CountDownRefresh_Tick, 1000);

                    }
                });
            });
        },
        initStatus: function (status, uplineStatus) {
            if (status == 1) {
                if (uplineStatus == 1) {
                    return "<span class='active'>" + LANGUAGES.Active + "</span>";
                }
                else {
                    return "<span class='lock'>" + LANGUAGES.CLock + "</span>";
                }
            }
            else if (status == 2) // Stop
            {
                return "<span class='stop'>" + LANGUAGES.Stopped + "</span>";
            }

            else {
                return "<span class='lock'>" + LANGUAGES.Locked + "</span>";
            }
        },
        showMenuList: function () {
            $('.edit-link').click(function (event) {
                clearTimeout(ACCOUNTLIST.agentListSettingTime);

                var $this = $(this),
                    $listMenu = $this.siblings('.list');

                if ($listMenu.is(":visible")) {
                    $listMenu.fadeOut('fast');
                    $this.removeClass('bg');
                } else {
                    ACCOUNTLIST.hideMenuLayout();
                    $this.addClass('bg');
                    $listMenu.fadeIn('fast');
                    //ACCOUNTLIST.agentListSettingTime = setTimeout('ACCOUNTLIST.hideMenuLayout()', 10000);
                }
                event.stopPropagation();
            });

            $('.edit-link').on('mouseover', function (event) {
                clearTimeout(ACCOUNTLIST.agentListSettingTime);
            });

            $('.edit-link').on('mouseout', function (event) {
                ACCOUNTLIST.agentListSettingTime = setTimeout('ACCOUNTLIST.hideMenuLayout()', 10000);
            });

            $('.menu-list').on('mouseover', function (event) {
                clearTimeout(ACCOUNTLIST.agentListSettingTime);
            });

            $('.menu-list').on('mouseout', function (event) {
                ACCOUNTLIST.agentListSettingTime = setTimeout('ACCOUNTLIST.hideMenuLayout()', 10000);
            });

            $('html, body').click(function () {
                clearTimeout(ACCOUNTLIST.agentListSettingTime);
                ACCOUNTLIST.hideMenuLayout();
            });
        },
        hideMenuLayout: function () {
            var editLink = $('.edit-link'),
                action = $('.action');

            editLink.removeClass('bg');
            action.children('.list').fadeOut('fast');
        }
    };
} (jQuery);

$(document).ready(function () {
    ACCOUNTLIST.init();
    ACCOUNTLIST.showMenuList();
});