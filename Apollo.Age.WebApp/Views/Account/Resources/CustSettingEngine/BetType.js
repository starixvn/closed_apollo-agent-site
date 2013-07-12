/// <reference path="Core.js" />

if (typeof (Fanex) == 'undefined') { // Require Fanex namespace
    Fanex = {};
}
if (typeof (Fanex.CustSetting) == 'undefined') { // Require Fanex.CustSetting namespace
    alert('Require Fanex.CustSetting namespace');
}
// Function name format Render_[RoleId_]TargetRoleId_Form
// [RoleId_] is optional
// Call method by Method.call(this, arg1, arg2...)

Fanex.CustSetting.BetType.prototype.Render_4_2_1 =
Fanex.CustSetting.BetType.prototype.Render_3_2_1 =
Fanex.CustSetting.BetType.prototype.Render_2_2_1 = function (owner) { // a1
    box = document.createElement("DIV");
    var currentPt = this.pt;
    // Bet type header
    var header = document.createElement("DIV");
    header.innerHTML = this.name;
    header.title = this.title;
    //ja-JP
    header.className = "bet-type-header";
    if (Fanex.CustSetting.CURRENT_CONTEXT.Language == 'ja-JP'
    && this.id == 8 && this.sportId == 1) {
        header.className += " minimize-fontsize";
    }
    box.appendChild(header); // Header bet type name
    header.style.minWidth = this.minWidth + 'px';

    //// Taken Share
    var presetSelected = this.pt.Preset;
    currentPt.MaxPresetSpan = Math.max(Fanex.RoundNumber(this.pt.Upline.Max - this.pt.Max, 2), 0);
    currentPt.MinPresetSpan = Math.max(Fanex.RoundNumber(this.pt.Upline.MinGiven - this.pt.Max, 2), 0);
    var presetContainer = document.createElement("DIV");
    presetContainer.className = "bet-type-select";
    var presetSpinBox = Fanex.CustSetting.PT_RenderSpinner(presetContainer,
    {
        minimum: currentPt.MinPresetSpan,
        maximum: currentPt.MaxPresetSpan,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: presetSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);
    box.appendChild(presetContainer);
    var presetMaxSpan = document.createElement("SPAN");
    presetMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinPresetSpan, currentPt.MaxPresetSpan);
    presetMaxSpan.className = "upline-agent-share";
    presetContainer.appendChild(presetMaxSpan);

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


    //// Min Share
    var minSelected = this.pt.Min;
    currentPt.MaxMinSpan = this.pt.Max;
    currentPt.MinMinSpan = Fanex.CustSetting.CURRENT_CONTEXT.HasMinConstrain ? Fanex.CustSetting.MIN_CONSTRAIN_VALUE : 0;
    var minContainer = document.createElement("DIV");
    minContainer.className = "bet-type-select";
    var minSpinBox = Fanex.CustSetting.PT_RenderSpinner(minContainer, {
        minimum: currentPt.MinMinSpan,
        maximum: currentPt.MaxMinSpan,
        decimals: Fanex.CustSetting.PT_DECIMALS,
        step: Fanex.CustSetting.PT_STEP,
        value: minSelected,
        minWidth: this.minWidth,
        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
    }, Fanex.CustSetting.PT_MAX_LENGTH);
    box.appendChild(minContainer);
    var uplineMinSpan = document.createElement("SPAN");
    uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinMinSpan, currentPt.MaxMinSpan);
    uplineMinSpan.className = "upline-agent-share";
    minContainer.appendChild(uplineMinSpan);

    //// Constrain

    //    var updateConstrainAfterEdited = function () {
    //        var maxValue = maxSpinBox.getValue();
    //        var minValue = minSpinBox.getValue();
    //        var presetValue = presetSpinBox.getValue();
    //    }

    //// Preset K Change Affect to Max K
    presetSpinBox.input.onchange = function (event) {
        if (!maxSpinBox.isValid()) {
            return;
        }

        if (!presetSpinBox.digistIsValid()) {
            return;
        }

        var presetValue = presetSpinBox.getValue();
        if (presetSpinBox.isValid()) {
            //presetSpinBox.setValue(presetValue);
        }
    }
    //// Max K Change affect To Min K and Preset K
    maxSpinBox.input.onchange = function (event) {
        var maxValue = maxSpinBox.getValue();
        var minValue = minSpinBox.getValue();
        var presetValue = presetSpinBox.getValue();
        var maxValueCorrect = maxValue;
        if (maxValue > currentPt.Upline.Max) {
            maxValueCorrect = currentPt.Upline.Max;
            //presetSpinBox.setValue(0);
        }
        else if (maxValue < currentPt.MinMaxSpan) {
            maxValueCorrect = currentPt.MinMaxSpan;
        }

        if (maxValueCorrect != maxValue) {
            maxSpinBox.alertRangeMessage();
            return;
        }

        if (!maxSpinBox.digistIsValid()) {
            return;
        }

        //// Update New Preset Range
        currentPt.MaxPresetSpan = Math.max(Fanex.RoundNumber(currentPt.Upline.Max - maxValueCorrect, 2), 0);
        currentPt.MinPresetSpan = Math.max(Fanex.RoundNumber(currentPt.Upline.MinGiven - maxValueCorrect, 2), 0);
        presetMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinPresetSpan, currentPt.MaxPresetSpan);
        presetSpinBox.updateMinValue(currentPt.MinPresetSpan);
        presetSpinBox.updateMaxValue(currentPt.MaxPresetSpan);

        //// Update New Min Range
        currentPt.MaxMinSpan = maxValueCorrect;
        uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPt.MaxMinSpan);
        minSpinBox.updateMaxValue(currentPt.MaxMinSpan);

        if (minValue > currentPt.Upline.Min && minValue > currentPt.MinMaxSpan && minValue <= maxValueCorrect) {
            //// Update New Max Range
            currentPt.MinMaxSpan = minValue;
            maxSpinBox.updateMinValue(minValue);
            uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(minValue, currentPt.MaxMaxSpan);
        }

    }
    //// Min K Change affect To Max K
    minSpinBox.input.onchange = function (event) {
        var minValue = minSpinBox.getValue();
        if (minSpinBox.isValid()) {
            var checkMinValue = Math.max(minValue, currentPt.Upline.Min);
            if (minValue <= checkMinValue && minValue <= currentPt.Upline.Max) {
                maxSpinBox.updateMinValue(minValue);
            };
        }
        if (!maxSpinBox.isValid()) {
            return;
        }
        var maxValue = maxSpinBox.getValue();
        var minValueCorrect = minValue;
        if (minValue > maxValue) {
            minValueCorrect = maxValue;
            //minSpinBox.setValue(maxValue);
        }

        

        if (minValueCorrect != minValue) {
            minSpinBox.alertRangeMessage();
            return;
        }

        if (!minSpinBox.digistIsValid()) {
            return;
        }

        minValueCorrect = Math.max(minValueCorrect, currentPt.Upline.Min);

        //// Update New Max Range
        currentPt.MinMaxSpan = minValueCorrect;
        maxSpinBox.updateMinValue(minValueCorrect);
        uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(minValueCorrect, currentPt.MaxMaxSpan);

        if (maxSpinBox.isValid(true) && currentPt.MaxMinSpan != maxSpinBox.getValue()) {
            //// Update New Min Range
            //            currentPt.MaxMinSpan = maxSpinBox.getValue();
            //            uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPt.MaxMinSpan);
            //            minSpinBox.updateMaxValue(currentPt.MaxMinSpan);
            maxSpinBox.input.onchange();
        }
    }



    box.id = "e$" + this.suffixId;
    box.className = "pt-box";
    box.minSpinBox = minSpinBox;
    box.maxSpinBox = maxSpinBox;
    box.presetSpinBox = presetSpinBox;

    this.Validate = function () {
        if (!maxSpinBox.isValid()) return false;
        if (!minSpinBox.isValid()) return false;
        if (!presetSpinBox.isValid()) return false;
        return true;
    };

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
    if (Fanex.CustSetting.CURRENT_CONTEXT.Language == 'ja-JP'
    && this.id == 8 && this.sportId == 1) {
        header.className += " minimize-fontsize";
    }
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
        if (!ptSpinBox.isValid()) {
            return;
        }

        if (!ptSpinBox.digistIsValid()) {
            return;
        }
    }

    box.id = "e$" + this.suffixId;
    box.className = "pt-box";
    box.ptSpinBox = ptSpinBox;

    this.Validate = function () {
        if (!ptSpinBox.isValid()) return false;
        return true;
    };

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
    if (Fanex.CustSetting.CURRENT_CONTEXT.Language == 'ja-JP'
    && this.id == 8 && this.sportId == 1) {
        header.className += " minimize-fontsize";
    }
    box.appendChild(header); // Header bet type name
    header.style.minWidth = this.minWidth + 'px';

    //// Given Share
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
        var maxValueCorrect = maxValue;
        if (maxValue > currentPt.Upline.Max) {
            maxValueCorrect = currentPt.Upline.Max;
        }
        else if (maxValue < currentPt.MinMaxSpan) {
            maxValueCorrect = currentPt.MinMaxSpan;
        }

        if (maxValue != maxValueCorrect) {
            maxSpinBox.alertRangeMessage();
            return;
        }

        if (!maxSpinBox.digistIsValid()) {
            return;
        }

        currentPt.MaxMinSpan = maxValueCorrect;
        minSpinBox.updateMaxValue(currentPt.MaxMinSpan);
        uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPt.MaxMinSpan);

        minSpinBox.input.onchange();
    }
    //// Min K Change affect To Max K
    minSpinBox.input.onchange = function (event) {
        var minValue = minSpinBox.getValue();
        if (minSpinBox.isValid()) {
            var checkMinValue = Math.max(minValue, currentPt.Upline.Min);
            if (minValue <= checkMinValue && minValue <= currentPt.Upline.Max) {
                maxSpinBox.updateMinValue(minValue);
            };
        }
        if (!maxSpinBox.isValid()) {
            return;
        };
        var maxValue = maxSpinBox.getValue();
        var minValueCorrect = minValue;
        if (minValue > maxValue) {
            minValueCorrect = maxValue;
        }

        if (minValue != minValueCorrect) {
            minSpinBox.alertRangeMessage();
            return;
        }

        if (!minSpinBox.digistIsValid()) {
            return;
        }

        minValueCorrect = Math.max(minValueCorrect, currentPt.Upline.Min);

        currentPt.MinMaxSpan = minValueCorrect;
        maxSpinBox.updateMinValue(currentPt.MinMaxSpan);
        uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinMaxSpan, currentPt.MaxMaxSpan);

        if (maxSpinBox.isValid(true) && currentPt.MaxMinSpan != maxSpinBox.getValue()) {
            //// Update New Min Range
            currentPt.MaxMinSpan = maxSpinBox.getValue();
            uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPt.MaxMinSpan);
            minSpinBox.updateMaxValue(currentPt.MaxMinSpan);
        }
    }


    box.id = "e$" + this.suffixId;
    box.className = "pt-box";
    box.minSpinBox = minSpinBox;
    box.maxSpinBox = maxSpinBox;

    this.Validate = function () {
        if (!maxSpinBox.isValid()) return false;
        if (!minSpinBox.isValid()) return false;
        return true;
    };

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
Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_4_2_1 =
Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_3_2_1 =
Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_2_2_1 = function (owner) {
    //box.appendChild(tableShareSetting);
    // Table
    var currentPt = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    var boxTBody = document.createElement("TBODY");
    tableShareSetting.appendChild(boxTBody);

    // Taken
    var trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting)

    var presetLabel = document.createElement("TD");
    presetLabel.innerHTML = Language.SIMPLE_MODE_PRESET_POSITION;
    presetLabel.title = Language.SIMPLE_MODE_PRESET_POSITION;
    presetLabel.className = "share-setting-label";
    trSetting.appendChild(presetLabel);

    var presetSelected = currentPt.Preset;
    currentPt.MaxPresetSpan = Math.max(Fanex.RoundNumber(currentPt.Upline.Max - currentPt.Max, 2), 0);
    currentPt.MinPresetSpan = Math.max(Fanex.RoundNumber(currentPt.Upline.MinGiven - currentPt.Max, 2), 0);
    var presetContainer = document.createElement("TD");
    presetContainer.className = "share-setting-value";
    trSetting.appendChild(presetContainer);
    var presetSpinBox = Fanex.CustSetting.PT_RenderSpinner(presetContainer,
                    {
                        minimum: currentPt.MinPresetSpan,
                        maximum: currentPt.MaxPresetSpan,
                        decimals: Fanex.CustSetting.PT_DECIMALS,
                        step: Fanex.CustSetting.PT_STEP,
                        value: presetSelected,
                        minWidth: 60,
                        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
                    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var presetMaxSpan = document.createElement("SPAN");
    presetMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinPresetSpan, currentPt.MaxPresetSpan);
    presetMaxSpan.className = "upline-agent-share";
    presetContainer.appendChild(presetMaxSpan);

    // Given
    trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting)
    var maxLabel = document.createElement("TD");
    maxLabel.innerHTML = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.title = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var maxSelected = currentPt.Max;
    var maxContainer = document.createElement("TD");
    currentPt.MinMaxSpan = Math.max(currentPt.Upline.Min, currentPt.Min);
    maxContainer.className = "share-setting-value";
    trSetting.appendChild(maxContainer);
    var maxSpinBox = Fanex.CustSetting.PT_RenderSpinner(maxContainer,
                    {
                        minimum: currentPt.MinMaxSpan,
                        maximum: currentPt.Upline.Max,
                        decimals: Fanex.CustSetting.PT_DECIMALS,
                        step: Fanex.CustSetting.PT_STEP,
                        value: maxSelected,
                        minWidth: 60,
                        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
                    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinMaxSpan, currentPt.Upline.Max);
    uplineMaxSpan.className = "upline-agent-share";
    maxContainer.appendChild(uplineMaxSpan);

    

    //// Preset K Change Affect to Max K
    presetSpinBox.input.onchange = function (event) {
        if (!maxSpinBox.isValid()) {
            return;
        }

        var presetValue = presetSpinBox.getValue();
        if (presetSpinBox.isValid()) {
            //presetSpinBox.setValue(presetValue);
        }
    }
    //// Max K Change affect To Min K and Preset K
    maxSpinBox.input.onchange = function (event) {
        var maxValue = maxSpinBox.getValue();
        var presetValue = presetSpinBox.getValue();
        var maxValueCorrect = maxValue;
        if (maxValue > currentPt.Upline.Max) {
            maxValueCorrect = currentPt.Upline.Max;
        }
        else if (maxValue < currentPt.MinMaxSpan) {
            maxValueCorrect = currentPt.MinMaxSpan;
        }
        else if (presetValue + maxValue > currentPt.Upline.Max) {
            //presetSpinBox.setValue(Fanex.RoundNumber(currentPt.Upline.Max - maxValue, 2));
        }

        if (maxValue != maxValueCorrect) {
            maxSpinBox.alertRangeMessage();
            return;
        }

        if (!maxSpinBox.digistIsValid()) {
            return;
        }

        currentPt.MaxPresetSpan = Math.max(Fanex.RoundNumber(currentPt.Upline.Max - maxValueCorrect, 2), 0);
        currentPt.MinPresetSpan = Math.max(Fanex.RoundNumber(currentPt.Upline.MinGiven - maxValueCorrect, 2), 0);
        presetSpinBox.updateMinValue(currentPt.MinPresetSpan);
        presetSpinBox.updateMaxValue(currentPt.MaxPresetSpan);
        presetMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinPresetSpan, currentPt.MaxPresetSpan);
    }

    this.Validate = function () {
        if (!maxSpinBox.isValid()) return false;
        if (!presetSpinBox.isValid()) return false;
        return true;
    };

    // Get updated data
    this.SaveSimpleMode = function () {
        var presetValue = presetSpinBox.getValue();
        var maxValue = maxSpinBox.getValue();
        return {
            Max: maxValue,
            Preset: presetValue,
            Min: Fanex.CustSetting.CURRENT_CONTEXT.HasMinConstrain ? Math.max(currentPt.Min, Fanex.CustSetting.MIN_CONSTRAIN_VALUE) : currentPt.Min
        };

    };

    return tableShareSetting;
};

Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_4_3_1 = function (owner) {
    //box.appendChild(tableShareSetting);
    // Table
    var currentPt = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    var boxTBody = document.createElement("TBODY");
    tableShareSetting.appendChild(boxTBody);
    trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting)

    var maxLabel = document.createElement("TD");
    maxLabel.innerHTML = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.title = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var valueSelected = currentPt.Max;
    var valueContainer = document.createElement("TD");
    currentPt.MinMaxSpan = Math.max(currentPt.Upline.Min, currentPt.Min);
    valueContainer.className = "share-setting-value";
    trSetting.appendChild(valueContainer);
    var maxSpinBox = Fanex.CustSetting.PT_RenderSpinner(valueContainer,
                    {
                        minimum: currentPt.MinMaxSpan,
                        maximum: currentPt.Upline.Max,
                        decimals: Fanex.CustSetting.PT_DECIMALS,
                        step: Fanex.CustSetting.PT_STEP,
                        value: valueSelected,
                        minWidth: 60,
                        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
                    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinMaxSpan, currentPt.Upline.Max);
    uplineMaxSpan.className = "upline-agent-share";
    valueContainer.appendChild(uplineMaxSpan);

    maxSpinBox.input.onchange = function (event) {
        if (!maxSpinBox.isValid()) {
            return;
        }
    }

    this.Validate = function () {
        if (!maxSpinBox.isValid()) return false;
        return true;
    };

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
    var currentPt = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    var boxTBody = document.createElement("TBODY");
    tableShareSetting.appendChild(boxTBody);
    trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting)

    var maxLabel = document.createElement("TD");
    maxLabel.innerHTML = Language.PERCENT_IN_MEMBER;
    maxLabel.title = Language.PERCENT_IN_MEMBER;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var maxSelected = currentPt.Value;
    var maxContainer = document.createElement("TD");
    maxContainer.className = "share-setting-value";
    trSetting.appendChild(maxContainer);
    var valueSpinBox = Fanex.CustSetting.PT_RenderSpinner(maxContainer,
                    {
                        minimum: currentPt.Upline.Min,
                        maximum: currentPt.Upline.Max,
                        decimals: Fanex.CustSetting.PT_DECIMALS,
                        step: Fanex.CustSetting.PT_STEP,
                        value: maxSelected,
                        minWidth: 60,
                        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
                    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.Upline.Min, currentPt.Upline.Max);
    uplineMaxSpan.className = "upline-agent-share";
    maxContainer.appendChild(uplineMaxSpan);

    valueSpinBox.input.onchange = function (event) {
        if (!valueSpinBox.isValid()) {
            return;
        }

    }

    this.Validate = function () {
        if (!valueSpinBox.isValid()) return false;
        return true;
    };
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
Fanex.CustSetting.BetType.prototype.Render_2_2_3 =
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

    this.Validate = function () {
        return true;
    };

    // Get updated data
    this.Save = function () {
        return null;
    }

    return box;
};

Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_2_2_3 =
Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_3_3_3 = function (owner) {
    var currentPt = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    var boxTBody = document.createElement("TBODY");
    tableShareSetting.appendChild(boxTBody);
    trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting)

    var maxLabel = document.createElement("TD");
    maxLabel.innerHTML = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.title = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var maxContainer = document.createElement("TD");
    maxContainer.className = "share-setting-value";
    maxContainer.innerHTML = " " + currentPt.Max;
    trSetting.appendChild(maxContainer);

    this.Validate = function () {
        return true;
    };

    // Get updated data
    this.SaveSimpleMode = function () {
        return null;
    };

    return tableShareSetting;
};

//// Shadow Account view
Fanex.CustSetting.BetType.prototype.Render_4_2_4 =
Fanex.CustSetting.BetType.prototype.Render_3_2_4 =
Fanex.CustSetting.BetType.prototype.Render_2_2_4 = function (owner) { // a1
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

    return box;
};

Fanex.CustSetting.BetType.prototype.Render_2_1_4 = function (owner) {
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
    var ptContainer = document.createElement("DIV");
    ptContainer.className = "bet-type-select-label";
    ptContainer.innerHTML = this.pt.Value;
    box.appendChild(ptContainer);
   
    box.id = "e$" + this.suffixId;
    box.className = "pt-box";

    return box;
};

Fanex.CustSetting.BetType.prototype.Render_4_3_4 = function (owner) {
    box = document.createElement("DIV");
    var currentPt = this.pt;
    // Bet type header
    var header = document.createElement("DIV");
    header.innerHTML = this.name;
    header.title = this.title;
    header.className = "bet-type-header";
    box.appendChild(header); // Header bet type name
    header.style.minWidth = this.minWidth + 'px';

    var maxContainer = document.createElement("DIV");
    maxContainer.className = "bet-type-select-label";
    maxContainer.innerHTML = this.pt.Max;
    box.appendChild(maxContainer);

    //// Min Given Share
    var minContainer = document.createElement("DIV");
    minContainer.className = "bet-type-select-label";
    minContainer.innerHTML = this.pt.Min;
    box.appendChild(minContainer);
   
    box.id = "e$" + this.suffixId;
    box.className = "pt-box";

    return box;
};

//// Simple Mode
Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_4_2_4 =
Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_3_2_4 =
Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_2_2_4 = function (owner) {
    //box.appendChild(tableShareSetting);
    // Table
    var currentPt = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    var boxTBody = document.createElement("TBODY");
    tableShareSetting.appendChild(boxTBody);

    var trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting)
    // Share Settings
    var presetLabel = document.createElement("TD");
    presetLabel.innerHTML = Language.SIMPLE_MODE_PRESET_POSITION;
    presetLabel.title = Language.SIMPLE_MODE_PRESET_POSITION;
    presetLabel.className = "share-setting-label";
    trSetting.appendChild(presetLabel);

    var presetContainer = document.createElement("TD");
    presetContainer.className = "share-setting-value";
    presetContainer.innerHTML = currentPt.Preset;
    trSetting.appendChild(presetContainer);

    trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting)

    var maxLabel = document.createElement("TD");
    maxLabel.innerHTML = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.title = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var maxContainer = document.createElement("TD");
    maxContainer.className = "share-setting-value";
    maxContainer.innerHTML = currentPt.Max;
    trSetting.appendChild(maxContainer);

    

    return tableShareSetting;
};

Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_4_3_4 = function (owner) {
    //box.appendChild(tableShareSetting);
    // Table
    var currentPt = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    var boxTBody = document.createElement("TBODY");
    tableShareSetting.appendChild(boxTBody);
    trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting)

    var maxLabel = document.createElement("TD");
    maxLabel.innerHTML = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.title = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var valueContainer = document.createElement("TD");
    valueContainer.className = "share-setting-value";
    valueContainer.innerHTML = currentPt.Max;
    trSetting.appendChild(valueContainer);
    
    return tableShareSetting;
};

Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_2_1_4 = function (owner) {
    //box.appendChild(tableShareSetting);
    // Table
    var currentPt = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    var boxTBody = document.createElement("TBODY");
    tableShareSetting.appendChild(boxTBody);
    trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting)

    var maxLabel = document.createElement("TD");
    maxLabel.innerHTML = Language.PERCENT_IN_MEMBER;
    maxLabel.title = Language.PERCENT_IN_MEMBER;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var maxSelected = currentPt.Value;
    var maxContainer = document.createElement("TD");
    maxContainer.className = "share-setting-value";
    maxContainer.innerHTML = currentPt.Value;
    trSetting.appendChild(maxContainer);

    return tableShareSetting;
};
