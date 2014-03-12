// ---------------------------------------------------------------------------------
// <copyright file="Choice26.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.Age.Resources;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// Both/One/Neither team to score.
    /// </summary>
    public class Choice26 : Choice1
    {
        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        protected override void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData)
        {
            base.BuildChoice(ticket, ticketHelper, ticketData);

            this.Template.handicap = BetList.vs;

            if (ticket.BetTeam == "b")
            {
                this.Template.betTeam = BetList.lblBoth;
            }
            else if (ticket.BetTeam == "o")
            {
                this.Template.betTeam = BetList.lblOne;
            }
            else if (ticket.BetTeam == "n")
            {
                this.Template.betTeam = BetList.nogoal;
            }
        }
    }
}