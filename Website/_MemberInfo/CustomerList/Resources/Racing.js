// Edit HorseRacing's  PositionTaking
function CustomerEditHR(custid, actionOn, isMult) {
    var url = "";
    var popH = 850, popW = 950;
    switch (_page.roleid) {
        case 4: // Super
            url += "../../RaceBook/Super/";
            if (isMult == 0) {
                if (actionOn == 'ViewAgent') {
                    url += "ViewAgent.aspx?custid=" + custid;
                }
                else if (actionOn == "Member") {
                    url += "EditMember.aspx?custid=" + custid;
                }
                else {
                    url += "EditMaster.aspx?custid=" + custid;
                }
                document.getElementById("arrayCustID").value = custid;
                document.getElementById("arrayUserName").value = "";
            }
            else {
                custid = GetCustID("chkid");
                if (actionOn == "Member") {
                    url += "EditMultMember.aspx?custid=" + custid;
                }
                else {
                    url += "EditMultMaster.aspx?custid=" + custid;
                }
            }
            break;
        case 3: // Master
            url += "../../RaceBook/Master/";
            if (isMult == 0) {
                if (actionOn == "Member") {
                    url += "EditMember.aspx?custid=" + custid;
                }
                else {
                    url += "EditAgent.aspx?custid=" + custid;
                }
                document.getElementById("arrayCustID").value = custid;
                document.getElementById("arrayUserName").value = "";
            }
            else {
                custid = GetCustID("chkid");
                if (actionOn == "Member") {
                    url += "EditMultMember.aspx?custid=" + custid;
                }
                else {
                    url += "EditMultAgent.aspx?custid=" + custid;
                }
            }
            break;
        case 2: // Agent
            url += "../../RaceBook/Agent/";
            if (isMult == 0) {
                url += "EditMember.aspx?custid=" + custid;
                document.getElementById("arrayCustID").value = custid;
                document.getElementById("arrayUserName").value = "";
            }
            else {
                custid = GetCustID("chkid");
                url += "EditMultMember.aspx?custid=" + custid;
            }
            break;
    }
    ageWnd.open(url, '', 0, 10, popW, popH);

}

function OpenEditMinMaxHR(custid, username) {
    // var url = "../MemberInfo/EditMinHR.aspx?custid=" + custid + "&username=" + username;
    var url = '';
    if (_page.roleid == 4) {
        url = '../../RaceBook/Super/EditMinPT.aspx';
    }

    else {
        url = '../../RaceBook/Master/EditMinPT.aspx';
    }

    url += '?custid=' + custid;
    document.getElementById("arrayCustID").value = custid;
    var popW = 800, popH = 900;

    //url, title, left, top, width, height
    ageWnd.open(url, '', 0, 10, popW, popH);
}