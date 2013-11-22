// ---------------------------------------------------------------------------------
// <copyright file="Choice123.cs" company="Nexcel Solutions Vietnam">
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
    /// Draw no draw.
    /// </summary>
    public class Choice123 : Choice1
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
            bool betHomeTeam = ticket.BetTeam == "h";

            string homeName = ticketHelper.GetTeamNameById(ticket.HomeId);
            string awayName = ticketHelper.GetTeamNameById(ticket.AwayId);

            this.Template.betTeam = betHomeTeam ? Apollo.Age.Resources.BetList.lblDraw : Apollo.Age.Resources.BetList.lblNoDraw;

            this.Template.homeTeam = homeName + (ticket.IsNeutral ? " (N)" : string.Empty);

            this.Template.awayTeam = awayName;

            this.Template.LeagueName.leagueName = ticketHelper.GetLeagueNameById(ticket.LeagueId);
            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
        }
    }
}