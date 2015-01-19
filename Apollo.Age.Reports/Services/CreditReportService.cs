namespace Apollo.Age.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Apollo.Age.Reports.Core;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Resources;
    using Apollo.Age.Services;
    using Apollo.Age.Web;
    using Fanex.NAP.Framework.Data;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Service for SportCredit.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
    public class CreditReportService : ReportService, ICreditReportService
    {
        /// <summary>
        /// Credits the report transactions.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Transaction Credit Model.</returns>
        public List<TransactionCreditModel> CreditReportTransactions(ICriteria criteria)
        {
            var transactionCreditList = this.GetData<TransactionCreditModel>(criteria).ToList();
            if (CacheRef.CurrentLanguage.Equals("ch") || CacheRef.CurrentLanguage.Equals("cs"))
            {
                foreach (var item in transactionCreditList)
                {
                    var langTransType = Constants.ReportCreditTypeDictionary.Where(t => t.Value.Equals(item.TransType, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                    if (langTransType.Key != null)
                    {
                        item.TransType = LanguageManager.GetResourceData(langTransType.Key.ToString());
                    }

                    var langTransDesc = Constants.ReportCreditTransDescDictionary.Where(t => item.TransDesc.Contains(t.Value)).FirstOrDefault();

                    if (langTransDesc.Key != null)
                    {
                        item.TransDesc = item.TransDesc.Replace(langTransDesc.Value.ToString(), LanguageManager.GetResourceData(langTransDesc.Key.ToString()));
                    }
                }
            }

            return transactionCreditList;
        }

        /// <summary>
        /// Exports the credit report.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="listCreditTrans">The list credit trans.</param>
        /// <returns>Excel file.</returns>
        public HSSFWorkbook ExportCreditReport(
            CreditReportViewModel model,
            IEnumerable<TransactionCreditModel> listCreditTrans)
        {
            var hssfworkbook = new HSSFWorkbook();
            CustomizeColor(hssfworkbook);
            ISheet sheet = hssfworkbook.CreateSheet("Credit Report");

            if (listCreditTrans != null)
            {
                RenderExcelTitle(hssfworkbook, sheet, 1, model);
                RenderExcelHeader(hssfworkbook, sheet, 4);

                var rowIndex = 5;
                var length = listCreditTrans.Count();
                //// create the format instance
                IDataFormat format = hssfworkbook.CreateDataFormat();
                var numberFormat = format.GetFormat("0.00_ ;[Red]-0.00 ");

                var bodyStyleRight = CreateStyle(hssfworkbook, align: HorizontalAlignment.Right);
                var bodyStyleCenter = CreateStyle(hssfworkbook, align: HorizontalAlignment.Center);
                var bodyStyleLeft = CreateStyle(hssfworkbook);

                sheet.CreateFreezePane(0, rowIndex);

                for (int i = 0; i < length; i++)
                {
                    var currentCreditTrans = listCreditTrans.ElementAt(i);

                    RenderExcelBodyRows(
                        sheet,
                        rowIndex + i,
                        i + 1,
                        currentCreditTrans,
                        numberFormat,
                        bodyStyleLeft,
                        bodyStyleRight,
                        bodyStyleCenter);
                }
            }

            return hssfworkbook;
        }

        /// <summary>
        /// Renders the excel title.
        /// </summary>
        /// <param name="workBook">The work book.</param>
        /// <param name="sheet">The excel sheet.</param>
        /// <param name="fromRow">From row index.</param>
        /// <param name="model">The Credit Report model.</param>
        private void RenderExcelTitle(
            HSSFWorkbook workBook,
            ISheet sheet,
            int fromRow,
            CreditReportViewModel model)
        {
            var reportType = model.IsAgent ? Lang.agent : Lang.member;

            //// Report Title
            var row = sheet.CreateRow(fromRow);

            var fontTitle = CreateFont(workBook, HSSFColor.Black.Index, 14, true);
            var styleTitle = CreateStyle(workBook, align: HorizontalAlignment.Center, font: fontTitle, isBorder: false);

            CreateStringCell(row, 0, styleTitle, string.Format("{0} {1}", reportType, Lang.CreditReport));
            sheet.AddMergedRegion(new CellRangeAddress(fromRow, fromRow, 0, 5));

            var rowFilter = sheet.CreateRow(rownum: 2);
            var fontFilter = CreateFont(workBook, HSSFColor.Blue.Index, 11, true, true);
            var styleFilter = CreateStyle(workBook, font: fontFilter, isBorder: false);
            var title = string.Format("{0} -- {1}: {2} ~ {3}", model.Username, Lang.dateRange, ShortDateFormat(model.FromDate), ShortDateFormat(model.ToDate));
            CreateStringCell(rowFilter, 0, styleFilter, title);

            sheet.AddMergedRegion(new CellRangeAddress(fromRow + 1, fromRow + 1, 0, 5));
        }

        /// <summary>
        /// Render Outstanding header report.
        /// </summary>
        /// <param name="workBook">HSSF Workbook.</param>
        /// <param name="sheet">Excel worksheet.</param>
        /// <param name="fromRow">Row start.</param>
        private void RenderExcelHeader(HSSFWorkbook workBook, ISheet sheet, int fromRow)
        {
            var row = sheet.CreateRow(fromRow);
            var font = CreateFont(workBook, HSSFColor.White.Index, 11);
            var headerStyle = CreateStyle(workBook, HSSFColor.Brown.Index, HorizontalAlignment.Center, font);

            var usernameCell = row.CreateCell(0);
            usernameCell.CellStyle = headerStyle;
            usernameCell.SetCellValue(string.Empty);

            CreateStringCell(row, 0, headerStyle, "#");
            CreateStringCell(row, 1, headerStyle, Lang.TransNo);
            CreateStringCell(row, 2, headerStyle, Lang.TransDate);
            CreateStringCell(row, 3, headerStyle, Lang.TransType);
            CreateStringCell(row, 4, headerStyle, Lang.Amount);
            CreateStringCell(row, 5, headerStyle, Lang.BalanceAmount);
            CreateStringCell(row, 6, headerStyle, Lang.lblRemarks);

            sheet.SetColumnWidth(0, 20 * 256);
            sheet.SetColumnWidth(1, 20 * 256);
            sheet.SetColumnWidth(2, 30 * 256);
            sheet.SetColumnWidth(3, 30 * 256);
            sheet.SetColumnWidth(4, 20 * 256);
            sheet.SetColumnWidth(5, 20 * 256);
            sheet.SetColumnWidth(6, 35 * 256);
        }

        /// <summary>
        /// Renders the excel body rows.
        /// </summary>
        /// <param name="sheet">The excel sheet.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="rowNumber">The row number.</param>
        /// <param name="item">The item transaction.</param>
        /// <param name="numberFormat">The number format.</param>
        /// <param name="bodyStyleLeft">The body style left.</param>
        /// <param name="bodyStyleRight">The body style right.</param>
        /// <param name="bodyStyleCenter">The body style center.</param>
        private void RenderExcelBodyRows(
            ISheet sheet,
            int rowIndex,
            int rowNumber,
            TransactionCreditModel item,
            short numberFormat,
            ICellStyle bodyStyleLeft,
            ICellStyle bodyStyleRight,
            ICellStyle bodyStyleCenter)
        {
            IRow row = sheet.CreateRow(rowIndex);

            CreateStringCell(row, 0, bodyStyleCenter, rowNumber.ToString());
            CreateStringCell(row, 1, bodyStyleCenter, item.TransId.ToString());
            CreateStringCell(row, 2, bodyStyleLeft, LongDateFormat(item.TransDate));
            CreateStringCell(row, 3, bodyStyleLeft, item.TransType);
            CreateNumberCell(row, 4, bodyStyleRight, numberFormat, (double)item.Amount);
            CreateNumberCell(row, 5, bodyStyleRight, numberFormat, (double)item.BalanceAmount);
            CreateStringCell(row, 6, bodyStyleLeft, item.TransDesc);
        }
    }
}