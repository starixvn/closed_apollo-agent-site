// --------------------------------------------------------------------------
// <copyright file="WinlossService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.11">Created</change>
//   <change who="Quinn.Vo" date="2013.7.16">Add function: ExportAgentWinLoss, ExportMemberWinLoss, ShortDateFormat,
//      AgentWinLossViewBySuper, AgentWinLossViewByNoneSuper, CreateNumberCell, CreateStringCell, FormatNumber20
//   </change>
// </history>

namespace Apollo.Age.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Resources;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public class WinLossService : ReportService, IWinLossService
    {
        /// <summary>
        /// Get List Match Entered from DB.
        /// </summary>
        /// <returns>List Match Entered.</returns>
        public List<MatchEnteredModel> GetMatchEntered()
        {
            var matchEnteredList = this.GetData<MatchEntered>(new MatchEnteredCriteria()).ToList();

            return ConvertToMatchEnteredModel(matchEnteredList);
        }
        
        /// <summary>
        /// Get List Agent Win Loss from DB.
        /// </summary>
        /// <param name="criteria">The input AgentWinLossCriteria.</param>
        /// <returns>List Agent Win Loss Model.</returns>
        public List<AgentWinLossModel> GetAgentWinLoss(AgentWinLossCriteria criteria)
        {
            var agentWinLossList = this.GetData<AgentWinLossModel>(criteria).ToList();

            return agentWinLossList;
        }

        /// <summary>
        /// Get List Member Win Loss from DB.
        /// </summary>
        /// <param name="criteria">The input MemberWinLossCriteria.</param>
        /// <returns>List Member Win Loss Model.</returns>
        public List<MemberWinLossModel> GetMemberWinLoss(MemberWinLossCriteria criteria)
        {
            var memberWinLossList = this.GetData<MemberWinLossModel>(criteria).ToList();

            return memberWinLossList;
        }

        /// <summary>
        /// Export Excel for Agent Win Loss.
        /// </summary>
        /// <param name="model">The input AgentWinLossViewModel to export Excel.</param>
        /// <returns>Agent Win Loss Excel File.</returns>
        public HSSFWorkbook ExportAgentWinLoss(AgentWinLossViewModel model)
        {
            if (model != null && model.AgentWinLossList != null && model.AgentWinLossList.Any())
            {
                var title = string.Format("{0} {1}", Lang.agentwinloss, Lang.lblReport);
                var filter = string.Format(
                    "{0} -- {1}: {2} ~ {3} -- {4}: {5} -- {6}: {7}",
                    model.AgentName, 
                    Lang.dateRange, 
                    ShortDateFormat(model.FromDate), 
                    ShortDateFormat(model.ToDate),
                    Lang.lblReport, 
                    model.SportName, 
                    Lang.WagerType, 
                    model.BettypeName);
                if (model.IsSuper)
                {
                    return AgentWinLossViewBySuper(model.AgentWinLossList, title, Lang.company, filter);
                }
                else
                {
                    return AgentWinLossViewByNoneSuper(model.AgentWinLossList, title, model.AgentName, filter);
                }
            }
            else
            {
                return new HSSFWorkbook();
            }
        }

        /// <summary>
        /// Export Excel for Member Win Loss.
        /// </summary>
        /// <param name="model">The input MemberWinLossViewModel to export Excel.</param>
        /// <returns>Member Win Loss Excel File.</returns>
        public HSSFWorkbook ExportMemberWinLoss(MemberWinLossViewModel model)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            if (model != null && model.MemberWinLossList != null && model.MemberWinLossList.Any())
            {
                var title = string.Format("{0} {1}", Lang.lblMemberWinLoss, Lang.lblReport);
                var filter = string.Format(
                    "{0} -- {1}: {2} ~ {3} -- {4}: {5} -- {6}: {7}",
                    model.AgentName, 
                    Lang.dateRange, 
                    ShortDateFormat(model.FromDate), 
                    ShortDateFormat(model.ToDate),
                    Lang.lblReport, 
                    model.SportName, 
                    Lang.WagerType, 
                    model.BettypeName);

                var sheet = hssfworkbook.CreateSheet(Lang.lblMemberWinLoss);

                #region declare style of cell
                HSSFPalette h = new HSSFPalette(new NPOI.HSSF.Record.PaletteRecord());
                HSSFColor footerColor = h.FindSimilarColor(153, 0, 0);

                var font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 11;
                font.Color = NPOI.HSSF.Util.HSSFColor.GREY_80_PERCENT.WHITE.index;

                ICellStyle headerStyle = hssfworkbook.CreateCellStyle();
                headerStyle.FillForegroundColor = HSSFColor.GREY_50_PERCENT.index;
                headerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
                headerStyle.FillBackgroundColor = HSSFColor.GREY_50_PERCENT.index;
                headerStyle.Alignment = HorizontalAlignment.CENTER;
                headerStyle.VerticalAlignment = VerticalAlignment.CENTER;
                headerStyle.SetFont(font);
                headerStyle.BorderTop = BorderStyle.THIN;
                headerStyle.BorderLeft = BorderStyle.THIN;
                headerStyle.BorderRight = BorderStyle.THIN;
                headerStyle.BorderBottom = BorderStyle.THIN;

                ICellStyle bodyStyle = hssfworkbook.CreateCellStyle();
                bodyStyle.Alignment = HorizontalAlignment.RIGHT;
                bodyStyle.VerticalAlignment = VerticalAlignment.CENTER;
                bodyStyle.BorderTop = BorderStyle.THIN;
                bodyStyle.BorderLeft = BorderStyle.THIN;
                bodyStyle.BorderRight = BorderStyle.THIN;
                bodyStyle.BorderBottom = BorderStyle.THIN;

                ICellStyle bodyStyle1 = hssfworkbook.CreateCellStyle();
                bodyStyle1.BorderTop = BorderStyle.THIN;
                bodyStyle1.BorderLeft = BorderStyle.THIN;
                bodyStyle1.BorderRight = BorderStyle.THIN;
                bodyStyle1.BorderBottom = BorderStyle.THIN;
                
                var font2 = hssfworkbook.CreateFont();
                font2.Color = HSSFColor.WHITE.index;

                ICellStyle footerStyle = hssfworkbook.CreateCellStyle();
                footerStyle.FillForegroundColor = footerColor.GetIndex();
                footerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
                footerStyle.FillBackgroundColor = footerColor.GetIndex();
                footerStyle.Alignment = HorizontalAlignment.RIGHT;
                footerStyle.VerticalAlignment = VerticalAlignment.CENTER;
                footerStyle.BorderTop = BorderStyle.THIN;
                footerStyle.BorderLeft = BorderStyle.THIN;
                footerStyle.BorderRight = BorderStyle.THIN;
                footerStyle.BorderBottom = BorderStyle.THIN;
                footerStyle.SetFont(font2);

                ICellStyle footerStyle1 = hssfworkbook.CreateCellStyle();
                footerStyle1.FillForegroundColor = footerColor.GetIndex();
                footerStyle1.FillPattern = FillPatternType.SOLID_FOREGROUND;
                footerStyle1.FillBackgroundColor = footerColor.GetIndex();
                footerStyle1.VerticalAlignment = VerticalAlignment.CENTER;
                footerStyle1.BorderTop = BorderStyle.THIN;
                footerStyle1.BorderLeft = BorderStyle.THIN;
                footerStyle1.BorderRight = BorderStyle.THIN;
                footerStyle1.BorderBottom = BorderStyle.THIN;
                footerStyle1.SetFont(font2);

                IDataFormat format = hssfworkbook.CreateDataFormat();
                var numberFormat = format.GetFormat("#,##0.00");

                #endregion

                #region header
                var rowIndex = 4;
                var columIndex = 0;
                var row = sheet.CreateRow(rowIndex);

                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, Lang.lblSoccerPT);
                CreateStringCell(row, columIndex++, headerStyle, Lang.BetAmount);
                CreateStringCell(row, columIndex++, headerStyle, Lang.CommBase);
                CreateStringCell(row, columIndex++, headerStyle, Lang.outstanding);
                CreateStringCell(row, columIndex++, headerStyle, Lang.ShareAmount);
                CreateStringCell(row, columIndex++, headerStyle, Lang.member);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, model.AgentName);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, Lang.UplineTotal);

                rowIndex++;
                columIndex = 0;
                row = sheet.CreateRow(rowIndex);

                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, Lang.lblWinLoss);
                CreateStringCell(row, columIndex++, headerStyle, Lang.lblComm);
                CreateStringCell(row, columIndex++, headerStyle, Lang.total);
                CreateStringCell(row, columIndex++, headerStyle, Lang.lblWinLoss);
                CreateStringCell(row, columIndex++, headerStyle, Lang.lblComm);
                CreateStringCell(row, columIndex++, headerStyle, Lang.total);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);

                rowIndex++;
                #endregion

                #region body
                //// Add data rows
                decimal totalTurnover = 0;
                decimal totalActual = 0;
                decimal totalMWinLoss = 0;
                decimal totalMComm = 0;
                decimal totalMTotal = 0;
                decimal totalAWinLoss = 0;
                decimal totalAComm = 0;
                decimal totalATotal = 0;
                decimal totalUpline = 0;
                decimal totalOutstanding = 0;
                decimal totalOutstandingShare = 0;
                
                foreach (var memberWinLoss in model.MemberWinLossList)
                {
                    columIndex = 0;
                    totalTurnover += memberWinLoss.Turnover;
                    totalActual += memberWinLoss.ActualTO;
                    totalMWinLoss += memberWinLoss.WinLoss;
                    totalMComm += memberWinLoss.MemComm;
                    totalMTotal += memberWinLoss.MemTotal;
                    totalAWinLoss += memberWinLoss.AWinLoss;
                    totalAComm += memberWinLoss.AComm;
                    totalATotal += memberWinLoss.ATotal;
                    totalUpline += memberWinLoss.UplineTotal;
                    totalOutstanding += memberWinLoss.Outstanding;
                    totalOutstandingShare += memberWinLoss.OutstandingShare;

                    row = sheet.CreateRow(rowIndex);

                    CreateStringCell(row, columIndex++, bodyStyle1, memberWinLoss.MemberName);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)(memberWinLoss.SoccerShare / 100.00));
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.Turnover);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.ActualTO);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.Outstanding);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.OutstandingShare);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.WinLoss);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.MemComm);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.MemTotal);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.AWinLoss);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.AComm);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.ATotal);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.UplineTotal);

                    rowIndex++;
                }

                #endregion

                #region footer
                row = sheet.CreateRow(rowIndex);
                columIndex = 0;

                CreateStringCell(row, columIndex++, footerStyle1, Lang.total);
                CreateStringCell(row, columIndex++, footerStyle, string.Empty);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalTurnover);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalActual);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalOutstanding);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalOutstandingShare);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalMWinLoss);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalMComm);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalMTotal);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalAWinLoss);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalAComm);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalATotal);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalUpline);
                #endregion footer

                #region auto size & freePane
                sheet.CreateFreezePane(0, 6);
                //// Auto-size each column
                for (var i = 0; i < sheet.GetRow(4).LastCellNum; i++)
                {
                    sheet.AutoSizeColumn(i);
                    var currentWidth = sheet.GetColumnWidth(i);
                    sheet.SetColumnWidth(i, currentWidth + 500);
                }
                
                #endregion

                #region title
                // row title report
                var rowTitle = sheet.CreateRow(1);
                var font1 = hssfworkbook.CreateFont();
                font1.FontHeightInPoints = 14;
                font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                ICellStyle titleStyle = hssfworkbook.CreateCellStyle();
                titleStyle.Alignment = HorizontalAlignment.CENTER;
                titleStyle.SetFont(font1);
                CreateStringCell(rowTitle, 0, titleStyle, title);

                // row filter
                var rowFilter = sheet.CreateRow(2);
                IFont fontFilter = hssfworkbook.CreateFont();
                fontFilter.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                fontFilter.IsItalic = true;
                fontFilter.Color = HSSFColor.BLUE.index;
                ICellStyle filterStyle = hssfworkbook.CreateCellStyle();
                filterStyle.SetFont(fontFilter);
                CreateStringCell(rowFilter, 0, filterStyle, filter);
                #endregion

                #region merge cell
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 12));
                sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 12));
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 0, 0));
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 1, 1));
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 2, 2));
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 3, 3));
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 4, 4));
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 5, 5));
                sheet.AddMergedRegion(new CellRangeAddress(4, 4, 6, 8));
                sheet.AddMergedRegion(new CellRangeAddress(4, 4, 9, 11));
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 12, 12));
                #endregion
            }

            return hssfworkbook;
        }

        /// <summary>
        /// Get Date Format (year-month-day).
        /// </summary>
        /// <param name="dt">Object date need format.</param>
        /// <returns>Return string date.</returns>
        public string ShortDateFormat(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return string.Empty;
            }

            return string.Format(new System.Globalization.CultureInfo("en-US"), "{0:yyyy-MM-dd}", dt);
        }

        /// <summary>
        /// Get Excel File by View Super.
        /// </summary>
        /// <param name="agentWinLossList">List Data need Export.</param>
        /// <param name="title">Title of File.</param>
        /// <param name="agentName">Current Agent Name.</param>
        /// <param name="filter">All parameters to query report.</param>
        /// <returns>Agent Win Loss Excel File.</returns>
        private HSSFWorkbook AgentWinLossViewBySuper(IList<AgentWinLossModel> agentWinLossList, string title, string agentName, string filter)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            var sheet = hssfworkbook.CreateSheet(Lang.agentwinloss);

            #region declare style of cell
            HSSFPalette h = new HSSFPalette(new NPOI.HSSF.Record.PaletteRecord());
            HSSFColor footerColor = h.FindSimilarColor(153, 0, 0);

            var font = hssfworkbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.Color = NPOI.HSSF.Util.HSSFColor.GREY_80_PERCENT.WHITE.index;

            ICellStyle headerStyle = hssfworkbook.CreateCellStyle();
            headerStyle.FillForegroundColor = HSSFColor.GREY_50_PERCENT.index;
            headerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            headerStyle.FillBackgroundColor = HSSFColor.GREY_50_PERCENT.index;
            headerStyle.Alignment = HorizontalAlignment.CENTER;
            headerStyle.VerticalAlignment = VerticalAlignment.CENTER;
            headerStyle.SetFont(font);
            headerStyle.BorderTop = BorderStyle.THIN;
            headerStyle.BorderLeft = BorderStyle.THIN;
            headerStyle.BorderRight = BorderStyle.THIN;
            headerStyle.BorderBottom = BorderStyle.THIN;

            ICellStyle bodyStyle = hssfworkbook.CreateCellStyle();
            bodyStyle.Alignment = HorizontalAlignment.RIGHT;
            bodyStyle.VerticalAlignment = VerticalAlignment.CENTER;
            bodyStyle.BorderTop = BorderStyle.THIN;
            bodyStyle.BorderLeft = BorderStyle.THIN;
            bodyStyle.BorderRight = BorderStyle.THIN;
            bodyStyle.BorderBottom = BorderStyle.THIN;

            ICellStyle bodyStyle1 = hssfworkbook.CreateCellStyle();
            bodyStyle1.BorderTop = BorderStyle.THIN;
            bodyStyle1.BorderLeft = BorderStyle.THIN;
            bodyStyle1.BorderRight = BorderStyle.THIN;
            bodyStyle1.BorderBottom = BorderStyle.THIN;

            IDataFormat format = hssfworkbook.CreateDataFormat();
            var numberFormat = format.GetFormat("#,##0.00");

            #endregion

            #region header
            var rowIndex = 4;
            var columIndex = 0;
            var row = sheet.CreateRow(rowIndex);

            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, Lang.lblSoccerPT);
            CreateStringCell(row, columIndex++, headerStyle, Lang.BetAmount);
            CreateStringCell(row, columIndex++, headerStyle, Lang.CommBase);
            CreateStringCell(row, columIndex++, headerStyle, agentName);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            
            rowIndex++;
            columIndex = 0;
            row = sheet.CreateRow(rowIndex);

            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, Lang.lblWinLoss);
            CreateStringCell(row, columIndex++, headerStyle, Lang.lblComm);
            CreateStringCell(row, columIndex++, headerStyle, Lang.total);

            rowIndex++;
            //// end header
            #endregion

            #region body

            //// Add data rows
            decimal totalTurnover = 0;
            decimal totalActual = 0;
            decimal totalGross = 0;
            decimal totalAWinLoss = 0;
            decimal totalAComm = 0;
            decimal totalATotal = 0;
            decimal totalUpline = 0;

            foreach (var agentWinLoss in agentWinLossList)
            {
                columIndex = 0;
                totalTurnover += agentWinLoss.Turnover;
                totalActual += agentWinLoss.ActualTO;
                totalGross += agentWinLoss.GrossTotal;
                totalAWinLoss += agentWinLoss.AWinLoss;
                totalAComm += agentWinLoss.AComm;
                totalATotal += agentWinLoss.ATotal;
                totalUpline += agentWinLoss.UplineTotal;

                row = sheet.CreateRow(rowIndex);
                
                CreateStringCell(row, columIndex++, bodyStyle1, agentWinLoss.AgentName);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)(agentWinLoss.SoccerShare / 100.00));
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.Turnover);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.ActualTO);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.AWinLoss);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.AComm);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.ATotal);

                rowIndex++;
            }

            #endregion

            #region auto size & freePane

            sheet.CreateFreezePane(0, 6);
            //// Auto-size each column
            for (var i = 0; i < sheet.GetRow(4).LastCellNum; i++)
            {
                sheet.AutoSizeColumn(i);
                var currentWidth = sheet.GetColumnWidth(i);
                sheet.SetColumnWidth(i, currentWidth + 500);
            }

            #endregion

            #region title

            var rowTitle = sheet.CreateRow(1);
            var font1 = hssfworkbook.CreateFont();
            font1.FontHeightInPoints = 14;
            font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
            ICellStyle titleStyle = hssfworkbook.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.CENTER;
            titleStyle.SetFont(font1);

            CreateStringCell(rowTitle, 0, titleStyle, title);

            var rowFilter = sheet.CreateRow(2);
           
            IFont fontFilter = hssfworkbook.CreateFont();
            fontFilter.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
            fontFilter.IsItalic = true;
            fontFilter.Color = HSSFColor.BLUE.index;
            ICellStyle filterStyle = hssfworkbook.CreateCellStyle();
            filterStyle.SetFont(fontFilter);

            CreateStringCell(rowFilter, 0, filterStyle, filter);
            #endregion

            #region merge cell
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 6));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 6));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 0, 0));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 1, 1));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 2, 2));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 3, 3));
            sheet.AddMergedRegion(new CellRangeAddress(4, 4, 4, 6));
            #endregion

            return hssfworkbook;
        }

        /// <summary>
        /// Get Excel File by View None Super.
        /// </summary>
        /// <param name="agentWinLossList">List Data need Export.</param>
        /// <param name="title">Title of File.</param>
        /// <param name="agentName">Current Agent Name.</param>
        /// <param name="filter">All parameters to query report.</param>
        /// <returns>Agent Win Loss Excel File.</returns>
        private HSSFWorkbook AgentWinLossViewByNoneSuper(IList<AgentWinLossModel> agentWinLossList, string title, string agentName, string filter)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            var sheet = hssfworkbook.CreateSheet(Lang.agentwinloss);

            #region declare style of cell

            HSSFPalette h = new HSSFPalette(new NPOI.HSSF.Record.PaletteRecord());
            HSSFColor footerColor = h.FindSimilarColor(153, 0, 0);

            var font = hssfworkbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.Color = NPOI.HSSF.Util.HSSFColor.GREY_80_PERCENT.WHITE.index;

            ICellStyle headerStyle = hssfworkbook.CreateCellStyle();
            headerStyle.FillForegroundColor = HSSFColor.GREY_50_PERCENT.index;
            headerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            headerStyle.FillBackgroundColor = HSSFColor.GREY_50_PERCENT.index;
            headerStyle.Alignment = HorizontalAlignment.CENTER;
            headerStyle.VerticalAlignment = VerticalAlignment.CENTER;
            headerStyle.SetFont(font);
            headerStyle.BorderTop = BorderStyle.THIN;
            headerStyle.BorderLeft = BorderStyle.THIN;
            headerStyle.BorderRight = BorderStyle.THIN;
            headerStyle.BorderBottom = BorderStyle.THIN;

            ICellStyle bodyStyle = hssfworkbook.CreateCellStyle();
            bodyStyle.Alignment = HorizontalAlignment.RIGHT;
            bodyStyle.VerticalAlignment = VerticalAlignment.CENTER;
            bodyStyle.BorderTop = BorderStyle.THIN;
            bodyStyle.BorderLeft = BorderStyle.THIN;
            bodyStyle.BorderRight = BorderStyle.THIN;
            bodyStyle.BorderBottom = BorderStyle.THIN;

            ICellStyle bodyStyle1 = hssfworkbook.CreateCellStyle();
            bodyStyle1.BorderTop = BorderStyle.THIN;
            bodyStyle1.BorderLeft = BorderStyle.THIN;
            bodyStyle1.BorderRight = BorderStyle.THIN;
            bodyStyle1.BorderBottom = BorderStyle.THIN;

            var font2 = hssfworkbook.CreateFont();
            font2.Color = HSSFColor.WHITE.index;
            ICellStyle footerStyle = hssfworkbook.CreateCellStyle();
            footerStyle.FillForegroundColor = footerColor.GetIndex();
            footerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            footerStyle.FillBackgroundColor = footerColor.GetIndex();
            footerStyle.Alignment = HorizontalAlignment.RIGHT;
            footerStyle.VerticalAlignment = VerticalAlignment.CENTER;
            footerStyle.BorderTop = BorderStyle.THIN;
            footerStyle.BorderLeft = BorderStyle.THIN;
            footerStyle.BorderRight = BorderStyle.THIN;
            footerStyle.BorderBottom = BorderStyle.THIN;
            footerStyle.SetFont(font2);

            ICellStyle footerStyle1 = hssfworkbook.CreateCellStyle();
            footerStyle1.FillForegroundColor = footerColor.GetIndex();
            footerStyle1.FillPattern = FillPatternType.SOLID_FOREGROUND;
            footerStyle1.FillBackgroundColor = footerColor.GetIndex();
            footerStyle1.VerticalAlignment = VerticalAlignment.CENTER;
            footerStyle1.BorderTop = BorderStyle.THIN;
            footerStyle1.BorderLeft = BorderStyle.THIN;
            footerStyle1.BorderRight = BorderStyle.THIN;
            footerStyle1.BorderBottom = BorderStyle.THIN;
            footerStyle1.SetFont(font2);

            IDataFormat format = hssfworkbook.CreateDataFormat();
            var numberFormat = format.GetFormat("#,##0.00");

            #endregion

            #region header
            var rowIndex = 4;
            var columIndex = 0;
            var row = sheet.CreateRow(rowIndex);

            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, Lang.lblSoccerPT);
            CreateStringCell(row, columIndex++, headerStyle, Lang.BetAmount);
            CreateStringCell(row, columIndex++, headerStyle, Lang.CommBase);
            CreateStringCell(row, columIndex++, headerStyle, Lang.GrossTotal);
            CreateStringCell(row, columIndex++, headerStyle, agentName);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, Lang.UplineTotal);

            rowIndex++;
            columIndex = 0;
            row = sheet.CreateRow(rowIndex);

            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, Lang.lblWinLoss);
            CreateStringCell(row, columIndex++, headerStyle, Lang.lblComm);
            CreateStringCell(row, columIndex++, headerStyle, Lang.total);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);

            rowIndex++;
            #endregion

            #region body
            //// Add data rows

            decimal totalTurnover = 0;
            decimal totalActual = 0;
            decimal totalGross = 0;
            decimal totalAWinLoss = 0;
            decimal totalAComm = 0;
            decimal totalATotal = 0;
            decimal totalUpline = 0;

            foreach (var agentWinLoss in agentWinLossList)
            {
                columIndex = 0;
                totalTurnover += agentWinLoss.Turnover;
                totalActual += agentWinLoss.ActualTO;
                totalGross += agentWinLoss.GrossTotal;
                totalAWinLoss += agentWinLoss.AWinLoss;
                totalAComm += agentWinLoss.AComm;
                totalATotal += agentWinLoss.ATotal;
                totalUpline += agentWinLoss.UplineTotal;

                row = sheet.CreateRow(rowIndex);

                if (agentWinLoss.IsAgent)
                {
                    CreateStringCell(row, columIndex++, bodyStyle1, agentWinLoss.AgentName);
                }
                else
                {
                    CreateStringCell(row, columIndex++, bodyStyle1, Lang.lblMemberWinLoss);
                }

                if (agentWinLoss.IsAgent)
                {
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)(agentWinLoss.SoccerShare / 100.00));
                }
                else
                {
                    CreateStringCell(row, columIndex++, bodyStyle, string.Empty);
                }

                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.Turnover);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.ActualTO);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.GrossTotal);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.AWinLoss);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.AComm);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.ATotal);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.UplineTotal);

                rowIndex++;
            }

            #endregion

            #region footer
            columIndex = 0;
            row = sheet.CreateRow(rowIndex);

            CreateStringCell(row, columIndex++, footerStyle1, Lang.total);
            CreateStringCell(row, columIndex++, footerStyle, string.Empty);
            CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalTurnover);
            CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalActual);
            CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalGross);
            CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalAWinLoss);
            CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalAComm);
            CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalATotal);
            CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalUpline);
            #endregion footer

            #region auto size & freePane
            sheet.CreateFreezePane(0, 6);
            //// Auto-size each column
            for (var i = 0; i < sheet.GetRow(4).LastCellNum; i++)
            {
                sheet.AutoSizeColumn(i);
                var currentWidth = sheet.GetColumnWidth(i);
                sheet.SetColumnWidth(i, currentWidth + 500);
            }

            #endregion

            #region title
            var rowTitle = sheet.CreateRow(1);
            var font1 = hssfworkbook.CreateFont();
            font1.FontHeightInPoints = 14;
            font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
            ICellStyle titleStyle = hssfworkbook.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.CENTER;
            titleStyle.SetFont(font1);
            CreateStringCell(rowTitle, 0, titleStyle, title);

            var rowFilter = sheet.CreateRow(2);
            IFont fontFilter = hssfworkbook.CreateFont();
            fontFilter.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
            fontFilter.IsItalic = true;
            fontFilter.Color = HSSFColor.BLUE.index;
            ICellStyle filterStyle = hssfworkbook.CreateCellStyle();
            filterStyle.SetFont(fontFilter);
            CreateStringCell(rowFilter, 0, filterStyle, filter);

            #endregion

            #region merge cell
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 8));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 8));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 0, 0));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 1, 1));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 2, 2));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 3, 3));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 4, 4));
            sheet.AddMergedRegion(new CellRangeAddress(4, 4, 5, 7));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 8, 8));

            #endregion

            return hssfworkbook;
        }

        /// <summary>
        /// Create new cell with format is number.
        /// </summary>
        /// <param name="row">Row need create cell.</param>
        /// <param name="columIndex">Index of column need create cell.</param>
        /// <param name="style">Style of cell.</param>
        /// <param name="numberFormat">Format of number.</param>
        /// <param name="value">Value in cell.</param>
        private void CreateNumberCell(IRow row, int columIndex, ICellStyle style, short numberFormat, double value)
        {
            var cell = row.CreateCell(columIndex);
            cell.CellStyle = style;
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle.DataFormat = numberFormat;
            cell.SetCellValue(value);
        }

        /// <summary>
        /// Create new cell with format is string.
        /// </summary>
        /// <param name="row">Row need create cell.</param>
        /// <param name="columIndex">Index of Column need create cell.</param>
        /// <param name="style">Style of cell.</param>
        /// <param name="value">Value in cell.</param>
        private void CreateStringCell(IRow row, int columIndex, ICellStyle style, string value)
        {
            var cell = row.CreateCell(columIndex);
            cell.CellStyle = style;
            cell.SetCellValue(value);
        }

        /// <summary>
        /// Get number format with 2 digit.
        /// </summary>
        /// <param name="obj">Input object.</param>
        /// <returns>A string format value.</returns>
        private string FormatNumber20(object obj)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }

            return string.Format("{0:#,##0.00}", obj);
        }

        /// <summary>
        /// Convert object from DB return to object map with view.
        /// </summary>
        /// <param name="enteredFromList">List MatchEntered object.</param>
        /// <returns>List MatchEnteredModel object.</returns>
        private List<MatchEnteredModel> ConvertToMatchEnteredModel(List<MatchEntered> enteredFromList)
        {
            List<MatchEnteredModel> enteredToList = null;
            enteredFromList = enteredFromList.OrderBy(t => t.SportType).ThenBy(t => t.EventDate).ToList();

            if (enteredFromList != null && enteredFromList.Any())
            {
                var sportType = 0;
                MatchEnteredModel matchEntered = null;
                enteredToList = new List<MatchEnteredModel>();

                foreach (var item in enteredFromList)
                {
                    if (item.SportType == sportType)
                    {
                        matchEntered.TodayEntered = item.Entered;
                        matchEntered.TodayNonEntered = item.NonEntered;
                    }
                    else
                    {
                        if (sportType != 0)
                        {
                            enteredToList.Add(matchEntered);
                            matchEntered = null;
                        }

                        if (item.IsToday)
                        {
                            matchEntered = new MatchEnteredModel()
                            {
                                SportTypeId = item.SportType,
                                TodayEntered = item.Entered,
                                TodayNonEntered = item.NonEntered
                            };
                        }
                        else
                        {
                            matchEntered = new MatchEnteredModel()
                            {
                                SportTypeId = item.SportType,
                                YesterdayEntered = item.Entered,
                                YesterdayNonEntered = item.NonEntered
                            };
                        }

                        sportType = item.SportType;
                    }
                }

                enteredToList.Add(matchEntered);
            }

            return enteredToList;
        }
    }
}
