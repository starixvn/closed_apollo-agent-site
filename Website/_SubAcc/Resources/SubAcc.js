// Update page access when user click on checkboxes
function UpdatePermission(agentUserId, agentId) {
    function OnComplete(result) {
        var errCode = result.errCode;
        if (errCode == 0) {
            age.DelayReloadPage();
        }
        else {
            ageMsg.Show(result.errMsg);
        }
    }

    var value = '';
    if (document.getElementById("chk" + agentUserId + "[TF][R][H]").checked) {
        value += "[TF][R][H]-";
    }
    if (document.getElementById("chk" + agentUserId + "[RP][R][H]").checked) {
        value += "[RP][R][H]-";
    }
    if (document.getElementById("chk" + agentUserId + "[AC][R]").checked) {
        value += "[AC][R]-";
    }
    if (document.getElementById("chk" + agentUserId + "[AC][H]").checked) {
        value += "[AC][H]-";
    }
    if (document.getElementById("chk" + agentUserId + "[AD][R]").checked) {
        value += "[AD][R]-";
    }
    if (document.getElementById("chk" + agentUserId + "[AD][H]").checked) {
        value += "[AD][H]-";
    }
    if (value != '') {
        value = value.substring(0, value.length - 1);
    }

    var params = 'agentUserId=' + agentUserId + '&PageAccess=' + value + '&agentId=' + agentId;

    // Post with return result in JSON format
    ajax.PostJSON(
        'Handlers/UpdatePageAccess.ashx',
        params,
        OnComplete
    );
}

function onClick_ChangePermission(els) {
    var id = '';
    if (els.id.indexOf("[R]") != -1) {//Read
        id = els.id.replace('[R]', '[H]');
        document.getElementById(id).disabled = !els.checked;
        if (document.getElementById(id).checked) {
            document.getElementById(id).checked = false;
        }
    }
    else //Host
    {
        id = els.id.replace('[H]', '[R]');
        if (els.checked) {
            document.getElementById(id).checked = true;
            document.getElementById(id).disabled = true;
        }
        else {
            document.getElementById(id).disabled = els.checked;
        }
    }
}