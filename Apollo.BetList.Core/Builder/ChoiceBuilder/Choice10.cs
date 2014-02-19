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
    using Apollo.BetList.Core.Entities;

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

                choice = string.Join(null, new string[] { Template.betTeam.Replace("&nbsp;", " ") });
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