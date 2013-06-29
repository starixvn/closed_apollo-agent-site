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

        public ActionResult Index(string bettype = "1")
        {

            ViewBag.Leagues = GetLeague(DateTime.Now, SingbetSportConstant.Soccer);
            string bettypeText = string.Empty;
            
            switch (bettype)
            {
                case WagersBettypes.Straight:
                    {
                        bettypeText = Resources.Lang.lblStraight;
                        break;
                    }
                case WagersBettypes.FirstHalf:
                    {
                        bettypeText = Resources.Lang.lbl1sthalf;
                        break;
                    }
                case WagersBettypes.RB:
                    {
                        bettypeText = Resources.Lang.lblRB;
                        break;
                    }
                case WagersBettypes.SpecialBet:
                    {
                        bettypeText = Resources.Lang.lblSpecialbet;
                        break;
                    }
                case WagersBettypes.CorrectScore:
                    {
                        bettypeText = Resources.Lang.correctscore;
                        break;
                    }
                case WagersBettypes.FirstHalfCorrectScore:
                    {
                        bettypeText = Resources.Lang.lbl1stHalfCorrectScore;
                        break;
                    }
                case WagersBettypes.HalfFull:
                    {
                        bettypeText = Resources.Lang.lblHalfFull;
                        break;
                    }
                case WagersBettypes.Total:
                    {
                        bettypeText = Resources.Lang.total;
                        break;
                    }
                case WagersBettypes.Parlay:
                    {
                        bettypeText = Resources.Lang.lblParlay;
                        break;
                    }
            }
           // string url = string.Format("~/Views/Results/{0}.cshtml", viewName);
            ViewBag.Title = bettypeText;
            return View();
        }
              
    }
}
