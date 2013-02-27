var tblRpt,boderRight,boxHeader,headerMain,tblContainer,clientDocument;
function Max(a,b)
{
    if(a>b)
        return a;
    return b;    
}

function FixWidthHeight() {
    var width=Max(670,tblRpt.offsetWidth);
    
    tblRpt.style.width = width;
    if(boderRight){    
        boderRight.style.width=width;
        boderRight.style.height=tblRpt.offsetHeight;
     }
     var boxHeaderHeight=0,height=0;
    if(boxHeader){    
        boxHeader.style.width=width;
        boxHeaderHeight+=boxHeader.offsetHeight;
    }
    if(headerMain){    
       headerMain.style.width=width;
       boxHeaderHeight+=headerMain.offsetHeight;
    }  
    if(clientDocument)
        height=clientDocument.clientHeight-boxHeaderHeight;
    if(tblContainer){
            tblContainer.style.width=width+18;
            tblContainer.style.height=Max(180,height-23);
        }
     
}
function Init(){
    /*var table = document.getElementById('tbl-rpt');	
	var cTR = table.getElementsByTagName('TR');  
    for (i = 0; i < cTR.length; i++)
	{
			var tr = cTR.item(i);
			tr.cells[0].className = 'locked';
			
	}*/
	tblRpt=document.getElementById("tbl-rpt");
	boderRight=document.getElementById("boderRight");
	boxHeader=document.getElementById("box_header");
	headerMain=document.getElementById("header_main");
	tblContainer=document.getElementById("tbl-container");
	if(document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight))
	     clientDocument=document.documentElement;
	else
	    clientDocument=document.body;
    FixWidthHeight();
}

window.onload =Init;
window.onresize=FixWidthHeight;
