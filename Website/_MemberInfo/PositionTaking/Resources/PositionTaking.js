function EditMemberRacing(cid, user, agentid, Customer) {
    var URL = "../_MemberInfo/RaceBook/Agent/EditMember.aspx?"; //V1

    if (Customer == 'Super') URL = "../_MemberInfo/RaceBook/Super/EditMember.aspx?";
    if (Customer == 'Master') URL = "../_MemberInfo/RaceBook/Master/EditMember.aspx?";

    URL += "m=0";
    URL += "&custid=" + cid;
    URL += "&USER=" + user;
    URL += '&agentid=' + agentid;
    var popH = 950, popW = 1024;

    if ($("arrayCustID")) {
        $("arrayCustID").value = "";
        $("arrayUserName").value = "";
    }

    //url, title, left, top, width, height
    ageWnd.open(URL, '', 0, 10, popW, popH);
}