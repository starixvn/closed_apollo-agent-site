namespace Sunplus.Agent.UI._BetList
{
    using System;
    using System.Data;
    using System.Web.UI;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Resources;
    using Sunplus.Agent.Model.BL.Casino;
    using Sunplus.Agent.Utilities;

    public partial class CasinoResult_4hJOB : TplPage
    {
        private int userBetId = 0;
        private string gameTypeNo = null;

        private string RenderTable(DataTable dt)
        {
            if (dt == null) return "DataTable is null <br />";

            string strRet = "<table border='1' style='border-collapse: collapse;'>";
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
            //writer.Write(RenderTable(TableDetail));
            //writer.Write(RenderTable(TableTotal));

            var tpl = new CasinoResult_4hJOB_Template();

            if ((this.TableTotal != null && this.TableDetail != null && this.TableTotal.Rows.Count > 0 && this.TableDetail.Rows.Count > 0))
            {
                tpl.NoInfo.Clear();

                var rows = TableDetail.Rows;
                var row = rows[0];

                this.RenderBasicInfo(tpl, row);
                this.RenderTotal(tpl, this.TableTotal.Rows[0]);
            }
            else
            {
                tpl.Content.Clear();
            }

            writer.Write(tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var casino = new CasinoBO(CurrentUserSession.NetSessionId);
            var tables = casino.Report.WinLossDetail(userBetId);

            return tables;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.gameTypeNo = Request["gametype"];
            int.TryParse(Request["userbetid"], out this.userBetId);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (userBetId == 0)
            {
                return new ErrorPage_Page(Lang.strNotAccessPage);
            }

            return null;
        }

        private DataTable TableDetail
        {
            get
            {
                return ((DataTable[])View)[0];
            }
        }

        private DataTable TableTotal
        {
            get
            {
                return ((DataTable[])View)[1];
            }
        }

        private void RenderBasicInfo(CasinoResult_4hJOB_Template tpl, DataRow dr)
        {
            tpl.Content.tablename = dr[CasinoBO.CasinoLang("TableName")].ToString();
            tpl.Content.datetime = dr["BetTime"].ToString();
            tpl.Content.Player = Lang.player;
            tpl.Content.UserName = dr["UserName"].ToString();

            tpl.Content.PlayerHistoryNo = dr[CasinoBO.CasinoLang("PlayerHistoryNo")].ToString();
            tpl.Content.PlayerHistoryNo_vl = dr["PlayerHistoryNo_Result"].ToString();

            if (TableDetail.Rows.Count == 1)
            {
                tpl.Content.Result2.Clear();

                tpl.Content.Result1.Result1_vl = dr["Result"].ToString();
                tpl.Content.Result1.roundType = dr[CasinoBO.CasinoLang("RoundTypeName")].ToString();
                tpl.Content.Result1.roundType_vl = dr[CasinoBO.CasinoLang("RoundTypeResult")].ToString();
            }
            else
            {
                tpl.Content.Result1.Clear();

                tpl.Content.Result2.HandNo = dr[CasinoBO.CasinoLang("HandNo")].ToString();
                tpl.Content.Result2.OriginHand = dr[CasinoBO.CasinoLang("OriginHand")].ToString();
                tpl.Content.Result2.FinalHand = dr[CasinoBO.CasinoLang("FinalHand")].ToString();
                tpl.Content.Result2.WinLost = dr[CasinoBO.CasinoLang("Win")].ToString();

                tpl.Content.Result2.Result2Row.Clear();

                var row = new CasinoResult_4hJOB_Content_Result2_Result2Row_Block();

                var dtrows = TableDetail.Rows;

                row.HandNo_vl = dtrows[0]["HandNoResult"].ToString();
                row.OriginHand_vl = dtrows[0]["Result"].ToString();
                row.FinalHand_vl = dtrows[0]["FinalResult"].ToString();
                row.Rank_vl = dtrows[0][CasinoBO.CasinoLang("RankResult")].ToString();
                row.WinLoss_vl = dtrows[0]["WinResult"].ToString();

                row.HandNo2_vl = dtrows[1]["HandNoResult"].ToString();
                row.FinalHand2_vl = dtrows[1]["FinalResult"].ToString();
                row.Rank2_vl = dtrows[1][CasinoBO.CasinoLang("RankResult")].ToString();
                row.WinLoss2_vl = dtrows[1]["WinResult"].ToString();

                row.HandNo3_vl = dtrows[2]["HandNoResult"].ToString();
                row.FinalHand3_vl = dtrows[2]["FinalResult"].ToString();
                row.Rank3_vl = dtrows[2][CasinoBO.CasinoLang("RankResult")].ToString();
                row.WinLoss3_vl = dtrows[2]["WinResult"].ToString();

                row.HandNo4_vl = dtrows[3]["HandNoResult"].ToString();
                row.FinalHand4_vl = dtrows[3]["FinalResult"].ToString();
                row.Rank4_vl = dtrows[3][CasinoBO.CasinoLang("RankResult")].ToString();
                row.WinLoss4_vl = dtrows[3]["WinResult"].ToString();

                tpl.Content.Result2.Result2Row.Append(row);
            }
        }

        private void RenderTotal(CasinoResult_4hJOB_Template tpl, DataRow dr)
        {
            var tmp = TableDetail.Rows.Count == 1 ? string.Empty : "(" + TableDetail.Rows[0]["CoinSize"].ToString() + ")";

            tpl.Content.bet = Resources.Casino.Bet;
            tpl.Content.bet_vl = string.Join(" ", new string[] { dr["Currency"].ToString(), this.NumberReport(dr["Bet"]) }) + tmp;
            tpl.Content.winloss = Lang.win;

            var dec = decimal.Round((decimal)dr["WinLoss"], 2);
            tpl.Content.winloss_vl = string.Join(" ", new string[] { dr["Currency"].ToString(), this.NumberReport(dec) });
        }

        private string NumberReport(object number)
        {
            decimal dcNumber = Convert.IsDBNull(number) ? 0 : (decimal)number;
            string resultText = Formater.DecFormat(number, 2);
            string spanClass = dcNumber >= 0 ? "bl_underdog" : "bl_favorite";
            return string.Format("<span class='{0}'>{1}</span>", spanClass, resultText);
        }
    }
}