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

    /// <summary>
    /// Service for SportCredit.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
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

        /// <summary>
        /// Exports the sport credit.
        /// </summary>
        /// <param name="model">Input the Sport credit view model.</param>
        /// <returns>Return a workbook.</returns>
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
            var sheet = hssfworkbook.CloneSheet("sheet1", Lang.lblLineSportCredit);

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

                sheet.CopyRow(templateSheet, _RowStartBody, rowIndex);
                var row = sheet.GetRow(rowIndex);

                var cellLevel = row.GetCell(0);
                var cellOutstanding = row.GetCell(1);
                var cellRemaining = row.GetCell(2);

                var langAgent = Constants.ReportAgentLevelDictionary.Where(t => (t.Key != 4 && t.Key == item.AgentLevel)).SingleOrDefault();
                if (langAgent.Value == null)
                {
                    // is not exits in Constants.ReportAgentLevelDictionary is it's Agent (key = 4)
                    keyLang = Constants.ReportAgentLevelDictionary[4];
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
            sheet.CopyRow(templateSheet, _RowFooter, rowIndex);

            var rowFooter = sheet.GetRow(rowIndex);

            var cellTotalLevel = rowFooter.GetCell(0);
            var cellTotalOutstanding = rowFooter.GetCell(1);
            var cellTotalRemaining = rowFooter.GetCell(2);

            cellTotalLevel.SetCellValue(Lang.total);
            cellTotalOutstanding.SetCellValue(totalOutstanding);
            cellTotalRemaining.SetCellValue(totalRemaining);
            //// end footer

            hssfworkbook.RemoveSheetAt(0);

            return hssfworkbook;
        }
    }
}