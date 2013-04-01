// --------------------------------------------------------------------------
// <copyright file="MoneyLineController.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.Age.TotalBets.ViewModels;
    using Apollo.Age.TotalBets.Criterias;

    public class MoneyLineController : TotalBetController
    {
        public MoneyLineController()
            : base()
        {
        
        }

        public ActionResult Index()
        {
            IEnumerable<MoneyLineViewModel> model = TotalBetService
                .GetReportData<MoneyLineViewModel>(new TotalBetsCriteria { AgentId = 3, BetType = 20 });

            return TotalBetsView(model);
        }
    }
}
