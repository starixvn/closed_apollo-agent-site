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
            var sports = _resultService.GetResultSports(date);
            IEnumerable<League> leagues = null;

            if (sports.Any())
            {
                leagues = _resultService.GetLeagueBySport(date, sports.First().Id);
            }

            var balanceUpDay = _resultService.GetBalanceUpDay();

            var viewModel = new ReportResultViewModel
            {
                BalanceUpDay = balanceUpDay,
                SportList = sports,
                LeagueList = leagues
            };

            return View(viewModel);
        }
        
        public ActionResult LeagueBySport()
        {
            return View();
        }
    }
}
