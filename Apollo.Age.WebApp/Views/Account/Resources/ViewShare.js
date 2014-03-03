/// <reference path="../../../_GlobalResources/Js/jquery.js" />
/*global APOLLO*/
'use strict';
var VIEWSHARE = (function ($) {
    return {
        init: function () {
            VIEWSHARE.sportType = $("#ddlSportType");
            VIEWSHARE.betType = $("#ddlBetType");
            VIEWSHARE.userName = $("#txtUsername");
            VIEWSHARE.isMember = $("#ddlIsMember");

            VIEWSHARE.bindAutocomplete();

            VIEWSHARE.sportType.change(function () {
                VIEWSHARE.bindBetType();
            });

            $("#btnSubmit").bind('click', function () {
                VIEWSHARE.getViewShare(-1, VIEWSHARE.userName.val().trim());
            });
        },
        bindBetType: function () {
            var sportId = VIEWSHARE.sportType.val();
            var sportName = VIEWSHARE.sportType.find(":selected").text().trim();

            if (sportId.indexOf("-") > -1) {
                var arrSport = sportId.split("-");

                VIEWSHARE.betType.prop('disabled', 'disabled');
                $.getJSON(APOLLO.resolveUrl("CustomerSetting/GetBetType"), { "sportId": arrSport[1], "isMixSportParlay": (arrSport[0] === "1") })
                    .done(function (data) {
                        // clean and rebind with data get by new selected sportid
                        VIEWSHARE.betType.empty();
                        $.each(data, function (i, item) {
                            VIEWSHARE.betType.append($('<option>', {
                                value: item.Value,
                                text: item.Text
                            }));
                        });

                        VIEWSHARE.betType.removeAttr('disabled');
                    });
            }
        },
        bindAutocomplete: function () {
            VIEWSHARE.userName.autocomplete({
                source: function (request, response) {
                    var url = "";
                    if (VIEWSHARE.isMember.val().toBoolean()) {
                        url = "Account/GetMemberByUsername?username=" + request.term.trim();
                    }
                    else {
                        url = "Account/GetAgentByUsername?username=" + request.term.trim();
                    }

                    $.ajax({
                        url: APOLLO.resolveUrl(url),
                        success: function (data) {
                            var found = $.map(data, function (item) {
                                return {
                                    label: item.Username,
                                    value: item.Username,
                                    id: item.Id
                                };
                            });

                            return response(found);
                        }
                    });
                },
                select: function (event, ui) {
                    VIEWSHARE.getViewShare(ui.item.id, ui.item.value);
                },
                position: { my: "right top", at: "right bottom" }
            });
        },
        getViewShare: function (custId, username) {
            var sportId = VIEWSHARE.sportType.val();
            var betTypeId = VIEWSHARE.betType.val();

            if (betTypeId.indexOf("-") > -1 && sportId.indexOf("-") > -1) {
                var arrSport = sportId.split("-");
                var arrBetType = betTypeId.split("-");
                var url = "";
                if (VIEWSHARE.isMember.val().toBoolean()) {
                    url = APOLLO.resolveUrl('CustomerSetting/MemberShare?memberId=' + custId + '&userName='
                            + username + "&sportType=" + arrSport[1] + "&betType=" + arrBetType[1]
                            + "&isLive=" + (arrBetType[0] === "1") + "&isMixSportParlay=" + (arrSport[0] === "1"));
                }
                else {
                    url = APOLLO.resolveUrl('CustomerSetting/AgentShare?agentId=' + custId + '&userName='
                            + username + "&sportType=" + arrSport[1] + "&betType=" + arrBetType[1]
                            + "&isLive=" + (arrBetType[0] === "1") + "&isMixSportParlay=" + (arrSport[0] === "1"));
                }

                if (url.length > 0) {
                    location.href = url;
                }
            }
        }
    };
} (jQuery));

$(document).ready(function () {
    $.blockUI();
    VIEWSHARE.init();
    $.unblockUI();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);