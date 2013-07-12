// --------------------------------------------------------------------------
// <copyright file="BasketballController.cs" company="Nexcel Solutions Vietnam">
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
    using System.Web.Mvc;
    using Apollo.Age.Wagers.Criterias;
    using Apollo.Age.Wagers.Criterias.Basketball;
    using Apollo.Age.Wagers.ViewModels;
    using Apollo.Age.Wagers.ViewModels.Basketball;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BasketballController : WagerController
    {
        public BasketballController()
            : base(null)
        {
        }

        /// <summary>
        /// Group bet type name Straight.
        /// </summary>
        /// <param name="criteria">BasketballCriteria model.</param>
        /// <returns>View results.</returns>
        public ActionResult Straight(BasketballCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.IsSuperView = CurrentSession.IsSuper;

            var list = WagerService.GetWagerData<StraightViewModel>(criteria);

            ViewBag.SportName = criteria.SportName;
            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(list);
        }

        /// <summary>
        /// Group bet type name FirstHalf.
        /// </summary>
        /// <param name="criteria">BasketballCriteria model.</param>
        /// <returns>View results.</returns>
        public ActionResult FirstHalf(BasketballCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.IsSuperView = CurrentSession.IsSuper;

            var list = WagerService.GetWagerData<FirstHaftViewModel>(criteria);

            ViewBag.SportName = criteria.SportName;
            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(list);
        }

        /// <summary>
        /// Group bet type name RunningBall.
        /// </summary>
        /// <param name="criteria">BasketballCriteria model.</param>
        /// <returns>View results.</returns>
        public ActionResult RunningBall(RunningBallCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.Sporttypes = SportId(criteria.SportName);
            criteria.IsSuperView = CurrentSession.IsSuper;

            var list = WagerService.GetWagerData<RunningBallViewModel>(criteria);

            ViewBag.SportName = criteria.SportName;
            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View("~/Areas/Wagers/Views/Wager/RunningBall.cshtml", list);
        }

        /// <summary>
        /// Group bet type name Parlay.
        /// </summary>
        /// <param name="criteria">BasketballCriteria model.</param>
        /// <returns>View results.</returns>
        public ActionResult Parlay(BasketballCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.IsSuperView = CurrentSession.IsSuper;

            var list = WagerService.GetWagerData<ParlayViewModel>(criteria);

            ViewBag.SportName = criteria.SportName;
            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View(list);
        }

        /// <summary>
        /// Group bet type name SpecialBet.
        /// </summary>
        /// <param name="criteria">BasketballCriteria model.</param>
        /// <returns>View results.</returns>
        public ActionResult SpecialBet(BasketballCriteria criteria)
        {
            criteria.GroupBetName = this.CurrentAction;
            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.IsSuperView = CurrentSession.IsSuper;

            var list = WagerService.GetWagerData<SpecialViewModel>(criteria);

            ViewBag.SportName = criteria.SportName;
            ViewBag.TotalItems = criteria.TotalItems;
            ViewBag.IsEM = criteria.IsEM;

            return View("~/Areas/Wagers/Views/Wager/SpecialBet.cshtml", list);
        }
    }
}
