// --------------------------------------------------------------------------
// <copyright file="BetListController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">Created and Implement</change>
// </history>

namespace Apollo.Age.BetList.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Services;
    using Apollo.Age.BetList.ViewModels;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Web;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// BetList Controller
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BetListController : BaseController
    {
        private readonly BetListService betListService;
        private readonly ReportService reportService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BetListController" /> class.
        /// </summary>
        public BetListController()
        {
            this.betListService = new BetListService();
            this.reportService = new ReportService();
        }

        public ViewResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// View detail popup Agent Share and Agent commission
        /// </summary>
        /// <param name="transId">Trans Id</param>
        /// <param name="fromDate">From date</param>
        /// <param name="toDate">to date</param>
        /// <param name="type">page type</param>
        /// <returns>Return view</returns>
        public ViewResult ViewDetail(long transId, string fromDate, string toDate, string type)
        {
            IList<ITicket> listAgentComm;
            var betListService = new BetListService();
            var balanceUpDay = this.reportService.GetBalanceUpDay();
            var moveBetsDay = this.reportService.MoveBetsDay();
            var viewModel = new BetListViewModel();

            var criteria = new BetListCriteria
            {
                LoggedId = this.CurrentSession.AgentId,
                TransId = transId,
                FromDate = Formater.ResetStartDate(string.IsNullOrEmpty(fromDate) ? moveBetsDay : Convert.ToDateTime(fromDate)),
                ToDate = Formater.ResetEndDate(string.IsNullOrEmpty(toDate) ? DateTime.Now : Convert.ToDateTime(toDate)),
                BalanceUpDay = balanceUpDay,
                MoveBetsDay = moveBetsDay
            };
            if (criteria.IsValid())
            {
                if (type.ToLower().Equals("report"))
                {
                    listAgentComm = betListService.GetDetailWinLossAgentComm(criteria);
                }
                else
                {
                    listAgentComm = betListService.GetDetailAgentComm(criteria);
                }

                viewModel.ListAgentComm = this.Ticket(listAgentComm);
            }

            return this.View(viewModel);
        }

        private List<TicketText> Ticket(IList<ITicket> tickets)
        {
            var index = 0;
            var builder = new BetListHTMLBuilder();
            var ticketTextList = new List<TicketText>();
            foreach (var ticket in tickets)
            {
                var ticketText = new TicketText
                {
                    UserName = ticket.UserName,
                    TransTime = builder.BuildTrans(ticket),
                    AgentComm = builder.BuildAgentComm(ticket, true),
                    AgentShare = builder.BuildAgentShare(ticket, null)
                };
                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }
    }
}