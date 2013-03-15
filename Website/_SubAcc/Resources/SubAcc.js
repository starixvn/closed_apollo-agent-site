// Update page access when user click on checkboxes
function UpdatePermission(agentUserId, agentId) {
    function OnComplete(result) {
        var errCode = result.errCode;
        if (errCode == 0) {
            alert("successfull");
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