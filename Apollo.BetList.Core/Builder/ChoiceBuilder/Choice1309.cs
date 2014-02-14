// ---------------------------------------------------------------------------------
// <copyright file="Choice1309.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
//     <change who="Marc.Bui" date="2013.01.18">Update ScoreClassName to BL_FAVORITE (red)
//                                              for all bettypes</change>
//     <change who="Marc.Bui" date="2013.02.21">Update showing Neutral</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Utils;

    /// <summary>
    ///    Match Handicap Games 3-way.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
    public class Choice1309 : Choice1
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

            string awayName = ticketHelper.GetTeamNameById(ticket.AwayId);

            bool betHome = ticket.BetTeam == "1";
            bool betAway = ticket.BetTeam == "2";
            decimal handicap = ticket.Handicap2 > ticket.Handicap1 ? ticket.Handicap2 : ticket.Handicap1;

            if (betHome)
            {
                this.Template.betTeam = this.Template.homeTeam;
            }
            else if (betAway)
            {
                this.Template.betTeam = awayName;
            }
            else
            {
                this.Template.betTeam = ticket.Handicap2 > ticket.Handicap1 ? string.Join(null, new string[] { Apollo.Age.Resources.BetList.lblDraw, "&nbsp;(", Apollo.Age.Resources.BetList.h, "+", ConvertByBetType.Hdp(ticket.Handicap2) + ")" }) :
                                                    string.Join(null, new string[] { Apollo.Age.Resources.BetList.lblDraw, "&nbsp;(", Apollo.Age.Resources.BetList.a, "+", ConvertByBetType.Hdp(ticket.Handicap1), ")" });
            }

            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
        }
    }
}