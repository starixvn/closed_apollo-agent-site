/// <reference path="../../_GlobalResources/Js/jquery-1.9.1.min.js" />



var currentMenuItem = null;
function ClearCurrentLeftMenu() {
    if (currentMenuItem != null) {
        currentMenuItem.className = 'Bleft_Sub';
    }
}

function MenuItemSelect(index) {
    ClearCurrentLeftMenu();

    // Clear header tab
    if (top.frHeader) {
        if (top.frHeader.ClearActiveTab) {
            top.frHeader.ClearActiveTab();
        }
    }
    
    var menuItem = document.getElementById('menuitem' + index);
    if(!menuItem) return;
    menuItem.className = 'Bleft_Sub_2';
    currentMenuItem = menuItem;
}

function MenuToggle(index) {
    var aMenu = $('#a' + index),
        subMenu = $('#div' + index),
        parent = $('#leftpane'),
        isThisActive = subMenu.is(':visible');

    parent.find('.Bleft_Parent0').removeClass('active');
    parent.find('.submenu-list').slideUp('fast');

    if (!isThisActive) {
        aMenu.parent().addClass('active');            
        subMenu.slideDown('fast');
    }
}

function StopBlinkAccInfoTab() {

}

function ShowFlashAccInfoMaster() {

}

function OpenPage(url, width, height) {
    var wnd = window.open(age.GetBaseUrl() + url, 'flashuserguide', 'height=' + height + ',width=' + width + ',menubar=no,status=no,scrollbars=yes,resizable=yes');
    if (typeof window.focus != 'undefined' && wnd) wnd.focus();
    return false;
}

function AddIconNew() {
    var imageNew = "&nbsp;&nbsp;<img border='0' src='Resources/Images/new3.gif' />";
    if (document.getElementById("menu3")) document.getElementById("menu3").innerHTML += imageNew;
}

function iniMenu() {
    //AddIconNew();
    SetiPhoneAdGifLinkUrl();
}

function SetiPhoneAdGifLinkUrl() {
    var ParentElement = document.getElementById('leftpane');
    if (document.getElementById('linkToMobileSiteID')) {
        var currentDomain= document.domain.replace("agt.","");
        document.getElementById('linkToMobileSiteID').href = "http://m."+ currentDomain;        
    }
}

RegisterStartUp("iniMenu()");