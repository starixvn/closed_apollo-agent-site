/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var VIEWCOMM = function ($) {
    var userName = null;
    var isMember = null;

    return {
        init: function () {
            VIEWCOMM.userName = $("#Username");
            VIEWCOMM.isMember = $("#IsMember");

            VIEWCOMM.bindAutocomplete();

            $("#btnSubmit").bind('click', function () {
                var url = "CustomerSetting/ViewCommission?IsMember=" + VIEWCOMM.isMember.val()
                        + "&Id=-1&Username=" + VIEWCOMM.userName.val();

                location.href = APOLLO.resolveUrl(url);
            });
        },
        bindAutocomplete: function () {
            VIEWCOMM.userName.autocomplete({
                source: function (request, response) {
                    var url = "";
                    if (VIEWCOMM.isMember.val().toBoolean()) {
                        url = "Account/GetMemberByUsername?username=" + request.term.trim();
                    }
                    else {
                        url = "Account/GetAgentByUsername?username=" + request.term.trim();
                    }

                    $.ajax({
                        url: APOLLO.resolveUrl(url),
                        success: function (data) {
                            var found = $.map(data, function (item) {
                                return {
                                    label: item.Username,
                                    value: item.Username,
                                    id: item.Id
                                }
                            });

                            return response(found);
                        }
                    });
                },
                select: function (event, ui) {
                    var url = "CustomerSetting/ViewCommission?IsMember=" + VIEWCOMM.isMember.val()
                        + "&Id=" + ui.item.id + "&Username=" + ui.item.value;

                    location.href = APOLLO.resolveUrl(url);
                },
                position: { my: "right top", at: "right bottom" }
            });
        }
    }
} (jQuery);

$(function () {
    VIEWCOMM.init();
});