
using System;
using System.Data;
using System.Web.UI;

using Fanex.Age.UI;
using Fanex.Age.UI.Components;

using Sunplus.Agent.Model.BL.Casino;
using Sunplus.Agent.Utilities;
using Resources;


namespace Sunplus.Agent.UI._BetList
{
    public partial class CasinoResult_TenOrBetter : TplPage
    {
        int userBetId = 0;
        string gameTypeNo = null;

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
            var ds = View as DataTable[];

            var dtBasic = ds[0];
            var dtDetail = ds[1];

            //writer.Write(RenderTable(dtBasic));

            //return;

            var tpl = new CasinoResult_TenOrBetter_Template();

            if (ds[0].Rows.Count != 0)
            {
                RenderBasicInfo(tpl, dtBasic);

                RenderTenOrBetter(tpl, dtDetail);
            }
            else
                RenderNoInfomation(tpl);

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
            gameTypeNo = Request["gametype"];
            int.TryParse(Request["userbetid"], out userBetId);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (userBetId == 0)
            {
                return new ErrorPage_Page(Lang.strNotAccessPage);
            }

            return null;
        }

        private void RenderBasicInfo(CasinoResult_TenOrBetter_Template tpl, DataTable dtDetail)
        {
            var rows = dtDetail.Rows;

            tpl.Content.tableName = rows[0][CasinoLang("TableName")] as string;
            tpl.Content.betTime = rows[0]["BetTime"] as string;
            tpl.Content.userName = rows[0]["UserName"] as string;

            tpl.Content.Result.Clear();

            if (dtDetail.Rows.Count == 2)
            {
                var res = new CasinoResult_TenOrBetter_Content_Result_Block();

                if (Convert.ToInt32(rows[0]["ShowType"]) == 2)
                {
                    res.result = rows[0]["Result"].ToString();
                    tpl.Content.Result.Append(res);

                    res = new CasinoResult_TenOrBetter_Content_Result_Block();
                    res.result = rows[1]["Result"].ToString();
                    tpl.Content.Result.Append(res);
                }
                else
                {
                    res.result = rows[1]["Result"].ToString();
                    tpl.Content.Result.Append(res);

                    res = new CasinoResult_TenOrBetter_Content_Result_Block();
                    res.result = rows[0]["Result"].ToString();
                    tpl.Content.Result.Append(res);
                }
            }
            else
            {
                var res = new CasinoResult_TenOrBetter_Content_Result_Block();
                res.result = rows[0]["Result"].ToString();

                tpl.Content.Result.Append(res);
            }

            tpl.Content.playerHistoryNo = rows[0][CasinoLang("PlayerHistoryNo")] as string;
            tpl.Content.playerHistoryNoResult = rows[0]["PlayerHistoryNo_Result"] as string;

            tpl.Content.ConSize = rows[0][CasinoLang("CoinSizeName")] as string;
            tpl.Content.ConSize_vl = rows[0][CasinoLang("CoinSizeAmount")] as string;

            tpl.Content.Bet = rows[0][CasinoLang("Bet")] as string;
            tpl.Content.WinLoss = rows[0][CasinoLang("Winloss")] as string;

            tpl.NoInfo.Hide();
        }

        private void RenderTenOrBetter(CasinoResult_TenOrBetter_Template tpl, DataTable dbt)
        {
            var rows = dbt.Rows;
            var currency = rows[0]["currency"] as string;

            tpl.Content.Bet_vl = currency + " " + NumberReport(rows[0]["Bet"]);
            tpl.Content.WinLoss_vl = currency + " " + NumberReport(rows[0]["WinLoss"]);
        }

        private void RenderNoInfomation(CasinoResult_TenOrBetter_Template tpl)
        {
            tpl.Content.Hide();
            tpl.NoInfo.Show();
        }

        private string NumberReport(object number)
        {
            var dcNumber = Convert.IsDBNull(number) ? 0 : (decimal)number;
            var resultText = Formater.DecFormat(number, 2);
            var spanClass = dcNumber >= 0 ? "bl_underdog" : "bl_favorite";

            return string.Format("<span class='{0}'>{1}</span>", spanClass, resultText);
        }

        public static string CasinoLang(string original)
        {
            return CasinoBO.CasinoLang(original);
        }
    }
}
