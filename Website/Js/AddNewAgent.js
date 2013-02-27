var flag = false;
var gisCopyAll = false;
function BtnOnClick() {
    var isEdit = ($("hidEdit") != null ? true : false);
    var arrAcc;
    var arrPAcc;
    var arrMin;
    if (!flag) {
        if (!CheckInfoValue(isEdit)) return false;
        if (!isEdit) {
            var isDirect = $("dirMaster") != null ? $("dirMaster").value : "0";
            if (isDirect == "1") // if Direct Master case is call CheckTime function
            {
                if (!CheckTime()) return false;
            }
        }
        
        //check info for Soccer
        
        if (!CheckLimitMinMaxBet(1)) return false;
        //-----end----
        var isMult = 0;
        if ($("isMult") && $("isMult").value == "1")
            isMult = 1;
        
        
        if (isMult || !$("chkCopyAll").checked) {
            //check info for Basketball
            if (!CheckLimitMinMaxBet(2)) return false;
            //-----end----
            if (isMult || !$("chkCopyBasket").checked) {
            
               if($('txtMaxPerBall'))
		        {
		            if(!CheckLimitMaxPerBall()) return false;
		        }
                //check info for Football
                if (!CheckLimitMinMaxBet(3)) return false;
                //-----end----
                //check info for Tennis
                if (!CheckLimitMinMaxBet(5)) return false;
                //-----end----
                //check info for Baseball
                if (!CheckLimitMinMaxBet(8)) return false;
                //-----end----
                //check info for Golf
                if (!CheckLimitMinMaxBet(10)) return false;
                //-----end----
                //check info for MotoSports
                if (!CheckLimitMinMaxBet(11)) return false;
                //-----end----	
                //check info for MotoSports				
                if ($('txtMinbet201') || $('txtMaxbet201') || $('txtMaxPerMatch201')) {
                    if (!CheckLimitMinMaxBet(201)) return false;
                }
                //-----end----
                
                //check info for Other Sports
                if (!CheckLimitMinMaxBet(99)) return false;
                
                if (!CheckLimitMinMaxBet('99_M')) return false;
                //-----end----
                //check info for Horse Racing
                if ($("chkDisabledHR")) {
                    if (!CheckLimitMinMaxBet(151)) return false;
                }
                //-----end----
            }
        }
        if ($('txtMinbet161') || $('txtMaxbet161') || $('txtMaxPerMatch161')) {
            if (!CheckLimitMinMaxBet(161)) return false;
        }
        if (!isEdit) {
            $("btnAdd").disabled = true;
            flag = true;
            DelayReloadPage(null, 300000);
            enabledBtn(false);
            document.form.submit();
        }
        else {            
            $("btnSubmit").disabled = true;
            var isPT = CheckPotionTakingAgent();
            if (!isPT) return false;
            document.form.submit();
        }
    }
    return true;
}
function CheckPotionTakingAgent() {
    
    var IsChaneAuto = CheckChangeAutoAgent();
    //var IsChaneAuto = false;
    if (IsChaneAuto) {
        //member maximum position taking		
        $("maxMemberPT").value = parseFloat($('maxM1HdpD').value) - parseFloat($("listM1HdpD").value);
        if (!flag) {
            DisableControls();
            ageWnd.open('ConfirmEditAgent.aspx', '', 300, 300, 500, 200);
            flag = true;
        }
        return false;
    }
    else {
        $("memberPT").value = 0;
        $("isUpdateMember").value = 0;
        if (!flag) {
            flag = true;
            DelayReloadPage(null, 300000);
        }
    }
    return true;
}
function CheckChangeAutoAgent() {
    
    var elements = $("divUpdPT1").getElementsByTagName("input");
    //this script for Multiple Edit and allow update PT				
    var isMult = ($("isMult").value == "1" ? true : false);
    if (isMult) {
        var sportId = ["1", "2", "3", "5", "8", "10", "11", "201", "99","161","99_M"];
        var isUpd;
        for (var i = 0; i < sportId.length; i++) {
            if ($("chkUpdPT" + sportId[i])) {
                if ($("chkUpdPT" + sportId[i]).checked) {
                    isUpd = true;
                    break;
                }
            }
        }
        if (isUpd && $("isDisableAutoMpt").value == "0") return true;
    }

    //----------------------------
    //this script to check for Soccer
    var isNewAuto;
    for (var i = 0; i < elements.length; i++) {
        var id = elements[i].id;
        if (id == "chkCopy1HdpDeadball" || id == "chkCopy11x2" || id == "chkCopy1HdpLive") {
            continue;
        }

        var isAutoId = id.replace("chkAuto", "isAuto");
        var isAuto = $(isAutoId).value == "1";
        if (isAuto && !elements[i].checked) {
            return true;
        }
    }
    //----------------------------				
    //this script to check for All Sporttype (except Soccer)
    sportId = ["2", "3", "5", "8", "10", "11", "201", "99", "161", "99_M"];
    for (var j = 0; j < sportId.length; j++) {
        if ($("divUpdPT" + sportId[j])) {
            elements = $("divUpdPT" + sportId[j]).getElementsByTagName("input");
            for (var i = 0; i < elements.length; i++) {

                var isAutoId = elements[i].id.replace("chkAuto", "isAuto");
                
                var isAuto = $(isAutoId).value == "1";
                //this script for Copy from Handicap is checked		
                //if(elements[i].id=="chkCopy" + sportId[j] + "Hdp") continue;	

                if ($("chkCopy" + sportId[j] + "Hdp") && $("chkCopy" + sportId[j] + "Hdp").checked == true) {
                    isNewAuto = elements[1].checked;
                }
                else if ((!$("chkCopy" + sportId[j] + "Hdp")) || ($("chkCopy" + sportId[j] + "Hdp") && $("chkCopy" + sportId[j] + "Hdp").checked == false)) {
                    isNewAuto = elements[i].checked;
                }
                if (isAuto && !isNewAuto) {
                    return true;
                }
            }
        }
    }
    //----------------------------			
    return false;
}
function CheckAutoPreset(chkAutoId, chkCopy) {

    var isChk = false;
    if (chkAutoId) {
        isChk = chkAutoId.checked;
    }    
    var isCopy = (chkCopy != null ? $(chkCopy).checked : false);
    var objPreset;
    var objAuto;
    var objMaster = '';
    if (chkAutoId) {
        objMaster = chkAutoId.id.replace("chkAuto", "listM");
    }
    if (isChk) {
        if (chkAutoId) {
            objPreset = chkAutoId.id.replace("chkAuto", "listS");
            objAuto = chkAutoId.id.replace("chkAuto", "autoS");
            chkAutoId.disabled = (isCopy ? isCopy : !isChk);
        }
    }
    else {
        if (chkAutoId) {
            objPreset = chkAutoId.id.replace("chkAuto", "listS");
            objAuto = chkAutoId.id.replace("chkAuto", "autoS");
            chkAutoId.disabled = (isCopy ? isCopy : isChk);
        }
    }
    if ($("chkCopyAll") != null) {
        if (!$("chkCopyAll").checked && !isCopy)	//this script for Copy All PT checkbox is checked
        {
            if ($(objMaster)) {
                $(objMaster).disabled = false;
            }
        }
    }
    if ($(objPreset)) {
        $(objPreset).disabled = (isCopy ? isCopy : isChk);
    }
    if ($(objAuto)) {
        $(objAuto).disabled = (isCopy ? isCopy : !isChk);
    }

    //******** may be this function is make IE slow *************		
    if (!gisCopyAll) {
        PresetValuePT(chkAutoId);
    }
    //***********************************************************
}
function PresetValuePT(chkAutoId) {

    var isAuto = chkAutoId.checked;
    var masterId = chkAutoId.id.replace("chkAuto", "listM");
    var maxMasterId = chkAutoId.id.replace("chkAuto", "maxA");
    var curMaster = $(masterId).value;

    var minMaster = $(masterId).options[$(masterId).length - 1].value;

    minMaster = parseInt(round(minMaster * 100));
    var maxMaster = $(maxMasterId).value;
    maxMaster = parseInt(round(maxMaster * 100));

    if (!isAuto) {
        var superId = chkAutoId.id.replace("chkAuto", "listS");
        var maxMSId = chkAutoId.id.replace("chkAuto", "maxM");
        var curSuper = parseFloat($(superId).value);
        var maxMS = parseFloat($(maxMSId).value);
        var msVal = maxMS - curSuper;
        msVal = parseInt(round(msVal * 100));
        maxMaster = (maxMaster > msVal ? msVal : maxMaster);
        var tmp = parseFloat(curSuper) + parseFloat(curMaster);
        if (tmp > maxMS) curMaster = round((maxMS - curSuper));
    }
    $(masterId).length = 0;
    var isRange = true;

    if ((curMaster * 100) > maxMaster) {
        isRange = false;
        $(masterId).options.add(new Option(curMaster, curMaster));
    }
    while (maxMaster >= minMaster) {
        var val = maxMaster / 100;
        $(masterId).options.add(new Option(val, val));
        maxMaster -= 1;
    }

    if (isRange) {
        $(masterId).value = curMaster;
    }
}

function ActiveSuperPreset(isCopyBasket) {
    var arrId1 = "";
    var isUpdPT1 = "";
    var isMult = "";
    if ($("divUpdPT1"))
        arrId1 = $("divUpdPT1").getElementsByTagName("input");    
    isUpdPT1 = ($("chkUpdPT1") != null ? $("chkUpdPT1").checked : true);    
    isMult = ($("isMult") != null ? $("isMult").value == "1" : false);
    
    var chkCopyId;
    //this case for Enable or disable control of Soccer
    
    if (!isCopyBasket) {
        if (isUpdPT1) {
           
            for (var i = 0; i < arrId1.length; i++) {

                if (arrId1[i].id.indexOf("chkCopy1") > -1 || arrId1[i].id.indexOf("chkUpd") > -1) continue;
                switch (arrId1[i].id) {
                    case "chkAuto1HdpD":
                        {

                            gisCopyAll = true;
                            CheckAutoPreset(arrId1[i]);
                            chkCopyId = "chkCopy1HdpDeadball";
                            break;
                        }
                    case "chkAuto11x2":
                        {
                            gisCopyAll = true;
                            CheckAutoPreset(arrId1[i]);
                            chkCopyId = "chkCopy11x2";
                            break;
                        }
                    case "chkAuto1HdpL":
                        {
                            gisCopyAll = true;
                            CheckAutoPreset(arrId1[i]);
                            chkCopyId = "chkCopy1HdpLive";
                            break;
                        }
                    default:
                        {

                            gisCopyAll = true;
                            //CheckAutoPreset(arrId1[i]);
                            CheckAutoPreset(arrId1[i], chkCopyId);
                            break;
                        }
                }
                $("chkCopy1HdpDeadball").disabled = false;
                $("chkCopy11x2").disabled = false;
                $("chkCopy1HdpLive").disabled = false;
            }
        }
        gisCopyAll = false;
    }

    if ($("divUpdPT161")) {
        var arrId161 = $("divUpdPT161").getElementsByTagName("input");
        for (var i = 0; i < arrId161.length; i++) {

            if (arrId161[i].id.indexOf("chkCopy") > -1 || arrId161[i].id.indexOf("chkUpd") > -1) continue;
            switch (arrId161[i].id) {
                case "chkAuto1611stOUD":
                    {
                        gisCopyAll = true;
                        CheckAutoPreset(arrId161[i]);
                        chkCopyId = "chkCopy1611stOUD";
                        break;
                    }
                case "chkAuto161OUL":
                    {
                        gisCopyAll = true;
                        CheckAutoPreset(arrId161[i]);
                        chkCopyId = "chkCopy161OUL";
                        break;
                    }
                default:
                    {

                        gisCopyAll = true;
                        CheckAutoPreset(arrId161[i], chkCopyId);
                        break;
                    }
            }
            $("chkCopy1611stOUD").disabled = false;
            $("chkCopy161OUL").disabled = false;
        }
    }


    if ($("divUpdPT99_M")) {
        var arrId99_M = $("divUpdPT99_M").getElementsByTagName("input");
        for (var i = 0; i < arrId99_M.length; i++) {
            switch (arrId99_M[i].id) {
                case "chkAuto99_M":
                    {
                        gisCopyAll = true;
                        CheckAutoPreset(arrId99_M[i]);
                        break;
                    }
            }
        }
    }
    
    //this case for Enable or disable control of all Sporttypes (except Soccer)
    var sportId = ["2", "3", "5", "8", "10", "11", "201", "99", "TO", "FO"];
    for (var i = 0; i < sportId.length; i++) {
        //this script to check PositionTaking checkbox is check or not
        isUpdPT1 = ($("chkUpdPT" + sportId[i]) != null ? $("chkUpdPT" + sportId[i]).checked : true);
        if (!isUpdPT1) continue;
        //--------------------------
        var pre = "Hdp";
        if (i == 6) pre = "OU";
        if (i == 8 || i == 9) pre = "Win";       
        
        if ($("divUpdPT" + sportId[i])) {
            var arrId2 = $("divUpdPT" + sportId[i]).getElementsByTagName("input");

            for (var j = 0; j < arrId2.length; j++) {
                if (arrId2[j].id.indexOf("chkCopy") > -1 || arrId2[j].id.indexOf("chkUpd") > -1) continue;
                if (arrId2[j].id == "chkAuto" + sportId[i] + pre) {
                    gisCopyAll = true;
                    CheckAutoPreset(arrId2[j]);
                }
                else {
                    gisCopyAll = true;
                    if ($("chkCopy" + sportId[i] + pre))
                        CheckAutoPreset(arrId2[j], "chkCopy" + sportId[i] + pre);
                    else
                        CheckAutoPreset(arrId2[j]);
                }
                if ($("chkCopy" + sportId[i] + pre)) {
                    $("chkCopy" + sportId[i] + pre).disabled = false;
                }
                if ($("chkCopy" + sportId[i] + "Preset")) {
                    $("chkCopy" + sportId[i] + "Preset").disabled = false;
                }
            }
        }
    }
    gisCopyAll = false;
}