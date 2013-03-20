// require: jquery, jquery-ui

var ExchRate = function ($) {
    return {
        custId: null,

        dialogEdit: null,
        editorWrp: null,

        editorCurr: null,
        editorDesc: null,
        editorMult: null,
        editorBase: null,
        editorMinb: null,
        editorMinbl: null,
        btnAddNew: null,
        btnEdits: null,

        fillEditor: function (multiply, baseRate, minBet, minBetLive, description) {
            ExchRate.editorDesc.val(description);
            ExchRate.editorMult.val(multiply);
            ExchRate.editorBase.val(baseRate);
            ExchRate.editorMinb.val(minBet);
            ExchRate.editorMinbl.val(minBetLive);
        },

        emptyEditor: function () {
            ExchRate.fillEditor('', '', '', '', '');
        },

        init: function () {
            ExchRate.custId = $('#CustId').val();

            ExchRate.dialogEdit = $('#dialog-edit');
            ExchRate.editorWrp = $('#editor-wrp');

            ExchRate.editorCurr = $("#editor-curr");
            ExchRate.editorDesc = $("#editor-desc");
            ExchRate.editorMult = $("#editor-mult");
            ExchRate.editorBase = $("#editor-base");
            ExchRate.editorMinb = $("#editor-minb");
            ExchRate.editorMinbl = $("#editor-minbl");

            ExchRate.btnAddNew = $('#btn-add-new').click(function () {
                ExchRate.emptyEditor();
                ExchRate.dialogEdit.find('th:first, td:first').show();

                APOLLO.openDialog(ExchRate.dialogEdit, "Add New Currency",
                {
                    buttons: [
                    {
                        text: "Submit",
                        click: function () {
                            var $this = $(this),
                                buttons = $this.next('.ui-dialog-buttonpane').find('.ui-button'),
                                valid = true;

                            ExchRate.editorWrp.find('input:not(#editor-desc)').each(function () {
                                if (valid) {
                                    var $this = $(this);

                                    $this.val($.trim($this.val()));
                                    if ($this.val().length == 0) {
                                        $this.addClass('input-validation-error');
                                        valid = false;
                                    } else {
                                        $this.removeClass('input-validation-error');
                                    }
                                }
                            });

                            if (valid) {
                                var url = "../API/InsertExchangeRate.ashx?custId=" + ExchRate.custId +
                                    "&currency=" + ExchRate.editorCurr.val() +
                                    "&description=" + ExchRate.editorDesc.val() +
                                    "&multipliedBy=" + ExchRate.editorMult.val() +
                                    "&baseRate=" + ExchRate.editorBase.val() +
                                    "&minBet=" + ExchRate.editorMinb.val() +
                                    "&minBetLive=" + ExchRate.editorMinbl.val();

                                $.ajax({
                                    url: url,
                                    dataType: "json",
                                    beforeSend: function () {
                                        $this.addClass('loading');
                                        buttons.attr('disabled', true).addClass("ui-state-disabled");
                                    }
                                }).done(function (data) {
                                    if (data.errCode == 0) {
                                        alert('Exchange Rate added successfully!');
                                        window.location.reload();
                                    } else {
                                        alert(data.errMsg);
                                    }
                                }).fail(function (jqXHR, textStatus, errorThrown) {
                                    APOLLO.handleAjaxError(jqXHR, textStatus, errorThrown);
                                }).always(function () {
                                    $this.removeClass('loading');
                                    buttons.attr('disabled', false).removeClass("ui-state-disabled"); ;
                                });
                            }
                        }
                    },
                    { text: "Cancel", click: function () { $(this).dialog("close"); } }]
                });

                return false;
            });

            ExchRate.btnEdits = $('.btn-edit').click(function () {
                var $this = $(this),
                    exchId = $this.attr('eid'),
                    row = $this.parents('tr'),
                    currency = row.children('td:eq(0)').text(),
                    multiply = row.children('td:eq(1)').text(),
                    baseRate = row.children('td:eq(2)').text(),
                    minBet = row.children('td:eq(3)').text(),
                    minBetLive = row.children('td:eq(4)').text(),
                    description = row.children('td:eq(5)').text();

                ExchRate.dialogEdit.find('th:first, td:first').hide();

                ExchRate.fillEditor(multiply, baseRate, minBet, minBetLive, description);

                APOLLO.openDialog(ExchRate.dialogEdit, "Edit Currency: " + currency,
                {
                    buttons: [
                    {
                        text: "Submit",
                        click: function () {
                            var $this = $(this),
                                buttons = $this.next('.ui-dialog-buttonpane').find('.ui-button'),
                                valid = true;

                            ExchRate.editorWrp.find('input:not(#editor-curr,#editor-desc)').each(function () {
                                if (valid) {
                                    var $this = $(this);

                                    $this.val($.trim($this.val()));
                                    if ($this.val().length == 0) {
                                        $this.addClass('input-validation-error');
                                        valid = false;
                                    } else {
                                        $this.removeClass('input-validation-error');
                                    }
                                }
                            });

                            if (valid) {
                                var url = "../API/UpdateExchangeRate.ashx?custId=" + ExchRate.custId +
                                    "&exchangeId=" + exchId +
                                    "&description=" + ExchRate.editorDesc.val() +
                                    "&multipliedBy=" + ExchRate.editorMult.val() +
                                    "&baseRate=" + ExchRate.editorBase.val() +
                                    "&minBet=" + ExchRate.editorMinb.val() +
                                    "&minBetLive=" + ExchRate.editorMinbl.val();

                                $.ajax({
                                    url: url,
                                    dataType: "json",
                                    beforeSend: function () {
                                        $this.addClass('loading');
                                        buttons.attr('disabled', true).addClass("ui-state-disabled");
                                    }
                                }).done(function (data) {
                                    if (data.errCode == 0) {
                                        alert('Your changes were saved!');
                                        window.location.reload();
                                    } else {
                                        alert(data.errMsg);
                                    }
                                }).fail(function (jqXHR, textStatus, errorThrown) {
                                    APOLLO.handleAjaxError(jqXHR, textStatus, errorThrown);
                                }).always(function () {
                                    $this.removeClass('loading');
                                    buttons.attr('disabled', false).removeClass("ui-state-disabled"); ;
                                });
                            }
                        }
                    },
                    { text: "Cancel", click: function () { $(this).dialog("close"); } }]
                });
            });
        }
    };
} (jQuery);

jQuery(document).ready(function () {
    ExchRate.init();
});