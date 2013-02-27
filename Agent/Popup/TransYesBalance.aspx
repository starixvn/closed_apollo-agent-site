<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransYesBalance.aspx.cs" Inherits="Popup_TransYesBalance" %>
<script type="text/javascript" src="../../Js/opt/jx.js?<%= AppSettings.JsVersion %>"></script>
<script type="text/javascript">
function OnkeyUpAmt(e)
{ 
    var isok=true;    
    if (!e) e =window.event;    
	var key =(e.keyCode) ? e.keyCode : e.which;  

	if ( (key < 48 || key > 57) && key != 8 && key != 13 && key!=0 && (key > 46 || key < 45) && (key < 37 || key > 40))
    {		
       return false;
    }
    else
    {
        if(key==13)   
        {      
            UpdCreditYes();                        
            return false;           
        }
    }
}
function UpdCreditYes() {
    var wrnconfirm = "";   
    wrnconfirm = wrnconfirmtranfer;
    if (confirm(wrnconfirm)) {
        Ajax.Request('../Handlers/UpdateTransferYesBalance.ashx', {
            method: 'post',
            parameters: 'custid=' + $('custid').value + '&status=' + $('isclose').value + '&amount=' + $('txtamount').value + '&oldamount=' + $('amount').value,
            onComplete: function(res) {
                if (res.responseText == 'Ok') {
                    if ($('nobackground')) {
                        showErrMsg(transfersuccessful, 1);
                        parent.top.menu.reloadMenuUsrInfo(); //reload left menu
                    }
                    else {
                        parent.DelayReloadPage(parent.$('hdCurrentUrl').value);
                        parent.ageWnd.close();
                    }
                }
                else if (res.responseText == '106') {
                    showErrMsg(lblconfirmclosesubacc, 0);

                }
                else if (res.responseText == '105') // kick out
                {
                    alert(lblConfirmClosed);
                    location.href = '../Authorization/Logout.ashx';
                }
                else {
                    Err(res.responseText);
                    AddHeight(170);
                }
            }
        });
    }
}

function Err(msg)
{
    showErrMsg(msg, 0);
}

var ageWnd = false;

if (typeof parent.AGEWnd != 'undefined') {
    if (!parent.AGEWnd._preHeight) {
        parent.AGEWnd._preHeight = 0;
    }
}

function AddHeight(h)
{
    if(!ageWnd)
    {
        ageWnd = parent.ageWnd;
        if( typeof ageWnd == 'undefined') return;

        if (parent.AGEWnd._preHeight == 0) {
            parent.AGEWnd._preHeight = ageWnd.height;
        }
    }
    
    ageWnd.setRect(ageWnd.left, ageWnd.top, ageWnd.width, parent.AGEWnd._preHeight + h);
}


window.onload = function()
{
    ageWnd = parent.ageWnd;
    if(typeof ageWnd == 'undefined')
    {
        return;
    }
    
    if(ageWnd.loaded)
    {
        ageWnd.setRect(ageWnd.left, ageWnd.top, 400, 115);
    }
}

</script>