namespace Apollo.Age.CreditBalance.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Apollo.Age.CreditBalance.Core;
    using Apollo.Age.CreditBalance.Criterias;
    using Apollo.Age.CreditBalance.Entities;
    using Apollo.Age.CreditBalance.Services;
    using Apollo.Age.CreditBalance.ViewModels;
    using Apollo.Age.Web;

    /// <summary>
    /// CreditBalanceController class.
    /// </summary>
    public class CreditBalanceController : BaseController
    {
        private readonly ICreditBalanceService _creditBalanceService;

        public CreditBalanceController()
            : this(null)
        {
        }

        public CreditBalanceController(ICreditBalanceService creditBalanceService)
        {
            _creditBalanceService = creditBalanceService ?? new CreditBalanceService();
        }

        [HttpGet]
        public ActionResult DepositAgent(int agentId)
        {
            var model = new CreditBalanceViewModel { CustomerId = agentId };
            var criteria = new AgentGetAgentRangeInfoCriteria { AgentId = CurrentSession.AgentId };
            model.MaxAmount = _creditBalanceService.AgentGetAgentInfo(criteria).SportCredit;

            return View(model);
        }

        [HttpPost]
        public ActionResult DepositAgent(AgentTransferAgentCriteria agent)
        {
            agent.AdminId = CurrentSession.AdminId;
            agent.LoggedId = CurrentSession.AgentId;
            agent.IP = GetClientIp;
            agent.TransType = Constants.Deposit;

            return Json(_creditBalanceService.AgentTransferAgent(agent));
        }

        [HttpGet]
        public ActionResult DrawAgent(int agentId)
        {
            var model = new CreditBalanceViewModel { CustomerId = agentId };
            var criteria = new AgentGetAgentRangeInfoCriteria { AgentId = CurrentSession.AgentId };
            model.MaxAmount = _creditBalanceService.AgentGetAgentInfo(criteria).SportCredit;

            return View(model);
        }

        [HttpPost]
        public ActionResult DrawAgent(AgentTransferAgentCriteria agent)
        {
            agent.AdminId = CurrentSession.AdminId;
            agent.LoggedId = CurrentSession.AgentId;
            agent.IP = GetClientIp;
            agent.TransType = Constants.Draw;

            return Json(_creditBalanceService.AgentTransferAgent(agent));
        }

        [HttpGet]
        public ActionResult DepositMember(int custId)
        {
            var model = new CreditBalanceViewModel { CustomerId = custId };
            var criteria = new AgentGetAgentRangeInfoCriteria { AgentId = CurrentSession.AgentId };
            model.MaxAmount = _creditBalanceService.AgentGetAgentInfo(criteria).SportCredit;

            return View(model);
        }

        [HttpPost]
        public ActionResult DepositMember(AgentTransferMemberCriteria member)
        {
            member.AdminId = CurrentSession.AdminId;
            member.LoggedId = CurrentSession.AgentId;
            member.IP = GetClientIp;
            member.TransType = Constants.Deposit;

            return Json(_creditBalanceService.AgentTransferMember(member));
        }

        [HttpGet]
        public ActionResult DrawMember(int custId)
        {
            var model = new CreditBalanceViewModel { CustomerId = custId };
            var criteria = new AgentGetAgentRangeInfoCriteria { AgentId = CurrentSession.AgentId };
            model.MaxAmount = _creditBalanceService.AgentGetAgentInfo(criteria).SportCredit;

            return View(model);
        }

        [HttpPost]
        public ActionResult DrawMember(AgentTransferMemberCriteria member)
        {
            member.AdminId = CurrentSession.AdminId;
            member.LoggedId = CurrentSession.AgentId;
            member.IP = GetClientIp;
            member.TransType = Constants.Draw;

            return Json(_creditBalanceService.AgentTransferMember(member));
        }
    }
}