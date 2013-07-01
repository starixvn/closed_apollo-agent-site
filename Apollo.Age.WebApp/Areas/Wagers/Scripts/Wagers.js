/// <reference path="../../../../../_GlobalResources/Js/jquery.js" />

var Wagers = function ($) {
    return {

        sportName: null,
        groupBetName: null,
        viewOption: 0,        
        isDirectMember: false,
        league: null,
        fromDate: null,
        groupComm: null,
        sportTypeIds: null,

        init: function () {
            Wagers.sportName = $('#SportName').val();
            Wagers.groupBetName = $('#GroupBetName').val();
            Wagers.viewOption = $('#ViewOption').val();
            Wagers.League = $('#League').val();
            Wagers.fromDate = $('#FromDate').val();
            Wagers.groupComm = $('#groupComm').val();
            Wagers.sportTypeIds = $('#SportTypeIds').val();

            if (Wagers.viewOption > 0) {
                Wagers.isDirectMember = true;
            }
            else {
                Wagers.isDirectMembe = (Wagers.viewOption == 0);
            }
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
            Wagers.init();

            $.ajax({
                url: APOLLO.resolveUrl(url),
                data: {
                    fromDate: Wagers.fromDate,
                    uGroup: Wagers.groupComm,
                    isDirectMember: Wagers.isDirectMember,
                    mainShareHolderId: Wagers.viewOption,
                    leagueId: Wagers.League,
                    sportTypes: Wagers.sportTypeIds,
                    sportName: Wagers.sportName
                },
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