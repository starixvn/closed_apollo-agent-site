//Cancelled Bets report
function selected_Option_master(e) {
    
    var form = $("frmCancelBetList");
    if (form) {
        var loading = $("loading");
        //if (loading) { loading.className = "loading1"; }   
        form.submit();        
    }
}

function selected_Option_agent(e) {

    var form = $("frmCancelBetList");
    if (form) {
        var loading = $("loading");
        //if (loading) { loading.className = "loading1"; }        
        form.submit();
    }
}

function selected_Option_member(e) {    

    var form = $("frmCancelBetList");
    if (form) {
        var loading = $("loading");
        //if (loading) { loading.className = "loading1"; }        
        form.submit();
    }
}

function InitExcelFunction() {
    var exportExcelImg = $("exporttoexcel");    
    if (exportExcelImg) {

        function exportExcelImg_OnClick(ev) {
            var url = location.href;
            
            url = SetParameterValue('exporttoexcel.x', 4, url);
            url = SetParameterValue('exporttoexcel.y', 8, url);
            
            location.href = url;

            if (IE) {
                window.event.cancelBubble = true;
                window.event.returnValue = false;
            } else {
                ev.preventDefault();
                ev.stopPropagation();
            }
        }

        age.addEvent(exportExcelImg, 'click', exportExcelImg_OnClick);

        exportExcelImg.onblur = function() {
            $("exporttoexcel").focused = false;
        }
        exportExcelImg.onfocus = function() {
            $("exporttoexcel").focused = true;
        }      
    }
}

function init()
{
   InitExcelFunction();
}

RegisterStartUp("init()");