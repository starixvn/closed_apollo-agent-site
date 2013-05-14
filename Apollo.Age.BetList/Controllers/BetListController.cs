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
    using Fanex.BetList.Age.BetListHTMLBuilderExt;
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
        /// View detail comm
        /// </summary>
        /// <param name="transId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        //public ViewResult ViewDetail(long transId, DateTime fromDate, DateTime toDate, string type)
        public ViewResult ViewDetail(long transId, string type)
        {
            IList<ITicket> listAgentComm;
            var betListService = new BetListService();

            var IsFromReportWinloss = type == "report" ? true : false;

            var criteria = new BetListCriteria
            {
                LoggedId = this.CurrentSession.AgentId,
                TransId = transId,
                //FromDate = Formater.ResetStartDate(fromDate),
                //ToDate = Formater.ResetEndDate(toDate),
                FromDate = reportService.MoveBetsDay(),
                ToDate = DateTime.Now,
                BalanceUpDay = reportService.GetBalanceUpDay(),
                MoveBetsDay = reportService.MoveBetsDay()
            };

            if (IsFromReportWinloss)
            {
                listAgentComm = betListService.GetDetailWinLossAgentComm(criteria);
            }
            else
            {
                listAgentComm = betListService.GetDetailAgentComm(criteria);
            }

            var viewModel = new BetListViewModel
            {
                ListAgentComm = listAgentComm
            };
            return this.View(viewModel);
        }

        private List<TicketText> Ticket(IList<ITicket> tickets)
        {
            var index = 0;
            var builder = new BetListHTMLBuilderExt();
            foreach (var ticket in tickets)
            {
                var ticketText = new TicketText
                {
                    Index = index++,
                    UserName = ticket.UserName,
                    AgentComm = ticket.AgentComm,
                    AgentShare = ticket.AgentPT
                    Status = builder.BuildStatus(ticket, this._ticketHelper, ticketResult.TicketDataList, false),
                    Choice = builder.BuildChoice(ticket, this._ticketHelper, ticketResult.TicketDataList, false),
                    Odds = builder.BuildOdds(ticket, ticketResult.TicketDataList, CacheRef.GetOddsTypeName),
                    Stake = builder.BuildStake(ticket),
                };

                ticketTextList.Add(ticketText);
            }
        }
    }
}