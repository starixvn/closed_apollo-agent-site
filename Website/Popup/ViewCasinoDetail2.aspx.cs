using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DNAide.Web;
using DNAide.Core;
using DNAide.Utils;
using CasinoLibrary.SBAgent;
using System.Text;

public partial class Popup_ViewCasinoDetail2 : TplPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "popup";
        int userBetID = CastAide.AsInt32(Request["userbetid"], 0);
        if (userBetID == 0) return;

        string gameTypeNo = Request["gametype"];

        string main = TemplateHandler.LoadTemplate("Popup/CasinoWinLossDetail2.html");

        if (gameTypeNo == "S0003" || gameTypeNo == "S0006" || gameTypeNo == "S0005")
        {
            TemplateHandler.RemoveBlock(ref main, "NotChineseChess");
        }

        TemplateHandler.Assign(ref main, "lblHeader", Resources.Lang.handhistorydetail);


        Report rpt = new Report();
        string typeGame = CastAide.AsString(Request.QueryString["typegame"], "casinogame");
        DataTable[] tableArray = rpt.WinLossDetail(userBetID);
        if (tableArray == null || tableArray[0] == null || tableArray[1] == null || tableArray[0].Rows.Count == 0 || tableArray[1].Rows.Count == 0)
        { 
            string noInfo = TemplateHandler.LoadTemplate("NoInfo.html");
            TemplateHandler.Assign(ref noInfo, "noinfo", Resources.Lang.noInfo);
            TemplateHandler.ReplaceBlock(ref main, "content", noInfo);
            this.TplSetContent(main);
            return;
        }

        DataTable dtResult = tableArray[0];
        DataTable dtSumWinloss = tableArray[1];


        TemplateHandler.Assign(ref main, "Player", Resources.Lang.player);
        TemplateHandler.Assign(ref main, "bet", Resources.Lang.lblBet);
        TemplateHandler.Assign(ref main, "winloss", Resources.Lang.win);

        // Total winloss
        DataRow r = dtSumWinloss.Rows[0];
        string strCurrency = r["Currency"] + " ";
        TemplateHandler.Assign(ref main, "bet_vl", strCurrency + DataFormat.FormatNumber20(r["Bet"]));

        decimal winloss_vl = 0;
        decimal.TryParse(r["WinLoss"] + "", out winloss_vl);

        string strWinloss_vl = DataFormat.FormatNumber20(winloss_vl);
        if (winloss_vl < 0) strWinloss_vl = "<font color='red'>" + strWinloss_vl + "</font>";

        TemplateHandler.Assign(ref main, "winloss_vl", strCurrency + strWinloss_vl);
        dtSumWinloss.Dispose();

        // Hit bet line
        StringBuilder hitRowBuilder = new StringBuilder(5000);
        string strBetLineHeader = TemplateHandler.CopyBlock(main, "hitBetLine");
        string strBetLineRow = TemplateHandler.CopyBlock(strBetLineHeader, "row");
        string strRollingReel = TemplateHandler.CopyBlock(strBetLineHeader, "rowRollingReel");
        string strMultiplier = TemplateHandler.CopyBlock(strBetLineHeader, "multiplier");

        TemplateHandler.RemoveBlock(ref strBetLineHeader, "row");
        TemplateHandler.RemoveBlock(ref strBetLineHeader, "rowRollingReel");
        TemplateHandler.RemoveBlock(ref strBetLineHeader, "multiplier");

        // Hit bet line for Chinese chess
        string strHitBetLineChessHeader = TemplateHandler.CopyBlock(main, "hitBetLineChess");
        string strBetLineChessRow = TemplateHandler.CopyBlock(strHitBetLineChessHeader, "row");
        TemplateHandler.RemoveBlock(ref strHitBetLineChessHeader, "row");
        TemplateHandler.RemoveBlock(ref main, "hitBetLineChess");

        // Hit bet line for Classic Slot
        string strHitBetLineClassicSlotHeader = TemplateHandler.CopyBlock(main, "hitBetLineClassicSlot");
        string strBetLineClassicSlotRow = TemplateHandler.CopyBlock(strHitBetLineClassicSlotHeader, "row");
        TemplateHandler.RemoveBlock(ref strHitBetLineClassicSlotHeader, "row");
        TemplateHandler.RemoveBlock(ref main, "hitBetLineClassicSlot");
        if (gameTypeNo == "S0006")
        {
            main = main.Replace(" colspan=\"6\"", " colspan=\"7\"");
            main = main.Replace(" colspan=\"5\"", " colspan=\"6\"");
        }


        string strHitBetLineChessHeaderSide = TemplateHandler.CopyBlock(main, "hitBetLineChessSide");
        string strBetLineChessRowSide = TemplateHandler.CopyBlock(strHitBetLineChessHeaderSide, "row");
        TemplateHandler.RemoveBlock(ref strHitBetLineChessHeaderSide, "row");
        TemplateHandler.RemoveBlock(ref main, "hitBetLineChessSide");


        // Assign information from tableArray[0] base on ShowType field
        int c = dtResult.Rows.Count;
        string showType;
        string strTmpBetLineHeader = string.Empty;
        for (int i = 0; i < c; i++)
        {
            r = dtResult.Rows[i];
            showType = r["ShowType"] as string;
            switch (showType)
            {
                case "BasicInfo":
                    TemplateHandler.Assign(ref main, "tablename", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("TableName")]));
                    TemplateHandler.Assign(ref main, "datetime", CastAide.AsString(r["BetTime"]));
                    TemplateHandler.Assign(ref main, "UserName", CastAide.AsString(r["UserName"]));
                    TemplateHandler.Assign(ref main, "Result", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("ResultName")]));
                    TemplateHandler.Assign(ref main, "result_vl", CastAide.AsString(r["Result"]));

                    if (gameTypeNo == "S0006") // Display PlayHistoryNo
                    {
                        TemplateHandler.Assign(ref main, "historyNo", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("PlayerHistoryNo")]));
                        TemplateHandler.Assign(ref main, "historyNo_vl", CastAide.AsString(r["PlayerHistoryNo_Result"]));
                    }

                    break;
                case "PlayHistoryNo":
                    TemplateHandler.Assign(ref main, "historyNo", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("PlayerHistoryNo")]));
                    TemplateHandler.Assign(ref main, "historyNo_vl", CastAide.AsString(r["PlayerHistoryNo_Result"]));
                    break;

                case "Mode":
                    TemplateHandler.Assign(ref main, "mode", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("Mode")]));
                    TemplateHandler.Assign(ref main, "mode_vl", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("ModeResult")]));
                    break;
                case "AutoSpin":
                    TemplateHandler.Assign(ref main, "autoSpin", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("AutoSpin")]));
                    TemplateHandler.Assign(ref main, "autoSpin_vl", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("AutoSpinResult")]));
                    break;
                case "FreeSpin":
                    TemplateHandler.Assign(ref main, "freeSpin", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("FreeSpin")]));
                    TemplateHandler.Assign(ref main, "freeSpin_vl", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("FreeSpinResult")]));
                    break;
                case "AddRow_HitBetLine":


                    // For Classic Game
                    if (gameTypeNo == "S0006")
                    {
                        strTmpBetLineHeader = strHitBetLineClassicSlotHeader;

                        TemplateHandler.Assign(ref strTmpBetLineHeader, "winType", Resources.Lang.wintype);
                        TemplateHandler.Assign(ref strTmpBetLineHeader, "symbol", Resources.Lang.symbol);
                        TemplateHandler.Assign(ref strTmpBetLineHeader, "coinSize", Resources.Lang.coinsize);
                        TemplateHandler.Assign(ref strTmpBetLineHeader, "bet", Resources.Lang.bet);
                        TemplateHandler.Assign(ref strTmpBetLineHeader, "odds", Resources.Lang.odds);
                        TemplateHandler.Assign(ref strTmpBetLineHeader, "multiplier", Resources.Lang.multiplier);
                        TemplateHandler.Assign(ref strTmpBetLineHeader, "win", Resources.Lang.lblWin);

                        hitRowBuilder.Append(strTmpBetLineHeader);

                        break;
                    }



                    // Copy and add new header
                    strTmpBetLineHeader = strBetLineHeader;

                    TemplateHandler.Assign(ref strTmpBetLineHeader, "lineNo", Resources.Lang.lineno);
                    TemplateHandler.Assign(ref strTmpBetLineHeader, "symbol", Resources.Lang.symbol);
                    TemplateHandler.Assign(ref strTmpBetLineHeader, "coinSize", Resources.Lang.coinsize);
                    TemplateHandler.Assign(ref strTmpBetLineHeader, "betPerLine", Resources.Lang.betperline);
                    TemplateHandler.Assign(ref strTmpBetLineHeader, "odds", Resources.Lang.odds);
                    TemplateHandler.Assign(ref strTmpBetLineHeader, "win", Resources.Lang.lblWin);

                    hitRowBuilder.Append(strTmpBetLineHeader);

                    break;
                case "Result_HitBetLine":


                    // For Classic Game
                    if (gameTypeNo == "S0006")
                    {
                        strTmpBetLineHeader = strBetLineClassicSlotRow;

                        TemplateHandler.Assign(ref strTmpBetLineHeader, "winType_vl", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("ResultName")]));
                        TemplateHandler.Assign(ref strTmpBetLineHeader, "symbol_vl", CastAide.AsString(r["Symbol"]));
                        TemplateHandler.Assign(ref strTmpBetLineHeader, "coinSize_vl", CastAide.AsString(r["CoinSize"]));
                        TemplateHandler.Assign(ref strTmpBetLineHeader, "bet_vl", CastAide.AsString(r["Bet"]));
                        TemplateHandler.Assign(ref strTmpBetLineHeader, "odds_vl", CastAide.AsString(r["Odds"]));
                        TemplateHandler.Assign(ref strTmpBetLineHeader, "multiplier_vl", CastAide.AsString(r["Multiplier"]));
                        TemplateHandler.Assign(ref strTmpBetLineHeader, "win_vl", CastAide.AsString(r["Win"]));

                        hitRowBuilder.Append(strTmpBetLineHeader);

                        break;
                    }



                    // For Tarzan game

                    // Copy and add new row
                    string strTmpBetLineRow = strBetLineRow;

                    TemplateHandler.Assign(ref strTmpBetLineRow, "lineNo_vl", CastAide.AsString(r["LineNo"]));
                    TemplateHandler.Assign(ref strTmpBetLineRow, "symbol_vl", CastAide.AsString(r["Symbol"]));
                    TemplateHandler.Assign(ref strTmpBetLineRow, "coinSize_vl", CastAide.AsString(r["CoinSize"]));
                    TemplateHandler.Assign(ref strTmpBetLineRow, "betPerLine_vl", CastAide.AsString(r["BetPerLine"]));
                    TemplateHandler.Assign(ref strTmpBetLineRow, "odds_vl", CastAide.AsString(r["Odds"]));
                    TemplateHandler.Assign(ref strTmpBetLineRow, "win_vl", CastAide.AsString(r["Win"]));

                    hitRowBuilder.Append(strTmpBetLineRow);


                    // For God of Prosperity and Chinese Chess
                    // Same as Tarzan

                    break;
                case "AddRow_HitScatter":
                    // Copy and add new header
                    strTmpBetLineHeader = strBetLineHeader;

                    TemplateHandler.Assign(ref strTmpBetLineHeader, "lineNo", Resources.Lang.totallinebet);
                    TemplateHandler.Assign(ref strTmpBetLineHeader, "symbol", Resources.Lang.symbol);
                    TemplateHandler.Assign(ref strTmpBetLineHeader, "coinSize", Resources.Lang.coinsize);
                    TemplateHandler.Assign(ref strTmpBetLineHeader, "betPerLine", Resources.Lang.betperline);
                    TemplateHandler.Assign(ref strTmpBetLineHeader, "odds", Resources.Lang.odds);
                    TemplateHandler.Assign(ref strTmpBetLineHeader, "win", Resources.Lang.lblWin);

                    hitRowBuilder.Append(strTmpBetLineHeader);

                    break;
                case "Result_HitScatter":

                    if (gameTypeNo == "S0001" || gameTypeNo == "S0007" || gameTypeNo == "S0004") // Tarzan
                    {
                        // Copy and add new row
                        strTmpBetLineRow = strBetLineRow;

                        TemplateHandler.Assign(ref strTmpBetLineRow, "lineNo_vl", CastAide.AsString(r["TotalLineBet"]));
                        TemplateHandler.Assign(ref strTmpBetLineRow, "symbol_vl", CastAide.AsString(r["Symbol"]));
                        TemplateHandler.Assign(ref strTmpBetLineRow, "coinSize_vl", CastAide.AsString(r["CoinSize"]));
                        TemplateHandler.Assign(ref strTmpBetLineRow, "betPerLine_vl", CastAide.AsString(r["BetPerLine"]));
                        TemplateHandler.Assign(ref strTmpBetLineRow, "odds_vl", CastAide.AsString(r["Odds"]));
                        TemplateHandler.Assign(ref strTmpBetLineRow, "win_vl", CastAide.AsString(r["Win"]));

                        hitRowBuilder.Append(strTmpBetLineRow);

                    }
                    else if (gameTypeNo == "S0003") // Chinese Chess
                    {
                        // Copy and add new row
                        strTmpBetLineRow = strBetLineChessRow;

                        TemplateHandler.Assign(ref strTmpBetLineRow, "col1_vl", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("ActionName")]));
                        TemplateHandler.Assign(ref strTmpBetLineRow, "col2_vl", CastAide.AsString(r["Symbol"]));
                        TemplateHandler.Assign(ref strTmpBetLineRow, "col3_vl", CastAide.AsString(r["Odds"]));
                        TemplateHandler.Assign(ref strTmpBetLineRow, "col4_vl", CastAide.AsString(r["Multiplier_Result"]));
                        TemplateHandler.Assign(ref strTmpBetLineRow, "col5_vl", CastAide.AsString(r["Win"]));

                        hitRowBuilder.Append(strTmpBetLineRow);

                    }

                    break;

                case "AddRow_RollingReel":
                    // Copy and add new row
                    string strTmpRollingReel = strRollingReel;

                    TemplateHandler.Assign(ref strTmpRollingReel, "rollingReel", Resources.Lang.rollingReel);

                    hitRowBuilder.Append(strTmpRollingReel);

                    break;
                case "MaxMultiply":
                    // Copy and add new row
                    string strTmpMultiplier = strMultiplier;

                    TemplateHandler.Assign(ref strTmpMultiplier, "multiplier", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("Multiplier")]));
                    TemplateHandler.Assign(ref strTmpMultiplier, "multiplier_vl", CastAide.AsString(r["Multiplier_Result"]));

                    hitRowBuilder.Append(strTmpMultiplier);
                    break;


                #region Chinese Chess

                case "AddRow_Normal_HitBetLine":
                    // Copy and add new header
                    strTmpBetLineHeader = strBetLineHeader;

                    TemplateHandler.Assign(ref strTmpBetLineHeader, "lineNo", Resources.Lang.lineno);
                    TemplateHandler.Assign(ref strTmpBetLineHeader, "symbol", Resources.Lang.symbol);
                    TemplateHandler.Assign(ref strTmpBetLineHeader, "coinSize", Resources.Lang.coinsize);
                    TemplateHandler.Assign(ref strTmpBetLineHeader, "betPerLine", Resources.Lang.betperline);
                    TemplateHandler.Assign(ref strTmpBetLineHeader, "odds", Resources.Lang.odds);
                    TemplateHandler.Assign(ref strTmpBetLineHeader, "win", Resources.Lang.perLineWin);

                    hitRowBuilder.Append(strTmpBetLineHeader);
                    break;

                case "AddRow_NormalGame":
                    // Copy and add new row
                    string strTmpNormalGame = strRollingReel;

                    TemplateHandler.Assign(ref strTmpNormalGame, "rollingReel", Resources.Lang.normalGame);

                    hitRowBuilder.Append(strTmpNormalGame);

                    break;


                case "AddRow_ScatterGame":
                    // Copy and add new row
                    string strTmpScatterGame = strRollingReel;

                    TemplateHandler.Assign(ref strTmpScatterGame, "rollingReel", Resources.Lang.scatterGame);

                    hitRowBuilder.Append(strTmpScatterGame);

                    break;

                case "AddRow_BonusGame":
                    // Copy and add new row
                    string strTmpBonusGame = strRollingReel;

                    TemplateHandler.Assign(ref strTmpBonusGame, "rollingReel", Resources.Lang.bonusGame);

                    hitRowBuilder.Append(strTmpBonusGame);

                    break;

                case "AddRow_Scatter_HitBetLine":
                    string strTmpBetLineChessHeader = strHitBetLineChessHeader;

                    TemplateHandler.Assign(ref strTmpBetLineChessHeader, "col1", Resources.Lang.name);
                    TemplateHandler.Assign(ref strTmpBetLineChessHeader, "col2", Resources.Lang.symbol);
                    TemplateHandler.Assign(ref strTmpBetLineChessHeader, "col3", Resources.Lang.odds);
                    TemplateHandler.Assign(ref strTmpBetLineChessHeader, "col4", Resources.Lang.multiplier);
                    TemplateHandler.Assign(ref strTmpBetLineChessHeader, "col5", Resources.Lang.scatterWin);

                    hitRowBuilder.Append(strTmpBetLineChessHeader);

                    break;

                case "AddRow_Bonus_HitBetLine":
                    strTmpBetLineChessHeader = strHitBetLineChessHeader;

                    TemplateHandler.RemoveBlock(ref strTmpBetLineChessHeader, "row5");
                    TemplateHandler.Assign(ref strTmpBetLineChessHeader, "colspan", 2);

                    TemplateHandler.Assign(ref strTmpBetLineChessHeader, "col1", Resources.Lang.bonusNo);
                    TemplateHandler.Assign(ref strTmpBetLineChessHeader, "col2", Resources.Lang.symbol);
                    TemplateHandler.Assign(ref strTmpBetLineChessHeader, "col3", Resources.Lang.actionDesc);
                    TemplateHandler.Assign(ref strTmpBetLineChessHeader, "col4", Resources.Lang.reward);

                    hitRowBuilder.Append(strTmpBetLineChessHeader);

                    break;

                case "Result_HitBonusGame":
                    string strTmpBetLineChessRow = strBetLineChessRow;

                    TemplateHandler.RemoveBlock(ref strTmpBetLineChessRow, "row5");
                    TemplateHandler.Assign(ref strTmpBetLineChessRow, "colspan", 2);

                    TemplateHandler.Assign(ref strTmpBetLineChessRow, "col1_vl", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("ActionName")]));
                    TemplateHandler.Assign(ref strTmpBetLineChessRow, "col2_vl", CastAide.AsString(r["Symbol"]));
                    TemplateHandler.Assign(ref strTmpBetLineChessRow, "col3_vl", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("Descript")]));
                    TemplateHandler.Assign(ref strTmpBetLineChessRow, "col4_vl", CastAide.AsString(r["Contents_Result"]));

                    hitRowBuilder.Append(strTmpBetLineChessRow);

                    break;

                case "AddRow_SideGame":
                    // Copy and add new row
                    string strTmpSideGame = strRollingReel;

                    TemplateHandler.Assign(ref strTmpSideGame, "rollingReel", Resources.Lang.sideGame);

                    hitRowBuilder.Append(strTmpSideGame);

                    break;

                case "AddRow_Side_HitBetLine":
                    strTmpBetLineChessHeader = strHitBetLineChessHeaderSide;

                    TemplateHandler.Assign(ref strTmpBetLineChessHeader, "col1", Resources.Lang.name);
                    TemplateHandler.Assign(ref strTmpBetLineChessHeader, "col2", Resources.Lang.result);
                    TemplateHandler.Assign(ref strTmpBetLineChessHeader, "col3", Resources.Lang.sideGameWin);

                    hitRowBuilder.Append(strTmpBetLineChessHeader);

                    break;

                case "Result_HitSideGame":
                    strTmpBetLineChessRow = strBetLineChessRowSide;

                    TemplateHandler.Assign(ref strTmpBetLineChessRow, "col1_vl", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("ActionName")]));
                    TemplateHandler.Assign(ref strTmpBetLineChessRow, "col2_vl", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("ActionResult")]));
                    TemplateHandler.Assign(ref strTmpBetLineChessRow, "col3_vl", CastAide.AsString(r["Win"]));

                    hitRowBuilder.Append(strTmpBetLineChessRow);

                    break;

                case "Result_SubWin":
                    // Copy and add new row
                    strTmpMultiplier = strMultiplier;

                    TemplateHandler.Assign(ref strTmpMultiplier, "multiplier", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("ActionName")]));
                    TemplateHandler.Assign(ref strTmpMultiplier, "multiplier_vl", CastAide.AsString(r["Win"]));

                    hitRowBuilder.Append(strTmpMultiplier);


                    break;

                #endregion

                default:
                    // Handle exception
                    break;
            }
        }

        dtResult.Dispose();

        TemplateHandler.ReplaceBlock(ref main, "hitBetLine", hitRowBuilder.ToString());

        this.TplSetContent(main);
    }


    public string RenderTable(DataTable dt)
    {
        string strRet = "<table border=1>";
        strRet += "<tr>";
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            strRet += "<td style='font-weight: bold'>" + dt.Columns[i].ColumnName + "</td>";
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            strRet += "<tr>";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                strRet += "<td>" + ((string.IsNullOrEmpty(dt.Rows[i][j].ToString())) ? "null" : dt.Rows[i][j].ToString()) + "</td>";
            }
            strRet += "</tr>";
        }

        strRet += "</tr>";
        strRet += "</table><br />";

        return strRet;
    }
}
