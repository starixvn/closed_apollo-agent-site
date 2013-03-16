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
    using Fanex.Age.Utility;

    internal class WinlossAnalysisSuperReport : TemplateReport, IReportBuilder
    {
        private WinlossAnalysisSuperReportBuilder builder;

        public WinlossAnalysisSuperReport(string filePath, WinlossAnalysisSuperReportBuilder builder)
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
            ISheet resultSheet = hssfWorkbook.GetSheet("WinlossAnalysisSuper");

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
            resultSheet.GetCell(rowId, colId + 8).SetCellValue(Resources.Lang.master);
            resultSheet.GetCell(rowId, colId + 11).SetCellValue(Resources.Lang.super);
            resultSheet.GetCell(rowId, colId + 14).SetCellValue(Resources.Lang.company);
            resultSheet.GetCell(rowId, colId + 15).SetCellValue(Resources.Lang.betcount);
            rowId++;
            resultSheet.GetRow(rowId).WriteCells(
                colId + 2,
                Resources.Lang.lblWinLoss,
                Resources.Lang.lblComm,
                Resources.Lang.total,
                Resources.Lang.lblWinLoss,
                Resources.Lang.lblComm,
                Resources.Lang.total,
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
                decimal masterwinlossTotal = 0;
                decimal mastercommTotal = 0;
                decimal masterTotal = 0;
                decimal superwinlossTotal = 0;
                decimal supercommTotal = 0;
                decimal superTotal = 0;
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
                    resultSheet.GetRow(rowId).WriteCells(colId, breakdownText, item.Turnover, item.MemberWinloss, item.MemberComm, item.MemberTotal, item.AgentWinloss, item.AgentComm, item.AgentTotal, item.MasterWinloss, item.MasterComm, item.MasterTotal, item.SuperWinloss, item.SuperComm, item.SuperTotal, item.Company, item.BetCount);

                    turnoverTotal += item.Turnover;
                    memberwinlossTotal += item.MemberWinloss;
                    membercommTotal += item.MemberComm;
                    memberTotal += item.MemberTotal;
                    agentwinlossTotal += item.AgentWinloss;
                    agentcommTotal += item.AgentComm;
                    agentTotal += item.AgentTotal;
                    masterwinlossTotal += item.MasterWinloss;
                    mastercommTotal += item.MasterComm;
                    masterTotal += item.MasterTotal;
                    superwinlossTotal += item.SuperWinloss;
                    supercommTotal += item.SuperComm;
                    superTotal += item.SuperTotal;
                    companyTotal += item.Company;
                    betcountTotal += item.BetCount;

                    id++;
                    rowId++;
                }

                //// total
                resultSheet.CopyRow(templateSheet, 5, rowId);
                resultSheet.GetRow(rowId).WriteCells(colId, Resources.Lang.total, turnoverTotal, memberwinlossTotal, membercommTotal, memberTotal, agentwinlossTotal, agentcommTotal, agentTotal, masterwinlossTotal, mastercommTotal, masterTotal, superwinlossTotal, supercommTotal, superTotal, companyTotal, betcountTotal);
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return this.hssfWorkbook;
        }
    }
}