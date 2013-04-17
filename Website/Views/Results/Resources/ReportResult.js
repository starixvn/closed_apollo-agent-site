/// <reference path="../../../_GlobalResources/Js/jquery.js" />
var REPORTRESULTMODULE = (function () {

    var sportDropDownList = $("#ddlSport");
    var leagueDropDownList = $("#ddlLeague");
    var date = $("#EventDate").val();
    var me = this;

    return {
        init: function () {
            sportDropDownList.change(function () {
                REPORTRESULTMODULE.bindLeague();
                REPORTRESULTMODULE.showMatchResult();
            });

            leagueDropDownList.change(function () {
                REPORTRESULTMODULE.showMatchResult();
            });
        },
        bindLeague: function () {

            // if sport id is undefined select first sportid of Sport selection        
            var sportId = sportDropDownList.val();

            if (leagueDropDownList.length > 0) {
                if (sportId != undefined) {
                    leagueDropDownList.prop('disabled', 'disabled');                                        
                    $.getJSON(APOLLO.resolveUrl("~/Results/League"), { "date": date, "sportId": sportId })
                .done(function (data) {
                    // clean and rebind with data get by new selected sportid                
                    leagueDropDownList.empty();
                    $.each(data, function (i, item) {
                        leagueDropDownList.append($('<option>', {
                            value: item.Id,
                            text: item.Name
                        }));
                    });
                    leagueDropDownList.removeAttr('disabled');
                });
                }
            }
        },
        showMatchResult: function () {
            var resultContainer = $("#resultContainer");
            if (resultContainer.length > 0) {

                var sportId = sportDropDownList.val();
                var leagueId = leagueDropDownList.val();
                
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
    }
})();

$(function () {
    REPORTRESULTMODULE.init();
    REPORTRESULTMODULE.bindLeague();
});