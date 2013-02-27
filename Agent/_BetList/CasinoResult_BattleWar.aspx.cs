namespace Sunplus.Agent.UI._BetList
{
    using System;
    using System.Data;
    using System.Web.UI;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.Model.BL.Casino;
    using Sunplus.Agent.Utilities;

    public partial class CasinoResult_BattleWar : TplPage
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
            //return;

            string totalBet = "";
            string totalWin = "";
            CasinoResult_BattleWar_Template tpl = new CasinoResult_BattleWar_Template();

            RenderBasicInfo(tpl, dtBasic);

            RenderFanTanGame(tpl, dtDetail, ref totalBet, ref totalWin);

            tpl.totalBetMoney = totalBet.ToString();
            tpl.totalWin = totalWin.ToString();
            writer.Write(tpl);
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

        private void RenderBasicInfo(CasinoResult_BattleWar_Template tpl, DataTable dtDetail)
        {
            CasinoResult_BattleWar_row_Block tplRow = new CasinoResult_BattleWar_row_Block();
            tpl.row.Clear();
            DataRowCollection rows = dtDetail.Rows;
            tpl.betTime = rows[0]["BetTime"].ToString();
            tpl.userName = rows[0]["UserName"] as string;
            tpl.tableName = rows[0][CasinoLang("TableName")] as string;
            for (int i = 0; i < rows.Count; i++)
            {
                tplRow.resultName = rows[i][CasinoLang("ResultName")] as string;
                tplRow.result = rows[i]["Result"] as string;
                tpl.row.Append(tplRow);
            }
        }

        private void RenderFanTanGame(CasinoResult_BattleWar_Template tpl, DataTable dbt, ref string totalBet, ref string totalWin)
        {
            DataRowCollection row = dbt.Rows;
            decimal _totalWin = 0;
            decimal _totalBet = 0;
            string currency = string.Empty;

            tpl.rowDetail.Clear();
            var rowDetail = new CasinoResult_BattleWar_rowDetail_Block();

            for (int i = 0; i < row.Count; i++)
            {
                currency = row[i]["currency"] as string;

                rowDetail.betMoney = currency + " " + Formater.DecFormat(row[i]["Bet"], 2);
                rowDetail.betTypeName = row[i][CasinoLang("BetTypeName")].ToString();
                rowDetail.Win = currency + " " + NumberReport(row[i]["Winloss"]);
                _totalBet += Convert.ToDecimal(row[i]["Bet"]);
                _totalWin += Convert.ToDecimal(row[i]["Winloss"]);

                tpl.rowDetail.Append(rowDetail);
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