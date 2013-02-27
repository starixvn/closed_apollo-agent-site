/*
 * Created 20110604@Thai - Render Bet Summary
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.UI.Reports.BetList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using Sunplus.Agent.Model.BL.BetSummary;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    public partial class BetSummary_Page : WinlossReports_Page
    {
        private int custId;
        private string userName = string.Empty;
        private DateTime dateFrom;
        private DateTime dateTo;

        private string option = string.Empty;
        private string lblBetList = string.Empty;
        private string selectedValue = string.Empty;

        public static string NumberMoney(object number)
        {
            decimal result = Cast.AsDecimal(number);
            return (result >= 0) ? "<span class='bl_underdog'>" + Formater.FormatNumber3(result, Sunplus.Agent.Model.Entities.UserSession.Current.Language)
                                   + "</span>" : "<span class='bl_favorite'>" + Formater.FormatNumber3(result, Sunplus.Agent.Model.Entities.UserSession.Current.Language) + "</span>";
        }

        public override void RenderContent(HtmlTextWriter writer)
        {
            BetSummary_Template tpl = new BetSummary_Template();
            List<BetSummaryEntity> betSummaryList = View as List<BetSummaryEntity>;

            // Add data to _jsData variables, access _jsData as JSON format
            this.RegisterJavascriptObject("chk_showsb", this.Request["chk_showsb"]);
            this.RegisterJavascriptObject("chk_showcasino", this.Request["chk_showcasino"]);
            this.RegisterJavascriptObject("chk_showrb", this.Request["chk_showrb"]);
            this.RegisterJavascriptObject("chk_showng", this.Request["chk_showng"]);
            this.RegisterJavascriptObject("chk_showbi", this.Request["chk_showbi"]);

            // Render Header
            tpl.title = Resources.Lang.betsummary + " - " + this.userName;
            this.RenderSearchPanel(tpl);

            // Render Content
            int c = betSummaryList.Count;

            if (0 == c)
            {
                // Remove content section
                tpl.Content = null;
                writer.Write(tpl);
                return;
            }
            else
            {
                // Remove no information section
                tpl.NoInfo = null;
            }

            tpl.Content.Clear();

            BetSummary_Content_Block rowContent = new BetSummary_Content_Block();

            for (int i = 0; i < c; i++)
            {
                BetSummaryEntity betSummary = betSummaryList[i];
                this.RenderBetSummary(rowContent, i, betSummary);
                tpl.Content.Append(rowContent);
            }

            writer.Write(tpl);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable hash)
        {
            if (this.Request["custid"] != null && this.Request["username"] != null)
            {
                int.TryParse(this.Request["custid"] + string.Empty, out this.custId);
                this.userName = this.Request["username"] + string.Empty;
            }

            this.option = (this.Request["option"] == null) ? "5" : this.Request["option"].ToString();

            this.lblBetList = Resources.Lang.betsummary;

            this.dateFrom = DateTime.Now;
            this.dateTo = DateTime.Now;
            if (!string.IsNullOrEmpty(this.Request["isdate"]))
            {
                if (this.Request["isdate"] == "-1")
                {
                    this.dateFrom = DateTime.Now.AddDays(-1);
                    this.dateTo = DateTime.Now.AddDays(-1);
                }
                else
                {
                    this.dateFrom = Cast.AsDateTime(this.Request["fdate"]);
                    this.dateTo = Cast.AsDateTime(this.Request["tdate"]);
                }
            }
            else
            {
                this.dateFrom = Cast.AsDateTime(this.Request["fdate"]);
                this.dateTo = Cast.AsDateTime(this.Request["tdate"]);
            }

            this.selectedValue = string.Join(null, new string[] { this.option, "^", this.lblBetList, "^", this.custId.ToString(), "^", this.userName });
        }

        public override object InvokeData(Hashtable p)
        {
            BetSummaryBO bo = new BetSummaryBO(CurrentUserSession.NetSessionId);

            return bo.GetBetSummaryList(this.custId, this.dateFrom, this.dateTo);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        protected void RenderSearchPanel(BetSummary_Template tpl)
        {
            this.MAX_REPORT_DATE = new DateTime(9999, 12, 31);

            this._dateFrom = this.dateFrom;
            this._dateTo = this.dateTo;

            tpl.DateRangeSelect.DateFrom = this._dateFrom;
            tpl.DateRangeSelect.DateTo = this._dateTo;

            DataTable dt = this.CreateBetListOption();
            tpl.BetListComboBox = HtmlGenerator.ComboBox("idoption", this.selectedValue, dt, "Opvalue", "option", "OnchangeData(this)");

            tpl.DateRangeSelect.SetDateRange(this.MIN_REPORT_DATE, this.MAX_REPORT_DATE);
        }

        private static void Render_Event_Choice(BetSummary_Content_Block rowContent, BetSummaryEntity entity)
        {
            int betType = entity.BetType;
            string betTeam = entity.BetTeam;

            string homeTeam = CacheRef.GetTeamName(entity.HomeId) + (Cast.AsBooleanInvariant(entity.IsNeutral) ? Resources.Lang.IsNeutral : string.Empty);
            string awayTeam = CacheRef.GetTeamName(entity.AwayId);
            string matchEvent = string.Join(null, new string[] { homeTeam, " -vs- ", awayTeam });

            if (betType == 1 || betType == 7)
            {
                if (betTeam == "h")
                {
                    rowContent.events = string.Join(null, new string[] { "<font color='blue'><b>", homeTeam, "</b></font>", " -vs- ", awayTeam });
                    rowContent.choice = homeTeam;
                }
                else
                {
                    rowContent.events = string.Join(null, new string[] { homeTeam, " -vs- ", "<font color='blue'><b>", awayTeam, "</b></font>" });
                    rowContent.choice = awayTeam;
                }
            }
            else if (betType == 3 || betType == 8)
            {
                rowContent.events = matchEvent;
                if (betTeam == "h")
                {
                    rowContent.choice = string.Join(null, new string[] { "<font color='blue'><b>", Resources.Lang.over, "</b></font>" });
                }
                else
                {
                    rowContent.choice = string.Join(null, new string[] { "<font color='blue'><b>", Resources.Lang.under, "</b></font>" });
                }
            }
            else if (betType == 16)
            {
                rowContent.events = matchEvent;
                rowContent.choice = betTeam;
            }
            else if (betType == 2)
            {
                rowContent.events = matchEvent;
                if (betTeam == "h")
                {
                    rowContent.choice = string.Join(null, new string[] { "<font color='blue'><b>", Resources.Lang.lblOdd, "</b></font>" });
                }
                else
                {
                    rowContent.choice = string.Join(null, new string[] { "<font color='blue'><b>", Resources.Lang.events, "</b></font>" });
                }
            }
        }

        private void RenderBetSummary(BetSummary_Content_Block rowContent, int i, BetSummaryEntity entity)
        {
            rowContent.eventDate = Formater.ShortDateFormat(entity.EventDate);

            Render_Event_Choice(rowContent, entity);

            rowContent.stake = NumberMoney(entity.TotalBet) + "<br>" + NumberMoney(entity.TotalStake);
            if (entity.Winning - entity.TotalStake >= 0)
            {
                rowContent.winning = NumberMoney(entity.Winning - entity.TotalStake);
            }
            else
            {
                rowContent.winning = NumberMoney(entity.Winning);
            }
        }

        private DataTable CreateBetListOption()
        {
            DataTable dt = new DataTable();

            DataColumn column;
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Opvalue";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "option";
            dt.Columns.Add(column);

            DataRow row;

            row = dt.NewRow();
            row["Opvalue"] = "0^";
            row["option"] = "--------------------------------";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Opvalue"] = "1^" + Resources.Lang.dailystatement + "^" + this.custId + "^" + this.userName;
            row["option"] = Resources.Lang.dailystatement;
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Opvalue"] = "3^" + Resources.Lang.betsummary + "^" + this.custId + "^" + this.userName;
            row["option"] = Resources.Lang.betsummary;
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Opvalue"] = "4^" + Resources.Lang.lblBetLists + "^" + this.custId + "^" + this.userName;
            row["option"] = Resources.Lang.lblBetLists;
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Opvalue"] = "5^" + Resources.Lang.lblBetListRunning + "^" + this.custId + "^" + this.userName;
            row["option"] = Resources.Lang.lblBetListRunning;
            dt.Rows.Add(row);

            return dt;
        }
    }
}