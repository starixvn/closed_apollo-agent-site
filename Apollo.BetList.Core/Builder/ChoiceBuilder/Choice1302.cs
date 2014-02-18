// ---------------------------------------------------------------------------------
// <copyright file="Choice1302.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
//     <change who="Marc.Bui" date="2013.01.24">Update betTeamClassName to follow general rules
//                                              Choice: black </change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// Match Correct Score.
    /// </summary>
    public class Choice1302 : Choice1
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

            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
            string betTeam = string.Empty;
            if (ticket.BetTeam.Length == 2)
            {
                betTeam = string.Join(null, new string[] { ticket.BetTeam.Substring(0, 1), ":", ticket.BetTeam.Substring(1, 1) });
            }

            this.Template.betTeam = string.Join(null, new string[] { "&nbsp;", betTeam });
        }
    }
}