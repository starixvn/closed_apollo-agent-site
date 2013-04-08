var ACCOUNT =
{
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
            }
        });


        $(".search-button").bind('click', function () {
            ACCOUNT.getAccountInfo(ACCOUNT.txtUsername.val());
        });

        $(".show-section").bind('click', function () {
            ACCOUNT.showSectionTab(this.parentNode.id, $(this).attr("agent-id"));
        });

        ACCOUNT.txtUsername.bind('click', function () {
            if (ACCOUNT.txtUsername.val() == ACCOUNT.enterUsernameValue.val()) {
                ACCOUNT.txtUsername.val().addClass = 'text_f';
                ACCOUNT.txtUsername.val = '';
            }
        });
        ACCOUNT.txtUsername.bind('keydown', function () {
            if (ACCOUNT.txtUsername.val() == ACCOUNT.enterUsernameValue.val()) {
                ACCOUNT.txtUsername.val().addClass = 'text_f';
                ACCOUNT.txtUsername.val = '';
            }
        });
        ACCOUNT.txtUsername.bind('blur', function () {
            if (length(ACCOUNT.txtUsername.val()) == 0) {
                ACCOUNT.txtUsername.val().addClass = 'text_f1';
                ACCOUNT.txtUsername.val = ACCOUNT.enterUsernameValue.val();
            }
        });

        ACCOUNT.ageDownLink.click(function (event) {
            var $this = $(this),
                agentID = $this.attr('aid'),
                agentList = ACCOUNT.ddlAgents.find('.agent-list');

            ajax.PostJSON('../_AccountInfo/Handlers/GetAgentPathList.ashx', 'agentId=' + agentID, function (result) {
                var data = result.Agents;

                agentList.html("<li class='root-agent'>" + data[0][1] + "</li>");

                for (var i = 1; i < data.length; ++i) {
                    agentList.append(
                            "<li>" +
                                "<a href='../_AccountInfo/AccountInfo.aspx" +
                                    "?IsMember=0" +
                                    "&CustId=" + data[i][0] +
                                    "&txtUsername=" + data[i][1] + "'>\\ " + data[i][1] + "</a>" +
                            "</li>"
                        );
                }
            });

            ACCOUNT.ddlAgents.fadeIn('fast');
            event.stopPropagation();
        });

        $('body, html').on('click', function () {
            if (ACCOUNT.ageDownLink.length > 0) {
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
        var link = "../_AccountInfo/DownlineMemberList.aspx?custid=" + agentId;
        var iframe = document.createElement('iframe');
        iframe.frameBorder = 0;
        iframe.width = "100%";
        iframe.height = "100%";
        iframe.id = "randomid";
        iframe.setAttribute("src", link);

        var contentId = document.getElementById("iframe-content");
        contentId.innerHTML = "";
        contentId.appendChild(iframe);
    }
};

$(function () {
    ACCOUNT.init();
    ACCOUNT.initSearchClick();
});