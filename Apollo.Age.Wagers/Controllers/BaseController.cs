using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;
using Apollo.Age.Core;
using Apollo.Age.Entities;
using Apollo.Age.Wagers.Services;

namespace Apollo.Age.Wagers.Controllers
{
    public class BaseController : Controller
    {
        private readonly WagersService _wagerService;

        public string ActionName { get; set; }

        public BaseController()
            : this(null)
        {
            this.ActionName = "Straight";
        }

        public BaseController(WagersService wagerService = null)
        {
            _wagerService = wagerService ?? new WagersService();
        }

        public List<League> GetLeague(DateTime fromDate, string sportTypes = null)
        {
            var leagues = _wagerService.GetLeague(fromDate, sportTypes);

            return leagues != null ? leagues.ToList() : new List<League>();
        }

        [NonAction]
        public ActionResult BaseIndex(string sportTypes)
        {
            // AddDays(-9) : sample code for get data
            ViewBag.Leagues = this.GetLeague(DateTime.Now.AddDays(-9), sportTypes);
            ViewBag.GroupBetName = ActionName;

            return View();
        }
    }
}
