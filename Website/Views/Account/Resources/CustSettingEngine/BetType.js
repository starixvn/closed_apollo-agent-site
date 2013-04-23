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
    header.style.minWidth = this.minWidth + 'px';

    //// Given share
    var maxSelected = this.pt.Max;
    currentPt.MaxMaxSpan = this.pt.Upline.Max;
    currentPt.MinMaxSpan = Math.max(this.pt.Upline.Min, this.pt.Min);

    var maxContainer = document.createElement("DIV");
    maxContainer.className = "bet-type-select";
    var maxSpinBox = Fanex.CustSetting.PT_RenderSpinner(maxContainer,
    {
        minimum: currentPt.MinMaxSpan,
        maximum: currentPt.MaxMaxSpan,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: maxSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);
    box.appendChild(maxContainer);

    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinMaxSpan, currentPt.MaxMaxSpan);
    uplineMaxSpan.className = "upline-agent-share";
    maxContainer.appendChild(uplineMaxSpan);

    //// Taken Share
    var presetSelected = this.pt.Preset;
    currentPt.MaxPresetSpan = this.pt.Upline.Max - this.pt.Max;
    var presetContainer = document.createElement("DIV");
    presetContainer.className = "bet-type-select";
    var presetSpinBox = Fanex.CustSetting.PT_RenderSpinner(presetContainer,
    {
        minimum: 0,
        maximum: currentPt.MaxPresetSpan,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: presetSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);
    box.appendChild(presetContainer);
    var presetMaxSpan = document.createElement("SPAN");
    presetMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPt.MaxPresetSpan);
    presetMaxSpan.className = "upline-agent-share";
    presetContainer.appendChild(presetMaxSpan);

    //// Min Share
    var minSelected = this.pt.Min;
    currentPt.MaxMinSpan = this.pt.Max;
    var minContainer = document.createElement("DIV");
    minContainer.className = "bet-type-select";
    var minSpinBox = Fanex.CustSetting.PT_RenderSpinner(minContainer, {
        minimum: 0,
        maximum: currentPt.MaxMinSpan,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: minSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);
    box.appendChild(minContainer);
    var uplineMinSpan = document.createElement("SPAN");
    uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPt.MaxMinSpan);
    uplineMinSpan.className = "upline-agent-share";
    minContainer.appendChild(uplineMinSpan);

    //// Constrain
    //// Preset K Change Affect to Max K
    presetSpinBox.input.onchange = function (event) {
        var presetValue = presetSpinBox.getValue();
        var maxValue = maxSpinBox.getValue();
        if (presetValue + maxValue > currentPt.Upline.Max) {
            var newMaxValue = currentPt.Upline.Max - maxValue;
            presetSpinBox.setValue(newMaxValue);
        }
        else {
            presetSpinBox.setValue(presetValue);
        }
    }
    //// Max K Change affect To Min K and Preset K
    maxSpinBox.input.onchange = function (event) {
        var maxValue = maxSpinBox.getValue();
        var minValue = minSpinBox.getValue();
        var presetValue = presetSpinBox.getValue();
        if (maxValue > currentPt.Upline.Max) {
            maxValue = currentPt.Upline.Max;
            presetSpinBox.setValue(0);
        }
        else if (maxValue < currentPt.MinMaxSpan) {
            maxValue = currentPt.MinMaxSpan;
        }
        else if (presetValue + maxValue > currentPt.Upline.Max) {
            presetSpinBox.setValue(currentPt.Upline.Max - maxValue);
        }
        maxSpinBox.setValue(maxValue);
        //// Update New Preset Range
        var currentMaxPreset = currentPt.Upline.Max - maxValue;
        presetMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentMaxPreset);
        presetSpinBox.updateMaxValue(currentMaxPreset);
        //// Update New Min Range
        uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, maxValue);
        minSpinBox.updateMaxValue(maxValue);
    }
    //// Min K Change affect To Max K
    minSpinBox.input.onchange = function (event) {
        var maxValue = maxSpinBox.getValue();
        var minValue = minSpinBox.getValue();
        if (minValue > maxValue) {
            minSpinBox.setValue(maxValue);
        }
        else {
            minSpinBox.setValue(minValue);
        }
        minValue = Math.max(minValue, currentPt.Upline.Min);
        //// Update New Max Range
        maxSpinBox.updateMinValue(minValue);
        uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(minValue, currentPt.MaxMaxSpan);
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

Fanex.CustSetting.BetType.prototype.Render_2_1_2 =
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

    //// Given Share
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
        if (ptValue < currentPt.Upline.Min) {
            ptValue = currentPt.Upline.Min;
        }
        else if (ptValue > currentPt.Upline.Max) {
            ptValue = currentPt.Upline.Max;
        }
        ptSpinBox.setValue(ptValue);
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
    header.style.minWidth = this.minWidth + 'px';

    //// Given Share
    var maxSelected = this.pt.Max;
    currentPt.MaxMaxSpan = this.pt.Upline.Max;
    var maxContainer = document.createElement("DIV");
    maxContainer.className = "bet-type-select";
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
    box.appendChild(maxContainer);
    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(this.pt.Upline.Min, currentPt.MaxMaxSpan);
    uplineMaxSpan.className = "upline-agent-share";
    maxContainer.appendChild(uplineMaxSpan);

    //// Min Given Share
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
        var minValue = minSpinBox.getValue();
        if (maxValue > currentPt.Upline.Max) {
            maxValue = currentPt.Upline.Max;
        }
        else if (maxValue < minValue) {
            maxValue = minValue;
        }
        maxSpinBox.setValue(maxValue);
        minSpinBox.updateMaxValue(maxValue);
        uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, maxValue);
    }
    //// Min K Change affect To Max K
    minSpinBox.input.onchange = function (event) {
        var maxValue = maxSpinBox.getValue();
        var minValue = minSpinBox.getValue();
        if (minValue > maxValue) {
            minValue = maxValue;
        }
        minSpinBox.setValue(minValue);
        minValue = Math.max(minValue, currentPt.Upline.Min);
        maxSpinBox.updateMinValue(minValue);
        uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(minValue, currentPt.MaxMaxSpan);
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
            this.pt.Preset = Fanex.RoundNumber(this.pt.Upline.Max - this.pt.Max, 2);
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

//// Simple Mode
Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_3_2_1 =
Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_2_2_1 = function (owner) {
    //box.appendChild(tableShareSetting);
    // Table
    var currentPtValue = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    var trSetting = document.createElement("TR");
    tableShareSetting.appendChild(trSetting)

    var maxLabel = document.createElement("TD");
    maxLabel.innerHTML = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.title = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var maxSelected = currentPtValue.Max;
    var maxContainer = document.createElement("TD");
    maxContainer.className = "share-setting-value";
    trSetting.appendChild(maxContainer);
    var maxSpinBox = Fanex.CustSetting.PT_RenderSpinner(maxContainer,
                    {
                        minimum: currentPtValue.Upline.Min,
                        maximum: currentPtValue.Upline.Max,
                        decimals: Fanex.CustSetting.PT_DECIMALS,
                        step: Fanex.CustSetting.PT_STEP,
                        value: maxSelected,
                        minWidth: 60,
                        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
                    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPtValue.Upline.Min, currentPtValue.Upline.Max);
    uplineMaxSpan.className = "upline-agent-share";
    maxContainer.appendChild(uplineMaxSpan);

    trSetting = document.createElement("TR");
    tableShareSetting.appendChild(trSetting)
    // Share Settings
    var presetLabel = document.createElement("TD");
    presetLabel.innerHTML = Language.SIMPLE_MODE_PRESET_POSITION;
    presetLabel.title = Language.SIMPLE_MODE_PRESET_POSITION;
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

    //// Preset K Change Affect to Max K
    presetSpinBox.input.onchange = function (event) {
        var presetValue = presetSpinBox.getValue();
        var maxValue = maxSpinBox.getValue();
        if (presetValue + maxValue > currentPtValue.Upline.Max) {
            presetValue = currentPtValue.Upline.Max - maxValue;
        }

        presetSpinBox.setValue(presetValue);
    }
    //// Max K Change affect To Min K and Preset K
    maxSpinBox.input.onchange = function (event) {
        var maxValue = maxSpinBox.getValue();
        var presetValue = presetSpinBox.getValue();

        if (maxValue > currentPtValue.Upline.Max) {
            maxValue = currentPtValue.Upline.Max;
            presetSpinBox.setValue(0);
        }
        else if (maxValue < currentPtValue.Upline.Min) {
            maxValue = currentPtValue.Upline.Min;
        }
        else if (presetValue + maxValue > currentPtValue.Upline.Max) {
            presetSpinBox.setValue(currentPtValue.Upline.Max - maxValue);
        }
        maxSpinBox.setValue(maxValue);
        var presetMaxValue = currentPtValue.Upline.Max - maxValue;
        presetSpinBox.updateMaxValue(presetMaxValue);
        presetMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, presetMaxValue);
    }

    // Get updated data
    this.SaveSimpleMode = function () {
        var presetValue = presetSpinBox.getValue();
        var maxValue = maxSpinBox.getValue();
        return {
            Max: maxValue,
            Preset: presetValue
        };

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
    maxLabel.innerHTML = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.title = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var valueSelected = currentPtValue.Max;
    var valueContainer = document.createElement("TD");
    valueContainer.className = "share-setting-value";
    trSetting.appendChild(valueContainer);
    var maxSpinBox = Fanex.CustSetting.PT_RenderSpinner(valueContainer,
                    {
                        minimum: currentPtValue.Upline.Min,
                        maximum: currentPtValue.Upline.Max,
                        decimals: Fanex.CustSetting.PT_DECIMALS,
                        step: Fanex.CustSetting.PT_STEP,
                        value: valueSelected,
                        minWidth: 60,
                        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
                    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPtValue.Upline.Min, currentPtValue.Upline.Max);
    uplineMaxSpan.className = "upline-agent-share";
    valueContainer.appendChild(uplineMaxSpan);

    // Get updated data
    this.SaveSimpleMode = function () {
        var maxValue = maxSpinBox.getValue();
        return {
            Max: maxValue
        };
    };

    return tableShareSetting;
};

Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_2_1_2 =
Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_2_1_1 = function (owner) {
    //box.appendChild(tableShareSetting);
    // Table
    var currentPtValue = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    trSetting = document.createElement("TR");
    tableShareSetting.appendChild(trSetting)

    var maxLabel = document.createElement("TD");
    maxLabel.innerHTML = Language.SIMPLE_MODE_PRESET_POSITION;
    maxLabel.title = Language.SIMPLE_MODE_PRESET_POSITION;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var maxSelected = currentPtValue.Value;
    var maxContainer = document.createElement("TD");
    maxContainer.className = "share-setting-value";
    trSetting.appendChild(maxContainer);
    var valueSpinBox = Fanex.CustSetting.PT_RenderSpinner(maxContainer,
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
        var value = valueSpinBox.getValue();
        return {
            Value: value
        };
    };

    return tableShareSetting;
};

//// View Setting
Fanex.CustSetting.BetType.prototype.Render_2_2_3 = function (owner) { // a1
    box = document.createElement("DIV");
    var currentPt = this.pt;
    // Bet type header
    var header = document.createElement("DIV");
    header.innerHTML = this.name;
    header.title = this.title;
    header.className = "bet-type-header";
    box.appendChild(header); // Header bet type name
    header.style.minWidth = this.minWidth + 'px';

    //// Given share
    var maxContainer = document.createElement("DIV");
    maxContainer.className = "bet-type-select-label";
    maxContainer.innerHTML = this.pt.Max;
    box.appendChild(maxContainer);

    //// Taken Share
    var presetContainer = document.createElement("DIV");
    presetContainer.className = "bet-type-select-label";
    presetContainer.innerHTML = this.pt.Preset;
    box.appendChild(presetContainer);

    //// Min Share

    var minContainer = document.createElement("DIV");
    minContainer.className = "bet-type-select-label";
    minContainer.innerHTML = this.pt.Min;
    box.appendChild(minContainer);

    box.id = "e$" + this.suffixId;
    box.className = "pt-box";

    // Get updated data
    this.Save = function () {
            return null;
    }

    return box;
};

Fanex.CustSetting.BetType.prototype.Render_3_3_3 = function (owner) {
    box = document.createElement("DIV");
    var currentPt = this.pt;
    // Bet type header
    var header = document.createElement("DIV");
    header.innerHTML = this.name;
    header.title = this.title;
    header.className = "bet-type-header";
    box.appendChild(header); // Header bet type name
    header.style.minWidth = this.minWidth + 'px';

    //// Given share
    var maxContainer = document.createElement("DIV");
    maxContainer.className = "bet-type-select-label";
    maxContainer.innerHTML = this.pt.Max;
    box.appendChild(maxContainer);

    //// Min Share

    var minContainer = document.createElement("DIV");
    minContainer.className = "bet-type-select-label";
    minContainer.innerHTML = this.pt.Min;
    box.appendChild(minContainer);

    box.id = "e$" + this.suffixId;
    box.className = "pt-box";

    // Get updated data
    this.Save = function () {
        return null;
    }

    return box;
};