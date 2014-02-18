// ---------------------------------------------------------------------------------
// <copyright file="Choice1313.cs" company="Nexcel Solutions Vietnam">
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
    using System.Diagnostics.CodeAnalysis;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Utils;

    /// <summary>
    ///    Set x Total Games 3-way.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
    public class Choice1313 : Choice1
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

            bool betOver = ticket.BetTeam == "1";
            bool betUnder = ticket.BetTeam == "2";

            string formatXY = Apollo.Age.Resources.BetList.betweenXandY;

            if (formatXY == null)
            {
                formatXY = "{0} - {1}";
            }

            this.Template.betTeam = betOver ? Apollo.Age.Resources.BetList.over : betUnder ? Apollo.Age.Resources.BetList.under : string.Format(formatXY, ConvertByBetType.Hdp(ticket.Handicap2), ConvertByBetType.Hdp(ticket.Handicap1));

            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
        }
    }
}