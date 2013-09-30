// ---------------------------------------------------------------------------------
// <copyright file="Choice23202.cs" company="Nexcel Solutions Vietnam">
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
    /// Bingo Pending
    /// </summary>
    public class Choice23202 : Choice1
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
            // this.Template.betTypeName = null;
            //this.Template.VS = null;
            //this.Template.sportTypeName = null;
            //this.Template.Score = null;
            //this.Template.Handicap.handicap = null;

            this.Template.betTeam = string.Join(null, new string[] { Apollo.Age.Resources.BetList.bingo, " - ", Apollo.Age.Resources.BetList.pending });
            //this.Template.betTeamClassName = BL_FAVORITE;
        }
    }
}