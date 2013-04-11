/// <reference path="../../../_GlobalResources/Js/jquery.js" />
var ReportResult = {
    init: function () {
        var me = this;
        $("#ddlSport").change(function () {
            me.bindLeague();
        });

        $("#ddlLeague").change(function () {
            console.log("adsfafasdf");
            me.showMatchResult();
        });
    },
    bindLeague: function () {

        // if sport id is undefined select first sportid of Sport selection        
        var sportId = $("#ddlSport").val();

        var dropDownLanguage = $("#ddlLeague");

        if (dropDownLanguage.length > 0) {
            if (sportId != undefined) {
                dropDownLanguage.prop('disabled', 'disabled');
                var date = $("#date").val();
                $.getJSON(APOLLO.resolveUrl("~/Results/League"), { "date": date, "sportId": sportId })
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
    },
    showMatchResult: function () {
        var resultContainer = $("#resultContainer");
        if (resultContainer.length > 0) {

            var sportId = $("#ddlSport").val();
            console.log(sportId);
            var leagueId = $("#ddlLeague").val();
            console.log(leagueId);
            var date = $("#date").val();
            console.log(leagueId);
            if (sportId != null) {
                $.get(APOLLO.resolveUrl("~/Results/MatchResult"),
              { "date": date, "sportId": sportId, "leagueId": leagueId })
              .done(function (resultContent) {
                  resultContainer.empty();
                  resultContainer.html(resultContent);
              });
            }
        }
    }
};

$(function () {
    ReportResult.init();
    ReportResult.bindLeague();
    ReportResult.showMatchResult();    
});