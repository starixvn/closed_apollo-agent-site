using System;
using System.Data;

using Fanex.Age.UI;
using Fanex.Age.UI.Components;

using Resources;

using Sunplus.Agent.Model.BL.Casino;

namespace Sunplus.Agent.UI._BetList
{
    public partial class CasinoResult_MoneySlot : TplPage
    {
        private int userBetId = 0;
        private string gameTypeNo = null;
        private string currency = null;

        private void RenderData(System.Web.UI.HtmlTextWriter writer)
        {
            var ds = View as DataTable[];

            var dtBasic = ds[0];
            var dtDetail = ds[1];

            writer.Write(RenderTable(dtBasic));
            writer.Write(RenderTable(dtDetail));

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

        private void renderBasicInfo(CasinoResult_MoneySlot_Template tpl, DataRow row)
        {
            tpl.content.tablename = row[this.CasinoLang("TableName")].ToString();
            tpl.content.datetime = row["BetTime"].ToString();

            tpl.content.Player = Lang.player;
            tpl.content.UserName = row["UserName"].ToString();

            tpl.content.Result = row[this.CasinoLang("ResultName")].ToString();
            tpl.content.result_vl = row["Result"].ToString();

            tpl.content.historyNo = row[this.CasinoLang("PlayerHistoryNo")].ToString();
            tpl.content.historyNo_vl = row["PlayerHistoryNo_Result"].ToString();
        }

        private void renderAutoSpin(CasinoResult_MoneySlot_Template tpl, DataRow row)
        {
            tpl.content.spin.autoSpin = row[this.CasinoLang("AutoSpin")].ToString();
            tpl.content.spin.autoSpin_vl = row[this.CasinoLang("AutoSpinResult")].ToString();
        }

        private void renderWinType(CasinoResult_MoneySlot_Template tpl)
        {
            tpl.content.winTypeRow.Clear();

            tpl.content.winType.winType = Lang.wintype;
            tpl.content.winType.symbol = Lang.symbol;
            tpl.content.winType.coinSize = Lang.coinsize;
            tpl.content.winType.odds = Lang.odds;
            tpl.content.winType.win = Lang.win;
            tpl.content.winType.bet = Lang.bet;
            tpl.content.winType.multiplier = Lang.multiplier;
        }

        private void renderRowWinType(CasinoResult_MoneySlot_Template tpl, DataRow row)
        {
            var block = new CasinoResult_MoneySlot_content_winTypeRow_Block();

            block.winType_vl = row[CasinoLang("ResultName")].ToString();
            block.symbol_vl = row["Symbol"].ToString();
            block.coinSize_vl = row["CoinSize"].ToString();
            block.odds_vl = row["Odds"].ToString();
            block.win_vl = row["Win"].ToString();
            block.bet_vl = row["Bet"].ToString();
            block.multiplier_vl = row["Multiplier"].ToString();

            tpl.content.winTypeRow.Append(block);
        }

        private void renderTotal(CasinoResult_MoneySlot_Template tpl, DataRow row)
        {
            var dr = this.TableDetail.Rows[0];

            tpl.content.total.bet = Resources.Casino.Bet;
            tpl.content.total.bet_vl = formatNumber(row["Bet"]);

            tpl.content.total.winloss = Lang.win;
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

            var tpl = new CasinoResult_MoneySlot_Template();

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
                tpl.NoInfo.Hide();

                var showType = string.Empty;

                var hasAutoSpin = false;
                var hasHitBetLine = false;

                for (int i = 0; i < rows.Count; i++)
                {
                    showType = rows[i]["ShowType"].ToString();

                    if (showType == "AutoSpin")
                    {
                        hasAutoSpin = true;
                    }

                    if (showType == "AddRow_HitBetLine")
                    {
                        hasHitBetLine = true;
                    }
                }

                if (!hasAutoSpin)
                    tpl.content.spin.Clear();
                if (!hasHitBetLine)
                {
                    tpl.content.winType.Clear();
                    tpl.content.winTypeRow.Clear();
                }

                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    showType = row["ShowType"].ToString();

                    if (showType == "BasicInfo")
                    {
                        this.renderBasicInfo(tpl, row);
                    }
                    else if (showType == "AutoSpin")
                    {
                        renderAutoSpin(tpl, row);
                    }
                    else if (showType == "AddRow_HitBetLine")
                    {
                        renderWinType(tpl);
                    }
                    else if (showType == "Result_HitBetLine")
                    {
                        renderRowWinType(tpl, row);
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