// Update page access when user click on checkboxes
function UpdatePermission(agentUserId, agentId) {
    function OnComplete(result) {
        var errCode = result.errCode;
        if (errCode == 0) {
            alert("Update permission successfully!!");
        }
    }

    var value = '';
    if ($("chk" + agentUserId + "[TF][R][H]").checked) {
        value += "[TF][R][H]-";
    }
    if ($("chk" + agentUserId + "[RP][R][H]").checked) {
        value += "[RP][R][H]-";
    }
    if ($("chk" + agentUserId + "[AC][R]").checked) {
        value += "[AC][R]-";
    }
    if ($("chk" + agentUserId + "[AC][H]").checked) {
        value += "[AC][H]-";
    }
    if ($("chk" + agentUserId + "[SM][R]").checked) {
        value += "[SM][R]-";
    }
    if ($("chk" + agentUserId + "[SM][H]").checked) {
        value += "[SM][H]-";
    }
    if ($("chk" + agentUserId + "[AD][R]").checked) {
        value += "[AD][R]-";
    }
    if ($("chk" + agentUserId + "[AD][H]").checked) {
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
        $(id).disabled = !els.checked;
        if ($(id).checked) {
            $(id).checked = false;
        }
    }
    else //Host
    {
        id = els.id.replace('[H]', '[R]');
        $(id).disabled = els.checked;
        $(id).checked = els.checked;
    }
}