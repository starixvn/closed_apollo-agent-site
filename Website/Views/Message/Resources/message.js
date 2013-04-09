function initMessage() {
    if (typeof jQuery != 'undefined') {
        $(document).ready(function () {
            $('.datepicker').datepicker();

            $('.pagequery').click(function () {
                var value = $(this).attr('query');
                showMsg(value);
            });
        });
    }
}

function showMsg(value) {
    $('#PageQuery').val(value);
    $('form').submit();
}

RegisterStartUp(initMessage);