// ---------------------------------------------------------------------------------
// <copyright file="Status1000.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.26">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.StatusBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// P2P Games
    /// </summary>
    public class Status1000 : Status1
    {
        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper"></param>
        /// <param name="ticketData">Ticket Reference Data for Status33 (Win/Place)</param>
        /// <param name="isShowVNIP"></param>
        /// <returns></returns>
        public override Templates.Status_Template Render(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, bool isShowVNIP)
        {
            base.Render(ticket, ticketHelper, ticketData, isShowVNIP);

            // Use sessionId instead of refNo to call API service
            this.Template.StatusResult.refNo = ticket.BetCheck;

            return Template;
        }
    }
}