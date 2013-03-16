var arrSuper201 = ['listM201OE'];
var arrMaster201 = [];
function CheckCopyPT(chkCopy, arrSuper, chk) {
    var c = document.getElementById(chk);
    if (!c) chk = 0;
    for (var i = 0; i < arrSuper.length; i++) {
        if (document.getElementById(arrSuper[i])) {
            document.getElementById(arrSuper[i]).disabled = (chkCopy.checked) ? true : ((!chk) ? false : ((document.getElementById(chk).checked) ? true : false));
        }
    }
}	