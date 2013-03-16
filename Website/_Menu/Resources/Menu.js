/*
* Created 20100203@Lee - Left menu page functions
* Revision ?@? - ... 
* 
*/


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
    var subMenu = document.getElementById('div' + index);
    subMenu.style.display = (subMenu.style.display == 'none') ? 'block' : 'none';
    var aMenu = document.getElementById('a' + index);
    aMenu.className = (aMenu.className == 'Bleft_Parent') ? 'Bleft_ParentAc' : 'Bleft_Parent';
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