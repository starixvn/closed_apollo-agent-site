// require: jquery, jquery-ui

var ExchRate = function ($) {
    return {
        dialogEdit: null,

        init: function () {
            ExchRate.dialogEdit = $('#dialog-edit');

            $('.btn-edit').click(function () {
                var $this = $(this),
                    exchId = $this.attr('eid'),
                    currency = $this.attr('curr');

                APOLLO.openDialog(ExchRate.dialogEdit, "Edit " + currency);
            });
        }
    };
} (jQuery);

jQuery(document).ready(function () {
    ExchRate.init();
});