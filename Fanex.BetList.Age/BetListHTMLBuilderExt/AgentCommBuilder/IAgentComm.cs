// --------------------------------------------------------------------------
// <copyright file="IAgentComm.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListHTMLBuilderExt.AgentCommBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
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
        /// <param name="hideDetail">If set to <c>true</c> [hide detail].</param>
        /// <returns>Agent commission template.</returns>
        AgentComm_Template Render(ITicket ticket, List<ITicketData> ticketData, bool hideDetail);
    }
}