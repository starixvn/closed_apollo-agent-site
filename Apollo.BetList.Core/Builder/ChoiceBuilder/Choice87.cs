// ---------------------------------------------------------------------------------
// <copyright file="Choice87.cs" company="Nexcel Solutions Vietnam">
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
    /// Next High/Low.
    /// </summary>
    public class Choice87 : Choice1
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

            string betId = ticket.BetId.ToString();
            if (betId == "0")
            {
                betId = string.Empty;
            }

            string liveScore = (!ticket.IsLive || betId == string.Empty) ? " [0]" : string.Join(null, new string[] { " [", betId, "]" });

            string betTeam = ticket.BetTeam == "h" ? Apollo.Age.Resources.BetList.high : Apollo.Age.Resources.BetList.low;

            this.Template.betTeam = string.Join(null, new string[] { betTeam, liveScore });

            this.Template.homeTeam = string.Join(null, new string[] { Apollo.Age.Resources.BetList.numbergameno, "&nbsp;", ticket.MatchCode });
        }
    }
}