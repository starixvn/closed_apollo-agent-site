
function disableCommission(isDisable) {
    $("groupA").disabled = isDisable;
    $("groupB").disabled = isDisable;
    $("groupC").disabled = isDisable;
    $("groupD").disabled = isDisable;
    $("discount1x2").disabled = isDisable;
    $("discountCS").disabled = isDisable;
    $("discountNumber").disabled = isDisable;
}

function initCommission() {
    var commission = customer.commissionSetting;
    Fanex.PTEngine.PT_SetSelect($("groupA"), 0, commission.groupAMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupA);
    Fanex.PTEngine.PT_SetSelect($("groupB"), 0, commission.groupBMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupB);
    Fanex.PTEngine.PT_SetSelect($("groupC"), 0, commission.groupCMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupC);
    Fanex.PTEngine.PT_SetSelect($("groupD"), 0, commission.groupDMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupD);

    Fanex.PTEngine.PT_SetSelect($("discount1x2"), 0, commission.discount1x2Max, Fanex.PTEngine.COMMISSION_STEP, commission.discount1x2);
    Fanex.PTEngine.PT_SetSelect($("discountCS"), 0, commission.discountCSMax, Fanex.PTEngine.COMMISSION_STEP, commission.discountCS);
    Fanex.PTEngine.PT_SetSelect($("discountNumber"), 0, commission.discountNumberMax, Fanex.PTEngine.COMMISSION_STEP, commission.discountNumber);

    currentStatusClose = customer.statusSetting.closed;
}

function saveCommissionSettings(commissionSettings) {
    commissionSettings.groupA = $("groupA").value;
    commissionSettings.groupB = $("groupB").value;
    commissionSettings.groupC = $("groupC").value;
    commissionSettings.groupD = $("groupD").value;
    commissionSettings.discount1x2 = $("discount1x2").value;
    commissionSettings.discountCS = $("discountCS").value;
    commissionSettings.discountNumber = $("discountNumber").value;
}

function CheckChangeM3(ptSettingOld, ptSettingNew) {
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
}

function MemberMaximumPT() {
        var ptSetting = customer.ptSetting;
        var s4 = ptSetting.sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].pt.s4;
        var m3 = ptSetting.sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].pt.m3;
        return Fanex.Round(s4 - m3);
}

function SetNewValueToMember(isUpdateAllMember) {
        isShowPopUpSetPTMember = true;
        if (isUpdateAllMember) {
            var valueMemberPositionTaking = $("selectedMemberPositionTaking").value;
            newValueMemberPT = valueMemberPositionTaking + ";" + isUpdateAllMember;
        }
        var customerModel = new CustomerModel(customer);
        customerModel.saveCustomer();
}

function ConfirmYes() {
    SetNewValueToMember(1);
}

function ConfirmNo() {
    SetNewValueToMember(0);
}
