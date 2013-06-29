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
    using Apollo.Age.Core;
    using Apollo.Age.Wagers.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BasketballController : BaseController
    {
        /// <summary>
        /// Index BaseketBall default is straight all
        /// </summary>
        /// <param name="bettype">Sample bettype = 1</param>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<League> listLeague = new List<League>();
            listLeague = this.GetLeague(DateTime.Now, SportGroup.Basketball_Football_Rugy);

            ViewBag.Leagues = listLeague;

            return View();
        }
    }
}
