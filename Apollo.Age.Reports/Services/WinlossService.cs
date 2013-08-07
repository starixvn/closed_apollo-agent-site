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
        private HSSFWorkbook _hssfworkbook = null;

        public WinLossService()
        {
            _hssfworkbook = new HSSFWorkbook();
        }

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
                    return AgentWinLossViewBySuper(model.AgentWinLossList, title, model.AgentName, filter);
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

                var sheet = _hssfworkbook.CreateSheet(Lang.lblMemberWinLoss);
                
                #region declare style of cell
                CustomizeColor();

                var font = CreateFont(HSSFColor.WHITE.index, 10);
                var fontHeader = CreateFont(HSSFColor.BLACK.index, 10);

                var headerStyle = CreateStyle(HSSFColor.BROWN.index, HorizontalAlignment.CENTER, font);
                var headerStyle1 = CreateStyle(HSSFColor.BLUE_GREY.index, HorizontalAlignment.CENTER, fontHeader);
                //var headerStyle2 = CreateStyle(HSSFColor.LIME.index, HorizontalAlignment.CENTER, font);
                //var headerStyle3 = CreateStyle(HSSFColor.BROWN.index, HorizontalAlignment.CENTER, font);
                //var headerStyle4 = CreateStyle(HSSFColor.PINK.index, HorizontalAlignment.CENTER, font);
                //var headerStyle5 = CreateStyle(HSSFColor.LIGHT_GREEN.index, HorizontalAlignment.CENTER, font);
                //var headerStyle6 = CreateStyle(HSSFColor.YELLOW.index, HorizontalAlignment.CENTER, font);

                var bodyStyle = CreateStyle(align: HorizontalAlignment.RIGHT);
                var bodyStyle1 = CreateStyle();

                var font2 = CreateFont(HSSFColor.WHITE.index, 10);
                var footerStyle = CreateStyle(HSSFColor.RED.index, HorizontalAlignment.RIGHT, font2);
                var footerStyle1 = CreateStyle(HSSFColor.RED.index, HorizontalAlignment.LEFT, font2);

                IDataFormat format = _hssfworkbook.CreateDataFormat();
                var numberFormat = format.GetFormat("#,##0.00");

                #endregion

                #region header
                var rowIndex = 4;
                var columIndex = 0;
                var row = sheet.CreateRow(rowIndex);

                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, Lang.lblSoccerPT);
                CreateStringCell(row, columIndex++, headerStyle, Lang.member);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle1, model.AgentName);
                CreateStringCell(row, columIndex++, headerStyle1, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle1, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, Lang.lblUpline);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, Lang.lblOutstandingAmount);
                CreateStringCell(row, columIndex++, headerStyle, Lang.lblOutstandingShareAmount);

                rowIndex++;
                columIndex = 0;
                row = sheet.CreateRow(rowIndex);

                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, Lang.BetAmount + " / " + Lang.lblBetCount);
                CreateStringCell(row, columIndex++, headerStyle, Lang.CommBase);
                CreateStringCell(row, columIndex++, headerStyle, Lang.lblWinLossReport);
                CreateStringCell(row, columIndex++, headerStyle, Lang.lblComm);
                CreateStringCell(row, columIndex++, headerStyle, Lang.total);
                CreateStringCell(row, columIndex++, headerStyle1, Lang.lblWinLossReport);
                CreateStringCell(row, columIndex++, headerStyle1, Lang.lblComm);
                CreateStringCell(row, columIndex++, headerStyle1, Lang.total);
                CreateStringCell(row, columIndex++, headerStyle, Lang.lblEffectiveStake);
                CreateStringCell(row, columIndex++, headerStyle, Lang.total);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);
                CreateStringCell(row, columIndex++, headerStyle, string.Empty);

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

                    CreateStringCell(row, columIndex++, bodyStyle1, memberWinLoss.MemberName);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)(memberWinLoss.SoccerShare / 100.00));
                    CreateStringCell(row, columIndex++, bodyStyle, FormatNumber20(memberWinLoss.BetAmount) + " / " + FormatNumber(memberWinLoss.BetCount));
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.CommBase);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.MemberWinLoss);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.MemberComm);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.MemberTotal);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.AWinLoss);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.AComm);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.ATotal);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.UpLineEffectiveStake);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.UpLineTotal);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.OutstandingAmount);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)memberWinLoss.OutstandingShareAmount);

                    rowIndex++;
                }

                #endregion

                #region footer
                row = sheet.CreateRow(rowIndex);
                columIndex = 0;

                CreateStringCell(row, columIndex++, footerStyle1, Lang.total);
                CreateStringCell(row, columIndex++, footerStyle, string.Empty);
                CreateStringCell(row, columIndex++, footerStyle, FormatNumber20(totalBetAmount) + " / " + FormatNumber(totalBetCount));
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalCommBase);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalMemWinLoss);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalMemComm);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalMemTotal);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalAWinLoss);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalAComm);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalATotal);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalUpLineEffectiveStake);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalUpLineWinLoss);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalOutstandingAmount);
                CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalOutstandingShareAmount);
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
                var font1 = CreateFont(HSSFColor.BLACK.index, 14, true);
                var titleStyle = CreateStyle(align: HorizontalAlignment.CENTER, font: font1, isBorder: false);
                CreateStringCell(rowTitle, 0, titleStyle, title);

                var rowFilter = sheet.CreateRow(2);
                var fontFilter = CreateFont(HSSFColor.BLUE.index, 11, true, true);
                var filterStyle = CreateStyle(font: fontFilter, isBorder: false);
                CreateStringCell(rowFilter, 0, filterStyle, filter);

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

            return _hssfworkbook;
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
            var sheet = _hssfworkbook.CreateSheet(Lang.agentwinloss);

            #region declare style of cell
            CustomizeColor();

            var font = CreateFont(HSSFColor.WHITE.index, 10);
            var fontHeader = CreateFont(HSSFColor.BLACK.index, 10);

            var headerStyle = CreateStyle(HSSFColor.BROWN.index, HorizontalAlignment.CENTER, font);
            var headerStyle1 = CreateStyle(HSSFColor.BLUE_GREY.index, HorizontalAlignment.CENTER, fontHeader);
            //var headerStyle2 = CreateStyle(HSSFColor.LIME.index, HorizontalAlignment.CENTER, font);
            //var headerStyle3 = CreateStyle(HSSFColor.BROWN.index, HorizontalAlignment.CENTER, font);
            //var headerStyle4 = CreateStyle(HSSFColor.PINK.index, HorizontalAlignment.CENTER, font);

            var bodyStyle = CreateStyle(align: HorizontalAlignment.RIGHT);
            var bodyStyle1 = CreateStyle();

            IDataFormat format = _hssfworkbook.CreateDataFormat();
            var numberFormat = format.GetFormat("#,##0.00");

            #endregion

            #region header
            var rowIndex = 4;
            var columIndex = 0;
            var row = sheet.CreateRow(rowIndex);

            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, Lang.lblSoccerPT);
            CreateStringCell(row, columIndex++, headerStyle, Lang.member);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, Lang.lblDownline);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle1, agentName);
            CreateStringCell(row, columIndex++, headerStyle1, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle1, string.Empty);
            
            rowIndex++;
            columIndex = 0;
            row = sheet.CreateRow(rowIndex);

            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, Lang.BetAmount + " / " + Lang.lblBetCount);
            CreateStringCell(row, columIndex++, headerStyle, Lang.CommBase);
            CreateStringCell(row, columIndex++, headerStyle, Lang.total);
            CreateStringCell(row, columIndex++, headerStyle, Lang.lblEffectiveStake);
            CreateStringCell(row, columIndex++, headerStyle, Lang.total);
            CreateStringCell(row, columIndex++, headerStyle1, Lang.lblWinLossReport);
            CreateStringCell(row, columIndex++, headerStyle1, Lang.lblComm);
            CreateStringCell(row, columIndex++, headerStyle1, Lang.total);

            rowIndex++;
            //// end header
            #endregion

            #region body

            //// Add data rows
            foreach (var agentWinLoss in agentWinLossList)
            {
                columIndex = 0;
                row = sheet.CreateRow(rowIndex);
                
                CreateStringCell(row, columIndex++, bodyStyle1, agentWinLoss.AgentName);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)(agentWinLoss.SoccerShare / 100.00));
                CreateStringCell(row, columIndex++, bodyStyle, FormatNumber20(agentWinLoss.BetAmount) + " / " + FormatNumber(agentWinLoss.BetCount));
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.CommBase);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.MemberTotal);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.DownLineEffectiveStake);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.DownLineTotal);
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
            var font1 = CreateFont(HSSFColor.BLACK.index, 14, isBold: true);
            var titleStyle = CreateStyle(align: HorizontalAlignment.CENTER, font: font1, isBorder: false);
            CreateStringCell(rowTitle, 0, titleStyle, title);

            var rowFilter = sheet.CreateRow(2);
            var fontFilter = CreateFont(HSSFColor.BLUE.index, 11, isBold: true, isItalic: true);
            var filterStyle = CreateStyle(font: fontFilter, isBorder: false);
            CreateStringCell(rowFilter, 0, filterStyle, filter);
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

            return _hssfworkbook;
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
            var sheet = _hssfworkbook.CreateSheet(Lang.agentwinloss);
            
            #region declare style of cell
            CustomizeColor();
            var font = CreateFont(HSSFColor.WHITE.index, 10);
            var fontHeader = CreateFont(HSSFColor.BLACK.index, 10);

            var headerStyle = CreateStyle(HSSFColor.BROWN.index, HorizontalAlignment.CENTER, font);
            var headerStyle1 = CreateStyle(HSSFColor.BLUE_GREY.index, HorizontalAlignment.CENTER, fontHeader);

            //var headerStyle2 = CreateStyle(HSSFColor.LIME.index, HorizontalAlignment.CENTER, font);
            //var headerStyle3 = CreateStyle(HSSFColor.BROWN.index, HorizontalAlignment.CENTER, font);
            //var headerStyle4 = CreateStyle(HSSFColor.PINK.index, HorizontalAlignment.CENTER, font);
            //var headerStyle5 = CreateStyle(HSSFColor.LIGHT_GREEN.index, HorizontalAlignment.CENTER, font);

            var bodyStyle = CreateStyle(align: HorizontalAlignment.RIGHT);
            var bodyStyle1 = CreateStyle();

            var font2 = CreateFont(HSSFColor.WHITE.index, 10);
            var footerStyle = CreateStyle(HSSFColor.RED.index, HorizontalAlignment.RIGHT, font2);
            var footerStyle1 = CreateStyle(HSSFColor.RED.index, HorizontalAlignment.LEFT, font2);

            IDataFormat format = _hssfworkbook.CreateDataFormat();
            var numberFormat = format.GetFormat("#,##0.00");

            #endregion

            #region header
            var rowIndex = 4;
            var columIndex = 0;
            var row = sheet.CreateRow(rowIndex);

            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, Lang.lblSoccerPT);
            CreateStringCell(row, columIndex++, headerStyle, Lang.member);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, Lang.lblDownline);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle1, agentName);
            CreateStringCell(row, columIndex++, headerStyle1, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle1, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, Lang.lblUpline);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);

            rowIndex++;
            columIndex = 0;
            row = sheet.CreateRow(rowIndex);

            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, string.Empty);
            CreateStringCell(row, columIndex++, headerStyle, Lang.BetAmount + " / " + Lang.lblBetCount);
            CreateStringCell(row, columIndex++, headerStyle, Lang.CommBase);
            CreateStringCell(row, columIndex++, headerStyle, Lang.total);
            CreateStringCell(row, columIndex++, headerStyle, Lang.lblEffectiveStake);
            CreateStringCell(row, columIndex++, headerStyle, Lang.total);
            CreateStringCell(row, columIndex++, headerStyle1, Lang.lblWinLossReport);
            CreateStringCell(row, columIndex++, headerStyle1, Lang.lblComm);
            CreateStringCell(row, columIndex++, headerStyle1, Lang.total);
            CreateStringCell(row, columIndex++, headerStyle, Lang.lblEffectiveStake);
            CreateStringCell(row, columIndex++, headerStyle, Lang.total);

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
                    CreateStringCell(row, columIndex++, bodyStyle1, agentWinLoss.AgentName);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)(agentWinLoss.SoccerShare / 100.00));
                    CreateStringCell(row, columIndex++, bodyStyle, FormatNumber20(agentWinLoss.BetAmount) + " / " + FormatNumber(agentWinLoss.BetCount));
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.CommBase);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.MemberTotal);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.DownLineEffectiveStake);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.DownLineTotal);
                }
                else
                {
                    CreateStringCell(row, columIndex++, bodyStyle1, Lang.lblMemberWinLoss);
                    CreateStringCell(row, columIndex++, bodyStyle, Lang.lblNA);
                    CreateStringCell(row, columIndex++, bodyStyle, FormatNumber20(agentWinLoss.BetAmount) + " / " + FormatNumber(agentWinLoss.BetCount));
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.CommBase);
                    CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.MemberTotal);
                    CreateStringCell(row, columIndex++, bodyStyle, Lang.lblNA);
                    CreateStringCell(row, columIndex++, bodyStyle, Lang.lblNA);
                }

                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.AWinLoss);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.AComm);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.ATotal);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.UpLineEffectiveStake);
                CreateNumberCell(row, columIndex++, bodyStyle, numberFormat, (double)agentWinLoss.UpLineTotal);

                rowIndex++;
            }

            #endregion

            #region footer
            columIndex = 0;
            row = sheet.CreateRow(rowIndex);

            CreateStringCell(row, columIndex++, footerStyle1, Lang.total);
            CreateStringCell(row, columIndex++, footerStyle, string.Empty);
            CreateStringCell(row, columIndex++, footerStyle, FormatNumber20(totalBetAmount) + " / " + FormatNumber(totalBetCount));
            CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalCommBase);
            CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalMemTotal);
            CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalDownLineEffectiveStake);
            CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalDownLineTotal);
            CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalAWinLoss);
            CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalAComm);
            CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalATotal);
            CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalUpLineEffectiveStake);
            CreateNumberCell(row, columIndex++, footerStyle, numberFormat, (double)totalUpLineTotal);
            
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
            var font1 = CreateFont(HSSFColor.BLACK.index, 14, true);
            var titleStyle = CreateStyle(align: HorizontalAlignment.CENTER, font: font1, isBorder: false);
            CreateStringCell(rowTitle, 0, titleStyle, title);

            var rowFilter = sheet.CreateRow(2);
            var fontFilter = CreateFont(HSSFColor.BLUE.index, 11, true, true);
            var filterStyle = CreateStyle(font: fontFilter, isBorder: false);
            CreateStringCell(rowFilter, 0, filterStyle, filter);

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

            return _hssfworkbook;
        }

        /// <summary>
        /// Create new Font.
        /// </summary>
        /// <param name="fontColor">Color of font.</param>
        /// <param name="fontSize">Font size.</param>
        /// <param name="isBold">Is Bold or no.</param>
        /// <param name="isItalic">Is Italic or no.</param>
        /// <returns>Return New font.</returns>
        private IFont CreateFont(short fontColor = -1, short fontSize = -1, bool isBold = false, bool isItalic = false)
        {
            var newfont = _hssfworkbook.CreateFont();
            if (fontColor != -1)
            {
                newfont.Color = fontColor;
            }

            if (fontSize != -1)
            {
                newfont.FontHeightInPoints = fontSize;
            }

            if (isBold)
            {
                newfont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
            }

            newfont.IsItalic = isItalic;

            return newfont;
        }

        /// <summary>
        /// Create new style.
        /// </summary>
        /// <param name="backgroundColor">Back ground color.</param>
        /// <param name="align">Horizontal Alignment.</param>
        /// <param name="font">Font of style.</param>
        /// <returns>New style.</returns>
        private ICellStyle CreateStyle(short backgroundColor = -1, HorizontalAlignment align = HorizontalAlignment.LEFT, IFont font = null, bool isBorder = true)
        {
            ICellStyle style = _hssfworkbook.CreateCellStyle();
            if (backgroundColor != -1)
            {
                style.FillForegroundColor = backgroundColor;
                style.FillPattern = FillPatternType.SOLID_FOREGROUND;
                style.FillBackgroundColor = backgroundColor;
            }

            style.Alignment = align;
            style.VerticalAlignment = VerticalAlignment.CENTER;
            if (font != null)
            {
                style.SetFont(font);
            }

            if (isBorder)
            {
                style.BorderTop = BorderStyle.THIN;
                style.BorderLeft = BorderStyle.THIN;
                style.BorderRight = BorderStyle.THIN;
                style.BorderBottom = BorderStyle.THIN;
            }

            return style;
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
        /// Customize Color of workbook.
        /// </summary>
        private void CustomizeColor()
        {
            var palette = _hssfworkbook.GetCustomPalette();
            palette.SetColorAtIndex(HSSFColor.BLUE_GREY.index, (byte)141, (byte)200, (byte)245);
            //palette.SetColorAtIndex(HSSFColor.LIME.index, (byte)197, (byte)217, (byte)241);
            //palette.SetColorAtIndex(HSSFColor.BROWN.index, (byte)242, (byte)220, (byte)219);
            //palette.SetColorAtIndex(HSSFColor.PINK.index, (byte)228, (byte)223, (byte)236);
            //palette.SetColorAtIndex(HSSFColor.LIGHT_GREEN.index, (byte)183, (byte)222, (byte)232);
            palette.SetColorAtIndex(HSSFColor.BROWN.index, (byte)104, (byte)119, (byte)128);
            palette.SetColorAtIndex(HSSFColor.RED.index, (byte)153, (byte)0, (byte)0);
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
