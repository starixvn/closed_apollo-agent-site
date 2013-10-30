// --------------------------------------------------------------------------
// <copyright file="WagerController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.6.30">Created</change>
//   <change who="Anders.Le" date="2013.6.30">Remove property WagerService</change>
//   <change who="Terry.Nguyen" date="2013.7.2">Add outright sporttype</change>
//   <change who="Edward.Nguyen" date="2013.7.8">Add get date from App Setting, if is null will get DateTime.Now</change>
// </history>

namespace Apollo.Age.Wagers.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web.Mvc;
    using Apollo.Age.Configurations;
    using Apollo.Age.Core;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Resources;
    using Apollo.Age.Services;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Wagers.Core;
    using Apollo.Age.Wagers.Core.BettypeGroup;
    using Apollo.Age.Wagers.Services;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Web.Attribute;
    using Fanex.Common;

    /// <summary>
    /// Wager Controller.
    /// </summary>
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class WagerController : BaseController
    {
        protected readonly IWagersService WagerService;
        protected readonly ReportService ReportService;
        protected readonly IUserService UserService;
        private DateTime _wagerDate;

        public WagerController()
            : this(null, null)
        {
            var configDate = ConfigurationManager.AppSettings["WagerDateConfig"];
            _wagerDate = Converter.ToDateTime(configDate, DateTime.Now);
        }

        public WagerController(IWagersService wagerService = null, IUserService userService = null)
        {
            WagerService = wagerService ?? new WagersService();
            ReportService = new ReportService();
            UserService = userService ?? new UserService();

            ViewBag.HasDownlineDetailsPermission = UserService.GetUserPolicy(this.CurrentSession.UserId, ApolloFunctionKeys.DownlineDetails) >= (byte)Policy.Query;
        }

        /// <summary>
        /// Get all leagues.
        /// </summary>
        /// <param name="fromDate">Input from date.</param>
        /// <param name="toDate">Input to date.</param>
        /// <param name="sportTypes">Input sport type.</param>
        /// <param name="isOutright">Input the status of Outright.</param>
        /// <returns>Return list of Leagues.</returns>
        public List<League> GetLeague(DateTime fromDate, DateTime? toDate = null, string sportTypes = null, bool isOutright = false)
        {
            var isShowDataTest = false;
            if (CurrentSession != null)
            {
                isShowDataTest = AppSettings.CurrencyTesting == CurrentSession.CurrencyId || CurrentSession.IsSuper;
            }

            var leagues = WagerService.GetLeague(fromDate, toDate, sportTypes, isOutright, isShowDataTest);
            var result = new List<League>();
            foreach (var league in leagues)
            {
                result.Add(new League
                {
                    Id = league.Id,
                    Name = CacheRef.GetLeagueName(league.Id)
                });
            }

            return leagues != null ? result.OrderBy(x => x.Name).ToList() : new List<League>();
        }

        /// <summary>
        /// Get league return JSON format.
        /// </summary>
        /// <param name="fromDate">Input from date.</param>
        /// <param name="toDate">Input to date.</param>
        /// <param name="sportTypes">Sport type id.</param>
        /// <param name="isOutright">Return true is out right.</param>
        /// <returns>Return value with JSON format.</returns>
        public JsonResult GetJsonLeague(
            DateTime fromDate,
            DateTime? toDate = null,
            string sportTypes = null,
            bool isOutright = false)
        {
            var result = this.GetLeague(fromDate, toDate, sportTypes, isOutright);
            if (result != null && result.Any())
            {
                result.Insert(
                    0,
                    new League
                    {
                        Id = 0,
                        Name = Lang.all
                    });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Wagers index.
        /// </summary>
        /// <param name="sportName">Sport name.</param>
        /// <param name="groupBetName">Group bet name.</param>
        /// <param name="resourceKey">Resource key of group bet name.</param>
        /// <param name="isEarlyMarket">Is early market.</param>
        /// <param name="isOutright">Is outright wagers.</param>
        /// <returns>View result.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Wagers + ";" + ApolloFunctionKeys.EMWagers + ";" + ApolloFunctionKeys.Outright, Policy = Policy.Query)]
        public ActionResult Index(
            string sportName,
            string groupBetName = "Straight",
            string resourceKey = "lblStraight",
            bool isEarlyMarket = false,
            bool isOutright = false)
        {
            var functionKey = ApolloFunctionKeys.Wagers;
            // Check permission
            if (isEarlyMarket)
            {
                functionKey = ApolloFunctionKeys.EMWagers;
            }
            else if (isOutright)
            {
                functionKey = ApolloFunctionKeys.Outright;
            }

            var policy = UserService.GetUserPolicy(CurrentSession.UserId, functionKey);

            if (policy < (byte)Policy.Query)
            {
                return View("~/Views/Error/AccessDenied.cshtml");
            }

            var sportTypes = string.Empty;
            sportName = !string.IsNullOrWhiteSpace(sportName) ? sportName.ToLower() : string.Empty;
            var title = string.Empty;
            var titleAll = string.Empty;
            var sportTypeIds = string.Empty;
            List<SelectListItem> earlyMarketListDate = null;

            groupBetName = groupBetName.ToLower();

            if (Equals(sportName, WagersSportName.Soccer))
            {
                var dic = Soccer.ListOfBettype();

                ViewBag.BetTypeIds = dic.ContainsKey(groupBetName) ? dic[groupBetName] : string.Empty;
            }
            else
            {
                var dic = Basketball.ListOfBettype();

                ViewBag.BetTypeIds = dic.ContainsKey(groupBetName) ? dic[groupBetName] : string.Empty;
            }

            switch (sportName)
            {
                case WagersSportName.Soccer:
                    sportTypes = SportGroup.Soccer;
                    title = Lang.soccer;
                    titleAll = Lang.lblsoccerAll;
                    sportTypeIds = SportGroup.Soccer;
                    break;

                case WagersSportName.Basketball:
                    sportTypes = SportGroup.BasketballFootballRugy;
                    title = Lang.Basketball;
                    titleAll = Lang.BasketballAll;
                    sportTypeIds = SportGroup.BasketballFootballRugy;
                    break;

                case WagersSportName.Tennis:
                    sportTypes = SportGroup.Tennis;
                    title = Lang.Tennis;
                    titleAll = Lang.lblTennisAll;
                    sportTypeIds = SportGroup.Tennis;
                    break;

                case WagersSportName.Volleyball:
                    sportTypes = SportGroup.VolleyballBadmintonTableTennis;
                    title = Lang.Volleyball;
                    titleAll = Lang.lblVolleyballAll;
                    sportTypeIds = SportGroup.VolleyballBadmintonTableTennis;
                    break;

                case WagersSportName.Baseball:
                    sportTypes = SportGroup.BaseBall;
                    title = Lang.Baseball;
                    titleAll = Lang.lblBaseballAll;
                    sportTypeIds = SportGroup.BaseBall;
                    break;

                default:
                    sportTypes = SportGroup.Others;
                    sportTypeIds = SportGroup.Others;
                    titleAll = Lang.all;
                    break;
            }

            ViewBag.BalanceUpDay = ReportService.GetBalanceUpDay();
            ViewBag.MoveBetDate = ReportService.MoveBetsDay();
            DateTime? toDate = null;

            if (isOutright)
            {
                toDate = DateTime.Now.Date;
            }

            var listMainShareHolder = this.GetMainShareHolder(sportName, isOutright ? Soccer.OutRight : groupBetName);
            var hasDownlineDetailsPermission = UserService.GetUserPolicy(this.CurrentSession.UserId, ApolloFunctionKeys.DownlineDetails) >= (byte)Policy.Query;

            if (!hasDownlineDetailsPermission)
            {
                for (int i = 0; i < listMainShareHolder.Count; i++)
                {
                    listMainShareHolder[i].Username = Web.Mvc.HtmlHelperExtensions.EncryptUsername(listMainShareHolder[i].Username);
                }
            }

            ViewBag.MainShareHolder = CurrentSession.IsSuper ? listMainShareHolder : new List<Agent>();
            ViewBag.GroupBetName = isOutright ? Soccer.OutRight : groupBetName;
            ViewBag.SportName = sportName;
            ViewBag.Title = title;
            ViewBag.TitleAll = titleAll;
            ViewBag.LableGroupBetName = GetResourceData(resourceKey);
            ViewBag.SportTypes = sportTypeIds;

            // Show button Refresh Now once logged in by Super and MainShareHolder.
            ViewBag.IsShowRefreshNow = CurrentSession.AgentLevel <= 1;
            if (isEarlyMarket)
            {
                earlyMarketListDate = GenerateEarlyMarketDates(sportName);

                if (earlyMarketListDate != null && earlyMarketListDate.Any())
                {
                    ViewBag.EarlyMarketDates = earlyMarketListDate;

                    _wagerDate = Convert.ToDateTime(earlyMarketListDate[0].Value);
                }
                else
                {
                    return View("~/Areas/Wagers/Views/Wager/NoData.cshtml");
                }
            }
            else
            {
                _wagerDate = DateTime.Now;
            }

            ViewBag.Leagues = this.GetLeague(
              isOutright ? ReportService.MoveBetsDay() : _wagerDate,
              toDate,
              sportTypes,
              isOutright);

            ViewBag.WagerDate = _wagerDate;
            ViewBag.isEM = isEarlyMarket;
            ViewBag.isOR = isOutright;

            if (CurrentSession.AgentLevel == 0)
            {
                ViewBag.IsShowViewOption = 0; // show selection Main Share Holder
            }
            else if (CurrentSession.AgentLevel > 0 && CurrentSession.AgentLevel < 3)
            {
                ViewBag.IsShowViewOption = 1; // Remove Option
            }
            else
            {
                ViewBag.IsShowViewOption = 2; // Show selected All and Direct Member
            }

            return View();
        }

        /// <summary>
        /// Get list main share holder.
        /// </summary>
        /// <param name="sportName">Sport name.</param>
        /// <param name="groupBetName">Group bet name.</param>
        /// <returns>List agent.</returns>
        protected List<Agent> GetMainShareHolder(string sportName, string groupBetName)
        {
            switch (sportName)
            {
                case WagersSportName.Soccer:
                    {
                        if (Soccer.ListOfBettype().ContainsKey(groupBetName.ToLower()))
                        {
                            groupBetName = Soccer.ListOfBettype()[groupBetName.ToLower()];
                        }

                        break;
                    }

                default:
                    {
                        if (Basketball.ListOfBettype().ContainsKey(groupBetName.ToLower()))
                        {
                            groupBetName = Basketball.ListOfBettype()[groupBetName.ToLower()];
                        }

                        break;
                    }
            }

            var mainShareHolders = WagerService.GetMainShareHolder(groupBetName);

            return mainShareHolders != null ? mainShareHolders.ToList() : new List<Agent>();
        }

        /// <summary>
        /// Get sport type id by sport name.
        /// </summary>
        /// <param name="sportName">Sport name.</param>
        /// <returns>Sport type id.</returns>
        protected string SportId(string sportName)
        {
            string sportId = string.Empty;
            if (WagersSportName.WagerSportGroup().ContainsKey(sportName))
            {
                sportId = WagersSportName.WagerSportGroup()[sportName];
            }

            return sportId;
        }

        /// <summary>
        /// Get date for early market wagers.
        /// </summary>
        /// <param name="sportName">Sport name.</param>
        /// <returns>List date.</returns>
        protected List<SelectListItem> GenerateEarlyMarketDates(string sportName)
        {
            var sportId = SportId(sportName);

            var listEventDate = WagerService.GetEventDate(sportId);

            var list = new List<SelectListItem>();

            foreach (var date in listEventDate)
            {
                list.Add(new SelectListItem() { Text = date.ToString("yyyy-MM-dd"), Value = date.ToString("yyyy-MM-dd") });
            }

            return list;
        }
    }
}