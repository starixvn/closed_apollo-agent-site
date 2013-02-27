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

function ClearMainMenu() {
    if ($('b0') != null) $('b0').className = 'Bleft_ParentNew';
    if ($('b1') != null) $('b1').className = 'Bleft_ParentNew';
}

function MenuItemSelect(index) {
    ClearCurrentLeftMenu();

    // Clear header tab
    if (top.frHeader) {
        if (top.frHeader.ClearActiveTab) {
            top.frHeader.ClearActiveTab();
        }
    }
    
    var menuItem = $('menuitem' + index);
    menuItem.className = 'Bleft_Sub_2';
    currentMenuItem = menuItem;

    ClearMainMenu();
}

function MenuToggle(index) {
    var subMenu = $('div' + index);
    subMenu.style.display = (subMenu.style.display == 'none') ? 'block' : 'none';
    var aMenu = $('a' + index);
    aMenu.className = (aMenu.className == 'Bleft_Parent') ? 'Bleft_ParentAc' : 'Bleft_Parent';
}

function MenuClick(index) {
    if (index == 'b0' || index == 'b1') {
        if (index == 'b0') {
            if ($('b1') != null) $('b1').className = 'Bleft_ParentNew';
        }
        else {
            if ($('b0') != null) $('b0').className = 'Bleft_ParentNew';
        }
        $(index).className = ($(index).className == 'Bleft_ParentNew') ? 'Bleft_ParentAcNew' : 'Bleft_ParentNew';
        ClearCurrentLeftMenu();
    }
    else {
        MenuToggle(index);
        ClearMainMenu();
    }
}

function OpenPage(url, width, height) {
    var wnd = window.open('../' + url, 'flashuserguide', 'height=' + height + ',width=' + width + ',menubar=no,status=no,scrollbars=yes,resizable=yes');
    if (typeof window.focus != 'undefined' && wnd) wnd.focus();
    return false;
}