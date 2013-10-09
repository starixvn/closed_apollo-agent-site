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
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Apollo.Age.Configurations;
    using Apollo.Age.PermissionControl.Services;
    using Apollo.Age.Reports.Core;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Web;
    using Fanex.NAP.Framework.Mvc;
    using Apollo.Age.Web.Web.Attribute;
    using Apollo.Age.PermissionControl.Core;

    /// <summary>
    /// Reports Controller.
    /// </summary>
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class ReportsController : BaseController
    {
        private readonly SportCreditService _sportCreditService;
        private readonly WinLossService _winLossService;
        private readonly OutstandingService _outstandingService;
        private readonly IAgentUserService _agentUserService;

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
            this._outstandingService = new OutstandingService();
            _agentUserService = new AgentUserService();
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
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Outstanding, Policy = Policy.Query)]
        public ViewResult Outstanding(int? agentId, string sporttypes, string bettypes)
        {
            this.CurrentAgentId = agentId;
            var report = this._outstandingService.GetAgentOutStandingReport(this.CurrentSession.IsSuper, this.CurrentAgentId, this.CurrentSession.AgentId, sporttypes, bettypes);
            var sessionKey = string.Format(Constants.SessionKeyAgentOutstandingReport, this.CurrentSession.UserId);

            HttpContext.Session[sessionKey] = report.ListOutStanding;
            ViewBag.allFunctionPermission = _agentUserService.GetUserFunctionPolicy(agentUserId: CurrentSession.UserId, isActive: true);

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
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Outstanding, Policy = Policy.Query)]
        public ActionResult ExportOutstandingReport(
            int? agentId,
            string agentUsername,
            string sporttypes,
            string sportName,
            string bettypes,
            string betTypeName,
            bool isAgentOutStanding)
        {
            this.CurrentAgentId = agentId;
            var sessionKey = string.Format(
                isAgentOutStanding ? Constants.SessionKeyAgentOutstandingReport : Constants.SessionKeyMemberOutstandingReport,
                this.CurrentSession.UserId);

            var listOutstanding = (IEnumerable<OutstandingModel>)HttpContext.Session[sessionKey];

            if (listOutstanding == null)
            {
                if (isAgentOutStanding)
                {
                    listOutstanding = this._outstandingService.GetData<OutstandingModel>(new OutstandingCriteria
                    {
                        AgentId = this.CurrentAgentId.Value,
                        Sporttypes = sporttypes,
                        Bettypes = bettypes
                    });
                }
                else
                {
                    listOutstanding = this._outstandingService.GetData<OutstandingModel>(new MemberOutstandingCriteria
                    {
                        AgentId = this.CurrentAgentId.Value,
                        Sporttypes = sporttypes,
                        Bettypes = bettypes
                    });
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
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Outstanding, Policy = Policy.Query)]
        public ActionResult MemberOutstanding(
            int? agentId,
            string sporttypes,
            string bettypes,
            int mainShareHolderId,
            string mainShareHolderUsername)
        {
            this.CurrentAgentId = agentId;

            //// call service to get listOutstanding data here
            var report = this._outstandingService.GetMemberOutstandingReport(this.CurrentAgentId, sporttypes, bettypes, mainShareHolderId, mainShareHolderUsername);
            var sessionKey = string.Format(Constants.SessionKeyMemberOutstandingReport, this.CurrentSession.UserId);

            HttpContext.Session[sessionKey] = report.ListOutstanding;
            ViewBag.allFunctionPermission = _agentUserService.GetUserFunctionPolicy(agentUserId: CurrentSession.UserId, isActive: true);
            //// return object
            return this.Json(report);
        }

        #endregion Outstanding Report

        #region Agent Win Loss Report

        /// <summary>
        /// Get MatchEntered report.
        /// </summary>
        /// <returns>MatchEntered ViewResult.</returns>        
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Win_Loss, Policy = Policy.Query)]
        public ViewResult MatchEntered()
        {
            var matchEnteredViewModel = new MatchEnteredViewModel();
            matchEnteredViewModel.BalanceUpDay = this._winLossService.GetBalanceUpDay();
            matchEnteredViewModel.SportList = SingbetSportService.GetSport(isOther: false);
            matchEnteredViewModel.BettypeList = this._winLossService.GetBettype();

            var isShowDataTest = false;
            if (SessionManager.CurrentSession != null)
            {
                isShowDataTest = AppSettings.CurrencyTesting == CurrentSession.CurrencyId || CurrentSession.IsSuper;
            }

            matchEnteredViewModel.MatchEnteredModelList = this._winLossService.GetMatchEntered(isShowDataTest);
            ViewBag.allFunctionPermission = _agentUserService.GetUserFunctionPolicy(agentUserId: CurrentSession.UserId, isActive: true);

            return this.View(matchEnteredViewModel);
        }

        /// <summary>
        /// Get agent win loss report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="sporttypes">Sport type list.</param>
        /// <param name="bettypes">Bet type list.</param>
        /// <param name="agentId">Agent id.</param>
        /// <param name="agentName">Agent name.</param>
        /// <returns>ViewResult agent win loss.</returns>        
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Win_Loss, Policy = Policy.Query)]
        public ViewResult AgentWinLoss(
            DateTime fromDate,
            DateTime toDate,
            string sporttypes,
            string bettypes,
            int? agentId,
            string agentName)
        {
            var balanceUpDay = this._winLossService.GetBalanceUpDay();
            var criteria = new AgentWinLossCriteria
            {
                AgentId = agentId.HasValue ? agentId.Value : this.CurrentSession.AgentId,
                Sporttypes = sporttypes,
                Bettypes = bettypes,
                FromDate = fromDate,
                ToDate = toDate,
                BalanceUpDay = balanceUpDay,
                MoveBetDay = this._winLossService.MoveBetsDay()
            };

            var agentWinLossList = this._winLossService.GetAgentWinLoss(criteria);
            var modelAgentDownLine = this._winLossService.AgentDownlinePath(this.CurrentSession.AgentId, criteria.AgentId).ToList();
            var isSuper = criteria.AgentId == this.CurrentSession.AgentId ? this.CurrentSession.IsSuper : false;

            var model = new AgentWinLossViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                Sporttypes = sporttypes,
                Bettypes = bettypes,
                AgentId = criteria.AgentId,
                AgentName = string.IsNullOrWhiteSpace(agentName) ? this.CurrentSession.AgentName : agentName,
                IsSuper = isSuper,
                AgentWinLossList = agentWinLossList,
                DownlinePath = modelAgentDownLine,
                SportList = SingbetSportService.GetSport(isOther: false),
                BettypeList = this._winLossService.GetBettype(),
                BalanceUpDay = balanceUpDay
            };

            var sessionKey = string.Format(Constants.SessionKeyAgentWinLossReport, this.CurrentSession.UserId);

            HttpContext.Session[sessionKey] = agentWinLossList;
            ViewBag.allFunctionPermission = _agentUserService.GetUserFunctionPolicy(agentUserId: CurrentSession.UserId, isActive: true);

            return this.View(model);
        }

        /// <summary>
        /// Get member win loss report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="sporttypes">Sport type list.</param>
        /// <param name="bettypes">Bet type list.</param>
        /// <param name="agentId">Agent id.</param>
        /// <param name="agentName">Agent name.</param>
        /// <returns>ViewResult member win loss.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Win_Loss, Policy = Policy.Query)]
        public ViewResult MemberWinLoss(
            DateTime fromDate,
            DateTime toDate,
            string sporttypes,
            string bettypes,
            int agentId,
            string agentName)
        {
            var balanceUpDay = this._winLossService.GetBalanceUpDay();
            var criteria = new MemberWinLossCriteria
            {
                AgentId = agentId,
                Sporttypes = sporttypes,
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
                Sporttypes = sporttypes,
                Bettypes = bettypes,
                AgentId = agentId,
                AgentName = agentName,
                MemberWinLossList = memberWinLossList,
                DownlinePath = modelAgentDownLine,
                SportList = SingbetSportService.GetSport(isOther: false),
                BettypeList = this._winLossService.GetBettype(),
                BalanceUpDay = balanceUpDay
            };

            var sessionKey = string.Format(Constants.SessionKeyMemberWinLossReport, this.CurrentSession.UserId);
            HttpContext.Session[sessionKey] = memberWinLossList;
            ViewBag.allFunctionPermission = _agentUserService.GetUserFunctionPolicy(agentUserId: CurrentSession.UserId, isActive: true);

            return this.View(model);
        }

        /// <summary>
        /// Export agent win loss report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="sporttypes">Sport type list.</param>
        /// <param name="bettypes">Bet type list.</param>
        /// <param name="agentId">Agent id.</param>
        /// <param name="agentName">Agent name.</param>
        /// <returns>ActionResult agent win loss.</returns>        
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Win_Loss, Policy = Policy.Query)]
        public ActionResult ExportAgentWinLoss(
            DateTime fromDate,
            DateTime toDate,
            string sporttypes,
            string bettypes,
            int? agentId,
            string agentName)
        {
            var sessionKey = string.Format(Constants.SessionKeyAgentWinLossReport, this.CurrentSession.UserId);
            var agentWinLossList = (List<AgentWinLossModel>)HttpContext.Session[sessionKey];
            var agentIdValue = agentId.HasValue ? agentId.Value : this.CurrentSession.AgentId;
            var balanceUpDay = this._winLossService.GetBalanceUpDay();

            if (agentWinLossList == null)
            {
                var criteria = new AgentWinLossCriteria
                {
                    AgentId = agentIdValue,
                    Sporttypes = sporttypes,
                    Bettypes = bettypes,
                    FromDate = fromDate,
                    ToDate = toDate,
                    BalanceUpDay = balanceUpDay,
                    MoveBetDay = this._winLossService.MoveBetsDay()
                };

                agentWinLossList = this._winLossService.GetAgentWinLoss(criteria);
                HttpContext.Session[sessionKey] = agentWinLossList;
            }

            var model = new AgentWinLossViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                Sporttypes = sporttypes,
                Bettypes = bettypes,
                AgentId = agentIdValue,
                AgentName = string.IsNullOrWhiteSpace(agentName) ? this.CurrentSession.AgentName : agentName,
                IsSuper = agentIdValue == this.CurrentSession.AgentId ? this.CurrentSession.IsSuper : false,
                AgentWinLossList = agentWinLossList,
                SportList = SingbetSportService.GetSport(isOther: false),
                BettypeList = this._winLossService.GetBettype(),
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
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="sporttypes">Sport type list.</param>
        /// <param name="bettypes">Bet type list.</param>
        /// <param name="agentId">Agent id.</param>
        /// <param name="agentName">Agent name.</param>
        /// <returns>ActionResult member win loss.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.Win_Loss, Policy = Policy.Query)]
        public ActionResult ExportMemberWinLoss(
            DateTime fromDate,
            DateTime toDate,
            string sporttypes,
            string bettypes,
            int agentId,
            string agentName)
        {
            var sessionKey = string.Format(Constants.SessionKeyMemberWinLossReport, this.CurrentSession.UserId);
            var memberWinLossList = (List<MemberWinLossModel>)HttpContext.Session[sessionKey];
            var balanceUpDay = this._winLossService.GetBalanceUpDay();

            if (memberWinLossList == null)
            {
                var criteria = new MemberWinLossCriteria
                {
                    AgentId = agentId,
                    Sporttypes = sporttypes,
                    Bettypes = bettypes,
                    FromDate = fromDate,
                    ToDate = toDate,
                    BalanceUpDay = balanceUpDay,
                    MoveBetDay = this._winLossService.MoveBetsDay()
                };

                memberWinLossList = this._winLossService.GetMemberWinLoss(criteria);
                HttpContext.Session[sessionKey] = memberWinLossList;
            }

            var model = new MemberWinLossViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                Sporttypes = sporttypes,
                Bettypes = bettypes,
                AgentId = agentId,
                AgentName = agentName,
                MemberWinLossList = memberWinLossList,
                SportList = SingbetSportService.GetSport(isOther: false),
                BettypeList = this._winLossService.GetBettype(),
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
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.LineSportCredits, Policy = Policy.Query)]
        public ActionResult SportCredit(string username)
        {
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
            ViewBag.allFunctionPermission = _agentUserService.GetUserFunctionPolicy(agentUserId: CurrentSession.UserId, isActive: true);

            return this.View(model);
        }

        /// <summary>
        /// Export Line sport credit report.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <returns>ActionResult Line sport credit report.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.LineSportCredits, Policy = Policy.Query)]
        public ActionResult ExportSportCredit(string username)
        {
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

        #endregion
    }
}