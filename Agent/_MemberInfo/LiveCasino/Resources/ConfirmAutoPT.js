/*
    Created by: Andy    
    Description: General updating pt for all downlines' member
*/
function GetMemberPT() {
    var maxMember = (parseFloat(parent._page.MaxPT_Max) - parseFloat(parent.document.getElementById('pt_3').value)).toFixed(2);
    document.getElementById("ddlMemberPT").options.length = 0;
    while (maxMember >= 0) {
        var val = maxMember;
        document.getElementById("ddlMemberPT").options.add(new Option(val, val));
        maxMember = (maxMember - 0.01).toFixed(2);
    }
}

function ButtonClick(str) {
    parent.document.getElementById("newMemberPT").innerHTML = document.getElementById("ddlMemberPT").value;
    parent.document.getElementById("isUpdateAllMember").innerHTML = str;    
    parent.ageWnd.close();

    if (!parent.submit) {
        parent.submitPTComm();
    }
    else {
        parent.submitBtnSubmitClick();
    }
}

window.onload = function () {
    GetMemberPT();
}