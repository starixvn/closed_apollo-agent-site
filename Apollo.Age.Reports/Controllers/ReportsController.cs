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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReportsController : Controller
    {
        private readonly OutStandingService _reportService;

        public ReportsController() : this(null) { }

        public ReportsController(OutStandingService reportService = null)
        {
            _reportService = reportService ?? new OutStandingService();
        }

        public ViewResult AgentOutstanding()
        {
            AgentOutstandingCriteria criteria = new AgentOutstandingCriteria();
            criteria.AgentId = 3;
            // call service to get report data here
            IEnumerable<OutstandingViewModel> models = _reportService.GetReportData<OutstandingViewModel>(criteria);

            // Add total row
            OutstandingViewModel totalModel = _reportService.Total(models);
            models.ToList().Add(totalModel);

            return View(models);
        }
    }
}