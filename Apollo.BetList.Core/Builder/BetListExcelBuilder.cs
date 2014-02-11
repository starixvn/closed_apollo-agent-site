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
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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
    using Apollo.BetList.Core.Utils.StringExt;
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

            OddCellStyle = _workbook.CreateCellStyle();
            EvenCellStyle = _workbook.CreateCellStyle();
            OddCellStyleCenterAligned = _workbook.CreateCellStyle();
            EvenCellStyleCenterAligned = _workbook.CreateCellStyle();

            _nextCellIndex = -1;

            RTFHelper = new RTFHelper()
            {
                NegFont = _workbook.CreateFont(),
                NormalFont = _workbook.CreateFont(),
                PosFont = _workbook.CreateFont(),
                ScoreFont = _workbook.CreateFont(),
                BlueAndBold = _workbook.CreateFont(),
                BlueNoBold = _workbook.CreateFont(),
                RedAndBold = _workbook.CreateFont(),
                RedNoBold = _workbook.CreateFont(),
                NegFontCrossed = _workbook.CreateFont(),
                NormalFontCrossed = _workbook.CreateFont(),
                PosFontCrossed = _workbook.CreateFont(),
                GreenAndBold = _workbook.CreateFont(),
                GreenNoBold = _workbook.CreateFont(),
                WhiteColor = _workbook.CreateFont(),
                RTFRenderer = new RtfTextRender()
            };

            InitDefaultRTFHelper();
            InitDefaultCellStyles();
        }

        #region Properties

        public ICellStyle OddCellStyle { get; set; }

        public ICellStyle EvenCellStyle { get; set; }

        public ICellStyle OddCellStyleCenterAligned { get; set; }

        public ICellStyle EvenCellStyleCenterAligned { get; set; }

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
            _worksheet.DefaultRowHeight = singleRowHeight;
            _reportColumnsNum = numberOfColumns;
            return this;
        }

        /// <summary>
        /// Gets the bet list.
        /// </summary>
        /// <returns>Return Work book.</returns>
        public IWorkbook GetBetList()
        {
            AdaptRowHeight();
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
            cellStyle.FillForegroundColor = HSSFColor.DARK_GREEN.index;
            cellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            cellStyle.Alignment = HorizontalAlignment.LEFT;
            cellStyle.VerticalAlignment = VerticalAlignment.CENTER;

            RTFHelper.RTFRenderer.AddText(title, RTFHelper.WhiteColor);
            var rtfString = RTFHelper.RTFRenderer.Render();

            AddMergedCell(rtfString, mergeRange, cellStyle);

            RTFHelper.RTFRenderer.Clear();

            return this;
        }

        /// <summary>
        /// Adds the header.
        /// </summary>
        /// <param name="columnTitle">The column title.</param>
        /// <returns>Bet List Excel Builder.</returns>
        public BetListExcelBuilder AddHeader(string columnTitle)
        {
            var cellStyle = _workbook.CreateCellStyle();

            cellStyle.Alignment = HorizontalAlignment.CENTER;

            cellStyle.FillForegroundColor = HSSFColor.OLIVE_GREEN.index;
            cellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            cellStyle.VerticalAlignment = VerticalAlignment.CENTER;
            RTFHelper.RTFRenderer.AddText(columnTitle, RTFHelper.WhiteColor);
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
        /// <returns>Bet List Excel Builder.</returns>
        public BetListExcelBuilder AddTotal(string value = "", CellRangeAddress mergeRange = null)
        {
            var cellStyle = _workbook.CreateCellStyle();
            cellStyle.FillForegroundColor = HSSFColor.DARK_BLUE.index;
            cellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            cellStyle.Alignment = HorizontalAlignment.RIGHT;
            cellStyle.VerticalAlignment = VerticalAlignment.CENTER;

            if (mergeRange != null)
            {
                _nextCellIndex = 0;
                MergeCell(mergeRange, cellStyle);
            }
            else
            {
                RTFHelper.RTFRenderer.AddText(value);
                var rftTotalValue = RTFHelper.RTFRenderer.Render();
                AddCell(rftTotalValue, cellStyle);
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

            if (lastRow.RowNum % 2 == 0)
            {
                lastCell = lastRow.CreateCell(_nextCellIndex, EvenCellStyle);
            }
            else
            {
                lastCell = lastRow.CreateCell(_nextCellIndex, OddCellStyle);
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
            IStake stakeBuilder = CreateStakeBuilder(ticket.BetTypeId);
            IRichTextString rtfStake = stakeBuilder.RenderRTF(ticket, RTFHelper);
            AddCell(rtfStake);

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
            this.AddCell(rtfStatus, OddCellStyleCenterAligned);
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

            this.AddCell(rtfTime, OddCellStyleCenterAligned);

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

            this.AddCell(rtfDiscount, OddCellStyleCenterAligned);

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
            var resultBuilder = new Result();
            IRichTextString rtfResult = resultBuilder.RenderRTF(ticket, page, RTFHelper);
            this.AddCell(rtfResult, OddCellStyleCenterAligned);

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
            this.AddCell(rtfTrans, OddCellStyleCenterAligned);

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
            string[] buildChoice = new string[]
                                            {
                                                ChoiceBuilderNameSpace,
                                                ".Choice",
                                                bettype.ToString()
                                            };
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

            string[] choiceBuilder = new string[]
                                            {
                                                OddsBuilderNameSpace,
                                                ".Odds",
                                                bettype.ToString()
                                            };
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
            string[] choiceBuilder = new string[]
                                            {
                                                StakeBuilderNameSpace,
                                                ".Stake",
                                                bettype.ToString()
                                            };
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
            string[] choiceBuilder = new string[]
                                            {
                                                StatusBuilderNameSpace,
                                                ".Status",
                                                bettype.ToString()
                                            };
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
            string[] choiceBuilder = new string[]
                                            {
                                                TransBuilderNameSpace,
                                                ".Trans",
                                                bettype.ToString()
                                            };
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
            string[] agentShareBuilder = new string[]
                                            {
                                                AgentShareBuilderNamespace,
                                                ".AgentShare",
                                                bettype.ToString()
                                            };
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
            string[] agentCommBuilder = new string[]
                                            {
                                                AgentCommBuilderNamespace,
                                                ".AgentComm",
                                                bettype.ToString()
                                            };
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

        /// <summary>
        /// Adapts the width of the row.
        /// </summary>
        private void AdaptRowWidth()
        {
            for (short i = 0; i < _reportColumnsNum; i++)
            {
                _worksheet.AutoSizeColumn(i);
            }
        }

        /// <summary>
        /// Adapts the height of the row.
        /// </summary>
        private void AdaptRowHeight()
        {
            IEnumerator rowEnum = _worksheet.GetRowEnumerator();

            while (rowEnum.MoveNext())
            {
                IRow currentRow = rowEnum.Current as HSSFRow;
                int totalLineBreaks = 0;
                foreach (ICell cell in currentRow.Cells)
                {
                    totalLineBreaks = Math.Max(totalLineBreaks, cell.StringCellValue.GetLineBreakTotal());
                }

                currentRow.HeightInPoints = totalLineBreaks * 15;
            }
        }

        /// <summary>
        /// Initializes the default RTF helper.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        private void InitDefaultRTFHelper()
        {
            RTFHelper.PosFont.Boldweight = RTFHelper.PosFontCrossed.Boldweight = (short)FontBoldWeight.BOLD;
            RTFHelper.NegFont.Boldweight = RTFHelper.NegFontCrossed.Boldweight = (short)FontBoldWeight.BOLD;
            RTFHelper.NormalFont.Boldweight = RTFHelper.NormalFontCrossed.Boldweight = (short)FontBoldWeight.NORMAL;

            RTFHelper.NegFont.FontName =
            RTFHelper.NormalFont.FontName =
            RTFHelper.PosFont.FontName =
            RTFHelper.ScoreFont.FontName =
            RTFHelper.BlueAndBold.FontName =
            RTFHelper.BlueNoBold.FontName =
            RTFHelper.RedAndBold.FontName =
            RTFHelper.NegFontCrossed.FontName =
            RTFHelper.NormalFontCrossed.FontName =
            RTFHelper.PosFontCrossed.FontName =
            RTFHelper.GreenAndBold.FontName =
            RTFHelper.GreenNoBold.FontName =
            RTFHelper.RedNoBold.FontName = "Tahoma";

            RTFHelper.NegFont.FontHeightInPoints =
            RTFHelper.NormalFont.FontHeightInPoints =
            RTFHelper.PosFont.FontHeightInPoints =
            RTFHelper.ScoreFont.FontHeightInPoints =
            RTFHelper.BlueAndBold.FontHeightInPoints =
            RTFHelper.BlueNoBold.FontHeightInPoints =
            RTFHelper.RedAndBold.FontHeightInPoints =
            RTFHelper.NormalFontCrossed.FontHeightInPoints =
            RTFHelper.PosFontCrossed.FontHeightInPoints =
            RTFHelper.NegFontCrossed.FontHeightInPoints =
            RTFHelper.GreenAndBold.FontHeightInPoints =
            RTFHelper.GreenNoBold.FontHeightInPoints =
            RTFHelper.RedNoBold.FontHeightInPoints = 9;

            RTFHelper.NormalFontCrossed.IsStrikeout = RTFHelper.PosFontCrossed.IsStrikeout = RTFHelper.NegFontCrossed.IsStrikeout = true;

            RTFHelper.NegFont.Color = RTFHelper.NegFontCrossed.Color = IndexedColors.RED.Index;

            RTFHelper.ScoreFont.Color = IndexedColors.GREEN.Index;
            RTFHelper.ScoreFont.Boldweight = (short)FontBoldWeight.BOLD;

            RTFHelper.BlueAndBold.Color = IndexedColors.BLUE.Index;
            RTFHelper.BlueAndBold.Boldweight = (short)FontBoldWeight.BOLD;

            RTFHelper.BlueNoBold.Color = IndexedColors.BLUE.Index;
            RTFHelper.BlueNoBold.Boldweight = (short)FontBoldWeight.NORMAL;

            RTFHelper.RedAndBold.Color = IndexedColors.RED.Index;
            RTFHelper.RedAndBold.Boldweight = (short)FontBoldWeight.BOLD;

            RTFHelper.RedNoBold.Color = IndexedColors.RED.Index;
            RTFHelper.RedNoBold.Boldweight = (short)FontBoldWeight.NORMAL;

            RTFHelper.GreenAndBold.Color = IndexedColors.GREEN.Index;
            RTFHelper.GreenAndBold.Boldweight = (short)FontBoldWeight.BOLD;

            RTFHelper.GreenNoBold.Color = IndexedColors.GREEN.Index;
            RTFHelper.GreenNoBold.Boldweight = (short)FontBoldWeight.NORMAL;

            RTFHelper.WhiteColor.Color = IndexedColors.WHITE.Index;
            RTFHelper.WhiteColor.Boldweight = (short)FontBoldWeight.BOLD;
        }

        /// <summary>
        /// Initializes the default cell styles.
        /// </summary>
        private void InitDefaultCellStyles()
        {
            OddCellStyle.Alignment = EvenCellStyle.Alignment = HorizontalAlignment.RIGHT;
            OddCellStyle.VerticalAlignment = EvenCellStyle.VerticalAlignment = VerticalAlignment.CENTER;
            OddCellStyle.FillPattern = EvenCellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;

            OddCellStyle.TopBorderColor =
            OddCellStyle.RightBorderColor =
            OddCellStyle.BottomBorderColor =
            OddCellStyle.LeftBorderColor =
            EvenCellStyle.TopBorderColor =
            EvenCellStyle.RightBorderColor =
            EvenCellStyle.BottomBorderColor =
            EvenCellStyle.LeftBorderColor = HSSFColor.BLACK.index;

            OddCellStyle.BorderTop = OddCellStyle.BorderLeft = OddCellStyle.BorderRight = OddCellStyle.BorderBottom = BorderStyle.THIN;
            EvenCellStyle.BorderTop = EvenCellStyle.BorderLeft = EvenCellStyle.BorderRight = EvenCellStyle.BorderBottom = BorderStyle.THIN;

            OddCellStyle.FillForegroundColor = HSSFColor.WHITE.index;
            OddCellStyle.FillBackgroundColor = HSSFColor.WHITE.index;

            EvenCellStyle.FillForegroundColor = HSSFColor.WHITE.index;
            EvenCellStyle.FillBackgroundColor = HSSFColor.WHITE.index;

            OddCellStyle.SetFont(RTFHelper.NormalFont);
            EvenCellStyle.SetFont(RTFHelper.NormalFont);

            OddCellStyleCenterAligned.CloneStyleFrom(OddCellStyle);
            OddCellStyleCenterAligned.Alignment = HorizontalAlignment.CENTER;
            OddCellStyleCenterAligned.SetFont(RTFHelper.NormalFont);
            OddCellStyleCenterAligned.VerticalAlignment = VerticalAlignment.CENTER;

            EvenCellStyleCenterAligned.CloneStyleFrom(EvenCellStyle);
            EvenCellStyleCenterAligned.Alignment = HorizontalAlignment.CENTER;
            EvenCellStyleCenterAligned.SetFont(RTFHelper.NormalFont);
            EvenCellStyleCenterAligned.VerticalAlignment = VerticalAlignment.CENTER;

            // For line-breaks in MS Office
            OddCellStyle.WrapText = true;
            EvenCellStyle.WrapText = true;
            EvenCellStyleCenterAligned.WrapText = true;
            OddCellStyleCenterAligned.WrapText = true;
        }
    }
}