namespace Sunplus.Agent.UI._BetList
{
    using System.Data;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Resources;
    using Sunplus.Agent.Model.BL.Casino;

    public partial class CasinoResult_WesternZodiac : TplPage
    {
        private int userBetId = 0;

        private string CasinoLang(string original)
        {
            return CasinoBO.CasinoLang(original);
        }

        private void renderBasicInfo(CasinoResult_WesternZodiac_Template tpl, DataRow row)
        {
            tpl.content.tableName.tablename = row[this.CasinoLang("TableName")] as string;
            tpl.content.tableName.datetime = row["BetTime"].ToString();

            tpl.content.userName.Player = Lang.player;
            tpl.content.userName.UserName = row["UserName"] as string;

            tpl.content.resultHeader.Result = row[this.CasinoLang("ResultName")] as string;
            tpl.content.resultValue.result_vl = row["Result"] as string;
        }

        private void renderPlayerHistoryNo(CasinoResult_WesternZodiac_Template tpl, DataRow row)
        {
            tpl.content.history.historyNo = row[this.CasinoLang("PlayerHistoryNo")] as string;
            tpl.content.history.historyNo_vl = row["PlayerHistoryNo_Result"] as string;
        }

        private void renderMode(CasinoResult_WesternZodiac_Template tpl, DataRow row)
        {
            tpl.content.mode.mode = row[this.CasinoLang("Mode")] as string;
            tpl.content.mode.mode_vl = row[this.CasinoLang("ModeResult")] as string;
        }

        private void renderAutoSpin(CasinoResult_WesternZodiac_Template tpl, DataRow row)
        {
            tpl.content.autoSpin.autoSpin = row[this.CasinoLang("AutoSpin")] as string;
            tpl.content.autoSpin.autoSpin_vl = row[this.CasinoLang("AutoSpinResult")] as string;
        }

        private void renderFreeSpin(CasinoResult_WesternZodiac_Template tpl, DataRow row)
        {
            tpl.content.freeSpin.freeSpin = row[this.CasinoLang("FreeSpin")] as string;
            tpl.content.freeSpin.freeSpin_vl = row[this.CasinoLang("FreeSpinResult")] as string;
        }

        private void renderHitBetLineHeader(CasinoResult_WesternZodiac_Template tpl)
        {
            tpl.content.lineNo.lineNo = Lang.lineno;
            tpl.content.lineNo.symbol = Lang.symbol;
            tpl.content.lineNo.coinSize = Lang.coinsize;
            tpl.content.lineNo.betPerLine = Lang.betperline;
            tpl.content.lineNo.odds = Lang.odds;
            tpl.content.lineNo.win = Lang.win;
        }

        private void renderHitBetLineResult(CasinoResult_WesternZodiac_content_lineNo_Block tpl, DataRowCollection rows, int index)
        {
            var item = new CasinoResult_WesternZodiac_content_lineNo_lineNoRow_Block();
            var rowsCount = this.CountDetailRows(index, "Result_HitBetLine");

            for (var i = index; i < rowsCount + index; i++)
            {
                var row = rows[i];
                item.lineNo_vl = row["LineNo"] as string;
                item.symbol_vl = row["Symbol"] as string;
                item.coinSize_vl = row["CoinSize"] as string;
                item.betPerLine_vl = row["BetPerLine"] as string;
                item.odds_vl = row["Odds"] as string;
                item.win_vl = row["Win"] as string;

                tpl.lineNoRow.Append(item);
            }
        }

        private void renderHitScatterHeader(CasinoResult_WesternZodiac_Template tpl)
        {
            tpl.content.totalLine.totalLineBet = Lang.totallinebet;
            tpl.content.totalLine.symbol = Lang.symbol;
            tpl.content.totalLine.coinSize = Lang.coinsize;
            tpl.content.totalLine.betPerLine = Lang.betperline;
            tpl.content.totalLine.odds = Lang.odds;
            tpl.content.totalLine.win = Lang.win;
        }

        private void renderHitScatterResult(CasinoResult_WesternZodiac_content_totalLine_Block tpl, DataRowCollection rows, int index)
        {
            var item = new CasinoResult_WesternZodiac_content_totalLine_totalLineRow_Block();
            var rowsCount = this.CountDetailRows(index, "Result_HitScatter");

            for (var i = index; i < rowsCount + index; i++)
            {
                var row = rows[i];
                item.totalLineBet_vl = row["TotalLineBet"] as string;
                item.symbol_vl = row["Symbol"] as string;
                item.coinSize_vl = row["CoinSize"] as string;
                item.betPerLine_vl = row["BetPerLine"] as string;
                item.odds_vl = row["Odds"] as string;
                item.win_vl = row["Win"] as string;

                tpl.totalLineRow.Append(item);
            }
        }

        private void renderRollingReelHeader(CasinoResult_WesternZodiac_Template tpl)
        {
            tpl.content.rollingTitle.rollingReel = Lang.rollingReel;
            tpl.content.rolling.lineNo = Lang.lineno;
            tpl.content.rolling.symbol = Lang.symbol;
            tpl.content.rolling.coinSize = Lang.coinsize;
            tpl.content.rolling.betPerLine = Lang.betperline;
            tpl.content.rolling.odds = Lang.odds;
            tpl.content.rolling.win = Lang.win;
        }

        private void renderRollingReelResult(CasinoResult_WesternZodiac_Template parent, CasinoResult_WesternZodiac_content_rolling_Block tpl, DataRowCollection rows, int index)
        {
            var hasDetail = false;
            var tmpIndex = 0;
            var item = new CasinoResult_WesternZodiac_content_rolling_rollingRow_Block();
            var rolling = new CasinoResult_WesternZodiac_content_rolling_Block();
            var rowsCount = this.CountDetailRows(index, "Result_HitBetLine");

            for (var i = index; i < rowsCount + index; i++)
            {
                tmpIndex = i;
                var row = rows[i];

                item.lineNo_vl = row["LineNo"] as string;
                item.symbol_vl = row["Symbol"] as string;
                item.coinSize_vl = row["CoinSize"] as string;
                item.betPerLine_vl = row["BetPerLine"] as string;
                item.odds_vl = row["Odds"] as string;
                item.win_vl = row["Win"] as string;

                tpl.rollingRow.Append(item);
            }

            if (rows[tmpIndex + 1]["ShowType"] == "AddRow_HitBetLine")
            {
                rolling.rollingRow.Clear();
                rolling.lineNo = Lang.lineno;
                rolling.symbol = Lang.symbol;
                rolling.coinSize = Lang.coinsize;
                rolling.betPerLine = Lang.betperline;
                rolling.odds = Lang.odds;
                rolling.win = Lang.win;

                rowsCount = this.CountDetailRows(tmpIndex + 2, "Result_HitBetLine");

                hasDetail = true;
            }

            if (hasDetail)
            {
                for (var i = tmpIndex + 2; i < rowsCount + tmpIndex + 2; i++)
                {
                    var row = rows[i];

                    item.lineNo_vl = row["LineNo"] as string;
                    item.symbol_vl = row["Symbol"] as string;
                    item.coinSize_vl = row["CoinSize"] as string;
                    item.betPerLine_vl = row["BetPerLine"] as string;
                    item.odds_vl = row["Odds"] as string;
                    item.win_vl = row["Win"] as string;

                    rolling.rollingRow.Append(item);
                }

                parent.content.rolling.Append(rolling);
            }
        }

        private void renderMaxMultiply(CasinoResult_WesternZodiac_Template tpl, DataRow row)
        {
            if (row["showType"].ToString() == "MaxMultiply")
            {
                tpl.content.multiplier.multiplier = Lang.multiplier;
                tpl.content.multiplier.multiplier_vl = row["Multiplier_Result"] as string;
            }
        }

        private void renderTotal(CasinoResult_WesternZodiac_Template tpl, DataRow row)
        {
            tpl.content.total.bet = Resources.Casino.Bet;
            tpl.content.total.bet_vl = string.Join(" ", new string[] { row["Currency"].ToString(), decimal.Round((decimal)row["Bet"], 2).ToString() });
            tpl.content.total.winloss = Lang.win;

            var dec = decimal.Round((decimal)row["WinLoss"], 2);
            var spanClass = dec >= 0 ? "bl_underdog" : "bl_favorite";
            tpl.content.total.winloss_vl = string.Join(" ", new string[] { row["Currency"].ToString(), string.Format("<span class='{0}'>{1}</span>", spanClass, dec.ToString()) });
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            //writer.Write(this.RenderTable(this.TableDetail));
            //writer.Write(this.RenderTable(this.TableTotal));

            var tpl = new CasinoResult_WesternZodiac_Template();
            var rows = this.TableDetail.Rows;

            var hasHitBetLine = false;
            var hasHitScatter = false;
            var hasRollingReel = false;
            var hasMultiplier = false;

            //Clear values
            tpl.content.lineNo.lineNoRow.Clear();
            tpl.content.rolling.rollingRow.Clear();
            tpl.content.totalLine.totalLineRow.Clear();

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
                    else if (showType == "AddRow_HitBetLine" && !hasHitBetLine)
                    {
                        this.renderHitBetLineHeader(tpl);
                        this.renderHitBetLineResult(tpl.content.lineNo, rows, i + 1);
                        hasHitBetLine = true;
                    }
                    else if (showType == "AddRow_RollingReel" && !hasRollingReel)
                    {
                        this.renderRollingReelHeader(tpl);
                        this.renderRollingReelResult(tpl, tpl.content.rolling, rows, i + 2);
                        hasRollingReel = true;
                    }
                    else if (showType == "AddRow_HitScatter" && !hasHitScatter)
                    {
                        this.renderHitScatterHeader(tpl);
                        this.renderHitScatterResult(tpl.content.totalLine, rows, i + 1);
                        hasHitScatter = true;
                    }
                    else if (showType == "MaxMultiply" && !hasMultiplier)
                    {
                        this.renderMaxMultiply(tpl, row);
                        hasMultiplier = true;
                    }

                    if (hasMultiplier && hasRollingReel && hasHitBetLine && hasHitScatter)
                    {
                        break;
                    }
                }

                //Clear blocks
                if (!hasHitBetLine)
                {
                    tpl.content.lineNo.Clear();
                }
                if (!hasHitScatter)
                {
                    tpl.content.totalLine.Clear();
                }
                if (!hasRollingReel)
                {
                    tpl.content.rollingTitle.Clear();
                    tpl.content.rolling.Clear();
                }
                if (!hasMultiplier)
                {
                    tpl.content.multiplier.Clear();
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

        private int CountDetailRows(int fromIndex, string showType)
        {
            var result = 0;
            var rows = this.TableDetail.Rows;

            for (var i = fromIndex; i < rows.Count; i++)
            {
                if (rows[i]["ShowType"] == showType)
                {
                    result++;
                }
                else
                {
                    break;
                }
            }

            return result;
        }
    }
}