// --------------------------------------------------------------------------
// <copyright file="BkNFLRugbyController.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.Age.Wagers.Criterias.Basketball;
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

        public ActionResult Straight(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            var criteria = new BasketballCriteria
            {
                GroupBetName = this.CurrentAction,
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                Sporttypes = SportGroup.Basketball_Football_Rugy
            };

            var list = _WagerService.GetWagerData<StraightViewModel>(criteria);

            return View(list);
        }

        public ActionResult FirstHalf(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            var criteria = new BasketballCriteria
            {
                GroupBetName = this.CurrentAction,
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                Sporttypes = SportGroup.Basketball_Football_Rugy
            };

            var list = _WagerService.GetWagerData<FirstHaftViewModel>(criteria);

            return View(list);
        }

        public ActionResult RunningBall(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            var criteria = new BasketballCriteria
            {
                GroupBetName = this.CurrentAction,
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                Sporttypes = SportGroup.Basketball_Football_Rugy
            };

            var list = _WagerService.GetWagerData<RunningBallViewModel>(criteria);

            return View(list);
        }

        public ActionResult Parlay(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            return View();
        }

        public ActionResult SpecialBet(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            var criteria = new BasketballCriteria
            {
                GroupBetName = this.CurrentAction,
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                Sporttypes = SportGroup.Basketball_Football_Rugy
            };

            var list = _WagerService.GetWagerData<SpecialViewModel>(criteria);

            return View(list);
        }
    }
}
