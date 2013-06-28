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
    using Apollo.Age.Entities;
    using Wagers.Core;
    using Age.Resources;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HomeWagersController : Controller
    {
        /// <summary>
        /// Index BaseketBall default is straight all
        /// </summary>
        /// <param name="betType">Sample bettype = 1</param>
        /// <returns></returns>
        public ActionResult Index(string sportType, string betType = "")
        {
            string title = string.Empty;
            switch (sportType)
            {
                case WagersSporttypes.SC:
                    title = Lang.soccer;
                    betType = string.IsNullOrWhiteSpace(betType) ? WagersBettypes.Straight : betType;
                    break;
                case WagersSporttypes.BK_NFL_Rugby:
                    title = Lang.Basketball;
                    break;
                case WagersSporttypes.TN:
                    title = Lang.Tennis;
                    break;
                case WagersSporttypes.VB:
                    title = Lang.Volleyball;
                    break;
                case WagersSporttypes.BS:
                    title = Lang.Baseball;
                    break;
                default:
                    title = Lang.other;
                    break;
            }

            List<League> listLeague = new List<League>();
            for (int i = 0; i < 10; i++)
            {
                listLeague.Add(new League
                {
                    Id = i,
                    Name = "League " + i.ToString()
                });
            }

            ViewBag.Leagues = listLeague;
            ViewBag.Title = title;
            ViewBag.SportType = sportType;
            return View();
        }
    }
}
