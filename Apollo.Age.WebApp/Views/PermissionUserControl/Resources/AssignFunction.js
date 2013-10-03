/// <reference path="../../../_GlobalResources/Js/jquery.js" />

var AssignFunction = function ($) {

    return {
        grid: null,
        message: null,
        isMaster: null,
        agentUserId: null,

        init: function () {
            AssignFunction.grid = $('.tblRpt:first');
            AssignFunction.isMaster = $('#IsMaster').val();
            AssignFunction.agentUserId = $('#AgentUserId').val();

            if (AssignFunction.isMaster == "1") {
                AssignFunction.message = Language.CONFIRM_AGENT;
            }
            else {
                AssignFunction.message = Language.CONFIRM_SHADOW;
            }
        },

        assignUserPermision: function () {
            var funcRead = "";
            var funcHost = "";
            var url = APOLLO.resolveUrl("~/PermissionUserControl/AssignUserPermission");

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

            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'JSON',
                data: { FuncRead: funcRead, FuncHost: funcHost, AgentUserId: AssignFunction.agentUserId },
                success: function (res) {

                },
                error: function (xhr, status, err) {
                }
            });
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
        if (confirm(AssignFunction.message)) {
            AssignFunction.assignUserPermision();
        }
    });

    $('#btnCancel').bind('click', function () {
        window.history.go(-1);
    });
});