// --------------------------------------------------------------------------
// <copyright file="IAgentComm.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.14">Interface Agent Comm</change>
// </history>

namespace Apollo.BetList.Core.Builder.AgentCommBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;

    public interface IAgentComm
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        AgentComm_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns></returns>
        AgentComm_Template Render(ITicket ticket, bool hideLinkDetail);
    }
}