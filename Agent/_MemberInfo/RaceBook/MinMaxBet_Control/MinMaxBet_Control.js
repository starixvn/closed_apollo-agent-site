var Bettypes = ["151_41", "151_31", "151_51", "152_41", "152_51", "153_51"];

var Prefixes = ["MinBet", "MaxBet", "MaxPerMatch"];
function CheckValidateBetSetting(copyallID) {
    var isOk = true;
    if ($(copyallID) && $(copyallID).checked) // is checked
    {
        return ValidateValue("151_41");
    }
    else // is unchecked
    {
        for (var i = 0; i < Bettypes.length; i++) {
            var id = "_page.IsShowRacing_" + Bettypes[i];            
            if (eval(id) == false) continue;           
            isOk = ValidateValue(Bettypes[i]);
            if (!isOk) return isOk;
        }
    }
    return isOk;
}

function ValidateValue(bettype) {
    if ($('ChkUpdateBetSetting_' + bettype) && !$('ChkUpdateBetSetting_' + bettype).checked) return true;

    var minbet = "MinBet_" + bettype;
    var maxbet = "MaxBet_" + bettype;
    var maxpermatch = "MaxPerMatch_" + bettype;

    var minbet_Min = "MinBet_" + bettype + "_Min";
    var maxbet_Max = "MaxBet_" + bettype + "_Max";
    var maxpermatch_Max = "MaxPerMatch_" + bettype + "_Max";

    if (!$(minbet) || !$(minbet) || !$(maxpermatch)) return true;

    if (isEmpty(minbet) || isString(minbet)) {
        $(minbet).focus();
        AutoShowHide("div_" + bettype);
        ShowBalloonMsg(minbet, _page.alertIncorrectMinBet);
        return false;
    }

    if (isEmpty(maxbet) || isString(maxbet)) {
        $(maxbet).focus();
        AutoShowHide("div_" + bettype);
        ShowBalloonMsg(maxbet, _page.alertIncorrectMaxBet);
        return false;
    }

    if (isEmpty(maxpermatch) || isString(maxpermatch)) {
        $(maxpermatch).focus();
        AutoShowHide("div_" + bettype);
        ShowBalloonMsg(maxpermatch, _page.alertIncorrectMaxPerMatch);
        return false;
    }

    // ------

    if (isNumeric(minbet) < 0) {
        $(minbet).focus();
        AutoShowHide("div_" + bettype);
        ShowBalloonMsg(minbet, _page.lblalertMinBetNegative);
        return false;
    }

    if (isNumeric(maxbet) < 0) {
        $(maxbet).focus();
        AutoShowHide("div_" + bettype);
        ShowBalloonMsg(maxbet, _page.lblalertMaxBetNegative);
        return false;
    }

    if (isNumeric(maxpermatch) < 0) {
        $(maxpermatch).focus();
        AutoShowHide("div_" + bettype);
        ShowBalloonMsg(maxpermatch, _page.lblalertMaxPerMatchNegative);
        return false;
    }

    //-----

    if (isNumeric(minbet) != 0 && isNumericSpan(minbet_Min) != 0 && isNumeric(minbet) < isNumericSpan(minbet_Min)) {
        $(minbet).focus();
        AutoShowHide("div_" + bettype);
        ShowBalloonMsg(minbet, _page.lblalertMinbetCompare + " " + isNumericSpan(minbet_Min));
        return false;
    }

    if (isNumeric(maxbet) != 0 && isNumericSpan(maxbet_Max) != 0 && isNumeric(maxbet) > isNumericSpan(maxbet_Max)) {
        $(maxbet).focus();
        AutoShowHide("div_" + bettype);
        ShowBalloonMsg(maxbet, _page.lblalerMaxbetCompare + " " + isNumericSpan(maxbet_Max));
        return false;
    }

    if (isNumeric(maxpermatch) != 0 && isNumericSpan(maxpermatch_Max) != 0 && isNumeric(maxpermatch) > isNumericSpan(maxpermatch_Max)) {
        $(maxpermatch).focus();
        AutoShowHide("div_" + bettype);
        ShowBalloonMsg(maxpermatch, _page.lblalertMaxPerMatchCompare + " " + isNumericSpan(maxpermatch_Max));
        return false;
    }

    //------

    if (isNumeric(minbet) > isNumeric(maxbet)) {
        $(minbet).focus();
        AutoShowHide("div_" + bettype);
        ShowBalloonMsg(minbet, _page.alertMaxBetLessMinBet);
        return false;
    }

    if (isNumeric(maxbet) > isNumeric(maxpermatch)) {
        $(maxbet).focus();
        AutoShowHide("div_" + bettype);
        ShowBalloonMsg(maxbet, _page.alertMaxPerMatchLessMaxBet);
        return false;
    }
    return true;
}


function isEmpty(id) {
    if (!$(id)) {
        alert(id + ' does not exists.');
        return;
    }
    return $(id).value == "";
}

//check string input field by id , return true or false
function isString(id) {
    return isNaN(ReplaceAll($(id).value, ',', ''));
}

//Find all occurrence of regexp in str and replace it with newstr
function ReplaceAll(str, regexp, newstr) {
    return str.replace(eval('/' + regexp + '/g'), newstr);
}

//check Numeric input field by id , return value
function isNumeric(id) {
    return parseFloat(ReplaceAll($(id).value, ',', ''));
}

function isNumericSpan(id) {
    if (!$(id)) return 0;
    return parseFloat(ReplaceAll($(id).innerHTML, ',', ''));
}
