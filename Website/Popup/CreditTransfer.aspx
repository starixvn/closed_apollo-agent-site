<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreditTransfer.aspx.cs" Inherits="Popup_CreditTransfer" %>
<script type="text/javascript" src="../../Js/opt/jx.js?<%= AppSettings.JsVersion %>"></script>
<script type="text/javascript">

function UpdCredit()
{
    Ajax.Request('../Handlers/UpdateCredit.ashx',{
            method:'post',
            parameters:'custid=' + $('custid').value + '&amt=' + $('txtamout').value + '&site=' + $('sitetype').value,   
            onComplete:function(res)
            {                
               if(res.responseText=='Ok')
               {                   
                    parent.DelayReloadPage();
                    parent.ageWnd.close();
                }
                else if (res.responseText == '106') {
                    showErrMsg(lblconfirmclosesubacc, 0);

                }
                else if (res.responseText == '105') // kick out
                {
                    alert(lblConfirmClosed);
                    location.href = '../Authorization/Logout.ashx';
                }
               else
               {
                   Err(res.responseText);
                   $('txtamout').focus();
                   AddHeight(80);
               }
            }               
    });
}

function OnkeyUpAmt(e)
{
    var isok=true;    
    if (!e) e =window.event;    
	var key =(e.keyCode) ? e.keyCode : e.which;  
		
	if ( (key < 48 || key > 57) && key != 8 && key != 13 && key!=0 && (key < 37 || key > 40))
    {		
       return false;
    }
    else
    {
        if(key==13)   
        {      
            UpdCredit();                        
            return false;           
        }
    }
 }  
 
var ageWnd = false;
var ibcH = 0;
function AddHeight(h)
{
    if(!ageWnd)
    {
        ageWnd = parent.ageWnd;
        if( typeof ageWnd == 'undefined') return;
        ibcH = ageWnd.height;
    }
    
    ageWnd.setRect(ageWnd.left, ageWnd.top, ageWnd.width, ibcH + h);
}
    

function Err(msg)
{
    $('spmsgerr').innerHTML = msg; 
    $('diverrmsg').style.display = window.attachEvent? 'block':'table';
} 

</script>
