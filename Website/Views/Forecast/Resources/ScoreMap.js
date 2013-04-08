/// <reference path="../../../_GlobalResources/Js/jquery.js" />
//// New Code
var scoreMap = {
    BaseLink: window.rootUrl + 'Forecast/',
    Init: function () {
        scoreMap.ResigterEvents();
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
                $("#content_main").append(contentView);
                var $matchItems = $(".match-item");
                var $checkAll = $("#chLCheckAll");
                scoreMap.RegisterCheckAll("#chLCheckAll", ".match-item");
                $("#dSubmitInMatch").click(function () {
                    var matchIds = scoreMap.GetIdFromSelectedCheckBox($matchItems.filter(":checked"));
                    $("#match-ids").val(matchIds);
                    $("#BoxMatchWL").hide();
                    //bet-type-container
                    var url = scoreMap.BaseLink + 'ScoreMapBetType';
                    $.get(url, function (contentView) {
                        $("#bet-type-container").html(contentView);
                        //bet-type-live
                        scoreMap.RegisterCheckAll("#chkAllL", ".bet-type-live");
                        scoreMap.RegisterCheckAll("#chkAllD", ".bet-type-deadball");
                    });
                });
            });
        });

        $("#dSubmit").click(function () {
            var url = scoreMap.BaseLink + 'ScoreMapResult?matchIds=' + $('#match-ids').val()
            + '&liveBetTypes=' + scoreMap.GetIdFromSelectedCheckBox($('.bet-type-live:checked'))
            + '&deadBallBetTypes=' + scoreMap.GetIdFromSelectedCheckBox($('.bet-type-deadball:checked'));
            //liveBetTypes
            //deadBallBetTypes
            $.get(url, function (contentView) {
                $("#mainContent").html(contentView);
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
