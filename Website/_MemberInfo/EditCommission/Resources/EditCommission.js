if (commission.roleId == 1) {
	document.getElementById("textBoxDiscountAsian").value = commission.discount;
	document.getElementById("textBoxDiscount1x2").value = commission.discount1x2;
	document.getElementById("textBoxDiscountCS").value = commission.discountCS;
	document.getElementById("textBoxDiscountNumber").value = commission.discountNumber;
	Fanex.PTEngine.PT_SetSelect(document.getElementById("discountAsian"), 0, commission.discount, Fanex.PTEngine.COMMISSION_STEP, commission.playerDiscount);
	Fanex.PTEngine.PT_SetSelect(document.getElementById("discount1x2"), 0, commission.discount1x2, Fanex.PTEngine.COMMISSION_STEP, commission.playerDiscount1x2);
	Fanex.PTEngine.PT_SetSelect(document.getElementById("discountCS"), 0, commission.discountCS, Fanex.PTEngine.COMMISSION_STEP, commission.playerDiscountCs);
	Fanex.PTEngine.PT_SetSelect(document.getElementById("discountNumber"), 0, commission.discountNumber, Fanex.PTEngine.COMMISSION_STEP, commission.playerDiscountNumber);
}
else {
	Fanex.PTEngine.PT_SetSelect(document.getElementById("groupA"), 0, commission.groupAMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupA);
	Fanex.PTEngine.PT_SetSelect(document.getElementById("groupB"), 0, commission.groupBMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupB);
	Fanex.PTEngine.PT_SetSelect(document.getElementById("groupC"), 0, commission.groupCMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupC);
	Fanex.PTEngine.PT_SetSelect(document.getElementById("groupD"), 0, commission.groupDMax, Fanex.PTEngine.COMMISSION_STEP, commission.groupD);
	Fanex.PTEngine.PT_SetSelect(document.getElementById("discount1x2"), 0, commission.discount1x2Max, Fanex.PTEngine.COMMISSION_STEP, commission.discount1x2);
	Fanex.PTEngine.PT_SetSelect(document.getElementById("discountCS"), 0, commission.discountCSMax, Fanex.PTEngine.COMMISSION_STEP, commission.discountCS);
	Fanex.PTEngine.PT_SetSelect(document.getElementById("discountNumber"), 0, commission.discountNumberMax, Fanex.PTEngine.COMMISSION_STEP, commission.discountNumber);
};

function updateCommission(updateUrl) {
	if (commission.roleId == 1) {
		commission.playerDiscount = document.getElementById("discountAsian").value;
		commission.playerDiscount1x2 = document.getElementById("discount1x2").value;
		commission.playerDiscountCs = document.getElementById("discountCS").value;
		commission.playerDiscountNumber = document.getElementById("discountNumber").value;
	}
	else {
		Fanex.Customer.SetValueCommissionSettings(commission);
	}
	age.ReloadPage(3000);

	var data = JSON.stringify(commission);
	ajax.PostJSON(updateUrl, data, function onComplete(result) {
		var errMsg = result.errMsg;
		if (result.errCode == 0) {
		    age.UnLock();
		    AddPopupHeight(50);
			ageMsg.Show(errMsg, 1);
			window.scroll(0, 0);
			return;
		}
		else {
		    age.UnLock();
		    AddPopupHeight(50);
			ageMsg.Show(errMsg);
			window.scroll(0, 0);
			return;
		}
	});
}

RegisterStartUp(function () {
	Fanex.Customer.HideTitle();
});