// ---------------------------------------------------------------------------------
// <copyright file="AgentPT33.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.01.28">Create</change>
//     <change who="Marc.Bui" date="2013.02.27">Add GetReferenceData for PT 1233</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListHTMLBuilderExt.AgentPTBuilder
{
    using System.Collections.Generic;
    using Apollo.Age.Resources;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    /// <summary>
    /// Agent PT 33.
    /// </summary>
    public class AgentPT33 : AgentPT1
    {
        protected int winBetId, placeBetId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentPT33" /> class.
        /// </summary>
        public AgentPT33()
        {
            winBetId = 31;
            placeBetId = 32;
        }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns>
        /// Agent PT template.
        /// </returns>
        public override AgentPT_Template Render(ITicket ticket, List<Core.Entities.ITicketData> ticketData)
        {
            base.Render(ticket, ticketData);

            decimal agentPT_Win = 0;
            decimal agentPT_Place = 0;

            List<ITicketData> refTicketData = this.GetReferenceData(ticket, ticketData);

            if (refTicketData.Count > 1)
            {
                agentPT_Win = refTicketData.FindLast(item => item.BetTypeId == winBetId).AgentPT;
                agentPT_Place = refTicketData.FindLast(item => item.BetTypeId == placeBetId).AgentPT;
            }

            this.Template.AgentPT.agentPT_setting = string.Join(
                                                                null,
                                                                "<span class='blue' style='font-weight:normal;'>",
                                                                    BetList.win + ": ",
                                                                    "<b>",
                                                                    Formatter.FormatNumber2(agentPT_Win * 100, null),
                                                                    "%</b>",
                                                                    "</span><br>",
                                                                    "<span class='' style='font-weight:normal;'>",
                                                                    BetList.place + ": ",
                                                                    "<b>",
                                                                    Formatter.FormatNumber2(agentPT_Place * 100, null),
                                                                    "%</b>",
                                                                    "</span>");

            return this.Template;
        }

        /// <summary>
        /// Gets the reference data.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns>List of tickets.</returns>
        protected virtual List<ITicketData> GetReferenceData(ITicket ticket, List<ITicketData> ticketData)
        {
            return ticketData.FindAll(item => item.RefNo.Equals(ticket.TransId.ToString()));
        }
    }
}