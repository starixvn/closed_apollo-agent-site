Fanex.PTVirtualSports.CURRENT_CONTEXT = context;
Fanex.PTVirtualSports.CURRENT_CONTEXT.ia3 = !customer.statusSetting.disableAutoMPT;

var oldCustomer = customer.clone();
oldCustomer.firstName = null;
oldCustomer.lastName = null;
oldCustomer.phone = null;
oldCustomer.mobilePhone = null;
oldCustomer.fax = null;
var newCustomer = oldCustomer.clone();

if (customer.bettingLimit.bettingLimitItems.length > 0) {
    var bettingLimit1 = new Fanex.PTVirtualSports.BettingLimit(customer.bettingLimit);
    var bettingLimitContainer = document.getElementById('bettingLimitContainer');
    bettingLimit1.Render(bettingLimitContainer, IsHideTextBox(context));
}
else {
    $("bettingLimitContainer").className = "noInfoBlock";
    $("bettingLimitContainer").innerHTML = Language.NOINFO;
    $("btnSubmit").disabled = true;
}

if (customer.ptSetting.sportTypes.length > 0) {
    var ptGrid = new Fanex.PTVirtualSports.PTGrid(customer.ptSetting.clone());
    var ptContainer = document.getElementById('positionTaking');
    ptGrid.Render(ptContainer);
}
else {
    $("positionTaking").className = "noInfoBlock";
    $("positionTaking").innerHTML = Language.NOINFO;
    $("btnSubmit").disabled = true;
}

var UpdateType = {
    Selected: 0,
    "All": 0,
    "Status": 1,
    "BetSetting": 2,
    "PositionTaking": 3
}

var CustomerModel = function (customer) {
    InitArrays();
    customer.userNames = usernames.join(", ");

    this.saveCustomer = function () {
        UpdateType.Selected = UpdateType.All;
        this.save();
    }

    this.saveStatus = function () {
        UpdateType.Selected = UpdateType.Status;
        this.save();
    }

    this.saveBettingLimit = function () {
        UpdateType.Selected = UpdateType.BetSetting;
        this.save();
    }

    this.savePositionTaking = function () {
        UpdateType.Selected = UpdateType.PositionTaking;
        this.save();
    }

    this.save = function () {
        newCustomer = oldCustomer.clone();

        if (ptGrid != null) {
            // get status settings
            if (UpdateType.Selected == UpdateType.All || UpdateType.Selected == UpdateType.Status) {
                var disableDownlinesVirtualSports = true;
                newCustomer.statusSetting.disableVirtualSports = !document.getElementById("chkEnabled").checked;
                if (context.targetRoleId != 1) {
                    newCustomer.statusSetting.disableDownlinesVirtualSports = !document.getElementById("chkDownline").checked;
                }
            }

            // get bettingLimit
            if (UpdateType.Selected == UpdateType.All || UpdateType.Selected == UpdateType.BetSetting) {
                if (context.targetRoleId == 1 && (context.roleId == 4 || context.roleId == 3)) {
                    newCustomer.bettingLimit = null;
                }
                else {
                    if (!bettingLimit1.Validate()) {
                        return;
                    }
                    newCustomer.bettingLimit = bettingLimit1.Save();
                }
            }

            // get pt setting
            if (UpdateType.Selected == UpdateType.All || UpdateType.Selected == UpdateType.PositionTaking) {
                var isupdateallmembers = ptGrid.Validate(newCustomer.ptSetting, IsMulti());
                newCustomer.ptSetting = ptGrid.Save();

                if (!isupdateallmembers) {
                    updateSettings();
                }
            }
            else {
                updateSettings();
            }
        }
    }
}

/*
This function support 3 cases: error, success & warning
errCode < -1 : success (Note: -1 is a Agent system error)
errCode >= 50 && errCode <= 99 : warning
errCode : all of remaing ones are errors.
*/
function GenerateMessage(ArrCodes, ArrMsgs, ArrUserName, isAdd) {
    var isSucc = ageMsg.CheckMessageIsSucc2(ArrCodes);

    function getClass(errCode) {
        var className = '';
        if (errCode < -1) {
            className = 'EnrichSuccmsg';
        }
        else if (errCode >= 50 && errCode <= 99) {
            className = 'EnrichWarmsg';
        }
        else {
            className = 'EnrichErrmsg';
        }

        return className;
    }

    function InsertRow(tableId, classname, message) {
        var tr = document.createElement('tr');
        var td = document.createElement('td');
        td.className = classname;
        td.innerHTML = message;
        tr.appendChild(td);
        var tbody = document.getElementById(tableId).getElementsByTagName('tbody')[0];
        tbody.appendChild(tr);
    }

    if (isAdd) // Add
    {
        for (var j = 0; j < ArrUserName.length; j++) {
            InsertRow('tblMessage', 'EnrichUserName', ArrUserName[j]);
            for (var i = 0; i < ArrCodes.length; i++) {
                InsertRow('tblMessage', getClass(ArrCodes[i]), ArrMsgs[i]);
            }
        }
    }
    else // Reset table
    {
        var myTable = '<table id="tblMessage" cellpadding="0" cellspacing="0" border="0" style="width:100%;"><tbody>';
        var myEndTable = '</tbody></table>';

        var tableBodyHTML = '';
        for (var j = 0; j < ArrUserName.length; j++) {
            tableBodyHTML += '<tr><td class="EnrichUserName">';
            tableBodyHTML += ArrUserName[j];
            tableBodyHTML += "</td></tr>";

            for (var i = 0; i < ArrCodes.length; i++) {
                tableBodyHTML += '<tr><td class="' + getClass(ArrCodes[i]) + '">' + ArrMsgs[i] + "</td></tr>";
            }
        }
        myTable += tableBodyHTML + myEndTable;

        ageMsg.ShowMsgs(myTable, isSucc);
    }
}

var isSuccess = true;
function updateSettings() {
    isSuccess = true;
    age.ReloadPage(300000);
    updateCustSettings(0);
}

function updateCustSettings(id) {
    var custid = custids[id];
    var username = usernames[id];
    var isMulti = false;
    if (id > 0) {
        isMulti = true;
    }
    var url = "Handlers/UpdateCustomerSetting.ashx?targetCustId=" + custid + "&targetRoleId=" + context.targetRoleId + "&updateType=" + UpdateType.Selected + "&isMulti=" + isMulti.toString();
    var parameters = ['oldCustomer=', JSON.stringify(oldCustomer), '&newCustomer=', JSON.stringify(newCustomer), '&otherSetting=', JSON.stringify(otherSetting)].join('');

    ajax.PostJSON(url, parameters, function onComplete(result) {
        var ArrCodes = new Array();
        var ArrMsgs = new Array();
        var ArrUserName = new Array();
        ArrUserName.push(username);

        if (result instanceof Array) {
            var len = result.length;
            for (var i = 0; i < len; i++) {
                var item = result[i];
                ArrCodes.push(item.errCode);
                ArrMsgs.push(item.errMsg);
                if (item.errCode >= 0) {
                    isSuccess = false;
                }
                else if (item.errCode == -2) { // update status successfully
                    reloadCheckboxes();
                }
            }
        }
        else {
            ArrCodes.push(result.errCode);
            ArrMsgs.push(result.errMsg);
            if (result.errCode >= 0) {
                isSuccess = false;
            }
        }

        GenerateMessage(ArrCodes, ArrMsgs, ArrUserName, id != 0);

        id++;
        if (id < custids.length) {
            updateCustSettings(id);
        }
        else {
            if (isSuccess) {
                oldCustomer = newCustomer;
            }
            age.UnLock();
            window.scroll(0, 0);
        }
    });
}

function IsHideTextBox(context) {
    var result = true;
    if (context.roleId - context.targetRoleId == 1 || context.targetRoleId == 4) {
        result = false;
    }

    return result;
}

var custids;
var usernames;

function InitArrays() {
    if (custids == null || custids == undefined) {
        if (parent.$("arrayCustID") != null) {
            custids = parent.$("arrayCustID").value.split('^');
            custids.pop();
            usernames = parent.$("arrayUserName").value.split('^');
            usernames.pop();
        }
        else {
            custids = [context.targetCustId];
            usernames = [customer.userName];
        }

        if (custids.length == 0) {
            custids = [context.targetCustId];
            usernames = [customer.userName];
        }
    }
}

function IsMulti() {
    if (custids != null && custids != undefined && custids.length > 1)
        return true;

    return false;
}

function reloadCheckboxes() {
    var chkEnabled = document.getElementById("chkEnabled");
    var chkDownline = document.getElementById("chkDownline");
    chkDownline.disabled = true;

    if (newCustomer.statusSetting.disableUplineVirtualSports === true) {
        chkEnabled.disabled = true;
        chkDownline.disabled = true;
    }

    if (context.targetRoleId != 1) {
        if (!chkEnabled.checked) {
            chkDownline.disabled = true;
        }

        chkEnabled.onclick = function () {
            if (this.checked == false) {
                chkDownline.checked = false;
                chkDownline.disabled = true;
            }
            else {
                chkDownline.checked = !newCustomer.statusSetting.disableDownlinesVirtualSports;
                if (newCustomer.statusSetting.disableVirtualSports == false) {
                    chkDownline.disabled = true;
                }
                else {
                    chkDownline.disabled = false;
                }
            }
        }
    }
}

function InitUI() {
    checkButtonPermission('btnSaveStatus');
    checkButtonPermission('btnSaveBetSetting');
    checkButtonPermission('btnSavePositionTaking');
    Fanex.Customer.HideTitle();

    // using for both of multi & single
    InitArrays();

    if (IsHideTextBox(context)) {
        document.getElementById("saveStatus").style.display = "none";
        document.getElementById("saveBetSetting").style.display = "none";
    }

    reloadCheckboxes();

    if (IsMulti()) {
        if (context.targetRoleId == 1) {
            if (context.roleId == 4) {
                document.getElementById("divStatus").style.display = "none";
            }
            else if (context.roleId == 3) {
                document.getElementById("divStatus").style.display = "none";
            }
            else if (context.roleId == 2) {
                document.getElementById("divDownline").style.display = "none";
            }
        }
    }
    else {
        if (context.targetRoleId == 1) {
            if (context.roleId == 4) {
                document.getElementById("divEnabled").style.display = "none";
                document.getElementById("lblEnabled").innerHTML = "";
                document.getElementById("divDownline").style.display = "none";
            }
            else if (context.roleId == 3) {
                document.getElementById("divEnabled").style.display = "none";
                document.getElementById("lblEnabled").innerHTML = "";
                document.getElementById("divDownline").style.display = "none";
            }
            else if (context.roleId == 2) {
                document.getElementById("divDownline").style.display = "none";
            }
        }
    }
}

var otherSetting = {
    'isupdateallmembers': false
};

function ConfirmYes() {
    setNewValueToMember(true);
}

function ConfirmNo() {
    setNewValueToMember(false);
}

function setNewValueToMember(isupdateallmembers) {
    otherSetting.isupdateallmembers = isupdateallmembers;

    if (isupdateallmembers) {
        for (i = 0; i < newCustomer.ptSetting.sportTypes.length; i++) {
            var sportType = newCustomer.ptSetting.sportTypes[i];
            for (j = 0; j < sportType.betTypeGroupSets.length; j++) {
                var betTypeGroupSet = sportType.betTypeGroupSets[j];
                for (k = 0; k < betTypeGroupSet.betTypeGroups.length; k++) {
                    var betTypeGroup = betTypeGroupSet.betTypeGroups[k];
                    for (l = 0; l < betTypeGroup.betTypes.length; l++) {
                        var betType = betTypeGroup.betTypes[l];
                        switch (context.targetRoleId) {
                            case 3: // super edit master
                                betType.pt.s1 = Popups.GetSportPTValue(sportType.id);
                                break;
                            case 2: // master edit agent
                                betType.pt.m1 = Popups.GetSportPTValue(sportType.id);
                                break;
                        }
                    }
                }
            }
        }
    }

    updateSettings();
    Popups.Close();
}

RegisterStartUp('InitUI()');
ko.applyBindings(new CustomerModel(customer));
