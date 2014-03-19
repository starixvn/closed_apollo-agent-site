/// <reference path="../../../_GlobalResources/Js/jquery.js" />
/// <reference path="../../../_GlobalResources/Js/jquery.cookie.js" />
/*global APOLLO*/
/*exported DOWNLINE */
'use strict';
var DOWNLINE = (function ($) {
    return {
        initAgentdownline: function () { 
            $('#treeview_list').on('click', '.view-agentinfo', function () {
                var agentId = $(this).attr("data-agentid");
                var agentName = $(this).attr("data-agentname");
                $.cookie('searching-id', agentId, { path: '/' });
                $.cookie('searching-name', agentName, { path: '/' });
                $.cookie('searching-ismember', false, { path: '/' });

                var url = "Account/AgentInfo" + "?agentid=" + agentId + "&username=" + agentName;
                parent.location.href = APOLLO.resolveUrl(url);
            });
        },
        initMemberdownline: function() {
            $('#members-list').on('click', '.view-memberinfo', function () {
                var memberId = $(this).attr("data-memberid");
                var memberName = $(this).attr("data-membername");
                $.cookie('searching-id', memberId, { path: '/' });
                $.cookie('searching-name', memberName, { path: '/' });
                $.cookie('searching-ismember', true, { path: '/' });

                var url = "Account/MemberInfo" + "?custid=" + memberId + "&username=" + memberName;
                parent.location.href = APOLLO.resolveUrl(url);
            });
        }
    };
} (jQuery));
