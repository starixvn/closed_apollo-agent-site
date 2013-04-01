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

    public class TotalBetsController : BaseController
    {
        private readonly TotalBetsServiceBase _totalBetsService;

        public TotalBetsController()
        {
            _totalBetsService = new TotalBetsServiceBase();
        }
   
        protected IEnumerable<T> GetViewModel<T>(int agentId, int betType)
        {
            return _totalBetsService.GetReportData<T>(new TotalBetsCriteria
            {
                AgentId = agentId,
                BetType = betType
            });
        }

        public ViewResult Handicap()
        {
            var model = GetViewModel<HandicapViewModel>(agentId: 3, betType: 1);
            return View(model);
        }

        public ViewResult MoneyLine()
        {
            var model = GetViewModel<MoneyLineViewModel>(agentId: 3, betType: 22);
            return View(model);
        }

        public ViewResult TotalGoal()
        {
            var model = GetViewModel<TotalGoalViewModel>(agentId: 3, betType: 6);
            return View(model);
        }

        public ViewResult OddEven()
        {
            var model = GetViewModel<dynamic>(agentId: 3, betType: 2);
            return View(model);
        }
    }
}
