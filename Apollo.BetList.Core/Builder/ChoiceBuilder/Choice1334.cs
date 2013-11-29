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
    using System.Diagnostics.CodeAnalysis;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    ///      Set x Games y and y + 1 Points.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
    public class Choice1334 : Choice1
    {
        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="betListPage">The bet list page.</param>
        protected override void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, BetListPage betListPage)
        {
            base.BuildChoice(ticket, ticketHelper, ticketData, betListPage);

            bool team1 = ticket.BetTeam == "1";
            bool team2 = ticket.BetTeam == "2";

            this.Template.betTeam = team1 ? string.Join(
                null,
                new string[] { "<span class=\"underdog\">", Apollo.Age.Resources.BetList.under, "</span>" }) : team2 ? string.Join(null, new string[] { "<span class=\"favorite\">", Apollo.Age.Resources.BetList.over, "</span>" }) : string.Join(null, new string[] { "<span class=\"underdog\">", Apollo.Age.Resources.BetList.exactly, "</span>" });

            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
        }
    }
}