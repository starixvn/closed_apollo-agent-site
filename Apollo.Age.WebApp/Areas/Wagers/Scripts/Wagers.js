/// <reference path="../../../../../_GlobalResources/Js/jquery.js" />

var Wagers = function ($) {
    return {

        sportName: null,
        groupBetName: null,
        viewOption: null,
        league: null,
        fromDate: null,
        groupComm: null,
        sportTypeIds: null,

        init: function () {
            Wagers.sportName = $('#SportName').val();
            Wagers.groupBetName = $('#GroupBetName').val();
        },

        initGetData: function () {

            if (Wagers.sportName != "soccer") {
                Wagers.sportName = "basketball";
            }

            var url = "~/wagers/" + Wagers.sportName + "/" + Wagers.groupBetName;

            Wagers.getWagerData(url);

            $('.za_select').on('change', function () {
                Wagers.getWagerData(url);
            });
        },

        getWagerData: function (url) {

            Wagers.viewOption = $('#ViewOption').val();
            Wagers.League = $('#League').val();
            Wagers.fromDate = $('#FromDate').val();
            Wagers.groupComm = $('#groupComm').val();
            Wagers.sportTypeIds = $('#SportTypeIds').val();

            $.ajax({
                url: APOLLO.resolveUrl(url),
                data: { fromDate: Wagers.fromDate, uGroup: Wagers.groupComm, isDirectMember: Wagers.viewOption, leagueId: Wagers.League, sportTypes: Wagers.sportTypeIds },
                success: function (data) {
                    $('#container').html(data);
                }
            });
        }
    }
} (jQuery);

$(function () {
    Wagers.init();
    Wagers.initGetData();
});