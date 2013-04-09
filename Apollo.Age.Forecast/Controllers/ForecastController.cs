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
    using System.Web.Mvc;
    using Apollo.Age.Forecast.Services;
    using Apollo.Age.Forecast.ViewModels;
    using Apollo.Age.Web;
    using Apollo.Age.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public ViewResult FC1X2(int? matchId, int betType = (int)BetTypes.FT_1X2)
        {
            IEnumerable<LeagueViewModel> leagues = _forecastService.GetRunningLeague(betType, agentId: this.CurrentSession.AgentId);
            ViewBag.Leagues = leagues;
            ViewBag.MatchId = matchId;
            ViewBag.BetType = betType;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public ActionResult FC1X2Matches(int? leagueId, int? matchId, int betType = (int)BetTypes.FT_1X2 )
        {
            IEnumerable<Forecast1X2ViewModel> forecasts = null;
            if (matchId != null && matchId.Value > 0)
            {
                forecasts = _forecastService.GetForecast1X2Matches( this.CurrentSession.AgentId, matchId.Value, betType);
            }
            else
            {
                forecasts = _forecastService.GetForecast1x2( this.CurrentSession.AgentId, leagueId.Value);
            }
            return View(forecasts);
        }

        #endregion FC1X2

        #region Forecast Handicap, Over/Under

        public ViewResult FCHandicapOverUnder()
        {
            // 1: for Hdp/OU/Live
            IEnumerable<LeagueViewModel> leagues = _forecastService.GetRunningLeague(betType: (int)BetTypes.Handicap, agentId: this.CurrentSession.AgentId);
            ViewBag.Leagues = leagues;
            return View();
        }

        public ActionResult FCHandicapOverUnderMatches(int leagueId)
        {
            var forecasts = _forecastService.GetForecastHandicapOverUnder(this.CurrentSession.AgentId, leagueId);
            return View(forecasts);
        }

        #endregion Handicap, Over/Under

        #region Forecast By Matches

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="currentScore"></param>
        /// <returns></returns>
        public ViewResult ForecastByMatchContent(int matchId, int betType = (int)BetTypes.Handicap, int currentScore = 0)
        {
            ViewBag.TopOddsByMatch = _forecastService.GetTopOddsByMatch(matchId, betType);
            ViewBag.BetType = betType;
            ViewBag.MatchId = matchId;
            ViewBag.CurrentScore = currentScore;
            return View(_forecastService.GetForecastMatcheValue( this.CurrentSession.AgentId, matchId, betType, currentScore));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="currentScore"></param>
        /// <returns></returns>
        public ViewResult ForecastByMatch(int matchId, int betType = (int)BetTypes.Handicap, int currentScore = 0)
        {
            ViewBag.BetType = betType;
            ViewBag.MatchId = matchId;
            ViewBag.CurrentScore = currentScore;
            return View();
        }

        #endregion

        #region Score Map

        public ViewResult ScoreMap(string matchIds = "", string liveBetTypes = "", string deadBallBetTypes = "")
        {
            if (!string.IsNullOrEmpty(matchIds))
            {
                ViewBag.MatchIds = matchIds;
                ViewBag.LiveBetTypes = liveBetTypes;
                ViewBag.DeadBallBetTypes = deadBallBetTypes;
            }
            else
            {
                ViewBag.Leagues = _forecastService.GetScoreMapLeagues( this.CurrentSession.AgentId);
            }
            return View();
        }

        public ViewResult ScoreMapMatch(int leagueId)
        {
            var matches = _forecastService.GetScoreMapMatches(this.CurrentSession.AgentId, leagueId);
            return View(matches);
        }

        public ViewResult ScoreMapBetType(int liveBetType = 0, int deadBetType = 0)
        {
            ViewBag.LiveBetType = liveBetType;
            ViewBag.DeadBallBetType = deadBetType;
            return View(_forecastService.GetScoreMapBetType());
        }

        public ViewResult ScoreMapLeagues(int currentLeagueId = 0)
        {
            ViewBag.CurrentLeagueId = currentLeagueId;
            return View(_forecastService.GetScoreMapLeagues(this.CurrentSession.AgentId));
        }

        public ViewResult ScoreMapResult(string matchIds, string liveBetTypes, string deadBallBetTypes)
        {
            return View(_forecastService.GetScoreMapResults(this.CurrentSession.AgentId, matchIds, liveBetTypes, deadBallBetTypes));
        }

        #endregion
    }
}