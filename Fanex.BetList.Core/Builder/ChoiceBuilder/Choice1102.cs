// ---------------------------------------------------------------------------------
// <copyright file="Choice1102.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
//     <change who="Marc.Bui" date="2013.01.31">Update export excel</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;
    using System.Text;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    /// <summary>
    /// Live Roulette
    /// </summary>
    public class Choice1102 : Choice1
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

            this.Template.LeagueName.leagueName = string.Join(null, new string[] { Apollo.Age.Resources.BetList.Roulette, "&nbsp;", ticket.ShowTime });

            if (ticketData != null && ticketData.Count > 0)
            {
                List<ITicketData> foundTicketData = ticketData.FindAll(item => item.RefNo.Equals(ticket.RefNo));

                if (foundTicketData != null && foundTicketData.Count > 0)
                {
                    string betTeamName;
                    string odds;
                    StringBuilder betTeam = new StringBuilder();

                    foreach (ITicketData refDataItem in foundTicketData)
                    {
                        betTeamName = ticketHelper.GetRouletteBetItemNameById(refDataItem.BetTeam);
                        odds = ConvertByBetType.Odds(refDataItem.Odds, ticket.BetTypeId, ticket.OddsType);
                        betTeam.AppendFormat("<div><span class='favorite'>{0}</span>&nbsp;<span class='stake'>{1}</span>&nbsp;@&nbsp;<span class='handicap custom'>{2}</span></div>", betTeamName, ConvertByBetType.Stake(refDataItem.Stake), odds);
                    }

                    this.Template.betTeam = betTeam.ToString();
                }
            }
            else
            {
                this.Template.betTeam = string.Empty;
            }
        }

        /// <summary>
        /// Adjusts the bet team to RTF.
        /// </summary>
        protected override void AdjustBetTeamToRTF()
        {
            Template.betTeam = Template.betTeam
                                    .Replace("<div><span class='favorite'>", string.Empty)
                                    .Replace("</span>", string.Empty)
                                    .Replace("<span class='handicap custom'>", string.Empty)
                                    .Replace("</div>", "\n")
                                    .Replace("<span class='stake'>", string.Empty);
            //Remove the last line-break
            int lastBreak = Template.betTeam.LastIndexOf("\n");
            Template.betTeam = Template.betTeam.Remove(lastBreak, 1);
        }
    }
}