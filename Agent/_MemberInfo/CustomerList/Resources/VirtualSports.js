var fullURL = '';

function EditSingleVirtualSports(custid, roleid, username) {
    $("arrayCustID").value = "";
    $("arrayUserName").value = "";
    EditVirtualSports(custid, roleid, username, false);
}

function EditVirtualSports(custid, roleid, username, isMulti) {
    var URL = "";
    var popH = 520, popW = 880;

    if (isMulti) {
        URL = '../../VirtualSports/EditMultiple.aspx?custid=';
        URL += custid + '&roleid=' + roleid + "&username=" + username;
    }
    else {
        URL = '../../VirtualSports/EditSingle.aspx?custid=';
        URL += custid + '&roleid=' + roleid + "&username=" + username;
    }

    //url, title, left, top, width, height
    ageWnd.open(URL, '', 0, 10, popW, popH);
}

function EditMultiVirtualSports() {
    var arCID = document.getElementsByName("chkid");
    if (!arCID) return;
    var SelCID = "";
    var SelUser = "";
    var intCount = 0;
    for (var i = 0; i < arCID.length; i++) {
        if (arCID[i].checked) {
            var status = arCID[i].getAttribute("statusvirtualsport");
            if (status == "0" || status == "1") {
                SelCID += arCID[i].id.split('_')[1] + "^";
                SelUser += arCID[i].value + "^";
                intCount += 1;
            }
        }
    }

    if (intCount == 0) return;
    var roleid = _page.roleidVirtualSports;

    if (intCount == 1) {
        var custid = SelCID.replace("^", "");
        var username = SelUser.replace("^", "");
        $("arrayCustID").value = "";
        $("arrayUserName").value = "";
        EditVirtualSports(custid, roleid, username, false);
    }
    else if (intCount > 1) {
        var custid = SelCID.split('^')[0];
        var username = SelUser.split('^')[0];
        $("arrayCustID").value = SelCID;
        $("arrayUserName").value = SelUser;
        EditVirtualSports(custid, roleid, username, true);
    }
}