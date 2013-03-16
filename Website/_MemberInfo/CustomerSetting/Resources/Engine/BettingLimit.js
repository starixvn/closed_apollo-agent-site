/// <reference path="Languages/en.js" />
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
		var c = this.items.length;
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
			if (item.SportId == 161) {
				if (validateExpr.test(item.maxPerBallElement.value) == false) {
					this.ValidateMessage(item.maxPerBallElement, Language.INCORRECT_MAX_PER_BALL);
					return false;
				}
				var maxPerBall = parseFloat(item.maxPerBallElement.value, true);
				// Check maxperball_min < maxperball < maxperball_max
				if (typeof (item.MaxPerBallMin) != 'undefined' && item.MaxPerBallMin > 0 && maxPerBall > item.MaxPerBallMin && maxPerBall != 0) {
					this.ValidateMessage(item.maxPerBallElement, Language.MAX_PER_BALL_LOWER_THAN_MAX_PER_BALL_MAX_BET.Format(''.FormatNumber(item.MaxPerBallMin)), -180, 200);
					return false;
				}
			}
		}
		// Success
		return true;
	}
	this.Save = function () {
		var bettingLimits = [];
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
		return bettingLimits;
	}

	this.Render = function (owner, hideTextBox) {
		var container = document.createElement("DIV");
		container.className = "betting-limit-container-header";

		var header = document.createElement("DIV");
		header.innerHTML = Language.BET_SETTING_HEADER;
		header.className = "pt-grid-header";
		var saveButton = document.createElement("SPAN");
		saveButton.className = "save-component-setting";
		header.appendChild(saveButton);
		container.appendChild(header);

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
		var maxPerBallHeader = document.createElement("DIV");
		maxPerBallHeader.innerHTML = Language.MAX_PER_BALL;
		maxPerBallHeader.className = "betting-limit-header-top";
		box.appendChild(maxPerBallHeader);
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
			var showLimitBox = Fanex.CustSetting.CURRENT_CONTEXT.RoleId != 3; //typeof (item.MaxBetMin) != 'undefined';
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
			if (hideTextBox) {
				var minBetBox = document.createElement("DIV");
				minBetBox.innerHTML = ''.FormatNumber(item.MinBet);
				minBetBox.className = "BettingLimitLabel";
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
			if (hideTextBox) {
				var maxBetBox = document.createElement("DIV");
				maxBetBox.innerHTML = ''.FormatNumber(item.MaxBet);
				maxBetBox.className = "BettingLimitLabel";
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
			if (hideTextBox) {
				var maxPerMatchBox = document.createElement("DIV");
				maxPerMatchBox.innerHTML = ''.FormatNumber(item.MaxPerMatch);
				maxPerMatchBox.className = "BettingLimitLabel";
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
			item.element = box2;
			box.appendChild(box2);
			var clear = document.createElement("DIV");
			clear.style.clear = "both";
			box.appendChild(clear);
		}
		box.className = (showLimitBox || hideTextBox) ? "betting-limit" : "betting-limit4";
		if (typeof (owner) == 'object') {
			if (!hideTextBox) {
				var headerSoccer = this.items[0].header;
				var headerBasketBall = this.items[1].header;
				headerSoccer.className = headerBasketBall.className = "betting-limit-header-copied";
				headerSoccer.title = Language.COPY_ALL_BETTING_LIMIT_FROM.Format(Language.SOCCER);
				headerBasketBall.title = Language.COPY_ALL_BETTING_LIMIT_FROM.Format(Language.BASKETBALL);
				var items = this.items;
				items[0].active = items[1].active = false;
				// Handle event copy from soccer
				headerSoccer.onclick = function () {
					var c = items.length - 1;
					if (items[0].active === false) { // Uncheck --> Checked
						items[0].active = true;
						headerSoccer.className = "betting-limit-header-copied-active";
						if (items[1].active === true) { // Untick basket ball
							items[1].active = false;
							headerBasketBall.className = "betting-limit-header-copied";
						}
						for (var i = 1; i < c; i++) {
							var item = items[i];
							item.copiedItem = {
								SportId: 0
							}; // Copied from Soccer, using index

							item.inactive = true;
							Fanex.CustSetting.DisableChildren(item.element, "bet-type-inactive");
						}
					} else {
						items[0].active = false;
						headerSoccer.className = "betting-limit-header-copied";
						for (var i = 1; i < c; i++) {
							var item = items[i];
							item.copiedItem = null; // Remove copiedItem
							if (item.inactive === false) break;
							item.inactive = false;
							Fanex.CustSetting.ReEnableChildren(item.element);
						}
					}
				}
				// Handle event copy from basket ball
				headerBasketBall.onclick = function () {
					if (items[1].inactive == true) { // Re-enable current
						items[1].inactive = false;
						items[1].copiedItem = null; // Remove copiedItem
						Fanex.CustSetting.ReEnableChildren(items[1].element);
					}
					var c = items.length - 1;
					if (items[1].active === false) { // Uncheck --> Checked
						items[1].active = true;
						headerBasketBall.className = "betting-limit-header-copied-active";
						if (items[0].active === true) { // Untick soccer
							items[0].active = false;
							headerSoccer.className = "betting-limit-header-copied";
						}
						for (var i = 2; i < c; i++) {
							var item = items[i];
							item.copiedItem = {
								SportId: 1
							}; // Copied from Basketball, using index

							item.inactive = true;
							Fanex.CustSetting.DisableChildren(item.element, "bet-type-inactive");
						}
					} else {
						items[1].active = false;
						headerBasketBall.className = "betting-limit-header-copied";
						for (var i = 2; i < c; i++) {
							var item = items[i];
							item.copiedItem = null; // Remove copiedItem
							if (item.inactive === false) break;
							item.inactive = false;
							Fanex.CustSetting.ReEnableChildren(item.element);
						}
					}
				}
			}
			container.appendChild(box);
			owner.appendChild(container);
		}
		return box;
	}
}