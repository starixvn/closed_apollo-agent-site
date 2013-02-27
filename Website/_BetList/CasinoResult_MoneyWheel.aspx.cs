using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Age.UI;
using CasinoLibrary.SBAgent;
using System.Data;
using Sunplus.Agent.Model.BL.Casino;
using Sunplus.Agent.Utilities;
using Fanex.Age.UI.Components;

namespace Sunplus.Agent.UI._BetList
{
    public partial class CasinoResult_MoneyWheel : TplPage
    {
        int userBetId = 0;
        string gameTypeNo = null;

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

        public override void RenderContent(HtmlTextWriter writer)
        {
            DataTable[] ds = View as DataTable[];
            DataTable dtBasic = ds[0];
            DataTable dtDetail = ds[1];

            //writer.Write(RenderTable(dtBasic));
            //writer.Write(RenderTable(dtDetail));


            string totalBet = "";
            string totalWin = "";
            CasinoResult_MoneyWheel_Template tpl = new CasinoResult_MoneyWheel_Template();

            RenderBasicInfo(tpl, dtBasic);

            RenderMoneyWheelGame(tpl, dtDetail, ref totalBet, ref totalWin);

            tpl.totalBet = totalBet.ToString();
            tpl.totalWin = totalWin.ToString();
            writer.Write(tpl);
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

        public override object InvokeData(System.Collections.Hashtable p)
        {
            CasinoBO casino = new CasinoBO(CurrentUserSession.NetSessionId);
            DataTable[] tableArray = casino.Report.WinLossDetail(userBetId);

            return tableArray;
        }

        private void RenderBasicInfo(CasinoResult_MoneyWheel_Template tpl, DataTable dtDetail)
        {
            DataRowCollection rows = dtDetail.Rows;

            tpl.betTime = rows[0]["BetTime"].ToString();
            tpl.userName = rows[0]["UserName"] as string;
            tpl.result = rows[0]["Result"] as string;
            tpl.playerHistoryResult = rows[0]["PlayerHistoryNo_Result"] as string;
            tpl.tableName = rows[0][CasinoLang("TableName")] as string;
            tpl.resultName = rows[0][CasinoLang("ResultName")] as string;
            tpl.playerHistoryNo = rows[0][CasinoLang("PlayerHistoryNo")] as string;

        }

        private void RenderMoneyWheelGame(CasinoResult_MoneyWheel_Template tpl, DataTable dbt, ref string totalBet, ref string totalWin)
        {
            CasinoResult_MoneyWheel_Row_Block tplRow = new CasinoResult_MoneyWheel_Row_Block();
            tpl.Row.Clear();
            DataRowCollection row = dbt.Rows;
            decimal _totalWin = 0;
            decimal _totalBet = 0;
            string currency = string.Empty;
            for (int i = 0; i < row.Count; i++)
            {
                currency = row[i]["currency"] as string;
                tplRow.betType = row[i][CasinoLang("BetTypeName")] as string;
                tplRow.bet = currency + " " + Formater.DecFormat(row[i]["Bet"], 2);
                tplRow.odds = Convert.ToInt32(row[i]["Odds"]).ToString();
                tplRow.win = currency + " " + NumberReport(row[i]["Winloss"]);
                _totalBet += Convert.ToDecimal(row[i]["Bet"]);
                _totalWin += Convert.ToDecimal(row[i]["Winloss"]);
                tpl.Row.Append(tplRow);
            }
            totalBet = currency + " " + Formater.DecFormat(_totalBet, 2);
            totalWin = currency + " " + NumberReport(_totalWin);
        }

        private string NumberReport(object number)
        {
            decimal dcNumber = Convert.IsDBNull(number) ? 0 : (decimal)number;
            string resultText = Formater.DecFormat(number, 2);
            string spanClass = dcNumber >= 0 ? "bl_underdog" : "bl_favorite";
            return string.Format("<span class='{0}'>{1}</span>", spanClass, resultText);
        }

        public static string CasinoLang(string original)
        {
            return CasinoBO.CasinoLang(original);
        }
    }
}
