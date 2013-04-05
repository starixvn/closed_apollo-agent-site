var ACCOUNT =
{
    txtUsername: null,
    dllIsMember: null,

    init: function () {
        ACCOUNT.txtUsername = $("#txtUsername");
        ACCOUNT.dllIsMember = $("#ddlIsMember");
    },

    getAutoCompleteUrl: function () {

        return APOLLO.resolveUrl("Account/GetSuggestValues" +
            "?query=" + ACCOUNT.txtUsername.val() +
            "&isMember=" + ACCOUNT.dllIsMember.val());
    },

    initSearchClick: function () {
        ACCOUNT.showSectionTab("custsettings", 1);

        ACCOUNT.txtUsername.autocomplete({
            source: function (request, response) {
                $.getJSON(
                    APOLLO.resolveUrl("Account/GetSuggestValues?isMember=" + ACCOUNT.dllIsMember.val()), //url
                    {query: request.term }, //param
                    function (result) {
                        response($.map(result, function (item) {
                            return item.Username;
                        }));
                    });
            }
        });

        $(".ui-menu-item").bind('click', function () {
            alert("AAA");
        });

        $(".ui-menu-item").bind('keydown', function () {
            alert("bbb");
        });

        $(".search-button").bind('click', function () {
            location.href = parent.baseURL + 'Account/AccountInfo?userName=' + $("#txtUsername").val() + '&isMember=' + $("#ddlIsMember").val();
        });

        $(".show-section").bind('click', function () {
            ACCOUNT.showSectionTab(this.parentNode.id, $(this).attr("agent-id"));
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

        //RegisterStartUp('initHeader()');
        //initAutoComplete('txtUsername', 'Handlers/QueryUserName.ashx');
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