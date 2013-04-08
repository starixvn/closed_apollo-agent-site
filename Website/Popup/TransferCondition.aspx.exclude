<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransferCondition.aspx.cs" Inherits="Popup_TransferCondition" %>

<script type="text/javascript" src="../../Js/opt/jx.js?<%= AppSettings.JsVersion %>"></script>
<script src="../_GlobalResources/Js/jx.js?<%= AppSettings.JsVersion %>" type="text/javascript"></script>
<script type="text/javascript">
    function CheckedTransTime() {
        var ele = $('alldays').getElementsByTagName("input");
        for (var i = 1; i < ele.length; i++) {
            ele[i].disabled = $('rdbdaily').checked;
        }

    }
    function CheckedDateChoose(val) {
        var count = 0;
        if ($('rdbweekly').checked == true) {
            if (val.checked) {
                val.value = '1';
            }
            else {
                val.value = '0';
            }
            var ele = $('alldays').getElementsByTagName("input");
            for (var i = 1; i < ele.length; i++) {
                if (ele[i].checked) {

                    count++;
                    if (count == 7) {
                        Err($('choosedaily').value);
                        AddHeight(200);
                        val.value = '0';
                        return false;
                    }

                }
            }
        }
    }
    function UpdateTransCondition() {

        var count = 0;
        var transop = $('chksun').value + $('chkmon').value + $('chktue').value + $('chkwed').value + $('chkthur').value + $('chkfri').value + $('chksat').value;
        if ($('rdbweekly').checked == true) {
            transop += '1';
            var ele = $('alldays').getElementsByTagName("input");
            for (var i = 1; i < ele.length; i++) {
                if (ele[i].checked) {
                    count++;
                }
            }
            if (count == 0) {
                Err($('chooseoneday').value);
                AddHeight(200);
                return false;
            }
        }
        else {
            transop = '00000000';
        }
        if (confirm($('confirm').value)) {
            Ajax.Request('../Handlers/UpdateTransferCondition.ashx', {
                method: 'post',
                parameters: 'transop=' + transop + '&custid=' + $('custid').value,
                onComplete: function (res) {
                    if (res.responseText == 'Ok') {
                        if ($('nobackground')) {
                            showErrMsg(transfersuccessful, 1);
                        }
                        else {
                            parent.DelayPage();
                        }
                    }
                    else if (res.responseText == '106') {
                        showErrMsg(lblconfirmclosesubacc, 0);

                    }
                    else if (res.responseText == '105') // kick out
                    {
                        alert(lblConfirmClosed);
                        location.href = '../Authorization/Logout.ashx';
                    }
                    else {
                        Err(res.responseText);
                        AddHeight(200);
                    }
                }
            });
        }

    }
    function Err(msg) {
        showErrMsg(msg, 0);
    }

    var ageWnd = false;
    if (typeof parent.AGEWnd != 'undefined') {
        if (!parent.AGEWnd._preHeight) {
            parent.AGEWnd._preHeight = 0;
        }
    }

    function AddHeight(h) {
        if (!ageWnd) {
            ageWnd = parent.ageWnd;
            if (typeof ageWnd == 'undefined') return;

            if (parent.AGEWnd._preHeight == 0) {
                parent.AGEWnd._preHeight = ageWnd.height;
            }
        }

        ageWnd.setRect(null, null, ageWnd.width, parent.AGEWnd._preHeight + h);
    }

    window.onload = function () {

        ageWnd = parent.ageWnd;
        if (typeof ageWnd == 'undefined') {
            return;
        }

        if (ageWnd.loaded) {
            ageWnd.setRect(null, null, 550, 170);
        }
    }
</script>
