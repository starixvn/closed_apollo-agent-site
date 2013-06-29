// --------------------------------------------------------------------------
// <copyright file="BkNFLRugbyController.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.Age.Core;
    using Apollo.Age.Wagers.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BasketballController : BaseController
    {
        public ActionResult Index(string groupBetName = "")
        {
            this.ActionName = string.IsNullOrWhiteSpace(groupBetName) ? this.ActionName : groupBetName;

            return base.BaseIndex(WagersSportName.Basketball);
        }

        public ActionResult Straight(int viewOption = 0, int leagueId = 0, int pageIndex = 0)
        {
            return View();
        }

        public ActionResult FirstHalf(int viewOption = 0, int leagueId = 0, int pageIndex = 0)
        {
            return View();
        }

        public ActionResult RunningBall(int viewOption = 0, int leagueId = 0, int pageIndex = 0)
        {
            return View();
        }

        public ActionResult Parlay(int viewOption = 0, int leagueId = 0, int pageIndex = 0)
        {
            return View();
        }

        public ActionResult SpecialBet(int viewOption = 0, int leagueId = 0, int pageIndex = 0)
        {
            return View();
        }

    }
}
