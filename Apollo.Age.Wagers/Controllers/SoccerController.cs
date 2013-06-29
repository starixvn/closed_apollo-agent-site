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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SoccerController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Straight()
        {
            //ViewBag.Leagues = GetLeague(DateTime.Now, SingbetSportConstant.Soccer);
            //ViewBag.Title = Resources.Lang.lblStraight;
            return View();
        }

        public ActionResult RunningBall()
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

        public ActionResult Start()
        {
            return View();
        }
    }
}
