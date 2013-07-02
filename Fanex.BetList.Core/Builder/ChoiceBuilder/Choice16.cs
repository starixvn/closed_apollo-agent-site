// ---------------------------------------------------------------------------------
// <copyright file="Choice16.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.BetList.Core.Utils;

    /// <summary>
    /// HT/FT
    /// </summary>
    public class Choice16 : Choice1
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
            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
            //this.Template.Handicap.handicap = null;
            this.Template.betTeam = ConvertByBetType.GetBetTeam(ticket.BetTeam, ticket.BetTypeId);
            //this.Template.betTeamClassName = BL_FAVORITE;
        }
    }
}