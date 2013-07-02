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
    using Apollo.Age.Wagers.Criterias.CorrectScore;
    using Apollo.Age.Wagers.Criterias.Soccer;
    using Apollo.Age.Wagers.ViewModels.Soccer;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SoccerController : WagerController
    {
        public SoccerController() : base(null) { }

        public ActionResult Straight(DateTime fromDate, string uGroup, bool isDirectMember,
            int mainShareHolderId, int leagueId, int pageIndex = 0)
        {
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = uGroup,
                GroupBetName = this.CurrentAction,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<StraightViewModel>(criteria).ToList();

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

        public ActionResult FirstHalf(DateTime fromDate, string uGroup, bool isDirectMember,
            int mainShareHolderId, int leagueId, int pageIndex = 0)
        {
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = uGroup,
                GroupBetName = this.CurrentAction,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<FirstHalfViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

        public ActionResult RunningBall(DateTime fromDate, string uGroup, bool isDirectMember,
            int mainShareHolderId, int leagueId, int pageIndex = 0)
        {
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = uGroup,
                GroupBetName = this.CurrentAction,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<RunningBallViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

        public ActionResult Correctscore(DateTime fromDate, string uGroup, bool isDirectMember,
            int mainShareHolderId, int leagueId, int pageIndex = 0)
        {
            var criteria = new CorrectScoreCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = uGroup,
                Bettype = 4,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<CorrectScoreViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

        public ActionResult FirstHalfCorrectScore(DateTime fromDate, string uGroup, bool isDirectMember,
            int mainShareHolderId, int leagueId, int pageIndex = 0)
        {
            var criteria = new CorrectScoreCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = uGroup,
                Bettype = 30,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<CorrectScoreViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

        public ActionResult Total(DateTime fromDate, string uGroup, bool isDirectMember,
            int mainShareHolderId, int leagueId, int pageIndex = 0)
        {
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = uGroup,
                GroupBetName = this.CurrentAction,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<TotalViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

        public ActionResult HalfFull(DateTime fromDate, string uGroup, bool isDirectMember,
            int mainShareHolderId, int leagueId, int pageIndex = 0)
        {
            var criteria = new CorrectScoreCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = uGroup,
                Bettype = 16,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<CorrectScoreViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

        public ActionResult Parlay(DateTime fromDate, string uGroup, bool isDirectMember,
            int mainShareHolderId, int leagueId, int pageIndex = 0)
        {
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = uGroup,
                GroupBetName = this.CurrentAction,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<ParlayViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

        public ActionResult SpecialBet(DateTime fromDate, string uGroup, bool isDirectMember,
            int mainShareHolderId, int leagueId, int pageIndex = 0)
        {
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = uGroup,
                GroupBetName = this.CurrentAction,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<ViewModels.Basketball.SpecialViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

    }
}