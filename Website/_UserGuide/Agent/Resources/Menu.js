var currentItem = 0;
var oldClassName = 'left_menu';
var newClassName = 'left_menu_curent';

var currentTab = 0;
var oldClssNameTab = 'b01';
var newClssNameTab = 'b01_current';
function TabMenu_Select(order) {

    document.getElementById('xxx' + order).className = newClssNameTab;
    if (currentTab != order) {
        document.getElementById('xxx' + currentTab).className = oldClssNameTab;
    }
    currentTab = order;
    document.getElementById('menuitem' + currentItem).className = oldClassName;
}

function menu_select(order) {

    document.getElementById('menuitem' + order).className = newClassName;
    if (currentItem != order) {
        document.getElementById('menuitem' + currentItem).className = oldClassName;

    }
    currentItem = order;
    document.getElementById('xxx' + currentTab).className = oldClssNameTab;
}

ddaccordion.init({
    headerclass: "expandable",
    contentclass: "categoryitems",
    revealtype: "click",
    mouseoverdelay: 200,
    collapseprev: true,
    defaultexpanded: [0],
    onemustopen: false,
    animatedefault: false,
    persiststate: true,
    toggleclass: ["", "openheader"],
    togglehtml: ["prefix", "", ""],
    animatespeed: "fast",
    oninit: function(headers, expandedindices) {
    },
    onopenclose: function(header, index, state, isuseractivated) {
    }
})

window.onload = loadwindow;
function loadwindow() {
    document.getElementById('xxx0').className = newClssNameTab;
}
