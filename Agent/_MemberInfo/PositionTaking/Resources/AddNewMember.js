Fanex.PTEngine.CURRENT_CONTEXT = context;
Fanex.PTEngine.CURRENT_CONTEXT.ia3 = !customer.statusSetting.disableAutoMPT; // Allow auto PT master on member

var ptGrid = null;
ptGrid = new Fanex.PTEngine.PTGrid(customer.ptSetting);
var div = $('ptGridContainer');
ptGrid.Render(div);

var bettingLimit = null;
bettingLimit = new Fanex.PTEngine.BettingLimit(customer.bettingLimit)
var bettingLimitContainer = $('bettingLimitContainer');
bettingLimit.Render(bettingLimitContainer);

//console.log(customer);
/*
Bind customer information to customer JSON object by knockout.js
Render PTGrid
*/
var CustomerModel = function (customer) {
    var uplineUserName = customer.userName;
    // Set label MaxCredit
    customer.agentMaxCreditTemp = String.prototype.FormatNumber(customer.maxCredit);
    customer.maxCredit = 0;
    customer.credit = String.prototype.FormatNumber(customer.credit);

    this.saveCustomer = function () {
        if (!Fanex.Customer.CheckInfoValue()) return;
        if (!bettingLimit.Validate()) return;

        // For Check Edit Postion Taking
        if (context.targetCustId == 0) {
            // Re-set value for customer object
            customer.userName = uplineUserName + Fanex.Customer.SetAutoUserName();
            customer.group = $("group").value;
        }
        customer.commissionSetting.playerDiscount = $("discountAsian").value;
        customer.commissionSetting.playerDiscount1x2 = $("discount1x2").value;
        customer.commissionSetting.playerDiscountCs = $("discountCS").value;
        customer.commissionSetting.playerDiscountNumber = $("discountNumber").value;
        customer.credit = Fanex.RoundNumber(parseFloat($("credit").value, true), 1); // Fix bug for IE7

        // EscapeHTML String
        customer.firstName = $("firstName").value.escapeHtml();
        customer.lastName = $("lastName").value.escapeHtml();
        customer.phone = $("phone").value.escapeHtml();
        customer.mobilePhone = $("mobilePhone").value.escapeHtml();
        customer.fax = $("fax").value.escapeHtml();
        // Add more .....

        if (ptGrid) {
            customer.ptSetting = ptGrid.Save();
            customer.bettingLimit = bettingLimit.Save();
            var text = JSON.stringify(customer);
            $("txtCustomerObject").value = text;
            var params = ajax.CreateParams('txtCustomerObject');

            function AddNewMemberComplete(result) {
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
                    if (result.errCode == "5") {
                        Fanex.Customer.NewMember.ReLoadNewCaptcha();
                        $("textBoxCaptcha").value = "";
                        $('textBoxCaptcha').focus();
                    }
                    ageMsg.Show(errMsg);
                    window.scroll(0, 0);
                    return;
                }
            }

            function AgentUpdateMemberComplete(result) {
                if (result.errCode == 0) {
                    if (_page.CallBack) {
                        eval(_page.CallBack);
                    }
                    else {
                        top.main.DelayReloadPage();
                    }
                }
                else {
                    age.UnLock();
                    ageMsg.Show(result.errMsg);
                    window.scroll(0, 0);
                    return;
                }
            }

            age.ReloadPage(30000);
            // Post with return result in JSON format
            var urlSubmit;
            if (context.targetCustId > 0 && context.targetRoleId == 1) { // Agent update member
                urlSubmit = "../Handlers/AgentUpdateMember.ashx";
                OnComplete = AgentUpdateMemberComplete;
            }
            else {
                if ($("textBoxCaptcha") && $("textBoxCaptcha").value != "") {
                    urlSubmit = "../Handlers/SaveNewMember.ashx?captchaCode=" + $("textBoxCaptcha").value;
                }
                else {
                    urlSubmit = "../Handlers/SaveNewMember.ashx";
                }
                OnComplete = AddNewMemberComplete;
            }

            ajax.PostJSON(
				urlSubmit,
				params,
				OnComplete
			);
        }
    };
};

Fanex.Customer.NewMember = {
    // Edit PT Member on account Info
    OnRedirectToPage: function () {
        top.menu.reloadMenuUsrInfo();
        age.DelayReloadPage('../../../_MemberInfo/CustomerList/Agent/MemberList.aspx');
    },

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

    // Check or UnCheck Suspend Status
    Suspended: function (checkBoxSuspend, currentStatusSuspend) {
        if (checkBoxSuspend.checked == true && checkBoxSuspend.checked != currentStatusSuspend) {
            if (!confirm(Language.ALERT_CONFIRM_SUSPEND_MEMBER)) {
                customer.statusSetting.suspended = false;
                checkBoxSuspend.checked = false;
            }
        }
        else if (checkBoxSuspend.checked == false && checkBoxSuspend.checked != currentStatusSuspend) {
            if (!confirm(Language.ALERT_CONFIRM_UNSUSPEND_MEMBER)) {
                customer.statusSetting.suspended = true;
                checkBoxSuspend.checked = true;
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
    ReLoadNewCaptcha: function () {
        var objDate = new Date();
        $("imgCaptCha").src = "../../../_Authorization/Handlers/Captcha.ashx?code=" + objDate.getMilliseconds();
    }
}
var commission = customer.commissionSetting;
/* Render Commission to selected*/
if (context.targetCustId == 0) {
    Fanex.Customer.SetNextCustomer($("selectedOne"), $("selectedTwo"), $("selectedThree"), customer.newUserName);
    $("textBoxDiscountAsian").value = commission.groupA;
    Fanex.Customer.NewMember.LoadGroupCommission($("group"), context.currencyId, context.siteName);

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
    currentStatusSuspend = customer.statusSetting.suspended;
}

ko.applyBindings(new CustomerModel(customer));
RegisterStartUp(function () {
    if (context.targetCustId == 0) {
        age.addEvent($('group'), 'change', function () {
            Fanex.Customer.NewMember.ChangeGroupCommission($('group'));
        });
    }
    else {
        age.addEvent($('closed'), 'click', function () {
            Fanex.Customer.NewMember.Closed($('closed'), currentStatusClose);
        });

        age.addEvent($('suspended'), 'click', function () {
            Fanex.Customer.NewMember.Suspended($('suspended'), currentStatusSuspend);
        });
    }

    Fanex.Customer.CheckUser(customer.userName, 'selectedOne', 'selectedOne', 'selectedTwo', 'selectedThree', 'msgIcon');
    Fanex.Customer.CheckUser(customer.userName, 'selectedTwo', 'selectedOne', 'selectedTwo', 'selectedThree', 'msgIcon');
    Fanex.Customer.CheckUser(customer.userName, 'selectedThree', 'selectedOne', 'selectedTwo', 'selectedThree', 'msgIcon');
});