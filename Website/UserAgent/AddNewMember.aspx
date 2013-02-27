<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddNewMember.aspx.vb" Inherits="AddNewMember2" %>
<link href="../../App_Themes/AgencyV2/newagent.css" type="text/css" rel="Stylesheet" />
<style type="text/css">
    span.mp
    {
        color: #fff;
        font-size: 8pt;
        font-weight: bold;
    }
    INPUT.number
    {
        padding-right: 5px;
        font-weight: bold;
        text-align: right;
    }
    .Sel30
    {
        width: 30px;
    }
    .Sel40
    {
        width: 40px;
    }
    .Sel60
    {
        width: 60px;
    }
    body
    {
        margin: 0 0 0 0;
        padding: 0 0 0 0;
    }
</style>

<script src="../../Js/AddNewMaster.js?<%= AppSettings.JsVersion %>" type="text/javascript"></script>
<script src="../../Js/Customer.js?<%= AppSettings.JsVersion %>" type="text/javascript"></script>

<script type="text/javascript">			
			
	var arr1Super1 = ['listM1OUD', 'listM11stHdpD', 'listM11stOUD', 'listM1OE'];
	var arr1Master1 = [];
	var arr2Super1 = ['listM1CS', 'listM1TG', 'listM1MP', 'listM1OR'];
	var arr2Master1 = [];
	var arr3Super1 = ['listM1OUL', 'listM11stHdpL', 'listM11stOUL'];
	var arr3Master1 = [];			
	var arrSuper2 = ['listM2OU', 'listM2OE', 'listM2MP', 'listM2ML'];
	var arrMaster2 = [];
	var arrchk = ['listM1HdpL', 'listM2Hdp', 'listM3Hdp', 'listM5Hdp', 'listM10Hdp', 'chkCopy10Hdp', 'listM99Hdp', 'listM8Hdp', 'listM11Hdp', 'chkCopy1HdpDeadball', 'chkCopy11x2', 'chkCopy1HdpLive', 'chkCopy2Hdp', 'chkCopy3Hdp', 'chkCopy5Hdp', 'chkCopy10Hdp', 'chkCopy99Hdp', 'chkCopy8Hdp', 'chkCopy11Hdp', 'listM11x2', 'chkCopyBasket', 'txtMinbet2', 'txtMaxbet2', 'txtMaxPerMatch2', 'txtMinbet3', 'txtMaxbet3', 'txtMaxPerMatch3', 'txtMinbet5', 'txtMaxbet5', 'txtMaxPerMatch5', 'txtMinbet8', 'txtMaxbet8', 'txtMaxPerMatch8', 'txtMinbet10', 'txtMaxbet10', 'txtMaxPerMatch10', 'txtMinbet11', 'txtMaxbet11', 'txtMaxPerMatch11', 'txtMinbet99', 'txtMaxbet99', 'txtMaxPerMatch99', 'txtMinbet99_M', 'txtMaxbet99_M', 'txtMaxPerMatch99_M'];
	var arrchkBaskball = ['chkCopyAll', 'listM3Hdp', 'listM5Hdp', 'listM10Hdp', 'chkCopy10Hdp', 'listM99Hdp', 'listM8Hdp', 'listM11Hdp', 'chkCopy2Hdp', 'chkCopy3Hdp', 'chkCopy5Hdp', 'chkCopy10Hdp', 'chkCopy99Hdp', 'chkCopy8Hdp', 'txtMinbet3', 'txtMaxbet3', 'txtMaxPerMatch3', 'txtMinbet5', 'txtMaxbet5', 'txtMaxPerMatch5', 'txtMinbet8', 'txtMaxbet8', 'txtMaxPerMatch8', 'txtMinbet10', 'txtMaxbet10', 'txtMaxPerMatch10', 'txtMinbet11', 'txtMaxbet11', 'txtMaxPerMatch11', 'txtMinbet99', 'txtMaxbet99', 'txtMaxPerMatch99', 'txtMinbet99_M', 'txtMaxbet99_M', 'txtMaxPerMatch99_M'];
	var arrSuper3 = ['listM3OU', 'listM3OE', 'listM3ML'];
	var arrMaster3 = [];
	var arrSuper5 = ['listM5OU', 'listM5OE', 'listM5ML'];
	var arrMaster5 = [];
	var arrSuper10 = ['listM10OU', 'listM10OE', 'listM10ML'];
	var arrMaster10 = [];
	var arrSuper99 = ['listM99OU', 'listM99OE', 'listM99ML'];
	var arrMaster99 = [];
	var arrSuper8 = ['listM8OU', 'listM8ML'];
	var arrMaster8 = [];
	var arrSuper11 = ['listM11ML'];
	var arrMaster11 = [];

	var arr161Super81 = ['listM1611stOED', 'listM161OED', 'listM161OED', 'listM161WRD'];
	var arr161Super85 = ['listM161OEL', 'listM161HLL', 'listM161NCL', 'listM161NGL'];

	var arrSuper99_M = ['listM99_M'];
	
    _FocusEl = 'number1';
	//chkCopy : checkbox copy all
	//arrSuper: array ids of PT
	//chk: checkbox copy from Handicap
	function CheckCopyPT(chkCopy,arrSuper, chk)
	{			
		var c = $(chk);
		if(!c) chk = 0;
		for(var i=0;i<arrSuper.length;i++)
		{
		    if($(arrSuper[i]))
		    {
			    $(arrSuper[i]).disabled = (chkCopy.checked) ? true : ((!chk) ? false : (($(chk).checked) ? true: false));
			}
		}
	}	
	
	//Copy all position taking settings from Soccer Handicap
	function CopyAllPT(chkCopyAll)
	{
	    if(chkCopyAll.checked) 
        {
            $("panelSoccer").style.display="";		
        }
		for(i = 0; i<arrchk.length ;i++)
		{
		    if($(arrchk[i]))
		    {
			    $(arrchk[i]).disabled=chkCopyAll.checked;
			}
		}
		CheckCopyPT(chkCopyAll, arr1Super1, 'chkCopy1HdpDeadball');
		CheckCopyPT(chkCopyAll, arr2Super1, 'chkCopy11x2');
		CheckCopyPT(chkCopyAll, arr3Super1, 'chkCopy1HdpLive');
		CheckCopyPT(chkCopyAll, arrSuper2, 'chkCopy2Hdp'); 
		CheckCopyPT(chkCopyAll, arrSuper3, 'chkCopy3Hdp');
		CheckCopyPT(chkCopyAll, arrSuper5, 'chkCopy5Hdp');
		CheckCopyPT(chkCopyAll, arrSuper10, 'chkCopy10Hdp');
		CheckCopyPT(chkCopyAll, arrSuper99, 'chkCopy99Hdp');
		CheckCopyPT(chkCopyAll, arrSuper8, 'chkCopy8Hdp');
		CheckCopyPT(chkCopyAll, arrSuper11, 'chkCopy11Hdp');
//		CheckCopyPT(chkCopyAll, arr161Super81, 'chkCopy1611stOUD');
//		CheckCopyPT(chkCopyAll, arr161Super85, 'chkCopy161OUL');
		CheckCopyPT(chkCopyAll, arrSuper99_M);
	}
	
	//Copy all position taking settings from Basketball Handicap
	
	function CopyBasketPT(chkCopyAll)
	{
	    var chkCopyBasket=$("chkCopyBasket");
	    if(chkCopyBasket.checked) 
        {
            $("panelBasket").style.display="";		
        }
		for(i = 0; i<arrchkBaskball.length ;i++)
		{
		    if($(arrchkBaskball[i]))
		    {
			    $(arrchkBaskball[i]).disabled=chkCopyAll.checked;
			}
		}
		CheckCopyPT(chkCopyAll, arrSuper2, 'chkCopy2Hdp'); 
		CheckCopyPT(chkCopyAll, arrSuper3, 'chkCopy3Hdp');
		CheckCopyPT(chkCopyAll, arrSuper5, 'chkCopy5Hdp');
		CheckCopyPT(chkCopyAll, arrSuper10, 'chkCopy10Hdp');
		CheckCopyPT(chkCopyAll, arrSuper99, 'chkCopy99Hdp');
		CheckCopyPT(chkCopyAll, arrSuper8, 'chkCopy8Hdp');
		CheckCopyPT(chkCopyAll, arrSuper11, 'chkCopy11Hdp');
//		CheckCopyPT(chkCopyAll, arr161Super81, 'chkCopy1611stOUD');
//		CheckCopyPT(chkCopyAll, arr161Super85, 'chkCopy161OUL');
		CheckCopyPT(chkCopyAll, arrSuper99_M);
	}
	var flag=false;

	function GetValidateImage() {
	    var objDate = new Date();
	    $("imgCode").src = "../_Authorization/Handlers/Captcha.ashx?code=" + objDate.getMilliseconds();
	}

	function BtnAddCSClick()
	{
	    $('hidCS').value = 'true';
	    var isOK = BtnOnClick();

	    if(isOK)
	    {
	        $("btnAddCS").style.color="#cccccc";					
	        $("btnAddCS").disabled=true;	
	        $("btnAddCS").onclick = ";";
	    }			    
	}
    
	
	function BtnOnClick()
	{
	    var isEdit = ($("hidEdit") != null ? true : false);

	    if ($('txtCaptchaAddNewMember')) {
	        if ($('txtCaptchaAddNewMember').value == '') {
	            UpdateErr(errRequireValidationCode);
	            $('txtCaptchaAddNewMember').focus();
	            return;
	        }
	    }

		if(!isEdit)
		{
			$("btnAdd").disabled=true;					
		}
		else
		{
			$("btnSubmit").disabled=true;
		}

		if(!flag)
		{				
			if(!CheckInfoValue(false)) return false;

			if (!CheckLimitMinMaxBetForMember(1)) return false;
			if(!$("chkCopyAll").checked)
			{
			    if (!CheckLimitMinMaxBetForMember(2)) return false;
				if(!$("chkCopyBasket").checked)
				{	
				    if($('txtMaxPerBall'))
		            {
		                if(!CheckLimitMaxPerBall()) return false;
		            }
		            if (!CheckLimitMinMaxBetForMember(3)) return false;
		            if (!CheckLimitMinMaxBetForMember(5)) return false;
		            if (!CheckLimitMinMaxBetForMember(8)) return false;
		            if (!CheckLimitMinMaxBetForMember(10)) return false;
		            if (!CheckLimitMinMaxBetForMember(11)) return false;

		            if (!CheckLimitMinMaxBetForMember(99)) return false;
					
					if ($('txtMinbet99_M') || $('txtMaxbet99_M') || $('txtMaxPerMatch99_M')) {
					    if (!CheckLimitMinMaxBetForMember('99_M')) return false;
					}
				}
            }
            
            if ($('txtMinbet161') || $('txtMaxbet161') || $('txtMaxPerMatch161')) {
                if (!CheckLimitMinMaxBetForMember(161)) return false;
            }	
				
						
			flag=true;		
			DelayReloadPage(null, 300000);
			enabledBtn(false);
			document.fUpdate.submit();
		}
		return true;
	}
</script>
