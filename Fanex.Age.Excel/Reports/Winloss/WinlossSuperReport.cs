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
    using Sunplus.Agent.Utilities;

    internal class WinlossSuperReport : TemplateReport, IReportBuilder
    {
        private WinlossSuperReportBuilder builder;

        public WinlossSuperReport(string filePath, WinlossSuperReportBuilder builder)
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
            ISheet resultSheet = hssfWorkbook.GetSheet("WinlossSuper");
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
            hssfWorkbook.SetSheetName(0, Resources.Lang.agentwinloss);

            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.masterwinloss + " " +
                Formater.ShortDateFormat(this.builder.FromDate) + "-->" + Formater.ShortDateFormat(this.builder.ToDate));
            rowId++;
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.master);
            resultSheet.GetCell(rowId, colId + 1).SetCellValue(Resources.Lang.turnover);
            resultSheet.GetCell(rowId, colId + 2).SetCellValue(Resources.Lang.grosscomm);
            resultSheet.GetCell(rowId, colId + 3).SetCellValue(Resources.Lang.mastertotal);
            resultSheet.GetCell(rowId, colId + 4).SetCellValue(Resources.Lang.super);
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
                decimal masterTotal = 0;
                decimal superwinlossTotal = 0;
                decimal supercommTotal = 0;
                decimal superTotal = 0;
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
                    resultSheet.GetCell(rowId, colId + 3).SetCellValue(item.MasterTotal);
                    resultSheet.GetCell(rowId, colId + 4).SetCellValue(item.SuperWinloss);
                    resultSheet.GetCell(rowId, colId + 5).SetCellValue(item.SuperComm);
                    resultSheet.GetCell(rowId, colId + 6).SetCellValue(item.SuperTotal);
                    resultSheet.GetCell(rowId, colId + 7).SetCellValue(item.Company);

                    turnoverTotal += item.Turnover;
                    grosscomTotal += item.GrossComm;
                    masterTotal += item.MasterTotal;
                    superwinlossTotal += item.SuperWinloss;
                    supercommTotal += item.SuperComm;
                    superTotal += item.SuperTotal;
                    companyTotal += item.Company;

                    id++;
                    rowId++;
                }

                //// total
                resultSheet.CopyRow(templateSheet, 5, rowId);
                resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.total);
                resultSheet.GetCell(rowId, colId + 1).SetCellValue(turnoverTotal);
                resultSheet.GetCell(rowId, colId + 2).SetCellValue(grosscomTotal);
                resultSheet.GetCell(rowId, colId + 3).SetCellValue(masterTotal);
                resultSheet.GetCell(rowId, colId + 4).SetCellValue(superwinlossTotal);
                resultSheet.GetCell(rowId, colId + 5).SetCellValue(supercommTotal);
                resultSheet.GetCell(rowId, colId + 6).SetCellValue(superTotal);
                resultSheet.GetCell(rowId, colId + 7).SetCellValue(companyTotal);
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return this.hssfWorkbook;
        }
    }
}