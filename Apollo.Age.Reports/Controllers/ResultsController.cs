// --------------------------------------------------------------------------
// <copyright file="ResultsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.9">Created</change>
//   <change who="Lewis" date="2013.4.17">
//      + Added BalanceUpDay
//      + Refactored Index (involved in implementing Index.cshtml)
//   </change>
// </history>

namespace Apollo.Age.Reports.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Apollo.Age.Entities;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Services;
    using Apollo.Age.Web;
    using Fanex.Common.Extensions;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ResultsController : BaseController
    {
        private readonly ResultService _resultService;

        #region Ctors

        public ResultsController() : this(null) 
        { 
        }

        public ResultsController(ResultService resultService = null)
        {
            _resultService = resultService ?? new ResultService();
        }

        #endregion Ctors

        protected DateTime BalanceUpDay
        {
            get { return _resultService.GetBalanceUpDay(); }
        }

        #region Match Results

        [HttpGet]
        public ActionResult Match()
        {
            return Match(DateTime.Now);
        }

        [HttpPost]
        public ActionResult Match(DateTime eventDate)
        {
            var viewModel = new MatchResultViewModel
            {
                BalanceUpDay = BalanceUpDay,
                SportList = _resultService.GetResultSports(eventDate),
                EventDate = eventDate
            };

            return View(viewModel);
        }

        public JsonResult MatchLeague(DateTime date, byte sportId)
        {
            // not add select all item yet
            var leaguesBySport = _resultService.GetLeagueBySport(date, sportId);
            List<League> leagues = new List<League>();
            if (leaguesBySport != null && leaguesBySport.Count() > 1)
            {
                leagues.Add(new League { Id = 0, Name = Resources.Lang.allleague });
            }

            leaguesBySport.ForEach((item) => leagues.Add(new League
            {
                Id = item.Id,
                Name = CacheRef.GetLeagueName(item.Id)
            }));

            return Json(leagues, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResultByLeague(DateTime date, byte sportId, int leagueId = -1)
        {
            IEnumerable<MatchResultModel> matchResults =
                _resultService.GetMatchResult(date, sportId, this.CurrentSession.UserId, leagueId);

            if (matchResults != null && matchResults.Any())
            {
                matchResults = matchResults.OrderBy(match => match.LeagueId);
            }

            string viewName = GetViewBySport(sportId);
            ViewBag.IsParlay = false;

            return View(string.Format("~/Views/Results/{0}.cshtml", viewName), matchResults);
        }

        public ActionResult ResultByMatch(int matchId, byte sportId)
        {
            IEnumerable<MatchResultModel> results = _resultService.GetResultByMatch(matchId);

            if (results != null && results.Any())
            {
                results = results.OrderBy(match => match.LeagueId);
            }

            string sportName = GetViewBySport(sportId);

            ViewBag.ViewSport = string.Format("~/Views/Results/{0}.cshtml", sportName);
            ViewBag.IsParlay = false;

            return View("~/Views/Results/MatchResult.cshtml", results);
        }

        public ActionResult ParlayResult(long refNo)
        {
            IEnumerable<MatchResultModel> results = _resultService.GetResultByParlay(refNo);

            foreach (var result in results)
            {
                result.ViewName = GetViewBySport(result.SportId);
            }

            if (results != null && results.Any())
            {
                results = results.OrderBy(match => match.LeagueId);
            }

            ViewBag.IsParlay = true;

            return View("~/Views/Results/ParlayResult.cshtml", results);
        }
        #endregion Match Results

        #region Outright Results

        [HttpGet]
        public ActionResult Outright()
        {
            return Outright(DateTime.Now, DateTime.Now);
        }

        [HttpPost]
        public ActionResult Outright(DateTime fromDate, DateTime toDate)
        {
            var viewModel = new OutrightResultViewModel
            {
                BalanceUpDay = BalanceUpDay,
                FromDate = fromDate,
                ToDate = toDate,
                OutrightResult = _resultService.GetOutrightResult(fromDate, toDate)
            };

            return View(viewModel);
        }

        public JsonResult OutrightLeague(DateTime fromDate, DateTime toDate, byte sportId)
        {
            var viewModel = new OutrightResultViewModel
            {
                BalanceUpDay = BalanceUpDay,
                FromDate = fromDate,
                ToDate = toDate,
                SportId = sportId,
                OutrightResult = _resultService.GetOutrightResult(fromDate, toDate)
            };

            return Json(viewModel.LeagueList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OutrightResult(DateTime fromDate, DateTime toDate, byte sportId = 0, int leagueId = 0)
        {
            IEnumerable<OutrightResultModel> outrightResults = _resultService.GetOutrightResult(fromDate, toDate);
            if (outrightResults != null && outrightResults.Any())
            {
                if (leagueId != 0)
                {
                    outrightResults = outrightResults.Where(x => x.LeagueId == leagueId);
                }
                else
                {
                    outrightResults = outrightResults.Where(x => x.SportId == sportId);
                }
            }

            return View(outrightResults);
        }

        #endregion Outright Results

        private string GetViewBySport(byte sportId)
        {
            switch (sportId)
            {
                case (byte)SportTypes.Soccer:
                    return SportTypes.Soccer.ToString();
                case (byte)SportTypes.BasketBall:
                case (byte)SportTypes.Netball:
                case (byte)SportTypes.FootBall:
                case (byte)SportTypes.Rugby:
                    return SportTypes.BasketBall.ToString();
                case (byte)SportTypes.IceHockey:
                case (byte)SportTypes.FieldHockey:
                    return SportTypes.IceHockey.ToString();
                case (byte)SportTypes.Tennis:
                    return SportTypes.Tennis.ToString();
                case (byte)SportTypes.BaseBall:
                    return SportTypes.BaseBall.ToString();
                case (byte)SportTypes.VolleyBall:
                case (byte)SportTypes.Badminton:
                case (byte)SportTypes.Squash:
                    return SportTypes.Badminton.ToString();
                case (byte)SportTypes.Golf:
                case (byte)SportTypes.Motorsports:
                case (byte)SportTypes.Billard:
                case (byte)SportTypes.Cycling:
                case (byte)SportTypes.Fencing:
                case (byte)SportTypes.Judo:
                case (byte)SportTypes.ModernPentathlon:
                case (byte)SportTypes.Rowing:
                case (byte)SportTypes.Sailing:
                case (byte)SportTypes.Shooting:
                case (byte)SportTypes.Taekwondo:
                case (byte)SportTypes.Triathlon:
                case (byte)SportTypes.Wrestling:
                    return SportTypes.Billard.ToString();
                default:
                    return SportTypes.OtherSports.ToString();
            }
        }
    }
}