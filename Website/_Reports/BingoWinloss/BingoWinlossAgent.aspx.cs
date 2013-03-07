using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.Bingo;
using Sunplus.Bingo.Core;
using Sunplus.Bingo.Services;
using Microsoft.Practices.Unity;
using Fanex.Age.UI.Reports;


/*
 * Created 20100529@Jacob - Render BingoWinlossAgent page
 * Revision:
 * ?@? - ... 
 * 
 */
namespace Fanex.Age.UI.Reports.BingoWinloss
{

    public partial class BingoWinlossAgent_Page : Reports_Page
    {
      
        #region Override
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            BingoWinlossAgent_Template tpl = new BingoWinlossAgent_Template();

            if (_exportToExcel)
            {
                DataTable reportData = (DataTable)View;
                var resMan = GetLangResourceManager();
                var builder = new Fanex.Age.Excel.Reports.BingoWinloss.BingoWinlossAgentReportBuilder(reportData, IsBold, _dateFrom, _dateTo, resMan);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "BingoWinlossAgent.xls");
                    excelStream.Dispose();
                    return;
                }

                // Need to render report content only
                tpl.ExcelContents.BoldRow = "";
                RenderReportContent(tpl.ExcelContents);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "BingoWinlossAgent");
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
            if (_dateFrom > _dateTo) _dateFrom = _dateTo;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {

            IReportService bingoReportService = BingoContainer.Current.Resolve<IReportService>();
            Sunplus.Bingo.Entities.IResult result = new Sunplus.Bingo.Entities.Result();
            DataTable winloss = null;
            try
            {
                winloss = bingoReportService.GetWinLoss(_dateFrom, _dateTo, _custId, BingoSettings.BingoGame.CodeGameNo, 3, ref result);
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
        private void RenderReportHeader(BingoWinlossAgent_Template template)
        {
            template.ExcelContents.BoldRow = this.BoldRowClass;
        }
        /// <summary>
        /// Render report content header
        /// Assign DateFrom, DateTo, Title
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderReportContentHeader(BingoWinlossAgent_ExcelContents_Block excelBlock)
        {
            // Render date from and date to
            excelBlock.DateFrom = Formater.ShortDateFormat(_dateFrom);
            excelBlock.DateTo = Formater.ShortDateFormat(_dateTo);
        }
        /// <summary>
        /// Rnder report content
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportContent(BingoWinlossAgent_ExcelContents_Block excelBlock)
        {
            DataTable reportData = View as DataTable;
            int numRow = 0;
            if (reportData != null)
            {
                numRow = reportData.Rows.Count;
            }

            RenderReportContentHeader(excelBlock);
            excelBlock.Content.Clear();
            if (numRow > 0)
            {
                BingoWinlossAgent_ExcelContents_Content_Block contentRow = new BingoWinlossAgent_ExcelContents_Content_Block();
                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];

                    // Assign Value
                    #region Assign value
                    contentRow.ColorClass = (i % 2) == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;

                    contentRow.TurnOverValue = Formater.DecFormat(dr["turnover"], 2);
                    contentRow.UserName = dr["username"].ToString();
                    contentRow.CustId = dr["custid"].ToString();
                    contentRow.GrossCommValue = Formater.DecFormat(dr["RollingTurnover"], 2);
                    contentRow.MemberTotalValue = Formater.DecFormat(dr["MemberTotal"], 2);
                    contentRow.AgentWinValue = Formater.DecFormat(dr["AgentWinLoss"], 2);
                    contentRow.AgentCommValue = Formater.DecFormat(dr["AgentRolling"], 2);
                    contentRow.AgentTotalValue = Formater.DecFormat(dr["AgentTotal"], 2);
                    contentRow.CompanyValue = Formater.DecFormat(dr["company"], 2);
                    contentRow.MemberCommValue = Formater.DecFormat(dr["MemberRolling"], 2);
                    contentRow.MemberWinValue = Formater.DecFormat(dr["MemberWinLoss"], 2);
                    #endregion

                    excelBlock.Content.Append(contentRow);                   

                }
                #region Assign TotalValue
                excelBlock.TotalContent.Turnover = Formater.DecFormat(reportData.Compute("SUM(turnover)", ""), 2);
                excelBlock.TotalContent.GrossComm = Formater.DecFormat(reportData.Compute("SUM(RollingTurnover)", ""), 2);
                excelBlock.TotalContent.MemberWin = Formater.DecFormat(reportData.Compute("SUM(MemberWinLoss)", ""), 2);
                excelBlock.TotalContent.MemberComm = Formater.DecFormat(reportData.Compute("SUM(MemberRolling)", ""), 2);
                excelBlock.TotalContent.MemberTotal = Formater.DecFormat(reportData.Compute("SUM(MemberTotal)", ""), 2);
                excelBlock.TotalContent.AgentWin = Formater.DecFormat(reportData.Compute("SUM(AgentWinLoss)", ""), 2);
                excelBlock.TotalContent.AgentComm = Formater.DecFormat(reportData.Compute("SUM(AgentRolling)", ""), 2);
                excelBlock.TotalContent.AgentTotal = Formater.DecFormat(reportData.Compute("SUM(AgentTotal)", ""), 2);
                excelBlock.TotalContent.Company = Formater.DecFormat(reportData.Compute("SUM(company)", ""), 2);

                #endregion

                excelBlock.NoInfo.Hide();
            }
            else
            {
                excelBlock.TotalContent.Hide();
                excelBlock.NoInfo.NoInfoRow.NumCols = "10";
            }

        }
        /// <summary>
        /// Render Hidden Field
        /// </summary>
        /// <param name="template"></param>
        private void RenderHiddenField(BingoWinlossAgent_Template template)
        {
            template.CustId = _custId.ToString();
        }
        #endregion
    }

}
