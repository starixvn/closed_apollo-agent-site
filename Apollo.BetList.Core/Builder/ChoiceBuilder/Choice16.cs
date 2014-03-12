// ---------------------------------------------------------------------------------
// <copyright file="Choice16.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.BetList.Core.Utils;

    /// <summary>
    /// Half time or full time.
    /// </summary>
    public class Choice16 : Choice1
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
            this.Template.betTeam = ConvertByBetType.GetBetTeam(ticket.BetTeam, ticket.BetTypeId);
        }
    }
}