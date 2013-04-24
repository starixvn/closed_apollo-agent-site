/* File Created: February 26, 2013 */
/// <reference path="Languages/en.js" />
/// <reference path="PTSetting.js" />

if (typeof (Fanex) == 'undefined') { // Require Fanex namespace
    Fanex = {};
}
if (typeof (Fanex.CustSetting) == 'undefined') { // Require Fanex.CustSetting namespace
    alert('Require Fanex.CustSetting namespace');
}
if (typeof (Validators) == 'undefined') { // Require Validators library
    alert('Require validators library');
}
Fanex.CustSetting.CommissionSetting = function (source, hasNumberGameSection) {
    this.hasNumberGameSection = hasNumberGameSection || false;
    this.spinBoxItems = [];
    this.items = source.CommissionItems;
    this.currentGroup = source.UserGroup;
    this.group = ["A", "B", "C", "D"];
    this.groupSelect = null;
    this.Save = function () {
        if (Fanex.CustSetting.CURRENT_CONTEXT.FormId == Fanex.CustSetting.FormTypes.EDIT_NON_DIRECT) return null;

        if (Fanex.CustSetting.CURRENT_CONTEXT.TargetRoleId != Fanex.CustSetting.RoleType.Member) {
            this.items[0].DiscountA = this.spinBoxItems[0].getValue();
            this.items[0].DiscountB = this.spinBoxItems[3].getValue();
            this.items[0].DiscountC = this.spinBoxItems[1].getValue();
            this.items[0].DiscountD = this.spinBoxItems[4].getValue();
            this.items[1].DiscountA = this.spinBoxItems[5].getValue();
            this.items[2].DiscountA = this.spinBoxItems[2].getValue();
            if (this.hasNumberGameSection) {
                this.items[3].DiscountA = this.spinBoxItems[6].getValue();
            }
        } else {
            this.items[0].Discount = this.spinBoxItems[0].getValue();
            this.items[1].Discount = this.spinBoxItems[2].getValue();
            this.items[2].Discount = this.spinBoxItems[1].getValue();
            if (this.hasNumberGameSection) {
                this.items[3].Discount = this.spinBoxItems[3].getValue();
            }
            this.currentGroup = this.groupSelect.value;
        }

        return {
            CommissionItems: this.items,
            UserGroup: this.currentGroup
        };
    };
    this.addEventConstrain = function (spinBoxElement, uplineDiscount) {
        //        spinBoxElement.input.onchange = function (event) {
        //            var currentComValue = spinBoxElement.getValue();
        //            if (currentComValue > uplineDiscount) {
        //                spinBoxElement.setValue(uplineDiscount);
        //            }
        //            else {
        //                spinBoxElement.setValue(currentComValue);
        //            }
        //        };
        spinBoxElement.updateMaxValue(uplineDiscount);
        var currentComValue = spinBoxElement.getValue();
        if (currentComValue > uplineDiscount) {
            spinBoxElement.setValue(uplineDiscount);
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
        };

        return {
            Discount: groupCom.Discount,
            Upline: {
                Discount: uplineDiscount
            }
        };
    };
    this.RenderSpinBox = function (comContainer, discount, uplineDiscount) {
        var spinBox = Fanex.CustSetting.PT_RenderSpinner(comContainer,
                {
                    minimum: 0,
                    maximum: uplineDiscount,
                    decimals: Fanex.CustSetting.COMMISSION_DECIMALS,
                    step: Fanex.CustSetting.COMMISSION_STEP,
                    value: discount,
                    minWidth: 150,
                    lastLetter: Fanex.CustSetting.COM_LAST_LETTER
                }, 7);
        this.addEventConstrain(spinBox, uplineDiscount);
        return spinBox;
    };
    this.RenderAgentCom = function (trBox, textName, discount, uplineDiscount, disable) {
        var tdTextBox = document.createElement("TD");
        tdTextBox.innerHTML = textName + ' :';
        tdTextBox.className = "commission-text";
        trBox.appendChild(tdTextBox);

        var tdValue = document.createElement("TD");
        tdValue.className = "commission-select";
        var comContainer2 = document.createElement("DIV");
        comContainer2.setAttribute("style", "float:left");
        var comContainer = document.createElement("DIV");
        comContainer.className = 'commission-spin-box';
        comContainer2.appendChild(comContainer);
        tdValue.appendChild(comContainer2);
        if (Fanex.CustSetting.CURRENT_CONTEXT.FormId == Fanex.CustSetting.FormTypes.VIEW_SETTING) {
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
                maxSpan.innerHTML = ' [0]';
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
                maxSpan.innerHTML = ' [0]';
            }
            else {
                maxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, uplineDiscount, 2);
            }
        }
        trBox.appendChild(tdValue);


        return trBox;
    };
    this.Render = function (owner) {
        var container = document.createElement("DIV");
        container.className = "commission-container-header";

        var header = document.createElement("DIV");
        header.innerHTML = Language.COMMISSION_SETTING_HEADER;
        header.className = "pt-grid-header";
        var saveButton = document.createElement("SPAN");
        saveButton.className = "save-component-setting";
        header.appendChild(saveButton);
        container.appendChild(header);

        var bodyContainer = document.createElement("DIV");
        bodyContainer.className = "commission-body-container";
        var box = document.createElement("TABLE");
        bodyContainer.appendChild(box);

        var context = Fanex.CustSetting.CURRENT_CONTEXT;
        var isEditNonDirectMember = context.FormId == Fanex.CustSetting.FormTypes.EDIT_NON_DIRECT;
        if (context.TargetRoleId != Fanex.CustSetting.RoleType.Member) {
            box.className = "commission-setting-table";
            var trBox = document.createElement("TR");
            var groupCom = this.items[0];
            this.RenderAgentCom(trBox, Language.GROUP_A, groupCom.DiscountA, groupCom.Upline.DiscountA);
            this.RenderAgentCom(trBox, Language.GROUP_C, groupCom.DiscountC, groupCom.Upline.DiscountC);
            var group1X2 = this.items[2];
            this.RenderAgentCom(trBox, Language._1X2, group1X2.DiscountA, group1X2.Upline.DiscountA, true);
            box.appendChild(trBox);
            trBox = document.createElement("TR");

            this.RenderAgentCom(trBox, Language.GROUP_B, groupCom.DiscountB, groupCom.Upline.DiscountB);
            this.RenderAgentCom(trBox, Language.GROUP_D, groupCom.DiscountD, groupCom.Upline.DiscountD, true);
            var groupOther = this.items[1];
            this.RenderAgentCom(trBox, Language.OTHERS_COM, groupOther.DiscountA, groupOther.Upline.DiscountA, true);
            if (this.hasNumberGameSection) {
                var groupNumberGame = this.items[3];
                this.RenderAgentCom(trBox, Language.NUMBER_GAME, groupNumberGame.DiscountA, groupNumberGame.Upline.DiscountA);
            }
            box.appendChild(trBox);
            container.appendChild(box);
        }
        else {
            box.className = "commission-setting-table-player";

            //// Header Text
            var headerTr = document.createElement("TR");
            var tdComName = document.createElement("TH");
            tdComName.innerHTML = Language.MEMBER_GROUP;
            headerTr.appendChild(tdComName);
            //            tdComName = document.createElement("TH");
            //            headerTr.appendChild(tdComName);
            //            tdComName = document.createElement("TH");
            //            tdComName.innerHTML = Language.COMM_FOR_AGENT;
            //            headerTr.appendChild(tdComName);
            tdComName = document.createElement("TH");
            tdComName.innerHTML = Language.COMM_FOR_MEMBER;
            tdComName.setAttribute("colspan", 2);
            headerTr.appendChild(tdComName);
            box.appendChild(headerTr);

            //// User Group
            var trGroup = document.createElement("TR");
            var tdGroupText = document.createElement("TD");
            tdGroupText.innerHTML = Language.GROUP;
            tdGroupText.className = "user-group";
            if (this.hasNumberGameSection) {
                tdGroupText.setAttribute("rowspan", 4);
            }
            else {
                tdGroupText.setAttribute("rowspan", 3);
            }
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

            var tdGroupText1 = document.createElement("TD");
            tdGroupText1.innerHTML = Language.ASIAN_HDC_OU_OE + ' &nbsp; ';
            tdGroupText1.className = "group-name";
            trGroup.appendChild(tdGroupText1);

            //            var tdGroupValue2 = document.createElement("TD");
            //            tdGroupValue2.className = "agent-com";
            //            var groupText1 = document.createElement("INPUT");
            //            groupText1.type = "Text";
            //            groupText1.disabled = true;
            //            groupText1.value = this.GetUplineGroupDiscountForMember(this.currentGroup).Upline.Discount + Fanex.CustSetting.COM_LAST_LETTER;
            //            tdGroupValue2.appendChild(groupText1);
            //            trGroup.appendChild(tdGroupValue2);

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
            var saveItems = this.items;
            if (!isEditNonDirectMember) {
                groupSelect.onchange = function (event) {
                    var currentValue = this.value;
                    var selectedCom = self.GetUplineGroupDiscountForMember(currentValue);
                    //groupText1.value = selectedCom.Upline.Discount + Fanex.CustSetting.COM_LAST_LETTER;
                    maxSpan.innerHTML = Fanex.CustSetting.BuildMinMaxShareString(0, selectedCom.Upline.Discount, 2);

                    self.addEventConstrain(comSpinBox, selectedCom.Upline.Discount);
                };
            }


            box.appendChild(trGroup);

            //// Other Com
            box.appendChild(this.RenderPlayerCom(isEditNonDirectMember, Language._1X2 + ' &nbsp; ', this.items[2].Discount, this.items[2].Upline.DiscountA, true));
            box.appendChild(this.RenderPlayerCom(isEditNonDirectMember, Language.OTHERS_COM + ' &nbsp; ', this.items[1].Discount, this.items[1].Upline.DiscountA, true));
            if (hasNumberGameSection) {
                box.appendChild(this.RenderPlayerCom(isEditNonDirectMember, Language.NUMBER_GAME + ' &nbsp; ', this.items[3].Discount, this.items[3].Upline.DiscountA));
            }

            container.appendChild(bodyContainer);
        }
        owner.appendChild(container);
    }
};