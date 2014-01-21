// ---------------------------------------------------------------------------------
// <copyright file="Choice3.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
//     <change who="Marc.Bui" date="2013.01.18">Update ScoreClassName to BL_FAVORITE (red)
//                                              for all bettypes</change>
//     <change who="Marc.Bui" date="2013.01.23">Update betTeamClassName to follow general rules
//                                              Over/Under - red/black regardless whether it's live</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;

    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Utils;

    /// <summary>
    /// Over Under.
    /// </summary>
    public class Choice3 : Choice1
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

            this.Template.betTeam = ticket.BetTeam == "h" ? Apollo.Age.Resources.BetList.over : Apollo.Age.Resources.BetList.under;
            this.Template.betTeam += Formatter.FormatHandicap(ticket.BetTypeId, ticket.Handicap1);

            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
        }
    }
}