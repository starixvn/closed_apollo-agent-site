var ACCOUNT =
{
    txtUsername: null,
    dllIsMember: null,

    init: function () {
        ACCOUNT.txtUsername = $("#txtUsername");
        ACCOUNT.dllIsMember = $("#ddlIsMember");
    },

    initSearchClick: function () {
        ACCOUNT.ShowSectionTab("custsettings", 1);

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
            }
        });

        $(".ui-corner-all").bind('click', function () {
            location.href = getAccountInfoUrl();
        });

        $("#txtUsername").keypress(function (e) {
            if (e.keyCode == '13') {
                location.href = getAccountInfoUrl();
            }
        });

        $(".search-button").bind('click', function () {
            location.href = getAccountInfoUrl();
        });

        $(".show-section").bind('click', function () {
            ACCOUNT.ShowSectionTab(this.parentNode.id, $(this).attr("agent-id"));
        });

        $("#txtUsername").bind('click', function () {
            if (document.getElementById('txtUsername').value == $("#enterUsernameValue").val()) {
                document.getElementById('txtUsername').className = 'text_f';
                document.getElementById('txtUsername').value = '';
            }
        });
        $("#txtUsername").bind('keydown', function () {
            if (document.getElementById('txtUsername').value == $("#enterUsernameValue").val()) {
                document.getElementById('txtUsername').className = 'text_f';
                document.getElementById('txtUsername').value = '';
            }
        });
        $("#txtUsername").bind('blur', function () {
            if (document.getElementById('txtUsername').value == '') {
                document.getElementById('txtUsername').className = 'text_f1';
                document.getElementById('txtUsername').value = $("#enterUsernameValue").val();
            }
        });
    },
    getAccountInfoUrl: function () {
        return APOLLO.resolveUrl('Account/AccountInfo?userName=' 
        + ACCOUNT.txtUsername.val() + '&isMember=' + ACCOUNT.dllIsMember.val());
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
                ACCOUNT.BindMembersList(agentId);
                break;
        }
    },
    BindMembersList: function (agentId) {
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