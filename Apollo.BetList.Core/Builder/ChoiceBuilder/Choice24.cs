// ---------------------------------------------------------------------------------
// <copyright file="Choice24.cs" company="Nexcel Solutions Vietnam">
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

namespace Apollo.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;

    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// Double Chance.
    /// </summary>
    public class Choice24 : Choice1
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

            string awayName = ticketHelper.GetTeamNameById(ticket.AwayId);

            string team = ticket.BetTeam.Contains("1") ? this.Template.homeTeam : awayName;
            string draw_away = ticket.BetTeam.EndsWith("x") ? Apollo.Age.Resources.BetList.lblDraw : this.Template.awayTeam;

            this.Template.betTeam = string.Join(
                                            null,
                                            " <span class=\"",
                                                BetListFavorite,
                                                "\">",
                                                team,
                                                "</span>",
                                                "&nbsp;",
                                                Apollo.Age.Resources.BetList.lblOr.ToLower(),
                                                " <span class=\"",
                                                BetListFavorite,
                                                "\">",
                                                "&nbsp;",
                                                draw_away,
                                                "</span>");
        }
    }
}