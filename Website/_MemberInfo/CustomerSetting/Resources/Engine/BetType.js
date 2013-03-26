if (typeof (Fanex) == 'undefined') { // Require Fanex namespace
	Fanex = {};
}
if (typeof (Fanex.CustSetting) == 'undefined') { // Require Fanex.CustSetting namespace
	alert('Require Fanex.CustSetting namespace');
}
// Function name format Render_[RoleId_]TargetRoleId_Form
// [RoleId_] is optional
// Call method by Method.call(this, arg1, arg2...)
Fanex.CustSetting.BetType.prototype.Render_5_2_1 =
Fanex.CustSetting.BetType.prototype.Render_4_2_1 =
Fanex.CustSetting.BetType.prototype.Render_3_2_1 = function (owner) { // a1
    box = document.createElement("DIV");
    var currentPt = this.pt;
    // Bet type header
    var header = document.createElement("DIV");
    header.innerHTML = this.name;
    header.title = this.title;
    header.className = "bet-type-header";
    box.appendChild(header); // Header bet type name
    //header.style.minWidth = this.minWidth + 'px';
    header.style.minWidth = this.minWidth + 'px';
    // preset PT
    var presetSelected = this.pt.Preset;
    var presetContainer = document.createElement("DIV");
    presetContainer.className = "bet-type-select";
    box.appendChild(presetContainer);
    var presetSpinBox = Fanex.CustSetting.PT_RenderSpinner(presetContainer,
    {
        minimum: 0,
        maximum: this.pt.Upline.Max,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: presetSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var presetMaxSpan = document.createElement("SPAN");
    presetMaxSpan.innerHTML = ' / ' + Fanex.RoundNumber(this.pt.Upline.Max, 2) + '%';
    presetMaxSpan.className = "upline-agent-share";
    presetContainer.appendChild(presetMaxSpan);


    var maxSelected = this.pt.Max;
    var maxContainer = document.createElement("DIV");
    maxContainer.className = "bet-type-select";
    box.appendChild(maxContainer);
    currentPt.Upline.MaxSpan = Fanex.RoundNumber(this.pt.Upline.Max - presetSelected, 2);
    var maxSpinBox = Fanex.CustSetting.PT_RenderSpinner(maxContainer,
    {
        minimum: 0,
        maximum: currentPt.Upline.MaxSpan,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: maxSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = ' / ' + currentPt.Upline.MaxSpan + '%';
    uplineMaxSpan.className = "upline-agent-share";
    maxContainer.appendChild(uplineMaxSpan);

    //// Preset K Change Affect to Max K
    presetSpinBox.input.onchange = function (event) {
        var presetValue = presetSpinBox.getValue();
        presetSpinBox.setValue(presetValue);
        if (presetValue > currentPt.Upline.Max) {
            presetSpinBox.setValue(currentPt.Upline.Max);
            currentPt.Upline.MaxSpan = 0;
            maxSpinBox.setValue(0);
        }
        else if (presetValue + currentPt.Upline.MaxSpan > currentPt.Upline.Max) {
            var newnaxValue = currentPt.Upline.Max - presetValue;
            currentPt.Upline.MaxSpan = newnaxValue;
            if (maxSpinBox.getValue() > newnaxValue) {
                maxSpinBox.setValue(newnaxValue);
            }
        }
        else {
            currentPt.Upline.MaxSpan = currentPt.Upline.Max - presetValue;
        }

        currentPt.Upline.MaxSpan = Fanex.RoundNumber(currentPt.Upline.MaxSpan, 2);
        uplineMaxSpan.innerHTML = ' / ' + currentPt.Upline.MaxSpan + '%';
        maxSpinBox.updateMaxValue(currentPt.Upline.MaxSpan);
    }
    //// Max K Change affect To Min K and Preset K
    maxSpinBox.input.onchange = function (event) {
        var maxValue = maxSpinBox.getValue();
        maxSpinBox.setValue(maxValue);
        var presetValue = presetSpinBox.getValue();
        if (maxValue > currentPt.Upline.MaxSpan) {
            maxSpinBox.setValue(currentPt.Upline.MaxSpan);
        }
    }

    box.id = "e$" + this.suffixId;
    box.className = "pt-box";

    // Get updated data
    this.Save = function () {
        this.pt.Max = maxSpinBox.getValue();
        this.pt.Preset = presetSpinBox.getValue();
        if (this.oldPt.Max != this.pt.Max
        || this.oldPt.Preset != this.pt.Preset) {
            return {
                Id: this.id,
                Pt: this.pt
            }
        }
        else {
            return null;
        }
    }
    return box;
};
Fanex.CustSetting.BetType.prototype.Render_5_4_1 =
Fanex.CustSetting.BetType.prototype.Render_5_3_1 =
Fanex.CustSetting.BetType.prototype.Render_5_1_1 =
Fanex.CustSetting.BetType.prototype.Render_4_3_1 =
Fanex.CustSetting.BetType.prototype.Render_4_1_1 =
Fanex.CustSetting.BetType.prototype.Render_2_1_1 =
Fanex.CustSetting.BetType.prototype.Render_1_1_1 = function (owner) {
    var currentPt = this.pt;
    box = document.createElement("DIV");

    // Bet type header
    var header = document.createElement("DIV");
    header.innerHTML = this.name;
    header.title = this.title;
    header.className = "bet-type-header";
    box.appendChild(header); // Header bet type name
    header.style.minWidth = this.minWidth + 'px';

    var maxSelected = this.pt.Max;
    var maxContainer = document.createElement("DIV");
    maxContainer.className = "bet-type-select";
    box.appendChild(maxContainer);
    var maxSpinBox = Fanex.CustSetting.PT_RenderSpinner(maxContainer, {
        minimum: 0,
        maximum: this.pt.Upline.Max,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: maxSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = ' / ' + Fanex.RoundNumber(this.pt.Upline.Max, 2) + '%';
    uplineMaxSpan.className = "upline-agent-share";
    maxContainer.appendChild(uplineMaxSpan);

    maxSpinBox.input.onchange = function (event) {
        var naxValue = maxSpinBox.getValue();
        maxSpinBox.setValue(naxValue);
        if (naxValue > currentPt.Upline.Max) {
            maxSpinBox.setValue(currentPt.Upline.Max);
        }
    }

    box.id = "e$" + this.suffixId;
    box.className = "pt-box";

    // Get updated data
    this.Save = function () {
        this.pt.Max = maxSpinBox.getValue();
        if (this.oldPt.Max != this.pt.Max) {
            return {
                Id: this.id,
                Pt: this.pt
            }
        }
        else {
            return null;
        }
    }
    return box;
};