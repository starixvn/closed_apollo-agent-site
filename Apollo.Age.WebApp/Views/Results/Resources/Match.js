/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var Match = (function () {
    var ddlSport;
    var ddlLeague;
    var date;
    var btnToday;
    var btnYesterday;

    return {
        init: function () {
            ddlSport = $("#ddlSport");
            ddlLeague = $("#ddlLeague");
            date = $("#EventDate").val();
            btnToday = $("#btnToday");
            btnYesterday = $("#btnYesterday");

            //            ddlSport.change(function () {
            //                Match.bindLeague();
            //            });

            //            ddlLeague.change(function () {
            //                Match.showResult();
            //            });

            btnToday.bind('click', function () {
                $("#EventDate").val($("#todayValue").val());
            });

            btnYesterday.bind('click', function () {
                $("#EventDate").val($("#yesterdayValue").val());
            });
            Match.showResult($("#todayValue").val(), 1);
            Match.bindingMenu();
            Match.filterByDateChanging();
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
        bindingMenu: function () {
            $("[name=menusport]").on("click", function () {
                var date = $("#filterbydate").attr("value");
                var sportid = $(this).attr("id");
                Match.showResult(date, sportid);
            });
        },
        filterByDateChanging: function () {
            $("#filterbydate").on("click", function () {
                $this = $(this);
                if ($this.attr("value") == $("#todayValue").val()) // compare with yesterdate
                {
                    $this.html("Yesterday");
                    $this.attr("value", $("#yesterdayValue").val());
                }
                else {
                    $this.html("Today");
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
        showResult: function (date, sportId) {
            var resultContainer = $("#resultContainer");
            if (resultContainer.length > 0) {
                if (sportId != null) {
                    $.get(APOLLO.resolveUrl("~/Results/ResultBySport"), { "date": date, "sportId": sportId })
                      .done(function (resultContent) {
                          resultContainer.empty();
                          resultContainer.html(resultContent);
                          Match.paging();

                      });
                }
            }
        }
    }
})();

$(function () {
    Match.init();
    //Match.bindLeague();
    
});