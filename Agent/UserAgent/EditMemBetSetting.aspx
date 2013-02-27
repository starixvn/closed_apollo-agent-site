<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditMemBetSetting.aspx.cs" Inherits="EditMemBetSetting" %>

<%@ OutputCache Location="None" %>

<script type="text/javascript">

function IsNumeric_Value(id) {
    return parseFloat(ReplaceAll($(id).value, ',', ''));
}

function CheckLimitMaxPerBall() {

    var maxperballId = "txtMaxPerBall";
    var maxperballMaxId = "txtMaxPerBall_Max";

    if (IsNumeric_Value(maxperballId) < 0) {
        var alertMsg = "";
        try {
            var alertMsg = alertMaxperBallNegative;
        }
        catch (ex) {
            alertMsg = "Max per Ball must be greater than zero";
        }
        $(maxperballId).focus();
        showMyBalloonMsg(maxperballId, alertMsg);
        return false;
    }

    if (isEmpty(maxperballId) || isString(maxperballId)) {
        $(maxperballId).focus();
        showMyBalloonMsg(maxperballId, alertMaxperball);
        return false;
    }

    if (IsNumeric_Value(maxperballId) != 0 && isNumericSpan(maxperballMaxId) != 0) {

        if (IsNumeric_Value(maxperballId) > isNumericSpan(maxperballMaxId)) {

            var str = "";
            str = alertMaxperballC.replace("{1}", formatNumber2(isNumericSpan(maxperballMaxId)));
            $(maxperballId).focus();
            showMyBalloonMsg(maxperballId, str);
            return false;
        }
    }

    return true;
}

function checkCopyAll(chkCopyAll) {
    for (i = 0; i < document.forms[0].length; i++) {
        element = document.forms[0][i];
        if (element.type == "text") {
            if (element.id != 'txtMinbet1' && element.id != 'txtMaxbet1' && element.id != 'txtMaxPerMatch1' && element.id != 'txtMaxPerBall' && element.id != 'txtMinbet161' && element.id != 'txtMaxbet161' && element.id != 'txtMaxPerMatch161') element.disabled = chkCopyAll.checked;
        }
    }
}

function checkAndSubmitCustInfo() {
    //check validate first    
    for (i = 0; i < document.forms[0].length; i++) {
        element = document.forms[0][i];
        if (element.type == "text" && !element.disabled) //if remove element.disabled condition, it will show detail error by tooltip!
        {
            txtId = element.id;
            txtValue = formatNo(element.value);

            //check numeric
            if (!isNumeric(txtValue)) {
                showMyBalloonMsg(txtId, errorinputnumber);
                focusIt(txtId);
                return false;
            }


            //not check if zero
            if (isAgent == '0') {
                if (!element.disabled && txtValue == 0) {
                    continue;
                }
            }


            //check minbet            
            txtMinMax = txtId.substring(0, 9);
            txtNoMinMax = txtId.substring(9, txtId.length);

            if (txtMinMax == 'txtMinbet') {
                if (element.disabled) txtValue = formatNo($('txtMinbet1').value); //copy value MinBet1 if disabled
                minBetMax = $('hidminbetmax' + txtNoMinMax).value;
                minBetMax = formatNo(minBetMax);

                if (element.disabled) maxBet = formatNo($('txtMaxbet1').value); //copy value MaxBet1 if disabled
                else {
                    maxBet = $('txtMaxbet' + txtNoMinMax).value;
                    maxBet = formatNo(maxBet);
                }

                if (isAgent == '1') {
                    if (txtValue == 0 && $('txtMaxbet' + txtNoMinMax).value == 0 && $('txtMaxPerMatch' + txtNoMinMax).value == 0) continue;
                    else {
                        if (minBetMax > 0 && txtValue - minBetMax < 0) //notice: do not check if minBetMax = 0
                        {
                            showMyBalloonMsg(txtId, lblalertMinbetCompare + ' ' + minBetMax);
                            return focusIt(txtId);
                        }
                    }
                }
                else {
                    if (minBetMax > 0 && txtValue - minBetMax < 0) //notice: do not check if minBetMax = 0
                    {
                        showMyBalloonMsg(txtId, lblalertMinbetCompare + ' ' + minBetMax);
                        return focusIt(txtId);
                    }
                }

                if (txtValue - maxBet > 0) {
                    showMyBalloonMsg(txtId, alertMaxLessMin);
                    return focusIt(txtId);
                }
                if (txtValue < 0) {
                    showMyBalloonMsg(txtId, lblalertMinBetNegativeHR);
                    return focusIt(txtId);
                }
            }
            //check maxbet
            else if (txtMinMax == 'txtMaxbet') {
                if (element.disabled) txtValue = formatNo($('txtMaxbet1').value); //copy value Maxbet1 if disabled
                maxBetMin = $('hidmaxbetmin' + txtNoMinMax).value;
                maxBetMin = formatNo(maxBetMin);

                if (element.disabled) maxPerMatch = formatNo($('txtMaxPerMatch1').value); //copy value MaxPerMatch1 if disabled
                else {
                    maxPerMatch = $('txtMaxPerMatch' + txtNoMinMax).value;
                    maxPerMatch = formatNo(maxPerMatch);
                }

                if (maxBetMin > 0 && txtValue - maxBetMin > 0) {
                    showMyBalloonMsg(txtId, lblalerMaxbetCompare + ' ' + maxBetMin);
                    return focusIt(txtId);
                }
                if (txtValue - maxPerMatch > 0) {
                    showMyBalloonMsg(txtId, alertMaxLessMaxPer);
                    return focusIt(txtId);
                }
                if (txtValue < 0) {
                    showMyBalloonMsg(txtId, lblalertMaxBetNegativeHR);
                    return focusIt(txtId);
                }
            }
            //check max per match
            else {
                txtMaxPerMatch = txtId.substring(0, 14);
                txtNoMinMax = txtId.substring(14, txtId.length);

                if (txtMaxPerMatch == 'txtMaxPerMatch') {
                    if (element.disabled) txtValue = formatNo($('txtMaxPerMatch1').value); //copy value MaxPerMatch1 if disabled
                    maxPerMatchMax = $('hidmaxpermatchmax' + txtNoMinMax).value;
                    maxPerMatchMax = formatNo(maxPerMatchMax);

                    maxPerMatchMin = $('hidmaxpermatchmin' + txtNoMinMax).value;
                    maxPerMatchMin = formatNo(maxPerMatchMin);

                    if (maxPerMatchMax > 0 && txtValue - maxPerMatchMax > 0) //do not check if maxPerMatchMax = 0
                    {
                        showMyBalloonMsg(txtId, lblalertMaxPerMatchCompareMax + ' ' + maxPerMatchMax);
                        return focusIt(txtId);
                    }
                    if (maxPerMatchMin > 0 && txtValue - maxPerMatchMin < 0) //do not check if maxPerMatchMin = 0
                    {
                        showMyBalloonMsg(txtId, lblalertMaxPerMatchCompareMin + ' ' + maxPerMatchMin);
                        return focusIt(txtId);
                    }
                    if (txtValue < 0) {
                        showMyBalloonMsg(txtId, lblalertMaxPerMatchNegativeHR);
                        return focusIt(txtId);
                    }
                }
            }
        }
    }

    if (!$("chkCopyAll").checked) {
        if ($('txtMaxPerBall')) {
            if (!CheckLimitMaxPerBall()) return false;
        }
    }


    showHideLoading(1);
    UnTip(); //untip all    
    document.fUpdateInfo.submit();
}

function showHideLoading(isShow) {
    $('btnSubmit').disabled = (isShow == 1);
}

function showInfoMsg(msg, msgType) {
    showHideLoading(0);
    showErrMsg(msg, msgType);
}

function focusIt(txtId) {
    return false;
}

function formatNo(str) {
    return replaceAll(str, ",", "");
}

function replaceAll(text, strA, strB) {
    while (text.indexOf(strA) != -1) {
        text = text.replace(strA, strB);
    }
    return text;
}

function isNumeric(x) {
    if (!x) return false;
    var RegExp = /^(-)?(\d*)(\.?)(\d*)$/; // Note: this WILL allow a number that ends in a decimal: -452.
    return x.match(RegExp);
}

function checkKeyEnter(e) {
    var keycode = window.event ? e.keyCode : e.which;
    if (keycode == 13) //you hit return!
    {
        $('btnSubmit').click();
    }
}

function showMyBalloonMsg(id, strMsg) {
    var e1 = $(id);
    e1.style.postion = "relative";

    var pathTooltip = $('pathTooltip') ? $('pathTooltip').value : '';

    var x = 0;
    var y = 0;
    var obj = e1;
    if (obj.offsetParent) {
        do {
            x += obj.offsetLeft;
            y += obj.offsetTop;
        }
        while (obj = obj.offsetParent);
    }

    x += e1.offsetWidth - 109;
    y -= e1.offsetHeight + 48;
    e1.ontip = function () {
        Tip("<div class='msgerr'>" + strMsg + "</div>", BALLOON, true, ABOVE, true, BALLOONIMGPATH, pathTooltip + "../Js/wz_tooltip/tip_balloon/", WIDTH, -350, FIX, [x, y]);
    }

    e1.ontip();
    if (!e1.disabled) e1.focus(); //focus if only not disabled
    window.scrollBy(0, -60);

    e1.onblur = UnTip;
}

</script>

