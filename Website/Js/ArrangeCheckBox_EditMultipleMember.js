window.ArrDisableCotrol = new Array();
window.ArrAll = new Array();
var ArrSportTypes = ['1', '2', '3', '5', '8', '10', '11', '99', '99_M', '161'];
var sportId = ["1", "2", "3", "5", "8", "10", "11", "201", "99", "TO", "FO", "99_M"];
var chkUpdPT = ["chkUpd_1", "chkUpd_2", "chkUpd_3", "chkUpd_5", "chkUpd_8", "chkUpd_10", "chkUpd_11", "chkUpd_201", "chkUpd_99", "chkUpd_TO", "cchkUpd_FO", "chkUpd_99_M"];
var arr1Super1 = arr1Master1;
var arr2Super1 = arr2Master1;
var arr3Super1 = arr3Master1;

var arrSuper2 = arrMaster2; // basketball

function InitDisableControl() {

    if ($('isMult').value == 1) {
        for (var i = 0; i < ArrSportTypes.length; i++) {
            window.ArrDisableCotrol = new Array();

            GetElement("input", "divUpdPT" + ArrSportTypes[i]);
            GetElement("select", "divUpdPT" + ArrSportTypes[i]);
            GetElement("a", "divUpdPT" + ArrSportTypes[i]);
            window.ArrAll[ArrAll.length] = window.ArrDisableCotrol;
        }
    }
}

function GetElement(elmName, parentElmName) {
    var Elements = $(parentElmName).getElementsByTagName(elmName);
    for (var k = 0; k < Elements.length; k++) {
        var elm = Elements[k];
        if (!elm.disabled) {
            elm.disabled = true;
            window.ArrDisableCotrol[ArrDisableCotrol.length] = elm;
        }
    }
}

function AllowUpdatePT(elm, sporttype) {


    if (sporttype == 1) {
        getElementDivforSoccer();
    }
    else if (sporttype == 2) {
        getElementDivforBasketball();
    }
    else if (sporttype == 161) {
        if (elm.checked) {
            for (var i = 0; i < ArrSportTypes.length; i++) {
                if (ArrSportTypes[i] == sporttype) {
                    for (var j = 0; j < window.ArrAll[i].length; j++) {
                        if (window.ArrAll[i][j].disabled)
                            window.ArrAll[i][j].disabled = false;
                    }

                }
            }
        }
        else {
            for (var i = 0; i < ArrSportTypes.length; i++) {
                if (ArrSportTypes[i] == sporttype) {
                    window.ArrDisableCotrol = new Array();

                    GetElement("input", "divUpdPT" + ArrSportTypes[i]);
                    GetElement("select", "divUpdPT" + ArrSportTypes[i]);
                    GetElement("a", "divUpdPT" + ArrSportTypes[i]);
                    window.ArrAll[i] = window.ArrDisableCotrol;
                }
            }
        }
    }
    else {
        getElementDiv(elm, sporttype);
    }

}

// Insert by David 15-02-2012
// Use for edit multi
function getElementDiv(chkObj, sportType) {
    var arrSuper;
    switch (chkObj.id) {
        case "chkUpd_3":
            {
                arrSuper = arrSuper3;
                break;
            }
        case "chkUpd_5":
            {
                arrSuper = arrSuper5;
                break;
            }
        case "chkUpd_8":
            {
                arrSuper = arrSuper8;
                break;
            }
        case "chkUpd_10":
            {
                arrSuper = arrSuper10;
                break;
            }
        case "chkUpd_11":
            {
                arrSuper = arrSuper11;
                break;
            }
        case "chkUpd_201":
            {
                arrSuper = arrSuper201;
                break;
            }
        case "chkUpd_99":
            {
                arrSuper = arrSuper99;
                break;
            }
    }

    if ($("divUpdPT" + sportType)) { // PT Div is available
        var elements = $("divUpdPT" + sportType).getElementsByTagName("input");
        if ($("chkUpd_" + sportType).disabled) { // chkPT is disabled. all  control's childs is disabled
            for (var j = 0; j < elements.length; j++) {
                elements[j].disabled = true;
            }
            var elm = $("divUpdPT" + sportType).getElementsByTagName("select");
            for (var i = 0; i < elm.length; i++) {
                elm[i].disabled = true;
            }
        }
        else { // chkPT is Enabled
            if ($('chkUpd_' + sportType).checked) { // chkPT is checked
                for (var j = 0; j < elements.length; j++) {
                    var id = elements[j].id;
                    var chkcopy = "chkCopy" + sportType + "Hdp";
                    if (elements[j].id == chkcopy) {
                        elements[j].disabled = false;
                        if (elements[j].checked) {
                            CheckCopyPT(elements[j], arrSuper);
                            var chkAuto = "chkAuto" + sportType + "Hdp";
                            var chkAutoHdp = document.getElementById(chkAuto);
                            $(chkAuto).disabled = false;
                            CheckAutoPreset(chkAutoHdp);
                            return;
                        }
                    }
                    else {
                        elements[j].disabled = false;
                        CheckAutoPreset(elements[j]);
                    }
                }
            }
            else { // chkPT is unchecked. all  control's childs is disabled
                for (var j = 0; j < elements.length; j++) {
                    elements[j].disabled = true;
                }
                var elm = $("divUpdPT" + sportType).getElementsByTagName("select");
                for (var i = 0; i < elm.length; i++) {
                    elm[i].disabled = true;
                }
            }
        }
    }
}

function getElementDivforBasketball() {
    if ($("divUpdPT2")) {
        var elements = $("divUpdPT2").getElementsByTagName("input");
        var elm = $("divUpdPT2").getElementsByTagName("select"); // all select controls init is disabled
        for (var i = 0; i < elm.length; i++) {
            elm[i].disabled = true;
        }
        if ($('chkUpd_2').disabled) {
            for (var j = 0; j < elements.length; j++) {
                elements[j].disabled = true;
            }
        }
        else {
            if ($('chkUpd_2').checked) { // chkPT is checked     
                if ($('chkCopyBasket').checked) { // chkCopyAllBK is checked
                    for (var j = 0; j < elements.length; j++) {
                        if (elements[j].id == "chkAuto2Hdp") {
                            elements[j].disabled = false;
                            CheckAutoPreset(elements[j]);
                        }
                        else {
                            elements[j].disabled = true;
                        }
                    }
                    if (!$("listM2Hdp")) {
                        $("chkCopy2Hdp").disabled = true;
                    }
                }
                else { // chkCopyAllBK is unchecked
                    for (var j = 0; j < elements.length; j++) {
                        elements[j].disabled = false;
                        if (elements[j].id == "chkCopy2Hdp") continue;
                        CheckAutoPreset(elements[j]);
                    }
                    CheckCopyPT($("chkCopy2Hdp"), arrSuper2);
                }
            }
            else { // chkPT is unchecked . consist of two case chkCopyAllBK is checked or unchecked
                for (var j = 0; j < elements.length; j++) {
                    elements[j].disabled = true;
                }
            }
        }
    }
}

function getElementDivforSoccer() {
    if ($("divUpdPT1")) {
        elements = $("divUpdPT1").getElementsByTagName("input");
        var elm = $("divUpdPT1").getElementsByTagName("select"); // all select controls init is disabled
        for (var i = 0; i < elm.length; i++) {
            elm[i].disabled = true;
        }

        if ($('chkUpd_1').disabled) {
            for (var j = 0; j < elements.length; j++) {
                elements[j].disabled = true;
            }
        }
        else {
            if ($('chkUpd_1').checked) {
                if ($('chkCopyAll').checked) {
                    for (var j = 0; j < elements.length; j++) {
                        if (elements[j].id == "chkAuto1HdpD") {
                            elements[j].disabled = false;
                            CheckAutoPreset(elements[j]);
                        }
                        else {
                            elements[j].disabled = true;
                        }
                    }
                }
                else {
                    if ($("listM1HdpD")) {
                        $("chkCopy1HdpDeadball").disabled = false;
                    }
                    else {
                        $("chkCopy1HdpDeadball").disabled = true;
                    }

                    if ($("listM11x2")) {
                        $("chkCopy11x2").disabled = false;
                    }
                    else {
                        $("chkCopy11x2").disabled = true;
                    }

                    if ($("listM1HdpL")) {
                        $("chkCopy1HdpLive").disabled = false;
                    }
                    else {
                        $("chkCopy1HdpLive").disabled = true;
                    }

                    for (var j = 0; j < elements.length; j++) {

                        if (elements[j].id == "chkCopy1HdpDeadball" || elements[j].id == "chkCopy11x2" || elements[j].id == "chkCopy1HdpLive") continue;
                        elements[j].disabled = false;
                        CheckAutoPreset(elements[j]);
                    }
                    CheckCopyPT($("chkCopy1HdpDeadball"), arr1Super1);
                    CheckCopyPT($("chkCopy11x2"), arr2Super1);
                    CheckCopyPT($("chkCopy1HdpLive"), arr3Super1);
                }
            }
            else {
                for (var j = 0; j < elements.length; j++) {
                    elements[j].disabled = true;
                }
            }
        }
    }
}
//