// Js file is dedicated to reports (Win Loss and Win Loss Detail)
// -- Common: --
//function urlFiltering() {
//    var url = '';
//    var showSB, showCasino, showP2P, showRB, showFI, showNG, showBI;

//    if (document.getElementById('chk_SB') != null) {
//        if (document.getElementById('chk_SB').checked == true) showSB = 1; else showSB = 0;
//    }
//    if (document.getElementById('chk_Casino') != null) {
//        if (document.getElementById('chk_Casino').checked == true) showCasino = 1; else showCasino = 0;
//    }
//    if (document.getElementById('chk_P2P') != null) {
//        if (document.getElementById('chk_P2P').checked == true) showP2P = 1; else showP2P = 0;
//    }
//    if (document.getElementById('chk_HR') != null) {
//        if (document.getElementById('chk_HR').checked == true) showRB = 1; else showRB = 0;
//    }
//    if (document.getElementById('chk_FI') != null) {
//        if (document.getElementById('chk_FI').checked == true) showFI = 1; else showFI = 0;
//    }
//    if (document.getElementById('chk_NG') != null) {
//        if (document.getElementById('chk_NG').checked == true) showNG = 1; else showNG = 0;
//    }
//    if (document.getElementById('chk_BI') != null) {
//        if (document.getElementById('chk_BI').checked == true) showBI = 1; else showBI = 0;
//    }
//    url += '&showSB=' + showSB + '&showCasino=' + showCasino + '&showP2P=' + showP2P + '&showRB=' + showRB + '&showFI=' + showFI + '&showNG=' + showNG + '&showBI=' + showBI;
//    return url;
//}

  
    function urlFiltering() {        
        var url = '';
        var all, showSB, showCasino, showP2P, showRB, showFI, showNG, showBI;
        
        if (document.getElementById('chk_all') != null) {
            if (document.getElementById('chk_all').value == 'on') all = "on"; else all = "off";
        }
        if (document.getElementById('chk_showsb') != null) {
            if (document.getElementById('chk_showsb').value == 'on') showSB = "on"; else showSB = "off";
        }
        if (document.getElementById('chk_showcasino') != null) {
            if (document.getElementById('chk_showcasino').value == 'on') showCasino = "on"; else showCasino = "off";
        }
        if (document.getElementById('chk_showp2p') != null) {
            if (document.getElementById('chk_showp2p').value == 'on') showP2P = "on"; else showP2P = "off";
        }
        if (document.getElementById('chk_showrb') != null) {
            if (document.getElementById('chk_showrb').value == 'on') showRB = "on"; else showRB = "off";
        }
        if (document.getElementById('chk_showfi') != null) {
            if (document.getElementById('chk_showfi').value == 'on') showFI = "on"; else showFI = "off";
        }
        if (document.getElementById('chk_showng') != null) {
            if (document.getElementById('chk_showng').value == 'on') showNG = "on"; else showNG = "off";            
        }
        if (document.getElementById('chk_showbi') != null) {
            if (document.getElementById('chk_showbi').value == 'on') showBI = "on"; else showBI = "off";            
        }    
        url += '&chk_all=' + all + '&chk_showsb=' + showSB + '&chk_showcasino=' + showCasino + '&chk_showp2p=' + showP2P + '&chk_showrb=' + showRB + '&chk_showfi=' + showFI + '&chk_showng=' + showNG + '&chk_showbi=' + showBI;
        return url;
    }

function urlDLFiltering() {
    var url = '';
    var showSB, showCasino, showP2P, showRB, showFI, showNG, showBI;
    var count = 0;

    if (document.getElementById('chk_SB') != null) {
        if (document.getElementById('chk_SB').checked == true) showSB = 1; else showSB = 0;
        count++;
    }
    if (document.getElementById('chk_Casino') != null) {
        if (document.getElementById('chk_Casino').checked == true) showCasino = 1; else showCasino = 0;
        count++;
    }
    if (document.getElementById('chk_P2P') != null) {
        if (document.getElementById('chk_P2P').checked == true) showP2P = 1; else showP2P = 0;
        count++;
    }
    if (document.getElementById('chk_HR') != null) {
        if (document.getElementById('chk_HR').checked == true) showRB = 1; else showRB = 0;
        count++;
    }
    if (document.getElementById('chk_FI') != null) {
        if (document.getElementById('chk_FI').checked == true) showFI = 1; else showFI = 0;
        count++;
    }
    if (document.getElementById('chk_NG') != null) {
        if (document.getElementById('chk_NG').checked == true) showNG = 1; else showNG = 0;
        count++;
    }
    if (document.getElementById('chk_BI') != null) {
        if (document.getElementById('chk_BI').checked == true) showBI = 1; else showBI = 0;
        count++;
    }

    if (count > 0) {
        url += '&showSB=' + showSB + '&showCasino=' + showCasino + '&showP2P=' + showP2P + '&showRB=' + showRB + '&showFI=' + showFI + '&showNG=' + showNG + '&showBI=' + showBI;
    }
    else {
        url += '&showSB=' + document.getElementById('hdSB').value + '&showCasino=' + document.getElementById('hdCasino').value + '&showP2P=' + '&showRB=' + document.getElementById('hdHR').value + '&showFI=' + document.getElementById('hdFI').value + '&showNG=' + document.getElementById('hdNG').value + '&showBI=' + document.getElementById('hdBI').value;
    }

    return url;
}
function InitOptionItems() {
    var arrayOptionItems = new Array(8);
    arrayOptionItems[0] = "chk_All";
    arrayOptionItems[1] = "chk_SB";
    arrayOptionItems[2] = "chk_Casino";
    arrayOptionItems[3] = "chk_P2P";
    arrayOptionItems[4] = "chk_HR";
    arrayOptionItems[5] = "chk_FI";
    arrayOptionItems[6] = "chk_NG";
    arrayOptionItems[7] = "chk_BI";

    return arrayOptionItems;
}
function CheckAll() {
    var arrayOptionItem = InitOptionItems();
    if (document.getElementById(arrayOptionItem[0]) != null) {
        var allStatus = document.getElementById(arrayOptionItem[0]).checked;
        for (var i = 1; i < arrayOptionItem.length; i++) {
            if (document.getElementById(arrayOptionItem[i]) != null) {
                document.getElementById(arrayOptionItem[i]).checked = allStatus;
            }
        }
    }
}
function IsCheck() {
    var arrayOptionItem = InitOptionItems();
    var isUncheck = true;
    var item;
    if (document.getElementById(arrayOptionItem[0]) != null) {
        for (var i = 1; i < arrayOptionItem.length; i++) {
            item = document.getElementById(arrayOptionItem[i]);
            if (item != undefined && item != null) {
                if (item.checked == false) {
                    isUncheck = false;
                    break;
                }
            }
        }
        document.getElementById(arrayOptionItem[0]).checked = isUncheck;
    }
}
function SearchByDate2(pageUrl) {    
    var url = pageUrl + '?' + 'custid=' + document.getElementById('hdcus').value + '&fdate=' + document.getElementById("fdate").value + '&tdate=' + document.getElementById("tdate").value;
    url += '&IsHistoryReport=' + document.getElementById('IsHistoryReport').value;
    if (document.getElementById('IsHistoryReport').value == '1' && document.getElementById('ddlMonth')) {
        url += '&month=' + document.getElementById('ddlMonth').value.split('^')[0];
        url += '&year=' + document.getElementById('ddlMonth').value.split('^')[1];
    }
    url += urlFiltering();
    _submit();    
    location.href = url;
}
function SearchOneDay2(pageUrl, value) {
    var url = pageUrl + '?' + 'custid=' + document.getElementById('hdcus').value + '&isdate=' + value;
    url += urlFiltering();
    _submit();
    location.href = url;
}
// --! Common --

// -- Win Loss: --
function urlBLFiltering() {
    var url = '';
    var showSB, showCasino, showP2P, showRB, showFI;
    var count = 0;

    if (document.getElementById('chk_SB') != null) {
        if (document.getElementById('chk_SB').checked == true) showSB = 1; else showSB = 0;
        count++;
    }
    if (document.getElementById('chk_Casino') != null) {
        if (document.getElementById('chk_Casino').checked == true) showCasino = 1; else showCasino = 0;
        count++;
    }
    if (document.getElementById('chk_P2P') != null) {
        if (document.getElementById('chk_P2P').checked == true) showP2P = 1; else showP2P = 0;
        count++;
    }
    if (document.getElementById('chk_HR') != null) {
        if (document.getElementById('chk_HR').checked == true) showRB = 1; else showRB = 0;
        count++;
    }
    if (document.getElementById('chk_FI') != null) {
        if (document.getElementById('chk_FI').checked == true) showFI = 1; else showFI = 0;
        count++;
    }

    if (count > 0) {
        url += '&showSB=' + showSB + '&showCasino=' + showCasino + '&showP2P=' + showP2P + '&showRB=' + showRB + '&showFI=' + showFI;
    }
    else {
        url += '&showSB=' + document.getElementById('hdSB').value + '&showCasino=' + document.getElementById('hdCasino').value + '&showP2P=' + '&showRB=' + document.getElementById('hdHR').value + '&showFI=' + document.getElementById('hdFI').value;
    }

    return url;
}
function doViewDownline2(pageURL, custid) {
    var url = pageURL + '?' + 'custid=' + custid + '&fdate=' + document.getElementById('txtFdate').value + '&tdate=' + document.getElementById('txtTdate').value;
    url += '&IsHistoryReport=' + document.getElementById('IsHistoryReport').value;
    url += '&IsVDL=1';
    if (document.getElementById('IsHistoryReport').value == '1' && document.getElementById('ddlMonth')) {
        url += '&month=' + document.getElementById('ddlMonth').value.split('^')[0];
        url += '&year=' + document.getElementById('ddlMonth').value.split('^')[1];
    }
    //url += urlFiltering();
    url += urlDLFiltering();
    window.location.href = url;
}
// --! Win Loss --

// -- Win Loss Detail: --
function doViewDownlineDetail2(pageURL, custid) {
    var url = pageURL + '?' + 'custid=' + custid;
    url += '&IsHistoryReport=' + document.getElementById('IsHistoryReport').value;
    if (document.getElementById('IsHistoryReport').value == '1' && document.getElementById('ddlMonth')) {
        url += '&month=' + document.getElementById('ddlMonth').value.split('^')[0];
        url += '&year=' + document.getElementById('ddlMonth').value.split('^')[1];
        url += '&fdate=' + document.getElementById('txtFdate').value.split('/')[1];
        url += '&tdate=' + document.getElementById('txtTdate').value.split('/')[1];
    }
    else {
        url += '&fdate=' + document.getElementById('txtFdate').value;
        url += '&tdate=' + document.getElementById('txtTdate').value;
    }
    //url += urlFiltering();
    url += urlDLFiltering();
    window.location.href = url;
}
function ChangeView(flag) {
    var f = 0;
    if (flag == 1)
        f = 1;
    else f = 0;

    var url = 'WinLossDetailMaster.aspx?fdate=' + document.getElementById('fdate').value + '&tdate=' + document.getElementById('tdate').value + '&custid=' + document.getElementById('hdcus').value;
    url += '&IsHistoryReport=' + document.getElementById('IsHistoryReport').value + '&oldView=' + f;
    if (document.getElementById('IsHistoryReport').value == '1' && document.getElementById('ddlMonth')) {
        url += '&month=' + document.getElementById('ddlMonth').value.split('^')[0];
        url += '&year=' + document.getElementById('ddlMonth').value.split('^')[1];
    }
    url += urlFiltering();
    window.location.href = url;
}
// --! Win Loss Detail --