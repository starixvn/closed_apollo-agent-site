/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Excel.Reports.Winloss
{
    using Fanex.Age.Utility.NPOI;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Fanex.Age.Utility;

    internal class WinlossAgentReport : TemplateReport, IReportBuilder
    {
        private WinlossAgentReportBuilder builder;

        public WinlossAgentReport(string filePath, WinlossAgentReportBuilder builder)
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
            ISheet resultSheet = hssfWorkbook.GetSheet("WinlossAgent");
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
            hssfWorkbook.SetSheetName(0, Resources.Lang.memberwinloss);

            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.memberwinloss + " " +
                Formater.ShortDateFormat(this.builder.FromDate) + "-->" + Formater.ShortDateFormat(this.builder.ToDate));
            rowId++;
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.member);
            resultSheet.GetCell(rowId, colId + 1).SetCellValue(Resources.Lang.turnover);
            resultSheet.GetCell(rowId, colId + 2).SetCellValue(Resources.Lang.grosscomm);
            resultSheet.GetCell(rowId, colId + 3).SetCellValue(Resources.Lang.member);
            resultSheet.GetCell(rowId, colId + 6).SetCellValue(Resources.Lang.agent);
            resultSheet.GetCell(rowId, colId + 9).SetCellValue(Resources.Lang.company);
            rowId++;
            resultSheet.GetCell(rowId, colId + 3).SetCellValue(Resources.Lang.win);
            resultSheet.GetCell(rowId, colId + 4).SetCellValue(Resources.Lang.com);
            resultSheet.GetCell(rowId, colId + 5).SetCellValue(Resources.Lang.total);
            resultSheet.GetCell(rowId, colId + 6).SetCellValue(Resources.Lang.win);
            resultSheet.GetCell(rowId, colId + 7).SetCellValue(Resources.Lang.com);
            resultSheet.GetCell(rowId, colId + 8).SetCellValue(Resources.Lang.total);

            if (data.Count > 0)
            {
                rowId = 3;
                int id = 1;
                decimal turnoverTotal = 0;
                decimal grosscomTotal = 0;
                decimal memberwinlossTotal = 0;
                decimal membercommTotal = 0;
                decimal memberTotal = 0;
                decimal agentwinlossTotal = 0;
                decimal agentcommTotal = 0;
                decimal agentTotal = 0;
                decimal companyTotal = 0;

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

                    resultSheet.GetCell(rowId, colId).SetCellValue(item.UserName);
                    resultSheet.GetCell(rowId, colId + 1).SetCellValue(item.Turnover);
                    resultSheet.GetCell(rowId, colId + 2).SetCellValue(item.GrossComm);
                    resultSheet.GetCell(rowId, colId + 3).SetCellValue(item.MemberWinloss);
                    resultSheet.GetCell(rowId, colId + 4).SetCellValue(item.MemberComm);
                    resultSheet.GetCell(rowId, colId + 5).SetCellValue(item.MemberTotal);
                    resultSheet.GetCell(rowId, colId + 6).SetCellValue(item.AgentWinloss);
                    resultSheet.GetCell(rowId, colId + 7).SetCellValue(item.AgentComm);
                    resultSheet.GetCell(rowId, colId + 8).SetCellValue(item.AgentTotal);
                    resultSheet.GetCell(rowId, colId + 9).SetCellValue(item.Company);

                    turnoverTotal += item.Turnover;
                    grosscomTotal += item.GrossComm;
                    memberwinlossTotal += item.MemberWinloss;
                    membercommTotal += item.MemberComm;
                    memberTotal += item.MemberTotal;
                    agentwinlossTotal += item.AgentWinloss;
                    agentcommTotal += item.AgentComm;
                    agentTotal += item.AgentTotal;
                    companyTotal += item.Company;

                    id++;
                    rowId++;
                }

                resultSheet.CopyRow(templateSheet, 5, rowId);
                resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.total);
                resultSheet.GetCell(rowId, colId + 1).SetCellValue(turnoverTotal);
                resultSheet.GetCell(rowId, colId + 2).SetCellValue(grosscomTotal);
                resultSheet.GetCell(rowId, colId + 3).SetCellValue(memberwinlossTotal);
                resultSheet.GetCell(rowId, colId + 4).SetCellValue(membercommTotal);
                resultSheet.GetCell(rowId, colId + 5).SetCellValue(memberTotal);
                resultSheet.GetCell(rowId, colId + 6).SetCellValue(agentwinlossTotal);
                resultSheet.GetCell(rowId, colId + 7).SetCellValue(agentcommTotal);
                resultSheet.GetCell(rowId, colId + 8).SetCellValue(agentTotal);
                resultSheet.GetCell(rowId, colId + 9).SetCellValue(companyTotal);
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return this.hssfWorkbook;
        }
    }
}