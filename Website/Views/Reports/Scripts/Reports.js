var REPORTS = {}; // Global namespace for common functions

if (typeof jQuery != 'undefined') {
    // Functions in this namespace require jquery and jquery-ui
    REPORTS = function ($) {
        return {
            init: function () {
                $("#AgentOutstanding").bind('click', function () {
                    $.get('agentoutstanding', { 'agentId': $("#AgentOutstanding").attr("agentid") })
                        .done(function (data) {
                            $('#divDetail').html(data);
                        });
                });

                $("#MemberOutstanding").bind('click', function () {
                    $.get('memberoutstanding', { 'agentId': $("#MemberOutstanding").attr("agentid") })
                        .done(function (data) {
                            $('#divDetail').html(data);
                        });
                });
            },
            ViewAgent: function (agentId) {
                location.href = "outstanding?agentid=" + agentId;
            },
        };
    } (jQuery);
}
$(document).ready(function () {
    REPORTS.init();
});