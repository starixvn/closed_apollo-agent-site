//this function is called in onload event of body tag
// to get Custid string & Username string from MasterList page for the case Multiple Edit 
//assign Username string to span username
function GetCustID() {
    var isMult = (document.getElementById("isMult") != null ? document.getElementById("isMult").value : "");
    if (isMult == "0") return true;
    var strCustid = parent.document.getElementById("arrayCustID"); // hidden field from MasterList page
    var objCustid = document.getElementById("saveCustid");
    if (strCustid) {
        if (strCustid.value != "") {
            objCustid.value = strCustid.value;
        }
    }
    var strUsername = parent.document.getElementById("arrayUserName"); // hidden field from MasterList page

    var objUsername = document.getElementById("saveUsername");
    if (strUsername) {
        if (strUsername.value != "") {
            var t = strUsername.value;
            while (t.indexOf("^") != -1) {
                t = t.replace("^", ", ");
            }
            objUsername.value = t.substring(0, t.length - 2);
            if (document.getElementById("username")) {
                document.getElementById("username").innerHTML = objUsername.value;
            }
        }
    }
}

//this function is used to visible or invisible sporttype area when menu sporttype is click
//panelId: id of <tr> tag that contains sporttype info
function OpenPanelEdit(panelId) {
    if (document.getElementById(panelId).style.display == "none") {
        document.getElementById(panelId).style.display = "";
    }
    else {
        document.getElementById(panelId).style.display = "none";
    }
}

function CheckLimitMaxPerBall() {
    var maxperballId = "txtMaxPerBall";
    var maxperballMaxId = "txtMaxPerBall_Max";

    if (isNumeric(maxperballId) < 0) {
        var alertMsg = "";
        try {
            var alertMsg = alertMaxperBallNegative;
        }
        catch (ex) {
            alertMsg = "Max per Ball must be greater than zero";
        }
        document.getElementById(maxperballId).focus();
        ShowBalloonMsg(maxperballId, alertMsg);
        return false;
    }

    if (isEmpty(maxperballId) || isString(maxperballId)) {
        document.getElementById(maxperballId).focus();
        ShowBalloonMsg(maxperballId, alertMaxperball);
        return false;
    }


    if (isNumeric(maxperballId) != 0 && isNumericSpan(maxperballMaxId) != 0) {
        if (isNumeric(maxperballId) > isNumericSpan(maxperballMaxId)) {
            var str = "";
            str = alertMaxperballC.replace("{1}", formatNumber2(isNumericSpan(maxperballMaxId)));
            document.getElementById(maxperballId).focus();
            ShowBalloonMsg(maxperballId, str);
            return false;
        }
    }

    return true;
}

//this function to check value input of min bet, max bet, max per match
//min bet <=maxbet <= max per match
//sporttypeId: is of sporttype
function CheckLimitMinMaxBet(sporttypeId) {

    try {
        var minbetId = "txtMinbet" + sporttypeId;
        var maxbetId = "txtMaxbet" + sporttypeId;
        var maxpermatchId = "txtMaxPerMatch" + sporttypeId;
        var lmMinbetId = "txtMinbet_Min" + sporttypeId;
        var lmMaxbetId = "txtMaxbet_Max" + sporttypeId;
        var maxpermatchMinId = "txtMaxPerMatch_Min" + sporttypeId;
        var maxpermatchMaxId = "txtMaxPerMatch_Max" + sporttypeId;

        if (isEmpty(minbetId) || isString(minbetId)) {

            ShowError(alertMinbet, "error" + sporttypeId);
            document.getElementById(minbetId).focus();
            ShowBalloonMsg(minbetId, alertMinbet);
            return false;
        }

        //phuong.nguyen
        //Check if input value (min, max, max per match) are negative
        //start

        if (isNumeric(minbetId) < 0) {

            var alertMsg = "";
            try {
                var alertMsg = alertMinbetNegative;
            }
            catch (ex) {
                alertMsg = "Min Bet must be greater than zero";
            }
            ShowError(alertMsg, "error" + sporttypeId);
            document.getElementById(minbetId).focus();
            ShowBalloonMsg(minbetId, alertMsg);
            return false;
        }

        if (isNumeric(maxbetId) < 0) {
            var alertMsg = "";
            try {
                var alertMsg = alertMaxbetNegative;
            }
            catch (ex) {
                alertMsg = "Max Bet must be greater than zero";
            }
            ShowError(alertMsg, "error" + sporttypeId);
            document.getElementById(maxbetId).focus();
            ShowBalloonMsg(maxbetId, alertMsg);
            return false;
        }

        if (isNumeric(maxpermatchId) < 0) {
            var alertMsg = "";
            try {
                var alertMsg = alertMaxperMatchNegative;
            }
            catch (ex) {
                alertMsg = "Max per Match must be greater than zero";
            }
            ShowError(alertMsg, "error" + sporttypeId);
            document.getElementById(maxpermatchId).focus();
            ShowBalloonMsg(maxpermatchId, alertMsg);
            return false;
        }
        //end	


        if (isNumeric(minbetId) != 0 && isNumericSpan(lmMinbetId) != 0 && isNumeric(minbetId) < isNumericSpan(lmMinbetId)) {
            ShowError(alertminbetC + " " + formatNumber2(isNumericSpan(lmMinbetId)), "error" + sporttypeId);
            document.getElementById(minbetId).focus();
            ShowBalloonMsg(minbetId, alertminbetC + " " + formatNumber2(isNumericSpan(lmMinbetId)));
            return false;
        }

        if (isEmpty(maxbetId) || isString(maxbetId)) {
            ShowError(alertMaxbet, "error" + sporttypeId);
            document.getElementById(maxbetId).focus();
            ShowBalloonMsg(maxbetId, alertMaxbet);
            return false;
        }

        if (isNumeric(maxbetId) != 0 && isNumericSpan(lmMaxbetId) != 0 && isNumeric(maxbetId) > isNumericSpan(lmMaxbetId)) {

            ShowError(alertMaxbetC + " " + formatNumber2(isNumericSpan(lmMaxbetId)), "error" + sporttypeId);
            document.getElementById(maxbetId).focus();
            ShowBalloonMsg(maxbetId, alertMaxbetC + " " + formatNumber2(isNumericSpan(lmMaxbetId)));
            return false;
        }

        if (isNumeric(minbetId) > isNumeric(maxbetId)) {
            ShowError(alertCompare3, "error" + sporttypeId);
            document.getElementById(minbetId).focus();
            ShowBalloonMsg(minbetId, alertCompare3);
            return false;
        }

        if (isEmpty(maxpermatchId) || isString(maxpermatchId)) {
            ShowError(alertCompare3, "error" + sporttypeId);
            document.getElementById(maxpermatchId).focus();
            ShowBalloonMsg(maxpermatchId, alertMaxpermatch);
            return false;
        }

        if (isNumeric(maxbetId) > isNumeric(maxpermatchId)) {
            ShowError(alertMaxpermatch, "error" + sporttypeId);
            document.getElementById(maxbetId).focus();
            ShowBalloonMsg(maxbetId, alertCompare);
            return false;
        }

        if (isNumeric(maxpermatchId) != 0 && isNumericSpan(maxpermatchMaxId) != 0) {
            if (isNumeric(maxpermatchId) > isNumericSpan(maxpermatchMaxId)) {
                var str = "";
                str = alertMaxpermatchC.replace("{1}", formatNumber2(isNumericSpan(maxpermatchMaxId)));
                ShowError(str, "error" + sporttypeId);
                document.getElementById(maxpermatchId).focus();
                ShowBalloonMsg(maxpermatchId, str);
                return false;
            }
        }
    }
    catch (ex) {
        alert(ex.message);
        return false;
    }
    return true;
}

function CheckLimitMinMaxBetForMember(sporttypeId) {

    try {
        var minbetId = "txtMinbet" + sporttypeId;
        var maxbetId = "txtMaxbet" + sporttypeId;
        var maxpermatchId = "txtMaxPerMatch" + sporttypeId;
        var lmMinbetId = "txtMinbet_Min" + sporttypeId;
        var lmMaxbetId = "txtMaxbet_Max" + sporttypeId;
        var maxpermatchMinId = "txtMaxPerMatch_Min" + sporttypeId;
        var maxpermatchMaxId = "txtMaxPerMatch_Max" + sporttypeId;

        if (isEmpty(minbetId) || isString(minbetId)) {

            ShowError(alertMinbet, "error" + sporttypeId);
            document.getElementById(minbetId).focus();
            ShowBalloonMsg(minbetId, alertMinbet);
            return false;
        }

        //phuong.nguyen
        //Check if input value (min, max, max per match) are negative
        //start

        if (isNumeric(minbetId) < 0) {

            var alertMsg = "";
            try {
                var alertMsg = alertMinbetNegative;
            }
            catch (ex) {
                alertMsg = "Min Bet must be greater than zero";
            }
            ShowError(alertMsg, "error" + sporttypeId);
            document.getElementById(minbetId).focus();
            ShowBalloonMsg(minbetId, alertMsg);
            return false;
        }

        if (isNumeric(maxbetId) < 0) {
            var alertMsg = "";
            try {
                var alertMsg = alertMaxbetNegative;
            }
            catch (ex) {
                alertMsg = "Max Bet must be greater than zero";
            }
            ShowError(alertMsg, "error" + sporttypeId);
            document.getElementById(maxbetId).focus();
            ShowBalloonMsg(maxbetId, alertMsg);
            return false;
        }

        if (isNumeric(maxpermatchId) < 0) {
            var alertMsg = "";
            try {
                var alertMsg = alertMaxperMatchNegative;
            }
            catch (ex) {
                alertMsg = "Max per Match must be greater than zero";
            }
            ShowError(alertMsg, "error" + sporttypeId);
            document.getElementById(maxpermatchId).focus();
            ShowBalloonMsg(maxpermatchId, alertMsg);
            return false;
        }
        //end

        if (isNumeric(minbetId) == 0 && isNumeric(maxbetId) == 0 && isNumeric(maxpermatchId) == 0)
            return true;
        else {
            if (isNumeric(minbetId) <= 0 || isNumeric(minbetId) < isNumericSpan(lmMinbetId)) {
                ShowError(alertminbetC + " " + formatNumber2(isNumericSpan(lmMinbetId)), "error" + sporttypeId);
                document.getElementById(minbetId).focus();
                ShowBalloonMsg(minbetId, alertminbetC + " " + formatNumber2(isNumericSpan(lmMinbetId)));
                return false;
            }
        }

        if (isEmpty(maxbetId) || isString(maxbetId)) {
            ShowError(alertMaxbet, "error" + sporttypeId);
            document.getElementById(maxbetId).focus();
            ShowBalloonMsg(maxbetId, alertMaxbet);
            return false;
        }

        if (isNumeric(maxbetId) != 0 && isNumericSpan(lmMaxbetId) != 0 && isNumeric(maxbetId) > isNumericSpan(lmMaxbetId)) {

            ShowError(alertMaxbetC + " " + formatNumber2(isNumericSpan(lmMaxbetId)), "error" + sporttypeId);
            document.getElementById(maxbetId).focus();
            ShowBalloonMsg(maxbetId, alertMaxbetC + " " + formatNumber2(isNumericSpan(lmMaxbetId)));
            return false;
        }

        if (isNumeric(minbetId) > isNumeric(maxbetId)) {
            ShowError(alertCompare3, "error" + sporttypeId);
            document.getElementById(minbetId).focus();
            ShowBalloonMsg(minbetId, alertCompare3);
            return false;
        }

        if (isEmpty(maxpermatchId) || isString(maxpermatchId)) {
            ShowError(alertCompare3, "error" + sporttypeId);
            document.getElementById(maxpermatchId).focus();
            ShowBalloonMsg(maxpermatchId, alertMaxpermatch);
            return false;
        }
        if (isNumeric(maxbetId) > isNumeric(maxpermatchId)) {
            ShowError(alertMaxpermatch, "error" + sporttypeId);
            document.getElementById(maxbetId).focus();
            ShowBalloonMsg(maxbetId, alertCompare);
            return false;
        }
        if (isNumeric(maxpermatchId) != 0 && isNumericSpan(maxpermatchMaxId) != 0) {
            if (isNumeric(maxpermatchId) > isNumericSpan(maxpermatchMaxId)) {
                var str = "";
                str = alertMaxpermatchC.replace("{1}", formatNumber2(isNumericSpan(maxpermatchMaxId)));
                ShowError(str, "error" + sporttypeId);
                document.getElementById(maxpermatchId).focus();
                ShowBalloonMsg(maxpermatchId, str);
                return false;
            }
        }
    }
    catch (ex) {
        alert(ex.message);
        return false;
    }
    return true;
}


function ChangeValuePT(ddlAgent, currentMaster, maxMaster, maxAgent, minAgent) {
    var valueAgent = parseFloat(round(maxMaster - currentMaster));
    var maxA = parseFloat(round(maxAgent));
    if (valueAgent > maxA) {
        valueAgent = maxA;
    }
    var minAgent2 = 0;
    if (minAgent != null) {
        minAgent2 = parseInt(minAgent * 100);
    }
    document.getElementById(ddlAgent).options.length = 0;
    valueAgent = parseInt(valueAgent * 100);
    var isRange = false;
    while (valueAgent >= minAgent2) {
        isRange = true;
        var val = parseFloat(valueAgent / 100);
        document.getElementById(ddlAgent).options.add(new Option(val, val));
        valueAgent = valueAgent - 1;
    }
    if (!isRange) document.getElementById(ddlAgent).options.add(new Option(minAgent2 / 100, minAgent2 / 100));
}

//this function is used to increase value in PT dropdown when increase button is clicked
//ddl: id of PT dropdown that want to reduce
//step: gap that is increase in each click times (it's 1 or 10)
//when pt value of account is changed it's not effect to upline
//when pt value of upline is changed, PT value of current account is changed following
function IncreasePT(ddl, step) {
    var max = document.getElementById(ddl).options[0].value;
    var roleId = (document.getElementById("roleId") != null ? document.getElementById("roleId").value : "");
    if (ddl.indexOf("listM") > -1) {
        Increase(ddl, step, max);
    }
    else if (ddl.indexOf("autoS") > -1) {
        Increase(ddl, step, max);
    }
    else if (ddl.indexOf("listS") > -1) {
        if (Increase(ddl, step, max)) {
            var ddlMaster = ddl.replace("listS", "listM");
            var hidMMax = ddl.replace("listS", "maxM");
            var hidMsMax = ddl.replace("listS", "maxMS");
            var hidMinM = ddl.replace("listS", "minM");
            switch (roleId) {
                case "3": //this case for Master
                    {
                        hidMMax = ddl.replace("listS", "maxA");
                        hidMsMax = ddl.replace("listS", "maxM");
                        hidMinM = document.getElementById(ddlMaster).options[document.getElementById(ddlMaster).length - 1].value;
                        hidMsMax = document.getElementById(hidMsMax).value;
                        hidMMax = document.getElementById(hidMMax).value;
                        break;
                    }
                default:
                    {
                        hidMMax = ddl.replace("listS", "maxM");
                        hidMsMax = ddl.replace("listS", "maxMS");
                        hidMinM = ddl.replace("listS", "listM");
                        hidMinM = document.getElementById(hidMinM).options[document.getElementById(hidMinM).length - 1].value;
                        hidMsMax = document.getElementById(hidMsMax).value;
                        hidMMax = document.getElementById(hidMMax).value;
                        break;
                    }
            }
            var currentSuper = document.getElementById(ddl).value;
            ChangeValuePT(ddlMaster, currentSuper, hidMsMax, hidMMax, hidMinM);
        }
    }
}

//this function is used to reduce value in PT dropdown when reduce button is clicked
//ddl: id of PT dropdown that want to reduce
//step: gap that is reduce in each click times (it's 1 or 10)
//when pt value of account is changed it's not effect to upline
//when pt value of upline is changed, PT value of current account is changed following
function ReducePT(ddl, step) {
    var min = 0;
    var roleId = (document.getElementById("roleId") != null ? document.getElementById("roleId").value : "");
    if (ddl.indexOf("autoS") > -1) {
        Reduce(ddl, step, min);
    }
    else if (ddl.indexOf("listM") > -1) {
        min = document.getElementById(ddl).options[document.getElementById(ddl).length - 1].value;
        Reduce(ddl, step, min);
    }
    else if (ddl.indexOf("listS") > -1) {
        var ddlMaster = ddl.replace("listS", "listM");
        var hidMMax;
        var hidMsMax;
        var hidMinM;
        var maxMaster = document.getElementById(ddl).options[0].value;
        switch (roleId) {
            case "3":
                {
                    hidMMax = ddl.replace("listS", "maxA");
                    hidMsMax = ddl.replace("listS", "maxM");
                    hidMinM = document.getElementById(ddlMaster).options[document.getElementById(ddlMaster).length - 1].value;
                    hidMsMax = document.getElementById(hidMsMax).value;
                    hidMMax = document.getElementById(hidMMax).value;
                    break;
                }
            default:
                {
                    hidMMax = ddl.replace("listS", "maxM");
                    hidMsMax = ddl.replace("listS", "maxMS");
                    hidMinM = ddl.replace("listS", "listM");
                    hidMinM = document.getElementById(hidMinM).options[document.getElementById(hidMinM).length - 1].value;
                    hidMsMax = document.getElementById(hidMsMax).value;
                    hidMMax = document.getElementById(hidMMax).value;
                    break;
                }
        }
        if (Reduce(ddl, step, min)) {
            currentSuper = document.getElementById(ddl).value;
            ChangeValuePT(ddlMaster, currentSuper, hidMsMax, hidMMax, hidMinM);
        }
    }
}


// this function is used to increase value of dropdown when increase button is clicked
//ddl: id of Positiontaking dropdown 
//step: value of each step (1 or 10)
// max : max value which dropdown can increase to			
function Increase(ddl, step, max) {
    var objDDL = document.getElementById(ddl);
    var maxValue = Math.round(max * 100);
    var currSelect = Math.round(parseFloat(objDDL.value) * 100);
    var nextValue = currSelect + step;
    for (var i = 0; i < objDDL.length; i++) {
        if (objDDL[i].value == (nextValue / 100.0)) {
            objDDL[i].selected = true;
            return true;
        }
    }
    return false;
}

// this function is used to Reduce value of dropdown when button is clicked
//ddl: id of Positiontaking dropdown 
//step: value of each step (1 or 10)
// min : min value which dropdown can reduce to
function Reduce(ddl, step, min) {
    var objDDL = document.getElementById(ddl);
    var minValue = Math.round(min * 100);
    var currSelect = Math.round(parseFloat(objDDL.value) * 100);
    var nextValue = currSelect - step;
    for (var i = 0; i < objDDL.length; i++) {
        if (objDDL[i].value == (nextValue / 100.0)) {
            objDDL[i].selected = true;
            return true;
        }
    }
    return false;
}
//this function is used to check Password and Max Credit 
//in Editing case just check Max Credit
function CheckInfoValue(isEdit) {
    var isMult = (document.getElementById("isMult") != null ? document.getElementById("isMult").value : "");
    if (isMult == "1") return true;
    if (!isEdit) {
        if (isEmpty("txtPwd")) {
            ShowError(alertPasswordEmpty, "err");
            window.scroll(0, 0);
            document.getElementById("txtPwd").focus();
            return false;
        }

        if (!checkpass(document.getElementById("txtPwd"))) {
            ShowError(lblPasswordRequire, "err");
            window.scroll(0, 0);
            document.getElementById("txtPwd").focus();
            return false;
        }
    }
    if (isEmpty("txtCredit") || isString("txtCredit")) {
        ShowError(alertIncorrectCredit, "err");
        document.getElementById("txtCredit").focus();
        window.scroll(0, 0);
        return false;
    }
    if ((isNumeric("txtCredit") < isNumericSpan("txtCreditMin")) || (isNumeric("txtCredit") > isNumericSpan("txtCreditMax"))) {
        var valMin = isNumericSpan("txtCreditMin") ? document.getElementById("txtCreditMin").innerHTML : 0;

        var str = alertCreditInvalid.replace("{1}", valMin);
        str = str.replace("{2}", document.getElementById("txtCreditMax").innerHTML);
        ShowError(str, "err");
        window.scroll(0, 0);
        document.getElementById("txtCredit").focus()
        return false;
    }

    if (document.getElementById("txtMaxCredit")) {
        if (isEmpty("txtMaxCredit") || isString("txtMaxCredit")) {
            ShowError(alertIncorrectMemberMaxCredit, "err");
            window.scroll(0, 0);
            document.getElementById("txtMaxCredit").focus();
            return false;
        }
        if (isNumeric("txtMaxCredit") != 0 && ((isNumeric("txtMaxCredit") < isNumericSpan("lblMinCredit")) || (isNumeric("txtMaxCredit") > isNumericSpan("lblMaxCredit")))) {
            var str = alertMemberCreditInvalid.replace("{1}", isNumericSpan("lblMinCredit") ? isNumericSpan("lblMinCredit") : 0);
            str = str.replace("{2}", isNumericSpan("lblMaxCredit"));
            ShowError(str, "err");
            window.scroll(0, 0);
            document.getElementById("txtMaxCredit").focus()
            return false;
        }
    }

    return true;
}

//this function is used to:
//check selected PT value of current account update & his upline can not less than min value
//arrDDL: array contains id of PT dropdowns for current account (Ex: Master)
//arrPDDL: array contains id of PT dropdowns for upline (Ex: Super)
//arrMin: array contains id of hidden fields that save min value
//errHolder: id of Label that display error according sportstype.
function CheckMinPTValue(arrDDL, arrPDDL, arrMin, errHolder) {
    for (var i = 0; i < arrDDL.length; i++) {
        var totalPT = isNumeric(arrDDL[i]) + isNumeric(arrPDDL[i]);
        var minVal = isNumeric(arrMin[i]);
        if (totalPT < minVal) {
            var msg = errMsgMinPT + minVal;
            ShowError(msg, errHolder);
            document.getElementById(arrDDL[i]).focus();
            return false;
        }
    }
    return true;
}

//this function is used to enable or disable controls when Copy All PT checkbox is checked (for Super)
//when Copy All Bet Setting & PT checkbox is checked
function CopyAllPT(chkCopyAll) {
    if (document.getElementById("chkCopyBasket")) {
        document.getElementById("chkCopyBasket").disabled = chkCopyAll.checked;
    }
    var isMult = (document.getElementById("isMult") == null ? false : (document.getElementById("isMult").value == "1"));
    var isEdit = (document.getElementById("hidEdit") == null ? false : (document.getElementById("hidEdit").value == "1"));
    var sportId = ["1", "2", "3", "5", "8", "10", "11", "201", "99", "TO", "FO", "99_M"];

    // For multiple
    if (isMult) {

        for (var i = 0; i < sportId.length; i++) {
            if (i == 0 || i == 10) continue;
            if (document.getElementById("divUpdInfo" + sportId[i])) {
                elements = document.getElementById("divUpdInfo" + sportId[i]).getElementsByTagName("input");
                document.getElementById('chkUpdInfo' + sportId[i]).disabled = chkCopyAll.checked;
                for (var j = 0; j < elements.length; j++) {
                    if (document.getElementById('chkUpdInfo' + sportId[i]).checked)
                        elements[j].disabled = chkCopyAll.checked;

                }
            }
        }


        if (chkCopyAll.checked) // This case of Copy all checkbox is checked
        {
            var isAuto = false;

            for (var i = 0; i < sportId.length; i++) {
                if (sportId[i] != "151") {
                    var elements = '';
                    var elements_input = '';
                    if (document.getElementById("divUpdPT" + sportId[i])) {
                        elements = document.getElementById("divUpdPT" + sportId[i]).getElementsByTagName("select");
                        elements_input = document.getElementById("divUpdPT" + sportId[i]).getElementsByTagName("input");
                        for (var j = 0; j < elements.length; j++) {

                            var id = elements[j].id;
                            if (id == "listM1HdpD" || id == "listS1HdpD" || id == "autoS1HdpD" || id == "listMFOWin" || id == "listSFOWin" || id == "autoSFOWin") continue;
                            elements[j].disabled = true;
                        }

                        for (var j = 0; j < elements_input.length; j++) {
                            var id = elements_input[j].id;
                            if (id == "chkAuto1HdpD" || id == "chkUpdPT1" || id == "chkAutoFOWin" || id == "chkCopyFOPreset") continue;
                            elements_input[j].disabled = true;
                        }
                    }
                }
            }

        }
        else // This case of Copy all checkbox is Unchecked
        {
            for (var i = 0; i < sportId.length; i++) {
                if (i == 0 || i == 10) continue;
                if (document.getElementById("divUpdInfo" + sportId[i])) {
                    elements = document.getElementById("divUpdInfo" + sportId[i]).getElementsByTagName("input");
                    document.getElementById('chkUpdInfo' + sportId[i]).disabled = chkCopyAll.checked;
                    for (var j = 0; j < elements.length; j++) {
                        if ((document.getElementById('chkUpdInfo' + sportId[i]).checked) && (j != 0))
                            elements[j].disabled = !document.getElementById("chkUpdInfo" + sportId[i]).checked ;

                    }
                }
            }
            for (var i = 0; i < sportId.length; i++) {
                if (document.getElementById('chkUpdPT' + sportId[i])) {
                    document.getElementById('chkUpdPT' + sportId[i]).disabled = false;
                    AllowUpdate(document.getElementById('chkUpdPT' + sportId[i]), false);
                }
            }
        }
       
        return;
    }
    // end For multiple


    if (!chkCopyAll.checked) //this case for Copy All PT checkbox is unchecked (add & edit)
    {
        if (document.getElementById("listM1HdpD"))
            document.getElementById("listM1HdpD").disabled = false;
        if (document.getElementById("listM11x2"))
            document.getElementById("listM11x2").disabled = false;
        if (document.getElementById("listM1HdpL"))
            document.getElementById("listM1HdpL").disabled = false;
        if (document.getElementById("listM2Hdp"))
            document.getElementById("listM2Hdp").disabled = false;
        if (document.getElementById("listM3Hdp"))
            document.getElementById("listM3Hdp").disabled = false;
        if (document.getElementById("listM5Hdp"))
            document.getElementById("listM5Hdp").disabled = false;
        if (document.getElementById("listM8Hdp"))
            document.getElementById("listM8Hdp").disabled = false;
        if (document.getElementById("listM10Hdp"))
            document.getElementById("listM10Hdp").disabled = false;
        if (document.getElementById("listM11Hdp"))
            document.getElementById("listM11Hdp").disabled = false;
        if (document.getElementById("listM201OU"))
            document.getElementById("listM201OU").disabled = false;
        if (document.getElementById("listM99Hdp"))
            document.getElementById("listM99Hdp").disabled = false;
        if (document.getElementById("listM1611stOUD"))
            document.getElementById("listM1611stOUD").disabled = false;
        if (document.getElementById("listMTOWin"))
            document.getElementById("listMTOWin").disabled = false;
        if (document.getElementById("listMFOWin"))
            document.getElementById("listMFOWin").disabled = false;

        //this script to enable Min bet, Max Bet, Max Per Match textbox in Copy from Soccer case
        for (var i = 0; i < sportId.length; i++) {
            if (i == 0 || i == 10) continue;
            if (document.getElementById("divUpdInfo" + sportId[i])) {
                elements = document.getElementById("divUpdInfo" + sportId[i]).getElementsByTagName("input");
                for (var j = 0; j < elements.length; j++) {
                    elements[j].disabled = false;
                }
            }
        }
        ActiveSuperPreset();
    }
    else //this case for Copy All PT checkbox is checked (add & edit)
    {
        var isAuto = false;
        for (var i = 0; i < sportId.length; i++) {
            if (sportId[i] != "151") {
                var elements = '';
                if (document.getElementById("divUpdPT" + sportId[i])) {
                    elements = document.getElementById("divUpdPT" + sportId[i]).getElementsByTagName("select");
                    for (var j = 0; j < elements.length; j++) {

                        var id = elements[j].id;
                        if (id == "listM1HdpD" || id == "listS1HdpD" || id == "autoS1HdpD" || id == "listMFOWin" || id == "listSFOWin" || id == "autoSFOWin") continue;
                        elements[j].disabled = chkCopyAll.checked;
                    }
                }
                if (document.getElementById("divUpdPT" + sportId[i])) {
                    elements = document.getElementById("divUpdPT" + sportId[i]).getElementsByTagName("input");
                    for (var j = 0; j < elements.length; j++) {
                        var id = elements[j].id;
                        if (id == "chkAuto1HdpD" || id == "chkUpdPT" + sportId[i] || id == "chkAutoFOWin" || id == "chkCopyFOPreset") continue;
                        elements[j].disabled = chkCopyAll.checked;
                    }
                }
            }
            if (i == 0 || i == 10) continue;
            if (document.getElementById("divUpdInfo" + sportId[i])) {
                elements = document.getElementById("divUpdInfo" + sportId[i]).getElementsByTagName("input");
                for (var j = 0; j < elements.length; j++) {
                    elements[j].disabled = chkCopyAll.checked;
                }
            }
        }
    }
}

//this function is used to disbale or enable dropdown positon according Copy from Handicap/1x2/OddEven
function CheckCopyPT(chkCopy, arrSuper, arrMaster, chkAutoId) {
    var isEdit = (document.getElementById("hidEdit") != null ? document.getElementById("hidEdit").value == "1" : false);
    var roleId = (document.getElementById("roleId") != null ? document.getElementById("roleId").value : "");
    var isCopy;
    if (chkCopy) {
        isCopy = chkCopy.checked;
    }
    else {
        isCopy = false;
    }

    var isAuto = false;

    if (document.getElementById(chkAutoId)) {
        isAuto = (chkAutoId != null ? document.getElementById(chkAutoId).checked : false);
    }

    for (var i = 0; i < arrMaster.length; i++) {
        if (chkCopy) {
            if (document.getElementById(arrMaster[i])) {
                document.getElementById(arrMaster[i]).disabled = chkCopy.checked;
            }
        }
        else {
            if (document.getElementById(arrMaster[i])) {
                document.getElementById(arrMaster[i]).disabled = false;
            }
        }
        var id = arrSuper[i].replace("listS", "autoS");
        var chkId = arrSuper[i].replace("listS", "chkAuto");
        gisCopyAll = true;
        if (chkCopy) {
            CheckAutoPreset(document.getElementById(chkId), chkCopy.id);
        }
        gisCopyAll = false;
    }
}

function GetArrSoccer(chkCopyId) {
    var arrAcc;
    var arrPAcc;
    var arrMin;
    //this case for Copy All PT checkbox is checked
    if (document.getElementById("chkCopyAll").checked) {
        switch (chkCopyId) {
            case "chkCopy1HdpDeadball":
                {
                    arrMin = ["minMS1HdpD", "minMS1OUD", "minMS11stHdpD", "minMS11stOUD", "minMS1OE"];
                    arrAcc = ["listM1HdpD", "listM1HdpD", "listM1HdpD", "listM1HdpD", "listM1HdpD"];
                    if (!document.getElementById("chkAuto1").checked) {
                        arrPAcc = ["listS1HdpD", "listS1HdpD", "listS1HdpD", "listS1HdpD", "listS1HdpD"];
                    }
                    else {
                        arrPAcc = ["autoS1HdpD", "autoS1HdpD", "autoS1HdpD", "autoS1HdpD", "autoS1HdpD"];
                    }
                    break;
                }
            case "chkCopy11x2":
                {
                    arrAcc = ["listM1HdpD", "listM1HdpD", "listM1HdpD", "listM1HdpD", "listM1HdpD"];
                    arrMin = ["minMS11x2", "minMS1CS", "minMS1TG", "minMS1MP", "minMS1OR"];
                    if (!document.getElementById("chkAuto1").checked) {
                        arrPAcc = ["listS1HdpD", "listS1HdpD", "listS1HdpD", "listS1HdpD", "listS1HdpD"];
                    }
                    else {
                        arrPAcc = ["autoS1HdpD", "autoS1HdpD", "autoS1HdpD", "autoS1HdpD", "autoS1HdpD"];
                    }
                    break;
                }
            case "chkCopy1HdpLive":
                {
                    arrAcc = ["listM1HdpD", "listM1HdpD", "listM1HdpD", "listM1HdpD"];
                    arrMin = ["minMS1HdpL", "minMS1OUL", "minMS11stHdpL", "minMS11stOUL"];
                    if (!document.getElementById("chkAuto1").checked) {
                        arrPAcc = ["listS1HdpD", "listS1HdpD", "listS1HdpD", "listS1HdpD"];
                    }
                    else {
                        arrPAcc = ["autoS1HdpD", "autoS1HdpD", "autoS1HdpD", "autoS1HdpD"];
                    }
                    break;
                }
        }
        this.arr1 = arrAcc;
        this.arr2 = arrPAcc;
        this.arr3 = arrMin
        return [this.arr1, this.arr2, this.arr3];
    } //-------------end case ---------------------
    switch (chkCopyId) {
        case "chkCopy1HdpDeadball":
            {
                if (document.getElementById("chkCopy1HdpDeadball").checked) {
                    arrAcc = ["listM1HdpD", "listM1HdpD", "listM1HdpD", "listM1HdpD", "listM1HdpD"];
                    arrMin = ["minMS1HdpD", "minMS1OUD", "minMS11stHdpD", "minMS11stOUD", "minMS1OE"];
                    if (!document.getElementById("chkAuto1").checked) {
                        arrPAcc = ["listS1HdpD", "listS1HdpD", "listS1HdpD", "listS1HdpD", "listS1HdpD"];
                    }
                    else {
                        arrPAcc = ["autoS1HdpD", "autoS1HdpD", "autoS1HdpD", "autoS1HdpD", "autoS1HdpD"];
                    }
                }
                else {
                    arrAcc = ["listM1HdpD", "listM1OUD", "listM11stHdpD", "listM11stOUD", "listM1OE"];
                    arrMin = ["minMS1HdpD", "minMS1OUD", "minMS11stHdpD", "minMS11stOUD", "minMS1OE"];
                    if (!document.getElementById("chkAuto1").checked) {
                        arrPAcc = ["listS1HdpD", "listS1OUD", "listS11stHdpD", "listS11stOUD", "listS1OE"];
                    }
                    else {
                        arrPAcc = ["autoS1HdpD", "autoS1OUD", "autoS11stHdpD", "autoS11stOUD", "autoS1OE"];
                    }
                }
                break;
            }
        case "chkCopy11x2":
            {
                if (document.getElementById("chkCopy11x2").checked) {
                    arrAcc = ["listM11x2", "listM11x2", "listM11x2", "listM11x2", "listM11x2"];
                    arrMin = ["minMS11x2", "minMS1CS", "minMS1TG", "minMS1MP", "minMS1OR"];
                    if (!document.getElementById("chkAuto1").checked) {
                        arrPAcc = ["listS11x2", "listS11x2", "listS11x2", "listS11x2", "listS11x2"];
                    }
                    else {
                        arrPAcc = ["autoS11x2", "autoS11x2", "autoS11x2", "autoS11x2", "autoS11x2"];
                    }
                }
                else {
                    arrAcc = ["listM11x2", "listM1CS", "listM1TG", "listM1MP", "listM1OR"];
                    arrMin = ["minMS11x2", "minMS1CS", "minMS1TG", "minMS1MP", "minMS1OR"];
                    if (!document.getElementById("chkAuto1").checked) {
                        arrPAcc = ["listS11x2", "listS1CS", "listS1TG", "listS1MP", "listS1OR"];
                    }
                    else {
                        arrPAcc = ["autoS11x2", "autoS1CS", "autoS1TG", "autoS1MP", "autoS1OR"];
                    }
                }
                break;
            }
        case "chkCopy1HdpLive":
            {
                if (document.getElementById("chkCopy1HdpLive").checked) {
                    arrAcc = ["listM1HdpL", "listM1HdpL", "listM1HdpL", "listM1HdpL"];
                    arrMin = ["minMS1HdpL", "minMS1OUL", "minMS11stHdpL", "minMS11stOUL"];
                    if (!document.getElementById("chkAuto1").checked) {
                        arrPAcc = ["listS1HdpL", "listS1HdpL", "listS1HdpL", "listS1HdpL"];
                    }
                    else {
                        arrPAcc = ["autoS1HdpL", "autoS1HdpL", "autoS1HdpL", "autoS1HdpL"];
                    }
                }
                else {
                    arrAcc = ["listM1HdpL", "listM1OUL", "listM11stHdpL", "listM11stOUL"];
                    arrMin = ["minMS1HdpL", "minMS1OUL", "minMS11stHdpL", "minMS11stOUL"];
                    if (!document.getElementById("chkAuto1").checked) {
                        arrPAcc = ["listS1HdpL", "listS1OUL", "listS11stHdpL", "listS11stOUL"];
                    }
                    else {
                        arrPAcc = ["autoS1HdpL", "autoS1OUL", "autoS11stHdpL", "autoS11stOUL"];
                    }
                }
                break;
            }
    }
    this.arr1 = arrAcc;
    this.arr2 = arrPAcc;
    this.arr3 = arrMin
    return [this.arr1, this.arr2, this.arr3];
}

//this function to get arrays that will use in checking Min Positon
//use for basketball, football, tennis, golf, other sports 
function GetArr4(sporttypeId) {
    var arrAcc;
    var arrPAcc;
    var arrMin;
    arrMin = ["minMS" + sporttypeId + "Hdp", "minMS" + sporttypeId + "OU", "minMS" + sporttypeId + "OE", "minMS" + sporttypeId + "ML"];
    //this case is used in Copy All PT checkbox is checked
    if (document.getElementById("chkCopyAll").checked) {
        arrAcc = ["listM1HdpD", "listM1HdpD", "listM1HdpD", "listM1HdpD"];
        if (!document.getElementById("chkAuto1").checked) {
            arrPAcc = ["listS1HdpD", "listS1HdpD", "listS1HdpD", "listS1HdpD"];
        }
        else {
            arrPAcc = ["autoS1HdpD", "autoS1HdpD", "autoS1HdpD", "autoS1HdpD"];
        }
        this.arr1 = arrAcc;
        this.arr2 = arrPAcc;
        this.arr3 = arrMin
        return [this.arr1, this.arr2, this.arr3];
    } //------- end case -----------
    if (document.getElementById("chkCopy" + sporttypeId + "Hdp").checked) {
        arrAcc = ["listM" + sporttypeId + "Hdp", "listM" + sporttypeId + "Hdp", "listM" + sporttypeId + "Hdp", "listM" + sporttypeId + "Hdp"];
        if (!document.getElementById("chkAuto" + sporttypeId).checked) {
            arrPAcc = ["listS" + sporttypeId + "Hdp", "listS" + sporttypeId + "Hdp", "listS" + sporttypeId + "Hdp", "listS" + sporttypeId + "Hdp"];
        }
        else {
            arrPAcc = ["autoS" + sporttypeId + "Hdp", "autoS" + sporttypeId + "Hdp", "autoS" + sporttypeId + "Hdp", "autoS" + sporttypeId + "Hdp"];
        }
    }
    else {
        arrAcc = ["listM" + sporttypeId + "Hdp", "listM" + sporttypeId + "OU", "listM" + sporttypeId + "OE", "listM" + sporttypeId + "ML"];
        if (!document.getElementById("chkAuto" + sporttypeId).checked) {
            arrPAcc = ["listS" + sporttypeId + "Hdp", "listS" + sporttypeId + "OU", "listS" + sporttypeId + "OE", "listS" + sporttypeId + "ML"];
        }
        else {
            arrPAcc = ["autoS" + sporttypeId + "Hdp", "autoS" + sporttypeId + "OU", "autoS" + sporttypeId + "OE", "autoS" + sporttypeId + "ML"];
        }
    }
    this.arr1 = arrAcc;
    this.arr2 = arrPAcc;
    this.arr3 = arrMin
    return [this.arr1, this.arr2, this.arr3];
}

function GetArrBaseball() {
    var arrAcc;
    var arrPAcc;
    var arrMin;
    arrMin = ["minMS8Hdp", "minMS8OU", "minMS8ML"];
    if (document.getElementById("chkCopyAll").checked) {
        arrAcc = ["listM1HdpD", "listM1HdpD", "listM1HdpD"];
        if (!document.getElementById("chkAuto1").checked) {
            arrPAcc = ["listS1HdpD", "listS1HdpD", "listS1HdpD"];
        }
        else {
            arrPAcc = ["autoS1HdpD", "autoS1HdpD", "autoS1HdpD"];
        }
        this.arr1 = arrAcc;
        this.arr2 = arrPAcc;
        this.arr3 = arrMin
        return [this.arr1, this.arr2, this.arr3];
    }

    if (document.getElementById("chkCopy8Hdp").checked) {
        arrAcc = ["listM8Hdp", "listM8Hdp", "listM8Hdp"];
        if (!document.getElementById("chkAuto8").checked) {
            arrPAcc = ["listS8Hdp", "listS8Hdp", "listS8Hdp"];
        }
        else {
            arrPAcc = ["autoS8Hdp", "autoS8Hdp", "autoS8Hdp"];
        }
    }
    else {
        arrAcc = ["listM8Hdp", "listM8OU", "listM8ML"];
        if (!document.getElementById("chkAuto8").checked) {
            arrPAcc = ["listS8Hdp", "listS8OU", "listS8ML"];
        }
        else {
            arrPAcc = ["autoS8Hdp", "autoS8OU", "autoS8ML"];
        }
    }
    this.arr1 = arrAcc;
    this.arr2 = arrPAcc;
    this.arr3 = arrMin
    return [this.arr1, this.arr2, this.arr3];
}

function GetArrFinancial() {
    var arrAcc;
    var arrPAcc;
    var arrMin;
    arrMin = ["minMS12OE", "minMS12FO", "minMS12FLO"];
    if (document.getElementById("chkCopyAll").checked) {
        arrAcc = ["listM1HdpD", "listM1HdpD", "listM1HdpD"];
        if (!document.getElementById("chkAuto1").checked) {
            arrPAcc = ["listS1HdpD", "listS1HdpD", "listS1HdpD"];
        }
        else {
            arrPAcc = ["autoS1HdpD", "autoS1HdpD", "autoS1HdpD"];
        }
        this.arr1 = arrAcc;
        this.arr2 = arrPAcc;
        this.arr3 = arrMin
        return [this.arr1, this.arr2, this.arr3];
    }

    if (document.getElementById("chkCopy12Hdp").checked) {
        arrAcc = ["listM12OE", "listM12OE", "listM12OE"];
        if (!document.getElementById("chkAuto8").checked) {
            arrPAcc = ["listS12OE", "listS12OE", "listS12OE"];
        }
        else {
            arrPAcc = ["autoS12OE", "autoS12OE", "autoS12OE"];
        }
    }
    else {
        arrAcc = ["listM12OE", "listM12FO", "listM12FLO"];
        if (!document.getElementById("chkAuto8").checked) {
            arrPAcc = ["listS12OE", "listS12FO", "listS12FLO"];
        }
        else {
            arrPAcc = ["autoS12OE", "autoS12FO", "autoS12FLO"];
        }
    }
    this.arr1 = arrAcc;
    this.arr2 = arrPAcc;
    this.arr3 = arrMin
    return [this.arr1, this.arr2, this.arr3];
}

function GetArrMoto() {
    var arrAcc;
    var arrPAcc;
    var arrMin;
    arrMin = ["minMS11Hdp", "minMS11ML"];
    if (document.getElementById("chkCopyAll").checked) {
        arrAcc = ["listM1HdpD", "listM1HdpD"];
        if (!document.getElementById("chkAuto1").checked) {
            arrPAcc = ["listS1HdpD", "listS1HdpD"];
        }
        else {
            arrPAcc = ["autoS1HdpD", "autoS1HdpD"];
        }
        this.arr1 = arrAcc;
        this.arr2 = arrPAcc;
        this.arr3 = arrMin
        return [this.arr1, this.arr2, this.arr3];
    }
    if (document.getElementById("chkCopy11Hdp").checked) {
        arrAcc = ["listM11Hdp", "listM11Hdp"];
        if (!document.getElementById("chkAuto11").checked) {
            arrPAcc = ["listS11Hdp", "listS11Hdp"];
        }
        else {
            arrPAcc = ["autoS11Hdp", "autoS11Hdp"];
        }
    }
    else {
        arrAcc = ["listM11Hdp", "listM11ML"];
        if (!document.getElementById("chkAuto11").checked) {
            arrPAcc = ["listS11Hdp", "listS11ML"];
        }
        else {
            arrPAcc = ["autoS11Hdp", "autoS11ML"];
        }
    }
    this.arr1 = arrAcc;
    this.arr2 = arrPAcc;
    this.arr3 = arrMin
    return [this.arr1, this.arr2, this.arr3];
}
//this function is used to disabled controls of all Sporttype except Soccer (for Super)
//when Copy All Bet Setting and PT from Basketball
function CopyBasketPT(chkCopy) {
    var isEdit = (document.getElementById("hidEdit") != null ? true : false);
    var isMult = (document.getElementById("isMult") != null ? document.getElementById("isMult").value == "1" : false)
    var isCopyBas = chkCopy.checked;
    var sportId = ["2", "3", "5", "8", "10", "11", "201", "99", "99_M"];
    document.getElementById("chkCopyAll").disabled = chkCopy.checked;
    if (chkCopy.checked) {
        document.getElementById("panelBasket").style.display = "";
    }

    // For Multiple
    if (isMult) {
        for (var i = 0; i < sportId.length; i++) {
            if (i == 0) continue;
            if (document.getElementById("divUpdInfo" + sportId[i])) {
                ele = document.getElementById("divUpdInfo" + sportId[i]).getElementsByTagName("input");
                for (var j = 0; j < ele.length; j++) {
                    if (document.getElementById("divUpdInfo" + sportId[i])) {
                        ele[j].disabled = chkCopy.checked;
                    }
                }
            }

        }
        if (isCopyBas) // Checked
        {
          
            for (var i = 0; i < sportId.length; i++) {

                if (document.getElementById("divUpdPT" + sportId[i]))
                    elements = document.getElementById("divUpdPT" + sportId[i]).getElementsByTagName("input");

                for (var j = 0; j < elements.length; j++) {
                    var id = elements[j].id;
                    if (id == "chkAuto2Hdp" || id == "chkUpdPT2") continue;
                    elements[j].disabled = chkCopy.checked;
                }

                if (document.getElementById("divUpdPT" + sportId[i]))
                    elements_select = document.getElementById("divUpdPT" + sportId[i]).getElementsByTagName("select");

                if (document.getElementById("chkCopy" + sportId[i] + "Hdp")) {
                    document.getElementById("chkCopy" + sportId[i] + "Hdp").disabled = chkCopy.checked;
                }
                for (var j = 0; j < elements_select.length; j++) {
                    var id = elements_select[j].id;
                    if (id == "listM2Hdp" || id == "listS2Hdp" || id == "autoS2Hdp") continue;
                    elements_select[j].disabled = chkCopy.checked;
                }
            }

          
        }
        else  //UnChecked
        {
            for (var i = 0; i < sportId.length; i++) {
                if (i == 0) continue;
                if (document.getElementById("divUpdInfo" + sportId[i])) {
                    ele = document.getElementById("divUpdInfo" + sportId[i]).getElementsByTagName("input");
                    for (var j = 0; j < ele.length; j++) {
                        if (document.getElementById("divUpdInfo" + sportId[i]) && (j != 0)) {
                            ele[j].disabled = !document.getElementById("chkUpdInfo" + sportId[i]).checked;
                        }
                    }
                }

            }
            for (var i = 0; i < sportId.length; i++) {
                if (document.getElementById('chkUpdPT' + sportId[i])) {
                    document.getElementById('chkUpdPT' + sportId[i]).disabled = false;
                    AllowUpdate(document.getElementById('chkUpdPT' + sportId[i]), false);
                }
            }
        }
        return;
    }
    // End for multiple

    if (chkCopy.checked) {
        if (isEdit && !isCopyBas) {
            ActiveSuperPreset(true);
        }
        for (var i = 0; i < sportId.length; i++) {
            var elements;
            if (sportId[i] != "151") {
                if (document.getElementById("divUpdPT" + sportId[i])) {
                    elements = document.getElementById("divUpdPT" + sportId[i]).getElementsByTagName("input");
                    {
                        for (var j = 0; j < elements.length; j++) {
                            var id = elements[j].id;
                            if (id == "chkAuto2Hdp" || id == "chkUpdPT" + sportId[i]) continue;
                            elements[j].disabled = chkCopy.checked;
                        }
                    }
                }
                if (isCopyBas) {
                    if (document.getElementById("divUpdPT" + sportId[i])) {
                        elements = document.getElementById("divUpdPT" + sportId[i]).getElementsByTagName("select");
                        if (document.getElementById("chkCopy" + sportId[i] + "Hdp")) {
                            document.getElementById("chkCopy" + sportId[i] + "Hdp").disabled = chkCopy.checked;
                        }
                        for (var j = 0; j < elements.length; j++) {
                            var id = elements[j].id;
                            if (id == "listM2Hdp" || id == "listS2Hdp") continue;
                            elements[j].disabled = chkCopy.checked;
                        }
                    }
                }
            }
            if (i == 0) continue;
            if (document.getElementById("divUpdInfo" + sportId[i])) {
                elements = document.getElementById("divUpdInfo" + sportId[i]).getElementsByTagName("input");
                for (var j = 0; j < elements.length; j++) {
                    elements[j].disabled = chkCopy.checked;
                }
            }
        }
        document.getElementById("listS2Hdp").disabled = document.getElementById("chkAuto2Hdp").checked;
        document.getElementById("autoS2Hdp").disabled = !document.getElementById("chkAuto2Hdp").checked;
    }
    else// this case for Copy All Basketball is unchecked 
    {

        var arrSuper = [arrSuper2, arrSuper3, arrSuper5, arrSuper8, arrSuper10, arrSuper11, arrSuper201, arrSuper99, arrSuper99_M];
        var arrMaster = [arrMaster2, arrMaster3, arrMaster5, arrMaster8, arrMaster10, arrMaster11, arrMaster201, arrMaster99, arrMaster99_M];
        for (var i = 0; i < sportId.length; i++) {
            if (sportId[i] != "151") {
                var pre = "Hdp";
                if (i == 6) pre = "OU";
                var chkObj = document.getElementById("chkCopy" + sportId[i] + pre);
                if (chkObj) {
                    chkObj.disabled = chkCopy.checked;
                }
                if (document.getElementById("listM" + sportId[i] + pre)) {
                    document.getElementById("listM" + sportId[i] + pre).disabled = chkCopy.checked;
                }
                var chkAuto = document.getElementById("chkAuto" + sportId[i] + pre);
                if (chkAuto) {
                    chkAuto.disabled = chkCopy.checked;
                }
                if (document.getElementById("listS" + sportId[i] + pre)) {
                    document.getElementById("listS" + sportId[i] + pre).disabled = chkAuto.checked;
                }
                if (document.getElementById("autoS" + sportId[i] + pre)) {
                    document.getElementById("autoS" + sportId[i] + pre).disabled = !chkAuto.checked;
                }
                if (chkObj) {

                    CheckCopyPT(chkObj, arrSuper[i], arrMaster[i]);
                }
                else {
                    CheckCopyPT(document.getElementById("chkCopyBasket"), arrSuper[i], arrMaster[i]);
                }

            }
            if (document.getElementById("divUpdInfo" + sportId[i])) {
                var elements = document.getElementById("divUpdInfo" + sportId[i]).getElementsByTagName("input");
                for (var j = 0; j < elements.length; j++) {
                    elements[j].disabled = chkCopy.checked;

                }
            }
        }
    }
}

//this function is used to change value of current account when PT dropdown value of upline is changed
//uplineDDLId: id of PT dropdown that is clicked
// ex current account is Master
//    upline is Super

function SelectOnChange(uplineDDLId, interval) {

    if (interval != 10) interval = 100;
    if (uplineDDLId.indexOf("listS") > -1) {
        var accDDL = uplineDDLId.replace("listS", "listM");
        var maxMS;
        var maxMaster;
        var isMaster = (document.getElementById("roleId") != null ? document.getElementById("roleId").value == "3" : false);
        if (isMaster)//this for Master Edit
        {
            maxMS = uplineDDLId.replace("listS", "maxM");
            maxMS = document.getElementById(maxMS).value;
            maxMaster = document.getElementById(uplineDDLId.replace("listS", "maxA")).value
        }
        else //this for Super Edit
        {
            maxMS = uplineDDLId.replace("listS", "maxMS");
            maxMS = document.getElementById(maxMS).value;
            maxMaster = document.getElementById(uplineDDLId.replace("listS", "maxM")).value;
        }
        var masterVal = parseFloat(maxMS) - parseFloat(document.getElementById(uplineDDLId).value);
        masterVal = parseInt(round(masterVal * interval));
        if (masterVal > parseInt(maxMaster * interval)) {
            masterVal = parseInt(round(maxMaster * interval));
        }
        //for Editing
        var minMaster = 0;
        minMaster = document.getElementById(accDDL).options[document.getElementById(accDDL).length - 1].value
        minMaster = parseInt(round(minMaster * interval));
        //
        document.getElementById(accDDL).options.length = 0;
        var isRang = false;
        while (masterVal >= minMaster) {
            isRang = true;
            var val = masterVal / interval;
            document.getElementById(accDDL).options.add(new Option(val, val));
            masterVal -= 1;
        }
        if (!isRang) document.getElementById(accDDL).options.add(new Option(minMaster / interval, minMaster / interval));
    }
}
//this function is used to disable day checkboxs to edit transfer time
function selectTranferTime() {
    if (document.getElementById("isTransferOption") == "0" || !document.getElementById("alldays")) return;
    var elements = document.getElementById("alldays").getElementsByTagName("input");
    var isDaily = document.getElementById("rdDaily").checked;
    for (var i = 1; i < elements.length; i++) {
        elements[i].disabled = isDaily;
    }
}
// The function will be show to warning customer when suspended or closed member
function ConfirmSuspendedMessage(chkSuspended, IsCurrent) {

    if (chkSuspended.checked == true && chkSuspended.checked != IsCurrent) {
        if (typeof confirmSuspend != 'undefined' && !confirm(confirmSuspend)) {
            chkSuspended.checked = false;
        }

    }
    else if (chkSuspended.checked == false && chkSuspended.checked != IsCurrent) {
        if (!confirm(confirmUnSuspend)) {
            chkSuspended.checked = true;
        }

    }
}

function ConfirmClosedMessage(chkClosed, IsCurrent) {

    if (chkClosed.checked == true && chkClosed.checked != IsCurrent) {
        if (typeof confirmClosed != 'undefined' && !confirm(confirmClosed)) {
            chkClosed.checked = false;
        }

    }
    else if (chkClosed.checked == false && chkClosed.checked != IsCurrent) {
        if (!confirm(confirmOpen)) {
            chkClosed.checked = true;
        }

    }
}