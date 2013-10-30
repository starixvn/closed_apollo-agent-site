// ---------------------------------------------------------------------------------
// <copyright file="Choice26.cs" company="Nexcel Solutions Vietnam">
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
    /// Both/One/Neither team to score.
    /// </summary>
    public class Choice26 : Choice1
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

            //this.Template.Handicap.handicap = null;
            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
            if (ticket.BetTeam == "b")
            {
                this.Template.betTeam = Apollo.Age.Resources.BetList.lblBoth;
            }
            else if (ticket.BetTeam == "o")
            {
                this.Template.betTeam = Apollo.Age.Resources.BetList.lblOne;
            }
            else if (ticket.BetTeam == "n")
            {
                this.Template.betTeam = Apollo.Age.Resources.BetList.nogoal;
            }

            //this.Template.betTeamClassName = BL_FAVORITE;
        }
    }
}