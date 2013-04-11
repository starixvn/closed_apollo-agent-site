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

        var dropDownLanguage = $("#ddlLeague");

        if (dropDownLanguage.length > 0) {

            if (sportId != undefined) {

                dropDownLanguage.prop('disabled', 'disabled');

                var date = $("#date").val();
                $.getJSON(APOLLO.resolveUrl("League"), { "date": date, "sportId": sportId })
            .done(function (data) {
                // clean and rebind with data get by new selected sportid                
                dropDownLanguage.empty();
                $.each(data, function (i, item) {
                    dropDownLanguage.append($('<option>', {
                        value: item.Id,
                        text: item.Name
                    }));
                });

                dropDownLanguage.removeAttr('disabled');
            });
            }
        }
    }
};

$(function () {
    ReportResult.init();
    ReportResult.changeSport();
});