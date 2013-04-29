/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var MatchResult = (function () {
    var ddlSport;
    var ddlLeague;
    var date;

    return {
        init: function () {
            ddlSport = $("#ddlSport");
            ddlLeague = $("#ddlLeague");
            date = $("#EventDate").val();

            ddlSport.change(function () {
                MatchResult.bindLeague();
                MatchResult.showMatchResult();
            });

            ddlLeague.change(function () {
                MatchResult.showMatchResult();
            });
        },

        bindLeague: function () {
            // if sport id is undefined select first sportid of Sport selection
            var sportId = ddlSport.val();

            if (ddlLeague.length > 0) {
                if (sportId != undefined) {
                    ddlLeague.attr('disabled', true);
                    $.getJSON(APOLLO.resolveUrl("~/Results/League"), { "date": date, "sportId": sportId })
                        .done(function (data) {
                            // clean and rebind with data get by new selected sportid
                            ddlLeague.empty();
                            $.each(data, function (i, item) {
                                ddlLeague.append($('<option>', {
                                    value: item.Id,
                                    text: item.Name
                                }));
                            });
                            ddlLeague.removeAttr('disabled');
                        });
                }
            }
        },

        showResult: function () {
            var resultContainer = $("#resultContainer");
            if (resultContainer.length > 0) {
                var sportId = ddlSport.val();
                var leagueId = ddlLeague.val();

                if (sportId != null) {
                    $.get(APOLLO.resolveUrl("~/Results/GetMatchResult"), { "date": date, "sportId": sportId, "leagueId": leagueId })
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
    MatchResult.init();
    MatchResult.bindLeague();
    MatchResult.showResult();
});