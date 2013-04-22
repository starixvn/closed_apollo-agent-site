/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var VIEWSHARE = function ($) {
    var sportType = $("#ddlSportType");
    var betType = $("#ddlBetType");
    var userName = $("#txtUsername");
    var isMember = $("#ddlIsMember");

    return {
        init: function () {
            sportType.change(function () {
                VIEWSHARE.bindBetType();
            });

            $("#btnSubmit").bind('click', function () {
                var betTypeId = betType.val();
                if (betTypeId.indexOf("-") > -1) {
                    var arrBetType = betTypeId.split("-");
                    var url = "";
                    if (isMember.val() == "0") {
                        url = APOLLO.resolveUrl('CustomerSetting/AgentShare?agentId=-1&userName='
                            + userName.val().trim() + "&sportType=" + sportType.val() + "&betType=" + arrBetType[1]
                            + "&isLive=" + (arrBetType[0] == "0" ? false : true));
                    }
                    else {
                        url = APOLLO.resolveUrl('CustomerSetting/MemberShare?memberId=-1&userName='
                            + userName.val().trim() + "&sportType=" + sportType.val() + "&betType=" + arrBetType[1]
                            + "&isLive=" + (arrBetType[0] == "0" ? false : true));
                    }

                    if (url.length > 0) {
                        location.href = url;
                    }
                }
            });
        },
        bindBetType: function () {
            var sportId = sportType.val();
            if (betType.length > 0) {
                if (sportId != undefined) {
                    betType.prop('disabled', 'disabled');
                    $.getJSON(APOLLO.resolveUrl("CustomerSetting/GetBetType"), { "sportId": sportId })
                        .done(function (data) {
                            // clean and rebind with data get by new selected sportid
                            betType.empty();
                            $.each(data, function (i, item) {
                                betType.append($('<option>', {
                                    value: item.Value,
                                    text: item.Text
                                }));
                            });

                            betType.removeAttr('disabled');
                        });
                }
            }
        }
    }
} (jQuery);

$(function () {
    VIEWSHARE.init();
});