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
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NPOI.HSSF.Util;

    /// <summary>
    /// Apollo.Age.Resources.BetList Excel Builder
    /// </summary>
    public class BetListExcelBuilder : IBetListBuilder
    {
        #region Attributes

        private const string CHOICE_BUILDER_NAMESPACE = "Apollo.BetList.Core.Builder.ChoiceBuilder";
        private const string ODDS_BUILDER_NAMESPACE = "Apollo.BetList.Core.Builder.OddsBuilder";
        private const string STAKE_BUILDER_NAMESPACE = "Apollo.BetList.Core.Builder.StakeBuilder";
        private const string TRANS_BUILDER_NAMESPACE = "Apollo.BetList.Core.Builder.TransBuilder";
        private const string STATUS_BUILDER_NAMESPACE = "Apollo.BetList.Core.Builder.StatusBuilder";
        private const string AgentCommBuilderNamespace = "Apollo.BetList.Core.Builder.AgentCommBuilder";
        private const string AgentShareBuilderNamespace = "Apollo.BetList.Core.Builder.AgentShareBuilder";
        private IWorkbook _workbook;
        private short _reportColumnsNum;
        private ISheet _worksheet;
        private int _nextCellIndex;

        #endregion Attributes

        #region Properties

        public ICellStyle OddCellStyle { get; set; }

        public ICellStyle EvenCellStyle { get; set; }

        public ICellStyle OddCellStyleCenterAligned { get; set; }

        public ICellStyle EvenCellStyleCenterAligned { get; set; }

        public RTFHelper RTFHelper { get; set; }

        public int LastContentRow { get { return _worksheet.LastRowNum; } }

        #endregion Properties

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

        /// <summary>
        /// Creates the cell style.
        /// </summary>
        /// <returns>ICellStyle</returns>
        public ICellStyle CreateCellStyle()
        {
            return _workbook.CreateCellStyle();
        }

        /// <summary>
        /// Creates the font.
        /// </summary>
        /// <returns></returns>
        public IFont CreateFont()
        {
            return _workbook.CreateFont();
        }

        /// <summary>
        /// Sets the width of the column.
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <param name="columnWith">The column with.</param>
        /// <returns></returns>
        public BetListExcelBuilder SetColumnWidth(int columnIndex, short columnWith)
        {
            _worksheet.AutoSizeColumn(columnIndex);

            var currentWidth = _worksheet.GetColumnWidth(columnIndex);

            _worksheet.SetColumnWidth(columnIndex, currentWidth + columnWith);
            
            return this;
        }


        /// <summary>
        /// Builds the worksheet.
        /// </summary>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="singleRowHeight">Height of the single row.</param>
        /// <returns>BetListExcelBuilder</returns>
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
        /// <returns>IWorkbook</returns>
        public IWorkbook GetBetList()
        {
            AdaptRowHeight();
            return _workbook;
        }

        public BetListExcelBuilder AddHeader(string columnTitle)
        {
            _nextCellIndex++;

            var newRow = GetLastRow();

            var cellStyle = _workbook.CreateCellStyle();

            cellStyle.Alignment = HorizontalAlignment.CENTER;

            cellStyle.FillForegroundColor = HSSFColor.OLIVE_GREEN.index;
            cellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            
            var newCell = newRow.CreateCell(_nextCellIndex, cellStyle);

            RTFHelper.RTFRenderer.AddText(columnTitle, RTFHelper.WhiteColor);

            newCell.SetCellValue(RTFHelper.RTFRenderer.Render());

            RTFHelper.RTFRenderer.Clear();

            return this;
        }

        public BetListExcelBuilder AddTotal(string value = "", CellRangeAddress mergeRange = null)
        {
            var cellStyle = _workbook.CreateCellStyle();
            cellStyle.FillForegroundColor = HSSFColor.DARK_BLUE.index;
            cellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            cellStyle.Alignment = HorizontalAlignment.RIGHT;

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
        /// <returns></returns>
        public CellAddress AddMergedCell(IRichTextString rtfString, CellRangeAddress mergedRange, ICellStyle cellStyle)
        {
            _nextCellIndex++;
            IRow lastRow = GetLastRow();

            if (_nextCellIndex >= _reportColumnsNum)
            {
                _nextCellIndex = 0;
            }

            MergeCell(mergedRange);

            IRow firstRow = _worksheet.GetRow(mergedRange.FirstRow);
            ICell lastCell = firstRow.GetCell(mergedRange.FirstColumn);
            lastCell.CellStyle = cellStyle;
            lastCell.SetCellValue(rtfString);

            //ICell lastCell = lastRow.CreateCell(_nextCellIndex, cellStyle);
            //lastRow.GetCell(1).CellStyle.

            return new CellAddress(lastCell.RowIndex, _nextCellIndex);
        }

        /// <summary>
        /// Merges the cell.
        /// </summary>
        /// <param name="mergeRange">The merge range.</param>
        public void MergeCell(CellRangeAddress mergeRange)
        {
            _worksheet.AddMergedRegion(mergeRange);
            _nextCellIndex += (mergeRange.LastColumn - mergeRange.FirstColumn);
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

            _nextCellIndex += (mergeRange.LastColumn - mergeRange.FirstColumn);
        }

        /// <summary>
        /// Adds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns>BetListExcelBuilder</returns>
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
        /// <param name="FuncGetOddsTypeName">Name of the func get odds type.</param>
        /// <returns>BetListExcelBuilder</returns>
        public BetListExcelBuilder AddOdds(ITicket ticket, List<ITicketData> ticketData, GetCachePropertyById FuncGetOddsTypeName)
        {
            IOdds oddsBuilder = CreateOddsBuilder(ticket.BetTypeId);
            IRichTextString rtfOdds = oddsBuilder.RenderRTF(ticket, ticketData, FuncGetOddsTypeName, RTFHelper);
            AddCell(rtfOdds);
            return this;
        }

        /// <summary>
        /// Adds the stake.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>BetListExcelBuilder</returns>
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
        /// <param name="factory">The factory.</param>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <returns></returns>
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
        /// Time string.
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
        /// <param name="ticket">The ticket.</param>
        /// <param name="page">The page.</param>
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
        /// <param name="ticket">The ticket.</param>
        /// <param name="page">The page.</param>
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
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="page">The page.</param>
        /// <returns>Bet List Excel Builder.</returns>
        public BetListExcelBuilder AddShare(ITicket ticket, List<ITicketData> ticketData, BetListPage page)
        {
            IAgentShare agentShareBuilder = this.CreateAgentShareBuilder(ticket.BetTypeId);

            IRichTextString rtfAgentShare = agentShareBuilder.RenderRTF(ticket, ticketData, page, RTFHelper);
            this.AddCell(rtfAgentShare);

            return this;
        }

        /// <summary>
        /// Adds the comm.
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
        /// <returns>IRow</returns>
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
        /// <returns>IRow</returns>
        private IRow AddNewRow()
        {
            int newRowIndex = _worksheet.LastRowNum;

            if (sheetHasRows())
            {
                newRowIndex += 1;
            }

            IRow newRow = _worksheet.CreateRow(newRowIndex);
            return newRow;
        }

        /// <summary>
        /// Sheets the has rows.
        /// </summary>
        /// <returns>bool</returns>
        private bool sheetHasRows()
        {
            return _worksheet.GetRow(_worksheet.LastRowNum) != null;
        }

        /// <summary>
        /// Creates the choice builder.
        /// </summary>
        /// <param name="bettype">The bettype.</param>
        /// <returns>IChoice</returns>
        private IChoice CreateChoiceBuilder(int bettype)
        {
            string typeName = string.Join(
                                            null,
                                            new string[]
                                            {
                                                CHOICE_BUILDER_NAMESPACE,
                                                ".Choice",
                                                bettype.ToString()
                                            }
                                         );

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
        /// <param name="bettype">The bettype.</param>
        /// <returns>IOdds</returns>
        private IOdds CreateOddsBuilder(int bettype)
        {
            if (bettype <= 1199 && bettype >= 1101)
            {
                //Odds of Live Casino bettypes
                bettype = 1101;
            }
            string typeName = string.Join(
                                            null,
                                            new string[]
                                            {
                                                ODDS_BUILDER_NAMESPACE,
                                                ".Odds",
                                                bettype.ToString()
                                            }
                                         );

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
        /// <param name="bettype">The bettype.</param>
        /// <returns>IStake</returns>
        private IStake CreateStakeBuilder(int bettype)
        {
            string typeName = string.Join(
                                            null,
                                            new string[]
                                            {
                                                STAKE_BUILDER_NAMESPACE,
                                                ".Stake",
                                                bettype.ToString()
                                            }
                                         );

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
        /// <returns></returns>
        private IStatus CreateStatusBuilder(int bettype)
        {
            string typeName = string.Join(
                                            null,
                                            new string[]
                                            {
                                                STATUS_BUILDER_NAMESPACE,
                                                ".Status",
                                                bettype.ToString()
                                            }
                                         );

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
        /// Creates the trans builder.
        /// </summary>
        /// <returns></returns>
        private ITrans CreateTransBuilder(int bettype)
        {
            string typeName = string.Join(
                                            null,
                                            new string[]
                                            {
                                                TRANS_BUILDER_NAMESPACE,
                                                ".Trans",
                                                bettype.ToString()
                                            }
                                         );

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

                currentRow.HeightInPoints = totalLineBreaks * _worksheet.DefaultRowHeight;
            }
        }

        /// <summary>
        /// Inits the default RTF helper.
        /// </summary>
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
        /// Inits the default cell styles.
        /// </summary>
        private void InitDefaultCellStyles()
        {
            OddCellStyle.Alignment = EvenCellStyle.Alignment = HorizontalAlignment.RIGHT;
            OddCellStyle.VerticalAlignment = EvenCellStyle.VerticalAlignment = VerticalAlignment.TOP;
            OddCellStyle.FillPattern = EvenCellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;

            OddCellStyle.TopBorderColor = OddCellStyle.RightBorderColor = OddCellStyle.BottomBorderColor = OddCellStyle.LeftBorderColor = EvenCellStyle.TopBorderColor = EvenCellStyle.RightBorderColor = EvenCellStyle.BottomBorderColor = EvenCellStyle.LeftBorderColor = HSSFColor.BLACK.index;
            
             
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

            EvenCellStyleCenterAligned.CloneStyleFrom(EvenCellStyle);
            EvenCellStyleCenterAligned.Alignment = HorizontalAlignment.CENTER;
            EvenCellStyleCenterAligned.SetFont(RTFHelper.NormalFont);

            // For line-breaks in MS Office
            OddCellStyle.WrapText = true;
            EvenCellStyle.WrapText = true;
            EvenCellStyleCenterAligned.WrapText = true;
            OddCellStyleCenterAligned.WrapText = true;
        }
    }
}