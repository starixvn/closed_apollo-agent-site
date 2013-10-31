// ---------------------------------------------------------------------------------
// <copyright file="Choice89.cs" company="Nexcel Solutions Vietnam">
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
    /// Next Combo
    /// do NOT show oddtype because this bettype just offer for DEC
    /// it supports for live ball, so always 37.5.
    /// </summary>
    public class Choice89 : Choice1
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

            string betId = ticket.BetId.ToString();
            if (betId == "0")
            {
                betId = string.Empty;
            }

            string liveScore = (!ticket.IsLive || betId == string.Empty) ? " [0]" : string.Join(null, new string[] { " [", betId, "]" });

            string betTeam = string.Empty;
            switch (ticket.BetTeam)
            {
                case "1:1": betTeam = Apollo.Age.Resources.BetList.lblOverOdd;
                    break;
                case "1:2": betTeam = Apollo.Age.Resources.BetList.lblOverEven;
                    break;
                case "2:1": betTeam = Apollo.Age.Resources.BetList.lblUnderOdd;
                    break;
                case "2:2": betTeam = Apollo.Age.Resources.BetList.lblUnderEven;
                    break;
            }

            betTeam += " <span style='color:#555555'>37.5</span> ";

            this.Template.betTeam = betTeam + liveScore;

            this.Template.homeTeam = string.Join(null, new string[] { Apollo.Age.Resources.BetList.numbergameno, "&nbsp;", ticket.MatchCode });
        }
    }
}