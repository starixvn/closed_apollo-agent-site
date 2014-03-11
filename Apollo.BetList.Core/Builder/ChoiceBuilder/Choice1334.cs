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

namespace Apollo.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;
    using Apollo.Age.Resources;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// Set x Games y and y + 1 Points.
    /// </summary>
    public class Choice1334 : Choice1
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
                this.Template.betTeam = string.Join(null, new string[] { "<span class=\"underdog\">", BetList.under, "</span>" });
            }
            else if (ticket.BetTeam == "2")
            {
                this.Template.betTeam = string.Join(null, new string[] { "<span class=\"favorite\">", BetList.over, "</span>" });
            }
            else
            {
                this.Template.betTeam = string.Join(null, new string[] { "<span class=\"underdog\">", BetList.exactly, "</span>" });
            }

            this.Template.handicap = BetList.vs;
        }
    }
}