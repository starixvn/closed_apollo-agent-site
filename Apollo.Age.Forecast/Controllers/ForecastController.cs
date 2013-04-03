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

        public ViewResult FC1X2()
        {
            IEnumerable<LeagueViewModel> leagues = _foreCastService.GetRunningLeague(betType: 5,agentId: base.AgentId);
            ViewBag.InitLeague = leagues.Any() ? leagues.ElementAt(0).LeagueId : 0;
            return View(leagues);
        }

        public ActionResult FC1X2Matches(int leagueId)
        {
            var forecasts = _foreCastService.GetForecast1x2(base.AgentId, leagueId);
            return View(forecasts);
        }

    }
}
