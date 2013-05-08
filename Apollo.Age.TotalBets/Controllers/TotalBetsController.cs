// --------------------------------------------------------------------------
// <copyright file="TotalBetController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.27">Created</change>
//   <change who="Anders.Le" date="2013.4.9">Change input param of GetModel to Enum</change>
//   <change who="Ricky.Nguyen" date="2013.4.29">Update SuperView</change>
//   <change who="Ricky.Nguyen" date="2013.5.8">Comment Code</change>
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
    using Apollo.Age.Criterias;
    using System.Linq;
    using Apollo.Age.Entities;
    using System.Web.SessionState;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class TotalBetsController : BaseController
    {
        private readonly TotalBetsService _totalBetsService;

        public TotalBetsController()
        {
            _totalBetsService = new TotalBetsService();
        }
   
        /// <summary>
        /// Get totalbet data by bettype, if MainShareHolderId > 0 and Current is Supper
        /// then load Data of that MainShareHolder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="betType"></param>
        /// <returns></returns>
        private IEnumerable<T> GetModel<T>(BetTypes betType, int mainShareHolderId)
        {
            int agentId = base.CurrentSession.AgentId;
            if (base.CurrentSession.IsSuper)
            {
                var mainShareHolders = _totalBetsService.GetData<Agent>(new GetAllMainShareHolderCriteria());
                if (mainShareHolderId > 0)
                {
                    agentId = mainShareHolderId;
                }
                else if (mainShareHolders != null && mainShareHolders.Any())
                {
                    agentId = mainShareHolderId = mainShareHolders.ElementAt(0).Id;
                }
                ViewBag.CurrentMainShareHolderId = mainShareHolderId;
                ViewBag.MainShareHolders = mainShareHolders;
            }
            ViewBag.CurrentAgentId = base.CurrentSession.AgentId;

            return _totalBetsService.GetData<T>(new TotalBetsCriteria
            {
                AgentId = agentId,
                BetType = (int)betType
            });
        }

        /// <summary>
        /// Load Handicap View
        /// </summary>
        /// <param name="mainShareHolderId"></param>
        /// <returns></returns>
        public ViewResult Handicap(int mainShareHolderId = 0)
        {
            var model = GetModel<HandicapModel>(BetTypes.Handicap, mainShareHolderId);
            return View(model);
        }

        /// <summary>
        /// Load Money Line View
        /// </summary>
        /// <param name="mainShareHolderId"></param>
        /// <returns></returns>
        public ViewResult MoneyLine(int mainShareHolderId = 0)
        {
            var model = GetModel<MoneyLineModel>(BetTypes.Money_Line, mainShareHolderId);
            return View(model);
        }

        /// <summary>
        /// Load Total Goal
        /// </summary>
        /// <param name="mainShareHolderId"></param>
        /// <returns></returns>
        public ViewResult TotalGoal(int mainShareHolderId = 0)
        {
            var model = GetModel<TotalGoalModel>(BetTypes.Total_Goal, mainShareHolderId);
            return View(model);
        }

        /// <summary>
        /// Load Odd Even
        /// </summary>
        /// <param name="mainShareHolderId"></param>
        /// <returns></returns>
        public ViewResult OddEven(int mainShareHolderId = 0)
        {
            var model = GetModel<OddEvenModel>(BetTypes.Odd_Even, mainShareHolderId);
            return View(model);
        }

        /// <summary>
        /// Load HTFT
        /// </summary>
        /// <param name="mainShareHolderId"></param>
        /// <returns></returns>
        public ViewResult HTFT(int mainShareHolderId = 0)
        {
            var model = GetModel<HTFTModel>(BetTypes.HT_FT, mainShareHolderId);
            return View(model);
        }

        /// <summary>
        /// Load OutRight
        /// </summary>
        /// <param name="mainShareHolderId"></param>
        /// <returns></returns>
        public ViewResult OutRight(int mainShareHolderId = 0)
        {
            var model = GetModel<OutRightModel>(BetTypes.Outright, mainShareHolderId);
            return View(model);
        }

        //public ViewResult NumberGame()
        //{
        //    var model = GetModel<NumberGameModel>(betType: 161);
        //    return View(model);
        //}

        /// <summary>
        /// Load Home No Bet, Draw No Bet
        /// </summary>
        /// <param name="mainShareHolderId"></param>
        /// <returns></returns>
        public ViewResult HomeNoBet(int mainShareHolderId = 0)
        {
            var model = GetModel<HomeNoBetModel>(BetTypes.Draw_No_Bet, mainShareHolderId);
            return View(model);
        }

        /// <summary>
        /// Load FGLG
        /// </summary>
        /// <param name="mainShareHolderId"></param>
        /// <returns></returns>
        public ViewResult FirstGoal(int mainShareHolderId = 0)
        {
            var model = GetModel<FirstGoalModel>(BetTypes.First_Goal_Last_Goal, mainShareHolderId);
            return View(model);
        }
    }
}
