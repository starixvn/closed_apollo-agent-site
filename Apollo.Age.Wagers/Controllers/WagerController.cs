using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apollo.Age.Wagers.Services;
using System.Web.Mvc;
using Apollo.Age.Core;
using Apollo.Age.Entities;
using Apollo.Age.Wagers.Core;
using Apollo.Age.Web;
using Apollo.Age.Resources;

namespace Apollo.Age.Wagers.Controllers
{
    public class WagerController : BaseController
    {
        private readonly WagersService _wagerService;

        public string ActionName { get; set; }

        public WagerController()
            : this(null)
        {
            this.ActionName = "Straight";
        }

        public WagerController(WagersService wagerService = null)
        {
            _wagerService = wagerService ?? new WagersService();
        }

        public List<League> GetLeague(DateTime fromDate, string sportTypes = null)
        {
            var leagues = _wagerService.GetLeague(fromDate, sportTypes);

            return leagues != null ? leagues.ToList() : new List<League>();
        }

        public ActionResult Index(string sportName, string groupBetName = "straight")
        {
            var sportTypes = string.Empty;
            sportName = !string.IsNullOrWhiteSpace(sportName) ? sportName.ToLower() : string.Empty;
            var title = string.Empty;
            var titleAll = string.Empty;

            switch (sportName)
            {
                case WagersSportName.Soccer:
                    sportTypes = SportGroup.Soccer;
                    title = Lang.soccer;
                    titleAll = Lang.lblsoccerAll;
                    break;
                case WagersSportName.Basketball:
                    sportTypes = SportGroup.Basketball_Football_Rugy;
                    title = Lang.Basketball;
                    titleAll = Lang.BasketballAll;
                    break;
                case WagersSportName.Tennis:
                    sportTypes = SportGroup.Tennis;
                    title = Lang.Tennis;
                    titleAll = Lang.lblTennisAll;
                    break;
                case WagersSportName.Volleyball:
                    sportTypes = SportGroup.Volleyball_Badminton_TableTennis;
                    title = Lang.Volleyball;
                    titleAll = Lang.lblVolleyballAll;
                    break;
                case WagersSportName.Baseball:
                    sportTypes = SportGroup.BaseBall;
                    title = Lang.Baseball;
                    titleAll = Lang.lblBaseballAll;
                    break;
                default:
                    sportTypes = SportGroup.Other;
                    break;
            }

            // AddDays(-9) : sample code for get data
            ViewBag.Leagues = this.GetLeague(DateTime.Now.AddDays(-9), sportTypes);
            ViewBag.GroupBetName = groupBetName;
            ViewBag.SportName = sportName;
            ViewBag.Title = title;
            ViewBag.TitleAll = titleAll;

            return View();
        }
    }
}
