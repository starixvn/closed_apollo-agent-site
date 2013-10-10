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
//   <change who="Edward.Nguyen" date="2013.7.8">Add get date from App Setting, if is null will get DateTime.Now</change>
// </history>

namespace Apollo.Age.Reports.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Apollo.Age.Configurations;
    using Apollo.Age.Core;
    using Apollo.Age.Entities;
    using Apollo.Age.Enumerations;
    using Apollo.Age.PermissionControl.Core;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Web.Attribute;

    /// <summary>
    /// Result controller.
    /// </summary>
    [ApolloAuth(FunctionKey = ApolloFunctionKeys.Result, Policy = Policy.Query)]
    [SessionState(SessionStateBehavior.ReadOnly)]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class ResultsController : BaseController
    {
        /// <summary>
        /// Get property from Result service.
        /// </summary>
        private readonly ResultService _resultService;

        private DateTime _resultDate;

        #region Ctors

        /// <summary>
        /// Constructor default.
        /// </summary>
        public ResultsController()
            : this(null)
        {
            _resultDate = AppSettings.ResultDateConfig;
        }

        /// <summary>
        /// Constructor Controller.
        /// </summary>
        /// <param name="resultService">Result Service.</param>
        public ResultsController(ResultService resultService = null)
        {
            _resultService = resultService ?? new ResultService();
        }

        #endregion Ctors

        /// <summary>
        /// Gets Balance Up Day.
        /// </summary>
        protected DateTime GetBalanceUpDay
        {
            get { return _resultService.GetBalanceUpDay(); }
        }

        #region Match Results

        /// <summary>
        /// By Default is Match Action.
        /// </summary>
        /// <param name="sportId">Input Sport Id.</param>
        /// <returns>Return a View Model.</returns>
        [HttpGet]
        public ActionResult Match(byte sportId = 1)
        {
            var viewModel = new MatchResultViewModel
            {
                BalanceUpDay = GetBalanceUpDay,
                EventDate = _resultDate,
            };

            return View(viewModel);
        }

        /// <summary>
        /// Get all leagues by sport.
        /// </summary>
        /// <param name="date">Input a Date.</param>
        /// <param name="sportTypes">Input a Sport Type.</param>
        /// <param name="sportId">Input a Sport Id.</param>
        /// <returns>Return a JSON result.</returns>
        public JsonResult GetLeagueBySport(
            DateTime date,
            string sportTypes,
            int sportId)
        {
            var isShowDataTest = false;
            if (SessionManager.CurrentSession != null)
            {
                isShowDataTest = AppSettings.CurrencyTesting == CurrentSession.CurrencyId || CurrentSession.IsSuper;
            }

            IEnumerable<MatchResultModel> matchResults =
                _resultService.GetMatchResult(date, sportTypes, this.CurrentSession.UserId, isShowDataTest);

            var listLeague = new List<League>();

            if (matchResults != null && matchResults.Any())
            {
                listLeague = matchResults.Where(item => item.SportId == sportId)
                    .Select(item => new League { Id = item.LeagueId, Name = item.LeagueName })
                    .Distinct().ToList();

                if (listLeague != null && listLeague.Count() > 1)
                {
                    listLeague.Insert(0, new League { Id = 0, Name = Resources.Lang.allleague });
                }
            }
            else
            {
                listLeague.Add(new League { Id = -1, Name = string.Empty });
            }

            return Json(listLeague, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get all sports by match.
        /// </summary>
        /// <param name="date">Input a Date.</param>
        /// <param name="sportTypes">Input a Sport types.</param>
        /// <returns>Return JSON data.</returns>
        public JsonResult GetSportByMatch(DateTime date, string sportTypes)
        {
            var isShowDataTest = false;
            if (SessionManager.CurrentSession != null)
            {
                isShowDataTest = AppSettings.CurrencyTesting == CurrentSession.CurrencyId || CurrentSession.IsSuper;
            }

            IEnumerable<MatchResultModel> matchResults =
                _resultService.GetMatchResult(date, sportTypes, this.CurrentSession.UserId, isShowDataTest);

            var sports = new List<Sport>();
            if (matchResults != null && matchResults.Any())
            {
                sports = matchResults
                       .Select(item => new Sport { Id = (byte)item.SportId, Name = item.SportName })
                       .Distinct().ToList();
            }
            else
            {
                sports.Add(new Sport { Id = 01, Name = string.Empty });
            }

            return Json(sports, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get all Match.
        /// </summary>
        /// <param name="date">Input a Date.</param>
        /// <param name="sportTypes">Input a Sport Type.</param>
        /// <returns>Return a view with exactly path.</returns>
        public ActionResult GetMatchResult(DateTime date, string sportTypes)
        {
            var isShowDataTest = false;
            if (SessionManager.CurrentSession != null)
            {
                isShowDataTest = AppSettings.CurrencyTesting == CurrentSession.CurrencyId || CurrentSession.IsSuper;
            }

            IEnumerable<MatchResultModel> matchResults =
                _resultService.GetMatchResult(date, sportTypes, this.CurrentSession.UserId, isShowDataTest);

            if (matchResults != null && matchResults.Any())
            {
                matchResults = matchResults.OrderBy(match => match.LeagueId);
            }

            string viewName = GetViewBySport(sportTypes);
            ViewBag.IsParlay = false;

            return View(string.Format("~/Views/Results/{0}.cshtml", viewName), matchResults);
        }

        /// <summary>
        /// Get all others.
        /// </summary>
        /// <param name="date">Input a Date.</param>
        /// <param name="sportTypes">Input a Sport Types.</param>
        /// <param name="sportId">Input a Sport ID.</param>
        /// <param name="leagueId">Input a League ID.</param>
        /// <returns>Return a view with exactly path.</returns>
        public ActionResult GetOtherResult(DateTime date, string sportTypes, int sportId, int leagueId = 0)
        {
            var isShowDataTest = false;
            if (SessionManager.CurrentSession != null)
            {
                isShowDataTest = AppSettings.CurrencyTesting == CurrentSession.CurrencyId || CurrentSession.IsSuper;
            }

            IEnumerable<MatchResultModel> matchResults =
                _resultService.GetMatchResult(date, sportTypes, this.CurrentSession.UserId, isShowDataTest);

            if (matchResults != null && matchResults.Any())
            {
                if (leagueId == 0)
                {
                    matchResults = matchResults.Where(item => item.SportId == sportId).Distinct();
                }
                else
                {
                    matchResults = matchResults.Where(item => item.SportId == sportId && item.LeagueId == leagueId).Distinct();
                }
            }

            string viewName = GetViewBySport(sportTypes);

            return View(string.Format("~/Views/Results/{0}.cshtml", viewName), matchResults);
        }

        #endregion Match Results

        #region Outright Results

        /// <summary>
        /// Get all outright.
        /// </summary>
        /// <param name="date">Input a Date.</param>
        /// <returns>Return JSON Result.</returns>
        public JsonResult GetSportOutright(DateTime date)
        {
            var isShowDataTest = false;
            if (SessionManager.CurrentSession != null)
            {
                isShowDataTest = AppSettings.CurrencyTesting == CurrentSession.CurrencyId || CurrentSession.IsSuper;
            }

            var viewModel = new OutrightResultViewModel
            {
                BalanceUpDay = GetBalanceUpDay,
                FromDate = date,
                OutrightResult = _resultService.GetOutrightResult(date, isShowDataTest)
            };

            return Json(viewModel.SportList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// By default is outright.
        /// </summary>
        /// <param name="date">Input a Date.</param>
        /// <param name="sportId">Input a Sport ID.</param>
        /// <returns>Action Result.</returns>
        public ActionResult Outright(DateTime date, int sportId = 0)
        {
            var isShowDataTest = false;
            if (SessionManager.CurrentSession != null)
            {
                isShowDataTest = AppSettings.CurrencyTesting == CurrentSession.CurrencyId || CurrentSession.IsSuper;
            }

            IEnumerable<OutrightResultModel> outrightResults = _resultService.GetOutrightResult(date, isShowDataTest);
            if (outrightResults != null && outrightResults.Any())
            {
                outrightResults = outrightResults.Where(x => x.SportId == (byte)sportId);
            }

            return View(outrightResults);
        }

        #endregion Outright Results

        /// <summary>
        /// Return action name by spotTypes.
        /// </summary>
        /// <param name="sportTypes">Input a Sport Type.</param>
        /// <returns>Return Sport ID.</returns>
        private string GetViewBySport(string sportTypes)
        {
            switch (sportTypes)
            {
                case SportGroup.Soccer:
                    return SportTypes.Soccer.ToString();
                case SportGroup.BasketballFootballRugy:
                    return SportTypes.BasketBall.ToString();
                case SportGroup.Tennis:
                    return SportTypes.Tennis.ToString();
                case SportGroup.VolleyballBadmintonTableTennis:
                    return SportTypes.Badminton.ToString();
                case SportGroup.BaseBall:
                    return SportTypes.BaseBall.ToString();
                case SportGroup.Others:
                    return SportTypes.Others.ToString();
                case SportGroup.Outright:
                    return "Outright";
                default:
                    return SportTypes.Soccer.ToString();
            }
        }
    }
}