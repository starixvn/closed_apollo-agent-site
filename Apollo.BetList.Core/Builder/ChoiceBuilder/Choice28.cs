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
        protected override void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData)
        {
            base.BuildChoice(ticket, ticketHelper, ticketData);

            string teamName = Apollo.Age.Resources.BetList.lblDraw;
          

            string handicap = string.Empty;

            handicap = BuildHandicap(ticket, ticketHelper, out teamName);

            this.Template.betTeam = string.Join(
                                                    null,
                                                    teamName,
                                                        "&nbsp;<span class=\"underdog\">(",
                                                        handicap.ToString(),
                                                        ")</span>");
        }

        private string BuildHandicap(ITicket ticket, ITicketHelper ticketHelper, out string teamName)
        {
            teamName = Apollo.Age.Resources.BetList.lblDraw;
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

                handicap += Formatter.FormatSignNumber(Math.Abs(hdp2 - hdp1), 0);
            }

            return handicap;
        }

        protected override RTFHelper CreateRTFHelper(ITicket ticket, RTFHelper rtfHelper, ITicketHelper ticketHelper, bool isShowSportName = false, bool isTrikeOut = false)
        {
            string choice = string.Empty;

            if (isShowSportName)
            {
                string sportName = ticketHelper.GetSportNameById(ticket.SportTypeId);
                sportName = string.Join(null, new string[] { sportName, " " });

                if (isTrikeOut)
                {
                    rtfHelper.RTFRenderer.AddText(sportName, rtfHelper.RedNoBoldTrike);
                }
                else
                {
                    rtfHelper.RTFRenderer.AddText(sportName, rtfHelper.RedNoBold);
                }
            }

            if (!string.IsNullOrWhiteSpace(Template.LeagueName))
            {
                choice = ticketHelper.GetLeagueNameById(ticket.LeagueId);
                if (isTrikeOut)
                {
                    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.NormalFontCrossed);
                }
                else
                {
                    rtfHelper.RTFRenderer.AddText(choice);
                }

                choice = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(Template.HomeAwayId.homeid) && !string.IsNullOrWhiteSpace(Template.HomeAwayId.awayid))
            {
                choice = string.Join(null, new string[] { "\n", "[", Template.HomeAwayId.homeid, "]", " vs ", "[", Template.HomeAwayId.awayid, "]" });
                if (isTrikeOut)
                {
                    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.NormalFontCrossed);
                }
                else
                {
                    rtfHelper.RTFRenderer.AddText(choice);
                }

                choice = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(Template.homeTeam) && !string.IsNullOrWhiteSpace(Template.awayTeam))
            {
                choice = string.Join(null, new string[] { "\n", Template.homeTeam });
                if (isTrikeOut)
                {
                    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.NormalFontCrossed);
                }
                else
                {
                    rtfHelper.RTFRenderer.AddText(choice);
                }

                choice = string.Empty;

                choice = string.Join(null, new string[] { " ", Template.handicap, " " });
                if (isTrikeOut)
                {
                    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.BlueAndBoldTrike);
                }
                else
                {
                    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.BlueAndBold);
                }

                choice = string.Empty;

                choice = string.Join(null, new string[] { Template.awayTeam });

                if (isTrikeOut)
                {
                    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.NormalFontCrossed);
                }
                else
                {
                    rtfHelper.RTFRenderer.AddText(choice);
                }

                choice = string.Empty;

                if (ticket.IsLive)
                {
                    choice = string.Join(null, new string[] { " ", Template.LiveScore.liveHomeScore, ":", Template.LiveScore.liveAwayScore });
                    if (isTrikeOut)
                    {
                        rtfHelper.RTFRenderer.AddText(choice, rtfHelper.RedNoBoldTrike);
                    }
                    else
                    {
                        rtfHelper.RTFRenderer.AddText(choice, rtfHelper.RedNoBold);
                    }

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
                    if (isTrikeOut)
                    {
                        rtfHelper.RTFRenderer.AddText(choice, rtfHelper.ScoreFontTrike);
                    }
                    else
                    {
                        rtfHelper.RTFRenderer.AddText(choice, rtfHelper.ScoreFont);
                    }

                    choice = string.Empty;
                }
                else
                {
                    choice = string.Join(null, new string[] { " \n ", " " });
                    if (isTrikeOut)
                    {
                        rtfHelper.RTFRenderer.AddText(choice, rtfHelper.NormalFontCrossed);
                    }
                    else
                    {
                        rtfHelper.RTFRenderer.AddText(choice);
                    }

                    choice = string.Empty;
                }

                string teamName = string.Empty;
                string handicap = BuildHandicap(ticket, ticketHelper, out teamName);
                handicap = handicap.Replace("&nbsp;", string.Empty);

                if (isTrikeOut)
                {
                    rtfHelper.RTFRenderer.AddText(teamName + " ", rtfHelper.RedNoBoldTrike);

                    rtfHelper.RTFRenderer.AddText("(" + handicap + ")", rtfHelper.NormalFontCrossed);
                }
                else
                {
                    rtfHelper.RTFRenderer.AddText(teamName + " ", rtfHelper.RedNoBold);
                    rtfHelper.RTFRenderer.AddText("(" + handicap + ")");
                }
                
                choice = string.Empty;
            }

            if (isTrikeOut)
            {
                rtfHelper.RTFRenderer.AddText(" @ ", rtfHelper.NormalFontCrossed);
            }
            else
            {
                rtfHelper.RTFRenderer.AddText(" @ ");
            }

            if (!string.IsNullOrWhiteSpace(Template.odds))
            {
                choice = string.Join(null, new string[] { Template.odds.Replace("<span class='red-color'>", string.Empty).Replace("</span>", string.Empty) });
                if (isTrikeOut)
                {
                    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.NegFontTrike);
                }
                else
                {
                    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.NegFont);
                }

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