var noClickSelect = true;

function SelectLeague() {
    DisableButton();
        
    if (document.getElementById('BoxLeagueWL').style.display == '') {
        hiddenBoxLeague();
        EnableButton();
        return;
    }
    
    noClickSelect = false;
    var showsb = (document.getElementById('chk_showsb') && document.getElementById('chk_showsb').checked) ? '1' : '0';
    var showrb = (document.getElementById('chk_showrb') && document.getElementById('chk_showrb').checked) ? '1' : '0';
    var showng = (document.getElementById('chk_showng') && document.getElementById('chk_showng').checked) ? '1' : '0';
    var showp2p = (document.getElementById('chk_showp2p') && document.getElementById('chk_showp2p').checked) ? '1' : '0';
    var showlivecasino = (document.getElementById('chk_showlivecasino') && document.getElementById('chk_showlivecasino').checked) ? '1' : '0';
    var showvs = (document.getElementById('chk_showvs') && document.getElementById('chk_showvs').checked) ? '1' : '0';
    var NewFilterLeague = document.getElementById('fdate').value + document.getElementById('tdate').value + showsb + showrb + showng + showp2p + showlivecasino + showvs + document.getElementById('isChanged').value;
    
    if (_page.FilterLeague != null && _page.FilterLeague != NewFilterLeague) {
        _page.FilterLeague = NewFilterLeague;

        http = jx.request(
        '../Common/SelectLeagues.aspx', onComplete, 'POST'
           , 'fdate=' + document.getElementById('fdate').value + '&tdate=' + document.getElementById('tdate').value + '&showsb=' + showsb + '&showrb=' + showrb + '&showng=' + showng + '&showp2p=' + showp2p + '&showlivecasino=' + showlivecasino + '&showvs=' + showvs
        );
    }
    else {
        EnableButton();
        document.getElementById('dSelLeague').value = _page.hideLeague;
        document.getElementById('BoxLeagueWL').style.display = '';
    }

    function onComplete(rs) {
        if (typeof rs == 'object') {
            document.getElementById('dSelLeague').value = _page.hideLeague;
            document.getElementById('BoxLeagueWL').style.display = '';
            document.getElementById('ContentLeagueWL').innerHTML = rs.responseText;
            setLeagueIds();
            GetLeagueSportFull();
            EnableButton();
        }
    }
}

function isAllLeague(curLeague) {
    if (document.getElementById(curLeague).checked) {
        var isCheckAll = true;
        var ids = document.getElementsByTagName("input");
        for (var i = 0; i < ids.length; i++) {
            if (ids[i].type == "checkbox" && ids[i].id.substring(0, 3) == "chL") {
                if (ids[i].id == 'chLAll') continue;
                if (!document.getElementById(ids[i].id).checked) {
                    isCheckAll = false;
                    break;
                }
            }
        }
        if (document.getElementById('chLAll') != null) document.getElementById('chLAll').checked = isCheckAll;
    }
    else {
        if (document.getElementById('chLAll') != null) document.getElementById('chLAll').checked = false;
    }

    setLeagueIds();
}

function setLeagueIds() {
    var leagueIds = '';
    var ids = document.getElementsByTagName("input");
    for (var i = 0; i < ids.length; i++) {
        if (ids[i].type == "checkbox" && ids[i].id.substring(0, 3) == "chL") {
            if (ids[i].checked && ids[i].id != 'chLAll') {
                leagueIds += (leagueIds == '' ? ids[i].id.substring(3, ids[i].id.length) : ',' + ids[i].id.substring(3, ids[i].id.length));
            }
        }
    }
    document.getElementById('leagueIds').value = (leagueIds==''?'-1':leagueIds);
}

function CheckSelAllLeague() {
    var bolLeague = document.getElementById('chLAll').checked;
    var ids = document.getElementsByTagName("input");
    for (var i = 0; i < ids.length; i++) {
        if (ids[i].type == "checkbox" && ids[i].id.substring(0, 3) == "chL") {
            ids[i].checked = bolLeague;
        }
    }
    setLeagueIds();
}

function GetLeagueSportFull() {
    var ids = document.getElementsByTagName("input");
    document.getElementById('leagueFull').value = '';
    for (var i = 0; i < ids.length; i++) {
        if (ids[i].type == "checkbox" && ids[i].id.substring(0, 3) == "chL") {
            if (ids[i].checked && (ids[i].id != 'chL0') && (ids[i].id != 'chLAll')) {
                document.getElementById('leagueFull').value += ids[i].id.substring(3, ids[i].id.length) + ',';
            }
        }
    }
    if (document.getElementById('leagueFull').value.substring(document.getElementById('leagueFull').value.length - 1, document.getElementById('leagueFull').value.length) == ',')
        document.getElementById('leagueFull').value = document.getElementById('leagueFull').value.substring(0, document.getElementById('leagueFull').value.length-1);
}

function Winloss_SearchByDate(fdate, tdate, targetCustId, actionPage) {
    hiddenBoxLeague();
    var showsb = (document.getElementById('chk_showsb') && document.getElementById('chk_showsb').checked) ? '1' : '0';
    var showrb = (document.getElementById('chk_showrb') && document.getElementById('chk_showrb').checked) ? '1' : '0';
    var showng = (document.getElementById('chk_showng') && document.getElementById('chk_showng').checked) ? '1' : '0';
    var showp2p = (document.getElementById('chk_showp2p') && document.getElementById('chk_showp2p').checked) ? '1' : '0';
    var showlivecasino = (document.getElementById('chk_showlivecasino') && document.getElementById('chk_showlivecasino').checked) ? '1' : '0';
    var showvs = (document.getElementById('chk_showvs') && document.getElementById('chk_showvs').checked) ? '1' : '0';

    var NewFilterLeague = document.getElementById('fdate').value + document.getElementById('tdate').value + showsb + showrb + showng + showp2p + showlivecasino + showvs + document.getElementById('isChanged').value;
    if (_page.FilterLeague != null && _page.FilterLeague != NewFilterLeague) {
        _page.FilterLeague = NewFilterLeague;
        document.getElementById('isChanged').value = '1';
        document.getElementById('leagueIds').value = '';
        document.getElementById('leagueFull').value = '';
    }

    if (targetCustId != null && actionPage != null) {
        document.getElementById("CustId").value = targetCustId;

        var url = document.getElementById("frmmain").action;
        url = url.substring(0, url.lastIndexOf("/", 0)) + actionPage;
        document.getElementById("frmmain").action = url;
    }

    if (fdate != null && tdate != null) {
        document.getElementById("fdate").value = fdate;
        document.getElementById("tdate").value = tdate;
    }

    document.getElementById("frmmain").submit();
}

function RestoreFormState() {
    if (document.getElementById("dSubmit")) document.getElementById("dSubmit").disabled = false;
    if (document.getElementById("dToday")) document.getElementById("dToday").disabled = false
    if (document.getElementById("dYesterday")) document.getElementById("dYesterday").disabled = false;
    if (document.getElementById("dSelLeague")) document.getElementById("dSelLeague").disabled = false;
    
    if (document.getElementById("CustId") && null != _page.CustId) document.getElementById("CustId").value = _page.CustId;
}

function setVisibleBoxLeague() {
    if (noClickSelect) {
        hiddenBoxLeague();
    }
    noClickSelect = true;
}

function hiddenBoxLeague() {
    document.getElementById('dSelLeague').value = _page.selectLeague;
    document.getElementById("BoxLeagueWL").style.display = 'none';
}

function InitExcelFunction() {
    var exportExcelImg = document.getElementById("exporttoexcel");
    var mainForm = document.forms[0];
    if (exportExcelImg && mainForm) {

        exportExcelImg.onclick = function() {
            document.forms[0].method = 'post';
        }

        exportExcelImg.onblur = function() {
            document.getElementById("exporttoexcel").focused = false;
        }
        exportExcelImg.onfocus = function() {
            document.getElementById("exporttoexcel").focused = true;
        }

        mainForm.onkeypress = function(evt) {
            var keyPressEvent = evt ? evt : window.event;
            if (keyPressEvent.keyCode == 13) {
                var src = keyPressEvent.srcElement || keyPressEvent.target;
                if (!src || (src.tagName.toLowerCase() != "textarea")) {
                    if (!document.getElementById("exporttoexcel").focused) {
                        if (keyPressEvent.stopPropagation) keyPressEvent.stopPropagation();
                        return false;
                    }
                }
            }
            return true;
        }
    }
}