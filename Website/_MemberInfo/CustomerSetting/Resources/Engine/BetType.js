if (typeof (Fanex) == 'undefined') { // Require Fanex namespace
	Fanex = {};
}
if (typeof (Fanex.CustSetting) == 'undefined') { // Require Fanex.CustSetting namespace
	alert('Require Fanex.CustSetting namespace');
}
// Function name format Render_[RoleId_]TargetRoleId_Form
// [RoleId_] is optional
// Call method by Method.call(this, arg1, arg2...)
Fanex.CustSetting.BetType.prototype.Render_2_2_1 = function (owner) { // a1
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
        maximum: this.pt.Upline.Max - this.pt.Min,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: presetSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);


    var maxSelected = this.pt.Max;
    var maxContainer = document.createElement("DIV");
    maxContainer.className = "bet-type-select";
    box.appendChild(maxContainer);
    var maxSpinBox = Fanex.CustSetting.PT_RenderSpinner(maxContainer,
    {
        minimum: this.pt.Min,
        maximum: this.pt.Upline.Max,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: maxSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);


    var minSelected = this.pt.Min;
    var minContainer = document.createElement("DIV");
    minContainer.className = "bet-type-select";
    box.appendChild(minContainer);
    var minSpinBox = Fanex.CustSetting.PT_RenderSpinner(minContainer, {
        minimum: 0,
        maximum: this.pt.Max,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: minSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);


    //// Preset K Change Affect to Max K
    presetSpinBox.input.onchange = function (event) {
        var presetValue = presetSpinBox.getValue();
        presetSpinBox.setValue(presetValue);
        var maxValue = maxSpinBox.getValue();
        var minValue = minSpinBox.getValue();
        if (presetValue > currentPt.Upline.Max - currentPt.Min) {
            presetSpinBox.setValue(currentPt.Upline.Max - currentPt.Min);
        }
        else if (presetValue + maxValue > currentPt.Upline.Max) {
            var newMaxValue = currentPt.Upline.Max - presetValue;
            maxSpinBox.setValue(newMaxValue);
            if (newMaxValue < minValue) {
                minSpinBox.setValue(newMaxValue);
            }
        }
    }
    //// Max K Change affect To Min K and Preset K
    maxSpinBox.input.onchange = function (event) {
        var maxValue = maxSpinBox.getValue();
        maxSpinBox.setValue(maxValue);
        var minValue = minSpinBox.getValue();
        var presetValue = presetSpinBox.getValue();
        if (maxValue > currentPt.Upline.Max) {
            maxSpinBox.setValue(currentPt.Upline.Max);
        }
        else if (maxValue < minValue) {
            maxSpinBox.setValue(minValue);
        }
        else if (presetValue + maxValue > currentPt.Upline.Max) {
            presetSpinBox.setValue(currentPt.Upline.Max - maxValue);
        }
        minSpinBox.updateMaxValue(maxSpinBox.getValue());
    }
    //// Min K Change affect To Max K
    minSpinBox.input.onchange = function (event) {
        var maxValue = maxSpinBox.getValue();
        var minValue = minSpinBox.getValue();
        minSpinBox.setValue(minValue);
        if (maxValue < minValue) {
            minSpinBox.setValue(maxValue);
        }
        maxSpinBox.updateMinValue(minSpinBox.getValue());
    }

    box.id = "e$" + this.suffixId;
    box.className = "pt-box";

    // Get updated data
    this.Save = function () {
        this.pt.Min = minSpinBox.getValue();
        this.pt.Max = maxSpinBox.getValue();
        this.pt.Preset = presetSpinBox.getValue();
        if (this.oldPt.Min != this.pt.Min
        || this.oldPt.Max != this.pt.Max
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

Fanex.CustSetting.BetType.prototype.Render_3_2_1 = function (owner) {
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
        minimum: this.pt.Upline.Min,
        maximum: this.pt.Upline.Max,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: maxSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var minSelected = this.pt.Min;
    var minContainer = document.createElement("DIV");
    minContainer.className = "bet-type-select";
    box.appendChild(minContainer);
    var minSpinBox = Fanex.CustSetting.PT_RenderSpinner(minContainer, {
        minimum: this.pt.Upline.Min,
        maximum: this.pt.Upline.Max,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: minSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);

    //// Max K Change affect To Min K and Preset K
    maxSpinBox.input.onchange = function (event) {
        var maxValue = maxSpinBox.getValue();
        maxSpinBox.setValue(maxValue);
        var minValue = minSpinBox.getValue();
        if (maxValue + minValue > currentPt.Upline.Max && currentPt.Upline.Max - maxValue >= 0) {
            minSpinBox.setValue(currentPt.Upline.Max - maxValue);
        }
        else if (currentPt.Upline.Max - maxValue < 0) {
            maxSpinBox.setValue(currentPt.Upline.Max);
        }
        else if (maxValue < minValue) {
            maxSpinBox.setValue(minValue);
        }
        minSpinBox.updateMaxValue(maxSpinBox.getValue());
    }
    //// Min K Change affect To Max K
    minSpinBox.input.onchange = function (event) {
        var maxValue = maxSpinBox.getValue();
        var minValue = minSpinBox.getValue();
        minSpinBox.setValue(minValue);
        if (maxValue + minValue > currentPt.Upline.Max && currentPt.Upline.Max - minValue >= 0) {
            maxSpinBox.setValue(currentPt.Upline.Max - minValue);
        }
        else if (currentPt.Upline.Max - minValue < 0) {
            minSpinBox.setValue(currentPt.Upline.Max);
        }
        else if (maxValue < minValue) {
            minSpinBox.setValue(maxValue);
        }
        maxSpinBox.updateMinValue(minSpinBox.getValue());
    }

    box.id = "e$" + this.suffixId;
    box.className = "pt-box";

    // Get updated data
    this.Save = function () {
        this.pt.Min = minSpinBox.getValue();
        this.pt.Max = maxSpinBox.getValue();
        if (this.oldPt.Min != this.pt.Min
        || this.oldPt.Max != this.pt.Max) {
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

Fanex.CustSetting.BetType.prototype.Render_2_1_1 = function (owner) {
    var currentPt = this.pt;
    box = document.createElement("DIV");

    // Bet type header
    var header = document.createElement("DIV");
    header.innerHTML = this.name;
    header.title = this.title;
    header.className = "bet-type-header";
    box.appendChild(header); // Header bet type name
    header.style.minWidth = this.minWidth + 'px';

    var ptSelected = this.pt.Value;
    var ptContainer = document.createElement("DIV");
    ptContainer.className = "bet-type-select";
    box.appendChild(ptContainer);
    var ptSpinBox = Fanex.CustSetting.PT_RenderSpinner(ptContainer,{
        minimum: this.pt.Upline.Min,
        maximum: this.pt.Upline.Max,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: ptSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);


    ptSpinBox.input.onchange = function (event) {
        var ptValue = ptSpinBox.getValue();
        ptSpinBox.setValue(ptValue);
        if (ptValue < currentPt.Upline.Min) {
            ptSpinBox.setValue(currentPt.Upline.Min);
        }
        else if (ptValue > currentPt.Upline.Max) {
            ptSpinBox.setValue(currentPt.Upline.Max);
        }
    }

    box.id = "e$" + this.suffixId;
    box.className = "pt-box";

    // Get updated data
    this.Save = function () {
        this.pt.Value = ptSpinBox.getValue();
        if (this.oldPt.Value != this.pt.Value) {
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