namespace Sunplus.Agent.UI._BetList
{
    using System;
    using System.Data;
    using System.Web.UI;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.Model.BL.Casino;
    using Sunplus.Agent.Utilities;

    public partial class CasinoResult_GongXiFaChai : TplPage
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

        private string RenderResult(string Result)
        {
            string[] split = Result.Split(',');
            string strRet = "<table border=1 style='text-align:center'>";
            strRet += "<tr>";
            for (int i = 0; i < 9; i++)
            {
                if (i % 3 == 2)
                {
                    strRet += "<td width=100px>" + split[i] + "</td>" + "</tr>";
                }
                else
                {
                    strRet += "<td width=100px>" + split[i] + "</td>";
                }
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
            //writer.Write(RenderResult("50,30,200,1,200,50,100,1,30"));
            //return;

            CasinoResult_GongXiFaChai_Template tpl = new CasinoResult_GongXiFaChai_Template();

            if (ds[0].Rows.Count != 0)
            {
                RenderBasicInfo(tpl, dtBasic);
                RenderGongXiFaChai(tpl, dtDetail);
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
                return new ErrorPage_Page("Invalid user bet id!");
            }

            return null;
        }

        private void RenderBasicInfo(CasinoResult_GongXiFaChai_Template tpl, DataTable dtDetail)
        {
            DataRowCollection rows = dtDetail.Rows;

            tpl.Content.betTime = rows[0]["BetTime"].ToString();
            tpl.Content.userName = rows[0]["UserName"] as string;

            var results = rows[0]["Result"].ToString().Split(',');
            tpl.Content.Val1 = results[0];
            tpl.Content.Val2 = results[1];
            tpl.Content.Val3 = results[2];
            tpl.Content.Val4 = results[3];
            tpl.Content.Val5 = results[4];
            tpl.Content.Val6 = results[5];
            tpl.Content.Val7 = results[6];
            tpl.Content.Val8 = results[7];
            tpl.Content.Val9 = results[8];

            tpl.Content.playerHistoryNoResult = rows[0]["PlayerHistoryNo_Result"] as string;
            tpl.Content.tableName = rows[0][CasinoLang("TableName")] as string;
            tpl.Content.playerHistoryNo = rows[0][CasinoLang("PlayerHistoryNo")] as string;
            tpl.Content.AutoPlay = rows[0][CasinoLang("AutoPlay")] as string;
            tpl.Content.AutoPlay = rows[0][CasinoLang("AutoPlayResult")] as string;

            tpl.NoInfo.Hide();
        }

        private void RenderGongXiFaChai(CasinoResult_GongXiFaChai_Template tpl, DataTable dbt)
        {
            DataRowCollection row = dbt.Rows;

            string currency = string.Empty;
            for (int i = 0; i < row.Count; i++)
            {
                currency = row[i]["currency"] as string;
                tpl.Content.BetMoney = currency + " " + Formater.DecFormat(row[i]["Bet"], 2);
                tpl.Content.Odds = Convert.ToInt32(row[i]["Odds"]).ToString();
                tpl.Content.Win = currency + " " + NumberReport(row[i]["Winloss"]);
            }
        }

        private void RenderNoInfomation(CasinoResult_GongXiFaChai_Template tpl)
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