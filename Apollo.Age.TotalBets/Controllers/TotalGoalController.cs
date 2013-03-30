// --------------------------------------------------------------------------
// <copyright file="TotalGoalController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.27">Created</change>
//   <change who="Edward.Nguyen" date="2013.3.29">Implement</change>
// </history>

namespace Apollo.Age.TotalBets.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.NAP.Framework.Data;
    using ViewModels;
    using Apollo.Age.TotalBets.Services;
    using Apollo.Age.TotalBets.Criterias;

    public class TotalGoalController : TotalBetController
    {       
        public TotalGoalController() : base()
        {
        
        }

        public ActionResult Index()
        {
            IEnumerable<TotalGoalViewModel> model = TotalBetService.GetReportData<TotalGoalViewModel>(new TotalGoalCriteria { AgentId = 3 });
            return TotalBetsView(model);
        }
    }
}
