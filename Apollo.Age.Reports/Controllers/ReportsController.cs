// --------------------------------------------------------------------------
// <copyright file="OutStandingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.29">Created</change>
//   <change who="Terry.Nguyen" date="2013.04.07">Implement</change>
//   <change who="Terry.Nguyen" date="2013.04.09">Get AgentID from BaseController</change>
// </history>

namespace Apollo.Age.Reports.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReportsController : BaseController
    {
        private readonly OutstandingService _reportService;
        private readonly IWinLossService _winLossService;
        private int _currentAgentId;

        public int? CurrentAgentId
        {
            set
            {
                _currentAgentId = (value.HasValue ? value.Value : this.CurrentSession.AgentId);
            }
            get
            {
                return _currentAgentId;
            }
        }

        public ReportsController() : this(null) { }

        public ReportsController(OutstandingService reportService = null)
        {
            _reportService = reportService ?? new OutstandingService();
            _winLossService = _winLossService ?? new WinLossService();
        }

        private IEnumerable<Agent> AgentDownlinePath(int agentId)
        {
            IEnumerable<Agent> models = _reportService.GetData<Agent>(new AgentDownlinePathCriteria
            {
                ParentId = CurrentSession.AgentId,
                AgentId = agentId
            });

            return models;
        }

        #region Outstanding Report

        public ViewResult Outstanding(int? agentId)
        {
            CurrentAgentId = agentId;
            IEnumerable<Agent> modelAgentDownLine = AgentDownlinePath(CurrentAgentId.Value);
            IEnumerable<OutstandingModel> models = _reportService.GetData<OutstandingModel>(new OutstandingCriteria
            {
                AgentId = CurrentAgentId.Value
            });

            return View(new OutstandingViewModel()
            {
                CurrentAgentId = CurrentAgentId.Value,
                ListOutStanding = models,
                DownlinePath = modelAgentDownLine,
                AgentLevel = modelAgentDownLine.LastOrDefault().Level
            });
        }

        [HttpPost]
        public ActionResult MemberOutstanding(int? agentId)
        {
            CurrentAgentId = agentId;
            // call service to get report data here
            IEnumerable<OutstandingModel> models = _reportService.GetData<OutstandingModel>(new MemberOutstandingCriteria
            {
                AgentId = CurrentAgentId.Value
            });

            // Sum total amount
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
            // return object
            return Json(new
            {
                ListOutstanding = models,
                Total = totalModels
            });
        }

        #endregion Outstanding Report

        #region Agent Win Loss Report

        public ViewResult MatchEntered()
        {
            var matchEnteredList = _winLossService.GetMatchEntered();

            return View(matchEnteredList);
        }

        public ViewResult WinLoss(DateTime fromDate, DateTime toDate)
        {
            var agentWinLossList = _winLossService.GetAgentWinLoss(this.CurrentSession.AgentId, fromDate, toDate);
            var modelAgentDownLine = AgentDownlinePath(this.CurrentSession.AgentId).ToList();

            var agentWinLossViewModel = new AgentWinLossViewModel()
            {
                IsSuper = this.CurrentSession.IsSuper,
                AgentId = this.CurrentSession.AgentId,
                AgentName = this.CurrentSession.AgentName,
                FromDate = fromDate,
                ToDate = toDate,
                AgentWinLossList = agentWinLossList,
                DownlinePath = modelAgentDownLine
            };

            return View("AgentWinLoss", agentWinLossViewModel);
        }

        public ViewResult AgentWinLoss(int agentId, string agentName, DateTime fromDate, DateTime toDate)
        {
            var agentWinLossList = _winLossService.GetAgentWinLoss(agentId, fromDate, toDate);
            var modelAgentDownLine = AgentDownlinePath(agentId).ToList();

            var agentWinLossViewModel = new AgentWinLossViewModel() {
                IsSuper = false,
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
    }
}