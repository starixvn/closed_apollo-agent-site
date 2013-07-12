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
            model.AgentId = model.AgentId == 0 ? this.CurrentSession.AgentId : model.AgentId;
            var agentName = string.IsNullOrWhiteSpace(model.AgentName) ? this.CurrentSession.AgentName : model.AgentName;
            var isSuper = model.AgentId == this.CurrentSession.AgentId ? this.CurrentSession.IsSuper : false;

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
            
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            MemoryStream output = new MemoryStream();
            _exportsService.ExportsAgentWinLoss(hssfworkbook, agentWinLossList, model.AgentName, model.SportName, model.BettypeName, model.FromDate, model.ToDate, isSuper);

            hssfworkbook.Write(output);

            return File(output.ToArray(), "application/vnd.ms-excel", "AgentWinLoss.xls");
        }

        public ActionResult ExportsMemberWinLoss(MemberWinLossViewModel model)
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

            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            MemoryStream output = new MemoryStream();
            _exportsService.ExportsMemberWinLoss(hssfworkbook, memberWinLossList, model.AgentName, model.SportName, model.BettypeName, model.FromDate, model.ToDate);

            hssfworkbook.Write(output);

            return File(output.ToArray(), "application/vnd.ms-excel", "MemberWinLoss.xls");
        }
    }
}
