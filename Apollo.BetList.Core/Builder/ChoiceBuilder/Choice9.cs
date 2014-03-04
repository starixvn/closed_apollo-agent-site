// ---------------------------------------------------------------------------------
// <copyright file="Choice9.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.ChoiceBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Mix Parlay.
    /// </summary>
    public class Choice9 : Choice1
    {
        private const string ChoiceBuilderNameSpace = "Apollo.BetList.Core.Builder.ChoiceBuilder";

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        /// <param name="isShowScoreMap">If set to <c>true</c> [is show score map].</param>
        /// <returns>
        /// Choice Template.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        public override Choice_Template Render(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, IList<ITicket> ticketMixParlay, bool isShowScoreMap)
        {
            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
            string betTypeName = ticketHelper.GetBetTypeNameById(ticket.BetTypeId);

            string mixParlay = string.Join(
                                                null,
                                                "<a id=\"hidden0\" href=\"javascript:showMP(&quot;",
                                                    ticket.TransId.ToString(),
                                                    "&quot;);\">",
                                                    betTypeName,
                                                    "</a>",
                                                    "<br/>",
                                                    "<div id=\"divEvent_",
                                                    ticket.TransId.ToString(),
                                                    "\" style=\"display: block\" >" + RenderMixParlayDetail(ticket, ticketMixParlay, ticketHelper) + "</div>");

            this.Template.SetValue(mixParlay);

            return this.Template;
        }

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        /// <param name="isShowScoreMap">If set to <c>true</c> [is show score map].</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>
        /// Choice Template.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        public override List<IRichTextString> RenderRTF(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, IList<ITicket> ticketMixParlay, bool isShowScoreMap, RTFHelper rtfHelper)
        {
            rtfHelper.RTFRenderer.AddText(Apollo.Age.Resources.BetList.lblMixParlay + "\n\n", rtfHelper.BlueNoBold);

            if (ticketMixParlay != null)
            {
                IList<ITicket> parlayData = ticketMixParlay.Where(x => x.RefNo == ticket.RefNo).ToList();

                foreach (ITicket tkt in parlayData)
                {
                    IChoice choiceBuilder = this.CreateChoiceBuilder(tkt.BetTypeId);
                    this.Template = choiceBuilder.Render(tkt, ticketHelper, null, isShowScoreMap);
                    if (tkt.StatusId.ToString() == BetStatusId.Void)
                    {
                        rtfHelper = this.CreateRTFHelper(tkt, rtfHelper, ticketHelper, true, true);
                    }
                    else
                    {
                        rtfHelper = this.CreateRTFHelper(tkt, rtfHelper, ticketHelper, true);
                    }

                    if (tkt.TransId != parlayData.Last().TransId)
                    {
                        rtfHelper.RTFRenderer.AddText("\n--------------------------------------------\n");
                    }
                }
            }

            var choice = rtfHelper.RTFRenderer.Render();

            rtfHelper.RTFRenderer.Clear();

            return new List<IRichTextString>() { choice };
        }

        /// <summary>
        /// Renders the mix parlay detail.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <returns>Return mix parlay string.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        private string RenderMixParlayDetail(ITicket ticket, IList<ITicket> ticketMixParlay, ITicketHelper ticketHelper)
        {
            var line = "<div class='line'></div>";
            var result = string.Empty;
            var tickets = ticketMixParlay.Where(x => x.RefNo == ticket.RefNo);
            foreach (var item in tickets)
            {
                var builder = new BetListHTMLBuilder();

                if (tickets.LastOrDefault().TransId == item.TransId)
                {
                    line = string.Empty;
                }

                if (item.StatusId.ToString() == BetStatusId.Void)
                {
                    builder.AddChoice(item, ticketHelper, null, "<div class='strike_through'>", "</div>" + line, false);
                }
                else
                {
                    builder.AddChoice(item, ticketHelper, null, string.Empty, line, false);
                }

                result += builder.GetBetList().Replace("<div class=\"league\">", string.Format("<div class=\"league\"> <span class=\"red-color\"> {0} </span> ", ticketHelper.GetSportNameById(item.SportTypeId)));
            }

            return result;
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
    }
}