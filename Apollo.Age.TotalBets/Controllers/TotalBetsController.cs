// --------------------------------------------------------------------------
// <copyright file="TotalBetController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.27">Created</change>
//   <change who="Anders.Le" date="2013.4.9">Change input param of GetViewModel to Enum</change>
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
    using Apollo.Age.Enumerations;

    public class TotalBetsController : BaseController
    {
        private readonly TotalBetsService _totalBetsService;

        public TotalBetsController()
        {
            _totalBetsService = new TotalBetsService();
        }
   
        /// <summary>
        /// Get totalbet data by bettype
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="betType"></param>
        /// <returns></returns>
        protected IEnumerable<T> GetViewModel<T>(BetTypes betType)
        {
            return _totalBetsService.GetData<T>(new TotalBetsCriteria
            {
                AgentId = this.CurrentSession.AgentId,
                BetType = (int)betType
            });
        }

        public ViewResult Handicap()
        {
            var model = GetViewModel<HandicapViewModel>(BetTypes.Handicap);
            return View(model);
        }

        public ViewResult MoneyLine()
        {
            var model = GetViewModel<MoneyLineViewModel>(BetTypes.Money_Line);
            return View(model);
        }

        public ViewResult TotalGoal()
        {
            var model = GetViewModel<TotalGoalViewModel>(BetTypes.Total_Goal);
            return View(model);
        }

        public ViewResult OddEven()
        {
            var model = GetViewModel<OddEvenViewModel>(BetTypes.Odd_Even);
            return View(model);
        }

        public ViewResult HTFT()
        {
            var model = GetViewModel<HTFTViewModel>(BetTypes.HT_FT);
            return View(model);
        }

        public ViewResult OutRight()
        {
            var model = GetViewModel<OutRightViewModel>(BetTypes.Outright);
            return View(model);
        }

        //public ViewResult NumberGame()
        //{
        //    var model = GetViewModel<NumberGameViewModel>(betType: 161);
        //    return View(model);
        //}

        public ViewResult HomeNoBet()
        {
            var model = GetViewModel<HomeNoBetViewModel>(BetTypes.Draw_No_Bet);
            return View(model);
        }

        public ViewResult FirstGoal()
        {
            var model = GetViewModel<FirstGoalViewModel>(BetTypes.First_Goal_Last_Goal);
            return View(model);
        }
    }
}
