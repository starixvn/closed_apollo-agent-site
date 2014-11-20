namespace Apollo.Age.Reports.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Reports.Core;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Extension;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Resources;
    using Apollo.Age.Web;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.Util;

    /// <summary>
    /// Service for SportCredit.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
    public class CreditReportService : ReportService, ICreditReportService
    {
        public List<TransactionCreditModel> CreditReportTransactions(CreditReportCriteria criteria)
        {
            var transactionCreditList = this.GetData<TransactionCreditModel>(criteria).ToList();

            return transactionCreditList;
        }

        /// <summary>
        /// Export Outstanding report excel file.
        /// </summary>
        /// <param name="isAgentOutStanding">Is Agent OutStanding.</param>
        /// <param name="listCreditTrans">List OutStanding data.</param>
        /// <param name="currentUsername">Current username.</param>
        /// <param name="sportName">Sport name.</param>
        /// <param name="betTypeName">Bet type name.</param>
        /// <param name="isSuper">Is super or no.</param>
        /// <returns>Outstanding Excel file.</returns>
        public HSSFWorkbook ExportCreditReport(
            CreditReportViewModel model,
            IEnumerable<TransactionCreditModel> listCreditTrans)
        {
            var hssfworkbook = new HSSFWorkbook();
            CustomizeColor(hssfworkbook);
            ISheet sheet = hssfworkbook.CreateSheet("Credit Report");

            if (listCreditTrans != null && listCreditTrans.Any())
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
        /// Render Outstanding Report Title.
        /// </summary>
        /// <param name="workBook">HSSF Workbook.</param>
        /// <param name="sheet">Excel worksheet.</param>
        /// <param name="fromRow">Row start.</param>
        /// <param name="isAgent">Is Agent or Member.</param>
        /// <param name="userName">User name.</param>
        /// <param name="sportName">Sport group name.</param>
        /// <param name="betTypeName">Bet type group name.</param>
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
            CreateStringCell(rowFilter, 0, styleFilter, string.Format("{0} -- {1}: {2} ~ {3}", model.Username, Lang.dateRange, ShortDateFormat(model.FromDate), ShortDateFormat(model.ToDate)));

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

            CreateStringCell(row, 0, headerStyle, Lang.TransNo);
            CreateStringCell(row, 1, headerStyle, Lang.TransDate);
            CreateStringCell(row, 2, headerStyle, Lang.TransType);
            CreateStringCell(row, 3, headerStyle, Lang.Amount);
            CreateStringCell(row, 4, headerStyle, Lang.BalanceAmount);
            CreateStringCell(row, 5, headerStyle, Lang.Remark);

            sheet.SetColumnWidth(0, 20 * 256);
            sheet.SetColumnWidth(1, 30 * 256);
            sheet.SetColumnWidth(2, 30 * 256);
            sheet.SetColumnWidth(3, 20 * 256);
            sheet.SetColumnWidth(4, 20 * 256);
            sheet.SetColumnWidth(5, 35 * 256);
        }

        /// <summary>
        /// Render Excel Outstanding Report body rows.
        /// </summary>
        /// <param name="sheet">Excel worksheet.</param>
        /// <param name="rowIndex">Row index.</param>
        /// <param name="userName">User name.</param>
        /// <param name="outstandingAmount">Outstanding Amount.</param>
        /// <param name="shareAmount">Share Amount.</param>
        /// <param name="numberFormat">Number Format.</param>
        /// <param name="bodyStyleLeft">The body style left.</param>
        /// <param name="bodyStyleRight">The body style right.</param>
        private void RenderExcelBodyRows(
            ISheet sheet,
            int rowIndex,
            TransactionCreditModel item,
            short numberFormat,
            ICellStyle bodyStyleLeft,
            ICellStyle bodyStyleRight,
            ICellStyle bodyStyleCenter)
        {
            IRow row = sheet.CreateRow(rowIndex);

            CreateStringCell(row, 0, bodyStyleCenter, item.TransId.ToString());
            CreateStringCell(row, 1, bodyStyleLeft, LongDateFormat(item.TransDate));
            CreateStringCell(row, 2, bodyStyleLeft, item.TransType);
            CreateNumberCell(row, 3, bodyStyleRight, numberFormat, (double)item.Amount);
            CreateNumberCell(row, 4, bodyStyleRight, numberFormat, (double)item.BalanceAmount);
            CreateStringCell(row, 5, bodyStyleLeft, item.TransDesc);
        }
    }
}