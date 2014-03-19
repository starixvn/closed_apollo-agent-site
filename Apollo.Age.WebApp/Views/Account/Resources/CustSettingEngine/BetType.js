/// <reference path="Core.js" />
/*global Language, Fanex*/
/*jshint camelcase: false */
/*jshint maxstatements:false*/
if (typeof (Fanex) === 'undefined') { // Require Fanex namespace
    Fanex = {}; // jshint ignore:line
}
if (typeof (Fanex.CustSetting) === 'undefined') { // Require Fanex.CustSetting namespace
    alert('Require Fanex.CustSetting namespace');
}
// Function name format Render_[RoleId_]TargetRoleId_Form
// [RoleId_] is optional
// Call method by Method.call(this, arg1, arg2...)

Fanex.CustSetting.BetType.prototype.Render_4_2_1 =
Fanex.CustSetting.BetType.prototype.Render_3_2_1 =
Fanex.CustSetting.BetType.prototype.Render_2_2_1 = function () { // a1
    'use strict';
    var currentContext = Fanex.CustSetting.CURRENT_CONTEXT;
    var box = document.createElement("DIV");
    var currentPt = this.pt;
    // Bet type header
    var header = document.createElement("DIV");
    header.innerHTML = this.name;
    header.title = this.title;
    //ja-JP
    header.className = "bet-type-header";
    if (currentContext.Language === 'ja-JP' && this.id === 8 && this.sportId === 1) {
        header.className += " minimize-fontsize";
    }
    box.appendChild(header); // Header bet type name
    header.style.minWidth = this.minWidth + 'px';

    //// Upline setting lable
    var uplineSettingContainer = document.createElement("DIV");
    uplineSettingContainer.className = "bet-type-select spinBox";
    uplineSettingContainer.style.minWidth = this.minWidth + 'px';
    box.appendChild(uplineSettingContainer);
    var uplineSettingSpan = document.createElement("SPAN");
    uplineSettingSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(this.pt.Upline.MinGiven, this.pt.Upline.Max);
    uplineSettingSpan.className = "upline-agent-share";
    uplineSettingContainer.appendChild(uplineSettingSpan);

    //// Taken Share
    var presetSelected = this.pt.Preset;
    currentPt.MaxPresetSpan = Fanex.RoundNumber(this.pt.Upline.Max - this.pt.Upline.Min, 2);
    currentPt.MinPresetSpan = 0;

    var presetContainer = document.createElement("DIV");
    presetContainer.className = "bet-type-select-preset";
    var presetSpinBox = Fanex.CustSetting.RenderSpinner(presetContainer,
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
    currentPt.MaxMaxSpan = Math.max(Fanex.RoundNumber(this.pt.Upline.Max - this.pt.Preset, 2), 0);
    currentPt.MinMaxSpan = Math.max(0, Fanex.RoundNumber(this.pt.Upline.MinGiven - this.pt.Preset, 2), this.pt.Upline.Min);

    var maxContainer = document.createElement("DIV");
    maxContainer.className = "bet-type-select";
    var maxSpinBox = Fanex.CustSetting.RenderSpinner(maxContainer,
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
    var minSpinBox = Fanex.CustSetting.RenderSpinner(minContainer, {
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

    //// Preset K Change Affect to Max K
    presetSpinBox.input.onchange = function () {
        var maxValue = maxSpinBox.getValue();
        var presetValue = presetSpinBox.getValue();
        var presetCorrectValue = presetValue;
        if (presetValue > currentPt.MaxPresetSpan) {
            presetCorrectValue = currentPt.MaxPresetSpan;
            //presetSpinBox.setValue(0);
        }
        else if (presetValue < currentPt.MinPresetSpan) {
            presetCorrectValue = currentPt.MinPresetSpan;
        }

        if (presetCorrectValue !== presetValue) {
            presetSpinBox.alertRangeMessage();
            return;
        }

        if (!presetSpinBox.digistIsValid()) {
            return;
        }

        //// Update New Preset Range
        currentPt.MaxMaxSpan = Math.max(Fanex.RoundNumber(currentPt.Upline.Max - presetValue, 2), 0);
        currentPt.MinMaxSpan = Math.max(0, Fanex.RoundNumber(currentPt.Upline.MinGiven - presetValue, 2), currentPt.Upline.Min);
        uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinMaxSpan, currentPt.MaxMaxSpan);
        maxSpinBox.updateMaxValue(currentPt.MaxMaxSpan);
        maxSpinBox.updateMinValue(currentPt.MinMaxSpan);
        maxSpinBox.setValue(currentPt.MaxMaxSpan);
        maxValue = maxSpinBox.getValue();

        //// Update New Min Range
        var newMinValue = Math.max(Fanex.RoundNumber(currentPt.Upline.MinGiven - presetValue, 2), 0);
        minSpinBox.updateMaxValue(maxValue);
        uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinMinSpan, maxValue);
        minSpinBox.setValue(newMinValue);
    };

    //// Max K Change affect To Min K and Preset K
    maxSpinBox.input.onchange = function () {
        if (!presetSpinBox.isValid()) {
            return;
        }
        if (!maxSpinBox.digistIsValid() || !maxSpinBox.isValid()) {
            return;
        }

        currentPt.MaxMinSpan = maxSpinBox.getValue();
        uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinMinSpan, currentPt.MaxMinSpan);
        minSpinBox.updateMaxValue(currentPt.MaxMinSpan);
        minSpinBox.setValue(Math.min(minSpinBox.getValue(), currentPt.MaxMinSpan));
    };

    //// Min K Change affect To Max K
    minSpinBox.input.onchange = function () {
        if (!minSpinBox.digistIsValid() || !minSpinBox.isValid()) {
            return;
        }
    };

    box.id = "e$" + this.suffixId;
    box.className = "pt-box";
    box.minSpinBox = minSpinBox;
    box.maxSpinBox = maxSpinBox;
    box.presetSpinBox = presetSpinBox;

    box.updateRangeOfMaxForCopy = function (minValue, maxValue) {
        uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(minValue, maxValue);
        maxSpinBox.updateMaxValue(maxValue);
        maxSpinBox.updateMinValue(minValue);
    };

    box.updateRangeOfMinForCopy = function (minValue, maxValue) {
        uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(minValue, maxValue);
        minSpinBox.updateMaxValue(maxValue);
    };

    this.Validate = function () {
        if (!presetSpinBox.isValid()) { return false; }
        if (!maxSpinBox.isValid()) { return false; }
        if (!minSpinBox.isValid()) { return false; }

        return true;
    };

    // Get updated data
    this.Save = function () {
        this.pt.Min = minSpinBox.getValue();
        this.pt.Max = maxSpinBox.getValue();
        this.pt.Preset = presetSpinBox.getValue();
        var isChangePt = this.oldPt.Min !== this.pt.Min
        || this.oldPt.Max !== this.pt.Max
        || this.oldPt.Preset !== this.pt.Preset;
        this.pt.IsChangePt = isChangePt;
        if (isChangePt) {
            return {
                Id: this.id,
                Pt: this.pt
            };
        }
        else {
            return null;
        }
    };

    return box;
};

Fanex.CustSetting.BetType.prototype.Render_2_1_5 =
Fanex.CustSetting.BetType.prototype.Render_2_1_2 =
Fanex.CustSetting.BetType.prototype.Render_2_1_1 = function () {
    'use strict';
    var box = document.createElement("DIV");

    // Bet type header
    var header = document.createElement("DIV");
    header.innerHTML = this.name;
    header.title = this.title;
    header.className = "bet-type-header";
    if (Fanex.CustSetting.CURRENT_CONTEXT.Language === 'ja-JP' && this.id === 8 && this.sportId === 1) {
        header.className += " minimize-fontsize";
    }
    box.appendChild(header); // Header bet type name
    header.style.minWidth = this.minWidth + 'px';

    //// Given Share
    var ptSelected = this.pt.Value;
    var ptContainer = document.createElement("DIV");
    ptContainer.className = "bet-type-select";
    box.appendChild(ptContainer);
    var ptSpinBox = Fanex.CustSetting.RenderSpinner(ptContainer, {
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

    ptSpinBox.input.onchange = function () {
        if (!ptSpinBox.isValid()) {
            return;
        }

        if (!ptSpinBox.digistIsValid()) {
            return;
        }
    };

    box.id = "e$" + this.suffixId;
    box.className = "pt-box";
    box.ptSpinBox = ptSpinBox;

    this.Validate = function () {
        if (!ptSpinBox.isValid()) { return false; }
        return true;
    };

    // Get updated data
    this.Save = function () {
        this.pt.Value = ptSpinBox.getValue();

        var isChangePt = this.oldPt.Value !== this.pt.Value || Fanex.CustSetting.CURRENT_CONTEXT.FormId === Fanex.CustSetting.FormTypes.EDIT_MULTIPLE;
        this.pt.IsChangePt = isChangePt;

        if (isChangePt) {
            return {
                Id: this.id,
                Pt: this.pt
            };
        }
        else {
            return null;
        }
    };

    return box;
};

Fanex.CustSetting.BetType.prototype.Render_4_3_1 = function () {
    'use strict';
    var box = document.createElement("DIV");
    var currentPt = this.pt;
    // Bet type header
    var header = document.createElement("DIV");
    header.innerHTML = this.name;
    header.title = this.title;
    header.className = "bet-type-header";
    if (Fanex.CustSetting.CURRENT_CONTEXT.Language === 'ja-JP' && this.id === 8 && this.sportId === 1) {
        header.className += " minimize-fontsize";
    }
    box.appendChild(header); // Header bet type name
    header.style.minWidth = this.minWidth + 'px';

    //// Upline setting lable    
    var uplineSettingContainer = document.createElement("DIV");
    uplineSettingContainer.className = "bet-type-select";
    uplineSettingContainer.style.minWidth = this.minWidth + 'px';
    box.appendChild(uplineSettingContainer);
    var uplineSettingSpan = document.createElement("SPAN");
    uplineSettingSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(this.pt.Upline.MinGiven, this.pt.Upline.Max);
    uplineSettingSpan.className = "upline-agent-share";
    uplineSettingContainer.appendChild(uplineSettingSpan);

    //// Given Share
    var maxSelected = this.pt.Max;
    currentPt.MaxMaxSpan = this.pt.Upline.Max;
    currentPt.MinMaxSpan = Math.max(this.pt.Upline.Min, this.pt.Min);
    var maxContainer = document.createElement("DIV");
    maxContainer.className = "bet-type-select";
    var maxSpinBox = Fanex.CustSetting.RenderSpinner(maxContainer,
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
    var minSpinBox = Fanex.CustSetting.RenderSpinner(minContainer, {
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
    maxSpinBox.input.onchange = function () {
        var maxValue = maxSpinBox.getValue();
        var maxValueCorrect = maxValue;
        if (maxValue > currentPt.Upline.Max) {
            maxValueCorrect = currentPt.Upline.Max;
        }
        else if (maxValue < currentPt.MinMaxSpan) {
            maxValueCorrect = currentPt.MinMaxSpan;
        }
        if (maxValue !== maxValueCorrect) {
            maxSpinBox.alertRangeMessage();
            return;
        }
        if (!maxSpinBox.digistIsValid()) {
            return;
        }
        currentPt.MaxMinSpan = maxValueCorrect;
        minSpinBox.updateMaxValueWithNoReset(currentPt.MaxMinSpan);
        uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPt.MaxMinSpan);
        //minSpinBox.input.onchange();
    };
    //// Min K Change affect To Max K
    minSpinBox.input.onchange = function () {
        var minValue = minSpinBox.getValue();
        if (minSpinBox.isValid()) {
            var checkMinValue = Math.max(minValue, currentPt.Upline.Min);
            if (minValue <= checkMinValue && minValue <= currentPt.Upline.Max) {
                maxSpinBox.updateMinValueWithNoReset(minValue);
            }
        }
        if (!maxSpinBox.isValid()) {
            return;
        }
        var maxValue = maxSpinBox.getValue();
        var minValueCorrect = minValue;
        if (minValue > maxValue) {
            minValueCorrect = maxValue;
        }
        if (minValue !== minValueCorrect) {
            minSpinBox.alertRangeMessage();
            return;
        }
        if (!minSpinBox.digistIsValid()) {
            return;
        }
        minValueCorrect = Math.max(minValueCorrect, currentPt.Upline.Min);
        currentPt.MinMaxSpan = minValueCorrect;
        maxSpinBox.updateMinValueWithNoReset(currentPt.MinMaxSpan);
        uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinMaxSpan, currentPt.MaxMaxSpan);
        if (maxSpinBox.isValid(true) && currentPt.MaxMinSpan !== maxSpinBox.getValue()) {
            //// Update New Min Range
            currentPt.MaxMinSpan = maxSpinBox.getValue();
            uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentPt.MaxMinSpan);
            minSpinBox.updateMaxValueWithNoReset(currentPt.MaxMinSpan);
        }
    };
    box.id = "e$" + this.suffixId;
    box.className = "pt-box";
    box.minSpinBox = minSpinBox;
    box.maxSpinBox = maxSpinBox;
    this.Validate = function () {
        if (!maxSpinBox.isValid()) { return false; }
        if (!minSpinBox.isValid()) { return false; }
        return true;
    };
    // Get updated data
    this.Save = function () {
        this.pt.Min = minSpinBox.getValue();
        this.pt.Max = maxSpinBox.getValue();
        var isChangePt = this.oldPt.Min !== this.pt.Min || this.oldPt.Max !== this.pt.Max;
        this.pt.IsChangePt = isChangePt;
        if (isChangePt) {
            this.pt.Preset = Fanex.RoundNumber(this.pt.Upline.Max - this.pt.Max, 2);
            return {
                Id: this.id,
                Pt: this.pt
            };
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
Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_2_2_1 = function () {
    //box.appendChild(tableShareSetting);
    // Table
    'use strict';
    var currentPt = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    var boxTBody = document.createElement("TBODY");
    tableShareSetting.appendChild(boxTBody);

    //// Upline setting lable
    var trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting);
    var uplineSettingLabel = document.createElement("TD");
    uplineSettingLabel.innerHTML = Language.UPLINE_SETTING;
    uplineSettingLabel.title = Language.UPLINE_SETTING;
    uplineSettingLabel.className = "share-setting-label";
    trSetting.appendChild(uplineSettingLabel);

    var uplineSettingContainer = document.createElement("TD");
    uplineSettingContainer.className = "bet-type-select spinBox";
    trSetting.appendChild(uplineSettingContainer);
    var uplineSettingSpan = document.createElement("SPAN");
    uplineSettingSpan.innerHTML = Fanex.CustSetting.BuildUplineShareString(this.currentPtValue.Upline.MinGiven, this.currentPtValue.Upline.Max);
    uplineSettingSpan.className = "upline-agent-share";
    uplineSettingContainer.appendChild(uplineSettingSpan);

    // Taken
    trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting);

    var presetLabel = document.createElement("TD");
    presetLabel.innerHTML = Language.SIMPLE_MODE_PRESET_POSITION;
    presetLabel.title = Language.SIMPLE_MODE_PRESET_POSITION;
    presetLabel.className = "share-setting-label";
    trSetting.appendChild(presetLabel);

    var presetSelected = currentPt.Preset;
    currentPt.MaxPresetSpan = Fanex.RoundNumber(currentPt.Upline.Max - currentPt.Upline.Min, 2);
    currentPt.MinPresetSpan = 0;
    var presetContainer = document.createElement("TD");
    presetContainer.className = "share-setting-value";
    trSetting.appendChild(presetContainer);
    var presetSpinBox = Fanex.CustSetting.RenderSpinner(presetContainer,
                    {
                        minimum: currentPt.MinPresetSpan,
                        maximum: currentPt.MaxPresetSpan,
                        decimals: Fanex.CustSetting.PT_DECIMALS,
                        step: Fanex.CustSetting.PT_STEP,
                        value: presetSelected,
                        minWidth: 400,
                        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
                    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var presetMaxSpan = document.createElement("SPAN");
    presetMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinPresetSpan, currentPt.MaxPresetSpan);
    presetMaxSpan.className = "upline-agent-share";
    presetContainer.appendChild(presetMaxSpan);


    // Given
    trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting);
    var maxLabel = document.createElement("TD");
    maxLabel.innerHTML = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.title = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var maxSelected = currentPt.Max;
    var maxContainer = document.createElement("TD");
    currentPt.MaxMaxSpan = Math.max(Fanex.RoundNumber(currentPt.Upline.Max - currentPt.Preset, 2), 0);
    currentPt.MinMaxSpan = Math.max(Fanex.RoundNumber(currentPt.Upline.MinGiven - currentPt.Preset, 2), 0, currentPt.Upline.Min);
    maxContainer.className = "share-setting-value";
    trSetting.appendChild(maxContainer);
    var maxSpinBox = Fanex.CustSetting.RenderSpinner(maxContainer,
                    {
                        minimum: currentPt.MinMaxSpan,
                        maximum: currentPt.MaxMaxSpan,
                        decimals: Fanex.CustSetting.PT_DECIMALS,
                        step: Fanex.CustSetting.PT_STEP,
                        value: maxSelected,
                        minWidth: 400,
                        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
                    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinMaxSpan, currentPt.MaxMaxSpan);
    uplineMaxSpan.className = "upline-agent-share";
    maxContainer.appendChild(uplineMaxSpan);

    // Min Given
    trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting);
    var minLabel = document.createElement("TD");
    minLabel.innerHTML = Language.MIN_POSITION;
    minLabel.title = Language.MIN_POSITION;
    minLabel.className = "share-setting-label";
    trSetting.appendChild(minLabel);

    var minSelected = currentPt.Min;
    var minContainer = document.createElement("TD");
    currentPt.MaxMinSpan = currentPt.Max;
    currentPt.MinMinSpan = Fanex.CustSetting.CURRENT_CONTEXT.HasMinConstrain ? Fanex.CustSetting.MIN_CONSTRAIN_VALUE : 0;
    minContainer.className = "share-setting-value";
    trSetting.appendChild(minContainer);
    var minSpinBox = Fanex.CustSetting.RenderSpinner(minContainer,
                    {
                        minimum: currentPt.MinMinSpan,
                        maximum: currentPt.MaxMinSpan,
                        decimals: Fanex.CustSetting.PT_DECIMALS,
                        step: Fanex.CustSetting.PT_STEP,
                        value: minSelected,
                        minWidth: 400,
                        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
                    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var uplineMinSpan = document.createElement("SPAN");
    uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinMinSpan, currentPt.MaxMinSpan);
    uplineMinSpan.className = "upline-agent-share";
    minContainer.appendChild(uplineMinSpan);

    //// Preset K Change Affect to Max K
    presetSpinBox.input.onchange = function () {
        var presetValue = presetSpinBox.getValue();
        var presetValueCorrect = presetValue;
        if (presetValue > currentPt.MaxPresetSpan) {
            presetValueCorrect = currentPt.MaxPresetSpan;
        }
        else if (presetValue < currentPt.MinPresetSpan) {
            presetValueCorrect = currentPt.MinPresetSpan;
        }

        if (presetValue !== presetValueCorrect) {
            presetSpinBox.alertRangeMessage();
            return;
        }

        if (!presetSpinBox.digistIsValid()) {
            return;
        }

        //// Update Max Taken Value
        currentPt.MaxMaxSpan = Math.max(Fanex.RoundNumber(currentPt.Upline.Max - presetValueCorrect, 2), 0);
        currentPt.MinMaxSpan = Math.max(Fanex.RoundNumber(currentPt.Upline.MinGiven - presetValueCorrect, 2), 0, currentPt.Upline.Min);
        maxSpinBox.updateMaxValue(currentPt.MaxMaxSpan);
        maxSpinBox.updateMinValue(currentPt.MinMaxSpan);
        uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinMaxSpan, currentPt.MaxMaxSpan);
        maxSpinBox.setValue(currentPt.MaxMaxSpan);
        var maxValue = maxSpinBox.getValue();
        //// Update Min Taken
        var newMinValue = Math.max(Fanex.RoundNumber(currentPt.Upline.MinGiven - presetValueCorrect, 2), 0);
        uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinMinSpan, maxValue);
        minSpinBox.updateMaxValue(maxValue);
        minSpinBox.setValue(newMinValue);
    };
    //// Max K Change affect To Min K and Preset K
    maxSpinBox.input.onchange = function () {
        if (!presetSpinBox.isValid() || !maxSpinBox.isValid()) {
            return;
        }

        currentPt.MaxMinSpan = maxSpinBox.getValue();
        minSpinBox.updateMaxValue(currentPt.MaxMinSpan);
        minSpinBox.setValue(Math.min(minSpinBox.getValue(), currentPt.MaxMinSpan));
        uplineMinSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinMinSpan, currentPt.MaxMinSpan);
    };

    minSpinBox.input.onchange = function () {
        if (!minSpinBox.isValid()) {
            return;
        }
    };

    this.Validate = function () {
        if (!presetSpinBox.isValid()) { return false; }
        if (!maxSpinBox.isValid()) { return false; }
        if (!minSpinBox.isValid()) { return false; }
        return true;
    };

    // Get updated data
    this.SaveSimpleMode = function () {
        var presetValue = presetSpinBox.getValue();
        var maxValue = maxSpinBox.getValue();
        var minValue = minSpinBox.getValue();
        return {
            Max: maxValue,
            Preset: presetValue,
            Min: minValue
        };
    };

    return tableShareSetting;
};

Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_4_3_1 = function () {
    //box.appendChild(tableShareSetting);
    // Table
    'use strict';
    var currentPt = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    var boxTBody = document.createElement("TBODY");
    tableShareSetting.appendChild(boxTBody);
    var trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting);

    //// Upline setting lable    
    var uplineSettingContainer = document.createElement("TD");
    uplineSettingContainer.innerHTML = Language.UPLINE_SETTING;
    uplineSettingContainer.title = Language.UPLINE_SETTING;
    uplineSettingContainer.className = "share-setting-label";
    trSetting.appendChild(uplineSettingContainer);

    var valueUplineSettingContainer = document.createElement("TD");
    valueUplineSettingContainer.className = "share-setting-value";
    trSetting.appendChild(valueUplineSettingContainer);
    var uplineSettingSpan = document.createElement("SPAN");
    uplineSettingSpan.innerHTML = Fanex.CustSetting.BuildUplineShareString(this.currentPtValue.Upline.MinGiven, this.currentPtValue.Upline.Max);
    uplineSettingSpan.className = "upline-agent-share";
    valueUplineSettingContainer.appendChild(uplineSettingSpan);

    trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting);
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
    var maxSpinBox = Fanex.CustSetting.RenderSpinner(valueContainer,
{
    minimum: currentPt.MinMaxSpan,
    maximum: currentPt.Upline.Max,
    decimals: Fanex.CustSetting.PT_DECIMALS,
    step: Fanex.CustSetting.PT_STEP,
    value: valueSelected,
    minWidth: 200,
    lastLetter: Fanex.CustSetting.PT_LAST_LETTER
}, Fanex.CustSetting.PT_MAX_LENGTH);
    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.MinMaxSpan, currentPt.Upline.Max);
    uplineMaxSpan.className = "upline-agent-share";
    valueContainer.appendChild(uplineMaxSpan);

    maxSpinBox.input.onchange = function () {
        if (!maxSpinBox.isValid()) {
            return;
        }
    };
    this.Validate = function () {
        if (!maxSpinBox.isValid()) { return false; }
        return true;
    };
    // Get updated data
    this.SaveSimpleMode = function () {
        var maxValue = maxSpinBox.getValue();
        return {
            Max: maxValue,
            IsChangePt: true
        };
    };
    return tableShareSetting;
};

Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_2_1_5 =
Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_2_1_2 =
Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_2_1_1 = function () {
    //box.appendChild(tableShareSetting);
    // Table
    'use strict';
    var currentPt = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    var boxTBody = document.createElement("TBODY");
    tableShareSetting.appendChild(boxTBody);

    // On member
    var trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting);
    var maxLabel = document.createElement("TD");
    maxLabel.innerHTML = Language.PERCENT_IN_MEMBER;
    maxLabel.title = Language.PERCENT_IN_MEMBER;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var maxSelected = currentPt.Value;
    var maxContainer = document.createElement("TD");
    maxContainer.className = "share-setting-value";
    trSetting.appendChild(maxContainer);
    var valueSpinBox = Fanex.CustSetting.RenderSpinner(maxContainer,
                    {
                        minimum: currentPt.Upline.Min,
                        maximum: currentPt.Upline.Max,
                        decimals: Fanex.CustSetting.PT_DECIMALS,
                        step: Fanex.CustSetting.PT_STEP,
                        value: maxSelected,
                        minWidth: 200,
                        lastLetter: Fanex.CustSetting.PT_LAST_LETTER
                    }, Fanex.CustSetting.PT_MAX_LENGTH);

    var uplineMaxSpan = document.createElement("SPAN");
    uplineMaxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(currentPt.Upline.Min, currentPt.Upline.Max);
    uplineMaxSpan.className = "upline-agent-share";
    maxContainer.appendChild(uplineMaxSpan);

    valueSpinBox.input.onchange = function () {
        if (!valueSpinBox.isValid()) {
            return;
        }
    };

    this.Validate = function () {
        if (!valueSpinBox.isValid()) { return false; }
        return true;
    };
    // Get updated data
    this.SaveSimpleMode = function () {
        var value = valueSpinBox.getValue();
        return {
            Value: value,
            IsChangePt: true
        };
    };

    return tableShareSetting;
};

//// View Setting
Fanex.CustSetting.BetType.prototype.Render_2_2_3 =
Fanex.CustSetting.BetType.prototype.Render_3_3_3 = function () {
    'use strict';
    var box = document.createElement("DIV");

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
    };

    return box;
};

Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_2_2_3 =
Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_3_3_3 = function () {
    'use strict';
    var currentPt = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    var boxTBody = document.createElement("TBODY");
    tableShareSetting.appendChild(boxTBody);
    var trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting);

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
Fanex.CustSetting.BetType.prototype.Render_2_2_4 = function () { // a1
    'use strict';
    var box = document.createElement("DIV");

    // Bet type header
    var header = document.createElement("DIV");
    header.innerHTML = this.name;
    header.title = this.title;
    header.className = "bet-type-header";
    box.appendChild(header); // Header bet type name
    header.style.minWidth = this.minWidth + 'px';

    //// Upline Setting Lable
    var uplineSettingContainer = document.createElement("DIV");
    uplineSettingContainer.className = "bet-type-select-label";
    uplineSettingContainer.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(this.pt.Upline.MinGiven, this.pt.Upline.Max);
    box.appendChild(uplineSettingContainer);

    //// Taken Share
    var presetContainer = document.createElement("DIV");
    presetContainer.className = "bet-type-select-label";
    presetContainer.innerHTML = this.pt.Preset;
    box.appendChild(presetContainer);

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

    return box;
};

Fanex.CustSetting.BetType.prototype.Render_2_1_4 = function () {
    'use strict';
    var box = document.createElement("DIV");

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

Fanex.CustSetting.BetType.prototype.Render_4_3_4 = function () {
    'use strict';
    var box = document.createElement("DIV");

    // Bet type header
    var header = document.createElement("DIV");
    header.innerHTML = this.name;
    header.title = this.title;
    header.className = "bet-type-header";
    box.appendChild(header); // Header bet type name
    header.style.minWidth = this.minWidth + 'px';

    //// Upline Setting Lable
    var uplineSettingContainer = document.createElement("DIV");
    uplineSettingContainer.className = "bet-type-select-label";
    uplineSettingContainer.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(this.pt.Upline.MinGiven, this.pt.Upline.Max);
    box.appendChild(uplineSettingContainer);

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
Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_2_2_4 = function () {
    'use strict';
    //box.appendChild(tableShareSetting);
    // Table
    var currentPt = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    var boxTBody = document.createElement("TBODY");
    tableShareSetting.appendChild(boxTBody);

    var trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting);

    //// Upline Setting Lable
    var uplineSettingLabel = document.createElement("TD");
    uplineSettingLabel.innerHTML = Language.UPLINE_SETTING;
    uplineSettingLabel.title = Language.UPLINE_SETTING;
    uplineSettingLabel.className = "share-setting-label";
    trSetting.appendChild(uplineSettingLabel);

    var uplineSettingContainer = document.createElement("TD");
    uplineSettingContainer.className = "share-setting-value";
    uplineSettingContainer.innerHTML = Fanex.CustSetting.BuildUplineShareString(this.currentPtValue.Upline.MinGiven, this.currentPtValue.Upline.Max);
    trSetting.appendChild(uplineSettingContainer);

    trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting);

    // Share Settings
    trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting);

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
    boxTBody.appendChild(trSetting);

    var maxLabel = document.createElement("TD");
    maxLabel.innerHTML = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.title = Language.SIMPLE_MODE_MAX_POSITION;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var maxContainer = document.createElement("TD");
    maxContainer.className = "share-setting-value";
    maxContainer.innerHTML = currentPt.Max;
    trSetting.appendChild(maxContainer);

    trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting);

    var minLabel = document.createElement("TD");
    minLabel.innerHTML = Language.MIN_POSITION;
    minLabel.title = Language.MIN_POSITION;
    minLabel.className = "share-setting-label";
    trSetting.appendChild(minLabel);

    var minContainer = document.createElement("TD");
    minContainer.className = "share-setting-value";
    minContainer.innerHTML = currentPt.Min;
    trSetting.appendChild(minContainer);

    return tableShareSetting;
};

Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_4_3_4 = function () {
    'use strict';
    //box.appendChild(tableShareSetting);
    // Table
    var currentPt = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    var boxTBody = document.createElement("TBODY");
    tableShareSetting.appendChild(boxTBody);

    var trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting);
    var uplineSettingLabel = document.createElement("TD");
    uplineSettingLabel.innerHTML = Language.UPLINE_SETTING;
    uplineSettingLabel.title = Language.UPLINE_SETTING;
    uplineSettingLabel.className = "share-setting-label";
    trSetting.appendChild(uplineSettingLabel);

    var valueUplineSettingContainer = document.createElement("TD");
    valueUplineSettingContainer.className = "share-setting-value";
    valueUplineSettingContainer.innerHTML = Fanex.CustSetting.BuildUplineShareString(this.currentPtValue.Upline.MinGiven, this.currentPtValue.Upline.Max);
    trSetting.appendChild(valueUplineSettingContainer);

    trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting);
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

Fanex.CustSetting.PTGrid.prototype.RenderSimpleMode_2_1_4 = function () {
    'use strict';
    //box.appendChild(tableShareSetting);
    // Table
    var currentPt = this.currentPtValue;
    var tableShareSetting = document.createElement("TABLE");
    tableShareSetting.className = "share-setting-table";
    var boxTBody = document.createElement("TBODY");
    tableShareSetting.appendChild(boxTBody);

    var trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting);

    // on member
    trSetting = document.createElement("TR");
    boxTBody.appendChild(trSetting);
    var maxLabel = document.createElement("TD");
    maxLabel.innerHTML = Language.PERCENT_IN_MEMBER;
    maxLabel.title = Language.PERCENT_IN_MEMBER;
    maxLabel.className = "share-setting-label";
    trSetting.appendChild(maxLabel);

    var maxContainer = document.createElement("TD");
    maxContainer.className = "share-setting-value";
    maxContainer.innerHTML = currentPt.Value;
    trSetting.appendChild(maxContainer);

    return tableShareSetting;
};
