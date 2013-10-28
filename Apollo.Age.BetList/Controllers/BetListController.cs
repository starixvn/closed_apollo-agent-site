// --------------------------------------------------------------------------
// <copyright file="BetListController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">Created and Implement</change>
//   <change who="Kaden.Trinh" date="2013.07.02">Implement cancel ticket</change>
//   <change who="Kaden.Trinh" date="2013.07.02">Implement Wagers</change>
// </history>

namespace Apollo.Age.BetList.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Services;
    using Apollo.Age.BetList.ViewModels;
    using Apollo.Age.Core;
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Resources;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.TotalBets.Services;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Web.Attribute;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;

    /// <summary>
    /// BetList Controller.
    /// </summary>
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class BetListController : BaseController
    {
        private readonly BetListService _betListService;
        private readonly ReportService _reportService;
        private readonly TotalBetsService _totalBetsService;
        private readonly IUserService _userService;
        private readonly PeriodDateRangeService _periodDateRangeSerivce;

        /// <summary>
        /// Initializes a new instance of the <see cref="BetListController" /> class.
        /// </summary>
        public BetListController()
        {
            this._betListService = new BetListService();
            this._reportService = new ReportService();
            this._totalBetsService = new TotalBetsService();
            _userService = new UserService();
            _periodDateRangeSerivce = new PeriodDateRangeService();
        }

        /// <summary>
        /// Bet list of member.
        /// </summary>
        /// <param name="custId">Input CUSTID who need to view bet list.</param>
        /// <returns>Return view result.</returns>
        [HttpGet]
        public ViewResult AccountInfo(int custId)
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

            var moveBetsDay = _reportService.MoveBetsDay().AddDays(1);

            // List ticket
            var betListService = new BetListService();
            var model = new BetListCriteria
            {
                AgentId = -1,
                LoggedId = CurrentSession.AgentId,
                CustId = custId,
                FromDate = ResetStartDate(moveBetsDay),
                ToDate = ResetEndDate(DateTime.Now),
                StatusIds = statusIds,
                BetListPage = BetListPage.AccountInfo,
                BalanceUpDay = _reportService.GetBalanceUpDay(),
                MoveBetsDay = moveBetsDay
            };

            var mainTickets = _betListService.GetTicketResult(model, true);

            var viewModel = new BetListReportViewModel
            {
                TicketList = this.Ticket(mainTickets, BetListPage.AccountInfo),
                FromDate = moveBetsDay,
                ToDate = DateTime.Now,
                BalanceUpDay = moveBetsDay
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// Bet list of member.
        /// </summary>
        /// <param name="model">Input a model.</param>
        /// <returns>Return view result.</returns>
        [HttpPost]
        public ViewResult AccountInfo(BetListCriteria model)
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

            var moveBetsDay = _reportService.MoveBetsDay().AddDays(1);

            // List ticket
            var betListService = new BetListService();
            model.AgentId = -1;
            model.LoggedId = CurrentSession.AgentId;
            model.StatusIds = statusIds;
            model.BetListPage = BetListPage.AccountInfo;
            model.ToDate = ResetEndDate(model.ToDate);
            model.BalanceUpDay = _reportService.GetBalanceUpDay();
            model.MoveBetsDay = _reportService.MoveBetsDay();

            var mainTickets = _betListService.GetTicketResult(model, true);

            var viewModel = new BetListReportViewModel
            {
                TicketList = this.Ticket(mainTickets, BetListPage.AccountInfo),
                FromDate = moveBetsDay,
                ToDate = DateTime.Now,
                BalanceUpDay = moveBetsDay,
                MoveBetDay = moveBetsDay
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// Bet list Transaction.
        /// </summary>
        /// <returns>Return view result.</returns>
        [HttpGet]
        public ViewResult Transactions()
        {
            // Get permission of user
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);

            ViewBag.allFunctionPermission = userPermission;

            var hasPermission = userPermission[ApolloFunctionKeys.Transactions] >= (byte)Policy.Query;
            var hasDownlineDetailsPermission = userPermission[ApolloFunctionKeys.DownlineDetails] >= (byte)Policy.Query;

            if (!hasPermission
                || !hasDownlineDetailsPermission)
            {
                return View("~/Views/Reports/ReportAccessDenied.cshtml");
            }

            var viewModel = new BetListReportViewModel();
            viewModel.Username = CurrentSession.AgentName;

            viewModel.ThisPeriod = _periodDateRangeSerivce.PeriodRange();
            viewModel.LastPeriod = _periodDateRangeSerivce.PeriodRange(true);

            return this.Transactions(viewModel);
        }

        /// <summary>
        /// Bet list Transaction.
        /// </summary>
        /// <param name="fromDate">Input from date.</param>
        /// <param name="toDate">Input to date.</param>
        /// <param name="sportTypes">Input sport types.</param>
        /// <param name="betTypes">Input bet types.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userType">Type of the user.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <returns>Return view result.</returns>
        [HttpGet]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Transactions, Policy = Policy.Query)]
        public ViewResult TransactionsPaging(
            DateTime fromDate,
            DateTime toDate,
            string sportTypes = "",
            string betTypes = "",
            string userName = "",
            int userType = 0,
            int pageIndex = 0)
        {
            var viewModel = new BetListReportViewModel()
            {
                FromDate = fromDate,
                ToDate = toDate,

                SportTypeIds = sportTypes == "-99" ? "0" : sportTypes,
                BetTypeIds = betTypes,

                Username = userName,
                UserType = userType,
                PageIndex = pageIndex,
                FromPaging = true
            };

            return this.Transactions(viewModel);
        }

        /// <summary>
        /// Bet list Transaction.
        /// </summary>
        /// <param name="model">Input the model.</param>
        /// <returns>Return view result.</returns>
        [HttpPost]
        public ViewResult Transactions(BetListReportViewModel model)
        {
            // Get permission of user
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);

            ViewBag.allFunctionPermission = userPermission;

            var hasPermission = userPermission[ApolloFunctionKeys.Transactions] >= (byte)Policy.Query;
            var hasDownlineDetailsPermission = userPermission[ApolloFunctionKeys.DownlineDetails] >= (byte)Policy.Query;

            if (!hasPermission
                || !hasDownlineDetailsPermission)
            {
                return View("~/Views/Reports/ReportAccessDenied.cshtml");
            }

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
            model.BettypeList = model.SportTypeIds == "-99" ? _reportService.GetParlayBettype() : _reportService.GetBettype(model.SportTypeIds == "-99" || model.SportTypeIds == "0");
            model.BalanceUpDay = balanceUpDay;

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

                    SportTypes = model.SportTypeIds == "-99" ? "0" : model.SportTypeIds,
                    BetTypes = model.BetTypeIds,

                    BetListPage = BetListPage.Transaction,
                    MoveBetsDay = _reportService.MoveBetsDay()
                };

                //// Cache Key
                var sessionKey = string.Format(
                    "TransactionsReport{0}-{1}-{2}",
                    criteria.LoggedId,
                    criteria.AgentId,
                    criteria.CustId);

                TicketResult mainTickets = null;
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

                ViewBag.TotalItems = mainTickets.TicketList.Count();

                mainTickets.TicketList = mainTickets.TicketList.Skip(model.PageSize * model.PageIndex).Take(model.PageSize).ToList() as IList<ITicket>;

                // Set data to view model
                model.TicketList = this.Ticket(mainTickets, BetListPage.Transaction);
            }
            else
            {
                ViewBag.TotalItems = 0;
            }

            model.ThisPeriod = _periodDateRangeSerivce.PeriodRange();
            model.LastPeriod = _periodDateRangeSerivce.PeriodRange(true);

            return this.View("~/Views/BetList/Transactions.cshtml", model);
        }

        /// <summary>
        /// Cancel ticket bet list.
        /// </summary>
        /// <returns>Return view result.</returns>
        [HttpGet]
        public ViewResult TicketCancel()
        {
            var viewModel = new BetListReportViewModel
                {
                    Username = CurrentSession.AgentName
                };

            // Get permission of user
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);

            ViewBag.allFunctionPermission = userPermission;

            var hasPermission = userPermission[ApolloFunctionKeys.CancelledTickets] >= (byte)Policy.Query;
            var hasDownlineDetailsPermission = userPermission[ApolloFunctionKeys.DownlineDetails] >= (byte)Policy.Query;

            if (!hasPermission
                || !hasDownlineDetailsPermission)
            {
                return View("~/Views/Reports/ReportAccessDenied.cshtml");
            }

            viewModel.ThisPeriod = _periodDateRangeSerivce.PeriodRange();
            viewModel.LastPeriod = _periodDateRangeSerivce.PeriodRange(true);

            return this.TicketCancel(viewModel);
        }

        /// <summary>
        /// Cancel ticket bet list.
        /// </summary>
        /// <param name="model">Input the model.</param>
        /// <returns>Return view result.</returns>
        [HttpPost]
        public ViewResult TicketCancel(BetListReportViewModel model)
        {
            // Get permission of user.
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);

            ViewBag.allFunctionPermission = userPermission;

            var hasPermission = userPermission[ApolloFunctionKeys.CancelledTickets] >= (byte)Policy.Query;
            var hasDownlineDetailsPermission = userPermission[ApolloFunctionKeys.DownlineDetails] >= (byte)Policy.Query;

            if (!hasPermission
                || !hasDownlineDetailsPermission)
            {
                return View("~/Views/Reports/ReportAccessDenied.cshtml");
            }

            var statusIds = string.Format("{0},{1}", (int)Apollo.Age.Enumerations.BetStatus.Void, (int)Apollo.Age.Enumerations.BetStatus.Reject);
            var balanceUpDay = _reportService.GetBalanceUpDay();
            var moveBetsDay = _reportService.MoveBetsDay();
            int agentId, custId;
            int errorCode;

            model.Username = !string.IsNullOrEmpty(model.Username) ? model.Username.Trim() : CurrentSession.AgentName;
            _betListService.GetUserIdByUserName(CurrentSession.AgentId, model.UserType == 1, model.Username, out agentId, out custId, out errorCode);

            model.SportList = SingbetSportService.GetSport(isOther: false);
            model.BettypeList = model.SportTypeIds == "-99" ? _reportService.GetParlayBettype() : _reportService.GetBettype(model.SportTypeIds == "-99" || model.SportTypeIds == "0");
            model.BalanceUpDay = balanceUpDay;
            model.MoveBetDay = moveBetsDay;

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

                    SportTypes = model.SportTypeIds == "-99" ? "0" : model.SportTypeIds,
                    BetTypes = model.BetTypeIds,
                    BetListPage = BetListPage.Cancel,

                    BalanceUpDay = model.BalanceUpDay,
                    MoveBetsDay = model.MoveBetDay
                };

                TicketResult mainTickets;
                mainTickets = betListService.GetTicketResult(criteria, true);

                // Set data to view model.
                model.TicketList = this.Ticket(mainTickets, BetListPage.Cancel);
            }

            model.ThisPeriod = _periodDateRangeSerivce.PeriodRange();
            model.LastPeriod = _periodDateRangeSerivce.PeriodRange(true);

            return this.View(model);
        }

        /// <summary>
        /// Bet list Outstanding listOutstanding.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="username">Name of user.</param>
        /// <param name="mainShareHolderId">The main share holder identifier.</param>
        /// <param name="mainShareHolderUsername">The main share holder username.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="sportType">Type of the sport.</param>
        /// <param name="sportName">Name of the sport.</param>
        /// <param name="betTypeName">Name of the bet type.</param>
        /// <returns>Return view result.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Outstanding, Policy = Policy.Query)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        public ViewResult Outstanding(
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
                SportTypes = sportType,
                LoggedId = CurrentSession.AgentId,
                CustId = custId,
                FromDate = ResetStartDate(moveBetsDay),
                ToDate = ResetEndDate(DateTime.Now),
                StatusIds = statusIds,
                BalanceUpDay = _reportService.GetBalanceUpDay(),
                BetListPage = BetListPage.Outstanding,
                MoveBetsDay = _reportService.MoveBetsDay()
            };

            var mainTickets = _betListService.GetTicketResult(criteria, true);

            var viewModel = new BetListViewModel
            {
                TicketList = this.Ticket(mainTickets, BetListPage.Outstanding),
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// Bet list win lost.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="fromDate">Input from date.</param>
        /// <param name="toDate">Input to date.</param>
        /// <param name="type">Bet list type.</param>
        /// <param name="username">Name of user.</param>
        /// <param name="mainShareHolderId">The main share holder identifier.</param>
        /// <param name="mainShareHolderUsername">The main share holder username.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="sportType">Type of the sport.</param>
        /// <param name="sportName">Name of the sport.</param>
        /// <param name="betTypeName">Name of the bet type.</param>
        /// <returns>Return view result.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Win_Loss, Policy = Policy.Query)]
        public ViewResult Winloss(
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

            var viewModel = new BetListViewModel
            {
                TicketList = this.Ticket(mainTickets, BetListPage.Report),
            };
            return this.View(viewModel);
        }

        /// <summary>
        /// Action view bet List from forecast.
        /// </summary>
        /// <param name="type">The type of bet list.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="betTeam">The bet team.</param>
        /// <param name="matchId">The match identifier.</param>
        /// <param name="mainShareHolderId">The main share holder identifier.</param>
        /// <param name="mainShareHolderUsername">The main share holder username.</param>
        /// <param name="isForecast">Is view from forecast or not.</param>
        /// <returns>Return view result.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Forecast, Policy = Policy.Query)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        public ViewResult Forecast(
            string type = "",
            string betType = "",
            string betTeam = "",
            int matchId = 0,
            int mainShareHolderId = 0,
            string mainShareHolderUsername = "",
            bool isForecast = false)
        {
            var strBetType = string.Empty;
            var title = string.Empty;
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

            IEnumerable<Agent> mainShareHolders = new List<Agent>();
            if (!isForecast)
            {
                mainShareHolders = this.ListMainShareHolder(strBetType, ref mainShareHolderId);
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

            var viewModel = new BetListViewModel
            {
                Title = title,
                CurrentMainShareHolderId = mainShareHolderId,
                ListMainShareHolders = mainShareHolders,
                TicketList = this.Ticket(mainTickets, BetListPage.Forecast),
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// Action view bet List from Wagers page.
        /// </summary>
        /// <param name="loggedId">The logged identifier.</param>
        /// <param name="fromDate">Input from date.</param>
        /// <param name="toDate">Input to date.</param>
        /// <param name="userGroup">The user group.</param>
        /// <param name="isDirectMem">Is direct member or not.</param>
        /// <param name="liveIndicator">Is live or not.</param>
        /// <param name="leagueId">The league identifier.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="sportType">Type of the sport.</param>
        /// <param name="betTeam">The bet team.</param>
        /// <param name="matchId">The match identifier.</param>
        /// <returns>Return view result.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Wagers, Policy = Policy.Query)]
        public ViewResult Wagers(
            int loggedId,
            DateTime fromDate,
            DateTime? toDate,
            string userGroup = "",
            bool isDirectMem = false,
            bool liveIndicator = false,
            int leagueId = 0,
            string betType = "",
            string sportType = "",
            string betTeam = "",
            int matchId = 0)
        {
            var betListService = new BetListService();
            if (toDate == null)
            {
                toDate = fromDate;
            }

            var criteria = new WagersCriteria
            {
                LoggedId = loggedId > 0 ? loggedId : CurrentSession.AgentId,
                FromDate = fromDate,
                ToDate = toDate,
                UGroup = userGroup,
                IsDirectMem = isDirectMem,
                LiveIndicator = liveIndicator,
                LeagueId = leagueId,
                SportTypes = string.IsNullOrEmpty(sportType) ? "0" : sportType,
                BetTypes = string.IsNullOrEmpty(betType) ? "0" : betType,
                BetTeam = betTeam,
                MatchId = matchId,
                IsSuperView = CurrentSession.IsSuper
            };

            TicketResult mainTickets = betListService.WagersTicket(criteria);

            var viewModel = new BetListViewModel
            {
                TicketList = this.Ticket(mainTickets, BetListPage.Wager),
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// Action Mix Parlay.
        /// </summary>
        /// <param name="transid">The transaction id.</param>
        /// <returns>Return view result.</returns>
        public ViewResult MixParlay(long transid)
        {
            var betListService = new BetListService();
            var criteria = new BetListCriteria
            {
                TransId = transid,
            };

            var viewModel = new MixParlayViewModel
           {
               TicketList = betListService.GetMixParlayDetailTicket(criteria)
           };

            return this.View(viewModel);
        }

        /// <summary>
        /// Combination Mix Parlay.
        /// </summary>
        /// <param name="refNo">The reference number.</param>
        /// <param name="winlossDate">The win lost date.</param>
        /// <param name="transId">The trans identifier.</param>
        /// <param name="betId">The bet identifier.</param>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>Return view result.</returns>
        public ViewResult CombinationMixParlay(string refNo, DateTime winlossDate, int transId, int betId, int custId)
        {
            return this.View();
        }

        /// <summary>
        /// View detail popup Agent Share and Agent commission.
        /// </summary>
        /// <param name="transId">Transaction Id.</param>
        /// <param name="fromDate">Input from date.</param>
        /// <param name="toDate">Input to date.</param>
        /// <param name="type">Page type.</param>
        /// <returns>
        /// Return view result.
        /// </returns>
        public ViewResult ViewDetail(long transId, string fromDate, string toDate, string type)
        {
            IList<ITicket> listAgentComm;
            var betListService = new BetListService();
            var balanceUpDay = this._reportService.GetBalanceUpDay();
            var moveBetsDay = this._reportService.MoveBetsDay();
            var viewModel = new BetListViewModel();

            var criteria = new BetListCriteria
            {
                LoggedId = this.CurrentSession.AgentId,
                TransId = transId,
                FromDate = ResetStartDate(string.IsNullOrEmpty(fromDate) ? moveBetsDay : Convert.ToDateTime(fromDate)),
                ToDate = ResetEndDate(string.IsNullOrEmpty(toDate) ? DateTime.Now : Convert.ToDateTime(toDate)),
                BalanceUpDay = balanceUpDay,
                MoveBetsDay = moveBetsDay
            };
            if (criteria.IsValid())
            {
                listAgentComm = betListService.GetDetailWinLossAgentComm(criteria);

                viewModel.TicketList = this.Ticket(listAgentComm);
            }

            return this.View(viewModel);
        }

        #region Common Action

        /// <summary>
        /// Creates the type of the bet.
        /// </summary>
        /// <param name="isIncludeParlay">Include parlay bet type or not.</param>
        /// <returns>JSON bet type.</returns>
        public JsonResult CreateBetType(bool isIncludeParlay)
        {
            var bettypeList = _reportService.GetBettype(isIncludeParlay);

            return Json(bettypeList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Creates the parlay bet type.
        /// </summary>
        /// <returns>JSON bet type.</returns>
        public JsonResult CreateParlayBettype()
        {
            var parlayBettypeList = _reportService.GetParlayBettype();

            return Json(parlayBettypeList, JsonRequestBehavior.AllowGet);
        }

        #endregion Common Action

        /// <summary>
        /// Get ticket for bet List.
        /// </summary>
        /// <param name="ticketResult">The ticket result.</param>
        /// <param name="betlistPage">The bet list page.</param>
        /// <returns>Return view result.</returns>
        private List<TicketText> Ticket(TicketResult ticketResult, BetListPage betlistPage)
        {
            GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;
            var ticketHelper = new TicketHelper();
            var builder = new BetListHTMLBuilder();
            var ticketTextList = new List<TicketText>();
            var tickets = ticketResult.TicketList;
            foreach (var ticket in tickets)
            {
                if (ticket.AgentLevel == 0 && CurrentSession.AgentLevel != 0)
                {
                    ticket.AgentLevel = CurrentSession.AgentLevel;
                }

                var ticketText = new TicketText
                {
                    TransId = ticket.TransId,
                    UserName = ticket.UserName,
                    TransTime = builder.BuildTime(ticket, betlistPage),
                    Discount = builder.BuildDiscount(ticket, betlistPage),
                    GameType = builder.BuildTrans(ticket, ticketHelper, funcGetOddsTypeName, betlistPage),
                    Choice = builder.BuildChoice(ticket, ticketHelper, ticketResult.TicketDataList, false, betlistPage),
                    Stake = ticket.Stake,
                    Status = builder.BuildStatus(ticket, ticketResult.TicketDataList),
                    AgentCommValue = ticket.AComm,
                    AgentComm = builder.BuildAgentComm(ticket),
                    AgentShare = builder.BuildAgentShare(ticket, null)
                };

                string ticketBetStatus = string.IsNullOrEmpty(ticket.Status) ? string.Empty : ticket.Status.ToLower();
                if (ticketBetStatus == Fanex.BetList.Core.Entities.BetStatus.Won
                    || ticketBetStatus == Fanex.BetList.Core.Entities.BetStatus.Draw
                    || ticketBetStatus == Fanex.BetList.Core.Entities.BetStatus.Lose
                    || (ticket.StatusId != null && ticket.StatusId.Value > 110 && ticket.StatusId.Value < 114))
                {
                    ticketText.Result = ticket.Winlost + ticket.PlayerComm;
                    ticketText.AgentWinlossValue = ticket.AWinlost;
                }
                else
                {
                    ticketText.Result = 0;
                    ticketText.AgentWinlossValue = 0;
                }

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }

        /// <summary>
        /// Get ticket for view detail.
        /// </summary>
        /// <param name="tickets">The tickets.</param>
        /// <returns>List of tickets.</returns>
        private List<TicketText> Ticket(IList<ITicket> tickets)
        {
            var builder = new BetListHTMLBuilder();
            var ticketTextList = new List<TicketText>();
            foreach (var ticket in tickets)
            {
                if (ticket.AgentLevel == 0 && CurrentSession.AgentLevel != 0)
                {
                    ticket.AgentLevel = CurrentSession.AgentLevel;
                }

                var ticketText = new TicketText
                {
                    UserName = ticket.UserName,
                    AgentComm = builder.BuildAgentComm(ticket),
                    AgentShare = builder.BuildAgentShare(ticket, null)
                };
                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }

        #region Format Start, End date

        /// <summary>
        /// Lists the main share holder.
        /// </summary>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="mainShareHolderId">The main share holder identifier.</param>
        /// <returns>List of main share holder.</returns>
        private IEnumerable<Agent> ListMainShareHolder(string betType, ref int mainShareHolderId)
        {
            int agentId = this.CurrentSession.AgentId;
            if (this.CurrentSession.IsSuper)
            {
                var mainShareHolders = this._totalBetsService.GetData<Agent>(new GetAllMainShareHolderCriteria { Bettypes = betType });
                if (mainShareHolderId == 0 && mainShareHolders != null && mainShareHolders.Any())
                {
                    mainShareHolderId = mainShareHolders.ElementAt(0).Id;
                }

                return mainShareHolders;
            }

            mainShareHolderId = agentId;
            return new List<Agent>();
        }

        /// <summary>
        /// Time start date.
        /// </summary>
        /// <param name="dt">Input a date.</param>
        /// <returns>Return a date with full hour.</returns>
        private DateTime ResetStartDate(DateTime dt)
        {
            return dt.AddMilliseconds(-(((dt.Hour * 60 + dt.Minute) * 60 + dt.Second) * 1000 + dt.Millisecond));
        }

        /// <summary>
        /// Time end date.
        /// </summary>
        /// <param name="dt">Input a date.</param>
        /// <returns>Return a date with full hour.</returns>
        private DateTime ResetEndDate(DateTime dt)
        {
            return ResetStartDate(dt).Add(new TimeSpan(0, 23, 59, 59, 000));
        }

        #endregion Format Start, End date
    }
}