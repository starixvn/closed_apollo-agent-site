// --------------------------------------------------------------------------
// <copyright file="IAgentShare.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.14">Interface Agent Share</change>
// </history>
namespace Fanex.BetList.Core.Builder.AgentShareBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;

    public interface IAgentShare
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        AgentShare_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns>Agent share template.</returns>
        AgentShare_Template Render(ITicket ticket, List<ITicketData> ticketData);
    }
}