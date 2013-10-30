// ---------------------------------------------------------------------------------
// <copyright file="Choice13.cs" company="Nexcel Solutions Vietnam">
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
    using System.Linq;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// Clean Sheet
    /// </summary>
    public class Choice13 : Choice1
    {
        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        protected override void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, BetListPage betListPage)
        {
            base.BuildChoice(ticket, ticketHelper, ticketData, betListPage);

            string awayName = ticketHelper.GetTeamNameById(ticket.AwayId);

            string team = ticket.BetTeam != null && ticket.BetTeam.Contains("h") ? this.Template.homeTeam : awayName;
            string yesNo = ticket.BetTeam.Contains('y') ? Apollo.Age.Resources.BetList.lblYes : Apollo.Age.Resources.BetList.lblNo;
            this.Template.betTeam = string.Join(
                                                    null,
                                                    new string[]
                                                    {
                                                        team,
                                                        " <span class=\"", BL_FAVORITE,
                                                        "\">",
                                                        yesNo.ToUpper(),
                                                        "</span>"
                                                    });
            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
            //this.Template.Handicap.handicap = null;
        }
    }
}