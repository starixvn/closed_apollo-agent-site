/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var Outright = (function () {
    var ddlSport;
    var ddlLeague;
    var fromDate;
    var toDate;

    return {
        init: function () {
            ddlSport = $("#ddlSport");
            ddlLeague = $("#ddlLeague");
            fromDate = $("#FromDate").val();
            toDate = $("#ToDate").val();
            ddlSport.change(function () {
                Outright.bindLeague();
            });

            ddlLeague.change(function () {
                Outright.showResult();
            });
        },

        bindLeague: function () {
            // if sport id is undefined select first sportid of Sport selection
            var sportId = ddlSport.val();

            if (ddlLeague.length > 0) {
                if (sportId != undefined) {
                    ddlLeague.attr('disabled', true);
                    $.getJSON(APOLLO.resolveUrl("~/Results/OutrightLeague"), { "fromDate": fromDate, "toDate": toDate, "sportId": sportId })
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
                            Outright.showResult();
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
                    $.get(APOLLO.resolveUrl("~/Results/GetOutrightResult"), { "fromdate": fromDate, "todate": toDate, "sportId": sportId, "leagueId": leagueId })
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
    Outright.init();
    Outright.bindLeague();
    //   Outright.showResult();
});