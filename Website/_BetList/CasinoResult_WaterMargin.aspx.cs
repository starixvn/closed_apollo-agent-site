namespace Sunplus.Agent.UI._BetList
{
    using System.Data;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Resources;
    using Sunplus.Agent.Model.BL.Casino;

    public partial class CasinoResult_WaterMargin : TplPage
    {
        private int userBetId = 0;

        private string CasinoLang(string original)
        {
            return CasinoBO.CasinoLang(original);
        }

        private void RenderBasicInfo(CasinoResult_WaterMargin_Template tpl, DataRow row)
        {
            tpl.content.tableName.tablename = row[this.CasinoLang("TableName")] as string;
            tpl.content.tableName.datetime = row["BetTime"].ToString();

            tpl.content.userName.Player = Lang.player;
            tpl.content.userName.UserName = row["UserName"] as string;

            tpl.content.resultHeader.Result = row[this.CasinoLang("ResultName")] as string;
            tpl.content.resultValue.result_vl = row["Result"] as string;
        }

        private void RenderPlayerHistoryNo(CasinoResult_WaterMargin_Template tpl, DataRow row)
        {
            tpl.content.history.historyNo = row[this.CasinoLang("PlayerHistoryNo")] as string;
            tpl.content.history.historyNo_vl = row["PlayerHistoryNo_Result"] as string;
        }

        private void RenderMode(CasinoResult_WaterMargin_Template tpl, DataRow row)
        {
            tpl.content.mode.mode = row[this.CasinoLang("Mode")] as string;
            tpl.content.mode.mode_vl = row[this.CasinoLang("ModeResult")] as string;
        }

        private void RenderAutoSpin(CasinoResult_WaterMargin_Template tpl, DataRow row)
        {
            tpl.content.autoSpin.autoSpin = row[this.CasinoLang("AutoSpin")] as string;
            tpl.content.autoSpin.autoSpin_vl = row[this.CasinoLang("AutoSpinResult")] as string;
        }

        private void RenderFreeSpin(CasinoResult_WaterMargin_Template tpl, DataRow row)
        {
            tpl.content.freeSpin.freeSpin = row[this.CasinoLang("FreeSpin")] as string;
            tpl.content.freeSpin.freeSpin_vl = row[this.CasinoLang("FreeSpinResult")] as string;
        }

        private void RenderHitBetLineHeader(CasinoResult_WaterMargin_Template tpl)
        {
            tpl.content.lineNo.lineNo = Lang.lineno;
            tpl.content.lineNo.symbol = Lang.symbol;
            tpl.content.lineNo.coinSize = Lang.coinsize;
            tpl.content.lineNo.betPerLine = Lang.betperline;
            tpl.content.lineNo.odds = Lang.odds;
            tpl.content.lineNo.win = Lang.win;
        }

        private void RenderHitBetLineResult(CasinoResult_WaterMargin_content_lineNo_Block tpl, DataRowCollection rows, int index)
        {
            var item = new CasinoResult_WaterMargin_content_lineNo_lineNoRow_Block();
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

        private void RenderHitScatterHeader(CasinoResult_WaterMargin_Template tpl)
        {
            tpl.content.totalLine.totalLineBet = Lang.totallinebet;
            tpl.content.totalLine.symbol = Lang.symbol;
            tpl.content.totalLine.coinSize = Lang.coinsize;
            tpl.content.totalLine.betPerLine = Lang.betperline;
            tpl.content.totalLine.odds = Lang.odds;
            tpl.content.totalLine.win = Lang.win;
        }

        private void RenderHitScatterResult(CasinoResult_WaterMargin_content_totalLine_Block tpl, DataRowCollection rows, int index)
        {
            var item = new CasinoResult_WaterMargin_content_totalLine_totalLineRow_Block();
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

        private void RenderBonusGameHeader(CasinoResult_WaterMargin_Template tpl)
        {
            tpl.content.Bonus.bonusGame = Lang.bonusGame;
            tpl.content.Bonus.symbol = Lang.symbol;
            tpl.content.Bonus.name = Lang.name;
            tpl.content.Bonus.multiplier = Lang.multiplier;
            tpl.content.Bonus.win = Lang.win;
        }

        private void RenderBonusGameResult(CasinoResult_WaterMargin_content_Bonus_Block tpl, DataRowCollection rows, int index)
        {
            var item = new CasinoResult_WaterMargin_content_Bonus_bonusRow_Block();
            var rowsCount = this.CountDetailRows(index, "Result_HitBonus");

            for (int i = index; i < rowsCount + index; i++)
            {
                item.name_vl = rows[i][CasinoLang("BonusName")].ToString();
                item.symbol_vl = rows[i]["Symbol"].ToString();
                item.multiplier_vl = rows[i]["Multiplier"].ToString();
                item.win_vl = rows[i]["Win"].ToString();

                tpl.bonusRow.Append(item);
            }
        }

        private void RenderTotal(CasinoResult_WaterMargin_Template tpl, DataRow row)
        {
            tpl.content.total.bet = Resources.Casino.totalBet;
            tpl.content.total.bet_vl = string.Join(" ", new string[] { row["Currency"].ToString(), decimal.Round((decimal)row["Bet"], 2).ToString() });
            tpl.content.total.winloss = Lang.win;

            var dec = decimal.Round((decimal)row["WinLoss"], 2);
            var spanClass = dec >= 0 ? "bl_underdog" : "bl_favorite";
            tpl.content.total.winloss_vl = string.Join(" ", new string[] { row["Currency"].ToString(), string.Format("<span class='{0}'>{1}</span>", spanClass, dec.ToString()) });
        }

        private void RenderTotalWin(CasinoResult_WaterMargin_content_Win_Block tpl, DataRow row)
        {
            tpl.win = row[CasinoLang("Win")].ToString();
            tpl.win_vl = row["Win"].ToString();
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            //writer.Write(this.RenderTable(this.TableDetail));
            //writer.Write(this.RenderTable(this.TableTotal));

            var tpl = new CasinoResult_WaterMargin_Template();
            var rows = this.TableDetail.Rows;

            var hasHitBetLine = false;
            var hasHitScatter = false;
            var hasBonusGame = false;
            var hasTotalWin = false;

            //Clear values
            tpl.content.lineNo.lineNoRow.Clear();
            tpl.content.Bonus.bonusRow.Clear();
            tpl.content.totalLine.totalLineRow.Clear();

            if (rows != null && rows.Count > 0)
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    var showType = row["showType"] as string;

                    if (showType == "BasicInfo")
                    {
                        this.RenderBasicInfo(tpl, row);
                        this.RenderPlayerHistoryNo(tpl, row);
                        this.RenderMode(tpl, row);
                        this.RenderAutoSpin(tpl, row);
                        this.RenderFreeSpin(tpl, row);
                    }
                    else if (showType == "AddRow_HitBetLine" && !hasHitBetLine)
                    {
                        this.RenderHitBetLineHeader(tpl);
                        this.RenderHitBetLineResult(tpl.content.lineNo, rows, i + 1);
                        hasHitBetLine = true;
                    }
                    else if (showType == "AddRow_BonusGame" && !hasBonusGame)
                    {
                        this.RenderBonusGameHeader(tpl);
                        this.RenderBonusGameResult(tpl.content.Bonus, rows, i + 1);
                        hasBonusGame = true;
                    }
                    else if (showType == "AddRow_HitScatter" && !hasHitScatter)
                    {
                        this.RenderHitScatterHeader(tpl);
                        this.RenderHitScatterResult(tpl.content.totalLine, rows, i + 1);
                        hasHitScatter = true;
                    }
                    else if (showType == "AddRow_TotalLineWin" && !hasTotalWin)
                    {
                        this.RenderTotalWin(tpl.content.Win, row);
                        hasTotalWin = true;
                    }
                }

                //Clear blocks
                if (!hasHitBetLine)
                {
                    tpl.content.lineNo.Clear();
                }
                if (!hasTotalWin)
                {
                    tpl.content.Win.Clear();
                }
                if (!hasHitScatter)
                {
                    tpl.content.totalLine.Clear();
                }
                if (!hasBonusGame)
                {
                    tpl.content.Bonus.Clear();
                }

                //Renders total region
                this.RenderTotal(tpl, this.TableTotal.Rows[0]);

                //Outputs to HTML
                writer.Write(tpl);
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(Request["userbetid"], out this.userBetId);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (userBetId == 0)
            {
                return new ErrorPage_Page(Lang.lblNotAuthorized);
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var casino = new CasinoBO(CurrentUserSession.NetSessionId);
            var tables = casino.Report.WinLossDetail(userBetId);

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