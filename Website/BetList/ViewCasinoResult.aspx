<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewCasinoResult.aspx.cs" Inherits="BetList_ViewCasinoResult" %>

<script type="text/javascript" src="../../_Components/AGEWnd/AGEWnd.js?<%= AppSettings.JsVersion %>"></script>
<script type="text/javascript">
    function ViewDetail(UserBetID, GameTypeNo) { // For normal game

        var width = 505;
        var params = '?userbetid=' + UserBetID
        if (parent.ageWnd)
            parent.ageWnd.open('../Popup/ViewCasinoDetail.aspx' + params, '', 50, 100, width, 550);
        else
            ageWnd.open('../Popup/ViewCasinoDetail.aspx' + params, '', 50, 100, width, 550);
    }

    function ViewDetail2(UserBetID, GameTypeNo) { // For tarzan, god prosperity and chinese chess

        var width = 700;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../Popup/ViewCasinoDetail2.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../Popup/ViewCasinoDetail2.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewDetailJourneyWest(UserBetID, GameTypeNo) { // For JourneyWest game

        var width = 700;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_JournetWest.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_JournetWest.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewDetailMoneyWheel(UserBetID, GameTypeNo) // For Mone Wheel game
    {
        var width = 700;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_MoneyWheel.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_MoneyWheel.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewDetailFanTan(UserBetID, GameTypeNo) // For FanTan game
    {

        var width = 500;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_FanTan.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_FanTan.aspx' + params, '', 50, 100, width, 600);
    }

    function BackToBetList() {
        var params = '?type=' + $('_type').value + '&custid=' + $('_custid').value + '&showSB=' + $('_showSB').value + '&showCasino=' + $('_showCasino').value + '&showP2P=' + $('_showP2P').value + '&showFI=' + $('_showFI').value + '&showRB=' + $('_showRB').value + '&fdate=' + $('_fdate').value + '&tdate=' + $('_tdate').value + '&isHistoryReport=' + $('_isHistoryReport').value + '&page=' + $('_page').value + '&username=' + $('_username').value + '&getall=' + $('_getall').value + '&option=' + $('_option').value + '&transtype=' + $('_transtype').value;
        var url = $('BL_URL').value + params;
        window.location.href = url;
    }

    function ShowPic(b) {
        var url = location.href;
        var i = url.indexOf('&showPic=');
        if (i != -1) {
            location.href = url.substr(0, i);
        }
        else {
            location.href += '&showPic=' + b;
        }
    }

    function ViewDetailWinterSlot(UserBetID, GameTypeNo) { // For WinterSlot game

        var width = 700;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_WinterSlot.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_WinterSlot.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewDetailHiLoMahjong(UserBetID, GameTypeNo) { // For HiLoMahjong game

        var width = 500;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_HiLoMahjong.aspx' + params, '', 50, 100, width, 570);
        else
            ageWnd.open('../_BetList/CasinoResult_HiLoMahjong.aspx' + params, '', 50, 100, width, 570);
    }

    function ViewDetailKeno(UserBetID, GameTypeNo) { // For Keno game

        var width = 500;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_Keno.aspx' + params, '', 50, 100, width, 570);
        else
            ageWnd.open('../_BetList/CasinoResult_Keno.aspx' + params, '', 50, 100, width, 570);
    }

    function ViewDetailScratch(UserBetID, GameTypeNo) { // For Keno game

        var width = 500;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_Scratch.aspx' + params, '', 50, 100, width, 570);
        else
            ageWnd.open('../_BetList/CasinoResult_Scratch.aspx' + params, '', 50, 100, width, 570);
    }

    function ViewDetailFaceUp(UserBetID, GameTypeNo) {

        var width = 500;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_FaceUp.aspx' + params, '', 50, 100, width, 570);
        else
            ageWnd.open('../_BetList/CasinoResult_FaceUp.aspx' + params, '', 50, 100, width, 570);
    }

    function ViewDetailKnockEggs(UserBetID, GameTypeNo) {

        var width = 500;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_KnockEggs.aspx' + params, '', 50, 100, width, 570);
        else
            ageWnd.open('../_BetList/CasinoResult_KnockEggs.aspx' + params, '', 50, 100, width, 570);
    }

    function ViewDetailTexas(UserBetID, GameTypeNo) {

        var width = 500;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_TexasHoldem.aspx' + params, '', 50, 100, width, 570);
        else
            ageWnd.open('../_BetList/CasinoResult_TexasHoldem.aspx' + params, '', 50, 100, width, 570);
    }

    function ViewDetailBattleWar(UserBetID, GameTypeNo) {

        var width = 500;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_BattleWar.aspx' + params, '', 50, 100, width, 570);
        else
            ageWnd.open('../_BetList/CasinoResult_BattleWar.aspx' + params, '', 50, 100, width, 570);
    }

    function ViewDetailLoveStory(UserBetID, GameTypeNo) {

        var width = 500;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_LoveStoryScratch.aspx' + params, '', 50, 100, width, 570);
        else
            ageWnd.open('../_BetList/CasinoResult_LoveStoryScratch.aspx' + params, '', 50, 100, width, 570);
    }

    function ViewDetailHoHoHo(UserBetID, GameTypeNo) {

        var width = 400;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_HoHoHoScratch.aspx' + params, '', 50, 100, width, 500);
        else
            ageWnd.open('../_BetList/CasinoResult_HoHoHoScratch.aspx' + params, '', 50, 100, width, 500);
    }

    function ViewDetailMiniRoulette(UserBetID, GameTypeNo) {

        var width = 550;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_MiniRoulette.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_MiniRoulette.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewDetailGongXiFaChai(UserBetID, GameTypeNo) {

        var width = 500;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_GongXiFaChai.aspx' + params, '', 50, 100, width, 570);
        else
            ageWnd.open('../_BetList/CasinoResult_GongXiFaChai.aspx' + params, '', 50, 100, width, 570);
    }

    function ViewDetail4H(UserBetID, GameTypeNo) {

        var width = 650;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_4hJOB.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_4hJOB.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewDetailWesternZodiac(UserBetID, GameTypeNo) {

        var width = 700;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_WesternZodiac.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_WesternZodiac.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewDetailHalloween(UserBetID, GameTypeNo) {

        var width = 700;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_Halloween.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_Halloween.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewDetailCrazyHospital(UserBetID, GameTypeNo) {

        var width = 700;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_CrazyHospital.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_CrazyHospital.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewDetailEngagement2(UserBetID, GameTypeNo) {

        var width = 700;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_Engagement2.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_Engagement2.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewTenOrBetter(UserBetID, GameTypeNo) {

        var width = 500;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_TenOrBetter.aspx' + params, '', 50, 100, width, 570);
        else
            ageWnd.open('../_BetList/CasinoResult_TenOrBetter.aspx' + params, '', 50, 100, width, 570);
    }

    function ViewDetailTattooArt(UserBetID, GameTypeNo) {

        var width = 550;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_TattooArt.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_TattooArt.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewDetailMoneySlot(UserBetID, GameTypeNo) {

        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_MoneySlot.aspx' + params, '', 50, 100, 700, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_MoneySlot.aspx' + params, '', 50, 100, 700, 600);
    }

    function ViewDetailHorseRacing(UserBetID, GameTypeNo) {

        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_HorseRacing.aspx' + params, '', 50, 100, 550, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_HorseRacing.aspx' + params, '', 50, 100, 550, 600);
    }

    function ViewDetailChampion(UserBetID, GameTypeNo) {

        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_Champion.aspx' + params, '', 50, 100, 550, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_Champion.aspx' + params, '', 50, 100, 550, 600);
    }

    function ViewDetailDragonSlot(UserBetID, GameTypeNo) {

        var width = 550;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_DragonSlot.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_DragonSlot.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewDetail25Line(UserBetID, GameTypeNo) {

        var width = 650;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_MultiJOB.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_MultiJOB.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewDetailWaterMargin(UserBetID, GameTypeNo) {

        var width = 700;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_WaterMargin.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_WaterMargin.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewDetailPenaltyHatTrick(UserBetID, GameTypeNo) {

        var width = 400;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_PenaltyHatTrick.aspx' + params, '', 50, 100, width, 500);
        else
            ageWnd.open('../_BetList/CasinoResult_PenaltyHatTrick.aspx' + params, '', 50, 100, width, 500);
    }

    function ViewDetailJacksOrBetter(UserBetID, GameTypeNo) {
        var width = 505;
        var params = '?userbetid=' + UserBetID
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_JacksOrBetter.aspx' + params, '', 50, 100, width, 550);
        else
            ageWnd.open('../_BetList/CasinoResult_JacksOrBetter.aspx' + params, '', 50, 100, width, 550);
    }

    function ViewDetailMahjongLegend(UserBetID, GameTypeNo) {

        var width = 700;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_MahjongLegend.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_MahjongLegend.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewDetailDinoCamp(UserBetID, GameTypeNo) {

        var width = 550;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_DinoCamp.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_DinoCamp.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewDetailMamakCorner(UserBetID, GameTypeNo) {

        var width = 550;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_MamakCorner.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_MamakCorner.aspx' + params, '', 50, 100, width, 600);
    }

    function ViewDetailMayaPush(UserBetID, GameTypeNo) {

        var width = 550;
        var params = '?userbetid=' + UserBetID + '&gametype=' + GameTypeNo;
        if (parent.ageWnd)
            parent.ageWnd.open('../_BetList/CasinoResult_MayaPush.aspx' + params, '', 50, 100, width, 600);
        else
            ageWnd.open('../_BetList/CasinoResult_MayaPush.aspx' + params, '', 50, 100, width, 600);
    }
</script>