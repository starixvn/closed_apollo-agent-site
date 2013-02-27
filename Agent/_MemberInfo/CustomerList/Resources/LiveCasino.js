var fullURL = '';
//Edit Live Casino's Positontaking
function EditLiveCS(custid, role, username) {
    var URL = "";
    var popH = 600, popW = 980;
    fullURL = '';
    var isDisabledLiveCS;
    isDisabledLiveCS = $(custid).getAttribute("isDisabledLiveCS");

    if (role == 4 && _page.roleidLiveCasino == 3) {
        URL = '../../LiveCasino/Super/EditMaster.aspx?custids=';
    }
    else if (role == 4 && _page.roleidLiveCasino == 1) {
        URL = '../../LiveCasino/Super/EditMember.aspx?custids=';
    }
    else if (role == 3 && _page.roleidLiveCasino == 2) {
        URL = '../../LiveCasino/Master/EditAgent.aspx?custids=';
    }
    else if (role == 3 && _page.roleidLiveCasino == 1) {
        URL = '../../LiveCasino/Master/EditMember.aspx?custids=';
    }
    else if (role == 2) {
        URL = '../../LiveCasino/Agent/EditMember.aspx?custids=';
    }

    URL += custid + '&roleid=' + _page.roleidLiveCasino + '&isMult=0' + '&disable=' + isDisabledLiveCS + "&username=" + username;
   
    //url, title, left, top, width, height
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

function EditMultiLiveCasino(custid, status, isMult, page) {
    fullURL = '';
    var URL = "";
    var popH = 600,
        popW = 980;
    var SelStatus = "";
    var bHasCheckall = true;
    var bAddToList = false;
    var droleid = _page.roleidLiveCasino;
    var arCID = document.getElementsByName("chkid");
    switch (page) {
        case 1:
            URL = "../../LiveCasino/Super/EditMaster.aspx?";
            break;
        case 2:
            URL = "../../LiveCasino/Super/EditMember.aspx?";
            break;
        case 3:
            URL = "../../LiveCasino/Master/EditAgent.aspx?";
            break;
        case 4:
            URL = "../../LiveCasino/Master/EditMember.aspx?";
            break;
        case 5:
            URL = "../../LiveCasino/Agent/EditMember.aspx?";
            break;
    }

    if (isMult == 1) {
        if (!arCID) return;
        var SelCID = "";
        var SelUser = "";
        var firstSynCustId = 0;
        var firstNotSynCustId = 0;
        var intCount = 0;
        for (var i = 0; i < arCID.length; i++) {
            if (arCID[i].checked) {
                if (arCID[i].getAttribute("statusLiveCasino") == "1") { //Already synchronized
                    if (firstSynCustId == 0) firstSynCustId = arCID[i].id.split('_')[1];
                    bAddToList = true;
                } else if (arCID[i].getAttribute("statusLiveCasino") == "0") {
                    if (firstNotSynCustId == 0) firstNotSynCustId = arCID[i].id.split('_')[1]
                    bAddToList = true;
                } else {
                    arCID[i].checked = null;
                    bHasCheckall = false;
                    bAddToList = false;
                }

                if (bAddToList) {
                    SelCID += arCID[i].id.split('_')[1] + "^";
                    SelUser += arCID[i].value.substring(arCID[i].value.indexOf(";") + 1) + "^";
                    SelStatus += arCID[i].getAttribute("statusLiveCasino") + "^";
                    intCount += 1;
                }              
            }
        }

//        if (!bHasCheckall) checkall.checked = null;

        if (firstSynCustId == 0) {
            firstSynCustId = firstNotSynCustId;
        }


        if (intCount == 0) return;
        if (intCount == 1) {
            $("arrayCustID").value = "";
            $("arrayUserName").value = "";

            fullURL += "custids=" + SelCID + "&custnames=" + SelUser + "&isMult=0&roleid=" + droleid;
            URL += "custids=" + SelCID.split('^')[0] + "&custnames=" + SelUser.split('^')[0] + "&isMult=0&roleid=" + droleid;
            
            
        } else if (intCount > 1) {
            $("arrayCustID").value = SelCID;
            $("arrayUserName").value = SelUser;
            $("arrayStatus").value = SelStatus;
            fullURL += "custids=" + SelCID + "&custnames=" + SelUser + "&isMult=1&roleid=" + droleid;
            URL += "custids=" + SelCID.split('^')[0] + "&custnames=" + SelUser.split('^')[0] + "&isMult=1&roleid=" + droleid + "&count=" + intCount;
            
        }
    } else { // isMult == 0
        $("arrayCustID").value = "";
        $("arrayUserName").value = "";
        $("arrayStatus").value = "";
        if (page == 3) {
            var username = arCID[0].value.substring(arCID[0].value.indexOf(";") + 1);

            fullURL += "custids=" + custid + "&username=" + username + "&isMult=0&roleid=" + droleid;
            URL += "custids=" + custid + "&username=" + username + "&isMult=0&roleid=" + droleid;
        }
        else
            URL += "custid=" + custid + "&isMult=0&status=" + SelStatus.substring(0, SelStatus.indexOf("^"));
    }

    ageWnd.open(URL, '', 0, 10, popW, popH);
}