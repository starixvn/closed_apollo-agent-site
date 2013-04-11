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
    using Apollo.Age.Reports.DTO;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReportsController : BaseController
    {
        private readonly OutstandingService _reportService;
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

            IEnumerable<OutstandingDTO> models = _reportService.GetData<OutstandingDTO>(new OutstandingCriteria
            {
                AgentId = CurrentAgentId.Value
            });

            return View(new OutstandingModel()
            {
                CurrentAgentId = CurrentAgentId.Value,
                ListOutStanding = models,
                DownlinePath = AgentDownlinePath(CurrentAgentId.Value)
            });
        }

        [HttpPost]
        public ActionResult MemberOutstanding(int? agentId)
        {
            CurrentAgentId = agentId;
            // call service to get report data here
            IEnumerable<OutstandingDTO> models = _reportService.GetData<OutstandingDTO>(new MemberOutstandingCriteria
            {
                AgentId = CurrentAgentId.Value
            });

            // Sum total amount
            OutstandingDTO totalModels = new OutstandingDTO();
            if (models != null && models.Any())
            {
                foreach (OutstandingDTO item in models)
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

            return View();
        }

        #endregion
    }
}