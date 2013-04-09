/// <reference path="../../../_GlobalResources/Js/jquery.js" />
//// New Code
var scoreMap = {
    BaseLink: window.rootUrl + 'Forecast/',
    Init: function () {
        var matchIds = $('#match-ids').val();
        if (matchIds != "") {
            scoreMap.LoadBetTypesList(matchIds);
            scoreMap.LoadResults($('#bet-types-live').val(), $('#bet-types-dead').val());
        }
        else {
            scoreMap.ResigterEvents();
        }
    },
    ResigterEvents: function () {
        $("#box_header").on("click", "#RefreshLeagues", function () {
            $("#loading").addClass("loading");
            var url = scoreMap.BaseLink + 'ScoreMapLeagues?currentLeagueId=' + $('#ddlLeague').val();
            $.get(url, function (contentView) {
                $("#selLeague").html(contentView);
                $("#loading").removeClass("loading");
            });
        });

        $("#dSelMatch").click(function () {
            $("#loading").addClass("loading");
            var url = scoreMap.BaseLink + 'ScoreMapMatch?leagueId=' + $('#ddlLeague').val();
            $.get(url, function (contentView) {
                $("#loading").removeClass("loading");
                $("#BoxMatchWL").replaceWith(contentView);
                var $matchItems = $(".match-item");
                var $checkAll = $("#chLCheckAll");
                scoreMap.RegisterCheckAll("#chLCheckAll", ".match-item");
                $("#closeBoxMatch").click(function () {
                    $("#BoxMatchWL").hide();
                });
                $("#dSubmitInMatch").click(function () {
                    scoreMap.LoadBetTypesList();
                });
            });
        });

        $("#dSubmit").click(function () {
            scoreMap.LoadResults();
        });
    },
    LoadBetTypesList: function (matchIds) {
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
        $('#tblMsgMatch').hide();
        $("#BoxMatchWL").hide();
        //bet-type-container
        var url = scoreMap.BaseLink + 'ScoreMapBetType';
        $.get(url, function (contentView) {
            $("#tblLiveDead").show();
            $("#bet-type-container").html(contentView);
            //bet-type-live
            scoreMap.RegisterCheckAll("#chkAllL", ".bet-type-live");
            scoreMap.RegisterCheckAll("#chkAllD", ".bet-type-deadball");
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
        $("#loadingSubmit").addClass("loading");
        var url = scoreMap.BaseLink + 'ScoreMapResult?matchIds=' + $('#match-ids').val()
            + '&liveBetTypes=' + betTypesLive
            + '&deadBallBetTypes=' + betTypesDead;
        $.get(url, function (contentView) {
            $("#loadingSubmit").removeClass("loading");
            ageMsg.Hide();
            $("#mainContent").html(contentView);
            // Events
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
            var url = scoreMap.BaseLink + 'ScoreMapResult?matchIds=' + $this.attr("match-id")
                    + '&liveBetTypes=' + scoreMap.GetIdFromSelectedCheckBox($('.bet-type-live:checked'))
                    + '&deadBallBetTypes=' + scoreMap.GetIdFromSelectedCheckBox($('.bet-type-deadball:checked'));
            var matchId = "#RefreshContent" + $this.attr("match-id");
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
});
