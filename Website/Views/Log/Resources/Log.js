/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var Log = function ($) {

    return {

        timerID: null,

        moreIsLoading: false,

        imagePath: null,

        txtUsername: null,

        ddlIsAgent: null,

        enterUsernameValue: null,

        init: function () {
            Log.txtUsername = $('#Username');
            Log.ddlIsAgent = $('#IsAgent');
            Log.enterUsernameValue = $('#enterUsernameValue');
        },

        initSearchClick: function () {

            Log.txtUsername.autocomplete({
                source: function (request, response) {
                    var url = Log.ddlIsAgent.val().toBoolean()
                        ? "Account/GetAgentByUsername?username=" + request.term.trim()
                        : "Account/GetMemberByUsername?username=" + request.term.trim();

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

                position: { my: "right top", at: "right bottom" }
            });

            Log.txtUsername.bind('click', function () {
                if (Log.txtUsername.val() == Log.enterUsernameValue.val()) {
                    Log.txtUsername.val('');
                }
            });
            Log.txtUsername.bind('keydown', function () {
                if (Log.txtUsername.val() == Log.enterUsernameValue.val()) {
                    Log.txtUsername.val('');
                }
            });
            Log.txtUsername.bind('blur', function () {
                if (Log.txtUsername.val().length == 0) {
                    Log.txtUsername.val(Log.enterUsernameValue.val());
                }
            });
        }
    }
} (jQuery);

$(document).ready(function () {
    Log.init();
    Log.initSearchClick();
});