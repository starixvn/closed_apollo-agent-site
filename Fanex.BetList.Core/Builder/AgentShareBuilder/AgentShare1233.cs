// --------------------------------------------------------------------------
// <copyright file="AgentShare1233.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.14">Agent PT</change>
// </history>

namespace Fanex.BetList.Core.Builder.AgentShareBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// Agent share.
    /// </summary>
    public class AgentShare1233 : AgentShare33
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentShare1233" /> class.
        /// </summary>
        public AgentShare1233()
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