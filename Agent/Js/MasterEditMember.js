//this function is used to enable or disable controls when Copy All PT checkbox is checked
//when Copy All PT checkbox is checked
function CopyAllPT(chkCopyAll) {
    if ($("chkCopyBasket")) {
        $("chkCopyBasket").disabled = chkCopyAll.checked;
    }


    var sportId = ["1", "2", "3", "5", "8", "10", "11", "201", "99", "TO", "FO", "99_M"];
    var arrSuper = [null, arrSuper2, arrSuper3, arrSuper5, arrSuper8, arrSuper10, arrSuper11, arrSuper201, arrSuper99, arrSuper151TO, arrSuper151FO, arrSuper99_M];
    var isMaster = ($("roleId") != null);

    var isMult = $('isMult') ? $('isMult').value : "0";

    // For copy all multi
    if (isMult == "1") {
        var chkUpdPT = ["chkUpd_1", "chkUpd_2", "chkUpd_3", "chkUpd_5", "chkUpd_8", "chkUpd_10", "chkUpd_11", "chkUpd_201", "chkUpd_99", "chkUpd_TO", "cchkUpd_FO", "chkUpd_99_M"];

        if (!chkCopyAll.checked) {
            // Case 1: Not IsAuto
            if ($("listS1HdpD") && $("listS2Hdp")) {
                $("chkCopyBasket").disabled = false;
            }
            // Case 2: Soccer: IsAuto
            if (!$("listS1HdpD") && $("listS2Hdp")) {
                $("chkCopyBasket").disabled = false;
            }
            // Case 3: Basketball: IsAuto
            if ($("listS1HdpD") && !$("listS2Hdp")) {
                if ($("chkAuto2Hdp").checked) {
                    $("chkCopyBasket").disabled = false;
                }
                else {
                    $("chkCopyBasket").disabled = true;
                }
            }
            // Case 4: Both is IsAuto
            if (!$("listS1HdpD") && !$("listS2Hdp")) {
                if ($("chkAuto2Hdp").checked) {
                    $("chkCopyBasket").disabled = false;
                }
                else {
                    $("chkCopyBasket").disabled = true;
                }
            }
        }
        else// this case for Copy All soccer is checked 
        {
            // Insert by David
            // For 4 case. chkCopyAllBK is disabled
            $("chkCopyBasket").disabled = true;



        }
        AllowUpdatePT(elm, sportId[0]);// for Soccer's PT
        for (var i = 1; i < chkUpdPT.length; i++) { // For others sport
            if ($(chkUpdPT[i]) == null) {
                continue;
            }
            $(chkUpdPT[i]).disabled = chkCopyAll.checked;
            var elm = document.getElementById(chkUpdPT[i]);
            AllowUpdatePT(elm, sportId[i]);
        }
        return;
    }
    //


    if (!chkCopyAll.checked) //this case for Copy All PT checkbox is unchecked (add & edit)
    {
        for (var i = 0; i < sportId.length; i++) {
            if (i == 0) {
                if (!isMaster) {
                    if ($("chkAuto11x2")) {
                        $("chkAuto11x2").disabled = false;
                        CheckAutoPreset($("chkAuto11x2"));
                    }
                    if ($("chkAuto1HdpL")) {
                        $("chkAuto1HdpL").disabled = false;
                        CheckAutoPreset($("chkAuto1HdpL"));
                    }
                }
                else {
                    if ($("listM11x2")) {
                        $("listM11x2").disabled = false;
                    }
                    if ($("listM1HdpL")) {
                        $("listM1HdpL").disabled = false;
                    }
                }
                if ($("chkCopy1HdpDeadball")) {
                    $("chkCopy1HdpDeadball").disabled = false;
                    CheckCopyPT($("chkCopy1HdpDeadball"), arr1Super1);
                }
                if ($("chkCopy11x2")) {
                    if ($("listS11x2"))
                        $("chkCopy11x2").disabled = false;
                    else {
                        if ($("chkAuto11x2")) {
                            if ($("chkAuto11x2").checked == true)
                                $("chkCopy11x2").disabled = false;
                            else
                                $("chkCopy11x2").disabled = true;
                        }
                        else {
                            $("chkCopy11x2").disabled = false;
                        }
                    }
                    CheckCopyPT($("chkCopy11x2"), arr2Super1);
                }
                if ($("chkCopy1HdpLive")) {
                    if ($("listS1HdpL"))
                        $("chkCopy1HdpLive").disabled = false;
                    else {
                        if ($("chkAuto1HdpL")) {
                            if ($("chkAuto1HdpL").checked == true)
                                $("chkCopy1HdpLive").disabled = false;
                            else
                                $("chkCopy1HdpLive").disabled = true;
                        }
                        else
                            $("chkCopy1HdpLive").disabled = false;
                    }
                    CheckCopyPT($("chkCopy1HdpLive"), arr3Super1);
                }
                continue;
            }
            if (i != 0 && $('chkUpd_' + sportId[i])) {
                $('chkUpd_' + sportId[i]).disabled = chkCopyAll.checked;
                if (!$('chkUpd_' + sportId[i]).checked) continue;
            }
            var pre = "Hdp";
            if (i == 7) pre = "OU";
            if (i == 9 || i == 10) pre = "Win";

            if (!isMaster) {
                var chkAutoId = "chkAuto" + sportId[i] + pre;
                if ($(chkAutoId)) {
                    $(chkAutoId).disabled = false;
                    CheckAutoPreset($(chkAutoId));
                }
            }
            else {
                if ($("listM" + sportId[i] + pre)) {
                    $("listM" + sportId[i] + pre).disabled = false;
                }
            }
            if ($("chkCopy" + sportId[i] + pre)) {
                if ($("listS" + sportId[i] + pre))
                    $("chkCopy" + sportId[i] + pre).disabled = false;
                else {
                    if ($("chkAuto" + sportId[i] + pre)) {
                        if ($("chkAuto" + sportId[i] + pre).checked)
                            $("chkCopy" + sportId[i] + pre).disabled = false;
                        else
                            $("chkCopy" + sportId[i] + pre).disabled = true;
                    }
                    else
                        $("chkCopy" + sportId[i] + pre).disabled = false;
                }
                CheckCopyPT($("chkCopy" + sportId[i] + pre), arrSuper[i]);
            }
            else {
                CheckCopyPT($("chkCopyAll"), arrSuper[i]);
            }
        }
    }
    else //this case for Copy All PT checkbox is checked (add & edit)
    {
        for (var i = 0; i < sportId.length; i++) {
            var pre = "Hdp";
            if (i == 7) pre = "OU";
            if (i == 9 || i == 10) pre = "Win";

            var elements = '';
            if ($("divUpdPT" + sportId[i])) {
                elements = $("divUpdPT" + sportId[i]).getElementsByTagName("select");
                if (i != 0 && $('chkUpd_' + sportId[i])) $('chkUpd_' + sportId[i]).disabled = chkCopyAll.checked;
                for (var j = 0; j < elements.length; j++) {
                    var id = elements[j].id;
                    if (id == "listS1HdpD" || id == "autoS1HdpD" || id == "listM1HdpD" || id == "listSFOWin" || id == "listMFOWin" || id == "autoSFOWin") continue;
                    elements[j].disabled = chkCopyAll.checked;
                }
            }
            if (!isMaster) {
                if ($("divUpdPT" + sportId[i])) {
                    elements = $("divUpdPT" + sportId[i]).getElementsByTagName("input");
                    for (var j = 0; j < elements.length; j++) {
                        var id = elements[j].id;
                        if (id == "chkAuto1HdpD" || id == "chkAutoFOWin") continue;
                        elements[j].disabled = chkCopyAll.checked;
                    }
                }
            }
            else {
                if (i == 0) {
                    if ($("chkCopy1HdpDeadball")) {
                        $("chkCopy1HdpDeadball").disabled = chkCopyAll.checked;
                    }
                    if ($("chkCopy11x2")) {
                        $("chkCopy11x2").disabled = chkCopyAll.checked;
                    }
                    if ($("chkCopy1HdpLive")) {
                        $("chkCopy1HdpLive").disabled = chkCopyAll.checked;
                    }
                    continue;
                }
                if ($("chkCopy" + sportId[i] + pre)) {
                    $("chkCopy" + sportId[i] + pre).disabled = chkCopyAll.checked;
                }
            }
        }
    }
}

//this function is used to disabled controls of all Sporttype except Soccer 
//when Copy All BS and PT from Basketball
function CopyBasketPT(chkCopy) {
    var sportId = ["2", "3", "5", "8", "10", "11", "201", "99", "99_M"];
    var arrSuper = [arrSuper2, arrSuper3, arrSuper5, arrSuper8, arrSuper10, arrSuper11, arrSuper201, arrSuper99, arrSuper99_M];
    if ($("chkCopyAll")) {
        if ($("chkAuto1HdpD")) {
            if ($("listS1HdpD") || $("chkAuto1HdpD").checked) {
                $("chkCopyAll").disabled = chkCopy.checked;
            }
            else
                $("chkCopyAll").disabled = true;
        }
        else {
            $("chkCopyAll").disabled = chkCopy.checked;
        }
    }
    var isMaster = ($("roleId") != null);

    var isMult = $('isMult') ? $('isMult').value : "0";
    // For copy all multi
    if (isMult == "1") {
        var sportId = ["1", "2", "3", "5", "8", "10", "11", "201", "99", "TO", "FO", "99_M"];
        var chkUpdPT = ["chkUpd_1", "chkUpd_2", "chkUpd_3", "chkUpd_5", "chkUpd_8", "chkUpd_10", "chkUpd_11", "chkUpd_201", "chkUpd_99", "chkUpd_TO", "cchkUpd_FO", "chkUpd_99_M"];
        if (!chkCopy.checked) {
            // Case 1: Not IsAuto
            if ($("listS1HdpD") && $("listS2Hdp")) {
                $("chkCopyAll").disabled = false;
            }
            // Case 2: Soccer: IsAuto
            if (!$("listS1HdpD") && $("listS2Hdp")) {
                if ($("chkAuto1HdpD").checked) {
                    $("chkCopyAll").disabled = false;
                }
                else {
                    $("chkCopyAll").disabled = true;
                }
            }
            // Case 3: Basketball: IsAuto
            if ($("listS1HdpD") && !$("listS2Hdp")) {
                $("chkCopyAll").disabled = false;
            }
            // Case 4: Both is IsAuto
            if (!$("listS1HdpD") && !$("listS2Hdp")) {
                if ($("chkAuto1HdpD").checked) {
                    $("chkCopyAll").disabled = false;
                }
                else {
                    $("chkCopyAll").disabled = true;
                }
            } 
        }
        else// this case for Copy All Basketball is checked 
        {
            // For 4 case. chkCopyAll is disabled
            $("chkCopyAll").disabled = true;
        }
        AllowUpdatePT(elm, sportId[1]);
        for (var i = 2; i < chkUpdPT.length; i++) {
            if ($(chkUpdPT[i]) == null) {
                continue;
            }
            $(chkUpdPT[i]).disabled = chkCopy.checked;
            var elm = document.getElementById(chkUpdPT[i]);
            AllowUpdatePT(elm, sportId[i]);
        }
        return;
    }
    //

    if (!chkCopy.checked) {
        for (var i = 0; i < sportId.length; i++) {
            var pre = "Hdp";
            if (i == 6) pre = "OU";
            if ($('chkUpd_' + sportId[i])) {
                $('chkUpd_' + sportId[i]).disabled = chkCopy.checked;
                if (!$('chkUpd_' + sportId[i]).checked) continue;
            }
            if (!isMaster) {

                var chkAutoId = "chkAuto" + sportId[i] + pre;
                if ($("divUpdPT" + sportId[i])) {
                    elements = $("divUpdPT" + sportId[i]).getElementsByTagName("input");
                }
                if ($(chkAutoId)) {
                    $(chkAutoId).disabled = false;
                    CheckAutoPreset($(chkAutoId));
                }
            }
            else {
                if ($("listM" + sportId[i] + pre)) {
                    $("listM" + sportId[i] + pre).disabled = false;
                }
            }
            if ($("chkCopy" + sportId[i] + pre)) {
                if ($("listS" + sportId[i] + pre))
                    $("chkCopy" + sportId[i] + pre).disabled = false;
                else {
                    if ($("chkAuto" + sportId[i] + pre)) {
                        if ($("chkAuto" + sportId[i] + pre).checked)
                            $("chkCopy" + sportId[i] + pre).disabled = false;
                        else
                            $("chkCopy" + sportId[i] + pre).disabled = true;
                    }
                    else
                        $("chkCopy" + sportId[i] + pre).disabled = false;
                }
                CheckCopyPT($("chkCopy" + sportId[i] + pre), arrSuper[i]);
            }
            else {
                CheckCopyPT($("chkCopyBasket"), arrSuper[i]);
            }

        }
    }
    else// this case for Copy All Basketball is checked 
    {
        $("panelBasket").style.display = "";
        var isAuto = false;
        for (var i = 0; i < sportId.length; i++) {
            var elements = '';
            var pre = "Hdp";
            if (i == 6) pre = "OU";

            if ($("divUpdPT" + sportId[i])) {
                if ($('chkUpd_' + sportId[i]))
                    $('chkUpd_' + sportId[i]).disabled = chkCopy.checked;
                elements = $("divUpdPT" + sportId[i]).getElementsByTagName("select");
                for (var j = 0; j < elements.length; j++) {
                    var id = elements[j].id;
                    if (id == "listS2Hdp" || id == "autoS2Hdp" || id == "listM2Hdp") continue;
                    elements[j].disabled = chkCopy.checked;
                }
            }
            if (!isMaster) {
                if ($("divUpdPT" + sportId[i])) {
                    elements = $("divUpdPT" + sportId[i]).getElementsByTagName("input");
                    for (var j = 0; j < elements.length; j++) {
                        var id = elements[j].id;
                        if (id == "chkAuto2Hdp") continue;
                        elements[j].disabled = chkCopy.checked;
                    }
                }
            }
            else {
                if ($("chkCopy" + sportId[i] + pre)) {
                    $("chkCopy" + sportId[i] + pre).disabled = chkCopy.checked;
                }
            }
        }
    }
}

function CheckCopyPT(chkCopy, arrSuper) {

    for (var i = 0; i < arrSuper.length; i++) {
        var autoId = arrSuper[i].replace("listS", "autoS");
        var chkId = arrSuper[i].replace("listS", "chkAuto");
        var autoMId = arrSuper[i].replace("listS", "isAutoM");
        if ($("roleId") != null) // this case for Master edit Member
        {

            var masterId = arrSuper[i].replace("listS", "listM");
            if ($(masterId)) {
                $(masterId).disabled = chkCopy.checked;
            }
        }
        else // this case for Super edit Member
        {
            if ($(chkId)) {
                $(chkId).disabled = chkCopy.checked;
            }
            if (chkCopy.checked) {
                if ($(autoMId)) {
                    if ($(autoMId).value == 1) {
                        if ($(autoId)) {
                            $(autoId).disabled = chkCopy.checked;
                        }
                    }
                    else {
                        if ($(arrSuper[i])) {

                            $(arrSuper[i]).disabled = chkCopy.checked;
                        }
                        if ($(autoId)) {
                            $(autoId).disabled = chkCopy.checked;
                        }
                    }
                }
                else {
                    if ($(arrSuper[i])) {

                        $(arrSuper[i]).disabled = chkCopy.checked;
                    }
                    if ($(autoId)) {
                        $(autoId).disabled = chkCopy.checked;
                    }
                }
            }
            else {
                if ($(autoMId)) {
                    if ($(autoMId).value == 1) {
                        if ($(autoId)) {
                            $(autoId).disabled = !$(chkId).checked;
                        }
                    }
                    else {
                        if ($(arrSuper[i])) {
                            $(arrSuper[i]).disabled = $(chkId).checked;
                        }
                        if ($(autoId)) {
                            $(autoId).disabled = !$(chkId).checked;
                        }
                    }
                }
                else {
                    if ($(arrSuper[i])) {
                        $(arrSuper[i]).disabled = $(chkId).checked;
                    }
                    if ($(autoId)) {
                        $(autoId).disabled = !$(chkId).checked;
                    }
                }
            }
        }
    }
}
function CheckAutoPreset(objChk) {

    var sId = objChk.id.replace("chkAuto", "listS");
    var autoId = objChk.id.replace("chkAuto", "autoS");
    var MAutoId = objChk.id.replace("chkAuto", "isAutoM");
    if ($(MAutoId)) {
        if ($(MAutoId).value == 1) {
            $(autoId).disabled = !objChk.checked;
        }
        else {
            if ($(sId)) {
                $(sId).disabled = objChk.checked;
            }
            $(autoId).disabled = !objChk.checked;
        }
    }
    else {

        if ($(sId)) {
            $(sId).disabled = objChk.checked;
        }
        else {
            if (objChk.id == "chkAuto1HdpD") { // check copy all
                if ($("chkCopyAll")) {
                    if (!objChk.checked && $("chkCopyAll").checked == true) {
                        $("chkCopyAll").checked = false;
                        CopyAllPT($("chkCopyAll"));
                    }
                }
                if (!objChk.checked && $("chkCopy1HdpDeadball").checked == true) {
                    $("chkCopy1HdpDeadball").checked = false;
                    CheckCopyPT($("chkCopy1HdpDeadball"), arr1Super1);
                }
                if ($("chkCopyBasket")) {
                    if ($("chkCopyBasket").checked) {
                        $("chkCopyAll").disabled = true;
                    }
                    else {
                        $("chkCopyAll").disabled = !objChk.checked;
                    }
                }
                if ($("chkCopyAll")) {
                    if ($('isMult').value == "1") { //  Insert by David 21-2-2012
                        if (objChk.checked && !$("listS1HdpD") && $("chkCopyBasket").checked) {
                            $("chkCopyAll").disabled = true;
                        }
                        else {
                            $("chkCopyAll").disabled = !objChk.checked;
                        }
                    }//
                    else {
                        $("chkCopyAll").disabled = !objChk.checked;
                    }         
                }

                $("chkCopy1HdpDeadball").disabled = !objChk.checked;
            }
            else if (objChk.id == "chkAuto11x2") {

                if (!objChk.checked && $("chkCopy11x2").checked == true) {
                    $("chkCopy11x2").checked = false;
                    CheckCopyPT($("chkCopy11x2"), arr2Super1);
                }
                $("chkCopy11x2").disabled = !objChk.checked;
            }
            else if (objChk.id == "chkAuto1HdpL") {
                $("chkCopy1HdpLive").disabled = !objChk.checked;
                if (!objChk.checked && $("chkCopy1HdpLive").checked == true) {
                    $("chkCopy1HdpLive").checked = false;
                    CheckCopyPT($("chkCopy1HdpLive"), arr3Super1);
                }
            }
            else if (objChk.id == "chkAuto2Hdp") {

                if ($("chkCopyBasket")) {
                    if (!objChk.checked && $("chkCopyBasket").checked == true) {
                        $("chkCopyBasket").checked = false;
                        CopyBasketPT($("chkCopyBasket"));
                    }
                }
                if (!objChk.checked && $("chkCopy2Hdp").checked == true) {
                    $("chkCopy2Hdp").checked = false;
                    CheckCopyPT($("chkCopy2Hdp"), arrSuper2);
                }
                if ($("chkCopyBasket")) {
                    $("chkCopyBasket").disabled = !objChk.checked;
                }
                $("chkCopy2Hdp").disabled = !objChk.checked;
            }
            else if (objChk.id == "chkAutoTOWin") {
                if ($("chkCopyAll")) {
                    if (!objChk.checked && $("chkCopyAll").checked == true) {
                        $("chkCopyAll").checked = false;
                        CopyAllPT($("chkCopyAll"));
                    }
                }
                if (!objChk.checked && $("chkCopyTOWin").checked == true) {
                    $("chkCopyTOWin").checked = false;
                    CheckCopyPT($("chkCopyTOWin"), arrSuper151TO);
                }
                if ($("chkCopyAll")) {
                    $("chkCopyAll").disabled = !objChk.checked;
                }
                $("chkCopyTOWin").disabled = !objChk.checked;

            }
            else {
                var idChkCopy = objChk.id.replace("chkAuto", "chkCopy");
                if ($(idChkCopy)) {
                    var sportId = ["3", "5", "8", "10", "11", "201", "99", "TO", "FO"];
                    var arrSuper = [arrSuper3, arrSuper5, arrSuper8, arrSuper10, arrSuper11, arrSuper201, arrSuper99, arrSuper151TO, arrSuper151FO];
                    for (var i = 0; i < sportId.length; i++) {
                        var pre = "Hdp";
                        if (i == 7 || i == 8) pre = "Win";
                        if (i == 5) pre = "OU";
                        if (objChk.id == "chkAuto" + sportId[i] + pre) {
                            if (!objChk.checked && $(idChkCopy).checked == true) {
                                $(idChkCopy).checked = false;
                                CheckCopyPT($(idChkCopy), arrSuper[i]);
                                break;
                            }
                        }
                    }
                    $(idChkCopy).disabled = !objChk.checked;
                }
            }
        }

        $(autoId).disabled = !objChk.checked;
    }
}
function IncreasePT(ddl, step) {
    var max = $(ddl).options[0].value;
    Increase(ddl, step, max);
}
function ReducePT(ddl, step) {
    var min = 0;
    min = $(ddl).options[$(ddl).length - 1].value;
    Reduce(ddl, step, min);
}
function AllowUpdate(chkObj) {
    $('chkClosed').disabled = (chkObj.checked) ? false : true;
}