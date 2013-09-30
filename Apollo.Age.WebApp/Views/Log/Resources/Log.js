/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var Log = function ($) {

    return {

        timerID: null,

        moreIsLoading: false,

        imagePath: null,

        txtUsername: null,

        ddlIsAgent: null,

        enterUsernameValue: null,

        monthDateTime: null,

        fromDate: null,

        toDate: null,

        toDayLable: null,

        fromDayLable: null,

        btnGoTop: null,

        init: function () {
            Log.txtUsername = $('#Username');
            Log.ddlIsAgent = $('#IsAgent');
            Log.enterUsernameValue = $('#enterUsernameValue');
            Log.monthDateTime = $('#monthDateTime');
            Log.fromDate = $('#From');
            Log.toDate = $('#To');
            Log.toDayLable = $('#ToDayLable');
            Log.fromDayLable = $('#FromDayLable');
            Log.btnGoTop = $("#btn-go-top");

            Log.monthDateTime.monPickr({
                shortMonthsInYear: LogOpts.MonthsInYear,
                longMonthsInYear: LogOpts.MonthsInYear,
                fromDayLabel: Log.fromDayLable.val(),
                toDayLabel: Log.toDayLable.val(),
                month: Log.fromDate.val(),
                fromDay: parseInt(Log.fromDate.val().split("/")[1], 10),
                toDay: parseInt(Log.toDate.val().split("/")[1], 10),
                afterChangeMonth: function () {
                    Log.fromDate.val(Log.monthDateTime.monPickr("getFromDate", "MM/dd/yyyy"));
                    Log.toDate.val(Log.monthDateTime.monPickr("getToDate", "MM/dd/yyyy"));
                },
                afterChangeDay: function () {
                    Log.fromDate.val(Log.monthDateTime.monPickr("getFromDate", "MM/dd/yyyy"));
                    Log.toDate.val(Log.monthDateTime.monPickr("getToDate", "MM/dd/yyyy"));
                }
            });

            $('#ShowOldValue').change(function () {
                $.blockUI();
                var valuechanged = $('.valuechanged');
                var line = valuechanged.next();
                var valueOld = line.next();
                var value = $(this).is(':checked');

                if (value) {
                    line.show();
                    valueOld.show();
                }
                else {
                    line.hide();
                    valueOld.hide();
                }
                $.unblockUI();
            });

            // go to the top
            Log.btnGoTop.click(function () {
                $("html, body").animate({ scrollTop: 0 }, 100);
                return false;
            });

            $(window).scroll(function () {
                if ($(this).scrollTop() > 100) {
                    Log.btnGoTop.fadeIn();
                } else {
                    Log.btnGoTop.fadeOut();
                }
            });
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
    $('.show-old-value').show();
    //format css
    $('.monpickr-wrap-parent, #monthDateTime').width(107);
    $('.monpickr-selected-month').width(60);
    $('.daypickr-selected-day').width(20);
    $('.monpickr-dropdown').width(105);
    $('#dSubmit').on('click', function () {
        $.blockUI();
    });
});