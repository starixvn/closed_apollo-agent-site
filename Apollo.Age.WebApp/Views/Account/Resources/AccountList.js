/// <reference path="../../../_GlobalResources/Js/jquery.js" />
/*global BASELANGUAGES*/

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

            $('#editShareMultiple,#viewDetailMultiple,#divEditMultiple .edit-multiple-member').click(function () {
                ACCOUNTLIST.editMultiple(this);
            });

            $('#cbAllMember').click(function () {
                var $this = $(this);
                if ($this.is(':checked')) {
                    $('#tblMemberList .cb-edit-multiple').prop('checked', true);
                }
                else {
                    $('#tblMemberList .cb-edit-multiple').prop('checked', false);
                }
            });

            $('#tblMemberList .cb-edit-multiple').click(function () {
                var $this = $(this);
                if (!$this.is(':checked')) {
                    $('#cbAllMember').prop('checked', false);
                }

                if ($('#tblMemberList .cb-edit-multiple').length == $('#tblMemberList .cb-edit-multiple:checked').length) {
                    $('#cbAllMember').prop('checked', true);
                }
            });
        },
        editMultiple: function (domObject) {
            var $this = $(domObject);
            var link = $this.data('edit-multiple-link'),
                    memberIds = [],
                    memberId = '',
                    usernames = [];

            $('#tblMemberList .cb-edit-multiple:checked').each(function () {
                var memberCheckbox = $(this);
                if (memberId == '') {
                    memberId = memberCheckbox.val();
                }
                memberIds.push(memberCheckbox.val());
                usernames.push(memberCheckbox.data('member-username'));
            });

            //// No Choosing, then alert message
            if (memberIds.length == 0) {
                alert(BASELANGUAGES.lblSelectAtLeast);
                return;
            } else if (memberIds.length == 1) { //// If edit just only one member, go to edit single page
                memberIds = [];
            }

            //window.location.href = link + '&memberIds=' + memberIds;
            window.location.href = link
            + '&memberIds=' + memberIds.join(',')
            + '&memberId=' + memberId
            + '&username=' + usernames.join(',%20');
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