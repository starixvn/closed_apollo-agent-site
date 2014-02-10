// --------------------------------------------------------------------------
// <copyright file="BetListBaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2014.2.7">Created</change>
// </history>

namespace Apollo.Age.BetList.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Services;
    using Apollo.Age.BetList.ViewModels;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Resources;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.TotalBets.Services;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Services;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.TicketHelper;

    /// <summary>
    /// Bet list base controller class.
    /// </summary>
    public class BetListBaseController : BaseController
    {
        private readonly BetListService _betListService;
        private readonly ReportService _reportService;
        private readonly TotalBetsService _totalBetsService;
        private readonly IUserService _userService;
        private readonly PeriodDateRangeService _periodDateRangeSerivce;
        private readonly ExcelService _excelService;

        public BetListBaseController()
        {
            this._betListService = new BetListService();
            this._reportService = new ReportService();
            this._totalBetsService = new TotalBetsService();
            _userService = new UserService();
            _periodDateRangeSerivce = new PeriodDateRangeService();
            this._excelService = new ExcelService();
        }

        /// <summary>
        /// Forecasts the bet list.
        /// </summary>
        /// <param name="title">The title bet list.</param>
        /// <param name="strBetType">Type of the string bet.</param>
        /// <param name="type">The type bet list.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="betTeam">The bet team.</param>
        /// <param name="matchId">The match identifier.</param>
        /// <param name="mainShareHolderId">The main share holder identifier.</param>
        /// <param name="mainShareHolderUsername">The main share holder username.</param>
        /// <param name="isForecast">If set to <c>true</c> [is forecast].</param>
        /// <returns>Return ticket result.</returns>
        public TicketResult ForecastBetlist(
            ref string title,
            ref string strBetType,
            string type = "",
            string betType = "",
            string betTeam = "",
            int matchId = 0,
            int mainShareHolderId = 0,
            string mainShareHolderUsername = "",
            bool isForecast = false)
        {
            if (type.Equals("correctscore", StringComparison.OrdinalIgnoreCase))
            {
                title = CacheRef.GetBettype((int)BetTypes.Correct_Score);
                strBetType = string.Format("{0},{1}", (int)BetTypes.Correct_Score, (int)BetTypes.FirstCorrectScore);
            }
            else if (type.Equals("mixparlay", StringComparison.OrdinalIgnoreCase))
            {
                strBetType = Convert.ToString((int)BetTypes.MixParlay);
                title = CacheRef.GetBettype((int)BetTypes.MixParlay);
            }
            else if (type.Equals("combmixparlaybetlist", StringComparison.OrdinalIgnoreCase))
            {
                strBetType = Convert.ToString((int)BetTypes.Combine_Mix_Parlay);
                title = Lang.lblSystemParlay;
            }
            else
            {
                strBetType = betType;
                title = CacheRef.GetBettype(betType);
            }

            var betListService = new BetListService();
            var criteria = new BetListCriteria
            {
                LoggedId = CurrentSession.AgentId,
                AgentId = mainShareHolderId,
                BetTypes = strBetType,
                MatchId = matchId,
                BetTeam = betTeam
            };

            TicketResult mainTickets;

            if (matchId > 0)
            {
                mainTickets = betListService.GetRunningByBettypeAndMatch(criteria);
            }
            else
            {
                mainTickets = betListService.GetRunningByBettype(criteria);
            }

            return mainTickets;
        }

        /// <summary>
        /// Win lost the bet list.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="fromDate">The from date.</param>
        /// <param name="toDate">The to date.</param>
        /// <param name="type">The type bet list.</param>
        /// <param name="username">The username.</param>
        /// <param name="mainShareHolderId">The main share holder identifier.</param>
        /// <param name="mainShareHolderUsername">The main share holder username.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="sportType">Type of the sport.</param>
        /// <param name="sportName">Name of the sport.</param>
        /// <param name="betTypeName">Name of the bet type.</param>
        /// <returns>Ticket result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public TicketResult WinlossBetlist(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            string type = "",
            string username = "",
            int mainShareHolderId = 0,
            string mainShareHolderUsername = "",
            string betType = "",
            string sportType = "",
            string sportName = "",
            string betTypeName = "")
        {
            var statusIds = string.Empty;
            if (type.Equals("report", StringComparison.OrdinalIgnoreCase))
            {
                statusIds = string.Format(
                      "{0},{1},{2},{3},{4},{5}",
                      (int)Apollo.Age.Enumerations.BetStatus.Void,
                      (int)Apollo.Age.Enumerations.BetStatus.Reject,
                      (int)Apollo.Age.Enumerations.BetStatus.Refund,
                      (int)Apollo.Age.Enumerations.BetStatus.Draw,
                      (int)Apollo.Age.Enumerations.BetStatus.Won,
                      (int)Apollo.Age.Enumerations.BetStatus.Lose);
            }
            else
            {
                statusIds = string.Format(
                         "{0},{1}",
                         (int)Apollo.Age.Enumerations.BetStatus.Running,
                         (int)Apollo.Age.Enumerations.BetStatus.Waiting);
            }

            var balanceUpDay = _reportService.GetBalanceUpDay();
            var moveBetsDay = _reportService.MoveBetsDay();
            var betListService = new BetListService();
            var criteria = new BetListCriteria
            {
                AgentId = -1,
                LoggedId = CurrentSession.AgentId,
                CustId = custId,
                FromDate = this.ResetStartDate(fromDate),
                ToDate = this.ResetEndDate(toDate),
                StatusIds = statusIds,
                BetTypes = betType,
                SportTypes = sportType == "-99" ? "0" : sportType,
                BalanceUpDay = balanceUpDay,
                MoveBetsDay = moveBetsDay
            };

            TicketResult mainTickets;

            if (type.Equals("report", StringComparison.OrdinalIgnoreCase))
            {
                mainTickets = betListService.GetWinLossByDateAndMem(criteria);
            }
            else
            {
                mainTickets = betListService.GetTicketResult(criteria);
            }

            return mainTickets;
        }

        /// <summary>
        /// Transactions the bet list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Ticket result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public TicketResult TransactionBetlist(BetListReportViewModel model)
        {
            var statusIds = string.Format(
                    "{0},{1},{2},{3},{4},{5},{6},{7}",
                    (int)Apollo.Age.Enumerations.BetStatus.Void,
                    (int)Apollo.Age.Enumerations.BetStatus.Reject,
                    (int)Apollo.Age.Enumerations.BetStatus.Running,
                    (int)Apollo.Age.Enumerations.BetStatus.Waiting,
                    (int)Apollo.Age.Enumerations.BetStatus.Refund,
                    (int)Apollo.Age.Enumerations.BetStatus.Draw,
                    (int)Apollo.Age.Enumerations.BetStatus.Won,
                    (int)Apollo.Age.Enumerations.BetStatus.Lose);
            var balanceUpDay = _reportService.GetBalanceUpDay();
            int agentId, custId;
            int errorCode;
            model.Username = !string.IsNullOrEmpty(model.Username) ? model.Username.Trim() : CurrentSession.AgentName;
            _betListService.GetUserIdByUserName(CurrentSession.AgentId, model.UserType == 1, model.Username, out agentId, out custId, out errorCode);

            model.SportList = SingbetSportService.GetSport(isOther: false);
            model.BettypeList =
                model.SportTypes == "-99"
                ? _reportService.GetParlayBettype()
                : _reportService.GetBettype(model.SportTypes == "-99" || model.SportTypes == "0");
            model.BalanceUpDay = balanceUpDay;
            TicketResult mainTickets = null;

            if (errorCode == 0)
            {
                var betListService = new BetListService();
                var criteria = new BetListCriteria
                {
                    AgentId = agentId,
                    LoggedId = CurrentSession.AgentId,
                    CustId = custId,
                    FromDate = this.ResetStartDate(model.FromDate),
                    ToDate = this.ResetEndDate(model.ToDate),
                    StatusIds = statusIds,

                    SportTypes = model.SportTypes == "-99" ? "0" : model.SportTypes,
                    BetTypes = model.BetTypes,

                    BetListPage = BetListPage.Transaction,
                    MoveBetsDay = _reportService.MoveBetsDay()
                };

                //// Cache Key
                var sessionKey = string.Format(
                    "TransactionsReport{0}-{1}-{2}",
                    criteria.LoggedId,
                    criteria.AgentId,
                    criteria.CustId);

                var getFromDB = true;
                if (model.FromPaging == true)
                {
                    var resultFromSession = HttpContext.Session[sessionKey] as TicketResult;
                    if (resultFromSession != null)
                    {
                        mainTickets = resultFromSession.Clone();
                        getFromDB = false;
                    }
                }

                if (getFromDB)
                {
                    mainTickets = betListService.GetTicketResult(criteria);
                    HttpContext.Session[sessionKey] = mainTickets.Clone();
                }
            }

            return mainTickets;
        }

        /// <summary>
        /// Tickets the cancel bet list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Ticket result.</returns>
        public TicketResult TicketCancelBetList(BetListReportViewModel model)
        {
            var statusIds = string.Format("{0},{1}", (int)Apollo.Age.Enumerations.BetStatus.Void, (int)Apollo.Age.Enumerations.BetStatus.Reject);
            var balanceUpDay = _reportService.GetBalanceUpDay();
            var moveBetsDay = _reportService.MoveBetsDay();
            int agentId, custId;
            int errorCode;

            model.Username = !string.IsNullOrEmpty(model.Username) ? model.Username.Trim() : CurrentSession.AgentName;
            _betListService.GetUserIdByUserName(CurrentSession.AgentId, model.UserType == 1, model.Username, out agentId, out custId, out errorCode);

            model.SportList = SingbetSportService.GetSport(isOther: false);
            model.BettypeList =
                model.SportTypes == "-99"
                ? _reportService.GetParlayBettype()
                : _reportService.GetBettype(model.SportTypes == "-99" || model.SportTypes == "0");
            model.BalanceUpDay = balanceUpDay;
            model.MoveBetDay = moveBetsDay;

            TicketResult mainTickets = null;

            if (errorCode == 0)
            {
                var betListService = new BetListService();
                var criteria = new BetListCriteria
                {
                    AgentId = agentId,
                    LoggedId = CurrentSession.AgentId,
                    CustId = custId,
                    FromDate = this.ResetStartDate(model.FromDate),
                    ToDate = this.ResetEndDate(model.ToDate),
                    StatusIds = statusIds,

                    SportTypes = model.SportTypes == "-99" ? "0" : model.SportTypes,
                    BetTypes = model.BetTypes,
                    BetListPage = BetListPage.Cancel,

                    BalanceUpDay = model.BalanceUpDay,
                    MoveBetsDay = model.MoveBetDay
                };

                mainTickets = betListService.GetTicketResult(criteria, true);
            }

            return mainTickets;
        }

        /// <summary>
        /// Outstanding the bet list.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="mainShareHolderId">The main share holder identifier.</param>
        /// <param name="mainShareHolderUsername">The main share holder username.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="sportType">Type of the sport.</param>
        /// <param name="sportName">Name of the sport.</param>
        /// <param name="betTypeName">Name of the bet type.</param>
        /// <returns>Ticket result.</returns>
        public TicketResult OutstandingBetList(
             int custId,
            string username = "",
            int mainShareHolderId = 0,
            string mainShareHolderUsername = "",
            string betType = "",
            string sportType = "",
            string sportName = "",
            string betTypeName = "")
        {
            var statusIds = string.Format(
                    "{0},{1}",
                   (int)Apollo.Age.Enumerations.BetStatus.Running,
                   (int)Apollo.Age.Enumerations.BetStatus.Waiting);

            var moveBetsDay = _reportService.MoveBetsDay();

            // List ticket
            var betListService = new BetListService();
            var criteria = new BetListCriteria
            {
                AgentId = -1,
                BetTypes = betType,
                SportTypes = sportType == "-99" ? "0" : sportType,
                LoggedId = CurrentSession.AgentId,
                CustId = custId,
                FromDate = ResetStartDate(moveBetsDay),
                ToDate = ResetEndDate(DateTime.Now),
                StatusIds = statusIds,
                BalanceUpDay = _reportService.GetBalanceUpDay(),
                BetListPage = BetListPage.Outstanding,
                MoveBetsDay = moveBetsDay
            };

            var mainTickets = _betListService.GetTicketResult(criteria, true);

            return mainTickets;
        }

        /// <summary>
        /// Time start date.
        /// </summary>
        /// <param name="dt">Input a date.</param>
        /// <returns>Return a date with full hour.</returns>
        public DateTime ResetStartDate(DateTime dt)
        {
            return dt.AddMilliseconds(-((((((dt.Hour * 60) + dt.Minute) * 60) + dt.Second) * 1000) + dt.Millisecond));
        }

        /// <summary>
        /// Time end date.
        /// </summary>
        /// <param name="dt">Input a date.</param>
        /// <returns>Return a date with full hour.</returns>
        public DateTime ResetEndDate(DateTime dt)
        {
            return ResetStartDate(dt).Add(new TimeSpan(0, 23, 59, 59, 000));
        }
    }
}