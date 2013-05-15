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
    using Apollo.Age.BetList.MetaModel;
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
        /// View detail comm
        /// </summary>
        /// <param name="metaModel"></param>
        /// <returns></returns>
        public ViewResult ViewDetail(BetListMetaModel metaModel)
        {
            IList<ITicket> listAgentComm;
            var betListService = new BetListService();
            var balanceUpDay = reportService.GetBalanceUpDay();
            var moveBetsDay = reportService.MoveBetsDay();

            if (metaModel.FromDate == DateTime.MinValue)
            {
                metaModel.FromDate = moveBetsDay;
            }
            if (metaModel.ToDate == DateTime.MinValue)
            {
                metaModel.ToDate = DateTime.Now;
            }

            var criteria = new BetListCriteria
            {
                LoggedId = this.CurrentSession.AgentId,
                TransId = metaModel.TransId,
                FromDate = metaModel.FromDate,
                ToDate = metaModel.ToDate,
                BalanceUpDay = balanceUpDay,
                MoveBetsDay = moveBetsDay
            };

            if (metaModel.Type == "report")
            {
                listAgentComm = betListService.GetDetailWinLossAgentComm(criteria);
            }
            else
            {
                listAgentComm = betListService.GetDetailAgentComm(criteria);
            }

            var viewModel = new BetListViewModel()
            {
                ListAgentComm = this.Ticket(listAgentComm)
            };
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