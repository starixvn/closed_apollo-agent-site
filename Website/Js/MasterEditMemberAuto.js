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
    if (document.getElementById("chkCopyBasket")) {
        document.getElementById("chkCopyBasket").disabled = chkCopyAll.checked;
    }
    var sportId = ["1", "2", "3", "5", "8", "10", "11", "201", "99", "TO", "FO", "99_M"];
    var arrSuper = [null, arrSuper2, arrSuper3, arrSuper5, arrSuper8, arrSuper10, arrSuper11, arrSuper201, arrSuper99, arrSuper151TO, arrSuper151FO, arrSuper99_M];
    var isSuper = (document.getElementById("roleId") != null);
    var isMult = document.getElementById('isMult') ? document.getElementById('isMult').value : "0";

    // Insert by David 15-02-2012
    //Apply for Master edit member Muilti
    // For copy all multi
    if (isMult == "1") {
        var chkUpdPT = ["chkUpd_1", "chkUpd_2", "chkUpd_3", "chkUpd_5", "chkUpd_8", "chkUpd_10", "chkUpd_11", "chkUpd_201", "chkUpd_99", "chkUpd_TO", "cchkUpd_FO", "chkUpd_99_M"];

        if (!chkCopyAll.checked) {
            // Case 1: Not IsAuto
            if (document.getElementById("listM1HdpD") && document.getElementById("listM2Hdp")) {
                document.getElementById("chkCopyBasket").disabled = false;
            }
            // Case 2: Soccer: IsAuto
            if (!document.getElementById("listM1HdpD") && document.getElementById("listM2Hdp")) {
                document.getElementById("chkCopyBasket").disabled = false;
            }
            // Case 3: Basketball: IsAuto
            if (document.getElementById("listM1HdpD") && !document.getElementById("listM2Hdp")) {
                if (document.getElementById("chkAuto2Hdp").checked) {
                    document.getElementById("chkCopyBasket").disabled = false;
                }
                else {
                    document.getElementById("chkCopyBasket").disabled = true;
                }
            }
            // Case 4: Both is IsAuto
            if (!document.getElementById("listM1HdpD") && !document.getElementById("listM2Hdp")) {
                if (document.getElementById("chkAuto2Hdp").checked) {
                    document.getElementById("chkCopyBasket").disabled = false;
                }
                else {
                    document.getElementById("chkCopyBasket").disabled = true;
                }
            }
        }
        else// this case for Copy All soccer is checked 
        {
            // Insert by David
            // For 4 case. chkCopyAllBK is disabled
            document.getElementById("chkCopyBasket").disabled = true;



        }
        AllowUpdatePT(elm, sportId[0]); // for Soccer's PT
        for (var i = 1; i < chkUpdPT.length; i++) { // For others sport
            if (document.getElementById(chkUpdPT[i]) == null) {
                continue;
            }
            document.getElementById(chkUpdPT[i]).disabled = chkCopyAll.checked;
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
                if (document.getElementById("chkAuto11x2")) {
                    document.getElementById("chkAuto11x2").disabled = false;
                    CheckAutoPreset(document.getElementById("chkAuto11x2"));
                }
                if (document.getElementById("chkAuto1HdpL")) {
                    document.getElementById("chkAuto1HdpL").disabled = false;
                    CheckAutoPreset(document.getElementById("chkAuto1HdpL"));
                }
                if (document.getElementById("chkCopy1HdpDeadball")) {
                    document.getElementById("chkCopy1HdpDeadball").disabled = false;
                    CheckCopyPT(document.getElementById("chkCopy1HdpDeadball"), arr1Super1);
                }
                if (document.getElementById("chkCopy11x2")) {
                    if (document.getElementById("listM11x2"))
                        document.getElementById("chkCopy11x2").disabled = false;
                    else {
                        if (document.getElementById("chkAuto11x2")) {
                            if (document.getElementById("chkAuto11x2").checked == true)
                                document.getElementById("chkCopy11x2").disabled = false;
                            else
                                document.getElementById("chkCopy11x2").disabled = true;
                        }
                    }
                    CheckCopyPT(document.getElementById("chkCopy11x2"), arr2Super1);
                }
                if (document.getElementById("chkCopy1HdpLive")) {
                    if (document.getElementById("listM1HdpL"))
                        document.getElementById("chkCopy1HdpLive").disabled = false;
                    else {
                        if (document.getElementById("chkAuto1HdpL")) {
                            if (document.getElementById("chkAuto1HdpL").checked == true)
                                document.getElementById("chkCopy1HdpLive").disabled = false;
                            else
                                document.getElementById("chkCopy1HdpLive").disabled = true;
                        }
                    }
                    CheckCopyPT(document.getElementById("chkCopy1HdpLive"), arr3Super1);
                }
                continue;
            }
            var pre = "Hdp";
            if (i == 7) pre = "OU";
            if (i == 9 || i == 10) pre = "Win";

            var chkAutoId = "chkAuto" + sportId[i] + pre;
            if (document.getElementById(chkAutoId)) {
                document.getElementById(chkAutoId).disabled = false;
                CheckAutoPreset(document.getElementById(chkAutoId));
            }
            if (document.getElementById("chkCopy" + sportId[i] + pre)) {
                if (document.getElementById("listM" + sportId[i] + pre))
                    document.getElementById("chkCopy" + sportId[i] + pre).disabled = false;
                else {
                    if (document.getElementById("chkAuto" + sportId[i] + pre)) {
                        if (document.getElementById("chkAuto" + sportId[i] + pre).checked)
                            document.getElementById("chkCopy" + sportId[i] + pre).disabled = false;
                        else
                            document.getElementById("chkCopy" + sportId[i] + pre).disabled = true;
                    }
                }
                CheckCopyPT(document.getElementById("chkCopy" + sportId[i] + pre), arrSuper[i]);
            }
            else {
                CheckCopyPT(document.getElementById("chkCopyAll"), arrSuper[i]);
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
            if (document.getElementById("divUpdPT" + sportId[i])) {
                elements = document.getElementById("divUpdPT" + sportId[i]).getElementsByTagName("select");
                for (var j = 0; j < elements.length; j++) {
                    var id = elements[j].id;
                    if (id == "listS1HdpD" || id == "autoM1HdpD" || id == "listM1HdpD" || id == "listSTOWin") continue;
                    elements[j].disabled = chkCopyAll.checked;
                }
            }
            if (document.getElementById("divUpdPT" + sportId[i])) {
                elements = document.getElementById("divUpdPT" + sportId[i]).getElementsByTagName("input");
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
    if (document.getElementById("chkCopyAll")) {
        if (document.getElementById("listM1HdpD") || document.getElementById("chkAuto1HdpD").checked) {
            document.getElementById("chkCopyAll").disabled = chkCopy.checked;
        }
        else
            document.getElementById("chkCopyAll").disabled = true;
    }
    var isSuper = (document.getElementById("roleId") != null);
    var isMult = document.getElementById('isMult') ? document.getElementById('isMult').value : "0";

    // Insert by David 15-02-2012
    //Apply for Master edit member Muilti
    // For copy all multi
    if (isMult == "1") {
        var sportId = ["1", "2", "3", "5", "8", "10", "11", "201", "99", "TO", "FO", "99_M"];
        var chkUpdPT = ["chkUpd_1", "chkUpd_2", "chkUpd_3", "chkUpd_5", "chkUpd_8", "chkUpd_10", "chkUpd_11", "chkUpd_201", "chkUpd_99", "chkUpd_TO", "cchkUpd_FO", "chkUpd_99_M"];
        if (!chkCopy.checked) {
            // Case 1: Not IsAuto
            if (document.getElementById("listM1HdpD") && document.getElementById("listM2Hdp")) {
                document.getElementById("chkCopyAll").disabled = false;
            }
            // Case 2: Soccer: IsAuto
            if (!document.getElementById("listM1HdpD") && document.getElementById("listM2Hdp")) {
                if (document.getElementById("chkAuto1HdpD").checked) {
                    document.getElementById("chkCopyAll").disabled = false;
                }
                else {
                    document.getElementById("chkCopyAll").disabled = true;
                }
            }
            // Case 3: Basketball: IsAuto
            if (document.getElementById("listM1HdpD") && !document.getElementById("listM2Hdp")) {
                document.getElementById("chkCopyAll").disabled = false;
            }
            // Case 4: Both is IsAuto
            if (!document.getElementById("listM1HdpD") && !document.getElementById("listM2Hdp")) {
                if (document.getElementById("chkAuto1HdpD").checked) {
                    document.getElementById("chkCopyAll").disabled = false;
                }
                else {
                    document.getElementById("chkCopyAll").disabled = true;
                }
            }
        }
        else// this case for Copy All Basketball is checked 
        {
            // For 4 case. chkCopyAll is disabled
            document.getElementById("chkCopyAll").disabled = true;
        }
        AllowUpdatePT(elm, sportId[1]);
        for (var i = 2; i < chkUpdPT.length; i++) {
            if (document.getElementById(chkUpdPT[i]) == null) {
                continue;
            }
            document.getElementById(chkUpdPT[i]).disabled = chkCopy.checked;
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



            if (document.getElementById("divUpdPT" + sportId[i])) {
                elements = document.getElementById("divUpdPT" + sportId[i]).getElementsByTagName("input");
            }
            if (document.getElementById(chkAutoId)) {
                document.getElementById(chkAutoId).disabled = false;
                CheckAutoPreset(document.getElementById(chkAutoId));
            }
            if (document.getElementById("chkCopy" + sportId[i] + pre)) {
                if (document.getElementById("listM" + sportId[i] + pre))
                    document.getElementById("chkCopy" + sportId[i] + pre).disabled = false;
                else {
                    if (document.getElementById("chkAuto" + sportId[i] + pre)) {
                        if (document.getElementById("chkAuto" + sportId[i] + pre).checked)
                            document.getElementById("chkCopy" + sportId[i] + pre).disabled = false;
                        else
                            document.getElementById("chkCopy" + sportId[i] + pre).disabled = true;
                    }
                }
                CheckCopyPT(document.getElementById("chkCopy" + sportId[i] + pre), arrSuper[i]);
            }
            else {
                CheckCopyPT(document.getElementById("chkCopyBasket"), arrSuper[i]);
            }
        }

    }
    else// this case for Copy All Basketball is checked 
    {


        document.getElementById("panelBasket").style.display = "";
        var isAuto = false;
        for (var i = 0; i < sportId.length; i++) {
            var elements = '';
            var pre = "Hdp";
            if (i == 6) pre = "OU";
            if (document.getElementById('chkUpd_' + sportId[i])) document.getElementById('chkUpd_' + sportId[i]).disabled = chkCopy.checked;
            if (document.getElementById("divUpdPT" + sportId[i])) {
                elements = document.getElementById("divUpdPT" + sportId[i]).getElementsByTagName("select");
                for (var j = 0; j < elements.length; j++) {
                    var id = elements[j].id;
                    if (id == "listS2Hdp" || id == "autoM2Hdp" || id == "listM2Hdp") continue;
                    elements[j].disabled = chkCopy.checked;
                }
            }
            if (document.getElementById("divUpdPT" + sportId[i])) {
                elements = document.getElementById("divUpdPT" + sportId[i]).getElementsByTagName("input");
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
        if (document.getElementById(chkId)) {
            document.getElementById(chkId).disabled = chkCopy.checked;
        }
        if (chkCopy.checked) {
            if (document.getElementById(autoMId)) {
                if (document.getElementById(autoMId).value == 1) {
                    if (document.getElementById(autoId)) {
                        document.getElementById(autoId).disabled = chkCopy.checked;
                    }
                }
                else {
                    if (document.getElementById(arrSuper[i])) {

                        document.getElementById(arrSuper[i]).disabled = chkCopy.checked;
                    }
                    if (document.getElementById(autoId)) {
                        document.getElementById(autoId).disabled = chkCopy.checked;
                    }
                }
            }
            else {
                if (document.getElementById(arrSuper[i])) {

                    document.getElementById(arrSuper[i]).disabled = chkCopy.checked;
                }
                if (document.getElementById(autoId)) {
                    document.getElementById(autoId).disabled = chkCopy.checked;
                }
            }
        }
        else {

            if (document.getElementById(autoMId)) {
                if (document.getElementById(autoMId).value == 1) {
                    if (document.getElementById(autoId)) {
                        document.getElementById(autoId).disabled = !document.getElementById(chkId).checked;
                    }
                }
                else {
                    if (document.getElementById(arrSuper[i])) {
                        document.getElementById(arrSuper[i]).disabled = document.getElementById(chkId).checked;
                    }
                    if (document.getElementById(autoId)) {
                        document.getElementById(autoId).disabled = !document.getElementById(chkId).checked;
                    }
                }
            }
            else {

                if (document.getElementById(arrSuper[i])) {
                    document.getElementById(arrSuper[i]).disabled = document.getElementById(chkId).checked;
                }
                if (document.getElementById(autoId)) {
                    document.getElementById(autoId).disabled = !document.getElementById(chkId).checked;
                }
            }
        }
    }
}
function CheckAutoPreset(objChk) {

    var sId = objChk.id.replace("chkAuto", "listM");
    var autoId = objChk.id.replace("chkAuto", "autoM");
    var MAutoId = objChk.id.replace("chkAuto", "isAutoM");
    if (document.getElementById(MAutoId)) {
        if (document.getElementById(MAutoId).value == 1) {
            document.getElementById(autoId).disabled = !objChk.checked;
        }
        else {
            if (document.getElementById(sId)) {
                document.getElementById(sId).disabled = objChk.checked;
            }
            document.getElementById(autoId).disabled = !objChk.checked;
        }
    }
    else {
        if (document.getElementById(sId)) {
            document.getElementById(sId).disabled = objChk.checked;
        }
        else {

            if (objChk.id == "chkAuto1HdpD") { // check copy all	            
                if (!objChk.checked && document.getElementById("chkCopyAll").checked == true) {
                    document.getElementById("spCopyAll").checked = document.getElementById("chkCopyAll").checked = false;
                    CopyAllPT(document.getElementById("chkCopyAll"));
                }
                if (!objChk.checked && document.getElementById("chkCopy1HdpDeadball").checked == true) {
                    document.getElementById("chkCopy1HdpDeadball").checked = false;
                    CheckCopyPT(document.getElementById("chkCopy1HdpDeadball"), arr1Super1);
                }
                if (document.getElementById("chkCopyBasket")) {
                    if (document.getElementById("chkCopyBasket").checked) {
                        document.getElementById("spCopyAll").checked = document.getElementById("chkCopyAll").disabled = true;
                    }
                    else {
                        document.getElementById("spCopyAll").checked = document.getElementById("chkCopyAll").disabled = !objChk.checked;
                    }
                }
                document.getElementById("chkCopy1HdpDeadball").disabled = !objChk.checked;
            }
            else if (objChk.id == "chkAuto11x2") {

                if (!objChk.checked && document.getElementById("chkCopy11x2").checked == true) {
                    document.getElementById("chkCopy11x2").checked = false;
                    CheckCopyPT(document.getElementById("chkCopy11x2"), arr2Super1);
                }
                document.getElementById("chkCopy11x2").disabled = !objChk.checked;
            }
            else if (objChk.id == "chkAuto1HdpL") {
                document.getElementById("chkCopy1HdpLive").disabled = !objChk.checked;
                if (!objChk.checked && document.getElementById("chkCopy1HdpLive").checked == true) {
                    document.getElementById("chkCopy1HdpLive").checked = false;
                    CheckCopyPT(document.getElementById("chkCopy1HdpLive"), arr3Super1);
                }
            }
            else if (objChk.id == "chkAuto2Hdp") { // Check copy basket ball

                if (!objChk.checked && document.getElementById("chkCopyBasket").checked == true) {
                    document.getElementById("spCopyBasket").checked = document.getElementById("chkCopyBasket").checked = false;
                    CopyBasketPT(document.getElementById("chkCopyBasket"));
                }
                if (!objChk.checked && document.getElementById("chkCopy2Hdp").checked == true) {
                    document.getElementById("chkCopy2Hdp").checked = false;
                    CheckCopyPT(document.getElementById("chkCopy2Hdp"), arrSuper2);
                }
                document.getElementById("spCopyBasket").disabled = document.getElementById("chkCopyBasket").disabled = document.getElementById("chkCopy2Hdp").disabled = !objChk.checked;

            }
            else if (objChk.id == "chkAutoTOWin") { // Check copy basket ball
                if (document.getElementById("chkCopyAll")) {
                    if (!objChk.checked && document.getElementById("chkCopyAll").checked == true) {
                        document.getElementById("spCopyAll").checked = document.getElementById("chkCopyAll").checked = false;
                        CopyAllPT(document.getElementById("chkCopyAll"));
                    }
                    if (!objChk.checked && document.getElementById("chkCopyTOWin").checked == true) {
                        document.getElementById("chkCopyTOWin").checked = false;
                        CheckCopyPT(document.getElementById("chkCopyTOWin"), arrSuper151TO);
                    }
                    document.getElementById("spCopyAll").checked = document.getElementById("chkCopyAll").disabled = document.getElementById("chkCopyTOWin").disabled = !objChk.checked;
                }
                else {
                    document.getElementById("chkCopyTOWin").disabled = !objChk.checked;
                }
            }
            else {

                var idChkCopy = objChk.id.replace("chkAuto", "chkCopy");
                if (document.getElementById(idChkCopy)) {
                    var sportId = ["3", "5", "8", "10", "11", "201", "99", "TO", "FO"];
                    var arrSuper = [arrSuper3, arrSuper5, arrSuper8, arrSuper10, arrSuper11, arrSuper201, arrSuper99, arrSuper151TO, arrSuper151FO];


                    for (var i = 0; i < sportId.length; i++) {
                        var pre = "Hdp";
                        if (i == 7 || i == 8) pre = "Win";
                        if (i == 5) pre = "OU";
                        if (objChk.id == "chkAuto" + sportId[i] + pre) {
                            if (!objChk.checked && document.getElementById(idChkCopy).checked == true) {

                                document.getElementById(idChkCopy).checked = false;
                                CheckCopyPT(document.getElementById(idChkCopy), arrSuper[i]);
                                break;
                            }
                        }
                    }
                    document.getElementById(idChkCopy).disabled = !objChk.checked;
                }
            }
        }
        document.getElementById(autoId).disabled = !objChk.checked;
    }
}
function IncreasePT(ddl, step) {
    var max = document.getElementById(ddl).options[0].value;
    Increase(ddl, step, max);
}
function ReducePT(ddl, step) {
    var min = 0;
    min = document.getElementById(ddl).options[document.getElementById(ddl).length - 1].value;
    Reduce(ddl, step, min);
}
function AllowUpdate(chkObj) {
    document.getElementById('chkClosed').disabled = (chkObj.checked) ? false : true;
}