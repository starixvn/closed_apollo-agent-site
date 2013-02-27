// --------------------------------------------------------------------------
// <copyright file="CasinoResult_Champion.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Andy.Nguyen" date="2012.6.8">Created</change>
// </history>

namespace Sunplus.Agent.UI._BetList
{
    using System;
    using System.Data;

    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;

    using Resources;

    using Sunplus.Agent.Model.BL.Casino;

    /// <summary>
    /// Hand history detail for Champion casino game
    /// </summary>
    public partial class CasinoResult_Champion : TplPage
    {
        private int userBetId = 0;
        private string gameTypeNo = string.Empty;
        private string currency = string.Empty;

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

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            // RenderData(writer);
            // return;
            var tpl = new CasinoResult_Champion_Template();

            DataRowCollection rows = null;
            DataRow rowTotal = null;

            if (this.TableDetail != null)
            {
                rows = this.TableDetail.Rows;
            }

            if (this.TableTotal != null && this.TableTotal.Rows.Count > 0)
            {
                rowTotal = this.TableTotal.Rows[0];
                this.currency = rowTotal["Currency"].ToString();
            }

            if (rows != null && rows.Count > 0)
            {
                tpl.NoInfo.Hide();

                // Clear blocks
                var hasLineNo = false;
                var hasTotalLine = false;
                var hasJackPot = false;

                var showType = string.Empty;

                foreach (DataRow dr in rows)
                {
                    showType = dr["ShowType"].ToString();

                    if (showType == "AddRow_HitBetLine")
                    {
                        hasLineNo = true;
                    }
                    else if (showType == "Add_TotalLineWin")
                    {
                        hasTotalLine = true;
                    }
                    else if (showType == "Add_JackpotGame")
                    {
                        hasJackPot = true;
                    }
                }

                if (!hasLineNo)
                {
                    tpl.content.lineNo.Clear();
                    tpl.content.lineNoRow.Clear();
                }

                if (!hasTotalLine)
                {
                    tpl.content.totalLine.Clear();
                }

                if (!hasJackPot)
                {
                    tpl.content.jackPotBlock.Clear();
                }

                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];

                    showType = row["ShowType"].ToString();

                    if (showType == "BasicInfo")
                    {
                        this.RenderBasicInfo(tpl, row);
                    }
                    else if (showType == "AddRow_HitBetLine")
                    {
                        this.RenderHitBetLineHeader(tpl);
                    }
                    else if (showType == "Result_HitBetLine")
                    {
                        this.RenderHitBetLineResult(tpl, row);
                    }
                    else if (showType == "Add_TotalLineWin")
                    {
                        this.RenderTotalLineWinLoss(tpl, row);
                    }
                    else if (showType == "Add_JackpotGame")
                    {
                        this.RenderJackPotGame(tpl, row);
                    }
                }

                // Renders total region
                this.RenderTotal(tpl, this.TableTotal.Rows[0]);

                // Outputs to HTML
                writer.Write(tpl);
            }
            else
            {
                tpl.NoInfo.Show();
                tpl.content.Hide();
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.gameTypeNo = this.Request["gametype"];
            int.TryParse(this.Request["userbetid"], out this.userBetId);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (this.userBetId == 0)
            {
                return new ErrorPage_Page(Lang.strNotAccessPage);
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var casino = new CasinoBO(CurrentUserSession.NetSessionId);
            var tables = casino.Report.WinLossDetail(this.userBetId);

            return tables;
        }

        private void RenderData(System.Web.UI.HtmlTextWriter writer)
        {
            var ds = View as DataTable[];

            var tableBasic = ds[0];
            var tableDetail = ds[1];

            writer.Write(this.RenderTable(tableBasic));
            writer.Write(this.RenderTable(tableDetail));

            return;
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

        private string CasinoLang(string original)
        {
            return CasinoBO.CasinoLang(original);
        }

        private void RenderBasicInfo(CasinoResult_Champion_Template tpl, DataRow row)
        {
            tpl.content.tablename = row[this.CasinoLang("TableName")] as string;
            tpl.content.datetime = row["BetTime"].ToString();

            tpl.content.Player = Lang.player;
            tpl.content.UserName = row["UserName"] as string;

            tpl.content.Result = row[this.CasinoLang("ResultName")] as string;
            tpl.content.result_vl = row["Result"] as string;

            tpl.content.historyNo = row[this.CasinoLang("PlayerHistoryNo")] as string;
            tpl.content.historyNo_vl = row["PlayerHistoryNo_Result"] as string;

            tpl.content.mode = row[this.CasinoLang("Mode")] as string;
            tpl.content.mode_vl = row[this.CasinoLang("ModeResult")] as string;

            tpl.content.autoSpin = row[this.CasinoLang("AutoSpin")] as string;
            tpl.content.autoSpin_vl = row[this.CasinoLang("AutoSpinResult")] as string;

            tpl.content.freeSpin = row[this.CasinoLang("FreeSpin")] as string;
            tpl.content.freeSpin_vl = row[this.CasinoLang("FreeSpinResult")] as string;
        }

        private void RenderHitBetLineHeader(CasinoResult_Champion_Template tpl)
        {
            tpl.content.lineNoRow.Clear();

            tpl.content.lineNo.lineNo = Lang.lineno;
            tpl.content.lineNo.symbol = Lang.symbol;
            tpl.content.lineNo.coinSize = Lang.coinsize;
            tpl.content.lineNo.bet = Casino.Bet;
            tpl.content.lineNo.odds = Lang.odds;
            tpl.content.lineNo.win = Lang.lblWin;
        }

        private void RenderHitBetLineResult(CasinoResult_Champion_Template tpl, DataRow row)
        {
            var rows = new CasinoResult_Champion_content_lineNoRow_Block();

            rows.lineNo_vl = row["LineNo"].ToString();
            rows.symbol_vl = row["Symbol"].ToString();
            rows.coinSize_vl = row["CoinSize"].ToString();
            rows.bet_vl = row["Bet"].ToString();
            rows.odds_vl = row["Odds"].ToString();
            rows.win_vl = row["Win"].ToString();

            tpl.content.lineNoRow.Append(rows);
        }

        private void RenderTotalLineWinLoss(CasinoResult_Champion_Template tpl, DataRow row)
        {
            tpl.content.totalLine.totalLineBet = row[this.CasinoLang("Win")].ToString();
            tpl.content.totalLine.totalLineBet_vl = this.FormatNumber(Convert.ToDecimal(row["Win_Result"].ToString().Replace(this.currency, string.Empty).Trim()));
        }

        private void RenderTotal(CasinoResult_Champion_Template tpl, DataRow row)
        {
            var dr = this.TableDetail.Rows[0];

            tpl.content.total.totalBet = dr[this.CasinoLang("TotalBet")].ToString();
            tpl.content.total.totalBet_vl = this.FormatNumber(row["Bet"]);

            tpl.content.total.winloss = dr[this.CasinoLang("TotalWinLoss")].ToString();
            tpl.content.total.winloss_vl = this.FormatNumber(row["WinLoss"]);
        }

        private void RenderJackPotGame(CasinoResult_Champion_Template tpl, DataRow row)
        {
            tpl.content.jackPotBlock.jackPot = row[this.CasinoLang("ResultName")].ToString();
            tpl.content.jackPotBlock.jackPotTitle = row[this.CasinoLang("Jackpot_Result")].ToString();
            tpl.content.jackPotBlock.jackPot_vl = row["JackpotWin_Result"].ToString();
        }

        private string FormatNumber(object number)
        {
            var dec = decimal.Round((decimal)number, 2);
            var spanClass = dec >= 0 ? "bl_underdog" : "bl_favorite";

            return string.Join(" ", new string[] { this.currency, string.Format("<span class='{0}'>{1}</span>", spanClass, dec.ToString("#,##0.##")) });
        }
    }
}