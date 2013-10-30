// ---------------------------------------------------------------------------------
// <copyright file="IAgentPT.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.18">Create</change>
//     <change who="Marc.Bui" date="2013.01.28">Update interface to work with Win/Place AgentPT</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListHTMLBuilderExt.AgentPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAgentPT
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        AgentPT_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns></returns>
        AgentPT_Template Render(ITicket ticket, List<ITicketData> ticketData);
    }
}
