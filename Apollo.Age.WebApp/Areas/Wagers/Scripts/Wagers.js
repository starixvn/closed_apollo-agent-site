/// <reference path="../../../../../_GlobalResources/Js/jquery.js" />

var Wagers = function ($) {
    return {

        sportName: null,
        groupBetName: null,
        viewOption: 0,
        isDirectMember: true,
        league: null,
        fromDate: null,
        groupComm: null,
        sportTypeIds: null,
        url: null,

        init: function () {
            Wagers.sportName = $('#SportName').val();
            Wagers.groupBetName = $('#GroupBetName').val();
            Wagers.viewOption = $('#ViewOption').val();
            Wagers.League = $('#League').val();
            Wagers.fromDate = $('#FromDate').val();
            Wagers.groupComm = $('#groupComm').val();
            Wagers.sportTypeIds = $('#SportTypeIds').val();

            if (Wagers.viewOption > 0) {
                Wagers.isDirectMember = false;
            }
            else {

                Wagers.isDirectMember = (Wagers.viewOption == 0 ? false : true);
            }

            if (Wagers.sportName.toLowerCase() != "soccer") {
                Wagers.url = "~/wagers/basketball/" + Wagers.groupBetName;
            }
            else {
                Wagers.url = "~/wagers/" + Wagers.sportName + "/" + Wagers.groupBetName;
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

        },

        getWagerData: function (pageIndex) {

            if (pageIndex == undefined) {
                pageIndex = 0;
            }

            $.ajax({
                url: APOLLO.resolveUrl(Wagers.url),
                data: {
                    fromDate: Wagers.fromDate,
                    groupComm: Wagers.groupComm,
                    isDirectMember: Wagers.isDirectMember,
                    mainShareHolderId: Wagers.viewOption,
                    leagueId: Wagers.League,
                    sportTypes: Wagers.sportTypeIds,
                    sportName: Wagers.sportName,
                    pageIndex: pageIndex
                },
                success: function (data) {
                    $('#container').html(data);
                    Wagers.initPager(pageIndex);
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
                    Wagers.getWagerData(currentPage);
                }
            });
        }
    }
} (jQuery);

$(function () {
    Wagers.init();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);