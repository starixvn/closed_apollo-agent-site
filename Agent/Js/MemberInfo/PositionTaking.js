function InsertCSS(cssLink)
{
    var headID = document.getElementsByTagName("head")[0];         
    var cssNode = document.createElement('link');
    cssNode.type = 'text/css';
    cssNode.rel = 'stylesheet';
    cssNode.href = cssLink; 
    cssNode.media = 'screen';headID.appendChild(cssNode);
}

function alertSize() 
{
	  var myHeight = 0; 
	  var rowcount = parseFloat($('rowcount').value);
	  if( typeof( window.innerWidth ) == 'number' ) 
	  {
	        //Non-IE
	  } 
	  else if( document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight ) ) 
	  {
	        //IE 6+ in 'standards compliant mode'
	        myHeight = document.documentElement.clientHeight-30;	        	        
	        
	        if(rowcount>=20)
	        {
		        if(myHeight > 100)
		        {
			        $("divParent").style.height = myHeight - 10;			
			        $("tbdContent").style.height = myHeight-25;
		        }
		        else
		        {
			        $("divParent").style.height = myHeight;
			        $("tbdContent").style.height = 70;    			
		        }
		    }
		    else
		    {
		       if(myHeight > 100)
		            $("divParent").style.height = myHeight - 40;		
		        else
		            $("divParent").style.height = myHeight;
		    }
	  } 
	  else if( document.body && ( document.body.clientWidth || document.body.clientHeight ) ) 
	  {
	        //IE 4 compatible
	        myHeight = document.body.clientHeight-30;
	        
	        if(rowcount>=20)
	        {
		        if(myHeight > 100)
		        {
			        $("divParent").style.height = myHeight - 10;			
			        $("tbdContent").style.height = myHeight-25;
		        }
		        else
		        {
			        $("divParent").style.height = myHeight;
			        $("tbdContent").style.height = 70;    			
		        }
		    }
		    else
		    {
		       if(myHeight > 100)
		            $("divParent").style.height = myHeight - 20;		
		        else
		            $("divParent").style.height = myHeight;
		    }
	  }
}	

window.onload = alertSize;
