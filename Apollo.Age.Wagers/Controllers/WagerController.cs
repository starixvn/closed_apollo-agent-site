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

            switch (sportName)
            {
                case WagersSportName.Soccer:
                    sportTypes = SportGroup.Soccer;
                    break;
                case WagersSportName.Basketball:
                    sportTypes = SportGroup.Basketball_Football_Rugy;
                    break;
                case WagersSportName.Tennis:
                    sportTypes = SportGroup.Tennis;
                    break;
                case WagersSportName.Volleyball:
                    sportTypes = SportGroup.Volleyball_Badminton_TableTennis;
                    break;
                case WagersSportName.Baseball:
                    sportTypes = SportGroup.BaseBall;
                    break;
                default:
                    sportTypes = SportGroup.Other;
                    break;
            }

            // AddDays(-9) : sample code for get data
            ViewBag.Leagues = this.GetLeague(DateTime.Now.AddDays(-9), sportTypes);
            ViewBag.GroupBetName = groupBetName;
            ViewBag.SportName = sportName;

            return View();
        }

        #region Private

        private List<SelectListItem> GenerateSoccerCommissionGroup()
        {
            var list = new List<SelectListItem>();

            list.Add(new SelectListItem
            {
                Value = "",
                Text = Lang.lblsoccerAll
            });

            list.Add(new SelectListItem
            {
                Value = "A",
                Text = Lang.lblsoccerA
            });

            list.Add(new SelectListItem
            {
                Value = "B",
                Text = Lang.lblsoccerB
            });

            list.Add(new SelectListItem
            {
                Value = "C",
                Text = Lang.lblsoccerC
            });

            list.Add(new SelectListItem
            {
                Value = "D",
                Text = Lang.lblsoccerD
            });

            return list;
        }

        private List<SelectListItem> GenerateBasketballCommissionGroup()
        {
            var list = new List<SelectListItem>();

            list.Add(new SelectListItem
            {
                Value = "",
                Text = Lang.Basketball
            });

            list.Add(new SelectListItem
            {
                Value = "A",
                Text = Lang.BasketballA
            });

            list.Add(new SelectListItem
            {
                Value = "B",
                Text = Lang.Basketball
            });

            list.Add(new SelectListItem
            {
                Value = "C",
                Text = Lang.Basketball
            });

            list.Add(new SelectListItem
            {
                Value = "D",
                Text = Lang.Basketball
            });

            return list;
        }

        private List<SelectListItem> GenerateBaseballCommissionGroup()
        {
            var list = new List<SelectListItem>();

            list.Add(new SelectListItem
            {
                Value = "",
                Text = Lang.lblBaseballAll
            });

            list.Add(new SelectListItem
            {
                Value = "A",
                Text = Lang.lblBaseballA
            });

            list.Add(new SelectListItem
            {
                Value = "B",
                Text = Lang.lblBaseballB
            });

            list.Add(new SelectListItem
            {
                Value = "C",
                Text = Lang.lblBaseballC
            });

            list.Add(new SelectListItem
            {
                Value = "D",
                Text = Lang.lblBaseballD
            });

            return list;
        }
        
        #endregion
    }
}
