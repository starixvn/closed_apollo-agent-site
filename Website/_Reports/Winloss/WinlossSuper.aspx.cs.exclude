using System;
using System.Collections.Generic;
using System.Web.UI;
using Fanex.Age.UI.Components;
using System.Data;
using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.Definitions;

/*
 * Created 20100706@Don - Render Win loss report
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.UI.Reports.Winloss
{
    public partial class WinlossSuper_Page : WinlossReports_Page
    {
        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {
            WinlossSuper_Template tpl = new WinlossSuper_Template();
            tpl.actionForm = "action='WinlossSuper.aspx?" + Request.QueryString.ToString() + "'";

            if (_exportToExcel)
            {

                DataTable reportData = (DataTable)View;
                var resMan = GetLangResourceManager();
                var builder = new Fanex.Age.Excel.Reports.Winloss.WinlossSuperReportBuilder(reportData, IsBold, _dateFrom, _dateTo, resMan);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "WinlossSuper.xls");
                    excelStream.Dispose();
                    return;
                }

                // Need to render report content only
                tpl.ExcelContents.BoldRow = string.Empty;
                RenderReportContent(tpl.ExcelContents);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "WinlossSuper");
            }
            else
            {
                // Render Report Header
                RenderReportHeader(tpl);
                RenderReportContent(tpl.ExcelContents);
                RenderHiddenField(tpl);

                writer.Write(tpl);
            }

            RegisterSortingJavascriptObject();
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            base.ProcessRequest(ref p);
            _userType = UserTypes.Super;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            DataTable reportData = null;
            if (!_isSwitched)
            {
                reportData = _ReportBO.GetWinlossSuper(
                    CurrentUserSession.CustId,
                    _dateFrom,
                    _dateTo,
                    _permission);
                reportData = SortData(reportData);
            }
            return reportData != null ? reportData : new DataTable();
        }

        #endregion



        #region Helper

        /// <summary>
        /// Render Report Header
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportHeader(WinlossSuper_Template template)
        {
            template.SearchPanel = RenderSearchPanel(true);
            template.ExcelContents.BoldRow = this.BoldRowClass;
        }
        /// <summary>
        /// Render report content header
        /// Assign DateFrom, DateTo, Title
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderReportContentHeader(WinlossSuper_ExcelContents_Block excelBlock)
        {
            // Render date from and date to
            excelBlock.DateFrom = Formater.ShortDateFormat(_dateFrom);
            excelBlock.DateTo = Formater.ShortDateFormat(_dateTo);
        }
        /// <summary>
        /// Render Report Content
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderReportContent(WinlossSuper_ExcelContents_Block excelBlock)
        {
            DataTable reportData = View as DataTable;

            // Render date from and date to
            RenderReportContentHeader(excelBlock);

            int numRow = reportData.Rows.Count;
            excelBlock.Content.Clear();
            excelBlock.ContentTotal.Clear();
            if (numRow > 0)
            {
                WinlossSuper_ExcelContents_Content_Block contentBlock = new WinlossSuper_ExcelContents_Content_Block();
                foreach (DataRow row in reportData.Rows)
                {
                    contentBlock.ColorClass = (numRow % 2) == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                    numRow--;

                    CheckClosedAccount(row, contentBlock);

                    contentBlock.UserName = row["username"].ToString();
                    contentBlock.DownlineMemberId = row["custid"].ToString();
                    contentBlock.FdateDownline = Formater.ShortDateFormat(_dateFrom);
                    contentBlock.TdateDownline = Formater.ShortDateFormat(_dateTo);
                    contentBlock.ActionPage = "WinlossMaster.aspx";
                    contentBlock.turnover = Formater.DecFormat(row["turnover"], 2);
                    contentBlock.gcomm = Formater.DecFormat(row["grosscomm"], 2);
                    contentBlock.total = Formater.DecFormat(row["magtotal"], 2);
                    contentBlock.winloss = Formater.DecFormat(row["sagwinlost"], 2);
                    contentBlock.com = Formater.DecFormat(row["comm"], 2);
                    contentBlock.totals = Formater.DecFormat(row["sagtotal"], 2);
                    contentBlock.company = Formater.DecFormat(row["company"], 2);

                    excelBlock.Content.Append(contentBlock);
                }

                // Render Total Summary
                WinlossSuper_ExcelContents_ContentTotal_Block totalBlock = new WinlossSuper_ExcelContents_ContentTotal_Block();
                totalBlock.total_turnover = Formater.DecFormat(reportData.Compute("SUM(turnover)", ""), 2);
                totalBlock.total_gcomm = Formater.DecFormat(reportData.Compute("SUM(grosscomm)", ""), 2);
                totalBlock.total_total = Formater.DecFormat(reportData.Compute("SUM(magtotal)", ""), 2);
                totalBlock.total_winloss = Formater.DecFormat(reportData.Compute("SUM(sagwinlost)", ""), 2);
                totalBlock.total_com = Formater.DecFormat(reportData.Compute("SUM(comm)", ""), 2);
                totalBlock.total_totals = Formater.DecFormat(reportData.Compute("SUM(sagtotal)", ""), 2);
                totalBlock.total_company = Formater.DecFormat(reportData.Compute("SUM(company)", ""), 2);

                excelBlock.Content.Append(totalBlock);

                excelBlock.NoInfo.Hide();
            }
            else
            {
                excelBlock.NoInfo.NoInfoRow.NumCols = "8";
                if (_isSwitched)
                {
                    excelBlock.Clear();
                }
            }
        }

        /// <summary>
        /// Render hidden values for Search condition and downline Url
        /// </summary>
        /// <param name="template"></param>
        private void RenderHiddenField(WinlossSuper_Template template)
        {
            template.CustId = _custId.ToString();
            template.IsHistoryReport = (_isHistoryReport ? "1" : "0");
            template.IsSwitch = "0"; //(_isSwitched ? "1" : "0");
            template.UserName = string.Empty;
            template.BetType = string.Empty;
            RenderFilterElementNames();
        }

        private void CheckClosedAccount(DataRow row, WinlossSuper_ExcelContents_Content_Block contentBlock)
        {
            string fieldName = "closed";
            if (row.Table.Columns.Contains(fieldName) && (Convert.ToInt32(row[fieldName]) != 0))
            {
                contentBlock.ColorClass = CLOSED_ROW_CLASS;
            }
        }       
        #endregion
    }
}
