using System;
using System.Collections.Generic;
using System.Web.UI;
using Fanex.Age.UI.Components;
using System.Data;
using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.Definitions;

/*
 * Created 20100708@Don - Render Win loss report
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.UI.Reports.Winloss
{
    public partial class WinlossMaster_Page : WinlossReports_Page
    {
        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {
            WinlossMaster_Template tpl = new WinlossMaster_Template();
            tpl.actionForm = "action='WinlossMaster.aspx?" + Request.QueryString.ToString() + "'";

            if (_exportToExcel)
            {

                DataTable reportData = (DataTable)View;
                var resMan = GetLangResourceManager();
                var builder = new Fanex.Age.Excel.Reports.Winloss.WinlossMasterReportBuilder(reportData, IsBold, _dateFrom, _dateTo, resMan);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "WinlossMaster.xls");
                    excelStream.Dispose();
                    return;
                }

                // Need to render report content only
                tpl.ExcelContents.BoldRow = string.Empty;
                tpl.BoldExportExcel.Hide();
                RenderReportContent(tpl.ExcelContents);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "WinlossMaster");
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
            _userType = UserTypes.Master;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            DataTable reportData = null;
            if (!_isSwitched)
            {
                reportData = _ReportBO.GetWinlossMaster(
                    _custId,
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
        private void RenderReportHeader(WinlossMaster_Template template)
        {
            template.ExcelContents.BoldRow = this.BoldRowClass;

            bool isShowed = (_custId == 0||_custId==CurrentUserSession.CustId);
            template.SearchPanel = RenderSearchPanel(isShowed);
            // Only show bold export excel button if search view is hidded
            if (isShowed) template.BoldExportExcel.Hide();
            else
            {
                template.BoldExportExcel.DateFrom = Formater.ShortDateFormat(_dateFrom);
                template.BoldExportExcel.DateTo = Formater.ShortDateFormat(_dateTo);
                template.BoldExportExcel.BoldExportExcel.Checked = this.BoldChecked;
            }
        }
        /// <summary>
        /// Render report content header
        /// Assign DateFrom, DateTo, Title
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderReportContentHeader(WinlossMaster_ExcelContents_Block excelBlock)
        {
            // Render date from and date to
            excelBlock.DateFrom = Formater.ShortDateFormat(_dateFrom);
            excelBlock.DateTo = Formater.ShortDateFormat(_dateTo);
        }
        /// <summary>
        /// Render Report Content
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderReportContent(WinlossMaster_ExcelContents_Block excelBlock)
        {
            DataTable reportData = View as DataTable;

            excelBlock.Content.Clear();
            excelBlock.ContentTotal.Clear();
            // Render date from and date to
            RenderReportContentHeader(excelBlock);
            
            int numRow = reportData.Rows.Count;
            if (numRow > 0)
            {
                WinlossMaster_ExcelContents_Content_Block contentBlock = new WinlossMaster_ExcelContents_Content_Block();
                foreach (DataRow row in reportData.Rows)
                {
                    contentBlock.ColorClass = (numRow % 2) == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                    numRow--;

                    CheckClosedAccount(row, contentBlock);

                    contentBlock.UserName = row["username"].ToString();
                    contentBlock.DownlineMemberId = row["custid"].ToString();
                    contentBlock.FdateDownline = Formater.ShortDateFormat(_dateFrom);
                    contentBlock.TdateDownline = Formater.ShortDateFormat(_dateTo);
                    contentBlock.ActionPage = "WinlossAgent.aspx";
                    contentBlock.turnover = Formater.DecFormat(row["turnover"], 2);
                    contentBlock.gcomm = Formater.DecFormat(row["grosscomm"], 2);
                    contentBlock.total = Formater.DecFormat(row["agtotal"], 2);
                    contentBlock.winloss = Formater.DecFormat(row["magwinlost"], 2);
                    contentBlock.com = Formater.DecFormat(row["comm"], 2);
                    contentBlock.totals = Formater.DecFormat(row["magtotal"], 2);
                    contentBlock.company = Formater.DecFormat(row["company"], 2);

                    excelBlock.Content.Append(contentBlock);

                }

                // Render Total Summary
                WinlossMaster_ExcelContents_ContentTotal_Block totalBlock = new WinlossMaster_ExcelContents_ContentTotal_Block();
                totalBlock.total_turnover = Formater.DecFormat(reportData.Compute("SUM(turnover)", ""), 2);
                totalBlock.total_gcomm = Formater.DecFormat(reportData.Compute("SUM(grosscomm)", ""), 2);
                totalBlock.total_total = Formater.DecFormat(reportData.Compute("SUM(agtotal)", ""), 2);
                totalBlock.total_winloss = Formater.DecFormat(reportData.Compute("SUM(magwinlost)", ""), 2);
                totalBlock.total_com = Formater.DecFormat(reportData.Compute("SUM(comm)", ""), 2);
                totalBlock.total_totals = Formater.DecFormat(reportData.Compute("SUM(magtotal)", ""), 2);
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
        private void RenderHiddenField(WinlossMaster_Template template)
        {
            template.CustId = _custId.ToString();
            template.IsHistoryReport = (_isHistoryReport ? "1" : "0");
            template.IsSwitch = "0"; //(_isSwitched ? "1" : "0");
            template.UserName = string.Empty;
            template.BetType = string.Empty;
            RenderFilterElementNames();
        }

        private void CheckClosedAccount(DataRow row, WinlossMaster_ExcelContents_Content_Block contentBlock)
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
