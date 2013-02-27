var Arr_tblId = ["tbl_151", "tbl_152", "tbl_153"];
var ArrType = ["input", "select"];
var ArrBetType = ['151_41', '151_31', '151_51', '152_41', '152_51', '153_51'];
var AllBetType = ['151_41', '151_42', '151_31', '151_32', '151_51', '151_52', '152_41', '151_42', '152_51', '152_52', '153_51', '153_52'];

var ArrExcAllowUpdatePT = ['ChkUpdateStatus_151_41', 'ChkUpdateStatus_151_31', 'ChkUpdateStatus_151_51', 'ChkUpdateStatus_152_41', 'ChkUpdateStatus_152_51', 'ChkUpdateStatus_153_51'];

var isSucc = true;

var ErrCode = '';
var ErrMsg = '';
var UserName = '';
var custids = [];


window.enableCopyList = new Array();
window.enableList = new Array();
window.enableAllowUpdate = new Array();
window.enableCopyAll = new Array();
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

function AutoShowHide(divID) {
    if ($(divID).nextSibling.style.display == 'none') {
        $(divID).nextSibling.style.display = 'block';
        $(divID).className = 'menu_content1';
    }
}

function CheckAuto(el, id_1, id_2, idMax_1, idMax_2, idMin) {
    var idList = el.id.replace('chkAuto', 'list');
    var idChkAuto = el.id.replace('chk', 'list');
    if ($(idList)) {
        $(idList).disabled = el.checked;
    }
    else {
        var ID = el.id;
        var suffix = ID.substring(ID.indexOf('_'), ID.length);
        if ($('chkCopyFromWin' + suffix)) {
            $('chkCopyFromWin' + suffix).disabled = !el.checked;

            if ($('chkCopyFromWin' + suffix).checked) {
                $('chkCopyFromWin' + suffix).checked = false;
                var _arr = "Arr" + suffix;
                SetCopy_Control(false, eval(_arr), Getlocation(el));
            }
        }

        if (suffix == "_151_41") {
            if ($('chkCopyAll')) {
                $('chkCopyAll').disabled = el.checked ? false : true;

                if ($('chkCopyAll').checked) {
                    $('chkCopyAll').checked = false;
                    chkCopyAll_OnClick($('chkCopyAll'), ArrExc);
                    $('chkCopyFromWin_151_41').disabled = true;
                }
            }

        }
    }
    if ($(idChkAuto)) $(idChkAuto).disabled = !el.checked;
    if ($(id_1) || $(id_2)) {
        ResetValue(el, id_1, id_2, idMax_1, idMax_2, idMin);
    }
}

function GetValue(id) {
    var val = 0;
    if (id != '' && id != 'undefined' && id != null) {
        var idPage = '_page.' + id;
        val = eval(idPage);
    }
    return val;

}

function ResetValue(el, id_1, id_2, idMax_1, idMax_2, idMin) {

    if (!idMax_1 || !idMax_2) return;

    var isAuto = el.checked;
    if (isAuto) // Auto PT is checked
    {
        var maxVal = GetValue(idMax_1);
        var minVal = GetValue(idMin);

        PT_RenderCombo($(id_2), minVal, maxVal, 0.1, $(id_2).value);
    }
    else  // Not Auto
    {
        var maxVal_1 = GetValue(idMax_1);
        var maxVal_2 = GetValue(idMax_2);

        var curVal = $(id_1).value;
        var minVal = GetValue(idMin);
        var maxVal = GetMaxValue(maxVal_1, maxVal_2, curVal);

        PT_RenderCombo($(id_2), minVal, maxVal, 0.1, $(id_2).value);
    }
}


// Copy from Win action
function Getlocation(elm) {
    var suffix = elm.id.substring(elm.id.indexOf('_') + 1, elm.id.length);
    for (var i = 0; i < ArrBetType.length; i++) {
        if (suffix == ArrBetType[i]) return i;
    }
}
function CheckCopy_Onclick(e, Arr) {
    SetCopy_Control(e.checked, Arr, Getlocation(e));
}

function SetCopy_Control(ischeck, Arr, location) {
    var arr_temp = new Array();
    if (ischeck) {
        for (var i = 0; i < Arr.length; i++) {
            if ($(Arr[i]) && !$(Arr[i]).disabled) {
                $(Arr[i]).disabled = true;
                arr_temp[arr_temp.length] = $(Arr[i]);
            }
        }
        window.enableCopyList[location] = arr_temp;
    }
    else {
        for (var i = 0; i < window.enableCopyList[location].length; i++) {
            window.enableCopyList[location][i].disabled = false;
        }
    }
}

Array.prototype.contains = function (item) {
    var c = this.length;
    for (var i = 0; i < c; i++) {
        if (this[i] == item) return true;
    }

    return false;
}





//Max_1 : maxMS
//Max_2 : maxM
function GetMaxValue(Max_1, Max_2, Curr) {
    var max = 0;
    max = Max_1 - Curr;
    if (max > Max_2)
        max = Max_2;
    return max;

}


function SelectOnChangePT(el, idComb, idMax_1, idMax_2, idMin) {
    var maxVal_1 = GetValue(idMax_1);
    var maxVal_2 = GetValue(idMax_2);
    var curVal = el.value;
    var minVal = GetValue(idMin);

    var maxVal = GetMaxValue(maxVal_1, maxVal_2, curVal);

    PT_RenderCombo($(idComb), minVal, maxVal, 0.1);
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
            if (element.type == 'checkbox' || element.type == 'radio') {

                value = element.checked;
            }
            else {
                value = element.value;
            }
        }
        else if (element.tagName == 'SELECT') {
            value = element.disabled ? 0 : element.value;
        }

        if (null != value) {
            params.push(query + '=' + escape(value));
        }
    }


    return params;
}

function UpdateCustomer(custid) {
    var inputs = (document.getElementsByTagName("input"));
    var selects = (document.getElementsByTagName("select"));
    var params = ((CreateParams(inputs)).concat(CreateParams(selects))).join('&');
    params = ("custid=" + custid + "&") + params;
    params = ("isedit=" + (_page.isEdit != false) + "&") + params;
    params = ("ismult=" + (_page.isMultiple == "1") + "&") + params;

    // Post with return result in JSON format
    ajax.PostJSON(
        document.forms[0].action,
        params,
        OnComplete);

    function OnComplete(result) {

        if (result.errCode == 1) {
            age.HideMaskDiv();
            ageMsg.Show(result.errMsg, false);
            return;
        }

        ErrCode = result.ArrCodes;
        ErrMsg = result.ArrMsgs;
        UserName = result.ArrUsernames;

        OnUpdatedCustomer();
    }
}

function OnUpdatedCustomer() {

    top.errCodes.push(ErrCode);
    top.errMsgs.push(ErrMsg);
    top.userNames.push(UserName);

    isSucc = isSucc & ageMsg.CheckMessageIsSucc(ErrCode);

    if (custids.length > 0) {
        var custid = custids.pop();
        UpdateCustomer(custid);
    }
    else {

        if (isSucc == false) {
            age.HideMaskDiv();
            ShowErrorMessages(false);
            return;
        }

        if (isSucc && (age.GetQueryString("popup") != 'false')) { // from account left menu or is add new
            if (parent.ageWnd) {

                top.errCodes = new Array();
                top.errMsgs = new Array();
                top.userNames = new Array();

                if (ErrCode != -1)
                    setTimeout('parent.ageWnd.close()', 1000);
                else
                    setTimeout('parent.ageWnd.close();' + ErrMsg, 1000);
            }
            else {
                age.DelayReloadPage(null, 1000);
            }
            return;
        }

        age.DelayReloadPage(null, 2000);
    }
}

function btnSubmit_OnClick() {

    ShowErrorMessages(true);

    if (parent.document.getElementById("arrayCustID")) {
        var arrayCustID = parent.$("arrayCustID").value;
        arrayCustID = arrayCustID.substring(0, arrayCustID.indexOf("^") != -1 ? arrayCustID.length - 1 : arrayCustID.length);
        custids = arrayCustID.split('^').reverse();
    }
    else {
        var custid = age.GetQueryString("custid", 0);
        custids.push(custid);
    }
    var isCheck = CheckValidateBetSetting('chkCopyAll');
    if (!isCheck) {
        return;
    }

    var isCheckAutoChange = false;
    if (window.ConfirmChangeAuto) isCheckAutoChange = ConfirmChangeAuto();

    if (isCheckAutoChange) {
        return;
    }
    else {
        age.DelayReloadPage(null, 30000);
        UpdateCustomer(custids.pop());
    }
}

function checkDisableBetTypes() {
    if (!_page.IsShowRacing_151_31) {
        $('div_151_31').style.display = 'none';
        $('div_151_31').nextSibling.style.display = 'none';
    }

    if (!_page.IsShowRacing_151_51) {
        $('div_151_51').style.display = 'none';
        $('div_151_51').nextSibling.style.display = 'none';
    }

    if (!_page.IsShowRacing_152_41) {
        $('div_152_41').style.display = 'none';
        $('div_152_41').nextSibling.style.display = 'none';
    }

    if (!_page.IsShowRacing_152_51) {
        $('div_152_51').style.display = 'none';
        $('div_152_51').nextSibling.style.display = 'none';
    }

    if (!_page.IsShowRacing_152_41 && !_page.IsShowRacing_152_51) {
        $('tbl_152').style.display = 'none';
    }

    if (!_page.IsShowRacing_153_51) {
        $('tbl_153').style.display = 'none';
    }

    document.body.style.display = 'block';
}

function initRaceBook() {

    checkDisableBetTypes();

    if (_page.isView) return;

    ShowErrorMessages(false);
    if (_page.isMultiple == '1') Init_DisabledControl();

}

function ShowErrorMessages(clear) {

    if (clear == true) {
        top.errCodes = new Array();
        top.errMsgs = new Array();
        top.userNames = new Array();

        custids = [];

        ageMsg.Hide();
    }

    // Re-show error messages
    var c = top.errCodes.length;
    if (c == 1) { // For Edit MinPT case
        top.userNames[0] = [_page.username];
    }
    for (var i = 0; i < c; i++) {
        ageMsg.GenerateMessage(top.errCodes.pop(), top.errMsgs.pop(), top.userNames.pop(), i > 0);
    }
}

function round(number, X) {
    X = (!X ? 2 : X);
    return Math.round(number * Math.pow(10, X)) / Math.pow(10, X);
}




function Init_DisabledControl() {
    for (var i = 0; i < ArrBetType.length; i++) {
        window.enableAllowUpdate[window.enableAllowUpdate.length] = GetElement("divstatus_" + ArrBetType[i]);
        window.enableAllowUpdate[window.enableAllowUpdate.length] = GetElement("divcomm_" + ArrBetType[i]);
        window.enableAllowUpdate[window.enableAllowUpdate.length] = GetElement("divbetsetting_" + ArrBetType[i]);
        window.enableAllowUpdate[window.enableAllowUpdate.length] = GetElement("divpositiontaking_" + ArrBetType[i]);
    }
}


function GetElement(parentElmName) {

    var arr = new Array();
    for (var i = 0; i < ArrType.length; i++) {
        var Elements = $(parentElmName).getElementsByTagName(ArrType[i]);
        for (var k = 0; k < Elements.length; k++) {
            var elm = Elements[k];
            if (!elm.disabled) {
                if (ArrExcAllowUpdatePT.contains(elm.id)) continue;
                elm.disabled = true;
                arr[arr.length] = elm;
            }
        }
    }
    return arr;
}

function GetLocationArray(sporttype) {
    var res = 0;
    for (var i = 0; i < ArrBetType.length; i++) {
        if (ArrBetType[i] == sporttype) return res;
        res += 4;
    }
}

function contains(a, obj) {
    var i = a.length;
    while (i--) {
        if (a[i] === obj) {
            return true;
        }
    }
    return false;
}

function chkCopyAll_OnClick(el, ArrExc) {
    Set_CopyAll(el.checked);
}

function Set_CopyAll(isChecked) {

    if (isChecked) {
        window.enableList = new Array();

        for (var i = 0; i < Arr_tblId.length; i++) {
            for (var j = 0; j < ArrType.length; j++) {
                var Elements = $(Arr_tblId[i]).getElementsByTagName(ArrType[j]);
                for (var k = 0; k < Elements.length; k++) {
                    var elm = Elements[k];
                    if (ArrExc.contains(elm.id)) continue;
                    if (i == 0 && elm.type == 'button') continue;

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
            if ($('ChkUpdate_151_41') && !$('ChkUpdate_151_41').checked && (contains(Arr_151_41, window.enableList[i].id) || window.enableList[i].id == "chkCopyFromWin_151_41")) continue;
            window.enableList[i].disabled = false;
        }
        if ($('ChkUpdate_151_41') && $('ChkUpdate_151_41').checked && !$('chkCopyFromWin_151_41').checked) {
            if ($('chkCopyAll').disabled)
                $('chkCopyFromWin_151_41').disabled = true;
            else
                $('chkCopyFromWin_151_41').disabled = false;


            if ($(Arr_151_41[1])) $(Arr_151_41[1]).disabled = $(Arr_151_41[2]).checked;
            if ($(Arr_151_41[3])) $(Arr_151_41[3]).disabled = !$(Arr_151_41[2]).checked;
            if ($(Arr_151_41[0])) $(Arr_151_41[0]).disabled = false;
            if ($(Arr_151_41[2])) $(Arr_151_41[2]).disabled = false;





        }
    }

}

function SetStatusCopyAll(isCheck) {
    var j = GetLocationArray("151_41") + 3;
    if (window.enableCopyAll.length <= 0) {
        window.enableCopyAll = window.enableAllowUpdate[j];
    }
    if (isCheck) {

        for (var i = 0; i < window.enableCopyAll.length; i++) {
            if (contains(Arr_151_41, window.enableCopyAll[i].id) || window.enableCopyAll[i].id == "chkCopyFromWin_151_41") continue;
            window.enableCopyAll[i].disabled = false;

        }
    }
    else {
        var arr = new Array();
        arr = GetElement("divpositiontaking_151_41");
        window.enableCopyAll = arr;

    }

}

function CheckStatusCopyAll(isChecked) {
    if (!$('listMaster_151_41') && !$('listSuper_151_41') && !$('listMember_151_41')) {
        if (($('chkAutoMaster_151_41') && $('chkAutoMaster_151_41').checked) || ($('chkAutoSuper_151_41') && $('chkAutoSuper_151_41').checked)) {
            if ($('chkCopyAll').checked) {
                $('chkCopyFromWin_151_41').disabled = true;
            }
            else {
                $('chkCopyFromWin_151_41').disabled = isChecked ? false : true;
            }
        }
    }
    else {
        if (!$('chkCopyAll').checked) {
            $('chkCopyFromWin_151_41').disabled = false;
        }
    }
}

function AllowUpdate(elm, sporttype) {



    if (elm.checked) {

        var j = 0;
        if (elm.id == "ChkUpdateStatus_" + sporttype) {
            j = GetLocationArray(sporttype);
            for (var i = 0; i < window.enableAllowUpdate[j].length; i++) {
                window.enableAllowUpdate[j][i].disabled = false;
            }
        } else if (elm.id == "ChkUpdateCommission_" + sporttype) {
            j = GetLocationArray(sporttype) + 1;
            for (var i = 0; i < window.enableAllowUpdate[j].length; i++) {
                window.enableAllowUpdate[j][i].disabled = false;
            }
        } else if (elm.id == "ChkUpdateBetSetting_" + sporttype) {
            j = GetLocationArray(sporttype) + 2;
            for (var i = 0; i < window.enableAllowUpdate[j].length; i++) {
                window.enableAllowUpdate[j][i].disabled = false;
            }
        } else if (elm.id == "ChkUpdate_" + sporttype) {
            if (sporttype == "151_41") {
                CheckStatusCopyAll(true);
            }
            j = GetLocationArray(sporttype) + 3;
            for (var i = 0; i < window.enableAllowUpdate[j].length; i++) {
                var arr = eval("Arr_" + sporttype);
                if ($('chkCopyAll').checked && (contains(arr, window.enableAllowUpdate[j][i].id) || window.enableAllowUpdate[j][i].id == "chkCopyFromWin_" + sporttype)) continue;
                if ($('chkCopyFromWin_' + sporttype).checked && contains(arr, window.enableAllowUpdate[j][i].id)) continue;
                window.enableAllowUpdate[j][i].disabled = false;
            }
        }


    } else {

        var j = 0;
        var arr = new Array();
        if (elm.id == "ChkUpdateStatus_" + sporttype) {
            j = GetLocationArray(sporttype);
            arr = GetElement("divstatus_" + sporttype);
        } else if (elm.id == "ChkUpdateCommission_" + sporttype) {
            j = GetLocationArray(sporttype) + 1;
            arr = GetElement("divcomm_" + sporttype);
        } else if (elm.id == "ChkUpdateBetSetting_" + sporttype) {
            j = GetLocationArray(sporttype) + 2;
            arr = GetElement("divbetsetting_" + sporttype);
        } else if (elm.id == "ChkUpdate_" + sporttype) {
            if (sporttype == "151_41") {
                CheckStatusCopyAll(false);
            }

            j = GetLocationArray(sporttype) + 3;
            arr = GetElement("divpositiontaking_" + sporttype);
            if ($('chkCopyAll').checked) {
                for (var i = 0; i < window.enableAllowUpdate[j].length; i++) {
                    if (contains(Arr_151_41, window.enableAllowUpdate[j][i].id)) {
                        arr[arr.length] = window.enableAllowUpdate[j][i];
                    }
                }
            }
            else {
                // arr = window.enableAllowUpdate[j];
            }
        }
        window.enableAllowUpdate[j] = arr;

    }
}


RegisterStartUp("initRaceBook()");
