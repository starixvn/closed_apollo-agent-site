Fanex.PTEngine.CURRENT_CONTEXT = context;

var ptGrid = null;
ptGrid = new Fanex.PTEngine.PTGrid(customer.ptSetting);
var div = $('ptGridContainer');
ptGrid.Render(div);

var bettingLimit = null;
bettingLimit = new Fanex.PTEngine.BettingLimit(customer.bettingLimit)
var bettingLimitContainer = $('bettingLimitContainer');
bettingLimit.Render(bettingLimitContainer, true);

var currentStatusClose = customer.statusSetting.closed;
var currentStatusSuspend = customer.statusSetting.suspended;

/*
Bind customer information to customer JSON object by knockout.js
Render PTGrid
*/
var CustomerModel = function (customer) {
    this.saveCustomer = function () {
        // EscapeHTML String
        customer.firstName = customer.firstName.escapeHtml();
        customer.lastName = customer.lastName.escapeHtml();
        customer.phone = customer.phone.escapeHtml();
        customer.mobilePhone = customer.mobilePhone.escapeHtml();
        customer.fax = customer.fax.escapeHtml();

		if (ptGrid) {
			customer.ptSetting = ptGrid.Save();
			customer.bettingLimit = null;
			var text = JSON.stringify(customer);
			$("txtCustomerObject").value = text;
			var params = ajax.CreateParams('txtCustomerObject');

			function SuperUpdateMemberComplete(result) {
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
			if (context.targetCustId > 0 && context.targetRoleId == 1) { // Super update member
				urlSubmit = "../Handlers/SuperUpdateMember.ashx";
				OnComplete = SuperUpdateMemberComplete;
			}

			ajax.PostJSON(
				urlSubmit,
				params,
				OnComplete
			);
		}
	};
};

Fanex.EditPT = {
	// Edit PT Master on account Info
	OnRedirectToPage: function (agentId) {
		top.menu.reloadMenuUsrInfo();
		age.DelayReloadPage('../../../_MemberInfo/CustomerList/Super/MemberList.aspx?custid=' + agentId);
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
	}
}

ko.applyBindings(new CustomerModel(customer));

// For Edit Position Taking
RegisterStartUp(function () {
	age.addEvent($('closed'), 'click', function () {
		Fanex.EditPT.Closed($('closed'), currentStatusClose);
	});

	age.addEvent($('suspended'), 'click', function () {
		Fanex.EditPT.Suspended($('suspended'), currentStatusSuspend);
	});
	Fanex.Customer.HideTitle();
});