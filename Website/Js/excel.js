AGE.addEvent(window, "load", function(){
    // make format excel
    if(document.getElementById("exporttoexcel"))
    {
        document.getElementById("exporttoexcel").onmousedown = function()
        {
            if(document.forms[0])
            {
                if(document.forms[0].method == 'get')
                {
                    document.forms[0].method = 'post';
                    document.getElementById("exporttoexcel").onblur = function()
                    {
                        var loading = document.getElementById("loading");
                        if (loading) loading.className = "none";

                        document.forms[0].method = 'get';
                    }
                }
            }
        }
    }
});