/* File Created: April 5, 2013 */
var isCalling = false;
var lnkRefresh = "lnkRefresh";
var lnkRefreshClick = "lnkRefreshClick";

RegisterStartUp("initScoreMap()");

function initScoreMap() {
    try {
        eCol();

        if (document.getElementById('page_main') != null && document.getElementById('page_main') != undefined) {
            document.getElementById("page_main").onclick = function () {
                setVisibleBoxMatch();
            }
        }

        if (document.getElementById('RefreshLeagues') != null && document.getElementById('RefreshLeagues') != undefined) {
            document.getElementById("RefreshLeagues").onclick = function () {
                RefreshLeague();
            }
        }

        if (document.getElementById('dSelMatch') != null && document.getElementById('dSelMatch') != undefined) {
            document.getElementById("dSelMatch").onclick = function () {
                SelectMatch();
            }
        }

        if (document.getElementById('closeBoxMatch') != null && document.getElementById('closeBoxMatch') != undefined) {
            document.getElementById("closeBoxMatch").onclick = function () {
                CloseBoxMatch();
            }
        }

        if (document.getElementById('dSubmit') != null && document.getElementById('dSubmit') != undefined) {
            document.getElementById("dSubmit").onclick = function () {
                LoadScoreMap();
            }
        }

        if (document.getElementById('dSubmitInMatch') != null && document.getElementById('dSubmitInMatch') != undefined) {
            document.getElementById("dSubmitInMatch").onclick = function () {
                hiddenBoxMatch();
            }
        }

        if (document.getElementById('chkAllL') != null && document.getElementById('chkAllL') != undefined) {
            document.getElementById("chkAllL").onclick = function () {
                CheckAllBetType('L');
            }
        }

        if (document.getElementById('chkAllD') != null && document.getElementById('chkAllD') != undefined) {
            document.getElementById("chkAllD").onclick = function () {
                CheckAllBetType('D');
            }
        }
    }
    catch (err) { }
}

function hideBoxMatch() {
    document.getElementById("BoxMatchWL").style.display = "none";
    document.getElementById('tblMsgMatch').style.display = "none";
    document.getElementById('dSelMatch').value = _page.selectMatch;
}

function ShowHideLiveDead(result) {
    document.getElementById("tblLiveDead").style.display = result;
    document.getElementById('mainContent').style.display = result;
    if (result == "none")
        document.getElementById('mainContent').innerHTML = "";
}

function hiddenBoxMatch() {
    if (document.getElementById('MatchIds').value != '-1') {
        hideBoxMatch();
        ShowHideLiveDead("");
    }
    else {
        document.getElementById('msgMatchErr').innerHTML = _page.plsChooseMatch;
        document.getElementById('tblMsgMatch').style.display = "";
    }
}

function CloseBoxMatch() {
    hideBoxMatch();
    if (document.getElementById('MatchIds').value != '-1') {
        ShowHideLiveDead("");
    }
    else {
        ShowHideLiveDead("none");
    }
    document.getElementById('dSelMatch').value = _page.selectMatch;
}

function CommonLoadData(MatchId) {
    hiddenBoxMatch();
    LoadingRefresh(MatchId, lnkRefreshClick);
    setBetTypes('L');
    setBetTypes('D');
}

function DisableBtnSubmit() {
    var dSubmit = document.getElementById('dSubmit');

    dSubmit.disabled = true;
    if (!HasClassName(dSubmit, 'hidden')) dSubmit.className = 'btn2';

    var loading = document.getElementById("loadingSubmit");
    if (loading) loading.className = "loading";
}

function EnableBtnSubmit() {
    var dSubmit = document.getElementById('dSubmit');

    dSubmit.disabled = false;
    if (!HasClassName(dSubmit, 'hidden')) dSubmit.className = 'btn';

    var loading = document.getElementById("loadingSubmit");
    if (loading) loading.className = "";
}

function LoadScoreMap() {
    if (isCalling && typeof (http) != 'undefined') {
        http.abort();
        isCalling = false;
    }
    isCalling = true;

    if (document.getElementById('MatchIds').value != '-1') {
        CommonLoadData(document.getElementById('MatchIds').value);
        if (document.getElementById('strValueL').value == '-1' && document.getElementById('strValueD').value == '-1') {
            document.getElementById('mainContent').innerHTML = "";
            document.getElementById('spmsgerr').innerHTML = _page.plsChooseBetType;
            document.getElementById('diverrmsg').style.display = "";
            LoadingRefresh(document.getElementById('MatchIds').value, lnkRefresh);
        }
        else {
            document.getElementById('diverrmsg').style.display = "none";
            DisableBtnSubmit();
            var paras = "isRefresh=false" + "&leagueid=0&matchIds=" + document.getElementById('MatchIds').value + "&bettypes_live=" + document.getElementById('strValueL').value + "&bettypes_dead=" + document.getElementById('strValueD').value;
            http = jx.request('../_Forecast/ScoreMapProcess.aspx', onComplete, 'POST', paras);
        }
    }
    else {
        SelectMatch();
    }

    function onComplete(rs) {
        if (!isCalling) {
            return;
        }

        if (typeof rs == 'object') {
            document.getElementById('mainContent').innerHTML = rs.responseText;
            LoadingRefresh(document.getElementById('MatchIds').value, lnkRefresh);
            EnableBtnSubmit();
            eCol();
        }

        isCalling = false;
    }
}

function GroupScoreMap(GroupMatchId) {
    if (document.getElementById(GroupMatchId).style.display != "") {
        document.getElementById(GroupMatchId).style.display = "";
        document.getElementById('lnkGroup' + GroupMatchId).className = "lnkGroup";
    }
    else {
        document.getElementById(GroupMatchId).style.display = "none";
        document.getElementById('lnkGroup' + GroupMatchId).className = "lnkGroupClick";
    }
}

function RefreshScoreMap(MatchId) {
    if (isCalling && typeof (http) != 'undefined') {
        http.abort();
        isCalling = false;
    }
    isCalling = true;

    CommonLoadData(MatchId);
    var paras = "isRefresh=true" + "&leagueid=0&matchIds=" + MatchId + "&bettypes_live=" + document.getElementById('strValueL').value + "&bettypes_dead=" + document.getElementById('strValueD').value;
    http = jx.request(
    '../_Forecast/ScoreMapProcess.aspx', onComplete, 'POST'
        , paras
    );

    function onComplete(rs) {
        if (!isCalling) {
            return;
        }

        if (typeof rs == 'object') {
            document.getElementById('RefreshContent' + MatchId).innerHTML = rs.responseText;
            LoadingRefresh(MatchId, lnkRefresh);
            eCol();
        }

        isCalling = false;
    }
}

function LoadingRefresh(Matchs, cName) {
    var arrMatchs = Matchs.split(',');
    for (var i = 0; i < arrMatchs.length; i++) {
        if (document.getElementById('lnkRefresh' + arrMatchs[i]) != null && document.getElementById('lnkRefresh' + arrMatchs[i]) != undefined) {
            document.getElementById('lnkRefresh' + arrMatchs[i]).className = cName;
        }
    }
}

function ChangeLeague() {
    document.getElementById('MatchIds').value = "-1";
    hideBoxMatch();
    ShowHideLiveDead("none");
}

function RefreshLeague() {
    DisableButton();
    var LeagueID = document.getElementById('ddlLeague') ? document.getElementById('ddlLeague').value : '0';
    http = jx.request(
    '../_Forecast/RefreshLeague.aspx', onComplete, 'POST'
        , 'LeagueID=' + LeagueID
    );

    function onComplete(rs) {
        if (typeof rs == 'object') {
            document.getElementById('selLeague').innerHTML = rs.responseText;
            document.getElementById('isChanged').value = document.getElementById('isChanged').value != 0 ? 0 : 1;
            document.getElementById('MatchIds').value = "-1";
            hideBoxMatch();
            EnableButton();
        }
    }
}

/* START: For Box Bet Types */
function isAllBetType(Type, curBetType) {
    var strChkBetType = "bt" + Type;
    var strChkAllBetType = "chkAll" + Type;
    if (document.getElementById(curBetType).checked) {
        var isCheckAll = true;
        var ids = document.getElementsByTagName("input");
        for (var i = 0; i < ids.length; i++) {
            if (ids[i].type == "checkbox" && ids[i].id.substring(0, 3) == strChkBetType) {
                if (ids[i].id == strChkAllBetType) continue;
                if (!document.getElementById(ids[i].id).checked) {
                    isCheckAll = false;
                    break;
                }
            }
        }
        if (document.getElementById(strChkAllBetType) != null) document.getElementById(strChkAllBetType).checked = isCheckAll;
    }
    else {
        if (document.getElementById(strChkAllBetType) != null) document.getElementById(strChkAllBetType).checked = false;
    }
}

function CheckAllBetType(Type) {
    var strChkBetType = "bt" + Type;
    var bolAll = document.getElementById('chkAll' + Type).checked;
    var ids = document.getElementsByTagName("input");
    for (var i = 0; i < ids.length; i++) {
        if (ids[i].type == "checkbox" && ids[i].id.substring(0, 3) == strChkBetType) {
            ids[i].checked = bolAll;
        }
    }
}

function setBetTypes(Type) {
    var strChkBetType = "bt" + Type;
    var BetTypes = '';
    var ids = document.getElementsByTagName("input");
    for (var i = 0; i < ids.length; i++) {
        if (ids[i].type == "checkbox" && ids[i].id.substring(0, 3) == strChkBetType) {
            if (ids[i].checked && ids[i].id != 'chLAll') {
                BetTypes += (BetTypes == '' ? ids[i].id.substring(3, ids[i].id.length) : ',' + ids[i].id.substring(3, ids[i].id.length));
            }
        }
    }
    document.getElementById('strValue' + Type).value = (BetTypes == '' ? '-1' : BetTypes);
}
/* END: For Box Bet Types */

function hCol(tblMatchId, n) {
    n = n - 1;
    var tTab = document.getElementById(tblMatchId);
    for (var j_tem = 0; j_tem < 7; j_tem++) {
        if (j_tem == n)
            tTab.rows[1].cells[j_tem].style.backgroundColor = '#d0d3d3';
    }
}
function lCol(tblMatchId, n) {
    n = n - 1;
    var tTab = document.getElementById(tblMatchId);
    for (var j_tem = 0; j_tem < 7; j_tem++) {
        if (j_tem == n)
            tTab.rows[1].cells[j_tem].style.backgroundColor = '';
    }
}
function eCol() {
    if (document.getElementById('MatchIds').value != -1) {
        var arrMatchIds = document.getElementById('MatchIds').value.split(',');
        for (k = 0; k < arrMatchIds.length; k++) {
            var tblMatchId = 'tblSM' + arrMatchIds[k];
            var tTab = document.getElementById(tblMatchId);
            for (var i_tem = 2; i_tem < tTab.rows.length; i_tem++)
                for (var j_tem = 1; j_tem < tTab.rows[i_tem].cells.length; j_tem++) {
                    tTab.rows[i_tem].cells[j_tem].onmouseover = new Function("hCol('" + tblMatchId + "'," + j_tem + ")");
                    tTab.rows[i_tem].cells[j_tem].onmouseout = new Function("lCol('" + tblMatchId + "'," + j_tem + ")");
                }
        }
    }
}