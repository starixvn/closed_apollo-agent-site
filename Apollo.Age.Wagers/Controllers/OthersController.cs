// --------------------------------------------------------------------------
// <copyright file="OthersController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.6.25">Created</change>
//   <change who="Edward.Nguyen" date="2013.6.27">Implement prototype</change>
// </history>


namespace Apollo.Age.Wagers.Controllers
{
    using System.Web.Mvc;
    using Apollo.Age.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OthersController : WagerController
    {
        //public ActionResult Index(string groupBetName = "")
        //{
        //    this.ActionName = string.IsNullOrWhiteSpace(groupBetName) ? this.ActionName : groupBetName;

        //    return base.Index(SportGroup.Other);
        //}

        public ActionResult Straight(int viewOption = 0, int leagueId = 0, int pageIndex = 0)
        {
            return View("~/Areas/Wagers/Views/Others/Straight.cshtml");
        }

        public ActionResult FirstHalf(int viewOption = 0, int leagueId = 0, int pageIndex = 0)
        {
            return View();
        }

        public ActionResult RB(int viewOption = 0, int leagueId = 0, int pageIndex = 0)
        {
            return View();
        }

        public ActionResult Correctscore(int viewOption = 0, int leagueId = 0, int pageIndex = 0)
        {
            return View();
        }

        public ActionResult FirstHalfCorrectScore(int viewOption = 0, int leagueId = 0, int pageIndex = 0)
        {
            return View();
        }

        public ActionResult HalfFull(int viewOption = 0, int leagueId = 0, int pageIndex = 0)
        {
            return View();
        }

        public ActionResult Total(int viewOption = 0, int leagueId = 0, int pageIndex = 0)
        {
            return View();
        }

        public ActionResult Parlay(int viewOption = 0, int leagueId = 0, int pageIndex = 0)
        {
            return View();
        }

        public ActionResult Start(int viewOption = 0, int leagueId = 0, int pageIndex = 0)
        {
            return View();
        }
    }
}
