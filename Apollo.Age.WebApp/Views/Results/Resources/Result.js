/// <reference path="../../../_GlobalResources/Js/jquery.js" />
/*global APOLLO*/
'use strict';
var Result = (function () {
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
            date = $("#hardcodedatetime").val() != null ? $("#hardcodedatetime").val() : $("#filterbydate").attr("value");
            sportTypes = "1";
            Result.bindingMenu();
            Result.filterByDateChanging();
            Result.showResult(date, sportTypes);
        },

        bindMatchLeague: function () {
            // if sport id is undefined select first sportid of Sport selection
            var _sportId = ddlSport.val();
            if (sportId !== _sportId) {
                sportId = _sportId;
            }
            if (ddlLeague.length > 0) {
                if (sportId !== undefined) {
                    ddlLeague.attr('disabled', true);
                    $.getJSON(APOLLO.resolveUrl("~/Results/GetLeagueBySport"), { "date": date, "sportTypes": sportTypes, "sportId": sportId })
                        .done(function (data) {
                            // clean and rebind with data get by new selected sportid
                            if (data[0].Id !== -1) {
                                ddlLeague.empty();
                                $.each(data, function (i, item) {
                                    if (i === 0) {
                                        leagueId = item.Id;
                                    }
                                    ddlLeague.append($('<option>', {
                                        value: item.Id,
                                        text: item.Name
                                    }));
                                });
                                ddlLeague.removeAttr('disabled');
                            }
                            else {
                                $("#filterResult").hide();
                                $("#filterResultLeague").hide();
                            }
                            Result.showOtherResult();
                        }).error(function (jqXHR, exception) {
                            location.href = APOLLO.resolveUrl("~/Error/AccessDenied");
                        });
                }
            }
        },
        bindMatchSport: function () {
            $.getJSON(APOLLO.resolveUrl("~/Results/GetSportByMatch"), { "date": date, "sportTypes": sportTypes })
                        .done(function (data) {
                            if (data[0].Id !== -1) {
                                ddlSport.empty();
                                $.each(data, function (i, item) {
                                    if (i === 0) {
                                        sportId = item.Id;
                                    }
                                    ddlSport.append($('<option>', {
                                        value: item.Id,
                                        text: item.Name
                                    }));
                                });
                            }
                            else {
                                $("#filterResult").hide();
                                $("#filterResultLeague").hide();
                            }
                            Result.bindMatchLeague();
                        }).error(function (jqXHR, exception) {
                            location.href = APOLLO.resolveUrl("~/Error/AccessDenied");
                        });
        },

        bindOutrightSport: function () {
            $.getJSON(APOLLO.resolveUrl("~/Results/GetSportOutright"), { "date": date })
            .done(function (data) {
                if (data[0].Id !== -1) {
                    ddlSport.empty();
                    $.each(data, function (i, item) {
                        if (i === 0) {
                            sportId = item.Id;
                        }
                        ddlSport.append($('<option>', {
                            value: item.Id,
                            text: item.Name
                        }));
                    });
                }
                else {
                    $("#filterResult").hide();
                }
                Result.showOutrightResult();
            }).error(function (jqXHR, exception) {
                location.href = APOLLO.resolveUrl("~/Error/AccessDenied");
            });
        },
        reActiveMenu: function (action) {
            $("#filterbydate").attr("action", action);
        },
        bindingMenu: function () {
            $("[name=menusport]").on("click", function () {
                date = $("#hardcodedatetime").val() != null ? $("#hardcodedatetime").val() : $("#filterbydate").attr("value");
                sportTypes = $(this).attr("id");
                Result.reActiveMenu(0);
                Result.showResult();
            });

            $("[name=menuothersport]").on("click", function () {
                date = $("#hardcodedatetime").val() != null ? $("#hardcodedatetime").val() : $("#filterbydate").attr("value");
                sportTypes = $(this).attr("id");
                Result.reActiveMenu(1);
                Result.OtherResult();

                ddlSport.unbind('change').bind('change', function () {
                    Result.bindMatchLeague();
                });

                ddlLeague.unbind('change').bind('change', function () {
                    Result.showOtherResult();
                });
            });
            $("[name=menuoutright]").on("click", function () {
                date = $("#hardcodedatetime").val() != null ? $("#hardcodedatetime").val() : $("#filterbydate").attr("value");
                sportTypes = $(this).attr("id");
                Result.reActiveMenu(2);
                Result.OutrightResult();

                ddlSport.unbind('change').bind('change', function () {
                    Result.showOutrightResult();
                });
            });
        },
        filterByDateChanging: function () {
            $("#filterbydate").on("click", function () {
                var $this = $(this);
                if ($this.attr("value") === $("#todayValue").val()) // compare with yesterdate
                {
                    $this.html($("#lbltoday").val());
                    $this.attr("value", $("#yesterdayValue").val());
                }
                else {
                    $this.html($("#lblyesterday").val());
                    $this.attr("value", $("#todayValue").val());
                }
                date = $this.attr("value");
                var $action = $this.attr("action");
                switch ($action) {
                    case "0":
                        {
                            Result.showResult();
                            break;
                        }
                    case "1":
                        {
                            Result.showOtherResult();
                            break;
                        }
                    case "2":
                        {
                            Result.showOutrightResult();
                            break;
                        }
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
            $("#filterResult").hide();
            var resultContainer = $("#resultContainer");
            if (resultContainer.length > 0) {
                if (sportTypes != null) {
                    $.get(APOLLO.resolveUrl("~/Results/GetMatchResult"), { "date": date, "sportTypes": sportTypes })
                      .done(function (resultContent) {
                          resultContainer.empty();
                          resultContainer.html(resultContent);
                          // Result.paging();
                      })
                      .error(function (jqXHR, exception) {
                          location.href = APOLLO.resolveUrl("~/Error/AccessDenied");
                      });
                }
            }
        },
        showOtherResult: function () {
            leagueId = $("#ddlLeague").val();
            var resultContainer = $("#resultContainer");
            if (resultContainer.length > 0) {
                if (sportTypes != null) {
                    $.get(APOLLO.resolveUrl("~/Results/GetOtherResult"), { "date": date, "sportTypes": sportTypes, "sportId": sportId, "leagueId": leagueId })
                      .done(function (resultContent) {
                          resultContainer.empty();
                          resultContainer.html(resultContent);
                      }).error(function (jqXHR, exception) {
                          location.href = APOLLO.resolveUrl("~/Error/AccessDenied");
                      });
                }
            }
        },
        showOutrightResult: function () {
            sportId = ddlSport.val();
            var resultContainer = $("#resultContainer");
            if (resultContainer.length > 0) {
                if (sportTypes != null) {
                    $.get(APOLLO.resolveUrl("~/Results/Outright"), { "date": date, "sportId": sportId })
                      .done(function (resultContent) {
                          resultContainer.empty();
                          resultContainer.html(resultContent);
                      }).error(function (jqXHR, exception) {
                          location.href = APOLLO.resolveUrl("~/Error/AccessDenied");
                      });
                }
            }
        },
        OtherResult: function () {
            $("#filterResult").show();
            $("#filterResultLeague").show();
            Result.bindMatchSport();
        },
        OutrightResult: function () {
            $("#filterResult").show();
            $("#filterResultLeague").hide();
            Result.bindOutrightSport();
        }
    };
})();

$(function () {
    Result.init();
    //Result.bindMatchLeague();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);