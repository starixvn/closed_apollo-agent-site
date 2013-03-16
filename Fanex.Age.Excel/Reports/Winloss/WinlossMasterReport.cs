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

    internal class WinlossMasterReport : TemplateReport, IReportBuilder
    {
        private WinlossMasterReportBuilder builder;

        public WinlossMasterReport(string filePath, WinlossMasterReportBuilder builder)
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
            ISheet resultSheet = hssfWorkbook.GetSheet("WinlossMaster");

            // apply Bold on all fonts
            if (this.builder.IsBold)
            {
                this.BoldAllFonts();
            }

            int rowId = 0;
            int colId = 0;
            hssfWorkbook.SetSheetName(0, Resources.Lang.agentwinloss);

            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.agentwinloss + " " +
                Formater.ShortDateFormat(this.builder.FromDate) + "-->" + Formater.ShortDateFormat(this.builder.ToDate));
            rowId++;
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.agent);
            resultSheet.GetCell(rowId, colId + 1).SetCellValue(Resources.Lang.memberturnover);
            resultSheet.GetCell(rowId, colId + 2).SetCellValue(Resources.Lang.grosscomm);
            resultSheet.GetCell(rowId, colId + 3).SetCellValue(Resources.Lang.agenttotal);
            resultSheet.GetCell(rowId, colId + 4).SetCellValue(Resources.Lang.master);
            resultSheet.GetCell(rowId, colId + 7).SetCellValue(Resources.Lang.company);
            rowId++;
            resultSheet.GetCell(rowId, colId + 4).SetCellValue(Resources.Lang.win);
            resultSheet.GetCell(rowId, colId + 5).SetCellValue(Resources.Lang.com);
            resultSheet.GetCell(rowId, colId + 6).SetCellValue(Resources.Lang.total);

            if (data.Count > 0)
            {
                rowId = 3;
                int id = 1;
                decimal turnoverTotal = 0;
                decimal grosscomTotal = 0;
                decimal agentTotal = 0;
                decimal masterwinlossTotal = 0;
                decimal mastercommTotal = 0;
                decimal masterTotal = 0;
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
                    resultSheet.GetCell(rowId, colId + 3).SetCellValue(item.AgentTotal);
                    resultSheet.GetCell(rowId, colId + 4).SetCellValue(item.MasterWinloss);
                    resultSheet.GetCell(rowId, colId + 5).SetCellValue(item.MasterComm);
                    resultSheet.GetCell(rowId, colId + 6).SetCellValue(item.MasterTotal);
                    resultSheet.GetCell(rowId, colId + 7).SetCellValue(item.Company);

                    turnoverTotal += item.Turnover;
                    grosscomTotal += item.GrossComm;
                    agentTotal += item.AgentTotal;
                    masterwinlossTotal += item.MasterWinloss;
                    mastercommTotal += item.MasterComm;
                    masterTotal += item.MasterTotal;
                    companyTotal += item.Company;

                    id++;
                    rowId++;
                }

                //// total
                resultSheet.CopyRow(templateSheet, 5, rowId);
                resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.total);
                resultSheet.GetCell(rowId, colId + 1).SetCellValue(turnoverTotal);
                resultSheet.GetCell(rowId, colId + 2).SetCellValue(grosscomTotal);
                resultSheet.GetCell(rowId, colId + 3).SetCellValue(agentTotal);
                resultSheet.GetCell(rowId, colId + 4).SetCellValue(masterwinlossTotal);
                resultSheet.GetCell(rowId, colId + 5).SetCellValue(mastercommTotal);
                resultSheet.GetCell(rowId, colId + 6).SetCellValue(masterTotal);
                resultSheet.GetCell(rowId, colId + 7).SetCellValue(companyTotal);
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return this.hssfWorkbook;
        }
    }
}