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
    using System.Collections.Generic;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Mix Parlay.
    /// </summary>
    public class Choice9 : Choice1
    {
        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        /// <param name="isShowScoreMap">If set to <c>true</c> [is show score map].</param>
        /// <param name="betListPage">The bet list page.</param>
        /// <returns>
        /// Choice Template.
        /// </returns>
        public override Choice_Template Render(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, IList<ITicket> ticketMixParlay, bool isShowScoreMap, BetListPage betListPage)
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
                                                    "<br/><br/>",
                                                    "<div id=\"divEvent_",
                                                    ticket.TransId.ToString(),
                                                    "\" style=\"display: block\" >" + RenderMixParlayDetail(ticket,ticketMixParlay,ticketHelper) + "</div>");

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
        public override List<IRichTextString> RenderRTF(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, IList<ITicket> ticketMixParlay, bool isShowScoreMap, RTFHelper rtfHelper)
        {
            List<IRichTextString> lstRTFChoices = new List<IRichTextString>(1);
            IRichTextString rtfChoice = new HSSFRichTextString(Apollo.Age.Resources.BetList.lblMixParlay);
            lstRTFChoices.Add(rtfChoice);
            return lstRTFChoices;
        }

        /// <summary>
        /// Renders the mix parlay detail.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <returns>Return mix parlay string.</returns>
        private string RenderMixParlayDetail(ITicket ticket, IList<ITicket> ticketMixParlay, ITicketHelper ticketHelper)
        {
            var line = "<div class='line'></div>";
            var result = string.Empty;
            foreach (var item in ticketMixParlay)
            {
                if (item.RefNo == ticket.RefNo)
                {
                    var builder = new BetListHTMLBuilder();
                    if (item.StatusId.ToString() == BetStatus.Void)
                    {
                        builder.AddChoice(item, ticketHelper, null, "<div class='strike_through'>", "</div>" + line, false);
                    }
                    else
                    {
                        builder.AddChoice(item, ticketHelper, null, string.Empty, line, false);
                    }

                    result += builder.GetBetList().Replace("<div class=\"league\">", string.Format("<div class=\"league\"> <span class=\"red-color\"> {0} </span> ", ticketHelper.GetSportNameById(item.SportTypeId)));
                }
            }

            return result;
        }
    }
}