// --------------------------------------------------------------------------
// <copyright file="SoccerController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.6.25">Created</change>
//   <change who="Edward.Nguyen" date="2013.6.27">Implement prototype</change>
// </history>

namespace Apollo.Age.Wagers.Controllers
{
    using System;
    using System.Web.Mvc;
    using Apollo.Age.Core;
    
    using Apollo.Age.WebApp;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Wagers.Criterias.Soccer;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SoccerController : BaseController
    {
        private readonly QueryService _soccerServices;

        public SoccerController() : this(null) { }

        public SoccerController(QueryService soccerService)
        {
            _soccerServices = soccerService ?? new QueryService();
        }

        public ActionResult Index(string groupBetName = "")
        {
            this.ActionName = string.IsNullOrWhiteSpace(groupBetName) ? this.ActionName : groupBetName;
            return this.BaseIndex(SportGroup.Soccer);
        }

        public ActionResult Straight(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            var criteria = new WagersStraightCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = 123,
                UGroup = uGroup
            };

            var result = _soccerServices.GetData<WagersStraightViewModel>(criteria);

            return View(result);
        }

        public ActionResult RunningBall(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            var criteria = new WagersRunningBallCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = 123,
                UGroup = uGroup
            };

            var result = _soccerServices.GetData<WagersRunningBallViewModel>(criteria);

            return View(result);
        }

        public ActionResult Correctscore(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            var criteria = new WagersCorrectScoreCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = 123,
                UGroup = uGroup,
                Bettype = 4
            };

            var result = _soccerServices.GetData<WagersCorrectScoreViewModel>(criteria);

            return View(result);
        }

        public ActionResult FirstHalfCorrectScore(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            return Correctscore(fromDate, uGroup, isDirectMember, leagueId);
        }

        public ActionResult Total(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            var criteria = new WagersTotalCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = 123,
                UGroup = uGroup
               
            };
            var result = _soccerServices.GetData<WagersTotalViewModel>(criteria);

            return View(result);
        }

        public ActionResult HalfFull(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            return Correctscore(fromDate, uGroup, isDirectMember, leagueId);
        }

        public ActionResult Parlay()
        {
            return View();
        }

        public ActionResult Start()
        {
            return View();
        }
    }
}