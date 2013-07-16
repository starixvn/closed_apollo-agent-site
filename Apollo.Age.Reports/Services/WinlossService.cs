// --------------------------------------------------------------------------
// <copyright file="WinlossService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.11">Created</change>
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
        public List<MatchEnteredModel> GetMatchEntered()
        {
            var matchEnteredList = this.GetData<MatchEntered>(new MatchEnteredCriteria()).ToList();

            return ConvertToMatchEnteredModel(matchEnteredList);
        }

        public List<AgentWinLossModel> GetAgentWinLoss(AgentWinLossCriteria criteria)
        {
            var agentWinLossList = this.GetData<AgentWinLossModel>(criteria).ToList();

            return agentWinLossList;
        }

        public List<MemberWinLossModel> GetMemberWinLoss(MemberWinLossCriteria criteria)
        {
            var memberWinLossList = this.GetData<MemberWinLossModel>(criteria).ToList();

            return memberWinLossList;
        }

        public HSSFWorkbook ExportAgentWinLoss(AgentWinLossViewModel model)
        {
            if (model != null && model.AgentWinLossList != null && model.AgentWinLossList.Any())
            {
                var title = string.Format("{0} {1}", Lang.agentwinloss, Lang.lblReport);
                var filter = string.Format("{0} -- {1}: {2} ~ {3} -- {4}: {5} -- {6}: {7}",
                    model.AgentName, Lang.dateRange, ShortDateFormat(model.FromDate), ShortDateFormat(model.ToDate),
                    Lang.lblReport, model.SportName, Lang.WagerType, model.BettypeName);
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

        public HSSFWorkbook ExportMemberWinLoss(MemberWinLossViewModel model)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            if (model != null && model.MemberWinLossList != null && model.MemberWinLossList.Any())
            {
                var title = string.Format("{0} {1}", Lang.lblMemberWinLoss, Lang.lblReport);
                var filter = string.Format("{0} -- {1}: {2} ~ {3} -- {4}: {5} -- {6}: {7}",
                    model.AgentName, Lang.dateRange, ShortDateFormat(model.FromDate), ShortDateFormat(model.ToDate),
                    Lang.lblReport, model.SportName, Lang.WagerType, model.BettypeName);

                var sheet = hssfworkbook.CreateSheet(Lang.lblMemberWinLoss);

                #region declare style of cell
                HSSFPalette h = new HSSFPalette(new NPOI.HSSF.Record.PaletteRecord());
                HSSFColor footerColor = h.FindSimilarColor(153, 0, 0);

                var font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 11;
                font.Color = NPOI.HSSF.Util.HSSFColor.GREY_80_PERCENT.WHITE.index;
                //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;

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

                ICell cell;
                #endregion

                #region header
                // header
                var rowIndex = 4;
                var columIndex = 0;
                var row = sheet.CreateRow(rowIndex);

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.lblSoccerPT);
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.BetAmount);
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.CommBase);
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.outstanding);
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.ShareAmount);
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.member);
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(model.AgentName);
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.UplineTotal);

                rowIndex++;
                columIndex = 0;
                row = sheet.CreateRow(rowIndex);

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.lblWinLoss);
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.lblComm);
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.total);
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.lblWinLoss);
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.lblComm);
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.total);
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = headerStyle;

                rowIndex++;
                //// end header
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

                    cell = row.CreateCell(columIndex);
                    cell.CellStyle = bodyStyle1;
                    cell.SetCellType(CellType.STRING);
                    cell.SetCellValue(memberWinLoss.MemberName);
                    columIndex++;

                    cell = row.CreateCell(columIndex);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellType(CellType.NUMERIC);
                    cell.CellStyle.DataFormat = numberFormat;
                    cell.SetCellValue((double)(memberWinLoss.SoccerShare / 100.00));
                    columIndex++;

                    cell = row.CreateCell(columIndex);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellType(CellType.NUMERIC);
                    cell.CellStyle.DataFormat = numberFormat;
                    cell.SetCellValue((double)(memberWinLoss.Turnover));
                    columIndex++;

                    cell = row.CreateCell(columIndex);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellType(CellType.NUMERIC);
                    cell.CellStyle.DataFormat = numberFormat;
                    cell.SetCellValue((double)memberWinLoss.ActualTO);
                    columIndex++;

                    cell = row.CreateCell(columIndex);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellType(CellType.NUMERIC);
                    cell.CellStyle.DataFormat = numberFormat;
                    cell.SetCellValue((double)memberWinLoss.Outstanding);
                    columIndex++;

                    cell = row.CreateCell(columIndex);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellType(CellType.NUMERIC);
                    cell.CellStyle.DataFormat = numberFormat;
                    cell.SetCellValue((double)memberWinLoss.OutstandingShare);
                    columIndex++;

                    cell = row.CreateCell(columIndex);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellType(CellType.NUMERIC);
                    cell.CellStyle.DataFormat = numberFormat;
                    cell.SetCellValue((double)memberWinLoss.WinLoss);
                    columIndex++;

                    cell = row.CreateCell(columIndex);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellType(CellType.NUMERIC);
                    cell.CellStyle.DataFormat = numberFormat;
                    cell.SetCellValue((double)memberWinLoss.MemComm);
                    columIndex++;

                    cell = row.CreateCell(columIndex);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellType(CellType.NUMERIC);
                    cell.CellStyle.DataFormat = numberFormat;
                    cell.SetCellValue((double)memberWinLoss.MemTotal);
                    columIndex++;

                    cell = row.CreateCell(columIndex);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellType(CellType.NUMERIC);
                    cell.CellStyle.DataFormat = numberFormat;
                    cell.SetCellValue((double)memberWinLoss.AWinLoss);
                    columIndex++;

                    cell = row.CreateCell(columIndex);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellType(CellType.NUMERIC);
                    cell.CellStyle.DataFormat = numberFormat;
                    cell.SetCellValue((double)memberWinLoss.AComm);
                    columIndex++;

                    cell = row.CreateCell(columIndex);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellType(CellType.NUMERIC);
                    cell.CellStyle.DataFormat = numberFormat;
                    cell.SetCellValue((double)memberWinLoss.ATotal);
                    columIndex++;

                    cell = row.CreateCell(columIndex);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellType(CellType.NUMERIC);
                    cell.CellStyle.DataFormat = numberFormat;
                    cell.SetCellValue((double)memberWinLoss.UplineTotal);

                    rowIndex++;
                }

                #endregion

                #region footer
                row = sheet.CreateRow(rowIndex);
                columIndex = 0;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = footerStyle1;
                cell.SetCellType(CellType.STRING);
                cell.SetCellValue(Lang.total);
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = footerStyle;
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = footerStyle;
                cell.SetCellType(CellType.NUMERIC);
                cell.CellStyle.DataFormat = numberFormat;
                cell.SetCellValue((double)(totalTurnover));
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = footerStyle;
                cell.SetCellType(CellType.NUMERIC);
                cell.CellStyle.DataFormat = numberFormat;
                cell.SetCellValue((double)(totalActual));
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = footerStyle;
                cell.SetCellType(CellType.NUMERIC);
                cell.CellStyle.DataFormat = numberFormat;
                cell.SetCellValue((double)(totalOutstanding));
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = footerStyle;
                cell.SetCellType(CellType.NUMERIC);
                cell.CellStyle.DataFormat = numberFormat;
                cell.SetCellValue((double)(totalOutstandingShare));
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = footerStyle;
                cell.SetCellType(CellType.NUMERIC);
                cell.CellStyle.DataFormat = numberFormat;
                cell.SetCellValue((double)(totalMWinLoss));
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = footerStyle;
                cell.SetCellType(CellType.NUMERIC);
                cell.CellStyle.DataFormat = numberFormat;
                cell.SetCellValue((double)(totalMComm));
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = footerStyle;
                cell.SetCellType(CellType.NUMERIC);
                cell.CellStyle.DataFormat = numberFormat;
                cell.SetCellValue((double)(totalMTotal));
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = footerStyle;
                cell.SetCellType(CellType.NUMERIC);
                cell.CellStyle.DataFormat = numberFormat;
                cell.SetCellValue((double)(totalAWinLoss));
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = footerStyle;
                cell.SetCellType(CellType.NUMERIC);
                cell.CellStyle.DataFormat = numberFormat;
                cell.SetCellValue((double)(totalAComm));
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = footerStyle;
                cell.SetCellType(CellType.NUMERIC);
                cell.CellStyle.DataFormat = numberFormat;
                cell.SetCellValue((double)(totalATotal));
                columIndex++;

                cell = row.CreateCell(columIndex);
                cell.CellStyle = footerStyle;
                cell.SetCellType(CellType.NUMERIC);
                cell.CellStyle.DataFormat = numberFormat;
                cell.SetCellValue((double)(totalUpline));
                #endregion footer

                #region auto size & freePane
                sheet.CreateFreezePane(0, 6);
                //// Auto-size each column
                for (var i = 0; i < sheet.GetRow(4).LastCellNum; i++)
                {
                    sheet.AutoSizeColumn(i);
                    sheet.SetColumnWidth(i, (sheet.GetColumnWidth(i) + 500));
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

                cell = rowTitle.CreateCell(0);
                cell.SetCellValue(title);
                cell.CellStyle = titleStyle;

                var rowFilter = sheet.CreateRow(2);
                ICell filterCell = rowFilter.CreateCell(0);
                IFont fontFilter = hssfworkbook.CreateFont();
                fontFilter.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                fontFilter.IsItalic = true;
                fontFilter.Color = HSSFColor.BLUE.index;
                ICellStyle filterStyle = hssfworkbook.CreateCellStyle();
                filterStyle.Alignment = HorizontalAlignment.RIGHT;
                filterStyle.SetFont(fontFilter);
                filterCell.CellStyle = filterStyle;
                filterCell.SetCellValue(filter);

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
            //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;

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

            ICell cell;
            #endregion

            #region header
            // header
            var rowIndex = 4; // start at row 3
            var row = sheet.CreateRow(rowIndex);

            cell = row.CreateCell(0);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(1);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.lblSoccerPT);

            cell = row.CreateCell(2);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.BetAmount);

            cell = row.CreateCell(3);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.CommBase);

            cell = row.CreateCell(4);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.outstanding);

            cell = row.CreateCell(5);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.ShareAmount);

            cell = row.CreateCell(6);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(agentName);

            cell = row.CreateCell(7);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(8);
            cell.CellStyle = headerStyle;

            rowIndex++;
            row = sheet.CreateRow(rowIndex);

            cell = row.CreateCell(0);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(1);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(2);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(3);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(4);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(5);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(6);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.lblWinLoss);

            cell = row.CreateCell(7);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.lblComm);

            cell = row.CreateCell(8);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.total);

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
            decimal totalOutstanding = 0;
            decimal totalOutstandingShare = 0;

            foreach (var agentWinLoss in agentWinLossList)
            {

                totalTurnover += agentWinLoss.Turnover;
                totalActual += agentWinLoss.ActualTO;
                totalGross += agentWinLoss.GrossTotal;
                totalAWinLoss += agentWinLoss.AWinLoss;
                totalAComm += agentWinLoss.AComm;
                totalATotal += agentWinLoss.ATotal;
                totalUpline += agentWinLoss.UplineTotal;
                totalOutstanding += agentWinLoss.Outstanding;
                totalOutstandingShare += agentWinLoss.OutstandingShare;

                row = sheet.CreateRow(rowIndex);

                cell = row.CreateCell(0);
                cell.CellStyle = bodyStyle1;
                if (agentWinLoss.IsAgent)
                {
                    cell.SetCellValue(agentWinLoss.AgentName);
                }
                else
                {
                    cell.SetCellValue(Lang.lblMemberWinLoss);
                }

                cell = row.CreateCell(1);
                cell.CellStyle = bodyStyle;
                if (agentWinLoss.IsAgent)
                {
                    cell.SetCellValue(FormatNumber20(agentWinLoss.SoccerShare / 100.00));
                }

                cell = row.CreateCell(2);
                cell.CellStyle = bodyStyle;
                cell.SetCellValue(FormatNumber20(agentWinLoss.Turnover));

                cell = row.CreateCell(3);
                cell.CellStyle = bodyStyle;
                cell.SetCellValue(FormatNumber20(agentWinLoss.ActualTO));

                cell = row.CreateCell(4);
                cell.CellStyle = bodyStyle;
                cell.SetCellValue(FormatNumber20(agentWinLoss.Outstanding));

                cell = row.CreateCell(5);
                cell.CellStyle = bodyStyle;
                cell.SetCellValue(FormatNumber20(agentWinLoss.OutstandingShare));

                cell = row.CreateCell(6);
                cell.CellStyle = bodyStyle;
                cell.SetCellValue(FormatNumber20(agentWinLoss.AWinLoss));

                cell = row.CreateCell(7);
                cell.CellStyle = bodyStyle;
                cell.SetCellValue(FormatNumber20(agentWinLoss.AComm));

                cell = row.CreateCell(8);
                cell.CellStyle = bodyStyle;
                cell.SetCellValue(FormatNumber20(agentWinLoss.ATotal));

                rowIndex++;
            }

            #endregion

            #region auto size & freePane

            sheet.CreateFreezePane(0, 6);
            //// Auto-size each column
            for (var i = 0; i < sheet.GetRow(4).LastCellNum; i++)
            {
                sheet.AutoSizeColumn(i);
                sheet.SetColumnWidth(i, (sheet.GetColumnWidth(i) + 500));
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

            cell = rowTitle.CreateCell(0);
            cell.SetCellValue(title);
            cell.CellStyle = titleStyle;

            var rowFilter = sheet.CreateRow(2);
            ICell filterCell = rowFilter.CreateCell(0);
            IFont fontFilter = hssfworkbook.CreateFont();
            fontFilter.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
            fontFilter.IsItalic = true;
            fontFilter.Color = HSSFColor.BLUE.index;
            ICellStyle filterStyle = hssfworkbook.CreateCellStyle();
            filterStyle.Alignment = HorizontalAlignment.RIGHT;
            filterStyle.SetFont(fontFilter);
            filterCell.CellStyle = filterStyle;
            filterCell.SetCellValue(filter);
            #endregion

            #region merge cell
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 8));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 8));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 0, 0));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 1, 1));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 2, 2));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 3, 3));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 4, 4));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 5, 5));
            sheet.AddMergedRegion(new CellRangeAddress(4, 4, 6, 8));
            #endregion

            return hssfworkbook;
        }

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
            //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;

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

            ICell cell;

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
            var row = sheet.CreateRow(rowIndex);

            cell = row.CreateCell(0);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(1);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.lblSoccerPT);

            cell = row.CreateCell(2);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.BetAmount);

            cell = row.CreateCell(3);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.CommBase);

            cell = row.CreateCell(4);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.outstanding);

            cell = row.CreateCell(5);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.ShareAmount);

            cell = row.CreateCell(6);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.GrossTotal);

            cell = row.CreateCell(7);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(agentName);

            cell = row.CreateCell(8);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(9);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(10);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.UplineTotal);

            rowIndex++;
            row = sheet.CreateRow(rowIndex);

            cell = row.CreateCell(0);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(1);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(2);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(3);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(4);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(5);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(6);
            cell.CellStyle = headerStyle;

            cell = row.CreateCell(7);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.lblWinLoss);

            cell = row.CreateCell(8);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.lblComm);

            cell = row.CreateCell(9);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.total);

            cell = row.CreateCell(10);
            cell.CellStyle = headerStyle;

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
            decimal totalOutstanding = 0;
            decimal totalOutstandingShare = 0;

            foreach (var agentWinLoss in agentWinLossList)
            {
                totalTurnover += agentWinLoss.Turnover;
                totalActual += agentWinLoss.ActualTO;
                totalGross += agentWinLoss.GrossTotal;
                totalAWinLoss += agentWinLoss.AWinLoss;
                totalAComm += agentWinLoss.AComm;
                totalATotal += agentWinLoss.ATotal;
                totalUpline += agentWinLoss.UplineTotal;
                totalOutstanding += agentWinLoss.Outstanding;
                totalOutstandingShare += agentWinLoss.OutstandingShare;

                row = sheet.CreateRow(rowIndex);

                cell = row.CreateCell(0);
                cell.CellStyle = bodyStyle1;
                if (agentWinLoss.IsAgent)
                {
                    cell.SetCellValue(agentWinLoss.AgentName);
                }
                else
                {
                    cell.SetCellValue(Lang.lblMemberWinLoss);
                }

                cell = row.CreateCell(1);
                cell.CellStyle = bodyStyle;
                if (agentWinLoss.IsAgent)
                {
                    cell.SetCellValue(FormatNumber20(agentWinLoss.SoccerShare / 100.00));
                }

                cell = row.CreateCell(2);
                cell.CellStyle = bodyStyle;
                cell.SetCellValue(FormatNumber20(agentWinLoss.Turnover));

                cell = row.CreateCell(3);
                cell.CellStyle = bodyStyle;
                cell.SetCellValue(FormatNumber20(agentWinLoss.ActualTO));

                cell = row.CreateCell(4);
                cell.CellStyle = bodyStyle;
                cell.SetCellValue(FormatNumber20(agentWinLoss.Outstanding));

                cell = row.CreateCell(5);
                cell.CellStyle = bodyStyle;
                cell.SetCellValue(FormatNumber20(agentWinLoss.OutstandingShare));

                cell = row.CreateCell(6);
                cell.CellStyle = bodyStyle;
                cell.SetCellValue(FormatNumber20(agentWinLoss.GrossTotal));

                cell = row.CreateCell(7);
                cell.CellStyle = bodyStyle;
                cell.SetCellValue(FormatNumber20(agentWinLoss.AWinLoss));

                cell = row.CreateCell(8);
                cell.CellStyle = bodyStyle;
                cell.SetCellValue(FormatNumber20(agentWinLoss.AComm));

                cell = row.CreateCell(9);
                cell.CellStyle = bodyStyle;
                cell.SetCellValue(FormatNumber20(agentWinLoss.ATotal));

                cell = row.CreateCell(10);
                cell.CellStyle = bodyStyle;
                cell.SetCellValue(FormatNumber20(agentWinLoss.UplineTotal));

                rowIndex++;
            }

            #endregion

            #region footer
            row = sheet.CreateRow(rowIndex);

            cell = row.CreateCell(0);
            cell.CellStyle = footerStyle1;
            cell.SetCellValue(Lang.total);

            cell = row.CreateCell(1);
            cell.CellStyle = footerStyle;

            cell = row.CreateCell(2);
            cell.CellStyle = footerStyle;
            cell.SetCellValue(FormatNumber20(totalTurnover));

            cell = row.CreateCell(3);
            cell.CellStyle = footerStyle;
            cell.SetCellValue(FormatNumber20(totalActual));

            cell = row.CreateCell(4);
            cell.CellStyle = footerStyle;
            cell.SetCellValue(FormatNumber20(totalOutstanding));

            cell = row.CreateCell(5);
            cell.CellStyle = footerStyle;
            cell.SetCellValue(FormatNumber20(totalOutstandingShare));

            cell = row.CreateCell(6);
            cell.CellStyle = footerStyle;
            cell.SetCellValue(FormatNumber20(totalGross));

            cell = row.CreateCell(7);
            cell.CellStyle = footerStyle;
            cell.SetCellValue(FormatNumber20(totalAWinLoss));

            cell = row.CreateCell(8);
            cell.CellStyle = footerStyle;
            cell.SetCellValue(FormatNumber20(totalAComm));

            cell = row.CreateCell(9);
            cell.CellStyle = footerStyle;
            cell.SetCellValue(FormatNumber20(totalATotal));

            cell = row.CreateCell(10);
            cell.CellStyle = footerStyle;
            cell.SetCellValue(FormatNumber20(totalUpline));
            #endregion footer

            #region auto size & freePane
            sheet.CreateFreezePane(0, 6);
            //// Auto-size each column
            for (var i = 0; i < sheet.GetRow(4).LastCellNum; i++)
            {
                sheet.AutoSizeColumn(i);
                sheet.SetColumnWidth(i, (sheet.GetColumnWidth(i) + 500));
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

            cell = rowTitle.CreateCell(0);
            cell.SetCellValue(title);
            cell.CellStyle = titleStyle;

            var rowFilter = sheet.CreateRow(2);
            ICell filterCell = rowFilter.CreateCell(0);
            IFont fontFilter = hssfworkbook.CreateFont();
            fontFilter.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
            fontFilter.IsItalic = true;
            fontFilter.Color = HSSFColor.BLUE.index;
            ICellStyle filterStyle = hssfworkbook.CreateCellStyle();
            filterStyle.Alignment = HorizontalAlignment.RIGHT;
            filterStyle.SetFont(fontFilter);
            filterCell.CellStyle = filterStyle;
            filterCell.SetCellValue(filter);
            #endregion

            #region merge cell
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 10));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 10));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 0, 0));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 1, 1));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 2, 2));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 3, 3));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 4, 4));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 5, 5));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 6, 6));
            sheet.AddMergedRegion(new CellRangeAddress(4, 4, 7, 9));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 10, 10));

            #endregion

            return hssfworkbook;
        }

        private string FormatNumber20(object obj)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }

            return string.Format("{0:#,##0.00}", obj);
        }

        public string ShortDateFormat(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return string.Empty;
            }

            return string.Format(new System.Globalization.CultureInfo("en-US"), "{0:yyyy-MM-dd}", dt);
        }

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
