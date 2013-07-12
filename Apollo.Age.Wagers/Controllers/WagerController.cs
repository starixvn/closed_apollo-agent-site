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
    using Apollo.Age.Core;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Resources;
    using Apollo.Age.Services;
    using Apollo.Age.Wagers.Core;
    using Apollo.Age.Wagers.Core.BettypeGroup;
    using Apollo.Age.Wagers.Services;
    using Apollo.Age.Web;
    using Fanex.Common;

    /// <summary>
    /// Wager Controller.
    /// </summary>
    public class WagerController : BaseController
    {
        protected readonly IWagersService WagerService;
        protected readonly ReportService ReportService;
        private DateTime _wagerDate;

        public WagerController()
            : this(null)
        {
            var configDate = ConfigurationManager.AppSettings["WagerDateConfig"];
            _wagerDate = Converter.ToDateTime(configDate, DateTime.Now);
        }

        public WagerController(IWagersService wagerService = null)
        {
            WagerService = wagerService ?? new WagersService();
            ReportService = new ReportService();
        }

        public List<League> GetLeague(DateTime fromDate, DateTime? toDate = null, string sportTypes = null, bool isOutright = false)
        {
            var leagues = WagerService.GetLeague(fromDate, toDate, sportTypes, isOutright);
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
        /// Get league return Json format.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="sportTypes">Sport type id.</param>
        /// <param name="isOutright">Return true is out right.</param>
        /// <returns>Json format.</returns>
        public JsonResult GetJsonLeague(DateTime fromDate, DateTime? toDate = null, string sportTypes = null, bool isOutright = false)
        {           
            var result = this.GetLeague(fromDate, toDate, sportTypes, isOutright);
            if (result != null && result.Any())
            {
                result.Insert(0, new League
                 {
                     Id = 0,
                     Name = Lang.all
                 });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
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

            var listEventDate = WagerService.GetEventDate(sportId); ;

            var list = new List<SelectListItem>();

            foreach (var date in listEventDate)
            {
                list.Add(new SelectListItem() { Text = date.ToString("yyyy-MM-dd"), Value = date.ToString("yyyy-MM-dd") });
            }

            return list;
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
        /// Wagers index.
        /// </summary>
        /// <param name="sportName">Sport name.</param>
        /// <param name="groupBetName">Group bet name.</param>
        /// <param name="resourceKey">Resource key of group bet name.</param>
        /// <param name="isEM">Is early market.</param>
        /// <param name="isOR">Is outright wagers.</param>
        /// <returns>View result.</returns>
        public ActionResult Index(
            string sportName,
            string groupBetName = "Straight",
            string resourceKey = "lblStraight",
            bool isEM = false,
            bool isOR = false)
        {
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
                    sportTypes = SportGroup.Basketball_Football_Rugy;
                    title = Lang.Basketball;
                    titleAll = Lang.BasketballAll;
                    sportTypeIds = SportGroup.Basketball_Football_Rugy;
                    break;
                case WagersSportName.Tennis:
                    sportTypes = SportGroup.Tennis;
                    title = Lang.Tennis;
                    titleAll = Lang.lblTennisAll;
                    sportTypeIds = SportGroup.Tennis;
                    break;
                case WagersSportName.Volleyball:
                    sportTypes = SportGroup.Volleyball_Badminton_TableTennis;
                    title = Lang.Volleyball;
                    titleAll = Lang.lblVolleyballAll;
                    sportTypeIds = SportGroup.Volleyball_Badminton_TableTennis;
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

            if (isOR)
            {
                toDate = DateTime.Now.Date;
            }

            // AddDays(-9) : sample code for get data
            //ViewBag.Leagues = this.GetLeague(new DateTime(2013, 8, 1), sportTypes, isOR);
            ViewBag.Leagues = this.GetLeague(
                isOR ? ReportService.MoveBetsDay() : _wagerDate,
                toDate,
                sportTypes,
                isOR);

            ViewBag.MainShareHolder = CurrentSession.IsSuper ? this.GetMainShareHolder(sportName, isOR ? Soccer.OutRight : groupBetName) : new List<Agent>();
            ViewBag.GroupBetName = isOR ? Soccer.OutRight : groupBetName;
            ViewBag.SportName = sportName;
            ViewBag.Title = title;
            ViewBag.TitleAll = titleAll;
            ViewBag.LableGroupBetName = GetResourceData(resourceKey);
            ViewBag.SportTypes = sportTypeIds;
            ViewBag.WagerDate = _wagerDate;
            ViewBag.IsShowRefreshNow = CurrentSession.AgentLevel <= 1; //show button Refresh Now once logged in by Super and MainShareHolder
            if (isEM)
            {
                earlyMarketListDate = GenerateEarlyMarketDates(sportName);

                if (earlyMarketListDate != null && earlyMarketListDate.Any())
                {
                    ViewBag.EarlyMarketDates = GenerateEarlyMarketDates(sportName);
                }
                else
                {
                    return View("~/Areas/Wagers/Views/Wager/NoData.cshtml");
                }
            }

            ViewBag.isEM = isEM;
            ViewBag.isOR = isOR;

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
    }
}