// Require Core.js
/*
* Created 20111122@Kaden
*
*/
age = new AGE();
//Init();

function Init() {
	var url = age.GetBaseUrl() + "_Charts/WinlossAnalysis/MonthlyAgent_Chart.aspx";
	$('ChartImage').src = url;
}
function CheckAll() {
    var arrayOptionItem = _page.FilterCollection;
    if ($(arrayOptionItem[0]) != null) {
        var allStatus = $(arrayOptionItem[0]).checked;
        for (var i = 1; i < arrayOptionItem.length; i++) {
            if ($(arrayOptionItem[i]) != null) {
                $(arrayOptionItem[i]).checked = allStatus;
            }
        }
    }
}

function IsCheck() {
    var arrayOptionItem = _page.FilterCollection;
    var isUncheck = true;
    var item;
    if ($(arrayOptionItem[0]) != null) {
        for (var i = 1; i < arrayOptionItem.length; i++) {
            item = $(arrayOptionItem[i]);
            if (item != undefined && item != null) {
                if (item.checked == false) {
                    isUncheck = false;
                    break;
                }
            }
        }
        $(arrayOptionItem[0]).checked = isUncheck;
    }
}

function Winloss_SearchByDate(fdate, tdate, targetCustId, actionPage) {
    var fdate = new Date($("fdate").value);
    var tdate = new Date($("tdate").value);
    var month = tdate.getMonth() - fdate.getMonth();
    if (tdate.getFullYear() > fdate.getFullYear())
        month += 12;
    if (month >= 2)
        $("frmmain").submit();
    else {
        ageMsg.Show(_page.least3monthtochart);
        EnableButton();
    }
}

//Tooltip on chart
var TOOLTIPONCHART
var wnd_height, wnd_width;
var tooltip_height, tooltip_width;
var tooltip_shown=false;
var	transparency = 100;
var timer_id = 1;
var tooltiptext;

// override events
window.onload = WindowLoading;
window.onresize = UpdateWindowSize;
document.onmousemove = AdjustToolTipPosition;

function DisplayTooltip(tooltipText, className) {
    TOOLTIPONCHART.innerHTML = tooltipText;
    TOOLTIPONCHART.className = className;
	tooltip_shown = (tooltipText != "")? true : false;
	if(tooltipText != "")
	{
		// Get tooltip window height
		tooltip_height=(TOOLTIPONCHART.style.pixelHeight)? TOOLTIPONCHART.style.pixelHeight : TOOLTIPONCHART.offsetHeight;
		transparency=0;
		ToolTipFading();
	}
	else
	{
		clearTimeout(timer_id);
		TOOLTIPONCHART.style.visibility="hidden";
	}
}

function AdjustToolTipPosition(e)
{
	if(tooltip_shown)
	{
		// Depending on IE/Firefox, find out what object to use to find mouse position
		var ev;
		if(e)
			ev = e;
		else
			ev = event;

		TOOLTIPONCHART.style.visibility = "visible";

		offset_y = (ev.clientY + tooltip_height - document.body.scrollTop + 30 >= wnd_height) ? - 15 - tooltip_height: 20;
		TOOLTIPONCHART.style.left = Math.min(wnd_width - tooltip_width - 10 , Math.max(3, ev.clientX + 6)) + document.body.scrollLeft + 'px';
		TOOLTIPONCHART.style.top = ev.clientY + offset_y + document.body.scrollTop + 'px';
	}
}

function WindowLoading() {
    TOOLTIPONCHART = document.getElementById('toolTipOnChart');
    if (TOOLTIPONCHART == null) return;
	// Get tooltip  window width
	tooltip_width = (TOOLTIPONCHART.style.pixelWidth) ? TOOLTIPONCHART.style.pixelWidth : TOOLTIPONCHART.offsetWidth;

	// Get tooltip window height
	tooltip_height=(TOOLTIPONCHART.style.pixelHeight)? TOOLTIPONCHART.style.pixelHeight : TOOLTIPONCHART.offsetHeight;

	UpdateWindowSize();
}

function ToolTipFading()
{
	if(transparency <= 100)
	{
		TOOLTIPONCHART.style.filter="alpha(opacity="+transparency+")";
		TOOLTIPONCHART.style.opacity=transparency/80;
		transparency += 5;
		timer_id = setTimeout('ToolTipFading()', 35);
	}
}

function UpdateWindowSize()
{
	wnd_height=document.body.clientHeight;
	wnd_width=document.body.clientWidth;
}