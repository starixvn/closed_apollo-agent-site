// --------------------------------------------------------------------------
// <copyright file="LogController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.5.16">Created</change>
// </history>

namespace Apollo.Age.Account.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Account.Services;
    using Apollo.Age.Web;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class LogController : BaseController
    {
        private ILogRunSpService _logService;

        private IAccountService _accountService;

        public LogController()
            : this(null, null)
        {
        }

        public LogController(ILogRunSpService logService, IAccountService accountService)
        {
            _logService = logService ?? new LogRunSpService();
            _accountService = accountService ?? new AccountService();
        }

        public ViewResult ViewLog(
            bool isAgent = true,
            string username = "",
            int actionTypeId = 0,
            DateTime? from = null,
            DateTime? to = null,
            bool showOldValue = true,
            int pageSize = 50,
            int pageIndex = 1)
        {
            IEnumerable<LogRunSpModel> list;
            List<LogRunSpModel> results = new List<LogRunSpModel>();

            var criteria = new GetLogCustSettingCriteria
            {
                ActionTypeId = actionTypeId,
                Username = username,
                ActorAgentId = this.CurrentSession.AgentId,
                From = from == null ? DateTime.Now : (DateTime)from,
                To = to == null ? DateTime.Now : (DateTime)to,
                PageIndex = pageIndex,
                PageSize = pageSize,
                IsAgent = isAgent,
                ShowOldValue = showOldValue
            };

            criteria.ActorAgentId = this.CurrentSession.AgentId;

            list = _logService.Search(ref criteria);
            results = list != null ? list.ToList() : new List<LogRunSpModel>();

            if (!ReferenceEquals(results, null) && results.Any())
            {
                if (criteria.IsAgent)
                {
                    var agentInfo = _accountService.GetAgentInfo(new GetAgentInfoCriteria
                    {
                        ActorAgentId = this.CurrentSession.AgentId,
                        ExcludeActor = false,
                        Username = criteria.Username
                    });

                    ViewBag.AgentLevel = agentInfo.Level;
                }
            }

            ViewBag.LogData = results;

            return View(criteria);
        }
    }
}