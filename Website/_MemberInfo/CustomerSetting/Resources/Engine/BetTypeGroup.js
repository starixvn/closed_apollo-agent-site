/// <reference path="PTSetting.js" />

if (typeof (Fanex) == 'undefined') { // Require Fanex namespace
	Fanex = {};
}
if (typeof (Fanex.CustSetting) == 'undefined') { // Require Fanex.CustSetting namespace
	alert('Require Fanex.CustSetting namespace');
}
// Function name format Render_[RoleId_]TargetRoleId_Form
// [RoleId_] is optional
// Call method by Method.call(this, arg1, arg2...)
Fanex.CustSetting.BetTypeGroup.prototype.Render_5_4_1 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_5_3_1 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_5_1_1 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_4_3_1 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_4_1_1 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_2_1_1 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_1_1_1 = function (owner) { // a1
    // There is no decorating for this implementation
    var box = document.createElement("DIV");
    var emptyLabel = document.createElement("DIV");
    emptyLabel.innerHTML = "&nbsp;";
    emptyLabel.className = "group-label";
    box.appendChild(emptyLabel);

    //// Set text before each bettype
    var agentShareLabel = document.createElement("DIV");
    switch (Fanex.CustSetting.CURRENT_CONTEXT.TargetRoleId) {
        case Fanex.CustSetting.RoleType.MainShareHolder:
            agentShareLabel.innerHTML = Language.MAIN_SHARE_HOLDER_SHARE;
            agentShareLabel.title = Language.MAIN_SHARE_HOLDER_SHARE;
            break;
        case Fanex.CustSetting.RoleType.ShareHolder:
            agentShareLabel.innerHTML = Language.SHARE_HOLDER_SHARE;
            agentShareLabel.title = Language.SHARE_HOLDER_SHARE;
            break;
        case Fanex.CustSetting.RoleType.Agent:
            agentShareLabel.innerHTML = Language.AGENT_SHARE;
            agentShareLabel.title = Language.AGENT_SHARE;
            break;
    }
    agentShareLabel.className = "group-label";
    box.appendChild(agentShareLabel);

    box.className = "pt-box";
    this.rowSpan = 3; // Total cells in height
    return box;
};

Fanex.CustSetting.BetTypeGroup.prototype.Render_5_2_1 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_4_2_1 =
Fanex.CustSetting.BetTypeGroup.prototype.Render_3_2_1 = function (owner) { 
    var box = document.createElement("DIV");
    var emptyLabel = document.createElement("DIV");
    emptyLabel.innerHTML = "&nbsp;";
    emptyLabel.className = "group-label";
    box.appendChild(emptyLabel);

    var agentShareLabel = document.createElement("DIV");
    agentShareLabel.innerHTML = Language.SHARE_HOLDER_SHARE;
    agentShareLabel.title = Language.SHARE_HOLDER_SHARE;
    agentShareLabel.className = "group-label";
    box.appendChild(agentShareLabel);

    var maxLabel = document.createElement("DIV");
    maxLabel.innerHTML = Language.MAIN_AGENT_SHARE;
    maxLabel.title = Language.MAIN_AGENT_SHARE;
    maxLabel.className = "group-label";
    box.appendChild(maxLabel);

    box.className = "pt-box";
    this.rowSpan = 4;
    return box;
};

