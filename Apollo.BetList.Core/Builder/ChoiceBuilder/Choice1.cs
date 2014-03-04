// ---------------------------------------------------------------------------------
// <copyright file="Choice1.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
//     <change who="Marc.Bui" date="2012.11.26">Add Odds to show in Mix Parlay bettypes
//                                              (ones with RefNo != 100000000000) </change>
//     <change who="Marc.Bui" date="2012.12.4">Add Excel rendering </change>
//     <change who="Marc.Bui" date="2013.01.18">Update ScoreClassName to BL_FAVORITE (red)
//                                              for all bettypes</change>
//     <change who="Marc.Bui" date="2013.02.05">Update betteam when the match is Neutral </change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.ChoiceBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using Apollo.BetList.Core.Utils;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Handicap bet type class.
    /// </summary>
    public class Choice1 : IChoice
    {
        #region Attributes

        protected const string BetListUnderDog = "underdog";
        protected const string BetListFavorite = "favorite";

        private static int[] _showHomeCharacterList = new[]
            {
                (int)SportTypeNames.Soccer,
                (int)SportTypeNames.BasketBall,
                (int)SportTypeNames.FootBall,
                (int)SportTypeNames.VolleyBall,
                (int)SportTypeNames.BaseBall,
                (int)SportTypeNames.IceHockey
            };

        #endregion Attributes

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Choice1" /> class.
        /// </summary>
        public Choice1()
        {
            this.Template = new Choice_Template();
        }

        #endregion Constructors

        #region Properties

        public Choice_Template Template { get; set; }

        #endregion Properties

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="isShowScoreMap">Is show score map or not.</param>
        /// <returns>
        /// Choice Template.
        /// </returns>
        public virtual Choice_Template Render(
            ITicket ticket,
            ITicketHelper ticketHelper,
            List<ITicketData> ticketData,
            bool isShowScoreMap)
        {
            this.BuildChoice(ticket, ticketHelper, ticketData);
            this.BuildHandicapOdds(ticket);
            this.BuildScoreMap(ticket, isShowScoreMap);

            return this.Template;
        }

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="isShowScoreMap">Is show score map or not.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>
        /// Choice Template.
        /// </returns>
        public virtual List<IRichTextString> RenderRTF(
            ITicket ticket,
            ITicketHelper ticketHelper,
            List<ITicketData> ticketData,
            bool isShowScoreMap,
            RTFHelper rtfHelper)
        {
            this.Render(ticket, ticketHelper, ticketData, isShowScoreMap);
            var choiceData = BuildRTFChoice(ticket, rtfHelper, ticketHelper);
            List<IRichTextString> rtfChoice;

            if (choiceData is List<IRichTextString>)
            {
                rtfChoice = (List<IRichTextString>)choiceData;
            }
            else
            {
                rtfChoice = new List<IRichTextString>() { (IRichTextString)choiceData };
            }

            return rtfChoice;
        }

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        /// <param name="isShowScoreMap">If set to <c>true</c> [is show score map].</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>Choice Template.</returns>
        public virtual List<IRichTextString> RenderRTF(
            ITicket ticket,
            ITicketHelper ticketHelper,
            List<ITicketData> ticketData,
            IList<ITicket> ticketMixParlay,
            bool isShowScoreMap,
            RTFHelper rtfHelper)
        {
            return RenderRTF(
             ticket,
             ticketHelper,
             ticketData,
             isShowScoreMap,
             rtfHelper);
        }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        /// <param name="isShowScoreMap">If set to <c>true</c> [is show score map].</param>
        /// <returns>
        /// Choice Template.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        public virtual Choice_Template Render(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, IList<ITicket> ticketMixParlay, bool isShowScoreMap)
        {
            this.BuildChoice(ticket, ticketHelper, ticketData, ticketMixParlay);
            this.BuildHandicapOdds(ticket);
            this.BuildScoreMap(ticket, isShowScoreMap);

            return this.Template;
        }

        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        protected virtual void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData)
        {
            this.Template.LeagueName.leagueName = ticketHelper.GetLeagueNameById(ticket.LeagueId);
            this.Template.HomeAwayId.homeid = ticket.HomeId.ToString();
            this.Template.HomeAwayId.awayid = ticket.AwayId.ToString();

            string homeName = ticketHelper.GetTeamNameById(ticket.HomeId);
            string awayName = ticketHelper.GetTeamNameById(ticket.AwayId);

            // For Final Score
            string ticketBetStatus = string.IsNullOrEmpty(ticket.Status) ? string.Empty : ticket.Status.ToLower();
            if (ticketBetStatus == BetStatus.Won || ticketBetStatus == BetStatus.Draw || ticketBetStatus == BetStatus.Lose
                || (ticket.StatusId != null && ticket.StatusId.Value > 1))
            {
                this.Template.Score.finalHomeScore = ticket.FinalHomeScore.ToString();
                this.Template.Score.finalAwayScore = ticket.FinalAwayScore.ToString();
            }
            else
            {
                this.Template.Score.Visible = false;
            }

            if (ticket.IsLive)
            {
                this.Template.LiveScore.liveHomeScore = ticket.LiveHomeScore.ToString();
                this.Template.LiveScore.liveAwayScore = ticket.LiveAwayScore.ToString();
            }
            else
            {
                this.Template.LiveScore.Hide();
            }

            this.Template.homeTeam = string.Join(
                null,
                homeName,
                 ticket.IsNeutral ? " (N) " : (_showHomeCharacterList.Contains(ticket.SportTypeId) ? " (" + Apollo.Age.Resources.BetList.h + ")" : string.Empty));

            this.Template.awayTeam = awayName;

            bool betHomeTeam = ticket.BetTeam != null && ticket.BetTeam.Contains("h");

            this.Template.betTeam = betHomeTeam ? this.Template.homeTeam : awayName;

            decimal handicap = ticket.Handicap2 > ticket.Handicap1 ? ticket.Handicap2 : -ticket.Handicap1;
            this.Template.handicap = Formatter.FormatHandicap(ticket.BetTypeId, handicap);
        }

        /// <summary>
        /// Builds the RTF choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <returns>Rich text interface.</returns>
        protected virtual IRichTextString BuildRTFChoice(ITicket ticket, RTFHelper rtfHelper, ITicketHelper ticketHelper)
        {
            // the 1st line
            this.AdjustBetTeamToRTF();

            rtfHelper = CreateRTFHelper(ticket, rtfHelper, ticketHelper);

            var choiceRtf = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return choiceRtf;
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
        protected virtual RTFHelper CreateRTFHelper(ITicket ticket, RTFHelper rtfHelper, ITicketHelper ticketHelper, bool isShowSportName = false, bool isTrikeOut = false)
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
                choice = string.Join(null, new string[] { "\n", Template.homeTeam.TrimEnd() });
                if (isTrikeOut)
                {
                    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.NormalFontCrossed);
                }
                else
                {
                    rtfHelper.RTFRenderer.AddText(choice);
                }

                choice = string.Empty;

                choice = string.Join(null, new string[] { " ", Template.handicap.Trim(), " " });
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

                choice = string.Join(null, new string[] { Template.betTeam.Replace("&nbsp;", string.Empty).Trim() });
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

        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket item.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        protected virtual void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, IList<ITicket> ticketMixParlay)
        {
            BuildChoice(ticket, ticketHelper, ticketData);
        }

        /// <summary>
        /// Calculates the F g_ LG.
        /// </summary>
        /// <param name="betStatus">The bet status.</param>
        /// <param name="homeFGLG">The home FGLG.</param>
        /// <param name="awayFGLG">The away FGLG.</param>
        protected virtual void CalculateFG_LG(string betStatus, ref string homeFGLG, ref string awayFGLG)
        {
            if (betStatus == null)
            {
                return;
            }

            // Format: FL:1-2
            string[] args = betStatus.Split(new char[] { ':', '-' });

            if (args.Length != 3)
            {
                return;
            }

            if (args[1] == "1")
            {
                homeFGLG += "<div class=\"firstGoal\">&nbsp;</div>";
            }
            else if (args[1] == "2")
            {
                awayFGLG += "<div class=\"firstGoal\">&nbsp;</div>";
            }

            if (args[2] == "1")
            {
                homeFGLG += "<div class=\"lastGoal\">&nbsp;</div>";
            }
            else if (args[2] == "2")
            {
                awayFGLG += "<div class=\"lastGoal\">&nbsp;</div>";
            }

            if (!homeFGLG.Equals(string.Empty))
            {
                homeFGLG = string.Join(null, new string[] { "&nbsp;", homeFGLG });
            }

            if (!awayFGLG.Equals(string.Empty))
            {
                awayFGLG = string.Join(null, new string[] { "&nbsp;", awayFGLG });
            }
        }

        /// <summary>
        /// Adjusts the bet team to RTF.
        /// </summary>
        protected virtual void AdjustBetTeamToRTF()
        {
            Template.betTeam = Template.betTeam.Replace("<span class=\"favorite\">", string.Empty);
            Template.betTeam = Template.betTeam.Replace("<span class=\"underdog\">", string.Empty);
            Template.betTeam = Template.betTeam.Replace("<span style='color:#555555'>", string.Empty);
            Template.betTeam = Template.betTeam.Replace("</span>", string.Empty);
        }

        /// <summary>
        /// Calculates the F g_ L g_ excel.
        /// </summary>
        /// <param name="betStatus">The bet status.</param>
        /// <param name="homeFGLG">The home FGLG.</param>
        /// <param name="awayFGLG">The away FGLG.</param>
        protected virtual void CalculateFG_LG_Excel(string betStatus, ref string homeFGLG, ref string awayFGLG)
        {
            if (betStatus == null)
            {
                return;
            }

            // Format: FL:1-2
            string[] args = betStatus.Split(new char[] { ':', '-' });

            if (args.Length != 3)
            {
                return;
            }

            if (args[1] == "1")
            {
                homeFGLG += "(F)";
            }
            else if (args[1] == "2")
            {
                awayFGLG += "(F)";
            }

            if (args[2] == "1")
            {
                homeFGLG += "(L)";
            }
            else if (args[2] == "2")
            {
                awayFGLG += "(L)";
            }
        }

        /// <summary>
        /// Builds the score map.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="isShowScoreMap">If set to <c>true</c> [is show score map].</param>
        private void BuildScoreMap(ITicket ticket, bool isShowScoreMap)
        {
            if (isShowScoreMap)
            {
                this.Template.ScoreMapIcon.matchId = ticket.MatchId.ToString();
                this.Template.ScoreMapIcon.betTypeId = ticket.BetTypeId.ToString();
                this.Template.ScoreMapIcon.custId = ticket.CustId.ToString();
                this.Template.ScoreMapIcon.liveindicator = Convert.ToInt32(ticket.IsLive).ToString();
            }
            else
            {
                this.Template.ScoreMapIcon = null;
            }
        }

        /// <summary>
        /// Builds the handicap odds.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        private void BuildHandicapOdds(ITicket ticket)
        {
            object oddsType = new object();

            if (string.IsNullOrEmpty(ticket.OddsType))
            {
                oddsType = 0;
            }
            else
            {
                oddsType = ticket.OddsType;
            }

            string odds = ConvertByBetType.Odds(ticket.Odds, ticket.BetTypeId, oddsType);

            this.Template.odds = odds;
        }
    }
}