using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Data;
using Sunplus.Agent.Utilities;
using Fanex.Age.UI.Components;
using Sunplus.Agent.Model.BL.Definitions;

/*
 * Created 20100708@Don - Render Win loss report
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.UI.Reports.Winloss
{
    public partial class WinlossAgent_Page : WinlossReports_Page
    {
        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {
            WinlossAgent_Template tpl = new WinlossAgent_Template();
            tpl.actionForm = "action='WinlossAgent.aspx?" + Request.QueryString.ToString() + "'";

            if (_exportToExcel)
            {

                DataTable reportData = (DataTable)View;
                var resMan = GetLangResourceManager();
                var builder = new Fanex.Age.Excel.Reports.Winloss.WinlossAgentReportBuilder(reportData, IsBold, _dateFrom, _dateTo, resMan);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "WinlossAgent.xls");
                    excelStream.Dispose();
                    return;
                }

                // Need to render report content only
                tpl.ExcelContents.BoldRow = string.Empty;
                tpl.BoldExportExcel.Hide();
                RenderReportContent(tpl.ExcelContents);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "WinlossAgent");
            }
            else
            {
                // Render Search Panel and Report 
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

            _userType = UserTypes.Agent;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            DataTable reportData = null;
            try
            {
                if (!_isSwitched)
                {
                    reportData = _ReportBO.GetWinlossAgent(
                        _custId,
                        _dateFrom,
                        _dateTo,
                        _permission);
                    reportData = SortData(reportData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(_permission.Count.ToString(), ex);
            }
            return reportData != null ? reportData : new DataTable();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Render Report Header
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportHeader(WinlossAgent_Template template)
        {
            template.ExcelContents.BoldRow = this.BoldRowClass;

            bool isShowed = (_custId == 0 || _custId == CurrentUserSession.CustId);
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
        private void RenderReportContentHeader(WinlossAgent_ExcelContents_Block excelBlock)
        {
            // Render date from and date to
            excelBlock.DateFrom = Formater.ShortDateFormat(_dateFrom);
            excelBlock.DateTo = Formater.ShortDateFormat(_dateTo);
        }

        /// <summary>
        /// Render Report Content
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderReportContent(WinlossAgent_ExcelContents_Block excelBlock)
        {
            DataTable reportData = View as DataTable;

            // Render date from and date to
            RenderReportContentHeader(excelBlock);

            excelBlock.Content.Clear();
            excelBlock.ContentTotal.Clear();
            int numRow = reportData.Rows.Count;
            if (numRow > 0)
            {
                WinlossAgent_ExcelContents_Content_Block contentBlock = new WinlossAgent_ExcelContents_Content_Block();
                foreach (DataRow row in reportData.Rows)
                {
                    contentBlock.ColorClass = (numRow % 2) == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                    numRow--;

                    CheckClosedAccount(row, contentBlock);

                    contentBlock.UserName = row["username"].ToString();
                    contentBlock.DownlineMemberId = row["custid"].ToString();
                    contentBlock.FdateDownline = Formater.ShortDateFormat(_dateFrom);
                    contentBlock.TdateDownline = Formater.ShortDateFormat(_dateTo);
                    contentBlock.ActionPage = "../../_BetList/BetList.aspx";
                    contentBlock.turnover = Formater.DecFormat(row["turnover"], 2);
                    contentBlock.gcomm = Formater.DecFormat(row["grosscomm"], 2);
                    contentBlock.winlossmem = Formater.DecFormat(row["memberwinlost"], 2);
                    contentBlock.commem = Formater.DecFormat(row["playercomm"], 2);
                    contentBlock.totalmem = Formater.DecFormat(row["membertotal"], 2);
                    contentBlock.winlossagent = Formater.DecFormat(row["agwinlost"], 2);
                    contentBlock.comagent = Formater.DecFormat(row["agcomm"], 2);
                    contentBlock.totalagent = Formater.DecFormat(row["agtotal"], 2);
                    contentBlock.company = Formater.DecFormat(row["company"], 2);

                    excelBlock.Content.Append(contentBlock);
                }

                // Render Total Summary
                WinlossAgent_ExcelContents_ContentTotal_Block totalBlock = new WinlossAgent_ExcelContents_ContentTotal_Block();
                totalBlock.total_turnover = Formater.DecFormat(reportData.Compute("SUM(turnover)", ""), 2);
                totalBlock.total_gcomm = Formater.DecFormat(reportData.Compute("SUM(grosscomm)", ""), 2);
                totalBlock.total_winlossmem = Formater.DecFormat(reportData.Compute("SUM(memberwinlost)", ""), 2);
                totalBlock.total_commem = Formater.DecFormat(reportData.Compute("SUM(playercomm)", ""), 2);
                totalBlock.total_totalmem = Formater.DecFormat(reportData.Compute("SUM(membertotal)", ""), 2);
                totalBlock.total_winlossagent = Formater.DecFormat(reportData.Compute("SUM(agwinlost)", ""), 2);
                totalBlock.total_comagent = Formater.DecFormat(reportData.Compute("SUM(agcomm)", ""), 2);
                totalBlock.total_totalagent = Formater.DecFormat(reportData.Compute("SUM(agtotal)", ""), 2);
                totalBlock.total_company = Formater.DecFormat(reportData.Compute("SUM(company)", ""), 2);

                excelBlock.Content.Append(totalBlock);

                excelBlock.NoInfo.Hide();
            }
            else
            {
                excelBlock.NoInfo.NoInfoRow.NumCols = "10";
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
        private void RenderHiddenField(WinlossAgent_Template template)
        {
            template.CustId = _custId.ToString();
            template.IsHistoryReport = (_isHistoryReport ? "1" : "0");
            template.IsSwitch = "0"; //(_isSwitched ? "1" : "0");
            template.UserName = string.Empty;
            template.BetType = Fanex.Age.UI.Common.BetListWinLossByDate;
            RenderFilterElementNames();

            // 1: working on current bet list (old)
            // 0: working on bet list already has redesigned
            template.IsWorkedOnOldBetList = "1";
        }

        private void CheckClosedAccount(DataRow row, WinlossAgent_ExcelContents_Content_Block contentBlock)
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
