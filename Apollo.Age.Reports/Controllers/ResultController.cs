// --------------------------------------------------------------------------
// <copyright file="ResultController.cs" company="Nexcel Solutions Vietnam">
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

    public class ResultController : BaseController
    {
        private readonly ResultService _resultService;

        public ResultController() : this(null) { }

        public ResultController(ResultService resultService = null)
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
            var leagues = _resultService.GetLeagueBySport(date, sportId);
            leagues = leagues.Select(l => new League
             {
                 Id = l.Id,
                 Name = CacheRef.GetLeagueName(l.Id)
             });
            return Json(leagues, JsonRequestBehavior.AllowGet);
        }

        public ViewResult MatchResult(DateTime date, byte sportId, int leagueId = -1)
        {            
            return View();
        }
    }
}
