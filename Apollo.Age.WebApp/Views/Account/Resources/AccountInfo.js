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

            // Set posituon for agent tab
            var heightSearchBox = $('.search-box').height(),
                heightButton = $('.btn-list').height(),
                topAgentList = heightSearchBox + heightButton + 30;
            $('#agent-setting').css({ 'top': topAgentList });
        },

        getAccountInfo: function (custId, username) {
            $.cookie('searching-id', custId, { path: '/' });
            $.cookie('searching-name', username, { path: '/' });
            $.cookie('searching-ismember', ACCOUNT.dllIsMember.val().toBoolean(), { path: '/' });
            var url = "";
            if (ACCOUNT.dllIsMember.val().toBoolean()) {
                url = 'Account/MemberInfo?custid=' + custId + '&username=' + username
            }
            else {
                url = 'Account/AgentInfo?agentid=' + custId + '&username=' + username
            }

            location.href = APOLLO.resolveUrl(url);
        },

        initSearchClick: function () {
            ACCOUNT.showSectionTab("custsettings", 1); // 1: default is show setting tab

            ACCOUNT.txtUsername.autocomplete({
                source: function (request, response) {
                    var url = ACCOUNT.dllIsMember.val().toBoolean()
                        ? "Account/GetMemberByUsername?username=" + request.term.trim()
                        : "Account/GetAgentByUsername?username=" + request.term.trim();

                    $.ajax({
                        url: APOLLO.resolveUrl(url),
                        success: function (data) {
                            var found = $.map(data, function (item) {
                                return {
                                    label: item.Username,
                                    value: item.Username,
                                    id: item.Id
                                }
                            });

                            return response(found);
                        }
                    });
                },
                select: function (event, ui) {
                    ACCOUNT.getAccountInfo(ui.item.id, ui.item.value);
                },
                position: { my: "right top", at: "right bottom" }
            });

            ACCOUNT.ageDownLink.on('click', '.view-agentinfo', function () {
                var agentId = $(this).attr("data-agentid");
                var agentName = $(this).attr("data-agentname");
                $.cookie('searching-id', agentId, { path: '/' });
                $.cookie('searching-name', agentName, { path: '/' });
                $.cookie('searching-ismember', false, { path: '/' });

                var url = "Account/AgentInfo" + "?agentid=" + agentId + "&username=" + agentName;
                location.href = APOLLO.resolveUrl(url);
            });

            $('.view-agentinfo').bind('click', function () {
                var agentId = $(this).attr("data-agentid");
                var agentName = $(this).attr("data-agentname");
                $.cookie('searching-id', agentId, { path: '/' });
                $.cookie('searching-name', agentName, { path: '/' });
                $.cookie('searching-ismember', false, { path: '/' });

                var url = "Account/AgentInfo" + "?agentid=" + agentId + "&username=" + agentName;
                location.href = APOLLO.resolveUrl(url);
            });

            $(".search-button").bind('click', function () {
                ACCOUNT.getAccountInfo(-1, ACCOUNT.txtUsername.val().trim()); // -1: only search username                
            });

            $(".show-section").bind('click', function () {
                ACCOUNT.showSectionTab(this.parentNode.id, $(this).attr("data-agentid"));
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
                    agentID = $this.attr('data-agentid');
                    $this.addClass("is-active");
                    agentList = ACCOUNT.ddlAgents.find('.agent-list');

                    $.ajax({
                        url: APOLLO.resolveUrl('Account/GetAgentDownlinePath?agentId=' + agentID),
                        type: 'GET',
                        success: function (data) {
                            if (data.length > 0) {
                                agentList.html("");
                                if (data[0].Level == 0) {
                                    for (var i = data.length - 1; i > 0; i--) {
                                        agentList.append(
                                            "<li><a class='view-agentinfo' href='javascript:;'" + "data-agentid='" + data[i].Id
                                                + "'data-agentname='" + data[i].Username + "'>\\ " + data[i].Username + "</a></li>"
                                        );
                                    }
                                    agentList.append("<li class='root-agent'><i class='icon-stop'></i>" + data[0].Username + "</li>");
                                }
                                else {
                                    for (var i = data.length - 1; i >= 0; i--) {
                                        agentList.append(
                                            "<li><a class='view-agentinfo' href='javascript:;'" + "data-agentid='" + data[i].Id
                                                + "'data-agentname='" + data[i].Username + "'>\\ " + data[i].Username + "</a></li>"
                                        );
                                    }
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

            // add loading image
            $('.lnk-setting').on('click', function () {
                parent.document.getElementById('main').contentWindow.APOLLO.showBlockLoading();
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
                    ACCOUNT.bindAgentsList(agentId);
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

        bindAgentsList: function (agentId) {
            var link = APOLLO.resolveUrl("Account/AgentTreeList?agentId=" + agentId),
            iframe = document.createElement('iframe');
            iframe.frameBorder = 0;
            iframe.setAttribute("src", link);
            iframe.setAttribute("id", "frmDownlineList");

            $("#agent-treeview").html(iframe);
        },

        bindMembersList: function (agentId) {
            var link = APOLLO.resolveUrl("Account/MemberDownline?agentId=" + agentId),
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