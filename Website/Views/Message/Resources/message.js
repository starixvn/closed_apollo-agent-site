function initMessage() {
    if (typeof jQuery != 'undefined') {
        $(document).ready(function () {
            $('.datepicker').datepicker();
        });
    }
}

function showMsg(value) {
    $('#PageQuery').val(value);
    $('form').submit();
}

RegisterStartUp(initMessage);