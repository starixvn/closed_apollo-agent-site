// ---------------------------------------------------------------------------------
// <copyright file="Choice28.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
//     <change who="Marc.Bui" date="2013.01.23">Update to follow general rules
//                                              teamName: red
//                                              handicap: grey</change>
//     <change who="Marc.Bui" date="2013.02.21">Update showing Neutral</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.ChoiceBuilder
{
    using System;
    using System.Collections.Generic;

    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Utils;

    /// <summary>
    /// 3-way handicap.
    /// </summary>
    public class Choice28 : Choice1
    {
        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="betListPage">The bet list page.</param>
        protected override void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData)
        {
            base.BuildChoice(ticket, ticketHelper, ticketData);

            string teamName = Apollo.Age.Resources.BetList.lblDraw;
            decimal hdp1 = ticket.Handicap1;
            decimal hdp2 = ticket.Handicap2;

            string handicap = string.Empty;

            if ("1" == ticket.BetTeam)
            {
                teamName = this.Template.homeTeam;
                handicap = Formatter.FormatSignNumber(hdp2 - hdp1, 2);
            }
            else if ("2" == ticket.BetTeam)
            {
                string awayName = ticketHelper.GetTeamNameById(ticket.AwayId);
                teamName = awayName;
                handicap = Formatter.FormatSignNumber(hdp1 - hdp2, 2);
            }
            else
            {
                teamName = Apollo.Age.Resources.BetList.lblDraw;
                if (hdp1 - hdp2 > 0)
                {
                    handicap = Apollo.Age.Resources.BetList.a + "&nbsp;";
                }
                else if (hdp1 - hdp2 < 0)
                {
                    handicap = Apollo.Age.Resources.BetList.h + "&nbsp;";
                }
                else
                {
                    handicap = string.Empty;
                }

                handicap += Formatter.FormatSignNumber(Math.Abs(hdp2 - hdp1), 2);
            }

            this.Template.betTeam = string.Join(
                                                    null,
                                                    teamName,
                                                        "&nbsp;<span class=\"underdog\">(",
                                                        handicap.ToString(),
                                                        ")</span>");
        }
    }
}