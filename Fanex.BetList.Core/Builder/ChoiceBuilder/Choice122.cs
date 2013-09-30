// ---------------------------------------------------------------------------------
// <copyright file="Choice122.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
//     <change who="Marc.Bui" date="2013.01.18">Update ScoreClassName to BL_FAVORITE (red)
//                                              for all bettypes</change>
//     <change who="Marc.Bui" date="2013.02.21">Update showing Neutral</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// Away No Bet
    /// </summary>
    public class Choice122 : Choice1
    {
        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        protected override void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, BetListPage betListPage)
        {
            base.BuildChoice(ticket, ticketHelper, ticketData, betListPage);
            bool betHomeTeam = ticket.BetTeam != null && ticket.BetTeam.Contains("h");
            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
            //this.Template.Handicap.handicap = null;

            string homeName = ticketHelper.GetTeamNameById(ticket.HomeId);
            string awayName = ticketHelper.GetTeamNameById(ticket.AwayId);

            this.Template.homeTeam = string.Join(null, new string[] { homeName, (ticket.IsNeutral ? " (N) " : string.Empty) });

            this.Template.betTeam = betHomeTeam ? this.Template.homeTeam : Apollo.Age.Resources.BetList.lblDraw;
            //this.Template.betTeamClassName = betHomeTeam ? (ticket.Handicap2 >= ticket.Handicap1 ? BL_UNDER_DOG : BL_FAVORITE) : (ticket.Handicap2 <= ticket.Handicap1 ? BL_UNDER_DOG : BL_FAVORITE);

            //this.Template.Score.homeScore = ticket.LiveHomeScore.ToString();
            //this.Template.Score.awayScore = ticket.LiveAwayScore.ToString();

            // For special cases
            //this.Template.Score.Visible = ticket.IsLive;

            this.Template.awayTeam = awayName;

            //this.CalculateFG_LG(ticket.EventStatus, ref this.Template.home_firstGoal_lastGoal, ref this.Template.away_firstGoal_lastGoal);

            //this.Template.sportTypeName = ticketHelper.GetSportNameById(ticket.SportTypeId);
            this.Template.LeagueName.leagueName = ticketHelper.GetLeagueNameById(ticket.LeagueId);
        }
    }
}