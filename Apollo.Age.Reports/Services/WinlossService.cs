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

    /// <summary>
    /// WinLoss Service.
    /// </summary>
    public class WinLossService : ReportService, IWinLossService
    {
        /// <summary>
        /// Get List Match Entered from DB.
        /// </summary>
        /// <returns>List Match Entered.</returns>
        public List<MatchEnteredModel> GetMatchEntered(bool isShowDataTest)
        {
            var criterias = new MatchEnteredCriteria() {
                isShowAll = isShowDataTest
            };
            var matchEnteredList = this.GetData<MatchEntered>(criterias).ToList();

            return this.ConvertToMatchEnteredModel(matchEnteredList);
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
                    return this.AgentWinLossViewBySuper(model.AgentWinLossList, title, model.AgentName, filter);
                }
                else
                {
                    return this.AgentWinLossViewByNoneSuper(model.AgentWinLossList, title, model.AgentName, filter);
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
                this.CustomizeColor(hssfworkbook);

                var fontHeaderWhite = this.CreateFont(hssfworkbook, HSSFColor.WHITE.index, 10);
                var fontHeaderBlack = this.CreateFont(hssfworkbook, HSSFColor.BLACK.index, 10);

                var headerStyleShare = this.CreateStyle(hssfworkbook, HSSFColor.GREY_50_PERCENT.index, HorizontalAlignment.CENTER, fontHeaderBlack);
                var headerStyleMember = this.CreateStyle(hssfworkbook, HSSFColor.GREEN.index, HorizontalAlignment.CENTER, fontHeaderWhite);
                var headerStyleAgent = this.CreateStyle(hssfworkbook, HSSFColor.GREY_50_PERCENT.index, HorizontalAlignment.CENTER, fontHeaderBlack);
                var headerStyleUpline = this.CreateStyle(hssfworkbook, HSSFColor.GREEN.index, HorizontalAlignment.CENTER, fontHeaderWhite);
                var headerStyleOutstanding = this.CreateStyle(hssfworkbook, HSSFColor.GREY_50_PERCENT.index, HorizontalAlignment.CENTER, fontHeaderBlack);

                var bodyStyleRight = CreateStyle(hssfworkbook, align: HorizontalAlignment.RIGHT);
                var bodyStyleLeft = CreateStyle(hssfworkbook);

                var fontFooter = CreateFont(hssfworkbook, HSSFColor.WHITE.index, 10);
                var footerStyleRight = CreateStyle(hssfworkbook, HSSFColor.DARK_RED.index, HorizontalAlignment.RIGHT, fontFooter);
                var footerStyleLeft = CreateStyle(hssfworkbook, HSSFColor.DARK_RED.index, HorizontalAlignment.LEFT, fontFooter);

                IDataFormat format = hssfworkbook.CreateDataFormat();
                var numberFormat = format.GetFormat("#,##0.00");

                #endregion

                #region header
                int rowIndex, startIndex;
                rowIndex = startIndex = 4;
                var columIndex = 0;
                var row = sheet.CreateRow(rowIndex);

                this.CreateStringCell(row, columIndex++, headerStyleShare, string.Empty);
                this.CreateStringCell(row, columIndex++, headerStyleShare, Lang.lblSoccerPT);
                this.CreateStringCell(row, columIndex++, headerStyleMember, Lang.member);
                this.CreateStringCell(row, columIndex++, headerStyleMember, string.Empty);
                this.CreateStringCell(row, columIndex++, headerStyleMember, string.Empty);
                this.CreateStringCell(row, columIndex++, headerStyleMember, string.Empty);
                this.CreateStringCell(row, columIndex++, headerStyleMember, string.Empty);
                this.CreateStringCell(row, columIndex++, headerStyleAgent, model.AgentName);
                this.CreateStringCell(row, columIndex++, headerStyleAgent, string.Empty);
                this.CreateStringCell(row, columIndex++, headerStyleAgent, string.Empty);
                this.CreateStringCell(row, columIndex++, headerStyleUpline, Lang.lblUpline);
                this.CreateStringCell(row, columIndex++, headerStyleUpline, string.Empty);
                this.CreateStringCell(row, columIndex++, headerStyleOutstanding, Lang.lblOutstandingAmount);
                this.CreateStringCell(row, columIndex++, headerStyleOutstanding, Lang.lblOutstandingShareAmount);

                rowIndex++;
                columIndex = 0;
                row = sheet.CreateRow(rowIndex);

                this.CreateStringCell(row, columIndex++, headerStyleShare, string.Empty);
                this.CreateStringCell(row, columIndex++, headerStyleShare, string.Empty);
                this.CreateStringCell(row, columIndex++, headerStyleMember, Lang.BetAmount + " / " + Lang.lblBetCount);
                this.CreateStringCell(row, columIndex++, headerStyleMember, Lang.CommBase);
                this.CreateStringCell(row, columIndex++, headerStyleMember, Lang.lblWinLossReport);
                this.CreateStringCell(row, columIndex++, headerStyleMember, Lang.lblComm);
                this.CreateStringCell(row, columIndex++, headerStyleMember, Lang.total);
                this.CreateStringCell(row, columIndex++, headerStyleAgent, Lang.lblWinLossReport);
                this.CreateStringCell(row, columIndex++, headerStyleAgent, Lang.lblComm);
                this.CreateStringCell(row, columIndex++, headerStyleAgent, Lang.total);
                this.CreateStringCell(row, columIndex++, headerStyleUpline, Lang.lblEffectiveStake2);
                this.CreateStringCell(row, columIndex++, headerStyleUpline, Lang.total);
                this.CreateStringCell(row, columIndex++, headerStyleOutstanding, string.Empty);
                this.CreateStringCell(row, columIndex++, headerStyleOutstanding, string.Empty);

                rowIndex++;
                #endregion

                #region body
                //// Add data rows
                decimal totalBetAmount = 0;
                decimal totalBetCount = 0;
                decimal totalCommBase = 0;
                decimal totalMemWinLoss = 0;
                decimal totalMemComm = 0;
                decimal totalMemTotal = 0;
                decimal totalAWinLoss = 0;
                decimal totalAComm = 0;
                decimal totalATotal = 0;
                decimal totalUpLineEffectiveStake = 0;
                decimal totalUpLineWinLoss = 0;
                decimal totalOutstandingAmount = 0;
                decimal totalOutstandingShareAmount = 0;

                foreach (var memberWinLoss in model.MemberWinLossList)
                {
                    columIndex = 0;
                    totalBetAmount += memberWinLoss.BetAmount;
                    totalBetCount += memberWinLoss.BetCount;
                    totalCommBase += memberWinLoss.CommBase;
                    totalMemWinLoss += memberWinLoss.MemberWinLoss;
                    totalMemComm += memberWinLoss.MemberComm;
                    totalMemTotal += memberWinLoss.MemberTotal;
                    totalAWinLoss += memberWinLoss.AWinLoss;
                    totalAComm += memberWinLoss.AComm;
                    totalATotal += memberWinLoss.ATotal;
                    totalUpLineEffectiveStake += memberWinLoss.UpLineEffectiveStake;
                    totalUpLineWinLoss += memberWinLoss.UpLineTotal;
                    totalOutstandingAmount += memberWinLoss.OutstandingAmount;
                    totalOutstandingShareAmount += memberWinLoss.OutstandingShareAmount;

                    row = sheet.CreateRow(rowIndex);

                    this.CreateStringCell(row, columIndex++, bodyStyleLeft, memberWinLoss.MemberName);
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)(memberWinLoss.SoccerShare / 100.00));
                    this.CreateStringCell(row, columIndex++, bodyStyleRight, this.FormatNumber20(memberWinLoss.BetAmount) + " / " + this.FormatNumber(memberWinLoss.BetCount));
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)memberWinLoss.CommBase);
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)memberWinLoss.MemberWinLoss);
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)memberWinLoss.MemberComm);
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)memberWinLoss.MemberTotal);
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)memberWinLoss.AWinLoss);
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)memberWinLoss.AComm);
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)memberWinLoss.ATotal);
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)memberWinLoss.UpLineEffectiveStake);
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)memberWinLoss.UpLineTotal);
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)memberWinLoss.OutstandingAmount);
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)memberWinLoss.OutstandingShareAmount);

                    rowIndex++;
                }

                #endregion

                #region footer
                row = sheet.CreateRow(rowIndex);
                columIndex = 0;

                this.CreateStringCell(row, columIndex++, footerStyleLeft, Lang.total);
                this.CreateStringCell(row, columIndex++, footerStyleRight, string.Empty);
                this.CreateStringCell(row, columIndex++, footerStyleRight, this.FormatNumber20(totalBetAmount) + " / " + this.FormatNumber(totalBetCount));
                this.CreateNumberCell(row, columIndex++, footerStyleRight, numberFormat, (double)totalCommBase);
                this.CreateNumberCell(row, columIndex++, footerStyleRight, numberFormat, (double)totalMemWinLoss);
                this.CreateNumberCell(row, columIndex++, footerStyleRight, numberFormat, (double)totalMemComm);
                this.CreateNumberCell(row, columIndex++, footerStyleRight, numberFormat, (double)totalMemTotal);
                this.CreateNumberCell(row, columIndex++, footerStyleRight, numberFormat, (double)totalAWinLoss);
                this.CreateNumberCell(row, columIndex++, footerStyleRight, numberFormat, (double)totalAComm);
                this.CreateNumberCell(row, columIndex++, footerStyleRight, numberFormat, (double)totalATotal);
                this.CreateNumberCell(row, columIndex++, footerStyleRight, numberFormat, (double)totalUpLineEffectiveStake);
                this.CreateNumberCell(row, columIndex++, footerStyleRight, numberFormat, (double)totalUpLineWinLoss);
                this.CreateNumberCell(row, columIndex++, footerStyleRight, numberFormat, (double)totalOutstandingAmount);
                this.CreateNumberCell(row, columIndex++, footerStyleRight, numberFormat, (double)totalOutstandingShareAmount);
                #endregion footer

                #region auto size & freePane
                sheet.CreateFreezePane(0, startIndex + 2); //// 2: Freeze Pane 2 row after start index
                //// Auto-size each column
                for (var i = 0; i < sheet.GetRow(startIndex).LastCellNum; i++)
                {
                    sheet.AutoSizeColumn(i);
                    var currentWidth = sheet.GetColumnWidth(i);
                    sheet.SetColumnWidth(i, currentWidth + 500); //// 500: is padding of cell
                }

                #endregion

                #region title
                // row title report
                var rowTitle = sheet.CreateRow(rownum: 1);
                var fontTitle = CreateFont(hssfworkbook, HSSFColor.BLACK.index, 14, true);
                var styleTitle = CreateStyle(hssfworkbook, align: HorizontalAlignment.CENTER, font: fontTitle, isBorder: false);
                this.CreateStringCell(rowTitle, 0, styleTitle, title);

                var rowFilter = sheet.CreateRow(rownum: 2);
                var fontFilter = CreateFont(hssfworkbook, HSSFColor.BLUE.index, 11, true, true);
                var styleFilter = CreateStyle(hssfworkbook, font: fontFilter, isBorder: false);
                this.CreateStringCell(rowFilter, 0, styleFilter, filter);

                #endregion

                #region merge cell
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 13));
                sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 13));
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 0, 0));
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 1, 1));
                sheet.AddMergedRegion(new CellRangeAddress(4, 4, 2, 6));
                sheet.AddMergedRegion(new CellRangeAddress(4, 4, 7, 9));
                sheet.AddMergedRegion(new CellRangeAddress(4, 4, 10, 11));
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 12, 12));
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 13, 13));
                #endregion
            }

            return hssfworkbook;
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
            this.CustomizeColor(hssfworkbook);

            var fontHeaderWhite = CreateFont(hssfworkbook, HSSFColor.WHITE.index, 10);
            var fontHeaderBlack = CreateFont(hssfworkbook, HSSFColor.BLACK.index, 10);

            var headerStyleShare = CreateStyle(hssfworkbook, HSSFColor.GREEN.index, HorizontalAlignment.CENTER, fontHeaderWhite);
            var headerStyleMember = CreateStyle(hssfworkbook, HSSFColor.GREY_25_PERCENT.index, HorizontalAlignment.CENTER, fontHeaderBlack);
            var headerStyleDownline = CreateStyle(hssfworkbook, HSSFColor.GREEN.index, HorizontalAlignment.CENTER, fontHeaderWhite);
            var headerStyleAgent = CreateStyle(hssfworkbook, HSSFColor.GREY_50_PERCENT.index, HorizontalAlignment.CENTER, fontHeaderBlack);
            
            var bodyStyleRight = CreateStyle(hssfworkbook, align: HorizontalAlignment.RIGHT);
            var bodyStyleLeft = CreateStyle(hssfworkbook);

            IDataFormat format = hssfworkbook.CreateDataFormat();
            var numberFormat = format.GetFormat("#,##0.00");

            #endregion

            #region header
            int rowIndex, startIndex;
            rowIndex = startIndex = 4;
            var columIndex = 0;
            var row = sheet.CreateRow(rowIndex);

            this.CreateStringCell(row, columIndex++, headerStyleShare, string.Empty);
            this.CreateStringCell(row, columIndex++, headerStyleShare, Lang.lblSoccerPT);
            this.CreateStringCell(row, columIndex++, headerStyleMember, Lang.member);
            this.CreateStringCell(row, columIndex++, headerStyleMember, string.Empty);
            this.CreateStringCell(row, columIndex++, headerStyleMember, string.Empty);
            this.CreateStringCell(row, columIndex++, headerStyleDownline, Lang.lblDownline);
            this.CreateStringCell(row, columIndex++, headerStyleDownline, string.Empty);
            this.CreateStringCell(row, columIndex++, headerStyleAgent, agentName);
            this.CreateStringCell(row, columIndex++, headerStyleAgent, string.Empty);
            this.CreateStringCell(row, columIndex++, headerStyleAgent, string.Empty);

            rowIndex++;
            columIndex = 0;
            row = sheet.CreateRow(rowIndex);

            this.CreateStringCell(row, columIndex++, headerStyleShare, string.Empty);
            this.CreateStringCell(row, columIndex++, headerStyleShare, string.Empty);
            this.CreateStringCell(row, columIndex++, headerStyleMember, Lang.BetAmount + " / " + Lang.lblBetCount);
            this.CreateStringCell(row, columIndex++, headerStyleMember, Lang.CommBase);
            this.CreateStringCell(row, columIndex++, headerStyleMember, Lang.total);
            this.CreateStringCell(row, columIndex++, headerStyleDownline, Lang.lblEffectiveStake);
            this.CreateStringCell(row, columIndex++, headerStyleDownline, Lang.total);
            this.CreateStringCell(row, columIndex++, headerStyleAgent, Lang.lblWinLossReport);
            this.CreateStringCell(row, columIndex++, headerStyleAgent, Lang.lblComm);
            this.CreateStringCell(row, columIndex++, headerStyleAgent, Lang.total);

            rowIndex++;
            //// end header
            #endregion

            #region body

            //// Add data rows
            foreach (var agentWinLoss in agentWinLossList)
            {
                columIndex = 0;
                row = sheet.CreateRow(rowIndex);

                this.CreateStringCell(row, columIndex++, bodyStyleLeft, agentWinLoss.AgentName);
                this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)(agentWinLoss.SoccerShare / 100.00));
                this.CreateStringCell(row, columIndex++, bodyStyleRight, this.FormatNumber20(agentWinLoss.BetAmount) + " / " + this.FormatNumber(agentWinLoss.BetCount));
                this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.CommBase);
                this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.MemberTotal);
                this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.DownLineEffectiveStake);
                this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.DownLineTotal);
                this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.AWinLoss);
                this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.AComm);
                this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.ATotal);

                rowIndex++;
            }

            #endregion

            #region auto size & freePane

            sheet.CreateFreezePane(0, startIndex + 2); //// 2: Freeze Pane 2 row after start index
            //// Auto-size each column
            for (var i = 0; i < sheet.GetRow(startIndex).LastCellNum; i++)
            {
                sheet.AutoSizeColumn(i);
                var currentWidth = sheet.GetColumnWidth(i);
                sheet.SetColumnWidth(i, currentWidth + 500); //// 500: is padding of cell
            }

            #endregion

            #region title

            var rowTitle = sheet.CreateRow(rownum: 1);
            var fontTitle = CreateFont(hssfworkbook, HSSFColor.BLACK.index, 14, isBold: true);
            var styleTitle = CreateStyle(hssfworkbook, align: HorizontalAlignment.CENTER, font: fontTitle, isBorder: false);
            this.CreateStringCell(rowTitle, 0, styleTitle, title);

            var rowFilter = sheet.CreateRow(rownum: 2);
            var fontFilter = CreateFont(hssfworkbook, HSSFColor.BLUE.index, 11, isBold: true, isItalic: true);
            var filterStyle = CreateStyle(hssfworkbook, font: fontFilter, isBorder: false);
            this.CreateStringCell(rowFilter, 0, filterStyle, filter);
            #endregion

            #region merge cell
            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 9));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 9));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 0, 0));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 1, 1));
            sheet.AddMergedRegion(new CellRangeAddress(4, 4, 2, 4));
            sheet.AddMergedRegion(new CellRangeAddress(4, 4, 5, 6));
            sheet.AddMergedRegion(new CellRangeAddress(4, 4, 7, 9));
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
            this.CustomizeColor(hssfworkbook);
            var fontHeaderWhite = CreateFont(hssfworkbook, HSSFColor.WHITE.index, 10);
            var fontHeaderBlack = CreateFont(hssfworkbook, HSSFColor.BLACK.index, 10);

            var headerStyleShare = CreateStyle(hssfworkbook, HSSFColor.GREEN.index, HorizontalAlignment.CENTER, fontHeaderWhite);
            var headerStyleMember = CreateStyle(hssfworkbook, HSSFColor.GREY_25_PERCENT.index, HorizontalAlignment.CENTER, fontHeaderBlack);
            var headerStyleDownline = CreateStyle(hssfworkbook, HSSFColor.GREEN.index, HorizontalAlignment.CENTER, fontHeaderWhite);
            var headerStyleAgent = CreateStyle(hssfworkbook, HSSFColor.GREY_50_PERCENT.index, HorizontalAlignment.CENTER, fontHeaderBlack);
            var headerStyleUpline = CreateStyle(hssfworkbook, HSSFColor.GREEN.index, HorizontalAlignment.CENTER, fontHeaderWhite);

            var bodyStyleRight = CreateStyle(hssfworkbook, align: HorizontalAlignment.RIGHT);
            var bodyStyleLeft = CreateStyle(hssfworkbook);

            var fontFooter = CreateFont(hssfworkbook, HSSFColor.WHITE.index, 10);
            var styleFooterRight = CreateStyle(hssfworkbook, HSSFColor.DARK_RED.index, HorizontalAlignment.RIGHT, fontFooter);
            var styleFooterLeft = CreateStyle(hssfworkbook, HSSFColor.DARK_RED.index, HorizontalAlignment.LEFT, fontFooter);

            IDataFormat format = hssfworkbook.CreateDataFormat();
            var numberFormat = format.GetFormat("#,##0.00");

            #endregion

            #region header
            int rowIndex, startIndex;
            rowIndex = startIndex = 4;
            var columIndex = 0;
            var row = sheet.CreateRow(rowIndex);

            this.CreateStringCell(row, columIndex++, headerStyleShare, string.Empty);
            this.CreateStringCell(row, columIndex++, headerStyleShare, Lang.lblSoccerPT);
            this.CreateStringCell(row, columIndex++, headerStyleMember, Lang.member);
            this.CreateStringCell(row, columIndex++, headerStyleMember, string.Empty);
            this.CreateStringCell(row, columIndex++, headerStyleMember, string.Empty);
            this.CreateStringCell(row, columIndex++, headerStyleDownline, Lang.lblDownline);
            this.CreateStringCell(row, columIndex++, headerStyleDownline, string.Empty);
            this.CreateStringCell(row, columIndex++, headerStyleAgent, agentName);
            this.CreateStringCell(row, columIndex++, headerStyleAgent, string.Empty);
            this.CreateStringCell(row, columIndex++, headerStyleAgent, string.Empty);
            this.CreateStringCell(row, columIndex++, headerStyleUpline, Lang.lblUpline);
            this.CreateStringCell(row, columIndex++, headerStyleUpline, string.Empty);

            rowIndex++;
            columIndex = 0;
            row = sheet.CreateRow(rowIndex);

            this.CreateStringCell(row, columIndex++, headerStyleShare, string.Empty);
            this.CreateStringCell(row, columIndex++, headerStyleShare, string.Empty);
            this.CreateStringCell(row, columIndex++, headerStyleMember, Lang.BetAmount + " / " + Lang.lblBetCount);
            this.CreateStringCell(row, columIndex++, headerStyleMember, Lang.CommBase);
            this.CreateStringCell(row, columIndex++, headerStyleMember, Lang.total);
            this.CreateStringCell(row, columIndex++, headerStyleDownline, Lang.lblEffectiveStake);
            this.CreateStringCell(row, columIndex++, headerStyleDownline, Lang.total);
            this.CreateStringCell(row, columIndex++, headerStyleAgent, Lang.lblWinLossReport);
            this.CreateStringCell(row, columIndex++, headerStyleAgent, Lang.lblComm);
            this.CreateStringCell(row, columIndex++, headerStyleAgent, Lang.total);
            this.CreateStringCell(row, columIndex++, headerStyleUpline, Lang.lblEffectiveStake2);
            this.CreateStringCell(row, columIndex++, headerStyleUpline, Lang.total);

            rowIndex++;
            #endregion

            #region body
            //// Add data rows

            decimal totalBetAmount = 0;
            decimal totalBetCount = 0;
            decimal totalCommBase = 0;
            decimal totalMemTotal = 0;
            decimal totalDownLineEffectiveStake = 0;
            decimal totalDownLineTotal = 0;
            decimal totalAWinLoss = 0;
            decimal totalAComm = 0;
            decimal totalATotal = 0;
            decimal totalUpLineEffectiveStake = 0;
            decimal totalUpLineTotal = 0;

            foreach (var agentWinLoss in agentWinLossList)
            {
                columIndex = 0;
                totalBetAmount += agentWinLoss.BetAmount;
                totalBetCount += agentWinLoss.BetCount;
                totalCommBase += agentWinLoss.CommBase;
                totalMemTotal += agentWinLoss.MemberTotal;
                totalDownLineEffectiveStake += agentWinLoss.DownLineEffectiveStake;
                totalDownLineTotal += agentWinLoss.DownLineTotal;
                totalAWinLoss += agentWinLoss.AWinLoss;
                totalAComm += agentWinLoss.AComm;
                totalATotal += agentWinLoss.ATotal;
                totalUpLineEffectiveStake += agentWinLoss.UpLineEffectiveStake;
                totalUpLineTotal += agentWinLoss.UpLineTotal;

                row = sheet.CreateRow(rowIndex);

                if (agentWinLoss.IsAgent)
                {
                    this.CreateStringCell(row, columIndex++, bodyStyleLeft, agentWinLoss.AgentName);
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)(agentWinLoss.SoccerShare / 100.00));
                    this.CreateStringCell(row, columIndex++, bodyStyleRight, this.FormatNumber20(agentWinLoss.BetAmount) + " / " + this.FormatNumber(agentWinLoss.BetCount));
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.CommBase);
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.MemberTotal);
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.DownLineEffectiveStake);
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.DownLineTotal);
                }
                else
                {
                    this.CreateStringCell(row, columIndex++, bodyStyleLeft, Lang.lblMemberWinLoss);
                    this.CreateStringCell(row, columIndex++, bodyStyleRight, Lang.lblNA);
                    this.CreateStringCell(row, columIndex++, bodyStyleRight, this.FormatNumber20(agentWinLoss.BetAmount) + " / " + this.FormatNumber(agentWinLoss.BetCount));
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.CommBase);
                    this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.MemberTotal);
                    this.CreateStringCell(row, columIndex++, bodyStyleRight, Lang.lblNA);
                    this.CreateStringCell(row, columIndex++, bodyStyleRight, Lang.lblNA);
                }

                this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.AWinLoss);
                this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.AComm);
                this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.ATotal);
                this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.UpLineEffectiveStake);
                this.CreateNumberCell(row, columIndex++, bodyStyleRight, numberFormat, (double)agentWinLoss.UpLineTotal);

                rowIndex++;
            }

            #endregion

            #region footer
            columIndex = 0;
            row = sheet.CreateRow(rowIndex);

            this.CreateStringCell(row, columIndex++, styleFooterLeft, Lang.total);
            this.CreateStringCell(row, columIndex++, styleFooterRight, string.Empty);
            this.CreateStringCell(row, columIndex++, styleFooterRight, this.FormatNumber20(totalBetAmount) + " / " + this.FormatNumber(totalBetCount));
            this.CreateNumberCell(row, columIndex++, styleFooterRight, numberFormat, (double)totalCommBase);
            this.CreateNumberCell(row, columIndex++, styleFooterRight, numberFormat, (double)totalMemTotal);
            this.CreateNumberCell(row, columIndex++, styleFooterRight, numberFormat, (double)totalDownLineEffectiveStake);
            this.CreateNumberCell(row, columIndex++, styleFooterRight, numberFormat, (double)totalDownLineTotal);
            this.CreateNumberCell(row, columIndex++, styleFooterRight, numberFormat, (double)totalAWinLoss);
            this.CreateNumberCell(row, columIndex++, styleFooterRight, numberFormat, (double)totalAComm);
            this.CreateNumberCell(row, columIndex++, styleFooterRight, numberFormat, (double)totalATotal);
            this.CreateNumberCell(row, columIndex++, styleFooterRight, numberFormat, (double)totalUpLineEffectiveStake);
            this.CreateNumberCell(row, columIndex++, styleFooterRight, numberFormat, (double)totalUpLineTotal);

            #endregion footer

            #region auto size & freePane
            sheet.CreateFreezePane(0, startIndex + 2); //// 2: Freeze Pane 2 row after start index
            //// Auto-size each column
            for (var i = 0; i < sheet.GetRow(startIndex).LastCellNum; i++)
            {
                sheet.AutoSizeColumn(i);
                var currentWidth = sheet.GetColumnWidth(i);
                sheet.SetColumnWidth(i, currentWidth + 500); //// 500: is padding of cell
            }

            #endregion

            #region title
            var rowTitle = sheet.CreateRow(1);
            var fontTitle = CreateFont(hssfworkbook, HSSFColor.BLACK.index, 14, true);
            var styleTitle = CreateStyle(hssfworkbook, align: HorizontalAlignment.CENTER, font: fontTitle, isBorder: false);
            this.CreateStringCell(rowTitle, 0, styleTitle, title);

            var rowFilter = sheet.CreateRow(2);
            var fontFilter = CreateFont(hssfworkbook, HSSFColor.BLUE.index, 11, true, true);
            var styleFilter = CreateStyle(hssfworkbook, font: fontFilter, isBorder: false);
            this.CreateStringCell(rowFilter, 0, styleFilter, filter);

            #endregion

            #region merge cell

            sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 11));
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 11));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 0, 0));
            sheet.AddMergedRegion(new CellRangeAddress(4, 5, 1, 1));
            sheet.AddMergedRegion(new CellRangeAddress(4, 4, 2, 4));
            sheet.AddMergedRegion(new CellRangeAddress(4, 4, 5, 6));
            sheet.AddMergedRegion(new CellRangeAddress(4, 4, 7, 9));
            sheet.AddMergedRegion(new CellRangeAddress(4, 4, 10, 11));

            #endregion

            return hssfworkbook;
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
        /// Get number format with 0 digit.
        /// </summary>
        /// <param name="obj">Input object.</param>
        /// <returns>A string format value.</returns>
        private string FormatNumber(object obj)
        {
            if (Convert.IsDBNull(obj) || obj == null)
            {
                return "0";
            }

            return string.Format("{0:#,##0}", obj);
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
