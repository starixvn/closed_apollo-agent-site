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
        public ActionResult DepositAgent(CreditBalanceViewModel agent)
        {
            var model = new CreditBalanceViewModel { CustomerId = agent.CustomerId, Username = agent.Username, BackLink = agent.BackLink };
            var parentId = agent.BackLink.Split('&').LastOrDefault().Split('=').LastOrDefault();
            var criteria = new AgentGetAgentRangeInfoCriteria { AgentId = Convert.ToInt16(parentId) };
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
        public ActionResult DrawAgent(int agentId, string username, string backLink)
        {
            var model = new CreditBalanceViewModel { CustomerId = agentId, Username = username, BackLink = backLink };
            var criteria = new AgentGetAgentRangeInfoCriteria { AgentId = agentId };
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
        public ActionResult DepositMember(int custId, string username, string backLink)
        {
            var model = new CreditBalanceViewModel { CustomerId = custId, Username = username, BackLink = backLink };
            var parentId = backLink.Split('&').LastOrDefault().Split('=').LastOrDefault();
            var criteria = new AgentGetAgentRangeInfoCriteria { AgentId = Convert.ToInt16(parentId) };
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
        public ActionResult DrawMember(int custId, string username, string backLink)
        {
            var model = new CreditBalanceViewModel { CustomerId = custId, Username = username, BackLink = backLink };
            var criteria = new AgentGetMemberRangeInfoCriteria { CustId = custId };
            model.MaxAmount = _creditBalanceService.AgentGetMemberInfo(criteria).SportCredit;

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