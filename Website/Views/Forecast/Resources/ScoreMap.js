/// <reference path="../../../_GlobalResources/Js/jquery.js" />
//// New Code
var scoreMap = {
    BaseLink: APOLLO.resolveUrl('Forecast/'),
    ViewPartial: false,
    Init: function () {
        //// Cached jQuery Objects
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
            var url = scoreMap.BaseLink + 'ScoreMapLeagues?currentLeagueId=' + $('#ddlLeague').val();
            $.get(url, function (contentView) {
                $("#selLeague").html(contentView);
            });
        });

        $("#dSelMatch").click(function () {
            var url = scoreMap.BaseLink + 'ScoreMapMatch?leagueId=' + $('#ddlLeague').val();
            $.get(url, function (contentView) {
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
            scoreMap.RegisterCheckAll("#chkAllL", ".bet-type-live", true);
            scoreMap.RegisterCheckAll("#chkAllD", ".bet-type-deadball", true);
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
        var url = scoreMap.BaseLink + 'ScoreMapResult?matchIds=' + $('#match-ids').val()
            + '&liveBetTypes=' + betTypesLive
            + '&deadBallBetTypes=' + betTypesDead;
        $.get(url, function (contentView) {
            //// Hide Loading and Hide Message
            ageMsg.Hide();
            $("#mainContent").html(contentView);
            scoreMap.RegisterEventsForResultMatch($("#content_main"));
        });
    },
    RegisterEventsForResultMatch: function (resultContainer) {
        resultContainer.find(".collapse-expand").click(function () {
            var $this = $(this);
            if ($this.hasClass("lnkGroup")) {
                $this.removeClass("lnkGroup").addClass("lnkGroupClick");
                $this.parent().parent().find(".title-scoremap").css({ "border-bottom": "1px solid #ccc" });
            }
            else {
                $this.removeClass("lnkGroupClick").addClass("lnkGroup");
                $this.parent().parent().find(".title-scoremap").css({ "border-bottom": "none" });
            }
            $this.parent().parent().find(".scoreMap").toggle();
        });

        resultContainer.find(".refresh-scoremap").click(function () {
            var $this = $(this);
            if ($this.hasClass("lnkRefresh")) {
                $this.removeClass("lnkRefresh").addClass("lnkRefreshClick");
            }
            else {
                $this.removeClass("lnkRefreshClick").addClass("lnkRefresh");
            }
            var attrMatchId = $this.attr("match-id");
            var url = scoreMap.BaseLink + 'ScoreMapResult?matchIds=' + attrMatchId
                    + '&liveBetTypes=' + scoreMap.GetIdFromSelectedCheckBox($('.bet-type-live:checked'))
                    + '&deadBallBetTypes=' + scoreMap.GetIdFromSelectedCheckBox($('.bet-type-deadball:checked'));
            var matchId = "#RefreshContent" + attrMatchId;
            //RefreshContent
            $.get(url, function (contentView) {
                $this.parent().parent().replaceWith(contentView);
                scoreMap.RegisterEventsForResultMatch($(matchId));
            });
        });
        //.hover-highlight
        resultContainer.find(".scoreMap tbody .csm")
        .mouseover(function () {
            var $this = $(this);
            $this.parent().parent().parent().find("th." + $this.attr("class-hover")).addClass("hover-highlight");
        })
        .mouseout(function () {
            var $this = $(this);
            $this.parent().parent().parent().find("th." + $this.attr("class-hover")).removeClass("hover-highlight");
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
    RegisterCheckAll: function (checkAllId, itemClass, checkAll) {
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

        if (checkAll && scoreMap.ViewPartial == false) {
            $checkAll.trigger("click");
        }
    }
};

$(function () {
    scoreMap.Init();
}).ajaxStart($.blockUI).ajaxStop($.unblockUI);
