function EditMemberSetting(element, custId, userName, userGroup) {
    if (custId) {
        var url = "../../_MemberInfo/CustomerSetting/Pages/EditMemberSetting.aspx?custid=" + custId + "&username=" + userName + "&showheader=0&ugroup=" + userGroup;
        var popW = 960, popH = 900;
        //url, title, left, top, width, height
        ageWnd.open(url, '', 50, 10, popW, popH);
    }
}
function EditAgentSetting(element, custId, userName, agentLevel) {
    if (custId) {
        var url = "../../_MemberInfo/CustomerSetting/Pages/EditAgentSetting.aspx?custid=" + custId
        + "&username=" + userName
        + "&showheader=0"
        + "&agentlevel=" + agentLevel;
        var popW = 960, popH = 650;
        //url, title, left, top, width, height
        ageWnd.open(url, '', 50, 10, popW, popH);
    }
}

function OpenEditAgentCommission(custId, userName, agentLevel) {
    if (custId) {
        var url = "../../_MemberInfo/CustomerSetting/Pages/EditAgentCommission.aspx?custid=" + custId
        + "&username=" + userName
        + "&showheader=0"
        + "&agentlevel=" + agentLevel;
        var popW = 955, popH = 170;
        //url, title, left, top, width, height
        ageWnd.open(url, '', 50, 200, popW, popH); 
    }
}

function OpenEditAgentBetLimit(custId, userName, agentLevel) {
    if (custId) {
        var url = "../../_MemberInfo/CustomerSetting/Pages/EditAgentBetLimit.aspx?custid=" + custId
        + "&username=" + userName
        + "&showheader=0"
        + "&agentlevel=" + agentLevel;
        var popW = 955, popH = 450;
        //url, title, left, top, width, height
        ageWnd.open(url, '', 50, 120, popW, popH);
    }
}

function OpenEditAgentPT(custId, userName, agentLevel) {
    if (custId) {
        var url = "../../_MemberInfo/CustomerSetting/Pages/EditAgentPositionTaking.aspx?custid=" + custId
        + "&username=" + userName
        + "&showheader=0"
        + "&agentlevel=" + agentLevel;
        var popW = 960, popH = 670;
        //url, title, left, top, width, height
        ageWnd.open(url, '', 50, 0, popW, popH); 
    }
}

function OpenEditMemberCommission(custId, userName, ugroup) {
    if (custId) {
        var url = "../../_MemberInfo/CustomerSetting/Pages/EditMemberCommission.aspx?custid=" + custId + "&username=" + userName + "&ugroup=" + ugroup + "&showheader=0";
        var popW = 955, popH = 270;
        //url, title, left, top, width, height
        ageWnd.open(url, '', 50, 200, popW, popH);
    }
}

function OpenEditMemberBetLimit(custId, userName, ugroup) {
    if (custId) {
        var url = "../../_MemberInfo/CustomerSetting/Pages/EditMemberBetLimit.aspx?custid=" + custId + "&username=" + userName + "&ugroup=" + ugroup + "&showheader=0";
        var popW = 955, popH = 440;
        //url, title, left, top, width, height
        ageWnd.open(url, '', 50, 120, popW, popH);
    }
}

function OpenEditMemberPT(custId, userName, ugroup) {
    if (custId) {
        var url = "../../_MemberInfo/CustomerSetting/Pages/EditMemberPositionTaking.aspx?custid=" + custId + "&username=" + userName + "&ugroup=" + ugroup + "&showheader=0";
        var popW = 960, popH = 900;
        //url, title, left, top, width, height
        ageWnd.open(url, '', 50, 10, popW, popH);
    }
}
