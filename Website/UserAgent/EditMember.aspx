<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditMember.aspx.vb" Inherits="EditMember2" %>

<%@ OutputCache Location="None" %>
<link href="../../App_Themes/AgencyV2/newagent.css?<%= AppSettings.JsVersion %>" type="text/css" rel="Stylesheet" />
<style type="text/css">
    span.mp
    {
        color: #fff;
        font-size: 8pt;
        font-weight: bold;
    }
    SELECT
    {
        width: 50px;
    }
    .Sel60
    {
        width: 60px;
    }
</style>
<script src="../../Js/AddNewMaster.js?<%= AppSettings.JsVersion %>" type="text/javascript"></script>
<script src="../../Js/Customer.js?<%= AppSettings.JsVersion %>" type="text/javascript"></script>
<script type="text/javascript">
    var arr1Super1 = ['listM1OUD', 'listM11stHdpD', 'listM11stOUD', 'listM1OE'];

    var arr1_BetSetting = [];
    var arr2_BetSetting = [];
    var arr3_BetSetting = [];
    var arr5_BetSetting = [];
    var arr10_BetSetting = [];
    var arr99BetSetting = [];
    var arr8BetSetting = [];
    var arr11BetSetting = [];
    var arr161BetSetting = [];
    var arr99_M_BetSetting = [];

    var arr1Master1 = [];
    var arr2Super1 = ['listM1CS', 'listM1TG', 'listM1MP', 'listM1OR'];
    var arr2Master1 = [];
    var arr3Super1 = ['listM1OUL', 'listM11stHdpL', 'listM11stOUL'];
    var arr3Master1 = [];
    var arrSuper2 = ['listM2OU', 'listM2OE', 'listM2MP', 'listM2ML'];
    var arrMaster2 = [];

    var arrchk = ['listM1HdpL', 'listM2Hdp', 'listM3Hdp', 'listM5Hdp', 'listM10Hdp', 'chkCopy10Hdp', 'listM99Hdp', 'listM8Hdp',
	              'chkCopy1HdpDeadball', 'chkCopy11x2', 'chkCopy1HdpLive', 'chkCopy2Hdp', 'chkCopy3Hdp', 'chkCopy5Hdp', 'chkCopy10Hdp', 'chkCopy99Hdp', 'chkCopy8Hdp', 'listM11Hdp', 'chkCopy11Hdp', 'listM11x2', 'chkCopyBasket',
	              'txtMinbet2', 'txtMaxbet2', 'txtMaxPerMatch2', 'txtMinbet3', 'txtMaxbet3', 'txtMaxPerMatch3', 'txtMinbet5', 'txtMaxbet5', 'txtMaxPerMatch5', 'txtMinbet8', 'txtMaxbet8', 'txtMaxPerMatch8', 'txtMinbet10', 'txtMaxbet10', 'txtMaxPerMatch10', 'txtMinbet11', 'txtMaxbet11', 'txtMaxPerMatch11', 'txtMinbet99', 'txtMaxbet99', 'txtMaxPerMatch99', 'txtMinbet99_M', 'txtMaxbet99_M', 'txtMaxPerMatch99_M'];


    var arrchkBaskball = ['chkCopyAll', 'listM3Hdp', 'listM5Hdp', 'listM10Hdp', 'chkCopy10Hdp', 'listM99Hdp', 'listM8Hdp', 'listM11Hdp',
	'chkCopy2Hdp', 'chkCopy3Hdp', 'chkCopy5Hdp', 'chkCopy10Hdp', 'chkCopy99Hdp', 'chkCopy8Hdp', 'txtMinbet3', 'txtMaxbet3', 'txtMaxPerMatch3', 'txtMinbet5', 'txtMaxbet5', 'txtMaxPerMatch5', 'txtMinbet8', 'txtMaxbet8', 'txtMaxPerMatch8', 'txtMinbet10', 'txtMaxbet10', 'txtMaxPerMatch10', 'txtMinbet11', 'txtMaxbet11', 'txtMaxPerMatch11', 'txtMinbet99', 'txtMaxbet99', 'txtMaxPerMatch99', 'txtMinbet99_M', 'txtMaxbet99_M', 'txtMaxPerMatch99_M'];


    var arrSuper3 = ['listM3OU', 'listM3OE', 'listM3ML'];
    var arrMaster3 = [];
    var arrSuper5 = ['listM5OU', 'listM5OE', 'listM5ML'];
    var arrMaster5 = [];
    var arrSuper10 = ['listM10OU', 'listM10OE', 'listM10ML'];
    var arrMaster10 = [];
    var arrSuper99 = ['listM99OU', 'listM99OE', 'listM99ML'];
    var arrMaster99 = [];
    var arrSuper8 = ['listM8OU', 'listM8ML'];
    var arrMaster8 = [];
    var arrSuper11 = ['listM11ML'];
    var arrMaster11 = [];

    var arr161Super81 = ['listM1611stOED', 'listM161OED', 'listM161OED', 'listM161WRD'];
    var arr161Master81 = [];
    var arr161Super85 = ['listM161OEL', 'listM161HLL', 'listM161NCL', 'listM161NGL'];
    var arr161Master85 = [];


    var arrSuper99_M = ['listM99_M'];
    var arrMaster99_M = [];

    // Insert code by David
    var chkUpInfo = ["chkUpd2Bet", "chkUpd3Bet", "chkUpd5Bet", "chkUpd8Bet", "chkUpd10Bet", "chkUpd11Bet", "chkUpdInfo201", "chkUpd99Bet", "chkUpd99_MBet", "chkUpdInfoTO", "chkUpdInfoFO", "chkUpdInfo99_M"];
    var chkUpdPT = ["chkUpd2PT", "chkUpd3PT", "chkUpd5PT", "chkUpd8PT", "chkUpd10PT", "chkUpd11PT", "chkUpdPT201", "chkUpd99PT", "chkUpd99_MPT", "chkUpdPTTO", "chkUpdPTFO", "chkUpdPT99_M"];
    var Id = ["2", "3", "5", "8", "10", "11", "201", "99", "99_M", "cTO", "FO", "99_M"];

    //

    //chkCopy : checkbox copy all
    //arrSuper: array ids of PT
    //chk: checkbox copy from Handicap
    function CheckCopyPT(chkCopy, arrSuper, chk) {
        var c = $(chk);
        if (!c) chk = 0;
        for (var i = 0; i < arrSuper.length; i++) {
            if ($(arrSuper[i])) {
                $(arrSuper[i]).disabled = (chkCopy.checked) ? true : ((!chk) ? false : (($(chk).checked) ? true : false));
            }
        }
    }

    //Copy all position taking settings from Soccer Handicap
    function CopyAllPT(chkCopyAll) {

        if (chkCopyAll.checked) {
            $("panelSoccer").style.display = "";
        }

        if ($("isMult").value == "1") {
            if ($('chkUpd2Bet').checked || $('chkUpd2PT').checked) {
                $('chkCopyBasket').disabled = chkCopyAll.checked;
            }
            var SportTypeID = ['2', '3', '5', '10', '11', '8', '99', '99_M'];

            if ($('chkUpdSoccerBet').checked) {

                for (var i = 0; i < SportTypeID.length; i++) {
                    if (i == 0) continue;
                    if ($("divUpdInfo" + SportTypeID[i])) {
                        elements = $("divUpdInfo" + SportTypeID[i]).getElementsByTagName("input");
                        for (var j = 0; j < elements.length; j++) {
                            var id = elements[j].id;
                            if ($('chkUpd' + SportTypeID[i] + 'Bet')) $('chkUpd' + SportTypeID[i] + 'Bet').disabled = chkCopyAll.checked;
                            if ($('chkUpd' + SportTypeID[i] + 'Bet') && $('chkUpd' + SportTypeID[i] + 'Bet').checked) {
                                elements[j].disabled = chkCopyAll.checked;
                            }
                        }
                    }
                }

            }
            if ($('chkUpdSoccerPT') && $('chkUpdSoccerPT').checked) {
                CheckCopyPT(chkCopyAll, arr1Super1, 'chkCopy1HdpDeadball');
                CheckCopyPT(chkCopyAll, arr3Super1, 'chkCopy1HdpLive');
                CheckCopyPT(chkCopyAll, arr2Super1, 'chkCopy11x2');
                $('chkCopy1HdpDeadball').disabled = chkCopyAll.checked;
                $('chkCopy1HdpLive').disabled = chkCopyAll.checked;
                $('chkCopy11x2').disabled = chkCopyAll.checked;
                $('listM11x2').disabled = chkCopyAll.checked;
                $('listM1HdpL').disabled = chkCopyAll.checked;

            }
            $('chkUpd2PT').disabled =
            $('chkUpd3PT').disabled =
            $('chkUpd5PT').disabled =
            $('chkUpd10PT').disabled =
            $('chkUpd99PT').disabled =
            $('chkUpd8PT').disabled =
            $('chkUpd11PT').disabled =
            $('chkUpd99_MPT').disabled = chkCopyAll.checked;


            if ($('chkUpd2PT') && $('chkUpd2PT').checked) {
                CheckCopyPT(chkCopyAll, arrSuper2, 'chkCopy2Hdp');
                $('listM2Hdp').disabled = $('chkCopy2Hdp').disabled = chkCopyAll.checked;

            }
            if ($('chkUpd3PT') && $('chkUpd3PT').checked) {
                CheckCopyPT(chkCopyAll, arrSuper3, 'chkCopy3Hdp');
                $('listM3Hdp').disabled = $('chkCopy3Hdp').disabled = chkCopyAll.checked;
            }
            if ($('chkUpd5PT') && $('chkUpd5PT').checked) {
                CheckCopyPT(chkCopyAll, arrSuper5, 'chkCopy5Hdp');
                $('listM5Hdp').disabled = $('chkCopy5Hdp').disabled = chkCopyAll.checked;
            }
            if ($('chkUpd10PT') && $('chkUpd10PT').checked) {
                CheckCopyPT(chkCopyAll, arrSuper10, 'chkCopy10Hdp');
                $('listM10Hdp').disabled = $('chkCopy10Hdp').disabled = chkCopyAll.checked;
            }
            if ($('chkUpd99PT') && $('chkUpd99PT').checked) {
                CheckCopyPT(chkCopyAll, arrSuper99, 'chkCopy99Hdp');
                $('listM99Hdp').disabled = $('chkCopy99Hdp').disabled = chkCopyAll.checked;
            }
            if ($('chkUpd8PT') && $('chkUpd8PT').checked) {
                CheckCopyPT(chkCopyAll, arrSuper8, 'chkCopy8Hdp');
                $('listM8Hdp').disabled = $('chkCopy8Hdp').disabled = chkCopyAll.checked;
            }
            if ($('chkUpd11PT') && $('chkUpd11PT').checked) {
                CheckCopyPT(chkCopyAll, arrSuper11, 'chkCopy11Hdp');
                $('listM11Hdp').disabled = $('chkCopy11Hdp').disabled = chkCopyAll.checked;
            }
            if ($('chkUpd99_MPT') && $('chkUpd99_MPT').checked) {
                CheckCopyPT(chkCopyAll, arrSuper99_M);
                $('listM99_M').disabled = $('chkUpd99_MPT').disabled = chkCopyAll.checked;
            }

            //Insert by David 2/2/2012
            if ($("chkCopyAll").checked) {

                $('chkCopyBasket').disabled = true;

                for (var i = 0; i < chkUpInfo.length; i++) {
                    if ($(chkUpInfo[i]) == null) {
                        continue;
                    }

                    var elements = $("divUpdInfo" + Id[i]).getElementsByTagName("input");
                    for (var j = 0; j < elements.length; j++) {
                        elements[j].disabled = true;
                    }
                }

            }

            else {
                $('chkCopyBasket').disabled = false;

                for (var i = 0; i < chkUpInfo.length; i++) {
                    if ($(chkUpInfo[i]) == null) {
                        continue;
                    }

                    $(chkUpInfo[i]).disabled = false;
                    var elements = $("divUpdInfo" + Id[i]).getElementsByTagName("input");
                    for (var j = 0; j < elements.length; j++) {
                        elements[j].disabled = false;
                    }
                }
            }



            //
            return;
        }


        for (i = 0; i < arrchk.length; i++) {
            if ($(arrchk[i])) {
                $(arrchk[i]).disabled = chkCopyAll.checked;
            }
        }


        CheckCopyPT(chkCopyAll, arr1Super1, 'chkCopy1HdpDeadball');

        CheckCopyPT(chkCopyAll, arr2Super1, 'chkCopy11x2');

        CheckCopyPT(chkCopyAll, arr3Super1, 'chkCopy1HdpLive');

        CheckCopyPT(chkCopyAll, arrSuper2, 'chkCopy2Hdp');

        CheckCopyPT(chkCopyAll, arrSuper3, 'chkCopy3Hdp');

        CheckCopyPT(chkCopyAll, arrSuper5, 'chkCopy5Hdp');
        CheckCopyPT(chkCopyAll, arrSuper10, 'chkCopy10Hdp');
        CheckCopyPT(chkCopyAll, arrSuper99, 'chkCopy99Hdp');
        CheckCopyPT(chkCopyAll, arrSuper8, 'chkCopy8Hdp');
        CheckCopyPT(chkCopyAll, arrSuper11, 'chkCopy11Hdp');

        CheckCopyPT(chkCopyAll, arrSuper99_M);
    }

    function CopyBasketPT(chkCopyAll) {

        if ($("isMult").value == "1") {
            if ($('chkUpdSoccerBet').checked || $('chkUpdSoccerPT').checked) {
                $('chkCopyAll').disabled = chkCopyAll.checked;
            }
            var SportTypeID = ['2', '3', '5', '10', '11', '8', '99', '99_M'];
            if ($('chkUpd2Bet').checked) {
                for (var i = 0; i < SportTypeID.length; i++) {
                    if (i == 0) continue;
                    if ($("divUpdInfo" + SportTypeID[i])) {
                        elements = $("divUpdInfo" + SportTypeID[i]).getElementsByTagName("input");
                        for (var j = 0; j < elements.length; j++) {
                            var id = elements[j].id;
                            if ($('chkUpd' + SportTypeID[i] + 'Bet')) $('chkUpd' + SportTypeID[i] + 'Bet').disabled = chkCopyAll.checked;
                            if ($('chkUpd' + SportTypeID[i] + 'Bet') && $('chkUpd' + SportTypeID[i] + 'Bet').checked) {
                                elements[j].disabled = chkCopyAll.checked;
                            }
                        }
                    }
                }
            }

            $('chkUpd3PT').disabled =
            $('chkUpd5PT').disabled =
            $('chkUpd10PT').disabled =
            $('chkUpd99PT').disabled =
            $('chkUpd8PT').disabled =
            $('chkUpd11PT').disabled =
            $('chkUpd99_MPT').disabled = chkCopyAll.checked;

            if ($('chkUpd2PT') && $('chkUpd2PT').checked) {
                CheckCopyPT(chkCopyAll, arrSuper2, 'chkCopy2Hdp');
                $('chkCopy2Hdp').disabled = chkCopyAll.checked;

            }
            if ($('chkUpd3PT') && $('chkUpd3PT').checked) {
                CheckCopyPT(chkCopyAll, arrSuper3, 'chkCopy3Hdp');
                $('listM3Hdp').disabled = $('chkCopy3Hdp').disabled = chkCopyAll.checked;
            }
            if ($('chkUpd5PT') && $('chkUpd5PT').checked) {
                CheckCopyPT(chkCopyAll, arrSuper5, 'chkCopy5Hdp');
                $('listM5Hdp').disabled = $('chkCopy5Hdp').disabled = chkCopyAll.checked;
            }
            if ($('chkUpd10PT') && $('chkUpd10PT').checked) {
                CheckCopyPT(chkCopyAll, arrSuper10, 'chkCopy10Hdp');
                $('listM10Hdp').disabled = $('chkCopy10Hdp').disabled = chkCopyAll.checked;
            }
            if ($('chkUpd99PT') && $('chkUpd99PT').checked) {
                CheckCopyPT(chkCopyAll, arrSuper99, 'chkCopy99Hdp');
                $('listM99Hdp').disabled = $('chkCopy99Hdp').disabled = chkCopyAll.checked;
            }
            if ($('chkUpd8PT') && $('chkUpd8PT').checked) {
                CheckCopyPT(chkCopyAll, arrSuper8, 'chkCopy8Hdp');
                $('listM8Hdp').disabled = $('chkCopy8Hdp').disabled = chkCopyAll.checked;
            }
            if ($('chkUpd11PT') && $('chkUpd11PT').checked) {
                CheckCopyPT(chkCopyAll, arrSuper11, 'chkCopy11Hdp');
                $('listM11Hdp').disabled = $('chkCopy11Hdp').disabled = chkCopyAll.checked;
            }
            if ($('chkUpd99_MPT') && $('chkUpd99_MPT').checked) {
                CheckCopyPT(chkCopyAll, arrSuper99_M);
                $('listM99_M').disabled = $('chkUpd99_MPT').disabled = chkCopyAll.checked;
            }


            //Insert by David 2/2/2012

            if ($("chkCopyBasket").checked) {

                $('chkCopyAll').disabled = true;

                for (var i = 1; i < chkUpInfo.length; i++) {
                    if ($(chkUpInfo[i]) == null) {
                        continue;
                    }

                    var elements = $("divUpdInfo" + Id[i]).getElementsByTagName("input");
                    for (var j = 0; j < elements.length; j++) {
                        elements[j].disabled = true;
                    }
                }

            }

            else {
                $('chkCopyAll').disabled = false;

                for (var i = 1; i < chkUpInfo.length; i++) {
                    if ($(chkUpInfo[i]) == null) {
                        continue;
                    }

                    $(chkUpInfo[i]).disabled = false;
                    var elements = $("divUpdInfo" + Id[i]).getElementsByTagName("input");
                    for (var j = 0; j < elements.length; j++) {
                        elements[j].disabled = false;
                    }
                }
            }
            //

            return;
        }


        chkCopyBasket = $("chkCopyBasket");
        if (chkCopyBasket.checked) {
            $("panelBasket").style.display = "";
        }
        for (i = 0; i < arrchkBaskball.length; i++) {
            if ($(arrchkBaskball[i])) {
                $(arrchkBaskball[i]).disabled = chkCopyAll.checked;
            }
        }
        CheckCopyPT(chkCopyAll, arrSuper2, 'chkCopy2Hdp');
        CheckCopyPT(chkCopyAll, arrSuper3, 'chkCopy3Hdp');
        CheckCopyPT(chkCopyAll, arrSuper5, 'chkCopy5Hdp');
        CheckCopyPT(chkCopyAll, arrSuper10, 'chkCopy10Hdp');
        CheckCopyPT(chkCopyAll, arrSuper99, 'chkCopy99Hdp');
        CheckCopyPT(chkCopyAll, arrSuper8, 'chkCopy8Hdp');
        CheckCopyPT(chkCopyAll, arrSuper11, 'chkCopy11Hdp');

        CheckCopyPT(chkCopyAll, arrSuper99_M);
    }
    var flag = false;
    function BtnOnClick() {
        var isEdit = ($("hidEdit") != null ? true : false);
        if (!isEdit) {
            $("btnAdd").disabled = true;
        }
        else {
            $("btnSubmit").disabled = true;
        }
        if (!flag) {

            if (!CheckInfoValue(true)) return false;

            if (!CheckLimitMinMaxBetForMember(1)) return false;
            if ($("isMult").value == "1" || !$("chkCopyAll").checked) {
                if (!CheckLimitMinMaxBetForMember(2)) return false;
                if ($("isMult").value == "1" || !$("chkCopyBasket").checked) {
                    if ($('txtMaxPerBall')) {
                        if (!CheckLimitMaxPerBall()) return false;
                    }
                    if (!CheckLimitMinMaxBetForMember(3)) return false;
                    if (!CheckLimitMinMaxBetForMember(5)) return false;

                    if (!CheckLimitMinMaxBetForMember(8)) return false;
                    if (!CheckLimitMinMaxBetForMember(10)) return false;
                    if (!CheckLimitMinMaxBetForMember(11)) return false;

                    if (!CheckLimitMinMaxBetForMember(99)) return false;

                    if ($('txtMinbet99_M') || $('txtMaxbet99_M') || $('txtMaxPerMatch99_M')) {
                        if (!CheckLimitMinMaxBetForMember('99_M')) return false;
                    }
                }
            }
            if ($('txtMinbet161') || $('txtMaxbet161') || $('txtMaxPerMatch161')) {
                if (!CheckLimitMinMaxBetForMember(161)) return false;
            }
            flag = true;
            DelayReloadPage(null, 300000);
            enabledBtn(false);
            document.fUpdate.submit();
        }
        return true;
    }

    var arrComm = ['ddrCommissionH_OU_OE', 'ddrCommissionCS_TG', 'ddrCommission1x2', 'ddrCommissionumbergame'];

    var arrSoccerBet = ['txtMinbet1', 'txtMaxbet1', 'txtMaxPerMatch1'];
    var arrSoccerPT = ['listM1HdpD', 'listM11x2', 'listM1HdpL', 'chkCopy1HdpDeadball', 'chkCopy11x2', 'chkCopy1HdpLive'];

    var arrBasketballBet = ['txtMinbet2', 'txtMaxbet2', 'txtMaxPerMatch2'];
    var arrBasketballPT = ['listM2Hdp', 'chkCopy2Hdp'];

    var arrFootballBet = ['txtMinbet3', 'txtMaxbet3', 'txtMaxPerMatch3'];
    var arrFootballPT = ['listM3Hdp', 'chkCopy3Hdp'];

    var arrTennisBet = ['txtMinbet5', 'txtMaxbet5', 'txtMaxPerMatch5'];
    var arrTennisPT = ['listM5Hdp', 'chkCopy5Hdp'];

    var arrBaseballBet = ['txtMinbet8', 'txtMaxbet8', 'txtMaxPerMatch8'];
    var arrBaseballPT = ['listM8Hdp', 'chkCopy8Hdp'];

    var arrGolfBet = ['txtMinbet10', 'txtMaxbet10', 'txtMaxPerMatch10'];
    var arrGolfPT = ['listM10Hdp', 'chkCopy10Hdp'];

    var arrMotoBet = ['txtMinbet11', 'txtMaxbet11', 'txtMaxPerMatch11'];
    var arrMotoPT = ['listM11Hdp', 'chkCopy11Hdp'];

    var arrOtherBet = ['txtMinbet99', 'txtMaxbet99', 'txtMaxPerMatch99'];
    var arrOtherPT = ['listM99Hdp', 'chkCopy99Hdp'];

    var arrNumberGameBet = ['txtMinbet161', 'txtMaxbet161', 'txtMaxPerMatch161', 'txtMaxPerBall'];
    var arrNumberGamePT = ['listM1611stOUD', 'listM161OUL', 'chkCopy1611stOUD', 'chkCopy161OUL'];

    var arrMixedSportBet = ['txtMinbet99_M', 'txtMaxbet99_M', 'txtMaxPerMatch99_M'];
    var arrMixedSportPT = ['listM99_M'];

    function AllowUpdate(chkObj, isBet) {

        if (chkObj.id == 'chkUpdStatus') $('chkClosed').disabled = (chkObj.checked) ? false : true; //Status       


        if (chkObj.id == 'chkUpdComm') SetEnableDisable(arrComm, (chkObj.checked) ? false : true); //Commission
        if (chkObj.id == 'chkUpdSoccerBet') SetEnableDisable(arrSoccerBet, (chkObj.checked) ? false : true); //Soccer Bet
        if (chkObj.id == 'chkUpdSoccerPT') //Soccer PT
        {
            SetEnableDisable(arr1Super1, (chkObj.checked) ? (($('chkCopy1HdpDeadball').checked) ? true : false) : true);
            SetEnableDisable(arr2Super1, (chkObj.checked) ? (($('chkCopy11x2').checked) ? true : false) : true);
            SetEnableDisable(arr3Super1, (chkObj.checked) ? (($('chkCopy1HdpLive').checked) ? true : false) : true);
            SetEnableDisable(arrSoccerPT, (chkObj.checked) ? false : true);
            if ($("chkCopyAll").checked) {

                var elements_input = $("divUpdPT1").getElementsByTagName("input");
                for (var j = 0; j < elements_input.length; j++) {
                    var id = elements_input[j].id;
                    if (id == "chkAuto1HdpD" || id == "chkUpdSoccerPT") continue;
                    elements_input[j].disabled = true;
                }

                var elm = $("divUpdPT1").getElementsByTagName("select");
                for (var i = 0; i < elm.length; i++) {
                    var id = elm[i].id;
                    if (id == "listM1HdpD") {
                        if ($("chkUpdSoccerPT").checked) {
                            elm[i].disabled = false;
                        }
                        else continue;
                    }
                    if (id == "listM1HdpD" || id == "listS1HdpD" || id == "autoS1HdpD") continue;
                    elm[i].disabled = true;
                }
            }


        }

        if (chkObj.id == 'chkUpd2Bet') SetEnableDisable(arrBasketballBet, (chkObj.checked) ? false : true); //Basketball Bet				
        if (chkObj.id == 'chkUpd2PT') //Basketball PT
        {
            SetEnableDisable(arrBasketballPT, (chkObj.checked) ? false : true);
            SetEnableDisable(arrSuper2, (chkObj.checked) ? (($('chkCopy2Hdp').checked) ? true : false) : true);

            if ($("chkCopyBasket").checked) {
                var elm = $("divUpdPT2").getElementsByTagName("select");
                for (var i = 0; i < elm.length; i++) {
                    var id = elm[i].id;
                    if (id == "listM2Hdp") {
                        if ($("chkUpd2PT").checked) {
                            elm[i].disabled = false;
                        }
                        else continue;
                    }
                    if (id == "listM2Hdp" || id == "listS2Hdp" || id == "autoS2Hdp") continue;
                    elm[i].disabled = true;
                }

                var elements_input = $("divUpdPT2").getElementsByTagName("input");
                for (var j = 0; j < elements_input.length; j++) {
                    var id = elements_input[j].id;

                    if (id == "chkAuto2Hdp" || id == "chkUpd2PT") continue;
                    elements_input[j].disabled = true;
                }
            }
        }

        if (chkObj.id == 'chkUpd3Bet') SetEnableDisable(arrFootballBet, (chkObj.checked) ? false : true); //Football Bet
        if (chkObj.id == 'chkUpd3PT')//Football PT
        {
            SetEnableDisable(arrSuper3, (chkObj.checked) ? (($('chkCopy3Hdp').checked) ? true : false) : true);
            SetEnableDisable(arrFootballPT, (chkObj.checked) ? false : true);
        }

        if (chkObj.id == 'chkUpd5Bet') SetEnableDisable(arrTennisBet, (chkObj.checked) ? false : true); //Tennis Bet
        if (chkObj.id == 'chkUpd5PT')//Tennis PT
        {
            SetEnableDisable(arrSuper5, (chkObj.checked) ? (($('chkCopy5Hdp').checked) ? true : false) : true);
            SetEnableDisable(arrTennisPT, (chkObj.checked) ? false : true);
        }

        if (chkObj.id == 'chkUpd8Bet') SetEnableDisable(arrBaseballBet, (chkObj.checked) ? false : true); //Baseball Bet
        if (chkObj.id == 'chkUpd8PT')//Baseball PT
        {
            SetEnableDisable(arrSuper8, (chkObj.checked) ? (($('chkCopy8Hdp').checked) ? true : false) : true);
            SetEnableDisable(arrBaseballPT, (chkObj.checked) ? false : true);
        }

        if (chkObj.id == 'chkUpd10Bet') SetEnableDisable(arrGolfBet, (chkObj.checked) ? false : true); //Golf Bet
        if (chkObj.id == 'chkUpd10PT')//Golf PT
        {
            SetEnableDisable(arrSuper10, (chkObj.checked) ? (($('chkCopy10Hdp').checked) ? true : false) : true);
            SetEnableDisable(arrGolfPT, (chkObj.checked) ? false : true);
        }

        if (chkObj.id == 'chkUpd11Bet') SetEnableDisable(arrMotoBet, (chkObj.checked) ? false : true); //Moto Bet
        if (chkObj.id == 'chkUpd11PT')//Golf PT
        {
            SetEnableDisable(arrSuper11, (chkObj.checked) ? (($('chkCopy11Hdp').checked) ? true : false) : true);
            SetEnableDisable(arrMotoPT, (chkObj.checked) ? false : true);
        }

        if (chkObj.id == 'chkUpd99Bet') SetEnableDisable(arrOtherBet, (chkObj.checked) ? false : true); //Other Bet
        if (chkObj.id == 'chkUpd99PT')//Other PT
        {
            SetEnableDisable(arrSuper99, (chkObj.checked) ? (($('chkCopy99Hdp').checked) ? true : false) : true);
            SetEnableDisable(arrOtherPT, (chkObj.checked) ? false : true);
        }

        if (chkObj.id == 'chkUpd161Bet') SetEnableDisable(arrNumberGameBet, (chkObj.checked) ? false : true); //Number Game Bet				
        if (chkObj.id == 'chkUpd161PT') //Basketball PT
        {
            SetEnableDisable(arrNumberGamePT, (chkObj.checked) ? false : true);
            SetEnableDisable(arr161Super81, (chkObj.checked) ? (($('chkCopy1611stOUD').checked) ? true : false) : true);
            SetEnableDisable(arr161Super85, (chkObj.checked) ? (($('chkCopy161OUL').checked) ? true : false) : true);
        }

        if (chkObj.id == 'chkUpd99_MBet') SetEnableDisable(arrMixedSportBet, (chkObj.checked) ? false : true); //Other Bet
        if (chkObj.id == 'chkUpd99_MPT')//Other PT
        {
            SetEnableDisable(arrSuper99_M, (chkObj.checked) ? false : true);
            SetEnableDisable(arrMixedSportPT, (chkObj.checked) ? false : true);
        }
    }

    function SetEnableDisable(arr, disabled) {
        for (i = 0; i < arr.length; i++) {
            if ($(arr[i])) $(arr[i]).disabled = disabled;
        }
    }
    function GetUserList() {
        if (!AllowUdpPT) DisableBetPT();
        if (isMult == 0) return;
        var strCustid = parent.$("arrayCustID");

        if (strCustid) {
            if (strCustid.value != "") {
                $('txtIDs').value = strCustid.value.substring(0, strCustid.value.length - 1);
            }
        }
        var strUsername = parent.$("arrayUserName");

        if (strUsername) {
            if (strUsername.value != "") {
                var t = strUsername.value;
                while (t.indexOf("^") != -1) { t = t.replace("^", ", "); }
                if (t.length > 0) { if (t.substring(t.length - 2, t.length) == ", ") t = t.substring(0, t.length - 2); }
                $('username').innerHTML = t;
            }
        }

        //set disable all control	
        DisablePT();
    }

    function DisablePT() {
        //set disable all control	
        //info user			
        $('chkClosed').disabled = true;
        SetEnableDisable(arrComm, true);
        SetEnableDisable(arrSoccerBet, true);
        SetEnableDisable(arrBasketballBet, true);
        SetEnableDisable(arrFootballBet, true);
        SetEnableDisable(arrTennisBet, true);
        SetEnableDisable(arrBaseballBet, true);
        SetEnableDisable(arrGolfBet, true);
        SetEnableDisable(arrMotoBet, true);
        SetEnableDisable(arrOtherBet, true);
        SetEnableDisable(arrFootballBet, true);
        SetEnableDisable(arrNumberGameBet, true);
        SetEnableDisable(arrMixedSportBet, true);
        NotAllowPT();
    }

    function NotAllowPT() {
        //soccer				
        SetEnableDisable(arr1Super1, true);
        SetEnableDisable(arr2Super1, true);
        SetEnableDisable(arr3Super1, true);
        SetEnableDisable(arrSoccerPT, true);
        //Basketball				
        SetEnableDisable(arrBasketballPT, true);
        SetEnableDisable(arrSuper2, true);
        //Football				
        SetEnableDisable(arrFootballPT, true);
        SetEnableDisable(arrSuper3, true);
        //Tennis				
        SetEnableDisable(arrTennisPT, true);
        SetEnableDisable(arrSuper5, true);
        //Baseball				
        SetEnableDisable(arrBaseballPT, true);
        SetEnableDisable(arrSuper8, true);
        //Golf				
        SetEnableDisable(arrGolfPT, true);
        SetEnableDisable(arrSuper10, true);
        //Golf				
        SetEnableDisable(arrMotoPT, true);
        SetEnableDisable(arrSuper11, true);
        //Other				
        SetEnableDisable(arrOtherPT, true);
        SetEnableDisable(arrSuper99, true);
        //Number Game
        SetEnableDisable(arrNumberGamePT, true);
        SetEnableDisable(arr161Super81, true);
        SetEnableDisable(arr161Super85, true);
        //Mixed Sports
        SetEnableDisable(arrMixedSportPT, true);
        SetEnableDisable(arrSuper99_M, true);
    }
    function DisableBetPT() {
        var chk = ['chkUpdSoccerBet', 'chkUpdSoccerPT', 'chkUpd2Bet', 'chkUpd2PT', 'chkUpd3Bet', 'chkUpd3PT', 'chkUpd5Bet', 'chkUpd5PT', 'chkUpd8Bet', 'chkUpd8PT', 'chkUpd10Bet', 'chkUpd10PT', 'chkUpd11Bet', 'chkUpd11PT', 'chkUpd99Bet', 'chkUpd99PT', 'chkUpd161Bet', 'chkUpd161PT', 'chkUpd99_MBet', 'chkUpd99_MPT'];
        SetEnableDisable(chk, true);
    }

    
</script>
