function ShowFrmUpdRacingStatus(obj, hasDownLine) {
    // Show div pop up to update 6 statuses
    showDivRacingPopup(obj);
    show_hide_column(1, hasDownLine, $('disabledRacingStatusPop').firstChild);

    function show_hide_column(col_no, do_show, tbl) {
        var stl;
        if (do_show) stl = 'block'
        else stl = 'none';
        var rows = tbl.getElementsByTagName('tr');
        for (var row = 0; row < rows.length; row++) {
            var cels = rows[row].getElementsByTagName('td')

            if (cels[col_no]) {
                cels[col_no].style.display = stl;
            }
        }
    }

    if (_page.RoleId - _page.CustRoleId >= 2) {
        
    }
}

var popupOffsetHeight = 245;
var popupOffsetRowHeight = 23;

function checkDisableBetTypes() {
    if (!_page.IsShowRacing_151_31) {
        $('row_151_31').style.display = 'none';
        popupOffsetHeight -= popupOffsetRowHeight;
    }

    if (!_page.IsShowRacing_151_51) {
        $('row_151_51').style.display = 'none';
        popupOffsetHeight -= popupOffsetRowHeight;
    }

    if (!_page.IsShowRacing_152_41) {
        $('row_152_41').style.display = 'none';
        popupOffsetHeight -= popupOffsetRowHeight;
    }

    if (!_page.IsShowRacing_152_51) {
        $('row_152_51').style.display = 'none';
        popupOffsetHeight -= popupOffsetRowHeight;
    }

    if (!_page.IsShowRacing_152_41 && !_page.IsShowRacing_152_51) {
        $('row_152_hd').style.display = 'none';
        popupOffsetHeight -= popupOffsetRowHeight;
    }

    if (!_page.IsShowRacing_153_51) {
        $('row_153_51').style.display = 'none';
        $('row_153_hd').style.display = 'none';
        popupOffsetHeight -= 2 * popupOffsetRowHeight;
    }    
}

var LENGTHS = 6;
var Ids = ['151_41', '151_31', '151_51', '152_41', '152_51', '153_51'];
var chkEnableds = ['chkEnabled_151_41', 'chkEnabled_151_31', 'chkEnabled_151_51', 'chkEnabled_152_41', 'chkEnabled_152_51', 'chkEnabled_153_51'];
var chkEnabledDLs = ['chkEnabledDL_151_41', 'chkEnabledDL_151_31', 'chkEnabledDL_151_51', 'chkEnabledDL_152_41', 'chkEnabledDL_152_51', 'chkEnabledDL_153_51'];

function refreshFrmUpdRacing() {

    if (window.resultRacing) {
        onGetComplete(window.resultRacing);
    }
    else {
        ajax.PostJSON('Handlers/GetRacingStatus.ashx?custid=' + _page.UserId, '', onGetComplete);
    }

    function onGetComplete(result) {
        if (result.roleid > 0) {
            window.resultRacing = result;

            var isDirectDL = (_page.RoleId - _page.CustRoleId) == 1;
            
            for (var i = 0; i < LENGTHS; i++) {
                $(chkEnableds[i]).checked = eval('!window.resultRacing.Disable' + Ids[i]);
                $(chkEnabledDLs[i]).checked = eval('!window.resultRacing.DisableDownlines' + Ids[i]);

                var checkPermisssionSubAccount = true;
                var permissionDetailRC = $(chkEnableds[i]).getAttribute('AccountPermisssion');
                if (permissionDetailRC != null) {
                    if (permissionDetailRC.length != 0) {
                        if (top.AGE.PERMISSIONS.indexOf(permissionDetailRC) == -1) {
                            $(chkEnableds[i]).disabled = true;
                            $(chkEnabledDLs[i]).disabled = true;
                            checkPermisssionSubAccount = false;
                        }
                    }
                }

                if (checkPermisssionSubAccount) {
                    $(chkEnableds[i]).disabled = !isDirectDL || eval('window.resultRacing.DisableUplines' + Ids[i]);
                    $(chkEnabledDLs[i]).disabled = true;
                }
            }
        }
    }

    $('btnUpdRacingStatus').disabled = true;
}

function initFrmUpdRacing() {

    age.addEvent($('disabledRacingStatusPop'), "click", checkBtnUpdRacingStatus, true);
    age.addEvent($('btnUpdRacingStatus'), 'click', function() {
        var params = ajax.CreateParams2(chkEnableds.concat(chkEnabledDLs));

        var updatedisables = new Array();
        for (var i = 0; i < LENGTHS; i++) {
        
            var isChanged = ($(chkEnableds[i]).checked == eval('window.resultRacing.Disable' + Ids[i]));
            isChanged = isChanged || ($(chkEnabledDLs[i]).checked == eval('window.resultRacing.DisableDownlines' + Ids[i]));
            
            updatedisables.push('updatedisable' + Ids[i] + '=' + isChanged);
        }

        params += '&' + updatedisables.join('&');

        var confirmed = true;
        for (var i = 0; i < LENGTHS; i++) {
            if (!$(chkEnableds[i]).checked && eval('!window.resultRacing.Disable' + Ids[i])) {
                confirmed = confirm(_page.alertUpdRacingStatus);
                break;
            }
        }

        if (!confirmed) {
            return;
        }

        $('btnUpdRacingStatus').disabled = true;

        ajax.PostJSON('Handlers/UpdateRacingStatus.ashx?custid=' + _page.UserId, params, onUpdateComplete);
    }, true);

    function onUpdateComplete(result) {

        window.resultRacing = null;
    
        if (typeof result.errCode != 'undefined' && result.errCode != 0) {
            refreshFrmUpdRacing();
            alert(result.errMsg);
            return;
        }
        
        var ArrCodes = (result.ArrCodes);
        var ArrMsgs = (result.ArrMsgs);
        var c = ArrCodes.length;
        for (var i = 0; i < c; i++) {
            if (parseInt('0' + ArrCodes[i]) > 0) {
                alert(ArrMsgs[i]);
            }
        }
        
        closeDivRacingPopup();
        setTimeout(refreshFrmUpdRacing, 2000);
    }

    // Check disable downlines check box
    for (var i = 0; i < LENGTHS; i++) {
        age.addEvent($(chkEnableds[i]), 'click', function() {

            var j = 0
            for (j = 0; j < LENGTHS; j++) {
                if (chkEnableds[j] == this.id)
                    break;
            }

            if (!$(chkEnableds[j]).checked) {
                $(chkEnabledDLs[j]).disabled = true;
                $(chkEnabledDLs[j]).checked = false;
            }
            else {
                $(chkEnabledDLs[j]).disabled = eval('!window.resultRacing.Disable' + Ids[j]);
                $(chkEnabledDLs[j]).checked = eval('!window.resultRacing.DisableDownlines' + Ids[j]);
            }

        }, true);
    }
}

function checkBtnUpdRacingStatus() {
    var isChanged = false;

    if (window.resultRacing) {
        for (var i = 0; i < LENGTHS; i++) {
        
            isChanged = isChanged || ($(chkEnableds[i]).checked == eval('window.resultRacing.Disable' + Ids[i]));
            if (_page.roleid != 1) {
                isChanged = isChanged || ($(chkEnabledDLs[i]).checked == eval('window.resultRacing.DisableDownlines' + Ids[i]));
            }
        }
    }
    
    $('btnUpdRacingStatus').disabled = !isChanged;
}

function showDivRacingPopup(obj) {

    refreshFrmUpdRacing();
    
    if (obj.custId != _page.UserId) {
        obj.custId = _page.UserId;
        initFrmUpdRacing();
        checkDisableBetTypes();
    }
    checkBtnUpdRacingStatus();
    
    var curTop = 0;
    if (obj.offsetParent) {
        do {
            curTop += obj.offsetTop;
        } while (obj = obj.offsetParent);
    }
    curTop -= popupOffsetHeight;
    
    $('disabledRacingStatusPop').style.top = curTop + 'px';
    $('disabledRacingStatusPop').style.display = 'block'; //show popup
    $('overlayBg').style.display = 'block'; //show overlay    
}

function closeDivRacingPopup() {
    if ($('disabledRacingStatusPop')) $('disabledRacingStatusPop').style.display = 'none';
    $('overlayBg').style.display = 'none';
}