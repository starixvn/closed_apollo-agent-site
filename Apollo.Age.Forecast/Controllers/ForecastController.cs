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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Web;
    using System.Web.Mvc;
    using Apollo.Age.Forecast.Services;
    using Apollo.Age.Forecast.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ForecastController : BaseController
    {
        private readonly ForecastService _foreCastService;

        public ForecastController() : this(null)
        {
        }

        public ForecastController(ForecastService foreCastService = null)
        {
            _foreCastService = foreCastService ?? new ForecastService();
        }

        #region FC1X2

        public ViewResult FC1X2()
        {
            IEnumerable<LeagueViewModel> leagues = _foreCastService.GetRunningLeague(betType: 5,agentId: base.AgentId);
            ViewBag.Leagues = leagues;
            return View();
        }

        public ActionResult FC1X2Matches(int leagueId)
        {
            var forecasts = _foreCastService.GetForecast1x2(base.AgentId, leagueId);
            return View(forecasts);
        }

        #endregion

        #region Handicap, Over/Under

        public ViewResult FCHandicapOverUnder()
        {
            // 1: for Hdp/OU/Live
            IEnumerable<LeagueViewModel> leagues = _foreCastService.GetRunningLeague(betType: 1, agentId: base.AgentId);
            ViewBag.Leagues = leagues;
            return View();
        }

        public ActionResult FCHandicapOverUnderMatches(int leagueId)
        {
            var forecasts = _foreCastService.GetForecastHandicapOverUnder(base.AgentId, leagueId);
            return View(forecasts);
        }

        #endregion
    }
}
