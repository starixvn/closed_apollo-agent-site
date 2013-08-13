// --------------------------------------------------------------------------
// <copyright file="SportCreditService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.7.31">Created</change>
// </history>

namespace Apollo.Age.Reports.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Reports.Core;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Resources;
    using Apollo.Age.Web;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Service for SportCredit.
    /// </summary>
    public class SportCreditService : ReportService, ISportCreditService
    {
        private const int _RowTitle = 1;
        private const int _RowFilter = 2;
        private const int _RowHeader = 4;
        private const int _RowStartBody = 5;
        private const int _RowFooter = 6;
        private const string _FilePathExcelTemplate = "\\ExcelTemplates\\LineSportCreditTemplate.xls";

        /// <summary>
        /// Get Sport Credits list.
        /// </summary>
        /// <param name="criteria">Input SportCreditCriteria.</param>
        /// <returns>List SportCreditModel.</returns>
        public List<SportCreditModel> GetSportCredits(SportCreditCriteria criteria)
        {
            var sportCreditList = this.GetData<SportCreditModel>(criteria).ToList();

            return sportCreditList;
        }

        public HSSFWorkbook ExportSportCredit(SportCreditViewModel model)
        {
            HSSFWorkbook hssfworkbook;

            var templatePath = HttpContext.Current.Server.MapPath(_FilePathExcelTemplate);
            using (FileStream template = new FileStream(templatePath, FileMode.Open, System.IO.FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(template);
            }

            //// Assign data
            var templateSheet = hssfworkbook.GetSheet("sheet1");
            var sheet = hssfworkbook.CloneSheet(0);
            hssfworkbook.SetSheetName(1, Lang.lblLineSportCredit);

            //// render title
            var title = string.Format("{0} {1}", Lang.lblLineSportCredit, Lang.lblReport);
            var rowTitle = sheet.GetRow(_RowTitle);
            var cellTitle = rowTitle.GetCell(0);
            cellTitle.SetCellValue(title);
            //// end title

            //// render fiter
            var filter = string.Format("{0}: {1}", Lang.enterusername, model.Username);
            var rowFilter = sheet.GetRow(_RowFilter);
            var cellFilter = rowFilter.GetCell(0);
            cellFilter.SetCellValue(filter);
            //// end fiter

            //// render header
            var rowHeader = sheet.GetRow(_RowHeader);
            var cellHeaderOutstanding = rowHeader.GetCell(1);
            var cellHeaderRemaining = rowHeader.GetCell(2);
            cellHeaderOutstanding.SetCellValue(Lang.lblTotalOutstandingCredit);
            cellHeaderRemaining.SetCellValue(Lang.lblTotalRemainingCredit);
            //// end header

            //// render body
            var rowIndex = _RowStartBody;
            double totalOutstanding = 0;
            double totalRemaining = 0;

            var keyLang = string.Empty;
            foreach (var item in model.SportCreditModelList)
            {
                totalOutstanding += (double)item.OutstandingCredit;
                totalRemaining += (double)item.RemainingCredit;

                CopyRow(hssfworkbook, templateSheet, sheet, _RowStartBody, rowIndex);
                var row = sheet.GetRow(rowIndex);

                var cellLevel = row.GetCell(0);
                var cellOutstanding = row.GetCell(1);
                var cellRemaining = row.GetCell(2);

                var langAgent = Constants.ReportAgentLevelDictionary.Where(t => (t.Key != 4 && t.Key == item.AgentLevel)).SingleOrDefault();
                if (langAgent.Value == null)
                {
                    // is not exits in Constants.ReportAgentLevelDictionary is it's Agent (key = 4)
                    keyLang = Apollo.Age.Reports.Core.Constants.ReportAgentLevelDictionary[4];
                }
                else
                {
                    keyLang = langAgent.Value;
                }

                if (item.AgentLevel > AgentLevel.MainAgent)
                {
                    cellLevel.SetCellValue(LanguageManager.GetResourceData(keyLang) + item.AgentLevel);
                }
                else
                {
                    cellLevel.SetCellValue(LanguageManager.GetResourceData(keyLang));
                }
                
                cellOutstanding.SetCellValue((double)item.OutstandingCredit);
                cellRemaining.SetCellValue((double)item.RemainingCredit);

                rowIndex++;
            }
            //// end render body

            //// render footer
            CopyRow(hssfworkbook, templateSheet, sheet, _RowFooter, rowIndex);

            var rowFooter = sheet.GetRow(rowIndex);

            var cellTotalLevel = rowFooter.GetCell(0);
            var cellTotalOutstanding = rowFooter.GetCell(1);
            var cellTotalRemaining = rowFooter.GetCell(2);

            cellTotalLevel.SetCellValue(Lang.total);
            cellTotalOutstanding.SetCellValue(totalOutstanding);
            cellTotalRemaining.SetCellValue(totalRemaining);
            //// end footer

            //// remove sheet & row template
            var rowBodyTemplate = sheet.GetRow(rowIndex + 1);
            var rowFooterTemplate = sheet.GetRow(rowIndex + 2);
            sheet.RemoveRow(rowBodyTemplate);
            sheet.RemoveRow(rowFooterTemplate);
            hssfworkbook.RemoveSheetAt(0);

            return hssfworkbook;
        }

        private void CopyRow(HSSFWorkbook workbook, ISheet sourceSheet, ISheet destinationSheet, int sourceRowNum, int destinationRowNum)
        {
            // Get the source / new row
            var newRow = destinationSheet.GetRow(destinationRowNum);
            var sourceRow = sourceSheet.GetRow(sourceRowNum);

            // If the row exist in destination, push down all rows by 1 else create a new row
            if (newRow != null)
            {
                destinationSheet.ShiftRows(destinationRowNum, destinationSheet.LastRowNum, 1);
            }
            else
            {
                newRow = destinationSheet.CreateRow(destinationRowNum);
            }

            // Loop through source columns to add to new row
            for (int i = 0; i < sourceRow.LastCellNum; i++)
            {
                // Grab a copy of the old/new cell
                var oldCell = sourceRow.GetCell(i);
                var newCell = newRow.CreateCell(i);

                // If the old cell is null jump to next cell
                if (oldCell == null)
                {
                    newCell = null;
                    continue;
                }

                // Copy style from old cell and apply to new cell
                var newCellStyle = workbook.CreateCellStyle();
                newCellStyle.CloneStyleFrom(oldCell.CellStyle);
                newCell.CellStyle = newCellStyle;

                // If there is a cell comment, copy
                if (newCell.CellComment != null)
                {
                    newCell.CellComment = oldCell.CellComment;
                }

                // If there is a cell hyperlink, copy
                if (oldCell.Hyperlink != null)
                {
                    newCell.Hyperlink = oldCell.Hyperlink;
                }

                // Set the cell data type
                newCell.SetCellType(oldCell.CellType);

                // Set the cell data value
                switch (oldCell.CellType)
                {
                    case CellType.BLANK:
                        newCell.SetCellValue(oldCell.StringCellValue);
                        break;
                    case CellType.BOOLEAN:
                        newCell.SetCellValue(oldCell.BooleanCellValue);
                        break;
                    case CellType.ERROR:
                        newCell.SetCellErrorValue(oldCell.ErrorCellValue);
                        break;
                    case CellType.FORMULA:
                        newCell.SetCellFormula(oldCell.CellFormula);
                        break;
                    case CellType.NUMERIC:
                        newCell.SetCellValue(oldCell.NumericCellValue);
                        break;
                    case CellType.STRING:
                        newCell.SetCellValue(oldCell.RichStringCellValue);
                        break;
                    case CellType.Unknown:
                        newCell.SetCellValue(oldCell.StringCellValue);
                        break;
                }
            }

            // If there are are any merged regions in the source row, copy to new row
            for (int i = 0; i < sourceSheet.NumMergedRegions; i++)
            {
                CellRangeAddress cellRangeAddress = sourceSheet.GetMergedRegion(i);
                if (cellRangeAddress.FirstRow == sourceRow.RowNum)
                {
                    CellRangeAddress newCellRangeAddress = new CellRangeAddress(
                        newRow.RowNum,
                        newRow.RowNum + (cellRangeAddress.FirstRow - cellRangeAddress.LastRow),
                        cellRangeAddress.FirstColumn,
                        cellRangeAddress.LastColumn);
                        destinationSheet.AddMergedRegion(newCellRangeAddress);
                }
            }
        }
    }
}
