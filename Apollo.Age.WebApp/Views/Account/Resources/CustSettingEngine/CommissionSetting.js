/* File Created: February 26, 2013 */
/// <reference path="Languages/en.js" />
/// <reference path="PTSetting.js" />
/*global Language*/
/* jshint maxlen:false */
/*jshint maxstatements:false*/
'use strict';

if (typeof (Fanex) === 'undefined') { // Require Fanex namespace
    Fanex = {};
}
if (typeof (Fanex.CustSetting) === 'undefined') { // Require Fanex.CustSetting namespace
    alert('Require Fanex.CustSetting namespace');
}
Fanex.CustSetting.CommissionSetting = function (source) {
	this.spinBoxItems = [];
	this.items = source.CommissionItems;
	this.currentGroup = source.UserGroup;
	this.oldGroup = source.UserGroup;
	this.group = ["A", "B", "C", "D"];
	this.currentDisableMaxSpanText = ' [0]';
	this.groupSelect = null;
	this.Save = function () {
		var currentContext = Fanex.CustSetting.CURRENT_CONTEXT;
		if (currentContext.FormId === Fanex.CustSetting.FormTypes.EDIT_NON_DIRECT) { return {}; }

		var items = this.items;
		var spinBoxItems = this.spinBoxItems;
		var editItems = [];
		var currentOthersValue;
		var current1x2Value;

		if (currentContext.TargetRoleId !== Fanex.CustSetting.RoleType.Member) {
			var currentGroupAValue = spinBoxItems[0].getValue();
			var currentGroupBValue = spinBoxItems[3].getValue();
			var currentGroupCValue = spinBoxItems[1].getValue();
			var currentGroupDValue = spinBoxItems[4].getValue();
			if (items[0].DiscountA !== currentGroupAValue
            || items[0].DiscountB !== currentGroupBValue
            || items[0].DiscountC !== currentGroupCValue) {
				editItems.push({
					SportId: items[0].SportId,
					BetTypeId: items[0].BetTypeId,
					DiscountA: currentGroupAValue,
					DiscountB: currentGroupBValue,
					DiscountC: currentGroupCValue,
					DiscountD: currentGroupDValue
				});
			}
			currentOthersValue = spinBoxItems[5].getValue();
			if (items[1].DiscountA !== currentOthersValue) {
				editItems.push({
					SportId: items[1].SportId,
					BetTypeId: items[1].BetTypeId,
					DiscountA: currentOthersValue
				});
			}
			current1x2Value = spinBoxItems[2].getValue();
			if (items[2].DiscountA !== current1x2Value) {
				editItems.push({
					SportId: items[2].SportId,
					BetTypeId: items[2].BetTypeId,
					DiscountA: current1x2Value
				});
			}
		} else {
			if (!currentContext.CanEditComGroup) {
				this.currentGroup = undefined;
			}
			else {
				this.currentGroup = this.groupSelect.value;
			}
			var isEditMultiple = currentContext.FormId === Fanex.CustSetting.FormTypes.EDIT_MULTIPLE;
			var currentGroupValue = spinBoxItems[0].getValue();
			if (items[0].Discount !== currentGroupValue || isEditMultiple) {
				editItems.push({
					SportId: items[0].SportId,
					BetTypeId: items[0].BetTypeId,
					Discount: currentGroupValue
				});
			}
			currentOthersValue = spinBoxItems[2].getValue();
			if (items[1].Discount !== currentOthersValue || isEditMultiple) {
				editItems.push({
					SportId: items[1].SportId,
					BetTypeId: items[1].BetTypeId,
					Discount: currentOthersValue
				});
			}
			current1x2Value = spinBoxItems[1].getValue();
			if (items[2].Discount !== current1x2Value || isEditMultiple) {
				editItems.push({
					SportId: items[2].SportId,
					BetTypeId: items[2].BetTypeId,
					Discount: current1x2Value
				});
			}
		}

		if (editItems.length === 0) {
			if (typeof (this.currentGroup) !== 'undefined' && this.currentGroup === this.oldGroup.toUpperCase()) {
				this.currentGroup = undefined;
			}
		}

		return {
			CommissionItems: editItems,
			UserGroup: this.currentGroup
		};
	};
	this.Validate = function () {
		if (Fanex.CustSetting.CURRENT_CONTEXT.FormId === Fanex.CustSetting.FormTypes.EDIT_NON_DIRECT) { return true; }

		var spinBoxItems = this.spinBoxItems;

		if (Fanex.CustSetting.CURRENT_CONTEXT.TargetRoleId !== Fanex.CustSetting.RoleType.Member) {
			if (!spinBoxItems[0].isValid()) {
				return false;
			}
			if (!spinBoxItems[1].isValid()) {
				return false;
			}
			if (!spinBoxItems[2].isValid()) {
				return false;
			}
			if (!spinBoxItems[3].isValid()) {
				return false;
			}
			if (!spinBoxItems[4].isValid()) {
				return false;
			}
			if (!spinBoxItems[5].isValid()) {
				return false;
			}
		} else {
			//            if (!Fanex.CustSetting.CURRENT_CONTEXT.CanEditComGroup) {
			//                this.currentGroup = undefined;
			//            }
			//            else {
			//                this.currentGroup = this.groupSelect.value;
			//                var currentSelectedGroup = this.GetUplineGroupDiscountForMember(this.currentGroup);
			//            }

			if (!spinBoxItems[0].isValid()) {
				return false;
			}
			if (!spinBoxItems[1].isValid()) {
				return false;
			}
			if (!spinBoxItems[2].isValid()) {
				return false;
			}
		}

		return true;
	},
	this.addEventConstrain = function (spinBoxElement, uplineDiscount) {
		var currentValue = spinBoxElement.getValue();
		spinBoxElement.updateMaxValue(uplineDiscount);
		if (spinBoxElement.input) {
			spinBoxElement.input.value = currentValue;
		}
	};
	this.GetUplineGroupDiscountForMember = function (group) {
		var groupCom = this.items[0];
		var uplineDiscount;
		if (group.indexOf("a") > -1 || group.indexOf("A") > -1) {
			uplineDiscount = groupCom.Upline.DiscountA;
		}
		else if (group.indexOf("b") > -1 || group.indexOf("B") > -1) {
			uplineDiscount = groupCom.Upline.DiscountB;
		}
		else if (group.indexOf("c") > -1 || group.indexOf("C") > -1) {
			uplineDiscount = groupCom.Upline.DiscountC;
		}
		else if (group.indexOf("d") > -1 || group.indexOf("D") > -1) {
			uplineDiscount = groupCom.Upline.DiscountD;
		}

		return {
			Discount: groupCom.Discount,
			Upline: {
				Discount: uplineDiscount
			}
		};
	};
	this.RenderSpinBox = function (comContainer, discount, uplineDiscount) {
		var spinBox = Fanex.CustSetting.RenderSpinner(comContainer,
				{
					minimum: 0,
					maximum: uplineDiscount,
					decimals: Fanex.CustSetting.COMMISSION_DECIMALS,
					step: Fanex.CustSetting.COMMISSION_STEP,
					value: discount,
					minWidth: 150,
					lastLetter: Fanex.CustSetting.COM_LAST_LETTER
				}, 4);
		this.addEventConstrain(spinBox, uplineDiscount);
		return spinBox;
	};
	this.RenderAgentCom = function (trBox, textName, discount, uplineDiscount, disable) {
		var tdTextBox = document.createElement("TD");
		var divText = document.createElement("DIV");
		if (textName === Language.OTHERS_COM) {
			divText.title = textName + ' :';
		}
		divText.className = "div-commission-text";
		divText.innerHTML = textName + ' :';
		tdTextBox.appendChild(divText);
		tdTextBox.className = "commission-text";
		trBox.appendChild(tdTextBox);

		var tdValue = document.createElement("TD");
		tdValue.className = "commission-select";
		var comContainer2 = document.createElement("DIV");
		comContainer2.setAttribute("style", "float:left");
		var comContainer = document.createElement("DIV");
		comContainer.className = 'commission-spin-box';
		comContainer2.appendChild(comContainer);
		tdValue.appendChild(comContainer);
		if (Fanex.CustSetting.CURRENT_CONTEXT.FormId === Fanex.CustSetting.FormTypes.VIEW_SETTING
        || Fanex.CustSetting.CURRENT_CONTEXT.FormId === Fanex.CustSetting.FormTypes.SHADOW_VIEW) {
			comContainer.innerHTML = " " + discount;
		}
		else {
			var comSpinBox = this.RenderSpinBox(comContainer, discount, uplineDiscount);
			this.spinBoxItems.push(comSpinBox);

			var maxSpan = document.createElement("SPAN");

			maxSpan.className = "upline-agent-share";
			comContainer.appendChild(maxSpan);
			if (disable) {
				comSpinBox.input.setAttribute("disabled", "disabled");
				maxSpan.innerHTML = this.currentDisableMaxSpanText;
			}
			else {
				maxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, uplineDiscount, 2);
			}
		}

		trBox.appendChild(tdValue);
	};
	this.RenderPlayerCom = function (isEditNonDirectMember, textName, discount, uplineDiscount, disable) {
		var trBox = document.createElement("TR");
		var tdTextBox = document.createElement("TD");
		tdTextBox.innerHTML = textName;
		tdTextBox.className = "group-name";
		trBox.appendChild(tdTextBox);

		var tdValue = document.createElement("TD");
		tdValue.className = "commission-select member-com";
		if (isEditNonDirectMember) {
			tdValue.innerHTML = ' &nbsp; ' + discount;
		}
		else {
			var comContainer1 = document.createElement("DIV");
			comContainer1.className = 'commission-spin-box';
			tdValue.appendChild(comContainer1);
			var comSpinBox1 = this.RenderSpinBox(comContainer1, discount, uplineDiscount);
			this.spinBoxItems.push(comSpinBox1);
			var maxSpan = document.createElement("SPAN");
			maxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, uplineDiscount, 2);
			maxSpan.className = "upline-agent-share";
			comContainer1.appendChild(maxSpan);
			if (disable) {
				comSpinBox1.input.setAttribute("disabled", "disabled");
				maxSpan.innerHTML = this.currentDisableMaxSpanText;
			}
			else {
				maxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, uplineDiscount, 2);
			}
		}
		trBox.appendChild(tdValue);

		return trBox;
	};
	this.Render = function (owner) {
		var context = Fanex.CustSetting.CURRENT_CONTEXT;
		var container = document.createElement("DIV");
		container.className = "commission-container-header";

		var header = document.createElement("DIV");
		header.innerHTML = Language.COMMISSION_SETTING_HEADER;
		header.className = "pt-grid-header";
		var saveButton = document.createElement("SPAN");
		saveButton.className = "section-info";

		if (context.TargetRoleId === Fanex.CustSetting.RoleType.Member) {
			saveButton.id = "member-commission-info-button";
		} else if (context.TargetRoleId === Fanex.CustSetting.RoleType.MainShareHolder) {
			saveButton.id = "mainshareholder-commission-info-button";
		}
		else {
			saveButton.id = "agent-commission-info-button";
		}

		if (context.FormId === Fanex.CustSetting.FormTypes.VIEW_SETTING) {
			saveButton.id = "view-" + saveButton.id;
		}
		saveButton.id += (context.IsSimpleMode ? "" : "-adv");
		header.appendChild(saveButton);
		container.appendChild(header);

		var bodyContainer = document.createElement("DIV");
		bodyContainer.className = "commission-body-container";
		var box = document.createElement("TABLE");
		bodyContainer.appendChild(box);

		var boxTBody = document.createElement("TBODY");

		var isEditNonDirectMember = (context.FormId >= Fanex.CustSetting.FormTypes.EDIT_NON_DIRECT && context.FormId !== Fanex.CustSetting.FormTypes.EDIT_MULTIPLE);
		if (context.TargetRoleId !== Fanex.CustSetting.RoleType.Member) {
			box.className = "commission-setting-table";
			box.appendChild(boxTBody);
			var trBox = document.createElement("TR");
			var groupCom = this.items[0];
			this.RenderAgentCom(trBox, Language.GROUP_A, groupCom.DiscountA, groupCom.Upline.DiscountA);
			this.RenderAgentCom(trBox, Language.GROUP_C, groupCom.DiscountC, groupCom.Upline.DiscountC);
			var group1X2 = this.items[2];
			this.RenderAgentCom(trBox, Language._1X2, group1X2.DiscountA, group1X2.Upline.DiscountA, true);
			boxTBody.appendChild(trBox);
			trBox = document.createElement("TR");

			this.RenderAgentCom(trBox, Language.GROUP_B, groupCom.DiscountB, groupCom.Upline.DiscountB);
			this.RenderAgentCom(trBox, Language.GROUP_D, groupCom.DiscountD, groupCom.Upline.DiscountD, true);
			var groupOther = this.items[1];
			this.RenderAgentCom(trBox, Language.OTHERS_COM, groupOther.DiscountA, groupOther.Upline.DiscountA, true);

			boxTBody.appendChild(trBox);
			container.appendChild(box);
		}
		else {
			box.className = "commission-setting-table-player";

			//// Header Text
			var headerTHead = document.createElement("THEAD");
			var headerTr = document.createElement("TR");

			if (context.CanEditComGroup) {
				var tdComText = document.createElement("TH");
				tdComText.innerHTML = Language.MEMBER_GROUP;
				headerTr.appendChild(tdComText);
			}

			var tdComName = document.createElement("TH");
			tdComName.innerHTML = Language.COMM_FOR_MEMBER;
			//tdComName.setAttribute("COLSPAN", 2);
			tdComName.colSpan = 2;
			headerTr.appendChild(tdComName);
			headerTHead.appendChild(headerTr);
			box.appendChild(headerTHead);

			//// User Group
			var trGroup = document.createElement("TR");
			if (context.CanEditComGroup) {
				var tdGroupText = document.createElement("TD");
				tdGroupText.innerHTML = Language.GROUP + ':';
				tdGroupText.className = "user-group";

				tdGroupText.rowSpan = 3;

				trGroup.appendChild(tdGroupText);
				if (isEditNonDirectMember) {
					tdGroupText.innerHTML += ' ' + this.currentGroup.toUpperCase();
				}
				else {
					var groupSelect = document.createElement("SELECT");
					var groupCount = this.group.length;
					for (var groupIndex = 0; groupIndex < groupCount; groupIndex++) {
						var currentGroup = this.group[groupIndex];
						var option = document.createElement("OPTION");
						option.setAttribute("value", currentGroup);
						option.innerHTML = currentGroup;
						groupSelect.appendChild(option);
					}
					groupSelect.value = this.currentGroup.toUpperCase();
					this.groupSelect = groupSelect;
					tdGroupText.appendChild(groupSelect);
				}
			}

			var tdGroupText1 = document.createElement("TD");
			tdGroupText1.innerHTML = Language.ASIAN_HDC_OU_OE + ': &nbsp; ';
			tdGroupText1.className = "group-name";
			trGroup.appendChild(tdGroupText1);

			var currentSelectGroup = this.GetUplineGroupDiscountForMember(this.currentGroup);
			var tdMemberValue = document.createElement("TD");
			tdMemberValue.className = "commission-select member-com";
			if (isEditNonDirectMember) {
				tdMemberValue.innerHTML = ' &nbsp; ' + currentSelectGroup.Discount.toString();
			}
			else {
				var comContainer = document.createElement("DIV");
				comContainer.className = "commission-spin-box";
				tdMemberValue.appendChild(comContainer);

				var comSpinBox = this.RenderSpinBox(comContainer, currentSelectGroup.Discount, currentSelectGroup.Upline.Discount);
				this.spinBoxItems.push(comSpinBox);
				var maxSpan = document.createElement("SPAN");
				maxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, currentSelectGroup.Upline.Discount, 2);
				maxSpan.className = "upline-agent-share";
				comContainer.appendChild(maxSpan);
			}
			trGroup.appendChild(tdMemberValue);

			//// Onchange Group
			var self = this;

			if (!isEditNonDirectMember && context.CanEditComGroup) {
				this.groupSelect.onchange = function () {
					var currentValue = this.value;
					var selectedCom = self.GetUplineGroupDiscountForMember(currentValue);
					//groupText1.value = selectedCom.Upline.Discount + Fanex.CustSetting.COM_LAST_LETTER;
					if (currentValue === 'D') {
						maxSpan.innerHTML = self.currentDisableMaxSpanText;
					}
					else {
						maxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, selectedCom.Upline.Discount, 2);
					}
					self.addEventConstrain(comSpinBox, selectedCom.Upline.Discount);
				};
			}

			boxTBody.appendChild(trGroup);
			box.appendChild(boxTBody);
			//// Other Com
			boxTBody.appendChild(this.RenderPlayerCom(isEditNonDirectMember, Language._1X2 + ': &nbsp; ', this.items[2].Discount, this.items[2].Upline.DiscountA, true));
			boxTBody.appendChild(this.RenderPlayerCom(isEditNonDirectMember, Language.OTHERS_COM + ': &nbsp; ', this.items[1].Discount, this.items[1].Upline.DiscountA, true));

			container.appendChild(bodyContainer);
		}
		owner.appendChild(container);
	};
};