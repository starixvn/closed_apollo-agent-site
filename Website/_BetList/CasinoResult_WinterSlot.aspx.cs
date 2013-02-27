using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sunplus.Agent.Model.BL.Casino;
using Sunplus.Agent.Utilities;
using Fanex.Age.UI;
using System.Data;
using Fanex.Age.UI.Components;
using Resources;

namespace Sunplus.Agent.UI._BetList
{
    public partial class CasinoResult_WinterSlot : TplPage
    {
        private int userBetId = 0;
        private string gameTypeNo = null;
        private int refCount = 0;
        private int totalBlock = 1;

        private string CasinoLang(string original)
        {
            return CasinoBO.CasinoLang(original);
        }

        private void renderBasicInfo(CasinoResult_WinterSlot_Template tpl, DataRow row) {
            tpl.content.tableName.tablename = row[this.CasinoLang("TableName")] as string;
            tpl.content.tableName.datetime = ((DateTime)row["BetTime"]).ToString("MM/dd/yyyy");

            tpl.content.userName.Player = Lang.player;
            tpl.content.userName.UserName = row["UserName"] as string;

            tpl.content.resultHeader.Result = row[this.CasinoLang("ResultName")] as string;
            tpl.content.resultValue.result_vl = row["Result"] as string;
        }

        private void renderPlayerHistoryNo(CasinoResult_WinterSlot_Template tpl, DataRow row) {
            tpl.content.history.historyNo = row[this.CasinoLang("PlayerHistoryNo")] as string;
            tpl.content.history.historyNo_vl = row["PlayerHistoryNo_Result"] as string;
        }

        private void renderMode(CasinoResult_WinterSlot_Template tpl, DataRow row) {
            tpl.content.mode.mode = row[this.CasinoLang("Mode")] as string;
            tpl.content.mode.mode_vl = row[this.CasinoLang("ModeResult")] as string;
        }

        private void renderAutoSpin(CasinoResult_WinterSlot_Template tpl, DataRow row) {
            tpl.content.autoSpin.autoSpin = row[this.CasinoLang("AutoSpin")] as string;
            tpl.content.autoSpin.autoSpin_vl = row[this.CasinoLang("AutoSpinResult")] as string;
        }

        private void renderFreeSpin(CasinoResult_WinterSlot_Template tpl, DataRow row) {
            tpl.content.freeSpin.freeSpin = row[this.CasinoLang("FreeSpin")] as string;
            tpl.content.freeSpin.freeSpin_vl = row[this.CasinoLang("FreeSpinResult")] as string;
        }

        private void renderHitBetLineHeader(CasinoResult_WinterSlot_Template tpl) {
            tpl.content.lineNo.lineNo = Lang.lineno;
            tpl.content.lineNo.symbol = Lang.symbol;
            tpl.content.lineNo.coinSize = Lang.coinsize;
            tpl.content.lineNo.betPerLine = Lang.betperline;
            tpl.content.lineNo.odds = Lang.odds;
            tpl.content.lineNo.win = Lang.win;            
        }

        private void renderHitBetLineResult(CasinoResult_WinterSlot_content_lineNo_Block tpl, DataRowCollection rows, int index) {
            var item = new CasinoResult_WinterSlot_content_lineNo_lineNoRow_Block();            
            for (var i = index; i < rows.Count; i++)
            {
                var row = rows[i];
                if (row["showType"].ToString() == "Result_HitBetLine")
                {
                    item.lineNo_vl = row["LineNo"] as string;
                    item.symbol_vl = row["Symbol"] as string;
                    item.coinSize_vl = row["CoinSize"] as string;
                    item.betPerLine_vl = row["BetPerLine"] as string;
                    item.odds_vl = row["Odds"] as string;
                    item.win_vl = row["Win"] as string;

                    tpl.lineNoRow.Append(item);

                    this.refCount++;
                }
            }
            this.totalBlock++;
        }

        private void renderHitScatterHeader(CasinoResult_WinterSlot_Template tpl) {
            tpl.content.totalLine.totalLineBet = Lang.totallinebet;            
            tpl.content.totalLine.symbol = Lang.symbol;
            tpl.content.totalLine.coinSize = Lang.coinsize;
            tpl.content.totalLine.betPerLine = Lang.betperline;
            tpl.content.totalLine.odds = Lang.odds;
            tpl.content.totalLine.win = Lang.win;
        }

        private void renderHitScatterResult(CasinoResult_WinterSlot_content_totalLine_Block tpl, DataRowCollection rows, int index) {
            var item = new CasinoResult_WinterSlot_content_totalLine_totalLineRow_Block();
            for (var i = index; i < rows.Count; i++)
            {
                var row = rows[i];
                if (row["showType"].ToString() == "Result_HitScatter")
                {
                    item.totalLineBet_vl = row["TotalLineBet"] as string;
                    item.symbol_vl = row["Symbol"] as string;
                    item.coinSize_vl = row["CoinSize"] as string;
                    item.betPerLine_vl = row["BetPerLine"] as string;
                    item.odds_vl = row["Odds"] as string;
                    item.win_vl = row["Win"] as string;

                    tpl.totalLineRow.Append(item);

                    this.refCount++;
                }
            }
            this.totalBlock++;
        }
        
        private void renderRollingReelHeader(CasinoResult_WinterSlot_Template tpl) {
            tpl.content.rollingTitle.rollingReel = Lang.rollingReel;
            tpl.content.rolling.lineNo = Lang.lineno;
            tpl.content.rolling.symbol = Lang.symbol;
            tpl.content.rolling.coinSize = Lang.coinsize;
            tpl.content.rolling.betPerLine = Lang.betperline;
            tpl.content.rolling.odds = Lang.odds;
            tpl.content.rolling.win = Lang.win;
        }

        private void renderRollingReelResult(CasinoResult_WinterSlot_content_rolling_Block tpl, DataRowCollection rows, int index) {
            var item = new CasinoResult_WinterSlot_content_rolling_rollingRow_Block();
            for (var i = index; i < rows.Count; i++)
            {
                var row = rows[i];
                if (row["showType"].ToString() == "AddRow_RollingReel")
                {
                    item.lineNo_vl = row["LineNo"] as string;
                    item.symbol_vl = row["Symbol"] as string;
                    item.coinSize_vl = row["CoinSize"] as string;
                    item.betPerLine_vl = row["BetPerLine"] as string;
                    item.odds_vl = row["Odds"] as string;
                    item.win_vl = row["Win"] as string;

                    tpl.rollingRow.Append(item);

                    this.refCount++;
                }
            }
            this.totalBlock++;
        }

        private void renderMaxMultiply(CasinoResult_WinterSlot_Template tpl, DataRow row) {
            if (row["showType"].ToString() == "MaxMultiply")
            {
                tpl.content.multiplier.multiplier = Lang.multiplier;
                tpl.content.multiplier.multiplier_vl = row["Multiplier_Result"] as string;
            }
        }

        private void renderTotal(CasinoResult_WinterSlot_Template tpl, DataRow row) {
            tpl.content.total.bet = Resources.Casino.Bet;
            tpl.content.total.bet_vl = string.Join(" ", new string[] { row["Currency"].ToString(), decimal.Round((decimal)row["Bet"], 2).ToString() });
            tpl.content.total.winloss = Lang.win;
            
            var dec = decimal.Round((decimal)row["WinLoss"], 2);
            var spanClass = dec >= 0 ? "bl_underdog" : "bl_favorite";
            tpl.content.total.winloss_vl = string.Join(" ", new string[] { row["Currency"].ToString(), string.Format("<span class='{0}'>{1}</span>", spanClass, dec.ToString()) });            
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            this.totalBlock = 1;

            var tpl = new CasinoResult_WinterSlot_Template();
            var rows = this.TableDetail.Rows;
            var count = 0;

            var hasHitBetLine = false;
            var hasHitScatter = false;
            var hasRollingReel = false;
            var hasMultiplier = false;

            //Clear values
            tpl.content.lineNo.lineNoRow.Clear();
            tpl.content.rolling.rollingRow.Clear();
            tpl.content.totalLine.totalLineRow.Clear();
            tpl.content.multiplier.Clear();

            if (rows != null && rows.Count > 0) {
                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    var showType = row["showType"] as string;

                    if (showType == "BasicInfo")
                    {
                        this.renderBasicInfo(tpl, row);
                    }
                    else if (showType == "PlayHistoryNo")
                    {
                        this.renderPlayerHistoryNo(tpl, row);
                    }
                    else if (showType == "Mode")
                    {
                        this.renderMode(tpl, row);
                    }
                    else if (showType == "AutoSpin")
                    {
                        this.renderAutoSpin(tpl, row);
                    }
                    else if (showType == "FreeSpin")
                    {
                        this.renderFreeSpin(tpl, row);
                    }
                    else if (showType == "AddRow_HitBetLine")
                    {
                        this.renderHitBetLineHeader(tpl);
                        hasHitBetLine = true;
                    }
                    else if (showType == "Result_HitBetLine")
                    {
                        this.renderHitBetLineResult(tpl.content.lineNo, rows, i);
                    }
                    else if (showType == "AddRow_HitScatter")
                    {
                        this.renderHitScatterHeader(tpl);
                        hasHitScatter = true;
                    }
                    else if (showType == "Result_HitScatter")
                    {
                        this.renderHitScatterResult(tpl.content.totalLine, rows, i);
                    }
                    else if (showType == "AddRow_RollingReel")
                    {
                        this.renderRollingReelHeader(tpl);
                        this.renderRollingReelResult(tpl.content.rolling, rows, i);
                        hasRollingReel = true;
                    }
                    else if (showType == "MaxMultiply")
                    {
                        this.renderMaxMultiply(tpl, row);
                        hasMultiplier = true;
                    }

                    count++;
                    if (this.refCount > 0)
                        i = (this.refCount + count - this.totalBlock);
                }

                //Clear blocks
                if (!hasHitBetLine) {
                    tpl.content.lineNo.Clear();
                }
                if (!hasHitScatter) {
                    tpl.content.totalLine.Clear();
                }
                if (!hasRollingReel) {
                    tpl.content.rollingTitle.Clear();
                    tpl.content.rolling.Clear();
                }
                if (!hasMultiplier) {
                    tpl.content.multiplier.Clear();
                }

                //Renders total region
                this.renderTotal(tpl, this.TableTotal.Rows[0]);

                //Outputs to HTML
                writer.Write(tpl);
            }            
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
                return new ErrorPage_Page("Invalid user bet id!");
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var casino = new CasinoBO(CurrentUserSession.NetSessionId);
            var tables = casino.Report.WinLossDetail(userBetId);

            return tables;
        }

        private DataTable TableDetail {
            get {
                return ((DataTable[])View)[0];
            }
        }

        private DataTable TableTotal {
            get
            {
                return ((DataTable[])View)[1];
            }
        }        
    }
}