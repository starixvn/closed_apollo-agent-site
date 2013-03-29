// --------------------------------------------------------------------------
// <copyright file="TotalBetController.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.Age.Web;
    using Apollo.Age.TotalBets.Services;
    using System.Web.Mvc;
    using Apollo.Age.TotalBets.ViewModels;
    using Apollo.Age.TotalBets.Criterias;

    public class TotalBetController : BaseController
    {
         private readonly TotalGoalService _totalGoalService;

         public TotalBetController()
        {
            _totalGoalService = new TotalGoalService();
        }

         public ActionResult TotalGoal()
        {
            IEnumerable<TotalGoalViewModel> model = _totalGoalService.GetReportData<TotalGoalViewModel>(new GetTotalGoalCriteria { AgentId = 3 });
            return View(model);
        }
    }
}
