// require: jquery, jquery-ui

var ExchRate = function ($) {
    return {
        dialogEdit: null,
        editorWrp: null,
        editorDesc: null,
        editorMult: null,
        editorBase: null,
        editorMinb: null,
        editorMinbl: null,
        dialogEditSubmit: null,
        dialogEditCancel: null,

        fillEditor: function (multiply, baseRate, minBet, minBetLive, description) {
            ExchRate.editorDesc.val(description);
            ExchRate.editorMult.val(multiply);
            ExchRate.editorBase.val(baseRate);
            ExchRate.editorMinb.val(minBet);
            ExchRate.editorMinbl.val(minBetLive);
        },

        init: function () {
            ExchRate.dialogEdit = $('#dialog-edit');
            ExchRate.editorWrp = $('#editor-wrp');

            ExchRate.editorDesc = $("#editor-desc");
            ExchRate.editorMult = $("#editor-mult");
            ExchRate.editorBase = $("#editor-base");
            ExchRate.editorMinb = $("#editor-minb");
            ExchRate.editorMinbl = $("#editor-minbl");

            $('.btn-edit').click(function () {
                var $this = $(this),
                    exchId = $this.attr('eid'),
                    row = $this.parents('tr'),
                    currency = row.children('td:eq(0)').text(),
                    multiply = row.children('td:eq(1)').text(),
                    baseRate = row.children('td:eq(2)').text(),
                    minBet = row.children('td:eq(3)').text(),
                    minBetLive = row.children('td:eq(4)').text(),
                    description = row.children('td:eq(5)').text();

                ExchRate.fillEditor(multiply, baseRate, minBet, minBetLive, description);
                APOLLO.openDialog(ExchRate.dialogEdit, "Edit Currency: " + currency,
                {
                    buttons: [
                    {
                        text: "Submit",
                        click: function () {
                            var valid = true;

                            ExchRate.editorWrp.find('input:not(#editor-desc)').each(function () {
                                if (valid) {
                                    var $this = $(this);

                                    $this.val($.trim($this.val()));
                                    if ($this.val().length == 0) {
                                        $this.addClass('input-validation-error');
                                        alert('This field cannot be blank');
                                        valid = false;
                                    } else {
                                        $this.removeClass('input-validation-error');
                                    }
                                }
                            });

                            if (valid) {
                                var url = "../API/UpdateExchangeRate.ashx" +
                                    "?ExchangeId=" + exchId +
                                    "&Description=" + ExchRate.editorDesc.val() +
                                    "&MultipliedBy=" + ExchRate.editorMult.val() +
                                    "&BaseRate=" + ExchRate.editorBase.val() +
                                    "&MinBet=" + ExchRate.editorMinb.val() +
                                    "&MinBetLive=" + ExchRate.editorMinbl.val();

                                $.ajax({
                                    url: url,
                                    dataType: "json",
                                    beforeSend: function () {

                                    }
                                }).done(function (data) {
                                    if (data.errCode == 0) {
                                        alert('Your changes were saved!');
                                        window.location.reload();
                                    } else {
                                        alert(data.errMsg);
                                        //APOLLO.alertGeneralError();
                                    }
                                }).fail(function (jqXHR, textStatus, errorThrown) {
                                    APOLLO.handleAjaxError(jqXHR, textStatus, errorThrown);
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