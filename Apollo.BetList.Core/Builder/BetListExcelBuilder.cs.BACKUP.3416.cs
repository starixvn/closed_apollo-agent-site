// ---------------------------------------------------------------------------------
// <copyright file="BetListExcelBuilder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
//     <change who="Marc.Bui" date="2013.01.29">Update line-breaks in MS Office</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder
{
    using System;
    using System.Collections.Generic;
    using Apollo.BetList.Core.Builder.AgentCommBuilder;
    using Apollo.BetList.Core.Builder.AgentShareBuilder;
    using Apollo.BetList.Core.Builder.ChoiceBuilder;
    using Apollo.BetList.Core.Builder.DiscountBuilder;
    using Apollo.BetList.Core.Builder.OddsBuilder;
    using Apollo.BetList.Core.Builder.ResultBuilder;
    using Apollo.BetList.Core.Builder.StakeBuilder;
    using Apollo.BetList.Core.Builder.StatusBuilder;
    using Apollo.BetList.Core.Builder.TimeBuilder;
    using Apollo.BetList.Core.Builder.TransBuilder;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Utils;
    using Apollo.BetList.Core.Utils.NPOIExt;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Bet List Excel Builder.
    /// </summary>
    public class BetListExcelBuilder : IBetListBuilder
    {
        #region Attributes

        private const string ChoiceBuilderNameSpace = "Apollo.BetList.Core.Builder.ChoiceBuilder";
        private const string OddsBuilderNameSpace = "Apollo.BetList.Core.Builder.OddsBuilder";
        private const string StakeBuilderNameSpace = "Apollo.BetList.Core.Builder.StakeBuilder";
        private const string TransBuilderNameSpace = "Apollo.BetList.Core.Builder.TransBuilder";
        private const string StatusBuilderNameSpace = "Apollo.BetList.Core.Builder.StatusBuilder";
        private const string AgentCommBuilderNamespace = "Apollo.BetList.Core.Builder.AgentCommBuilder";
        private const string AgentShareBuilderNamespace = "Apollo.BetList.Core.Builder.AgentShareBuilder";
        private IWorkbook _workbook;
        private short _reportColumnsNum;
        private ISheet _worksheet;
        private int _nextCellIndex;

        #endregion Attributes

        /// <summary>
        /// Initializes a new instance of the <see cref="BetListExcelBuilder" /> class.
        /// </summary>
        public BetListExcelBuilder()
        {
            _workbook = new HSSFWorkbook();
            _nextCellIndex = -1;

            RTFHelper = new RTFHelper()
            {
                RTFRenderer = new RtfTextRender(),
                Workbook = _workbook
            };
        }

        #region Properties

        public RTFHelper RTFHelper { get; set; }

        #endregion Properties

        public int LastContentRow
        {
            get
            {
                return _worksheet.LastRowNum;
            }
        }

        /// <summary>
        /// Creates the cell style.
        /// </summary>
        /// <returns>
        /// Cell Style.
        /// </returns>
        public ICellStyle CreateCellStyle()
        {
            return _workbook.CreateCellStyle();
        }

        /// <summary>
        /// Creates the font.
        /// </summary>
        /// <returns>Return font.</returns>
        public IFont CreateFont()
        {
            return _workbook.CreateFont();
        }

        /// <summary>
        /// Sets the width of the column.
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <param name="columnWith">The column with.</param>
        /// <returns>Bet List Excel Builder.</returns>
        public BetListExcelBuilder SetColumnWidth(int columnIndex, short columnWith)
        {
            _worksheet.AutoSizeColumn(columnIndex);

            _worksheet.SetColumnWidth(columnIndex, columnWith * 256);

            return this;
        }

        /// <summary>
        /// Sets the high of the row.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="rowHigh">The row high.</param>
        /// <param name="cellIndex">Index of the cell.</param>
        /// <returns>
        /// Bet List Excel Builder.
        /// </returns>
        public BetListExcelBuilder SetRowHigh(int rowIndex, short rowHigh, int cellIndex = -1)
        {
            IRow currentRow = _worksheet.GetRow(rowIndex);
            if (currentRow != null)
            {
                if (cellIndex != -1)
                {
                    int totalLineBreaks = 1;
                    ICell cell = currentRow.GetCell(cellIndex);
                    if (cell.CellType == CellType.String)
                    {
                        string[] stringSeparators = new string[] { "--------------------------------------------" };

                        totalLineBreaks = Math.Max(totalLineBreaks, cell.StringCellValue.Split(stringSeparators, StringSplitOptions.None).Length);
                    }

                    currentRow.HeightInPoints = currentRow.HeightInPoints * totalLineBreaks * rowHigh;
                }
                else
                {
                    currentRow.HeightInPoints = currentRow.HeightInPoints * rowHigh;
                }
            }

            return this;
        }

        /// <summary>
        /// Sets the last high of the row.
        /// </summary>
        /// <param name="rowHigh">The row high.</param>
        /// <returns>Bet List Excel Builder.</returns>
        public BetListExcelBuilder SetLastRowHigh(short rowHigh)
        {
            IRow currentRow = _worksheet.GetRow(_worksheet.LastRowNum);

            if (currentRow != null)
            {
                currentRow.HeightInPoints = currentRow.HeightInPoints * rowHigh;
            }

            return this;
        }

        /// <summary>
        /// Builds the worksheet.
        /// </summary>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="singleRowHeight">Height of the single row.</param>
        /// <returns>Bet List Excel Builder.</returns>
        public BetListExcelBuilder BuildWorksheet(string sheetName, short numberOfColumns, short singleRowHeight = 20)
        {
            _worksheet = _workbook.CreateSheet(sheetName);
            _worksheet.DisplayGridlines = false;
            _reportColumnsNum = numberOfColumns;

            return this;
        }

        /// <summary>
        /// Gets the bet list.
        /// </summary>
        /// <returns>Return Work book.</returns>
        public IWorkbook GetBetList()
        {
            return _workbook;
        }

        /// <summary>
        /// Adds the title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="mergeRange">The merge range.</param>
        /// <returns>Bet List Excel Builder.</returns>
        public BetListExcelBuilder AddTitle(string title, CellRangeAddress mergeRange = null)
        {
            var cellStyle = _workbook.CreateCellStyle();

            cellStyle.FillForegroundColor = HSSFColor.LightBlue.Index;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.Alignment = HorizontalAlignment.Left;
            cellStyle.VerticalAlignment = VerticalAlignment.Top;

            RTFHelper.RTFRenderer.AddText(title, RTFHelper.Font(HSSFColor.White.Index));
            var rtfString = RTFHelper.RTFRenderer.Render();

            AddMergedCell(rtfString, mergeRange, cellStyle);

            RTFHelper.RTFRenderer.Clear();

            return this;
        }

        public BetListExcelBuilder AddRow(int cellNumber)
        {
            var cellStyle = _workbook.CreateCellStyle();

            for (int i = 0; i < cellNumber; i++)
            {
                AddCell(null, cellStyle);
            }

            return this;
        }

        /// <summary>
        /// Adds the header.
        /// </summary>
        /// <param name="columnTitle">The column title.</param>
        /// <returns>Bet List Excel Builder.</returns>
        public BetListExcelBuilder AddHeader(string columnTitle)
        {
            var font = RTFHelper.Font(HSSFColor.White.Index);
            var cellStyle = RTFHelper.CellStyle(HorizontalAlignment.Center, VerticalAlignment.Top, font, HSSFColor.Grey50Percent.Index, true);

            RTFHelper.RTFRenderer.AddText(columnTitle);
            var rtfString = RTFHelper.RTFRenderer.Render();

            AddCell(rtfString, cellStyle);

            RTFHelper.RTFRenderer.Clear();

            return this;
        }

        /// <summary>
        /// Adds the total.
        /// </summary>
        /// <param name="value">The total value.</param>
        /// <param name="mergeRange">The merge range.</param>
        /// <param name="isformatNumber">If set to <c>true</c> [is format number].</param>
        /// <returns>
        /// Bet List Excel Builder.
        /// </returns>
        public BetListExcelBuilder AddTotal(string value = "", CellRangeAddress mergeRange = null, bool isformatNumber = false)
        {
            var font = RTFHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Bold);
            var cellStyle = RTFHelper.CellStyle(
                HorizontalAlignment.Right,
                VerticalAlignment.Top,
                font,
                HSSFColor.Grey25Percent.Index,
                true);

            cellStyle.BorderTop = BorderStyle.Double;
            cellStyle.TopBorderColor = HSSFColor.Orange.Index;

            if (mergeRange != null)
            {
                _nextCellIndex = 0;
                MergeCell(mergeRange, cellStyle);
            }
            else
            {
                RTFHelper.RTFRenderer.AddText(value, RTFHelper.Font(HSSFColor.White.Index));
                var rftTotalValue = RTFHelper.RTFRenderer.Render();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    AddCellNumber(Convert.ToDouble(value), cellStyle, isformatNumber);
                }
                else
                {
                    AddCell(rftTotalValue, cellStyle);
                }

                RTFHelper.RTFRenderer.Clear();
            }

            return this;
        }

        /// <summary>
        /// Adds the cell.
        /// </summary>
        /// <param name="rtfString">The RTF string.</param>
        /// <returns>Cell Address.</returns>
        public CellAddress AddCell(IRichTextString rtfString)
        {
            _nextCellIndex++;
            IRow lastRow = GetLastRow();

            if (_nextCellIndex >= _reportColumnsNum)
            {
                _nextCellIndex = 0;
            }

            ICell lastCell;
            var font = RTFHelper.Font(HSSFColor.Black.Index);
            if (lastRow.RowNum % 2 == 0)
            {
                lastCell = lastRow.CreateCell(
                    _nextCellIndex,
                    RTFHelper.CellStyle(HorizontalAlignment.Right, VerticalAlignment.Top, font, HSSFColor.White.Index, true));
            }
            else
            {
                lastCell = lastRow.CreateCell(
                    _nextCellIndex,
                    RTFHelper.CellStyle(HorizontalAlignment.Right, VerticalAlignment.Top, font, HSSFColor.White.Index, true));
            }

            lastCell.SetCellValue(rtfString);

            return new CellAddress(lastCell.RowIndex, _nextCellIndex);
        }

        /// <summary>
        /// Adds the cell.
        /// </summary>
        /// <param name="rtfString">The RTF string.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <returns>Cell Address.</returns>
        public CellAddress AddCell(IRichTextString rtfString, ICellStyle cellStyle)
        {
            _nextCellIndex++;
            IRow lastRow = GetLastRow();

            if (_nextCellIndex >= _reportColumnsNum)
            {
                _nextCellIndex = 0;
            }

            ICell lastCell = lastRow.CreateCell(_nextCellIndex, cellStyle);

            lastCell.SetCellValue(rtfString);

            return new CellAddress(lastCell.RowIndex, _nextCellIndex);
        }

        /// <summary>
        /// Adds the cell number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Cell Address.</returns>
        public CellAddress AddCellNumber(double value)
        {
            _nextCellIndex++;
            IRow lastRow = GetLastRow();

            if (_nextCellIndex >= _reportColumnsNum)
            {
                _nextCellIndex = 0;
            }

            ICell lastCell = lastRow.CreateCell(_nextCellIndex);

            lastCell.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0.00");

            lastCell.SetCellType(CellType.Numeric);
            lastCell.SetCellValue(value);

            return new CellAddress(lastCell.RowIndex, _nextCellIndex);
        }

        /// <summary>
        /// Adds the cell number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <param name="isFormatNumber">If set to <c>true</c> [is format number].</param>
        /// <returns>Cell Address.</returns>
        public CellAddress AddCellNumber(double value, ICellStyle cellStyle, bool isFormatNumber = false)
        {
            _nextCellIndex++;
            IRow lastRow = GetLastRow();

            if (_nextCellIndex >= _reportColumnsNum)
            {
                _nextCellIndex = 0;
            }

            ICell lastCell = lastRow.CreateCell(_nextCellIndex, cellStyle);

            if (isFormatNumber)
            {
                lastCell.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0.00");
            }

            lastCell.SetCellType(CellType.Numeric);
            lastCell.SetCellValue(value);

            return new CellAddress(lastCell.RowIndex, _nextCellIndex);
        }

        /// <summary>
        /// Adds the merged cell.
        /// </summary>
        /// <param name="rtfString">The RTF string.</param>
        /// <param name="mergedRange">The merged range.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <returns>Cell Address.</returns>
        public CellAddress AddMergedCell(IRichTextString rtfString, CellRangeAddress mergedRange, ICellStyle cellStyle)
        {
            _nextCellIndex++;

            if (_nextCellIndex >= _reportColumnsNum)
            {
                _nextCellIndex = 0;
            }

            MergeCell(mergedRange);
            IRow lastRow = GetLastRow();

            ICell lastCell = lastRow.CreateCell(mergedRange.FirstColumn);
            lastCell.CellStyle = cellStyle;
            lastCell.SetCellValue(rtfString);

            return new CellAddress(lastCell.RowIndex, _nextCellIndex);
        }

        /// <summary>
        /// Merges the cell.
        /// </summary>
        /// <param name="mergeRange">The merge range.</param>
        public void MergeCell(CellRangeAddress mergeRange)
        {
            _worksheet.AddMergedRegion(mergeRange);
            _nextCellIndex += mergeRange.LastColumn - mergeRange.FirstColumn;
        }

        /// <summary>
        /// Merges the cell.
        /// </summary>
        /// <param name="mergeRange">The merge range.</param>
        /// <param name="cellStyle">The cell style.</param>
        public void MergeCell(CellRangeAddress mergeRange, ICellStyle cellStyle)
        {
            _worksheet.AddMergedRegion(mergeRange);

            for (int i = mergeRange.FirstRow; i <= mergeRange.LastRow; i++)
            {
                IRow mergedRow = _worksheet.GetRow(i);
                if (mergedRow != null)
                {
                    mergedRow.CreateCell(mergeRange.LastColumn, cellStyle);
                }
            }

            _nextCellIndex += mergeRange.LastColumn - mergeRange.FirstColumn;
        }

        /// <summary>
        /// Adds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        /// <returns>
        /// Bet List Excel Builder.
        /// </returns>
        public BetListExcelBuilder AddChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, IList<ITicket> ticketMixParlay)
        {
            IChoice choiceBuilder = CreateChoiceBuilder(ticket.BetTypeId);
            List<IRichTextString> rtfChoiceList = choiceBuilder.RenderRTF(ticket, ticketHelper, ticketData, ticketMixParlay, false, RTFHelper);
            foreach (IRichTextString rtfString in rtfChoiceList)
            {
                AddCell(rtfString);
            }

            return this;
        }

        /// <summary>
        /// Adds the odds.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="funcGetOddsTypeName">Name of the function get odds type.</param>
        /// <returns>Bet List Excel Builder.</returns>
        public BetListExcelBuilder AddOdds(ITicket ticket, List<ITicketData> ticketData, GetCachePropertyById funcGetOddsTypeName)
        {
            IOdds oddsBuilder = CreateOddsBuilder(ticket.BetTypeId);
            IRichTextString rtfOdds = oddsBuilder.RenderRTF(ticket, ticketData, funcGetOddsTypeName, RTFHelper);
            AddCell(rtfOdds);
            return this;
        }

        /// <summary>
        /// Adds the stake.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Bet List Excel Builder.</returns>
        public BetListExcelBuilder AddStake(ITicket ticket)
        {
            var font = RTFHelper.Font(HSSFColor.Black.Index);
            var cellStyle = RTFHelper.CellStyle(
<<<<<<< HEAD
                HorizontalAlignment.Right, 
                VerticalAlignment.Top, 
                font, 
                HSSFColor.White.Index, 
=======
                HorizontalAlignment.Right,
                VerticalAlignment.Top, 
                font,
                HSSFColor.White.Index,
>>>>>>> 2d206d25e34a6b73ac09143bf39f022d4c9b94a3
                true);

            IStake stakeBuilder = CreateStakeBuilder(ticket.BetTypeId);
            IRichTextString rtfStake = stakeBuilder.RenderRTF(ticket, RTFHelper);
            this.AddCellNumber(Convert.ToDouble(rtfStake.String), cellStyle, true);

            return this;
        }

        /// <summary>
        /// Adds the status.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns>
        /// Bet List Excel Builder.
        /// </returns>
        public BetListExcelBuilder AddStatus(ITicket ticket, List<ITicketData> ticketData)
        {
            IStatus statusBuilder = this.CreateStatusBuilder(ticket.BetTypeId);
            IRichTextString rtfStatus = statusBuilder.RenderRTF(ticket, ticketData, RTFHelper);
            var font = RTFHelper.Font(HSSFColor.Black.Index);
            this.AddCell(rtfStatus, RTFHelper.CellStyle(HorizontalAlignment.Center, VerticalAlignment.Top, font, HSSFColor.White.Index, true));
            return this;
        }

        /// <summary>
        /// Build the time.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        /// <param name="page">The page bet list.</param>
        /// <returns>
        /// Bet List Excel Builder.
        /// </returns>
        public BetListExcelBuilder AddTime(ITicket ticket, BetListPage page)
        {
            var timeBuilder = new Time();

            IRichTextString rtfTime = timeBuilder.RenderRTF(ticket, page, RTFHelper);
            var font = RTFHelper.Font(HSSFColor.Black.Index);
            this.AddCell(rtfTime, RTFHelper.CellStyle(HorizontalAlignment.Center, VerticalAlignment.Top, font, HSSFColor.White.Index, true));

            return this;
        }

        /// <summary>
        /// Adds the discount.
        /// </summary>
        /// <param name="ticket">The ticket item.</param>
        /// <param name="page">The bet list page.</param>
        /// <returns>Bet List Excel Builder.</returns>
        public BetListExcelBuilder AddDiscount(ITicket ticket, BetListPage page)
        {
            var discountBuilder = new Discount();
            IRichTextString rtfDiscount = discountBuilder.RenderRTF(ticket, page, RTFHelper);
            var font = RTFHelper.Font(HSSFColor.Black.Index);
            this.AddCell(rtfDiscount, RTFHelper.CellStyle(HorizontalAlignment.Center, VerticalAlignment.Top, font, HSSFColor.White.Index, true));

            return this;
        }

        /// <summary>
        /// Adds the result.
        /// </summary>
        /// <param name="ticket">The ticket item.</param>
        /// <param name="page">The bet list page.</param>
        /// <returns>Bet List Excel Builder.</returns>
        public BetListExcelBuilder AddResult(ITicket ticket, BetListPage page)
        {
            var cellStyle = RTFHelper.CellStyle(
                HorizontalAlignment.Right,
                VerticalAlignment.Top,
                RTFHelper.Font(HSSFColor.Black.Index),
                HSSFColor.White.Index,
                true);
            var resultBuilder = new Result();

            IRichTextString rtfResult = resultBuilder.RenderRTF(ticket, page, RTFHelper);
            this.AddCellNumber(Convert.ToDouble(rtfResult.String), cellStyle, true);

            return this;
        }

        /// <summary>
        /// Adds the share.
        /// </summary>
        /// <param name="ticket">The ticket item.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="page">The bet list page.</param>
        /// <returns>Bet List Excel Builder.</returns>
        public BetListExcelBuilder AddShare(ITicket ticket, List<ITicketData> ticketData, BetListPage page)
        {
            IAgentShare agentShareBuilder = this.CreateAgentShareBuilder(ticket.BetTypeId);

            IRichTextString rtfAgentShare = agentShareBuilder.RenderRTF(ticket, ticketData, page, RTFHelper);
            this.AddCell(rtfAgentShare);

            return this;
        }

        /// <summary>
        /// Adds the commission.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Bet List Excel Builder.</returns>
        public BetListExcelBuilder AddComm(ITicket ticket)
        {
            IAgentComm agentCommBuilder = this.CreateAgentCommBuilder(ticket.BetTypeId);

            IRichTextString rtfAgentComm = agentCommBuilder.RenderRTF(ticket, RTFHelper);
            this.AddCell(rtfAgentComm);

            return this;
        }

        /// <summary>
        /// Adds the type of the game.
        /// </summary>
        /// <param name="ticket">The ticket item.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="page">The bet list page.</param>
        /// <param name="funcGetOddsTypeName">Name of the function get odds type.</param>
        /// <returns>Bet List Excel Builder.</returns>
        public BetListExcelBuilder AddGameType(ITicket ticket, ITicketHelper ticketHelper, BetListPage page, GetCachePropertyById funcGetOddsTypeName)
        {
            ITrans transBuilder = this.CreateTransBuilder(ticket.BetTypeId);
            IRichTextString rtfTrans = transBuilder.RenderRTF(ticket, ticketHelper, page, RTFHelper, funcGetOddsTypeName);
            var font = RTFHelper.Font(HSSFColor.Black.Index);
            this.AddCell(rtfTrans, RTFHelper.CellStyle(HorizontalAlignment.Center, VerticalAlignment.Top, font, HSSFColor.White.Index, true));

            return this;
        }

        /// <summary>
        /// Gets the last row.
        /// </summary>
        /// <returns>Return row.</returns>
        private IRow GetLastRow()
        {
            IRow lastRow = _worksheet.GetRow(_worksheet.LastRowNum);
            if (lastRow == null || lastRow.Cells.Count == _reportColumnsNum || _nextCellIndex >= _reportColumnsNum)
            {
                lastRow = AddNewRow();
            }

            return lastRow;
        }

        /// <summary>
        /// Adds the new row.
        /// </summary>
        /// <returns>Return row.</returns>
        private IRow AddNewRow()
        {
            int newRowIndex = _worksheet.LastRowNum;

            if (SheetHasRows())
            {
                newRowIndex += 1;
            }

            IRow newRow = _worksheet.CreateRow(newRowIndex);
            return newRow;
        }

        /// <summary>
        /// Sheets the has rows.
        /// </summary>
        /// <returns>
        /// Return true if has rows.
        /// </returns>
        private bool SheetHasRows()
        {
            return _worksheet.GetRow(_worksheet.LastRowNum) != null;
        }

        /// <summary>
        /// Creates the choice builder.
        /// </summary>
        /// <param name="bettype">The bet type.</param>
        /// <returns>Return choice.</returns>
        private IChoice CreateChoiceBuilder(int bettype)
        {
            string[] buildChoice = new string[] { ChoiceBuilderNameSpace, ".Choice", bettype.ToString() };
            string typeName = string.Join(null, buildChoice);

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new Choice1();
            }
            else
            {
                return Activator.CreateInstance(type) as IChoice;
            }
        }

        /// <summary>
        /// Creates the odds builder.
        /// </summary>
        /// <param name="bettype">The bet type.</param>
        /// <returns>Return odd.</returns>
        private IOdds CreateOddsBuilder(int bettype)
        {
            if (bettype <= 1199 && bettype >= 1101)
            {
                // Odds of Live Casino bet types.
                bettype = 1101;
            }

            string[] choiceBuilder = new string[] { OddsBuilderNameSpace, ".Odds", bettype.ToString() };
            string typeName = string.Join(null, choiceBuilder);

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new Odds1();
            }
            else
            {
                return Activator.CreateInstance(type) as IOdds;
            }
        }

        /// <summary>
        /// Creates the stake builder.
        /// </summary>
        /// <param name="bettype">The bet type.</param>
        /// <returns>Return stake.</returns>
        private IStake CreateStakeBuilder(int bettype)
        {
            string[] choiceBuilder = new string[] { StakeBuilderNameSpace, ".Stake", bettype.ToString() };
            string typeName = string.Join(null, choiceBuilder);

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new Stake1();
            }
            else
            {
                return Activator.CreateInstance(type) as IStake;
            }
        }

        /// <summary>
        /// Creates the status builder.
        /// </summary>
        /// <param name="bettype">The bet type.</param>
        /// <returns>Return status.</returns>
        private IStatus CreateStatusBuilder(int bettype)
        {
            string[] choiceBuilder = new string[] { StatusBuilderNameSpace, ".Status", bettype.ToString() };
            string typeName = string.Join(null, choiceBuilder);

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new Status1();
            }
            else
            {
                return Activator.CreateInstance(type) as IStatus;
            }
        }

        /// <summary>
        /// Creates the transaction builder.
        /// </summary>
        /// <param name="bettype">The bet type.</param>
        /// <returns>
        /// Return transaction.
        /// </returns>
        private ITrans CreateTransBuilder(int bettype)
        {
            string[] choiceBuilder = new string[] { TransBuilderNameSpace, ".Trans", bettype.ToString() };
            string typeName = string.Join(null, choiceBuilder);

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new Trans1();
            }
            else
            {
                return Activator.CreateInstance(type) as ITrans;
            }
        }

        /// <summary>
        /// Creates the agent PT builder.
        /// </summary>
        /// <param name="bettype">The bet type.</param>
        /// <returns>
        /// Agent share interface.
        /// </returns>
        private IAgentShare CreateAgentShareBuilder(int bettype)
        {
            string[] agentShareBuilder = new string[] { AgentShareBuilderNamespace, ".AgentShare", bettype.ToString() };
            string typeName = string.Join(null, agentShareBuilder);

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new AgentShare1();
            }
            else
            {
                return Activator.CreateInstance(type) as IAgentShare;
            }
        }

        /// <summary>
        /// Creates the agent Commission builder.
        /// </summary>
        /// <param name="bettype">The bet type.</param>
        /// <returns>
        /// Agent commission interface.
        /// </returns>
        private IAgentComm CreateAgentCommBuilder(int bettype)
        {
            string[] agentCommBuilder = new string[] { AgentCommBuilderNamespace, ".AgentComm", bettype.ToString() };
            string typeName = string.Join(null, agentCommBuilder);

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new AgentComm1();
            }
            else
            {
                return Activator.CreateInstance(type) as IAgentComm;
            }
        }
    }
}