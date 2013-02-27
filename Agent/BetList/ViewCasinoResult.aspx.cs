using System;
using System.Collections;
using System.Data;
using System.Text;
using CasinoLibrary.SBAgent;
using DNAide.Core;
using DNAide.Web;

public partial class BetList_ViewCasinoResult : TplPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "main";
        long refNo = CastAide.AsInt64(Request["ibcrefno"], 0);
        string strUsername = CastAide.AsString(Request["uname"]);

        int ibcCustID = CastAide.AsInt32(Request["ibccustid"], 0);
        DateTime dtmFDate = DateTime.Now;
        DateTime dtmTDate = DateTime.Now;
        dtmFDate = CastAide.AsDateTime(Request["fdate"]);
        dtmTDate = CastAide.AsDateTime(Request["tdate"]);

        string strHiddenField = string.Empty;

        string betType = CastAide.AsString(Request["bettype"]);
        string main = TemplateHandler.LoadTemplate("BetList/CasinoWinLoss.html");
        string noinfo = TemplateHandler.LoadTemplate("NoInfo.html");
        string content = string.Empty;
        string row = TemplateHandler.CopyBlock(main, "row");

        //Load language
        TemplateHandler.Assign(ref main, "lblStake", Resources.Lang.stake);
        TemplateHandler.Assign(ref main, "lblBet", Resources.Lang.lblBet);
        TemplateHandler.Assign(ref main, "lblWin", Resources.Lang.win);
        TemplateHandler.Assign(ref main, "lblResult", Resources.Lang.csresult);
        TemplateHandler.Assign(ref main, "lblGameID", Resources.Lang.lblGameID);
        TemplateHandler.Assign(ref main, "lblPlayerHistoryNo", Resources.Lang.lblPlayerHistoryNo);
        TemplateHandler.Assign(ref main, "lblGameType", Resources.Lang.lblGameType);
        TemplateHandler.Assign(ref main, "lblGameGroup", Resources.Lang.lblGameGroup);
        TemplateHandler.Assign(ref main, "lblStartDate", Resources.Lang.lblStartDate);
        TemplateHandler.Assign(ref main, "lblEndDate", Resources.Lang.lblEndDate);
        TemplateHandler.Assign(ref main, "lblStartBalance", Resources.Lang.lblStartBalance);
        TemplateHandler.Assign(ref main, "lblEndBalance", Resources.Lang.lblEndBalance);
        TemplateHandler.Assign(ref main, "lblRollingComm", Resources.Lang.lblRollingComm);
        string typeGame = CastAide.AsString(Request.QueryString["typegame"], "casinogame");
        TemplateHandler.Assign(ref main, "bettype", Resources.Lang.lblCasinoHandHistory);

        TemplateHandler.Assign(ref main, "Back", Resources.Lang.lblBack);
        TemplateHandler.Assign(ref main, "total", Resources.Lang.total);
        TemplateHandler.Assign(ref main, "lblSessionID", Resources.Lang.sessionID);
        TemplateHandler.Assign(ref main, "withPic", Resources.Lang.withPictures);
        TemplateHandler.Assign(ref main, "checked", (Request["showPic"] != null) ? " checked='checked'" : "");

        strHiddenField += Helper.GenHidden("_type", CastAide.AsString(Request["type"]));

        if (ibcCustID != 0)
            //This to get agent custid, to go back to Agent Real Time Win/Loss
            strHiddenField += Helper.GenHidden("_custid", CastAide.AsString(Request["acustid"]));
        else
            strHiddenField += Helper.GenHidden("_custid", CastAide.AsString(Request["custid"]));

        strHiddenField += Helper.GenHidden("_showSB", Request["showSB"]);
        strHiddenField += Helper.GenHidden("_showCasino", Request["showCasino"]);
        strHiddenField += Helper.GenHidden("_showP2P", Request["showP2P"]);
        strHiddenField += Helper.GenHidden("_showFI", Request["showFI"]);
        strHiddenField += Helper.GenHidden("_showRB", Request["showRB"]);
        strHiddenField += Helper.GenHidden("_fdate", Request["fdate"]);
        strHiddenField += Helper.GenHidden("_tdate", Request["tdate"]);
        strHiddenField += Helper.GenHidden("_isHistoryReport", Request["isHistoryReport"]);
        strHiddenField += Helper.GenHidden("BL_URL", Request["BL_URL"]);
        strHiddenField += Helper.GenHidden("_page", Request["page"]);
        strHiddenField += Helper.GenHidden("_getall", Request["getall"]);
        strHiddenField += Helper.GenHidden("_option", Request["option"]);
        strHiddenField += Helper.GenHidden("_username", strUsername);
        strHiddenField += Helper.GenHidden("_transtype", Request["transtype"]);

        TemplateHandler.Assign(ref main, "hiddenFields", strHiddenField);

        if (AppSettings.IsAgentSite)
        {
            TemplateHandler.Assign(ref main, "lblAG", Resources.Lang.lblAG + "%");
            TemplateHandler.Assign(ref main, "lblMA", "");
            TemplateHandler.Assign(ref main, "lblSA", "");
        }
        else if (AppSettings.IsMasterSite)
        {
            TemplateHandler.Assign(ref main, "lblSA", "");
            TemplateHandler.Assign(ref main, "lblMA", Resources.Lang.lblMA + "%");
            TemplateHandler.Assign(ref main, "lblAG", "<br>" + Resources.Lang.lblAG + "%");
        }
        else if (AppSettings.IsSuperSite)
        {
            TemplateHandler.Assign(ref main, "lblSA", Resources.Lang.lblSA + "%");
            TemplateHandler.Assign(ref main, "lblMA", "<br>" + Resources.Lang.lblMA + "%");
            TemplateHandler.Assign(ref main, "lblAG", "<br>" + Resources.Lang.lblAG + "%");
        }

        TemplateHandler.Assign(ref main, "head-clor-class", AppSettings.RowBgEven);
        // assign username
        TemplateHandler.Assign(ref main, "UserName", Resources.Lang.username + ": " + strUsername);

        //--Get data from CASINO API
        Report report = new Report();

        DataTable dataTable = null;

        // assign sessionid
        if (refNo != 0)
        {
            TemplateHandler.Assign(ref main, "RefNo", Resources.Lang.refno + ": " + refNo.ToString());
            dataTable = report.WinLossHandHistory(refNo, betType, Globals.CASINO_GROUP1, Request["showPic"] != null); // hardcode CasinoNo = 01 for tuning performance
        }
        else
        {
            TemplateHandler.Assign(ref main, "RefNo", "");
            if (ibcCustID != 0)
            {
                dataTable = report.RealTimeWinLossHandHistory(dtmFDate, dtmTDate, ibcCustID, Globals.CASINO_GROUP1, Request["showPic"] != null);// hardcode CasinoNo = 01 for tuning performance
            }
        }

        StringBuilder _s = new StringBuilder();
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            int intCount = 1;
            int nRow = 0;
            decimal dcBet = 0, dcWinLoss = 0, dcComm = 0, dcStake = 0;//, dcCompanyTotal = 0;
            int status = 0;
            string sID = CastAide.AsString(dataTable.Rows[0]["SessionLogID"]).Trim();
            string totalRow = TemplateHandler.CopyBlock(main, "totalRow");
            string tRow = string.Empty;
            foreach (DataRow r in dataTable.Rows)
            {
                nRow++;

                //Append total for session ID
                if (sID != CastAide.AsString(r["SessionLogID"]).Trim())
                {
                    tRow = totalRow;
                    TemplateHandler.Assign(ref tRow, "totalstake", Common.NumberReport(dcStake));
                    TemplateHandler.Assign(ref tRow, "totalbet", Common.NumberReport(dcBet));
                    TemplateHandler.Assign(ref tRow, "totalwinloss", Common.NumberReport(dcWinLoss));
                    TemplateHandler.Assign(ref tRow, "totalcomm", Common.NumberReport(dcComm));
                    TemplateHandler.Assign(ref tRow, "sID", sID);
                    _s.Append(tRow);
                    intCount = 1;

                    dcStake = 0;
                    dcBet = 0;
                    dcWinLoss = 0;
                    dcComm = 0;
                    sID = CastAide.AsString(r["SessionLogID"]).Trim();
                }

                string _row = row;
                TemplateHandler.Assign(ref _row, "no", intCount.ToString());
                // status=25 is broken but not finish, status=27 is broken and finish, status=20 is playing
                status = CastAide.AsInt32(r["Status"]);

                // ibcCustID != 0 ==> real time winloss
                // Assign SessionID on each row
                if (ibcCustID != 0)
                {
                    if (status != 20 && status != 27)
                    {
                        dcStake += CastAide.AsDecimal(r["stake"]);
                        dcBet += CastAide.AsDecimal(r["Bet"]);
                        dcWinLoss += CastAide.AsDecimal(r["WinLoss"]);
                        dcComm += CastAide.AsDecimal(r["RollComm"]);
                    }
                }
                else
                {
                    if (status != 25 && status != 27)
                    {
                        dcStake += CastAide.AsDecimal(r["stake"]);
                        dcBet += CastAide.AsDecimal(r["Bet"]);
                        dcWinLoss += CastAide.AsDecimal(r["WinLoss"]);
                        dcComm += CastAide.AsDecimal(r["RollComm"]);
                    }
                }

                // Tarzan game, God of prosperity, Chinese chess
                string strGameTypeNo = r["GameTypeNo"].ToString();

                bool isSpecialGame = false;
                if ((strGameTypeNo == "S0001") || (strGameTypeNo == "S0004") || (strGameTypeNo == "S0003") || (strGameTypeNo == "S0007") || (strGameTypeNo == "S0005"))
                {
                    isSpecialGame = true;
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetail2");
                }
                else if (strGameTypeNo == "S0008")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailJourneyWest");
                }
                else if (strGameTypeNo == "S0009")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailWinterSlot");
                }
                else if (strGameTypeNo == "S0010" || strGameTypeNo == "S0011")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailWesternZodiac");
                }
                else if (strGameTypeNo == "S0012")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailWaterMargin");
                }
                else if (strGameTypeNo == "S0013")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailChampion");
                }
                else if (strGameTypeNo == "S0014" || strGameTypeNo == "S0018")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailCrazyHospital");
                }
                else if (strGameTypeNo == "S0015")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailMahjongLegend");
                }
                else if (strGameTypeNo == "S0016")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailHalloween");
                }
                else if (strGameTypeNo == "S0019")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailEngagement2");
                }
                else if (strGameTypeNo == "S8001")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailDragonSlot");
                }
                else if (strGameTypeNo == "S8004")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailDinoCamp");
                }
                else if (strGameTypeNo == "S8005")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailTattooArt");
                }
                else if (strGameTypeNo == "S8006")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailMamakCorner");
                }
                else if (strGameTypeNo == "M0006" || strGameTypeNo == "S0006")//Mini Money Slot && Money Slot
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailMoneySlot");
                }
                else if (strGameTypeNo == "RA003" || strGameTypeNo == "RA002")//HiLoMahjong && HiLoMonkey
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailHiLoMahjong");
                }
                else if (strGameTypeNo == "RA004" || strGameTypeNo == "RA005")//Keno Word && Keno Number
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailKeno");
                }
                else if (strGameTypeNo == "RA006")//Horse Racing
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailHorseRacing");
                }
                else if (strGameTypeNo == "RH001")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailMayaPush");
                }
                else if (strGameTypeNo == "RH005")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailPenaltyHatTrick");
                }
                else if (strGameTypeNo == "RH006" || strGameTypeNo == "RH011")//Fancy Scratch && Fruit Scratch
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailScratch");
                }
                else if (strGameTypeNo == "RH007" || strGameTypeNo == "RH012")//HoHoHo Scratch && Olympic Scratch
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailHoHoHo");
                }
                else if (strGameTypeNo == "R0009" || strGameTypeNo == "R0022" || strGameTypeNo == "R0030" || strGameTypeNo == "R0027")//Mini Roulette && Poker Wheel && Red Dog
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailMiniRoulette");
                }
                else if (strGameTypeNo == "R0012")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailJacksOrBetter");
                }
                else if (strGameTypeNo == "R0019")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailMoneyWheel");
                }
                else if (strGameTypeNo == "R0021")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailFanTan");
                }
                else if (strGameTypeNo == "R0026")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailFaceUp");
                }

                else if (strGameTypeNo == "R0025")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailTexas");
                }
                else if (strGameTypeNo == "RH008" || strGameTypeNo == "RH010")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailKnockEggs");
                }

                else if (strGameTypeNo == "R0002")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailBattleWar");
                }

                else if (strGameTypeNo == "RH004")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailLoveStory");
                }

                else if (strGameTypeNo == "RH003")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetailGongXiFaChai");
                }
                else if (strGameTypeNo == "RV005")//4H JOB
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetail4H");
                }
                else if (strGameTypeNo == "RV002")//Ten or better
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewTenOrBetter");
                }
                else if (strGameTypeNo == "RV009" || strGameTypeNo == "RV010")
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetail25Line");
                }
                else
                {
                    TemplateHandler.Assign(ref _row, "viewFunc", "ViewDetail");
                }

                TemplateHandler.Assign(ref _row, "GameTypeNo", "'" + strGameTypeNo + "'");

                TemplateHandler.Assign(ref _row, "GameID", CastAide.AsString(r["GameID"]));
                TemplateHandler.Assign(ref _row, "PlayerHistoryNo", CastAide.AsString(r["PlayerHistoryNo"]));

                string gameTypeName = CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("GameTypeName")]);
                string strShoeName = string.Empty;

                if (isSpecialGame)
                {
                    string strFreeSpinName = Resources.Lang.freeSpin;
                    string strNormal = Resources.Lang.normal;
                    string strBonusFreeSpin = Resources.Lang.bonusFreeSpin;
                }

                TemplateHandler.Assign(ref _row, "GameType", gameTypeName);

                TemplateHandler.Assign(ref _row, "GameGroup", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("GameGroupName")]));
                TemplateHandler.Assign(ref _row, "StartDate", CastAide.AsString(r["StartTime"]));
                TemplateHandler.Assign(ref _row, "EndDate", CastAide.AsString(r["EndTime"]));
                TemplateHandler.Assign(ref _row, "Username", CastAide.AsString(r["Username"]));
                TemplateHandler.Assign(ref _row, "BeginBalance", Common.NumberReport(r["BeginBalance"]));
                TemplateHandler.Assign(ref _row, "EndBalance", Common.NumberReport(r["EndBalance"]));
                TemplateHandler.Assign(ref _row, "Bet", Common.NumberReport(r["Bet"]));
                TemplateHandler.Assign(ref _row, "Stake", Common.NumberReport(r["Stake"]));
                TemplateHandler.Assign(ref _row, "Win", Common.NumberReport(r["WinLoss"]));
                TemplateHandler.Assign(ref _row, "RollingComm", Common.NumberReport(r["RollComm"]));
                TemplateHandler.Assign(ref _row, "UserBetID", CastAide.AsString(r["UserBetID"]));
                TemplateHandler.Assign(ref _row, "Result", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("Result")]));

                if (AppSettings.IsAgentSite)
                {
                    TemplateHandler.Assign(ref _row, "AG", Common.NumberMoney(CastAide.AsDouble(r["AG%"]) * 100));
                    TemplateHandler.Assign(ref _row, "MA", "");
                    TemplateHandler.Assign(ref _row, "SA", "");
                }
                else if (AppSettings.IsMasterSite)
                {
                    TemplateHandler.Assign(ref _row, "AG", "<br>" + Common.NumberMoney(CastAide.AsDouble(r["AG%"]) * 100));
                    TemplateHandler.Assign(ref _row, "MA", Common.NumberMoney(CastAide.AsDouble(r["MA%"]) * 100));
                    TemplateHandler.Assign(ref _row, "SA", "");
                }
                else if (AppSettings.IsSuperSite)
                {
                    TemplateHandler.Assign(ref _row, "AG", "<br>" + Common.NumberMoney(CastAide.AsDouble(r["AG%"]) * 100));
                    TemplateHandler.Assign(ref _row, "MA", "<br>" + Common.NumberMoney(CastAide.AsDouble(r["MA%"]) * 100));
                    TemplateHandler.Assign(ref _row, "SA", Common.NumberMoney(CastAide.AsDouble(r["SH%"]) * 100));
                }

                TemplateHandler.Assign(ref _row, "clor-over", AppSettings.RowBgOver);
                if (nRow % 2 == 0)
                {
                    TemplateHandler.Assign(ref _row, "row-class", "even_row");
                    TemplateHandler.Assign(ref _row, "row-over", "live_row");
                    TemplateHandler.Assign(ref _row, "clor-class", AppSettings.RowBgEven); //AppSettings.Color
                }
                else
                {
                    TemplateHandler.Assign(ref _row, "row-class", "odd_row");
                    TemplateHandler.Assign(ref _row, "row-over", "live_row");
                    TemplateHandler.Assign(ref _row, "clor-class", AppSettings.RowBgOdd);
                }

                _s.Append(_row);
                intCount++;

                //end of the list
                if (nRow == dataTable.Rows.Count)
                {
                    tRow = totalRow;
                    TemplateHandler.Assign(ref tRow, "totalstake", Common.NumberReport(dcStake));
                    TemplateHandler.Assign(ref tRow, "totalbet", Common.NumberReport(dcBet));
                    TemplateHandler.Assign(ref tRow, "totalwinloss", Common.NumberReport(dcWinLoss));
                    TemplateHandler.Assign(ref tRow, "totalcomm", Common.NumberReport(dcComm));
                    TemplateHandler.Assign(ref tRow, "sID", sID);
                    _s.Append(tRow);
                }
            }
            content = _s.ToString();
        }
        else
        {
            TemplateHandler.Assign(ref noinfo, "noinfo", Resources.Lang.noInfo);
            TemplateHandler.Assign(ref noinfo, "num-cols", "15");
            content = noinfo;
        }

        TemplateHandler.RemoveBlock(ref main, "totalRow");
        TemplateHandler.ReplaceBlock(ref main, "row", content);

        this.TplSetContent(main);
    }

    public string RenderTable(DataTable dt)
    {
        string strRet = "<table border=1>";
        strRet += "<tr>";
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            strRet += "<td>" + dt.Columns[i].ColumnName + "</td>";
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            strRet += "<tr>";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                strRet += "<td>" + (dt.Rows[i][j] == null ? "null" : dt.Rows[i][j].ToString()) + "</td>";
            }
            strRet += "</tr>";
        }

        strRet += "</tr>";
        strRet += "</table><br />";

        return strRet;
    }
}