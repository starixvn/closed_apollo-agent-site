/// <reference path="../../../_GlobalResources/Js/jquery.js" />
var ACCOUNTLIST = function ($) {
    return {
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
            })
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
        }
    };
} (jQuery);

$(document).ready(function () {
    ACCOUNTLIST.init();

    $('.edit-link').click(function (event) {
        $this = $(this);
        $listMenu = $this.siblings('.menu-list');
        if ($listMenu.is(":visible")) {
            $listMenu.hide();
            $this.removeClass('bg');
        } else {
            $('.edit-link').removeClass('bg');
            $('.action').children('.menu-list').hide();
            $this.addClass('bg');
            $listMenu.show();
        }
        event.stopPropagation();
    });

    $('body').click(function () {
        $('.action').children('.menu-list').hide();
    });
});