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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SoccerController : WagerController
    {
        public ActionResult Straight(
            DateTime fromDate,
            string groupComm,
            bool isDirectMember,
            int mainShareHolderId,
            int leagueId,
            int pageIndex = 0)
        {
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = groupComm,
                GroupBetName = this.CurrentAction,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<StraightViewModel>(criteria).ToList();

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

        public ActionResult FirstHalf(
            DateTime fromDate,
            string groupComm,
            bool isDirectMember,
            int mainShareHolderId,
            int leagueId,
            int pageIndex = 0)
        {
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = groupComm,
                GroupBetName = this.CurrentAction,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<FirstHalfViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

        public ActionResult RunningBall(
            DateTime fromDate,
            string groupComm,
            bool isDirectMember,
            int mainShareHolderId,
            int leagueId,
            int pageIndex = 0)
        {
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = groupComm,
                GroupBetName = this.CurrentAction,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<RunningBallViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

        public ActionResult Correctscore(
            DateTime fromDate,
            string groupComm,
            bool isDirectMember,
            int mainShareHolderId,
            int leagueId,
            int pageIndex = 0)
        {
            var criteria = new CorrectScoreCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = groupComm,
                Bettype = 4,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<CorrectScoreViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

        public ActionResult FirstHalfCorrectScore(
            DateTime fromDate,
            string groupComm,
            bool isDirectMember,
            int mainShareHolderId,
            int leagueId,
            int pageIndex = 0)
        {
            var criteria = new CorrectScoreCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = groupComm,
                Bettype = 30,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<CorrectScoreViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

        public ActionResult Total(
            DateTime fromDate,
            string groupComm,
            bool isDirectMember,
            int mainShareHolderId,
            int leagueId,
            int pageIndex = 0)
        {
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = groupComm,
                GroupBetName = this.CurrentAction,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<TotalViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

        public ActionResult HalfFull(
            DateTime fromDate,
            string groupComm,
            bool isDirectMember,
            int mainShareHolderId,
            int leagueId,
            int pageIndex = 0)
        {
            var criteria = new CorrectScoreCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = groupComm,
                Bettype = 16,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<CorrectScoreViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

        public ActionResult Parlay(
            DateTime fromDate,
            string groupComm,
            bool isDirectMember,
            int mainShareHolderId,
            int leagueId,
            int pageIndex = 0)
        {
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = groupComm,
                GroupBetName = this.CurrentAction,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<ParlayViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }

        public ActionResult SpecialBet(
            DateTime fromDate,
            string groupComm,
            bool isDirectMember,
            int mainShareHolderId,
            int leagueId,
            int pageIndex = 0)
        {
            var criteria = new SoccerCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = mainShareHolderId > 0 ? mainShareHolderId : CurrentSession.AgentId,
                UGroup = groupComm,
                GroupBetName = this.CurrentAction,
                PageIndex = pageIndex
            };

            var result = _WagerService.GetWagerData<ViewModels.Basketball.SpecialViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;

            return View(result);
        }
    }
}