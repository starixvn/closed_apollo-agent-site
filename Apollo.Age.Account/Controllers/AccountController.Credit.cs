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
        /// <summary>
        /// Deposits the agent.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>Deposit Agent.</returns>
        [HttpGet]
        public ActionResult DepositAgent(DepositViewModel viewModel)
        {
            if (viewModel.AgentLevel <= Enumerations.AgentLevel.MainShareholder)
            {
                viewModel.MaxAmount = long.MaxValue;
            }
            else
            {
                var criteria = new AgentGetAgentRangeInfoCriteria { AgentId = viewModel.ParentAgentId };
                viewModel.MaxAmount = _accountService.AgentGetAgentInfo(criteria).SportCredit;
            }

            return View(viewModel);
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
        public ActionResult DrawAgent(DrawViewModel viewModel)
        {
            var criteria = new AgentGetAgentRangeInfoCriteria { AgentId = viewModel.AgentId };
            viewModel.MaxAmount = _accountService.AgentGetAgentInfo(criteria).SportCredit;

            return View(viewModel);
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
        public ActionResult DepositMember(DepositViewModel viewModel)
        {
            var criteria = new AgentGetAgentRangeInfoCriteria { AgentId = viewModel.AgentId };
            viewModel.MaxAmount = _accountService.AgentGetAgentInfo(criteria).SportCredit;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DepositMember(AgentTransferMemberCriteria criteria)
        {
            criteria.AdminId = CurrentSession.AdminId;
            criteria.LoggedId = CurrentSession.AgentId;
            criteria.IP = GetClientIp;
            criteria.TransType = Constants.Deposit;

            return Json(_accountService.AgentTransferMember(criteria));
        }

        [HttpGet]
        public ActionResult DrawMember(DrawViewModel viewModel)
        {
            var criteria = new AgentGetMemberRangeInfoCriteria { CustId = viewModel.CustId };
            viewModel.MaxAmount = _accountService.AgentGetMemberInfo(criteria).SportCredit;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DrawMember(AgentTransferMemberCriteria criteria)
        {
            criteria.AdminId = CurrentSession.AdminId;
            criteria.LoggedId = CurrentSession.AgentId;
            criteria.IP = GetClientIp;
            criteria.TransType = Constants.Draw;

            return Json(_accountService.AgentTransferMember(criteria));
        }
    }
}