// ---------------------------------------------------------------------------------
// <copyright file="Choice9.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Mix Parlay
    /// </summary>
    public class Choice9 : Choice1
    {
        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="isShowScoreMap">if set to <c>true</c> [is show score map].</param>
        /// <returns>Choice_Template.</returns>
        public override Choice_Template Render(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, bool isShowScoreMap, BetListPage betListPage)
        {
            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
            string betTypeName = ticketHelper.GetBetTypeNameById(ticket.BetTypeId);

            string mixParlay = string.Join(
                                                null,
                                                new string[]
                                                {
                                                    "<a id=\"hidden0\" href=\"javascript:showMP(&quot;",
                                                    ticket.TransId.ToString(),
                                                    "&quot;);\">",
                                                    betTypeName,
                                                    "</a>",
                                                    "<br/><br/>",
                                                    "<div id=\"divEvent_",
                                                    ticket.TransId.ToString(),
                                                    "\" style=\"display: none\" ></div>"
                                                }
                                          );

            this.Template.SetValue(mixParlay);

            return this.Template;
        }

        public override List<IRichTextString> RenderRTF(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, bool isShowScoreMap, RTFHelper rtfHelper)
        {
            List<IRichTextString> lstRTFChoices = new List<IRichTextString>(1);
            IRichTextString rtfChoice = new HSSFRichTextString(Apollo.Age.Resources.BetList.lblMixParlay);
            lstRTFChoices.Add(rtfChoice);
            return lstRTFChoices;
        }
    }
}