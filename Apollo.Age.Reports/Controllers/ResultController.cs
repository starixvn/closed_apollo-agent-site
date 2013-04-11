// --------------------------------------------------------------------------
// <copyright file="ResultsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Reports.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Web;
    using System.Web.Mvc;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Entities;
    using Apollo.Age.Services;
    using Fanex.Common.Extensions;
using Apollo.Age.Reports.Models;

    public class ResultsController : BaseController
    {
        private readonly ResultService _resultService;

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
        public ActionResult Index(DateTime date)
        {
            var balanceUpDay = _resultService.GetBalanceUpDay();

            var sports = _resultService.GetResultSports(date);

            var viewModel = new ReportResultViewModel
            {
                BalanceUpDay = balanceUpDay,
                SportList = sports,
                EventDate = date
            };

            return View(viewModel);
        }

        public JsonResult League(DateTime date, byte sportId)
        {
            // not add select all item yet
            var leaguesBySport = _resultService.GetLeagueBySport(date, sportId);
            List<League> leagues = new List<League>();
            leagues.Add(new League { Id = -1, Name = Resources.Lang.allleague });

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

        public ActionResult MatchResult(DateTime date, byte sportId, int leagueId = -1)
        {
            var matchResults = _resultService.GetMatchResult(date, sportId, this.CurrentSession.UserId, leagueId);
            return RedirectToAction("Soccer", matchResults);
        }

        public ActionResult Soccer(IEnumerable<MatchResultModel> matchResults)
        {
            if (matchResults != null && matchResults.Any())
            {
                matchResults = matchResults.OrderBy(match => match.LeagueId);
            }

            return View(matchResults);
        }
    }
}
