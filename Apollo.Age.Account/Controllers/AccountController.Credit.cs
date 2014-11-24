namespace Apollo.Age.Account.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Apollo.Age.Account.Core;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Web;

    /// <summary>
    /// CreditBalanceController class.
    /// </summary>
    public partial class AccountController : BaseController
    {
        [HttpGet]
        public ActionResult DepositAgent(CreditBalanceViewModel viewModel)
        {
            var model = new CreditBalanceViewModel { CustomerId = viewModel.CustomerId, Username = viewModel.Username, BackLink = viewModel.BackLink };
            var parentId = viewModel.BackLink.Split('&').LastOrDefault().Split('=').LastOrDefault();
            var criteria = new AgentGetAgentRangeInfoCriteria { AgentId = Convert.ToInt16(parentId) };
            model.MaxAmount = _accountService.AgentGetAgentInfo(criteria).SportCredit;

            return View(model);
        }

        [HttpPost]
        public ActionResult DepositAgent(AgentTransferAgentCriteria agent)
        {
            agent.AdminId = CurrentSession.AdminId;
            agent.LoggedId = CurrentSession.AgentId;
            agent.IP = GetClientIp;
            agent.TransType = Constants.Deposit;

            return Json(_accountService.AgentTransferAgent(agent));
        }

        [HttpGet]
        public ActionResult DrawAgent(CreditBalanceViewModel viewModel)
        {
            var model = new CreditBalanceViewModel { CustomerId = viewModel.CustomerId, Username = viewModel.Username, BackLink = viewModel.BackLink };
            var criteria = new AgentGetAgentRangeInfoCriteria { AgentId = viewModel.CustomerId };
            model.MaxAmount = _accountService.AgentGetAgentInfo(criteria).SportCredit;

            return View(model);
        }

        [HttpPost]
        public ActionResult DrawAgent(AgentTransferAgentCriteria agent)
        {
            agent.AdminId = CurrentSession.AdminId;
            agent.LoggedId = CurrentSession.AgentId;
            agent.IP = GetClientIp;
            agent.TransType = Constants.Draw;

            return Json(_accountService.AgentTransferAgent(agent));
        }

        [HttpGet]
        public ActionResult DepositMember(int custId, string username, string backLink)
        {
            var model = new CreditBalanceViewModel { CustomerId = custId, Username = username, BackLink = backLink };
            var parentId = backLink.Split('&').LastOrDefault().Split('=').LastOrDefault();
            var criteria = new AgentGetAgentRangeInfoCriteria { AgentId = Convert.ToInt16(parentId) };
            model.MaxAmount = _accountService.AgentGetAgentInfo(criteria).SportCredit;

            return View(model);
        }

        [HttpPost]
        public ActionResult DepositMember(AgentTransferMemberCriteria member)
        {
            member.AdminId = CurrentSession.AdminId;
            member.LoggedId = CurrentSession.AgentId;
            member.IP = GetClientIp;
            member.TransType = Constants.Deposit;

            return Json(_accountService.AgentTransferMember(member));
        }

        [HttpGet]
        public ActionResult DrawMember(int custId, string username, string backLink)
        {
            var model = new CreditBalanceViewModel { CustomerId = custId, Username = username, BackLink = backLink };
            var criteria = new AgentGetMemberRangeInfoCriteria { CustId = custId };
            model.MaxAmount = _accountService.AgentGetMemberInfo(criteria).SportCredit;

            return View(model);
        }

        [HttpPost]
        public ActionResult DrawMember(AgentTransferMemberCriteria member)
        {
            member.AdminId = CurrentSession.AdminId;
            member.LoggedId = CurrentSession.AgentId;
            member.IP = GetClientIp;
            member.TransType = Constants.Draw;

            return Json(_accountService.AgentTransferMember(member));
        }
    }
}