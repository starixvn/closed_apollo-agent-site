Fanex.PTEngine.CURRENT_CONTEXT = context;
var ptGrid = new Fanex.PTEngine.PTGrid(customer.ptSetting);
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
    show(copiedElements[this.copiedItem.sportId], hint, 0, 10, 68, -10, top, 30000);
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

var div = $('ptGridContainer');
ptGrid.Render(div);

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
            customer.statusSetting = null;
            customer.commissionSetting = null;
            customer.commissionSetting2 = null;
            var text = JSON.stringify(customer);
            $("txtCustomerObject").value = text;
            var params = ajax.CreateParams('txtCustomerObject');

            function SuperUpdateMinPTMasterComplete(result) {
                var errMsg = result.errMsg;
                
                if (result.errCode == 0) {
                    if (_page.CallBack) {
                        eval(_page.CallBack);
                    }
                    else {
                        age.UnLock();
                        ageMsg.Show(errMsg, 1);
                        window.scroll(0, 0);
                        age.DelayReloadPage();

                        return;
                    }
                }
                else {
                    age.UnLock();
                    ageMsg.Show(errMsg);
                    window.scroll(0, 0);

                    return;
                }
            }

            age.ReloadPage(30000);
            // Post with return result in JSON format
            var urlSubmit;
            if (context.targetCustId > 0 && context.targetRoleId == 3) {
                urlSubmit = "../Handlers/SuperUpdateMinPTMaster.ashx";
                OnComplete = SuperUpdateMinPTMasterComplete;
            }

            ajax.PostJSON(
				urlSubmit,
				params,
				OnComplete
			);
        }
    };
};

Fanex.EditMinPT = {
    // Edit Min PT Master on Master list
    OnRedirectToPage: function (masterId) {
        top.main.DelayReloadPage();
    }
}

ko.applyBindings(new CustomerModel(customer));

// For Edit Position Taking
RegisterStartUp(function () {
    Fanex.Customer.HideTitle();
});