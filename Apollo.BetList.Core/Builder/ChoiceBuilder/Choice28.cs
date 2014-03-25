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
    using System.Diagnostics.CodeAnalysis;
    using Apollo.Age.Resources;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Utils;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

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
        protected override void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData)
        {
            base.BuildChoice(ticket, ticketHelper, ticketData);

            string teamName = BetList.lblDraw;
            string handicap = BuildHandicap(ticket, ticketHelper, out teamName);

            this.Template.betTeam = string.Join(
                null,
                teamName,
                "&nbsp;<span class=\"underdog\">(",
                handicap.ToString(),
                ")</span>");
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
        protected override RTFHelper CreateRTFHelper(
            ITicket ticket,
            RTFHelper rtfHelper,
            ITicketHelper ticketHelper,
            bool isShowSportName = false,
            bool isTrikeOut = false)
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

                rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Bold, isTrikeOut));

                choice = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(Template.HomeAwayId.homeid) && !string.IsNullOrWhiteSpace(Template.HomeAwayId.awayid))
            {
                choice = string.Join(null, new string[] { "\n", "[", Template.HomeAwayId.homeid, "]", " vs ", "[", Template.HomeAwayId.awayid, "]" });

                rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Bold, isTrikeOut));

                choice = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(Template.homeTeam) && !string.IsNullOrWhiteSpace(Template.awayTeam))
            {
                choice = string.Join(null, new string[] { "\n", Template.homeTeam.TrimEnd() });

                rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Bold, isTrikeOut));

                choice = string.Join(null, new string[] { " ", Template.handicap.Trim(), " " });

                rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Blue.Index, FontBoldWeight.Bold, isTrikeOut));

                choice = string.Join(null, new string[] { Template.awayTeam });

                rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Bold, isTrikeOut));

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
                    choice = string.Join(null, new string[] { " \n ", Template.Score.finalHomeScore, ":", Template.Score.finalAwayScore, " " });
                    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Green.Index, FontBoldWeight.Bold, isTrikeOut));

                    choice = string.Empty;
                }
                else
                {
                    choice = string.Join(null, new string[] { " \n ", " " });
                    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Bold, isTrikeOut));

                    choice = string.Empty;
                }

                string teamName = string.Empty;
                string handicap = BuildHandicap(ticket, ticketHelper, out teamName);
                handicap = handicap.Replace("&nbsp;", string.Empty);

                rtfHelper.RTFRenderer.AddText(teamName + " ", rtfHelper.Font(HSSFColor.Red.Index, FontBoldWeight.Normal, isTrikeOut));

                rtfHelper.RTFRenderer.AddText("(" + handicap + ")", rtfHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Bold, isTrikeOut));

                choice = string.Empty;
            }

            rtfHelper.RTFRenderer.AddText(" @ ", rtfHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Bold, isTrikeOut));

            if (!string.IsNullOrWhiteSpace(Template.odds))
            {
                choice = string.Join(null, new string[] { Template.odds.Replace("<span class='red-color'>", string.Empty).Replace("</span>", string.Empty) });

                rtfHelper.RTFRenderer.AddText(choice, rtfHelper.Font(HSSFColor.Red.Index, FontBoldWeight.Normal, isTrikeOut));

                choice = string.Empty;
            }

            if (!string.IsNullOrEmpty(choice))
            {
                choice = choice.Replace("&nbsp;", " ");
                rtfHelper.RTFRenderer.AddText("\n" + choice);
            }

            return rtfHelper;
        }

        /// <summary>
        /// Builds the handicap.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="teamName">Name of the team.</param>
        /// <returns>Return handicap string.</returns>
        private string BuildHandicap(ITicket ticket, ITicketHelper ticketHelper, out string teamName)
        {
            teamName = BetList.lblDraw;
            decimal hdp1 = ticket.Handicap1;
            decimal hdp2 = ticket.Handicap2;

            string handicap = string.Empty;

            if ("1" == ticket.BetTeam)
            {
                teamName = this.Template.homeTeam;
                handicap = Formatter.FormatSignNumber(hdp2 - hdp1, 0);
            }
            else if ("2" == ticket.BetTeam)
            {
                string awayName = ticketHelper.GetTeamNameById(ticket.AwayId);
                teamName = awayName;
                handicap = Formatter.FormatSignNumber(hdp1 - hdp2, 0);
            }
            else
            {
                teamName = BetList.lblDraw;
                if (hdp1 - hdp2 > 0)
                {
                    handicap = BetList.a + "&nbsp;";
                }
                else if (hdp1 - hdp2 < 0)
                {
                    handicap = BetList.h + "&nbsp;";
                }
                else
                {
                    handicap = string.Empty;
                }

                handicap += Formatter.FormatSignNumber(Math.Abs(hdp2 - hdp1), 0);
            }

            return handicap;
        }
    }
}