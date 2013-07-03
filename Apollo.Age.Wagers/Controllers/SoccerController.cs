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
    using System.Linq;
    using System.Web.Mvc;
    using Apollo.Age.Wagers.Criterias.CorrectScore;
    using Apollo.Age.Wagers.Criterias.Soccer;
    using Apollo.Age.Wagers.ViewModels.Soccer;
    using Apollo.Age.Wagers.Criterias;
    using Apollo.Age.Wagers.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SoccerController : WagerController
    {
        public ActionResult Straight(SoccerCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;

            var result = WagerService.GetWagerData<StraightViewModel>(criteria).ToList();

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(result);
        }

        public ActionResult FirstHalf(SoccerCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            
            var result = WagerService.GetWagerData<FirstHalfViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(result);
        }

        public ActionResult RunningBall(RunningBallCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.Sporttypes = SportId(criteria.SportName);

            var result = WagerService.GetWagerData<RunningBallViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(result);
        }

        public ActionResult Correctscore(CorrectScoreCriteria criteria)
        {
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.Bettype = 4;

            var result = WagerService.GetWagerData<CorrectScoreViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(result);
        }

        public ActionResult FirstHalfCorrectScore(CorrectScoreCriteria criteria)
        {
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.Bettype = 30;

            var result = WagerService.GetWagerData<CorrectScoreViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(result);
        }

        public ActionResult Total(SoccerCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;

            var result = WagerService.GetWagerData<TotalViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(result);
        }

        public ActionResult HalfFull(CorrectScoreCriteria criteria)
        {
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.Bettype = 16;

            var result = WagerService.GetWagerData<CorrectScoreViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(result);
        }

        public ActionResult Parlay(SoccerCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;

            var result = WagerService.GetWagerData<ParlayViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(result);
        }

        public ActionResult SpecialBet(SoccerCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;

            var result = WagerService.GetWagerData<ViewModels.Basketball.SpecialViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(result);
        }
    }
}