/// <reference path="../../../_GlobalResources/Js/jquery.js" />
//// New Code
var scoreMap = {
    BaseLink: window.rootUrl + 'Forecast/',
    ViewPartial: false,
    Init: function () {
        //// Cached jQuery Objects
        scoreMap.Loading = $("#loading");
        scoreMap.LoadingSubmit = $("#loadingSubmit");
        var matchIds = $('#match-ids').val();
        if (matchIds != "") {
            scoreMap.ViewPartial = true;
            scoreMap.LoadBetTypesList(matchIds);
            scoreMap.LoadResults($('#bet-types-live').val(), $('#bet-types-dead').val());
        }
        else {
            scoreMap.ResigterEvents();
        }
    },
    ResigterEvents: function () {
        $("#box_header").on("click", "#RefreshLeagues", function () {
            scoreMap.Loading.addClass("loading");
            var url = scoreMap.BaseLink + 'ScoreMapLeagues?currentLeagueId=' + $('#ddlLeague').val();
            $.get(url, function (contentView) {
                $("#selLeague").html(contentView);
                scoreMap.Loading.removeClass("loading");
            });
        });

        $("#dSelMatch").click(function () {
            scoreMap.Loading.addClass("loading");
            var url = scoreMap.BaseLink + 'ScoreMapMatch?leagueId=' + $('#ddlLeague').val();
            $.get(url, function (contentView) {
                scoreMap.Loading.removeClass("loading");
                $("#BoxMatchWL").replaceWith(contentView);
                scoreMap.RegisterCheckAll("#chLCheckAll", ".match-item");
                $("#closeBoxMatch").click(function () {
                    $("#BoxMatchWL").hide();
                });
                $("#dSubmitInMatch").click(function () {
                    scoreMap.LoadBetTypesList();
                });
            });
        });

    },
    LoadBetTypesList: function (matchIds) {
        $('#tblMsgMatch').hide();
        if (matchIds) {
            matchIds = $('#match-ids').val();
        }
        else {
            matchIds = scoreMap.GetIdFromSelectedCheckBox($(".match-item:checked"));
            $("#match-ids").val(matchIds);
            
        }
        if (matchIds == "") {
            $('#msgMatchErr').text("Please choose a Match!");
            $('#tblMsgMatch').show();
            return;
        }
        $("#BoxMatchWL").hide();
        var url = scoreMap.BaseLink + 'ScoreMapBetType';
        //// If Show In PopUp then add BetType
        if (scoreMap.ViewPartial) {
            url += "?liveBetType=" + $('#bet-types-live').val()
            + "&deadBetType=" + $('#bet-types-dead').val();
        }
        $.get(url, function (contentView) {
            $("#tblLiveDead").show();
            $("#bet-type-container").html(contentView);
            //bet-type-live
            scoreMap.RegisterCheckAll("#chkAllL", ".bet-type-live");
            scoreMap.RegisterCheckAll("#chkAllD", ".bet-type-deadball");
            $("#dSubmit").click(function () {
                scoreMap.LoadResults();
            });
        });
    },
    LoadResults: function (betTypesLive, betTypesDead) {
        if (betTypesLive || betTypesDead) {
        }
        else {
            betTypesLive = scoreMap.GetIdFromSelectedCheckBox($('.bet-type-live:checked'));
            betTypesDead = scoreMap.GetIdFromSelectedCheckBox($('.bet-type-deadball:checked'));
        }
        if (betTypesLive == "" && betTypesDead == "") {
            ageMsg.Show("Please choose a bet type", 0);
            return;
        }
        scoreMap.LoadingSubmit.addClass("loading");
        var url = scoreMap.BaseLink + 'ScoreMapResult?matchIds=' + $('#match-ids').val()
            + '&liveBetTypes=' + betTypesLive
            + '&deadBallBetTypes=' + betTypesDead;
        $.get(url, function (contentView) {
            //// Hide Loading and Hide Message
            scoreMap.LoadingSubmit.removeClass("loading");
            ageMsg.Hide();
            $("#mainContent").html(contentView);
            scoreMap.RegisterEventsForResultMatch($("#content_main"));
        });
    },
    RegisterEventsForResultMatch: function (resultContainer) {
        resultContainer.find(".button_mid").click(function () {
            var $this = $(this);
            var spanExpand = $this.find(".span-expand");
            if (spanExpand.hasClass("lnkGroup")) {
                spanExpand.removeClass("lnkGroup").addClass("lnkGroupClick");
            }
            else {
                spanExpand.removeClass("lnkGroupClick").addClass("lnkGroup");
            }
            $this.parent().find(".scoreMap").toggle();
        });

        resultContainer.find(".button_right").click(function () {
            var $this = $(this);
            var spanButton = $this.find(".refresh-button");
            if (spanButton.hasClass("lnkRefresh")) {
                spanButton.removeClass("lnkRefresh").addClass("lnkRefreshClick");
            }
            else {
                spanButton.removeClass("lnkRefreshClick").addClass("lnkRefresh");
            }
            var attrMatchId = $this.attr("match-id");
            var url = scoreMap.BaseLink + 'ScoreMapResult?matchIds=' + attrMatchId
                    +'&liveBetTypes=' + scoreMap.GetIdFromSelectedCheckBox($('.bet-type-live:checked'))
                    + '&deadBallBetTypes=' + scoreMap.GetIdFromSelectedCheckBox($('.bet-type-deadball:checked'));
            var matchId = "#RefreshContent" + attrMatchId;
            //RefreshContent
            $.get(url, function (contentView) {
                $this.parent().replaceWith(contentView);
                scoreMap.RegisterEventsForResultMatch($(matchId));
            });
        });
    },
    GetIdFromSelectedCheckBox: function (selectedCheckBox) {
        var length = selectedCheckBox.length;
        var ids = "";
        for (var i = 0; i < length; i++) {
            ids += selectedCheckBox[i].value + ",";
        }
        return ids;
    },
    RegisterCheckAll: function (checkAllId, itemClass) {
        var $matchItems = $(itemClass);
        var $checkAll = $(checkAllId);
        $checkAll.click(function () {
            var $this = $(this);
            if ($this.prop("checked") == true) {
                $matchItems.prop("checked", true);
            }
            else {
                $matchItems.prop("checked", false);
            }
        });

        $matchItems.click(function () {
            var $matchItem = $(this);
            if ($matchItem.prop("checked") == false) {
                $checkAll.prop("checked", false);
            }
            else {
                if ($matchItems.not(":checked").length == 0) {
                    $checkAll.prop("checked", true);
                }
            }
        });
    }
};

$(function () {
    scoreMap.Init();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);
