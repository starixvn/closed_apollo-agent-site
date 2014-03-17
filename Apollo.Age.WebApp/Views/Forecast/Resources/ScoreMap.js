/// <reference path="../../../_GlobalResources/Js/jquery.js" />
/*global DEFAULT_OBJECT*/
/*global DEFAULT_LAYOUT*/
/*global APOLLO*/

'use strict';
if (typeof jQuery !== 'undefined') {
    var SCOREMAP = (function ($) {
        return {
            baseLink: APOLLO.resolveUrl('Forecast/'),
            viewPartial: false,
            init: function () {
                //// Cached jQuery Objects
                var matchIds = $('#match-ids').val();
                if (matchIds !== "") {
                    $("#header_main").hide();
                    SCOREMAP.viewPartial = true;
                    SCOREMAP.loadBetTypesList(matchIds);
                    SCOREMAP.loadResults($('#bet-types-live').val(), $('#bet-types-dead').val());
                }
                else {
                    SCOREMAP.resigterEvents();
                }
            },
            resigterEvents: function () {
                $("#box_header").on("click", "#RefreshLeagues", function () {
                    var url = SCOREMAP.baseLink + 'ScoreMapLeagues?currentLeagueId=' + $('#ddlLeague').val();
                    if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                        url += '&mainShareHolderId=' + DEFAULT_LAYOUT.currentMainShareHolderId;
                    }
                    $.get(url, function (contentView) {
                        $("#selLeague").html(contentView);
                    });
                });

                $("#dSelMatch").click(function () {
                    var url = SCOREMAP.baseLink + 'ScoreMapMatch?leagueId=' + $('#ddlLeague').val();
                    if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                        url += '&mainShareHolderId=' + DEFAULT_LAYOUT.currentMainShareHolderId;
                    }
                    $.get(url, function (contentView) {
                        $("#BoxMatchWL").replaceWith(contentView);
                        if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                            $("#BoxMatchWL").css("top", "124px");
                        }
                        SCOREMAP.registerCheckAll("#chLCheckAll", ".match-item");
                        $("#closeBoxMatch").click(function () {
                            $("#BoxMatchWL").hide();
                        });
                        $("#dSubmitInMatch").click(function () {
                            SCOREMAP.loadBetTypesList();
                        });
                    });
                });

            },
            loadBetTypesList: function (matchIds) {
                $('#tblMsgMatch').hide();
                if (matchIds) {
                    matchIds = $('#match-ids').val();
                }
                else {
                    matchIds = SCOREMAP.getIdFromSelectedCheckBox($(".match-item:checked"));
                    $("#match-ids").val(matchIds);

                }
                if (matchIds === "") {
                    $('#msgMatchErr').text(DEFAULT_OBJECT.plsChooseMatch);
                    $('#tblMsgMatch').show();
                    return;
                }
                $("#BoxMatchWL").hide();
                var url = SCOREMAP.baseLink + 'ScoreMapBetType';
                //// If Show In PopUp then add BetType
                if (SCOREMAP.viewPartial) {
                    var btLive = $('#bet-types-live').val().split(',')[0] || "";
                    var btDead = $('#bet-types-dead').val().split(',')[0] || "";
                    url += "?liveBetType=" + btLive
            + "&deadBetType=" + btDead;
                }
                $.get(url, function (contentView) {
                    $("#tblLiveDead").show();
                    $("#bet-type-container").html(contentView);
                    //bet-type-live
                    SCOREMAP.registerCheckAll("#chkAllL", ".bet-type-live", true);
                    SCOREMAP.registerCheckAll("#chkAllD", ".bet-type-deadball", true);
                    $("#dSubmit").click(function () {
                        SCOREMAP.loadResults();
                    });
                });
            },
            loadResults: function (betTypesLive, betTypesDead) {
                if (!betTypesLive && !betTypesDead) {
                    betTypesLive = SCOREMAP.getIdFromSelectedCheckBox($('.bet-type-live:checked'));
                    betTypesDead = SCOREMAP.getIdFromSelectedCheckBox($('.bet-type-deadball:checked'));
                }

                if (betTypesLive === "" && betTypesDead === "") {
                	document.getElementById('spmsgerr').innerHTML = DEFAULT_OBJECT.plsChooseBetType;
                	document.getElementById('diverrmsg').style.display = IE ? 'block' : 'table';
                    $("#mainContent").html('');
                    return;
                }
                var url = SCOREMAP.baseLink + 'ScoreMapResult?matchIds=' + $('#match-ids').val()
            + '&liveBetTypes=' + betTypesLive
            + '&deadBallBetTypes=' + betTypesDead;
                if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                    url += '&mainShareHolderId=' + DEFAULT_LAYOUT.currentMainShareHolderId;
                }
                $.get(url, function (contentView) {
                    //// Hide Loading and Hide Message
                	document.getElementById('diverrmsg').style.display = 'none';
                    $("#mainContent").html(contentView);
                    SCOREMAP.registerEventsForResultMatch($("#content_main"));
                });
            },
            registerEventsForResultMatch: function (resultContainer) {
                resultContainer.find(".collapse-expand").click(function () {
                    var $this = $(this);
                    if ($this.hasClass("lnkGroup")) {
                        $this.removeClass("lnkGroup").addClass("lnkGroupClick");
                        $this.parent().parent().find(".title-scoremap").css({ "border-bottom": "1px solid #363636" });
                    }
                    else {
                        $this.removeClass("lnkGroupClick").addClass("lnkGroup");
                        $this.parent().parent().find(".title-scoremap").css({ "border-bottom": "none" });
                    }
                    $this.parent().parent().find(".scoreMap").toggle();
                });

                resultContainer.find(".refresh-scoremap").click(function () {
                    var $this = $(this);
                    var attrMatchId = $this.attr("match-id");
                    var url = SCOREMAP.baseLink + 'ScoreMapResult?matchIds=' + attrMatchId
                    + '&liveBetTypes=' + SCOREMAP.getIdFromSelectedCheckBox($('.bet-type-live:checked'))
                    + '&deadBallBetTypes=' + SCOREMAP.getIdFromSelectedCheckBox($('.bet-type-deadball:checked'));
                    if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
                        url += '&mainShareHolderId=' + DEFAULT_LAYOUT.currentMainShareHolderId;
                    }
                    var matchId = "#RefreshContent" + attrMatchId;
                    //RefreshContent
                    $.get(url, function (contentView) {
                        $this.parent().parent().replaceWith(contentView);
                        SCOREMAP.registerEventsForResultMatch($(matchId));
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
            getIdFromSelectedCheckBox: function (selectedCheckBox) {
                var length = selectedCheckBox.length;
                var ids = "";
                for (var i = 0; i < length; i++) {
                    ids += selectedCheckBox[i].value + ",";
                }
                return ids;
            },
            registerCheckAll: function (checkAllId, itemClass, checkAll) {
                var $matchItems = $(itemClass);
                var $checkAll = $(checkAllId);
                $checkAll.click(function () {
                    var $this = $(this);
                    if ($this.prop("checked") === true) {
                        $matchItems.prop("checked", true);
                    }
                    else {
                        $matchItems.prop("checked", false);
                    }
                });

                $matchItems.click(function () {
                    var $matchItem = $(this);
                    if ($matchItem.prop("checked") === false) {
                        $checkAll.prop("checked", false);
                    }
                    else {
                        if ($matchItems.not(":checked").length === 0) {
                            $checkAll.prop("checked", true);
                        }
                    }
                });

                if (checkAll && SCOREMAP.viewPartial === false) {
                    $checkAll.trigger("click");
                }
            }
        };
    } (jQuery));

    $(function () {
        if (DEFAULT_LAYOUT.currentMainShareHolderId > 0) {
            $("#ddlMainShareHolder").change(function () {
                var $this = $(this);
                var markIndex = document.location.href.indexOf('?');
                var link = document.location.href;
                if (markIndex > 0) {
                    link = document.location.href.substring(0, markIndex);
                }

                document.location.href = link + "?mainShareHolderId=" + $this.val();
            });

            $(".selectLeague label").width($("#main-shareholder-box label").width());
        }
        SCOREMAP.init();
    }).ajaxStart($.blockUI).ajaxStop($.unblockUI);
}
