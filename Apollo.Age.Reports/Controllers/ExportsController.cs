// --------------------------------------------------------------------------
// <copyright file="ExportsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.7.12">Created</change>
// </history>

namespace Apollo.Age.Reports.Controllers
{
    using System.IO;
    using System.Web.Mvc;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Reports.ViewModels;
    using Apollo.Age.Web;
    using NPOI.HSSF.UserModel;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Services.Impl;
    using Fanex.NAP.Framework.Mvc;
    using Apollo.Age.Reports.Core;
    using System;
    using System.Web.SessionState;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ExportsController : BaseController
    {
        private readonly ExportsServices _exportsService;
        private readonly WinLossService _reportService;

        public ExportsController()
			: this(null)
		{
		}

        public ExportsController(ExportsServices exportsService = null, WinLossService reportService = null)
        {
            _exportsService = exportsService ?? new ExportsServices();
            _reportService = reportService ?? new WinLossService();
        }

        public ActionResult ExportsAgentWinLoss(AgentWinLossViewModel model)
        {
            //var sessionKey = string.Format(Constants.SessionWinLossReport, this.CurrentSession.AgentId);
            //var report = (AgentWinLossViewModel)HttpContext.Session[sessionKey];

            //if (report == null)
            {
                model.AgentId = model.AgentId == 0 ? this.CurrentSession.AgentId : model.AgentId;
                var agentName = string.IsNullOrWhiteSpace(model.AgentName) ? this.CurrentSession.AgentName : model.AgentName;
                model.IsSuper = model.AgentId == this.CurrentSession.AgentId ? this.CurrentSession.IsSuper : false;

                model.SportList = SingbetSportService.GetSport(isOther: false);
                model.BettypeList = _reportService.GetBettype();

                var criteria = new AgentWinLossCriteria
                {
                    AgentId = model.AgentId,
                    Sporttypes = model.Sporttypes,
                    Bettypes = model.Bettypes,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    BalanceUpDay = _reportService.GetBalanceUpDay(),
                    MoveBetDay = _reportService.MoveBetsDay()
                };

                var agentWinLossList = _reportService.GetAgentWinLoss(criteria);
                model.AgentWinLossList = agentWinLossList;

                //report = model;
                //HttpContext.Session[sessionKey] = model;
            }

            var excelResult = _exportsService.ExportsAgentWinLoss(model);

            return new ExcelResult(excelResult, "AgentWinLoss");
        }

        public ActionResult ExportsMemberWinLoss(MemberWinLossViewModel model)
        {
            //var sessionKey = string.Format(Constants.SessionWinLossReport, this.CurrentSession.AgentId);
            //model = (MemberWinLossViewModel)HttpContext.Session[sessionKey];

            //if (model == null)
            {
                model.SportList = SingbetSportService.GetSport(isOther: false);
                model.BettypeList = _reportService.GetBettype();

                var criteria = new MemberWinLossCriteria
                {
                    AgentId = model.AgentId,
                    Sporttypes = model.Sporttypes,
                    Bettypes = model.Bettypes,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    BalanceUpDay = _reportService.GetBalanceUpDay(),
                    MoveBetDay = _reportService.MoveBetsDay()
                };

                var memberWinLossList = _reportService.GetMemberWinLoss(criteria);
                model.MemberWinLossList = memberWinLossList;

                //HttpContext.Session[sessionKey] = model;
            }

            var excelResult = _exportsService.ExportsMemberWinLoss(model);

            return new ExcelResult(excelResult, "MemberWinLoss");
        }
    }
}
