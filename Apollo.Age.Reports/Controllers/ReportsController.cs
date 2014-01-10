// --------------------------------------------------------------------------
// <copyright file="ReportsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.29">Created</change>
//   <change who="Terry.Nguyen" date="2013.04.07">Implement</change>
//   <change who="Terry.Nguyen" date="2013.04.09">Get AgentID from BaseController</change>
//   <change who="Quinn.Vo" date="2013.04.16">Add Action: MatchEntered, AgentWinLoss, MemberWinLoss</change>
//   <change who="Quinn.Vo" date="2013.07.16">Add Action: ExportAgentWinLoss, ExportMemberWinLoss</change>
//   <change who="Quinn.Vo" date="2013.07.31">Add Action: SportCredit, ExportSportCredit</change>
// </history>

namespace Apollo.Age.Reports.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Apollo.Age.Configurations;
    using Apollo.Age.Core;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Core;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Web.Attribute;
    using Fanex.NAP.Framework.Mvc;

    /// <summary>
    /// Reports Controller.
    /// </summary>
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
    public class ReportsController : BaseController
    {
        private readonly SportCreditService _sportCreditService;
        private readonly WinLossService _winLossService;
        private readonly OutstandingService _outstandingService;
        private readonly IUserService _userService;
        private readonly PeriodDateRangeService _periodDateRangeSerivce;
        private int _currentAgentId;

        /// <summary>
        /// Initializes a new instance of the ReportsController class.
        /// </summary>
        public ReportsController()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ReportsController class.
        /// </summary>
        /// <param name="outstandingService">Outstanding Service.</param>
        /// <param name="winlossService">Win loss Service.</param>
        /// <param name="sportCreditService">SportCredit Service.</param>
        public ReportsController(
            OutstandingService outstandingService = null,
            WinLossService winlossService = null,
            SportCreditService sportCreditService = null)
        {
            this._sportCreditService = sportCreditService ?? new SportCreditService();
            this._winLossService = winlossService ?? new WinLossService();
            this._outstandingService = outstandingService ?? new OutstandingService();
            _userService = new UserService();
            _periodDateRangeSerivce = new PeriodDateRangeService();
        }

        public int? CurrentAgentId
        {
            get
            {
                return this._currentAgentId;
            }

            set
            {
                this._currentAgentId = value.HasValue ? value.Value : this.CurrentSession.AgentId;
            }
        }

        #region Outstanding Report

        /// <summary>
        /// Get Agent Outstanding report.
        /// </summary>
        /// <param name="agentId">Input agent id.</param>
        /// <param name="sporttypes">Sport types.</param>
        /// <param name="bettypes">Bet types.</param>
        /// <returns>Outstanding ViewResult.</returns>
        public ViewResult Outstanding(int? agentId, string sporttypes, string bettypes)
        {
            // Get user permission.
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);

            var hasPermission = userPermission[ApolloFunctionKeys.Outstanding] >= (byte)Policy.Query;
            var hasDownlineDetailsPermission = userPermission[ApolloFunctionKeys.DownlineDetails] >= (byte)Policy.Query;

            ViewBag.allFunctionPermission = userPermission;

            if (!hasPermission || (agentId != null && !hasDownlineDetailsPermission))
            {
                return View(
                    "ReportAccessDenied",
                    new OutstandingViewModel
                    {
                        SportTypeIds = sporttypes,
                        BetTypeIds = bettypes,
                        SportList = SingbetSportService.GetSport(isOther: false)
                    });
            }

            this.CurrentAgentId = agentId;
            sporttypes = string.IsNullOrWhiteSpace(sporttypes) ? "0" : sporttypes;
            bettypes = string.IsNullOrWhiteSpace(bettypes) ? "0" : bettypes;

            var report = this._outstandingService.GetAgentOutStandingReport(
                this.CurrentSession.IsSuper,
                this.CurrentAgentId,
                this.CurrentSession.AgentId,
                sporttypes,
                bettypes);
            var sessionKey = string.Format(Constants.SessionKeyAgentOutstandingReport, this.CurrentSession.UserId);

            HttpContext.Session[sessionKey] = report.ListOutStanding;

            return this.View(report);
        }

        /// <summary>
        /// Export excel Outstanding report.
        /// </summary>
        /// <param name="agentId">Input agent id.</param>
        /// <param name="agentUsername">Agent user name.</param>
        /// <param name="sporttypes">Sport types.</param>
        /// <param name="sportName">Sport name.</param>
        /// <param name="bettypes">Bet types.</param>
        /// <param name="betTypeName">Bet type name.</param>
        /// <param name="isAgentOutStanding">Is agent Outstanding.</param>
        /// <returns>Excel Outstanding report.</returns>
        public ActionResult ExportOutstandingReport(
            int? agentId,
            string agentUsername,
            string sporttypes,
            string sportName,
            string bettypes,
            string betTypeName,
            bool isAgentOutStanding)
        {
            // Get user permission.
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);

            var hasPermission = userPermission[ApolloFunctionKeys.Outstanding] >= (byte)Policy.Query;
            var hasDownlineDetailsPermission = userPermission[ApolloFunctionKeys.DownlineDetails] >= (byte)Policy.Query;

            ViewBag.allFunctionPermission = userPermission;

            if (!hasPermission)
            {
                return View(
                    "ReportAccessDenied",
                    new OutstandingViewModel
                    {
                        SportTypeIds = sporttypes,
                        BetTypeIds = bettypes,
                        SportList = SingbetSportService.GetSport(isOther: false)
                    });
            }

            this.CurrentAgentId = agentId;
            var sessionKey = string.Format(
                isAgentOutStanding ? Constants.SessionKeyAgentOutstandingReport : Constants.SessionKeyMemberOutstandingReport,
                this.CurrentSession.UserId);

            var listOutstanding = (List<OutstandingModel>)HttpContext.Session[sessionKey];

            if (listOutstanding == null)
            {
                if (isAgentOutStanding)
                {
                    listOutstanding = this._outstandingService.GetData<OutstandingModel>(new OutstandingCriteria
                    {
                        AgentId = this.CurrentAgentId.Value,
                        Sporttypes = sporttypes == "-99" ? "0" : sporttypes,
                        Bettypes = bettypes
                    }).ToList();
                }
                else
                {
                    listOutstanding = this._outstandingService.GetData<OutstandingModel>(new MemberOutstandingCriteria
                    {
                        AgentId = this.CurrentAgentId.Value,
                        Sporttypes = sporttypes == "-99" ? "0" : sporttypes,
                        Bettypes = bettypes
                    }).ToList();
                }
            }

            if (!hasDownlineDetailsPermission
                && listOutstanding != null
                && listOutstanding.Any())
            {
                for (int i = 0; i < listOutstanding.Count; i++)
                {
                    listOutstanding[i].UserName = Web.Mvc.HtmlHelperExtensions.EncryptUsername(listOutstanding[i].UserName);
                }
            }

            var isSupper = this.CurrentSession.IsSuper && (this.CurrentAgentId == this.CurrentSession.AgentId);
            var excelResult = this._outstandingService.ExportOutstandingReport(
                isAgentOutStanding,
                listOutstanding,
                agentUsername,
                HttpUtility.UrlDecode(sportName),
                HttpUtility.UrlDecode(betTypeName),
                isSupper);

            return new ExcelResult(excelResult, string.Format("Outstanding_{0}_{1}", agentUsername, this._outstandingService.ShortDateFormat(DateTime.Now)));
        }

        /// <summary>
        /// Get member Outstanding report.
        /// </summary>
        /// <param name="agentId">Input agent id.</param>
        /// <param name="sporttypes">Sport types.</param>
        /// <param name="bettypes">Bet types.</param>
        /// <param name="mainShareHolderId">Main ShareHolder Id.</param>
        /// <param name="mainShareHolderUsername">Main ShareHolder name.</param>
        /// <returns>Member Outstanding report.</returns>
        [HttpPost]
        public ActionResult MemberOutstanding(
            int? agentId,
            string sporttypes,
            string bettypes,
            int mainShareHolderId,
            string mainShareHolderUsername)
        {
            // Get user permission
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);

            var hasPermission = userPermission[ApolloFunctionKeys.Outstanding] >= (byte)Policy.Query;
            var hasDownlineDetailsPermission = userPermission[ApolloFunctionKeys.DownlineDetails] >= (byte)Policy.Query;

            ViewBag.allFunctionPermission = userPermission;

            if (!hasPermission || !hasDownlineDetailsPermission)
            {
                return Json(new MemberOutstandingViewModel
                {
                    HasPermissionDownlineDetail = false
                });
            }

            this.CurrentAgentId = agentId;

            //// call service to get listOutstanding data here
            var report = this._outstandingService.GetMemberOutstandingReport(this.CurrentAgentId, sporttypes, bettypes, mainShareHolderId, mainShareHolderUsername);
            var sessionKey = string.Format(Constants.SessionKeyMemberOutstandingReport, this.CurrentSession.UserId);

            HttpContext.Session[sessionKey] = report.ListOutstanding;

            //// return object
            report.HasPermissionDownlineDetail = true;
            return this.Json(report);
        }

        #endregion Outstanding Report

        #region Agent Win Loss Report

        /// <summary>
        /// Get MatchEntered report.
        /// </summary>
        /// <returns>MatchEntered ViewResult.</returns>
        public ActionResult MatchEntered()
        {
            var matchEnteredViewModel = new MatchEnteredViewModel();
            matchEnteredViewModel.BalanceUpDay = this._winLossService.GetBalanceUpDay();
            matchEnteredViewModel.SportList = SingbetSportService.GetSport(isOther: false);
            matchEnteredViewModel.BettypeList = this._winLossService.GetBettype();
            matchEnteredViewModel.PeriodList = _periodDateRangeSerivce.ListOfPeriods();
            var isShowDataTest = false;
            if (SessionManager.CurrentSession != null)
            {
                isShowDataTest = AppSettings.CurrencyTesting == CurrentSession.CurrencyId || CurrentSession.IsSuper;
            }

            matchEnteredViewModel.MatchEnteredModelList = this._winLossService.GetMatchEntered(isShowDataTest);

            var functionPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);

            // If not have permission on win loss report
            if (functionPermission[ApolloFunctionKeys.WinLoss] < (byte)Policy.Query)
            {
                if (functionPermission[ApolloFunctionKeys.Outstanding] == (byte)Policy.Query)
                {
                    return RedirectToAction("OutStanding", "Reports");
                }

                if (functionPermission[ApolloFunctionKeys.Transactions] == (byte)Policy.Query)
                {
                    return RedirectToAction("Transactions", "BetList");
                }

                if (functionPermission[ApolloFunctionKeys.CancelledTickets] == (byte)Policy.Query)
                {
                    return RedirectToAction("TicketCancel", "BetList");
                }

                if (functionPermission[ApolloFunctionKeys.LineSportCredits] == (byte)Policy.Query)
                {
                    return RedirectToAction("SportCredit", "Reports");
                }

                return RedirectToAction("AccessDenied", "Error");
            }

            ViewBag.allFunctionPermission = functionPermission;
            matchEnteredViewModel.ThisPeriod = _periodDateRangeSerivce.PeriodDates();
            matchEnteredViewModel.LastPeriod = _periodDateRangeSerivce.PeriodDates(true);

            return this.View(matchEnteredViewModel);
        }

        /// <summary>
        /// Get agent win loss report.
        /// </summary>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="sporttypes">Sport type list.</param>
        /// <param name="bettypes">Bet type list.</param>
        /// <param name="agentId">The agent ID.</param>
        /// <param name="agentName">Agent name.</param>
        /// <param name="isHistory">If set to <c>true</c> [is history].</param>
        /// <returns>
        /// ViewResult agent win loss.
        /// </returns>
        [ValidateInput(false)]
        public ViewResult AgentWinLoss(
            DateTime? fromDate,
            DateTime? toDate,
            string sporttypes,
            string bettypes,
            int? agentId,
            string agentName,
            bool isHistory = false)
        {
            var balanceUpDay = this._winLossService.GetBalanceUpDay();

            // Get user permission
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);

            var hasPermission = userPermission[ApolloFunctionKeys.WinLoss] >= (byte)Policy.Query;
            var hasDownlineDetailsPermission = userPermission[ApolloFunctionKeys.DownlineDetails] >= (byte)Policy.Query;

            List<CustomerCounterViewModel> customerCounterList = null;

            ViewBag.allFunctionPermission = userPermission;

            if ((fromDate == null || toDate == null) && isHistory)
            {
                return View(new AgentWinLossViewModel
                {
                    FromDate = _periodDateRangeSerivce.FivePeriodRanges().First().FromDate,
                    ToDate = _periodDateRangeSerivce.FivePeriodRanges().First().ToDate,
                    SportTypeIds = "0",
                    BetTypeIds = "0",
                    AgentId = this.CurrentSession.AgentId,
                    AgentName = this.CurrentSession.AgentName,
                    IsSuper = this.CurrentSession.IsSuper,
                    AgentWinLossList = null,
                    CustomerCounterList = null,
                    DownlinePath = null,
                    SportList = SingbetSportService.GetSport(isOther: false),
                    BettypeList = this._winLossService.GetBettype(true),
                    BalanceUpDay = balanceUpDay,
                    ThisPeriod = isHistory ? _periodDateRangeSerivce.FivePeriodRanges().First() : _periodDateRangeSerivce.PeriodDates(),
                    LastPeriod = isHistory ? _periodDateRangeSerivce.FivePeriodRanges().Last() : _periodDateRangeSerivce.PeriodDates(true),
                    FivePeriods = _periodDateRangeSerivce.FivePeriodRanges(),
                    PeriodList = _periodDateRangeSerivce.ListOfPeriods(),
                    IsHistoryReport = isHistory
                });
            }

            if (!hasPermission || (agentId != null && !hasDownlineDetailsPermission))
            {
                return View(
                    "ReportAccessDenied",
                    new AgentWinLossViewModel
                    {
                        SportTypeIds = sporttypes,
                        BetTypeIds = bettypes,
                        SportList = SingbetSportService.GetSport(isOther: false)
                    });
            }

            var criteria = new AgentWinLossCriteria
            {
                AgentId = agentId.HasValue ? agentId.Value : this.CurrentSession.AgentId,
                Sporttypes = sporttypes == "-99" ? "0" : sporttypes,
                Bettypes = bettypes,
                FromDate = fromDate.Value,
                ToDate = toDate.Value,
                BalanceUpDay = balanceUpDay,
                MoveBetDay = this._winLossService.MoveBetsDay()
            };

            List<AgentWinLossModel> agentWinLossList = null;
            List<Agent> modelAgentDownLine = null;
            if (!isHistory || !IsPeakHour())
            {
                agentWinLossList = this._winLossService.GetAgentWinLoss(criteria);
                modelAgentDownLine = this._winLossService.AgentDownlinePath(this.CurrentSession.AgentId, criteria.AgentId).ToList();

                // Only super and main shareholder can see number of member.
                if (modelAgentDownLine.Last().Level <= 1)
                {
                    customerCounterList = this._winLossService.GetCustomerCounter(new CustomerCounterCriteria
                    {
                        AgentId = agentId.HasValue ? agentId.Value : this.CurrentSession.AgentId,
                        Sporttypes = sporttypes == "-99" ? "0" : sporttypes,
                        Bettypes = bettypes,
                        FromDate = fromDate.Value,
                        ToDate = toDate.Value,
                        BalanceUpDay = balanceUpDay,
                        MoveBetDay = this._winLossService.MoveBetsDay()
                    });
                }
            }

            var isSuper = criteria.AgentId == this.CurrentSession.AgentId ? this.CurrentSession.IsSuper : false;

            var model = new AgentWinLossViewModel
            {
                FromDate = fromDate.Value,
                ToDate = toDate.Value,
                SportTypeIds = sporttypes,
                BetTypeIds = bettypes,
                AgentId = criteria.AgentId,
                AgentName = string.IsNullOrWhiteSpace(agentName) ? this.CurrentSession.AgentName : agentName,
                IsSuper = isSuper,
                AgentWinLossList = agentWinLossList,
                CustomerCounterList = customerCounterList,
                DownlinePath = modelAgentDownLine,
                SportList = SingbetSportService.GetSport(isOther: false),
                BettypeList = sporttypes == "-99" ? this._winLossService.GetParlayBettype() : this._winLossService.GetBettype(sporttypes == "-99" || sporttypes == "0"),
                BalanceUpDay = balanceUpDay,
                ThisPeriod = isHistory ? _periodDateRangeSerivce.FivePeriodRanges().First() : _periodDateRangeSerivce.PeriodDates(),
                LastPeriod = isHistory ? _periodDateRangeSerivce.FivePeriodRanges().Last() : _periodDateRangeSerivce.PeriodDates(true),
                FivePeriods = _periodDateRangeSerivce.FivePeriodRanges(),
                PeriodList = _periodDateRangeSerivce.ListOfPeriods(),
                IsHistoryReport = isHistory,
                IsPeakHour = isHistory ? IsPeakHour() : false
            };

            var sessionKey = string.Format(Constants.SessionKeyAgentWinLossReport, this.CurrentSession.UserId);

            HttpContext.Session[sessionKey] = agentWinLossList;

            return this.View(model);
        }

        /// <summary>
        /// Get member win loss report.
        /// </summary>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="sporttypes">Sport type list.</param>
        /// <param name="bettypes">Bet type list.</param>
        /// <param name="agentId">The agent ID.</param>
        /// <param name="agentName">Agent name.</param>
        /// <returns>ViewResult member win loss.</returns>
        [ValidateInput(false)]
        public ViewResult MemberWinLoss(
            DateTime fromDate,
            DateTime toDate,
            string sporttypes,
            string bettypes,
            int agentId,
            string agentName)
        {
            // Get user permission
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);

            var hasPermission = userPermission[ApolloFunctionKeys.WinLoss] >= (byte)Policy.Query;
            var hasDownlineDetailsPermission = userPermission[ApolloFunctionKeys.DownlineDetails] >= (byte)Policy.Query;

            ViewBag.allFunctionPermission = userPermission;

            if (!hasPermission || !hasDownlineDetailsPermission)
            {
                return View(
                    "ReportAccessDenied",
                    new MemberWinLossViewModel
                    {
                        SportTypeIds = sporttypes,
                        BetTypeIds = bettypes,
                        SportList = SingbetSportService.GetSport(isOther: false)
                    });
            }

            var balanceUpDay = this._winLossService.GetBalanceUpDay();
            var criteria = new MemberWinLossCriteria
            {
                AgentId = agentId,
                Sporttypes = sporttypes == "-99" ? "0" : sporttypes,
                Bettypes = bettypes,
                FromDate = fromDate,
                ToDate = toDate,
                BalanceUpDay = balanceUpDay,
                MoveBetDay = this._winLossService.MoveBetsDay()
            };

            var memberWinLossList = this._winLossService.GetMemberWinLoss(criteria);
            var modelAgentDownLine = this._winLossService.AgentDownlinePath(CurrentSession.AgentId, criteria.AgentId).ToList();

            var model = new MemberWinLossViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                SportTypeIds = sporttypes,
                BetTypeIds = bettypes,
                AgentId = agentId,
                AgentName = agentName,
                MemberWinLossList = memberWinLossList,
                DownlinePath = modelAgentDownLine,
                SportList = SingbetSportService.GetSport(isOther: false),
                BettypeList = sporttypes == "-99" ? this._winLossService.GetParlayBettype() : this._winLossService.GetBettype(sporttypes == "-99" || sporttypes == "0"),
                BalanceUpDay = balanceUpDay,
                ThisPeriod = _periodDateRangeSerivce.PeriodDates(),
                LastPeriod = _periodDateRangeSerivce.PeriodDates(true)
            };

            var sessionKey = string.Format(Constants.SessionKeyMemberWinLossReport, this.CurrentSession.UserId);
            HttpContext.Session[sessionKey] = memberWinLossList;

            return this.View(model);
        }

        /// <summary>
        /// Export agent win loss report.
        /// </summary>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="sporttypes">Sport type list.</param>
        /// <param name="bettypes">Bet type list.</param>
        /// <param name="agentId">The agent ID.</param>
        /// <param name="agentName">Agent name.</param>
        /// <returns>ActionResult agent win loss.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.WinLoss, Policy = Policy.Query)]
        public ActionResult ExportAgentWinLoss(
            DateTime fromDate,
            DateTime toDate,
            string sporttypes,
            string bettypes,
            int? agentId,
            string agentName)
        {
            // Get user permission
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);

            var hasPermission = userPermission[ApolloFunctionKeys.WinLoss] >= (byte)Policy.Query;
            var hasDownlineDetailsPermission = userPermission[ApolloFunctionKeys.DownlineDetails] >= (byte)Policy.Query;

            ViewBag.allFunctionPermission = userPermission;

            List<CustomerCounterViewModel> customerCounterList = null;

            if (!hasPermission)
            {
                return View(
                    "ReportAccessDenied",
                    new AgentWinLossViewModel
                    {
                        SportTypeIds = sporttypes,
                        BetTypeIds = bettypes,
                        SportList = SingbetSportService.GetSport(isOther: false)
                    });
            }

            var sessionKey = string.Format(Constants.SessionKeyAgentWinLossReport, this.CurrentSession.UserId);
            var agentWinLossList = (List<AgentWinLossModel>)HttpContext.Session[sessionKey];
            var agentIdValue = agentId.HasValue ? agentId.Value : this.CurrentSession.AgentId;
            var balanceUpDay = this._winLossService.GetBalanceUpDay();

            if (agentWinLossList == null)
            {
                var criteria = new AgentWinLossCriteria
                {
                    AgentId = agentIdValue,
                    Sporttypes = sporttypes == "-99" ? "0" : sporttypes,
                    Bettypes = bettypes,
                    FromDate = fromDate,
                    ToDate = toDate,
                    BalanceUpDay = balanceUpDay,
                    MoveBetDay = this._winLossService.MoveBetsDay()
                };

                agentWinLossList = this._winLossService.GetAgentWinLoss(criteria);
                HttpContext.Session[sessionKey] = agentWinLossList;
            }

            if (!hasDownlineDetailsPermission
                && agentWinLossList != null
                && agentWinLossList.Any())
            {
                for (int i = 0; i < agentWinLossList.Count; i++)
                {
                    agentWinLossList[i].AgentName = Web.Mvc.HtmlHelperExtensions.EncryptUsername(agentWinLossList[i].AgentName);
                }
            }

            var modelAgentDownLine = this._winLossService.AgentDownlinePath(this.CurrentSession.AgentId, agentId.Value).ToList();

            // Only super and main shareholder can see number of member.
            if (modelAgentDownLine.Last().Level <= 1)
            {
                customerCounterList = this._winLossService.GetCustomerCounter(new CustomerCounterCriteria
                {
                    AgentId = agentId.HasValue ? agentId.Value : this.CurrentSession.AgentId,
                    Sporttypes = sporttypes == "-99" ? "0" : sporttypes,
                    Bettypes = bettypes,
                    FromDate = fromDate,
                    ToDate = toDate,
                    BalanceUpDay = balanceUpDay,
                    MoveBetDay = this._winLossService.MoveBetsDay()
                });
            }

            var model = new AgentWinLossViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                SportTypeIds = sporttypes,
                BetTypeIds = bettypes,
                AgentId = agentIdValue,
                AgentName = string.IsNullOrWhiteSpace(agentName) ? this.CurrentSession.AgentName : agentName,
                IsSuper = agentIdValue == this.CurrentSession.AgentId ? this.CurrentSession.IsSuper : false,
                AgentWinLossList = agentWinLossList,
                CustomerCounterList = customerCounterList,
                SportList = SingbetSportService.GetSport(isOther: false),
                BettypeList = sporttypes == "-99" ? this._winLossService.GetParlayBettype() : this._winLossService.GetBettype(sporttypes == "-99" || sporttypes == "0"),
                BalanceUpDay = balanceUpDay,
            };

            var excelResult = this._winLossService.ExportAgentWinLoss(model);

            var fileName = string.Format(
                "AgentWinLoss_{0}_{1}_{2}",
                agentName,
                this._winLossService.ShortDateFormat(model.FromDate),
                this._winLossService.ShortDateFormat(model.ToDate));
            return new ExcelResult(excelResult, fileName);
        }

        /// <summary>
        /// Export member win loss report.
        /// </summary>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="sporttypes">Sport type list.</param>
        /// <param name="bettypes">Bet type list.</param>
        /// <param name="agentId">The agent ID.</param>
        /// <param name="agentName">Agent name.</param>
        /// <returns>ActionResult member win loss.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.WinLoss, Policy = Policy.Query)]
        public ActionResult ExportMemberWinLoss(
            DateTime fromDate,
            DateTime toDate,
            string sporttypes,
            string bettypes,
            int agentId,
            string agentName)
        {
            // Get user permission
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);

            var hasPermission = userPermission[ApolloFunctionKeys.WinLoss] >= (byte)Policy.Query;
            var hasDownlineDetailsPermission = userPermission[ApolloFunctionKeys.DownlineDetails] >= (byte)Policy.Query;

            ViewBag.allFunctionPermission = userPermission;

            if (!hasPermission)
            {
                return View(
                    "ReportAccessDenied",
                    new MemberWinLossViewModel
                    {
                        SportTypeIds = sporttypes,
                        BetTypeIds = bettypes,
                        SportList = SingbetSportService.GetSport(isOther: false)
                    });
            }

            var sessionKey = string.Format(Constants.SessionKeyMemberWinLossReport, this.CurrentSession.UserId);
            var memberWinLossList = (List<MemberWinLossModel>)HttpContext.Session[sessionKey];
            var balanceUpDay = this._winLossService.GetBalanceUpDay();

            if (memberWinLossList == null)
            {
                var criteria = new MemberWinLossCriteria
                {
                    AgentId = agentId,
                    Sporttypes = sporttypes == "-99" ? "0" : sporttypes,
                    Bettypes = bettypes,
                    FromDate = fromDate,
                    ToDate = toDate,
                    BalanceUpDay = balanceUpDay,
                    MoveBetDay = this._winLossService.MoveBetsDay()
                };

                memberWinLossList = this._winLossService.GetMemberWinLoss(criteria);
                HttpContext.Session[sessionKey] = memberWinLossList;
            }

            if (!hasDownlineDetailsPermission
                && memberWinLossList != null
                && memberWinLossList.Any())
            {
                for (int i = 0; i < memberWinLossList.Count; i++)
                {
                    memberWinLossList[i].MemberName = Web.Mvc.HtmlHelperExtensions.EncryptUsername(memberWinLossList[i].MemberName);
                }
            }

            var model = new MemberWinLossViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                SportTypeIds = sporttypes,
                BetTypeIds = bettypes,
                AgentId = agentId,
                AgentName = agentName,
                MemberWinLossList = memberWinLossList,
                SportList = SingbetSportService.GetSport(isOther: false),
                BettypeList = sporttypes == "-99" ? this._winLossService.GetParlayBettype() : this._winLossService.GetBettype(sporttypes == "-99" || sporttypes == "0"),
                BalanceUpDay = balanceUpDay
            };

            var excelResult = this._winLossService.ExportMemberWinLoss(model);

            var fileName = string.Format(
                "MemberWinLoss_{0}_{1}_{2}",
                model.AgentName,
                this._winLossService.ShortDateFormat(model.FromDate),
                this._winLossService.ShortDateFormat(model.ToDate));

            return new ExcelResult(excelResult, fileName);
        }

        #endregion Agent Win Loss Report

        #region Line sport credit

        /// <summary>
        /// Get Line sport credit report.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <returns>ActionResult Line sport credit report.</returns>
        [ValidateInput(false)]
        public ActionResult SportCredit(string username)
        {
            // Get user permission
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);

            var hasPermission = userPermission[ApolloFunctionKeys.LineSportCredits] >= (byte)Policy.Query;
            var hasDownlineDetailsPermission = userPermission[ApolloFunctionKeys.DownlineDetails] >= (byte)Policy.Query;

            ViewBag.allFunctionPermission = userPermission;

            if (!hasPermission)
            {
                return View("ReportAccessDenied");
            }

            if (!string.IsNullOrWhiteSpace(username)
                && !string.Equals(username, this.CurrentSession.AgentName)
                && !hasDownlineDetailsPermission)
            {
                username = string.Empty;
            }

            var criteria = new SportCreditCriteria
            {
                LoggedId = this.CurrentSession.AgentId,
                Username = username
            };

            var sportCreditList = this._sportCreditService.GetSportCredits(criteria);

            var model = new SportCreditViewModel
            {
                Username = username,
                SportCreditModelList = sportCreditList
            };

            var sessionKey = string.Format(Constants.SessionKeyLineSportCreditReport, this.CurrentSession.UserId);
            HttpContext.Session[sessionKey] = sportCreditList;

            return this.View(model);
        }

        /// <summary>
        /// Export Line sport credit report.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <returns>ActionResult Line sport credit report.</returns>
        public ActionResult ExportSportCredit(string username)
        {
            // Get user permission
            var userPermission = _userService.GetAllUserPermission(this.CurrentSession.UserId);

            var hasPermission = userPermission[ApolloFunctionKeys.LineSportCredits] >= (byte)Policy.Query;

            ViewBag.allFunctionPermission = userPermission;

            if (!hasPermission)
            {
                return View("ReportAccessDenied");
            }

            var sessionKey = string.Format(Constants.SessionKeyLineSportCreditReport, this.CurrentSession.UserId);
            var sportCreditList = (List<SportCreditModel>)HttpContext.Session[sessionKey];

            if (sportCreditList == null)
            {
                var criteria = new SportCreditCriteria
                {
                    LoggedId = this.CurrentSession.AgentId,
                    Username = username
                };

                sportCreditList = this._sportCreditService.GetSportCredits(criteria);
                HttpContext.Session[sessionKey] = sportCreditList;
            }

            var model = new SportCreditViewModel
            {
                Username = username,
                SportCreditModelList = sportCreditList
            };

            var excelResult = this._sportCreditService.ExportSportCredit(model);

            var fileName = string.Format("LineSportCredit_{0}_{1}", model.Username, this._sportCreditService.ShortDateFormat(DateTime.Now));

            return new ExcelResult(excelResult, fileName);
        }

        #endregion Line sport credit

        #region Common Action

        /// <summary>
        /// Creates the type of the bet.
        /// </summary>
        /// <param name="isIncludeParlay">Include parlay bet type or not.</param>
        /// <returns>JSON bet type.</returns>
        public JsonResult CreateBetType(bool isIncludeParlay)
        {
            var bettypeList = this._winLossService.GetBettype(isIncludeParlay);

            return Json(bettypeList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Creates the parlay bet type.
        /// </summary>
        /// <returns>JSON bet type.</returns>
        public JsonResult CreateParlayBettype()
        {
            var parlayBettypeList = this._winLossService.GetParlayBettype();

            return Json(parlayBettypeList, JsonRequestBehavior.AllowGet);
        }

        #endregion Common Action

        /// <summary>
        /// Determines whether [is peak hour].
        /// </summary>
        /// <returns>True for Peak hour.</returns>
        private bool IsPeakHour()
        {
            var currentDate = AppSettings.PeriodDateRange;
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            TimeZoneInfo gmtPlus8 = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            DateTime gmtPlus8Time = TimeZoneInfo.ConvertTime(currentDate, localTimeZone, gmtPlus8);
            string[] peakHour = AppSettings.ReportPeakHour.Split('>', ':');
            if (peakHour.Count() <= 0 || peakHour.Count() > 4)
            {
                return false;
            }
            else
            {
                // Hard code for peak hour. The history report available time 9:00 - 17:00 GMT +8
                DateTime fromHour = new DateTime(gmtPlus8Time.Year, gmtPlus8Time.Month, gmtPlus8Time.Day, int.Parse(peakHour[0]), int.Parse(peakHour[1]), 0);
                DateTime toHour = new DateTime(gmtPlus8Time.Year, gmtPlus8Time.Month, gmtPlus8Time.Day, int.Parse(peakHour[2]), int.Parse(peakHour[3]), 0);

                if (gmtPlus8Time >= fromHour && gmtPlus8Time <= toHour)
                {
                    return false;
                }
            }

            return true;
        }
    }
}