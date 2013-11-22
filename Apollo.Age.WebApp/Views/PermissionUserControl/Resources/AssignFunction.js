/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var AssignFunction = function ($) {

    return {
        grid: null,
        message: null,
        messageApplyDownline: null,
        isMaster: null,
        agentUserId: null,
        oldValue: null,
        agentId: null,
        username: null,
        isDirectAgent: null,
        parentAgentId: null,
        isApplyDownLine: null,
        unCheckReadValue: "",
        unCheckHostValue: "",

        init: function () {
            AssignFunction.grid = $('.tblRpt:first');
            AssignFunction.isMaster = $('#IsMaster').val();
            AssignFunction.agentUserId = $('#AgentUserId').val();
            AssignFunction.agentId = $('#AgentId').val();
            AssignFunction.username = $('#AgentUsername').val();
            AssignFunction.isDirectAgent = $('#IsDirectAgent').val();
            AssignFunction.parentAgentId = $('#ParentAgentId').val();
            AssignFunction.messageApplyDownline = Language.APPLYDOWNLINE;

            if (AssignFunction.isMaster == "1") {
                AssignFunction.message = Language.CONFIRM_AGENT;
            }
            else {
                AssignFunction.message = Language.CONFIRM_SHADOW;
            }

            var funcRead = "";
            var funcHost = "";

            AssignFunction.grid.find('[type=checkbox][name=funcRead]').each(function () {
                var $this = $(this),
					thisIsChecked = $this.is(':checked'),
					row = $this.parents('tr');

                if (thisIsChecked) {
                    funcRead += $this.val() + ",";

                    var itemHost = row.find('[type=checkbox][name=funcHost]');
                    if (itemHost.is(':checked')) {
                        funcHost += itemHost.val() + ",";
                    }
                    else {
                        if (itemHost.val() != undefined) {
                            AssignFunction.unCheckHostValue += itemHost.val() + ",";
                        }
                    }
                }
                else {
                    AssignFunction.unCheckReadValue += $this.val() + ",";
                }
            });

            AssignFunction.oldValue = funcRead + funcHost;
        },

        assignUserPermision: function () {
            var funcRead = "";
            var funcHost = "";

            AssignFunction.grid.find('[type=checkbox][name=funcRead]').each(function () {
                var $this = $(this),
					thisIsChecked = $this.is(':checked'),
					row = $this.parents('tr');

                if (thisIsChecked) {
                    funcRead += $this.val() + ",";

                    var itemHost = row.find('[type=checkbox][name=funcHost]');
                    if (itemHost.is(':checked')) {
                        funcHost += itemHost.val() + ",";
                    }
                }
            });

            if (AssignFunction.oldValue == (funcRead + funcHost)) {
                APOLLO.showMessage(Language.NOCHANGE, 'success');
                return;
            }

            AssignFunction.isApplyDownLine = AssignFunction.checkHasNewPermission(AssignFunction.unCheckReadValue, AssignFunction.unCheckHostValue, funcRead, funcHost);

            if (AssignFunction.isApplyDownLine && AssignFunction.isMaster == "1") {
                var confirmMess = $("#confirm-mess");
                confirmMess.dialog({
                    resizable: false,
                    height: 140,
                    width: 500,
                    modal: true,
                    title: Language.UPDATE_TITLE,
                    buttons: {
                        "Yes": function () {
                            AssignFunction.updatePermission(funcRead, funcHost, 1);
                            $(this).dialog("close");
                        },
                        "No": function () {
                            AssignFunction.updatePermission(funcRead, funcHost, 0);
                            $(this).dialog("close");
                        }
                    }
                });               
            }
            else {
                if (confirm(AssignFunction.message)) {
                    AssignFunction.updatePermission(funcRead, funcHost, -1);
                }
            }
        },

        updatePermission: function (funcRead, funcHost, isApplyDownLine) {
            var url = APOLLO.resolveUrl("~/PermissionUserControl/AssignPermissionControl");

            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'JSON',
                data: { FuncRead: funcRead, FuncHost: funcHost, AgentUserId: AssignFunction.agentUserId, IsApplyDownLine: isApplyDownLine },
                success: function (res) {
                    if (res.ErrCode == "-1") {
                        window.location.href = APOLLO.resolveUrl("~/Error/AccessDenied");
                    } else if (res.ErrCode != "0") {
                        APOLLO.showMessage(res.ErrMsg, 'error');
                    }
                    else {
                        APOLLO.showMessage(Language.UPDATED_SUCCESS, 'success');
                    }

                    AssignFunction.oldValue = funcRead + funcHost;

                    $('#noty_top_layout_container').css({ width: "500px", left: "5px" });

                    setTimeout(function () {
                        if (AssignFunction.isMaster == "1") {
                            var url = AssignFunction.isDirectAgent == "1" ? "Account/AgentList" : "Account/IndirectAgentList?parentAgentId=" + AssignFunction.parentAgentId + "&agentid=" + AssignFunction.agentId + "";

                            location.href = APOLLO.resolveUrl(url);
                        }
                        else {
                            location.href = APOLLO.resolveUrl("~/PermissionUserControl/AgentUserList?agentId=" + AssignFunction.agentId + "&&username=" + AssignFunction.username + "");
                        }
                    }, 3000);
                },
                error: function (xhr, status, err) {
                    location.href = APOLLO.resolveUrl("~/Error/AccessDenied");
                }
            });
        },

        checkHasNewPermission: function (unCheckReadValue, unCheckHostValue, funcRead, funcHost) {
            var hasNew = false;
            var arrUnCheckRead = unCheckReadValue.split(',');
            var arrUnCheckHost = unCheckHostValue.split(',');
            var arrFuncRead = funcRead.split(',');
            var arrFuncHost = funcHost.split(',');

            for (var i = 0; i < arrFuncRead.length; i++) {
                var functionId = arrFuncRead[i];

                if (AssignFunction.findInArray(arrUnCheckRead, functionId)) {
                    return true;
                }
            }

            for (var i = 0; i < arrFuncHost.length; i++) {
                var functionId = arrFuncHost[i];

                if (AssignFunction.findInArray(arrUnCheckHost, functionId)) {
                    return true;
                }
            }

            return hasNew;

        },

        findInArray: function (arr, value) {
            if (value != null && value != "") {
                for (var i = 0; i < arr.length; i++) {
                    var item = arr[i];
                    if (item != "" && item == value) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
} (jQuery);

$(document).ready(function () {

    AssignFunction.init();

    AssignFunction.grid.find('[type=checkbox]').change(function () {
        var $this = $(this),
			thisIsChecked = $this.is(':checked'),
			row = $this.parents('tr');

        if ($this.attr('name') == 'funcHost') {
            var funcRead = row.find('[type=checkbox][name=funcRead]');
            funcRead.prop('checked', true);
        } else { // funcRead
            var funcHost = row.find('[type=checkbox][name=funcHost]');
            funcHost.prop('checked', false);
        }
    });

    $('#btnSubmit').bind('click', function () {
        AssignFunction.assignUserPermision();
    });

    $('#btnCancel').bind('click', function () {
        window.history.go(-1);
    });
});