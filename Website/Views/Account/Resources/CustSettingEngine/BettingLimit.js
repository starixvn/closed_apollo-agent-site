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
Fanex.CustSetting.BettingLimit = function (bettingLimitItems) {
    this.items = bettingLimitItems;
    var validateExpr = /^(\$|)([0-9]\d{0,2}(\,\d{3})*|([0-9]\d*))(\.?\d*)?$/;
    this.ValidateMessage = function (elementInput, errorMessage, xpos, pointerx) {
        var isXPos = (typeof (xpos) != "undefined" && xpos != null);
        var isPointerXPos = (typeof (pointerx) != "undefined" && pointerx != null);
        if (isXPos && pointerx) {
            Validators.show(elementInput, errorMessage, xpos, pointerx);
        }
        else {
            Validators.show(elementInput, errorMessage);
        }
        Validators.animate(elementInput);
    };
    this.Validate = function () {

        if (Fanex.CustSetting.CURRENT_CONTEXT.FormId == Fanex.CustSetting.FormTypes.EDIT_NON_DIRECT) return true;

        var c = this.items.length;
        var isSimpleMode = Fanex.CustSetting.CURRENT_CONTEXT.IsSimpleMode;
        if (isSimpleMode) {
            c = 1;
        }
        for (var i = 0; i < c; i++) {
            var item = this.items[i];
            var copiedItem = item.copiedItem;
            var elementInput = null;
            // Validate each sport type
            if (copiedItem == null) { // Copy checkbox was ticked
                if (validateExpr.test(item.minBetElement.value) == false) {
                    this.ValidateMessage(item.minBetElement, Language.INCORRECT_MIN_BET);
                    return false;
                }
                if (validateExpr.test(item.maxBetElement.value) == false) {
                    this.ValidateMessage(item.maxBetElement, Language.INCORRECT_MAX_BET);
                    return false;
                }
                if (validateExpr.test(item.maxPerMatchElement.value) == false) {
                    this.ValidateMessage(item.maxPerMatchElement, Language.INCORRECT_MAX_PER_MATCH);
                    return false;
                }
                // Check min bet < max bet < max per match
                var minBet = parseFloat(item.minBetElement.value, true);
                var maxBet = parseFloat(item.maxBetElement.value, true);
                var maxPerMatch = parseFloat(item.maxPerMatchElement.value, true);
                if (minBet > maxBet) {
                    this.ValidateMessage(item.minBetElement, Language.MIN_BET_LOWER_THAN_MIN_MAX_BET.Format(item.Name));
                    return false;
                }
                if (minBet > maxPerMatch) {
                    this.ValidateMessage(item.minBetElement, Language.MIN_BET_LOWER_THAN_MIN_MAX_PER_MATCH_BET.Format(item.Name));
                    return false;
                }
                if (maxBet > maxPerMatch) {
                    this.ValidateMessage(item.maxBetElement, Language.MAX_BET_LOWER_THAN_MIN_MAX_PER_MATCH_BET.Format(item.Name));
                    return false;
                }
                // Check minbet < minbetmax
                if (typeof (item.MinBetMax) != 'undefined' && item.MinBetMax > 0 && minBet < item.MinBetMax && minBet != 0) {
                    this.ValidateMessage(item.minBetElement, Language.MIN_BET_GREATER_THAN_MIN_MIN_BET.Format(''.FormatNumber(item.MinBetMax)));
                    return false;
                }
                // Check maxbet < maxbetmin
                if (typeof (item.MaxBetMin) != 'undefined' && item.MaxBetMin > 0 && maxBet > item.MaxBetMin && maxBet != 0) {
                    this.ValidateMessage(item.maxBetElement, Language.MAX_BET_LOWER_THAN_MAX_MAX_BET.Format(''.FormatNumber(item.MaxBetMin)));
                    return false;
                }
                // Check maxpermatch_min < maxpermatch < maxpermatch_max
                if (typeof (item.MaxPerMatchMin) != 'undefined' && item.MaxPerMatchMin > 0 && maxPerMatch > item.MaxPerMatchMin && maxPerMatch != 0) {
                    this.ValidateMessage(item.maxPerMatchElement, Language.MAX_PER_MATCH_LOWER_THAN_MAX_PER_MATCH_MAX_BET.Format(''.FormatNumber(item.MaxPerMatchMin)), -130, 160);
                    return false;
                }

            }
            else {
                var copyData = this.items[copiedItem.SportId];
                var minBet = parseFloat(copyData.minBetElement.value, true);
                var maxBet = parseFloat(copyData.maxBetElement.value, true);
                var maxPerMatch = parseFloat(copyData.maxPerMatchElement.value, true);
                if (minBet > maxBet) {
                    this.ValidateMessage(item.minBetElement, Language.MIN_BET_LOWER_THAN_MIN_MAX_BET_AFTER_COPY.Format(item.Name));
                    return false;
                }
                if (minBet > maxPerMatch) {
                    this.ValidateMessage(item.minBetElement, Language.MIN_BET_LOWER_THAN_MIN_MAX_PER_MATCH_BET_AFTER_COPY.Format(item.Name));
                    return false;
                }
                if (maxBet > maxPerMatch) {
                    this.ValidateMessage(item.maxBetElement, Language.MAX_BET_LOWER_THAN_MIN_MAX_PER_MATCH_BET_AFTER_COPY.Format(item.Name));
                    return false;
                }
                // Check minbet < minbetmax
                if (typeof (item.MinBetMax) != 'undefined' && item.MinBetMax > 0 && minBet < item.MinBetMax && minBet != 0) {
                    this.ValidateMessage(item.minBetElement, Language.MIN_BET_GREATER_THAN_MIN_MIN_BET_AFTER_COPY.Format(''.FormatNumber(item.MinBetMax)));
                    return false;
                }
                // Check maxbet < maxbetmin
                if (typeof (item.MaxBetMin) != 'undefined' && item.MaxBetMin > 0 && maxBet > item.MaxBetMin && maxBet != 0) {
                    this.ValidateMessage(item.maxBetElement, Language.MAX_BET_LOWER_THAN_MAX_MAX_BET_AFTER_COPY.Format(''.FormatNumber(item.MaxBetMin)));
                    return false;
                }
                // Check maxpermatch_min < maxpermatch < maxpermatch_max
                if (typeof (item.MaxPerMatchMin) != 'undefined' && item.MaxPerMatchMin > 0 && maxPerMatch > item.MaxPerMatchMin && maxPerMatch != 0) {
                    this.ValidateMessage(item.maxPerMatchElement, Language.MAX_PER_MATCH_LOWER_THAN_MAX_PER_MATCH_MAX_BET_AFTER_COPY.Format(''.FormatNumber(item.MaxPerMatchMin)), -130, 160);
                    return false;
                }
            }

            //// Check  Number Game
            if (item.SportId == 161) {
                if (validateExpr.test(item.maxPerBallElement.value) == false) {
                    this.ValidateMessage(item.maxPerBallElement, Language.INCORRECT_MAX_PER_BALL);
                    return false;
                }
                var maxPerBall = parseFloat(item.maxPerBallElement.value, true);

                if (minBet && minBet > maxPerBall) {
                    this.ValidateMessage(item.minBetElement, Language.MIN_BET_LOWER_THAN_MIN_MAX_PER_BALL.Format(item.Name));
                    return false;
                }
                // Check maxperball_min < maxperball < maxperball_max
                if (typeof (item.MaxPerBallMin) != 'undefined' && item.MaxPerBallMin > 0 && maxPerBall > item.MaxPerBallMin && maxPerBall != 0) {
                    this.ValidateMessage(item.maxPerBallElement, Language.MAX_PER_BALL_LOWER_THAN_MAX_PER_BALL_MAX_BET.Format(''.FormatNumber(item.MaxPerBallMin)), -180, 200);
                    return false;
                }
            }
        }
        // Success
        return true;
    };
    this.Save = function () {
        var bettingLimits = [];
        if (Fanex.CustSetting.CURRENT_CONTEXT.FormId == Fanex.CustSetting.FormTypes.EDIT_NON_DIRECT) return null;
        if (Fanex.CustSetting.CURRENT_CONTEXT.IsSimpleMode) {
            //Fanex.RoundNumber(parseFloat(this.items[copiedItem.SportId].minBetElement.value, true), 1)
            var minBetValue = Fanex.RoundNumber(parseFloat(this.items[0].minBetElement.value, true), 1);
            var maxBetValue = Fanex.RoundNumber(parseFloat(this.items[0].maxBetElement.value, true), 1);
            var maxPerMatchValue = Fanex.RoundNumber(parseFloat(this.items[0].maxPerMatchElement.value, true), 1);
            if (minBetValue != this.items[0].MinBet
            || maxBetValue != this.items[0].MaxBet
            || maxPerMatchValue != this.items[0].MaxPerMatch) {
                var c = this.items.length;
                for (var i = 0; i < c; i++) {
                    var item = this.items[i];
                    bettingLimits.push({
                        SportId: item.SportId,
                        BetTypeId: item.BetTypeId,
                        Key: item.key,
                        MinBet: minBetValue,
                        MaxBet: maxBetValue,
                        MaxPerMatch: maxPerMatchValue
                    });
                }
            }

        }
        else {

            var c = this.items.length;
            var saveIndex = 0;
            for (var i = 0; i < c; i++) {
                var item = this.items[i];
                var bettingLimitItem = null;
                var copiedItem = item.copiedItem;
                var minBetValue = 0;
                var maxBetValue = 0;
                var maxPerMatchValue = 0;
                var maxPerBall = 0;

                if (item.SportId == 161) {
                    minBetValue = Fanex.RoundNumber(parseFloat(item.minBetElement.value, true), 1);
                    maxBetValue = Fanex.RoundNumber(parseFloat(item.maxBetElement.value, true), 1);
                    maxPerMatchValue = Fanex.RoundNumber(parseFloat(item.maxPerMatchElement.value, true), 1);
                    maxPerBall = Fanex.RoundNumber(parseFloat(item.maxPerBallElement.value, true), 1);
                    if (minBetValue != item.MinBet
					|| maxBetValue != item.MaxBet
					|| maxPerMatchValue != item.MaxPerMatch
					|| maxPerBall != item.MaxPerBall) {
                        bettingLimitItem = {
                            SportId: item.SportId,
                            BetTypeId: item.BetTypeId,
                            Key: item.key,
                            MinBet: minBetValue,
                            MaxBet: maxBetValue,
                            MaxPerMatch: maxPerMatchValue,
                            MaxPerBall: maxPerBall
                        };
                    }
                }
                else {
                    if (copiedItem != null) { // Copy checkbox was ticked
                        bettingLimitItem = {
                            SportId: item.SportId,
                            BetTypeId: item.BetTypeId,
                            key: item.key,
                            MinBet: Fanex.RoundNumber(parseFloat(this.items[copiedItem.SportId].minBetElement.value, true), 1),
                            MaxBet: Fanex.RoundNumber(parseFloat(this.items[copiedItem.SportId].maxBetElement.value, true), 1),
                            MaxPerMatch: Fanex.RoundNumber(parseFloat(this.items[copiedItem.SportId].maxPerMatchElement.value, true), 1)
                        };
                    } else {
                        minBetValue = Fanex.RoundNumber(parseFloat(item.minBetElement.value, true), 1);
                        maxBetValue = Fanex.RoundNumber(parseFloat(item.maxBetElement.value, true), 1);
                        maxPerMatchValue = Fanex.RoundNumber(parseFloat(item.maxPerMatchElement.value, true), 1);
                        maxPerBall = 0;
                        if (minBetValue != item.MinBet
					|| maxBetValue != item.MaxBet
					|| maxPerMatchValue != item.MaxPerMatch) {
                            bettingLimitItem = {
                                SportId: item.SportId,
                                BetTypeId: item.BetTypeId,
                                Key: item.key,
                                MinBet: minBetValue,
                                MaxBet: maxBetValue,
                                MaxPerMatch: maxPerMatchValue
                            };
                        }
                    }
                }
                if (bettingLimitItem) {
                    bettingLimits.push(bettingLimitItem);
                }
            }
        }
        return bettingLimits;
    };
    this.HasNumberGame = function () {
        var c = this.items.length;
        for (var i = 0; i < c; i++) {
            if (this.items[i].SportId == 161) {
                return true;
            }
        };
        return false;
    };
    this.Render = function (owner) {
        var context = Fanex.CustSetting.CURRENT_CONTEXT;
        var isEditNonDirectMember = (context.FormId == Fanex.CustSetting.FormTypes.EDIT_NON_DIRECT) || (Fanex.CustSetting.CURRENT_CONTEXT.FormId == Fanex.CustSetting.FormTypes.VIEW_SETTING);
        var hasNumberGame = this.HasNumberGame();
        var container = document.createElement("DIV");
        container.className = Fanex.CustSetting.CURRENT_CONTEXT.IsSimpleMode ? "" : "betting-limit-container-header";

        var header = document.createElement("DIV");
        header.innerHTML = Language.BET_SETTING_HEADER;
        header.className = "pt-grid-header";
        var saveButton = document.createElement("SPAN");
        saveButton.className = "save-component-setting";
        header.appendChild(saveButton);
        container.appendChild(header);

        if (Fanex.CustSetting.CURRENT_CONTEXT.IsSimpleMode == false) {

            var box = document.createElement("DIV");
            // Create MinBet, MaxBet, MaxPerMatch header
            var emptyBox = document.createElement("DIV");
            emptyBox.innerHTML = "&nbsp;";
            emptyBox.className = "betting-limit-header";
            box.appendChild(emptyBox);
            var minBetHeader = document.createElement("DIV");
            minBetHeader.innerHTML = Language.MIN_BET;
            minBetHeader.className = "betting-limit-header-top";
            box.appendChild(minBetHeader);
            var maxBetHeader = document.createElement("DIV");
            maxBetHeader.innerHTML = Language.MAX_BET;
            maxBetHeader.className = "betting-limit-header-top";
            box.appendChild(maxBetHeader);
            var maxPerMatchHeader = document.createElement("DIV");
            maxPerMatchHeader.innerHTML = Language.MAX_PER_MATCH;
            maxPerMatchHeader.className = "betting-limit-header-top";
            box.appendChild(maxPerMatchHeader);
            if (hasNumberGame) {
                var maxPerBallHeader = document.createElement("DIV");
                maxPerBallHeader.innerHTML = Language.MAX_PER_BALL;
                maxPerBallHeader.className = "betting-limit-header-top";
                box.appendChild(maxPerBallHeader);
            }
            var clear = document.createElement("DIV");
            clear.style.clear = "both";
            box.appendChild(clear);
            var showLimitBox = false;
            var c = this.items.length;
            for (var i = 0; i < c; i++) {
                var item = this.items[i];
                var box2 = document.createElement("DIV");
                if (typeof (item.MinBet) == 'undefined') { // For Add new case
                    item.MinBet = item.MinBetMax;
                }
                if (typeof (item.MaxBet) == 'undefined') { // For Add new case
                    item.MaxBet = item.MaxBetMin;
                }
                if (typeof (item.MaxPerMatch) == 'undefined') { // For Add new case
                    item.MaxPerMatch = item.MaxPerMatchMin;
                }
                if (typeof (item.MaxPerBall) == 'undefined' && item.SportId == 161) { // For Add new case
                    item.MaxPerBall = item.MaxPerBallMin;
                }
                var showLimitBox = Fanex.CustSetting.CURRENT_CONTEXT.TargetRoleId != Fanex.CustSetting.RoleType.MainShareHolder; //typeof (item.MaxBetMin) != 'undefined';
                var header = document.createElement("DIV");
                header.innerHTML = item.Name;
                header.className = "betting-limit-header";
                item.header = header;
                box2.appendChild(header);
                // Hard-code for max per ball of number game
                if (item.SportId == 161) {
                }
                // Current value
                // Show label instead to TextBox
                if (isEditNonDirectMember) {
                    var minBetBox = document.createElement("DIV");
                    minBetBox.innerHTML = ''.FormatNumber(item.MinBet);
                    minBetBox.className = "betting-limit-label";
                    box2.appendChild(minBetBox);
                    minBetHeader.className = "betting-limit-header-top";
                }
                else {
                    var minBetBox = document.createElement("DIV");
                    var minBet = document.createElement("INPUT");
                    minBet.type = "text";
                    minBet.maxLength = 14;
                    minBet.onkeyup = function (event) { Fanex.FormatNumber(event, true); };
                    minBet.className = "betting-limit-textbox";
                    minBet.value = ''.FormatNumber(item.MinBet);
                    minBetBox.appendChild(minBet);
                    minBetBox.className = "betting-limit-item";
                    box2.appendChild(minBetBox);
                    item.minBetElement = minBet;
                    // Limit box
                    var minBetMax = document.createElement("DIV");
                    if (showLimitBox) {
                        minBetMax.innerHTML = item.MinBetMax == 0 ? '' : '>= '.FormatNumber(item.MinBetMax);
                        minBetMax.className = "betting-limit-item2";
                        box2.appendChild(minBetMax);
                    } else {
                        minBet.className = "betting-limit-textbox4";
                        minBetBox.className = "betting-limit-item4";
                        minBetHeader.className = "betting-limit-header-top4";
                    }
                }

                // Current value
                // Show label instead to TextBox
                if (isEditNonDirectMember) {
                    var maxBetBox = document.createElement("DIV");
                    maxBetBox.innerHTML = ''.FormatNumber(item.MaxBet);
                    maxBetBox.className = "betting-limit-label";
                    box2.appendChild(maxBetBox);
                    maxBetHeader.className = "betting-limit-header-top";
                }
                else {
                    var maxBetBox = document.createElement("DIV");
                    var maxBet = document.createElement("INPUT");
                    maxBet.type = "text";
                    maxBet.maxLength = 14;
                    maxBet.onkeyup = function (event) { Fanex.FormatNumber(event, true); };
                    maxBet.className = "betting-limit-textbox";
                    maxBet.value = ''.FormatNumber(item.MaxBet);
                    maxBetBox.appendChild(maxBet);
                    maxBetBox.className = "betting-limit-item";
                    box2.appendChild(maxBetBox);
                    item.maxBetElement = maxBet;
                    // Limit box
                    var maxBetMin = document.createElement("DIV");
                    if (showLimitBox) {
                        maxBetMin.innerHTML = item.MaxBetMin == 0 ? '' : '<= '.FormatNumber(item.MaxBetMin);
                        maxBetMin.className = "betting-limit-item2";
                        box2.appendChild(maxBetMin);
                    } else {
                        maxBet.className = "betting-limit-textbox4";
                        maxBetBox.className = "betting-limit-item4";
                        maxBetHeader.className = "betting-limit-header-top4";
                    }
                }

                // Current value
                // Show label instead to TextBox
                if (isEditNonDirectMember) {
                    var maxPerMatchBox = document.createElement("DIV");
                    maxPerMatchBox.innerHTML = ''.FormatNumber(item.MaxPerMatch);
                    maxPerMatchBox.className = "betting-limit-label";
                    box2.appendChild(maxPerMatchBox);
                    if (showLimitBox) {
                        maxPerMatchHeader.className = "betting-limit-header-top";
                    }
                    else {
                        maxPerMatchHeader.className = "betting-limit-header-top4";
                    }

                    if (item.SportId == 161) {
                        minBetBox.className = maxBetBox.className = maxPerMatchBox.className = "BettingLimitLabel1";

                        var maxPerBallBox = document.createElement("DIV");
                        maxPerBallBox.className = "BettingLimitLabel1";
                        maxPerBallBox.innerHTML = ''.FormatNumber(item.MaxPerBall);
                        if (showLimitBox) {
                            maxPerBallHeader.className = "betting-limit-header-top";
                        }
                        else {
                            maxPerBallHeader.className = "betting-limit-header-top4";
                        }
                        box2.appendChild(maxPerBallBox);
                    }
                }
                else {
                    var maxPerMatchBox = document.createElement("DIV");
                    var maxPerMatch = document.createElement("INPUT");
                    maxPerMatch.type = "text";
                    maxPerMatch.maxLength = 14;
                    maxPerMatch.onkeyup = function (event) { Fanex.FormatNumber(event, true); };
                    maxPerMatch.className = "betting-limit-textbox";
                    maxPerMatch.value = ''.FormatNumber(item.MaxPerMatch);
                    maxPerMatchBox.appendChild(maxPerMatch);
                    maxPerMatchBox.className = "betting-limit-item";
                    box2.appendChild(maxPerMatchBox);
                    item.maxPerMatchElement = maxPerMatch;
                    // Limit box
                    var maxPerMatchMin = document.createElement("DIV");
                    if (showLimitBox) {
                        maxPerMatchMin.innerHTML = item.MaxPerMatchMin == 0 ? '' : '<= '.FormatNumber(item.MaxPerMatchMin);
                        maxPerMatchMin.className = "betting-limit-item2";
                        box2.appendChild(maxPerMatchMin);
                    } else {
                        maxPerMatch.className = "betting-limit-textbox4";
                        maxPerMatchBox.className = "betting-limit-item4";
                        maxPerMatchHeader.className = "betting-limit-header-top4";
                    }
                    if (hasNumberGame) {
                        // Max Per Ball
                        var maxPerBallBox = document.createElement("DIV");
                        maxPerBallBox.className = showLimitBox ? "betting-limit-item-no-info" : "betting-limit-item5";
                        box2.appendChild(maxPerBallBox);

                        // Limit box
                        if (item.SportId == 161) {
                            maxPerBallBox.className = "betting-limit-item";
                            var maxPerBall = document.createElement("INPUT");
                            maxPerBall.type = "text";
                            maxPerBall.maxLength = 14;
                            maxPerBall.value = ''.FormatNumber(item.MaxPerBall);
                            maxPerBall.onkeyup = function (event) { Fanex.FormatNumber(event, true); };
                            maxPerBall.className = showLimitBox ? "betting-limit-textbox" : "betting-limit-textbox5";
                            maxPerBallBox.appendChild(maxPerBall);
                            if (showLimitBox) {
                                maxPerBallHeader.className = "betting-limit-header-top";
                            }
                            else {
                                maxPerBallBox.className = "betting-limit-item4";
                                maxPerBallHeader.className = "betting-limit-header-top4";
                            }
                            item.maxPerBallElement = maxPerBall;
                        }

                        var maxPerBallMin = document.createElement("DIV");
                        if (showLimitBox) {
                            maxPerBallMin.className = "betting-limit-item2-no-info";
                            if (item.SportId == 161) {
                                maxPerBallMin.innerHTML = item.MaxPerBallMin == 0 ? '' : '<= '.FormatNumber(item.MaxPerBallMin);
                                maxPerBallMin.className = "betting-limit-item2";
                            }
                            box2.appendChild(maxPerBallMin);
                        }
                    }
                }
                item.element = box2;
                box.appendChild(box2);
                var clear = document.createElement("DIV");
                clear.style.clear = "both";
                box.appendChild(clear);
            }
            box.className = (showLimitBox || isEditNonDirectMember) ? "betting-limit" : "betting-limit4";
            if (typeof (owner) == 'object') {
                if (!isEditNonDirectMember) {
                    var headerSoccer = this.items[0].header;
                    var headerBasketBall = this.items[1].header;
                    headerSoccer.className = headerBasketBall.className = "betting-limit-header-copied";
                    headerSoccer.title = Language.COPY_ALL_BETTING_LIMIT_FROM.Format(Language.SOCCER);
                    headerBasketBall.title = Language.COPY_ALL_BETTING_LIMIT_FROM.Format(Language.BASKETBALL);
                    var items = this.items;
                    items[0].active = items[1].active = false;
                    // Handle event copy from soccer
                    headerSoccer.onclick = function () {
                        var c = items.length;
                        var currentSoccerBetLimit = items[0];
                        for (var i = 1; i < c; i++) {
                            var currentItem = items[i];
                            currentItem.minBetElement.value = currentSoccerBetLimit.minBetElement.value;
                            currentItem.maxBetElement.value = currentSoccerBetLimit.maxBetElement.value;
                            currentItem.maxPerMatchElement.value = currentSoccerBetLimit.maxPerMatchElement.value;
                        }
                    }
                    // Handle event copy from basket ball
                    headerBasketBall.onclick = function () {
                        var c = items.length;
                        var currentSoccerBetLimit = items[1];
                        for (var i = 2; i < c; i++) {
                            var currentItem = items[i];
                            currentItem.minBetElement.value = currentSoccerBetLimit.minBetElement.value;
                            currentItem.maxBetElement.value = currentSoccerBetLimit.maxBetElement.value;
                            currentItem.maxPerMatchElement.value = currentSoccerBetLimit.maxPerMatchElement.value;
                        }
                    }
                }
                container.appendChild(box);
                owner.appendChild(container);
            }
        }
        else {
            var box = document.createElement("DIV");
            box.className = "simple-mode-container";

            // Table
            var tableShareSetting = document.createElement("TABLE");
            tableShareSetting.className = "bet-limit-table";
            box.appendChild(tableShareSetting);
            var trSetting = document.createElement("TR");
            tableShareSetting.appendChild(trSetting)
            // Share Settings
            var presetLabel = document.createElement("TD");
            presetLabel.innerHTML = Language.SOCCER;
            presetLabel.title = Language.SOCCER;
            presetLabel.className = "share-setting-label";
            trSetting.appendChild(presetLabel);

            var item = this.items[0];
            //// Min Bet
            item.minBetElement = this.RenderBetLimitBlock(isEditNonDirectMember,
            trSetting,
            Language.MIN_BET,
            item.MinBetMax == 0 ? '' : '>= '.FormatNumber(item.MinBetMax),
            item.MinBet,
            false);

            //// Max Bet
            item.maxBetElement = this.RenderBetLimitBlock(isEditNonDirectMember,
            trSetting,
            Language.MAX_BET,
            item.MaxBetMin == 0 ? '' : '<= '.FormatNumber(item.MaxBetMin),
            item.MaxBet, false);

            /// Max Per Match
            item.maxPerMatchElement = this.RenderBetLimitBlock(isEditNonDirectMember,
            trSetting,
            Language.MAX_PER_MATCH,
            item.MaxPerMatchMin == 0 ? '' : '<= '.FormatNumber(item.MaxPerMatchMin),
            item.MaxPerMatch,
            false);

            container.appendChild(box);
            owner.appendChild(container);
        }
        return box;
    },
    this.RenderBetLimitBlock = function (isEditNonDirectMember, parentElement, minMaxText, constrainText, betLimitValue, isMaxPerMatch) {
        /// Max Per Match
        var betSettingContainer = document.createElement("TD");
        if (isEditNonDirectMember) {
            betSettingContainer.className = "bet-limit-text";
            betSettingContainer.innerHTML = minMaxText + ' = '.FormatNumber(betLimitValue);
            parentElement.appendChild(betSettingContainer);
        }
        else {
            betSettingContainer.className = "bet-limit-value";
            betSettingContainer.innerHTML = minMaxText + ' ';
            parentElement.appendChild(betSettingContainer);

            var inputValue = document.createElement("INPUT");
            inputValue.type = "text";
            inputValue.className = "bet-limit-input";
            inputValue.onkeyup = function (event) { Fanex.FormatNumber(event, true); };
            inputValue.value = ''.FormatNumber(betLimitValue);
            betSettingContainer.appendChild(inputValue);

            var constrainBetSettingContainer = document.createElement("TD");
            constrainBetSettingContainer.className = "bet-limit-constrain";
            constrainBetSettingContainer.innerHTML = constrainText;
            parentElement.appendChild(constrainBetSettingContainer);
        }
        return inputValue;
    }
}