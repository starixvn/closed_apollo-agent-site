// --------------------------------------------------------------------------
// <copyright file="OutstandingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.12">Add Export Excel</change>
// </history>

namespace Apollo.Age.Reports.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Resources;
    using Apollo.Age.Services.Impl;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Outstanding service class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
    public class OutstandingService : ReportService
    {
        /// <summary>
        /// Get agent outstanding report.
        /// </summary>
        /// <param name="isSuper">Is super or no.</param>
        /// <param name="currentAgentId">Current Agent Id.</param>
        /// <param name="parentId">Agent Id of user login.</param>
        /// <param name="sporttypes">Sport type group.</param>
        /// <param name="bettypes">Bet type group.</param>
        /// <returns>Outstanding ViewModel.</returns>
        public OutstandingViewModel GetAgentOutStandingReport(
            bool isSuper,
            int? currentAgentId,
            int parentId,
            string sporttypes,
            string bettypes)
        {
            var mainShareHolder = new Apollo.Age.Entities.Agent();

            var outstandingViewModel = new OutstandingViewModel()
            {
                CurrentAgentId = currentAgentId.Value,
                MainShareHolder = mainShareHolder,
                SportTypeIds = sporttypes,
                BetTypeIds = bettypes
            };

            if (sporttypes != null)
            {
                IEnumerable<Agent> modelAgentDownLine = AgentDownlinePath(parentId, currentAgentId.Value);

                if (isSuper)
                {
                    if (modelAgentDownLine.Count() > 1)
                    {
                        mainShareHolder = modelAgentDownLine.ToList()[1];
                    }
                }

                IEnumerable<OutstandingModel> models = GetData<OutstandingModel>(new OutstandingCriteria
                {
                    AgentId = currentAgentId.Value,
                    Sporttypes = sporttypes == "-99" ? "0" : sporttypes,
                    Bettypes = bettypes
                });

                outstandingViewModel.ListOutStanding = models;
                outstandingViewModel.DownlinePath = modelAgentDownLine;
                outstandingViewModel.AgentLevel = modelAgentDownLine.LastOrDefault().Level;
            }

            outstandingViewModel.SportList = SingbetSportService.GetSport(isOther: false);
            outstandingViewModel.BettypeList = sporttypes == "-99" ? GetParlayBettype() : GetBettype(sporttypes == "-99" || sporttypes == "0");

            return outstandingViewModel;
        }

        /// <summary>
        /// Get Member outstanding report.
        /// </summary>
        /// <param name="currentAgentId">Current Agent Id.</param>
        /// <param name="sporttypes">Sport type group.</param>
        /// <param name="bettypes">Bet type group.</param>
        /// <param name="mainShareHolderId">Main ShareHolder Id.</param>
        /// <param name="mainShareHolderUsername">Main ShareHolder Name.</param>
        /// <returns>Member Outstanding ViewModel.</returns>
        public MemberOutstandingViewModel GetMemberOutstandingReport(
            int? currentAgentId,
            string sporttypes,
            string bettypes,
            int mainShareHolderId,
            string mainShareHolderUsername)
        {
            try
            {
                IEnumerable<OutstandingModel> models = GetData<OutstandingModel>(new MemberOutstandingCriteria
                {
                    AgentId = currentAgentId.Value,
                    Sporttypes = sporttypes == "-99" ? "0" : sporttypes,
                    Bettypes = bettypes
                });

                //// Sum total amount
                var totalModels = new OutstandingModel();
                if (models != null && models.Any())
                {
                    foreach (OutstandingModel item in models)
                    {
                        totalModels.Outstanding += item.Outstanding;
                        totalModels.ShareAmount += item.ShareAmount;
                        totalModels.GameOutstanding += item.GameOutstanding;
                        totalModels.GameShareAmount += item.GameShareAmount;
                    }
                }

                return new MemberOutstandingViewModel
                {
                    ListOutstanding = models,
                    Total = totalModels,
                    MainShareHolderId = mainShareHolderId,
                    MainShareHolderUsername = mainShareHolderUsername,
                };
            }
            catch
            {
                return new MemberOutstandingViewModel();
            }
        }

        /// <summary>
        /// Export Outstanding report excel file.
        /// </summary>
        /// <param name="isAgentOutStanding">Is Agent OutStanding.</param>
        /// <param name="listOutStanding">List OutStanding data.</param>
        /// <param name="currentUsername">Current username.</param>
        /// <param name="sportName">Sport name.</param>
        /// <param name="betTypeName">Bet type name.</param>
        /// <param name="isSuper">Is super or no.</param>
        /// <returns>Outstanding Excel file.</returns>
        public HSSFWorkbook ExportOutstandingReport(
            bool isAgentOutStanding,
            IEnumerable<OutstandingModel> listOutStanding,
            string currentUsername,
            string sportName,
            string betTypeName,
            bool isSuper)
        {
            var hssfworkbook = new HSSFWorkbook();
            CustomizeColor(hssfworkbook);
            ISheet sheet = hssfworkbook.CreateSheet("OutStanding Report");

            if (listOutStanding != null && listOutStanding.Any())
            {
                RenderExcelTitle(hssfworkbook, sheet, 1, isAgentOutStanding, currentUsername, sportName, betTypeName);
                RenderExcelHeader(hssfworkbook, sheet, 4);
                var rowIndex = 5;
                var length = listOutStanding.Count();
                //// create the format instance
                IDataFormat format = hssfworkbook.CreateDataFormat();
                var numberFormat = format.GetFormat("#,##0.00");
                double totalOutstanding = 0, totalShareAmount = 0, currentOutStanding = 0, currentShareAmount = 0;
                string userName = string.Empty;

                var bodyStyleRight = CreateStyle(hssfworkbook, align: HorizontalAlignment.Right);
                var bodyStyleLeft = CreateStyle(hssfworkbook);

                sheet.CreateFreezePane(0, rowIndex);

                for (int i = 0; i < length; i++)
                {
                    var currentOutstanding = listOutStanding.ElementAt(i);
                    //// If current agent level is agent, then add member outstanding
                    if (!currentOutstanding.IsAgent && isAgentOutStanding)
                    {
                        userName = Lang.lblMemberOutstanding;
                    }
                    else
                    {
                        userName = currentOutstanding.UserName;
                    }

                    currentOutStanding = (double)currentOutstanding.Outstanding;
                    currentShareAmount = (double)currentOutstanding.ShareAmount;
                    RenderExcelBodyRows(                        
                        sheet,
                        rowIndex + i,
                        userName,
                        currentOutStanding,
                        currentShareAmount,
                        numberFormat,
                        bodyStyleLeft,
                        bodyStyleRight);
                    totalOutstanding += currentOutStanding;
                    totalShareAmount += currentShareAmount;
                }

                //// If not is supper, then add total row
                if (!isSuper)
                {
                    var fontFooter = CreateFont(hssfworkbook, HSSFColor.White.Index, 10);
                    var footerStyleRight = CreateStyle(hssfworkbook, HSSFColor.Red.Index, HorizontalAlignment.Right, fontFooter);
                    var footerStyleLeft = CreateStyle(hssfworkbook, HSSFColor.Red.Index, HorizontalAlignment.Left, fontFooter);

                    RenderExcelBodyRows(
                        sheet,
                        rowIndex + length,
                        Lang.total,
                        totalOutstanding,
                        totalShareAmount,
                        numberFormat,
                        footerStyleLeft,
                        footerStyleRight);
                }
            }

            return hssfworkbook;
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
            string userName,
            double outstandingAmount,
            double shareAmount,
            short numberFormat,
            ICellStyle bodyStyleLeft,
            ICellStyle bodyStyleRight)
        {
            IRow row = sheet.CreateRow(rowIndex);

            CreateStringCell(row, 0, bodyStyleLeft, userName);
            CreateNumberCell(row, 1, bodyStyleRight, numberFormat, outstandingAmount);
            CreateNumberCell(row, 2, bodyStyleRight, numberFormat, shareAmount);
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
            bool isAgent,
            string userName,
            string sportName,
            string betTypeName)
        {
            var reportType = isAgent ? Lang.agent : Lang.member;

            //// Report Title
            var row = sheet.CreateRow(fromRow);

            var fontTitle = CreateFont(workBook, HSSFColor.Black.Index, 14, true);
            var styleTitle = CreateStyle(workBook, align: HorizontalAlignment.Center, font: fontTitle, isBorder: false);
            CreateStringCell(row, 0, styleTitle, string.Format("{0} {1} {2}", reportType, Lang.outstanding, Lang.lblReport));
            sheet.AddMergedRegion(new CellRangeAddress(fromRow, fromRow, 0, 2));

            var rowFilter = sheet.CreateRow(rownum: 2);
            var fontFilter = CreateFont(workBook, HSSFColor.Blue.Index, 11, true, true);
            var styleFilter = CreateStyle(workBook, font: fontFilter, isBorder: false);
            CreateStringCell(rowFilter, 0, styleFilter, string.Format("{0} -- {1}: {2} -- {3}: {4}", userName, Lang.lblReport, sportName, Lang.WagerType, betTypeName));

            sheet.AddMergedRegion(new CellRangeAddress(fromRow + 1, fromRow + 1, 0, 2));
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

            CreateStringCell(row, 0, headerStyle, string.Empty);
            CreateStringCell(row, 1, headerStyle, Lang.outstanding);
            CreateStringCell(row, 2, headerStyle, Lang.ShareAmount);

            sheet.SetColumnWidth(0, 35 * 256);
            sheet.SetColumnWidth(1, 35 * 256);
            sheet.SetColumnWidth(2, 35 * 256);
        }
    }
}