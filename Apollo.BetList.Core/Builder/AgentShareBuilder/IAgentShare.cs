// --------------------------------------------------------------------------
// <copyright file="IAgentShare.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.14">Interface Agent Share</change>
// </history>
namespace Apollo.BetList.Core.Builder.AgentShareBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using NPOI.SS.UserModel;

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

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="page">The page.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>Rich Text String.</returns>
        IRichTextString RenderRTF(ITicket ticket, List<ITicketData> ticketData, BetListPage page, RTFHelper rtfHelper);
    }
}