/*
 * Created 20130318@Lewis
 * Revision:
 *
 */

namespace Fanex.Age.Excel.Admin.ExchangeRate
{
    using Fanex.Age.Utility;
    using Fanex.Age.Utility.NPOI;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    internal class ExchangeRateList : TemplateReport, IReportBuilder
    {
        private ExchangeRateDataBuilder builder;

        public ExchangeRateList(string filePath, ExchangeRateDataBuilder builder)
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
            ISheet resultSheet = hssfWorkbook.GetSheet("ExchangeRate");

            int rowIdx = 0;
            int colIdx = 0;

            // HEADER
            resultSheet.GetCell(rowIdx, colIdx).SetCellValue(Resources.Lang.ExchangeRate);
            rowIdx++;
            resultSheet.GetCell(rowIdx, colIdx).SetCellValue(Resources.Lang.currency);
            resultSheet.GetCell(rowIdx, colIdx + 1).SetCellValue(Resources.Lang.MultipliedBy);
            resultSheet.GetCell(rowIdx, colIdx + 2).SetCellValue(Resources.Lang.BaseRate);
            resultSheet.GetCell(rowIdx, colIdx + 3).SetCellValue(Resources.Lang.MinBet);
            resultSheet.GetCell(rowIdx, colIdx + 4).SetCellValue(Resources.Lang.MinBetLive);
            resultSheet.GetCell(rowIdx, colIdx + 5).SetCellValue(Resources.Lang.Description);

            if (data.Count > 0)
            {
                rowIdx = 2;
                int rowNum = 1;

                foreach (var item in data)
                {
                    if (rowNum % 2 == 0)
                    {
                        resultSheet.CopyRow(templateSheet, 1, rowIdx);
                    }
                    else
                    {
                        resultSheet.CopyRow(templateSheet, 3, rowIdx);
                    }

                    resultSheet.GetCell(rowIdx, colIdx).SetCellValue(item.Currency);
                    resultSheet.GetCell(rowIdx, colIdx + 1).SetCellValue(item.MultipliedBy);
                    resultSheet.GetCell(rowIdx, colIdx + 2).SetCellValue(item.BaseRate);
                    resultSheet.GetCell(rowIdx, colIdx + 3).SetCellValue(item.MinBet);
                    resultSheet.GetCell(rowIdx, colIdx + 4).SetCellValue(item.MinBetLive);
                    resultSheet.GetCell(rowIdx, colIdx + 5).SetCellValue(item.Description);

                    rowNum++;
                    rowIdx++;
                }
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return this.hssfWorkbook;
        }
    }
}