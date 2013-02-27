var maxMemberPT = 0;

function CheckChangeAutoPT() {
    for (var i = 0; i < AllBetType.length; i++) {
        var IsAutoSuper = "_page.IsAutoMaster_" + AllBetType[i];
        var IsAutoSuper_val = eval(IsAutoSuper) == true;
        var chkAutoSuper = "chkAutoMaster_" + AllBetType[i];
        if (IsAutoSuper_val && !$(chkAutoSuper).checked) return true;
    }
    return false;
}

function ConfirmChangeAuto() {
    var isChangeAuto = CheckChangeAutoPT();
    if (isChangeAuto) {
        var msPT_max = "_page.v153mapositiontaking_51_max";
        var msPT_max_val = eval(msPT_max);
        maxMemberPT = msPT_max_val - $("listAgent_151_41").value;
        ageWnd.open('ConfirmAutoPT.aspx', '', 300, 300, 450, 170, 'popup_main', 'popup_tile');
        return true;
    }
    return false;
}

function SetMemberPT() {
    if ($("listMemberPT")) {
        var maxMember = Math.round(parseFloat(parent.maxMemberPT) * 10);
        $("listMemberPT").options.length = 0;
        while (maxMember >= 0) {
            var val = round(maxMember / 10);
            $("listMemberPT").options.add(new Option(val, val));
            maxMember = maxMember - 1;
        }
    }
    document.body.style.display = 'block';
}

function Button_Onclick(action) {
    parent.$('memberPT').value = $("listMemberPT").value;
    parent.$('isUpdateMember').value = action;
    parent.ageWnd.close();
    parent.age.DelayReloadPage(null, 30000);
    parent.UpdateCustomer(parent.custids.pop());
}

RegisterStartUp("SetMemberPT()");