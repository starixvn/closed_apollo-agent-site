var btnRefresh = $('ucCountDown_btnRefresh');
var pnlMinDown = $('ucCountDown_PanelDown');
var curCountDown = initCountDown;
var ckiName = "ibcCountDown" + custId + countDownId;

function ucCountDown_Tick() {
    btnRefresh.innerHTML = curCountDown;
    curCountDown--;
    setTimeout("ucCountDown_Tick()", 1000);

    if (curCountDown < -1) {
        ucCountDown_refresh();
    }
}

function ucCountDown_refresh() {
    if (btnRefresh.innerHTML.search("span") > 0) { return false; } //disable when loading
    btnRefresh.innerHTML = loadingTxt;
    //btnRefresh.className = "btnLoading";    

    if (funcRefresh != "") {        
        eval(funcRefresh);
    }
    else {
        if (handlerProc == "")
            window.location.href = window.location.href;
        else updateData(handlerProc, placeHolder);
    }

    curCountDown = initCountDown; //reset count down
    //btnRefresh.className = "btnRefresh";
    return true;
}

function updateData(handler, placeHolder) {
    Ajax.Request(handler,
                    {
                        asynchronous: true,
                        method: 'get',
                        parameters: '',
                        onComplete: function bindData(rs) {
                            $(placeHolder).innerHTML = rs.responseText;                            
                        }
                    }
                );
}

function saveMinutes(idNewMinute) {    
    pnlMinDown.style.display = 'none';
    var txtMins = $(idNewMinute).innerHTML;
    initCountDown = parseInt(txtMins);
    doCheckMenu();
    createCookie(ckiName, initCountDown, 35);
    setCountDown(initCountDown); //reset
    
}

function doCheckMenu() {    
    var menuMins = pnlMinDown.getElementsByTagName("div");
    for (i = 0; i < menuMins.length; i++) {
        if (parseInt(menuMins[i].innerHTML) == initCountDown) {            
            addClassName(menuMins[i].id, "menuitemcheck");
        }
        else
            removeClassName(menuMins[i].id, "menuitemcheck");
    }    
}

function doRefresh() {
    var pnlDown = $('ucCountDown_PanelDown');
    if (pnlDown.style.display == '')
        pnlDown.style.display = 'none';
    return ucCountDown_refresh();
}

function doOpenPnlOption(btnDown) {
    var pnlDown = $('ucCountDown_PanelDown');
    var arrBtnPos = findPosition(btnDown.parentNode);
    if (pnlDown.style.display == 'none') {
        pnlDown.style.left = arrBtnPos[0] + 'px';
        pnlDown.style.top = (arrBtnPos[1] + 20) + 'px';
        pnlDown.style.display = '';
    }
    else pnlDown.style.display = 'none';
}

function setCountDown(newCountDown) {
    initCountDown = newCountDown;
    curCountDown = initCountDown;
}

//read countdown from cookie
var countDownFromCookie = readCookie(ckiName);
if (countDownFromCookie && countDownFromCookie != 0) {
    setCountDown(countDownFromCookie);
}
doCheckMenu();

//do count down
ucCountDown_Tick();

