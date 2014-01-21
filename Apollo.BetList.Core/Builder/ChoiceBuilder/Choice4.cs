// ---------------------------------------------------------------------------------
// <copyright file="Choice4.cs" company="Nexcel Solutions Vietnam">
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
    /// Correct Score.
    /// </summary>
    public class Choice4 : Choice1
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
            string choice = ticket.BetTeam;
            choice = (ticket.BetTeam == "0:5" || ticket.BetTeam == "5:0") ? ticket.BetTeam + " UP" : ticket.BetTeam;

            // "&nbsp" because export format
            this.Template.betTeam = string.Join(
                                                    null,
                                                     "&nbsp;",
                                                        choice);
        }
    }
}