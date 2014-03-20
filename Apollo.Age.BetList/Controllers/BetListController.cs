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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Services;
    using Apollo.Age.BetList.ViewModels;
    using Apollo.Age.Core;
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.TotalBets.Services;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Web.Attribute;
    using Apollo.BetList.Core.Builder;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Factories;
    using Apollo.BetList.Core.TicketHelper;
    using Apollo.BetList.Core.Utils;
    using Fanex.NAP.Framework.Mvc;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// BetList Controller.
    /// </summary>
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
    public class BetListController : BetListBaseController
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
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        public ViewResult AccountInfo(int custId)
        {
            var moveBetsDay = _reportService.MoveBetsDay().AddDays(1);

            var model = new BetListCriteria
            {
                LoggedId = CurrentSession.AgentId,
                CustId = custId,
                FromDate = ResetStartDate(moveBetsDay),
                ToDate = ResetEndDate(DateTime.Now)
            };

            var mainTickets = this.AccountInfoBetlist(model);

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
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        public ViewResult AccountInfo(BetListCriteria model)
        {
            var moveBetsDay = _reportService.MoveBetsDay().AddDays(1);

            model.LoggedId = CurrentSession.AgentId;
            model.ToDate = ResetEndDate(model.ToDate);
            model.FromDate = ResetStartDate(model.FromDate);

            var mainTickets = this.AccountInfoBetlist(model);

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
        /// Accounts the information export excel.
        /// </summary>
        /// <param name="model">The model bet list.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult AccountInfoExportExcel(BetListCriteria model)
        {
            model.LoggedId = CurrentSession.AgentId;
            model.ToDate = ResetEndDate(model.ToDate);
            model.FromDate = ResetStartDate(model.FromDate);

            var mainTickets = this.AccountInfoBetlist(model);

            string sheetName = "Betlist-AccountInfo";
            string betlistName = "BetList From " + model.FromDate.ToString("MM/dd/yyyy") + " To " + model.ToDate.ToString("MM/dd/yyyy") + " - AccountInfo - " + model.UserName;

            string[] titleBetlist = new string[]
            {
                Resources.BetList.time,
                Resources.BetList.discount,
                Resources.BetList.gameType,
                Resources.BetList.explain,
                Resources.BetList.amount,
                Resources.BetList.result,
                Resources.BetList.status,
                Resources.Lang.lblAgentShare,
                Resources.BetList.agentComm
            };

            var excelResult = this.TicketExcel(mainTickets, BetListPage.AccountInfo, sheetName, betlistName, titleBetlist);

            var fileName = string.Format(
                "BetList-{0}-{1}-From-{2}-To-{3}",
                "AccountInfo",
                model.UserName,
                model.FromDate.ToString("yyyyMMdd"),
                model.ToDate.ToString("yyyyMMdd"));

            return ExcelResult(excelResult, fileName);
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

            viewModel.ThisPeriod = _periodDateRangeSerivce.PeriodDates();
            viewModel.LastPeriod = _periodDateRangeSerivce.PeriodDates(true);

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

                SportTypes = sportTypes == "-99" ? "0" : sportTypes,
                BetTypes = betTypes,

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
        [ValidateInput(false)]
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

            TicketResult mainTickets = TransactionBetlist(model);

            if (mainTickets != null && mainTickets.TicketList != null && mainTickets.TicketList.Any())
            {
                ViewBag.TotalItems = mainTickets.TicketList.Count();

                mainTickets.TicketList = mainTickets.TicketList.Skip(model.PageSize * model.PageIndex).Take(model.PageSize).ToList() as IList<ITicket>;

                // Set data to view model
                model.TicketList = this.Ticket(mainTickets, BetListPage.Transaction);
            }
            else
            {
                ViewBag.TotalItems = 0;
            }

            model.ThisPeriod = _periodDateRangeSerivce.PeriodDates();
            model.LastPeriod = _periodDateRangeSerivce.PeriodDates(true);

            return this.View("~/Views/BetList/Transactions.cshtml", model);
        }

        /// <summary>
        /// Transactions the export excel.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Action Result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult TransactionExportExcel(BetListReportViewModel model)
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

            TicketResult mainTickets = TransactionBetlist(model);

            string sheetName = "Betlist-Transactions";
            string betlistName = "BetList From " + model.FromDate.ToString("MM/dd/yyyy") + " To " + model.ToDate.ToString("MM/dd/yyyy") + " - Transactions - " + model.Username;

            string[] titleBetlist = new string[]
            {
                Resources.BetList.time,
                Resources.BetList.discount,
                Resources.BetList.gameType,
                Resources.BetList.explain,
                Resources.BetList.amount,
                Resources.BetList.result,
                Resources.BetList.status,
                Resources.Lang.lblAgentShare,
                Resources.BetList.agentComm
            };

            var excelResult = this.TicketExcel(mainTickets, BetListPage.Report, sheetName, betlistName, titleBetlist);

            var fileName = string.Format(
                "BetList-{0}-{1}-From-{2}-To-{3}",
                "Transactions",
                model.Username,
                model.FromDate.ToString("yyyyMMdd"),
                model.ToDate.ToString("yyyyMMdd"));

            return ExcelResult(excelResult, fileName);
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

            return this.TicketCancel(viewModel);
        }

        /// <summary>
        /// Cancel ticket bet list.
        /// </summary>
        /// <param name="model">Input the model.</param>
        /// <returns>Return view result.</returns>
        [HttpPost]
        [ValidateInput(false)]
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

            TicketResult mainTickets = TicketCancelBetList(model);

            if (mainTickets != null && mainTickets.TicketList != null && mainTickets.TicketList.Any())
            {
                model.TicketList = this.Ticket(mainTickets, BetListPage.Cancel);
            }

            return this.View(model);
        }

        /// <summary>
        /// Tickets the cancel export excel.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Action result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult TicketCancelExportExcel(BetListReportViewModel model)
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

            TicketResult mainTickets = TicketCancelBetList(model);

            string sheetName = "Betlist-Cancelled Tickets";
            string betlistName = "BetList From " + model.FromDate.ToString("MM/dd/yyyy") + " To " + model.ToDate.ToString("MM/dd/yyyy") + " - Cancelled Tickets - " + model.Username;

            string[] titleBetlist = new string[]
            {
                Resources.BetList.time,
                Resources.BetList.discount,
                Resources.BetList.gameType,
                Resources.BetList.explain,
                Resources.BetList.amount,
                Resources.BetList.result,
                Resources.BetList.status,
                Resources.Lang.lblAgentShare,
                Resources.BetList.agentComm
            };

            var excelResult = this.TicketExcel(mainTickets, BetListPage.Report, sheetName, betlistName, titleBetlist);

            var fileName = string.Format(
                "BetList-{0}-{1}-From-{2}-To-{3}",
                "CancelledTickets",
                model.Username,
                model.FromDate.ToString("yyyyMMdd"),
                model.ToDate.ToString("yyyyMMdd"));

            return ExcelResult(excelResult, fileName);
        }

        /// <summary>
        /// Bet list Outstanding listOutstanding.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="sportType">Type of the sport.</param>
        /// <returns>
        /// Return view result.
        /// </returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Outstanding, Policy = Policy.Query)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        [ValidateInput(false)]
        public ViewResult Outstanding(
            int custId,
            string betType = "",
            string sportType = "")
        {
            var mainTickets = OutstandingBetList(custId, betType, sportType);

            var viewModel = new BetListViewModel
            {
                TicketList = this.Ticket(mainTickets, BetListPage.Outstanding),
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// Exports the excel outstanding.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="sportType">Type of the sport.</param>
        /// <returns>
        /// Action Result.
        /// </returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Outstanding, Policy = Policy.Query)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        [ValidateInput(false)]
        public ActionResult OutstandingExportExcel(
            int custId,
            string username = "",
            string betType = "",
            string sportType = "")
        {
            var mainTickets = OutstandingBetList(custId, betType, sportType);

            var sheetName = "Betlist-Outstanding";
            var betlistName = "BetList on " + DateTime.Now.ToString("MM/dd/yyyy") + " - Outstanding - " + username;

            string[] titleBetlist = new string[]
            {
                Resources.BetList.time,
                Resources.BetList.rebate,
                Resources.BetList.ticket,
                Resources.BetList.content,
                Resources.BetList.amount,
                Resources.BetList.result,
                Resources.BetList.status,
                Resources.Lang.lblAgentShare,
                Resources.BetList.agentComm
            };

            var excelResult = this.TicketExcel(mainTickets, BetListPage.Outstanding, sheetName, betlistName, titleBetlist);

            var fileName = string.Format(
                "BetList_{0}_{1}_{2}",
                "Outstanding",
                username,
                DateTime.Now.ToString("yyyyMMdd"));

            return ExcelResult(excelResult, fileName);
        }

        /// <summary>
        /// Bet list win lost.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="fromDate">Input from date.</param>
        /// <param name="toDate">Input to date.</param>
        /// <param name="type">Bet list type.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="sportType">Type of the sport.</param>
        /// <returns>
        /// Return view result.
        /// </returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.WinLoss, Policy = Policy.Query)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        [ValidateInput(false)]
        public ViewResult Winloss(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            string type = "",
            string betType = "",
            string sportType = "")
        {
            TicketResult mainTickets = WinlossBetlist(
                custId,
                fromDate,
                toDate,
                type,
                betType,
                sportType);

            var viewModel = new BetListViewModel
            {
                TicketList = this.Ticket(mainTickets, BetListPage.Report),
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// Exports the win loss.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="fromDate">The From date.</param>
        /// <param name="toDate">The To date.</param>
        /// <param name="type">The type bet list.</param>
        /// <param name="username">The username.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="sportType">Type of the sport.</param>
        /// <returns>
        /// Return action result.
        /// </returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.WinLoss, Policy = Policy.Query)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        [ValidateInput(false)]
        public ActionResult WinlossExportExcel(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            string type = "",
            string username = "",
            string betType = "",
            string sportType = "")
        {
            TicketResult mainTickets = WinlossBetlist(
                custId,
                fromDate,
                toDate,
                type,
                betType,
                sportType);

            var sheetName = "Betlist-WinLoss";
            var betlistName = "BetList From " + fromDate.ToString("MM/dd/yyyy") + " To " + toDate.ToString("MM/dd/yyyy") + " - WinLoss - " + username;

            string[] titleBetlist = new string[]
            {
                Resources.BetList.time,
                Resources.BetList.discount,
                Resources.BetList.gameType,
                Resources.BetList.explain,
                Resources.BetList.amount,
                Resources.BetList.result,
                Resources.BetList.status,
                Resources.Lang.lblAgentShare,
                Resources.BetList.agentComm
            };

            var excelResult = this.TicketExcel(mainTickets, BetListPage.Report, sheetName, betlistName, titleBetlist);

            var fileName = string.Format(
                "BetList-{0}-{1}-From-{2}-To-{3}",
                "WinLoss",
                username,
                fromDate.ToString("yyyyMMdd"),
                toDate.ToString("yyyyMMdd"));

            return ExcelResult(excelResult, fileName);
        }

        /// <summary>
        /// Action view bet List from forecast.
        /// </summary>
        /// <param name="type">The type of bet list.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="betTeam">The bet team.</param>
        /// <param name="matchId">The match identifier.</param>
        /// <param name="mainShareHolderId">The main share holder identifier.</param>
        /// <param name="isForecast">Is view from forecast or not.</param>
        /// <returns>
        /// Return view result.
        /// </returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Forecast, Policy = Policy.Query)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        public ViewResult Forecast(
            string type = "",
            string betType = "",
            string betTeam = "",
            int matchId = 0,
            int mainShareHolderId = 0,
            bool isForecast = false)
        {
            var strBetType = string.Empty;
            var title = string.Empty;

            // List mainShare Holder
            IEnumerable<Agent> mainShareHolders = new List<Agent>();
            if (!isForecast)
            {
                mainShareHolders = this.ListMainShareHolder(strBetType, ref mainShareHolderId);
            }

            TicketResult mainTickets = ForecastBetlist(ref title, ref strBetType, type, betType, betTeam, matchId, mainShareHolderId);

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
        /// Forecast the export excel.
        /// </summary>
        /// <param name="type">The type bet list.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="betTeam">The bet team.</param>
        /// <param name="matchId">The match identifier.</param>
        /// <param name="mainShareHolderId">The main share holder identifier.</param>
        /// <returns>
        /// Return action result.
        /// </returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Forecast, Policy = Policy.Query)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        public ActionResult ForecastExportExcel(
            string type = "",
            string betType = "",
            string betTeam = "",
            int matchId = 0,
            int mainShareHolderId = 0)
        {
            var strBetType = string.Empty;
            var title = string.Empty;

            TicketResult mainTickets = ForecastBetlist(ref title, ref strBetType, type, betType, betTeam, matchId, mainShareHolderId);

            title = title.Replace("/", "_");
            var sheetName = "BetList-Forecast";
            var betlistName = string.Format("BetList on {0} - {1} - {2}", DateTime.Now.ToString("MM/dd/yyyy"), "Forecast", title);

            string[] titleBetlist = new string[]
            {
                Resources.BetList.time,
                Resources.BetList.rebate,
                Resources.BetList.ticket,
                Resources.BetList.content,
                Resources.BetList.amount,
                Resources.BetList.result,
                Resources.BetList.status,
                Resources.Lang.lblAgentShare,
                Resources.BetList.agentComm
            };

            var excelResult = this.TicketExcel(mainTickets, BetListPage.Forecast, sheetName, betlistName, titleBetlist);

            var fileName = string.Format(
                "BetList-{0}-{1}-{2}", "Forecast", title, DateTime.Now.ToString("yyyyMMdd"));

            return ExcelResult(excelResult, fileName);
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
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Wagers + ";" + ApolloFunctionKeys.EMWagers + ";" + ApolloFunctionKeys.Outright, Policy = Policy.Query)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
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
            TicketResult mainTickets = WagerBetlist(
                loggedId,
                fromDate,
                toDate,
                userGroup,
                isDirectMem,
                liveIndicator,
                leagueId,
                betType,
                sportType,
                betTeam,
                matchId);

            var viewModel = new BetListViewModel
            {
                TicketList = this.Ticket(mainTickets, BetListPage.Wager),
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// Wagers the export excel.
        /// </summary>
        /// <param name="loggedId">The logged identifier.</param>
        /// <param name="fromDate">The from date.</param>
        /// <param name="toDate">The to date.</param>
        /// <param name="userGroup">The user group.</param>
        /// <param name="isDirectMem">If set to <c>true</c> [is direct memory].</param>
        /// <param name="liveIndicator">If set to <c>true</c> [live indicator].</param>
        /// <param name="leagueId">The league identifier.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <param name="sportType">Type of the sport.</param>
        /// <param name="betTeam">The bet team.</param>
        /// <param name="matchId">The match identifier.</param>
        /// <returns>Action result.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Wagers + ";" + ApolloFunctionKeys.EMWagers + ";" + ApolloFunctionKeys.Outright, Policy = Policy.Query)]
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.DownlineDetails, Policy = Policy.Query)]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult WagerExportExcel(
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
            TicketResult mainTickets = WagerBetlist(
               loggedId,
               fromDate,
               toDate,
               userGroup,
               isDirectMem,
               liveIndicator,
               leagueId,
               betType,
               sportType,
               betTeam,
               matchId);

            string sportTypeName = string.Empty;

            switch (sportType)
            {
                case SportGroup.Soccer:
                    {
                        sportTypeName = Resources.Lang.soccer;
                        break;
                    }

                case SportGroup.BasketballFootballRugy:
                    {
                        sportTypeName = Resources.Lang.lblBkNflRugby;
                        break;
                    }

                case SportGroup.Tennis:
                    {
                        sportTypeName = Resources.Lang.Tennis;
                        break;
                    }

                case SportGroup.VolleyballBadmintonTableTennis:
                    {
                        sportTypeName = Resources.Lang.lblVbBdmTt;
                        break;
                    }

                case SportGroup.BaseBall:
                    {
                        sportTypeName = Resources.Lang.Baseball;
                        break;
                    }

                default:
                    {
                        sportTypeName = Resources.Lang.other;
                        break;
                    }
            }

            string betTypeName = CacheRef.GetBettype(betType);
            var sheetName = "Betlist-Wager";
            var betlistName = string.Format("Bet List on {0} - {1} - {2} - {3}", fromDate.ToString("MM/dd/yyyy"), "Wagers", sportTypeName, betTypeName);
            var fileName = string.Format(
              "BetList-Wager-{0}-{1}-{2}",
              sportTypeName.Replace(".", string.Empty).Replace(" / ", "-"),
              betTypeName,
              fromDate.ToString("yyyyMMdd"));

            if (betType == "10")
            {
                sheetName = "BetList-Outright";
                betlistName = string.Format("Bet List From {0} To {1} - {2} - {3}", fromDate.ToString("MM/dd/yyyy"), toDate.Value.ToString("MM/dd/yyyy"), betTypeName, sportTypeName);
                fileName = string.Format(
                "BetList-Outright-{0}-{1}-{2}-{3}",
                fromDate.ToString("yyyyMMdd"),
                toDate.Value.ToString("yyyyMMdd"),
                betTypeName,
                sportTypeName.Replace(".", string.Empty).Replace(" / ", "-"));
            }

            string[] titleBetlist = new string[]
            {
                Resources.BetList.time,
                Resources.BetList.rebate,
                Resources.BetList.ticket,
                Resources.BetList.content,
                Resources.BetList.amount,
                Resources.BetList.result,
                Resources.BetList.status,
                Resources.Lang.lblAgentShare,
                Resources.BetList.agentComm
            };

            var excelResult = this.TicketExcel(mainTickets, BetListPage.Wager, sheetName, betlistName, titleBetlist);

            return ExcelResult(excelResult, fileName);
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
        /// <param name="custId">The customer identifier.</param>
        /// <returns>
        /// Return view result.
        /// </returns>
        public ViewResult CombinationMixParlay(long refNo, int custId)
        {
            var balanceUpDay = _reportService.GetBalanceUpDay();
            var moveBetsDay = _reportService.MoveBetsDay();

            var criteria = new SystemParlayDetailCriteria
            {
                CustId = custId,
                Language = CurrentSession.Language,
                BalanceUpDay = balanceUpDay,
                MoveBetsDay = moveBetsDay,
                RefNo = refNo
            };

            var mixParlayCombinationDatas = _betListService.GetCombinationMixParlayDetail(criteria);

            return this.View(mixParlayCombinationDatas);
        }

        /// <summary>
        /// View detail popup Agent Share and Agent commission.
        /// </summary>
        /// <param name="transId">Transaction Id.</param>
        /// <param name="fromDate">Input from date.</param>
        /// <param name="toDate">Input to date.</param>
        /// <returns>
        /// Return view result.
        /// </returns>
        public ViewResult ViewDetail(long transId, DateTime? fromDate, DateTime? toDate)
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
                FromDate = ResetStartDate(!fromDate.HasValue ? moveBetsDay : Convert.ToDateTime(fromDate)),
                ToDate = ResetEndDate(!toDate.HasValue ? DateTime.Now : Convert.ToDateTime(toDate)),
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
        /// Tickets the excel.
        /// </summary>
        /// <param name="ticketResult">The ticket result.</param>
        /// <param name="betListPage">The bet list page.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="betlistName">Name of the bet list.</param>
        /// <param name="titleBetlists">The title bet lists.</param>
        /// <returns>
        /// HSSF Work book.
        /// </returns>
        private HSSFWorkbook TicketExcel(TicketResult ticketResult, BetListPage betListPage, string sheetName, string betlistName, string[] titleBetlists)
        {
            GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;
            var ticketHelper = new TicketHelper();
            BetListExcelFactory factory = new BetListExcelFactory();
            BetListExcelBuilder builder = factory.CreateBuilder();
            var tickets = ticketResult.TicketList;
            short columnNumber = 9;
            builder.BuildWorksheet(sheetName, columnNumber, 20);
            decimal totalAmount = 0;
            decimal totalResult = 0;
            decimal totalShare = 0;
            decimal totalComm = 0;

            CellRangeAddress rangeAddress = new CellRangeAddress(0, 0, 0, 8);

            builder
                .SetColumnWidth(0, 15)
                .SetColumnWidth(1, 20)
                .SetColumnWidth(2, 20)
                .SetColumnWidth(3, 50)
                .SetColumnWidth(4, 20)
                .SetColumnWidth(5, 20)
                .SetColumnWidth(6, 20)
                .SetColumnWidth(7, 15)
                .SetColumnWidth(8, 15);

            builder.AddTitle(betlistName, rangeAddress);
            builder.AddRow(9);

            foreach (string title in titleBetlists)
            {
                builder.AddHeader(title);
            }

            int rowNumber = 3;
            foreach (var ticket in tickets)
            {
                if (ticket.AgentLevel == 0 && CurrentSession.AgentLevel != 0)
                {
                    ticket.AgentLevel = CurrentSession.AgentLevel;
                }

                builder.AddTime(ticket, betListPage)
                       .AddDiscount(ticket, betListPage)
                       .AddGameType(ticket, ticketHelper, betListPage, funcGetOddsTypeName)
                       .AddChoice(ticket, ticketHelper, ticketResult.TicketDataList, ticketResult.TicketListMixParlay)
                       .AddStake(ticket)
                       .AddResult(ticket, betListPage)
                       .AddStatus(ticket, ticketResult.TicketDataList)
                       .AddShare(ticket, ticketResult.TicketDataList, betListPage)
                       .AddComm(ticket);

                builder.SetRowHigh(rowNumber, 6, 3);

                totalAmount += ticket.Stake;

                string ticketBetStatus = string.IsNullOrEmpty(ticket.Status) ? string.Empty : ticket.Status.ToLower();
                if (ticketBetStatus == Apollo.BetList.Core.Entities.BetStatus.Won
                    || ticketBetStatus == Apollo.BetList.Core.Entities.BetStatus.Draw
                    || ticketBetStatus == Apollo.BetList.Core.Entities.BetStatus.Lose
                    || (ticket.StatusId != null && ticket.StatusId.Value > 110 && ticket.StatusId.Value < 114))
                {
                    var result = ticket.Winlost + ticket.PlayerComm;
                    totalResult += result;
                    totalShare += ticket.AWinlost;
                }

                totalComm += ticket.AComm;

                rowNumber++;
            }

            builder
                .AddTotal()
                .AddTotal()
                .AddTotal()
                .AddTotal(tickets.Count.ToString())
                .AddTotal(Formatter.FormatNumber20(totalAmount, Formatter.CULT_NAME_ENG), null, true)
                .AddTotal(Formatter.FormatNumber20(totalResult, Formatter.CULT_NAME_ENG), null, true)
                .AddTotal()
                .AddTotal(Formatter.FormatNumber20(totalShare, Formatter.CULT_NAME_ENG), null, true)
                .AddTotal(Formatter.FormatNumber20(totalComm, Formatter.CULT_NAME_ENG), null, true);

            builder
                .SetRowHigh(0, 2)
                .SetRowHigh(2, 2)
                .SetLastRowHigh(2);

            HSSFWorkbook workBook = (HSSFWorkbook)builder.GetBetList();

            return workBook;
        }

        /// <summary>
        /// Get ticket for bet List.
        /// </summary>
        /// <param name="ticketResult">The ticket result.</param>
        /// <param name="betlistPage">The bet list page.</param>
        /// <returns>
        /// Return view result.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
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
                    Choice = ticket.BetTypeId == 9 || ticket.BetTypeId == 29 ?
                    builder.BuildChoice(ticket, ticketHelper, ticketResult.TicketDataList, ticketResult.TicketListMixParlay, false) :
                    builder.BuildChoice(ticket, ticketHelper, ticketResult.TicketDataList, false),
                    Stake = ticket.Stake,
                    Status = builder.BuildStatus(ticket, ticketResult.TicketDataList),
                    AgentCommValue = ticket.AComm,
                    AgentComm = builder.BuildAgentComm(ticket),
                    AgentShare = builder.BuildAgentShare(ticket, null)
                };

                string ticketBetStatus = string.IsNullOrEmpty(ticket.Status) ? string.Empty : ticket.Status.ToLower();
                if (ticketBetStatus == Apollo.BetList.Core.Entities.BetStatus.Won
                    || ticketBetStatus == Apollo.BetList.Core.Entities.BetStatus.Draw
                    || ticketBetStatus == Apollo.BetList.Core.Entities.BetStatus.Lose
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

        #endregion Format Start, End date
    }
}