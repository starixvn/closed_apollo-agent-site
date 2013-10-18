// --------------------------------------------------------------------------
// <copyright file="TotalBetsController.cs" company="Nexcel Solutions Vietnam">
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
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;
    using Apollo.Age.Enumerations;
    using Apollo.Age.TotalBets.Criterias;
    using Apollo.Age.TotalBets.Models;
    using Apollo.Age.TotalBets.Services;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Web.Attribute;
    using Apollo.Age.Core;

    /// <summary>
    /// Total Bets Controller.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    [ApolloAuth(FunctionKey = ApolloFunctionKeys.Forecast, Policy = Policy.Query)]
    public class TotalBetsController : BaseController
    {
        private readonly TotalBetsService _totalBetsService;

        public TotalBetsController()
        {
            _totalBetsService = new TotalBetsService();
        }

        /// <summary>
        /// Index Page.
        /// </summary>
        /// <returns>View return.</returns>
        public ActionResult Index()
        {
            ViewBag.IsShowRefreshNow = CurrentSession.AgentLevel <= 1; //show button Refresh Now once logged in by Super and MainShareHolder
            return View();
        }

        /// <summary>
        /// Load Handicap View.
        /// </summary>
        /// <param name="mainShareHolderId">Get current main shareholder data if id > 0.</param>
        /// <returns>View return.</returns>
        public ViewResult Handicap(int mainShareHolderId = 0)
        {
            var model = GetModel<HandicapModel>(BetTypes.Handicap, mainShareHolderId);
            return View(model);
        }

        /// <summary>
        /// Load Money Line View.
        /// </summary>
        /// <param name="mainShareHolderId">Get current main shareholder data if id > 0.</param>
        /// <returns>View return.</returns>
        public ViewResult MoneyLine(int mainShareHolderId = 0)
        {
            var model = GetModel<MoneyLineModel>(BetTypes.Money_Line, mainShareHolderId);
            return View(model);
        }

        /// <summary>
        /// Load Total Goal.
        /// </summary>
        /// <param name="mainShareHolderId">Get current main shareholder data if id > 0.</param>
        /// <returns>View return.</returns>
        public ViewResult TotalGoal(int mainShareHolderId = 0)
        {
            var model = GetModel<TotalGoalModel>(BetTypes.Total_Goal, mainShareHolderId);
            return View(model);
        }

        /// <summary>
        /// Load Odd Even.
        /// </summary>
        /// <param name="mainShareHolderId">Get current main shareholder data if id > 0.</param>
        /// <returns>View return.</returns>
        public ViewResult OddEven(int mainShareHolderId = 0)
        {
            var model = GetModel<OddEvenModel>(BetTypes.Odd_Even, mainShareHolderId);
            return View(model);
        }

        /// <summary>
        /// Load HTFT page.
        /// </summary>
        /// <param name="mainShareHolderId">Get current main shareholder data if id > 0.</param>
        /// <returns>View return.</returns>
        public ViewResult HTFT(int mainShareHolderId = 0)
        {
            var model = GetModel<HTFTModel>(BetTypes.HT_FT, mainShareHolderId);
            return View(model);
        }

        /// <summary>
        /// Load OutRight page.
        /// </summary>
        /// <param name="mainShareHolderId">Get current main shareholder data if id > 0.</param>
        /// <returns>View return.</returns>
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
        /// Load Home No Bet, Draw No Bet.
        /// </summary>
        /// <param name="mainShareHolderId">Get current main shareholder data if id > 0.</param>
        /// <returns>View return.</returns>
        public ViewResult HomeNoBet(int mainShareHolderId = 0)
        {
            var model = GetModel<HomeNoBetModel>(BetTypes.Draw_No_Bet, mainShareHolderId);
            return View(model);
        }

        /// <summary>
        /// Load FGLG page.
        /// </summary>
        /// <param name="mainShareHolderId">Get current main shareholder data if id > 0.</param>
        /// <returns>View return.</returns>
        public ViewResult FirstGoal(int mainShareHolderId = 0)
        {
            var model = GetModel<FirstGoalModel>(BetTypes.First_Goal_Last_Goal, mainShareHolderId);
            return View(model);
        }

        /// <summary>
        /// Get total bet data by bet type, if MainShareHolderId > 0 and Current is Supper
        /// then load Data of that MainShareHolder.
        /// </summary>
        /// <typeparam name="T">Return Data.</typeparam>
        /// <param name="betType">Bet Type gets data.</param>
        /// <param name="mainShareHolderId">Main share holder id for get data.</param>
        /// <returns>List of total bet data.</returns>
        private IEnumerable<T> GetModel<T>(BetTypes betType, int mainShareHolderId)
        {
            int agentId = this.CurrentSession.AgentId;
            if (this.CurrentSession.IsSuper)
            {
                var mainShareHolders = _totalBetsService.GetData<Agent>(new GetAllMainShareHolderCriteria(betType));
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

            ViewBag.CurrentAgentId = this.CurrentSession.AgentId;
            ViewBag.IsSuper = this.CurrentSession.IsSuper;
            return _totalBetsService.GetData<T>(new TotalBetsCriteria
            {
                AgentId = agentId,
                BetType = (int)betType,
                LoggedId = this.CurrentSession.AgentId
            });
        }
    }
}
