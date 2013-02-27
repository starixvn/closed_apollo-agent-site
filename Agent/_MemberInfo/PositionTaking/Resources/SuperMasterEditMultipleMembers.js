Fanex.PTEngine.CURRENT_CONTEXT = context;
Fanex.PTEngine.CURRENT_CONTEXT.ia3 = !customer.statusSetting.disableAutoMPT; // Allow auto PT master on member

var ptGrid = null;
ptGrid = new Fanex.PTEngine.PTGrid(customer.ptSetting);
var div = $('ptGridContainer');
ptGrid.Render(div);

var custIds;
var userNames;
var isUpdatePositionTaking = false;
var isUpdate = false;

ptGrid.ShowCloneHint = function () {
    if (this.copiedItem == null) {
        return;
    }
    var copiedElements = [0, this.items[0].items[0].items[0].items[0].element,
                                        this.items[1].items[0].items[0].items[0].element]; // Handicap of Soccer and Basketball
    var hint = Language.COPY_ALL_HINT.Format('<b>' + this.activeCopiedTitle + '</b>');
    var otherBetTypesOf = Language.OTHER_BET_TYPE_OF.Format(this.items[this.copiedItem.sportId - 1].name);
    hint = hint + '<div class="ul-position"><ul><li>' + otherBetTypesOf + '</li>';
    // Find all copied items to generate tooltip
    var count = this.items.length;
    var top = 0;
    for (var i = this.copiedItem.sportId; i < count; i++) {
        if (this.items[i].copiedItem != null && this.items[i].copiedItem.sportId == this.copiedItem.sportId) {
            hint += '<li>' + this.items[i].name + '</li>';
            top += 18;
        }
    }
    hint = hint + '</ul></div>';
    top = top + 40;
    // Show tooltip
    var yPosition = 95;
    if (context.roleId == 4) {
        yPosition = 122;
    }

    show(copiedElements[this.copiedItem.sportId], hint, 0, 10, yPosition, -10, top, 30000);
};

function show(elementInput, message, xPos, pointerXPos, yPos, pointerYPos, height, timeout) {
    // <span class="hint"><span>This is the message.<span><span class="hint-pointer">&nbsp;</span></span>
    var hint = document.getElementById('fHint');
    var hintPointer = null;
    if (hint == null) {
        var hint = document.createElement("SPAN");
        hint.className = 'hint';
        var hintMsg = document.createElement("SPAN");
        hint.appendChild(hintMsg);
        hintPointer = document.createElement("SPAN");
        hintPointer.className = 'hint-pointerDown';
        hint.appendChild(hintPointer);
        hint.id = 'fHint';
        hint.style.top = '10px';
        hint.style.left = '10px';
        hint.onclick = function () { // Hide onclick
            hint.style.display = 'none';
        };
        document.body.appendChild(hint);
    };
    hintPointer = hint.lastChild;
    hint.firstChild.innerHTML = message;
    if (this.timeOutHideTooltip) {
        clearTimeout(this.timeOutHideTooltip);
        this.timeOutHideTooltip = null;
    };
    if (typeof (timeout) == 'undefined') {
        timeout = 10000;
    }
    this.timeOutHideTooltip = setTimeout(function () {
        if (hint == null) {
            return;
        }
        hint.style.display = 'none';
    }, timeout); // Auto hide in 10s
    if (elementInput != null) {
        if (typeof xPos == 'undefined') {
            xPos = 0;
        }
        if (typeof pointerXPos == 'undefined') {
            pointerXPos = 10;
        }
        if (typeof yPos == 'undefined') {
            yPos = -45;
        }
        if (typeof (pointerYPos) == 'undefined') {
            pointerYPos = 38;
        }
        if (typeof (height) == 'undefined') {
            height = 18;
        }
        hint.style.height = height + 'px';
        hintPointer.style.left = (pointerXPos + 'px');
        hintPointer.style.top = (pointerYPos + 'px');
        var pos = Validators.utils.findPos(elementInput);
        hint.style.left = (pos[0] + xPos) + 'px';
        hint.style.top = (pos[1] + yPos) + 'px';
        elementInput.onchange = function () {
            hint.style.display = 'none';
        };
        elementInput.focus();
    };
    hint.style.display = 'block';
}

function disableSportType(id, isDisable) {
    if (typeof (id) == 'undefined') return false;
    if (ptGrid.items.length <= 0) return false;

    var betTypeGroupSets = ptGrid.items[id].items;
    for (var j = 0; j < betTypeGroupSets.length; j++) {
        var betTypeGroups = betTypeGroupSets[j].items;
        isDisable ? betTypeGroupSets[j].copiedItem = {} : betTypeGroupSets[j].copiedItem = null;
        for (var k = 0; k < betTypeGroups.length; k++) {
            var betTypes = betTypeGroups[k].items;
            for (var l = 0; l < betTypes.length; l++) {
                var idCopied = "c$" + betTypeGroups[k].betTypes[0].id + "$" + betTypeGroups[k].betTypes[0].sportId + "$" + (betTypeGroups[k].isLive ? "0" : "1");
                if (isDisable) {
                    if (betTypes[l].actived == true) {
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
                else {
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
}

function initPositionTaking() {
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

function getUpdateParamaters(paramaters) {
    var paramaters = "";

    if ($("status").checked) {
        paramaters += "1";
        if (!isUpdate) {
            isUpdate = true;
        }
    }
    else {
        paramaters += "0";
    }

    var itemsPt = ptGrid.items;
    var cloneFromSoccer = itemsPt[0].cloneButton;
    var cloneFromBasketball = itemsPt[1].cloneButton;
    if ($("positionTaking0").checked && cloneFromSoccer.firstChild.checked) {
        isUpdatePositionTaking = true;
        for (var i = 0; i < 10; i++) {
            paramaters += "1";
            if (!isUpdate) {
                isUpdate = true;
            }
        }
    }
    else if ($("positionTaking1").checked && cloneFromBasketball.firstChild.checked) {
        $("positionTaking0").checked ? paramaters += "1" : paramaters += "0";
        isUpdatePositionTaking = true;
        for (var i = 1; i < 10; i++) {
            paramaters += "1";
            if (!isUpdate) {
                isUpdate = true;
            }
        }
    }
    else {
        for (var i = 0; i < 10; i++) {
            if ($("positionTaking" + i).checked) {
                paramaters += "1";
                if (!isUpdatePositionTaking) {
                    isUpdatePositionTaking = true;
                }

                if (!isUpdate) {
                    isUpdate = true;
                }
            }
            else {
                paramaters += "0";
            }
        }
    }

    return paramaters;
}

function init() {
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

    initPositionTaking();

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
                cloneFromBasketball.title = Language.COPY_ALL_OTHER_POSITION_TAKING_FROM.Format(items[1].name, sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].name);
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
                cloneFromBasketball.title = String.Empty;
            } else {
                cloneFromBasketball.title = Language.COPY_ALL_OTHER_POSITION_TAKING_FROM.Format(sportTypes[1].name, sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].name);
            }

            for (var i = 1; i < count; i++) {
                if (cloneFromBasketball.firstChild.checked) {
                    disableSportType(i, true);
                    sportTypes[i].Inactive(copiedItemBasketball.clone());
                    if (i > 1) {
                        var id = "positionTaking" + i;
                        $(id).checked = false;
                        $(id).disabled = true;
                    }
                }
                else if (i > 1) {
                    var id = "positionTaking" + i;
                    $(id).disabled = false;
                }

                if (i > 1) {
                    sportTypes[i].cloneButton.className = cloneFromBasketball.firstChild.checked ? "PTSportEdit" : "PTSportCopiedInactive";
                    sportTypes[i].cloneButton.title = cloneFromBasketball.firstChild.checked ? Language.EDIT : String.Empty;
                }
            };

            if (!cloneFromBasketball.firstChild.checked) {
                sportTypes[1].Active();
                cloneFromBasketball.parent.copiedItem = null;
            }

            cloneFromBasketball.parent.ShowCloneHint();
        }
    };

    $('closed').disabled = true;
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

        if (ptGrid) {
            var url = "";
            if (context.roleId == 4) {
                url = "../Handlers/SuperUpdateMember.ashx";
            }
            else {
                url = "../Handlers/MasterUpdateMember.ashx";
            }

            var updateParameters = "";
            updateParameters = getUpdateParamaters(updateParameters);

            if (!isUpdate) {
                age.ReloadPage(300000);
                top.main.DelayReloadPage();

                return;
            }

            if (isUpdatePositionTaking) {
                customer.ptSetting = ptGrid.Save();
            }

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

Fanex.Customer.EditMultipleMembers = {
    // Check or UnCheck Close Status
    Closed: function (checkBoxClose, currentStatusClose) {
        if (checkBoxClose.checked == true && checkBoxClose.checked != currentStatusClose) {
            if (!confirm(Language.ALERT_CONFIRM_CLOSE_MEMBER)) {
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

    GenerateCheckBoxStatus: function () {
        var header = $('statusHeader');
        var checkBox = document.createElement('input');
        checkBox.setAttribute('type', 'checkbox');
        checkBox.setAttribute('value', 'checkbox');
        checkBox.setAttribute('id', 'status');
        if (customer.statusSetting.uplineClosed == true) {
            checkBox.setAttribute('disabled', true);
        }

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
                            var idPT = 'positionTaking' + i;
                            if (i > 2) {
                                items[i].cloneButton.className = "PTSportCopiedInactive";
                                items[i].cloneButton.title = String.Empty;
                            }

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
    if (context.targetCustId != 0) {
        currentStatusClose = customer.statusSetting.closed;
        age.addEvent($('closed'), 'click', function () {
            Fanex.Customer.EditMultipleMembers.Closed($('closed'), currentStatusClose);
        });
    }

    Fanex.Customer.EditMultipleMembers.GenerateCheckBoxStatus();
    Fanex.Customer.EditMultipleMembers.GenerateCheckBoxPositionTaking();

    init();
});