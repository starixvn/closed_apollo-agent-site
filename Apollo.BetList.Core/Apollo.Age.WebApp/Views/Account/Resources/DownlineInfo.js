/// <reference path="../../../_GlobalResources/Js/jquery.js" />
/// <reference path="../../../_GlobalResources/Js/jquery.cookie.js" />

var DOWNLINE = function ($) {
    return {
        initAgentdownline: function () { 
            $('#treeview_list').on('click', '.view-agentinfo', function () {
                var agentId = $(this).attr("agent-id");
                var agentName = $(this).attr("agent-name");
                $.cookie('searching-id', agentId, { path: '/' });
                $.cookie('searching-name', agentName, { path: '/' });
                $.cookie('searching-ismember', false, { path: '/' });

                var url = "Account/AgentInfo" + "?agentid=" + agentId + "&username=" + agentName;
                parent.location.href = APOLLO.resolveUrl(url);
            });
        },
        initMemberdownline: function() {
            $('#members-list').on('click', '.view-memberinfo', function () {
                var memberId = $(this).attr("member-id");
                var memberName = $(this).attr("member-name");
                $.cookie('searching-id', memberId, { path: '/' });
                $.cookie('searching-name', memberName, { path: '/' });
                $.cookie('searching-ismember', true, { path: '/' });

                var url = "Account/MemberInfo" + "?custid=" + memberId + "&username=" + memberName;
                parent.location.href = APOLLO.resolveUrl(url);
            });
        }
    };
} (jQuery);
