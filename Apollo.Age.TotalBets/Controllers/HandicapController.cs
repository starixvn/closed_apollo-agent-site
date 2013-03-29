// --------------------------------------------------------------------------
// <copyright file="HandicapController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.27">Created</change>
// </history>

namespace Apollo.Age.TotalBets.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// Handle all requests regarding of Handicap, Over Under Live
    /// </summary>
    public class HandicapController : TotalBetController
    {
        public ActionResult Index()
        {
            return View();
        }        
    }
}
