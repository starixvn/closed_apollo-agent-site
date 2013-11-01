// ---------------------------------------------------------------------------------
// <copyright file="Choice83.cs" company="Nexcel Solutions Vietnam">
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
    /// 1st ball Odd Even.
    /// </summary>
    public class Choice83 : Choice1
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

            this.Template.handicap = Apollo.Age.Resources.BetList.vs;

            this.Template.betTeam = ticket.BetTeam == "h" ? Apollo.Age.Resources.BetList.odd : Apollo.Age.Resources.BetList.even;

            this.Template.homeTeam = string.Join(
                                                    null,
                                                    new string[]
                                                    {
                                                        Apollo.Age.Resources.BetList.numbergameno,
                                                        "&nbsp;",
                                                        ticket.MatchCode
                                                    });
        }
    }
}