<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddMaster.aspx.vb" Inherits="AddNewMaster" %>

<html>
<head>
    <title></title>
    <link type="text/css" href="../../App_Themes/AgencyV2/main.css" rel="Stylesheet" />
    <link href="../../App_Themes/AgencyV2/newagent.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        .right
        {
            text-align: right;
        }
        span.mp
        {
            color: #ffffff;
            font-size: 8pt;
            font-weight: bold;
        }
        body
        {
            margin: 0 0 0 0;
            padding: 0 0 0 0;
        }
    </style>
    <script type="text/javascript" src="../../Js/opt/Core.js?<%= AppSettings.JsVersion %>"></script>
    <script type="text/javascript" src="../../Js/Common.js?<%= AppSettings.JsVersion %>"></script>
    <script type="text/javascript" src="../../Js/AddNewMaster.js?<%=AppSettings.JsVersion %>"></script>
    <script type="text/javascript" src="../../Js/EditMaster.js?<%=AppSettings.JsVersion %>"></script>
    <script type="text/javascript" src="../../Js/Customer.js?<%=AppSettings.JsVersion %>"></script>
    <%VariableOfScript()%>
    <script type="text/javascript">
        var errMsgMinPT = alertLessthanMinPT;
        var flag = false;

        function BtnAddCSClick() {
            $('hidCS').value = 'true';
            var isOK = BtnOnClick();

            if (isOK) {
                var btnAddCS = $("btnAddCS");
                btnAddCS.style.color = "#cccccc";
                btnAddCS.disabled = true;
                btnAddCS.onclick = ";";
            }
        }

        function BtnOnClick() {
            var arrAcc;
            var arrPAcc;
            var arrMin;
            var isDirect = $("dirMaster") != null ? $("dirMaster").value : "0";

            if (!CheckInfoValue(false)) return false;
            if (isDirect == "1") {
                if (!CheckTime()) return false;
            }
            if (!CheckLimitMinMaxBet(1)) return false;

            if ($("chkCopyAll").checked) //this case for Copy All from Soccer
            {
                $("btnAdd").disabled = true;
                DelayReloadPage(null, 300000);
                enabledBtn(false);
                document.form.submit();
                return true;
            }

            //Begin check min Position taking for Basketball
            if (!CheckLimitMinMaxBet(2)) return false;

            if ($("chkCopyBasket").checked) //this case for Copy All from Basket ball
            {
                $("btnAdd").disabled = true;
                DelayReloadPage(null, 300000);
                enabledBtn(false);
                document.form.submit();
                return true;
            }
            if ($('txtMaxPerBall')) {
                if (!CheckLimitMaxPerBall()) return false;
            }
            //Begin check min Position taking for Football
            if (!CheckLimitMinMaxBet(3)) return false;

            //Begin check min Position taking for Tennis
            if (!CheckLimitMinMaxBet(5)) return false;

            //Begin check min Position taking for Baseball
            if (!CheckLimitMinMaxBet(8)) return false;

            //Begin check min Position taking for Golf
            if (!CheckLimitMinMaxBet(10)) return false;

            //Begin check min Position taking for MotoSports
            if (!CheckLimitMinMaxBet(11)) return false;

            if ($('txtMinbet161') || $('txtMaxbet161') || $('txtMaxPerMatch161')) {
                // Check min max position taking for Number Game
                if (!CheckLimitMinMaxBet(161)) return false;
            }

            //Begin check min Position taking for Other Sports
            if (!CheckLimitMinMaxBet(99)) return false;

            // Check min max position taking for Mix Sports
            if ($('txtMinbet99_M') || $('txtMaxbet99_M') || $('txtMaxPerMatch99_M')) {
                if (!CheckLimitMinMaxBet('99_M')) return false;
            }

            $("btnAdd").disabled = true;

            DelayReloadPage(null, 300000);
            enabledBtn(false);
            document.form.submit();

            return true;
        }

        AGE.addEvent(window, 'load', selectTranferTime);
        _FocusEl = 'number1';

    </script>
</head>
<body>
    <iframe name="InsertData" id="InsertData" style="display: none"></iframe>
    <form id="form" name="form" method="post" action="UpdateNewMaster.aspx" target="InsertData">
    <input type="hidden" id="dirMaster" name="dirMaster" runat="server" />
    <asp:Literal ID="htmlAddMasterContent" runat="server" EnableViewState="false"></asp:Literal>
    </form>
</body>
</html>