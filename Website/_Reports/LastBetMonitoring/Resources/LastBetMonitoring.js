require(["/_Reports/LastBetMonitoring/Resources/smoothie.js"], function () { });

require(["/_Components/AGEWnd/AGEWnd.js"], function () { });

require(["/_GlobalResources/Js/shortcut.js"], function () { });

require(["/_Components/ErrorMsg/ErrorMsg.js"], function () { });

require(["/_BetList/Resources/BetList.js"], function () {
    RegisterStartUp(InitLastBetMonitering);
});

function isCanvasSupported() {
    var elem = document.createElement('canvas');
    return !!(elem.getContext && elem.getContext('2d'));
}

var timeoutId;
var request;
var isCalling = false;
var interval = 2000;
var SYSTEM_ERROR = 'Our server is too busy at the moment so this function is turned off temporarily. Please try again later and sorry for the inconvenience.';
var divChart = null;
var chartWidth = 0;
var betSeries = null;
var chart = null;

function setDisplayStyle(selectorText, value) {
    var theRules = new Array();
    if (document.styleSheets[4].cssRules) {
        theRules = document.styleSheets[4].cssRules;
    } else if (document.styleSheets[4].rules) {
        theRules = document.styleSheets[4].rules;
    }

    var c = theRules.length;
    for (var i = 0; i < c; i++) {
        if (theRules[i].selectorText == selectorText) {
            theRules[i].style.display = value;
        }
    }
}

function isAllSport(sport) {
    if (sport != 'chkSport0') {
        if ($(sport).checked) {
            isCheckAllSport();
        } else {
            $('chkSport0').checked = false;
        }
    }
}

function LoadDataByGroup(sportTypeGroup) {
    _page.sportTypeGroup = sportTypeGroup;
    setCookie(_page.TransIdCookieKey, 0, 1);
    $('betTable').innerHTML = '';
    GetLatestBets();
}

function LoadDataByBetType(betTypeGroup) {
    ClearCssBetType();
    $('BetTypeGroup' + betTypeGroup).className = 'c selectedBetType';
    _page.betTypeGroup = betTypeGroup;
    setCookie(_page.TransIdCookieKey, 0, 1);
    $('betTable').innerHTML = '';

    if (betTypeGroup == 1 || betTypeGroup == 3) {
        setDisplayStyle('.bl_btype', 'none');
    } else {
        setDisplayStyle('.bl_btype', 'inline');
    }

    GetLatestBets();
}

function LoadDataByRowCount(limitRowCount) {
    ClearCssRowCount();
    $('paging' + limitRowCount).className = 'c b';
    _page.limitRowCount = limitRowCount;
    setCookie(_page.TransIdCookieKey, 0, 1);
    $('betTable').innerHTML = '';
    GetLatestBets();
}

function ClearCssBetType() {
    $('BetTypeGroup0').className = 'c';
    $('BetTypeGroup1').className = 'c';
    $('BetTypeGroup3').className = 'c';
    $('BetTypeGroup19001').className = 'c';
}

function ClearCssRowCount() {
    $('paging10').className = 'c';
    $('paging20').className = 'c';
    $('paging30').className = 'c';
}

function showMP(transid) {
    var divEvent = $('divEvent_' + transid);
    if (divEvent.style.display == 'none') {
        if (divEvent.innerHTML == "") {
            ajax.Request(age.GetBaseUrl() + "_BetList/MixParlay/MixParlay.aspx", {
                method: 'get',
                parameters: 'transid=' + transid,
                onComplete: function (data) {
                    divEvent.innerHTML = data.responseText;
                    divEvent.style.display = '';
                }
            })
        } else {
            divEvent.style.display = '';
        }
    } else {
        divEvent.style.display = 'none';
    }
}

function GetLatestBets() {
    if (isCalling) {
        request.abort();
        isCalling = false;
    }

    $("loading").className = "loading";

    isCalling = true;

    var url = 'MonitorBetList.aspx?sportTypeGroup=' + _page.sportTypeGroup + '&betTypeGroup=' + _page.betTypeGroup + '&limitRowCount=' + _page.limitRowCount;
    var times = GetParameterValue('times', location.href);
    url = SetParameterValue('times', times, url);

    var testcustid = GetParameterValue('testcustid', location.href);
    url = SetParameterValue('testcustid', testcustid, url);

    request = jx.request(url, onComplete, 'POST', null);

    function onComplete(result) {
        if (!isCalling) {
            return;
        }

        if (typeof result == 'object' && typeof result.errCode == 'undefined') {
            ageMsg.Hide();

            var div = document.createElement("DIV");
            div.innerHTML = result.responseText;

            var tableTarget = GetFirstChild(div);
            var tableSource = GetFirstChild($('betTable'));

            if (tableSource == null) { // No information is available
                $('betTable').innerHTML = div.innerHTML;
            } else {
                try {
                    SetNewRows(tableSource, tableTarget);
                    interval = 2000;
                } catch (exception) {
                    EnterSystemError();
                }
            }
        } else {
            EnterSystemError();
        }

        $("loading").className = '';
        clearTimeout(timeoutId);
        timeoutId = setTimeout("GetLatestBets()", interval);
        isCalling = false;
    }
}

function EnterSystemError() {
    ageMsg.Show(SYSTEM_ERROR);
    $('betTable').innerHTML = '';
    interval = 600000; // Protection level, avoid through too many exceptions on server
}

function GetFirstChild(element) {
    return element.firstElementChild ? element.firstElementChild : element.firstChild;
}

function SetNewRows(tableSource, tableTarget) {
    var sourceCount = tableSource.rows.length;
    var targetCount = tableTarget.rows.length;

    // Delete / update old rows
    for (var i = 1; i < sourceCount; i++) {
        if (tableSource.rows[i].cells.length > 1 && FindRow(tableTarget, tableSource.rows[i]) == null) {
            tableSource.deleteRow(i);
            sourceCount--;
            i = i > 1 ? i - 1 : i;
        } else {
            var row = FindRow(tableTarget, tableSource.rows[i]);
            if (!row) continue;
            var cellIndex = tableSource.rows[i].cells.length - 1;
            tableSource.rows[i].cells[cellIndex].innerHTML = row.cells[cellIndex].innerHTML;
            tableSource.rows[i].style.backgroundColor = '#ffffff';
        }
    }

    var totalStake = 0;
    // Insert new rows (excluding header row, no information row)
    for (var i = 1; i < targetCount; i++) {
        if (tableTarget.rows[i].cells.length > 1 && FindRow(tableSource, tableTarget.rows[i]) == null) {
            var row = tableSource.insertRow(1);
            ImportCells(row, tableTarget.rows[i]);
            row.style.backgroundColor = '#ffff99';

            var stake = parseInt('0' + row.cells[5].firstChild.innerHTML.replace(/,/g, String.Empty), 10);
            totalStake += stake;
        }
    }

    if (betSeries != null) {
        var width = $('Content').clientWidth;
        if (width != chartWidth) {
            chartWidth = width;
            divChart.style.width = chartWidth;
            divChart.firstChild.width = chartWidth;
        }

        if (chart.options.maxValue < totalStake) {
            // chart.options.maxValue = totalStake;
        }

        betSeries.append(new Date().getTime(), totalStake);
    }

    // Re-order
    var sourceCount = tableSource.rows.length;
    for (var i = 1; i < sourceCount; i++) {
        if (tableSource.rows[i].cells.length > 1) {
            tableSource.rows[i].cells[0].innerHTML = i;
        } else if (tableSource.rows[i].cells.length == 1 && sourceCount > 2) {
            tableSource.deleteRow(i);
            sourceCount--;
            i = i > 1 ? i - 1 : i;
        }
    }
}

function FindRow(table, row) {
    var c = table.rows.length;
    for (var i = 1; i < c; i++) {
        if (table.rows[i].id == row.id) {
            return table.rows[i];
        }
    }

    return null;
}

function ImportCells(rowSource, rowTarget, position) {
    var c = rowTarget.cells.length;
    rowSource.id = rowTarget.id;
    for (var i = 0; i < c; i++) {
        var cell1 = rowSource.insertCell(i);
        cell1.innerHTML = rowTarget.cells[i].innerHTML;
        cell1.className = rowTarget.cells[i].className;
    }
}

function ViewBetSlip(transID, winLostDate, isRunning) { }

function OpenIPInfo(ip) {
    ageWnd.open(age.GetBaseUrl() + '_IPInfo/IpInfo.aspx?ip=' + ip, '', 300, 100, 400, 150);
}

function InitLastBetMonitering() {
    // Set up chart
    if (isCanvasSupported()) {
        chartWidth = $('Content').clientWidth;
        divChart = document.createElement("DIV");
        divChart.id = 'divChart';
        divChart.style.paddingBottom = '5px';
        //divChart.style.display = 'none';
        divChart.style.width = chartWidth + 'px';
        var canvas = document.createElement('canvas');
        canvas.width = chartWidth;
        canvas.height = 50;
        divChart.appendChild(canvas);

        $('Content').insertBefore(divChart, $('betTable'));

        chart = new SmoothieChart({
            grid: {
                strokeStyle: 'rgb(180, 180, 180)',
                fillStyle: 'rgb(255, 255, 255)',
                lineWidth: 1,
                millisPerLine: 200,
                verticalSections: 5
            },
            labels: {
                fillStyle: 'rgb(0, 0, 0)'
            },
            minValue: 0,
            maxValue: 1000,
            interpolation: 'line'
        });

        betSeries = new TimeSeries();
        chart.addTimeSeries(betSeries, { strokeStyle: 'rgb(0, 100, 0)', fillStyle: 'rgba(100, 255, 100, 0.4)', lineWidth: 1 });
        chart.streamTo(canvas, 2000);
    }

    // Set up short-cut
    shortcut.add("Shift+P", function () {
        if (timeoutId != 0) {
            clearTimeout(timeoutId);
            timeoutId = 0;
        } else {
            timeoutId = setTimeout(GetLatestBets, interval);
        }
    });

    GetLatestBets();
}