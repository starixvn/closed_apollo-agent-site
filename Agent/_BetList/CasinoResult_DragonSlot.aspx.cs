using System;
using System.Data;

using Fanex.Age.UI;
using Fanex.Age.UI.Components;

using Resources;

using Sunplus.Agent.Model.BL.Casino;

namespace Sunplus.Agent.UI._BetList
{
    public partial class CasinoResult_DragonSlot : TplPage
    {
        private int userBetId = 0;
        private string currency = string.Empty;
        private bool hasBonusGame = false;
        private bool hasMaxMultiply = false;

        private void RenderData(System.Web.UI.HtmlTextWriter writer)
        {
            var ds = View as DataTable[];

            var dtBasic = ds[0];
            var dtDetail = ds[1];

            writer.Write(RenderTable(dtBasic));
            writer.Write(RenderTable(dtDetail));
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

        private void renderBasicInfo(CasinoResult_DragonSlot_Template tpl, DataRow row)
        {
            tpl.content.tablename = row[this.CasinoLang("TableName")] as string;
            tpl.content.datetime = row["BetTime"].ToString();

            tpl.content.Player = Lang.player;
            tpl.content.UserName = row["UserName"] as string;

            tpl.content.Result = row[this.CasinoLang("ResultName")] as string;
            tpl.content.result_vl = row["Result"] as string;

            tpl.content.historyNo = row[this.CasinoLang("PlayerHistoryNo")] as string;
            tpl.content.historyNo_vl = row["PlayerHistoryNo_Result"] as string;

            tpl.content.autoSpin = row[this.CasinoLang("AutoSpin")] as string;
            tpl.content.autoSpin_vl = row[this.CasinoLang("AutoSpinResult")] as string;
        }

        private void renderHitBetLineHeader(CasinoResult_DragonSlot_Template tpl)
        {
            tpl.content.lineNo.lineNo = Lang.lineno;
            tpl.content.lineNo.symbol = Lang.symbol;
            tpl.content.lineNo.coinSize = Lang.coinsize;
            tpl.content.lineNo.odds = Lang.odds;
            tpl.content.lineNo.win = Lang.win;
        }

        private void renderHitBetLineResult(CasinoResult_DragonSlot_Template tpl, DataRow row)
        {
            var rows = new CasinoResult_DragonSlot_content_lineNo_lineNoRow_Block();

            rows.lineNo_vl = row["LineNo"].ToString();
            rows.symbol_vl = row["Symbol"].ToString();
            rows.coinSize_vl = row["CoinSize"].ToString();
            rows.odds_vl = row["Odds"].ToString();
            rows.win_vl = row["Win"].ToString();

            tpl.content.lineNo.lineNoRow.Append(rows);
        }

        private void renderTotalLineWinLoss(CasinoResult_DragonSlot_Template tpl, DataRow row)
        {
            tpl.content.totalLine.totalLineBet = row[CasinoLang("WinLoss")].ToString();
            tpl.content.totalLine.totalLineBet_vl = formatNumber(Convert.ToDecimal(row["WinLoss_Result"].ToString().Replace(currency, string.Empty).Trim()));
        }

        private void renderBonusGame(CasinoResult_DragonSlot_Template tpl, DataRow row)
        {
            /*
            tpl.content.bonusGame.bonusGame = row[CasinoLang("ResultName")].ToString();

            tpl.content.bonusGame.type = row[CasinoLang("Type")].ToString();
            tpl.content.bonusGame.type_vl = row[CasinoLang("Type_Result")].ToString();

            tpl.content.bonusGame.freeSpin = row[CasinoLang("FreeSpin")].ToString();
            tpl.content.bonusGame.freeSpinBonus_vl = row[CasinoLang("FreeSpinResult")].ToString();
             */

            tpl.content.addBonus.bonus = row[CasinoLang("Bouse")].ToString();
            tpl.content.addBonus.bonus_vl = row["Bouse_Result"].ToString();
        }

        private void renderTotal(CasinoResult_DragonSlot_Template tpl, DataRow row)
        {
            var dr = this.TableDetail.Rows[0];

            tpl.content.total.bet = dr[CasinoLang("TotalBet")].ToString();
            tpl.content.total.bet_vl = formatNumber(row["Bet"]);

            tpl.content.total.winloss = dr[CasinoLang("TotalWinLoss")].ToString();
            tpl.content.total.winloss_vl = formatNumber(row["WinLoss"]);
        }

        private string formatNumber(object number)
        {
            var dec = decimal.Round((decimal)number, 2);
            var spanClass = dec >= 0 ? "bl_underdog" : "bl_favorite";

            return string.Join(" ", new string[] { currency, string.Format("<span class='{0}'>{1}</span>", spanClass, dec.ToString()) });
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            //RenderData(writer);
            //return;

            var tpl = new CasinoResult_DragonSlot_Template();

            DataRowCollection rows = null;
            DataRow rowTotal = null;

            if (this.TableDetail != null)
                rows = this.TableDetail.Rows;

            if (this.TableTotal != null && this.TableTotal.Rows.Count > 0)
            {
                rowTotal = this.TableTotal.Rows[0];
                currency = rowTotal["Currency"].ToString();
            }

            if (rows != null && rows.Count > 0)
            {
                //Clear blocks
                var hasLineNo = false;
                var hasTotalLine = false;
                hasBonusGame = false;

                var showType = string.Empty;

                foreach (DataRow dr in rows)
                {
                    showType = dr["ShowType"].ToString();

                    if (showType == "AddRow_HitBetLine")
                    {
                        hasLineNo = true;
                    }
                    else if (showType == "Add_TotalLineWinLoss")
                    {
                        hasTotalLine = true;
                    }
                    else if (showType == "Add_Bonus")
                    {
                        hasBonusGame = true;
                    }
                }

                if (!hasLineNo)
                {
                    tpl.content.lineNo.Clear();
                }
                else
                {
                    tpl.content.lineNo.lineNoRow.Clear();
                }

                if (!hasTotalLine)
                {
                    tpl.content.totalLine.Clear();
                }

                if (!hasBonusGame)
                {
                    tpl.content.addBonus.Clear();
                }

                tpl.NoInfo.Hide();

                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    showType = row["ShowType"].ToString();

                    if (showType == "BasicInfo")
                    {
                        this.renderBasicInfo(tpl, row);
                    }
                    else if (showType == "AddRow_HitBetLine")
                    {
                        renderHitBetLineHeader(tpl);
                    }
                    else if (showType == "Result_HitBetLine")
                    {
                        renderHitBetLineResult(tpl, row);
                    }
                    else if (showType == "Add_TotalLineWinLoss")
                    {
                        renderTotalLineWinLoss(tpl, row);
                    }
                    else if (showType == "Add_Bonus")
                    {
                        renderBonusGame(tpl, row);
                    }
                }

                //Renders total region
                this.renderTotal(tpl, this.TableTotal.Rows[0]);

                //Outputs to HTML
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
    }
}