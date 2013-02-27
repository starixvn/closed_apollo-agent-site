Fanex.PTEngine.CURRENT_CONTEXT = context;
Fanex.PTEngine.CURRENT_CONTEXT.ia3 = !customer.statusSetting.disableAutoMPT; // Allow auto PT master on member

var ptGrid = null;
ptGrid = new Fanex.PTEngine.PTGrid(customer.ptSetting);
var div = $('ptGridContainer');
ptGrid.Render(div);

var custIds;
var userNames;
var isUpdate = false;
var isUpdatePositionTaking = false;
var bettingLimit = null;

var isChangeM3 = false;
var isShowPopUpSetPTMember = false;
var newValueMemberPT = "";
var oldPTSetting = customer.ptSetting.clone();

bettingLimit = new Fanex.PTEngine.BettingLimit(customer.bettingLimit);
var bettingLimitContainer = $('bettingLimitContainer');

bettingLimit.Render(bettingLimitContainer);

function disableChildren(element, cssApplied) { // Check dublicated disable
    if (typeof (element) != 'object') return false;
    var count = element.childNodes.length;
    if (typeof (count) == 'undefined') return false;
    for (var i = 1; i < count; i++) {
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

            Fanex.PTEngine.DisableChildren(element.childNodes[i]);
        }
    }

    return true;
}

function reEnableChildren(element, isCopyAll) { // Check dublicated enable
    if (typeof (element) != 'object') return false;
    var count = element.childNodes.length;
    var j = 1;
    if (isCopyAll == 1) {
        count = 1;
        j = 0;
    }

    if (typeof (count) == 'undefined') return false;
    for (var i = j; i < count; i++) {
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

            Fanex.PTEngine.ReEnableChildren(element.childNodes[i], 0);
        }
    }

    return true;
}

function disableCommission(isDisable) {
    if ($("discountAsian")) {
        $("discountAsian").disabled = isDisable;
    }

    if ($("discount1x2")) {
        $("discount1x2").disabled = isDisable;
    }

    if ($("discountCS")) {
        $("discountCS").disabled = isDisable;
    }

    if ($("discountNumber")) {
        $("discountNumber").disabled = isDisable;
    }
}

function disableSportType(id, isDisable) {
    if (typeof (id) == 'undefined') return false;
    if (ptGrid.items.length <= 0) return false;

    var betTypeGroupSets =  ptGrid.items[id].items;
    for (var j = 0; j < betTypeGroupSets.length; j++) {
        var betTypeGroups = betTypeGroupSets[j].items;
        isDisable ? betTypeGroupSets[j].copiedItem = {} : betTypeGroupSets[j].copiedItem = null;
        for (var k = 0; k < betTypeGroups.length; k++) {
            var betTypes = betTypeGroups[k].items;
            for (var l = 0; l < betTypes.length; l++) {
                var idCopied = "c$" + betTypeGroups[k].betTypes[0].id + "$" + betTypeGroups[k].betTypes[0].sportId + "$" + (betTypeGroups[k].isLive ? "0" : "1");
                if (isDisable) {
                    if (betTypes[l].actived == true){
                        Fanex.PTEngine.DisableChildren(betTypes[l].element, "BetTypeInactive");
                        betTypes[l].actived = false;
                        
                        if ($(idCopied)) {
                            $(idCopied).label.className = "BetTypeHeader2 BetTypeInactive";
                            $(idCopied).checked = false;
                            betTypeGroups[k].copiedItem = null;
                            betTypeGroups[k].copiedFromFirst = false;
                        }
                    }
                }
                else{
                    if (betTypes[l].actived == false) {
                        Fanex.PTEngine.ReEnableChildren(betTypes[l].element);
                        betTypes[l].actived = true;
                        if ($(idCopied)) {
                            $(idCopied).label.className = "BetTypeHeader2";
                        }
                    }
                }
            }
        }
    }

    return true;
}

function disablePositionTaking() {
    if (ptGrid.items.length <= 0) return false;
    for (var i = 0; i < ptGrid.items.length; i++) {
        var betTypeGroupSets = ptGrid.items[i].items;
        for (var j = 0; j < betTypeGroupSets.length; j++) {
            var betTypeGroups = betTypeGroupSets[j].items;
            betTypeGroupSets[j].copiedItem = {};
            for (var k = 0; k < betTypeGroups.length; k++) {
                var betTypes = betTypeGroups[k].items;
                for (var l = 0; l < betTypes.length; l++) {
                    if (betTypes[l].actived == true) {
                        Fanex.PTEngine.DisableChildren(betTypes[l].element, "BetTypeInactive");
                        betTypes[l].actived = false;
                    }
                }
            }
        }
    }

    return true;
}

function getUpdateParamaters() {
    var paramaters = "";
    var items = bettingLimit.items;

    if($("status").checked)
    {
        paramaters += "1";
        if(!isUpdate){
            isUpdate = true;
        }
    }
    else
    {
        paramaters += "0";
    }

    if ($("commission").checked) {
        paramaters += "1";
        if (!isUpdate) {
            isUpdate = true;
        }
    }
    else {
        paramaters += "0";
    }   

    if ($("bettingLimit0").checked && items[0].active === true) {
        for (var i = 0; i < items.length - 1; i++) {
            paramaters += "1";
            if (!isUpdate) {
                isUpdate = true;
            }
        }

        $("bettingLimit9").checked ? paramaters += "1" : paramaters += "0";
    }
    else if ($("bettingLimit1").checked && items[1].active === true) {
        $("bettingLimit0").checked ? paramaters += "1" : paramaters += "0";
        for (var i = 1; i < items.length - 1; i++) {
            paramaters += "1";
            if (!isUpdate) {
                isUpdate = true;
            }
        }

        $("bettingLimit9").checked ? paramaters += "1" : paramaters += "0";
    }
    else {
        for (var i = 0; i < items.length; i++) {
            if ($("bettingLimit" + i).checked) {
                paramaters += "1";
                if (!isUpdate) {
                    isUpdate = true;
                }
            }
            else {
                paramaters += "0";
            }   
        }
    }

    var itemsPt = ptGrid.items;
    var cloneFromSoccer = itemsPt[0].cloneButton;
    var cloneFromBasketball = itemsPt[1].cloneButton;
    if ($("positionTaking0").checked && cloneFromSoccer.firstChild.checked) {
        isUpdatePositionTaking = true;
        for (var i = 0; i < itemsPt.length; i++) {
            paramaters += "1";
            if (!isUpdate) {
                isUpdate = true;
            }
        }
    }
    else if ($("positionTaking1").checked && cloneFromBasketball.firstChild.checked) {
        $("positionTaking0").checked ? paramaters += "1" : paramaters += "0";
        isUpdatePositionTaking = true;
        for (var i = 1; i < itemsPt.length; i++) {
            paramaters += "1";
            if (!isUpdate) {
                isUpdate = true;
            }
        }
    }
    else {
        for (var i = 0; i < itemsPt.length; i++) {
            if($("positionTaking" + i).checked)
            {
                paramaters += "1";
                if (!isUpdatePositionTaking) {
                    isUpdatePositionTaking = true;
                }

                if (!isUpdate) {
                    isUpdate = true;
                }
            }
            else{
                paramaters += "0";
            }
        }
    }

    return paramaters;
}

function initBettingLimit() {
    var items = bettingLimit.items;
    var c = items.length;
    for (var i = 0; i < c; i++) {
        var item = items[i];
        disableChildren(item.element, "BetTypeInactive");
    }

    var headerSoccer = items[0].header;
    var headerBasketBall = items[1].header;
    headerSoccer.onclick = function () {
        if ($('bettingLimit0').checked) {
            if (items[0].active === false) { // Uncheck --> Checked
                items[0].active = true;
                headerSoccer.className = "BettingLimitHeaderCopiedActive";
                if (items[1].active === true) { // Untick basket ball
                    items[1].active = false;
                    headerBasketBall.className = "BettingLimitHeaderCopied";
                }

                for (var i = 1; i < c - 1; i++) {
                    var item = items[i];
                    item.copiedItem = {
                        id: 0
                    }; // Copied from Soccer, using index

                    item.inactive = true;
                    var checkBox = 'bettingLimit' + i;
                    $(checkBox).checked = false;
                    Fanex.PTEngine.DisableChildren(item.element, "BetTypeInactive");
                }
            } else {
                items[0].active = false;
                headerSoccer.className = "BettingLimitHeaderCopied";
                for (var i = 1; i < c; i++) {
                    var item = items[i];
                    item.copiedItem = null; // Remove copiedItem
                    if (item.inactive === false) break;
                    item.inactive = false;
                    reEnableChildren(item.element, 1);
                }
            }
        }
    };

    headerBasketBall.onclick = function () {
        if ($('bettingLimit1').checked) {
            if (items[1].inactive == true) { // Re-enable current
                items[1].inactive = false;
                items[1].copiedItem = null; // Remove copiedItem
                Fanex.PTEngine.ReEnableChildren(items[1].element);
            }
            var c = items.length - 1;
            if (items[1].active === false) { // Uncheck --> Checked
                items[1].active = true;
                headerBasketBall.className = "BettingLimitHeaderCopiedActive";
                if (items[0].active === true) { // Untick soccer
                    items[0].active = false;
                    headerSoccer.className = "BettingLimitHeaderCopied";
                }

                for (var i = 2; i < c; i++) {
                    var item = items[i];
                    item.copiedItem = {
                        id: 1
                    }; // Copied from Basketball, using index

                    item.inactive = true;
                    var checkBox = 'bettingLimit' + i;
                    $(checkBox).checked = false;
                    Fanex.PTEngine.DisableChildren(item.element, "BetTypeInactive");
                }
            } else {
                items[1].active = false;
                headerBasketBall.className = "BettingLimitHeaderCopied";
                for (var i = 2; i < c; i++) {
                    var item = items[i];
                    item.copiedItem = null; // Remove copiedItem
                    if (item.inactive === false) break;
                    item.inactive = false;
                    var checkBox = 'bettingLimit' + i;
                    reEnableChildren(item.element, 1);
                }
            }
        }
    };
}

function initPositionTaking() {
    var sportTypes = ptGrid.items;
    var cloneFromSoccer = sportTypes[0].cloneButton;
    var cloneFromBasketball = sportTypes[1].cloneButton;
    var editBasketball = sportTypes[1].editButton;
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

    disablePositionTaking();

    cloneFromSoccer.onclick = function () {
        if ($('positionTaking0').checked) {
            Validators.hide();
            var count = sportTypes.length;
            cloneFromSoccer.firstChild.checked = !cloneFromSoccer.firstChild.checked;
            cloneFromSoccer.parent.copiedItem = copiedItemSoccer;
            cloneFromSoccer.parent.activeCopiedTitle = "{0} {1}".Format(sportTypes[0].name, sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].name);
            cloneFromSoccer.className = cloneFromSoccer.firstChild.checked ? "PTSportCopiedActive" : "PTSportCopied";
            cloneFromSoccer.parent.items[0].Toggle(true); // Expand
            // Rollback Basketball
            if (cloneFromBasketball.firstChild.checked) {
                cloneFromBasketball.className = "PTSportCopied";
                cloneFromBasketball.firstChild.checked = false;
                cloneFromBasketball.title = Language.COPY_ALL_OTHER_POSITION_TAKING_FROM.Format(sportTypes[1].name, sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].name);
            }
            // Show / Hide edit button of Basketball
            if (cloneFromSoccer.firstChild.checked) {
                editBasketball.className = "PTSportEdit";
                editBasketball.title = Language.EDIT;
                cloneFromSoccer.title = String.Empty;
            } else {
                editBasketball.className = "PTSportCopiedInactive";
                editBasketball.title = String.Empty;
                cloneFromSoccer.title = Language.COPY_ALL_OTHER_POSITION_TAKING_FROM.Format(sportTypes[0].name, sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].name);
            }

            for (var i = 0; i < count; i++) {
                if (cloneFromSoccer.firstChild.checked) { 
                    if (i > 0) {
                        var id = 'positionTaking' + i;
                        $(id).checked = false;
                        $(id).disabled = true;
                        disableSportType(i, true);
                    }

                    sportTypes[i].Inactive(copiedItemSoccer.clone());
                }
                else if (i > 0) {
                    var id = 'positionTaking' + i;
                    $(id).disabled = false;
                    disableSportType(i, true);
                }

                if (i > 1) {
                    sportTypes[i].cloneButton.className = cloneFromSoccer.firstChild.checked ? "PTSportEdit" : "PTSportCopiedInactive";
                    sportTypes[i].cloneButton.title = cloneFromSoccer.firstChild.checked ? Language.EDIT : String.Empty;
                }
            };

            if (!cloneFromSoccer.firstChild.checked) {
                sportTypes[0].Active();
                cloneFromSoccer.parent.copiedItem = null;
            }

            cloneFromSoccer.parent.ShowCloneHint();
        }
    };

    // Basketball
    cloneFromBasketball.onclick = function () {
        if ($('positionTaking1').checked) {
            Validators.hide();
            var count = sportTypes.length;
            cloneFromBasketball.firstChild.checked = !cloneFromBasketball.firstChild.checked;
            cloneFromBasketball.parent.copiedItem = copiedItemBasketball;
            cloneFromBasketball.parent.activeCopiedTitle = "{0} {1}".Format(sportTypes[1].name, sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].name);
            cloneFromBasketball.className = cloneFromBasketball.firstChild.checked ? "PTSportCopiedActive" : "PTSportCopied";
            cloneFromBasketball.parent.items[1].Toggle(true); // Expand
            // Rollback Soccer
            if (cloneFromSoccer.firstChild.checked) {
                sportTypes[0].Active();
                cloneFromSoccer.className = "PTSportCopied";
                cloneFromSoccer.firstChild.checked = false;
                cloneFromSoccer.title = Language.COPY_ALL_OTHER_POSITION_TAKING_FROM.Format(sportTypes[0].name, sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].name);
            };
            // Hide edit button of Basketball
            if (cloneFromBasketball.firstChild.checked) {
                editBasketball.className = "PTSportCopiedInactive";
                cloneFromBasketball.title = String.Empty;
            } else {
                cloneFromBasketball.title = Language.COPY_ALL_OTHER_POSITION_TAKING_FROM.Format(sportTypes[1].name, sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].name);
            }

            for (var i = 1; i < count; i++) {
                if (cloneFromBasketball.firstChild.checked) {
                    disableSportType(i, true);
                    sportTypes[i].Inactive(copiedItemBasketball.clone());
                    if (i > 1) {
                        var id = 'positionTaking' + i;
                        $(id).checked = false;
                        $(id).disabled = true;
                    }
                }
                else if (i > 1) {
                    var id = 'positionTaking' + i;
                    $(id).disabled = false;
                }

                if (i > 1) {
                    sportTypes[i].cloneButton.className = cloneFromBasketball.firstChild.checked ? "PTSportEdit" : "PTSportCopiedInactive";
                    sportTypes[i].cloneButton.title = cloneFromBasketball.firstChild.checked ? Language.EDIT : String.Empty;
                    disableSportType(i, true);
                }
            };

            if (!cloneFromBasketball.firstChild.checked) {
                sportTypes[1].Active();
                cloneFromBasketball.parent.copiedItem = null;
            }

            cloneFromBasketball.parent.ShowCloneHint();
        }
    };
}

function init() {
    $('closed').disabled = true;
    disableCommission(true);
    initBettingLimit();
    initPositionTaking();
}

function saveCommissionSettings(commissionSettings) {
    customer.commissionSetting.playerDiscount = $("discountAsian").value;
    customer.commissionSetting.playerDiscount1x2 = $("discount1x2").value;
    customer.commissionSetting.playerDiscountCs = $("discountCS").value;
    customer.commissionSetting.playerDiscountNumber = $("discountNumber").value;
}

//console.log(customer);
/*
Bind customer information to customer JSON object by knockout.js
Render PTGrid
*/
var CustomerModel = function (customer) {
    initArrays();
    customer.userNames = userNames.join(", ");

    this.saveCustomer = function () {
        if (!bettingLimit.Validate()) return;
        saveCommissionSettings(customer.commissionSetting);

        if (ptGrid) {
            var url = "";
            var updateParameters = "";
            updateParameters = getUpdateParamaters();

            if (!isUpdate) {
                age.ReloadPage(300000);
                top.main.DelayReloadPage();

                return; 
            }

            if (isUpdatePositionTaking) {
                customer.ptSetting = ptGrid.Save();
            }

            if (context.targetCustId > 0 && context.roleId != 2) {
                if (!isShowPopUpSetPTMember) {
                    isChangeM3 = CheckChangeM3(oldPTSetting, customer.ptSetting);
                }

                if (isChangeM3 && !isShowPopUpSetPTMember) {
                    Popups.Confirm(MemberMaximumPT(), Language.CONFIRM_MASTER_NOT_TO_TAKE_AUTO_POSITION_TAKING);
                    return;
                }
                else if (isChangeM3 && isShowPopUpSetPTMember) {
                    Popups.Close();
                }
            }

            if (context.roleId == 4) {
                if (newValueMemberPT != "") {
                    url = "../Handlers/SuperUpdateMaster.ashx?memberPositionTaking=" + newValueMemberPT;
                }
                else {
                    url = "../Handlers/SuperUpdateMaster.ashx";
                }
            }
            else if (context.roleId == 3) {
                if (newValueMemberPT != "") {
                    url = "../Handlers/MasterUpdateAgent.ashx?memberPositionTaking=" + newValueMemberPT;
                }
                else {
                    url = "../Handlers/MasterUpdateAgent.ashx";
                }
            }
            else {
                url = "../Handlers/AgentUpdateMember.ashx";
            }

            customer.bettingLimit = bettingLimit.Save();
            updateSettings(url, updateParameters);
        }
    };
};

var isSuccess = true;
function updateSettings(url, updateParameters) {
    isSuccess = true;
    age.ReloadPage(300000);
    updateCustSettings(0, url, updateParameters);
}

function updateCustSettings(id, url, updateParameters) {
    var custId = custIds[id];
    var userName = userNames[id];

    customer.custId = custId;
    customer.userName = userName;
    var text = JSON.stringify(customer);
    $("txtCustomerObject").value = text;
    var parameters = ajax.CreateParams('txtCustomerObject');
    parameters += '&updateParameters=' + updateParameters;

    ajax.PostJSON(url, parameters, function onComplete(result) {
        var ArrCodes = new Array();
        var ArrMsgs = new Array();
        var ArrUserName = new Array();
        ArrUserName.push(userName);

        if (result.errCode == 0) {
            ArrCodes.push(-2);
            ArrMsgs.push(result.errMsg);
            if (id == custIds.length - 1 && isSuccess == true) {
                top.main.DelayReloadPage();
            }
        }
        else {
            isSuccess = false;
            if (result.errCode == -1 && result.errMsg.indexOf('Oops') == -1) {
                ArrCodes.push(9999);
            }
            else {
                ArrCodes.push(result.errCode);
            }

            ArrMsgs.push(result.errMsg);
        }

        Fanex.Customer.GenerateMessage(ArrCodes, ArrMsgs, ArrUserName, id != 0);

        id++;
        if (id < custIds.length) {
            updateCustSettings(id, url, updateParameters);
        }
        else {
            isUpdate = false;
            age.UnLock();
            window.scroll(0, 0);
        }
    });
}

Fanex.Customer.EditMultiple = {
    // Check or UnCheck Close Status
    Closed: function (checkBoxClose, currentStatusClose) {
        if (checkBoxClose.checked == true && checkBoxClose.checked != currentStatusClose) {
            if (!confirm(context.roleId == 1 ? Language.ALERT_CONFIRM_CLOSE_MEMBER : Language.ALERT_CONFIRM_CLOSE)) {
                customer.statusSetting.closed = false;
                checkBoxClose.checked = false;
            }
        }
        else if (checkBoxClose.checked == false && checkBoxClose.checked != currentStatusClose) {
            if (!confirm(Language.ALERT_CONFIRM_CLOSE_MEMBER)) {
                customer.statusSetting.closed = true;
                checkBoxClose.checked = true;
            }
        }
    },

    // Load Group Commission
    LoadGroupCommission: function (selectId, currencyId, siteName) {
        var listGroupCommission;
        // If Currency name is Won
        if (currencyId == 45) {
            listGroupCommission = new Array("dea", "deb", "dec", "ded");
        }
        else if (siteName == "IBSBET") {
            listGroupCommission = new Array("a", "b", "c", "d", "indoa", "indob", "indoc", "indod");
        }
        else {
            listGroupCommission = new Array("a", "b", "c", "d", "hka", "hkb", "hkc", "hkd");
        }

        for (var i = 0; i < listGroupCommission.length; i++) {
            var selectedOpt = document.createElement("option");
            selectedOpt.text = selectedOpt.value = listGroupCommission[i];
            selectId.options.add(selectedOpt);
        }
    },

    // Set Asian HDP, OU, OE selected value Commission for Member
    SelectedAsianCommissionMember: function (value) {
        Fanex.PTEngine.PT_SetSelect(document.getElementById("discountAsian"), 0, value, Fanex.PTEngine.COMMISSION_STEP, value);
    },

    // Change Group Commission
    ChangeGroupCommission: function (group) {
        if ((group.options[group.selectedIndex].text == "a") || (group.options[group.selectedIndex].text == "hka")
		|| (group.options[group.selectedIndex].text == "indoa") || (group.options[group.selectedIndex].text == "dea")) {
            $("textBoxDiscountAsian").value = commission.groupA;
            this.SelectedAsianCommissionMember(commission.groupA)
        }
        if ((group.options[group.selectedIndex].text == "b") || (group.options[group.selectedIndex].text == "hkb")
		|| (group.options[group.selectedIndex].text == "indob") || (group.options[group.selectedIndex].text == "deb")) {
            $("textBoxDiscountAsian").value = commission.groupB;
            this.SelectedAsianCommissionMember(commission.groupB)
        }
        if ((group.options[group.selectedIndex].text == "c") || (group.options[group.selectedIndex].text == "hkc")
		|| (group.options[group.selectedIndex].text == "indoc") || (group.options[group.selectedIndex].text == "dec")) {
            $("textBoxDiscountAsian").value = commission.groupC;
            this.SelectedAsianCommissionMember(commission.groupC)
        }
        if ((group.options[group.selectedIndex].text == "d") || (group.options[group.selectedIndex].text == "hkd")
		|| (group.options[group.selectedIndex].text == "indod") || (group.options[group.selectedIndex].text == "ded")) {
            $("textBoxDiscountAsian").value = commission.groupD;
            this.SelectedAsianCommissionMember(commission.groupD)
        }
    },

    GenerateCheckBoxStatus: function () {
        var header = $('statusHeader');
        var checkBox = document.createElement('input');
        checkBox.setAttribute('type', 'checkbox');
        checkBox.setAttribute('value', 'checkbox');
        checkBox.setAttribute('id', 'status');
        checkBox.onclick = function () {
            if (this.checked) {
                $('closed').disabled = false;
            }
            else {
                $('closed').disabled = true;
            }
        }

        header.insertBefore(checkBox, header.firstChild);
    },

    GenerateCheckBoxCommission: function () {
        var header = $('commissionHeader');
        var checkBoxHeader = document.createElement('input');

        checkBoxHeader.setAttribute('type', 'checkbox');
        checkBoxHeader.setAttribute('value', 'checkbox');
        checkBoxHeader.setAttribute('id', 'commission');
        checkBoxHeader.onclick = function () {
            if (this.checked) {
                disableCommission(false);
            }
            else {
                disableCommission(true);
            }
        }

        header.insertBefore(checkBoxHeader, header.firstChild);
    },

    GenerateCheckBoxBettingLimit: function () {
        var items = bettingLimit.items;
        for (var i = 0; i < 2; i++) {
            var checkBox = document.createElement('input');
            checkBox.setAttribute('type', 'checkbox');
            checkBox.setAttribute('value', 'checkbox');
            checkBox.setAttribute('id', 'bettingLimit' + i);
            checkBox.onclick = function (event) {
                var items = bettingLimit.items;
                var item = this.id.toString();
                var id = item.substring(12, 13);
                if (this.checked) {
                    reEnableChildren(items[id].element, 0);
                }
                else {
                    disableChildren(items[id].element, "BetTypeInactive");
                    var headerSoccer = items[id].header;
                    var count = parseInt(id);
                    if (items[id].active === true) { // Uncheck --> Checked
                        items[id].active = false;
                        headerSoccer.className = "BettingLimitHeaderCopied";
                        for (var i = count + 1; i < items.length; i++) {
                            items[i].copiedItem = null; // Remove copiedItem
                            if (items[i].inactive === false) break;
                            items[i].inactive = false;
                            var checkBox = 'bettingLimit' + i;
                            if ($(checkBox).checked) {
                                Fanex.PTEngine.ReEnableChildren(items[i].element, 0);
                            }
                            else {
                                reEnableChildren(items[i].element, 1);
                            }
                        }
                    }
                }

                //Disable event click of parent
                if (typeof (event) == 'undefined') {
                    event = window.event;
                }

                event.cancelBubble = true;
                if (event.stopPropagation) {
                    event.stopPropagation();
                }
            }

            items[i].element.childNodes[0].insertBefore(checkBox, items[i].element.childNodes[0].firstChild);
        }

        for (var i = 2; i < items.length; i++) {
            var checkBox = document.createElement('input');
            checkBox.setAttribute('type', 'checkbox');
            checkBox.setAttribute('value', 'checkbox');
            checkBox.setAttribute('id', 'bettingLimit' + i);
            checkBox.onclick = function () {
                var items = bettingLimit.items;
                var item = this.id.toString();
                var id = item.substring(12, 13);
                if (this.checked) {
                    reEnableChildren(items[id].element, 0);
                }
                else {
                    disableChildren(items[id].element, "BetTypeInactive");
                }
            }

            items[i].element.childNodes[0].insertBefore(checkBox, items[i].element.childNodes[0].firstChild);
        }
    },

    GenerateCheckBoxPositionTaking: function () {
        var j = 0;
        for (var i = 0; i < ptGrid.items.length; i++) {
            var checkBox = document.createElement('input');
            checkBox.setAttribute('type', 'checkbox');
            checkBox.setAttribute('value', 'checkbox');
            checkBox.setAttribute('id', 'positionTaking' + i);
            checkBox.onclick = function (event) {
                var items = ptGrid.items;
                var item = this.id.toString();
                var id = item.substring(14, 15);

                if (this.checked) {
                    disableSportType(id, false);
                }
                else {
                    disableSportType(id, true);
                    var cloneFromSoccer = items[0].cloneButton;
                    var cloneFromBasketball = items[1].cloneButton;
                    cloneFromSoccer.parent.copiedItem = null;
                    cloneFromBasketball.parent.copiedItem = null;
                    if (cloneFromSoccer.firstChild.checked) {
                        cloneFromSoccer.firstChild.checked = false;
                        cloneFromSoccer.className = "PTSportCopied";
                        for (var i = 1; i < items.length; i++) {
                            var idPT = 'positionTaking' + i;
                            if (i > 1) {
                                items[i].cloneButton.className = "PTSportCopiedInactive";
                                items[i].cloneButton.title = String.Empty;
                            }

                            $(idPT).disabled = false;
                            disableSportType(i, true);
                        };

                        items[1].editButton.className = "PTSportCopiedInactive";
                        items[1].editButton.title = String.Empty;
                    }

                    if (cloneFromBasketball.firstChild.checked) {
                        cloneFromBasketball.firstChild.checked = false;
                        cloneFromBasketball.className = "PTSportCopied";
                        for (var i = 2; i < items.length; i++) {
                            items[i].cloneButton.className = "PTSportCopiedInactive";
                            items[i].cloneButton.title = String.Empty;
                          
                            var idPT = 'positionTaking' + i;
                            $(idPT).disabled = false;
                            disableSportType(i, true);
                        };
                    }
                }

                //Disable event click of parent
                if (typeof (event) == 'undefined') {
                    event = window.event;
                }

                event.cancelBubble = true;
                if (event.stopPropagation) {
                    event.stopPropagation();
                }
            }

            $('ptGridContainer').childNodes[0].childNodes[j].childNodes[0].insertBefore(checkBox, $('ptGridContainer').childNodes[0].childNodes[j].childNodes[0].firstChild);
            j += 2;
        }
    },

    CheckStatusCopyAll: function () {
        var bettingLimitHeaderCopied = bettingLimit.items;

        if (bettingLimitHeaderCopied[0].active === true) {
            bettingLimitHeaderCopied[0].active = false;
        }
        else {
            bettingLimitHeaderCopied[0].active = true;
        }

        if (bettingLimitHeaderCopied[1].active === true) {
            bettingLimitHeaderCopied[1].active = false;
        }
        else {
            bettingLimitHeaderCopied[1].active = true;
        }
    }

}

function initCommission() {
    var commission = customer.commissionSetting;
    /* Render Commission to selected*/
    if (context.targetCustId == 0) {
        Fanex.Customer.SetNextCustomer($("selectedOne"), $("selectedTwo"), $("selectedThree"), customer.newUserName);
        $("textBoxDiscountAsian").value = commission.groupA;
        Fanex.Customer.EditMultiple.LoadGroupCommission($("group"), context.currencyId, context.siteName);

        Fanex.PTEngine.PT_SetSelect($("discountAsian"), 0, commission.groupAMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupA);
        Fanex.PTEngine.PT_SetSelect($("discount1x2"), 0, commission.discount1x2Max, Fanex.PTEngine.COMMISSION_STEP, commission.discount1x2);
        Fanex.PTEngine.PT_SetSelect($("discountCS"), 0, commission.discountCSMax, Fanex.PTEngine.COMMISSION_STEP, commission.discountCS);
        Fanex.PTEngine.PT_SetSelect($("discountNumber"), 0, commission.discountNumberMax, Fanex.PTEngine.COMMISSION_STEP, commission.discountNumber);
    }
    else {
        $("textBoxDiscountAsian").value = commission.discount;
        Fanex.PTEngine.PT_SetSelect($("discountAsian"), 0, commission.discount, Fanex.PTEngine.COMMISSION_STEP, commission.playerDiscount);
        Fanex.PTEngine.PT_SetSelect($("discount1x2"), 0, commission.discount1x2, Fanex.PTEngine.COMMISSION_STEP, commission.playerDiscount1x2);
        Fanex.PTEngine.PT_SetSelect($("discountCS"), 0, commission.discountCS, Fanex.PTEngine.COMMISSION_STEP, commission.playerDiscountCs);
        Fanex.PTEngine.PT_SetSelect($("discountNumber"), 0, commission.discountNumber, Fanex.PTEngine.COMMISSION_STEP, commission.playerDiscountNumber);

        currentStatusClose = customer.statusSetting.closed;
    }
}

function initArrays() {
    if (custIds == null || custIds == undefined) {
        if (parent.$("arrayCustID") != null) {
            custIds = parent.$("arrayCustID").value.split('^');
            custIds.pop();
            userNames = parent.$("arrayUserName").value.split('^');
            userNames.pop();
        }
        else {
            custIds = [context.targetCustId];
            userNames = [customer.userName];
        }

        if (custIds.length == 0) {
            custIds = [context.targetCustId];
            userNames = [customer.userName];
        }
    }
}

ko.applyBindings(new CustomerModel(customer));
RegisterStartUp(function () {
    if (context.targetCustId == 0) {
        age.addEvent($('group'), 'change', function () {
            Fanex.Customer.EditMultiple.ChangeGroupCommission($('group'));
        });
    }
    else {
        age.addEvent($('closed'), 'click', function () {
            Fanex.Customer.EditMultiple.Closed($('closed'), currentStatusClose);
        });

    }

    Fanex.Customer.EditMultiple.GenerateCheckBoxStatus();
    Fanex.Customer.EditMultiple.GenerateCheckBoxCommission();
    Fanex.Customer.EditMultiple.GenerateCheckBoxBettingLimit();
    Fanex.Customer.EditMultiple.GenerateCheckBoxPositionTaking();

    init();
    initCommission();
});