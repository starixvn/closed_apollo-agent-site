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

    using Apollo.BetList.Core.Entities;

    /// <summary>
    ///      Set x Games y and y+1 Winner.
    /// </summary>
    public class Choice1333 : Choice1
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

            bool team1 = ticket.BetTeam == "1";
            bool team2 = ticket.BetTeam == "2";

            string awayName = ticketHelper.GetTeamNameById(ticket.AwayId);

            this.Template.betTeam = team1 ? this.Template.homeTeam : team2 ? awayName : Apollo.Age.Resources.BetList.lblNeither;

            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
        }
    }
}