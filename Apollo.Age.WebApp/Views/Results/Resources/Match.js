/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var Match = (function () {
    var ddlSport;
    var ddlLeague;
    var date;
    var sportTypes;
    return {
        init: function () {
            ddlSport = $("#ddlSport");
            ddlLeague = $("#ddlLeague");
            date = $("#filterbydate").attr("value");
            sportTypes = "1";
            btnToday = $("#btnToday");
            btnYesterday = $("#btnYesterday");

            //            ddlSport.change(function () {
            //                Match.bindLeague();
            //            });

            //            ddlLeague.change(function () {
            //                Match.showResult();
            //            });

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

        bindLeague: function () {
            // if sport id is undefined select first sportid of Sport selection
            var sportId = ddlSport.val();

            if (ddlLeague.length > 0) {
                if (sportId != undefined) {
                    ddlLeague.attr('disabled', true);
                    $.getJSON(APOLLO.resolveUrl("~/Results/MatchLeague"), { "date": date, "sportId": sportId })
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
                            Match.showResult();
                        });
                }
            }
        },
        bindSport: function (date, sportTypes) {
            $.getJSON(APOLLO.resolveUrl("~/Results/MatchSport"), { "date": date, "sportTypes": sportTypes })
                        .done(function (data) {
                            ddlSport.empty();
                            $.each(data, function (i, item) {
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
                Match.showOtherResult();
            });
            $("[name=menuoutright]").on("click", function () {
                date = $("#filterbydate").attr("value");
                sportTypes = $(this).attr("id");
                Match.showOutrightResult();
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
            //            else {
            //                location.href = "/results/match?Sporttype=" + sportId + "&date:" + date;
            //            }
        },
        showOtherResult: function () {
            $("#filterOtherResult").show();
            //Match.bindSport(date, sportId);
            Match.showResult();

        },
        showOutrightResult: function () {
            alert("test");
        }
    }
})();

$(function () {
    Match.init();
    //Match.bindLeague();
    
});