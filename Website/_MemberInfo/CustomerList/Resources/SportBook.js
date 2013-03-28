function EditMemberSetting(element, custId, userName, userGroup) {
    if (custId) {
        var url = "../../_MemberInfo/CustomerSetting/Pages/EditMemberSetting.aspx?custid=" + custId + "&username=" + userName + "&showheader=0&ugroup=" + userGroup;
        ageWnd.openDialog(url, '', {
            width: 990,
            height: 950,
            position: [10,10]
        });
    }
}
function EditAgentSetting(element, custId, userName, agentLevel) {
    if (custId) {
        var url = "../../_MemberInfo/CustomerSetting/Pages/EditAgentSetting.aspx?custid=" + custId
        + "&username=" + userName
        + "&showheader=0"
        + "&agentlevel=" + agentLevel;
        //url, title, left, top, width, height
        ageWnd.openDialog(url, '', {
            width: 990,
            height: 700,
            position: { my: "top+15", at: "center top", of: window }
        });
    }
}

function OpenEditAgentCommission(custId, userName, agentLevel) {
    if (custId) {
        var url = "../../_MemberInfo/CustomerSetting/Pages/EditAgentCommission.aspx?custid=" + custId
        + "&username=" + userName
        + "&showheader=0"
        + "&agentlevel=" + agentLevel;
        //url, title, left, top, width, height
        ageWnd.openDialog(url, '', {
            width: 990,
            height: 220,
            position: { my: "top+15", at: "center top", of: window }
        });
    }
}

function OpenEditAgentBetLimit(custId, userName, agentLevel) {
    if (custId) {
        var url = "../../_MemberInfo/CustomerSetting/Pages/EditAgentBetLimit.aspx?custid=" + custId
        + "&username=" + userName
        + "&showheader=0"
        + "&agentlevel=" + agentLevel;
        //url, title, left, top, width, height
        ageWnd.openDialog(url, '', {
            width: 990,
            height: 500,
            position: { my: "top+15", at: "center top", of: window }
        });
    }
}

function OpenEditAgentPT(custId, userName, agentLevel) {
    if (custId) {
        var url = "../../_MemberInfo/CustomerSetting/Pages/EditAgentPositionTaking.aspx?custid=" + custId
        + "&username=" + userName
        + "&showheader=0"
        + "&agentlevel=" + agentLevel;

        ageWnd.openDialog(url, '', {
            width: 990,
            height: 720,
            position: { my: "top+15", at: "center top", of: window }
        });
    }
}

function OpenEditMemberCommission(custId, userName, ugroup) {
    if (custId) {
        var url = "../../_MemberInfo/CustomerSetting/Pages/EditMemberCommission.aspx?custid=" + custId + "&username=" + userName + "&ugroup=" + ugroup + "&showheader=0";

        ageWnd.openDialog(url, '', {
            width: 990,
            height: 320,
            position: { my: "top+15", at: "center top", of: window }
        });
    }
}

function OpenEditMemberBetLimit(custId, userName, ugroup) {
    if (custId) {
        var url = "../../_MemberInfo/CustomerSetting/Pages/EditMemberBetLimit.aspx?custid=" + custId + "&username=" + userName + "&ugroup=" + ugroup + "&showheader=0";
        ageWnd.openDialog(url, '', {
            width: 990,
            height: 490,
            position: { my: "top+15", at: "center top", of: window }
        });
    }
}

function OpenEditMemberPT(custId, userName, ugroup) {
    if (custId) {
        var url = "../../_MemberInfo/CustomerSetting/Pages/EditMemberPositionTaking.aspx?custid=" + custId + "&username=" + userName + "&ugroup=" + ugroup + "&showheader=0";
        ageWnd.openDialog(url, '', {
            width: 990,
            height: 950,
            position: { my: "top+15", at: "center top", of: window }
        });
    }
}
