function formatCommas(numString)
{	
    var re = /(-?\d+)(\d{3})/;
    while (re.test(numString)) {numString = numString.replace(re, "$1,$2");}
    return numString;
}

function round(number,X)
{
	X = (!X ? 2 : X);
	return Math.round(number*Math.pow(10,X))/Math.pow(10,X);
}
function toNumber(str)
{
	var value ="";
	for (i =0; i <str.length; i++) 
	{
		var c =str.charAt(i);
		if (c !=',') value +=c;		
	}
	return value;
}


function clearOptions(select){				
	for (i = select.options.length - 1; i >=0 ; i--){
		select.options[i] = null;
	}	
}
			
function changeCommHDP(value){				
	var select = document.getElementById("ddrCommissionH_OU_OE");
	clearOptions(select);					
	
	value = value * 10000 ; 
	
	while (value > 0){		
		var format = value / 10000;				
		
		var op = new Option(format, format);						
		select.options[select.options.length] = op;	
		value = value - 5;					
	}
	if (value <= 0){
		var op = new Option("0", "0");						
		select.options[select.options.length] = op;	
	}
	select.options.selectedIndex = 0;
}

function checkinput(e){				
	var code = (window.Event) ? e.which : e.keyCode;				
	if ( (code < 46 || code > 57) && code != 8 && code != 13 && code!=0){		
		return false;
	}
	return true;		
}
		
function CopyCheck(obj){
	if(obj.id == "chkDeadball")	{					
		document.getElementById("ddl2").disabled = obj.checked;
		document.getElementById("ddl3").disabled = obj.checked;
		document.getElementById("ddl4").disabled = obj.checked;
		document.getElementById("ddl5").disabled = obj.checked;				
	}
	
	if(obj.id == "chkDeadballOther"){
		document.getElementById("ddl7").disabled = obj.checked;
		document.getElementById("ddl6").disabled = obj.checked;
		document.getElementById("ddl9").disabled = obj.checked;
		document.getElementById("ddl19").disabled = obj.checked;
		
	}
	
	if(obj.id == "chkLive"){
		document.getElementById("ddl11").disabled = obj.checked;
		document.getElementById("ddl12").disabled = obj.checked;
		document.getElementById("ddl13").disabled = obj.checked;
	}
	
	if(obj.id == "chkBasketball"){
		document.getElementById("ddl15").disabled = obj.checked;		
		document.getElementById("ddl18").disabled = obj.checked;
	}
}
//*************function checkvalue input **************//
function setValueCommission(form, vcom)
{
	if(form.hidden_discount) form.hidden_discount.value = vcom;		
}
function updateCommission(form)
{
	
	var ug = document.getElementById('ddlUGruop');

	if ((ug.options[ug.selectedIndex].text == "a") || (ug.options[ug.selectedIndex].text == "hka") || (ug.options[ug.selectedIndex].text == "indoa") || (ug.options[ug.selectedIndex].text == "dea"))
	{
		form.txtDiscount.value = valGroupa;		
		setValueCommission(form, valGroupa);
		changeCommHDP(parseFloat(valGroupa));
		
	}
if ((ug.options[ug.selectedIndex].text == "b") || (ug.options[ug.selectedIndex].text == "hkb") || (ug.options[ug.selectedIndex].text == "indob") || (ug.options[ug.selectedIndex].text == "deb"))
	{
        form.txtDiscount.value = valGroupb;
        setValueCommission(form, valGroupb);
        changeCommHDP(parseFloat(valGroupb));
    }
    if ((ug.options[ug.selectedIndex].text == "c") || (ug.options[ug.selectedIndex].text == "hkc") || (ug.options[ug.selectedIndex].text == "indoc") || (ug.options[ug.selectedIndex].text == "dec"))
	{
		form.txtDiscount.value = valGroupc;
		setValueCommission(form, valGroupc);
		changeCommHDP(parseFloat(valGroupc));
	}
if ((ug.options[ug.selectedIndex].text == "d") || (ug.options[ug.selectedIndex].text == "hkd") || (ug.options[ug.selectedIndex].text == "indod") || (ug.options[ug.selectedIndex].text == "ded"))
	{
	    form.txtDiscount.value = valGroupd;
		changeCommHDP(parseFloat(valGroupd));
	}
}
function ReplaceAll(str, regexp, newstr)
{
	while(str.indexOf(regexp) > 0)
	{
		str = str.replace(regexp,newstr);
	}
	return str;
}
function checkvalue(form)
{
	var maxcredit=document.getElementById("membermaxcredit");
	if(maxcredit)
	{				
		if (maxcredit.value == '' || isNaN(maxcredit.value))
		{
			alert(strAlertIncorrectMmaxcredit);
			maxcredit.value='';
			maxcredit.focus();
			return false;
		}
	}
	var pass = document.getElementById("password");
	if (pass)
	{
		if (pass.value.length < 6)
		{
			alert('password not validate');
			return false;
		}
		else
		{
			if (check(pass.value) == false)
			{
				alert('error pass');
				return false;
			}
		}
	}	
	if (document.form.minbet.value == '' || isNaN(ReplaceAll(document.form.minbet.value,',','')))
	{
		alert(strAlertIncorrectMinBet);
		document.form.minbet.value='';
		document.form.minbet.focus();
		return false;
	}
			
	if (parseFloat(ReplaceAll(document.form.minbet.value,',',''))!=0 && parseFloat(ReplaceAll(lminbet,',','')) !=0 && parseFloat(ReplaceAll(document.form.minbet.value,',','')) < parseFloat(ReplaceAll(lminbet,',','')))
	{
		alert(lblalertMinbetCompare + " " +lminbet);
		document.form.minbet.focus();
		return false;
	}
				
	if (document.form.maxbet.value == '' || isNaN(ReplaceAll(document.form.maxbet.value,',','')))
	{
		alert(strAlertIncorrectMaxBet);
		document.form.maxbet.value='';
		document.form.maxbet.focus();
		return false;
	}
	if (parseFloat(ReplaceAll(document.form.maxbet.value,',',''))!=0 && parseFloat(ReplaceAll(lmaxbet,',',''))!=0 && parseFloat(ReplaceAll(document.form.maxbet.value,',','')) > parseFloat(ReplaceAll(lmaxbet,',','')))
	{
		alert(lblalerMaxbetCompare + " " +lmaxbet);
		document.form.maxbet.focus();
		return false;
	}
	if (document.form.maxpermatch.value == '' || isNaN(ReplaceAll(document.form.maxpermatch.value,',','')))
	{
		alert(strAlertIncorrectMaxPerMatch);
		document.form.maxpermatch.value='';
		document.form.maxpermatch.focus();
		return false;
	}
	if (parseFloat(ReplaceAll(document.form.maxpermatch.value,',','')) >parseFloat(ReplaceAll(lmaxpermatch,',','')) || parseFloat(ReplaceAll(document.form.maxpermatch.value,',','')) <parseFloat(ReplaceAll(lminpermatch,',','')))
	{
		alert(lblalertMaxPerMatchCompare + " [" +lminpermatch + ";" + lmaxpermatch + "]");
		document.form.maxpermatch.focus();
		return false;
	}
	
	if ((parseFloat(ReplaceAll(document.form.minbet.value,',','')) > parseFloat(ReplaceAll(document.form.maxbet.value,',',''))))
	{
		alert(strAlertMaxLessMin);
		return false;
	}
	
	if (document.form.minbetB.value == '' || isNaN(ReplaceAll(document.form.minbetB.value,',','')))
	{
		alert(strAlertIncorrectMinBet);
		document.form.minbetB.value='';
		document.form.minbetB.focus();
		return false;
	}
	if(parseFloat(ReplaceAll(document.form.minbetB.value,',',''))!=0 && parseFloat(ReplaceAll(lminbetb,',',''))!=0 && parseFloat(ReplaceAll(document.form.minbetB.value,',','')) <parseFloat(ReplaceAll(lminbetb,',','')))
	{
		alert(lblalertMinbetCompare + " " +lminbetb);
		document.form.minbetB.focus();
		return false;	
	}
	if (document.form.maxbetB.value == '' || isNaN(ReplaceAll(document.form.maxbetB.value,',','')))
	{
		alert(strAlertIncorrectMaxBet);
		document.form.maxbetB.value='';
		document.form.maxbetB.focus();
		return false;
	}
	if (parseFloat(ReplaceAll(document.form.maxbetB.value,',',''))!=0 && parseFloat(ReplaceAll(lmaxbetb,',',''))!=0  && parseFloat(ReplaceAll(document.form.maxbetB.value,',','')) >parseFloat(ReplaceAll(lmaxbetb,',','')))				
	{
		alert(lblalerMaxbetCompare + " " +lmaxbetb);
		document.form.maxbetB.focus();
		return false;				
	}
	if (document.form.maxpermatchB.value == '' || isNaN(ReplaceAll(document.form.maxpermatchB.value,',','')))
	{
		alert(strAlertIncorrectMaxPerMatch);
		document.form.maxpermatchB.value='';
		document.form.maxpermatchB.focus();
		return false;
	}
	if (parseFloat(ReplaceAll(document.form.maxpermatchB.value,',','')) >parseFloat(ReplaceAll(lmaxpermatchb,',','')) || parseFloat(ReplaceAll(document.form.maxpermatchB.value,',','')) < parseFloat(ReplaceAll(lminpermatchb,',','')))
	{
		alert(lblalertMaxPerMatchCompare + " [" +lminpermatchb + ";" + lmaxpermatchb + "]");
		document.form.maxpermatchB.focus();
		return false;
					
	}
	if ((parseFloat(ReplaceAll(document.form.minbetB.value,',','')) > parseFloat(ReplaceAll(document.form.maxbetB.value,',',''))))
	{
		alert(strAlertMaxLessMin);
		return false;
	}
	
	var strDeadball = new String();
    if (document.form.chkDeadball.checked == true)
	{
		var objDeadball = document.getElementById('ddl1');
		if(objDeadball)
		{
			if(objDeadball.value > document.getElementById('ddl2').options[0].value || objDeadball.value > document.getElementById('ddl3').options[0].value || objDeadball.value > document.getElementById('ddl4').options[0].value || objDeadball.value > document.getElementById('ddl5').options[0].value)
			{
				strDeadball += "-" + document.getElementById("lblSoccerPositionTaking").innerHTML + " " + document.getElementById("lblDeadball").innerHTML + "\n";
			}
			if(objDeadball.value > document.getElementById('ddl2').options[0].value)
			{
				strDeadball += "......." + document.getElementById("lblOverUnder").innerHTML + " " + strAlertMaxIs + " " + document.getElementById('ddl2').options[0].text + "\n";
			}
			if(objDeadball.value > document.getElementById('ddl3').options[0].value)
			{
				strDeadball += "......." + document.getElementById("lbl1stHdp").innerHTML + " " + strAlertMaxIs + " " + document.getElementById('ddl3').options[0].text + "'\n";
			}
			if(objDeadball.value > document.getElementById('ddl4').options[0].value)
			{
				strDeadball += "......." + document.getElementById("lbl1stOU").innerHTML + " " + strAlertMaxIs + " " + document.getElementById('ddl4').options[0].text +  "'\n";
			}
			if(objDeadball.value > document.getElementById('ddl5').options[0].value)
			{
				strDeadball += "......." + document.getElementById("lblOddEven").innerHTML + " " + strAlertMaxIs + " " + document.getElementById('ddl5').options[0].text + "'\n";
			}
		}
	}
	if (document.form.chkDeadballOther.checked == true)
	{
		var objDeadballOther = document.getElementById('ddl8');
		if(objDeadballOther)
		{
			if(objDeadballOther.value > document.getElementById('ddl7').options[0].value)
			{
				strDeadball += "......." + document.getElementById("lblMixParlay").innerHTML + " " + strAlertMaxIs + " " + document.getElementById('ddl7').options[0].text + "'\n";
			}
			if(objDeadballOther.value > document.getElementById('ddl6').options[0].value)
			{
				strDeadball += "......." + document.getElementById("lblCorrectScore").innerHTML + " " + strAlertMaxIs + " " + document.getElementById('ddl6').options[0].text + "'\n";
			}
			if(objDeadballOther.value > document.getElementById('ddl9').options[0].value)
			{
				strDeadball += "......." + document.getElementById("lblTotalGoal").innerHTML + " " + strAlertMaxIs + " " + document.getElementById('ddl9').options[0].text + "'\n";
			}
			if(objDeadballOther.value > document.getElementById('ddl19').options[0].value)
			{
				strDeadball += "......." + document.getElementById("lblOutRight").innerHTML + " " + strAlertMaxIs + " "+ document.getElementById('ddl19').options[0].text + "'\n";
			}
		}
	}
	if (document.form.chkLive.checked == true)
	{
		var objLive = document.getElementById('ddl10');
		if(objLive)
		{
			if(objLive.value > document.getElementById('ddl11').options[0].value || objLive.value > document.getElementById('ddl12').options[0].value || objLive.value > document.getElementById('ddl13').options[0].value)
			{
				strDeadball += "-" + document.getElementById("lblSoccerPositionTaking").innerHTML + " " + document.getElementById("lblLive").innerHTML + "\n";
			}
			if(objLive.value > document.getElementById('ddl11').options[0].value)
			{
				strDeadball += "......." + document.getElementById("lblOddEven").innerHTML + " " + strAlertMaxIs + " " + document.getElementById('ddl11').options[0].text + "'\n";
			}
			if(objLive.value > document.getElementById('ddl12').options[0].value)
			{
				strDeadball += "......." + document.getElementById("lbl1stHdp").innerHTML + " " + strAlertMaxIs + " " + document.getElementById('ddl12').options[0].text + "'\n";
			}
			if(objLive.value > document.getElementById('ddl13').options[0].value)
			{
				strDeadball += "......." + document.getElementById("lbl1stOU").innerHTML + " " + strAlertMaxIs + " " + document.getElementById('ddl13').options[0].text + "\n";
			}
		}
	}
	if (document.form.chkBasketball.checked == true)
	{
		var objBasketball = document.getElementById('ddl14');
		if(objLive)
		{
			if(objBasketball.value > document.getElementById('ddl15').options[0].value || objBasketball.value > document.getElementById('ddl18').options[0].value)
			{
				strDeadball += "-" + document.getElementById("lblBasketballPositionTaking").innerHTML + "\n";
			}
			if(objBasketball.value > document.getElementById('ddl15').options[0].value)
			{
				strDeadball += "......." + document.getElementById("lblOverUnder").innerHTML + " " + strAlertMaxIs + " "  + document.getElementById('ddl15').options[0].text + "\n";
			}						
			if(objBasketball.value > document.getElementById('ddl18').options[0].value)
			{
				strDeadball += "......." + document.getElementById("lblOddEven").innerHTML + " " + strAlertMaxIs + " " + document.getElementById('ddl18').options[0].text + "\n";
			}
		}
	}
	if(strDeadball != '')
	{
		var s = strAlertCopyPositionTaking + " \n";
		s += strDeadball;
		alert(s);
		return false;
	}	
	return true;
}


