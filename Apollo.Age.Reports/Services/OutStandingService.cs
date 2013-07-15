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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Services.Impl;
    using NPOI.HSSF.UserModel;
    using Apollo.Age.Reports.Criterias;
    using NPOI.SS.UserModel;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Models;
    using NPOI.HSSF.Util;
    using Apollo.Age.Resources;
    using NPOI.SS.Util;


    public class OutstandingService : ReportService
    {
        /// <summary>
        /// Get agent outstanding report
        /// </summary>
        /// <param name="isSupper"></param>
        /// <param name="CurrentAgentId"></param>
        /// <param name="parentId"></param>
        /// <param name="agentId"></param>
        /// <param name="sporttypes"></param>
        /// <param name="bettypes"></param>
        /// <returns></returns>
        public OutstandingViewModel GetAgentOutStandingReport(bool isSupper, 
            int? CurrentAgentId, 
            int parentId, 
            int? agentId, 
            string sporttypes, 
            string bettypes)
        {
            var mainShareHolder = new Apollo.Age.Entities.Agent();

            var outstandingViewModel = new OutstandingViewModel()
            {
                CurrentAgentId = CurrentAgentId.Value,
                MainShareHolder = mainShareHolder,
                Sporttypes = sporttypes,
                Bettypes = bettypes
            };

            try
            {
                if (sporttypes != null)
                {
                    IEnumerable<Agent> modelAgentDownLine = AgentDownlinePath(parentId, CurrentAgentId.Value);

                    if (isSupper)
                    {
                        if (modelAgentDownLine.Count() > 1)
                        {
                            mainShareHolder = modelAgentDownLine.ToList()[1];
                        }
                    }

                    IEnumerable<OutstandingModel> models = GetData<OutstandingModel>(new OutstandingCriteria
                    {
                        AgentId = CurrentAgentId.Value,
                        Sporttypes = sporttypes,
                        Bettypes = bettypes
                    });

                    outstandingViewModel.ListOutStanding = models;
                    outstandingViewModel.DownlinePath = modelAgentDownLine;
                    outstandingViewModel.AgentLevel = modelAgentDownLine.LastOrDefault().Level;
                }

                outstandingViewModel.SportList = SingbetSportService.GetSport(isOther: false);
                outstandingViewModel.BettypeList = GetBettype();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return outstandingViewModel;
        }

        /// <summary>
        /// Get Member outstanding report
        /// </summary>
        /// <param name="CurrentAgentId"></param>
        /// <param name="agentId"></param>
        /// <param name="sporttypes"></param>
        /// <param name="bettypes"></param>
        /// <param name="mainShareHolderId"></param>
        /// <param name="mainShareHolderUsername"></param>
        /// <returns></returns>
        public MemberOutstadingViewModel GetMemberOutstandingReport(int? CurrentAgentId, int? agentId, string sporttypes, string bettypes, int mainShareHolderId, string mainShareHolderUsername)
        {
            try
            {
                IEnumerable<OutstandingModel> models = GetData<OutstandingModel>(new MemberOutstandingCriteria
                {
                    AgentId = CurrentAgentId.Value,
                    Sporttypes = sporttypes,
                    Bettypes = bettypes
                });

                //// Sum total amount
                OutstandingModel totalModels = new OutstandingModel();
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

                return new MemberOutstadingViewModel
                {
                    ListOutstanding = models,
                    Total = totalModels,
                    MainShareHolderId = mainShareHolderId,
                    MainShareHolderUsername = mainShareHolderUsername
                };
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return new MemberOutstadingViewModel();
            }
        }

        /// <summary>
        /// Export Outstanding report excel file
        /// </summary>
        /// <param name="isAgentOutStanding"></param>
        /// <param name="listOutStanding"></param>
        /// <param name="currentUsername"></param>
        /// <param name="sportName"></param>
        /// <param name="betTypeName"></param>
        /// <param name="isSuper"></param>
        /// <returns></returns>
        public HSSFWorkbook ExportOutstandingReport(
            bool isAgentOutStanding,
            IEnumerable<OutstandingModel> listOutStanding,
            string currentUsername,
            string sportName,
            string betTypeName,
            bool isSuper)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet("Out Standing Report");

            try
            {
                if (listOutStanding != null
                    && listOutStanding.Any())
                {

                    RenderExcelTitle(hssfworkbook, sheet, 2, isAgentOutStanding, currentUsername, sportName, betTypeName);
                    RenderExcelHeader(hssfworkbook, sheet, 5);
                    var rowIndex = 6;
                    var length = listOutStanding.Count();
                    //create the format instance
                    IDataFormat format = hssfworkbook.CreateDataFormat();
                    var numberFormat = format.GetFormat("#,##0.00");
                    double totalOutstanding = 0, totalShareAmount = 0, currentOutStanding = 0, currentShareAmount = 0;
                    string userName = string.Empty;

                    ICellStyle bodyStyle = hssfworkbook.CreateCellStyle();
                    bodyStyle.BorderTop = BorderStyle.DOTTED;
                    bodyStyle.BorderLeft = BorderStyle.DOTTED;
                    bodyStyle.BorderRight = BorderStyle.DOTTED;
                    bodyStyle.BorderBottom = BorderStyle.DOTTED;

                    for (int i = 0; i < length; i++)
                    {
                        IRow row = sheet.CreateRow(rowIndex + i);
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
                        RenderExcelBodyRows(hssfworkbook, sheet, rowIndex + i, userName, currentOutStanding,
                            currentShareAmount,
                            numberFormat,
                            false,
                            bodyStyle);
                        totalOutstanding += currentOutStanding;
                        totalShareAmount += currentShareAmount;
                    }

                    //// If not is supper, then add total row
                    if (!isSuper)
                    {
                        ICellStyle footerStyle = hssfworkbook.CreateCellStyle();
                        footerStyle.FillForegroundColor = HSSFColor.DARK_RED.index;
                        footerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
                        footerStyle.FillBackgroundColor = HSSFColor.DARK_RED.index;
                        RenderExcelBodyRows(hssfworkbook, sheet, rowIndex + length, Lang.total, totalOutstanding,
                                totalShareAmount,
                                numberFormat,
                                true,
                                footerStyle);
                    }

                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return hssfworkbook;
        }

        /// <summary>
        /// Render Excel Outstanding Report body rows
        /// </summary>
        /// <param name="workBook"></param>
        /// <param name="sheet"></param>
        /// <param name="rowIndex"></param>
        /// <param name="userName"></param>
        /// <param name="outstandingAmount"></param>
        /// <param name="shareAmount"></param>
        /// <param name="numberFormat"></param>
        /// <param name="isTotal"></param>
        /// <param name="cellStyle"></param>
        private void RenderExcelBodyRows(
            HSSFWorkbook workBook,
            ISheet sheet,
            int rowIndex,
            string userName,
            double outstandingAmount,
            double shareAmount,
            short numberFormat,
            bool isTotal,
            ICellStyle cellStyle )
        {
            IRow row = sheet.CreateRow(rowIndex);
            var userNameCell = row.CreateCell(0);
            var outstadingValue = row.CreateCell(1);
            var shareAmountValue = row.CreateCell(2);

            if (cellStyle != null)
            {
                userNameCell.CellStyle = cellStyle;
                outstadingValue.CellStyle = cellStyle;
                shareAmountValue.CellStyle = cellStyle;
            }
            //// Username Cell
            userNameCell.SetCellType(CellType.STRING);
            userNameCell.SetCellValue(userName);

            //// Outstanding Value Cell
            outstadingValue.SetCellType(CellType.NUMERIC);
            outstadingValue.SetCellValue(outstandingAmount);
            outstadingValue.CellStyle.DataFormat = numberFormat;

            //// Share Amount Cell
            shareAmountValue.SetCellType(CellType.NUMERIC);
            shareAmountValue.SetCellValue(shareAmount);
            shareAmountValue.CellStyle.DataFormat = numberFormat;
        }

        /// <summary>
        /// Render Outstanding Report Title
        /// </summary>
        /// <param name="workBook"></param>
        /// <param name="sheet"></param>
        /// <param name="fromRow"></param>
        /// <param name="isAgent"></param>
        /// <param name="userName"></param>
        /// <param name="sportName"></param>
        /// <param name="betTypeName"></param>
        private void RenderExcelTitle(HSSFWorkbook workBook, 
            ISheet sheet, 
            int fromRow, 
            bool isAgent,
            string userName,
            string sportName, 
            string betTypeName)
        {
            //// UserName text
            var userNameRow = sheet.CreateRow(fromRow - 1);
            var reportType = isAgent ? Lang.agent : Lang.member;
            ICell userNameCell = userNameRow.CreateCell(0);
            userNameCell.SetCellValue(string.Format("{0}: {1}", Lang.agent, userName));
            sheet.AddMergedRegion(new CellRangeAddress(fromRow - 1, fromRow - 1, 0, 2));

            //// Report Title
            var row = sheet.CreateRow(fromRow);
            ICell cell = row.CreateCell(0);
            cell.SetCellValue(string.Format("{0} {1} {2} on {3}", reportType, Lang.outstanding, Lang.lblReport, DateTime.Now.ToString("yyyy-MM-dd")));
            ICellStyle style = workBook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.CENTER;
            //create a font style
            IFont font = workBook.CreateFont();
            font.FontHeight = 15 * 20;
            style.SetFont(font);
            cell.CellStyle = style;

            sheet.AddMergedRegion(new CellRangeAddress(fromRow, fromRow, 0, 2));

            //// BetType and Sport Type Name
            var informationRow = sheet.CreateRow(fromRow + 1);
            ICell informationCell = informationRow.CreateCell(0);
            informationCell.SetCellValue(string.Format("{0}: {1}, {2}: {3}", Lang.lblReport, sportName, Lang.WagerType, betTypeName));

            sheet.AddMergedRegion(new CellRangeAddress(fromRow + 1, fromRow + 1, 0, 2));

        }

        /// <summary>
        /// Render Outstanding header report
        /// </summary>
        /// <param name="workBook"></param>
        /// <param name="sheet"></param>
        /// <param name="fromRow"></param>
        private void RenderExcelHeader(HSSFWorkbook workBook, ISheet sheet, int fromRow)
        {
            var row = sheet.CreateRow(fromRow);
            var font1 = workBook.CreateFont();
            font1.FontHeightInPoints = 11;
            font1.Color = NPOI.HSSF.Util.HSSFColor.GREY_80_PERCENT.WHITE.index;
            font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;

            ICellStyle headerStyle = workBook.CreateCellStyle();
            headerStyle.FillForegroundColor = HSSFColor.GREY_80_PERCENT.index;
            headerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            headerStyle.FillBackgroundColor = HSSFColor.GREY_80_PERCENT.index;
            headerStyle.Alignment = HorizontalAlignment.CENTER;
            headerStyle.VerticalAlignment = VerticalAlignment.CENTER;
            headerStyle.SetFont(font1);
            headerStyle.BorderTop = BorderStyle.DOTTED;
            headerStyle.BorderLeft = BorderStyle.DOTTED;
            headerStyle.BorderRight = BorderStyle.DOTTED;
            headerStyle.BorderBottom = BorderStyle.DOTTED;

            var usernameCell = row.CreateCell(0);
            usernameCell.CellStyle = headerStyle;
            usernameCell.SetCellValue(string.Empty);

            var outStandingCell = row.CreateCell(1);
            outStandingCell.CellStyle = headerStyle;
            outStandingCell.SetCellValue(Lang.outstanding);

            var shareAmountCell = row.CreateCell(2);
            shareAmountCell.CellStyle = headerStyle;
            shareAmountCell.SetCellValue(Lang.ShareAmount);

            sheet.SetColumnWidth(0, 35 * 256);
            sheet.SetColumnWidth(1, 35 * 256);
            sheet.SetColumnWidth(2, 35 * 256);
        }

    }
}