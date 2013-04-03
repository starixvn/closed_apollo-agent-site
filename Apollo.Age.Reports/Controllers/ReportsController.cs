// --------------------------------------------------------------------------
// <copyright file="OutStandingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.29">Created</change>
// </history>

namespace Apollo.Age.Reports.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Reports.ViewModels;
    using Sunplus.Agent.Model.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReportsController : Controller
    {
        private readonly OutstandingService _reportService;

        private int currentAgentId;

        public ReportsController() : this(null) { }

        public ReportsController(OutstandingService reportService = null)
        {
            _reportService = reportService ?? new OutstandingService();
            currentAgentId = UserSession.Current.AgentId;
        }

        private IEnumerable<AgentDownlinePathViewModel> AgentDownlinePath(int agentId)
        {
            IEnumerable<AgentDownlinePathViewModel> models = _reportService.GetReportData<AgentDownlinePathViewModel>(new AgentDownlinePathCriteria
            {
                ParentId = UserSession.Current.AgentId,
                AgentId = agentId
            });

            return models;
        }

        #region Outstanding Report

        public ViewResult Outstanding(int? agentId)
        {
            currentAgentId = !agentId.HasValue ? currentAgentId : agentId.Value;
            if (UserSession.Current.AgentLevel == 0)
            {
                return AgentOutstanding(currentAgentId);
            }

            // call service to get report data here
            IEnumerable<OutstandingViewModel> models = _reportService.GetReportData<OutstandingViewModel>(new OutstandingCriteria
            {
                AgentId = currentAgentId
            });

            return View(new OutstandingDownlinePathViewModel()
            {
                CurrentAgentId = currentAgentId,
                ListOutStanding = models,
                DownlinePath = AgentDownlinePath(currentAgentId)
            });
        }

        public ViewResult AgentOutstanding(int? agentId)
        {
            currentAgentId = !agentId.HasValue ? currentAgentId : agentId.Value;
            // call service to get report data here
            IEnumerable<OutstandingViewModel> models = _reportService.GetReportData<OutstandingViewModel>(new AgentOutstandingCriteria
            {
                AgentId = currentAgentId
            });

            return View(models);
        }

        [HttpPost]
        public ActionResult MemberOutstanding(int? agentId)
        {
            currentAgentId = !agentId.HasValue ? currentAgentId : agentId.Value;
            // call service to get report data here
            IEnumerable<OutstandingViewModel> models = _reportService.GetReportData<OutstandingViewModel>(new MemberOutstandingCriteria
            {
                AgentId = currentAgentId
            });

            // Sum total amount
            OutstandingViewModel totalModels = new OutstandingViewModel();
            foreach (OutstandingViewModel item in models)
            {
                totalModels.Outstanding += item.Outstanding;
                totalModels.ShareAmount += item.ShareAmount;
                totalModels.GameOutstanding += item.GameOutstanding;
                totalModels.GameShareAmount += item.GameShareAmount;
            }

            // return object
            return Json(new
            {
                ListOutstanding = models,
                Total = totalModels
            });
        }

        #endregion Outstanding Report
    }
}