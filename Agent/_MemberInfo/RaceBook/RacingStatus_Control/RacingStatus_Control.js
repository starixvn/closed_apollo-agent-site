function DisableRacing_Onclick(el, downlineId) {
    var uplineID = '_page.' + el.id;
    var uplineOld = eval(uplineID);
    var downlineID = '_page.' + downlineId;
    var downlineOld = eval(downlineID);

    if (uplineOld == false && downlineOld == false) {
        if ($(downlineId))
            $(downlineId).checked = el.checked;
    }
    else if (uplineOld == true && downlineOld == true) {
        if ($(downlineId))
            $(downlineId).disabled = !el.checked;
        if (el.checked == false)
            if ($(downlineId))
                $(downlineId).checked = false;
    }
    else if (uplineOld == false && downlineOld == true) {
        if ($(downlineId))
            $(downlineId).disabled = true;
    }
    else {
        if($(downlineId))
            $(downlineId).checked = el.checked;
    }
}

function DisableHR(idchkHR, ischkHRDownline) {
    var idUplines = idchkHR.replace('chk', 'hd');
    var idDownlines = ischkHRDownline.replace('chk', 'hd');
    var idUplinesOld = idchkHR.replace('chk', 'hd') + '_Old';
    var idDownlinesOld = ischkHRDownline.replace('chk', 'hd') + '_Old';

    if ($(idUplinesOld).value == '0' && $(idDownlinesOld).value == '0') {
        if ($(ischkHRDownline)) $(ischkHRDownline).checked = $(idchkHR).checked;
    }
    else if ($(idUplinesOld).value == '1' && $(idDownlinesOld).value == '1') {
        if ($(ischkHRDownline)) {
            $(ischkHRDownline).disabled = !$(idchkHR).checked;
            if ($(idchkHR).checked == false)
                $(ischkHRDownline).checked = false;
        }
    }
    else if ($(idUplinesOld).value == '0' && $(idDownlinesOld).value == '1') {
        if ($(ischkHRDownline)) $(ischkHRDownline).disabled = true;
    }
    else {
        if ($(ischkHRDownline)) $(ischkHRDownline).checked = $(idchkHR).checked;
    }
    $(idUplines).value = $(idchkHR).checked ? '0' : '1';
    if ($(ischkHRDownline)) {
        $(idDownlines).value = $(ischkHRDownline).checked ? '0' : '1';
    }
}