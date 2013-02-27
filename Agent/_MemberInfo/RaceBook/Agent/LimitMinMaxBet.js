

function initLimitMinMaxBet() {
    window.ValidateValue2 = window.ValidateValue;

    window.ValidateValue = function(bettype) {

        var isChecked = window.ValidateValue2.call(window, bettype);
        if (isChecked) {

            var minbet = "MinBet_" + bettype;
            var maxbet = "MaxBet_" + bettype;
            var maxpermatch = "MaxPerMatch_" + bettype;

            var minbet_Min = "MinBet_" + bettype + "_Min";
            var maxbet_Max = "MaxBet_" + bettype + "_Max";
            var maxpermatch_Max = "MaxPerMatch_" + bettype + "_Max";

            if (isNumeric(minbet) == 0 && isNumeric(maxbet) == 0 && isNumeric(maxpermatch) == 0) {
                return true;
            }
            else {                
                if (isNumeric(minbet) < isNumericSpan(minbet_Min)) {
                    $(minbet).focus();
                    AutoShowHide("div_" + bettype);
                    ShowBalloonMsg(minbet, _page.lblalertMinbetCompare + " " + isNumericSpan(minbet_Min));
                    return false;
                }
                return true;
            }
        }
        return isChecked;
    }
}


RegisterStartUp("initLimitMinMaxBet()");
