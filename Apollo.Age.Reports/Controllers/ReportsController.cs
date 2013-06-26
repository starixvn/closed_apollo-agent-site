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
// </history>

namespace Apollo.Age.Reports.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Web;
    using Apollo.Age.Services.Impl;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ReportsController : BaseController
    {
        private readonly OutstandingService _reportService;
        private readonly IWinLossService _winLossService;
        private int _currentAgentId;

        public ReportsController() : this(null) 
        { 
        }

        public ReportsController(OutstandingService reportService = null)
        {
            _reportService = reportService ?? new OutstandingService();
            _winLossService = _winLossService ?? new WinLossService();
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

        public ViewResult Outstanding(int? agentId)
        {
            CurrentAgentId = agentId;
            var mainShareHolder = new Apollo.Age.Entities.Agent();

            IEnumerable<Agent> modelAgentDownLine = AgentDownlinePath(CurrentAgentId.Value);

            if (CurrentSession.IsSuper)
            {
                if (modelAgentDownLine.Count() > 1)
                {
                    mainShareHolder = modelAgentDownLine.ToList()[1];
                }
            }

            IEnumerable<OutstandingModel> models = _reportService.GetData<OutstandingModel>(new OutstandingCriteria
            {
                AgentId = CurrentAgentId.Value
            });

            return View(new OutstandingViewModel()
            {
                CurrentAgentId = CurrentAgentId.Value,
                ListOutStanding = models,
                DownlinePath = modelAgentDownLine,
                AgentLevel = modelAgentDownLine.LastOrDefault().Level,
                MainShareHolder = mainShareHolder
            });
        }

        [HttpPost]
        public ActionResult MemberOutstanding(int? agentId, int mainShareHolderId, string mainShareHolderUsername)
        {
            CurrentAgentId = agentId;
            //// call service to get report data here
            IEnumerable<OutstandingModel> models = _reportService.GetData<OutstandingModel>(new MemberOutstandingCriteria
            {
                AgentId = CurrentAgentId.Value
            });

            //// Sum total amount
            OutstandingModel totalModels = new OutstandingModel();
            if (models != null && models.Any())
            {
                foreach (OutstandingModel item in models)
                {
                    totalModels.Outstanding += item.Outstanding;
                    totalModels.ShareAmount += item.ShareAmount;
                    totalModels.GameOutstanding += item.GameOutstanding;
                    totalModels.GameShareAmount += item.GameShareAmount;
                }
            }
            //// return object
            return Json(new
            {
                ListOutstanding = models,
                Total = totalModels,
                MainShareHolderId = mainShareHolderId,
                MainShareHolderUsername = mainShareHolderUsername
            });
        }

        #endregion Outstanding Report

        #region Agent Win Loss Report

        public ViewResult MatchEntered()
        {
            var matchEnteredViewModel = new MatchEnteredViewModel();
            matchEnteredViewModel.BalanceUpDay = _reportService.GetBalanceUpDay();
            matchEnteredViewModel.SportList = SingbetSportService.GetSport(false);
            matchEnteredViewModel.BettypeList = SingbetSportService.GetBettype();
            matchEnteredViewModel.matchEnteredModelList = _winLossService.GetMatchEntered();

            return View(matchEnteredViewModel);
        }

        public ViewResult AgentWinLoss(int? id, string name, DateTime fromDate, DateTime toDate)
        {
            var agentId = id.HasValue ? id.Value : this.CurrentSession.AgentId;
            var agentName = string.IsNullOrWhiteSpace(name) ? this.CurrentSession.AgentName : name;
            var isSuper = agentId == this.CurrentSession.AgentId ? this.CurrentSession.IsSuper : false;

            var agentWinLossList = _winLossService.GetAgentWinLoss(agentId, fromDate, toDate);
            var modelAgentDownLine = AgentDownlinePath(agentId).ToList();

            var agentWinLossViewModel = new AgentWinLossViewModel()
            {
                IsSuper = isSuper,
                AgentId = agentId,
                AgentName = agentName,
                FromDate = fromDate,
                ToDate = toDate,
                AgentWinLossList = agentWinLossList,
                DownlinePath = modelAgentDownLine
            };

            return View(agentWinLossViewModel);
        }

        public ViewResult MemberWinLoss(int agentId, string agentName, DateTime fromDate, DateTime toDate)
        {
            var memberWinLossList = _winLossService.GetMemberWinLoss(agentId, fromDate, toDate);
            var modelAgentDownLine = AgentDownlinePath(agentId).ToList();

            var memberWinLossViewModel = new MemberWinLossViewModel()
            {
                AgentName = agentName,
                FromDate = fromDate,
                ToDate = toDate,
                MemberWinLossList = memberWinLossList,
                DownlinePath = modelAgentDownLine
            };

            return View(memberWinLossViewModel);
        }

        #endregion Agent Win Loss Report

        private IEnumerable<Agent> AgentDownlinePath(int agentId)
        {
            IEnumerable<Agent> models = _reportService.GetData<Agent>(new AgentDownlinePathCriteria
            {
                ParentId = CurrentSession.AgentId,
                AgentId = agentId
            });

            return models;
        }
    }
}