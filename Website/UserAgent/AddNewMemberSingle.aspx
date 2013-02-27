<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddNewMemberSingle.aspx.cs" Inherits="UserAgent_AddNewMember_Single" %>
<link href="../../App_Themes/AgencyV2/newagent.css" type="text/css" rel="Stylesheet" />
<style type="text/css">
    .slwidth
    {
        width: 50px;
    }
</style>
<script type="text/javascript">
    _FocusEl = 'Number1';
    function IncreasePT(ddl,step)
    {
	    var max=$(ddl).options[0].value;   
		Increase(ddl,step,max);
    }
    
    function Increase(ddl,step,max)
    {
	    var objDDL=$(ddl);
	    var maxValue=Math.round(max*100);
	    var currSelect=Math.round(parseFloat(objDDL.value)*100);	
	    var nextValue=currSelect+step;                        
	    for(var i=0;i<objDDL.length;i++) 
	    {
		    if(objDDL[i].value==(nextValue/100.0))
		    {
			    objDDL[i].selected=true;
			    return true;
		    }		
	    }
	    return false;
    }
    function ReducePT(ddl,step)
    {
	    var min=0;  
		Reduce(ddl,step,min);    
    }  
    function Reduce(ddl,step,min)
    {
	    var objDDL=$(ddl);
	    var minValue=Math.round(min*100);
	    var currSelect=Math.round(parseFloat(objDDL.value)*100);
	    var nextValue=currSelect-step;                       
	    for(var i=0;i<objDDL.length;i++) 
	    {		
		    if(objDDL[i].value == (nextValue/100.0))
		    {
			    objDDL[i].selected=true;
			    return true;
		    }		
	    }
	    return false;
    }  
    function ChangeGroup()
    {
        $("txtComHdp").value = $("slGroup").value;
        changeComm($("slGroup").value, "slComHdp");
        $("hdGroup").value = $("slGroup").options[$("slGroup").selectedIndex].text;
    }
    window.onload = function()
    {
        ChangeGroup();
        CopyHandicap();
    }
    function changeComm(value, id){				
	    var select = $(id);
	    clearOptions(select);
    	
	    value = value * 10000 ; 
    	
	    while (value > 0){		
		    var format = value / 10000;				
		    var str = new String(format);		
			
		    var op = new Option(str, str);						
		    select.options[select.options.length] = op;	
		    value = value - 5;					
	    }
	    if (value <= 0){
		    var op = new Option("0", "0");						
		    select.options[select.options.length] = op;	
	    }
	    select.options.selectedIndex = 0;
    }    
    function clearOptions(select){				
	    for (i = select.options.length - 1; i >=0 ; i--){
		    select.options[i] = null;
	    }	
    }
    function BtnOnClick()
	{	
	    if(!CheckInfoValue())
	        return ;
        if(!CheckMinMax())
            return ;
		
		DelayReloadPage(null, 300000);
		enabledBtn(false);
		$("fUpdate").submit();
	}
	function CheckMinMax()
	{	    
	    if(isEmpty("txtMinBet"))
	    {
	        $("txtMinBet").focus();
		    ShowBalloonMsg("txtMinBet",alertMinbet);
		    return false;
	    }
	    if(isEmpty("txtMaxBet"))
	    {
	        $("txtMaxBet").focus();
		    ShowBalloonMsg("txtMaxBet",alertMaxbet);
		    return false;
	    }
	    if(isEmpty("txtMaxPerMatch"))
	    {
	        $("txtMaxPerMatch").focus();
		    ShowBalloonMsg("txtMaxPerMatch",alertMaxpermatch);
		    return false;
		}

		if (isEmpty("txtMaxPerBall")) {
		    $("txtMaxPerBall").focus();
		    ShowBalloonMsg("txtMaxPerBall", alertMaxperball);
		    return false;
		}
	    
        var MinBet = toFloat($("txtMinBet").value);
	    var MaxBet = toFloat($("txtMaxBet").value);
	    var MaxPerMatch = toFloat($("txtMaxPerMatch").value);
	    var MaxPerBall = toFloat($("txtMaxPerBall").value);
	    
	    var MinBet_max = toFloat($("hdMinBet").value);
	    var MaxBet_Min = toFloat($("hdMaxBet").value);
	    var MaxPerMatch_max = toFloat($("hdMaxPerMatch").value);
	    var MaxPerMatch_min = toFloat($("hdMaxPerMatchMin").value);
	    var MaxPerBall_max = toFloat($("hdMaxPerBallMax").value);

	    if ((MaxPerBall != 0) && (MaxPerBall_max != 0) && (MaxPerBall > MaxPerBall_max)) {
	        $("txtMaxPerBall").focus();
	        var str = "";
	        str = alertMaxperballC.replace("{1}",$("hdMaxPerBallMax").value);
	        ShowBalloonMsg("txtMaxPerBall", str);
	        return false;
	    }


	    if ((MinBet == 0) && (MaxBet == 0) && (MaxPerMatch == 0))
	        return true;
	    else {
	        if ((MinBet <= 0) || (MinBet < MinBet_max)) {
	            $("txtMinBet").focus();
	            ShowBalloonMsg("txtMinBet", alertminbetC + " " + $("hdMinBet").value);
	            return false;
	        }
	    }
	   
	    if ((MaxBet != 0) && (MaxBet_Min!=0) && MaxBet > MaxBet_Min)
	    {
	        $("txtMaxBet").focus();
	        ShowBalloonMsg("txtMaxBet", alertMaxbetC + " " + $("hdMaxBet").value);
	        return false;
	    }
	    if ((MaxPerMatch != 0) && (MaxPerMatch_max!=0) && (MaxPerMatch > MaxPerMatch_max || MaxPerMatch < MaxPerMatch_min))
	    {
	        $("txtMaxPerMatch").focus();
	        ShowBalloonMsg("txtMaxPerMatch", alertMaxpermatchC.replace("{1}", $("hdMaxPerMatchMin").value).replace("{2}",$("hdMaxPerMatch").value));
	        return false;
	    }
	    if (MinBet > MaxBet)
	    {
	        $("txtMinBet").focus();
	        ShowBalloonMsg("txtMinBet",alertCompare3);
	        return false;
	    }
        if (MaxBet > MaxPerMatch)
	    {
	        $("txtMaxBet").focus();
	        ShowBalloonMsg("txtMaxBet",alertCompare);
	        return false;
	    }
	    return true;
	}	
	function CheckInfoValue()
    {
	    if(isEmpty("txtPass"))
	    {
		    $("txtPass").focus();
		    ShowError(alertPasswordEmpty,"err");
		    return false;
	    }	 							
	    if(!checkpass($("txtPass")))
	    {						
		    $("txtPass").focus();
		    return false;
	    }
	    if(isEmpty("txtMaxCredit"))
	    {
	        $("txtMaxCredit").focus();
		    ShowBalloonMsg("txtMaxCredit",alertIncorrectCredit);
		    return false;
	    }
	    if(toFloat($("txtMaxCredit").value) > toFloat($("hdMaxCredit").value))
	    {
	        $("txtMaxCredit").focus();
		    ShowBalloonMsg("txtMaxCredit",alertCreditInvalid.replace("{1}", "0").replace("{2}", $("hdMaxCredit").value));
		    return false;
	    }
	    return true;				
    }

	function toFloat(str)
    {
	    var value ="";
	    for (i =0; i <str.length; i++) 
	    {
		    var c =str.charAt(i);
		    if (c !=',') value +=c;		
	    }
	    return parseFloat(value);
    }
    function CopyHandicap()
    {
        $("slLivePT").disabled =
        $("slOutrightPT").disabled =
        $("slOtherPT").disabled = 
        $("slNumberGamePT").disabled = $("chkCpyHdp").checked; 
    }

</script>