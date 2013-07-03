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

            Wagers.fromDate = Wagers.isOR == "true" ? $('#OutrightFromDate').val() : $('#FromDate').val();
            Wagers.toDate = $('#OutrightToDate').val();
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
                if ($this.val() > 0) {
                    Wagers.isDirectMember = false;
                }
                else {
                    Wagers.isDirectMember = ($this.val() == 0 ? false : true);
                }

                Wagers.getWagerData();
            });

            $('#League').on('change', function () {
                Wagers.League = $(this).val();
                Wagers.getWagerData();
            });

            $('#groupComm').on('change', function () {
                Wagers.groupComm = $(this).val();
                Wagers.getWagerData();
            });

            $('#EmDate').on('change', function () {
                Wagers.fromDate = $(this).val();
                Wagers.getWagerData();
            });

            if (Wagers.isOR == "true") {
                $("#btnSubmit").on('click', function () {
                    Wagers.fromDate = $('#OutrightFromDate').val();
                    Wagers.toDate = $('#OutrightToDate').val();
                    Wagers.getWagerData();
                });
            }
        },

        getWagerData: function () {

            $.ajax({
                url: APOLLO.resolveUrl(Wagers.url),
                data: {
                    fromDate: Wagers.fromDate,
                    toDate: Wagers.toDate,
                    uGroup: Wagers.groupComm,
                    isDirectMember: Wagers.isDirectMember,
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

                var url = "~/Betlist/Wagers?fromDate=" + Wagers.fromDate
                + "&uGroup=" + Wagers.groupComm
                + "&isDirectMem=" + Wagers.isDirectMember
                + "&liveIndicator=" + live
                + "&leagueId=" + Wagers.League
                + "&betType=" + Wagers.betTypeIds
                + "&sportType=" + Wagers.sportTypeIds
                + "&betTeam=" + betteam
                + "&loggedId=" + Wagers.viewOption;


                window.open(APOLLO.resolveUrl(url), '_blank');
            });
        }
    }
} (jQuery);

$(function () {
    Wagers.init();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);