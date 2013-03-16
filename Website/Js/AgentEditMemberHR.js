var arrSuper151TO = ['listMTOPlace'];
var arrSuper151FO = ['listMFOPlace'];
var arrchk = ['listMTOPlace', 'listMFOWin', 'listMFOPlace', 'chkCopyTOWin', 'chkCopyFOWin', 'txtMinbetFO', 'txtMaxbetFO', 'txtMaxPerMatchFO'];
var arrMaster151TO = [];
var arrMaster151FO = [];

function CheckCopyPT(chkCopy, arrSuper, chk) {
    var c = document.getElementById(chk);
    if (!c) chk = 0;
    for (var i = 0; i < arrSuper.length; i++) {
        if (document.getElementById(arrSuper[i])) {
            document.getElementById(arrSuper[i]).disabled = (chkCopy.checked) ? true : ((!chk) ? false : ((document.getElementById(chk).checked) ? true : false));
        }
    }
}

function CopyAllPT(chkCopyAll) {    
    for (i = 0; i < arrchk.length; i++) {
        if (document.getElementById(arrchk[i])) {
            document.getElementById(arrchk[i]).disabled = chkCopyAll.checked;
        }
    }
    CheckCopyPT(chkCopyAll, arrSuper151TO, 'chkCopyTOWin');
    CheckCopyPT(chkCopyAll, arrSuper151FO, 'chkCopyFOWin');
    
}