//Edit Casino's Positontaking
function EditCS(cid) {
    var URL = "../../../MemberInfo/PositionTakingCS.aspx?custid=" + cid + "&isupdate=false";
    var popH = 600, popW = 980;
    //url, title, left, top, width, height
    ageWnd.open(URL, '', 0, 10, popW, popH);
}