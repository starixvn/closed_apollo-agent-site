var arrSuper201 = ['listM201OE'];
var arrMaster201 = [];
function CheckCopyPT(chkCopy, arrSuper, chk) {
    var c = $(chk);
    if (!c) chk = 0;
    for (var i = 0; i < arrSuper.length; i++) {
        if ($(arrSuper[i])) {
            $(arrSuper[i]).disabled = (chkCopy.checked) ? true : ((!chk) ? false : (($(chk).checked) ? true : false));
        }
    }
}	