/// <reference path="../../../_GlobalResources/Js/jquery.js" />



var ReportResult = {
    init: function () {
        $("#ddlSport").change(function () {
            ReportResult.changeSport($(this).val());
        });
    },
    changeSport: function (sportId) {

        // if sport id is undefined select first sportid of Sport selection
        if (sportId === undefined) {
            sportId = $("#ddlSport").val();
        }

        if (sportId != undefined) {
            var date = $("#date").val();
            $.getJSON(APOLLO.resolveUrl("League"), { "date": date, "sportId": sportId })
            .done(function (data) {
                // clean and rebind with data get by new selected sportid
                $("#ddlLeague").empty();                
                $.each(data, function (i, item) {
                    $("#ddlLeague").append($('<option>', {
                        value: item.Id,
                        text: item.Name
                    }));
                });
            });
        }
    }
};

$(function () {
    ReportResult.init();
    ReportResult.changeSport();
});