// ---------------------------------------------------------------------------------
// <copyright file="Choice73.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;

    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// Bingo.
    /// </summary>
    public class Choice73 : Choice1
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

            this.Template.homeTeam = null;

            this.Template.awayTeam = null;

            this.Template.LeagueName.leagueName = null;

            this.Template.betTeam = Apollo.Age.Resources.BetList.bingo;

            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
        }
    }
}