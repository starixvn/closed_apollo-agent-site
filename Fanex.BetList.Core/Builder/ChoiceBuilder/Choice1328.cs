// ---------------------------------------------------------------------------------
// <copyright file="Choice1328.cs" company="Nexcel Solutions Vietnam">
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
    ///      Set x Game y Total Points Exact.
    /// </summary>
    public class Choice1328 : Choice1
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
            string choice = ticket.BetTeam;
            var betTeam = string.Empty;
            switch (choice)
            {
                case "01":
                    betTeam = "4 " + Apollo.Age.Resources.BetList.points;
                    break;

                case "02":
                    betTeam = "5 " + Apollo.Age.Resources.BetList.points;
                    break;

                case "03":
                    betTeam = "6 " + Apollo.Age.Resources.BetList.points;
                    break;

                case "04":
                    betTeam = "7 " + Apollo.Age.Resources.BetList.upPoints;
                    break;

                default:
                    break;
            }

            // "&nbsp" because export format
            this.Template.betTeam = "&nbsp;" + betTeam;
        }
    }
}