// --------------------------------------------------------------------------
// <copyright file="TotalGoalController.cs" company="Nexcel Solutions Vietnam">
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

    using Apollo.Age;
    using ViewModels;
    using Fanex.NAP.Framework.Data;

    public class TotalGoalController : TotalBetController
    {
        private readonly DynamicRepository _dynamicRepository;

        public TotalGoalController()
        {
            _dynamicRepository = new DynamicRepository();
        }

        public ActionResult Index()
        {
            IEnumerable<TotalGoalViewModel> model = _dynamicRepository.Fetch<TotalGoalViewModel>();
            return View(model);
        }
    }
}
