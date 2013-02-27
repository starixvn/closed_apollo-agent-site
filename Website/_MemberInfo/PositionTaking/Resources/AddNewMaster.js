Fanex.PTEngine.CURRENT_CONTEXT = context;
Fanex.PTEngine.CURRENT_CONTEXT.ia3 = false;

var isChangeM3 = false;
var isShowPopUpSetPTMember = false;
var newValueMemberPT = "";
var oldPTSetting = customer.ptSetting.clone();

var ptGrid = null;
ptGrid = new Fanex.PTEngine.PTGrid(customer.ptSetting);
var div = $('ptGridContainer');
ptGrid.Render(div);

var bettingLimit = null;
bettingLimit = new Fanex.PTEngine.BettingLimit(customer.bettingLimit)
var bettingLimitContainer = $('bettingLimitContainer');
bettingLimit.Render(bettingLimitContainer);

/* Render Commission to selected*/
var commission = customer.commissionSetting;
Fanex.PTEngine.PT_SetSelect($("groupA"), 0, commission.groupAMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupA);
Fanex.PTEngine.PT_SetSelect($("groupB"), 0, commission.groupBMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupB);
Fanex.PTEngine.PT_SetSelect($("groupC"), 0, commission.groupCMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupC);
Fanex.PTEngine.PT_SetSelect($("groupD"), 0, commission.groupDMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupD);

Fanex.PTEngine.PT_SetSelect($("discount1x2"), 0, commission.discount1x2Max, Fanex.PTEngine.COMMISSION_STEP, commission.discount1x2);
Fanex.PTEngine.PT_SetSelect($("discountCS"), 0, commission.discountCSMax, Fanex.PTEngine.COMMISSION_STEP, commission.discountCS);
Fanex.PTEngine.PT_SetSelect($("discountNumber"), 0, commission.discountNumberMax, Fanex.PTEngine.COMMISSION_STEP, commission.discountNumber);

Fanex.Customer.SetNextCustomer($("selectedOne"), $("selectedTwo"), null, customer.newUserName);

// For Check Edit Postion Taking
if (context.targetCustId > 0) {
    var currentStatusClose = customer.statusSetting.closed;
    var currentStatusSuspend = customer.statusSetting.suspended;
    $("minCreditId").className = "";
    customer.minCreditTemp = String.prototype.FormatNumber(customer.minCredit);
    $("allowautoptId").style.display = "none";
}
else {
    $("minCreditId").className = "hide";
    $("closedId").style.display = "none";
    customer.minCreditTemp = 0;
}

//console.log(customer);
/*
Bind customer information to customer JSON object by knockout.js
Render PTGrid
*/
var CustomerModel = function (customer) {
    var uplineUserName = customer.userName;
    // Check Transfer condition
    displayTransferCondition = Fanex.Customer.DisplayTransferCondition(customer.transferOption);

    // Super allow master using auto master PT
    customer.statusSetting.disableAutoMPT = ko.observable(true);

    // Set label MaxCredit
    customer.maxCreditTemp = String.prototype.FormatNumber(customer.maxCredit);
    customer.credit = String.prototype.FormatNumber(customer.credit);

    this.saveCustomer = function () {
        if (!Fanex.Customer.CheckInfoValue()) return;
        if (!bettingLimit.Validate()) return;

        if (typeof (customer.statusSetting.disableAutoMPT) == 'function')
            customer.statusSetting.disableAutoMPT = customer.statusSetting.disableAutoMPT();
        else
            customer.statusSetting.disableAutoMPT = customer.statusSetting.disableAutoMPT;

        // Set value to transfer option
        customer.transferOption = Fanex.Customer.TransferOption(customer.transferOption);
        customer.commissionSetting = Fanex.Customer.SetValueCommissionSettings(customer.commissionSetting);
        customer.credit = Fanex.RoundNumber(parseFloat($("credit").value, true), 1); // Fix bug for IE7

        // For Check Edit Postion Taking
        if (context.targetCustId == 0) {
            // Set next to userName
            customer.userName = uplineUserName + Fanex.Customer.SetAutoUserName();
        }

        // EscapeHTML String
        customer.firstName = $("firstName").value.escapeHtml();
        customer.lastName = $("lastName").value.escapeHtml();
        customer.phone = $("phone").value.escapeHtml();
        customer.mobilePhone = $("mobilePhone").value.escapeHtml();
        customer.fax = $("fax").value.escapeHtml();

        if (ptGrid) {
            customer.ptSetting = ptGrid.Save();
            customer.bettingLimit = bettingLimit.Save();
            var text = JSON.stringify(customer);
            $("txtCustomerObject").value = text;
            var params = ajax.CreateParams('txtCustomerObject');

            // For Check Edit Postion Taking
            if (context.targetCustId > 0) {
                if (!isShowPopUpSetPTMember) {
                    isChangeM3 = Fanex.EditPT.CheckChangeM3(oldPTSetting, customer.ptSetting);
                }
                if (isChangeM3 && !isShowPopUpSetPTMember) {
                    Popups.Confirm(Fanex.EditPT.MemberMaximumPT(), Language.CONFIRM_MASTER_NOT_TO_TAKE_AUTO_POSITION_TAKING);
                    return;
                }
                else if (isChangeM3 && isShowPopUpSetPTMember) {
                    Popups.Close();
                }
            }

            function AddNewMasterComplete(result) {
                var errMsg = result.errMsg;
                if (result.errCode == 0) {
                    ageMsg.Hide();
                    var splitName = errMsg.split(',');
                    var username = splitName[0];
                    var custid = splitName[1];
                    var isFI = splitName[2];
                    var isCS = splitName[3];
                    var isHR = splitName[4];
                    var isBG = splitName[5];
                    var isLiveCS = splitName[6];
                    if (isFI == 0 || isCS == 0 || isHR == 0 || isBG == 0) {
                        url = age.GetBaseUrl() + "MemberInfo/RedirectPT.aspx?username=" + username + "&custid=" + custid + "&isfi=" + isFI + "&iscs=" + isCS + "&ishr=" + isHR + "&isbg=" + isBG + "&islivecs=" + isLiveCS + "&isedit=false" + "&insert=true";
                    }
                    setTimeout(function () { parent.location.href = url; }, 4000);
                }
                else {
                    age.UnLock();
                    ageMsg.Show(errMsg);
                    window.scroll(0, 0);
                    return;
                }
            }

            function SuperUpdateMasterComplete(result) {
                if (result.errCode == 0) {
                    if (_page.CallBack) {
                        eval(_page.CallBack);
                    }
                    else {
                        top.main.DelayReloadPage();
                    }
                }
                else {
                    isChangeM3 = false;
                    isShowPopUpSetPTMember = false;
                    newValueMemberPT = "";
                    age.UnLock();
                    ageMsg.Show(result.errMsg);
                    window.scroll(0, 0);
                    return;
                }
            }

            function SuperUpdateMemberComplete(result) {
                alert("Super Update Member.");
                top.main.DelayReloadPage();
            }

            age.ReloadPage(30000);
            // Post with return result in JSON format
            var submitUrl = "";
            if (context.targetCustId > 0 && context.targetRoleId == 3) { // Super update master
                if (newValueMemberPT != "") {
                    submitUrl = "../Handlers/SuperUpdateMaster.ashx?memberPositionTaking=" + newValueMemberPT;
                }
                else {
                    submitUrl = "../Handlers/SuperUpdateMaster.ashx";
                }
                OnComplete = SuperUpdateMasterComplete;
            }
            else if (context.targetCustId > 0 && context.targetRoleId == 1) { // Super update member
                submitUrl = "../Handlers/SuperUpdateMember.ashx";
                OnComplete = SuperUpdateMemberComplete;
            }
            else {
                submitUrl = "../Handlers/SaveNewMaster.ashx";
                OnComplete = AddNewMasterComplete;
            }

            ajax.PostJSON(
				submitUrl,
				params,
				OnComplete
			);
        }
    };
};

ko.applyBindings(new CustomerModel(customer));

Fanex.EditPT = {
    // Edit PT Master on account Info
    OnRedirectToPage: function () {
        top.menu.reloadMenuUsrInfo();
        age.DelayReloadPage('../../../_MemberInfo/CustomerList/Super/MasterList.aspx');
    },

    // Check or UnCheck Close Status
    Closed: function (checkBoxClose, currentStatusClose) {
        if (checkBoxClose.checked == true && checkBoxClose.checked != currentStatusClose) {
            if (!confirm(Language.ALERT_CONFIRM_CLOSE)) {
                customer.statusSetting.closed = false;
                checkBoxClose.checked = false;
            }
        }
        else if (checkBoxClose.checked == false && checkBoxClose.checked != currentStatusClose) {
            if (!confirm(Language.ALERT_CONFIRM_OPEN)) {
                customer.statusSetting.closed = true;
                checkBoxClose.checked = true;
            }
        }
    },

    // Check or UnCheck Suspend Status
    Suspended: function (checkBoxSuspend, currentStatusSuspend) {
        if (checkBoxSuspend.checked == true && checkBoxSuspend.checked != currentStatusSuspend) {
            if (!confirm(Language.ALERT_CONFIRM_SUSPEND)) {
                customer.statusSetting.suspended = false;
                checkBoxSuspend.checked = false;
            }
        }
        else if (checkBoxSuspend.checked == false && checkBoxSuspend.checked != currentStatusSuspend) {
            if (!confirm(Language.ALERT_CONFIRM_UNSUSPEND)) {
                customer.statusSetting.suspended = true;
                checkBoxSuspend.checked = true;
            }
        }
    },

    // We need check have any PT value change from auto to none auto. And Set new value to Member
    CheckChangeM3: function (ptSettingOld, ptSettingNew) {
        var sportTypes = ptSettingOld.sportTypes;
        for (var sportTypeIndex = 0; sportTypeIndex < sportTypes.length; sportTypeIndex++) {
            var sportTypesItem = sportTypes[sportTypeIndex]
            var betTypeGroupSets = sportTypesItem.betTypeGroupSets;

            for (var groupSetIndex = 0; groupSetIndex < betTypeGroupSets.length; groupSetIndex++) {
                var betTypeGroupSetsItem = betTypeGroupSets[groupSetIndex];
                var betTypeGroups = betTypeGroupSetsItem.betTypeGroups;

                for (var groupIndex = 0; groupIndex < betTypeGroups.length; groupIndex++) {
                    var betTypeGroupItem = betTypeGroups[groupIndex];
                    var betTypes = betTypeGroupItem.betTypes;

                    for (var betTypeIndex = 0; betTypeIndex < betTypes.length; betTypeIndex++) {
                        betTypeItem = betTypes[betTypeIndex];
                        var isAutoOld = betTypeItem.pt.is3;
                        var isAutoNew = ptSettingNew.sportTypes[sportTypeIndex].betTypeGroupSets[groupSetIndex].betTypeGroups[groupIndex].betTypes[betTypeIndex].pt.is3;

                        if (isAutoOld && !isAutoNew) {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    },

    // Get selected and m4 value of Soccer HdpD Master Preset
    MemberMaximumPT: function () {
        var ptSetting = customer.ptSetting;
        var s4 = ptSetting.sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].pt.s4;
        var m3 = ptSetting.sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].pt.m3;
        return Fanex.Round(s4 - m3);
    },

    // Confirm new value for all member before transfer to server
    SetNewValueToMember: function (isUpdateAllMember) {
        isShowPopUpSetPTMember = true;
        if (isUpdateAllMember) {
            var valueMemberPositionTaking = $("selectedMemberPositionTaking").value;
            newValueMemberPT = valueMemberPositionTaking + ";" + isUpdateAllMember;
        }
        var customerModel = new CustomerModel(customer);
        customerModel.saveCustomer();
    }
}

function ConfirmYes() {
    Fanex.EditPT.SetNewValueToMember(1);
}

function ConfirmNo() {
    Fanex.EditPT.SetNewValueToMember(0);
}

// For Edit Position Taking
RegisterStartUp(function () {
    if (context.targetCustId > 0) {
        age.addEvent($('closed'), 'click', function () {
            Fanex.EditPT.Closed($('closed'), currentStatusClose);
        });

        age.addEvent($('suspended'), 'click', function () {
            Fanex.EditPT.Suspended($('suspended'), currentStatusSuspend);
        });
    }

    Fanex.Customer.CheckUser(customer.userName, 'selectedOne', 'selectedOne', 'selectedTwo', null, 'msgIcon');
    Fanex.Customer.CheckUser(customer.userName, 'selectedTwo', 'selectedOne', 'selectedTwo', null, 'msgIcon');
});