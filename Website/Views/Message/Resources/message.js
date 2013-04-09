function initMessage()
{    
    if (typeof jQuery != 'undefined') {
        $(document).ready(function () {
            $('.datepicker').datepicker();
        });
    }
}

RegisterStartUp(initMessage);