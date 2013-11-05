// ---------------------------------------------------------------------------------
// <copyright file="IStatus.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.24">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.StatusBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
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
        /// <param name="isShowVNIP">If set to <c>true</c> [is show VNIP].</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>
        /// Rich text string.
        /// </returns>
        IRichTextString RenderRTF(ITicket ticket, List<ITicketData> ticketData, bool isShowVNIP, RTFHelper rtfHelper);
    }
}