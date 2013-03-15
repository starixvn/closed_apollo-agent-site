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
    using Fanex.BetList.Core.App_GlobalResources;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// Both/One/Neither team to score
    /// </summary>
    public class Choice26:Choice1
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

            this.Template.Handicap.handicap = null;      

            if (ticket.BetTeam == "b")
            {
                this.Template.betTeam = CoreBetList.lblBoth;
            }
            else if (ticket.BetTeam == "o")
            {
                this.Template.betTeam = CoreBetList.lblOne;
            }
            else if (ticket.BetTeam == "n")
            {
                this.Template.betTeam = CoreBetList.nogoal;
            }

            this.Template.betTeamClassName = BL_FAVORITE;              
        }
    }
}