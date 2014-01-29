// ---------------------------------------------------------------------------------
// <copyright file="Choice30.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// 1st Correct score.
    /// </summary>
    public class Choice30 : Choice4
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
            string choice = ticket.BetTeam;
            choice = (choice == "0:4" || choice == "4:0") ? choice + " UP" : choice;

            // "&nbsp" because export format
            this.Template.betTeam = "&nbsp;" + choice;
        }
    }
}