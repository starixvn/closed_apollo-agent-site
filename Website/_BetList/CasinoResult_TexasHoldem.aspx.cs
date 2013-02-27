namespace Sunplus.Agent.UI._BetList
{
    using System;
    using System.Web.UI;
    using Fanex.Age.UI;
    using System.Data;
    using Sunplus.Agent.Model.BL.Casino;
    using Sunplus.Agent.Utilities;
    using Fanex.Age.UI.Components;

    public partial class CasinoResult_TexasHoldem : TplPage
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

            //return;

            string totalBet = "";
            string totalWin = "";
            CasinoResult_TexasHoldem_Template tpl = new CasinoResult_TexasHoldem_Template();

            if (ds[0].Rows.Count != 0)
            {
                RenderBasicInfo(tpl, dtBasic);

                RenderFaceUpGame(tpl, dtDetail, ref totalBet, ref totalWin);

                tpl.Content.totalBetMoney = totalBet.ToString();
                tpl.Content.totalWin = totalWin.ToString();
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


        private void RenderBasicInfo(CasinoResult_TexasHoldem_Template tpl, DataTable dtDetail)
        {
            DataRowCollection rows = dtDetail.Rows; 
            //CasinoResult_TexasHoldem_Content_Block tplContent = new CasinoResult_TexasHoldem_Content_Block();
            CasinoResult_TexasHoldem_Content_result_Block tplResult = new CasinoResult_TexasHoldem_Content_result_Block();
            //tpl.Content.Clear();
           
            tpl.Content.betTime = rows[0]["BetTime"].ToString();
            tpl.Content.userName = rows[0]["UserName"] as string; ;
            tpl.Content.tableName = rows[0][CasinoLang("TableName")] as string;
            tpl.Content.staticResultName = rows[0][CasinoLang("ResultName")] as string;

            tpl.Content.result.Clear();
            for (int i = 1; i < rows.Count; i++)
            {
                tplResult.result = rows[i]["Result"] as string;
                tplResult.resultName = rows[i][CasinoLang("ResultName")] as string;
                tpl.Content.result.Append(tplResult);
            }

            tpl.NoInfo.Hide();
        }

        private void RenderFaceUpGame(CasinoResult_TexasHoldem_Template tpl, DataTable dbt, ref string totalBet, ref string totalWin)
        {
            CasinoResult_TexasHoldem_Content_Row_Block tplRow = new CasinoResult_TexasHoldem_Content_Row_Block();

            tpl.Content.Row.Clear();
            DataRowCollection row = dbt.Rows;
            decimal _totalWin = 0;
            decimal _totalBet = 0;
            string currency = string.Empty;
            for (int i = 0; i < row.Count; i++)
            {
                currency = row[i]["currency"] as string;
                tplRow.BetType = row[i][CasinoLang("BetTypeName")] as string;
                tplRow.betMoney = currency + " " + Formater.DecFormat(row[i]["Bet"], 2);
                tplRow.Win = currency + " " + NumberReport(row[i]["Winloss"]);
                _totalBet += Convert.ToDecimal(row[i]["Bet"]);
                _totalWin += Convert.ToDecimal(row[i]["Winloss"]);
                tpl.Content.Row.Append(tplRow);
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

        private void RenderNoInfomation(CasinoResult_TexasHoldem_Template tpl)
        {
            tpl.Content.Hide();
            tpl.NoInfo.Show();
        }
    }
}