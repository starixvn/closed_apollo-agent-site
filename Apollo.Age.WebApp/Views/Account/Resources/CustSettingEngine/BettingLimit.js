/// <reference path="Languages/en.js" />
/// <reference path="PTSetting.js" />
/*global Language*/
/* jshint maxlen:false */

if (typeof (Fanex) === 'undefined') { // Require Fanex namespace
    Fanex = {};
}
if (typeof (Fanex.CustSetting) === 'undefined') { // Require Fanex.CustSetting namespace
    alert('Require Fanex.CustSetting namespace');
}
if (typeof (Validators) === 'undefined') { // Require Validators library
    alert('Require validators library');
}
Fanex.CustSetting.BettingLimit = function (bettingLimitItems) {
	'use strict';
	this.items = bettingLimitItems;
	var validateExpr = /^(\$|)([0-9]\d{0,2}(\,\d{3})*)$/;
	this.Validate = function () {
		if (Fanex.CustSetting.CURRENT_CONTEXT.FormId === Fanex.CustSetting.FormTypes.EDIT_NON_DIRECT) {
			return true;
		}

		var c = this.items.length;
		var isSimpleMode = Fanex.CustSetting.CURRENT_CONTEXT.IsSimpleMode;
		if (isSimpleMode) {
			c = 1;
		}
		var minBet;
		var maxBet;
		var maxPerMatch;
		for (var i = 0; i < c; i++) {
			var item = this.items[i];
			var copiedItem = item.copiedItem;

			// Validate each sport type
			if (copiedItem == null) { // Copy checkbox was ticked
				if (validateExpr.test(item.maxPerMatchElement.value) === false) {
					Fanex.ValidateMessage(item.maxPerMatchElement, Language.INCORRECT_MAX_PER_MATCH, 0, 100);
					return false;
				}
				if (validateExpr.test(item.maxBetElement.value) === false) {
					Fanex.ValidateMessage(item.maxBetElement, Language.INCORRECT_MAX_BET, 0, 100);
					return false;
				}

				if (validateExpr.test(item.minBetElement.value) === false) {
					Fanex.ValidateMessage(item.minBetElement, Language.INCORRECT_MIN_BET, 0, 100);
					return false;
				}

				// Check min bet < max bet < max per match
				minBet = parseFloat(item.minBetElement.value, true);
				maxBet = parseFloat(item.maxBetElement.value, true);
				maxPerMatch = parseFloat(item.maxPerMatchElement.value, true);

				if (maxBet > maxPerMatch) {
					Fanex.ValidateMessage(item.maxPerMatchElement, Language.MAX_BET_LOWER_THAN_MIN_MAX_PER_MATCH_BET.Format(item.Name));
					return false;
				}

				if (maxBet !== 0 && maxBet < maxBetLimit) {
					Fanex.ValidateMessage(item.maxBetElement, Language.BET_SETTING_LIMIT_MIN_SINGBET_MESSAGE + " " + Fanex.FormatNumberFloat(maxBetLimit));
					return false;
				}

				if (minBet > maxPerMatch) {
					Fanex.ValidateMessage(item.maxPerMatchElement, Language.MIN_BET_LOWER_THAN_MIN_MAX_PER_MATCH_BET.Format(item.Name));
					return false;
				}

				if (minBet !== 0 && maxBet < maxBetLimit) {
					Fanex.ValidateMessage(item.minBetElement, Language.BET_SETTING_LIMIT_MIN_SINGGAME_MESSAGE + " " + Fanex.FormatNumberFloat(maxBetLimit));
					return false;
				}

				if (minBet > maxBet) {
					Fanex.ValidateMessage(item.maxBetElement, Language.MIN_BET_LOWER_THAN_MIN_MAX_BET.Format(item.Name));
					return false;
				}

				if (Fanex.CustSetting.CURRENT_CONTEXT.TargetRoleId !== Fanex.CustSetting.RoleType.MainShareHolder) {
					// Check maxpermatch_min < maxpermatch < maxpermatch_max
					if (typeof (item.MaxPerMatchMin) !== 'undefined' && maxPerMatch > item.MaxPerMatchMin) {
						Fanex.ValidateMessage(item.maxPerMatchElement, Language.MAX_PER_MATCH_LOWER_THAN_MAX_PER_MATCH_MAX_BET.Format(''.FormatNumber(item.MaxPerMatchMin)), -130, 160);
						return false;
					}

					// Check maxbet < maxbetmin
					if (typeof (item.MaxBetMin) !== 'undefined' && maxBet > item.MaxBetMin) {
						Fanex.ValidateMessage(item.maxBetElement, Language.MAX_BET_LOWER_THAN_MAX_MAX_BET.Format(''.FormatNumber(item.MaxBetMin)));
						return false;
					}

					// Check minbet > minbetmax
					if (typeof (item.MinBetMax) !== 'undefined' && minBet < item.MinBetMax) {
						Fanex.ValidateMessage(item.minBetElement, Language.MIN_BET_GREATER_THAN_MIN_MIN_BET.Format(''.FormatNumber(item.MinBetMax)));
						return false;
					}
				}
			}
			else {
				var copyData = this.items[copiedItem.SportId];
				minBet = parseFloat(copyData.minBetElement.value, true);
				maxBet = parseFloat(copyData.maxBetElement.value, true);
				maxPerMatch = parseFloat(copyData.maxPerMatchElement.value, true);
				if (minBet > maxBet) {
					Fanex.ValidateMessage(item.minBetElement, Language.MIN_BET_LOWER_THAN_MIN_MAX_BET_AFTER_COPY.Format(item.Name));
					return false;
				}
				if (minBet > maxPerMatch) {
					Fanex.ValidateMessage(item.minBetElement, Language.MIN_BET_LOWER_THAN_MIN_MAX_PER_MATCH_BET_AFTER_COPY.Format(item.Name));
					return false;
				}
				if (maxBet > maxPerMatch) {
					Fanex.ValidateMessage(item.maxBetElement, Language.MAX_BET_LOWER_THAN_MIN_MAX_PER_MATCH_BET_AFTER_COPY.Format(item.Name));
					return false;
				}
				// Check minbet < minbetmax
				if (typeof (item.MinBetMax) !== 'undefined' && item.MinBetMax > 0 && minBet < item.MinBetMax && minBet !== 0) {
					Fanex.ValidateMessage(item.minBetElement, Language.MIN_BET_GREATER_THAN_MIN_MIN_BET_AFTER_COPY.Format(''.FormatNumber(item.MinBetMax)));
					return false;
				}
				// Check maxbet < maxbetmin
				if (typeof (item.MaxBetMin) !== 'undefined' && item.MaxBetMin > 0 && maxBet > item.MaxBetMin && maxBet !== 0) {
					Fanex.ValidateMessage(item.maxBetElement, Language.MAX_BET_LOWER_THAN_MAX_MAX_BET_AFTER_COPY.Format(''.FormatNumber(item.MaxBetMin)));
					return false;
				}
				// Check maxpermatch_min < maxpermatch < maxpermatch_max
				if (typeof (item.MaxPerMatchMin) !== 'undefined' && item.MaxPerMatchMin > 0 && maxPerMatch > item.MaxPerMatchMin && maxPerMatch !== 0) {
					Fanex.ValidateMessage(item.maxPerMatchElement, Language.MAX_PER_MATCH_LOWER_THAN_MAX_PER_MATCH_MAX_BET_AFTER_COPY.Format(''.FormatNumber(item.MaxPerMatchMin)), -130, 160);
					return false;
				}
			}
		}
		// Success
		return true;
	};
	this.Save = function () {
		var bettingLimits = [];
		var currentContext = Fanex.CustSetting.CURRENT_CONTEXT;
		if (currentContext.FormId === Fanex.CustSetting.FormTypes.EDIT_NON_DIRECT) { return null; }

		var isEditMultiple = currentContext.FormId === Fanex.CustSetting.FormTypes.EDIT_MULTIPLE;
		var minBetValue;
		var maxBetValue;
		var maxPerMatchValue;
		var c = this.items.length;
		var item;
		var i;

		if (currentContext.IsSimpleMode) {
			//Fanex.RoundNumber(parseFloat(this.items[copiedItem.SportId].minBetElement.value, true), 1)
			minBetValue = Fanex.RoundNumber(parseFloat(this.items[0].minBetElement.value, true), 1);
			maxBetValue = Fanex.RoundNumber(parseFloat(this.items[0].maxBetElement.value, true), 1);
			maxPerMatchValue = Fanex.RoundNumber(parseFloat(this.items[0].maxPerMatchElement.value, true), 1);

			for (i = 0; i < c; i++) {
				item = this.items[i];
				if (minBetValue !== item.MinBet
                    || maxBetValue !== item.MaxBet
                    || maxPerMatchValue !== item.MaxPerMatch
                    || isEditMultiple) {
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
			for (i = 0; i < c; i++) {
				item = this.items[i];
				var bettingLimitItem = null;
				var copiedItem = item.copiedItem;
				minBetValue = 0;
				maxBetValue = 0;
				maxPerMatchValue = 0;

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
					if (minBetValue !== item.MinBet
						|| maxBetValue !== item.MaxBet
						|| maxPerMatchValue !== item.MaxPerMatch
						|| isEditMultiple) {
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

				if (bettingLimitItem) {
					bettingLimits.push(bettingLimitItem);
				}
			}
		}

		return bettingLimits;
	};

	this.Render = function (owner) {
		var context = Fanex.CustSetting.CURRENT_CONTEXT;
		var isEditNonDirectMember = (context.FormId === Fanex.CustSetting.FormTypes.EDIT_NON_DIRECT) || (context.FormId === Fanex.CustSetting.FormTypes.VIEW_SETTING || Fanex.CustSetting.CURRENT_CONTEXT.FormId === Fanex.CustSetting.FormTypes.SHADOW_VIEW);

		var container = document.createElement("DIV");
		container.className = context.IsSimpleMode ? "" : "betting-limit-container-header";

		var header = document.createElement("DIV");
		header.innerHTML = Language.BET_SETTING_HEADER;
		header.className = "pt-grid-header";
		var informationButton = document.createElement("SPAN");
		informationButton.className = "section-info";
		if (context.TargetRoleId === Fanex.CustSetting.RoleType.Member) {
			informationButton.id = "member-";
		}
		else if (context.TargetRoleId === Fanex.CustSetting.RoleType.MainShareHolder) {
			informationButton.id = "mainshareholder-";
		}
		else {
			informationButton.id = "agent-";
		}
		if (context.FormId === Fanex.CustSetting.FormTypes.VIEW_SETTING) {
			informationButton.id = "view-" + informationButton.id;
		}
		informationButton.id += "bet-limit-info-button" + (context.IsSimpleMode ? "" : "-adv");

		header.appendChild(informationButton);
		container.appendChild(header);

		var box;

		if (context.IsSimpleMode === false) {
			box = document.createElement("DIV");
			// Create MinBet, MaxBet, MaxPerMatch header
			var emptyBox = document.createElement("DIV");
			emptyBox.innerHTML = "&nbsp;";
			emptyBox.className = "betting-limit-header";
			box.appendChild(emptyBox);
			var maxPerMatchHeader = document.createElement("DIV");
			maxPerMatchHeader.innerHTML = Language.MAX_PER_MATCH;
			maxPerMatchHeader.className = "betting-limit-header-top";
			box.appendChild(maxPerMatchHeader);
			var maxBetHeader = document.createElement("DIV");
			maxBetHeader.innerHTML = Language.MAX_BET;
			maxBetHeader.className = "betting-limit-header-top";
			box.appendChild(maxBetHeader);
			var minBetHeader = document.createElement("DIV");
			minBetHeader.innerHTML = Language.MIN_BET;
			minBetHeader.className = "betting-limit-header-top";
			box.appendChild(minBetHeader);

			var clear = document.createElement("DIV");
			clear.style.clear = "both";
			box.appendChild(clear);
			var showLimitBox = false;
			var c = this.items.length;
			for (var i = 0; i < c; i++) {
				var item = this.items[i];
				var box2 = document.createElement("DIV");
				if (typeof (item.MinBet) === 'undefined') { // For Add new case
					item.MinBet = item.MinBetMax;
				}
				if (typeof (item.MaxBet) === 'undefined') { // For Add new case
					item.MaxBet = item.MaxBetMin;
				}
				if (typeof (item.MaxPerMatch) === 'undefined') { // For Add new case
					item.MaxPerMatch = item.MaxPerMatchMin;
				}

				showLimitBox = context.TargetRoleId !== Fanex.CustSetting.RoleType.MainShareHolder; //typeof (item.MaxBetMin) != 'undefined';

				header = document.createElement("DIV");
				header.innerHTML = item.Name;
				header.className = "betting-limit-header";
				item.header = header;
				box2.appendChild(header);

				var maxPerMatchBox;

				// Current value
				// Show label instead to TextBox
				if (isEditNonDirectMember) {
					maxPerMatchBox = document.createElement("DIV");
					maxPerMatchBox.innerHTML = ''.FormatNumber(item.MaxPerMatch);
					maxPerMatchBox.className = "betting-limit-label";
					box2.appendChild(maxPerMatchBox);
					if (showLimitBox) {
						maxPerMatchHeader.className = "betting-limit-header-top";
					}
					else {
						maxPerMatchHeader.className = "betting-limit-header-top4";
					}
				}
				else {
					maxPerMatchBox = document.createElement("DIV");
					var maxPerMatch = document.createElement("INPUT");
					maxPerMatch.type = "text";
					maxPerMatch.maxLength = 14;
					maxPerMatch.onkeyup = function (event) { Fanex.FormatNumber(event); };
					maxPerMatch.onkeypress = Fanex.HandleKeyDownInputNumber;
					maxPerMatch.className = "betting-limit-textbox";
					maxPerMatch.value = ''.FormatNumber(item.MaxPerMatch);
					maxPerMatchBox.appendChild(maxPerMatch);
					maxPerMatchBox.className = "betting-limit-item";
					box2.appendChild(maxPerMatchBox);
					item.maxPerMatchElement = maxPerMatch;
					// Limit box
					var maxPerMatchMin = document.createElement("DIV");
					if (showLimitBox) {
						maxPerMatchMin.innerHTML = '<= '.FormatNumber(item.MaxPerMatchMin);
						maxPerMatchMin.className = "betting-limit-item2";
						box2.appendChild(maxPerMatchMin);
					} else {
						maxPerMatch.className = "betting-limit-textbox4";
						maxPerMatchBox.className = "betting-limit-item4";
						maxPerMatchHeader.className = "betting-limit-header-top4";
					}
				}

				var maxBetBox;

				// Current value
				// Show label instead to TextBox
				if (isEditNonDirectMember) {
					maxBetBox = document.createElement("DIV");
					maxBetBox.innerHTML = ''.FormatNumber(item.MaxBet);
					maxBetBox.className = "betting-limit-label";
					box2.appendChild(maxBetBox);
					maxBetHeader.className = "betting-limit-header-top";
				}
				else {
					maxBetBox = document.createElement("DIV");
					var maxBet = document.createElement("INPUT");
					maxBet.type = "text";
					maxBet.maxLength = 14;
					maxBet.onkeyup = function (event) { Fanex.FormatNumber(event); };
					maxBet.onkeypress = Fanex.HandleKeyDownInputNumber;
					maxBet.className = "betting-limit-textbox";
					maxBet.value = ''.FormatNumber(item.MaxBet);
					maxBetBox.appendChild(maxBet);
					maxBetBox.className = "betting-limit-item";
					box2.appendChild(maxBetBox);
					item.maxBetElement = maxBet;
					// Limit box
					var maxBetMin = document.createElement("DIV");
					if (showLimitBox) {
						maxBetMin.innerHTML = '<= '.FormatNumber(item.MaxBetMin);
						maxBetMin.className = "betting-limit-item2";
						box2.appendChild(maxBetMin);
					} else {
						maxBet.className = "betting-limit-textbox4";
						maxBetBox.className = "betting-limit-item4";
						maxBetHeader.className = "betting-limit-header-top4";
					}
				}

				var minBetBox;

				// Current value
				// Show label instead to TextBox
				if (isEditNonDirectMember) {
					minBetBox = document.createElement("DIV");
					minBetBox.innerHTML = ''.FormatNumber(item.MinBet);
					minBetBox.className = "betting-limit-label";
					box2.appendChild(minBetBox);
					minBetHeader.className = "betting-limit-header-top";
				}
				else {
					minBetBox = document.createElement("DIV");
					var minBet = document.createElement("INPUT");
					minBet.type = "text";
					minBet.maxLength = 14;
					minBet.onkeyup = function (event) { Fanex.FormatNumber(event); };
					minBet.onkeypress = Fanex.HandleKeyDownInputNumber;
					minBet.className = "betting-limit-textbox";
					minBet.value = ''.FormatNumber(item.MinBet);
					minBet.setAttribute("disabled", true);
					minBetBox.appendChild(minBet);
					minBetBox.className = "betting-limit-item";
					box2.appendChild(minBetBox);
					item.minBetElement = minBet;
					// Limit box
					var minBetMax = document.createElement("DIV");
					if (showLimitBox) {
						minBetMax.innerHTML = '>= '.FormatNumber(item.MinBetMax);
						minBetMax.className = "betting-limit-item2";
						box2.appendChild(minBetMax);
					} else {
						minBet.className = "betting-limit-textbox4";
						minBetBox.className = "betting-limit-item4";
						minBetHeader.className = "betting-limit-header-top4";
					}
				}

				item.element = box2;
				box.appendChild(box2);

				clear = document.createElement("DIV");
				clear.style.clear = "both";
				box.appendChild(clear);
			}
			box.className = (showLimitBox || isEditNonDirectMember) ? "betting-limit" : "betting-limit4";
			if (typeof (owner) === 'object') {
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
					};
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
					};
				}
				container.appendChild(box);
				owner.appendChild(container);
			}
		}
		else {
			box = document.createElement("DIV");
			box.className = "simple-mode-container";

			// Table
			var tableShareSetting = document.createElement("TABLE");
			tableShareSetting.className = "bet-limit-table";
			var boxTBody = document.createElement("TBODY");
			tableShareSetting.appendChild(boxTBody);

			box.appendChild(tableShareSetting);
			var trSetting = document.createElement("TR");
			boxTBody.appendChild(trSetting);
			// Share Settings
			var presetLabel = document.createElement("TD");
			presetLabel.innerHTML = Language.SOCCER + ':';
			presetLabel.title = Language.SOCCER;
			presetLabel.className = "bet-setting-label";
			trSetting.appendChild(presetLabel);

			var item = this.items[0];

			/// Max Per Match
			item.maxPerMatchElement = this.RenderBetLimitBlock(isEditNonDirectMember,
				trSetting,
				Language.MAX_PER_MATCH,
				context.TargetRoleId === Fanex.CustSetting.RoleType.MainShareHolder ? '' : '<= '.FormatNumber(item.MaxPerMatchMin),
				item.MaxPerMatch,
				false,
				"max-match");

			//// Max Bet
			item.maxBetElement = this.RenderBetLimitBlock(isEditNonDirectMember,
				trSetting,
				Language.MAX_BET,
				context.TargetRoleId === Fanex.CustSetting.RoleType.MainShareHolder ? '' : '<= '.FormatNumber(item.MaxBetMin),
				item.MaxBet,
				false,
				"max");

			//// Min Bet
			item.minBetElement = this.RenderBetLimitBlock(isEditNonDirectMember,
				trSetting,
				Language.MIN_BET,
				context.TargetRoleId === Fanex.CustSetting.RoleType.MainShareHolder ? '' : '>= '.FormatNumber(item.MinBetMax),
				item.MinBet,
				false,
				"min");

			container.appendChild(box);
			owner.appendChild(container);
		}
		return box;
	},
	this.RenderBetLimitBlock = function (isEditNonDirectMember, parentElement, minMaxText, constrainText, betLimitValue, isMaxPerMatch, suffixClass) {
		/// Max Per Match
		var betSettingContainer = document.createElement("TD");
		if (isEditNonDirectMember) {
			betSettingContainer.className = "bet-limit-text";
			betSettingContainer.innerHTML = minMaxText + ' = '.FormatNumber(betLimitValue);
			parentElement.appendChild(betSettingContainer);
		}
		else {
			betSettingContainer.className = "bet-limit-value-" + suffixClass;
			var divText = document.createElement("DIV");
			divText.innerHTML = minMaxText + ':';
			divText.title = minMaxText + ' ';
			divText.className = "div-bet-limit-value-" + suffixClass;
			betSettingContainer.appendChild(divText);
			parentElement.appendChild(betSettingContainer);

			var inputValue = document.createElement("INPUT");
			inputValue.type = "text";
			inputValue.className = "bet-limit-input";
			inputValue.onkeyup = function (event) { Fanex.FormatNumber(event); };
			inputValue.value = ''.FormatNumber(betLimitValue);
			betSettingContainer.appendChild(inputValue);

			if (Fanex.CustSetting.CURRENT_CONTEXT.TargetRoleId === Fanex.CustSetting.RoleType.MainShareHolder) {
				betSettingContainer.className = "bet-limit-value-mainshareholder";
				inputValue.className = "bet-limit-input-mainshareholder";
				divText.className = "div-bet-limit-value-mainshareholder";
				//constrainBetSettingContainer.className = "bet-limit-constrain-mainshareholder";
			}
			else {
				var constrainBetSettingContainer = document.createElement("TD");
				constrainBetSettingContainer.className = "bet-limit-constrain-" + suffixClass;
				constrainBetSettingContainer.innerHTML = constrainText;
				parentElement.appendChild(constrainBetSettingContainer);
			}
		}
		return inputValue;
	};
};