// ---------------------------------------------------------------------------------
// <copyright file="Choice10.cs" company="Nexcel Solutions Vietnam">
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
    using System.Diagnostics.CodeAnalysis;
    using Apollo.BetList.Core.Entities;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Out right class.
    /// </summary>
    public class Choice10 : Choice1
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
            this.Template.handicap = null;
            this.Template.homeTeam = null;
            this.Template.awayTeam = null;

            this.Template.betTeam = ticketHelper.GetTeamNameById(ticket.HomeId == 0 ? ticket.AwayId : ticket.HomeId);
            this.Template.LeagueName.leagueName = ticketHelper.GetLeagueNameById(ticket.LeagueId);
            this.Template.HomeAwayId.Hide();
            this.Template.Score.Hide();
        }

        /// <summary>
        /// Creates the RTF helper.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="isShowSportName">If set to <c>true</c> [is show sport name].</param>
        /// <param name="isTrikeOut">If set to <c>true</c> [is trike out].</param>
        /// <returns>
        /// RTF Helper.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        protected override RTFHelper CreateRTFHelper(ITicket ticket, RTFHelper rtfHelper, ITicketHelper ticketHelper, bool isShowSportName = false, bool isTrikeOut = false)
        {
            string choice = string.Empty;

            if (isShowSportName)
            {
                string sportName = ticketHelper.GetSportNameById(ticket.SportTypeId);
                sportName = string.Join(null, new string[] { sportName, " " });

                rtfHelper.RTFRenderer.AddText(sportName, rtfHelper.Font(HSSFColor.Red.Index, FontBoldWeight.Normal, isTrikeOut));
            }

            if (!string.IsNullOrWhiteSpace(Template.LeagueName))
            {
                choice = ticketHelper.GetLeagueNameById(ticket.LeagueId);

                rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Normal, isTrikeOut));

                choice = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(Template.homeTeam) && !string.IsNullOrWhiteSpace(Template.awayTeam))
            {
                choice = string.Join(null, new string[] { "\n", Template.homeTeam.TrimEnd() });

                rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Normal, isTrikeOut));

                choice = string.Join(null, new string[] { " ", Template.handicap.Trim(), " " });

                rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Blue.Index, FontBoldWeight.Bold, isTrikeOut));

                choice = string.Join(null, new string[] { Template.awayTeam });

                rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Normal, isTrikeOut));

                choice = string.Empty;

                if (ticket.IsLive)
                {
                    choice = string.Join(null, new string[] { " ", Template.LiveScore.liveHomeScore, ":", Template.LiveScore.liveAwayScore });

                    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Red.Index, FontBoldWeight.Normal, isTrikeOut));

                    choice = string.Empty;
                }
            }

            if (!string.IsNullOrWhiteSpace(Template.betTeam))
            {
                string ticketBetStatus = string.IsNullOrEmpty(ticket.Status) ? string.Empty : ticket.Status.ToLower();

                if (ticketBetStatus == BetStatus.Won || ticketBetStatus == BetStatus.Draw || ticketBetStatus == BetStatus.Lose
                    || (ticket.StatusId != null && ticket.StatusId.Value > 1))
                {
                    choice = string.Join(null, new string[] { " \n " });

                    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Green.Index, FontBoldWeight.Bold, isTrikeOut));

                    choice = string.Empty;
                }
                else
                {
                    choice = string.Join(null, new string[] { " \n ", " " });

                    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Normal, isTrikeOut));

                    choice = string.Empty;
                }

                choice = string.Join(null, new string[] { Template.betTeam.Replace("&nbsp;", string.Empty).Trim() });

                rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Red.Index, FontBoldWeight.Normal, isTrikeOut));

                choice = string.Empty;
            }

            rtfHelper.RTFRenderer.AddText(" @ ", rtfHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Normal, isTrikeOut));

            if (!string.IsNullOrWhiteSpace(Template.odds))
            {
                choice = string.Join(null, new string[] { Template.odds.Replace("<span class='red-color'>", string.Empty).Replace("</span>", string.Empty) });

                rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Red.Index, FontBoldWeight.Bold, isTrikeOut));

                choice = string.Empty;
            }

            if (!string.IsNullOrEmpty(choice))
            {
                choice = choice.Replace("&nbsp;", " ");
                rtfHelper.RTFRenderer.AddText("\n" + choice);
            }

            return rtfHelper;
        }
    }
}