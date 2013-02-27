//declare array that contain id of
var arr1Super1 = arr1Master1;
var arr2Super1 = arr2Master1;
var arr3Super1 = arr3Master1;

var arrSuper2 = arrMaster2; // basketball
var arrSuper3 = arrMaster3; // football
var arrSuper5 = arrMaster5; // tennis
var arrSuper8 = arrMaster8; // baseball
var arrSuper10 = arrMaster10; // golf
var arrSuper11 = arrMaster11; // motosports
var arrSuper201 = arrMaster201; // financial
var arr161Super81 = arr161Master81;
var arr161Super85 = arr161Master85;
var arrSuper99 = arrMaster99; // othersports
var arrSuper151TO = arrMaster151TO; // Tote
var arrSuper151FO = arrMaster151FO; // Fixed Odds
var arrSuper99_M = arrMaster99_M; // Mixed Sport

//this function is used to enable or disable controls when Copy All PT checkbox is checked
//when Copy All PT checkbox is checked for SOCCER
function CopyAllPT(chkCopyAll) {
    if ($("chkCopyBasket")) {
        $("chkCopyBasket").disabled = chkCopyAll.checked;
    }
    var sportId = ["1", "2", "3", "5", "8", "10", "11", "201", "99", "TO", "FO", "99_M"];
    var arrSuper = [null, arrSuper2, arrSuper3, arrSuper5, arrSuper8, arrSuper10, arrSuper11, arrSuper201, arrSuper99, arrSuper151TO, arrSuper151FO, arrSuper99_M];
    var isSuper = ($("roleId") != null);
    var isMult = $('isMult') ? $('isMult').value : "0";

    // Insert by David 15-02-2012
    //Apply for Master edit member Muilti
    // For copy all multi
    if (isMult == "1") {
        var chkUpdPT = ["chkUpd_1", "chkUpd_2", "chkUpd_3", "chkUpd_5", "chkUpd_8", "chkUpd_10", "chkUpd_11", "chkUpd_201", "chkUpd_99", "chkUpd_TO", "cchkUpd_FO", "chkUpd_99_M"];

        if (!chkCopyAll.checked) {
            // Case 1: Not IsAuto
            if ($("listM1HdpD") && $("listM2Hdp")) {
                $("chkCopyBasket").disabled = false;
            }
            // Case 2: Soccer: IsAuto
            if (!$("listM1HdpD") && $("listM2Hdp")) {
                $("chkCopyBasket").disabled = false;
            }
            // Case 3: Basketball: IsAuto
            if ($("listM1HdpD") && !$("listM2Hdp")) {
                if ($("chkAuto2Hdp").checked) {
                    $("chkCopyBasket").disabled = false;
                }
                else {
                    $("chkCopyBasket").disabled = true;
                }
            }
            // Case 4: Both is IsAuto
            if (!$("listM1HdpD") && !$("listM2Hdp")) {
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
        AllowUpdatePT(elm, sportId[0]); // for Soccer's PT
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



    // For 
    if (!chkCopyAll.checked) //this case for Copy All PT checkbox is unchecked (add & edit)
    {


        for (var i = 0; i < sportId.length; i++) {
            if (i == 0) {
                if ($("chkAuto11x2")) {
                    $("chkAuto11x2").disabled = false;
                    CheckAutoPreset($("chkAuto11x2"));
                }
                if ($("chkAuto1HdpL")) {
                    $("chkAuto1HdpL").disabled = false;
                    CheckAutoPreset($("chkAuto1HdpL"));
                }
                if ($("chkCopy1HdpDeadball")) {
                    $("chkCopy1HdpDeadball").disabled = false;
                    CheckCopyPT($("chkCopy1HdpDeadball"), arr1Super1);
                }
                if ($("chkCopy11x2")) {
                    if ($("listM11x2"))
                        $("chkCopy11x2").disabled = false;
                    else {
                        if ($("chkAuto11x2")) {
                            if ($("chkAuto11x2").checked == true)
                                $("chkCopy11x2").disabled = false;
                            else
                                $("chkCopy11x2").disabled = true;
                        }
                    }
                    CheckCopyPT($("chkCopy11x2"), arr2Super1);
                }
                if ($("chkCopy1HdpLive")) {
                    if ($("listM1HdpL"))
                        $("chkCopy1HdpLive").disabled = false;
                    else {
                        if ($("chkAuto1HdpL")) {
                            if ($("chkAuto1HdpL").checked == true)
                                $("chkCopy1HdpLive").disabled = false;
                            else
                                $("chkCopy1HdpLive").disabled = true;
                        }
                    }
                    CheckCopyPT($("chkCopy1HdpLive"), arr3Super1);
                }
                continue;
            }
            var pre = "Hdp";
            if (i == 7) pre = "OU";
            if (i == 9 || i == 10) pre = "Win";

            var chkAutoId = "chkAuto" + sportId[i] + pre;
            if ($(chkAutoId)) {
                $(chkAutoId).disabled = false;
                CheckAutoPreset($(chkAutoId));
            }
            if ($("chkCopy" + sportId[i] + pre)) {
                if ($("listM" + sportId[i] + pre))
                    $("chkCopy" + sportId[i] + pre).disabled = false;
                else {
                    if ($("chkAuto" + sportId[i] + pre)) {
                        if ($("chkAuto" + sportId[i] + pre).checked)
                            $("chkCopy" + sportId[i] + pre).disabled = false;
                        else
                            $("chkCopy" + sportId[i] + pre).disabled = true;
                    }
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
            if (i == 11) pre = "1stOUD";
            if (i == 12) pre = "OUL";

            var elements = '';
            if ($("divUpdPT" + sportId[i])) {
                elements = $("divUpdPT" + sportId[i]).getElementsByTagName("select");
                for (var j = 0; j < elements.length; j++) {
                    var id = elements[j].id;
                    if (id == "listS1HdpD" || id == "autoM1HdpD" || id == "listM1HdpD" || id == "listSTOWin") continue;
                    elements[j].disabled = chkCopyAll.checked;
                }
            }
            if ($("divUpdPT" + sportId[i])) {
                elements = $("divUpdPT" + sportId[i]).getElementsByTagName("input");
                for (var j = 0; j < elements.length; j++) {
                    var id = elements[j].id;
                    if (id == "chkAuto1HdpD" || id == "chkAutoTOWin") continue;
                    elements[j].disabled = chkCopyAll.checked;
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
        if ($("listM1HdpD") || $("chkAuto1HdpD").checked) {
            $("chkCopyAll").disabled = chkCopy.checked;
        }
        else
            $("chkCopyAll").disabled = true;
    }
    var isSuper = ($("roleId") != null);
    var isMult = $('isMult') ? $('isMult').value : "0";

    // Insert by David 15-02-2012
    //Apply for Master edit member Muilti
    // For copy all multi
    if (isMult == "1") {
        var sportId = ["1", "2", "3", "5", "8", "10", "11", "201", "99", "TO", "FO", "99_M"];
        var chkUpdPT = ["chkUpd_1", "chkUpd_2", "chkUpd_3", "chkUpd_5", "chkUpd_8", "chkUpd_10", "chkUpd_11", "chkUpd_201", "chkUpd_99", "chkUpd_TO", "cchkUpd_FO", "chkUpd_99_M"];
        if (!chkCopy.checked) {
            // Case 1: Not IsAuto
            if ($("listM1HdpD") && $("listM2Hdp")) {
                $("chkCopyAll").disabled = false;
            }
            // Case 2: Soccer: IsAuto
            if (!$("listM1HdpD") && $("listM2Hdp")) {
                if ($("chkAuto1HdpD").checked) {
                    $("chkCopyAll").disabled = false;
                }
                else {
                    $("chkCopyAll").disabled = true;
                }
            }
            // Case 3: Basketball: IsAuto
            if ($("listM1HdpD") && !$("listM2Hdp")) {
                $("chkCopyAll").disabled = false;
            }
            // Case 4: Both is IsAuto
            if (!$("listM1HdpD") && !$("listM2Hdp")) {
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
    //

    // For copy single
    if (!chkCopy.checked) {
        for (var i = 0; i < sportId.length; i++) {
            var pre = "Hdp";
            if (i == 6) pre = "OU";
            var chkAutoId = "chkAuto" + sportId[i] + pre;



            if ($("divUpdPT" + sportId[i])) {
                elements = $("divUpdPT" + sportId[i]).getElementsByTagName("input");
            }
            if ($(chkAutoId)) {
                $(chkAutoId).disabled = false;
                CheckAutoPreset($(chkAutoId));
            }
            if ($("chkCopy" + sportId[i] + pre)) {
                if ($("listM" + sportId[i] + pre))
                    $("chkCopy" + sportId[i] + pre).disabled = false;
                else {
                    if ($("chkAuto" + sportId[i] + pre)) {
                        if ($("chkAuto" + sportId[i] + pre).checked)
                            $("chkCopy" + sportId[i] + pre).disabled = false;
                        else
                            $("chkCopy" + sportId[i] + pre).disabled = true;
                    }
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
            if ($('chkUpd_' + sportId[i])) $('chkUpd_' + sportId[i]).disabled = chkCopy.checked;
            if ($("divUpdPT" + sportId[i])) {
                elements = $("divUpdPT" + sportId[i]).getElementsByTagName("select");
                for (var j = 0; j < elements.length; j++) {
                    var id = elements[j].id;
                    if (id == "listS2Hdp" || id == "autoM2Hdp" || id == "listM2Hdp") continue;
                    elements[j].disabled = chkCopy.checked;
                }
            }
            if ($("divUpdPT" + sportId[i])) {
                elements = $("divUpdPT" + sportId[i]).getElementsByTagName("input");
                for (var j = 0; j < elements.length; j++) {
                    var id = elements[j].id;
                    if (id == "chkAuto2Hdp") continue;
                    elements[j].disabled = chkCopy.checked;
                }
            }
        }
    }
    //
}

function CheckCopyPT(chkCopy, arrSuper) {

    for (var i = 0; i < arrSuper.length; i++) {
        var autoId = arrSuper[i].replace("listM", "autoM");
        var chkId = arrSuper[i].replace("listM", "chkAuto");
        var autoMId = arrSuper[i].replace("listM", "isAutoM");
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
function CheckAutoPreset(objChk) {

    var sId = objChk.id.replace("chkAuto", "listM");
    var autoId = objChk.id.replace("chkAuto", "autoM");
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
                if (!objChk.checked && $("chkCopyAll").checked == true) {
                    $("spCopyAll").checked = $("chkCopyAll").checked = false;
                    CopyAllPT($("chkCopyAll"));
                }
                if (!objChk.checked && $("chkCopy1HdpDeadball").checked == true) {
                    $("chkCopy1HdpDeadball").checked = false;
                    CheckCopyPT($("chkCopy1HdpDeadball"), arr1Super1);
                }
                if ($("chkCopyBasket")) {
                    if ($("chkCopyBasket").checked) {
                        $("spCopyAll").checked = $("chkCopyAll").disabled = true;
                    }
                    else {
                        $("spCopyAll").checked = $("chkCopyAll").disabled = !objChk.checked;
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
            else if (objChk.id == "chkAuto2Hdp") { // Check copy basket ball

                if (!objChk.checked && $("chkCopyBasket").checked == true) {
                    $("spCopyBasket").checked = $("chkCopyBasket").checked = false;
                    CopyBasketPT($("chkCopyBasket"));
                }
                if (!objChk.checked && $("chkCopy2Hdp").checked == true) {
                    $("chkCopy2Hdp").checked = false;
                    CheckCopyPT($("chkCopy2Hdp"), arrSuper2);
                }
                $("spCopyBasket").disabled = $("chkCopyBasket").disabled = $("chkCopy2Hdp").disabled = !objChk.checked;

            }
            else if (objChk.id == "chkAutoTOWin") { // Check copy basket ball
                if ($("chkCopyAll")) {
                    if (!objChk.checked && $("chkCopyAll").checked == true) {
                        $("spCopyAll").checked = $("chkCopyAll").checked = false;
                        CopyAllPT($("chkCopyAll"));
                    }
                    if (!objChk.checked && $("chkCopyTOWin").checked == true) {
                        $("chkCopyTOWin").checked = false;
                        CheckCopyPT($("chkCopyTOWin"), arrSuper151TO);
                    }
                    $("spCopyAll").checked = $("chkCopyAll").disabled = $("chkCopyTOWin").disabled = !objChk.checked;
                }
                else {
                    $("chkCopyTOWin").disabled = !objChk.checked;
                }
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