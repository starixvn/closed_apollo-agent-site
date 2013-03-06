/*
 * Created 20120220@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Excel.Reports.WinlossAnalysis
{
    using Fanex.Age.Utility.NPOI;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Sunplus.Agent.Utilities;

    internal class WinlossAnalysisAgentReport : TemplateReport, IReportBuilder
    {
        private WinlossAnalysisAgentReportBuilder builder;

        public WinlossAnalysisAgentReport(string filePath, WinlossAnalysisAgentReportBuilder builder)
            : base(filePath)
        {
            this.builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var data = this.builder.Data;
            if (data == null)
            {
                return null;
            }

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("WinlossAnalysisAgent");
            //// apply Bold on all fonts
            if (this.builder.IsBold)
            {
                for (short i = 0; i < hssfWorkbook.NumberOfFonts; i++)
                {
                    var font = hssfWorkbook.GetFontAt(i);
                    font.Boldweight = (short)FontBoldWeight.BOLD;
                }
            }

            int rowId = 0;
            int colId = 0;
            hssfWorkbook.SetSheetName(0, CommonLibs.GetHeaderTitle(this.builder.ReportType));

            //// header
            string title = CommonLibs.GetHeaderTitle(this.builder.ReportType);
            resultSheet.GetCell(rowId, colId).SetCellValue(title + " " + Formater.ShortDateFormat(this.builder.FromDate) + "-->" + Formater.ShortDateFormat(this.builder.ToDate));
            rowId++;
            string breakdownTitle = Reports.CommonLibs.GetBreakDownTitle(this.builder.ReportType);
            resultSheet.GetCell(rowId, colId).SetCellValue(breakdownTitle);
            resultSheet.GetRow(rowId).WriteCells(
                colId + 1,
                Resources.Lang.turnover,
                Resources.Lang.member);
            resultSheet.GetCell(rowId, colId + 5).SetCellValue(Resources.Lang.agent);
            resultSheet.GetCell(rowId, colId + 8).SetCellValue(Resources.Lang.company);
            resultSheet.GetCell(rowId, colId + 9).SetCellValue(Resources.Lang.betcount);
            rowId++;
            resultSheet.GetRow(rowId).WriteCells(
                colId + 2,
                Resources.Lang.lblWinLoss,
                Resources.Lang.lblComm,
                Resources.Lang.total,
                Resources.Lang.lblWinLoss,
                Resources.Lang.lblComm,
                Resources.Lang.total);

            if (data.Count > 0)
            {
                rowId = 3;
                int id = 1;
                decimal turnoverTotal = 0;
                decimal memberwinlossTotal = 0;
                decimal membercommTotal = 0;
                decimal memberTotal = 0;
                decimal agentwinlossTotal = 0;
                decimal agentcommTotal = 0;
                decimal agentTotal = 0;
                decimal companyTotal = 0;
                int betcountTotal = 0;

                foreach (var item in data)
                {
                    if (id % 2 == 0)
                    {
                        resultSheet.CopyRow(templateSheet, 1, rowId);
                    }
                    else
                    {
                        resultSheet.CopyRow(templateSheet, 3, rowId);
                    }

                    string breakdownText = Reports.CommonLibs.GetBreakDownText(item.BreakDown, this.builder.ReportType);
                    resultSheet.GetRow(rowId).WriteCells(colId, breakdownText, item.Turnover, item.MemberWinloss, item.MemberComm, item.MemberTotal, item.AgentWinloss, item.AgentComm, item.AgentTotal, item.Company, item.BetCount);

                    turnoverTotal += item.Turnover;
                    memberwinlossTotal += item.MemberWinloss;
                    membercommTotal += item.MemberComm;
                    memberTotal += item.MemberTotal;
                    agentwinlossTotal += item.AgentWinloss;
                    agentcommTotal += item.AgentComm;
                    agentTotal += item.AgentTotal;
                    companyTotal += item.Company;
                    betcountTotal += item.BetCount;

                    id++;
                    rowId++;
                }

                //// total
                resultSheet.CopyRow(templateSheet, 5, rowId);
                resultSheet.GetRow(rowId).WriteCells(colId, Resources.Lang.total, turnoverTotal, memberwinlossTotal, membercommTotal, memberTotal, agentwinlossTotal, agentcommTotal, agentTotal, companyTotal, betcountTotal);
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return this.hssfWorkbook;
        }
    }
}