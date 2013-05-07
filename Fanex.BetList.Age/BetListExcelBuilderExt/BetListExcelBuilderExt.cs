// ---------------------------------------------------------------------------------
// <copyright file="BetListExcelBuilderExt.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.01.29">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListExcelBuilderExt
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Resources;
    using BetListHTMLBuilderExt;
    using Fanex.BetList.Age.BetListHTMLBuilderExt.AgentCommBuilder;
    using Fanex.BetList.Age.BetListHTMLBuilderExt.AgentPTBuilder;
    using Fanex.BetList.Age.BetListHTMLBuilderExt.FooterBuilder;
    using Fanex.BetList.Age.BetListHTMLBuilderExt.WinLostBuilder;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Extend corebetlist excelbuilder
    /// </summary>
    public static class BetListExcelBuilderExt
    {
        #region Attributes

        private const string FOOTER_BUILDER_NAMESPACE = "Fanex.BetList.Age.BetListHTMLBuilderExt.FooterBuilder";
        private const string SUPERPT_BUILDER_NAMESPACE = "Fanex.BetList.Age.BetListHTMLBuilderExt.SuperPTBuilder";
        private const string MASTERPT_BUILDER_NAMESPACE = "Fanex.BetList.Age.BetListHTMLBuilderExt.MasterPTBuilder";
        private const string AGENTPT_BUILDER_NAMESPACE = "Fanex.BetList.Age.BetListHTMLBuilderExt.AgentPTBuilder";
        private const string AGENTCOMM_BUILDER_NAMESPACE = "Fanex.BetList.Age.BetListHTMLBuilderExt.AgentCommBuilder";

        #endregion Attributes

        /// <summary>
        /// Adds the no.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static BetListExcelBuilder AddNo(this BetListExcelBuilder factory, int index)
        {
            IRichTextString rtfNo = new HSSFRichTextString(index.ToString());
            factory.AddCell(rtfNo);
            return factory;
        }

        /// <summary>
        /// Adds the win loss.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
        public static BetListExcelBuilder AddWinLoss(this BetListExcelBuilder factory, ITicket ticket)
        {
            IWinLoss winlossBuilder = CreateWinLossBuilder();
            WinLoss_Template resultTemplate = winlossBuilder.Render(ticket);

            RTFNumber rtfNumber = new RTFNumber(factory.RTFHelper.RTFRenderer, factory.RTFHelper.PosFont, factory.RTFHelper.NegFont);

            rtfNumber.Render(resultTemplate.winloss);

            if (!string.IsNullOrEmpty(resultTemplate.comm))
            {
                factory.RTFHelper.RTFRenderer.AddText(string.Join(null, new string[] { "\n", resultTemplate.comm }), factory.RTFHelper.PosFont);
            }

            IRichTextString rtfWinLoss = factory.RTFHelper.RTFRenderer.Render();
            factory.RTFHelper.RTFRenderer.Clear();
            factory.AddCell(rtfWinLoss);

            return factory;
        }

        /// <summary>
        /// Adds the agent PT.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
        public static BetListExcelBuilder AddAgentPT(this BetListExcelBuilder factory, ITicket ticket, List<ITicketData> ticketData)
        {
            IAgentPT agentPTBuilder = CreateAgentPTBuilder(ticket.BetTypeId);
            AgentPT_Template resultTemplate = agentPTBuilder.Render(ticket, ticketData);

            RTFNumber rtfNumber = new RTFNumber(factory.RTFHelper.RTFRenderer, factory.RTFHelper.PosFont, factory.RTFHelper.NegFont);

            string agentPtComm = string.Empty;

            if (resultTemplate.AgentPT != null && resultTemplate.AgentPT.Visible)
            {
                // Special case for Win/Place bettype
                if (ticket.BetTypeId == 33)
                {
                    int breakPos = resultTemplate.AgentPT.agentPT_setting.IndexOf("<br>");

                    string agentPT_WinString = resultTemplate.AgentPT.agentPT_setting.Substring(0, breakPos);
                    string agentPT_PlaceString = resultTemplate.AgentPT.agentPT_setting.Replace(agentPT_WinString, string.Empty);
                    string agentPT_WinVal = ExtractStringBetweenTag("b", agentPT_WinString);
                    string agentPT_PlaceVal = ExtractStringBetweenTag("b", agentPT_PlaceString);

                    //Add Win
                    factory.RTFHelper.RTFRenderer.AddText(string.Join(null, new string[] { BetList.win, ": " }), factory.RTFHelper.PosFont);
                    rtfNumber.Render(agentPT_WinVal);

                    //Add Place
                    factory.RTFHelper.RTFRenderer.AddText(string.Join(null, new string[] { "\n", BetList.place, ": " }), factory.RTFHelper.PosFont);
                    rtfNumber.Render(agentPT_PlaceVal);
                    factory.RTFHelper.RTFRenderer.AddText("\n");
                }
                else
                {
                    agentPtComm = resultTemplate.AgentPT.agentPT_setting + "\n";
                    factory.RTFHelper.RTFRenderer.AddText(agentPtComm, factory.RTFHelper.PosFont);
                }
            }

            if (resultTemplate.AgentPTValue != null && resultTemplate.AgentPTValue.Visible)
            {
                agentPtComm = resultTemplate.AgentPTValue.agentPT_value + "\n";
                rtfNumber.Render(agentPtComm);
            }

            var rtfAgentPT = factory.RTFHelper.RTFRenderer.Render();
            factory.RTFHelper.RTFRenderer.Clear();
            factory.AddCell(rtfAgentPT);

            return factory;
        }

        /// <summary>
        /// Adds the agent Comm.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
        public static BetListExcelBuilder AddAgentComm(this BetListExcelBuilder factory, ITicket ticket, bool hideDetail, List<ITicketData> ticketData)
        {
            IAgentComm agentCommBuilder = CreateAgentCommBuilder(ticket.BetTypeId);
            AgentComm_Template resultTemplate = agentCommBuilder.Render(ticket, ticketData, hideDetail);

            RTFNumber rtfNumber = new RTFNumber(factory.RTFHelper.RTFRenderer, factory.RTFHelper.PosFont, factory.RTFHelper.NegFont);

            string agentComm = string.Empty;

            if (resultTemplate.AgentComm != null && resultTemplate.AgentComm.Visible)
            {
                agentComm = resultTemplate.AgentComm.agentComm_setting + "\n";
                factory.RTFHelper.RTFRenderer.AddText(agentComm);
            }

            if (resultTemplate.AgentCommValue != null && resultTemplate.AgentCommValue.Visible)
            {
                agentComm = resultTemplate.AgentCommValue.agentComm_value;
                factory.RTFHelper.RTFRenderer.AddText(agentComm);
            }

            var rtfAgentComm = factory.RTFHelper.RTFRenderer.Render();
            factory.RTFHelper.RTFRenderer.Clear();
            factory.AddCell(rtfAgentComm);

            return factory;
        }

        /// <summary>
        /// Adds the member.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
        public static BetListExcelBuilder AddMember(this BetListExcelBuilder factory, ITicket ticket, ITicketHelper ticketHelper)
        {
            IRichTextString rtfMember = new HSSFRichTextString(ticket.UserName);
            factory.AddCell(rtfMember, ticketHelper.Index % 2 == 0 ? factory.OddCellStyleCenterAligned : factory.EvenCellStyleCenterAligned);

            return factory;
        }

        /// <summary>
        /// Adds the caption.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="columnsNum">The columns num.</param>
        /// <returns></returns>
        public static BetListExcelBuilder AddCaption(this BetListExcelBuilder factory, string caption, int columnsNum)
        {
            ICellStyle captionStyle = factory.CreateCellStyle();
            captionStyle.CloneStyleFrom(factory.OddCellStyle);
            captionStyle.Alignment = HorizontalAlignment.LEFT;
            captionStyle.SetFont(factory.RTFHelper.PosFont);

            IRichTextString rtfCaption = new HSSFRichTextString(string.Join(null, new string[] { "        ", caption.ToUpper() }));

            CellAddress headerCellAddress = factory.AddCell(rtfCaption, captionStyle);
            CellRangeAddress mergeRange = new CellRangeAddress(headerCellAddress.RowIndex, headerCellAddress.RowIndex, 0, columnsNum - 1);
            factory.MergeCell(mergeRange, captionStyle);

            return factory;
        }

        /// <summary>
        /// Adds the header.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="isShowWinLost">if set to <c>true</c> [is show win lost].</param>
        /// <param name="isShowSuperPT">if set to <c>true</c> [is show super PT].</param>
        /// <param name="isShowMasterPT">if set to <c>true</c> [is show master PT].</param>
        /// <param name="isShowAgentPT">if set to <c>true</c> [is show agent PT].</param>
        /// <param name="isShowMember">if set to <c>true</c> [is show member].</param>
        /// <returns></returns>
        public static BetListExcelBuilder AddHeader(this BetListExcelBuilder factory, bool isShowWinLost, bool isShowAgentPT, bool isShowMember)
        {
            ICellStyle headerStyle = factory.CreateCellStyle();
            headerStyle.CloneStyleFrom(factory.EvenCellStyle);
            headerStyle.FillForegroundColor = 44;
            headerStyle.Alignment = HorizontalAlignment.CENTER;
            headerStyle.VerticalAlignment = VerticalAlignment.CENTER;
            headerStyle.SetFont(factory.RTFHelper.PosFont);

            //Set Width
            CellAddress cellAddr;
            cellAddr = factory.AddCell(new HSSFRichTextString("#"), headerStyle);
            factory.SetColumnWidth(cellAddr.ColumnIndex, 1200);

            if (isShowMember)
            {
                cellAddr = factory.AddCell(new HSSFRichTextString(BetList.member), headerStyle);
                factory.SetColumnWidth(cellAddr.ColumnIndex, 4800);
            }

            cellAddr = factory.AddCell(new HSSFRichTextString(BetList.transtime), headerStyle);
            factory.SetColumnWidth(cellAddr.ColumnIndex, 5400);

            cellAddr = factory.AddCell(new HSSFRichTextString(BetList.choice), headerStyle);
            factory.SetColumnWidth(cellAddr.ColumnIndex, 15500);

            cellAddr = factory.AddCell(new HSSFRichTextString(BetList.odds), headerStyle);
            factory.SetColumnWidth(cellAddr.ColumnIndex, 2400);

            cellAddr = factory.AddCell(new HSSFRichTextString(BetList.stake), headerStyle);
            factory.SetColumnWidth(cellAddr.ColumnIndex, 2400);

            if (isShowWinLost)
            {
                cellAddr = factory.AddCell(new HSSFRichTextString(BetList.winloss), headerStyle);
                factory.SetColumnWidth(cellAddr.ColumnIndex, 3400);
            }

            cellAddr = factory.AddCell(new HSSFRichTextString(BetList.status), headerStyle);
            factory.SetColumnWidth(cellAddr.ColumnIndex, 3400);

            if (isShowAgentPT)
            {
                cellAddr = factory.AddCell(new HSSFRichTextString(BetList.agentPT.Replace("<br/>", "\n")), headerStyle);
                factory.SetColumnWidth(cellAddr.ColumnIndex, 3400);

                cellAddr = factory.AddCell(new HSSFRichTextString(BetList.agentComm.Replace("<br/>", "\n")), headerStyle);
                factory.SetColumnWidth(cellAddr.ColumnIndex, 3400);
            }

            return factory;
        }

        /// <summary>
        /// Adds the footer.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="tickets">The tickets.</param>
        /// <param name="isShowMember">if set to <c>true</c> [is show member].</param>
        /// <param name="totalColumns">The total columns.</param>
        /// <returns></returns>
        public static BetListExcelBuilder AddFooter(this BetListExcelBuilder factory, IList<ITicket> tickets, bool isShowMember, int totalColumns)
        {
            ICellStyle footerStyle = factory.CreateCellStyle();
            footerStyle.CloneStyleFrom(factory.EvenCellStyle);
            footerStyle.FillForegroundColor = 47;
            footerStyle.Alignment = HorizontalAlignment.RIGHT;
            //footerStyle.BorderTop = CellBorderType.NONE;
            //footerStyle.BorderLeft = CellBorderType.NONE;
            //footerStyle.BorderBottom = CellBorderType.NONE;
            footerStyle.SetFont(factory.RTFHelper.PosFont);

            var rtfNumberRenderer = new RTFNumber(factory.RTFHelper.RTFRenderer, factory.RTFHelper.PosFont, factory.RTFHelper.NegFont);

            IList<ITicket> filteredTickets = tickets.Where(ticket => ticket.Status.ToLower() != BetStatus.Running).ToList();

            decimal sumWinLoss = filteredTickets.Sum(ticket => ticket.Winlost);
            decimal sumPlayerComm = filteredTickets.Sum(ticket => ticket.PlayerComm);

            HSSFRichTextString rtfSubtotalWinlossText = new HSSFRichTextString(string.Join(null, new string[] { BetList.subtotalWinlost, ":" }));

            rtfNumberRenderer.Render(ConvertByBetType.Winloss(sumWinLoss));

            IRichTextString rtfSubtotalWinlossValue = factory.RTFHelper.RTFRenderer.Render();
            factory.RTFHelper.RTFRenderer.Clear();

            HSSFRichTextString rtfSubtotalCommText = new HSSFRichTextString(string.Join(null, new string[] { BetList.subtotalCom, ":" }));

            rtfNumberRenderer.Render(ConvertByBetType.Comm(sumPlayerComm));
            IRichTextString rtfSubtotalCommValue = factory.RTFHelper.RTFRenderer.Render();
            factory.RTFHelper.RTFRenderer.Clear();

            HSSFRichTextString rtfBetListTotalText = new HSSFRichTextString(string.Join(null, new string[] { BetList.total, ":" }));

            rtfNumberRenderer.Render(ConvertByBetType.Winloss(sumWinLoss + sumPlayerComm));

            IRichTextString rtfBetListTotalValue = factory.RTFHelper.RTFRenderer.Render();
            factory.RTFHelper.RTFRenderer.Clear();

            List<KeyValuePair<IRichTextString, IRichTextString>> footerContent = new List<KeyValuePair<IRichTextString, IRichTextString>>() {
                new KeyValuePair<IRichTextString,IRichTextString>(rtfSubtotalWinlossText,rtfSubtotalWinlossValue),
                new KeyValuePair<IRichTextString,IRichTextString>(rtfSubtotalCommText,rtfSubtotalCommValue)
            };

            int lastRowIndex;
            CellRangeAddress subtotalRange, emtyCellsRange;
            int subtotalLastCellIndex = isShowMember ? 5 : 4;

            foreach (KeyValuePair<IRichTextString, IRichTextString> pair in footerContent)
            {
                CellAddress firstCell = factory.AddCell(pair.Key, footerStyle);
                lastRowIndex = factory.LastContentRow;
                subtotalRange = new CellRangeAddress(lastRowIndex, lastRowIndex, 0, subtotalLastCellIndex);
                emtyCellsRange = new CellRangeAddress(lastRowIndex, lastRowIndex, subtotalLastCellIndex + 2, totalColumns - 1);
                factory.MergeCell(subtotalRange, footerStyle);
                factory.AddCell(pair.Value, footerStyle);
                factory.AddCell(new HSSFRichTextString(string.Empty), footerStyle);
                factory.MergeCell(emtyCellsRange, footerStyle);
            }

            //Total row with bottom bordered

            ICellStyle totalCellStyle = factory.CreateCellStyle();
            totalCellStyle.CloneStyleFrom(footerStyle);
            //totalCellStyle.BorderBottom = CellBorderType.THIN;
            totalCellStyle.SetFont(factory.RTFHelper.PosFont);

            CellAddress cell = factory.AddCell(rtfBetListTotalText, totalCellStyle);
            lastRowIndex = factory.LastContentRow;
            subtotalRange = new CellRangeAddress(lastRowIndex, lastRowIndex, 0, subtotalLastCellIndex);
            emtyCellsRange = new CellRangeAddress(lastRowIndex, lastRowIndex, subtotalLastCellIndex + 2, totalColumns - 1);
            factory.MergeCell(subtotalRange, totalCellStyle);
            factory.AddCell(rtfBetListTotalValue, totalCellStyle);
            factory.AddCell(new HSSFRichTextString(string.Empty), totalCellStyle);
            factory.MergeCell(emtyCellsRange, totalCellStyle);

            return factory;
        }

        /// <summary>
        /// Creates the win loss builder.
        /// </summary>
        /// <returns></returns>
        private static IWinLoss CreateWinLossBuilder()
        {
            return new WinLost1();
        }

        /// <summary>
        /// Creates the agent PT builder.
        /// </summary>
        /// <param name="bettype">The bettype.</param>
        /// <returns></returns>
        private static IAgentPT CreateAgentPTBuilder(int bettype)
        {
            string typeName = string.Join(
                                           null,
                                           new string[]
                                            {
                                                AGENTPT_BUILDER_NAMESPACE,
                                                ".AgentPT",
                                                bettype.ToString()
                                            }
                                        );

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new AgentPT1();
            }
            else
            {
                return Activator.CreateInstance(type) as IAgentPT;
            }
        }

        /// <summary>
        /// Creates the agent Comm builder.
        /// </summary>
        /// <returns></returns>
        private static IAgentComm CreateAgentCommBuilder(int bettype)
        {
            string typeName = string.Join(
                                           null,
                                           new string[]
                                            {
                                                AGENTCOMM_BUILDER_NAMESPACE,
                                                ".AgentComm",
                                                bettype.ToString()
                                            }
                                        );

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
        /// Creates the footer builder.
        /// </summary>
        /// <param name="footerType">Type of the footer.</param>
        /// <returns></returns>
        private static Fanex.BetList.Age.BetListHTMLBuilderExt.FooterBuilder.IFooter CreateFooterBuilder(int footerType)
        {
            return new Footer1();
        }

        /// <summary>
        /// Extract string value between tags from a source string
        /// </summary>
        /// <param name="tag">tag name</param>
        /// <param name="source">source string</param>
        /// <returns></returns>
        private static string ExtractStringBetweenTag(string tag, string source)
        {
            string startTag = string.Join(null, new string[] { "<", tag, ">" });
            string endTag = string.Join(null, new string[] { "</", tag, ">" });

            int startIndex = source.IndexOf(startTag) + startTag.Length;
            int endIndex = source.IndexOf(endTag, startIndex);
            return source.Substring(startIndex, endIndex - startIndex);
        }
    }
}