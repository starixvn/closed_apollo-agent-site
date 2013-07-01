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
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Services;
    using Apollo.Age.BetList.ViewModels;
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Resources;
    using Apollo.Age.TotalBets.Services;
    using Apollo.Age.Web;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;

    /// <summary>
    /// BetList Controller
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BetListController : BaseController
    {
        private const string _StringEmpty = "";
        private readonly BetListService _betListService;
        private readonly ReportService _reportService;
        private readonly TotalBetsService _totalBetsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BetListController" /> class.
        /// </summary>
        public BetListController()
        {
            this._betListService = new BetListService();
            this._reportService = new ReportService();
            this._totalBetsService = new TotalBetsService();
        }

        public ViewResult Index()
        {
            return this.View();
        }

        public ViewResult Outstanding(int custId, string username = _StringEmpty, int mainShareHolderId = 0, string mainShareHolderUsername = _StringEmpty)
        {
            var statusIds = string.Format("{0},{1}",
                   (int)Apollo.Age.Enumerations.BetStatus.Running,
                   (int)Apollo.Age.Enumerations.BetStatus.Waiting);

            var moveBetsDay = _reportService.MoveBetsDay().AddDays(1);

            // List ticket
            var betListService = new BetListService();
            var criteria = new BetListCriteria
            {
                AgentId = -1,
                LoggedId = CurrentSession.AgentId,
                CustId = custId,
                FromDate = ResetStartDate(moveBetsDay),
                ToDate = ResetEndDate(DateTime.Now),
                StatusIds = statusIds
            };

            var mainTickets = _betListService.GetAllBetsByDateAndMem(criteria);
            var modelAgentDownLine = betListService.GetAgentUpLineOfMember(CurrentSession.AgentId, custId);

            var viewModel = new BetListViewModel
            {
                DownlinePath = modelAgentDownLine,
                TicketList = this.Ticket(mainTickets, BetListPage.Outstanding),
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// Betlist winloss report
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="type"></param>
        /// <param name="username"></param>
        /// <param name="betType"></param>
        /// <param name="mainShareHolderId"></param>
        /// <param name="mainShareHolderUsername"></param>
        /// <returns></returns>
        public ViewResult Winloss(int custId, DateTime fromDate, DateTime toDate, string type = _StringEmpty, string username = _StringEmpty, string betType = _StringEmpty, int mainShareHolderId = 0, string mainShareHolderUsername = _StringEmpty)
        {
            var statusIds = string.Empty;
            if (type.Equals("report", StringComparison.OrdinalIgnoreCase))
            {
                statusIds = string.Format("{0},{1},{2},{3},{4},{5}",
                      (int)Apollo.Age.Enumerations.BetStatus.Void,
                      (int)Apollo.Age.Enumerations.BetStatus.Reject,
                      (int)Apollo.Age.Enumerations.BetStatus.Refund,
                      (int)Apollo.Age.Enumerations.BetStatus.Draw,
                      (int)Apollo.Age.Enumerations.BetStatus.Won,
                      (int)Apollo.Age.Enumerations.BetStatus.Lose);
            }
            else
            {
                statusIds = string.Format("{0},{1}",
                         (int)Apollo.Age.Enumerations.BetStatus.Running,
                         (int)Apollo.Age.Enumerations.BetStatus.Waiting);
            }

            var balanceUpDay = _reportService.GetBalanceUpDay();
            var moveBetsDay = _reportService.MoveBetsDay().AddDays(1);
            var betListService = new BetListService();
            var criteria = new BetListCriteria
            {
                AgentId = -1,
                LoggedId = CurrentSession.AgentId,
                CustId = custId,
                FromDate = this.ResetStartDate(fromDate),
                ToDate = this.ResetEndDate(toDate),
                StatusIds = statusIds,
                Bettypes = betType,
                BalanceUpDay = balanceUpDay,
                MoveBetsDay = moveBetsDay
            };

            TicketResult mainTickets;
            if (type.Equals("report", StringComparison.OrdinalIgnoreCase))
            {
                mainTickets = betListService.GetWinLossByDateAndMem(criteria);
            }
            else
            {
                mainTickets = betListService.GetAllBetsByDateAndMem(criteria);
            }

            var modelAgentDownLine = betListService.GetAgentUpLineOfMember(CurrentSession.AgentId, custId);

            var viewModel = new BetListViewModel
            {
                TicketList = this.Ticket(mainTickets, BetListPage.Report),
                DownlinePath = modelAgentDownLine
            };
            return this.View(viewModel);
        }

        /// <summary>
        /// Action view betList from forecast
        /// </summary>
        /// <param name="type"></param>
        /// <param name="betType"></param>
        /// <param name="betTeam"></param>
        /// <param name="matchId"></param>
        /// <param name="mainShareHolderId"></param>
        /// <param name="mainShareHolderUsername"></param>
        /// <param name="isForecast"></param>
        /// <returns></returns>
        public ViewResult Forecast(string type = _StringEmpty, string betType = _StringEmpty, string betTeam = _StringEmpty, int matchId = 0, int mainShareHolderId = 0, string mainShareHolderUsername = _StringEmpty, bool isForecast = false)
        {
            var strBetType = string.Empty;
            var title = string.Empty;
            if (type.Equals("correctscore", StringComparison.OrdinalIgnoreCase))
            {
                title = CacheRef.GetBettype((int)BetTypes.Correct_Score);
                strBetType = string.Format("{0},{1}", (int)BetTypes.Correct_Score, (int)BetTypes.FirstCorrectScore);
            }
            else if (type.Equals("mixparlay", StringComparison.OrdinalIgnoreCase))
            {
                strBetType = Convert.ToString((int)BetTypes.MixParlay);
                title = CacheRef.GetBettype(betType);
            }
            else if (type.Equals("combmixparlaybetlist", StringComparison.OrdinalIgnoreCase))
            {
                strBetType = Convert.ToString((int)BetTypes.Combine_Mix_Parlay);
                title = Lang.lblSystemParlay;
            }
            else
            {
                strBetType = betType;
                title = CacheRef.GetBettype(betType);
            }

            // List mainShare Holder
            IEnumerable<Agent> mainShareHolders = new List<Agent>();
            if (!isForecast)
            {
                mainShareHolders = this.ListMainShareHolder(strBetType, ref mainShareHolderId);
            }
            // List ticket
            var betListService = new BetListService();
            var criteria = new BetListCriteria
            {
                LoggedId = CurrentSession.AgentId,
                AgentId = mainShareHolderId,
                Bettypes = strBetType,
                MatchId = matchId,
                BetTeam = betTeam
            };

            TicketResult mainTickets;

            if (matchId > 0)
            {
                mainTickets = betListService.GetRunningByBettypeAndMatch(criteria);
            }
            else
            {
                mainTickets = betListService.GetRunningByBettype(criteria);
            }

            var viewModel = new BetListViewModel
            {
                Title = title,
                CurrentMainShareHolderId = mainShareHolderId,
                ListMainShareHolders = mainShareHolders,
                TicketList = this.Ticket(mainTickets, BetListPage.Forecast),
            };

            return this.View(viewModel);
        }

        public ViewResult MixParlay(long transid)
        {
            var betListService = new BetListService();
            var criteria = new BetListCriteria
            {
                TransId = transid,
            };

            var viewModel = new MixParlayViewModel
           {
               TicketList = betListService.GetMixParlayDetailTicket(criteria)
           };

            return this.View(viewModel);
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
            var balanceUpDay = this._reportService.GetBalanceUpDay();
            var moveBetsDay = this._reportService.MoveBetsDay();
            var viewModel = new BetListViewModel();

            var criteria = new BetListCriteria
            {
                LoggedId = this.CurrentSession.AgentId,
                TransId = transId,
                FromDate = ResetStartDate(string.IsNullOrEmpty(fromDate) ? moveBetsDay : Convert.ToDateTime(fromDate)),
                ToDate = ResetEndDate(string.IsNullOrEmpty(toDate) ? DateTime.Now : Convert.ToDateTime(toDate)),
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

                viewModel.TicketList = this.Ticket(listAgentComm);
            }

            return this.View(viewModel);
        }

        /// <summary>
        /// Get ticket for betList
        /// </summary>
        /// <param name="ticketResult"></param>
        /// <param name="betlistPage"></param>
        /// <returns></returns>
        private List<TicketText> Ticket(TicketResult ticketResult, BetListPage betlistPage)
        {
            GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;
            var ticketHelper = new TicketHelper();
            var builder = new BetListHTMLBuilder();
            var ticketTextList = new List<TicketText>();
            var tickets = ticketResult.TicketList;
            foreach (var ticket in tickets)
            {
                var ticketText = new TicketText
                {
                    TransId = ticket.TransId,
                    UserName = ticket.UserName,
                    TransTime = builder.BuildTime(ticket, betlistPage),
                    Discount = builder.BuildDiscount(ticket, betlistPage),
                    GameType = builder.BuildTrans(ticket, ticketHelper, funcGetOddsTypeName, betlistPage),
                    Choice = builder.BuildChoice(ticket, ticketHelper, ticketResult.TicketDataList, false),
                    Stake = ticket.Stake,
                    Result = ticket.Winlost + ticket.PlayerComm,
                    Status = builder.BuildStatus(ticket, ticketResult.TicketDataList),
                    AgentWinlossValue = ticket.AWinlost,
                    AgentCommValue = ticket.AComm,
                    AgentComm = builder.BuildAgentComm(ticket),
                    AgentShare = builder.BuildAgentShare(ticket, null)
                };
                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }

        /// <summary>
        /// Get ticket for view detail
        /// </summary>
        /// <param name="tickets"></param>
        /// <returns></returns>
        private List<TicketText> Ticket(IList<ITicket> tickets)
        {
            var builder = new BetListHTMLBuilder();
            var ticketTextList = new List<TicketText>();
            foreach (var ticket in tickets)
            {
                var ticketText = new TicketText
                {
                    UserName = ticket.UserName,
                    AgentComm = builder.BuildAgentComm(ticket),
                    AgentShare = builder.BuildAgentShare(ticket, null)
                };
                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }

        #region Format Start, End date

        /// <summary>
        /// Get total bet data by betType, if MainShareHolderId > 0 and Current is Supper
        /// then load Data of that MainShareHolder
        /// </summary>
        /// <param name="betType"></param>
        /// <param name="mainShareHolderId"></param>
        /// <returns></returns>
        private IEnumerable<Agent> ListMainShareHolder(string betType, ref int mainShareHolderId)
        {
            int agentId = this.CurrentSession.AgentId;
            if (this.CurrentSession.IsSuper)
            {
                var mainShareHolders = this._totalBetsService.GetData<Agent>(new GetAllMainShareHolderCriteria { Bettypes = betType });
                if (mainShareHolderId == 0 && mainShareHolders != null && mainShareHolders.Any())
                {
                    mainShareHolderId = mainShareHolders.ElementAt(0).Id;
                }

                return mainShareHolders;
            }

            return new List<Agent>();
        }

        private DateTime ResetStartDate(DateTime dt)
        {
            return dt.AddMilliseconds(-(((dt.Hour * 60 + dt.Minute) * 60 + dt.Second) * 1000 + dt.Millisecond));
        }

        private DateTime ResetEndDate(DateTime dt)
        {
            return ResetStartDate(dt).Add(new TimeSpan(23, 59, 59));
        }

        #endregion Format Start, End date
    }
}