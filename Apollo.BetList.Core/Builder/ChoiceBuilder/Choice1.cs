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
        /// <param name="betListPage">The bet list page.</param>
        /// <returns>
        /// Choice Template.
        /// </returns>
        public virtual Choice_Template Render(
            ITicket ticket,
            ITicketHelper ticketHelper,
            List<ITicketData> ticketData,
            bool isShowScoreMap,
            BetListPage betListPage)
        {
            this.BuildChoice(ticket, ticketHelper, ticketData, betListPage);
            this.BuildHandicapOdds(ticket, ticketHelper);
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
            this.Render(ticket, ticketHelper, ticketData, isShowScoreMap, BetListPage.Default);
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
            List<ITicket> ticketMixParlay,
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
        /// <param name="betListPage">The bet list page.</param>
        /// <returns>
        /// Choice Template.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        public Choice_Template Render(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, List<ITicket> ticketMixParlay, bool isShowScoreMap, BetListPage betListPage)
        {
            this.BuildChoice(ticket, ticketHelper, ticketData, betListPage);
            this.BuildHandicapOdds(ticket, ticketHelper);
            this.BuildScoreMap(ticket, isShowScoreMap);

            return this.Template;
        }

        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="betListPage">The bet list page.</param>
        protected virtual void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, BetListPage betListPage)
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
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        /// <param name="betListPage">The bet list page.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        protected virtual void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, List<ITicket> ticketMixParlay, BetListPage betListPage)
        {
            BuildChoice(ticket, ticketHelper, ticketData, betListPage);
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
            string choice = Template.betTeam;
            var choiceRtf = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return choiceRtf;
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
        /// <param name="ticketHelper">The ticket helper.</param>
        private void BuildHandicapOdds(ITicket ticket, ITicketHelper ticketHelper)
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