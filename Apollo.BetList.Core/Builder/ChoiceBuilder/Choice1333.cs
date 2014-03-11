// ---------------------------------------------------------------------------------
// <copyright file="Choice1333.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
//     <change who="Marc.Bui" date="2013.02.21">Update showing Neutral</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;
    using Apollo.Age.Resources;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// Set x Games y and y+1 Winner.
    /// </summary>
    public class Choice1333 : Choice1
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

            if (ticket.BetTeam == "1")
            {
                this.Template.betTeam = this.Template.homeTeam;
            }
            else if (ticket.BetTeam == "2")
            {
                this.Template.betTeam = ticketHelper.GetTeamNameById(ticket.AwayId);
            }
            else
            {
                this.Template.betTeam = BetList.lblNeither;
            }

            this.Template.handicap = BetList.vs;
        }
    }
}