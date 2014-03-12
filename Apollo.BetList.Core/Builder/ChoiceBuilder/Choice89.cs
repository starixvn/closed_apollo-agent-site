// ---------------------------------------------------------------------------------
// <copyright file="Choice89.cs" company="Nexcel Solutions Vietnam">
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
    /// Next Combo.
    /// Do NOT show odd type because this bet type just offer for DEC.
    /// It supports for live ball, so always 37.5.
    /// </summary>
    public class Choice89 : Choice1
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

            string liveScore = (!ticket.IsLive || betId == string.Empty) ? " [0]" : string.Join(null, new string[] { " [", betId, "]" });

            string betTeam = string.Empty;

            switch (ticket.BetTeam)
            {
                case "1:1": 
                    betTeam = BetList.lblOverOdd;
                    break;

                case "1:2": 
                    betTeam = BetList.lblOverEven;
                    break;

                case "2:1": 
                    betTeam = BetList.lblUnderOdd;
                    break;

                case "2:2": 
                    betTeam = BetList.lblUnderEven;
                    break;
            }

            betTeam += " <span style='color:#555555'>37.5</span> ";

            this.Template.betTeam = betTeam + liveScore;

            this.Template.homeTeam = string.Join(null, new string[] { BetList.numbergameno, "&nbsp;", ticket.MatchCode });
        }
    }
}