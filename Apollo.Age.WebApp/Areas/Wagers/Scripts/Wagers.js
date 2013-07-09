/// <reference path="../../../../../_GlobalResources/Js/jquery.js" />

var Wagers = function ($) {
    return {
        sportName: null,
        groupBetName: null,
        viewOption: 0,
        isDirectMember: true,
        league: null,
        fromDate: null,
        toDate: null,
        groupComm: null,
        sportTypeIds: null,
        url: null,
        isEM: false,
        isOR: false,
        pageIndex: 0,
        betTypeIds: null,

        init: function () {
            Wagers.sportName = $('#SportName').val();
            Wagers.groupBetName = $('#GroupBetName').val();
            Wagers.viewOption = $('#ViewOption').val();
            Wagers.League = $('#League').val();
            Wagers.groupComm = $('#groupComm').val();
            Wagers.sportTypeIds = $('#SportTypeIds').val();
            Wagers.isEM = $('#IsEM').val();
            Wagers.isOR = $('#IsOR').val();
            Wagers.betTypeIds = $('#BetTypeIds').val();
            
            if (Wagers.isOR == "true") {
                Wagers.fromDate = $('#OutrightFromDate').val();
                Wagers.toDate = $('#OutrightToDate').val();
            }
            else if (Wagers.isEM == "true") {
                Wagers.fromDate = $('#EmDate').val();
            }
            else {
                Wagers.fromDate = $('#FromDate').val();
                Wagers.toDate = $('#ToDate').val();
            }            

           
            if (Wagers.viewOption > 0) {
                Wagers.isDirectMember = false;
            }
            else {
                Wagers.isDirectMember = (Wagers.viewOption == 0 ? false : true);
            }
            if (Wagers.isOR == "true") {
                Wagers.url = "~/wagers/Outright/WagerOutright";
            }
            else {
                if (Wagers.sportName.toLowerCase() != "soccer") {
                    Wagers.url = "~/wagers/basketball/" + Wagers.groupBetName;
                }
                else {
                    Wagers.url = "~/wagers/" + Wagers.sportName + "/" + Wagers.groupBetName;
                }
            }
            Wagers.getWagerData();

            $('#ViewOption').on('change', function () {
                var $this = $(this);
                Wagers.viewOption = $this.val();
                if ($this.val() > 0) {
                    Wagers.isDirectMember = false;
                }
                else {
                    Wagers.isDirectMember = ($this.val() == 0 ? false : true);
                }
                if (Wagers.isOR == "true") {
                    Wagers.getLeagues();
                }
                else {
                    Wagers.getWagerData();
                }
            });

            $('#League').on('change', function () {
                Wagers.League = $(this).val();
                Wagers.getWagerData();
            });

            $('#groupComm').on('change', function () {
                if (Wagers.isOR == "true") {
                    location.href = APOLLO.resolveUrl("~/wagers/wager?sportname=" + $(this).val()  + "&isOR=true");
                }
                else {
                    Wagers.groupComm = $(this).val();
                    Wagers.getWagerData();
                }

            });

            $('#EmDate').on('change', function () {
                Wagers.fromDate = $(this).val();
                Wagers.getLeagues(false);                      
            });

            if (Wagers.isOR == "true") {
                $("#btnSubmit").on('click', function () {
                    var newFromDate = $('#OutrightFromDate').val();
                    var newToDate = $('#OutrightToDate').val();

                    if (newFromDate > newToDate) {
                        alert($('#RecheckRangeOfDateText').val());
                        return;
                    }

                    if (Wagers.fromDate != newFromDate || newToDate != Wagers.toDate) {
                        Wagers.fromDate = newFromDate;
                        Wagers.toDate = newToDate;
                        Wagers.getLeagues(true);
                    }
                    else {
                        Wagers.getWagerData();
                    }
                });
            }
        },

        getLeagues: function (isOutright) {
            $.ajax({
                url: APOLLO.resolveUrl("~/wagers/wager/GetJsonLeague"),
                data: {
                    fromDate: Wagers.fromDate,
                    toDate: Wagers.isOR == "true" ? Wagers.toDate : null,
                    sportTypes: Wagers.sportTypeIds,
                    isOutright: isOutright
                },
                success: function (data) {
                    $('#League').empty();
                    $.each(data, function (i, item) {
                        if (i == 0) {
                            Wagers.League = item.Id;
                        }
                        $('#League').append($('<option>', {
                            value: item.Id,
                            text: item.Name
                        }));
                    });
                    Wagers.getWagerData();
                }
            });
        },

        getWagerData: function () {
            if (Wagers.isOR == "true") {
                Wagers.groupComm = "All";
            }

            $.ajax({
                url: APOLLO.resolveUrl(Wagers.url),
                data: {
                    fromDate: Wagers.fromDate,
                    toDate: Wagers.toDate,
                    uGroup: Wagers.groupComm,
                    IsDirectMem: Wagers.isDirectMember,
                    loggedId: Wagers.viewOption,
                    leagueId: Wagers.League,
                    sportTypes: Wagers.sportTypeIds,
                    sportName: Wagers.sportName,
                    pageIndex: Wagers.pageIndex,
                    isEM: Wagers.isEM
                },
                success: function (data) {
                    $('#container').html(data);
                    Wagers.initPager(Wagers.pageIndex);
                    Wagers.initWagerBetList();
                    $('.est-zone').html($('#TimeZone').val());
                }
            });
        },

        initPager: function (pageIndex) {
            $('#pager').sbPager({
                currentPage: pageIndex,
                itemPerPage: 40,
                totalItem: $('#TotalItems').val(),
                //numPageDisplay: 8,
                callback: function (currentPage) {
                    Wagers.pageIndex = currentPage;
                    Wagers.getWagerData();
                }
            });
        },

        initWagerBetList: function () {
            $('.wager-link').on('click', function () {

                var betteam = $(this).attr('data-betTeams');
                var live = $(this).attr('data-live');
                var league = $(this).attr('data-league');
                var bettype = $(this).parents('td').attr('data-bettype');
                var match = $(this).attr('data-match');

                var url = "~/Betlist/Wagers?fromDate=" + Wagers.fromDate
                + "&toDate=" + (Wagers.isOR == "true" ? Wagers.toDate : null)
                + "&uGroup=" + (Wagers.isOR == "true" ? 'All' : Wagers.groupComm)
                + "&isDirectMem=" + Wagers.isDirectMember
                + "&liveIndicator=" + live
                + "&leagueId=" + league
                + "&betType=" + bettype
                + "&sportType=" + Wagers.sportTypeIds
                + "&betTeam=" + betteam
                + "&matchId=" + match
                + "&loggedId=" + Wagers.viewOption;


                window.open(APOLLO.resolveUrl(url), '_blank');
            });
        }
    }
} (jQuery);

$(function () {
    Wagers.init();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);