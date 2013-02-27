<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePass.aspx.cs" Inherits="AccountInfo_ChangePass" %>
<script type="text/javascript" src="../../Js/opt/jx.js?<%= AppSettings.JsVersion %>"></script>
<script type="text/javascript">
function Validate()
{
    if($('OldPassword').value=='')
    {
        Err($('alertInputOldPwd').value);	
        $('OldPassword').focus();
        return false;
    }
    else if ($('NewPassword').value == "")
    {
        Err($('alertPasswordEmpty').value);
        
        $('NewPassword').focus();        
        return false;
    }
    else if ($('NewPassword').value != $('ConfirmPassword').value) 
    {
        Err($('alertErrorConfirm').value);
        
        $('ConfirmPassword').focus();
        return false;
    }
    else
    {
        var str = $("NewPassword").value;					
		numbers = new String("0123456789");						
		if(str.length < 6)
		{							
			Err($("alertPasswordRequire").value);
			return false;
		}
		var i = 0;
		var nCount = 0;
		while(i < str.length)
		{
			if(numbers.indexOf(str.substring(i,i+1)) >= 0)
			{							
				nCount ++;
			}
			i++;
		}					
		if(nCount == str.length || nCount < 2)
		{						
			Err($("alertPasswordRequire").value);
			
			$('NewPassword').focus();
			return false;
		}
    }
    return true;
}

function Err(msg)
{
    $('trMsg').style.display = 'block'; 
    
    var tbl2 = $('tbl2');
    if(typeof tbl2 != 'undefined' && tbl2 != null)
    {
        tbl2.style.display = 'none';
    }
    
    $('tbl1').style.display = 'block';
    $('spmsgerr').innerHTML = msg; 
    $('spmsgerr').className='msgerr';
}
function Succ(msg)
{
    $('trMsg').style.display = 'block';
    
    var tbl2 = $('tbl2');
    if(typeof tbl2 != 'undefined' && tbl2 != null)
    {
        tbl2.style.display = 'none';
    }
    
    $('tbl1').style.display = 'block';
    $('spmsgerr').innerHTML = msg;
    $('spmsgerr').className='msgscc';
    $('1_1').className="succ_1_1";
    $('1_2').className="succ_1_2";
    $('1_3').className="succ_1_3";
    $('2_1').className="succ_2_1";
    $('2_2').className="succ_2_2";
    $('3_1').className="succ_3_1";
    $('3_2').className="succ_3_2";
    $('3_3').className="succ_3_3";
}
function ExcSubmit()
{
    if (Validate())
    {
        var myajax= new Ajax.Request('../Authorization/ChangePassword.ashx',{
                    method:'post',
                    parameters: 'op=' + $('OldPassword').value + '&np=' + $("NewPassword").value,
                    onComplete:function(res)
                    {               
                       if(res.responseText=='Ok')
                       {    
                            $('NewPassword').value='';
                            $('ConfirmPassword').value='';
                            $('OldPassword').value='';                  
                            Succ($('alertPasswordSucc').value);
                            ResetCheckPass('td_pass');
                            ResetCheckPass('td_confirmpass');
                       }  
                       else if(res.responseText=='changed')    
                       {
                       
                            top.menu.location = top.menu.location.href;
                            top.frHeader.location = top.frHeader.location.href;
                            $('NewPassword').value='';
                            $('ConfirmPassword').value='';
                            $('OldPassword').value='';                  
                            Succ($('alertPasswordSucc').value);
                            ResetCheckPass('td_pass');
                            ResetCheckPass('td_confirmpass');
                       }     
                       else
                       {
                            Err(res.responseText);                            
                       }
                    }               
                });
    }
}

function ExcReset()
{
    $('trMsg').style.display = 'none';    
    if ($('pass_td_pass'))
        $('pass_td_pass').src = '../App_Themes/AgencyV2/Images/dot.gif';
    if ($('pass_td_confirmpass'))
        $('pass_td_confirmpass').src = '../App_Themes/AgencyV2/Images/dot.gif';
    
    $('OldPassword').focus();
}

function PreSubmit(e)
{
    e =(e) ?e :window.event;
	var key =(e.keyCode) ?e.keyCode :e.which;
	if (key ==13) ExcSubmit();
}

</script>
