// --------------------------------------------------------------------------
// <copyright file="TotalBetController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.27">Created</change>
//   <change who="Anders.Le" date="2013.4.9">Change input param of GetModel to Enum</change>
// </history>

namespace Apollo.Age.TotalBets.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Apollo.Age.Enumerations;
    using Apollo.Age.TotalBets.Criterias;
    using Apollo.Age.TotalBets.Models;
    using Apollo.Age.TotalBets.Services;
    using Apollo.Age.Web;

    public class TotalBetsController : BaseController
    {
        private readonly TotalBetsService _totalBetsService;

        public TotalBetsController()
        {
            _totalBetsService = new TotalBetsService();
        }
   
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CurrentSession.IsSuper)
            {
                filterContext.Result = new HttpNotFoundResult();
            }
        }

        /// <summary>
        /// Get totalbet data by bettype
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="betType"></param>
        /// <returns></returns>
        protected IEnumerable<T> GetModel<T>(BetTypes betType)
        {
            return _totalBetsService.GetData<T>(new TotalBetsCriteria
            {
                AgentId = this.CurrentSession.AgentId,
                BetType = (int)betType
            });
        }

        public ViewResult Handicap()
        {
            var model = GetModel<HandicapModel>(BetTypes.Handicap);
            return View(model);
        }

        public ViewResult MoneyLine()
        {
            var model = GetModel<MoneyLineModel>(BetTypes.Money_Line);
            return View(model);
        }

        public ViewResult TotalGoal()
        {
            var model = GetModel<TotalGoalModel>(BetTypes.Total_Goal);
            return View(model);
        }

        public ViewResult OddEven()
        {
            var model = GetModel<OddEvenModel>(BetTypes.Odd_Even);
            return View(model);
        }

        public ViewResult HTFT()
        {
            var model = GetModel<HTFTModel>(BetTypes.HT_FT);
            return View(model);
        }

        public ViewResult OutRight()
        {
            var model = GetModel<OutRightModel>(BetTypes.Outright);
            return View(model);
        }

        //public ViewResult NumberGame()
        //{
        //    var model = GetModel<NumberGameModel>(betType: 161);
        //    return View(model);
        //}

        public ViewResult HomeNoBet()
        {
            var model = GetModel<HomeNoBetModel>(BetTypes.Draw_No_Bet);
            return View(model);
        }

        public ViewResult FirstGoal()
        {
            var model = GetModel<FirstGoalModel>(BetTypes.First_Goal_Last_Goal);
            return View(model);
        }
    }
}
