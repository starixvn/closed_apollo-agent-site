// ---------------------------------------------------------------------------------
// <copyright file="Choice1328.cs" company="Nexcel Solutions Vietnam">
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
    /// Set x Game y Total Points Exact.
    /// </summary>
    public class Choice1328 : Choice1
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

            var betTeam = string.Empty;

            switch (ticket.BetTeam)
            {
                case "01":
                    betTeam = "4 " + BetList.points;
                    break;

                case "02":
                    betTeam = "5 " + BetList.points;
                    break;

                case "03":
                    betTeam = "6 " + BetList.points;
                    break;

                case "04":
                    betTeam = "7 " + BetList.upPoints;
                    break;

                default:
                    break;
            }

            // "&nbsp" because export format
            this.Template.betTeam = "&nbsp;" + betTeam;
        }
    }
}