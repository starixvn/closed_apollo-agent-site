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
    using Apollo.Age.Wagers.ViewModels.Soccer;
    using Apollo.Age.Wagers.Services;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SoccerController : WagerController
    {       

        public SoccerController() : base(null) { }

     
        public ActionResult Straight(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            fromDate = new DateTime(2013, 05, 23);
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                GroupBetName = "Straight"
            };

            var result = _wagerService.GetWagerData<WagersStraightViewModel>(criteria);

            return View(result);
        }

        public ActionResult RunningBall(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            fromDate = new DateTime(2013, 05, 23);
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                GroupBetName = "RunningBall"
            };

            var result = _wagerService.GetWagerData<WagersRunningBallViewModel>(criteria);

            return View(result);
        }

        public ActionResult Correctscore(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            var criteria = new CorrectScoreCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                Bettype = 4
            };

            var result = _wagerService.GetWagerData<WagersCorrectScoreViewModel>(criteria);

            return View(result);
        }

        public ActionResult FirstHalfCorrectScore(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            return Correctscore(fromDate, uGroup, isDirectMember, leagueId);
        }

        public ActionResult Total(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                GroupBetName = "Total"

            };
            var result = _wagerService.GetWagerData<WagersTotalViewModel>(criteria);

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

        public ActionResult SpecialBet(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                GroupBetName = "SpecialBet"

            };
            var result = _wagerService.GetWagerData<WagersSpecialViewModel>(criteria);

            return View(result);
        }

        public ActionResult Start()
        {
            return View();
        }
    }
}