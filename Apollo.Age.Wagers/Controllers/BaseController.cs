using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apollo.Age.Wagers.Services;
using System.Web.Mvc;
using Apollo.Age.Core;

namespace Apollo.Age.Wagers.Controllers
{
    public class BaseController : System.Web.Mvc.Controller
    {
        private readonly WagersService _wagerService; 

        public BaseController() : this(null)
        { 
           
        }

        public BaseController(WagersService wagerService = null)
        {
            _wagerService = wagerService ?? new WagersService();
        }

        public List<SelectListItem> GetLeague()
        {
            var Leagues = _wagerService.GetLeague(DateTime.Now, SingbetSportConstant.Soccer);
            List<SelectListItem> listLeague = new List<SelectListItem>();
            foreach (var league in Leagues)
            {
                listLeague.Add(new SelectListItem
                {
                    Value = league.Id.ToString(),
                    Text = league.Name

                });
            }
            return listLeague;
        }
    }
}
