using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.Bingo;
using Sunplus.Bingo.Services;
using Sunplus.Bingo.Core;
using Microsoft.Practices.Unity;

using Fanex.Age.UI.Reports;

/*
 * Created 20100529@Jacob - Render BingoWinlossDetailSuper page
 * Revision:
 * ?@? - ... 
 * 
 */
namespace Fanex.Age.UI.Reports.BingoWinloss
{

    public partial class BingoWinlossDetailSuper_Page : Reports_Page
    {
        #region Override
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            BingoWinlossDetailSuper_Template tpl = new BingoWinlossDetailSuper_Template();

            if (_exportToExcel)
            {
                // Need to render report content only
                tpl.ExcelContents.BoldRow = "";
                RenderReportContent(tpl.ExcelContents);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "BingoWinlossDetailSuper");
            }
            else
            {
                // Render Report Header and history message
                RenderDateRange(tpl.ReportSearchHeader);
                RenderReportHeader(tpl);
                RenderReportContent(tpl.ExcelContents);
                writer.Write(tpl);
            }

            RegisterSortingJavascriptObject();
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            DateTime dateFrom = _dateFrom < Sunplus.Agent.Model.BL.Reports.ReportsBO.BalanceUpDate ? Sunplus.Agent.Model.BL.Reports.ReportsBO.BalanceUpDate : _dateFrom;
            IReportService bingoReportService = BingoContainer.Current.Resolve<IReportService>();
            DataTable winloss = null;
            try
            {
                winloss = bingoReportService.GetWinLossBetTypeDetail(dateFrom, _dateTo, _custId, 0, 0, BingoSettings.BingoGame.CodeGameNo);
                if (winloss == null) winloss = new DataTable();

                winloss = SortData(winloss);
            }
            catch (Exception ex)         
            {
                Logs.Write(ex.ToString());
            }

            return winloss;
        }
        #endregion

        #region Render Utility
        /// <summary>
        /// Render Report header
        /// </summary>
        /// <param name="headerTemplate"></param>
        private void RenderReportHeader(BingoWinlossDetailSuper_Template template)
        {
            template.ExcelContents.BoldRow = this.BoldRowClass;
        }
        /// <summary>
        /// Render report content header
        /// Assign DateFrom, DateTo, Title
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderReportContentHeader(BingoWinlossDetailSuper_ExcelContents_Block excelBlock)
        {
            // Render date from and date to
            excelBlock.DateFrom = Formater.ShortDateFormat(_dateFrom);
            excelBlock.DateTo = Formater.ShortDateFormat(_dateTo);
        }
        /// <summary>
        /// Rnder report content
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportContent(BingoWinlossDetailSuper_ExcelContents_Block excelBlock)
        {
            DataTable reportData = View as DataTable;
            int numRow = 0;
            if (reportData != null)
            {
                numRow = reportData.Rows.Count;
            }

            // Render date from and date to
            RenderReportContentHeader(excelBlock);
            excelBlock.Content.Clear();
            if (numRow > 0)
            {
                BingoWinlossDetailSuper_ExcelContents_Content_Block rowBlock = new BingoWinlossDetailSuper_ExcelContents_Content_Block();

                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];

                    rowBlock.ColorClass = (i % 2 == 0) ? EVEN_ROW_CLASS : ODD_ROW_CLASS;

                    rowBlock.GameTypeName = dr[BingoBO.BingoLang("gametypename")].ToString();
                    rowBlock.TurnOver = Formater.DecFormat(dr["turnover"], 2);
                    rowBlock.Company = Formater.DecFormat(dr["Company"], 2);
                    rowBlock.TotalMaster = Formater.DecFormat(dr["MasterTotal"], 2);
                    rowBlock.Winloss = Formater.DecFormat(dr["SuperWinloss"], 2);
                    rowBlock.Commission = Formater.DecFormat(dr["SuperRolling"], 2);
                    rowBlock.TotalSuper = Formater.DecFormat(dr["SuperTotal"], 2);

                    // Add row
                    excelBlock.Content.Append(rowBlock);
                }
                // Total row
                excelBlock.TotalContent.TurnOver = Formater.DecFormat(reportData.Compute("SUM(turnover)", ""), 2);
                excelBlock.TotalContent.TotalMaster = Formater.DecFormat(reportData.Compute("SUM(MasterTotal)", ""), 2);
                excelBlock.TotalContent.Winloss = Formater.DecFormat(reportData.Compute("SUM(SuperWinloss)", ""), 2);
                excelBlock.TotalContent.Commission = Formater.DecFormat(reportData.Compute("SUM(SuperRolling)", ""), 2);
                excelBlock.TotalContent.TotalSuper = Formater.DecFormat(reportData.Compute("SUM(SuperTotal)", ""), 2);
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
        #endregion
    }

}
