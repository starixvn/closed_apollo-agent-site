// --------------------------------------------------------------------------
// <copyright file="ForecastController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.1">Created</change>
//   <change who="Anders.Le" date="2013.4.9">Change the way of AgentId getting</change>
//   <change who="Ricky.Nguyen" date="2013.4.9">Update Actions</change>
//   <change who="Ricky.Nguyen" date="2013.4.29">Update Super View</change>
// </history>

namespace Apollo.Age.Forecast.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Apollo.Age.Core;
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Forecast.Criterias;
    using Apollo.Age.Forecast.Models;
    using Apollo.Age.Forecast.Services;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Web.Attribute;

    /// <summary>
    /// Forecast Function controller.
    /// </summary>
    [ApolloAuth(FunctionKey = ApolloFunctionKeys.Forecast, Policy = Policy.Query)]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ForecastController : BaseController
    {
        private readonly ForecastService _forecastService;

        private readonly IUserService _userService;

        #region Ctors

        public ForecastController()
            : this(null)
        {
        }

        public ForecastController(ForecastService forecastService = null, IUserService userService = null)
        {
            _forecastService = forecastService ?? new ForecastService();
            _userService = userService ?? new UserService();

            ViewBag.IsShowRefreshNow = CurrentSession.AgentLevel <= 1;
        }

        #endregion Ctors

        #region Forecast 1X2

        /// <summary>
        /// Get Running LeagueModel, with BetType
        /// if MatchId != null, that means forecast be shown in TotalBets.
        /// </summary>
        /// <param name="matchId">The match ID.</param>
        /// <param name="betType">The bet type.</param>
        /// <returns></returns>
        public ViewResult FC1X2(
            int? matchId, 
            int betType = (int)BetTypes.FT_1X2, 
            string leagueName = "", 
            int leagueId = 0, 
            int mainShareHolderId = 0)
        {
            var currentAgentId = GetCurrentAgentId(mainShareHolderId, ((int)BetTypes.FT_1X2).ToString());
            IEnumerable<LeagueModel> leagues = _forecastService.GetRunningLeague(betType, currentAgentId);
            ViewBag.Leagues = leagues;
            ViewBag.MatchId = matchId;
            ViewBag.BetType = betType;
            ViewBag.LeagueName = leagueName;
            ViewBag.LeagueId = leagueId;
            if (matchId == null)
            {
                if (leagueId == 0 && leagues != null && leagues.Any())
                {
                    leagueId = leagues.ElementAt(0).LeagueId;
                }

                ViewBag.FC1X2List = _forecastService.GetForecast1x2(currentAgentId, this.CurrentSession.AgentId, leagueId);
            }

            return View();
        }

        /// <summary>
        /// Get Forecast 1x2 Matches.
        /// </summary>
        /// <param name="leagueId">The league ID.</param>
        /// <param name="matchId">The match ID.</param>
        /// <param name="betType">The bet type.</param>
        /// <returns></returns>
        public ActionResult FC1X2Matches(
            int? leagueId, 
            int? matchId, 
            int betType = (int)BetTypes.FT_1X2, 
            int mainShareHolderId = 0)
        {
            IEnumerable<Forecast1X2Model> forecasts = null;
            var currentAgentId = mainShareHolderId > 0 ? mainShareHolderId : this.CurrentSession.AgentId;
            if (matchId != null && matchId.Value > 0)
            {
                forecasts = _forecastService.GetForecast1X2Matches(currentAgentId, this.CurrentSession.AgentId, matchId.Value, betType);
            }
            else
            {
                forecasts = _forecastService.GetForecast1x2(currentAgentId, this.CurrentSession.AgentId, leagueId.Value);
            }

            return View(forecasts);
        }

        #endregion Forecast 1X2

        #region Forecast Handicap, Over/Under

        /// <summary>
        /// Handicap OU Forecast Page.
        /// </summary>
        /// <returns></returns>
        public ViewResult FCHandicapOverUnder(string leagueName = "", int leagueId = 0, int mainShareHolderId = 0)
        {
            var currentAgentId = GetCurrentAgentId(mainShareHolderId, "1,3,7,8");
            IEnumerable<LeagueModel> leagues = _forecastService.GetRunningLeague(betType: (int)BetTypes.Handicap, agentId: currentAgentId);
            ViewBag.LeagueId = leagueId;
            ViewBag.Leagues = leagues;
            ViewBag.LeagueName = leagueName;
            if (leagueId == 0 && leagues != null && leagues.Any())
            {
                leagueId = leagues.ElementAt(0).LeagueId;
            }

            ViewBag.FCHDPList = _forecastService.GetForecastHandicapOverUnder(currentAgentId, this.CurrentSession.AgentId, leagueId);
            return View();
        }

        /// <summary>
        /// Get Handicap OU Matches.
        /// </summary>
        /// <param name="leagueId">The league ID.</param>
        /// <returns></returns>
        public ActionResult FCHandicapOverUnderMatches(int leagueId, int mainShareHolderId = 0)
        {
            var currentAgentId = mainShareHolderId > 0 ? mainShareHolderId : this.CurrentSession.AgentId;
            var forecasts = _forecastService.GetForecastHandicapOverUnder(currentAgentId, this.CurrentSession.AgentId, leagueId);
            return View(forecasts);
        }

        #endregion Forecast Handicap, Over/Under

        #region Forecast By Matches

        /// <summary>
        /// Forecast by Match Content Page.
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="betType">The bet type.</param>
        /// <param name="currentScore">Current score.</param>
        /// <returns></returns>
        public ViewResult ForecastByMatchContent(
            int matchId, 
            int betType = (int)BetTypes.Handicap, 
            int currentScore = 0, 
            int mainShareHolderId = 0)
        {
            ViewBag.TopOddsByMatch = _forecastService.GetTopOddsByMatch(matchId, betType);
            ViewBag.BetType = betType;
            ViewBag.MatchId = matchId;
            ViewBag.CurrentScore = currentScore;

            var currentAgentId = mainShareHolderId > 0 ? mainShareHolderId : this.CurrentSession.AgentId;
            return View(_forecastService.GetForecastMatcheValue(currentAgentId, this.CurrentSession.AgentId, matchId, betType, currentScore));
        }

        /// <summary>
        /// Forecast By Match Page.
        /// </summary>
        /// <param name="matchId">The match ID.</param>
        /// <param name="betType">The bet type.</param>
        /// <param name="currentScore">Current score.</param>
        /// <returns></returns>
        public ViewResult ForecastByMatch(
            int matchId, 
            int betType = (int)BetTypes.Handicap, 
            int currentScore = 0, 
            string leagueName = "", 
            int mainShareHolderId = 0)
        {
            ViewBag.BetType = betType;
            ViewBag.MatchId = matchId;
            ViewBag.CurrentScore = currentScore;
            ViewBag.LeagueName = leagueName;
            ViewBag.CurrentMainShareHolderId = mainShareHolderId;
            return View();
        }

        #endregion Forecast By Matches

        #region Score Map

        /// <summary>
        /// Score Map Page, if MatchIds valid, then Render Score Map of these match
        /// If matchIds != empty, It means that ScoreMap will show in bet list.
        /// </summary>
        /// <param name="matchIds"></param>
        /// <param name="liveBetTypes"></param>
        /// <param name="deadBallBetTypes"></param>
        /// <returns></returns>
        public ViewResult ScoreMap(
            string matchIds = "", 
            int liveBetTypes = 0, 
            int deadBallBetTypes = 0, 
            int mainShareHolderId = 0, 
            int custId = 0)
        {
            if (!string.IsNullOrEmpty(matchIds))
            {
                ViewBag.MatchIds = matchIds;
                ViewBag.LiveBetTypes = _forecastService.GetAllBetTypeFromBetType(liveBetTypes, true);
                ViewBag.DeadBallBetTypes = _forecastService.GetAllBetTypeFromBetType(deadBallBetTypes, false);

                if (this.CurrentSession.IsSuper && mainShareHolderId == 0 && custId != 0)
                {
                    mainShareHolderId = _forecastService.GetMainShareHolderByMember(
                        new GetMainShareHolderByMemberCriteria
                        {
                            CustId = custId
                        });
                }

                ViewBag.CurrentMainShareHolderId = mainShareHolderId;
            }
            else
            {
                var currentAgentId = GetCurrentAgentId(mainShareHolderId);
                ViewBag.Leagues = _forecastService.GetScoreMapLeagues(currentAgentId);
            }

            return View();
        }

        /// <summary>
        /// Match of LeagueModel.
        /// </summary>
        /// <param name="leagueId">The league ID.</param>
        /// <returns></returns>
        public ViewResult ScoreMapMatch(int leagueId, int mainShareHolderId = 0)
        {
            var currentAgentId = mainShareHolderId > 0 ? mainShareHolderId : this.CurrentSession.AgentId;
            var matches = _forecastService.GetScoreMapMatches(currentAgentId, leagueId);
            return View(matches);
        }

        /// <summary>
        /// BetType List.
        /// </summary>
        /// <param name="liveBetType"></param>
        /// <param name="deadBetType"></param>
        /// <returns></returns>
        public ViewResult ScoreMapBetType(int liveBetType = 0, int deadBetType = 0)
        {
            ViewBag.LiveBetType = liveBetType;
            ViewBag.DeadBallBetType = deadBetType;
            return View(_forecastService.GetScoreMapBetType());
        }

        /// <summary>
        /// Load LeagueModel.
        /// </summary>
        /// <param name="currentLeagueId"></param>
        /// <returns></returns>
        public ViewResult ScoreMapLeagues(int currentLeagueId = 0, int mainShareHolderId = 0)
        {
            ViewBag.CurrentLeagueId = currentLeagueId;
            var currentAgentId = mainShareHolderId > 0 ? mainShareHolderId : this.CurrentSession.AgentId;
            return View(_forecastService.GetScoreMapLeagues(currentAgentId));
        }

        /// <summary>
        /// Load Score Map Result.
        /// </summary>
        /// <param name="matchIds"></param>
        /// <param name="liveBetTypes"></param>
        /// <param name="deadBallBetTypes"></param>
        /// <returns></returns>
        public ViewResult ScoreMapResult(
            string matchIds,
            string liveBetTypes,
            string deadBallBetTypes,
            int mainShareHolderId = 0)
        {
            int currentAgentId = mainShareHolderId > 0 ? mainShareHolderId : this.CurrentSession.AgentId;
            var scoreMapResult = _forecastService.GetScoreMapResults(
                currentAgentId,
                this.CurrentSession.AgentId,
                matchIds,
                liveBetTypes,
                deadBallBetTypes);

            return View(scoreMapResult);
        }

        #endregion Score Map

        #region Helpers

        /// <summary>
        /// Get Current Agent Id
        /// If MainShareholderId > 0 and Current Login user is Super then set first is selected.
        /// </summary>
        /// <param name="mainShareHolderId"></param>
        /// <returns></returns>
        private int GetCurrentAgentId(int mainShareHolderId, string betType = null)
        {
            int agentId = this.CurrentSession.AgentId;
            if (this.CurrentSession.IsSuper)
            {
                //// Should Use Cache, fix later
                var mainShareHolders = _forecastService.GetData<Agent>(new GetAllMainShareHolderCriteria
                {
                    Bettypes = betType
                });

                if (mainShareHolderId > 0)
                {
                    agentId = mainShareHolderId;
                }
                else if (mainShareHolders != null && mainShareHolders.Any())
                {
                    agentId = mainShareHolderId = mainShareHolders.ElementAt(0).Id;
                }

                ViewBag.CurrentMainShareHolderId = mainShareHolderId;
                ViewBag.MainShareHolders = mainShareHolders;
            }

            ViewBag.CurrentAgentId = this.CurrentSession.AgentId;

            return agentId;
        }

        #endregion Helpers
    }
}