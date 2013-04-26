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
    using Apollo.Age.Entities;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Services;
    using Apollo.Age.Web;
    using Fanex.Common.Extensions;

    public class ResultsController : BaseController
    {
        private readonly ResultService _resultService;

        protected DateTime BalanceUpDay
        {
            get { return _resultService.GetBalanceUpDay(); }
        }

        public ResultsController() : this(null) { }

        public ResultsController(ResultService resultService = null)
        {
            _resultService = resultService ?? new ResultService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return Index(DateTime.Now);
        }

        [HttpPost]
        public ActionResult Index(DateTime eventDate)
        {
            var viewModel = new ReportResultViewModel
            {
                BalanceUpDay = BalanceUpDay,
                SportList = _resultService.GetResultSports(eventDate),
                EventDate = eventDate
            };

            return View(viewModel);
        }

        public JsonResult League(DateTime date, byte sportId)
        {
            // not add select all item yet
            var leaguesBySport = _resultService.GetLeagueBySport(date, sportId);
            List<League> leagues = new List<League>();
            leagues.Add(new League { Id = 0, Name = Resources.Lang.allleague });

            leaguesBySport.ForEach((item) => leagues.Add(new League
            {
                Id = item.Id,
                Name = CacheRef.GetLeagueName(item.Id)
            }));

            //leaguesBySport = leaguesBySport
            //    .Select(l => new League
            // {
            //     Id = l.Id,
            //     Name = CacheRef.GetLeagueName(l.Id)
            // });

            return Json(leagues, JsonRequestBehavior.AllowGet);
        }

        private string GetSportName(int sportId)
        {
            switch (sportId)
            {
                case (int)SportTypes.Soccer:
                    return SportTypes.Soccer.ToString();
                case (int)SportTypes.BasketBall:
                case (int)SportTypes.Netball:
                case (int)SportTypes.FootBall:
                case (int)SportTypes.Rugby:
                    return SportTypes.BasketBall.ToString();
                case (int)SportTypes.IceHockey:
                case (int)SportTypes.FieldHockey:
                    return SportTypes.IceHockey.ToString();
                case (int)SportTypes.Tennis:
                    return SportTypes.Tennis.ToString();
                case (int)SportTypes.BaseBall:
                    return SportTypes.BaseBall.ToString();
                case (int)SportTypes.VolleyBall:
                case (int)SportTypes.Badminton:
                case (int)SportTypes.Squash:
                    return SportTypes.Badminton.ToString();
                case (int)SportTypes.Golf:
                case (int)SportTypes.Motorsports:
                case (int)SportTypes.Billard:
                case (int)SportTypes.Cycling:
                case (int)SportTypes.Fencing:
                case (int)SportTypes.Judo:
                case (int)SportTypes.ModernPentathlon:
                case (int)SportTypes.Rowing:
                case (int)SportTypes.Sailing:
                case (int)SportTypes.Shooting:
                case (int)SportTypes.Taekwondo:
                case (int)SportTypes.Triathlon:
                case (int)SportTypes.Wrestling:
                    return SportTypes.Billard.ToString();
                default:
                    return SportTypes.OtherSports.ToString();
            }
        }

        public ActionResult MatchResult(DateTime date, byte sportId, int leagueId = -1)
        {
            IEnumerable<MatchResultModel> matchResults =
                _resultService.GetMatchResult(date, sportId, this.CurrentSession.UserId, leagueId);

            if (matchResults != null && matchResults.Any())
            {
                matchResults = matchResults.OrderBy(match => match.LeagueId);
            }

            string viewName = GetSportName(sportId);

            return View(string.Format("~/Views/Results/{0}.cshtml", viewName), matchResults);
        }
    }
}