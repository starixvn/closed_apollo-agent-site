// ---------------------------------------------------------------------------------
// <copyright file="Choice1334.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
//     <change who="Marc.Bui" date="2013.01.24">Update business</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;

    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    /// <summary>
    ///      Set x Games y & y+1 Points
    /// </summary>
    public class Choice1334 : Choice1
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

            // this.Template.Handicap.handicap = ConvertByBetType.Hdp(ticket.Handicap1);
            bool team1 = ticket.BetTeam == "1";
            bool team2 = ticket.BetTeam == "2";

            this.Template.betTeam = team1 ? string.Join(null, new string[] { "<span class=\"underdog\">", Apollo.Age.Resources.BetList.under, "</span>" }) : team2 ? string.Join(null, new string[] { "<span class=\"favorite\">", Apollo.Age.Resources.BetList.over, "</span>" }) : string.Join(null, new string[] { "<span class=\"underdog\">", Apollo.Age.Resources.BetList.exactly, "</span>" });
            //this.Template.betTypeName = ticketHelper.GetBetTypeNameById(ticket.BetTypeId, ticket.BetId);
            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
        }
    }
}