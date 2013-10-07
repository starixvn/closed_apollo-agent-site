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

namespace Fanex.BetList.Core.Builder.ChoiceBuilder
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
    using Fanex.BetList.Core.Utils;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Handicap
    /// </summary>
    public class Choice1 : IChoice
    {
        #region Attributes

        protected const string BL_UNDER_DOG = "underdog";
        protected const string BL_FAVORITE = "favorite";
        private static int[] _showHomeCharacterList = new[] { 
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
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="isShowScoreMap">if set to <c>true</c> [is show score map].</param>
        /// <returns>Choice_Template.</returns>
        public virtual Choice_Template Render(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, bool isShowScoreMap, BetListPage betListPage)
        {
            this.BuildChoice(ticket, ticketHelper, ticketData, betListPage);
            this.BuildHandicapOdds(ticket, ticketHelper);
            this.BuildScoreMap(ticket, isShowScoreMap);

            return this.Template;
        }

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="isShowScoreMap">if set to <c>true</c> [is show score map].</param>
        /// <returns>Choice_Template.</returns>
        public virtual List<IRichTextString> RenderRTF(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, bool isShowScoreMap, RTFHelper rtfHelper)
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
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        protected virtual void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, BetListPage betListPage)
        {
            //this.Template.Score.homeScore = ticket.LiveHomeScore.ToString();
            //this.Template.Score.awayScore = ticket.LiveAwayScore.ToString();

            //switch (betListPage)
            //{
            //    case BetListPage.Wager:
            //    case BetListPage.Forecast:
            //        this.Template.LeagueName.Hide();
            //        break;
            //    case BetListPage.Outstanding:
            //    case BetListPage.AccountInfo:
            //    case BetListPage.Default:
            //    case BetListPage.Cancel:
            //    case BetListPage.Report:
            //    case BetListPage.Transaction:
            //        this.Template.LeagueName.leagueName = ticketHelper.GetLeagueNameById(ticket.LeagueId);
            //        break;
            //}
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

            //this.Template.betTypeName = ticketHelper.GetBetTypeNameById(ticket.BetTypeId);
            this.Template.homeTeam = string.Join(
                                                    null,
                                                    new string[]
                                                    {
                                                        homeName,
                                                        ticket.IsNeutral ? 
                                                            " (N) " 
                                                            : (_showHomeCharacterList.Contains(ticket.SportTypeId) ? " (H)" : string.Empty)
                                                    }
                                                );

            this.Template.awayTeam = awayName;
            //this.Template.sportTypeName = ticketHelper.GetSportNameById(ticket.SportTypeId);

            //this.Template.home_firstGoal_lastGoal = string.Empty;
            //this.Template.away_firstGoal_lastGoal = string.Empty;

            //this.CalculateFG_LG(ticket.BetStatus, ref this.Template.home_firstGoal_lastGoal, ref this.Template.away_firstGoal_lastGoal);

            bool betHomeTeam = ticket.BetTeam != null && ticket.BetTeam.Contains("h");

            this.Template.betTeam = betHomeTeam ? this.Template.homeTeam : awayName;

            //this.Template.betTeamClassName = betHomeTeam ? (ticket.Handicap2 >= ticket.Handicap1 ? BL_UNDER_DOG : BL_FAVORITE) : (ticket.Handicap2 <= ticket.Handicap1 ? BL_UNDER_DOG : BL_FAVORITE);

            //this.Template.Score.scoreClassName = BL_FAVORITE;
            //bool isFavorite = betHomeTeam ? (ticket.Handicap2 <= ticket.Handicap1) : (ticket.Handicap2 >= ticket.Handicap1);
            decimal handicap = ticket.Handicap2 > ticket.Handicap1 ? ticket.Handicap2 : -ticket.Handicap1;
            this.Template.handicap = Formatter.FormatHandicap(ticket.BetTypeId, handicap);
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
        /// <returns></returns>
        protected virtual IRichTextString BuildRTFChoice(ITicket ticket, RTFHelper rtfHelper, ITicketHelper ticketHelper)
        {
            //bool isMixParlay = false;

            // the 1st line
            this.AdjustBetTeamToRTF();
            string choice = Template.betTeam;

            //if (Template.Handicap != null && Template.Handicap.Visible && Template.Handicap.handicap != null)
            //{
            //    choice += string.Join(null, new string[] { " ", Template.Handicap.handicap });
            //}

            //if (Template.Score != null && Template.Score.Visible)
            //{
            //    choice += string.Join(null, new string[] { " [", Template.Score.homeScore, "-", Template.Score.awayScore, "]" });
            //}

            //if (isMixParlay)
            //{
            //    choice += string.Join(null, new string[] { " @ ", Formatter.FormatNumber3(ticket.Odds, null) });
            //}

            //if (!string.IsNullOrEmpty(choice))
            //{
            //    choice = choice.Replace("&nbsp;", " ");
            //    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.PosFont);
            //    choice = string.Empty;
            //}

            //// the 2nd line
            //if (!string.IsNullOrEmpty(Template.betTypeName))
            //{
            //    choice += Template.betTypeName;
            //}

            //if (!string.IsNullOrEmpty(choice))
            //{
            //    choice = choice.Replace("&nbsp;", " ");
            //    rtfHelper.RTFRenderer.AddText("\n" + choice);
            //    choice = string.Empty;
            //}

            //// the 3rd line
            //bool isAddNewLine = false;

            //if (!string.IsNullOrEmpty(Template.homeTeam))
            //{
            //    choice += Template.homeTeam;
            //}

            //if (!string.IsNullOrEmpty(choice))
            //{
            //    isAddNewLine = true;
            //    choice = choice.Replace("&nbsp;", " ");
            //    rtfHelper.RTFRenderer.AddText("\n" + choice);
            //}

            //if (ticket.EventStatus != null)
            //{
            //    Template.home_firstGoal_lastGoal = string.Empty;
            //    Template.away_firstGoal_lastGoal = string.Empty;
            //    this.CalculateFG_LG_Excel(ticket.EventStatus, ref Template.home_firstGoal_lastGoal, ref Template.away_firstGoal_lastGoal);
            //}

            //choice = string.Empty;

            //if (!string.IsNullOrEmpty(Template.home_firstGoal_lastGoal))
            //{
            //    choice = string.Join(null, new string[] { " " + Template.home_firstGoal_lastGoal });
            //}

            //if (!string.IsNullOrEmpty(choice))
            //{
            //    isAddNewLine = true;
            //    choice = choice.Replace("&nbsp;", " ");
            //    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.PosFont);
            //}

            //choice = string.Empty;
            //if (Template.VS != null && Template.VS.Visible)
            //{
            //    choice = "- vs -";
            //}

            //if (!string.IsNullOrEmpty(Template.awayTeam))
            //{
            //    choice += string.Join(null, new string[] { " ", Template.awayTeam }); ;
            //}

            //if (!string.IsNullOrEmpty(choice))
            //{
            //    isAddNewLine = true;
            //    choice = choice.Replace("&nbsp;", " ");
            //    rtfHelper.RTFRenderer.AddText(choice);
            //}

            //choice = string.Empty;
            //if (!string.IsNullOrEmpty(Template.away_firstGoal_lastGoal))
            //{
            //    choice = " " + Template.away_firstGoal_lastGoal;
            //}

            //if (!string.IsNullOrEmpty(choice) || isAddNewLine)
            //{
            //    choice = choice.Replace("&nbsp;", " ");
            //    rtfHelper.RTFRenderer.AddText(choice, rtfHelper.PosFont);
            //    choice = string.Empty;
            //}

            //// the 4th line
            //if (!string.IsNullOrEmpty(Template.sportTypeName))
            //{
            //    choice += Template.sportTypeName;
            //}

            //if (Template.LeagueName != null && !string.IsNullOrEmpty(Template.LeagueName.leagueName))
            //{
            //    if (!string.IsNullOrEmpty(choice))
            //    {
            //        choice += " ";
            //    }

            //    choice += Template.LeagueName.leagueName;
            //}

            //if (!string.IsNullOrEmpty(choice))
            //{
            //    choice = choice.Replace("&nbsp;", " ");
            //    rtfHelper.RTFRenderer.AddText("\n" + choice);
            //}

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
        /// <param name="isShowScoreMap">if set to <c>true</c> [is show score map].</param>
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
        /// <param name="ticket">The ticket.</param>
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