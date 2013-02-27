namespace Sunplus.Agent.UI._BetList
{
    using System.Data;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Resources;
    using Sunplus.Agent.Model.BL.Casino;
    using TplCore.Element;

    public partial class CasinoResult_Engagement2 : TplPage
    {
        private int userBetId = 0;

        private string CasinoLang(string original)
        {
            return CasinoBO.CasinoLang(original);
        }

        private void renderBasicInfo(CasinoResult_Engagement2_Template tpl, DataRow row)
        {
            tpl.tableName.tablename = row[this.CasinoLang("TableName")] as string;
            tpl.tableName.datetime = row["BetTime"].ToString();

            tpl.userName.Player = Lang.player;
            tpl.userName.UserName = row["UserName"] as string;

            tpl.resultHeader.Result = row[this.CasinoLang("ResultName")] as string;
            tpl.resultValue.result_vl = row["Result"] as string;
        }

        private void renderPlayerHistoryNo(CasinoResult_Engagement2_Template tpl, DataRow row)
        {
            tpl.history.historyNo = row[this.CasinoLang("PlayerHistoryNo")] as string;
            tpl.history.historyNo_vl = row["PlayerHistoryNo_Result"] as string;
        }

        private void renderAutoSpin(CasinoResult_Engagement2_Template tpl, DataRow row)
        {
            tpl.autoSpin.autoSpin = row[this.CasinoLang("AutoSpin")] as string;
            tpl.autoSpin.autoSpin_vl = row[this.CasinoLang("AutoSpinResult")] as string;
        }

        private void renderHitBonusGameHeader(Block tpl, DataRow row)
        {
            var b = new CasinoResult_Engagement2_Details_AddRow_BonusGame_Block();
            b.bonusGame = Lang.bonusGame;
            b.bonusNo = Lang.bonusNo;
            b.symbol = Lang.symbol;
            b.description = Lang.actionDesc;
            b.contents = Lang.reward;
            tpl.Append(b);
        }

        private void renderHitBonusGameResult(Block tpl, DataRow row)
        {
            var b = new CasinoResult_Engagement2_Details_Result_BonusGame_Block();
            b.bonusNo_vl = row[this.CasinoLang("ActionName")] as string;
            b.symbol_vl = row["Symbol"] as string;
            b.description_vl = row[this.CasinoLang("Descript")] as string;
            b.contents_vl = row["Contents_Result"] as string;
            tpl.Append(b);
        }

        private void renderHitSideGameHeader(Block tpl, DataRow row)
        {
            var b = new CasinoResult_Engagement2_Details_AddRow_HitSideGame_Block();
            b.sideGame = Lang.sideGame;
            b.name = Lang.name;
            b.result = Resources.Casino.Result;
            b.win = Lang.win;
            tpl.Append(b);
        }

        private void renderHitSideGameResult(Block tpl, DataRow row)
        {
            var b = new CasinoResult_Engagement2_Details_Result_HitSideGame_Block();
            b.name_vl = row[this.CasinoLang("ActionName")] as string;
            b.result_vl = row[this.CasinoLang("ActionResult")] as string;
            b.win_vl = row["Win"] as string;
            tpl.Append(b);
        }

        private void renderHitBetLineHeader(Block tpl)
        {
            var b = new CasinoResult_Engagement2_Details_AddRow_HitBetLine_Block();
            b.normalGame = Resources.Casino.normalGame;
            b.lineNo = Lang.lineno;
            b.symbol = Lang.symbol;
            b.coinSize = Lang.coinsize;
            b.betPerLine = Lang.betperline;
            b.odds = Lang.odds;
            b.win = Lang.lblWin;
            tpl.Append(b);
        }

        private void renderHitBetLineResult(Block tpl, DataRow row)
        {
            var b = new CasinoResult_Engagement2_Details_Result_HitBetLine_Block();
            b.lineNo_vl = row["LineNo"] as string;
            b.symbol_vl = row["Symbol"] as string;
            b.coinSize_vl = row["CoinSize"] as string;
            b.betPerLine_vl = row["BetPerLine"] as string;
            b.odds_vl = row["Odds"] as string;
            b.win_vl = row["Win"] as string;
            tpl.Append(b);
        }

        private void renderHitScatterHeader(Block tpl)
        {
            var b = new CasinoResult_Engagement2_Details_AddRow_HitScatter_Block();
            b.scatterGame = Resources.Casino.scatterGame;
            b.name = Resources.Casino.name;
            b.symbol = Lang.symbol;
            b.odds = Lang.odds;
            b.multiplier = Lang.multiplier;
            b.win = Lang.lblWin;
            tpl.Append(b);
        }

        private void renderHitScatterResult(Block tpl, DataRow row)
        {
            var b = new CasinoResult_Engagement2_Details_Result_HitScatter_Block();
            b.name_vl = row[this.CasinoLang("ActionName")] as string;
            b.symbol_vl = row["Symbol"] as string;
            b.odds_vl = row["Odds"] as string;
            b.multiplier_vl = row["Multiplier_Result"] as string;
            b.win_vl = row["Win"] as string;
            tpl.Append(b);
        }

        private void renderGameTotalWin(Block tpl, DataRow row)
        {
            var b = new CasinoResult_Engagement2_Details_AddRow_GameTotalWin_Block();
            b.win = Lang.lblWin;
            b.win_vl = row["Win"] as string;
            tpl.Append(b);
        }

        private void renderTotal(CasinoResult_Engagement2_Template tpl, DataRow row)
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
            var tpl = new CasinoResult_Engagement2_Template();
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
                    else if (showType == "AddRow_GameTotalWin")
                    {
                        this.renderGameTotalWin(tpl.Details, row);
                    }
                    else if (showType == "AddRow_HitScatter")
                    {
                        this.renderHitScatterHeader(tpl.Details);
                    }
                    else if (showType == "Result_HitScatter")
                    {
                        this.renderHitScatterResult(tpl.Details, row);
                    }
                    else if (showType == "AddRow_HitBonusGame")
                    {
                        this.renderHitBonusGameHeader(tpl.Details, row);
                    }
                    else if (showType == "Result_HitBonusGame")
                    {
                        this.renderHitBonusGameResult(tpl.Details, row);
                    }
                    else if (showType == "AddRow_HitSideGame")
                    {
                        this.renderHitSideGameHeader(tpl.Details, row);
                    }
                    else if (showType == "Result_HitSideGame")
                    {
                        this.renderHitSideGameResult(tpl.Details, row);
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