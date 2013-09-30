// ---------------------------------------------------------------------------------
// <copyright file="Choice3.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
//     <change who="Marc.Bui" date="2013.01.18">Update ScoreClassName to BL_FAVORITE (red)
//                                              for all bettypes</change>
//     <change who="Marc.Bui" date="2013.01.23">Update betTeamClassName to follow general rules
//                                              Over/Under - red/black regardless whether it's live</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;

    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    /// <summary>
    /// Over/Under
    /// </summary>
    public class Choice3 : Choice1
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

            //this.Template.Handicap.handicap = ConvertByBetType.Hdp(ticket.Handicap1);
            this.Template.betTeam = ticket.BetTeam == "h" ? Apollo.Age.Resources.BetList.over : Apollo.Age.Resources.BetList.under;
            this.Template.betTeam += Formatter.FormatHandicap(ticket.Handicap1);
            //if ((ticket.Handicap1 * 10) % 5 == 0)
            //{
            //    this.Template.betTeam += string.Format(" {0}", Formatter.DecFormat(ticket.Handicap1, 1, true));
            //}
            //else
            //{
            //    this.Template.betTeam += string.Format(" {0}/{1}", Formatter.DecFormat(ticket.Handicap1 - (decimal)0.25, 1, true), Formatter.DecFormat(ticket.Handicap1 + (decimal)0.25, 1, true));
            //}
            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
        }
    }
}