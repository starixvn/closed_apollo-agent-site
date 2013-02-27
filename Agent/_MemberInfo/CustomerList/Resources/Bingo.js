function EditBG(cid) {
    var URL = "../../../MemberInfo/PositionTakingBingo.aspx?custid=" + cid + "&isupdate=false";

    var popH = 600, popW = 980;
    //url, title, left, top, width, height
    ageWnd.open(URL, '', 0, 10, popW, popH);
}