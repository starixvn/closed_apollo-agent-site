// ---------------------------------------------------------------------------------
// <copyright file="Choice86.cs" company="Nexcel Solutions Vietnam">
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
    /// Next Odd/Even.
    /// </summary>
    public class Choice86 : Choice2
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

            string betId = ticket.BetId.ToString();

            if (betId == "0")
            {
                betId = string.Empty;
            }

            string liveScore = string.Empty;
            string betTeam = this.Template.betTeam;
            string bettypeName = string.Empty;

            if (!ticket.IsLive)
            {
                bettypeName = string.Join(null, new string[] { BetList.ft, ". ", bettypeName });
            }
            else
            {
                bettypeName = BetList.next + "&nbsp;" + bettypeName;
                liveScore = (!ticket.IsLive || string.IsNullOrEmpty(betId)) ? " [0]" : string.Join(null, new string[] { " [", betId, "]" });
            }

            this.Template.betTeam = betTeam + liveScore;
            this.Template.homeTeam = string.Join(null, new string[] { BetList.numbergameno, "&nbsp;", ticket.MatchCode });
        }
    }
}