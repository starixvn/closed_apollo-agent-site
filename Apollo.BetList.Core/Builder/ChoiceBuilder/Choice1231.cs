// ---------------------------------------------------------------------------------
// <copyright file="Choice1231.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.02.27">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// Virtual Horse Racing - Win.
    /// </summary>
    public class Choice1231 : Choice1
    {
        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="betListPage">The bet list page.</param>
        protected override void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData)
        {
            base.BuildChoice(ticket, ticketHelper, ticketData);

            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
            this.Template.awayTeam = null;
            this.Template.LeagueName = null;

            this.Template.betTeam = string.Join(
                null,
                ticket.MatchId.ToString(),
                                                        " - ",
                                                        ticketHelper.GetHorseTeamNameById(ticket.HomeId, ticket.AwayId));

            this.Template.homeTeam = ticket.TransDate.ToString("M/d/yyyy hh:mm");
        }

        /// <summary>
        /// Calculates the first goal and last goal.
        /// </summary>
        /// <param name="betStatus">The bet status.</param>
        /// <param name="homeFGLG">The home first goal and last goal.</param>
        /// <param name="awayFGLG">The away first goal and last goal.</param>
        protected override void CalculateFG_LG(string betStatus, ref string homeFGLG, ref string awayFGLG)
        {
            // There's no first goal last goal for this bettype
            homeFGLG = string.Empty;
            awayFGLG = string.Empty;
        }

        /// <summary>
        /// Calculates the first goal and last goal for export excel.
        /// </summary>
        /// <param name="betStatus">The bet status.</param>
        /// <param name="homeFGLG">The home FGLG.</param>
        /// <param name="awayFGLG">The away FGLG.</param>
        protected override void CalculateFG_LG_Excel(string betStatus, ref string homeFGLG, ref string awayFGLG)
        {
            // There's no first goal last goal for this bettype
            homeFGLG = string.Empty;
            awayFGLG = string.Empty;
        }
    }
}