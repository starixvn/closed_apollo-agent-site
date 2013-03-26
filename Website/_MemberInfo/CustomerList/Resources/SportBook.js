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
        var popW = 960, popH = 780;
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
        var popW = 960, popH = 780;
        //url, title, left, top, width, height
        ageWnd.open(url, '', 50, 10, popW, popH); 
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

//function OpenEditMinMax(custid, username) {
//    //var url = "../../../UserSuper/EditMinPT.aspx?custid=" + custid + "&username=" + username;
//    var url = "";
//    if (_page.roleid == 4) {
//        url = "../../../_MemberInfo/PositionTaking/Super/EditMinPTMaster.aspx?custid=" + custid + "&user=" + username + "&CallBack=Fanex.EditMinPT.OnRedirectToPage()";
//    }
//    else if (_page.roleid == 3) {
//        url = "../../../_MemberInfo/PositionTaking/Master/EditMinPTAgent.aspx?custid=" + custid + "&user=" + username + "&CallBack=Fanex.EditMinPT.OnRedirectToPage()";
//    }

//	var popW = 870, popH = 810;

//	//url, title, left, top, width, height
//	ageWnd.open(url, '', 0, 10, popW, popH);
//}

//function EditMember_Multi(agentid, Customer, masterid) {
//	arrCusId = "";
//	selectCusId = "";
//	selectUser = "";
//	Count = 0;
//	CheckedCount("chkid");
//	if (!arrCusId) return;

//	if (Count == 0) return;
//	if (Count == 1) {
//		var URL = "../../PositionTaking/Agent/EditMember.aspx?";
//		if (Customer == 'Super') URL = "../../../UserSuper/EditMember.aspx?";
//		if (Customer == 'Master') URL = "../../../UserMaster/EditMember.aspx?";
//		URL += "m=0";
//		URL += "&custid=" + selectCusId.replace("^", "");
//		URL += "&USER=" + selectUser.replace("^", "");
//		var popH = 950,
//            popW = 890;
//		document.getElementById("arrayCustID").value = "";
//		document.getElementById("arrayUserName").value = "";
//	} else if (Count > 1) {
//        var URL = "../../PositionTaking/Agent/EditMultipleMembers.aspx?";

//        if (Customer == 'Super') URL = "../../PositionTaking/Super/EditMultipleMembers.aspx?";
//        if (Customer == 'Master') URL = "../../PositionTaking/Master/EditMultipleMembers.aspx?";

//		URL += "m=1";
//		URL += "&custid=" + selectCusId.split('^')[0];
//		URL += "&USER=" + selectUser.split('^')[0];

//		document.getElementById("arrayCustID").value = selectCusId;
//		document.getElementById("arrayUserName").value = selectUser;
//		var popH = 950,
//            popW = 884;
//	}

//	URL += '&agentid=' + agentid + '&masterid=' + masterid;

//	//url, title, left, top, width, height
//	ageWnd.open(URL, '', 0, 10, popW, popH);
//}

//function EditMember_Single(cid, user, agentid, Customer) {
//	var URL = "../../PositionTaking/Agent/EditMember.aspx?"; //V1
//	var popH = 950;
//	var popW = 890;

//	if (Customer == 'Super') {
//		URL = "../../PositionTaking/Super/EditMember.aspx?";
//		popH = 950;
//		popW = 890;
//	}

//	if (Customer == 'Master') {
//	    URL = "../../PositionTaking/Master/EditMember.aspx?";
//	}

//	URL += "m=0";
//	URL += "&custid=" + cid;
//	URL += "&USER=" + user;
//	URL += '&agentid=' + agentid;

//	//url, title, left, top, width, height
//	ageWnd.open(URL, '', 0, 10, popW, popH);
//}

////this function is used when master edit agent that from agent list -- edit multi
//function EditAgent_Multi(masterid) {
//	arrCusId = "";
//	selectCusId = "";
//	selectUser = "";
//	Count = 0;
//	CheckedCount("chkid");
//	if (!arrCusId) return;

//	if (Count == 0) return;
//	if (Count == 1) {
//		var URL = "../../PositionTaking/Master/EditAgent.aspx?";
//		URL += "m=0";
//		var popH = 950, popW = 890;
//		URL += "&custid=" + selectCusId.replace("^", "");
//		URL += "&USER=" + selectUser.replace("^", "");
//		document.getElementById("arrayCustID").value = "";
//		document.getElementById("arrayUserName").value = "";
//	} else if (Count > 1) {
//        var URL = "../../PositionTaking/Master/EditMultipleAgents.aspx?";
//        URL += "m=1";
//        URL += "&custid=" + selectCusId.split('^')[0];
//        URL += "&USER=" + selectUser.split('^')[0];
//		var popH = 950, popW = 884;
//		document.getElementById("arrayCustID").value = selectCusId;
//		document.getElementById("arrayUserName").value = selectUser;
//	}

//	URL += '&masterid=' + masterid;

//	//url, title, left, top, width, height
//	ageWnd.open(URL, '', 0, 10, popW, popH);
//}

////this function is used when master edit agent that from agent list -- edit single
//function EditAgent_Single(id, us, masterid) {
//	document.getElementById("arrayCustID").value = "";
//	document.getElementById("arrayUserName").value = "";
//	var url = "../../PositionTaking/Master/EditAgent.aspx?m=0&custid=" + id + "&USER=" + us + '&masterid=' + masterid; //V1
//	var popH = 950, popW = 890;

//	//url, title, left, top, width, height
//	ageWnd.open(url, '', 0, 10, popW, popH);
//}