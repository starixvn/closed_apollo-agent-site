// ---------------------------------------------------------------------------------
// <copyright file="AgentPT1233.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.02.27">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListHTMLBuilderExt.AgentPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentPT1233 : AgentPT33
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentPT1233" /> class.
        /// </summary>
        public AgentPT1233()
        {
            WinBetId = 1231;
            PlaceBetId = 1232;
        }

        /// <summary>
        /// Gets the reference data.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns>List of tickets.</returns>
        protected override List<ITicketData> GetReferenceData(ITicket ticket, List<ITicketData> ticketData)
        {
            return ticketData.FindAll(item => item.RefNo.Equals(ticket.RefNo));
        }
    }
}
