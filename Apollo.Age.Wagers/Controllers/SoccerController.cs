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

    
    using Apollo.Age.Services.Impl;    
    using Apollo.Age.Wagers.Services;
    using Apollo.Age.Wagers.ViewModels.Soccer;
    using Apollo.Age.Wagers.Criterias.Soccer;
    using Apollo.Age.Wagers.Criterias.CorrectScore;
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
                GroupBetName = this.CurrentAction
            };

            var result = _WagerService.GetWagerData<StraightViewModel>(criteria);

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
                GroupBetName = this.CurrentAction
            };

            var result = _WagerService.GetWagerData<RunningBallViewModel>(criteria);

            return View(result);
        }

        public ActionResult Correctscore(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            fromDate = new DateTime(2013, 05, 23);
            var criteria = new CorrectScoreCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                Bettype = 4,                
            };

            var result = _WagerService.GetWagerData<CorrectScoreViewModel>(criteria);

            return View(result);
        }

        public ActionResult FirstHalfCorrectScore(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            fromDate = new DateTime(2013, 05, 23);
            var criteria = new CorrectScoreCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                Bettype = 30
            };

            var result = _WagerService.GetWagerData<CorrectScoreViewModel>(criteria);

            return View(result);
        }

        public ActionResult Total(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            fromDate = new DateTime(2013, 05, 23);
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                GroupBetName = this.CurrentAction

            };
            var result = _WagerService.GetWagerData<TotalViewModel>(criteria);

            return View(result);
        }

        public ActionResult HalfFull(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            fromDate = new DateTime(2013, 05, 23);
            var criteria = new CorrectScoreCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                Bettype = 16
            };

            var result = _WagerService.GetWagerData<CorrectScoreViewModel>(criteria);

            return View(result);
        }

        public ActionResult Parlay()
        {
            return View();
        }

        public ActionResult SpecialBet(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            fromDate = new DateTime(2013, 05, 23);
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                GroupBetName = this.CurrentAction

            };
            var result = _WagerService.GetWagerData<SpecialViewModel>(criteria);

            return View(result);
        }

        public ActionResult Start()
        {
            return View();
        }
    }
}