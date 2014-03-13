/// <reference path="PTSetting.js" />
/*global Language*/

if (typeof (Fanex) === 'undefined') { // Require Fanex namespace
    Fanex = {};
}
if (typeof (Fanex.CustSetting) === 'undefined') { // Require Fanex.CustSetting namespace
    alert('Require Fanex.CustSetting namespace');
}
// Function name format Render_[RoleId_]TargetRoleId_Form
// [RoleId_] is optional
// Call method by Method.call(this, arg1, arg2...)
Fanex.CustSetting.BetTypeGroup.prototype.Render_2_1_4 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_2_1_5 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_2_1_2 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_2_1_1 = function (owner) { // a1
    'use strict';
    // There is no decorating for this implementation
    var box = document.createElement("DIV");
    var emptyLabel = document.createElement("DIV");
    emptyLabel.innerHTML = "&nbsp;";
    emptyLabel.className = "group-label";
    box.appendChild(emptyLabel);

    var ptLabel = document.createElement("DIV");
    ptLabel.innerHTML = Language.PERCENT_IN_MEMBER;
    ptLabel.title = Language.PERCENT_IN_MEMBER;
    ptLabel.className = "group-label";
    box.appendChild(ptLabel);

    box.className = "pt-box";
    this.rowSpan = 3; // Total cells in height
    return box;
};

Fanex.CustSetting.BetTypeGroup.prototype.Render_4_2_4 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_3_2_4 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_2_2_4 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_4_2_1 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_3_2_1 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_2_2_1 = function (owner) {
    'use strict';
    var box = document.createElement("DIV");

    var emptyLabel = document.createElement("DIV");
    emptyLabel.innerHTML = "&nbsp;";
    emptyLabel.className = "group-label";
    box.appendChild(emptyLabel);

    // Upline setting label
    var uplineSettingLabel = document.createElement("DIV");
    uplineSettingLabel.innerHTML = Language.UPLINE_SETTINGTEXT;
    uplineSettingLabel.title = Language.UPLINE_SETTINGTEXT;
    uplineSettingLabel.className = "group-label";
    box.appendChild(uplineSettingLabel);

    var presetLabel = document.createElement("DIV");
    presetLabel.innerHTML = Language.PRESET_POSITION;
    presetLabel.title = Language.PRESET_POSITION;
    presetLabel.className = "group-label";
    box.appendChild(presetLabel);

    var maxLabel = document.createElement("DIV");
    maxLabel.innerHTML = Language.MAX_POSITION;
    maxLabel.title = Language.MAX_POSITION;
    maxLabel.className = "group-label";
    box.appendChild(maxLabel);

    var minLabel = document.createElement("DIV");
    minLabel.innerHTML = Language.MIN_POSITION;
    minLabel.title = Language.MIN_POSITION;
    minLabel.className = "group-label";
    box.appendChild(minLabel);

    box.className = "pt-box";
    this.rowSpan = 5;
    return box;
};

Fanex.CustSetting.BetTypeGroup.prototype.Render_2_2_3 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_3_3_3 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_4_3_4 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_4_3_1 = function (owner) {
    'use strict';
    var box = document.createElement("DIV");
    var emptyLabel = document.createElement("DIV");
    emptyLabel.innerHTML = "&nbsp;";
    emptyLabel.className = "group-label";
    box.appendChild(emptyLabel);

    // Upline setting lable
    var uplineSettingLabel = document.createElement("DIV");
    uplineSettingLabel.innerHTML = Language.UPLINE_SETTINGTEXT;
    uplineSettingLabel.title = Language.UPLINE_SETTINGTEXT;
    uplineSettingLabel.className = "group-label";
    box.appendChild(uplineSettingLabel);

    var minLabel = document.createElement("DIV");
    minLabel.innerHTML = Language.MAX_POSITION;
    minLabel.title = Language.MAX_POSITION;
    minLabel.className = "group-label";
    box.appendChild(minLabel);

    var maxLabel = document.createElement("DIV");
    maxLabel.innerHTML = Language.MIN_POSITION;
    maxLabel.title = Language.MIN_POSITION;
    maxLabel.className = "group-label";
    box.appendChild(maxLabel);

    box.className = "pt-box";
    this.rowSpan = 4;
    return box;
};
