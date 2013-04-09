function initMessage() {
    if (typeof jQuery != 'undefined') {
        $(document).ready(function () {
            $('.datepicker').datepicker({
                showOn: "button",
                buttonImage: APOLLO.resolveUrl("_GlobalResources/Css/img/cal/calendar_10.png"),
                buttonImageOnly: true
            });

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