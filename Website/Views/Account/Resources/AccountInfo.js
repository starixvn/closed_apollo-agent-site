/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var ACCOUNT = function ($) {
    return {
        txtUsername: null,
        dllIsMember: null,
        ageDownLink: null,
        ddlAgents: null,
        enterUsernameValue: null,

        init: function () {
            ACCOUNT.txtUsername = $("#txtUsername");
            ACCOUNT.dllIsMember = $("#ddlIsMember");
            ACCOUNT.ageDownLink = $("#agentDownlineLink");
            ACCOUNT.ddlAgents = $("#ddl-agents");
            ACCOUNT.enterUsernameValue = $("#enterUsernameValue");
        },

        getAccountInfo: function (username) {
            location.href = APOLLO.resolveUrl('Account/AccountInfo?userName='
            + username + '&isMember=' + ACCOUNT.dllIsMember.val());
        },

        initSearchClick: function () {
            ACCOUNT.showSectionTab("custsettings", 1);

            ACCOUNT.txtUsername.autocomplete({
                source: function (request, response) {
                    var url = "Account/GetSuggestValues" +
                    "?term=" + request.term +
                    "&isMember=" + (ACCOUNT.dllIsMember.val() == 1);

                    $.ajax({
                        url: APOLLO.resolveUrl(url),
                        success: function (data) {
                            return response(data);
                        }
                    });
                },
                select: function (event, ui) {
                    ACCOUNT.getAccountInfo(ui.item.value);
                },
                position: { my: "right top", at: "right bottom" }
            });


            $(".search-button").bind('click', function () {
                ACCOUNT.getAccountInfo(ACCOUNT.txtUsername.val());
            });

            $(".show-section").bind('click', function () {
                ACCOUNT.showSectionTab(this.parentNode.id, $(this).attr("agent-id"));
            });

            ACCOUNT.txtUsername.bind('click', function () {
                if (ACCOUNT.txtUsername.val() == ACCOUNT.enterUsernameValue.val()) {
                    ACCOUNT.txtUsername.val('');
                }
            });
            ACCOUNT.txtUsername.bind('keydown', function () {
                if (ACCOUNT.txtUsername.val() == ACCOUNT.enterUsernameValue.val()) {
                    ACCOUNT.txtUsername.val('');
                }
            });
            ACCOUNT.txtUsername.bind('blur', function () {
                if (ACCOUNT.txtUsername.val().length == 0) {
                    ACCOUNT.txtUsername.val(ACCOUNT.enterUsernameValue.val());
                }
            });

            ACCOUNT.ageDownLink.click(function (event) {
                if (ACCOUNT.ageDownLink.hasClass("is-active")) {
                    ACCOUNT.ageDownLink.removeClass("is-active");
                    ACCOUNT.ddlAgents.fadeOut('fast');
                }
                else {
                    var $this = $(this);
                    agentID = $this.attr('aid');
                    $this.addClass("is-active");
                    agentList = ACCOUNT.ddlAgents.find('.agent-list');

                    $.ajax({
                        url: APOLLO.resolveUrl('Account/GetAgentDownlinePath?agentId=' + agentID),
                        type: 'GET',
                        success: function (data) {
                            if (data.length > 0) {
                                agentList.html("<li class='root-agent'>" + data[0].Username + "</li>");
                                for (var i = 1; i < data.length; ++i) {
                                    agentList.append(
                                        "<li>" +
                                            "<a href='" + APOLLO.resolveUrl("Account/AccountInfo" + "?isMember=0" +
                                                "&custId=" + data[i].Id +
                                                "&userName=" + data[i].Username) + "'>\\ " + data[i].Username + "</a>" +
                                        "</li>"
                                    );
                                }
                            }

                        }
                    });

                    ACCOUNT.ddlAgents.fadeIn('fast');
                    event.stopPropagation();
                }
            });

            $('body, html').on('click', function () {
                if (ACCOUNT.ageDownLink.hasClass("is-active")) {
                    ACCOUNT.ageDownLink.removeClass("is-active");
                    ACCOUNT.ddlAgents.fadeOut('fast');
                }
            });
        },

        showSectionTab: function (tabId, agentId) {
            switch (tabId) {
                case "custsettings":
                    $("#custsettings").addClass("current");
                    $("#agentsTab").removeClass();
                    $("#membersTab").removeClass();
                    $("#custsettings-content").show();
                    $("#agentsTab-content").hide();
                    $("#membersTab-content").hide();
                    break;
                case "agentsTab":
                    $("#agentsTab").addClass("current");
                    $("#custsettings").removeClass();
                    $("#membersTab").removeClass();
                    $("#custsettings-content").hide();
                    $("#agentsTab-content").show();
                    $("#membersTab-content").hide();
                    break;
                case "membersTab":
                    $("#membersTab").addClass("current");
                    $("#custsettings").removeClass();
                    $("#agentsTab").removeClass();
                    $("#custsettings-content").hide();
                    $("#agentsTab-content").hide();
                    $("#membersTab-content").show();
                    ACCOUNT.bindMembersList(agentId);
                    break;
            }
        },
        bindMembersList: function (agentId) {
            var link = APOLLO.resolveUrl("Account/MemberDownline?custId=" + agentId),
            iframe = document.createElement('iframe');

            iframe.frameBorder = 0;
            iframe.setAttribute("src", link);

            $("#membersTab-content").html(iframe);
        }
    }
} (jQuery);

$(function () {
    ACCOUNT.init();
    ACCOUNT.initSearchClick();
});