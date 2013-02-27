using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.Casino;
using Fanex.Age.UI.Reports;


/*
 * Created 20100529@Jacob - Render CSWinlossDetailMaster page
 * Revision:
 * ?@? - ... 
 * 
 */
namespace Fanex.Age.UI.Reports.CasinoWinloss
{

    public partial class CSWinlossDetailMaster_Page : Reports_Page
    {
        #region Variable
        private String _betType = "";
        #endregion

        #region Override
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            CSWinlossDetailMaster_Template tpl = new CSWinlossDetailMaster_Template();

            if (_exportToExcel)
            {

                DataTable reportData = (DataTable)View;
                var resMan = GetLangResourceManager();
                var builder = new Fanex.Age.Excel.Reports.CasinoWinloss.CSWinlossDetailMasterReportBuilder(reportData, IsBold, _dateFrom, _dateTo, resMan);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "CSWinlossDetail.xls");
                    excelStream.Dispose();
                    return;
                }

                // Need to render report content only
                tpl.ExcelContents.BoldRow = "";
                RenderReportContent(tpl.ExcelContents);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "CSWinlossDetailMaster");
            }
            else
            {
                // Render Report Header and history message
                RenderDateRange(tpl.ReportSearchHeader);
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
            _betType = GetStringRequest("btype", "");
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            CasinoBO casinoBO = new CasinoBO(CurrentUserSession.NetSessionId);
            DateTime dateFrom = _dateFrom < Sunplus.Agent.Model.BL.Reports.ReportsBO.BalanceUpDate ? Sunplus.Agent.Model.BL.Reports.ReportsBO.BalanceUpDate : _dateFrom;
            DataTable reportData = null;
            try
            {
                reportData = casinoBO.Report.WinLossBetTypeDetail(dateFrom, _dateTo, _betType, 0, CurrentUserSession.CustId, 0);
                reportData = SortData(reportData);
            }
            catch (Exception ex)
            {
                Logs.Write(ex.ToString());
            }

            return reportData;
        }
        #endregion

        #region Render Utility
        /// <summary>
        /// Render Report header
        /// </summary>
        /// <param name="headerTemplate"></param>
        private void RenderReportHeader(CSWinlossDetailMaster_Template template)
        {
            template.ExcelContents.BoldRow = this.BoldRowClass;
        }
        /// <summary>
        /// Render report content header
        /// Assign DateFrom, DateTo, Title
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderReportContentHeader(CSWinlossDetailMaster_ExcelContents_Block excelBlock)
        {
            // Render date from and date to
            excelBlock.DateFrom = Formater.ShortDateFormat(_dateFrom);
            excelBlock.DateTo = Formater.ShortDateFormat(_dateTo);
        }
        /// <summary>
        /// Rnder report content
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportContent(CSWinlossDetailMaster_ExcelContents_Block excelBlock)
        {
            DataTable reportData = View as DataTable;
            int numRow = 0;
            // Render date from and date to
            RenderReportContentHeader(excelBlock);

            excelBlock.Content.Clear();
            if (reportData != null)
            {
                numRow = reportData.Rows.Count;
            }

            if (numRow > 0)
            {
                CSWinlossDetailMaster_ExcelContents_Content_Block rowBlock = new CSWinlossDetailMaster_ExcelContents_Content_Block();

                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];


                    rowBlock.ColorClass = (i % 2 == 0) ? EVEN_ROW_CLASS : ODD_ROW_CLASS;

                    rowBlock.GameTypeName = dr[CasinoBO.CasinoLang("gametypename")].ToString();
                    rowBlock.TurnOver = Formater.DecFormat(dr["turnover"], 2);
                    rowBlock.Company = Formater.DecFormat(dr["Company"], 2);
                    rowBlock.TotalMaster = Formater.DecFormat(dr["AgentTotal"], 2);
                    rowBlock.Winloss = Formater.DecFormat(dr["MasterWinloss"], 2);
                    rowBlock.Commission = Formater.DecFormat(dr["MasterRolling"], 2);
                    rowBlock.TotalSuper = Formater.DecFormat(dr["MasterTotal"], 2);

                    // Add row
                    excelBlock.Content.Append(rowBlock);
                }
                // Total row
                excelBlock.TotalContent.TurnOver = Formater.DecFormat(reportData.Compute("SUM(turnover)", ""), 2);
                excelBlock.TotalContent.TotalMaster = Formater.DecFormat(reportData.Compute("SUM(AgentTotal)", ""), 2);
                excelBlock.TotalContent.Winloss = Formater.DecFormat(reportData.Compute("SUM(MasterWinloss)", ""), 2);
                excelBlock.TotalContent.Commission = Formater.DecFormat(reportData.Compute("SUM(MasterRolling)", ""), 2);
                excelBlock.TotalContent.TotalSuper = Formater.DecFormat(reportData.Compute("SUM(MasterTotal)", ""), 2);
                excelBlock.TotalContent.Company = Formater.DecFormat(reportData.Compute("SUM(Company)", ""), 2);
                // Hide noinfo row
                excelBlock.NoInfo.Hide();
            }
            else
            {
                excelBlock.TotalContent.Hide();
                excelBlock.NoInfo.NoInfoRow.NumCols = "7";
            }

        }
        /// <summary>
        /// Render Hidden field
        /// </summary>
        /// <param name="template"></param>
        private void RenderHiddenField(CSWinlossDetailMaster_Template template)
        {
            template.BetType = _betType;
        }
        #endregion
    }

}
