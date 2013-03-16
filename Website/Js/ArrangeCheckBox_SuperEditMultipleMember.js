window.ArrDisableCotrol = new Array();
window.ArrAll = new Array();
var ArrSportTypes = ['1', '2', '3', '5', '8', '10', '11', '99', '99_M', '161'];
var sportId = ["1", "2", "3", "5", "8", "10", "11", "201", "99", "TO", "FO", "99_M"];
var chkUpdPT = ["chkUpd_1", "chkUpd_2", "chkUpd_3", "chkUpd_5", "chkUpd_8", "chkUpd_10", "chkUpd_11", "chkUpd_201", "chkUpd_99", "chkUpd_TO", "cchkUpd_FO", "chkUpd_99_M"];


function InitDisableControl() {

    if (document.getElementById('isMult').value == 1) {
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
    var Elements = document.getElementById(parentElmName).getElementsByTagName(elmName);
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

    if (document.getElementById("divUpdPT" + sportType)) { // PT Div is available
        var elements = document.getElementById("divUpdPT" + sportType).getElementsByTagName("input");
        if (document.getElementById("chkUpd_" + sportType).disabled) { // chkPT is disabled. all  control's childs is disabled
            for (var j = 0; j < elements.length; j++) {
                elements[j].disabled = true;
            }
            var elm = document.getElementById("divUpdPT" + sportType).getElementsByTagName("select");
            for (var i = 0; i < elm.length; i++) {
                elm[i].disabled = true;
            }
        }
        else { // chkPT is Enabled
            if (document.getElementById('chkUpd_' + sportType).checked) { // chkPT is checked
                for (var j = 0; j < elements.length; j++) {
                    var id = elements[j].id;
                    var chkcopy = "chkCopy" + sportType + "Hdp";
                    if (elements[j].id == chkcopy) {
                        elements[j].disabled = false;
                        if (elements[j].checked) {
                            CheckCopyPT(elements[j], arrSuper);
                            var chkAuto = "chkAuto" + sportType + "Hdp";
                            var chkAutoHdp = document.getElementById(chkAuto);
                            document.getElementById(chkAuto).disabled = false;
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
                var elm = document.getElementById("divUpdPT" + sportType).getElementsByTagName("select");
                for (var i = 0; i < elm.length; i++) {
                    elm[i].disabled = true;
                }
            }
        }
    }
}

function getElementDivforBasketball() {
    if (document.getElementById("divUpdPT2")) {
        var elements = document.getElementById("divUpdPT2").getElementsByTagName("input");
        var elm = document.getElementById("divUpdPT2").getElementsByTagName("select"); // all select controls init is disabled
        for (var i = 0; i < elm.length; i++) {
            elm[i].disabled = true;
        }
        if (document.getElementById('chkUpd_2').disabled) {
            for (var j = 0; j < elements.length; j++) {
                elements[j].disabled = true;
            }
        }
        else {
            if (document.getElementById('chkUpd_2').checked) { // chkPT is checked     
                if (document.getElementById('chkCopyBasket').checked) { // chkCopyAllBK is checked
                    for (var j = 0; j < elements.length; j++) {
                        if (elements[j].id == "chkAuto2Hdp") {
                            elements[j].disabled = false;
                            CheckAutoPreset(elements[j]);
                        }
                        else {
                            elements[j].disabled = true;
                        }
                    }
                    if (!document.getElementById("listS2Hdp")) {
                        document.getElementById("chkCopy2Hdp").disabled = true;
                    }
                }
                else { // chkCopyAllBK is unchecked
                    for (var j = 0; j < elements.length; j++) {
                        elements[j].disabled = false;
                        if (elements[j].id == "chkCopy2Hdp") continue;
                        CheckAutoPreset(elements[j]);
                    }
                    CheckCopyPT(document.getElementById("chkCopy2Hdp"), arrSuper2);
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
    if (document.getElementById("divUpdPT1")) {
        elements = document.getElementById("divUpdPT1").getElementsByTagName("input");
        var elm = document.getElementById("divUpdPT1").getElementsByTagName("select"); // all select controls init is disabled
        for (var i = 0; i < elm.length; i++) {
            elm[i].disabled = true;
        }

        if (document.getElementById('chkUpd_1').disabled) {
            for (var j = 0; j < elements.length; j++) {
                elements[j].disabled = true;
            }
        }
        else {
            if (document.getElementById('chkUpd_1').checked) {
                if (document.getElementById('chkCopyAll').checked) {
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
                    if (document.getElementById("listS1HdpD")) {
                        document.getElementById("chkCopy1HdpDeadball").disabled = false;
                    }
                    else {
                        document.getElementById("chkCopy1HdpDeadball").disabled = true;
                    }

                    if (document.getElementById("listS11x2")) {
                        document.getElementById("chkCopy11x2").disabled = false;
                    }
                    else {
                        document.getElementById("chkCopy11x2").disabled = true;
                    }

                    if (document.getElementById("listS1HdpL")) {
                        document.getElementById("chkCopy1HdpLive").disabled = false;
                    }
                    else {
                        document.getElementById("chkCopy1HdpLive").disabled = true;
                    }

                    for (var j = 0; j < elements.length; j++) {
                        
                        if (elements[j].id == "chkCopy1HdpDeadball" || elements[j].id == "chkCopy11x2" || elements[j].id == "chkCopy1HdpLive") continue;
                        elements[j].disabled = false;
                        CheckAutoPreset(elements[j]);
                    }
                    CheckCopyPT(document.getElementById("chkCopy1HdpDeadball"), arr1Super1);
                    CheckCopyPT(document.getElementById("chkCopy11x2"), arr2Super1);
                    CheckCopyPT(document.getElementById("chkCopy1HdpLive"), arr3Super1);
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