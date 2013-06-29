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
    using Apollo.Age.Core;
    using Apollo.Age.Wagers.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SoccerController : BaseController
    {
        private readonly WagersService _wagerService;

        public SoccerController()
            : this(null)
        {

        }

        public SoccerController(WagersService wagerService = null)
        {
            _wagerService = wagerService ?? new WagersService();
        }

        public ActionResult Index()
        {
            ViewBag.Leagues = GetLeague(DateTime.Now, WagerSports.Soccer);
            ViewBag.Title = Resources.Lang.lblStraight;
            return View();
        }

        public ActionResult GetDataContain(string bettype = WagersBettypes.Straight)
        {
            string bettypeText = string.Empty;
            string viewName = string.Empty;
            switch (bettype)
            {
                case WagersBettypes.Straight:
                    {
                        bettypeText = Resources.Lang.lblStraight;
                        viewName = "Straight";
                        break;
                    }
                case WagersBettypes.FirstHalf:
                    {
                        bettypeText = Resources.Lang.lbl1sthalf;
                        viewName = "FirstHalf";
                        break;
                    }
                case WagersBettypes.RB:
                    {
                        bettypeText = Resources.Lang.lblRB;
                        viewName = "RunningBall";
                        break;
                    }
                case WagersBettypes.SpecialBet:
                    {
                        bettypeText = Resources.Lang.lblSpecialbet;
                        viewName = "Specialbet";
                        break;
                    }
                case WagersBettypes.CorrectScore:
                    {
                        bettypeText = Resources.Lang.correctscore;
                        viewName = "Correctscore";
                        break;
                    }
                case WagersBettypes.FirstHalfCorrectScore:
                    {
                        bettypeText = Resources.Lang.lbl1stHalfCorrectScore;
                        viewName = "FirstHalfCorrectScore";
                        break;
                    }
                case WagersBettypes.HalfFull:
                    {
                        bettypeText = Resources.Lang.lblHalfFull;
                        viewName = "HalfFull";
                        break;
                    }
                case WagersBettypes.Total:
                    {
                        bettypeText = Resources.Lang.total;
                        viewName = "Total";
                        break;
                    }
                case WagersBettypes.Parlay:
                    {
                        bettypeText = Resources.Lang.lblParlay;
                        viewName = "Parlay";
                        break;
                    }
            }
            string url = string.Format("~/Areas/Wagers/Views/Soccer/{0}.cshtml", viewName);
            ViewBag.Title = bettypeText;

            return View(url);
        }
              
    }
}
