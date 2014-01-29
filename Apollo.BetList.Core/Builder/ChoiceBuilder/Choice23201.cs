// ---------------------------------------------------------------------------------
// <copyright file="Choice23201.cs" company="Nexcel Solutions Vietnam">
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
    /// Bingo Transfer.
    /// </summary>
    public class Choice23201 : Choice1
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
            this.Template.handicap = Apollo.Age.Resources.BetList.vs;

            this.Template.betTeam = string.Join(null, new string[] { Apollo.Age.Resources.BetList.bingo, " - ", Apollo.Age.Resources.BetList.transfer });
        }
    }
}