using System;
using System.Collections.Generic;
using System.Web.UI;
using Fanex.Age.UI.Components;
using System.Data;
using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.Definitions;
using System.Collections;

/*
 * Created 20100714@Don - Render Win loss detail report
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.UI.Reports.Winloss
{
    public partial class WinlossDetailSuper_Page : WinlossReports_Page
    {
        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {
            WinlossDetailSuper_Template tpl = new WinlossDetailSuper_Template();

            if (_exportToExcel)
            {

                DataTable reportData = (DataTable)View;
                var resMan = GetLangResourceManager();
                var builder = new Fanex.Age.Excel.Reports.Winloss.WinlossDetailSuperReportBuilder(reportData, IsBold, _dateFrom, _dateTo, resMan);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "WinlossDetailSuper.xls");
                    excelStream.Dispose();
                    return;
                }

                // Need to render report content only
                tpl.ExcelContents.BoldRow = string.Empty;
                RenderReportContent(tpl);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "WinlossDetailSuper");
            }
            else
            {
                // Render Report Header
                RenderReportHeader(tpl);
                RenderReportContent(tpl);
                RenderHiddenField(tpl);

                writer.Write(tpl);
            }

            RegisterSortingJavascriptObject();
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            _isPaging = true; //this line is needed above the line - base.ProcessRequest(ref p);

            base.ProcessRequest(ref p);
            _userType = UserTypes.Super;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            DataTable reportData = null;
            Hashtable pagingParams = GetPagingParams();
            if (!_isSwitched)
            {
                reportData = _ReportBO.GetWinlossDetailSuper(
                    CurrentUserSession.CustId,
                    _dateFrom,
                    _dateTo,
                    _permission,
                    pagingParams);
                UpdateRowCount(pagingParams);
                reportData = SortData(reportData);
            }
            
            UpdateRowCount(pagingParams);

            return reportData != null ? reportData : new DataTable();
        }

        #endregion



        #region Helper

        /// <summary>
        /// Render Report Header
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportHeader(WinlossDetailSuper_Template template)
        {
            template.SearchPanel = RenderSearchPanel(true);
            template.ExcelContents.BoldRow = this.BoldRowClass;
        }
        /// <summary>
        /// Render report content header
        /// Assign DateFrom, DateTo, Title
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderReportContentHeader(WinlossDetailSuper_ExcelContents_Block excelBlock)
        {
            // Render date from and date to
            excelBlock.DateFrom = Formater.ShortDateFormat(_dateFrom);
            excelBlock.DateTo = Formater.ShortDateFormat(_dateTo);
        }
        /// <summary>
        /// Render Report Content
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderReportContent(WinlossDetailSuper_Template tpl)
        {
            DataTable reportData = View as DataTable;

            WinlossDetailSuper_ExcelContents_Block excelBlock = tpl.ExcelContents;
            excelBlock.Content.Clear();
            excelBlock.ContentTotal.Clear();
            // Render date from and date to
            RenderReportContentHeader(excelBlock);

            int numRow = reportData.Rows.Count;
            if (numRow > 0)
            {
                WinlossDetailSuper_ExcelContents_Content_Block contentBlock = new WinlossDetailSuper_ExcelContents_Content_Block();
                foreach (DataRow row in reportData.Rows)
                {
                    contentBlock.ColorClass = (numRow % 2) == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS; ;
                    numRow--;

                    CheckClosedAccount(row, contentBlock);

                    contentBlock.UserName = row["username"].ToString();
                    contentBlock.DownlineMemberId = row["custid"].ToString();
                    contentBlock.FdateDownline = Formater.ShortDateFormat(_dateFrom);
                    contentBlock.TdateDownline = Formater.ShortDateFormat(_dateTo);
                    contentBlock.ActionPage = "WinlossDetailMaster.aspx";
                    contentBlock.turnover_value = Formater.DecFormat(row["turnover"], 2);
                    contentBlock.grosscomm_value = Formater.DecFormat(row["grosscomm"], 2);
                    contentBlock.memberwin_value = Formater.DecFormat(row["memberwinlost"], 2);
                    contentBlock.membercomm_value = Formater.DecFormat(row["playercomm"], 2);
                    contentBlock.membertotal_value = Formater.DecFormat(row["membertotal"], 2);
                    contentBlock.agentwin_value = Formater.DecFormat(row["agwinlost"], 2);
                    contentBlock.agentcomm_value = Formater.DecFormat(row["agcomm"], 2);
                    contentBlock.agenttotal_value = Formater.DecFormat(row["agtotal"], 2);
                    contentBlock.masterwin_value = Formater.DecFormat(row["magwinlost"], 2);
                    contentBlock.mastercomm_value = Formater.DecFormat(row["magcomm"], 2);
                    contentBlock.mastertotal_value = Formater.DecFormat(row["magtotal"], 2);
                    contentBlock.superagentwin_value = Formater.DecFormat(row["sagwinlost"], 2);
                    contentBlock.superagentcomm_value = Formater.DecFormat(row["sagcomm"], 2);
                    contentBlock.superagenttotal_value = Formater.DecFormat(row["sagtotal"], 2);
                    contentBlock.company_value = Formater.DecFormat(row["company"], 2);

                    excelBlock.Content.Append(contentBlock);

                }

                // Render Total Summary
                WinlossDetailSuper_ExcelContents_ContentTotal_Block totalBlock = new WinlossDetailSuper_ExcelContents_ContentTotal_Block();
                totalBlock.turnover_footer = Formater.DecFormat(reportData.Compute("SUM(turnover)", ""), 2);
                totalBlock.grosscomm_footer = Formater.DecFormat(reportData.Compute("SUM(grosscomm)", ""), 2);
                totalBlock.memberwin_footer = Formater.DecFormat(reportData.Compute("SUM(memberwinlost)", ""), 2);
                totalBlock.membercomm_footer = Formater.DecFormat(reportData.Compute("SUM(playercomm)", ""), 2);
                totalBlock.membertotal_footer = Formater.DecFormat(reportData.Compute("SUM(membertotal)", ""), 2);
                totalBlock.agentwin_footer = Formater.DecFormat(reportData.Compute("SUM(agwinlost)", ""), 2);
                totalBlock.agentcomm_footer = Formater.DecFormat(reportData.Compute("SUM(agcomm)", ""), 2);
                totalBlock.agenttotal_footer = Formater.DecFormat(reportData.Compute("SUM(agtotal)", ""), 2);
                totalBlock.masterwin_footer = Formater.DecFormat(reportData.Compute("SUM(magwinlost)", ""), 2);
                totalBlock.mastercomm_footer = Formater.DecFormat(reportData.Compute("SUM(magcomm)", ""), 2);
                totalBlock.mastertotal_footer = Formater.DecFormat(reportData.Compute("SUM(magtotal)", ""), 2);
                totalBlock.superagentwin_footer = Formater.DecFormat(reportData.Compute("SUM(sagwinlost)", ""), 2);
                totalBlock.superagentcomm_footer = Formater.DecFormat(reportData.Compute("SUM(sagcomm)", ""), 2);
                totalBlock.superagenttotal_footer = Formater.DecFormat(reportData.Compute("SUM(sagtotal)", ""), 2);
                totalBlock.company_footer = Formater.DecFormat(reportData.Compute("SUM(company)", ""), 2);

                excelBlock.Content.Append(totalBlock);

                excelBlock.NoInfo.Hide();

                // Paging
                tpl.PagingControl = string.Empty;
                if (_isPaging)
                {
                    Paging_Control pagingControl = new Paging_Control(tpl, "WinlossDetailAgent_Paging", _pageSize, Request.Url.AbsoluteUri, _rowCount, PAGE_SIZE_COLLECTION);
                    tpl.PagingControl = pagingControl.ToString();
                }
            }
            else
            {
                excelBlock.NoInfo.NoInfoRow.NumCols = "16";
                if (_isSwitched)
                {
                    excelBlock.Clear();
                }
                tpl.PagingControl = string.Empty;
            }
        }

        /// <summary>
        /// Render hidden values for Search condition and downline Url
        /// </summary>
        /// <param name="template"></param>
        private void RenderHiddenField(WinlossDetailSuper_Template template)
        {
            template.CustId = _custId.ToString();
            template.IsHistoryReport = (_isHistoryReport ? "1" : "0");
            template.IsSwitch = "0"; //(_isSwitched ? "1" : "0");
            template.OldView = string.Empty;
            template.UserName = string.Empty;
            template.BetType = string.Empty;
            RenderFilterElementNames();
        }

        private void CheckClosedAccount(DataRow row, WinlossDetailSuper_ExcelContents_Content_Block contentBlock)
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
