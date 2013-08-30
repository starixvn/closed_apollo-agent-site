if (typeof (Fanex) == 'undefined') { // Require Fanex namespace
    Fanex = {};
}
if (typeof (Fanex.CustSetting) == 'undefined') { // Define Fanex.CustSetting namespace
    Fanex.CustSetting = {
        RoleType: {
            Super: 4,
            MainShareHolder: 3,
            Agent: 2,
            Member: 1
        },
        FormTypes: {
            ADD: 0,
            EDIT_PT: 1,
            EDIT_NON_DIRECT: 2,
            VIEW_SETTING: 3,
            SHADOW_VIEW: 4,
            EDIT_MULTIPLE: 5
        },
        PT_STEP10: 1,
        PT_STEP100: 10,
        PT_STEP: 0.01,
        PT_DECIMALS: 2,
        PT_MAX_LENGTH: 5,
        COMMISSION_STEP: 0.01,
        COMMISSION_DECIMALS: 2,
        MIN_PT_BOX_WIDTH: 144,
        SPINNER_BOX_WIDTH: 45,
        PT_LAST_LETTER: '',
        COM_LAST_LETTER: '',
        MIN_CONSTRAIN_VALUE: 50,
        Min_WIDTH_BET_TYPE_GROUP: 140,
        DisableChildren: function (element, cssApplied) { // Check dublicated disable
            if (typeof (element) != 'object') return false;
            var count = element.childNodes.length;
            if (typeof (count) == 'undefined') return false;
            for (var i = 0; i < count; i++) {
                if (element.childNodes[i].tagName == "INPUT" || element.childNodes[i].tagName == "SELECT") {
                    var previousValue = element.childNodes[i].disabled;
                    if (previousValue != true) {
                        element.childNodes[i].disabled = true;
                        element.childNodes[i].setAttribute("previousValue", previousValue);
                    };
                } else {
                    if (typeof (cssApplied) == 'string' && element.childNodes[i].tagName == "DIV") {
                        var addedCss = " " + cssApplied;
                        if (element.childNodes[i].className.indexOf(addedCss) == -1) { // Avoid dublicated adding
                            element.childNodes[i].setAttribute("previousValue", element.childNodes[i].className);
                            element.childNodes[i].className += " " + cssApplied;
                        };
                    };
                    Fanex.CustSetting.DisableChildren(element.childNodes[i]);
                }
            }
            return true;
        },
        ReEnableChildren: function (element) { // Check dublicated enable
            if (typeof (element) != 'object') return false;
            var count = element.childNodes.length;
            if (typeof (count) == 'undefined') return false;
            for (var i = 0; i < count; i++) {
                if (element.childNodes[i].tagName == "INPUT" || element.childNodes[i].tagName == "SELECT") {
                    var previousValue = element.childNodes[i].getAttribute("previousValue");
                    if (previousValue != null && typeof (previousValue) != 'undefined') {
                        element.childNodes[i].setAttribute("previousValue", element.childNodes[i].disabled); // Store previous value
                        element.childNodes[i].disabled = (previousValue == "true");
                    }
                } else {
                    if (element.childNodes[i].tagName == "DIV") {
                        var previousValue = element.childNodes[i].getAttribute("previousValue");
                        if (previousValue != null && typeof (previousValue) != 'undefined') {
                            element.childNodes[i].setAttribute("previousValue", element.childNodes[i].className);
                            element.childNodes[i].className = previousValue;
                        };
                    }
                    Fanex.CustSetting.ReEnableChildren(element.childNodes[i]);
                }
            }
            return true;
        },
        BuildMinMaxShareString: function (minShare, maxShare, digits) {
            if (maxShare < minShare) {
                maxShare = minShare;
            }
            digits = digits || 2;
            return ' [' + Fanex.RoundNumber(minShare, digits) + ' - ' + Fanex.RoundNumber(maxShare, digits) + ']';
        },
        BuildUplineShareString: function (minShare, maxShare, digits) {
            if (maxShare < minShare) {
                maxShare = minShare;
            }
            digits = digits || 2;
            return ' ' + Language.MIN + ' = ' + Fanex.RoundNumber(minShare, digits) + ' || ' + Language.MAX + ' = ' + Fanex.RoundNumber(maxShare, digits);
        },
        PT_RenderSpinner: function (spinBoxElement, spinOptions, maxLength) {
            spinBoxElement.style.width = spinOptions.minWidth + 'px';
            var minInput = document.createElement("INPUT");
            minInput.type = "text";
            minInput.maxLength = maxLength;
            minInput.style.width = Fanex.CustSetting.SPINNER_BOX_WIDTH + "px";
            spinBoxElement.appendChild(minInput);

            var spinBox = new Spinner(spinBoxElement, spinOptions);
            return spinBox;
        },
        CalculateMinWidth: function (text) {
            if (typeof (text) != 'string') return Fanex.CustSetting.MIN_PT_BOX_WIDTH;
            var measure = 6.7; // Default for en-US
            if (Fanex.CustSetting.CURRENT_CONTEXT.Language == 'th-TH') {
                measure = 4.5;
            }
            else if (Fanex.CustSetting.CURRENT_CONTEXT.Language == 'zh-TW'
			|| Fanex.CustSetting.CURRENT_CONTEXT.Language == 'zh-CN'
			|| Fanex.CustSetting.CURRENT_CONTEXT.Language == 'ko-KR') {
                measure = 12;
            }
            return Math.round(text.length * measure);
        },
        SizingChildren: function (element) { // For element has each child as no-child no-float div
            if (typeof (element) != 'object') return false;
            var count = element.childNodes.length;
            if (typeof (count) == 'undefined') return false;
            // Calculate max min-width
            var maxLength = 0;
            var text = String.Empty;
            for (var i = 0; i < count; i++) {
                var length = element.childNodes[i].innerHTML.length;
                if (length > maxLength) {
                    maxLength = length;
                    text = element.childNodes[i].innerHTML;
                }
            }
            // Set children min-width
            var minWidth = Fanex.CustSetting.Min_WIDTH_BET_TYPE_GROUP;
            for (var i = 0; i < count; i++) {
                element.childNodes[i].style.minWidth = minWidth + 'px';
            }
            return minWidth;
        },
        Context: function (currentContext) {
            this.RoleId = currentContext.RoleId;
            this.TargetRoleId = currentContext.TargetRoleId;
            this.FormId = currentContext.FormId;
            this.Language = currentContext.Language;
            this.IsSimpleMode = currentContext.IsSimpleMode;
            this.HasMinConstrain = currentContext.HasMinConstrain;
            this.CanEditComGroup = currentContext.CanEditComGroup;
            if (this.Language == 'cn-CN') {
                Language = "new language";
            }
        },
        CURRENT_CONTEXT: {
            RoleId: 2,
            TargetRoleId: 2,
            FormId: 1,
            Language: 'en-US',
            IsSimpleMode: true,
            HasMinConstrain: false,
            CanEditComGroup: false
        },
        // BetType class, need to create each bet type class for each PT form
        BetType: function (item) {
            this.name = item.Name; // Handicap, Over / Under
            this.title = item.Name; // Title  Handicap, Over / Under
            this.id = item.Id; // Bet type id
            this.sportId = item.SportId;
            this.showButtons = false; // Show 4 buttons (increase, decrease) after selection
            this.minWidth = Fanex.CustSetting.CalculateMinWidth(this.name);
            this.minWidth = Math.max(this.minWidth, Fanex.CustSetting.MIN_PT_BOX_WIDTH);
            if (typeof (item.MinWidth) == 'number') {
                this.minWidth = Math.max(item.MinWidth, this.minWidth);
            }
            this.pt = {
                Upline:
				{
				    Max: 0,
				    Min: 0,
				    Preset: 0,
				    PositionTaking: 0
				},
                Min: 0,
                Max: 0,
                Preset: 0,
                PositionTaking: 0
            };
            if (typeof (item.Pt) == 'object') {
                this.pt = item.Pt;
                this.oldPt = Fanex.CloneObject(item.Pt);
            }
            this.isLive = item.IsLive; // Live! or Deadball
            this.suffixId = this.sportId + "$" + this.id + "$" + (this.isLive ? "1" : "0");
            this.context = Fanex.CloneObject(Fanex.CustSetting.CURRENT_CONTEXT);
            // Will be removed
            if (this.context.FormId == Fanex.CustSetting.FormTypes.ADD) { // Use the same implementation for Edit PT / Add New
                this.context.FormId = Fanex.CustSetting.FormTypes.EDIT_PT;
            }
            this.element = document.createElement("DIV");
            this.actived = true;
            this.Active = function () {
                if (this.actived) return;
                Fanex.CustSetting.ReEnableChildren(this.element);
                this.actived = true;
            };
            this.Inactive = function (copiedItem) {
                if (!this.actived) return;
                Fanex.CustSetting.DisableChildren(this.element, "bet-type-inactive");
                this.actived = false;
            };
            this.Render = function (owner) { // Render each bet type section
                // Just edit member form require roldId in implementation
                var formId = this.context.FormId;
                var roleId = this.context.RoleId; //(this.context.TargetRoleId == 1 || formId != Fanex.CustSetting.FormTypes.EDIT_PT) ? this.context.RoleId : 0;
                var targetRoleId = this.context.TargetRoleId;
                var box = null;
                var method = this['Render_'.concat(roleId, '_', targetRoleId, '_', formId)];
                if (typeof (method) == "function") {
                    box = method.call(this, owner);
                } else {
                    console.log("Could not find the implementation of " + methodName);
                }
                if (box != null) {
                    this.element = box;
                    // Append or return the object
                    if (typeof (owner) == 'object') {
                        owner.appendChild(box);
                    };
                };
                return box;
            };
        },
        // BetTypeGroup, could have "Copy From " check box
        BetTypeGroup: function (betTypeGroup) { // Each group normaly contains 4 or 5 bet types
            this.id = betTypeGroup.Id || null; // Bet type group could omit id property
            this.betTypes = betTypeGroup.BetTypes;
            this.elements = []; // Child bet type HTML elements
            this.items = []; // Bet type objects
            this.showGroupLabel = true; // Master Preset, Super Preset...
            this.isLive = betTypeGroup.IsLive;
            this.sportId = betTypeGroup.SportId;
            this.context = Fanex.CloneObject(Fanex.CustSetting.CURRENT_CONTEXT);
            if (this.context.FormId == Fanex.CustSetting.FormTypes.ADD) { // Use the same implementation for Edit PT / Add New
                this.context.FormId = Fanex.CustSetting.FormTypes.EDIT_PT;
            }
            this.copiedItem = null; // Store bet type item copied to other bet types
            this.copiedFromFirst = false;
            // Find an item {sportId, id, isLive}
            this.Find = function (object) {
                if (typeof (object.id) != 'number') return null; // Bet type id is not a number, return null
                var items = this.items;
                var count = this.items.length;
                for (var i = 0; i < count; i++) {
                    var item = items[i];
                    if (item.isLive == object.isLive && item.id == object.id && items[i].sportId == object.sportId) {
                        return item; // Return found bet type
                    };
                };
                return null;
            };
            this.Active = function () {
                var count = this.elements.length;
                this.copiedItem = null;
                for (var i = 0; i < count; i++) {
                    this.items[i].Active();
                    if (count > 0 && this.copiedFromFirst) {
                        this.copiedItem = {
                            sportId: this.items[0].sportId,
                            id: this.items[0].id,
                            isLive: this.items[0].isLive
                        };
                        break;
                    }
                };
            };
            this.Inactive = function (copiedItem) {
                if (typeof (copiedItem) != 'object') {
                    return;
                };
                this.copiedItem = copiedItem;
                var count = this.elements.length;
                for (var i = 0; i < count; i++) {
                    if (this.items[i].id == copiedItem.id && this.isLive == copiedItem.isLive && this.sportId == copiedItem.sportId) {
                        this.items[i].Active(); // Activate if first item matching copied item
                        continue;
                    };
                    this.items[i].Inactive(copiedItem);
                }
            };
            this.Save = function () { // Get data for each bet types
                var count = this.items.length;
                var result = [];
                var copiedItem = (this.copiedItem == null) ? null : Fanex.CloneObject(this.copiedItem); // Avoid changing copiedItem by reference
                // Find PT of copied item
                var grid = this.parent.parent; // Parent grid object
                var item = grid.Find(copiedItem);
                if (item != null && item instanceof Fanex.CustSetting.BetType) {
                    item.Save();
                    copiedItem.pt = Fanex.CloneObject(item.pt);
                    copiedItem.sportId = this.sportId; // Do not copy sport type id
                }
                for (var i = 0; i < count; i++) {
                    if (copiedItem != null) {
                        result.push(
						{
						    Id: this.items[i].id, // Do not copy bet type id
						    SportId: copiedItem.sportId || null,
						    Pt: copiedItem.pt || null,
						    IsLive: copiedItem.isLive
						});
                    } else {
                        var ptResult = this.items[i].Save();
                        if (ptResult) {
                            result.push(Fanex.CloneObject(ptResult));
                        }
                    };
                };
                return {
                    Id: this.id,
                    BetTypes: result
                };
            };

            this.Validate = function () {
                var items = this.items;
                var count = items.length;
                for (var i = 0; i < count; i++) {
                    if (!items[i].Validate()) {
                        return false;
                    }
                };
                return true;
            };

            this.Render = function (owner) {
                var box = null;
                var elements = []; // Child bet type element
                if (typeof (this.betTypes) != 'object' || typeof (this.betTypes.length) == 'undefined' || this.betTypes.length <= 0) {
                    console.log("Input data source betTypes is empty.");
                } else {
                    box = document.createElement("DIV")
                    var totalGroupWidth = 0;
                    // Header label
                    if (this.showGroupLabel) {
                        var header = null;
                        var roleId = this.context.RoleId; //(this.context.TargetRoleId == 1) ? this.context.RoleId : 0; // Just edit member form require roldId in implementation
                        var targetRoleId = this.context.TargetRoleId;
                        var formId = this.context.FormId;
                        var method = this['Render_'.concat(roleId, '_', targetRoleId, '_', formId)];
                        if (typeof (method) == "function") {
                            header = method.call(this, owner);
                        } else {
                            console.log("Could not find the implementation of " + methodName);
                        }
                        if (header != null) {
                            totalGroupWidth = Fanex.CustSetting.SizingChildren(header) + 4; // Add constant 4 as padding left, padding right
                            box.appendChild(header);
                        }
                    }
                    // Each bet types
                    var count = this.betTypes.length;
                    for (var i = 0; i < count; i++) {
                        var item = this.betTypes[i];
                        item.SportId = this.sportId;
                        item.IsLive = this.isLive;
                        var betItem = new Fanex.CustSetting.BetType(item);
                        betItem.parent = this; // Keep reference to current bet type group
                        betItem.showButtons = (i == 0);
                        //betItem.minWidth = (betItem.showButtons) ? (140) : betItem.minWidth;
                        //betItem.minWidth = (count == 1) ? (250) : betItem.minWidth;
                        totalGroupWidth += betItem.minWidth + 5; // Padding left 2, padding right 2 and border 1
                        var element = betItem.Render(box);
                        if (element != null) {
                            elements[i] = element;
                            this.items[i] = betItem;
                        }
                    }
                    this.elements = elements;
                    box.betTypeElements = elements;
                };
                if (this.elements.length == 0) {
                    box = null;
                };
                if (box != null) {
                    if (this.elements.length > 1) {
                        var elements = this.elements;
                        if (Fanex.CustSetting.CURRENT_CONTEXT.FormId != Fanex.CustSetting.FormTypes.VIEW_SETTING
						&& Fanex.CustSetting.CURRENT_CONTEXT.FormId != Fanex.CustSetting.FormTypes.SHADOW_VIEW) {
                            // Copy all check box
                            var copyCheck = document.createElement("INPUT");
                            copyCheck.type = "checkbox";
                            copyCheck.id = copyCheck.name = "c$" + this.betTypes[0].Id + "$" + this.betTypes[0].SportId + "$" + (this.isLive ? "0" : "1");
                            var copyDiv = document.createElement("DIV");
                            copyDiv.appendChild(copyCheck);
                            copyDiv.className = "pt-check-copied";
                            this.elements[0].firstChild.title = Language.COPY_FROM.Format(this.betTypes[0].Name);
                            this.elements[0].firstChild.className = "bet-type-header2";
                            this.elements[0].firstChild.appendChild(copyDiv);
                            copyCheck.label = this.elements[0].firstChild;
                            copyCheck.parent = this;
                            var items = this.items;
                            copyCheck.label.onclick = function () {
                                var currentCopyBetType = elements[0];
                                if (currentCopyBetType.presetSpinBox && !currentCopyBetType.presetSpinBox.isValid()) {
                                    return;
                                }
                                if (currentCopyBetType.maxSpinBox && !currentCopyBetType.maxSpinBox.isValid()) {
                                    return;
                                }
                                if (currentCopyBetType.minSpinBox && !currentCopyBetType.minSpinBox.isValid()) {
                                    return;
                                }
                                if (currentCopyBetType.ptSpinBox && !currentCopyBetType.ptSpinBox.isValid()) {
                                    return;
                                }
                                var betTypes = elements;
                                var betTypesLength = betTypes.length;
                                for (var betTypeIndex = 0; betTypeIndex < betTypesLength; betTypeIndex++) {
                                    var betType = betTypes[betTypeIndex];
                                    if (betType.id != currentCopyBetType.id) {
                                        if (betType.presetSpinBox) {
                                            betType.presetSpinBox.setValue(currentCopyBetType.presetSpinBox.getValue(), true);
                                            betType.presetSpinBox.input.onchange();
                                            betType.presetOnMember.checked = currentCopyBetType.presetOnMember.checked;
                                        }
                                        if (betType.maxSpinBox) {
                                            betType.maxSpinBox.setValue(currentCopyBetType.maxSpinBox.getValue(), true);
                                            betType.maxSpinBox.input.onchange();
                                        }
                                        if (betType.minSpinBox) {
                                            betType.minSpinBox.setValue(currentCopyBetType.minSpinBox.getValue(), true);
                                            betType.minSpinBox.input.onchange();
                                        }
                                        if (betType.ptSpinBox) {
                                            betType.ptSpinBox.setValue(currentCopyBetType.ptSpinBox.getValue(), true);
                                            betType.ptSpinBox.input.onchange();
                                        }

                                    }
                                }
                                clearInterval(Validators.intervalAnimate);
                                Validators.hide();
                                //};
                            };
                        };
                    };
                    this.minWidth = totalGroupWidth;
                    box.className = "pt-group";
                    this.DOM = box;
                    // Append or return the object
                    if (typeof (owner) == 'object') {
                        owner.appendChild(box);
                    };
                };
                return box;
            };
        },
        SportType: function (sportType) {
            if (typeof (sportType) != 'object') {
                console.log("Input sport type is an invalid object.");
                return null;
            }
            this.id = sportType.Id; // Sport type id
            this.name = sportType.Name; // Sport type name as Soccer, Handicap
            this.key = sportType.Key; // Sport type key as Soccer, Handicap
            this.minWidth = 0; // Init minWidth
            this.betTypeGroups = sportType.BetTypeGroups;
            this.isLive = sportType.IsLive;
            this.elements = []; // Bet type group set HTML elements
            this.items = []; // Bet Type group set objects
            this.cloneObjectButton = null; // Copy all button for Soccer and Basketball
            this.copiedItem = null;
            this.collapsed = typeof (sportType.Collapsed) == 'undefined' ? true : sportType.Collapsed; // Default collapse or not
            // Find an item {sportId, id, isLive}
            this.Find = function (object) {
                if (typeof (object.id) != 'number') return this; // Bet type id is not a number, return sport type
                var items = this.items;
                var count = this.items.length;
                for (var i = 0; i < count; i++) {
                    if (object.sportId == this.id) {
                        var item = items[i].Find(object);
                        if (item != null) return item;
                    }
                };
                return null;
            };
            this.Active = function () {
                this.copiedItem = null;
                var count = this.items.length;
                for (var i = 0; i < count; i++) {
                    this.items[i].Active();
                }
            };
            this.Inactive = function (copiedItem) {
                if (typeof (copiedItem) != 'object') {
                    return;
                };
                this.copiedItem = copiedItem;
                var count = this.items.length;
                for (var i = 0; i < count; i++) {
                    this.items[i].Inactive(copiedItem);
                }
            };
            this.Save = function () {
                var count = this.items.length;
                var result = [];
                for (var i = 0; i < count; i++) {
                    result[i] = this.items[i].Save();
                };
                return {
                    Id: this.id,
                    Key: this.key,
                    IsLive: this.isLive,
                    BetTypeGroups: result
                };
            };

            this.Validate = function () {
                var items = this.items;
                var count = items.length;
                for (var i = 0; i < count; i++) {
                    if (!items[i].Validate()) {
                        return false;
                    }
                };
                return true;
            };

            this.Render = function (owner) {
                var box = null;
                var elements = []; // Child bet type group sets element
                if (typeof (this.betTypeGroups) != 'object' || typeof (this.betTypeGroups.length) == 'undefined' || this.betTypeGroups.length <= 0) {
                    console.log("Input data source betTypeGroups is empty.");
                } else {
                    box = document.createElement("DIV");
                    var box2 = document.createElement("DIV");
                    // Each bet type group sets
                    var count = this.betTypeGroups.length;
                    for (var i = 0; i < count; i++) {
                        var item = this.betTypeGroups[i];
                        item.SportId = this.id;
                        item.IsLive = this.isLive;
                        var groupItem = new Fanex.CustSetting.BetTypeGroup(item);
                        groupItem.parent = this; // Keep reference to current sport type
                        var element = groupItem.Render(box2);
                        if (element != null) {
                            elements[i] = element;
                            this.items[i] = groupItem;
                        }
                        this.minWidth = Math.max(groupItem.minWidth, this.minWidth);
                    }
                    box2.className = "pt-groupset2";
                    this.elements = elements;
                    box.betTypeGroupElements = elements;
                    // Add sport type name label
                    var headerLabel = this.name;
                    if (this.id == 1 || this.id == 161) {
                        headerLabel += ' - ' + (this.isLive ? Language.LIVE : Language.DEAD_BALL);
                    }
                    var header = document.createElement("DIV");
                    header.innerHTML = headerLabel;
                    header.className = (this.id == 2) ? "pt-sport-label2" : "pt-sport-label"; // Basketball sport id == 2 has 2 buttons
                    header.parent = this;
                    box.appendChild(header);
                    if (Fanex.CustSetting.CURRENT_CONTEXT.FormId != Fanex.CustSetting.FormTypes.VIEW_SETTING
					&& Fanex.CustSetting.CURRENT_CONTEXT.FormId != Fanex.CustSetting.FormTypes.SHADOW_VIEW) {
                        // Copy from sport
                        var copyCheck = document.createElement("INPUT");
                        copyCheck.type = "checkbox";
                        copyCheck.id = copyCheck.name = "cp$" + this.id; // Copy from sport check box
                        copyCheck.className = "pt-check-copied";
                        var copyDiv = document.createElement("DIV");
                        copyDiv.appendChild(copyCheck);
                        copyDiv.parent = this;
                        var minWidth = (this.id == 2) ? (this.minWidth - 72) : this.minWidth - 40; // Basketball sport id == 2 has 2 buttons
                        if ((this.id == 1 && this.isLive == false) || this.id == 2) { // For Soccer and Basketball
                            copyDiv.className = "pt-sport-copied";
                            copyDiv.title = Language.COPY_ALL_OTHER_POSITION_TAKING_FROM.Format(this.name, this.betTypeGroups[0].BetTypes[0].Name);
                            if (this.id == 2) {
                                // Add more Edit button for Basketball, because Basketball also have Copy button
                                var copyCheck2 = document.createElement("INPUT");
                                copyCheck2.type = "checkbox";
                                copyCheck2.id = copyCheck2.name = "c2p$" + this.id; // Copy from sport check box
                                copyCheck2.className = "pt-check-copied";
                                var editDiv = document.createElement("DIV");
                                editDiv.appendChild(copyCheck2);
                                editDiv.parent = this;
                                editDiv.className = "pt-sport-copied-inactive";
                                editDiv.onclick = function () {
                                }
                                this.editButton = editDiv; // Keep reference for implementation in parent level
                                box.appendChild(editDiv);
                            }
                        } else {
                            copyDiv.className = "pt-sport-copied-inactive";
                            copyDiv.onclick = function () {
                            };
                        };
                        // Handle default collapse / expand
                        if (this.collapsed) {
                            box2.style.display = "none"; // box2 contains PT setting
                        } else {
                            header.style.minWidth = minWidth + "px";
                        }
                        this.cloneObjectButton = copyDiv; // Keep reference for implementation in parent level
                        copyDiv.appendChild(copyCheck);
                        box.appendChild(copyDiv);
                    }
                    // Event click to collapse sportType
                    this.Toggle = function (expanded) {
                        var collapsed = false;
                        if (typeof (expanded) != "undefined") {
                            collapsed = !expanded;
                        } else {
                            collapsed = (box2.style.display != "none");
                        };
                        if (!collapsed) {
                            box2.style.display = String.Empty;
                            header.style.minWidth = minWidth + "px";
                            this.collapsed = false;
                        } else {
                            box2.style.display = "none";
                            header.style.minWidth = String.Empty;
                        }
                        this.collapsed = collapsed;
                    };
                    header.onclick = function () {
                        header.parent.Toggle();
                    };
                    box.appendChild(box2);
                }
                if (this.elements.length == 0) {
                    box = null;
                }
                if (box != null) {
                    box.className = "pt-sport";
                    this.DOM = box;
                    // Append or return the object
                    if (typeof (owner) == 'object') {
                        owner.appendChild(box);
                    }
                }
                return box;
            }
        },
        PTGrid: function (source) {
            this.elements = []; // Sport type HTML elements
            this.items = []; // Sport type objects
            this.sportTypes = [];
            if (typeof (source) == 'object') {
                this.name = source.Name;
                this.sportTypes = source.SportTypes;
            }
            // Find an item {sportId, id, isLive}
            this.Find = function (object) {
                if (typeof (object) != 'object' || object == null) return null;
                if (typeof (object.sportId) != 'number') return null;
                var items = this.items;
                var count = this.items.length;
                for (var i = 0; i < count; i++) {
                    if (items[i].id == object.sportId) {
                        var item = items[i].Find(object);
                        if (item != null) return item;
                    };
                };
                return null;
            };
            this.Save = function () {
                var result = [];
                var currentContext = Fanex.CustSetting.CURRENT_CONTEXT;
                var isEditMultiple = currentContext.FormId == Fanex.CustSetting.FormTypes.EDIT_MULTIPLE;
                if (currentContext.IsSimpleMode == true) {
                    var currentPtValue = this.SaveSimpleMode();
                    var dataChanged = false;
                    if (currentPtValue == null) return null;
                    var sportTypes = Fanex.CloneObject(this.sportTypes);
                    var sportTypeLength = sportTypes.length;
                    for (var sportTypeIndex = 0; sportTypeIndex < sportTypeLength; sportTypeIndex++) {
                        var currentBetTypeGroups = sportTypes[sportTypeIndex].BetTypeGroups;
                        var betTypeGroupLength = currentBetTypeGroups.length;
                        for (var btGroupIndex = 0; btGroupIndex < betTypeGroupLength; btGroupIndex++) {
                            var currentBetTypes = currentBetTypeGroups[btGroupIndex].BetTypes;
                            var betTypeLength = currentBetTypes.length;
                            for (var btIndex = 0; btIndex < betTypeLength; btIndex++) {
                                var currentBetType = sportTypes[sportTypeIndex].BetTypeGroups[btGroupIndex].BetTypes[btIndex];
                                dataChanged = false;

                                if (currentContext.TargetRoleId == Fanex.CustSetting.RoleType.MainShareHolder
                                    && currentBetType.Pt.Max != currentPtValue.Max) {
                                    dataChanged = true;
                                    currentPtValue.Min = currentBetType.Pt.Min;
                                }

                                if (currentContext.TargetRoleId == Fanex.CustSetting.RoleType.Agent
								&& (currentBetType.Pt.Max != currentPtValue.Max
								|| currentBetType.Pt.Preset != currentPtValue.Preset
                                || currentBetType.Pt.Min != currentPtValue.Min)) {
                                    dataChanged = true;
                                    //currentPtValue.Min = currentBetType.Pt.Min;
                                }

                                if (currentContext.TargetRoleId == Fanex.CustSetting.RoleType.Member
								&& currentBetType.Pt.Value != currentPtValue.Value) {
                                    dataChanged = true;
                                }

                                if (dataChanged == true || isEditMultiple || currentPtValue.IsApplyMember) {
                                    currentBetType.Pt = Fanex.CloneObject(currentPtValue);
                                }
                                else {
                                    currentBetType.Pt = null;
                                }
                            }
                        }
                    }

                    result = sportTypes;
                }
                else {
                    var count = this.items.length;
                    for (var i = 0; i < count; i++) {
                        result[i] = this.items[i].Save();
                    };
                }
                return {
                    SportTypes: result
                };
            };

            this.CheckChangeData = function (sportTypes) {
                var sportTypeLength = sportTypes.length;
                for (var sportTypeIndex = 0; sportTypeIndex < sportTypeLength; sportTypeIndex++) {
                    var currentBetTypeGroups = sportTypes[sportTypeIndex].BetTypeGroups;
                    var betTypeGroupLength = currentBetTypeGroups.length;
                    for (var btGroupIndex = 0; btGroupIndex < betTypeGroupLength; btGroupIndex++) {
                        var currentBetTypes = currentBetTypeGroups[btGroupIndex].BetTypes;
                        var betTypeLength = currentBetTypes.length;
                        for (var btIndex = 0; btIndex < betTypeLength; btIndex++) {
                            var currentPt = sportTypes[sportTypeIndex].BetTypeGroups[btGroupIndex].BetTypes[btIndex].Pt;
                            if (currentPt != null) {
                                return true;
                            }
                        }
                    }
                }
                return false;
            };

            this.Validate = function () {
                if (Fanex.CustSetting.CURRENT_CONTEXT.IsSimpleMode == true) {
                    return true;
                }
                else {
                    var items = this.items;
                    var count = items.length;
                    for (var i = 0; i < count; i++) {
                        if (!items[i].Validate()) {
                            items[i].Toggle(true);
                            items[i].Validate();
                            return false;
                        }
                    };
                }
                return true;
            };

            this.GetSportName = function (sportId) {
                var count = this.items.length;
                for (var index = 0; index < count; index++) {
                    if (this.items[index].id == sportId) {
                        return this.items[index].name;
                    }
                }
                return '';
            };
            this.ShowCloneHint = function () {
                if (this.copiedItem == null) {
                    return;
                }
                var copiedElements = [0, this.items[0].items[0].items[0].element,
										this.items[2].items[0].items[0].element]; // Handicap of Soccer and Basketball
                var hint = Language.COPY_ALL_HINT.Format('<b>' + this.activeCopiedTitle + '</b>');
                var otherBetTypesOf = Language.OTHER_BET_TYPE_OF.Format(this.GetSportName(this.copiedItem.sportId));
                hint = hint + '<div class="ul-position"><ul><li>' + otherBetTypesOf + '</li>';
                // Find all copied items to generate tooltip
                var count = this.items.length;
                var top = 0;

                for (var i = this.copiedItem.sportId + 1; i < count; i++) {
                    hint += '<li>' + this.items[i].name + '</li>';
                    top += 18;
                }
                hint = hint + '</ul></div>';
                top = top + 78;
                var height = top - 20;
                // Show tooltip
                //Validators.show(copiedElements[this.copiedItem.sportId], hint, 0, 10, -top - 54, top + 20, top, 30000);
                //show: function (elementInput, message, xPos, pointerXPos, yPos, pointerYPos, height, timeout) {
                if (this.copiedItem.sportId == 2) {
                    top += 15;
                }
                Validators.show(copiedElements[this.copiedItem.sportId], hint, 0, 25, top - 158, -10, height, 10000, true);
            },
            this.UpdateCopyValue = function (currentCopyBetType, betType) {
                //                var reload = (Fanex.CustSetting.CURRENT_CONTEXT.TargetRoleId == Fanex.CustSetting.RoleType.Member ? 1 : 0);
                //                while (reload <= 1) {
                if (betType.id != currentCopyBetType.id) {
                    if (betType.presetSpinBox) {
                        betType.presetSpinBox.setValue(currentCopyBetType.presetSpinBox.getValue(), true);
                        betType.presetSpinBox.input.onchange();
                        betType.presetOnMember.checked = currentCopyBetType.presetOnMember.checked;
                    }
                    if (betType.maxSpinBox) {
                        betType.maxSpinBox.setValue(currentCopyBetType.maxSpinBox.getValue(), true);
                        betType.maxSpinBox.input.onchange();
                    }
                    if (betType.minSpinBox) {
                        betType.minSpinBox.setValue(currentCopyBetType.minSpinBox.getValue(), true);
                        betType.minSpinBox.input.onchange();
                    }

                    if (betType.ptSpinBox) {
                        betType.ptSpinBox.setValue(currentCopyBetType.ptSpinBox.getValue(), true);
                        betType.ptSpinBox.input.onchange();
                    }
                }
            };
            this.Render = function (owner) {
                var currentContext = Fanex.CustSetting.CURRENT_CONTEXT;
                var box = null;
                if (currentContext.IsSimpleMode == false) {
                    var elements = []; // Child sport type elements
                    if (typeof (this.sportTypes) != 'object' || typeof (this.sportTypes.length) == 'undefined') {
                        console.log("Input data source sportTypes is empty.");
                    } else {
                        box = document.createElement("DIV");
                        //// Position Taking Header
                        var header = document.createElement("DIV");
                        header.innerHTML = Language.POSITION_SETTING_HEADER;
                        header.className = "pt-grid-header";
                        var informationButton = document.createElement("SPAN");
                        informationButton.className = "section-info";
                        if (currentContext.TargetRoleId == Fanex.CustSetting.RoleType.Member) {
                            informationButton.id = "member-";
                        }
                        else if (currentContext.TargetRoleId == Fanex.CustSetting.RoleType.MainShareHolder) {
                            informationButton.id = "mainshareholder-";
                        }
                        else {
                            informationButton.id = "agent-";
                        }
                        if (currentContext.FormId == Fanex.CustSetting.FormTypes.VIEW_SETTING) {
                            informationButton.id = "view-" + informationButton.id;
                        }
                        informationButton.id += "share-setting-info-button" + (currentContext.IsSimpleMode ? "" : "-adv");
                        header.appendChild(informationButton);
                        box.appendChild(header);
                        // Each sport types
                        var count = this.sportTypes.length;
                        for (var i = 0; i < count; i++) {
                            var item = this.sportTypes[i];
                            var sportItem = new Fanex.CustSetting.SportType(item);
                            sportItem.parent = this; // Keep reference to current grid
                            var element = sportItem.Render(box);
                            var clear = document.createElement("DIV");
                            clear.style.clear = "both";
                            box.appendChild(clear);
                            if (element != null) {
                                elements[i] = element;
                                this.items[i] = sportItem;
                            }
                        }
                        this.elements = elements;
                    }
                    if (this.elements.length == 0) {
                        box = null;
                    }
                    if (box != null) {
                        box.className = "pt-grid";
                        var items = this.items;
                        var elements = this.elements;
                        if (currentContext.FormId != Fanex.CustSetting.FormTypes.VIEW_SETTING
						&& currentContext.FormId != Fanex.CustSetting.FormTypes.SHADOW_VIEW) {
                            // Implementation for clone button of Soccer 1 and Basketball 2
                            var cloneFromSoccer = this.items[0].cloneObjectButton;
                            var cloneFromBasketball = this.items[2].cloneObjectButton;
                            var editBasketball = this.items[2].editButton;
                            var copiedItemSoccer = {
                                sportId: 1,
                                id: 1,
                                isLive: false
                            };
                            var copiedItemBasketball = {
                                sportId: 2,
                                id: 1,
                                isLive: false
                            };
                            cloneFromSoccer.parent = cloneFromBasketball.parent = this;
                            var hideHintTimeout = 0;
                            cloneFromSoccer.onclick = function () {
                                Validators.hide();
                                cloneFromSoccer.parent.copiedItem = copiedItemSoccer;
                                cloneFromSoccer.parent.activeCopiedTitle = "{0} {1}".Format(items[0].name, items[0].betTypeGroups[0].BetTypes[0].Name);
                                cloneFromSoccer.parent.items[0].Toggle(true); // Expand
                                var c = elements.length;
                                var currentCopyBetType = elements[0].betTypeGroupElements[0].betTypeElements[0];
                                if (currentCopyBetType.presetSpinBox && !currentCopyBetType.presetSpinBox.isValid()) {
                                    return;
                                }
                                if (currentCopyBetType.maxSpinBox && !currentCopyBetType.maxSpinBox.isValid()) {
                                    return;
                                }
                                if (currentCopyBetType.minSpinBox && !currentCopyBetType.minSpinBox.isValid()) {
                                    return;
                                }

                                if (currentCopyBetType.ptSpinBox && !currentCopyBetType.ptSpinBox.isValid()) {
                                    return;
                                }
                                var sportTypes = elements;
                                var sportTypesLength = elements.length;
                                for (var sportTypeIndex = 0; sportTypeIndex < sportTypesLength; sportTypeIndex++) {
                                    var betTypeGroups = sportTypes[sportTypeIndex].betTypeGroupElements;
                                    var betTypeGroupsLength = betTypeGroups.length;
                                    for (var betTypeGroupIndex = 0; betTypeGroupIndex < betTypeGroupsLength; betTypeGroupIndex++) {
                                        var betTypes = betTypeGroups[betTypeGroupIndex].betTypeElements;
                                        var betTypesLength = betTypes.length;
                                        for (var betTypeIndex = 0; betTypeIndex < betTypesLength; betTypeIndex++) {
                                            var betType = betTypes[betTypeIndex];
                                            cloneFromSoccer.parent.UpdateCopyValue(currentCopyBetType, betType);
                                        }
                                    }

                                }
                                clearInterval(Validators.intervalAnimate);
                                cloneFromSoccer.parent.ShowCloneHint();
                            };
                            // Basketball
                            cloneFromBasketball.onclick = function () {
                                Validators.hide();
                                cloneFromBasketball.parent.copiedItem = copiedItemBasketball;
                                cloneFromBasketball.parent.activeCopiedTitle = "{0} {1}".Format(items[2].name, items[0].betTypeGroups[0].BetTypes[0].Name);
                                cloneFromBasketball.parent.items[2].Toggle(true); // Expand
                                var c = elements.length;
                                var currentCopyBetType = elements[2].betTypeGroupElements[0].betTypeElements[0];
                                if (currentCopyBetType.presetSpinBox && !currentCopyBetType.presetSpinBox.isValid()) {
                                    return;
                                }
                                if (currentCopyBetType.maxSpinBox && !currentCopyBetType.maxSpinBox.isValid()) {
                                    return;
                                }
                                if (currentCopyBetType.minSpinBox && !currentCopyBetType.minSpinBox.isValid()) {
                                    return;
                                }

                                if (currentCopyBetType.ptSpinBox && !currentCopyBetType.ptSpinBox.isValid()) {
                                    return;
                                }
                                var sportTypes = elements;
                                var sportTypesLength = elements.length;
                                for (var sportTypeIndex = 2; sportTypeIndex < sportTypesLength; sportTypeIndex++) {
                                    var betTypeGroups = sportTypes[sportTypeIndex].betTypeGroupElements;
                                    var betTypeGroupsLength = betTypeGroups.length;
                                    for (var betTypeGroupIndex = 0; betTypeGroupIndex < betTypeGroupsLength; betTypeGroupIndex++) {
                                        var betTypes = betTypeGroups[betTypeGroupIndex].betTypeElements;
                                        var betTypesLength = betTypes.length;
                                        for (var betTypeIndex = 0; betTypeIndex < betTypesLength; betTypeIndex++) {
                                            var betType = betTypes[betTypeIndex];
                                            cloneFromBasketball.parent.UpdateCopyValue(currentCopyBetType, betType);
                                        }
                                    }

                                }
                                clearInterval(Validators.intervalAnimate);
                                cloneFromBasketball.parent.ShowCloneHint();
                            };
                        }
                        this.DOM = box;
                        // Append or return the object
                        if (typeof (owner) == 'object') {
                            //// Render 
                            owner.appendChild(box);
                        }
                    }
                }
                else {
                    this.currentPtValue = this.sportTypes[0].BetTypeGroups[0].BetTypes[0].Pt;
                    box = document.createElement("DIV");
                    box.className = "simple-mode-container";
                    //Fanex.CustSetting.SPINNER_BOX_WIDTH = 80;
                    //// Position Taking Header
                    var header = document.createElement("DIV");
                    header.innerHTML = Language.POSITION_SETTING_SIMPLE_MODE_HEADER;
                    header.className = "pt-grid-header";
                    var informationButton = document.createElement("SPAN");
                    informationButton.className = "section-info";
                    if (currentContext.TargetRoleId == Fanex.CustSetting.RoleType.Member) {
                        informationButton.id = "member-";
                    }
                    else if (currentContext.TargetRoleId == Fanex.CustSetting.RoleType.MainShareHolder) {
                        informationButton.id = "mainshareholder-";
                    }
                    else {
                        informationButton.id = "agent-";
                    }
                    if (currentContext.FormId == Fanex.CustSetting.FormTypes.VIEW_SETTING) {
                        informationButton.id = "view-" + informationButton.id;
                    }
                    informationButton.id += "share-setting-info-button" + (currentContext.IsSimpleMode ? "" : "-adv");
                    header.appendChild(informationButton);
                    box.appendChild(header);

                    var formId = currentContext.FormId;
                    var roleId = currentContext.RoleId; //(this.context.TargetRoleId == 1 || formId != Fanex.CustSetting.FormTypes.EDIT_PT) ? this.context.RoleId : 0;
                    var targetRoleId = currentContext.TargetRoleId;
                    var method = this['RenderSimpleMode_'.concat(roleId, '_', targetRoleId, '_', formId)];
                    if (typeof (method) == "function") {
                        box.appendChild(method.call(this, owner));
                    }

                    this.DOM = box;
                    // Append or return the object
                    if (typeof (owner) == 'object') {
                        //// Render 
                        owner.appendChild(box);
                    }
                }
                return box;
            }
        }
    }
};
