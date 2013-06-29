using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apollo.Age.Wagers.Services;
using System.Web.Mvc;
using Apollo.Age.Core;
using Apollo.Age.Entities;

namespace Apollo.Age.Wagers.Controllers
{
    public class BaseController : System.Web.Mvc.Controller
    {
        private readonly WagersService _wagerService;

        public BaseController()
            : this(null)
        {

        }

        public BaseController(WagersService wagerService = null)
        {
            _wagerService = wagerService ?? new WagersService();
        }

        public List<League> GetLeague(string sportTypes = null)
        {
            var leagues = _wagerService.GetLeague(sportTypes);

            return leagues.ToList();
        }
    }
}
