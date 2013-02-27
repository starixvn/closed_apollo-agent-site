//global variables:
var gisCopyAll = false;
//this function is used to enable or disable controls in Multiple edit case


function AllowUpdate(chkObj, isBet) {
    var divId = chkObj.id;
    if (chkObj.id == "chkUpdInfo161") {
        $("txtMaxPerBall").disabled = $("txtMinbet161").disabled = $("txtMaxbet161").disabled = $("txtMaxPerMatch161").disabled = !chkObj.checked;
        return;
    }

    if (divId == "chkUpdStatus") {
        $("chkClosed").disabled = !chkObj.checked;

        return;
    }


    divId = divId.replace("chk", "div");

    var isMult = $('isMult') ? $('isMult').value : "0";

    var element;
    if (!chkObj.checked || chkObj.id == "chkUpdComm") {

        element = $(divId).getElementsByTagName("input");
        for (var i = 1; i < element.length; i++) {
            element[i].disabled = !chkObj.checked;
        }
        if (!isBet) {
            element = $(divId).getElementsByTagName("select");
            for (var i = 0; i < element.length; i++) {
                element[i].disabled = !chkObj.checked;
            }
        }
    }
    else {	//this case for Bet Setting checkbox is checked or Position Taking checkbox is checked (Super case)

        element = $(divId).getElementsByTagName("input");
        for (var i = 0; i < element.length; i++) {
            element[i].disabled = !chkObj.checked;
            if (!isBet) {
                var id = element[i].id;
                if (id.indexOf("chkCopy") > -1) continue;
                var masterId = id.replace("chkAuto", "listM");
                var superId = id.replace("chkAuto", "listS");
                var autoId = id.replace("chkAuto", "autoS");
                $(masterId).disabled = !chkObj.checked;
                $(superId).disabled = $(id).checked;
                $(autoId).disabled = !$(id).checked;
            }
        }
        // end case
        if (isBet) return;

        if (chkObj.id == "chkUpdPT1") {
            // Insert by David 15-02-2012

            if ($('chkCopyAll').checked) {

                if ($('chkUpdInfo1').checked) {
                    $('txtMinbet1').disabled = false;
                    $('txtMaxbet1').disabled = false;
                    $('txtMaxPerMatch1').disabled = false;
                }

                var elm = $("divUpdPT1").getElementsByTagName("select");
                for (var i = 0; i < elm.length; i++) {
                    var id = elm[i].id;
                    if (id == "listM1HdpD" || id == "listS1HdpD" || id == "autoS1HdpD") continue;
                    elm[i].disabled = true;
                }

                var elements_input = $("divUpdPT1").getElementsByTagName("input");
                for (var j = 0; j < elements_input.length; j++) {
                    var id = elements_input[j].id;
                    if (id == "chkAuto1HdpD" || id == "chkUpdPT1") continue;
                    elements_input[j].disabled = true;
                }
            }
            else {
                CheckCopyPT($("chkCopy1HdpDeadball"), arr1Super1, arr1Master1);
                CheckCopyPT($("chkCopy1HdpDeadball"), arr1Super1, arr1Master1);
                CheckCopyPT($("chkCopy11x2"), arr2Super1, arr2Master1);
                CheckCopyPT($("chkCopy1HdpLive"), arr3Super1, arr3Master1);
            }
            return;

        }

        // Insert by David 15-02-2012

        if (chkObj.id == "chkUpdPT2") {
            if ($('chkCopyBasket').checked) {

                if ($('chkUpdInfo2').checked) {
                    $('txtMinbet2').disabled = false;
                    $('txtMaxbet2').disabled = false;
                    $('txtMaxPerMatch2').disabled = false;
                }

                var elm = $("divUpdPT2").getElementsByTagName("select");
                for (var i = 0; i < elm.length; i++) {
                    var id = elm[i].id;
                    if (id == "listM2Hdp" || id == "listS2Hdp" || id == "autoS2Hdp") continue;
                    elm[i].disabled = true;
                }

                var elements_input = $("divUpdPT2").getElementsByTagName("input");
                for (var j = 0; j < elements_input.length; j++) {
                    var id = elements_input[j].id;
                    if (id == "chkAuto2Hdp" || id == "chkUpdPT2") continue;
                    elements_input[j].disabled = true;
                }
            }
            else {
                CheckCopyPT($("chkCopy2Hdp"), arrSuper2, arrMaster2);
            }
            return;
        }

        //

        if (chkObj.id == "chkUpdPT161") {
            CheckCopyPT($("chkCopy1611stOUD"), arr161Super81, arr161Master81);
            CheckCopyPT($("chkCopy161OUL"), arr161Super85, arr161Master85);
            return;
        }
        if (chkObj.id == "chkUpdPT99_M") {
            CheckCopyPT("", arrSuper99_M, arrMaster99_M);
            return;
        }

        var sId = "";
        var arrSuper;
        var arrMaster;
        switch (chkObj.id) {
            case "chkUpdPT3":
                {
                    sId = "chkCopy3Hdp";
                    arrSuper = arrSuper3;
                    arrMaster = arrMaster3;
                    break;
                }
            case "chkUpdPT5":
                {
                    sId = "chkCopy5Hdp";
                    arrSuper = arrSuper5;
                    arrMaster = arrMaster5;
                    break;
                }
            case "chkUpdPT8":
                {
                    sId = "chkCopy8Hdp";
                    arrSuper = arrSuper8;
                    arrMaster = arrMaster8;
                    break;
                }
            case "chkUpdPT10":
                {
                    sId = "chkCopy10Hdp";
                    arrSuper = arrSuper10;
                    arrMaster = arrMaster10;
                    break;
                }
            case "chkUpdPT11":
                {
                    sId = "chkCopy11Hdp";
                    arrSuper = arrSuper11;
                    arrMaster = arrMaster11;
                    break;
                }
            case "chkUpdPT201":
                {
                    sId = "chkCopy201OU";
                    arrSuper = arrSuper201;
                    arrMaster = arrMaster201;
                    break;
                }
            case "chkUpdPT99":
                {
                    sId = "chkCopy99Hdp";
                    arrSuper = arrSuper99;
                    arrMaster = arrMaster99;
                    break;
                }
        }
        CheckCopyPT($(sId), arrSuper, arrMaster);
    }
}

var flag = false;

function BtnOnClick() {
    var arrAcc;
    var arrPAcc;
    var arrMin;
    if (!CheckInfoValue(true)) return false;
    var IsMinMax = CheckMinMax();
    if (!IsMinMax) return false;
    var isPT = CheckPotionTaking();
    if (!isPT) return false;
    $('form').submit();
    return true;
}

function CheckPotionTaking() {
    var IsChaneAuto = CheckChangeAuto();
    if (IsChaneAuto) {
        //member maximum position taking		
        $("maxMemberPT").value = parseFloat($('maxMS1HdpD').value) - parseFloat($("listM1HdpD").value);
        if (!flag) {
            DisableControls();
            ageWnd.open('ConfirmEditMaster.aspx', '', 300, 300, 500, 200);
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
            enabledBtn(false);
        }
    }
    return true;
}

function CheckMinMax() {


    //check info for Soccer
    if (!CheckLimitMinMaxBet(1)) return false;
    //-----end----	
    if ($("isMult").value == "1" || !$("chkCopyAll").checked) {
        //check info for Basketball
        if (!CheckLimitMinMaxBet(2)) return false;
        //-----end----
        if ($("isMult").value == "1" || !$("chkCopyBasket").checked) {
            if ($('txtMaxPerBall')) {
                if (!CheckLimitMaxPerBall()) return false;
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
            //check info for Financial
            if ($('txtMinbet201') || $('txtMaxbet201') || $('txtMaxPerMatch201')) {
                if (!CheckLimitMinMaxBet(201)) return false;
            }
            //-----end----
            //check info for Other Sports
            if ($('txtMinbet161') || $('txtMaxbet161') || $('txtMaxPerMatch161')) {
                if (!CheckLimitMinMaxBet(161)) return false;
            }
            //-----end----

            //check info for Other Sports

            if (!CheckLimitMinMaxBet(99)) return false;

            //-----end----

            //check info for Other Sports
            if ($('txtMinbet99_M') || $('txtMaxbet99_M') || $('txtMaxPerMatch99_M')) {
                if (!CheckLimitMinMaxBet('99_M')) return false;
            }
            //-----end----			
        }
    }
    return true;
}

//this function is Override that is used for Edit case.
var count = 0;
function CheckAutoPreset(chkAutoId, chkCopy, interval) {
    if (interval != 10) interval = 100;
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
        PresetValuePT(chkAutoId, interval);
    }
    //***********************************************************
}
//this function is used for Copy All PT checkbox is unchecked
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
                    CheckAutoPreset(arrId2[j], "chkCopy" + sportId[i] + pre);
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

//this function is used to Set value for dropdown:
//when Auto checkbox is checked
//			value Master Preset PT dropdown display from min Master value to max Master value 
//else
//			value Master Preset PT dropdown display from min Master value to min(max Master value, maxMS-current Super) 			
function PresetValuePT(chkAutoId, interval) {

    if (interval != 10) interval = 100;
    var isAuto = chkAutoId.checked;
    var masterId = chkAutoId.id.replace("chkAuto", "listM");
    var maxMasterId = chkAutoId.id.replace("chkAuto", "maxM");

    var curMaster = $(masterId).value;
    var minMaster = $(masterId).options[$(masterId).length - 1].value;
    minMaster = parseInt(round(minMaster * interval));
    var maxMaster = $(maxMasterId).value;
    maxMaster = parseInt(round(maxMaster * interval));
    if (!isAuto) {
        var superId = chkAutoId.id.replace("chkAuto", "listS");
        var maxMSId = chkAutoId.id.replace("chkAuto", "maxMS");
        var curSuper = parseFloat($(superId).value);
        var maxMS = parseFloat($(maxMSId).value);
        var msVal = maxMS - curSuper;
        msVal = parseInt(round(msVal * interval));
        maxMaster = (maxMaster > msVal ? msVal : maxMaster);
        var tmp = parseFloat(curSuper) + parseFloat(curMaster);
        if (tmp > maxMS) curMaster = round((maxMS - curSuper));
    }
    $(masterId).length = 0;
    var isRange = true;
    if ((curMaster * interval) > maxMaster) {
        isRange = false;
        $(masterId).options.add(new Option(curMaster, curMaster));
    }
    while (maxMaster >= minMaster) {
        var val = maxMaster / interval;
        $(masterId).options.add(new Option(val, val));
        maxMaster -= 1;
    }

    if (isRange) {
        $(masterId).value = curMaster;
    }
}
//this function is used to check
//have any PT value change from auto to none auto (alert)
function CheckChangeAuto() {
    var elements = $("divUpdPT1").getElementsByTagName("input");
    //this script for Multiple Edit and allow update PT				
    var isMult = ($("isMult").value == "1" ? true : false);
    if (isMult) {
        var sportId = ["1", "2", "3", "5", "8", "10", "11", "201", "161", "99", "161", "99_M"];
        var isUpd;
        for (var i = 0; i < sportId.length; i++) {
            if ($("chkUpdPT" + sportId[i])) {
                if ($("chkUpdPT" + sportId[i]).checked) {
                    isUpd = true;
                    break;
                }
            }
        }

        if (isUpd) return true;
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
    sportId = ["2", "3", "5", "8", "10", "11", "201", "161", "99", "161", "99_M"];
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
//this function to check min PT value 
//
function CheckMinPTValue() {
    var elements;
    var isCopyAll = $("chkCopyAll").checked;
    //this script to check for Soccer
    elements = $("divUpdPT1").getElementsByTagName("input");
    for (var i = 0; i < elements.length; i++) {
        var chkId = elements[i].id;
        if (chkId.indexOf("chkCopy1") > -1) continue;
    }
}

//this function is used to 
//disable all controls in form when Confirm Edit PT of All Member is display
function DisableControls() {
    var isDisable = "";
    for (var i = 0; i < document.form.elements.length; i++) {
        if (document.form.elements[i].disabled == false) {

            isDisable += 1 + ',';
            document.form.elements[i].disabled = true;
        }
        else {
            isDisable += 0 + ',';
        }
    }
    $("saveControlDisabled").value = isDisable;
}

//View Downline for CasinoPositionTaking
function ViewDL(type, custid, user) {
    document.location.href = '../MemberInfo/CasinoPositionTaking.aspx?type=' + type + '&custid=' + custid + '&USER=' + user;
}

//View Downline for BingoPositionTaking
function ViewBingoDL(type, custid, user) {
    document.location.href = '../MemberInfo/BingoPositionTaking.aspx?type=' + type + '&custid=' + custid + '&USER=' + user;
}

function InProcess(isDisable) {
    var arrBtn = ['btnUpdateInfo', 'btnUpdCredit', 'btnUpdSuspend', 'btnUdpClosed', 'btnUdpComm', 'btnUdpMixMaxBet', 'btnUdpPT', 'btnUdp'];
    for (i = 0; i < arrBtn.length; i++) {
        $(arrBtn[i]).disabled = isDisable;
    }
    if (isDisable) DelayReloadPage(null, 300000); ;
}

function UdpCustInfo() {
    InProcess(true);
    $('typeUdp').value = 'UdpCustInfo';
    $('form').submit();
}

function UdpMaxCredit() {
    if (!CheckInfoValue(true)) return false;
    InProcess(true);
    $('typeUdp').value = 'UdpCredit';
    $('form').submit();
}

function UdpSuspend() {
    InProcess(true);
    $('typeUdp').value = 'UdpSuspend';
    $('form').submit();
}

function UdpStatus() {
    InProcess(true);
    $('typeUdp').value = 'UdpStatus';
    $('form').submit();
}

function UdpComm() {
    InProcess(true);
    $('typeUdp').value = 'UdpComm';
    $('form').submit();
}

function UdpMinMax() {
    if (!CheckMinMax()) return false;
    InProcess(true);
    $('typeUdp').value = 'UdpMinMax';
    $('form').submit();
}

function UdpPT() {
    $('typeUdp').value = 'UdpPT';
    var site = $('site') ? $('site').value : '';
    if (site == 'master') {
        if (!CheckPotionTaking()) return false;
    }
    InProcess(true);
    $('form').submit();
}

function UdpAll() {
    $('typeUdp').value = 'UdpAll';
    if (!CheckInfoValue(true)) return false;
    if (!CheckMinMax()) return false;

    var site = $('site') ? $('site').value : '';
    if (site == 'master') {
        if (!CheckPotionTaking()) return false;
    }
    InProcess(true);
    $('form').submit();
}

function viewPT(page, agenttype, custId, userName) {
    switch (page) {
        case "1":
            window.location.href = 'PositionTaking.aspx?type=' + agenttype + '&custid=' + custId + '&USER=' + userName;
            break;
        case "2":
            window.location.href = 'PositionTakingRacing.aspx?type=' + agenttype + '&custid=' + custId + '&USER=' + userName;
            break;
        case "3":
            window.location.href = 'PositionTakingNumberGame.aspx?type=' + agenttype + '&custid=' + custId + '&USER=' + userName;
            break;
    }
}


