/// <reference path="../../../../../_GlobalResources/Js/jquery.js" />

var Wagers = function ($) {
    return {

        sportName: null,

        groupBetName: null,

        viewOption: null,

        league: null,

        fromDate: null,

        groupComm: null,

        init: function () {
            Wagers.sportName = $('#SportName').val();
            Wagers.groupBetName = $('#GroupBetName').val();
        },

        initGetData: function () {
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

            $.ajax({
                url: APOLLO.resolveUrl(url),
                data: { fromDate: Wagers.fromDate, uGroup: Wagers.groupComm, isDirectMember: Wagers.viewOption, leagueId: Wagers.League },
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