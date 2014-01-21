// ---------------------------------------------------------------------------------
// <copyright file="Choice81.cs" company="Nexcel Solutions Vietnam">
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
    /// 1st ball Over Under.
    /// </summary>
    public class Choice81 : Choice1
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

            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
            this.Template.homeTeam = string.Join(
                                                null,
                                                Apollo.Age.Resources.BetList.numbergameno,
                                                    "&nbsp;",
                                                    ticket.MatchCode);
        }
    }
}