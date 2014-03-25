// ---------------------------------------------------------------------------------
// <copyright file="Choice15.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.Age.Resources;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// First one x two.
    /// </summary>
    public class Choice15 : Choice1
    {
        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        protected override void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData)
        {
            base.BuildChoice(ticket, ticketHelper, ticketData);

            this.Template.betTeam = string.Join(null, BetList.ht, ".", ticket.BetTeam.ToUpper());

            if (ticket.SportTypeId == 1)
            {
                string ticketBetStatus = string.IsNullOrEmpty(ticket.Status) ? string.Empty : ticket.Status.ToLower();

                if (ticketBetStatus == BetStatus.Won || ticketBetStatus == BetStatus.Draw || ticketBetStatus == BetStatus.Lose
                    || (ticket.StatusId != null && ticket.StatusId.Value > 1))
                {
                    this.Template.Score.finalHomeScore = ticket.HalfTimeHomeScore.ToString();
                    this.Template.Score.finalAwayScore = ticket.HalfTimeAwayScore.ToString();
                }
                else
                {
                    this.Template.Score.Visible = false;
                }
            }

            this.Template.handicap = BetList.vs;
        }
    }
}