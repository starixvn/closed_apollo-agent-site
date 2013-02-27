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
    public partial class CasinoResult_MiniRoulette : TplPage
    {
        int userBetId = 0;
        string gameTypeNo = null;

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
            //writer.Write(RenderTable(dtDetail));

            //return;

            CasinoResult_MiniRoulette_Template tpl = new CasinoResult_MiniRoulette_Template();

            if (ds != null && ds[0].Rows.Count != 0)
            {
                RenderBasicInfo(tpl, dtBasic);

                if (ds[1].Rows.Count != 0)
                    RenderMiniRoulette(tpl, dtDetail);
                else
                {
                    tpl.Content.Rows.Hide();
                    tpl.Content.Total.BetTotal = CurrentUserSession.Currency + " 0.00";
                    tpl.Content.Total.WinLossTotal = CurrentUserSession.Currency + " 0.00";
                    tpl.NoInfo.Hide();
                }
            }
            else
                RenderNoInfomation(tpl);

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
                return new ErrorPage_Page(Resources.Lang.invalidLogin);
            }

            return null;
        }

        private void RenderBasicInfo(CasinoResult_MiniRoulette_Template tpl, DataTable dtDetail)
        {
            DataRowCollection rows = dtDetail.Rows;

            tpl.Content.tableName = rows[0][CasinoLang("TableName")] as string;
            tpl.Content.betTime = rows[0]["BetTime"].ToString();
            tpl.Content.userName = rows[0]["UserName"] as string;
            if (gameTypeNo != "R0027")
            {
                tpl.Content.Result1.Hide();
                tpl.Content.ResultName = rows[0][CasinoLang("ResultName")] as string;
                tpl.Content.Result.result = rows[0]["Result"].ToString();
            }
            else
            {
                tpl.Content.Result.Hide();
                tpl.Content.Result1.Clear();
                tpl.Content.ResultName = Resources.Casino.Result;

                var block = new CasinoResult_MiniRoulette_Content_Result1_Block();

                for (var i = 0; i < dtDetail.Rows.Count; i++)
                {
                    block.Result1 = rows[i][CasinoLang("ResultName")].ToString();
                    block.Result1_vl = rows[i]["Result"].ToString();

                    tpl.Content.Result1.Append(block);
                }
            }
            tpl.Content.Bet = Resources.Casino.Bet;
            tpl.Content.WinLoss = Resources.Casino.Win;

            tpl.NoInfo.Hide();
        }

        private void RenderMiniRoulette(CasinoResult_MiniRoulette_Template tpl, DataTable dbt)
        {
            DataRowCollection row = dbt.Rows;

            string currency = string.Empty;

            currency = row[0]["currency"] as string;

            var rows = new CasinoResult_MiniRoulette_Content_Rows_Block();
            tpl.Content.Rows.Clear();

            var betTotal = 0m;
            var winLossTotal = 0m;

            for (int i = 0; i < dbt.Rows.Count; i++)
            {
                betTotal += row[i].Field<decimal>("Bet");
                winLossTotal += row[i].Field<decimal>("WinLoss");

                rows.BetTypeName_vl = row[i].Field<string>(CasinoLang("BetTypeName"));
                rows.Bet_vl = currency + " " + NumberReport(row[i].Field<decimal>("Bet"));
                rows.WinLoss_vl = currency + " " + NumberReport(row[i].Field<decimal>("WinLoss"));

                tpl.Content.Rows.Append(rows);
            }

            tpl.Content.Total.BetTotal = currency + " " + NumberReport(betTotal);
            tpl.Content.Total.WinLossTotal = currency + " " + NumberReport(winLossTotal);
        }

        private void RenderNoInfomation(CasinoResult_MiniRoulette_Template tpl)
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