// ---------------------------------------------------------------------------------
// <copyright file="IStatus.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.24">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.StatusBuilder
{
    using System.Collections.Generic;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using NPOI.SS.UserModel;

    public interface IStatus
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        Status_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data (Status33 - Win/Place).</param>
        /// <returns>
        /// Status template.
        /// </returns>
        Status_Template Render(ITicket ticket, List<ITicketData> ticketData);

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>
        /// Rich text string.
        /// </returns>
        IRichTextString RenderRTF(ITicket ticket, List<ITicketData> ticketData, RTFHelper rtfHelper);
    }
}