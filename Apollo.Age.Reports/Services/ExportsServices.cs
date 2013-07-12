// --------------------------------------------------------------------------
// <copyright file="ExportsServices.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.7.12">Created</change>
// </history>

namespace Apollo.Age.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Resources;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    
    public class ExportsServices
    {
        public void ExportsAgentWinLoss(HSSFWorkbook workbook, IList<AgentWinLossModel> agentWinLossList, 
            string agentName, string sportName, string bettypeName, DateTime fromDate, DateTime toDate, bool isSuper)
        {
            if (isSuper)
            {
                AgentWinLossViewBySuper(workbook, agentWinLossList, Lang.company, sportName, bettypeName, fromDate, toDate);
            }
            else
            {
                AgentWinLossViewByNoneSuper(workbook, agentWinLossList, agentName, sportName, bettypeName, fromDate, toDate);
            }
        }

        public void AgentWinLossViewBySuper(HSSFWorkbook workbook, IList<AgentWinLossModel> agentWinLossList,
            string agentName, string sportName, string bettypeName, DateTime fromDate, DateTime toDate)
        {
            HSSFPalette h = new HSSFPalette(new NPOI.HSSF.Record.PaletteRecord());
            HSSFColor footerColor = h.FindSimilarColor(153, 0, 0);

            var titleString = string.Format("{0} ({1}: {2} -- {3}: {4} -- {5}: {6} -- {7}: {8})",
                Lang.agentwinloss, Lang.from, ShortDateFormat(fromDate), Lang.to, ShortDateFormat(toDate),
                Lang.sport, sportName, Lang.bettype, bettypeName);

            var sheet = workbook.CreateSheet(Lang.agentwinloss);

            #region header
            // header
            var rowIndex = 3;
            var row = sheet.CreateRow(rowIndex);
            var font1 = workbook.CreateFont();
            font1.FontHeightInPoints = 11;
            font1.Color = NPOI.HSSF.Util.HSSFColor.GREY_80_PERCENT.WHITE.index;
            font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;

            ICellStyle headerStyle = workbook.CreateCellStyle();
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

            var cell1 = row.CreateCell(0);
            cell1.CellStyle = headerStyle;
            cell1.SetCellValue(string.Empty);

            var cell2 = row.CreateCell(1);
            cell2.CellStyle = headerStyle;
            cell2.SetCellValue(Lang.lblSoccerPT);

            var cell3 = row.CreateCell(2);
            cell3.CellStyle = headerStyle;
            cell3.SetCellValue(Lang.BetAmount);

            var cell4 = row.CreateCell(3);
            cell4.CellStyle = headerStyle;
            cell4.SetCellValue(Lang.CommBase);

            var cell5 = row.CreateCell(4);
            cell5.CellStyle = headerStyle;
            cell5.SetCellValue(Lang.outstanding);

            var cell6 = row.CreateCell(5);
            cell6.CellStyle = headerStyle;
            cell6.SetCellValue(Lang.ShareAmount);

            var cell8 = row.CreateCell(6);
            cell8.CellStyle = headerStyle;
            cell8.SetCellValue(agentName);

            var cell9 = row.CreateCell(7);
            cell9.CellStyle = headerStyle;
            cell9.SetCellValue(string.Empty);

            var cell10 = row.CreateCell(8);
            cell10.CellStyle = headerStyle;
            cell10.SetCellValue(string.Empty);

            rowIndex++;
            row = sheet.CreateRow(rowIndex);

            var cell12 = row.CreateCell(6);
            cell12.CellStyle = headerStyle;
            cell12.SetCellValue(Lang.lblWinLoss);

            var cell13 = row.CreateCell(7);
            cell13.CellStyle = headerStyle;
            cell13.SetCellValue(Lang.lblComm);

            var cell14 = row.CreateCell(8);
            cell14.CellStyle = headerStyle;
            cell14.SetCellValue(Lang.total);

            rowIndex++;
            //// end header
            #endregion

            #region body
            //// body

            ICellStyle bodyStyle = workbook.CreateCellStyle();
            bodyStyle.Alignment = HorizontalAlignment.RIGHT;
            bodyStyle.VerticalAlignment = VerticalAlignment.CENTER;
            bodyStyle.BorderTop = BorderStyle.DOTTED;
            bodyStyle.BorderLeft = BorderStyle.DOTTED;
            bodyStyle.BorderRight = BorderStyle.DOTTED;
            bodyStyle.BorderBottom = BorderStyle.DOTTED;

            ICellStyle bodyStyle1 = workbook.CreateCellStyle();
            bodyStyle1.BorderTop = BorderStyle.DOTTED;
            bodyStyle1.BorderLeft = BorderStyle.DOTTED;
            bodyStyle1.BorderRight = BorderStyle.DOTTED;
            bodyStyle1.BorderBottom = BorderStyle.DOTTED;

            ICell cellBody;
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

                cellBody = row.CreateCell(0);
                cellBody.CellStyle = bodyStyle1;
                if (agentWinLoss.IsAgent)
                {
                    cellBody.SetCellValue(agentWinLoss.AgentName);
                }
                else
                {
                    cellBody.SetCellValue(Lang.lblMemberWinLoss);
                }

                cellBody = row.CreateCell(1);
                cellBody.CellStyle = bodyStyle;
                if (agentWinLoss.IsAgent)
                {
                    cellBody.SetCellValue(FormatNumber20(agentWinLoss.SoccerShare / 100.00));
                }
                else
                {
                    cellBody.SetCellValue(string.Empty);
                }

                cellBody = row.CreateCell(2);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(agentWinLoss.Turnover));

                cellBody = row.CreateCell(3);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(agentWinLoss.ActualTO));

                cellBody = row.CreateCell(4);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(agentWinLoss.Outstanding));

                cellBody = row.CreateCell(5);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(agentWinLoss.OutstandingShare));

                cellBody = row.CreateCell(6);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(agentWinLoss.AWinLoss));

                cellBody = row.CreateCell(7);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(agentWinLoss.AComm));

                cellBody = row.CreateCell(8);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(agentWinLoss.ATotal));

                rowIndex++;
            }

            #endregion

            sheet.CreateFreezePane(0, 5);
            //// Auto-size each column
            for (var i = 0; i < sheet.GetRow(3).LastCellNum; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            #region title

            var rowTitle = sheet.CreateRow(1);
            var font = workbook.CreateFont();
            font.FontHeightInPoints = 14;
            font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;

            var cell = rowTitle.CreateCell(0);
            cell.SetCellValue(titleString);
            cell.CellStyle = workbook.CreateCellStyle();
            cell.CellStyle.SetFont(font);

            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 8));

            #endregion

            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 0, 0));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 1, 1));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 2, 2));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 3, 3));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 4, 4));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 5, 5));
            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 6, 8));
        }

        public void AgentWinLossViewByNoneSuper(HSSFWorkbook workbook, IList<AgentWinLossModel> agentWinLossList,
            string agentName, string sportName, string bettypeName, DateTime fromDate, DateTime toDate)
        {
            HSSFPalette h = new HSSFPalette(new NPOI.HSSF.Record.PaletteRecord());
            HSSFColor footerColor = h.FindSimilarColor(153, 0, 0);

            var titleString = string.Format("{0} ({1}: {2} -- {3}: {4} -- {5}: {6} -- {7}: {8})",
                Lang.agentwinloss, Lang.from, ShortDateFormat(fromDate), Lang.to, ShortDateFormat(toDate),
                Lang.sport, sportName, Lang.bettype, bettypeName);

            var sheet = workbook.CreateSheet(Lang.agentwinloss);

            #region header
            // header
            var rowIndex = 3;
            var row = sheet.CreateRow(rowIndex);
            var font1 = workbook.CreateFont();
            font1.FontHeightInPoints = 11;
            font1.Color = NPOI.HSSF.Util.HSSFColor.GREY_80_PERCENT.WHITE.index;
            font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;

            ICellStyle headerStyle = workbook.CreateCellStyle();
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

            var cell1 = row.CreateCell(0);
            cell1.CellStyle = headerStyle;
            cell1.SetCellValue(string.Empty);

            var cell2 = row.CreateCell(1);
            cell2.CellStyle = headerStyle;
            cell2.SetCellValue(Lang.lblSoccerPT);

            var cell3 = row.CreateCell(2);
            cell3.CellStyle = headerStyle;
            cell3.SetCellValue(Lang.BetAmount);

            var cell4 = row.CreateCell(3);
            cell4.CellStyle = headerStyle;
            cell4.SetCellValue(Lang.CommBase);

            var cell5 = row.CreateCell(4);
            cell5.CellStyle = headerStyle;
            cell5.SetCellValue(Lang.outstanding);

            var cell6 = row.CreateCell(5);
            cell6.CellStyle = headerStyle;
            cell6.SetCellValue(Lang.ShareAmount);

            var cell7 = row.CreateCell(6);
            cell7.CellStyle = headerStyle;
            cell7.SetCellValue(Lang.GrossTotal);

            var cell8 = row.CreateCell(7);
            cell8.CellStyle = headerStyle;
            cell8.SetCellValue(agentName);

            var cell9 = row.CreateCell(8);
            cell9.CellStyle = headerStyle;
            cell9.SetCellValue(string.Empty);

            var cell10 = row.CreateCell(9);
            cell10.CellStyle = headerStyle;
            cell10.SetCellValue(string.Empty);

            var cell11 = row.CreateCell(10);
            cell11.CellStyle = headerStyle;
            cell11.SetCellValue(Lang.UplineTotal);

            rowIndex++;
            row = sheet.CreateRow(rowIndex);

            var cell12 = row.CreateCell(7);
            cell12.CellStyle = headerStyle;
            cell12.SetCellValue(Lang.lblWinLoss);

            var cell13 = row.CreateCell(8);
            cell13.CellStyle = headerStyle;
            cell13.SetCellValue(Lang.lblComm);

            var cell14 = row.CreateCell(9);
            cell14.CellStyle = headerStyle;
            cell14.SetCellValue(Lang.total);

            rowIndex++;
            //// end header
            #endregion

            #region body
            //// body

            ICellStyle bodyStyle = workbook.CreateCellStyle();
            bodyStyle.Alignment = HorizontalAlignment.RIGHT;
            bodyStyle.VerticalAlignment = VerticalAlignment.CENTER;
            bodyStyle.BorderTop = BorderStyle.DOTTED;
            bodyStyle.BorderLeft = BorderStyle.DOTTED;
            bodyStyle.BorderRight = BorderStyle.DOTTED;
            bodyStyle.BorderBottom = BorderStyle.DOTTED;

            ICellStyle bodyStyle1 = workbook.CreateCellStyle();
            bodyStyle1.BorderTop = BorderStyle.DOTTED;
            bodyStyle1.BorderLeft = BorderStyle.DOTTED;
            bodyStyle1.BorderRight = BorderStyle.DOTTED;
            bodyStyle1.BorderBottom = BorderStyle.DOTTED;

            ICell cellBody;
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

                cellBody = row.CreateCell(0);
                cellBody.CellStyle = bodyStyle1;
                if (agentWinLoss.IsAgent)
                {
                    cellBody.SetCellValue(agentWinLoss.AgentName);
                }
                else
                {
                    cellBody.SetCellValue(Lang.lblMemberWinLoss);
                }

                cellBody = row.CreateCell(1);
                cellBody.CellStyle = bodyStyle;
                if (agentWinLoss.IsAgent)
                {
                    cellBody.SetCellValue(FormatNumber20(agentWinLoss.SoccerShare / 100.00));
                }
                else
                {
                    cellBody.SetCellValue(string.Empty);
                }

                cellBody = row.CreateCell(2);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(agentWinLoss.Turnover));

                cellBody = row.CreateCell(3);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(agentWinLoss.ActualTO));

                cellBody = row.CreateCell(4);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(agentWinLoss.Outstanding));

                cellBody = row.CreateCell(5);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(agentWinLoss.OutstandingShare));

                cellBody = row.CreateCell(6);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(agentWinLoss.GrossTotal));

                cellBody = row.CreateCell(7);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(agentWinLoss.AWinLoss));

                cellBody = row.CreateCell(8);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(agentWinLoss.AComm));

                cellBody = row.CreateCell(9);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(agentWinLoss.ATotal));

                cellBody = row.CreateCell(10);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(agentWinLoss.UplineTotal));

                //cellBody = row.CreateCell(11);
                //cellBody.CellStyle = bodyStyle;
                //cellBody.SetCellValue(totalWorkingHours.TotalHours.ToString("#.##"));

                rowIndex++;
            }

            #endregion

            #region footer

            row = sheet.CreateRow(rowIndex);

            ICellStyle footerStyle = workbook.CreateCellStyle();
            footerStyle.FillForegroundColor = footerColor.GetIndex();
            footerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            footerStyle.FillBackgroundColor = footerColor.GetIndex();
            footerStyle.Alignment = HorizontalAlignment.RIGHT;
            footerStyle.VerticalAlignment = VerticalAlignment.CENTER;
            footerStyle.BorderTop = BorderStyle.DOTTED;
            footerStyle.BorderLeft = BorderStyle.DOTTED;
            footerStyle.BorderRight = BorderStyle.DOTTED;
            footerStyle.BorderBottom = BorderStyle.DOTTED;

            ICellStyle footerStyle1 = workbook.CreateCellStyle();
            footerStyle1.FillForegroundColor = footerColor.GetIndex();
            footerStyle1.FillPattern = FillPatternType.SOLID_FOREGROUND;
            footerStyle1.FillBackgroundColor = footerColor.GetIndex();
            footerStyle1.VerticalAlignment = VerticalAlignment.CENTER;
            footerStyle1.BorderTop = BorderStyle.DOTTED;
            footerStyle1.BorderLeft = BorderStyle.DOTTED;
            footerStyle1.BorderRight = BorderStyle.DOTTED;
            footerStyle1.BorderBottom = BorderStyle.DOTTED;

            cellBody = row.CreateCell(0);
            cellBody.CellStyle = footerStyle1;
            cellBody.SetCellValue(Lang.total);

            cellBody = row.CreateCell(1);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(string.Empty);

            cellBody = row.CreateCell(2);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalTurnover));

            cellBody = row.CreateCell(3);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalActual));

            cellBody = row.CreateCell(4);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalOutstanding));

            cellBody = row.CreateCell(5);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalOutstandingShare));

            cellBody = row.CreateCell(6);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalGross));

            cellBody = row.CreateCell(7);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalAWinLoss));

            cellBody = row.CreateCell(8);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalAComm));

            cellBody = row.CreateCell(9);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalATotal));

            cellBody = row.CreateCell(10);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalUpline));

            ////end footer
            #endregion footer

            sheet.CreateFreezePane(0, 5);
            //// Auto-size each column
            for (var i = 0; i < sheet.GetRow(3).LastCellNum; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            #region title

            var rowTitle = sheet.CreateRow(1);
            var font = workbook.CreateFont();
            font.FontHeightInPoints = 14;
            font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;

            var cell = rowTitle.CreateCell(0);
            cell.SetCellValue(titleString);
            cell.CellStyle = workbook.CreateCellStyle();
            cell.CellStyle.SetFont(font);

            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 10));

            #endregion

            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 0, 0));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 1, 1));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 2, 2));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 3, 3));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 4, 4));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 5, 5));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 6, 6));
            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 7, 9));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 10, 10));
        }

        public void ExportsMemberWinLoss(HSSFWorkbook workbook, IList<MemberWinLossModel> memberWinLossList,
            string agentName, string sportName, string bettypeName, DateTime fromDate, DateTime toDate)
        {
            HSSFPalette h = new HSSFPalette(new NPOI.HSSF.Record.PaletteRecord());
            HSSFColor footerColor = h.FindSimilarColor(153, 0, 0);

            var titleString = string.Format("{0} ({1}: {2} -- {3}: {4} -- {5}: {6} -- {7}: {8})",
                Lang.lblMemberWinLoss, Lang.from, ShortDateFormat(fromDate), Lang.to, ShortDateFormat(toDate),
                Lang.sport, sportName, Lang.bettype, bettypeName);

            var sheet = workbook.CreateSheet(Lang.lblMemberWinLoss);

            #region header
            // header
            var rowIndex = 3;
            var row = sheet.CreateRow(rowIndex);
            var font1 = workbook.CreateFont();
            font1.FontHeightInPoints = 11;
            font1.Color = NPOI.HSSF.Util.HSSFColor.GREY_80_PERCENT.WHITE.index;
            font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;

            ICellStyle headerStyle = workbook.CreateCellStyle();
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

            var cell1 = row.CreateCell(0);
            cell1.CellStyle = headerStyle;
            cell1.SetCellValue(string.Empty);

            var cell2 = row.CreateCell(1);
            cell2.CellStyle = headerStyle;
            cell2.SetCellValue(Lang.lblSoccerPT);

            var cell3 = row.CreateCell(2);
            cell3.CellStyle = headerStyle;
            cell3.SetCellValue(Lang.BetAmount);

            var cell4 = row.CreateCell(3);
            cell4.CellStyle = headerStyle;
            cell4.SetCellValue(Lang.CommBase);

            var cell5 = row.CreateCell(4);
            cell5.CellStyle = headerStyle;
            cell5.SetCellValue(Lang.outstanding);

            var cell6 = row.CreateCell(5);
            cell6.CellStyle = headerStyle;
            cell6.SetCellValue(Lang.ShareAmount);

            var cell7 = row.CreateCell(6);
            cell7.CellStyle = headerStyle;
            cell7.SetCellValue(Lang.member);

            var cell8 = row.CreateCell(7);
            cell8.CellStyle = headerStyle;
            cell8.SetCellValue(string.Empty);

            var cell9 = row.CreateCell(8);
            cell9.CellStyle = headerStyle;
            cell9.SetCellValue(string.Empty);

            var cell10 = row.CreateCell(9);
            cell10.CellStyle = headerStyle;
            cell10.SetCellValue(agentName);

            var cell11 = row.CreateCell(10);
            cell11.CellStyle = headerStyle;
            cell11.SetCellValue(string.Empty);

            var cell12 = row.CreateCell(11);
            cell12.CellStyle = headerStyle;
            cell12.SetCellValue(string.Empty);

            var cell13 = row.CreateCell(12);
            cell13.CellStyle = headerStyle;
            cell13.SetCellValue(Lang.UplineTotal);

            rowIndex++;
            row = sheet.CreateRow(rowIndex);

            var cell14 = row.CreateCell(6);
            cell14.CellStyle = headerStyle;
            cell14.SetCellValue(Lang.lblWinLoss);

            var cell15 = row.CreateCell(7);
            cell15.CellStyle = headerStyle;
            cell15.SetCellValue(Lang.lblComm);

            var cell16 = row.CreateCell(8);
            cell16.CellStyle = headerStyle;
            cell16.SetCellValue(Lang.total);

            var cell17 = row.CreateCell(9);
            cell17.CellStyle = headerStyle;
            cell17.SetCellValue(Lang.lblWinLoss);

            var cell18 = row.CreateCell(10);
            cell18.CellStyle = headerStyle;
            cell18.SetCellValue(Lang.lblComm);

            var cell19 = row.CreateCell(11);
            cell19.CellStyle = headerStyle;
            cell19.SetCellValue(Lang.total);

            rowIndex++;
            //// end header
            #endregion

            #region body
            //// body

            ICellStyle bodyStyle = workbook.CreateCellStyle();
            bodyStyle.Alignment = HorizontalAlignment.RIGHT;
            bodyStyle.VerticalAlignment = VerticalAlignment.CENTER;
            bodyStyle.BorderTop = BorderStyle.DOTTED;
            bodyStyle.BorderLeft = BorderStyle.DOTTED;
            bodyStyle.BorderRight = BorderStyle.DOTTED;
            bodyStyle.BorderBottom = BorderStyle.DOTTED;

            ICellStyle bodyStyle1 = workbook.CreateCellStyle();
            bodyStyle1.BorderTop = BorderStyle.DOTTED;
            bodyStyle1.BorderLeft = BorderStyle.DOTTED;
            bodyStyle1.BorderRight = BorderStyle.DOTTED;
            bodyStyle1.BorderBottom = BorderStyle.DOTTED;

            ICell cellBody;
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

            foreach (var memberWinLoss in memberWinLossList)
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

                cellBody = row.CreateCell(0);
                cellBody.CellStyle = bodyStyle1;

                cellBody.SetCellValue(memberWinLoss.MemberName);

                cellBody = row.CreateCell(1);
                cellBody.CellStyle = bodyStyle;

                cellBody.SetCellValue(FormatNumber20(memberWinLoss.SoccerShare / 100.00));
               
                cellBody = row.CreateCell(2);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(memberWinLoss.Turnover));

                cellBody = row.CreateCell(3);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(memberWinLoss.ActualTO));

                cellBody = row.CreateCell(4);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(memberWinLoss.Outstanding));

                cellBody = row.CreateCell(5);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(memberWinLoss.OutstandingShare));

                cellBody = row.CreateCell(6);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(memberWinLoss.WinLoss));

                cellBody = row.CreateCell(7);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(memberWinLoss.MemComm));

                cellBody = row.CreateCell(8);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(memberWinLoss.MemTotal));

                cellBody = row.CreateCell(9);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(memberWinLoss.AWinLoss));

                cellBody = row.CreateCell(10);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(memberWinLoss.AComm));

                cellBody = row.CreateCell(11);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(memberWinLoss.ATotal));

                cellBody = row.CreateCell(12);
                cellBody.CellStyle = bodyStyle;
                cellBody.SetCellValue(FormatNumber20(memberWinLoss.UplineTotal));

                rowIndex++;
            }

            #endregion

            #region footer

            row = sheet.CreateRow(rowIndex);

            ICellStyle footerStyle = workbook.CreateCellStyle();
            footerStyle.FillForegroundColor = footerColor.GetIndex();
            footerStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            footerStyle.FillBackgroundColor = footerColor.GetIndex();
            footerStyle.Alignment = HorizontalAlignment.RIGHT;
            footerStyle.VerticalAlignment = VerticalAlignment.CENTER;
            footerStyle.BorderTop = BorderStyle.DOTTED;
            footerStyle.BorderLeft = BorderStyle.DOTTED;
            footerStyle.BorderRight = BorderStyle.DOTTED;
            footerStyle.BorderBottom = BorderStyle.DOTTED;

            ICellStyle footerStyle1 = workbook.CreateCellStyle();
            footerStyle1.FillForegroundColor = footerColor.GetIndex();
            footerStyle1.FillPattern = FillPatternType.SOLID_FOREGROUND;
            footerStyle1.FillBackgroundColor = footerColor.GetIndex();
            footerStyle1.VerticalAlignment = VerticalAlignment.CENTER;
            footerStyle1.BorderTop = BorderStyle.DOTTED;
            footerStyle1.BorderLeft = BorderStyle.DOTTED;
            footerStyle1.BorderRight = BorderStyle.DOTTED;
            footerStyle1.BorderBottom = BorderStyle.DOTTED;

            cellBody = row.CreateCell(0);
            cellBody.CellStyle = footerStyle1;
            cellBody.SetCellValue(Lang.total);

            cellBody = row.CreateCell(1);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(string.Empty);

            cellBody = row.CreateCell(2);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalTurnover));

            cellBody = row.CreateCell(3);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalActual));

            cellBody = row.CreateCell(4);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalOutstanding));

            cellBody = row.CreateCell(5);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalOutstandingShare));

            cellBody = row.CreateCell(6);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalMWinLoss));

            cellBody = row.CreateCell(7);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalMComm));

            cellBody = row.CreateCell(8);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalMTotal));

            cellBody = row.CreateCell(9);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalAWinLoss));

            cellBody = row.CreateCell(10);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalAComm));

            cellBody = row.CreateCell(11);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalATotal));

            cellBody = row.CreateCell(12);
            cellBody.CellStyle = footerStyle;
            cellBody.SetCellValue(FormatNumber20(totalUpline));
            ////end footer
            #endregion footer

            sheet.CreateFreezePane(0, 5);
            //// Auto-size each column
            for (var i = 0; i < sheet.GetRow(3).LastCellNum; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            #region title

            var rowTitle = sheet.CreateRow(1);
            var font = workbook.CreateFont();
            font.FontHeightInPoints = 14;
            font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;

            var cell = rowTitle.CreateCell(0);
            cell.SetCellValue(titleString);
            cell.CellStyle = workbook.CreateCellStyle();
            cell.CellStyle.SetFont(font);

            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 12));

            #endregion

            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 0, 0));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 1, 1));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 2, 2));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 3, 3));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 4, 4));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 5, 5));
            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 6, 8));
            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 9, 11));
            sheet.AddMergedRegion(new CellRangeAddress(3, 4, 12, 12));
        }

        public string FormatNumber20(object obj)
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
    }
}
