// ---------------------------------------------------------------------------------
// <copyright file="Status31.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.31">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.StatusBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// Win
    /// </summary>
    public class Status31 : Status1
    {
        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper"></param>
        /// <param name="ticketData">Ticket Reference Data for Status33 (Win/Place)</param>
        /// <param name="isShowVNIP"></param>
        /// <returns></returns>
        public override Templates.Status_Template Render(Entities.ITicket ticket, Entities.ITicketHelper ticketHelper, List<ITicketData> ticketData, bool isShowVNIP)
        {
            base.Render(ticket, ticketHelper, ticketData, isShowVNIP);
            this.Template.StatusResult.race = string.IsNullOrEmpty(ticket.Race) ? "0" : ticket.Race;

            return this.Template;
        }
    }
}