// --------------------------------------------------------------------------
// <copyright file="ForecastController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.1">Created</change>
//   <change who="Anders.Le" date="2013.4.9">Change the way of AgentId getting</change>
//   <change who="Ricky.Nguyen" date="2013.4.9">Update Actions</change>
// </history>

namespace Apollo.Age.Forecast.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Forecast.Services;
    using Apollo.Age.Web;
    using Apollo.Age.Forecast.Models;
    using Apollo.Age.Criterias;
    using System.Linq;
    using Apollo.Age.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ForecastController : BaseController
    {
        private readonly ForecastService _forecastService;

        public ForecastController()
            : this(null)
        {
        }

        public ForecastController(ForecastService forecastService = null)
        {
            _forecastService = forecastService ?? new ForecastService();
        }

        #region Forecast 1X2

        private int GetCurrentAgentId(int mainShareHolderId)
        {
            int agentId = base.CurrentSession.AgentId;
            if (base.CurrentSession.IsSuper)
            {
                var mainShareHolders = _forecastService.GetData<MainShareHolderModel>(new GetAllMainShareHolderCriteria());
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
            ViewBag.CurrentAgentId = base.CurrentSession.AgentId;

            return agentId;
        }

        /// <summary>
        /// Get Running LeagueModel, with BetType
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public ViewResult FC1X2(int? matchId, int betType = (int)BetTypes.FT_1X2, string leagueName = "", int leagueId = 0, int mainShareHolderId = 0)
        {
            var currentAgentId = GetCurrentAgentId(mainShareHolderId);
            IEnumerable<LeagueModel> leagues = _forecastService.GetRunningLeague(betType, currentAgentId);
            ViewBag.Leagues = leagues;
            ViewBag.MatchId = matchId;
            ViewBag.BetType = betType;
            ViewBag.LeagueName = leagueName;
            ViewBag.LeagueId = leagueId;
            return View();
        }

        /// <summary>
        /// Get Forecast 1x2 Matches
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public ActionResult FC1X2Matches(int? leagueId, int? matchId, int betType = (int)BetTypes.FT_1X2, int mainShareHolderId = 0)
        {
            IEnumerable<Forecast1X2Model> forecasts = null;
            var currentAgentId = mainShareHolderId > 0 ? mainShareHolderId : this.CurrentSession.AgentId;
            if (matchId != null && matchId.Value > 0)
            {
                forecasts = _forecastService.GetForecast1X2Matches(currentAgentId, matchId.Value, betType);
            }
            else
            {
                forecasts = _forecastService.GetForecast1x2(currentAgentId, leagueId.Value);
            }
            return View(forecasts);
        }

        #endregion Forecast 1X2

        #region Forecast Handicap, Over/Under

        /// <summary>
        /// Handicap OU Forecast Page
        /// </summary>
        /// <returns></returns>
        public ViewResult FCHandicapOverUnder(string leagueName = "", int leagueId = 0, int mainShareHolderId = 0)
        {
            var currentAgentId = GetCurrentAgentId(mainShareHolderId);
            IEnumerable<LeagueModel> leagues = _forecastService.GetRunningLeague(betType: (int)BetTypes.Handicap, agentId: currentAgentId);
            ViewBag.LeagueId = leagueId;
            ViewBag.Leagues = leagues;
            ViewBag.LeagueName = leagueName;
            return View();
        }

        /// <summary>
        /// Get Handicap OU Matches
        /// </summary>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public ActionResult FCHandicapOverUnderMatches(int leagueId, int mainShareHolderId = 0)
        {
            var currentAgentId = mainShareHolderId > 0 ? mainShareHolderId : this.CurrentSession.AgentId;
            var forecasts = _forecastService.GetForecastHandicapOverUnder(currentAgentId, leagueId);
            return View(forecasts);
        }

        #endregion Forecast Handicap, Over/Under

        #region Forecast By Matches

        /// <summary>
        /// Forecast by Match Content Page
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="currentScore"></param>
        /// <returns></returns>
        public ViewResult ForecastByMatchContent(int matchId, int betType = (int)BetTypes.Handicap, int currentScore = 0, int mainShareHolderId = 0)
        {
            ViewBag.TopOddsByMatch = _forecastService.GetTopOddsByMatch(matchId, betType);
            ViewBag.BetType = betType;
            ViewBag.MatchId = matchId;
            ViewBag.CurrentScore = currentScore;
            var currentAgentId = mainShareHolderId > 0 ? mainShareHolderId : this.CurrentSession.AgentId;
            return View(_forecastService.GetForecastMatcheValue(currentAgentId, matchId, betType, currentScore));
        }

        /// <summary>
        /// Forecast By Match Page
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="currentScore"></param>
        /// <returns></returns>
        public ViewResult ForecastByMatch(int matchId, int betType = (int)BetTypes.Handicap, int currentScore = 0, string leagueName = "", int mainShareHolderId = 0)
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
        /// </summary>
        /// <param name="matchIds"></param>
        /// <param name="liveBetTypes"></param>
        /// <param name="deadBallBetTypes"></param>
        /// <returns></returns>
        public ViewResult ScoreMap(string matchIds = "", int liveBetTypes = 0, int deadBallBetTypes = 0)
        {
            if (!string.IsNullOrEmpty(matchIds))
            {
                ViewBag.MatchIds = matchIds;
                ViewBag.LiveBetTypes = _forecastService.GetAllBetTypeFromBetType(liveBetTypes, true);
                ViewBag.DeadBallBetTypes = _forecastService.GetAllBetTypeFromBetType(deadBallBetTypes, false);
            }
            else
            {
                ViewBag.Leagues = _forecastService.GetScoreMapLeagues(this.CurrentSession.AgentId);
            }
            return View();
        }

        /// <summary>
        /// Match of LeagueModel
        /// </summary>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public ViewResult ScoreMapMatch(int leagueId)
        {
            var matches = _forecastService.GetScoreMapMatches(this.CurrentSession.AgentId, leagueId);
            return View(matches);
        }

        /// <summary>
        /// BetType List
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
        /// Load LeagueModel
        /// </summary>
        /// <param name="currentLeagueId"></param>
        /// <returns></returns>
        public ViewResult ScoreMapLeagues(int currentLeagueId = 0)
        {
            ViewBag.CurrentLeagueId = currentLeagueId;
            return View(_forecastService.GetScoreMapLeagues(this.CurrentSession.AgentId));
        }

        /// <summary>
        /// Load Score Map Result
        /// </summary>
        /// <param name="matchIds"></param>
        /// <param name="liveBetTypes"></param>
        /// <param name="deadBallBetTypes"></param>
        /// <returns></returns>
        [OutputCache(Duration=0)]
        public ViewResult ScoreMapResult(string matchIds, string liveBetTypes, string deadBallBetTypes)
        {
            return View(_forecastService.GetScoreMapResults(this.CurrentSession.AgentId, matchIds, liveBetTypes, deadBallBetTypes));
        }

        #endregion Score Map
    }
}