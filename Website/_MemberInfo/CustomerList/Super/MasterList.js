// Super edit SINGLE Master
function EditMaster_Single(cid, cname, superid) {
	$("arrayCustID").value = "";
	$("arrayUserName").value = "";
	var URL = "../../PositionTaking/Super/EditMaster.aspx?"; //V1

	URL += "m=0";
	var popH = 950, popW = 890;
	URL += "&custid=" + cid;
	URL += "&USER=" + cname;
	URL += '&superid=' + superid;
	var w = screen.availWidth;
	var h = screen.availHeight;
	var leftPos = (w - popW) / 2, topPos = (h - popH) / 2;

	//url, title, left, top, width, height
	ageWnd.open(URL, '', 0, 10, popW, popH);
}

// Super edit MULTI Master
function EditMaster_Multi(superid) {
	var arCID = document.getElementsByName("chkid");
	if (!arCID) return;
	var SelCID = "";
	var SelUser = "";
	var intCount = 0;
	for (var i = 0; i < arCID.length; i++) {
		if (arCID[i].checked) {
			SelCID += arCID[i].id.split('_')[1] + "^";
			SelUser += arCID[i].value + "^";
			intCount += 1;
		}
	}

	if (intCount == 0) return;
	if (intCount == 1) {
		var URL = "../../PositionTaking/Super/EditMaster.aspx?";
		URL += "m=0";
		var popH = 950, popW = 890;
		URL += "&custid=" + SelCID.replace("^", "");
		URL += "&USER=" + SelUser.replace("^", "");
		$("arrayCustID").value = "";
		$("arrayUserName").value = "";
	} else if (intCount > 1) {
        var URL = "../../PositionTaking/Super/EditMultipleMasters.aspx?";
        URL += "m=1";
        URL += "&custid=" + SelCID.split('^')[0];
        URL += "&USER=" + SelUser.split('^')[0];
        var popH = 950, popW = 884;
		$("arrayCustID").value = SelCID;
		$("arrayUserName").value = SelUser;
	}

	URL += '&superid=' + superid;
	//url, title, left, top, width, height
	ageWnd.open(URL, '', 0, 10, popW, popH);
}