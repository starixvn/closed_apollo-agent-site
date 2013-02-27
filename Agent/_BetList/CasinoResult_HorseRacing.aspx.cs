using System;
using System.Data;
using System.Web.UI;

using Fanex.Age.UI;
using Fanex.Age.UI.Components;

using Resources;

using Sunplus.Agent.Model.BL.Casino;
using Sunplus.Agent.Utilities;

namespace Sunplus.Agent.UI._BetList
{
    public partial class CasinoResult_HorseRacing : TplPage
    {
        private int userBetId = 0;
        private string currency = string.Empty;

        private void RenderData(System.Web.UI.HtmlTextWriter writer)
        {
            var ds = View as DataTable[];

            var dtBasic = ds[0];
            var dtDetail = ds[1];

            writer.Write(RenderTable(dtBasic));
            writer.Write(RenderTable(dtDetail));

            return;
        }

        private string RenderTable(DataTable dt)
        {
            var strRet = "<table border=\"1\" style=\"border-collapse: collapse;\">";
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
            //this.RenderData(writer);
            //return;

            var ds = View as DataTable[];

            var dtBasic = ds[0];
            var dtDetail = ds[1];

            var tpl = new CasinoResult_HorseRacing_Template();

            if (ds != null && ds[0].Rows.Count != 0)
            {
                this.currency = dtDetail.Rows[0]["Currency"].ToString();

                this.RenderBasicInfo(tpl, dtBasic);

                this.RenderHorseRacing(tpl, dtDetail);
            }
            else
                this.RenderNoInfomation(tpl);

            writer.Write(tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var casino = new CasinoBO(CurrentUserSession.NetSessionId);
            var tableArray = casino.Report.WinLossDetail(userBetId);

            return tableArray;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(Request["userbetid"], out userBetId);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (this.userBetId == 0)
            {
                return new ErrorPage_Page(Lang.strNotAccessPage);
            }

            return null;
        }

        private void RenderBasicInfo(CasinoResult_HorseRacing_Template tpl, DataTable dtDetail)
        {
            var rows = dtDetail.Rows;

            tpl.Content.tableName = rows[0][CasinoLang("TableName")].ToString();
            tpl.Content.betTime = rows[0]["BetTime"].ToString();
            tpl.Content.userName = rows[0]["UserName"].ToString();

            tpl.Content.result = rows[0][CasinoLang("ResultName")].ToString();
            tpl.Content.result_vl = rows[0]["Result"].ToString();

            tpl.Content.playerHistoryNo = rows[0][CasinoLang("PlayerHistoryNo")].ToString();
            tpl.Content.playerHistoryNoResult = rows[0]["PlayerHistoryNo_Result"].ToString();

            tpl.Content.Combination = rows[0][CasinoLang("Combination")].ToString();
            tpl.Content.Bet = rows[0][CasinoLang("Bet")].ToString();
            tpl.Content.Odds = rows[0][CasinoLang("Odds")].ToString();
            tpl.Content.WinLoss = rows[0][CasinoLang("WinLoss")].ToString();

            tpl.Content.Total = rows[0][CasinoLang("Total")].ToString();

            tpl.NoInfo.Hide();
        }

        private void RenderHorseRacing(CasinoResult_HorseRacing_Template tpl, DataTable dt)
        {
            var rows = dt.Select("BetContent is not null", "BetContent");

            tpl.Content.Row.Clear();

            var block = new CasinoResult_HorseRacing_Content_Row_Block();

            var betTotal = 0.0m;
            var winTotal = 0.0m;

            foreach (DataRow row in rows)
            {
                var bet = Convert.ToDecimal(row["Bet"].ToString().Replace(currency, string.Empty).Trim());
                var win = Convert.ToDecimal(row["WinLoss"].ToString().Replace(currency, string.Empty).Trim());

                betTotal += bet;
                winTotal += win;

                block.Combination_vl = row["BetContent"].ToString();
                block.Bet_vl = this.formatNumber(bet);
                block.Odds_vl = decimal.Round(Convert.ToDecimal(row["Odds"]), 2).ToString();
                block.WinLoss_vl = this.formatNumber(win);

                tpl.Content.Row.Append(block);
            }

            tpl.Content.Bet_tt = this.formatNumber(betTotal);
            tpl.Content.WinLoss_tt = this.formatNumber(winTotal);
        }

        private void RenderNoInfomation(CasinoResult_HorseRacing_Template tpl)
        {
            tpl.Content.Hide();
            tpl.NoInfo.Show();
        }

        private string formatNumber(object number)
        {
            var dec = decimal.Round((decimal)number, 2);
            var spanClass = dec >= 0 ? "bl_underdog" : "bl_favorite";

            return string.Join(" ", new string[] { currency, string.Format("<span class='{0}'>{1}</span>", spanClass, dec.ToString()) });
        }

        public static string CasinoLang(string original)
        {
            return CasinoBO.CasinoLang(original);
        }
    }
}