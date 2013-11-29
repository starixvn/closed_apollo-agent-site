// ---------------------------------------------------------------------------------
// <copyright file="Choice21.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// Clean Sheet.
    /// </summary>
    public class Choice21 : Choice20
    {
        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="betListPage">The bet list page.</param>
        protected override void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, BetListPage betListPage)
        {
            base.BuildChoice(ticket, ticketHelper, ticketData, betListPage);
            if (ticket.SportTypeId == 1)
            {
                string ticketBetStatus = string.IsNullOrEmpty(ticket.Status) ? string.Empty : ticket.Status.ToLower();
                if (ticketBetStatus == BetStatus.Won || ticketBetStatus == BetStatus.Draw || ticketBetStatus == BetStatus.Lose
                    || (ticket.StatusId != null && ticket.StatusId.Value > 1))
                {
                    this.Template.Score.finalHomeScore = ticket.HaftTimeHomeScore.ToString();
                    this.Template.Score.finalAwayScore = ticket.HaftTimeAwayScore.ToString();
                }
                else
                {
                    this.Template.Score.Visible = false;
                }
            }
        }
    }
}