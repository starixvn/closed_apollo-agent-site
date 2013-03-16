var noClickSelect = true;

function DisableButton() {
    var dSubmit = document.getElementById('dSubmit');
    var dSelMatch = document.getElementById('dSelMatch');

    dSubmit.disabled = true;
    if (!HasClassName(dSubmit, 'hidden')) dSubmit.className = 'btn2';

    if (dSelMatch) {
        dSelMatch.disabled = true;
        if (!HasClassName(dSelMatch, 'hidden')) dSelMatch.className = 'btn2';
    }

    var loading = document.getElementById("loading");
    if (loading) loading.className = "loading";
}

function EnableButton() {
    var dSubmit = document.getElementById('dSubmit');
    var dSelMatch = document.getElementById('dSelMatch');

    dSubmit.disabled = false;
    if (!HasClassName(dSubmit, 'hidden')) dSubmit.className = 'btn';

    if (dSelMatch) {
        dSelMatch.disabled = false;
        if (!HasClassName(dSelMatch, 'hidden')) dSelMatch.className = 'btn';
    }

    var loading = document.getElementById("loading");
    if (loading) loading.className = "";
}

function SelectMatch() {    
    DisableButton();
    if (document.getElementById('BoxMatchWL').style.display == '') {
        hiddenBoxMatch();
        EnableButton();
        return;
    }
    noClickSelect = false;
    var LeagueID = document.getElementById('ddlLeague') ? document.getElementById('ddlLeague').value : '0';
    var NewFilterMatch = LeagueID + document.getElementById('isChanged').value;
    if (_page.FilterMatch != null && _page.FilterMatch != NewFilterMatch) {
        _page.FilterMatch = NewFilterMatch;
        http = jx.request(
        '../_Components/MatchByLeague/MatchByLeague.aspx', onComplete, 'POST'
           , 'LeagueID=' + LeagueID
        );
    }
    else {
        EnableButton();
        document.getElementById('dSelMatch').value = _page.hideMatch;
        document.getElementById('BoxMatchWL').style.display = '';
    }

    function onComplete(rs) {
        if (typeof rs == 'object') {
            document.getElementById('dSelMatch').value = _page.hideMatch;
            document.getElementById('tblMsgMatch').style.display = "none";
            document.getElementById('BoxMatchWL').style.display = '';
            document.getElementById('ContentMatchWL').innerHTML = rs.responseText;
            setMatchIds();
            GetMatchSportFull();
            EnableButton();
        }
    }
}

function isAllMatch(curMatch) {
    if (document.getElementById(curMatch).checked) {
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

    setMatchIds();
}

function setMatchIds() {
    var MatchIds = '';
    var ids = document.getElementsByTagName("input");
    for (var i = 0; i < ids.length; i++) {
        if (ids[i].type == "checkbox" && ids[i].id.substring(0, 3) == "chL") {
            if (ids[i].checked && ids[i].id != 'chLAll') {
                MatchIds += (MatchIds == '' ? ids[i].id.substring(3, ids[i].id.length) : ',' + ids[i].id.substring(3, ids[i].id.length));
            }
        }
    }
    document.getElementById('MatchIds').value = (MatchIds==''?'-1':MatchIds);
}

function CheckSelAllMatch() {
    var bolMatch = document.getElementById('chLAll').checked;
    var ids = document.getElementsByTagName("input");
    for (var i = 0; i < ids.length; i++) {
        if (ids[i].type == "checkbox" && ids[i].id.substring(0, 3) == "chL") {
            ids[i].checked = bolMatch;
        }
    }
    setMatchIds();
}

function GetMatchSportFull() {
    var ids = document.getElementsByTagName("input");
    document.getElementById('MatchFull').value = '';
    for (var i = 0; i < ids.length; i++) {
        if (ids[i].type == "checkbox" && ids[i].id.substring(0, 3) == "chL") {
            if (ids[i].checked && (ids[i].id != 'chL0') && (ids[i].id != 'chLAll')) {
                document.getElementById('MatchFull').value += ids[i].id.substring(3, ids[i].id.length) + ',';
            }
        }
    }
    if (document.getElementById('MatchFull').value.substring(document.getElementById('MatchFull').value.length - 1, document.getElementById('MatchFull').value.length) == ',')
        document.getElementById('MatchFull').value = document.getElementById('MatchFull').value.substring(0, document.getElementById('MatchFull').value.length-1);
}

function setVisibleBoxMatch() {
    if (noClickSelect) {
        hiddenBoxMatch();
    }
    noClickSelect = true;
}