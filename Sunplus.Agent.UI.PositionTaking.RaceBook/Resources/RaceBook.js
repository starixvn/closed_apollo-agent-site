function ShowHide(el) {
    if (el.nextSibling.style.display == 'none') {
        el.nextSibling.style.display = 'block';
        el.className = 'menu_content1';
    }
    else {
        el.nextSibling.style.display = 'none';
        el.className = 'menu_content';
    }
}

function CheckAuto(el, id_1, id_2, idMax) {
    var idList = el.id.replace('chkAuto', 'list');
    var idChkAuto = el.id.replace('chk', 'list');
    if ($(idList)) {
        $(idList).disabled = el.checked;
    }
    else {
        var ID = el.id;
        var suffix = ID.substring(ID.indexOf('_'), ID.length);
        if ($('chkCopyFromWin' + suffix)) $('chkCopyFromWin' + suffix).disabled = !el.checked;
    }
    if ($(idChkAuto)) $(idChkAuto).disabled = !el.checked;
    ResetValue(el, id_1, id_2, idMax);
}

function ResetValue(el, id_1, id_2, idMax) {

    if (!idMax) return;

    var isAuto = el.checked;
    if (isAuto) // Auto PT is checked
    {
        var id = '_page.' + idMax;
        var maxVal = eval(id);
        PT_RenderCombo($(id_2), 0, maxVal, 0.1, $(id_2).value);
    }
    else  // Not Auto
    {
        var maxVal = $(id_1).options[0].value;
        var curVal = $(id_1).value;
        maxVal = maxVal - curVal;
        if (curVal == 0) {
            var id = '_page.' + idMax;
            var maxVal = eval(id);
        }
        PT_RenderCombo($(id_2), 0, maxVal, 0.1, $(id_2).value);
    }
}


// Copy from Win action
window.enableCopyList = new Array();
function CheckCopy(e, Arr, ArrIsAuto) {
    if (e.checked) {
        window.enableCopyList = new Array();
        for (i = 0; i < Arr.length; i++) {
            if ($(Arr[i]) && !$(Arr[i]).disabled) {
                $(Arr[i]).disabled = true;
                window.enableCopyList[enableCopyList.length] = $(Arr[i]);
            }
        }
    }
    else {
        for (var i = 0; i < window.enableCopyList.length; i++) {
            window.enableCopyList[i].disabled = false;
        }
    }
}


var Arr_tblId = ["tbl_151", "tbl_152", "tbl_153"];
var ArrType = ["input", "select"];
window.enableList = new Array();

Array.prototype.contains = function(item) {
    var c = this.length;
    for (var i = 0; i < c; i++) {
        if (this[i] == item) return true;
    }

    return false;
}

function chkCopyAll_OnClick(el, ArrExc) {
    if (el.checked) {
        window.enableList = new Array();
        for (var i = 0; i < Arr_tblId.length; i++) {
            for (var j = 0; j < ArrType.length; j++) {
                var Elements = $(Arr_tblId[i]).getElementsByTagName(ArrType[j]);
                for (var k = 0; k < Elements.length; k++) {
                    var elm = Elements[k];
                    if (ArrExc.contains(elm.id)) continue;
                    if (!elm.disabled) {
                        elm.disabled = true;
                        window.enableList[enableList.length] = elm;
                    }
                }
            }
        }
    }
    else {
        for (var i = 0; i < window.enableList.length; i++) {
            window.enableList[i].disabled = false;
        }
    }
}


function SelectOnChangePT(el, idComb, idMax) {
    var maxVal = el.options[0].value;
    var curVal = el.value;
    maxVal = maxVal - curVal;
    if (curVal == 0) {
        var id = '_page.' + idMax;
        maxVal = eval(id);
    }

    PT_RenderCombo($(idComb), 0, maxVal, 0.1);
}

function GetCustID_Mult(divUserName) {
    var CustIDs = parent.$("arrayCustID");
    var UserNames = parent.$("arrayUserName");
    if (CustIDs.value != null || UserNames.value != null) {
        var arrUserName = UserNames.value.split("^");
        if (arrUserName.length > 1) {
            $(divUserName).innerHTML = '';
            for (var i = 0; i < arrUserName.length; i++) {
                if (arrUserName[i] != '') {
                    if (i >= arrUserName.length - 2) {
                        $(divUserName).innerHTML += arrUserName[i];
                    }
                    else {
                        $(divUserName).innerHTML += arrUserName[i] + ', ';
                    }
                }
            }
        }
    }
}

var ArrMsgId = ["1_1", "1_2", "1_3", "2_1", "2_2", "3_1", "3_2", "3_3"];
function Active_EnrichMessage(isSucc) {
    $('Div_Message').display = 'block';
    for (var i = 0; i < ArrMsgId.length; i++) {
        if (isSucc == 1 || isSucc == true) {
            $(ArrMsgId[i]).className = 'succ_' + ArrMsgId[i];
        }
        else {
            $(ArrMsgId[i]).className = 'emsg_' + ArrMsgId[i];
        }
    }
}

function CreateParams(elements) {

    var params = new Array();
    var c = elements.length;

    for (var i = 0; i < c; i++) {
        var element = elements[i];
        var query = element.name;
        if (null == query || '' == query) {
            query = element.id;
        }
        if (null == query || '' == query) {
            continue;
        }
        var value = null;
        if (element.tagName == 'INPUT') {
            if (element.type == 'checkbox' || element.type == 'radio')
                value = element.checked;
            else
                value = element.value;
        }
        else if (element.tagName == 'SELECT')
            value = element.value;

        if (null != value)
            params.push(query + '=' + escape(value));
    }

    return params;
}

function btnSubmit_OnClick() {

    // Get all controls and post ajax do form's action

    var inputs = (document.getElementsByTagName("input"));
    var selects = (document.getElementsByTagName("select"));
    var custid = age.GetQueryString("custid", 0);

    var params = ((CreateParams(inputs)).concat(CreateParams(selects))).join('&');
    params = ("custid=" + custid + "&") + params;

    function OnComplete(result) {
        var errCode = result.errCode;
        if (errCode == 0) {

        }
        else if (result.errCode == 105) {

        }
        else {
        }
    }


    // Post with return result in JSON format
    ajax.PostJSON(
        document.forms[0].action,
        params,
        OnComplete
    );
}

var ArrCopyFromWin = ['151_41', '151_31', '151_51', '152_41', '152_51', '153_51'];

function initRaceBook() {

    if (_page.isView) return;

    var c = ArrCopyFromWin.length;
    for (var i = 0; i < c; i++) {
        eval("var ArrCopyFromWin_OnClick_" + i + " = function() {CheckCopy($('chkCopyFromWin_' + ArrCopyFromWin[" + i + "]), eval('Arr_' + ArrCopyFromWin[" + i + "]), eval('ArrisAuto_' + ArrCopyFromWin[" + i + "]));}");
        eval("age.addEvent($('chkCopyFromWin_' + ArrCopyFromWin[" + i + "]), 'click', ArrCopyFromWin_OnClick_" + i + ");");
    }
}




RegisterStartUp("initRaceBook()");
