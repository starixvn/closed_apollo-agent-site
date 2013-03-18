//global variables:
var gisCopyAll = false;
//this function is used to enable or disable controls in Multiple edit case


function AllowUpdate(chkObj, isBet) {
    var divId = chkObj.id;
    if (chkObj.id == "chkUpdInfo161") {
        document.getElementById("txtMaxPerBall").disabled = document.getElementById("txtMinbet161").disabled = document.getElementById("txtMaxbet161").disabled = document.getElementById("txtMaxPerMatch161").disabled = !chkObj.checked;
        return;
    }

    if (divId == "chkUpdStatus") {
        document.getElementById("chkClosed").disabled = !chkObj.checked;

        return;
    }


    divId = divId.replace("chk", "div");

    var isMult = document.getElementById('isMult') ? document.getElementById('isMult').value : "0";

    var element;
    if (!chkObj.checked || chkObj.id == "chkUpdComm") {

        element = document.getElementById(divId).getElementsByTagName("input");
        for (var i = 1; i < element.length; i++) {
            element[i].disabled = !chkObj.checked;
        }
        if (!isBet) {
            element = document.getElementById(divId).getElementsByTagName("select");
            for (var i = 0; i < element.length; i++) {
                element[i].disabled = !chkObj.checked;
            }
        }
    }
    else {	//this case for Bet Setting checkbox is checked or Position Taking checkbox is checked (Super case)

        element = document.getElementById(divId).getElementsByTagName("input");
        for (var i = 0; i < element.length; i++) {
            element[i].disabled = !chkObj.checked;
            if (!isBet) {
                var id = element[i].id;
                if (id.indexOf("chkCopy") > -1) continue;
                var masterId = id.replace("chkAuto", "listM");
                var superId = id.replace("chkAuto", "listS");
                var autoId = id.replace("chkAuto", "autoS");
                document.getElementById(masterId).disabled = !chkObj.checked;
                document.getElementById(superId).disabled = document.getElementById(id).checked;
                document.getElementById(autoId).disabled = !document.getElementById(id).checked;
            }
        }
        // end case
        if (isBet) return;

        if (chkObj.id == "chkUpdPT1") {
            // Insert by David 15-02-2012

            if (document.getElementById('chkCopyAll').checked) {

                if (document.getElementById('chkUpdInfo1').checked) {
                    document.getElementById('txtMinbet1').disabled = false;
                    document.getElementById('txtMaxbet1').disabled = false;
                    document.getElementById('txtMaxPerMatch1').disabled = false;
                }

                var elm = document.getElementById("divUpdPT1").getElementsByTagName("select");
                for (var i = 0; i < elm.length; i++) {
                    var id = elm[i].id;
                    if (id == "listM1HdpD" || id == "listS1HdpD" || id == "autoS1HdpD") continue;
                    elm[i].disabled = true;
                }

                var elements_input = document.getElementById("divUpdPT1").getElementsByTagName("input");
                for (var j = 0; j < elements_input.length; j++) {
                    var id = elements_input[j].id;
                    if (id == "chkAuto1HdpD" || id == "chkUpdPT1") continue;
                    elements_input[j].disabled = true;
                }
            }
            else {
                CheckCopyPT(document.getElementById("chkCopy1HdpDeadball"), arr1Super1, arr1Master1);
                CheckCopyPT(document.getElementById("chkCopy1HdpDeadball"), arr1Super1, arr1Master1);
                CheckCopyPT(document.getElementById("chkCopy11x2"), arr2Super1, arr2Master1);
                CheckCopyPT(document.getElementById("chkCopy1HdpLive"), arr3Super1, arr3Master1);
            }
            return;

        }

        // Insert by David 15-02-2012

        if (chkObj.id == "chkUpdPT2") {
            if (document.getElementById('chkCopyBasket').checked) {

                if (document.getElementById('chkUpdInfo2').checked) {
                    document.getElementById('txtMinbet2').disabled = false;
                    document.getElementById('txtMaxbet2').disabled = false;
                    document.getElementById('txtMaxPerMatch2').disabled = false;
                }

                var elm = document.getElementById("divUpdPT2").getElementsByTagName("select");
                for (var i = 0; i < elm.length; i++) {
                    var id = elm[i].id;
                    if (id == "listM2Hdp" || id == "listS2Hdp" || id == "autoS2Hdp") continue;
                    elm[i].disabled = true;
                }

                var elements_input = document.getElementById("divUpdPT2").getElementsByTagName("input");
                for (var j = 0; j < elements_input.length; j++) {
                    var id = elements_input[j].id;
                    if (id == "chkAuto2Hdp" || id == "chkUpdPT2") continue;
                    elements_input[j].disabled = true;
                }
            }
            else {
                CheckCopyPT(document.getElementById("chkCopy2Hdp"), arrSuper2, arrMaster2);
            }
            return;
        }

        //

        if (chkObj.id == "chkUpdPT161") {
            CheckCopyPT(document.getElementById("chkCopy1611stOUD"), arr161Super81, arr161Master81);
            CheckCopyPT(document.getElementById("chkCopy161OUL"), arr161Super85, arr161Master85);
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
        CheckCopyPT(document.getElementById(sId), arrSuper, arrMaster);
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
    document.getElementById('form').submit();
    return true;
}

function CheckPotionTaking() {
    var IsChaneAuto = CheckChangeAuto();
    if (IsChaneAuto) {
        //member maximum position taking		
        document.getElementById("maxMemberPT").value = parseFloat(document.getElementById('maxMS1HdpD').value) - parseFloat(document.getElementById("listM1HdpD").value);
        if (!flag) {
            DisableControls();
            ageWnd.open('ConfirmEditMaster.aspx', '', 300, 300, 500, 200);
            flag = true;
        }
        return false;
    }
    else {
        document.getElementById("memberPT").value = 0;
        document.getElementById("isUpdateMember").value = 0;
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
    if (document.getElementById("isMult").value == "1" || !document.getElementById("chkCopyAll").checked) {
        //check info for Basketball
        if (!CheckLimitMinMaxBet(2)) return false;
        //-----end----
        if (document.getElementById("isMult").value == "1" || !document.getElementById("chkCopyBasket").checked) {
            if (document.getElementById('txtMaxPerBall')) {
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
            if (document.getElementById('txtMinbet201') || document.getElementById('txtMaxbet201') || document.getElementById('txtMaxPerMatch201')) {
                if (!CheckLimitMinMaxBet(201)) return false;
            }
            //-----end----
            //check info for Other Sports
            if (document.getElementById('txtMinbet161') || document.getElementById('txtMaxbet161') || document.getElementById('txtMaxPerMatch161')) {
                if (!CheckLimitMinMaxBet(161)) return false;
            }
            //-----end----

            //check info for Other Sports

            if (!CheckLimitMinMaxBet(99)) return false;

            //-----end----

            //check info for Other Sports
            if (document.getElementById('txtMinbet99_M') || document.getElementById('txtMaxbet99_M') || document.getElementById('txtMaxPerMatch99_M')) {
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

    var isCopy = (chkCopy != null ? document.getElementById(chkCopy).checked : false);
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
    if (document.getElementById("chkCopyAll") != null) {
        if (!document.getElementById("chkCopyAll").checked && !isCopy)	//this script for Copy All PT checkbox is checked
        {
            if (document.getElementById(objMaster)) {
                document.getElementById(objMaster).disabled = false;
            }
        }
    }
    if (document.getElementById(objPreset)) {

        document.getElementById(objPreset).disabled = (isCopy ? isCopy : isChk);
    }
    if (document.getElementById(objAuto)) {
        document.getElementById(objAuto).disabled = (isCopy ? isCopy : !isChk);
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
    if (document.getElementById("divUpdPT1"))
        arrId1 = document.getElementById("divUpdPT1").getElementsByTagName("input");
    isUpdPT1 = (document.getElementById("chkUpdPT1") != null ? document.getElementById("chkUpdPT1").checked : true);
    isMult = (document.getElementById("isMult") != null ? document.getElementById("isMult").value == "1" : false);
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
                document.getElementById("chkCopy1HdpDeadball").disabled = false;
                document.getElementById("chkCopy11x2").disabled = false;
                document.getElementById("chkCopy1HdpLive").disabled = false;
            }
        }

        gisCopyAll = false;
    }

    if (document.getElementById("divUpdPT161")) {
        var arrId161 = document.getElementById("divUpdPT161").getElementsByTagName("input");
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
            document.getElementById("chkCopy1611stOUD").disabled = false;
            document.getElementById("chkCopy161OUL").disabled = false;
        }
    }


    if (document.getElementById("divUpdPT99_M")) {
        var arrId99_M = document.getElementById("divUpdPT99_M").getElementsByTagName("input");
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
        isUpdPT1 = (document.getElementById("chkUpdPT" + sportId[i]) != null ? document.getElementById("chkUpdPT" + sportId[i]).checked : true);
        if (!isUpdPT1) continue;
        //--------------------------
        var pre = "Hdp";
        if (i == 6) pre = "OU";
        if (i == 8 || i == 9) pre = "Win";

        if (document.getElementById("divUpdPT" + sportId[i])) {
            var arrId2 = document.getElementById("divUpdPT" + sportId[i]).getElementsByTagName("input");

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
                if (document.getElementById("chkCopy" + sportId[i] + pre)) {
                    document.getElementById("chkCopy" + sportId[i] + pre).disabled = false;
                }
                if (document.getElementById("chkCopy" + sportId[i] + "Preset")) {
                    document.getElementById("chkCopy" + sportId[i] + "Preset").disabled = false;
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

    var curMaster = document.getElementById(masterId).value;
    var minMaster = document.getElementById(masterId).options[document.getElementById(masterId).length - 1].value;
    minMaster = parseInt(round(minMaster * interval));
    var maxMaster = document.getElementById(maxMasterId).value;
    maxMaster = parseInt(round(maxMaster * interval));
    if (!isAuto) {
        var superId = chkAutoId.id.replace("chkAuto", "listS");
        var maxMSId = chkAutoId.id.replace("chkAuto", "maxMS");
        var curSuper = parseFloat(document.getElementById(superId).value);
        var maxMS = parseFloat(document.getElementById(maxMSId).value);
        var msVal = maxMS - curSuper;
        msVal = parseInt(round(msVal * interval));
        maxMaster = (maxMaster > msVal ? msVal : maxMaster);
        var tmp = parseFloat(curSuper) + parseFloat(curMaster);
        if (tmp > maxMS) curMaster = round((maxMS - curSuper));
    }
    document.getElementById(masterId).length = 0;
    var isRange = true;
    if ((curMaster * interval) > maxMaster) {
        isRange = false;
        document.getElementById(masterId).options.add(new Option(curMaster, curMaster));
    }
    while (maxMaster >= minMaster) {
        var val = maxMaster / interval;
        document.getElementById(masterId).options.add(new Option(val, val));
        maxMaster -= 1;
    }

    if (isRange) {
        document.getElementById(masterId).value = curMaster;
    }
}
//this function is used to check
//have any PT value change from auto to none auto (alert)
function CheckChangeAuto() {
    var elements = document.getElementById("divUpdPT1").getElementsByTagName("input");
    //this script for Multiple Edit and allow update PT				
    var isMult = (document.getElementById("isMult").value == "1" ? true : false);
    if (isMult) {
        var sportId = ["1", "2", "3", "5", "8", "10", "11", "201", "161", "99", "161", "99_M"];
        var isUpd;
        for (var i = 0; i < sportId.length; i++) {
            if (document.getElementById("chkUpdPT" + sportId[i])) {
                if (document.getElementById("chkUpdPT" + sportId[i]).checked) {
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
        var isAuto = document.getElementById(isAutoId).value == "1";
        if (isAuto && !elements[i].checked) {
            return true;
        }
    }
    //----------------------------				
    //this script to check for All Sporttype (except Soccer)
    sportId = ["2", "3", "5", "8", "10", "11", "201", "161", "99", "161", "99_M"];
    for (var j = 0; j < sportId.length; j++) {
        if (document.getElementById("divUpdPT" + sportId[j])) {
            elements = document.getElementById("divUpdPT" + sportId[j]).getElementsByTagName("input");
            for (var i = 0; i < elements.length; i++) {

                var isAutoId = elements[i].id.replace("chkAuto", "isAuto");
                var isAuto = document.getElementById(isAutoId).value == "1";
                //this script for Copy from Handicap is checked		
                //if(elements[i].id=="chkCopy" + sportId[j] + "Hdp") continue;	

                if (document.getElementById("chkCopy" + sportId[j] + "Hdp") && document.getElementById("chkCopy" + sportId[j] + "Hdp").checked == true) {
                    isNewAuto = elements[1].checked;
                }
                else if ((!document.getElementById("chkCopy" + sportId[j] + "Hdp")) || (document.getElementById("chkCopy" + sportId[j] + "Hdp") && document.getElementById("chkCopy" + sportId[j] + "Hdp").checked == false)) {
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
    var isCopyAll = document.getElementById("chkCopyAll").checked;
    //this script to check for Soccer
    elements = document.getElementById("divUpdPT1").getElementsByTagName("input");
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
    document.getElementById("saveControlDisabled").value = isDisable;
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
        document.getElementById(arrBtn[i]).disabled = isDisable;
    }
    if (isDisable) DelayReloadPage(null, 300000); ;
}

function UdpCustInfo() {
    InProcess(true);
    document.getElementById('typeUdp').value = 'UdpCustInfo';
    document.getElementById('form').submit();
}

function UdpMaxCredit() {
    if (!CheckInfoValue(true)) return false;
    InProcess(true);
    document.getElementById('typeUdp').value = 'UdpCredit';
    document.getElementById('form').submit();
}

function UdpSuspend() {
    InProcess(true);
    document.getElementById('typeUdp').value = 'UdpSuspend';
    document.getElementById('form').submit();
}

function UdpStatus() {
    InProcess(true);
    document.getElementById('typeUdp').value = 'UdpStatus';
    document.getElementById('form').submit();
}

function UdpComm() {
    InProcess(true);
    document.getElementById('typeUdp').value = 'UdpComm';
    document.getElementById('form').submit();
}

function UdpMinMax() {
    if (!CheckMinMax()) return false;
    InProcess(true);
    document.getElementById('typeUdp').value = 'UdpMinMax';
    document.getElementById('form').submit();
}

function UdpPT() {
    document.getElementById('typeUdp').value = 'UdpPT';
    var site = document.getElementById('site') ? document.getElementById('site').value : '';
    if (site == 'master') {
        if (!CheckPotionTaking()) return false;
    }
    InProcess(true);
    document.getElementById('form').submit();
}

function UdpAll() {
    document.getElementById('typeUdp').value = 'UdpAll';
    if (!CheckInfoValue(true)) return false;
    if (!CheckMinMax()) return false;

    var site = document.getElementById('site') ? document.getElementById('site').value : '';
    if (site == 'master') {
        if (!CheckPotionTaking()) return false;
    }
    InProcess(true);
    document.getElementById('form').submit();
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


