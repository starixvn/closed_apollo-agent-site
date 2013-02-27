AGE.addEvent(window, "load", function(){
    // make format excel
    if($("exporttoexcel"))
    {
        $("exporttoexcel").onmousedown = function()
        {
            if(document.forms[0])
            {
                if(document.forms[0].method == 'get')
                {
                    document.forms[0].method = 'post';
                    $("exporttoexcel").onblur = function()
                    {
                        var loading = $("loading");
                        if (loading) loading.className = "none";

                        document.forms[0].method = 'get';
                    }
                }
            }
        }
    }
});