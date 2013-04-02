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

        public ReportsController() : this(null) { }

        public ReportsController(OutstandingService reportService = null)
        {
            _reportService = reportService ?? new OutstandingService();
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

        public ViewResult Outstanding()
        {
            if (UserSession.Current.AgentLevel == 0)
            {
                return AgentOutstanding();
            }

            int agentId = string.IsNullOrEmpty(HttpContext.Request["agentId"]) ? UserSession.Current.AgentId : Convert.ToInt32(HttpContext.Request["agentId"]);
            // call service to get report data here
            IEnumerable<OutstandingViewModel> models = _reportService.GetReportData<OutstandingViewModel>(new OutstandingCriteria
            {
                AgentId = agentId
            });

            ViewBag.AgentDownlinePath = AgentDownlinePath(agentId);

            return View(models);
        }

        public ViewResult AgentOutstanding()
        {
            int agentId = string.IsNullOrEmpty(HttpContext.Request["agentId"]) ? UserSession.Current.AgentId : Convert.ToInt32(HttpContext.Request["agentId"]);
            // call service to get report data here
            IEnumerable<OutstandingViewModel> models = _reportService.GetReportData<OutstandingViewModel>(new AgentOutstandingCriteria
            {
                AgentId = agentId
            });

            return View(models);
        }

        [HttpPost]
        public JsonResult MemberOutstanding()
        {
            int agentId = string.IsNullOrEmpty(HttpContext.Request["agentId"]) ? UserSession.Current.AgentId : Convert.ToInt32(HttpContext.Request["agentId"]);
            // call service to get report data here
            IEnumerable<OutstandingViewModel> models = _reportService.GetReportData<OutstandingViewModel>(new MemberOutstandingCriteria
            {
                AgentId = agentId
            });

            return Json(models);
        }

        #endregion Outstanding Report
    }
}