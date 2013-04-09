function initMessage()
{    
    if (typeof jQuery != 'undefined') {
        $(document).ready(function () {            
            $('.input-date').datepicker();
        });
    }
}

RegisterStartUp(initMessage);