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
// </history>

namespace Apollo.Age.Reports.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Apollo.Age.Reports.Core;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Resources;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Web;
    using Fanex.NAP.Framework.Mvc;
    using System;

    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class ReportsController : BaseController
    {
        private readonly SportCreditService _sportCreditService;
        private readonly WinLossService _winLossService;
        private readonly OutstandingService _outstandingService;

        private int _currentAgentId;

        public ReportsController()
            : this(null)
        {
        }

        public ReportsController(
            OutstandingService outstandingService = null,
            WinLossService winlossService = null,
            SportCreditService sportCreditService = null)
        {
            _sportCreditService = sportCreditService ?? new SportCreditService();
            _winLossService = winlossService ?? new WinLossService();
            _outstandingService = new OutstandingService();
        }

        public int? CurrentAgentId
        {
            get
            {
                return _currentAgentId;
            }

            set
            {
                _currentAgentId = value.HasValue ? value.Value : this.CurrentSession.AgentId;
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
            CurrentAgentId = agentId;
            var report = _outstandingService.GetAgentOutStandingReport(CurrentSession.IsSuper, CurrentAgentId, CurrentSession.AgentId, sporttypes, bettypes);
            var sessionKey = string.Format(Constants.SessionKeyAgentOutstandingReport, this.CurrentSession.UserId);

            HttpContext.Session[sessionKey] = report.ListOutStanding;

            return View(report);
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
            CurrentAgentId = agentId;
            var sessionKey = string.Format(
                isAgentOutStanding ? Constants.SessionKeyAgentOutstandingReport : Constants.SessionKeyMemberOutstandingReport,
                this.CurrentSession.UserId);

            var listOutstanding = (IEnumerable<OutstandingModel>)HttpContext.Session[sessionKey];

            if (listOutstanding == null)
            {
                if (isAgentOutStanding)
                {
                    listOutstanding = _outstandingService.GetData<OutstandingModel>(new OutstandingCriteria
                    {
                        AgentId = CurrentAgentId.Value,
                        Sporttypes = sporttypes,
                        Bettypes = bettypes
                    });
                }
                else
                {
                    listOutstanding = _outstandingService.GetData<OutstandingModel>(new MemberOutstandingCriteria
                    {
                        AgentId = CurrentAgentId.Value,
                        Sporttypes = sporttypes,
                        Bettypes = bettypes
                    });
                }
            }

            var isSupper = CurrentSession.IsSuper && (CurrentAgentId == CurrentSession.AgentId);
            var excelResult = _outstandingService.ExportOutstandingReport(
                isAgentOutStanding,
                listOutstanding,
                agentUsername,
                HttpUtility.UrlDecode(sportName),
                HttpUtility.UrlDecode(betTypeName),
                isSupper);

            return new ExcelResult(excelResult, string.Format("Outstanding_{0}_{1}", agentUsername, _outstandingService.ShortDateFormat(DateTime.Now)));
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
        public ActionResult MemberOutstanding(int? agentId, string sporttypes, string bettypes, int mainShareHolderId, string mainShareHolderUsername)
        {
            CurrentAgentId = agentId;

            //// call service to get listOutstanding data here
            var report = _outstandingService.GetMemberOutstandingReport(CurrentAgentId, sporttypes, bettypes, mainShareHolderId, mainShareHolderUsername);
            var sessionKey = string.Format(Constants.SessionKeyMemberOutstandingReport, this.CurrentSession.UserId);

            HttpContext.Session[sessionKey] = report.ListOutstanding;

            //// return object
            return Json(report);
        }

        #endregion Outstanding Report

        #region Agent Win Loss Report

        /// <summary>
        /// Get MatchEntered report.
        /// </summary>
        /// <returns>MatchEntered ViewResult.</returns>
        public ViewResult MatchEntered()
        {
            var matchEnteredViewModel = new MatchEnteredViewModel();
            matchEnteredViewModel.BalanceUpDay = _winLossService.GetBalanceUpDay();
            matchEnteredViewModel.SportList = SingbetSportService.GetSport(isOther: false);
            matchEnteredViewModel.BettypeList = _winLossService.GetBettype();
            matchEnteredViewModel.MatchEnteredModelList = _winLossService.GetMatchEntered();

            return View(matchEnteredViewModel);
        }

        /// <summary>
        /// Get agent win loss report.
        /// </summary>
        /// <param name="model">Input AgentWinLossViewModel.</param>
        /// <returns>AgentWinLoss ViewResult.</returns>
        public ViewResult AgentWinLoss(AgentWinLossViewModel model)
        {
            model.AgentId = model.AgentId == 0 ? this.CurrentSession.AgentId : model.AgentId;
            var agentName = string.IsNullOrWhiteSpace(model.AgentName) ? this.CurrentSession.AgentName : model.AgentName;
            var isSuper = model.AgentId == this.CurrentSession.AgentId ? this.CurrentSession.IsSuper : false;

            var criteria = new AgentWinLossCriteria
            {
                AgentId = model.AgentId,
                Sporttypes = model.Sporttypes,
                Bettypes = model.Bettypes,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                BalanceUpDay = _winLossService.GetBalanceUpDay(),
                MoveBetDay = _winLossService.MoveBetsDay()
            };

            var agentWinLossList = _winLossService.GetAgentWinLoss(criteria);
            var modelAgentDownLine = _winLossService.AgentDownlinePath(CurrentSession.AgentId, model.AgentId).ToList();

            model.IsSuper = isSuper;
            model.AgentName = agentName;
            model.AgentWinLossList = agentWinLossList;
            model.DownlinePath = modelAgentDownLine;
            model.SportList = SingbetSportService.GetSport(isOther: false);
            model.BettypeList = _winLossService.GetBettype();
            model.BalanceUpDay = _winLossService.GetBalanceUpDay();

            var sessionKey = string.Format(Constants.SessionKeyAgentWinLossReport, this.CurrentSession.UserId);

            HttpContext.Session[sessionKey] = agentWinLossList;

            return View(model);
        }

        /// <summary>
        /// Get member win loss report.
        /// </summary>
        /// <param name="model">Input MemberWinLossViewModel.</param>
        /// <returns>MemberWinLoss ViewResult.</returns>
        public ViewResult MemberWinLoss(MemberWinLossViewModel model)
        {
            var criteria = new MemberWinLossCriteria
            {
                AgentId = model.AgentId,
                Sporttypes = model.Sporttypes,
                Bettypes = model.Bettypes,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                BalanceUpDay = _winLossService.GetBalanceUpDay(),
                MoveBetDay = _winLossService.MoveBetsDay()
            };

            var memberWinLossList = _winLossService.GetMemberWinLoss(criteria);
            var modelAgentDownLine = _winLossService.AgentDownlinePath(CurrentSession.AgentId, model.AgentId).ToList();

            model.MemberWinLossList = memberWinLossList;
            model.DownlinePath = modelAgentDownLine;
            model.SportList = SingbetSportService.GetSport(isOther: false);
            model.BettypeList = _winLossService.GetBettype();
            model.BalanceUpDay = _winLossService.GetBalanceUpDay();

            var sessionKey = string.Format(Constants.SessionKeyMemberWinLossReport, this.CurrentSession.UserId);
            HttpContext.Session[sessionKey] = memberWinLossList;

            return View(model);
        }

        /// <summary>
        /// Export excel agent win loss report.
        /// </summary>
        /// <param name="model">Input AgentWinLossViewModel.</param>
        /// <returns>Excel file agent win loss.</returns>
        public ActionResult ExportAgentWinLoss(AgentWinLossViewModel model)
        {
            var sessionKey = string.Format(Constants.SessionKeyAgentWinLossReport, this.CurrentSession.UserId);
            var agentWinLossList = (List<AgentWinLossModel>)HttpContext.Session[sessionKey];

            model.AgentId = model.AgentId == 0 ? this.CurrentSession.AgentId : model.AgentId;
            var agentName = string.IsNullOrWhiteSpace(model.AgentName) ? this.CurrentSession.AgentName : model.AgentName;
            model.IsSuper = model.AgentId == this.CurrentSession.AgentId ? this.CurrentSession.IsSuper : false;

            model.SportList = SingbetSportService.GetSport(isOther: false);
            model.BettypeList = _winLossService.GetBettype();

            if (agentWinLossList == null)
            {
                var criteria = new AgentWinLossCriteria
                {
                    AgentId = model.AgentId,
                    Sporttypes = model.Sporttypes,
                    Bettypes = model.Bettypes,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    BalanceUpDay = _winLossService.GetBalanceUpDay(),
                    MoveBetDay = _winLossService.MoveBetsDay()
                };

                agentWinLossList = _winLossService.GetAgentWinLoss(criteria);
                HttpContext.Session[sessionKey] = agentWinLossList;
            }

            model.AgentWinLossList = agentWinLossList;
            var excelResult = _winLossService.ExportAgentWinLoss(model);

            var fileName = string.Format(
                "AgentWinLoss_{0}_{1}_{2}",
                agentName,
                _winLossService.ShortDateFormat(model.FromDate),
                _winLossService.ShortDateFormat(model.ToDate));
            return new ExcelResult(excelResult, fileName);
        }

        /// <summary>
        /// Export excel member win loss report.
        /// </summary>
        /// <param name="model">Input MemberWinLossViewModel.</param>
        /// <returns>Excel file member win loss.</returns>
        public ActionResult ExportMemberWinLoss(MemberWinLossViewModel model)
        {
            var sessionKey = string.Format(Constants.SessionKeyMemberWinLossReport, this.CurrentSession.UserId);
            var memberWinLossList = (List<MemberWinLossModel>)HttpContext.Session[sessionKey];

            model.SportList = SingbetSportService.GetSport(isOther: false);
            model.BettypeList = _winLossService.GetBettype();

            if (memberWinLossList == null)
            {
                var criteria = new MemberWinLossCriteria
                {
                    AgentId = model.AgentId,
                    Sporttypes = model.Sporttypes,
                    Bettypes = model.Bettypes,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    BalanceUpDay = _winLossService.GetBalanceUpDay(),
                    MoveBetDay = _winLossService.MoveBetsDay()
                };

                memberWinLossList = _winLossService.GetMemberWinLoss(criteria);
                HttpContext.Session[sessionKey] = memberWinLossList;
            }

            model.MemberWinLossList = memberWinLossList;
            var excelResult = _winLossService.ExportMemberWinLoss(model);

            var fileName = string.Format(
                "MemberWinLoss_{0}_{1}_{2}",
                model.AgentName,
                _winLossService.ShortDateFormat(model.FromDate),
                _winLossService.ShortDateFormat(model.ToDate));
            return new ExcelResult(excelResult, fileName);
        }

        #endregion Agent Win Loss Report

        public ActionResult SportCredit(string username)
        {
            var criteria = new SportCreditCriteria
            {
                LoggedId = this.CurrentSession.AgentId,
                Username = username
            };

            var sportCreditList = _sportCreditService.GetSportCredits(criteria);

            var model = new SportCreditViewModel
            {
                Username = username,
                SportCreditModelList = sportCreditList
            };

            var sessionKey = string.Format(Constants.SessionKeyLineSportCreditReport, this.CurrentSession.UserId);
            HttpContext.Session[sessionKey] = sportCreditList;

            return View(model);
        }

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

                sportCreditList = _sportCreditService.GetSportCredits(criteria);
                HttpContext.Session[sessionKey] = sportCreditList;
            }


            var model = new SportCreditViewModel
            {
                Username = username,
                SportCreditModelList = sportCreditList
            };

            var excelResult = _sportCreditService.ExportSportCredit(model);

            var fileName = string.Format("LineSportCredit_{0}_{1}", model.Username, _sportCreditService.ShortDateFormat(DateTime.Now));

            return new ExcelResult(excelResult, fileName);
        }
    }
}