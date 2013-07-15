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
    using NPOI.HSSF.UserModel;
    using Apollo.Age.Resources;
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

        public HSSFWorkbook ExportsAgentWinLoss(AgentWinLossViewModel model)
        {
            if (model != null && model.AgentWinLossList != null && model.AgentWinLossList.Any())
            {
                var titleString = string.Format("{0} ({1}: {2} -- {3}: {4} -- {5}: {6} -- {7}: {8})",
                Lang.agentwinloss, Lang.from, ShortDateFormat(model.FromDate), Lang.to, ShortDateFormat(model.ToDate),
                Lang.sport, model.SportName, Lang.bettype, model.BettypeName);

                if (model.IsSuper)
                {
                    return AgentWinLossViewBySuper(model.AgentWinLossList, titleString, model.AgentName);
                }
                else
                {
                    return AgentWinLossViewByNoneSuper(model.AgentWinLossList, titleString, model.AgentName);
                }
            }
            else
            {
                return new HSSFWorkbook();
            }
        }

        public HSSFWorkbook ExportsMemberWinLoss(MemberWinLossViewModel model)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            if (model != null && model.MemberWinLossList != null && model.MemberWinLossList.Any())
            {
                var title = string.Format("{0} ({1}: {2} -- {3}: {4} -- {5}: {6} -- {7}: {8})",
                    Lang.lblMemberWinLoss, Lang.from, ShortDateFormat(model.FromDate), Lang.to, ShortDateFormat(model.ToDate),
                    Lang.sport, model.SportName, Lang.bettype, model.BettypeName);

                var sheet = hssfworkbook.CreateSheet(Lang.lblMemberWinLoss);

                #region declare style of cell
                HSSFPalette h = new HSSFPalette(new NPOI.HSSF.Record.PaletteRecord());
                HSSFColor footerColor = h.FindSimilarColor(153, 0, 0);

                var font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 11;
                font.Color = NPOI.HSSF.Util.HSSFColor.GREY_80_PERCENT.WHITE.index;
                //font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;

                ICellStyle headerStyle = hssfworkbook.CreateCellStyle();
                headerStyle.FillForegroundColor = HSSFColor.GREY_80_PERCENT.index;
                headerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
                headerStyle.FillBackgroundColor = HSSFColor.GREY_80_PERCENT.index;
                headerStyle.Alignment = HorizontalAlignment.CENTER;
                headerStyle.VerticalAlignment = VerticalAlignment.CENTER;
                headerStyle.SetFont(font);
                headerStyle.BorderTop = BorderStyle.DOTTED;
                headerStyle.BorderLeft = BorderStyle.DOTTED;
                headerStyle.BorderRight = BorderStyle.DOTTED;
                headerStyle.BorderBottom = BorderStyle.DOTTED;

                ICellStyle bodyStyle = hssfworkbook.CreateCellStyle();
                bodyStyle.Alignment = HorizontalAlignment.RIGHT;
                bodyStyle.VerticalAlignment = VerticalAlignment.CENTER;
                bodyStyle.BorderTop = BorderStyle.DOTTED;
                bodyStyle.BorderLeft = BorderStyle.DOTTED;
                bodyStyle.BorderRight = BorderStyle.DOTTED;
                bodyStyle.BorderBottom = BorderStyle.DOTTED;

                ICellStyle bodyStyle1 = hssfworkbook.CreateCellStyle();
                bodyStyle1.BorderTop = BorderStyle.DOTTED;
                bodyStyle1.BorderLeft = BorderStyle.DOTTED;
                bodyStyle1.BorderRight = BorderStyle.DOTTED;
                bodyStyle1.BorderBottom = BorderStyle.DOTTED;

                ICellStyle footerStyle = hssfworkbook.CreateCellStyle();
                footerStyle.FillForegroundColor = footerColor.GetIndex();
                footerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
                footerStyle.FillBackgroundColor = footerColor.GetIndex();
                footerStyle.Alignment = HorizontalAlignment.RIGHT;
                footerStyle.VerticalAlignment = VerticalAlignment.CENTER;
                footerStyle.BorderTop = BorderStyle.DOTTED;
                footerStyle.BorderLeft = BorderStyle.DOTTED;
                footerStyle.BorderRight = BorderStyle.DOTTED;
                footerStyle.BorderBottom = BorderStyle.DOTTED;

                ICellStyle footerStyle1 = hssfworkbook.CreateCellStyle();
                footerStyle1.FillForegroundColor = footerColor.GetIndex();
                footerStyle1.FillPattern = FillPatternType.SOLID_FOREGROUND;
                footerStyle1.FillBackgroundColor = footerColor.GetIndex();
                footerStyle1.VerticalAlignment = VerticalAlignment.CENTER;
                footerStyle1.BorderTop = BorderStyle.DOTTED;
                footerStyle1.BorderLeft = BorderStyle.DOTTED;
                footerStyle1.BorderRight = BorderStyle.DOTTED;
                footerStyle1.BorderBottom = BorderStyle.DOTTED;

                ICell cell;
                #endregion

                #region header
                // header
                var rowIndex = 3;
                var row = sheet.CreateRow(rowIndex);
                
                cell = row.CreateCell(0);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(string.Empty);

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
                cell.SetCellValue(Lang.member);

                cell = row.CreateCell(7);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(string.Empty);

                cell = row.CreateCell(8);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(string.Empty);

                cell = row.CreateCell(9);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(model.AgentName);

                cell = row.CreateCell(10);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(string.Empty);

                cell = row.CreateCell(11);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(string.Empty);

                cell = row.CreateCell(12);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.UplineTotal);

                rowIndex++;
                row = sheet.CreateRow(rowIndex);

                cell = row.CreateCell(6);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.lblWinLoss);

                cell = row.CreateCell(7);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.lblComm);

                cell = row.CreateCell(8);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.total);

                cell = row.CreateCell(9);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.lblWinLoss);

                cell = row.CreateCell(10);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.lblComm);

                cell = row.CreateCell(11);
                cell.CellStyle = headerStyle;
                cell.SetCellValue(Lang.total);

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

                    cell = row.CreateCell(0);
                    cell.CellStyle = bodyStyle1;

                    cell.SetCellValue(memberWinLoss.MemberName);

                    cell = row.CreateCell(1);
                    cell.CellStyle = bodyStyle;

                    cell.SetCellValue(FormatNumber20(memberWinLoss.SoccerShare / 100.00));

                    cell = row.CreateCell(2);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellValue(FormatNumber20(memberWinLoss.Turnover));

                    cell = row.CreateCell(3);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellValue(FormatNumber20(memberWinLoss.ActualTO));

                    cell = row.CreateCell(4);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellValue(FormatNumber20(memberWinLoss.Outstanding));

                    cell = row.CreateCell(5);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellValue(FormatNumber20(memberWinLoss.OutstandingShare));

                    cell = row.CreateCell(6);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellValue(FormatNumber20(memberWinLoss.WinLoss));

                    cell = row.CreateCell(7);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellValue(FormatNumber20(memberWinLoss.MemComm));

                    cell = row.CreateCell(8);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellValue(FormatNumber20(memberWinLoss.MemTotal));

                    cell = row.CreateCell(9);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellValue(FormatNumber20(memberWinLoss.AWinLoss));

                    cell = row.CreateCell(10);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellValue(FormatNumber20(memberWinLoss.AComm));

                    cell = row.CreateCell(11);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellValue(FormatNumber20(memberWinLoss.ATotal));

                    cell = row.CreateCell(12);
                    cell.CellStyle = bodyStyle;
                    cell.SetCellValue(FormatNumber20(memberWinLoss.UplineTotal));

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
                cell.SetCellValue(string.Empty);

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
                cell.SetCellValue(FormatNumber20(totalMWinLoss));

                cell = row.CreateCell(7);
                cell.CellStyle = footerStyle;
                cell.SetCellValue(FormatNumber20(totalMComm));

                cell = row.CreateCell(8);
                cell.CellStyle = footerStyle;
                cell.SetCellValue(FormatNumber20(totalMTotal));

                cell = row.CreateCell(9);
                cell.CellStyle = footerStyle;
                cell.SetCellValue(FormatNumber20(totalAWinLoss));

                cell = row.CreateCell(10);
                cell.CellStyle = footerStyle;
                cell.SetCellValue(FormatNumber20(totalAComm));

                cell = row.CreateCell(11);
                cell.CellStyle = footerStyle;
                cell.SetCellValue(FormatNumber20(totalATotal));

                cell = row.CreateCell(12);
                cell.CellStyle = footerStyle;
                cell.SetCellValue(FormatNumber20(totalUpline));
                #endregion footer

                #region auto size & freePane
                sheet.CreateFreezePane(0, 5);
                //// Auto-size each column
                for (var i = 0; i < sheet.GetRow(3).LastCellNum; i++)
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

                cell = rowTitle.CreateCell(0);
                cell.SetCellValue(title);
                cell.CellStyle = hssfworkbook.CreateCellStyle();
                cell.CellStyle.SetFont(font1);

                #endregion

                #region merge cell
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 12));
                sheet.AddMergedRegion(new CellRangeAddress(3, 4, 0, 0));
                sheet.AddMergedRegion(new CellRangeAddress(3, 4, 1, 1));
                sheet.AddMergedRegion(new CellRangeAddress(3, 4, 2, 2));
                sheet.AddMergedRegion(new CellRangeAddress(3, 4, 3, 3));
                sheet.AddMergedRegion(new CellRangeAddress(3, 4, 4, 4));
                sheet.AddMergedRegion(new CellRangeAddress(3, 4, 5, 5));
                sheet.AddMergedRegion(new CellRangeAddress(3, 3, 6, 8));
                sheet.AddMergedRegion(new CellRangeAddress(3, 3, 9, 11));
                sheet.AddMergedRegion(new CellRangeAddress(3, 4, 12, 12));
                #endregion
            }

            return hssfworkbook;
        }

        private HSSFWorkbook AgentWinLossViewBySuper(IList<AgentWinLossModel> agentWinLossList, string title, string agentName)
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
            headerStyle.FillForegroundColor = HSSFColor.GREY_80_PERCENT.index;
            headerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            headerStyle.FillBackgroundColor = HSSFColor.GREY_80_PERCENT.index;
            headerStyle.Alignment = HorizontalAlignment.CENTER;
            headerStyle.VerticalAlignment = VerticalAlignment.CENTER;
            headerStyle.SetFont(font);
            headerStyle.BorderTop = BorderStyle.DOTTED;
            headerStyle.BorderLeft = BorderStyle.DOTTED;
            headerStyle.BorderRight = BorderStyle.DOTTED;
            headerStyle.BorderBottom = BorderStyle.DOTTED;

            ICellStyle bodyStyle = hssfworkbook.CreateCellStyle();
            bodyStyle.Alignment = HorizontalAlignment.RIGHT;
            bodyStyle.VerticalAlignment = VerticalAlignment.CENTER;
            bodyStyle.BorderTop = BorderStyle.DOTTED;
            bodyStyle.BorderLeft = BorderStyle.DOTTED;
            bodyStyle.BorderRight = BorderStyle.DOTTED;
            bodyStyle.BorderBottom = BorderStyle.DOTTED;

            ICellStyle bodyStyle1 = hssfworkbook.CreateCellStyle();
            bodyStyle1.BorderTop = BorderStyle.DOTTED;
            bodyStyle1.BorderLeft = BorderStyle.DOTTED;
            bodyStyle1.BorderRight = BorderStyle.DOTTED;
            bodyStyle1.BorderBottom = BorderStyle.DOTTED;

            ICell cell;
            #endregion

            #region header
            // header
            var rowIndex = 3; // start at row 3
            var row = sheet.CreateRow(rowIndex);

            cell = row.CreateCell(0);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(string.Empty);

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
            cell.SetCellValue(string.Empty);

            cell = row.CreateCell(8);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(string.Empty);

            rowIndex++;
            row = sheet.CreateRow(rowIndex);

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
                else
                {
                    cell.SetCellValue(string.Empty);
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

            sheet.CreateFreezePane(0, 5);
            //// Auto-size each column
            for (var i = 0; i < sheet.GetRow(3).LastCellNum; i++)
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

            cell = rowTitle.CreateCell(0);
            cell.SetCellValue(title);
            cell.CellStyle = hssfworkbook.CreateCellStyle();
            cell.CellStyle.SetFont(font1);

            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 8));

            #endregion

            #region merge cell
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 0, 0));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 1, 1));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 2, 2));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 3, 3));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 4, 4));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 5, 5));
            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 6, 8));
            #endregion

            return hssfworkbook;
        }

        private HSSFWorkbook AgentWinLossViewByNoneSuper(IList<AgentWinLossModel> agentWinLossList, string title, string agentName)
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
            headerStyle.FillForegroundColor = HSSFColor.GREY_80_PERCENT.index;
            headerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            headerStyle.FillBackgroundColor = HSSFColor.GREY_80_PERCENT.index;
            headerStyle.Alignment = HorizontalAlignment.CENTER;
            headerStyle.VerticalAlignment = VerticalAlignment.CENTER;
            headerStyle.SetFont(font);
            headerStyle.BorderTop = BorderStyle.DOTTED;
            headerStyle.BorderLeft = BorderStyle.DOTTED;
            headerStyle.BorderRight = BorderStyle.DOTTED;
            headerStyle.BorderBottom = BorderStyle.DOTTED;

            ICellStyle bodyStyle = hssfworkbook.CreateCellStyle();
            bodyStyle.Alignment = HorizontalAlignment.RIGHT;
            bodyStyle.VerticalAlignment = VerticalAlignment.CENTER;
            bodyStyle.BorderTop = BorderStyle.DOTTED;
            bodyStyle.BorderLeft = BorderStyle.DOTTED;
            bodyStyle.BorderRight = BorderStyle.DOTTED;
            bodyStyle.BorderBottom = BorderStyle.DOTTED;

            ICellStyle bodyStyle1 = hssfworkbook.CreateCellStyle();
            bodyStyle1.BorderTop = BorderStyle.DOTTED;
            bodyStyle1.BorderLeft = BorderStyle.DOTTED;
            bodyStyle1.BorderRight = BorderStyle.DOTTED;
            bodyStyle1.BorderBottom = BorderStyle.DOTTED;

            ICell cell;

            ICellStyle footerStyle = hssfworkbook.CreateCellStyle();
            footerStyle.FillForegroundColor = footerColor.GetIndex();
            footerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            footerStyle.FillBackgroundColor = footerColor.GetIndex();
            footerStyle.Alignment = HorizontalAlignment.RIGHT;
            footerStyle.VerticalAlignment = VerticalAlignment.CENTER;
            footerStyle.BorderTop = BorderStyle.DOTTED;
            footerStyle.BorderLeft = BorderStyle.DOTTED;
            footerStyle.BorderRight = BorderStyle.DOTTED;
            footerStyle.BorderBottom = BorderStyle.DOTTED;

            ICellStyle footerStyle1 = hssfworkbook.CreateCellStyle();
            footerStyle1.FillForegroundColor = footerColor.GetIndex();
            footerStyle1.FillPattern = FillPatternType.SOLID_FOREGROUND;
            footerStyle1.FillBackgroundColor = footerColor.GetIndex();
            footerStyle1.VerticalAlignment = VerticalAlignment.CENTER;
            footerStyle1.BorderTop = BorderStyle.DOTTED;
            footerStyle1.BorderLeft = BorderStyle.DOTTED;
            footerStyle1.BorderRight = BorderStyle.DOTTED;
            footerStyle1.BorderBottom = BorderStyle.DOTTED;

            #endregion

            #region header
            var rowIndex = 3;
            var row = sheet.CreateRow(rowIndex);

            cell = row.CreateCell(0);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(string.Empty);

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
            cell.SetCellValue(string.Empty);

            cell = row.CreateCell(9);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(string.Empty);

            cell = row.CreateCell(10);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.UplineTotal);

            rowIndex++;
            row = sheet.CreateRow(rowIndex);

            cell = row.CreateCell(7);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.lblWinLoss);

            cell = row.CreateCell(8);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.lblComm);

            cell = row.CreateCell(9);
            cell.CellStyle = headerStyle;
            cell.SetCellValue(Lang.total);

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
                else
                {
                    cell.SetCellValue(string.Empty);
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
            cell.SetCellValue(string.Empty);

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
            sheet.CreateFreezePane(0, 5);
            //// Auto-size each column
            for (var i = 0; i < sheet.GetRow(3).LastCellNum; i++)
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

            cell = rowTitle.CreateCell(0);
            cell.SetCellValue(title);
            cell.CellStyle = hssfworkbook.CreateCellStyle();
            cell.CellStyle.SetFont(font1);
            #endregion

            #region merge cell
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 10));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 0, 0));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 1, 1));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 2, 2));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 3, 3));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 4, 4));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 5, 5));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 6, 6));
            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 7, 9));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 10, 10));

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

        private string ShortDateFormat(object dt)
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
