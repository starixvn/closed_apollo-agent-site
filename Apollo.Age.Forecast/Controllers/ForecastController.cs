// --------------------------------------------------------------------------
// <copyright file="ForecastController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.1">Created</change>
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

        #region FC1X2

        public ViewResult FC1X2(int? matchId, int betType = (int)BetTypes.FT_1X2)
        {
            IEnumerable<LeagueViewModel> leagues = _forecastService.GetRunningLeague(betType, agentId: base.AgentId);
            ViewBag.Leagues = leagues;
            ViewBag.MatchId = matchId;
            ViewBag.BetType = betType;
            return View();
        }

        public ActionResult FC1X2Matches(int? leagueId, int? matchId, int betType = (int)BetTypes.FT_1X2 )
        {
            IEnumerable<Forecast1X2ViewModel> forecasts = null;
            if (matchId != null && matchId.Value > 0)
            {
                forecasts = _forecastService.GetForecast1X2Matches(base.AgentId, matchId.Value, betType);
            }
            else
            {
                forecasts = _forecastService.GetForecast1x2(base.AgentId, leagueId.Value);
            }
            return View(forecasts);
        }

        #endregion FC1X2

        #region Handicap, Over/Under

        public ViewResult FCHandicapOverUnder()
        {
            // 1: for Hdp/OU/Live
            IEnumerable<LeagueViewModel> leagues = _forecastService.GetRunningLeague(betType: (int)BetTypes.Handicap, agentId: base.AgentId);
            ViewBag.Leagues = leagues;
            return View();
        }

        public ActionResult FCHandicapOverUnderMatches(int leagueId)
        {
            var forecasts = _forecastService.GetForecastHandicapOverUnder(base.AgentId, leagueId);
            return View(forecasts);
        }

        #endregion Handicap, Over/Under

        public ViewResult ForecastByMatchContent(int matchId, int betType = (int)BetTypes.Handicap, int currentScore = 0)
        {
            ViewBag.TopOddsByMatch = _forecastService.GetTopOddsByMatch(matchId, betType);
            ViewBag.BetType = betType;
            ViewBag.MatchId = matchId;
            ViewBag.CurrentScore = currentScore;
            return View(_forecastService.GetForecastMatcheValue(base.AgentId, matchId, betType, currentScore));
        }

        public ViewResult ForecastByMatch(int matchId, int betType = (int)BetTypes.Handicap, int currentScore = 0)
        {
            ViewBag.BetType = betType;
            ViewBag.MatchId = matchId;
            ViewBag.CurrentScore = currentScore;
            return View();
        }

        #region Score Map
        public ViewResult ScoreMapMatch(int leagueId)
        {
            return View(_forecastService.GetScoreMapMatches(base.AgentId, leagueId));
        }

        public ViewResult ScoreMapBetType()
        {
            return View(_forecastService.GetScoreMapBetType());
        }

        public ViewResult ScoreMapResult(string matchIds, string liveBetTypes, string deadBallBetTypes)
        {
            return View(_forecastService.GetScoreMapResults(base.AgentId, matchIds, liveBetTypes, deadBallBetTypes));
        }
        #endregion
    }
}