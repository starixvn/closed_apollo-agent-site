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
    using Sunplus.Agent.Model.Entities;

    public class TotalBetsController : BaseController
    {
        private readonly TotalBetsService _totalBetsService;

        public TotalBetsController()
        {
            _totalBetsService = new TotalBetsService();
        }
   
        protected IEnumerable<T> GetViewModel<T>(int betType)
        {
            return _totalBetsService.GetReportData<T>(new TotalBetsCriteria
            {
                AgentId = UserSession.Current.AgentId,
                BetType = betType
            });
        }

        public ViewResult Handicap()
        {
            var model = GetViewModel<HandicapViewModel>(betType: 1);
            return View(model);
        }

        public ViewResult MoneyLine()
        {
            var model = GetViewModel<MoneyLineViewModel>(betType: 20);
            return View(model);
        }

        public ViewResult TotalGoal()
        {
            var model = GetViewModel<TotalGoalViewModel>(betType: 6);
            return View(model);
        }

        public ViewResult OddEven()
        {
            var model = GetViewModel<OddEvenViewModel>(betType: 2);
            return View(model);
        }

        public ViewResult HTFT()
        {
            var model = GetViewModel<HTFTViewModel>(betType: 16);
            return View(model);
        }

        public ViewResult OutRight()
        {
            var model = GetViewModel<OutRightViewModel>(betType: 10);
            return View(model);
        }

        public ViewResult NumberGame()
        {
            var model = GetViewModel<NumberGameViewModel>(betType: 161);
            return View(model);
        }

        public ViewResult HomeNoBet()
        {
            var model = GetViewModel<HomeNoBetViewModel>(betType: 25);
            return View(model);
        }

        public ViewResult FirstGoal()
        {
            var model = GetViewModel<FirstGoalViewModel>(betType: 14);
            return View(model);
        }
    }
}
