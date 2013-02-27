namespace Sunplus.Agent.UI._BetList
{
    using System;
    using System.Data;
    using System.Web.UI;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.Model.BL.Casino;
    using Sunplus.Agent.Utilities;

    public partial class CasinoResult_LoveStoryScratch : TplPage
    {
        private int userBetId = 0;
        private string gameTypeNo = null;

        private string RenderTable(DataTable dt)
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

            //return;
            CasinoResult_LoveStoryScratch_Template tpl = new CasinoResult_LoveStoryScratch_Template();

            if (ds[0].Rows.Count != 0)
            {
                RenderBasicInfo(tpl, dtBasic);

                RenderLoveStoryScratch(tpl, dtDetail);
            }
            else
                RenderNoInfomation(tpl);

            writer.Write(tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            CasinoBO casino = new CasinoBO(CurrentUserSession.NetSessionId);
            DataTable[] tableArray = casino.Report.WinLossDetail(userBetId);
            //tableArray[0] = new DataTable();
            //tableArray[1] = new DataTable();
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

        private void RenderBasicInfo(CasinoResult_LoveStoryScratch_Template tpl, DataTable dtDetail)
        {
            DataRowCollection rows = dtDetail.Rows;

            tpl.Content.tableName = rows[0][CasinoLang("TableName")] as string;
            tpl.Content.betTime = rows[0]["BetTime"] as string;
            tpl.Content.userName = rows[0]["UserName"] as string;
            tpl.Content.ResultName = rows[0][CasinoLang("ResultName")] as string;
            tpl.Content.result = rows[0]["Result"] as string;
            tpl.Content.playerHistoryNoResult = rows[0]["PlayerHistoryNo_Result"] as string;
            tpl.Content.playerHistoryNo = rows[0][CasinoLang("PlayerHistoryNo")] as string;
            tpl.Content.Bet = rows[0][CasinoLang("Bet")] as string;
            tpl.Content.Odds = rows[0][CasinoLang("Odds")] as string;
            tpl.Content.AutoPlay = rows[0][CasinoLang("AutoPlay")] as string;
            tpl.Content.AutoPlayResult = rows[0][CasinoLang("AutoPlayResult")] as string;
            tpl.Content.Multiplier = rows[0][CasinoLang("Multiplier")] as string;
            tpl.Content.MultiplierValue = Convert.ToInt32(rows[0]["Multiplier"]).ToString();

            tpl.NoInfo.Hide();
        }

        private void RenderLoveStoryScratch(CasinoResult_LoveStoryScratch_Template tpl, DataTable dbt)
        {
            DataRowCollection row = dbt.Rows;

            string currency = string.Empty;

            currency = row[0]["currency"] as string;
            tpl.Content.BetMoney = currency + " " + Formater.DecFormat(row[0]["Bet"], 2);
            tpl.Content.OddsValue = Convert.ToInt32(row[0]["Odds"]).ToString();
            tpl.Content.Win = currency + " " + NumberReport(row[0]["Winloss"]);
        }

        private void RenderNoInfomation(CasinoResult_LoveStoryScratch_Template tpl)
        {
            tpl.Content.Hide();
            tpl.NoInfo.Show();
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