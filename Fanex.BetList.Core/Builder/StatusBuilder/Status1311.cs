// ---------------------------------------------------------------------------------
// <copyright file="Status1311.cs" company="Nexcel Solutions Vietnam">
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
    ///  Set x Winner
    /// </summary>
    public class Status1311 : Status1
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

            string refNo = "0";
            if (ticket.BetId.ToString().Length > 2)
            {
                refNo = ticket.BetId.ToString().Substring(1, 2);
            }

            this.Template.StatusResult.refNo = refNo;

            return this.Template;
        }
    }
}