var fullURL = '';

function EditP2P(custid, roleid, username) {
    var isDisabledP2P;
    isDisabledP2P = $(custid).getAttribute("isDisabledP2P");
   
    fullURL = '';
    var URL = '../../../_MemberInfo/P2P/P2P_Page.aspx?custids=' + custid + '&roleid=' + _page.roleidP2P + '&username=' + username + '&disable=' + isDisabledP2P;
    var popH = 550, popW = 950;
    ageWnd.open(URL, '', 0, 10, popW, popH);
}


function EditMulti(custid, status, isMult, page) {
    fullURL = '';
    var URL = "";
    var popH = 600,
        popW = 980;
    var SelStatus = "";
    var bHasCheckall = true;
    var bAddToList = false;
    var droleid = _page.roleidP2P;
    var arCID = document.getElementsByName("chkid");
    switch (page) {
        case 1:
            URL = "../../../MemberInfo/PositionTakingCS.aspx?";
            break;
        case 2:
            URL = "../../../MemberInfo/PositionTakingBingo.aspx?";
            break;
        case 3:
            URL = "../../P2P/P2P_Page.aspx?";
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
                if (page == 1) {
                    //Casino
                    if (arCID[i].getAttribute("statusCS") == "1") { //Already synchronized
                        if (firstSynCustId == 0) firstSynCustId = arCID[i].id.split('_')[1];
                        bAddToList = true;
                    } else if (arCID[i].getAttribute("statusCS") == "0") {
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
                        SelStatus += arCID[i].getAttribute("statusCS") + "^";
                        intCount += 1;
                    }
                }
                //                    Bingo
                else if (page == 2) {
                    if (arCID[i].getAttribute("statusbingo") == "1") { //Already synchronized
                        if (firstSynCustId == 0) firstSynCustId = arCID[i].id.split('_')[1];
                        bAddToList = true;
                    } else if (arCID[i].getAttribute("statusbingo") == "0") {
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
                        SelStatus += arCID[i].getAttribute("statusbingo") + "^";
                        intCount += 1;
                    }
                }
                //P2P
                if (page == 3) {
                    if (arCID[i].getAttribute("statusp2p") == "1") { //Already synchronized
                        if (firstSynCustId == 0) firstSynCustId = arCID[i].id.split('_')[1];
                        bAddToList = true;
                    } else if (arCID[i].getAttribute("statusp2p") == "0") {
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
                        SelStatus += arCID[i].getAttribute("statusp2p") + "^";
                        intCount += 1;
                    }
                }


            }
        }

        if (!bHasCheckall) checkall.checked = null;

        if (firstSynCustId == 0) {
            firstSynCustId = firstNotSynCustId;
        }


        if (intCount == 0) return;
        if (intCount == 1) {
            $("arrayCustID").value = "";
            $("arrayUserName").value = "";
            if (page == 3) {
                fullURL += "custids=" + SelCID + "&custnames=" + SelUser + "&isMult=0&roleid=" + droleid;
                URL += "custids=" + SelCID.split('^')[0] + "&custnames=" + SelUser.split('^')[0] + "&isMult=0&roleid=" + droleid;
            }
            else
                URL += "custid=" + firstSynCustId + "&isMult=0";
        } else if (intCount > 1) {
            $("arrayCustID").value = SelCID;
            $("arrayUserName").value = SelUser;
            $("arrayStatus").value = SelStatus;
            if (page == 3) {
                fullURL += "custids=" + SelCID + "&custnames=" + SelUser + "&isMult=1&roleid=" + droleid;
                URL += "custids=" + SelCID.split('^')[0] + "&custnames=" + SelUser.split('^')[0] + "&isMult=1&roleid=" + droleid + "&count=" + intCount;
            }
            else
                URL += "custid=" + firstSynCustId + "&isMult=1";
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