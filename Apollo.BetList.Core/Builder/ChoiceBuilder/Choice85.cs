// ---------------------------------------------------------------------------------
// <copyright file="Choice85.cs" company="Nexcel Solutions Vietnam">
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
    /// Next Over/Under
    /// it supports live ball, so always 37.5.
    /// </summary>
    public class Choice85 : Choice3
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

            string liveScore = string.Empty;
            string betTeam = this.Template.betTeam;

            betTeam += " <span style='color:#555555'>37.5</span> ";

            string bettypeName = string.Empty;
            if (!ticket.IsLive)
            {
                bettypeName = string.Join(
                                                null,
                                                Apollo.Age.Resources.BetList.ft,
                                                    ". ",
                                                    bettypeName);
            }
            else
            {
                bettypeName = string.Join(
                                                null,
                                                Apollo.Age.Resources.BetList.next,
                                                    "&nbsp;",
                                                    bettypeName);

                liveScore = (!ticket.IsLive || betId == string.Empty) ? " [0]" : string.Join(
                                                                                                null,
                                                                                               " [",
                                                                                                    betId,
                                                                                                    "]");
            }

            this.Template.betTeam = betTeam + liveScore;

            this.Template.homeTeam = string.Join(
                                                null,
                                               Apollo.Age.Resources.BetList.numbergameno,
                                                    "&nbsp;",
                                                    ticket.MatchCode);
        }
    }
}