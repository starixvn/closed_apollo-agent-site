namespace Sunplus.Agent.UI._BetList
{
    using System.Data;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Resources;
    using Sunplus.Agent.Model.BL.Casino;
    using TplCore.Element;

    public partial class CasinoResult_DinoCamp : TplPage
    {
        private int userBetId = 0;

        private string CasinoLang(string original)
        {
            return CasinoBO.CasinoLang(original);
        }

        private void renderBasicInfo(CasinoResult_DinoCamp_Template tpl, DataRow row)
        {
            tpl.tableName.tablename = row[this.CasinoLang("TableName")] as string;
            tpl.tableName.datetime = row["BetTime"].ToString();

            tpl.userName.Player = Lang.player;
            tpl.userName.UserName = row["UserName"] as string;

            tpl.resultHeader.Result = row[this.CasinoLang("ResultName")] as string;
            tpl.resultValue.result_vl = row["Result"] as string;
        }

        private void renderPlayerHistoryNo(CasinoResult_DinoCamp_Template tpl, DataRow row)
        {
            tpl.history.historyNo = row[this.CasinoLang("PlayerHistoryNo")] as string;
            tpl.history.historyNo_vl = row["PlayerHistoryNo_Result"] as string;
        }

        private void renderAutoSpin(CasinoResult_DinoCamp_Template tpl, DataRow row)
        {
            tpl.autoSpin.autoSpin = row[this.CasinoLang("AutoSpin")] as string;
            tpl.autoSpin.autoSpin_vl = row[this.CasinoLang("AutoSpinResult")] as string;
        }

        private void renderHitBetLineHeader(Block tpl)
        {
            var b = new CasinoResult_DinoCamp_Details_AddRow_HitBetLine_Block();
            b.lineNo = Lang.lineno;
            b.symbol = Lang.symbol;
            b.coinSize = Lang.coinsize;
            b.odds = Lang.odds;
            b.win = Lang.lblWin;
            tpl.Append(b);
        }

        private void renderHitBetLineResult(Block tpl, DataRow row)
        {
            var b = new CasinoResult_DinoCamp_Details_Result_HitBetLine_Block();
            b.lineNo_vl = row["LineNo"] as string;
            b.symbol_vl = row["Symbol"] as string;
            b.coinSize_vl = row["CoinSize"] as string;
            b.odds_vl = row["Odds"] as string;
            b.win_vl = row["Win"] as string;
            tpl.Append(b);
        }

        private void renderTotalLineWinLoss(Block tpl, DataRow row)
        {
            var b = new CasinoResult_DinoCamp_Details_AddRow_TotalLineWinLoss_Block();
            b.win = Lang.win;
            b.win_vl = row["WinLoss_Result"] as string;
            tpl.Append(b);
        }

        private void renderBonus(Block tpl, DataRow row)
        {
            var b = new CasinoResult_DinoCamp_Details_AddRow_Bonus_Block();
            b.bonus = row[this.CasinoLang("Bonus")] as string;
            b.bonus_vl = row["Bonus_Result"] as string;
            tpl.Append(b);
        }

        private void renderTotal(CasinoResult_DinoCamp_Template tpl, DataRow row)
        {
            tpl.total.bet = Resources.Casino.totalBet;
            tpl.total.bet_vl = string.Join(" ", new string[] { row["Currency"].ToString(), decimal.Round((decimal)row["Bet"], 2).ToString() });
            tpl.total.winloss = Resources.Casino.totalWin;

            var dec = decimal.Round((decimal)row["WinLoss"], 2);
            var spanClass = dec >= 0 ? "bl_underdog" : "bl_favorite";
            tpl.total.winloss_vl = string.Join(" ", new string[] { row["Currency"].ToString(), string.Format("<span class='{0}'>{1}</span>", spanClass, dec.ToString()) });
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            var tpl = new CasinoResult_DinoCamp_Template();
            var rows = this.TableDetail.Rows;

            //Clear values
            tpl.Details.Clear();

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
                        this.renderAutoSpin(tpl, row);
                    }
                    else if (showType == "AddRow_HitBetLine")
                    {
                        this.renderHitBetLineHeader(tpl.Details);
                    }
                    else if (showType == "Result_HitBetLine")
                    {
                        this.renderHitBetLineResult(tpl.Details, row);
                    }
                    else if (showType == "Add_TotalLineWinLoss")
                    {
                        this.renderTotalLineWinLoss(tpl.Details, row);
                    }
                    else if (showType == "Add_Bonus")
                    {
                        this.renderBonus(tpl.Details, row);
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