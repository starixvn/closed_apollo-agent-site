namespace Sunplus.Agent.UI._BetList
{
    using System.Data;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Resources;
    using Sunplus.Agent.Model.BL.Casino;
    using TplCore.Element;

    public partial class CasinoResult_CrazyHospital : TplPage
    {
        private int userBetId = 0;

        private string CasinoLang(string original)
        {
            return CasinoBO.CasinoLang(original);
        }

        private void renderBasicInfo(CasinoResult_CrazyHospital_Template tpl, DataRow row)
        {
            tpl.tableName.tablename = row[this.CasinoLang("TableName")] as string;
            tpl.tableName.datetime = row["BetTime"].ToString();

            tpl.userName.Player = Lang.player;
            tpl.userName.UserName = row["UserName"] as string;

            tpl.resultHeader.Result = row[this.CasinoLang("ResultName")] as string;
            tpl.resultValue.result_vl = row["Result"] as string;
        }

        private void renderPlayerHistoryNo(CasinoResult_CrazyHospital_Template tpl, DataRow row)
        {
            tpl.history.historyNo = row[this.CasinoLang("PlayerHistoryNo")] as string;
            tpl.history.historyNo_vl = row["PlayerHistoryNo_Result"] as string;
        }

        private void renderMode(CasinoResult_CrazyHospital_Template tpl, DataRow row)
        {
            tpl.mode.mode = row[this.CasinoLang("Mode")] as string;
            tpl.mode.mode_vl = row[this.CasinoLang("ModeResult")] as string;
        }

        private void renderAutoSpin(CasinoResult_CrazyHospital_Template tpl, DataRow row)
        {
            tpl.autoSpin.autoSpin = row[this.CasinoLang("AutoSpin")] as string;
            tpl.autoSpin.autoSpin_vl = row[this.CasinoLang("AutoSpinResult")] as string;
        }

        private void renderTotalLineWin(Block tpl, DataRow row)
        {
            var b = new CasinoResult_CrazyHospital_Details_AddRow_Win_Block();
            b.win = row[this.CasinoLang("Win")] as string;
            b.win_vl = row["Win"] as string;
            tpl.Append(b);
        }

        private void renderHitBonusGame(Block tpl, DataRow row)
        {
            var b = new CasinoResult_CrazyHospital_Details_AddRow_BonusGame_Block();
            b.bonusGame = row[this.CasinoLang("BonusName")] as string;
            b.bonusGameWin = row[this.CasinoLang("Win")] as string;
            b.bonusGameWin_vl = row["Win"] as string;
            tpl.Append(b);
        }

        private void renderFreeSpinResult(Block tpl, DataRow row)
        {
            var b = new CasinoResult_CrazyHospital_Details_AddRow_FreeSpinResult_Block();
            b.freeSpinResult = row[this.CasinoLang("FreeSpinName")] as string;
            b.freeSpin = row[this.CasinoLang("FreeSpin")] as string;
            b.freeSpin_vl = row["FreeSpinTimes_Result"] as string;
            tpl.Append(b);
        }

        private void renderFreeSpin(CasinoResult_CrazyHospital_Template tpl, DataRow row)
        {
            tpl.freeSpin.freeSpin = row[this.CasinoLang("FreeSpin")] as string;
            tpl.freeSpin.freeSpin_vl = row[this.CasinoLang("FreeSpinResult")] as string;
        }

        private void renderMaxMultiply(Block tpl, DataRow row)
        {
            var b = new CasinoResult_CrazyHospital_Details_MaxMultiply_Block();
            b.multiplier = Lang.multiplier;
            b.multiplier_vl = row["Multiplier_Result"] as string;
            tpl.Append(b);
        }

        private void renderHitBetLineHeader(Block tpl)
        {
            var b = new CasinoResult_CrazyHospital_Details_AddRow_HitBetLine_Block();
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
            var b = new CasinoResult_CrazyHospital_Details_Result_HitBetLine_Block();
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
            var b = new CasinoResult_CrazyHospital_Details_AddRow_HitScatter_Block();
            b.totalLineBet = Lang.totallinebet;
            b.symbol = Lang.symbol;
            b.coinSize = Lang.coinsize;
            b.betPerLine = Lang.betperline;
            b.odds = Lang.odds;
            b.win = Lang.lblWin;
            tpl.Append(b);
        }

        private void renderHitScatterResult(Block tpl, DataRow row)
        {
            var b = new CasinoResult_CrazyHospital_Details_Result_HitScatter_Block();
            b.totalLineBet_vl = row["TotalLineBet"] as string;
            b.symbol_vl = row["Symbol"] as string;
            b.coinSize_vl = row["CoinSize"] as string;
            b.betPerLine_vl = row["BetPerLine"] as string;
            b.odds_vl = row["Odds"] as string;
            b.win_vl = row["Win"] as string;
            tpl.Append(b);
        }

        private void renderTotal(CasinoResult_CrazyHospital_Template tpl, DataRow row)
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
            var tpl = new CasinoResult_CrazyHospital_Template();
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
                        this.renderMode(tpl, row);
                        this.renderAutoSpin(tpl, row);
                        this.renderFreeSpin(tpl, row);
                    }
                    else if (showType == "AddRow_HitBetLine")
                    {
                        this.renderHitBetLineHeader(tpl.Details);
                    }
                    else if (showType == "Result_HitBetLine")
                    {
                        this.renderHitBetLineResult(tpl.Details, row);
                    }
                    else if (showType == "AddRow_TotalLineWin")
                    {
                        this.renderTotalLineWin(tpl.Details, row);
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
                        this.renderHitBonusGame(tpl.Details, row);
                    }
                    else if (showType == "AddRow_HitFreeSpin")
                    {
                        this.renderFreeSpinResult(tpl.Details, row);
                    }
                    else if (showType == "MaxMultiply")
                    {
                        this.renderMaxMultiply(tpl.Details, row);
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