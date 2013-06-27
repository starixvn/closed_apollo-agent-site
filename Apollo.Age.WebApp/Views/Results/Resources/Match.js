/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var Match = (function () {
    var ddlSport;
    var ddlLeague;
    var date;
    var sportTypes;
    var sportId;
    var leagueId;
    return {
        init: function () {
            ddlSport = $("#ddlSport");
            ddlLeague = $("#ddlLeague");
            date = $("#filterbydate").attr("value");
            sportTypes = "1";
            btnToday = $("#btnToday");
            btnYesterday = $("#btnYesterday");



            //            btnToday.bind('click', function () {
            //                $("#EventDate").val($("#todayValue").val());
            //            });

            //            btnYesterday.bind('click', function () {
            //                $("#EventDate").val($("#yesterdayValue").val());
            //            });

            Match.bindingMenu();
            Match.filterByDateChanging();
            Match.showResult(date, sportTypes);
        },

        bindMatchLeague: function () {
            // if sport id is undefined select first sportid of Sport selection
            var _sportId = ddlSport.val();
            if (sportId != _sportId) {
                sportId = _sportId;
            }
            if (ddlLeague.length > 0) {
                if (sportId != undefined) {
                    ddlLeague.attr('disabled', true);
                    $.getJSON(APOLLO.resolveUrl("~/Results/MatchLeague"), { "date": date, "sportTypes": sportTypes, "sportId": sportId })
                        .done(function (data) {
                            // clean and rebind with data get by new selected sportid
                            ddlLeague.empty();
                            $.each(data, function (i, item) {
                                if (i == 0) {
                                    leagueId = item.Id;
                                }
                                ddlLeague.append($('<option>', {
                                    value: item.Id,
                                    text: item.Name
                                }));
                            });
                            ddlLeague.removeAttr('disabled');
                            Match.showOtherResult();
                        });
                }
            }
        },
        bindMatchSport: function () {
            $.getJSON(APOLLO.resolveUrl("~/Results/MatchSport"), { "date": date, "sportTypes": sportTypes })
                        .done(function (data) {
                            ddlSport.empty();
                            $.each(data, function (i, item) {
                                if (i == 0) {
                                    sportId = item.Id;
                                }
                                ddlSport.append($('<option>', {
                                    value: item.Id,
                                    text: item.Name
                                }));
                            });
                            Match.bindMatchLeague();
                        });
        },

        bindOutrightSport: function () {
            $.getJSON(APOLLO.resolveUrl("~/Results/OutrightSport"), { "date": date })
            .done(function (data) {
                ddlSport.empty();
                $.each(data, function (i, item) {
                    if (i == 0) {
                        sportId = item.Id;
                    }
                    ddlSport.append($('<option>', {
                        value: item.Id,
                        text: item.Name
                    }));
                });
            });
        },

        bindingMenu: function () {
            $("[name=menusport]").on("click", function () {
                date = $("#filterbydate").attr("value");
                sportTypes = $(this).attr("id");
                Match.showResult();
            });

            $("[name=menuothersport]").on("click", function () {
                date = $("#filterbydate").attr("value");
                sportTypes = $(this).attr("id");
                Match.OtherResult();

                ddlSport.change(function () {
                    Match.bindMatchLeague();
                });

                ddlLeague.change(function () {
                    Match.showOtherResult();
                });
            });
            $("[name=menuoutright]").on("click", function () {
                date = $("#filterbydate").attr("value");
                sportTypes = $(this).attr("id");
                Match.OutrightResult();

                ddlSport.change(function () {
                    Match.showOutrightrResult();
                });
            });

        },
        filterByDateChanging: function () {
            $("#filterbydate").on("click", function () {
                $this = $(this);
                if ($this.attr("value") == $("#todayValue").val()) // compare with yesterdate
                {
                    $this.html("Today");
                    $this.attr("value", $("#yesterdayValue").val());
                }
                else {
                    $this.html("Yesterday");
                    $this.attr("value", $("#todayValue").val());
                }
            });
        },
        paging: function () {
            $('#pager').sbPager({
                itemPerPage: 5,
                totalItem: 200,
                callback: function (currentPage) {
                    alert('page ' + currentPage + ' is clicked');
                }
            });
        },
        showResult: function () {
            $("#filterOtherResult").hide();
            var resultContainer = $("#resultContainer");
            if (resultContainer.length > 0) {
                if (sportTypes != null) {
                    $.get(APOLLO.resolveUrl("~/Results/MatchResultBySport"), { "date": date, "sportTypes": sportTypes })
                      .done(function (resultContent) {
                          resultContainer.empty();
                          resultContainer.html(resultContent);
                          Match.paging();

                      });
                }
            }
        },
        showOtherResult: function () {
            leagueId = $("#ddlLeague").val();
            var resultContainer = $("#resultContainer");
            if (resultContainer.length > 0) {
                if (sportTypes != null) {
                    $.get(APOLLO.resolveUrl("~/Results/MatchOtherResultBySport"), { "date": date, "sportTypes": sportTypes, "sportId": sportId, "leagueId": leagueId })
                      .done(function (resultContent) {
                          resultContainer.empty();
                          resultContainer.html(resultContent);

                      });
                }
            }
          },
          showOutrightrResult: function () {
              leagueId = $("#ddlLeague").val();
              var resultContainer = $("#resultContainer");
              if (resultContainer.length > 0) {
                  if (sportTypes != null) {
                      $.get(APOLLO.resolveUrl("~/Results/OutrightResult"), { "date": date, "sportTypes": sportTypes, "sportId": sportId, "leagueId": leagueId })
                      .done(function (resultContent) {
                          resultContainer.empty();
                          resultContainer.html(resultContent);

                      });
                  }
              }
          },
        OtherResult: function () {
            $("#filterOtherResult").show();
            Match.bindMatchSport();

        },
        OutrightResult: function () {
            $("#filterOtherResult").show();
            Match.bindOutrightSport();
        }
    }
})();

$(function () {
    Match.init();
    //Match.bindMatchLeague();
    
});