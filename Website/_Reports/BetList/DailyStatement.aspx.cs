using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Fanex.Age.UI.Components;
using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.Reports;
using Sunplus.Agent.UI.BetList.Factories;
using System.Text;
using Sunplus.Agent.Model.BL.DailyStatement;
using Sunplus.Agent.Model.Entities;
using System.Collections.Generic;
using Sunplus.Agent.UI.BetList.Utils;

/*
 * Created 20110604@Thai - Render DailyStatement
 * Revision:
 * ?@? - .. 
 * 
 */

namespace Fanex.Age.UI.Reports.BetList
{
    public partial class DailyStatement_Page : WinlossReports_Page
    {                        
        int custId;
        string userName = string.Empty;
        DateTime dateFrom;
        DateTime dateTo;
        
        string option = string.Empty;
        string lblBetList = string.Empty;
        string selectedValue = string.Empty;

        decimal dcBalance = 0;

        public override void RenderContent(HtmlTextWriter writer)
        {
            DailyStatement_Template tpl = new DailyStatement_Template();
            List<DailyStatementEntity> dailyStatementList = View as List<DailyStatementEntity>;
            
            // Add data to _jsData variables, access _jsData as JSON format
            RegisterJavascriptObject("chk_showsb", Request["chk_showsb"]);
            RegisterJavascriptObject("chk_showcasino", Request["chk_showcasino"]);
            RegisterJavascriptObject("chk_showrb", Request["chk_showrb"]);
            RegisterJavascriptObject("chk_showng", Request["chk_showng"]);
            RegisterJavascriptObject("chk_showbi", Request["chk_showbi"]);

            // Render Header
            tpl.title = Resources.Lang.dailystatement + " - " + userName;
            RenderSearchPanel(tpl);

            // Render Content
            int c = dailyStatementList.Count;

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

            DailyStatement_Content_Block rowContent = new DailyStatement_Content_Block();
                        
            for (int i = 0; i < c; i++)
            {
                DailyStatementEntity DailyStatement = dailyStatementList[i];                
                RenderDailyStatement(rowContent, i, DailyStatement);
                tpl.Content.Append(rowContent);
            }

            writer.Write(tpl);
        }

        private void RenderDailyStatement(DailyStatement_Content_Block rowContent, int i, DailyStatementEntity entity)
        {                      
            if ((i+1) % 2 == 0)
            {
                rowContent.row_class = "even_row";             
            }
            else
            {
                rowContent.row_class = "odd_row";                
            }

            int intTransType = 0;//All trans in date
            if (!string.IsNullOrEmpty(entity.Transdesc))
            {
                switch (entity.Transdesc.ToLower())
                {
                    case "betting statement"://for bets are settled
                        intTransType = 1;
                        break;
                    case "void/cancelled bet"://for bets are void and cancelled
                        intTransType = 2;
                        break;
                    case "outstanding bet"://for bets are running or waiting
                        intTransType = 3;
                        break;
                }
            }

            decimal credit = 0;
            decimal comm = Cast.AsDecimal(entity.Commission);

            rowContent.orderNo = (i+1).ToString();

            DateTime dtmWinLost = Cast.AsDateTime(entity.WinlostDate);

            SetDate(rowContent, entity, intTransType, dtmWinLost);

            SetContents(rowContent, entity, ref credit, comm, ref dtmWinLost);
        }

        private void SetContents(DailyStatement_Content_Block rowContent, DailyStatementEntity entity, ref decimal credit, decimal comm, ref DateTime dtmWinLost)
        {
            string transtype = entity.TransType.ToLower();

            rowContent.remark = entity.TransType + "<br/> -" + entity.Transdesc;

            if (DateTime.Compare(dtmWinLost, ReportsBO.BalanceUpDate) < 0 && string.Compare(transtype, "bet", true) == 0)
                rowContent.turnover = "--";
            else
                rowContent.turnover = NumberReport(Cast.AsDecimal(entity.Turnover));

            if (string.Compare(entity.Transdesc, "Outstanding Bet", true) == 0 || string.Compare(transtype, "bal", true) == 0)
                credit = 0;
            else
                credit = Cast.AsDecimal(entity.TotalAmt);

            if (string.Compare(transtype, "bal", true) == 0)
                dcBalance = Cast.AsDecimal(entity.TotalAmt);
            else
                dcBalance = dcBalance + comm + credit;

            rowContent.credit_debit = NumberReport(credit);
            rowContent.commission = NumberReport(comm);
            rowContent.balance = NumberReport(dcBalance);
        }

        private void SetDate(DailyStatement_Content_Block rowContent, DailyStatementEntity entity, int intTransType, DateTime dtmWinLost)
        {
            string transtype = entity.TransType.ToLower();
            if (transtype == "get" || transtype == "post" || transtype == "com" || transtype == "bet")
            {
                rowContent.dateNoLink.Visible = false;
                rowContent.dateLink.Visible = true;

                rowContent.dateLink.custid = custId.ToString();
                rowContent.dateLink.fdate = Formater.ShortDateFormat(dtmWinLost);
                rowContent.dateLink.transtype = intTransType.ToString();
                rowContent.dateLink.type = Fanex.Age.UI.Common.BetListByDailyStatement;
                rowContent.dateLink.uname = userName; ;
            }
            else
            {
                rowContent.dateLink.Visible = false;
                rowContent.dateNoLink.Visible = true;
            }

            if (transtype == "bet" || transtype == "bal")
            {
                rowContent.dateLink.date = dtmWinLost.ToString("ddddd") + "<br />" + Formater.ShortDateFormat(dtmWinLost);
            }
            else
            {
                rowContent.dateLink.date = dtmWinLost.ToString("ddddd") + "<br />" + Formater.FullDateFormat(dtmWinLost);
            }
            rowContent.dateNoLink.date = rowContent.dateLink.date;            
        }

        public override void ProcessRequest(ref System.Collections.Hashtable hash)
        {            
            if (Request["custid"] != null && Request["username"] != null)
            {
                int.TryParse(Request["custid"] + string.Empty, out custId);
                userName = Request["username"] + string.Empty;
            }

            option = (Request["option"] == null) ? "5" : Request["option"].ToString();

            #region Date

            lblBetList = Resources.Lang.dailystatement;

            dateFrom = DateTime.Now;
            dateTo = DateTime.Now;
            if (string.IsNullOrEmpty(Request["isdate"]))
            {
                dateFrom = Sunplus.Agent.Model.BL.Reports.ReportsBO.BalanceUpDate;
                dateTo = Cast.AsDateTime(Request["tdate"]);
                dateTo = dateTo.AddDays(7);
            }
            else
            {
                dateFrom = Cast.AsDateTime(Request["fdate"]);
                dateTo = Cast.AsDateTime(Request["tdate"]);
            }


            dateFrom = Sunplus.Agent.Utilities.Formater.ResetStartDate(dateFrom);
            dateTo = Sunplus.Agent.Utilities.Formater.ResetEndDate(dateTo);

            #endregion

            selectedValue = string.Join(null, new string[] { option, "^", lblBetList, "^", custId.ToString(), "^", userName });
        }

        public override object InvokeData(Hashtable p)
        {
            DailyStatementBO bo = new DailyStatementBO(CurrentUserSession.NetSessionId);

            return bo.GetDailyStatementList(custId, dateFrom, dateTo);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        #region Utility

        private decimal ToDecimal(object number)
        {
            return (Convert.IsDBNull(number) | number == null) ? 0 : (decimal)number;
        }

        /// <summary>
        /// Reformat text for number in this report
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string NumberReport(object number)
        {
            decimal dcNumber =  ToDecimal(number);
            string resultText = Formater.DecFormat(dcNumber, 2);
            string spanClass = dcNumber >= 0 ? "bl_underdog" : "bl_favorite";
            return string.Format("<span class='{0}'>{1}</span>", spanClass, resultText);
        }

        #endregion

        #region Helper

        protected void RenderSearchPanel(DailyStatement_Template tpl)
        {
            MAX_REPORT_DATE = new DateTime(9999, 12, 31);

            _dateFrom = this.dateFrom;
            _dateTo = this.dateTo;

            tpl.DateRangeSelect.DateFrom = _dateFrom;
            tpl.DateRangeSelect.DateTo = _dateTo;

            DataTable dt = createBetListOption();
            tpl.BetListComboBox = HtmlGenerator.ComboBox("idoption", selectedValue, dt, "Opvalue", "option", "OnchangeData(this)");

            tpl.DateRangeSelect.SetDateRange(MIN_REPORT_DATE, MAX_REPORT_DATE);
            
            tpl.DateRangeSelect._template.TodayYesterdayButton.Visible = false;            
        }

        private DataTable createBetListOption()
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
            row["Opvalue"] = "1^" + Resources.Lang.dailystatement + "^" + custId + "^" + userName;
            row["option"] = Resources.Lang.dailystatement;
            dt.Rows.Add(row);

            //row = dt.NewRow();
            //row["Opvalue"] = "3^" + Resources.Lang.betsummary + "^" + custid + "^" + username;
            //row["option"] = Resources.Lang.betsummary;
            //dt.Rows.Add(row);

            row = dt.NewRow();
            row["Opvalue"] = "4^" + Resources.Lang.lblBetLists + "^" + custId + "^" + userName;
            row["option"] = Resources.Lang.lblBetLists;
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Opvalue"] = "5^" + Resources.Lang.lblBetListRunning + "^" + custId + "^" + userName;
            row["option"] = Resources.Lang.lblBetListRunning;
            dt.Rows.Add(row);

            return dt;
        }

        #endregion
    }
}