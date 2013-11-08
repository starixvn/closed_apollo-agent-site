// ---------------------------------------------------------------------------------
// <copyright file="Choice1101.cs" company="Nexcel Solutions Vietnam">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    /// <summary>
    /// Baccarat game.
    /// </summary>
   [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
    public class Choice1101 : Choice1
    {
        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="betListPage">The bet list page.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        protected override void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, BetListPage betListPage)
        {
            base.BuildChoice(ticket, ticketHelper, ticketData, betListPage);

            this.Template.LeagueName.leagueName = string.Join(null, new string[] { Apollo.Age.Resources.BetList.baccarat, "&nbsp;", ticket.ShowTime });

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
                        betTeamName = ticketHelper.GetCasinoBetTeamNameById(refDataItem.BetTeam);
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

            // Remove the last line-break
            int lastBreak = Template.betTeam.LastIndexOf("\n");
            Template.betTeam = Template.betTeam.Remove(lastBreak, 1);
        }
    }
}