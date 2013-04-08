<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResetPass.aspx.cs" Inherits="Popup_ResetPass" %>

<script type="text/javascript" src="../../Js/opt/jx.js?<%= AppSettings.JsVersion %>"></script>

<script src="../_GlobalResources/Js/jx.js?<%= AppSettings.JsVersion %>" type="text/javascript"></script>

<script type="text/javascript">

    function doCancel() {
        if (IsNoBackground == 'True') {
            parent.ageWnd.close();
            return false;
        }
        else {
            $('diverrmsg').style.display = 'none';
            $('pw').value = '';
            $('cpw').value = '';
            if ($('pass_td_pass'))
                $('pass_td_pass').src = '../App_Themes/AgencyV2/Images/dot.gif';
            if ($('pass_td_confirmpass'))
                $('pass_td_confirmpass').src = '../App_Themes/AgencyV2/Images/dot.gif';
        }
    }

    function GetParameterValue(name, url) {
        if (url == null) return null;

        var separator = url.contains("?" + name + "=") ? "?" : "&";

        var parameter = separator + name + "=";

        var i1 = url.indexOf(parameter);
        if (i1 == -1) return null;

        var tmp = url.substr(i1);
        var i2 = tmp.indexOf("&", 1);

        var value = i2 >= 0 ? tmp.substr(parameter.length, i2 - parameter.length) : tmp.substr(parameter.length);

        return value;
    }

    String.prototype.contains = function(sub) {
        return this.indexOf(sub, 0) != -1;
    }


    function ChangePass(custid) {

        var issync = GetParameterValue("issync", location.href);



        if ($('pw').value == "") {
            Err($('errmsgPassEmpty').value);
            AddHeight(50);
            $('pw').focus();
            return false;
        }
        else if ($('cpw').value == "") {
            Err($('errmsgCofirmPassEmpty').value);
            $('cpw').focus();
            AddHeight(50);
            return false;
        }
        else if ($('cpw').value != $('pw').value) {
            Err($('errmsgErrConfirm').value);
            $('cpw').focus();
            AddHeight(50);
            return false;
        }
        else if (check($('pw').value) == false || check($('cpw').value) == false) {
            Err($('errmsgPassValidate').value);
            AddHeight(90);
            return false;
        }
        else {

            Ajax.Request('../Handlers/ChangePassMemberInfo.ashx', {
                method: 'post',
                parameters: 'upw=' + $('pw').value + '&custid=' + custid + '&issync=' + issync,
                onComplete: function(res) {
                    if (res.responseText == 'Ok') {
                        if (IsNoBackground == 'True') {
                           parent.ageWnd.close();
                        }
                        else {
                            doCancel();
                            $('btnSubmit').focus();
                            Err($('succmsg').value, 1);
                            AddHeight(50);
                        }
                    }
                    else if (res.responseText == '106') {
                        showErrMsg(lblconfirmclosesubacc, 0);
                        AddHeight(50);

                    }
                    else if (res.responseText == '105') // kick out
                    {
                        alert(lblConfirmClosed);
                        location.href = '../Authorization/Logout.ashx';
                    }
                    else {
                        Err(res.responseText);
                        AddHeight(100);
                    }
                }
            });
        }
        return false;
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

    function checkKeyEnter(e, custid) {
        var keycode = window.event ? e.keyCode : e.which;
        if (keycode == 13) //you hit return!
        {
            $('btnSubmit').click();
        }
    }

    function Err(msg, type) {
        showErrMsg(msg, type);
    }

    function init() {
        $('pw').value = '';
    }
    window.onload = init; 
</script>

