var REPORT = {};

if (typeof jQuery != 'undefined') {
    REPORT = function ($) {
        return {
            
        };
    } (jQuery);

    $(document).ready(function () {
        $('.input-date').datepicker();

    });
}




// Require Core.js
_focusElementId = -1; // Do not use default focus
age = new AGE();

function boldTable(isBold) {
    var bold = isBold ? "1" : "0";

    var table1 = document.getElementById("tbl-rpt");

    if (isBold) {
        if (table1) AddClassName(table1, "boldrow");
    }
    else {
        if (table1) RemoveClassName(table1, "boldrow");
    }
    setCookie("rpt_bold", bold, 30);
}

function viewBetListByMatch(matchId, bettype, viewType, islive) {
    var url = age.GetBaseUrl() + "_BetList/BetList.aspx";

    url = SetParameterValue("matchid", matchId, url);
    url = SetParameterValue("bettype", bettype, url);
    url = SetParameterValue("type", viewType, url);
    url = SetParameterValue("islive", islive, url);

    window.location.href = url;
}

function doViewBetList(leagueId, matchId, bettype, viewType, race, islive) {
    var url = age.GetBaseUrl() + "_BetList/BetList.aspx?";
    if (parseInt(race) > 0) {
        url = SetParameterValue("leagueid", leagueId, url);
        url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
        url = SetParameterValue("tdate", document.getElementById("tdate").value, url);
        url = SetParameterValue("race", race, url);
    }
    else {
        url = SetParameterValue("matchid", matchId, url);
        if (bettype == 10) { // Outright bet type
            url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
            url = SetParameterValue("tdate", document.getElementById("tdate").value, url);
        }
    }
    url = SetParameterValue("bettype", bettype, url);
    url = SetParameterValue("type", viewType, url);
    url = SetParameterValue("islive", islive, url);

    window.location.href = url;
}

function doViewMPBetList(dateFrom, dateTo, betType, viewType) {
    var url = age.GetBaseUrl() + "_BetList/BetList.aspx";
    url = SetParameterValue("fdate", dateFrom, url);
    url = SetParameterValue("tdate", dateTo, url);
    url = SetParameterValue("bettype", betType, url);
    url = SetParameterValue("type", viewType, url);

    window.location.href = url;
}

function viewHRBetList(custid, username)//ViewBetList HorseRacing
{
    var url = age.GetBaseUrl() + "_BetList/BetList.aspx";
    url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
    url = SetParameterValue("tdate", document.getElementById("tdate").value, url);
    url = SetParameterValue("custid", custid, url);
    url = SetParameterValue("username", username, url);
    url = SetParameterValue("type", "WLByDate", url);
    url = SetParameterValue("FilterPostback", "1", url);
    url = SetParameterValue("IsHistoryReport", "0", url);
    url = SetParameterValue("chk_showrb", "on", url);
    url = SetParameterValue("chk_showsb", "off", url);
    window.location.href = url;
}

function viewSBBetList(custid, username, type)//ViewBetList for Sport Book
{
    var url = age.GetBaseUrl() + "_Reports/BetList/OutstandingDetail.aspx";
    url = SetParameterValue("custid", custid, url);
    url = SetParameterValue("username", username, url);
    url = SetParameterValue("type", type, url);
    url = SetParameterValue("chkgetall", "on", url);
    url = SetParameterValue("chk_showsb", "on", url);
    url = SetParameterValue("chk_showcasino", "off", url);
    url = SetParameterValue("chk_showrb", "on", url);
    url = SetParameterValue("chk_showng", "on", url);
    url = SetParameterValue("chk_showbi", "off", url);
    url = SetParameterValue("chk_showp2p", "off", url);
    url = SetParameterValue("chk_showlivecasino", "on", url);
    url = SetParameterValue("chk_showvs", "on", url);
    window.location.href = url;
}

function viewCasinoOutstandingBetList(custid, username, type)//ViewBetList for Casino
{
    var url = age.GetBaseUrl() + "_Reports/BetList/OutstandingDetail.aspx"
    url = SetParameterValue("custid", custid, url);
    url = SetParameterValue("username", username, url);
    url = SetParameterValue("type", type, url);
    url = SetParameterValue("chkgetall", "on", url);
    url = SetParameterValue("chk_showsb", "off", url);
    url = SetParameterValue("chk_showcasino", "on", url);
    url = SetParameterValue("chk_showrb", "off", url);
    url = SetParameterValue("chk_showng", "off", url);
    url = SetParameterValue("chk_showbi", "off", url);
    url = SetParameterValue("chk_showvs", "off", url);

    window.location.href = url;
}

function viewBingoOutstandingBetList(custid, username, type)//ViewBetList for Bingo
{
    var url = age.GetBaseUrl() + "_Reports/BetList/OutstandingDetail.aspx"
    url = SetParameterValue("custid", custid, url);
    url = SetParameterValue("username", username, url);
    url = SetParameterValue("type", type, url);
    url = SetParameterValue("chkgetall", "on", url);
    url = SetParameterValue("chk_showsb", "off", url);
    url = SetParameterValue("chk_showcasino", "off", url);
    url = SetParameterValue("chk_showrb", "off", url);
    url = SetParameterValue("chk_showng", "off", url);
    url = SetParameterValue("chk_showbi", "on", url);
    url = SetParameterValue("chk_showvs", "off", url);

    window.location.href = url;
}

function viewCasinoBetList(custid, username) {
    var url = age.GetBaseUrl() + "BetList/ViewCasinoResult.aspx";

    url = SetParameterValue("ibccustid", custid, url);
    url = SetParameterValue("uname", username, url);
    url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
    url = SetParameterValue("tdate", document.getElementById("tdate").value, url);
    url = SetParameterValue("ACustID", document.getElementById("custid").value, url);

    window.location.href = url;
}

function viewBingoBetList(custid, username) {
    var url = age.GetBaseUrl() + "_BetList/BingoResult.aspx";

    url = SetParameterValue("ibccustid", custid, url);
    url = SetParameterValue("uname", username, url);
    url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
    url = SetParameterValue("tdate", document.getElementById("tdate").value, url);
    url = SetParameterValue("ACustID", document.getElementById("custid").value, url);

    window.location.href = url;
}
/* End Bet list*/

/* Downline */
function doViewOutstandingDownline(custid, roleid)//ViewDownline
{
    var url = "Outstanding.aspx";
    url = SetParameterValue("custid", custid, url);
    url = SetParameterValue("roleid", roleid, url);
    url = SetParameterValue("sortingcolumn", _page.SortingColumn, url);
    url = SetParameterValue("sortingup", _page.SortingUp, url);

    window.location.href = url;
}

function viewCommissionDownLine(custid, roleid) {
    var url = "CommissionByBettype.aspx";

    url = SetParameterValue("custid", custid, url);
    url = SetParameterValue("roleid", roleid, url);
    url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
    url = SetParameterValue("tdate", document.getElementById("tdate").value, url);
    url = SetParameterValue("sortingcolumn", _page.SortingColumn, url);
    url = SetParameterValue("sortingup", _page.SortingUp, url);

    window.location.href = url;
}

function viewFinanceCommissionDownline(custid, roleid) {
    var url = "FinanceCommission.aspx";

    url = SetParameterValue("custid", custid, url);
    url = SetParameterValue("roleid", roleid, url);
    url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
    url = SetParameterValue("tdate", document.getElementById("tdate").value, url);
    url = SetParameterValue("sortingcolumn", _page.SortingColumn, url);
    url = SetParameterValue("sortingup", _page.SortingUp, url);

    window.location.href = url;
}

function viewHRCommissionDownline(custid, roleid) {
    var url = "HRCommission.aspx";
    url = SetParameterValue("custid", custid, url);
    url = SetParameterValue("roleid", roleid, url);
    url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
    url = SetParameterValue("tdate", document.getElementById("tdate").value, url);
    url = SetParameterValue("sortingcolumn", _page.SortingColumn, url);
    url = SetParameterValue("sortingup", _page.SortingUp, url);

    window.location.href = url;
}

function doViewCSWinlossDownline(custid, currentRole) {
    var url = "";
    if (currentRole == "super") url = "CSWinlossMaster.aspx";
    if (currentRole == "master") url = "CSWinlossAgent.aspx";
    if (url != "") {
        url = SetParameterValue("custid", custid, url);
        url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
        url = SetParameterValue("tdate", document.getElementById("tdate").value, url);
        url = SetParameterValue("sortingcolumn", _page.SortingColumn, url);
        url = SetParameterValue("sortingup", _page.SortingUp, url);

        window.location.href = url;
    }
}
/* End Downline */

/* Casino detail */
function ViewSuperCasinoDetail(bettype) {
    var url = age.GetBaseUrl() + "_Reports/CasinoWinloss/CSWinlossDetailSuper.aspx";
    url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
    url = SetParameterValue("tdate", document.getElementById("tdate").value, url);
    url = SetParameterValue("btype", bettype, url);
    url = SetParameterValue("sortingcolumn", _page.SortingColumn, url);
    url = SetParameterValue("sortingup", _page.SortingUp, url);

    location.href = url;
}

function ViewMasterCasinoDetail(bettype) {
    var url = age.GetBaseUrl() + "_Reports/CasinoWinloss/CSWinlossDetailMaster.aspx";
    url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
    url = SetParameterValue("tdate", document.getElementById("tdate").value, url);
    url = SetParameterValue("btype", bettype, url);
    url = SetParameterValue("sortingcolumn", _page.SortingColumn, url);
    url = SetParameterValue("sortingup", _page.SortingUp, url);

    location.href = url;
}

function ViewAgentCasinoDetail(bettype) {
    if (_page.role == 4) {
        ViewSuperCasinoDetail(bettype);
        return;
    }
    else if (_page.role == 3) {
        ViewMasterCasinoDetail(bettype);
        return;
    }

    var url = age.GetBaseUrl() + "_Reports/CasinoWinloss/CSWinlossDetailAgent.aspx";
    url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
    url = SetParameterValue("tdate", document.getElementById("tdate").value, url);
    url = SetParameterValue("btype", bettype, url);
    url = SetParameterValue("sortingcolumn", _page.SortingColumn, url);
    url = SetParameterValue("sortingup", _page.SortingUp, url);

    location.href = url;
}

function ViewCasinoDetail(bettype) {
    if (_page.role == 4) {
        ViewSuperCasinoDetail(bettype);
        return;
    }
    else if (_page.role == 3) {
        ViewMasterCasinoDetail(bettype);
        return;
    }
    else {
        ViewAgentCasinoDetail(bettype);
    }
}
/* End Casino detail */

/* Bingo detail */
function ViewSuperBingoDetail() {
    var url = age.GetBaseUrl() + "_Reports/BingoWinloss/BingoWinlossDetailSuper.aspx";
    url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
    url = SetParameterValue("tdate", document.getElementById("tdate").value, url);
    url = SetParameterValue("sortingcolumn", _page.SortingColumn, url);
    url = SetParameterValue("sortingup", _page.SortingUp, url);

    location.href = url;
}

function ViewMasterBingoDetail(bettype) {
    var url = age.GetBaseUrl() + "_Reports/BingoWinloss/BingoWinlossDetailMaster.aspx";
    url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
    url = SetParameterValue("tdate", document.getElementById("tdate").value, url);
    url = SetParameterValue("sortingcolumn", _page.SortingColumn, url);
    url = SetParameterValue("sortingup", _page.SortingUp, url);

    location.href = url;
}

function ViewAgentBingoDetail(bettype) {
    var url = age.GetBaseUrl() + "_Reports/BingoWinloss/BingoWinlossDetailAgent.aspx";
    url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
    url = SetParameterValue("tdate", document.getElementById("tdate").value, url);
    url = SetParameterValue("sortingcolumn", _page.SortingColumn, url);
    url = SetParameterValue("sortingup", _page.SortingUp, url);

    location.href = url;
}

function ViewBingoDetail(bettype) {
    if (_page.role == 4) {
        ViewSuperBingoDetail(bettype);
    }
    else if (_page.role == 3) {
        ViewMasterBingoDetail(bettype);
    }
    else {
        ViewAgentBingoDetail(bettype);
    }
}

function doViewBingoWinlossDownline(custid, currentRole) {
    var url = "";
    if (currentRole == "super") url = "BingoWinlossMaster.aspx";
    if (currentRole == "master") url = "BingoWinlossAgent.aspx";
    if (url != "") {
        url = SetParameterValue("custid", custid, url);
        url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
        url = SetParameterValue("tdate", document.getElementById("tdate").value, url);
        url = SetParameterValue("sortingcolumn", _page.SortingColumn, url);
        url = SetParameterValue("sortingup", _page.SortingUp, url);

        window.location.href = url;
    }
}
/* End Bingo detail */

/* Result page */
function Result_ChangeSport() {
    document.getElementById("ddlLeague").selectedIndex = 0;
    document.getElementById("changeSportType").value = 1;
    document.getElementById("frmmain").method = "get";
    document.getElementById("frmmain").submit();
}

function Result_ChangeLeague() {
    document.getElementById("changeSportType").value = 1;
    document.getElementById("frmmain").method = "get";
    document.getElementById("frmmain").submit();
}

function Result_ToggleResultType() {
    var url = "Result.aspx";
    var isOutright = document.getElementById("is_outright").value == "0" ? "1" : "0";
    url = SetParameterValue("is_outright", isOutright, url);

    location.href = url;
}

function Result_SearchByDate() {
    var url = "Result.aspx";
    var tdate = document.getElementById("tdate") != null ? document.getElementById("tdate").value : document.getElementById("fdate").value;
    url = SetParameterValue("ddlSport", "0", url);
    url = SetParameterValue("ddlLeague", "0", url);
    url = SetParameterValue("is_outright", document.getElementById("is_outright").value, url);
    url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
    url = SetParameterValue("tdate", tdate, url);
    url = SetParameterValue("sortingcolumn", _page.SortingColumn, url);
    url = SetParameterValue("sortingup", _page.SortingUp, url);

    location.href = url;
}

function Result_SearchOneDay(value) {
    var url = "Result.aspx?ddlSport=0&ddlLeague=0";
    url = SetParameterValue("ddlSport", "0", url);
    url = SetParameterValue("ddlLeague", "0", url);
    url = SetParameterValue("is_outright", document.getElementById("is_outright").value, url);
    url = SetParameterValue("fdate", value, url)
    url = SetParameterValue("tdate", value, url);
    url = SetParameterValue("sortingcolumn", _page.SortingColumn, url);
    url = SetParameterValue("sortingup", _page.SortingUp, url);

    location.href = url;
}

function Result_ViewSCRInfo(evendate, raceno, leagueid) {
    var url = age.GetBaseUrl() + "Popup/SCRInfo.aspx";
    url = SetParameterValue("lid", leagueid, url);
    url = SetParameterValue("race", raceno, url);
    url = SetParameterValue("date", evendate, url);

    var popH = 250, popW = 400;
    ageWnd.open(url, "", 50, 200, popW, popH);
}
/* End Result page */

function initReport() {
    InitExcelFunction();

    RestoreFormState();

    InitSortingHeader();
}

function InitExcelFunction() {
    var exportExcelImg = document.getElementById("exporttoexcel");
    var mainForm = document.forms[0];
    if (exportExcelImg && mainForm) {
        function exportExcelImg_OnClick(ev) {
            var url = location.href;
            url = SetParameterValue('exporttoexcel.x', 4, url);
            url = SetParameterValue('exporttoexcel.y', 8, url);
            location.href = url;

            if (IE) {
                window.event.cancelBubble = true;
                window.event.returnValue = false;
            } else {
                ev.preventDefault();
                ev.stopPropagation();
            }
        }

        age.addEvent(exportExcelImg, 'click', exportExcelImg_OnClick);

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

function RestoreFormState() {
    if (document.getElementById("dSubmit")) document.getElementById("dSubmit").disabled = false;
    if (document.getElementById("dToday")) document.getElementById("dToday").disabled = false
    if (document.getElementById("dYesterday")) document.getElementById("dYesterday").disabled = false;

    if (document.getElementById("CustId") && null != _page.CustId) document.getElementById("CustId").value = _page.CustId;

    if (document.getElementById("IsSwitch")) document.getElementById("IsSwitch").value = 0;
    RestoreObjectState("IsHistoryReport", "0");
    RestoreObjectState("OldView", "0");
}

function RestoreObjectState(name, defaultValue) {
    var obj = document.getElementById(name);
    if (obj != null) {
        var data = GetParameterValue(name, window.location.href);
        data = data != null ? data : defaultValue;
        obj.value = data;
    }
}

/*
* Created 20100706@Don - Js functions is dedicated to reports (Win Loss and Win Loss Detail)
*
*/

function CheckAll() {
    var arrayOptionItem = _page.FilterCollection;
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
    var arrayOptionItem = _page.FilterCollection;
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

/* Search by date win loss*/
function MatchResultEntered_GetSubmitPageUrl() {
    var roleId = document.getElementById("roleid").value;

    var url = age.GetBaseUrl() + "_Reports/Winloss/WinlossAgent.aspx";
    if (roleId == 4) url = age.GetBaseUrl() + "_Reports/Winloss/WinlossSuper.aspx";
    else if (roleId == 3) url = age.GetBaseUrl() + "_Reports/Winloss/WinlossMaster.aspx";
    return url;
}

function MatchResultEntered_ViewHistory() {
    var url = MatchResultEntered_GetSubmitPageUrl();
    url = SetParameterValue("custid", document.getElementById("custid").value, url);
    url = SetParameterValue("IsHistoryReport", 1, url)
    url = SetParameterValue("IsSwitch", 1, url); ;
    window.location.href = url;
}

function MatchResultEntered_SearchByDate() {
    var url = MatchResultEntered_GetSubmitPageUrl();
    url = SetParameterValue("custid", document.getElementById("custid").value, url);
    url = SetParameterValue("fdate", document.getElementById("fdate").value, url);
    url = SetParameterValue("tdate", document.getElementById("tdate").value, url);

    window.location.href = url;
}

function MatchResultEntered_SearchOneDay(value) {
    var url = MatchResultEntered_GetSubmitPageUrl();
    url = SetParameterValue("custid", document.getElementById("custid").value, url);
    url = SetParameterValue("fdate", value, url);
    url = SetParameterValue("tdate", value, url);
    url = SetParameterValue("IsHistoryReport", "0", url)
    url = SetParameterValue("IsSwitch", "0", url);

    window.location.href = url;
}

function Winloss_SearchByDate(fdate, tdate, targetCustId, actionPage) {
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

function Winloss_SearchOneDay(objButton) {
    Winloss_SearchByDate(objButton.value, objButton.value, null, null);
}

function ViewDownlineWLReport(fdate, tdate, targetCustId, actionPage, username) {
    // Begin - Turn back to match bet list hasn't redesigned
    // Pls remove this block from the function when bet list has redesigned.
//    if (document.getElementById('WorkOnOldBetList') != null) {
//        if (document.getElementById('WorkOnOldBetList').value == '1') {
//            var url = actionPage;
//            url = SetParameterValue("custid", targetCustId, url);
//            url = SetParameterValue("fdate", fdate, url);
//            url = SetParameterValue("tdate", tdate, url);
//            url = SetParameterValue("type", document.getElementById('Type').value, url);
//            url = SetParameterValue("IsHistoryReport", document.getElementById('IsHistoryReport').value, url);
//            url = SetParameterValue("username", username, url);
//            url = SetParameterValue("sortingcolumn", _page.SortingColumn, url);
//            url = SetParameterValue("sortingup", _page.SortingUp, url);

//            var paramName, item;
//            var arrayOptionItem = _page.FilterCollection;
//            var arrayStatusItem = _page.FilterStatusCollection;
//            if (arrayOptionItem != null) {
//                for (var i = 1; i < arrayOptionItem.length; i++) {
//                    paramName = arrayOptionItem[i].substring(4);
//                    item = document.getElementById(arrayOptionItem[i]);
//                    if (item != undefined && item != null) {
//                        url = SetParameterValue(paramName, (item.checked ? "1" : "0"), url);
//                    }
//                    else {
//                        url = SetParameterValue(paramName, (arrayStatusItem[i] == "on" ? "1" : "0"), url);
//                    }
//                }
//            }

//            window.location.href = url;

//            return;
//        }
//    }
    // End - Turn back to match bet list hasn't redesigned

    if (ProcessDownlineForWinlossOnly(fdate, tdate, targetCustId, actionPage, username, document.getElementById('Type').value ) == true) {
        return;
    }

    if (targetCustId != null && actionPage != null) {
        document.getElementById("CustId").value = targetCustId;

        document.getElementById("frmmain").action = actionPage;
    }

    if (username != null && username != "") {
        document.getElementById("UserName").value = username;
    }

    document.getElementById("frmmain").submit();
}

function ViewHistory() {
    document.getElementById("IsHistoryReport").value = (document.getElementById("IsHistoryReport").value == "1" ? "0" : "1");
    document.getElementById("IsSwitch").value = "1";

    document.getElementById("frmmain").submit();
}

function ChangeView() {
    document.getElementById("OldView").value = (document.getElementById("OldView").value == "1" ? "0" : "1");

    document.getElementById("frmmain").submit();
}

function ElementExisted(name) {
    name = name.toLowerCase().trim();
    for (var i = 0; i < document.forms.length; i++) {
        var form = document.forms[i];
        for (var j = 0; j < form.elements.length; j++) {
            var element = form.elements[j];
            if (element.name != null && element.name.toLowerCase().trim() == name) return true;
        }
    }
    return false;
}

// Sorting function
// parse the current url and save each parameters as a hidden field
function SaveSortingParametersAsHiddenField() {
    var searchString = document.location.search;
    // strip off the leading '?'
    searchString = searchString.substring(1);
    var pairs = searchString.split("&");

    for (i = 0; i < pairs.length; i++) {
        var nvPair = pairs[i].split("=");
        var name = nvPair[0];
        var value = nvPair[1];
        if (ElementExisted(name) == false) {
            createHiddenInput(document.getElementById("frmmain"), name, value);
        }
    }
}

function createSortingIcon() {
    var element = document.createElement("span");
    var className = _page.SortingUp == true ? "ascSorting" : "descSorting";
    AddClassName(element, className);
    element.innerHTML = "&nbsp;&nbsp;&nbsp;";
    return element;
}

function createHiddenInput(parent, name, value) {
    var element = document.createElement("input");
    element.setAttribute("type", "hidden");
    element.setAttribute("name", name);
    element.setAttribute("id", name);
    element.setAttribute("value", value);
    parent.appendChild(element);
}

function SortData(obj) {
    var sortingColumn = obj.getAttribute("columnname");
    if (sortingColumn.toLowerCase() == _page.SortingColumn.toLowerCase()) {
        _page.SortingUp = _page.SortingUp == true ? false : true;
    }
    else _page.SortingUp = "true";

    SaveSortingParametersAsHiddenField();
    document.getElementById("sortingcolumn").value = sortingColumn;
    document.getElementById("sortingup").value = _page.SortingUp;

    document.getElementById("frmmain").submit();
}

function InitSortingHeader() {
    // Does not sort if there is paging control and page count > 1
    var sortingEnabled = true;
    var currentUrl = window.location.href;
    var reportTable = document.getElementById("tbl-rpt");

    if (!reportTable) return;

    if (_page.ReportRowCount != null && _page.ReportRowCount <= 1) sortingEnabled = false;
    if (_page.SortingEnabled != null && _page.SortingEnabled == false) sortingEnabled = false;

    if (sortingEnabled) {
        if (_page.SortingColumn == '') _page.SortingColumn = 'username';

        createHiddenInput(document.getElementById("frmmain"), "sortingcolumn", _page.SortingColumn);
        createHiddenInput(document.getElementById("frmmain"), "sortingup", _page.SortingUp);

        var reportTableBody = reportTable.childNodes[0];
        for (var i = 0, rowCount = reportTableBody.childNodes.length; i < rowCount && i < 3; i++) {
            var row = reportTableBody.childNodes[i];
            for (var j = 0, columnCount = row.childNodes.length; j < columnCount; j++) {
                var col = row.childNodes[j];
                var columnName = col.getAttribute("columnname");
                if (columnName != null && columnName != "") {
                    if (columnName.toLowerCase() == _page.SortingColumn.toLowerCase()) {
                        var sortingIcon = createSortingIcon();
                        col.appendChild(sortingIcon);
                    }
                    col.onclick = function() { SortData(this); };
                    AddClassName(col, "hand");
                }
            }
        }
    }
    try {
        reportTable.style.display = "table";
    } catch (e) {
        reportTable.style.display = "block";
    }
}

// End of sorting function

/*
ProcessDownlineForWinlossOnly
-   For winloss report only, user can view downline and when search panel doesn't show
so this function control URL (page and params)to make a request manually.
*/
function ProcessDownlineForWinlossOnly(fdate, tdate, targetCustId, actionPage, username, type) {
    var item;
    var flag = false;
    var filterParams = "";
    var arrayOptionItem = _page.FilterCollection;
    var arrayStatusItem = _page.FilterStatusCollection;
    if (arrayOptionItem != null) {
        for (var i = 1; i < arrayOptionItem.length; i++) {
            item = document.getElementById(arrayOptionItem[i]);
            if (item == undefined || item == null) {
                if (arrayStatusItem[i] == "on") {
                    filterParams += "&" + arrayOptionItem[i] + "=" + arrayStatusItem[i];
                    flag = true;
                }
            }
        }
    }

    if (flag == true) {
        var url = actionPage;
        url = SetParameterValue("custid", targetCustId, url);
        url = SetParameterValue("fdate", fdate, url);
        url = SetParameterValue("tdate", tdate, url);
        url = SetParameterValue("IsHistoryReport", document.getElementById('IsHistoryReport').value, url);
        url = SetParameterValue("FilterPostback", "postback", url);
        url = SetParameterValue("sortingcolumn", _page.SortingColumn, url);
        url = SetParameterValue("sortingup", _page.SortingUp, url);
        url = SetParameterValue("username",username, url);
        url = SetParameterValue("type", type, url);
        url += filterParams;
        window.location.href = url;
    }

    return flag;
}

/* End search by date win loss*/

// -- End - Js functions is dedicated to reports (Win Loss and Win Loss Detail) --

RegisterStartUp(initReport);