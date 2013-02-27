using System;
using System.Collections.Generic;
using System.Web;
using Fanex.Age.UI;

using CasinoLibrary.SBAgent;
using System.Data;
using Sunplus.Agent.Model.BL.Casino;
using Sunplus.Agent.Utilities;
using Fanex.Age.UI.Components;

namespace Sunplus.Agent.UI._BetList
{
    public partial class CasinoResult_JournetWest_Page : TplPage
    {
        int userBetId = 0;
        string gameTypeNo = null;

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            DataTable[] ds = View as DataTable[];
            DataTable dtTotal = ds[1];
            DataTable dtDetail = ds[0];

            CasinoResult_JourneyWest_Template tpl = new CasinoResult_JourneyWest_Template();


            tpl.totalBet = String.Join(" ", new string[] { dtTotal.Rows[0]["Currency"].ToString(), NumberReport(dtTotal.Rows[0]["Bet"]) });
            tpl.totalWin = String.Join(" ", new string[] { dtTotal.Rows[0]["Currency"].ToString(), NumberReport(dtTotal.Rows[0]["WinLoss"]) });

            //writer.Write(RenderTable(dtTotal));
            //writer.Write(RenderTable(dtDetail));

            tpl.NormalGame.Clear();
            tpl.ScatterGame.Clear();
            tpl.ExperienceGame.Clear();
            tpl.JackpotGame.Clear();
            tpl.KidnappingGame.Clear();


            DataRowCollection rows = dtDetail.Rows;
            int c = rows.Count;
            for (int i = 0; i < c; i++)
            {
                string showType = rows[i]["ShowType"] as string;
                if (showType == "BasicInfo")
                {
                    RenderBasicInfo(tpl, dtDetail);
                }
                else if (showType == "AddRow_NormalGame") // Enter Normal Game rendering
                {
                    CasinoResult_JourneyWest_NormalGame_Block normalGame = new CasinoResult_JourneyWest_NormalGame_Block();
                    RenderNormalGame(normalGame, rows, ref i, c);
                    tpl.NormalGame.Append(normalGame);
                }
                else if (showType == "AddRow_ScatterGame") // Enter Scatter Game rendering
                {
                    CasinoResult_JourneyWest_ScatterGame_Block scatterGame = new CasinoResult_JourneyWest_ScatterGame_Block();
                    RenderScatterGame(scatterGame, rows, ref i, c);
                    tpl.ScatterGame.Append(scatterGame);
                }
                else if (showType == "AddRow_ExperienceGame") // Enter Experience Game rendering
                {
                    CasinoResult_JourneyWest_ExperienceGame_Block experienceGame = new CasinoResult_JourneyWest_ExperienceGame_Block();
                    RenderExperienceGame(experienceGame, rows, ref i, c);
                    tpl.ExperienceGame.Append(experienceGame);
                }
                else if (showType == "AddRow_JackpotGame") // Enter Jackpot Game rendering
                {
                    CasinoResult_JourneyWest_JackpotGame_Block jackpotGame = new CasinoResult_JourneyWest_JackpotGame_Block();
                    RenderJackpotGame(jackpotGame, rows, ref i, c);
                    tpl.ExperienceGame.Append(jackpotGame);
                }
                else if (showType == "AddRow_KidnappingGame") // Enter Kidnapping Game rendering
                {
                    CasinoResult_JourneyWest_KidnappingGame_Block didnappingGame = new CasinoResult_JourneyWest_KidnappingGame_Block();
                    RenderKidnappingGame(didnappingGame, rows, ref i, c);
                    tpl.KidnappingGame.Append(didnappingGame);
                }
            }

            writer.Write(tpl);

            
            
        }


        private void RenderNormalGame(CasinoResult_JourneyWest_NormalGame_Block tpl, DataRowCollection rows, ref int fromIndex, int count)
        {
            fromIndex += 2; // Skip 2 headers (already defined in template for AddRow_NormalGame, AddRow_Normal_HitBetLine
            CasinoResult_JourneyWest_NormalGame_Row_Block row = new CasinoResult_JourneyWest_NormalGame_Row_Block();
            tpl.Row.Clear();
            for (int i = fromIndex; i < count; i++)
            {
                string showType = rows[i]["ShowType"] as string;
                if (showType == "Result_HitBetLine")
                { 
                    row.lineNo = rows[i]["LineNo"] as string;
                    row.symbol = rows[i]["Symbol"] as string;
                    row.coinSize = rows[i]["CoinSize"] as string;
                    row.betPerLine = rows[i]["BetPerLine"] as string;
                    row.odds = rows[i]["Odds"].ToString();
                    row.perLineWin = rows[i]["Win"].ToString();
                    tpl.Row.Append(row);
                }
                else if (showType == "Result_SubWin")
                {
                    tpl.actionName = rows[i][CasinoLang("ActionName")] as string;
                    tpl.win = rows[i]["Win"].ToString();
                    break;
                }
            }
        }

        private void RenderScatterGame(CasinoResult_JourneyWest_ScatterGame_Block tpl, DataRowCollection rows, ref int fromIndex, int count)
        {
            CasinoResult_JourneyWest_ScatterGame_Row_Block row = new CasinoResult_JourneyWest_ScatterGame_Row_Block();
            tpl.Row.Clear();
            for (int i = fromIndex; i < count; i++)
            {
                string showType = rows[i]["ShowType"] as string;
                if (showType == "Result_HitScatter")
                {
                    row.name = rows[i][CasinoLang("ActionName")] as string;
                    row.symbol = rows[i]["Symbol"] as string;
                    row.odds = rows[i]["Odds"].ToString();
                    row.scatterWin = rows[i]["Win"].ToString();
                    tpl.Row.Append(row);
                }
                else if (showType == "Result_SubWin")
                {
                    tpl.actionName = rows[i][CasinoLang("ActionName")] as string;
                    tpl.win = rows[i]["Win"].ToString();
                    break;
                }
            }

        }

        private void RenderExperienceGame(CasinoResult_JourneyWest_ExperienceGame_Block tpl, DataRowCollection rows, ref int fromIndex, int count)
        {
            fromIndex += 2; // Skip 2 headers
            CasinoResult_JourneyWest_ExperienceGame_Row_Block row = new CasinoResult_JourneyWest_ExperienceGame_Row_Block();
            tpl.Row.Clear();
            for (int i = fromIndex; i < count; i++)
            {
                string showType = rows[i]["ShowType"] as string;
                if (showType == "Result_Experience")
                {
                    row.actionName = rows[i][CasinoLang("ActionName")] as string;
                    row.win = rows[i]["Win"].ToString();
                    tpl.Row.Append(row);
                }
                else if (showType == "MaxMultiply")
                {
                    row.actionName = rows[i][CasinoLang("Multiplier")] as string;
                    row.win = rows[i]["Win"].ToString();
                    tpl.Row.Append(row);
                }
                else if (showType == "Result_SubWin")
                {
                    tpl.actionName = rows[i][CasinoLang("ActionName")] as string;
                    tpl.win = rows[i]["Win"].ToString();

                    break;
                }
            }
        }

        private void RenderJackpotGame(CasinoResult_JourneyWest_JackpotGame_Block tpl, DataRowCollection rows, ref int fromIndex, int count)
        {
            fromIndex += 2; // Skip 2 headers
            CasinoResult_JourneyWest_JackpotGame_Row_Block row = new CasinoResult_JourneyWest_JackpotGame_Row_Block();
            tpl.Row.Clear();
            for (int i = fromIndex; i < count; i++)
            {
                string showType = rows[i]["ShowType"] as string;
                if (showType == "Result_HitJackpot")
                {
                    row.actionName = rows[i][CasinoLang("ActionName")] as string;
                    row.win = rows[i]["Win"].ToString();
                    tpl.Row.Append(row);
                }
                else if (showType == "Result_SubWin")
                {
                    tpl.actionName = rows[i][CasinoLang("ActionName")] as string;
                    tpl.win = rows[i]["Win"].ToString();

                    
                    break;
                }
            }
        }

        private void RenderKidnappingGame(CasinoResult_JourneyWest_KidnappingGame_Block tpl, DataRowCollection rows, ref int fromIndex, int count)
        {
            fromIndex += 2; // Skip 2 headers
            CasinoResult_JourneyWest_KidnappingGame_Row_Block row = new CasinoResult_JourneyWest_KidnappingGame_Row_Block();
            tpl.Row.Clear();
            for (int i = fromIndex; i < count; i++)
            {
                string showType = rows[i]["ShowType"] as string;
                if (showType == "Result_Kidnapping")
                {
                    row.actionName = rows[i][CasinoLang("ActionName")] as string;
                    row.symbol = rows[i]["Symbol"] as string;
                    row.multiplier = rows[i]["Multiplier_Result"] as string;
                    row.kidnappingGameWin = rows[i]["Win"].ToString();
                    tpl.Row.Append(row);
                }
                else if (showType == "Result_SubWin")
                {
                    tpl.actionName = rows[i][CasinoLang("ActionName")] as string;
                    tpl.win = rows[i]["Win"].ToString();
                    break;
                }
            }

        }

        private void RenderBasicInfo(CasinoResult_JourneyWest_Template tpl, DataTable dtDetail)
        {
            DataRowCollection rows = dtDetail.Rows;

            tpl.player = Resources.Lang.player;
            tpl.tableName = rows[0][CasinoLang("TableName")] as string;
            tpl.betTime = rows[0]["BetTime"].ToString();
            tpl.userName = rows[0]["UserName"] as string;
            tpl.result = rows[0]["Result"] as string;
            tpl.playerHistoryNo = rows[0][CasinoLang("PlayerHistoryNo")] as string;
            tpl.playerHistoryNo_value = rows[0]["PlayerHistoryNo_Result"] as string;
            tpl.autoSpin = rows[0][CasinoLang("AutoSpin")] as string;
            tpl.autoSpin_value = rows[0][CasinoLang("AutoSpinResult")] as string;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {

            CasinoBO casino = new CasinoBO(CurrentUserSession.NetSessionId);
            DataTable[] tableArray = casino.Report.WinLossDetail(userBetId);

            return tableArray;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            gameTypeNo = Request["gametype"];
            int.TryParse(Request["userbetid"], out userBetId);            
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (userBetId == 0)
            {
                return new ErrorPage_Page("Invalid user bet id!");
            }

            return null;
        }



        public static string CasinoLang(string original)
        {
            return CasinoBO.CasinoLang(original);
        }


        /// <summary>
        /// Reformat text for number in this report
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string NumberReport(object number)
        {
            decimal dcNumber = Convert.IsDBNull(number) ? 0 : (decimal)number;
            string resultText = Formater.DecFormat(number, 2);
            string spanClass = dcNumber >= 0 ? "bl_underdog" : "bl_favorite";
            return string.Format("<span class='{0}'>{1}</span>", spanClass, resultText);
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

}