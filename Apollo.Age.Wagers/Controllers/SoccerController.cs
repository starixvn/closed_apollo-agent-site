// --------------------------------------------------------------------------
// <copyright file="SoccerController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.6.25">Created</change>
//   <change who="Edward.Nguyen" date="2013.6.27">Implement prototype</change>
// </history>


namespace Apollo.Age.Wagers.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Apollo.Age.Entities;
    using Apollo.Age.Wagers.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SoccerController : Controller
    {
        private readonly WagersService _wagerService; 

        public SoccerController() : this(null)
        { 
           
        }

        public SoccerController(WagersService wagerService = null)
        {
            _wagerService = wagerService ?? new WagersService();
        }

        public ActionResult Index(string bettype = "1")
        {
            List<SelectListItem> listLeague = new List<SelectListItem>();
            for (int i = 0; i < 10; i++)
            {
                listLeague.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = "League " + i.ToString()
                });
            }

            ViewBag.Leagues = listLeague;
            ViewBag.Title = bettype;
            return View();
        }

        //protected List<SelectListItem> GetLeague()
        //{
        //    var Leagues = _wagerService.GetLeague(DateTime.Now, Apollo.Age.Core.SingbetConstants.Soccer);
        //    return Leagues.ToList();
        //}

        public ActionResult Straight()
        {
            return View();
        }

        public ActionResult FirstHalf()
        {
            return View();
        }

        public ActionResult Specialbet()
        {
            return View();
        }

        public ActionResult Correctscore()
        {
            return View();
        }

        public ActionResult FirstHalfCorrectScore()
        {
            return View();
        }

        public ActionResult Total()
        {
            return View();
        }

        public ActionResult HalfFull()
        {
            return View();
        }

        public ActionResult Parlay()
        {
            return View();
        }

        public ActionResult RunningBall()
        {
            return View();
        }
    }
}
