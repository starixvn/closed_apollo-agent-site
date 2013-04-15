if (typeof (Fanex) == 'undefined') { // Require Fanex namespace
    Fanex = {};
}
if (typeof (Fanex.CustSetting) == 'undefined') { // Require Fanex.CustSetting namespace
    alert('Require Fanex.CustSetting namespace');
}
// Function name format Render_[RoleId_]TargetRoleId_Form
// [RoleId_] is optional
// Call method by Method.call(this, arg1, arg2...)

Fanex.CustSetting.BetType.prototype.Render_3_2_1 =
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
        maximum: this.pt.Upline.Max - this.pt.Max,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: presetSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var presetMaxSpan = document.createElement("SPAN");
    presetMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, this.pt.Upline.Max - this.pt.Max);
    presetMaxSpan.className = "upline-agent-share";
    presetContainer.appendChild(presetMaxSpan);


    var maxSelected = this.pt.Max;
    var maxContainer = document.createElement("DIV");
    maxContainer.className = "bet-type-select";
    box.appendChild(maxContainer);
    currentPt.MaxPresetSpan = this.pt.Upline.Max;
    var maxSpinBox = Fanex.CustSetting.PT_RenderSpinner(maxContainer,
    {
        minimum: this.pt.Upline.Min,
        maximum: this.pt.Upline.Max,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: maxSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);


    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(this.pt.Upline.Min, currentPt.MaxPresetSpan);
    uplineMaxSpan.className = "upline-agent-share";
    maxContainer.appendChild(uplineMaxSpan);

    var minSelected = this.pt.Min;
    var minContainer = document.createElement("DIV");
    minContainer.className = "bet-type-select";
    box.appendChild(minContainer);
    currentPt.MaxMinSpan = this.pt.Max;
    var minSpinBox = Fanex.CustSetting.PT_RenderSpinner(minContainer, {
        minimum: 0,
        maximum: this.pt.Max,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: minSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var uplineMinSpan = document.createElement("SPAN");
    uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPt.MaxMinSpan);
    uplineMinSpan.className = "upline-agent-share";
    minContainer.appendChild(uplineMinSpan);

    //// Preset K Change Affect to Max K
    presetSpinBox.input.onchange = function (event) {
        var presetValue = presetSpinBox.getValue();
        presetSpinBox.setValue(presetValue);
        var maxValue = maxSpinBox.getValue();
        if (presetValue + maxValue > currentPt.Upline.Max) {
            var newMaxValue = currentPt.Upline.Max - maxValue;
            presetSpinBox.setValue(newMaxValue);
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
            presetSpinBox.setValue(0);
        }
        else if (maxValue < minValue) {
            maxSpinBox.setValue(minValue);
        }
        else if (presetValue + maxValue > currentPt.Upline.Max) {
            var presetMaxValue = currentPt.Upline.Max - maxValue;
            presetSpinBox.setValue(presetMaxValue);

        }
        currentPt.MaxMinSpan = maxSpinBox.getValue();
        minSpinBox.updateMaxValue(currentPt.MaxMinSpan);
        presetSpinBox.updateMaxValue(currentPt.Upline.Max - maxValue);
        presetMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPt.Upline.Max - maxValue);
        uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPt.MaxMinSpan);
    }
    //// Min K Change affect To Max K
    minSpinBox.input.onchange = function (event) {
        var maxValue = maxSpinBox.getValue();
        var minValue = minSpinBox.getValue();
        minSpinBox.setValue(minValue);
        if (minValue > maxValue) {
            minSpinBox.setValue(maxValue);
        }
        minValue = minSpinBox.getValue();
        minValue = currentPt.Upline.Min > minValue ? minValue : currentPt.Upline.Min;
        maxSpinBox.updateMinValue(minValue);
        uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(minValue, currentPt.MaxPresetSpan);
    }

    box.id = "e$" + this.suffixId;
    box.className = "pt-box";
    box.minSpinBox = minSpinBox;
    box.maxSpinBox = maxSpinBox;
    box.presetSpinBox = presetSpinBox;

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
    var ptSpinBox = Fanex.CustSetting.PT_RenderSpinner(ptContainer, {
        minimum: this.pt.Upline.Min,
        maximum: this.pt.Upline.Max,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: ptSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var uplineShareSpan = document.createElement("SPAN");
    uplineShareSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(this.pt.Upline.Min, this.pt.Upline.Max);
    uplineShareSpan.className = "upline-agent-share";
    ptContainer.appendChild(uplineShareSpan);

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
    box.ptSpinBox = ptSpinBox;
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
    };

    return box;
};

Fanex.CustSetting.BetType.prototype.Render_4_3_1 = function (owner) {
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

    var maxSelected = this.pt.Max;
    var maxContainer = document.createElement("DIV");
    maxContainer.className = "bet-type-select";
    box.appendChild(maxContainer);
    currentPt.MaxPresetSpan = this.pt.Upline.Max;
    var maxSpinBox = Fanex.CustSetting.PT_RenderSpinner(maxContainer,
    {
        minimum: this.pt.Upline.Min,
        maximum: this.pt.Upline.Max,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: maxSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);


    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(this.pt.Upline.Min, currentPt.MaxPresetSpan);
    uplineMaxSpan.className = "upline-agent-share";
    maxContainer.appendChild(uplineMaxSpan);

    var minSelected = this.pt.Min;
    var minContainer = document.createElement("DIV");
    minContainer.className = "bet-type-select";
    box.appendChild(minContainer);
    currentPt.MaxMinSpan = this.pt.Max;
    var minSpinBox = Fanex.CustSetting.PT_RenderSpinner(minContainer, {
        minimum: 0,
        maximum: currentPt.MaxMinSpan,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: minSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var uplineMinSpan = document.createElement("SPAN");
    uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPt.MaxMinSpan);
    uplineMinSpan.className = "upline-agent-share";
    minContainer.appendChild(uplineMinSpan);

    //// Max K Change affect To Min K and Preset K
    maxSpinBox.input.onchange = function (event) {
        var maxValue = maxSpinBox.getValue();
        maxSpinBox.setValue(maxValue);
        var minValue = minSpinBox.getValue();
        if (maxValue > currentPt.Upline.Max) {
            maxSpinBox.setValue(currentPt.Upline.Max);
        }
        else if (maxValue < minValue) {
            maxSpinBox.setValue(minValue);
        }
        currentPt.MaxMinSpan = maxSpinBox.getValue();
        minSpinBox.updateMaxValue(currentPt.MaxMinSpan);
        uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPt.MaxMinSpan);
    }
    //// Min K Change affect To Max K
    minSpinBox.input.onchange = function (event) {
        var maxValue = maxSpinBox.getValue();
        var minValue = minSpinBox.getValue();
        minSpinBox.setValue(minValue);
        if (minValue > maxValue) {
            minSpinBox.setValue(maxValue);
        }
        minValue = minSpinBox.getValue();
        minValue = currentPt.Upline.Min > minValue ? minValue : currentPt.Upline.Min;
        maxSpinBox.updateMinValue(minValue);
        uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(minValue, currentPt.MaxPresetSpan);
    }

    box.id = "e$" + this.suffixId;
    box.className = "pt-box";
    box.minSpinBox = minSpinBox;
    box.maxSpinBox = maxSpinBox;

    // Get updated data
    this.Save = function () {
        this.pt.Min = minSpinBox.getValue();
        this.pt.Max = maxSpinBox.getValue();
        if (this.oldPt.Min != this.pt.Min
        || this.oldPt.Max != this.pt.Max) {
            this.pt.Preset = this.pt.Upline.Max - this.pt.Max;
            return {
                Id: this.id,
                Pt: this.pt
            }
        }
        else {
            return null;
        }
    };

    return box;
};

Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_3_2_1 =
Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_2_2_1 = function (owner) {
    //box.appendChild(tableShareSetting);
    // Table
    var currentPtValue = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    var trSetting = document.createElement("TR");
    tableShareSetting.appendChild(trSetting)
    // Share Settings
    var presetLabel = document.createElement("TD");
    presetLabel.innerHTML = Language.PRESET_POSITION;
    presetLabel.title = Language.PRESET_POSITION;
    presetLabel.className = "share-setting-label";
    trSetting.appendChild(presetLabel);

    var presetSelected = currentPtValue.Preset;
    var presetContainer = document.createElement("TD");
    presetContainer.className = "share-setting-value";
    trSetting.appendChild(presetContainer);
    var presetSpinBox = Fanex.CustSetting.PT_RenderSpinner(presetContainer,
                    {
                        minimum: 0,
                        maximum: currentPtValue.Upline.Max - currentPtValue.Max,
                        decimals: Fanex.CustSetting.PT_DECIMALS,
                        step: Fanex.CustSetting.PT_STEP,
                        value: presetSelected,
                        minWidth: 60,
                        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
                    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var presetMaxSpan = document.createElement("SPAN");
    presetMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPtValue.Upline.Max - currentPtValue.Max);
    presetMaxSpan.className = "upline-agent-share";
    presetContainer.appendChild(presetMaxSpan);

    trSetting = document.createElement("TR");
    tableShareSetting.appendChild(trSetting)

    var maxLabel = document.createElement("TD");
    maxLabel.innerHTML = Language.MAX_POSITION;
    maxLabel.title = Language.MAX_POSITION;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var maxSelected = currentPtValue.Max;
    var maxContainer = document.createElement("TD");
    maxContainer.className = "share-setting-value";
    trSetting.appendChild(maxContainer);
    //currentPt.MaxPresetSpan = 2;
    var maxSpinBox = Fanex.CustSetting.PT_RenderSpinner(maxContainer,
                    {
                        minimum: 0,
                        maximum: currentPtValue.Upline.Max,
                        decimals: Fanex.CustSetting.PT_DECIMALS,
                        step: Fanex.CustSetting.PT_STEP,
                        value: maxSelected,
                        minWidth: 60,
                        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
                    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPtValue.Upline.Max);
    uplineMaxSpan.className = "upline-agent-share";
    maxContainer.appendChild(uplineMaxSpan);

    //// Preset K Change Affect to Max K
    presetSpinBox.input.onchange = function (event) {
        var presetValue = presetSpinBox.getValue();
        presetSpinBox.setValue(presetValue);
        var maxValue = maxSpinBox.getValue();
        if (presetValue + maxValue > currentPtValue.Upline.Max) {
            var newMaxValue = currentPtValue.Upline.Max - maxValue;
            presetSpinBox.setValue(newMaxValue);
        }
    }
    //// Max K Change affect To Min K and Preset K
    maxSpinBox.input.onchange = function (event) {
        var maxValue = maxSpinBox.getValue();
        maxSpinBox.setValue(maxValue);
        var presetValue = presetSpinBox.getValue();
        if (maxValue > currentPtValue.Upline.Max) {
            maxSpinBox.setValue(currentPtValue.Upline.Max);
            presetSpinBox.setValue(0);
        }
        else if (maxValue < currentPtValue.Upline.Min) {
            maxSpinBox.setValue(minValue);
        }
        else if (presetValue + maxValue > currentPtValue.Upline.Max) {
            var presetMaxValue = currentPtValue.Upline.Max - maxValue;
            presetSpinBox.setValue(presetMaxValue);
        }
        currentPtValue.MaxMinSpan = maxSpinBox.getValue();
        presetSpinBox.updateMaxValue(currentPtValue.Upline.Max - maxValue);
        presetMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPtValue.Upline.Max - maxValue);
    }

    // Get updated data
    this.SaveSimpleMode = function () {
        var presetValue = presetSpinBox.getValue();
        var maxValue = maxSpinBox.getValue();
        if (currentPtValue.Preset != presetValue
        || currentPtValue.Max != maxValue) {
            return {
                Max: maxValue,
                Preset: presetValue
            };
        }
        else {
            return null;
        }
    };

    return tableShareSetting;
};

Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_4_3_1 = function (owner) {
    //box.appendChild(tableShareSetting);
    // Table
    var currentPtValue = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    trSetting = document.createElement("TR");
    tableShareSetting.appendChild(trSetting)

    var maxLabel = document.createElement("TD");
    maxLabel.innerHTML = Language.MAX_POSITION;
    maxLabel.title = Language.MAX_POSITION;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var valueSelected = currentPtValue.Value;
    var valueContainer = document.createElement("TD");
    valueContainer.className = "share-setting-value";
    trSetting.appendChild(valueContainer);
    //currentPt.MaxPresetSpan = 2;
    var valueSpinBox = Fanex.CustSetting.PT_RenderSpinner(valueContainer,
                    {
                        minimum: 0,
                        maximum: currentPtValue.Upline.Max,
                        decimals: Fanex.CustSetting.PT_DECIMALS,
                        step: Fanex.CustSetting.PT_STEP,
                        value: valueSelected,
                        minWidth: 60,
                        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
                    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPtValue.Upline.Max);
    uplineMaxSpan.className = "upline-agent-share";
    valueContainer.appendChild(uplineMaxSpan);


    // Get updated data
    this.SaveSimpleMode = function () {
        var value = valueSpinBox.getValue();
        if (currentPtValue.Value != value) {
            return {
                Value: value
            };
        }
        else {
            return null;
        }
    };

    return tableShareSetting;
};

Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_2_1_1 = function (owner) {
    //box.appendChild(tableShareSetting);
    // Table
    var currentPtValue = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    trSetting = document.createElement("TR");
    tableShareSetting.appendChild(trSetting)

    var maxLabel = document.createElement("TD");
    maxLabel.innerHTML = Language.MAX_POSITION;
    maxLabel.title = Language.MAX_POSITION;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var maxSelected = currentPtValue.Max;
    var maxContainer = document.createElement("TD");
    maxContainer.className = "share-setting-value";
    trSetting.appendChild(maxContainer);
    //currentPt.MaxPresetSpan = 2;
    var maxSpinBox = Fanex.CustSetting.PT_RenderSpinner(maxContainer,
                    {
                        minimum: 0,
                        maximum: currentPtValue.Upline.Max,
                        decimals: Fanex.CustSetting.PT_DECIMALS,
                        step: Fanex.CustSetting.PT_STEP,
                        value: maxSelected,
                        minWidth: 60,
                        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
                    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPtValue.Upline.Max);
    uplineMaxSpan.className = "upline-agent-share";
    maxContainer.appendChild(uplineMaxSpan);


    // Get updated data
    this.SaveSimpleMode = function () {
        var maxValue = maxSpinBox.getValue();
        if (currentPtValue.Max != maxValue) {
            return {
                Max: maxValue
            };
        }
        else {
            return null;
        }
    };

    return tableShareSetting;
};