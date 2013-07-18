// --------------------------------------------------------------------------
// <copyright file="SoccerController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.6.25">Created</change>
//   <change who="Edward.Nguyen" date="2013.6.27">Implement prototype</change>
//   <change who="Terry.Nguyen" date="2013.7.3">Return exactly url for Runningball and Special bet</change>
// </history>

namespace Apollo.Age.Wagers.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Apollo.Age.Wagers.Criterias;
    using Apollo.Age.Wagers.Criterias.CorrectScore;
    using Apollo.Age.Wagers.Criterias.Soccer;
    using Apollo.Age.Wagers.ViewModels;
    using Apollo.Age.Wagers.ViewModels.Soccer;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class SoccerController : WagerController
    {
        /// <summary>
        /// Group bet type name Straight of Soccer.
        /// </summary>
        /// <param name="criteria">SoccerCriteria model.</param>
        /// <returns>View result.</returns>
        public ActionResult Straight(SoccerCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.IsSuperView = CurrentSession.IsSuper;

            var result = WagerService.GetWagerData<StraightViewModel>(criteria).ToList();

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(result);
        }

        /// <summary>
        /// Group bet type name FirstHalf of Soccer.
        /// </summary>
        /// <param name="criteria">SoccerCriteria model.</param>
        /// <returns>View result.</returns>
        public ActionResult FirstHalf(SoccerCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.IsSuperView = CurrentSession.IsSuper;

            var result = WagerService.GetWagerData<FirstHalfViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(result);
        }

        /// <summary>
        /// Group bet type name FirstHalf of Soccer.
        /// </summary>
        /// <param name="criteria">RunningBallCriteria model.</param>
        /// <returns>View result.</returns>
        public ActionResult RunningBall(RunningBallCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.Sporttypes = SportId(criteria.SportName);
            criteria.IsSuperView = CurrentSession.IsSuper;

            var result = WagerService.GetWagerData<RunningBallViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View("~/Areas/Wagers/Views/Wager/RunningBall.cshtml", result);
        }

        /// <summary>
        /// Group bet type name Correct score of Soccer.
        /// </summary>
        /// <param name="criteria">CorrectScoreCriteria model.</param>
        /// <returns>View result.</returns>
        public ActionResult Correctscore(CorrectScoreCriteria criteria)
        {
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.Bettype = 4;
            criteria.IsSuperView = CurrentSession.IsSuper;

            var result = WagerService.GetWagerData<CorrectScoreViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(result);
        }

        /// <summary>
        /// Group bet type name FirstHalfCorrectScore of Soccer.
        /// </summary>
        /// <param name="criteria">CorrectScoreCriteria model.</param>
        /// <returns>View result.</returns>
        public ActionResult FirstHalfCorrectScore(CorrectScoreCriteria criteria)
        {
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.Bettype = 30;
            criteria.IsSuperView = CurrentSession.IsSuper;

            var result = WagerService.GetWagerData<CorrectScoreViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(result);
        }
        
        /// <summary>
        /// Group bet type name Total of Soccer.
        /// </summary>
        /// <param name="criteria">SoccerCriteria model.</param>
        /// <returns>View result.</returns>
        public ActionResult Total(SoccerCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.IsSuperView = CurrentSession.IsSuper;

            var result = WagerService.GetWagerData<TotalViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(result);
        }

        /// <summary>
        /// Group bet type name HalfFull of Soccer.
        /// </summary>
        /// <param name="criteria">CorrectScoreCriteria model.</param>
        /// <returns>View result.</returns>
        public ActionResult HalfFull(CorrectScoreCriteria criteria)
        {
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.Bettype = 16;
            criteria.IsSuperView = CurrentSession.IsSuper;

            var result = WagerService.GetWagerData<CorrectScoreViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(result);
        }

        /// <summary>
        /// Group bet type name Parlay of Soccer.
        /// </summary>
        /// <param name="criteria">SoccerCriteria model.</param>
        /// <returns>View result.</returns>
        public ActionResult Parlay(SoccerCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.IsSuperView = CurrentSession.IsSuper;

            var result = WagerService.GetWagerData<ParlayViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(result);
        }

        /// <summary>
        /// Group bet type name SpecialBet of Soccer.
        /// </summary>
        /// <param name="criteria">SoccerCriteria model.</param>
        /// <returns>View result.</returns>
        public ActionResult SpecialBet(SoccerCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.IsSuperView = CurrentSession.IsSuper;

            var result = WagerService.GetWagerData<ViewModels.Basketball.SpecialViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View("~/Areas/Wagers/Views/Wager/SpecialBet.cshtml", result);
        }
    }
}