var noClickSelect = true;

function SelectLeague() {
    DisableButton();
        
    if ($('BoxLeagueWL').style.display == '') {
        hiddenBoxLeague();
        EnableButton();
        return;
    }
    
    noClickSelect = false;
    var showsb = ($('chk_showsb') && $('chk_showsb').checked) ? '1' : '0';
    var showrb = ($('chk_showrb') && $('chk_showrb').checked) ? '1' : '0';
    var showng = ($('chk_showng') && $('chk_showng').checked) ? '1' : '0';
    var showp2p = ($('chk_showp2p') && $('chk_showp2p').checked) ? '1' : '0';
    var showlivecasino = ($('chk_showlivecasino') && $('chk_showlivecasino').checked) ? '1' : '0';
    var showvs = ($('chk_showvs') && $('chk_showvs').checked) ? '1' : '0';
    var NewFilterLeague = $('fdate').value + $('tdate').value + showsb + showrb + showng + showp2p + showlivecasino + showvs + $('isChanged').value;
    
    if (_page.FilterLeague != null && _page.FilterLeague != NewFilterLeague) {
        _page.FilterLeague = NewFilterLeague;

        http = jx.request(
        '../Common/SelectLeagues.aspx', onComplete, 'POST'
           , 'fdate=' + $('fdate').value + '&tdate=' + $('tdate').value + '&showsb=' + showsb + '&showrb=' + showrb + '&showng=' + showng + '&showp2p=' + showp2p + '&showlivecasino=' + showlivecasino + '&showvs=' + showvs
        );
    }
    else {
        EnableButton();
        $('dSelLeague').value = _page.hideLeague;
        $('BoxLeagueWL').style.display = '';
    }

    function onComplete(rs) {
        if (typeof rs == 'object') {
            $('dSelLeague').value = _page.hideLeague;
            $('BoxLeagueWL').style.display = '';
            $('ContentLeagueWL').innerHTML = rs.responseText;
            setLeagueIds();
            GetLeagueSportFull();
            EnableButton();
        }
    }
}

function isAllLeague(curLeague) {
    if ($(curLeague).checked) {
        var isCheckAll = true;
        var ids = document.getElementsByTagName("input");
        for (var i = 0; i < ids.length; i++) {
            if (ids[i].type == "checkbox" && ids[i].id.substring(0, 3) == "chL") {
                if (ids[i].id == 'chLAll') continue;
                if (!$(ids[i].id).checked) {
                    isCheckAll = false;
                    break;
                }
            }
        }
        if ($('chLAll') != null) $('chLAll').checked = isCheckAll;
    }
    else {
        if ($('chLAll') != null) $('chLAll').checked = false;
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
    $('leagueIds').value = (leagueIds==''?'-1':leagueIds);
}

function CheckSelAllLeague() {
    var bolLeague = $('chLAll').checked;
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
    $('leagueFull').value = '';
    for (var i = 0; i < ids.length; i++) {
        if (ids[i].type == "checkbox" && ids[i].id.substring(0, 3) == "chL") {
            if (ids[i].checked && (ids[i].id != 'chL0') && (ids[i].id != 'chLAll')) {
                $('leagueFull').value += ids[i].id.substring(3, ids[i].id.length) + ',';
            }
        }
    }
    if ($('leagueFull').value.substring($('leagueFull').value.length - 1, $('leagueFull').value.length) == ',')
        $('leagueFull').value = $('leagueFull').value.substring(0, $('leagueFull').value.length-1);
}

function Winloss_SearchByDate(fdate, tdate, targetCustId, actionPage) {
    hiddenBoxLeague();
    var showsb = ($('chk_showsb') && $('chk_showsb').checked) ? '1' : '0';
    var showrb = ($('chk_showrb') && $('chk_showrb').checked) ? '1' : '0';
    var showng = ($('chk_showng') && $('chk_showng').checked) ? '1' : '0';
    var showp2p = ($('chk_showp2p') && $('chk_showp2p').checked) ? '1' : '0';
    var showlivecasino = ($('chk_showlivecasino') && $('chk_showlivecasino').checked) ? '1' : '0';
    var showvs = ($('chk_showvs') && $('chk_showvs').checked) ? '1' : '0';

    var NewFilterLeague = $('fdate').value + $('tdate').value + showsb + showrb + showng + showp2p + showlivecasino + showvs + $('isChanged').value;
    if (_page.FilterLeague != null && _page.FilterLeague != NewFilterLeague) {
        _page.FilterLeague = NewFilterLeague;
        $('isChanged').value = '1';
        $('leagueIds').value = '';
        $('leagueFull').value = '';
    }

    if (targetCustId != null && actionPage != null) {
        $("CustId").value = targetCustId;

        var url = $("frmmain").action;
        url = url.substring(0, url.lastIndexOf("/", 0)) + actionPage;
        $("frmmain").action = url;
    }

    if (fdate != null && tdate != null) {
        $("fdate").value = fdate;
        $("tdate").value = tdate;
    }

    $("frmmain").submit();
}

function RestoreFormState() {
    if ($("dSubmit")) $("dSubmit").disabled = false;
    if ($("dToday")) $("dToday").disabled = false
    if ($("dYesterday")) $("dYesterday").disabled = false;
    if ($("dSelLeague")) $("dSelLeague").disabled = false;
    
    if ($("CustId") && null != _page.CustId) $("CustId").value = _page.CustId;
}

function setVisibleBoxLeague() {
    if (noClickSelect) {
        hiddenBoxLeague();
    }
    noClickSelect = true;
}

function hiddenBoxLeague() {
    $('dSelLeague').value = _page.selectLeague;
    $("BoxLeagueWL").style.display = 'none';
}

function InitExcelFunction() {
    var exportExcelImg = $("exporttoexcel");
    var mainForm = document.forms[0];
    if (exportExcelImg && mainForm) {

        exportExcelImg.onclick = function() {
            document.forms[0].method = 'post';
        }

        exportExcelImg.onblur = function() {
            $("exporttoexcel").focused = false;
        }
        exportExcelImg.onfocus = function() {
            $("exporttoexcel").focused = true;
        }

        mainForm.onkeypress = function(evt) {
            var keyPressEvent = evt ? evt : window.event;
            if (keyPressEvent.keyCode == 13) {
                var src = keyPressEvent.srcElement || keyPressEvent.target;
                if (!src || (src.tagName.toLowerCase() != "textarea")) {
                    if (!$("exporttoexcel").focused) {
                        if (keyPressEvent.stopPropagation) keyPressEvent.stopPropagation();
                        return false;
                    }
                }
            }
            return true;
        }
    }
}