// --------------------------------------------------------------------------
// <copyright file="WagerController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.6.30">Created</change>
//   <change who="Anders.Le" date="2013.6.30">Remove property WagerService</change>
// </history>

namespace Apollo.Age.Wagers.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Apollo.Age.Core;
    using Apollo.Age.Entities;
    using Apollo.Age.Resources;
    using Apollo.Age.Wagers.Core;
    using Apollo.Age.Wagers.Services;
    using Apollo.Age.Web;
    using Apollo.Age.Wagers.Core.BettypeGroup;

    public class WagerController : BaseController
    {
        protected readonly IWagersService _WagerService;

        public WagerController()
            : this(null)
        {
        }

        public WagerController(IWagersService wagerService = null)
        {
            _WagerService = wagerService ?? new WagersService();
        }

        public List<League> GetLeague(DateTime fromDate, string sportTypes = null)
        {
            var leagues = _WagerService.GetLeague(fromDate, sportTypes);

            return leagues != null ? leagues.ToList() : new List<League>();
        }

        public List<SelectListItem> GenerateEarlyMarketCalendar()
        {
            var list = new List<SelectListItem>();
            string result = string.Empty;
            for (var date = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1); date.Month == DateTime.Now.AddMonths(1).Month; date = date.AddDays(1))
            {
                result = date.ToString("yyyy-MM-dd");
                list.Add(new SelectListItem() { Text = result, Value = result });
            }
            return list;
        }

        public List<Agent> GetMainShareHolder(string sportName, string groupBetName)
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
            var mainShareHolders = _WagerService.GetMainShareHolder(groupBetName);

            return mainShareHolders != null ? mainShareHolders.ToList() : new List<Agent>();
        }

        public ActionResult Index(string sportName, string groupBetName = "Straight", string resourceKey = "lblStraight",
            bool isEM = false)
        {
            var sportTypes = string.Empty;
            sportName = !string.IsNullOrWhiteSpace(sportName) ? sportName.ToLower() : string.Empty;
            var title = string.Empty;
            var titleAll = string.Empty;
            var sportTypeIds = string.Empty;

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
                    sportTypes = SportGroup.Other;
                    sportTypeIds = SportGroup.Other;
                    titleAll = Lang.all;
                    break;
            }

            // AddDays(-9) : sample code for get data
            ViewBag.Leagues = this.GetLeague(DateTime.Now.AddDays(-11), sportTypes);
            ViewBag.MainShareHolder = (CurrentSession.IsSuper ? this.GetMainShareHolder(sportName, groupBetName) : new List<Agent>());
            ViewBag.GroupBetName = groupBetName;
            ViewBag.SportName = sportName;
            ViewBag.Title = title;
            ViewBag.TitleAll = titleAll;
            ViewBag.LableGroupBetName = GetResourceData(resourceKey);
            ViewBag.SportTypes = sportTypeIds;
            if (isEM)
            {
                ViewBag.EmCalendar = GenerateEarlyMarketCalendar();
            }
            else
            {
                ViewBag.EmCalendar = null;
            }
            return View();
        }
    }
}
