namespace Sunplus.Agent.UI._BetList
{
    using System.Data;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Resources;
    using Sunplus.Agent.Model.BL.Casino;
    using TplCore.Element;

    public partial class CasinoResult_MayaPush : TplPage
    {
        private int userBetId = 0;

        private string CasinoLang(string original)
        {
            return CasinoBO.CasinoLang(original);
        }

        private void renderBasicInfo(CasinoResult_MayaPush_Template tpl, DataRow row)
        {
            tpl.tableName.tablename = row[this.CasinoLang("TableName")] as string;
            tpl.tableName.datetime = row["BetTime"].ToString();

            tpl.userName.Player = Lang.player;
            tpl.userName.UserName = row["UserName"] as string;

            tpl.resultHeader.Result = row[this.CasinoLang("ResultName")] as string;
            tpl.resultValue.result_vl = row["Result"] as string;
        }

        private void renderPlayerHistoryNo(CasinoResult_MayaPush_Template tpl, DataRow row)
        {
            tpl.history.historyNo = row[this.CasinoLang("PlayerHistoryNo")] as string;
            tpl.history.historyNo_vl = row["PlayerHistoryNo_Result"] as string;
        }

        private void renderAutoPlay(CasinoResult_MayaPush_Template tpl, DataRow row)
        {
            tpl.autoPlay = row[this.CasinoLang("AutoPlay")] as string;
            tpl.autoPlay_vl = row[this.CasinoLang("AutoPlayResult")] as string;
        }

        private void renderTotal(CasinoResult_MayaPush_Template tpl, DataRow row)
        {
            tpl.bet = Resources.Casino.totalBet;
            tpl.bet_vl = string.Join(" ", new string[] { row["Currency"].ToString(), decimal.Round((decimal)row["Bet"], 2).ToString() });
            tpl.odds = Resources.Casino.Odds;
            tpl.odds_vl = decimal.Round((decimal)row["Odds"], 2).ToString();

            tpl.winloss = Resources.Casino.totalWin;

            var dec = decimal.Round((decimal)row["WinLoss"], 2);
            var spanClass = dec >= 0 ? "bl_underdog" : "bl_favorite";
            tpl.winloss_vl = string.Join(" ", new string[] { row["Currency"].ToString(), string.Format("<span class='{0}'>{1}</span>", spanClass, dec.ToString()) });
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            var tpl = new CasinoResult_MayaPush_Template();
            var rows = this.TableDetail.Rows;

            //Clear values

            if (rows != null && rows.Count > 0)
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    var showType = row["showType"] as string;

                    if (showType == "BasicInfo")
                    {
                        this.renderBasicInfo(tpl, row);
                        this.renderPlayerHistoryNo(tpl, row);
                        this.renderAutoPlay(tpl, row);
                    }
                }

                //Renders total region
                this.renderTotal(tpl, this.TableTotal.Rows[0]);

                //Outputs to HTML
                writer.Write(tpl);
            }
            else
            {
                writer.Write(new ErrorPage_Page(Resources.Lang.noInfo));
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(Request["userbetid"], out this.userBetId);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (this.userBetId == 0)
            {
                return new ErrorPage_Page("Invalid user bet id!");
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var casino = new CasinoBO(CurrentUserSession.NetSessionId);
            var tables = casino.Report.WinLossDetail(this.userBetId);

            return tables;
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
    }
}