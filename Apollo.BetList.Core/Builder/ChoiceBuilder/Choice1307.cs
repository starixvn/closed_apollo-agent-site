// ---------------------------------------------------------------------------------
// <copyright file="Choice1307.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
//     <change who="Marc.Bui" date="2013.01.21">Update betTeamClassName due to changes in Betlist
//                                              Specifications:
//                                                  - Over/Between/Under: Red/Black/Black </change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;
    using Apollo.Age.Resources;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Utils;

    /// <summary>
    ///   Match Total Games 3 ways.
    /// </summary>
    public class Choice1307 : Choice1
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

            if (ticket.BetTeam == "1")
            {
                this.Template.betTeam = BetList.over;
            }
            else if (ticket.BetTeam == "2")
            {
                this.Template.betTeam = BetList.under;
            }
            else
            {
                this.Template.betTeam = string.Format(BetList.betweenXandY, ConvertByBetType.Hdp(ticket.Handicap2), ConvertByBetType.Hdp(ticket.Handicap1));
            }

            this.Template.handicap = BetList.vs;
        }
    }
}