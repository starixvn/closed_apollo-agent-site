// --------------------------------------------------------------------------
// <copyright file="BetListController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">Created and Implement</change>
//   <change who="Kaden.Trinh" date="2013.07.02">Implement cancel ticket</change>
//   <change who="Kaden.Trinh" date="2013.07.02">Implement Wagers</change>
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
    using Apollo.Age.Services.Impl;
    using Apollo.Age.TotalBets.Services;
    using Apollo.Age.Web;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Common;
    using Fanex.BetList.Core.Utils;

    /// <summary>
    /// BetList Controller
    /// </summary>
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

        /// <summary>
        /// Bet list of member
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        [HttpGet]
        public ViewResult AccountInfo(int custId)
        {
            var statusIds = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                    (int)Apollo.Age.Enumerations.BetStatus.Void,
                    (int)Apollo.Age.Enumerations.BetStatus.Reject,
                    (int)Apollo.Age.Enumerations.BetStatus.Running,
                    (int)Apollo.Age.Enumerations.BetStatus.Waiting,
                    (int)Apollo.Age.Enumerations.BetStatus.Refund,
                    (int)Apollo.Age.Enumerations.BetStatus.Draw,
                    (int)Apollo.Age.Enumerations.BetStatus.Won,
                    (int)Apollo.Age.Enumerations.BetStatus.Lose);

            var moveBetsDay = _reportService.MoveBetsDay().AddDays(1);

            // List ticket
            var betListService = new BetListService();
            var model = new BetListCriteria
            {
                AgentId = -1,
                LoggedId = CurrentSession.AgentId,
                CustId = custId,
                FromDate = ResetStartDate(moveBetsDay),
                ToDate = ResetEndDate(DateTime.Now),
                StatusIds = statusIds,
                BetListPage = BetListPage.AccountInfo,
            };

            var mainTickets = _betListService.GetTicketResult(model);

            var viewModel = new BetListReportViewModel
            {
                TicketList = this.Ticket(mainTickets, BetListPage.AccountInfo),
                FromDate = moveBetsDay,
                ToDate = DateTime.Now,
                BalanceUpDay = moveBetsDay
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// Bet list of member
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ViewResult AccountInfo(BetListCriteria model)
        {
            var statusIds = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                   (int)Apollo.Age.Enumerations.BetStatus.Void,
                   (int)Apollo.Age.Enumerations.BetStatus.Reject,
                   (int)Apollo.Age.Enumerations.BetStatus.Running,
                   (int)Apollo.Age.Enumerations.BetStatus.Waiting,
                   (int)Apollo.Age.Enumerations.BetStatus.Refund,
                   (int)Apollo.Age.Enumerations.BetStatus.Draw,
                   (int)Apollo.Age.Enumerations.BetStatus.Won,
                   (int)Apollo.Age.Enumerations.BetStatus.Lose);

            var moveBetsDay = _reportService.MoveBetsDay().AddDays(1);

            // List ticket
            var betListService = new BetListService();
            model.AgentId = -1;
            model.LoggedId = CurrentSession.AgentId;
            model.StatusIds = statusIds;
            model.BetListPage = BetListPage.AccountInfo;
            model.ToDate = ResetEndDate(model.ToDate);

            var mainTickets = _betListService.GetTicketResult(model);

            var viewModel = new BetListReportViewModel
            {
                TicketList = this.Ticket(mainTickets, BetListPage.AccountInfo),
                FromDate = moveBetsDay,
                ToDate = DateTime.Now,
                BalanceUpDay = moveBetsDay
            };

            return this.View(viewModel);
        }

        /// <summary>
        ///  Bet list Transaction
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Transactions()
        {
            var viewModel = new BetListReportViewModel();
            viewModel.Username = CurrentSession.AgentName;

            return this.Transactions(viewModel);
        }

        /// <summary>
        /// Bet list Transaction
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="sportTypes"></param>
        /// <param name="betTypes"></param>
        /// <param name="userName"></param>
        /// <param name="userType"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        public ViewResult TransactionsPaging(DateTime fromDate, DateTime toDate, string sportTypes = _StringEmpty,
            string betTypes = _StringEmpty, string userName = _StringEmpty,
            int userType = 0, int pageIndex = 0)
        {
            var viewModel = new BetListReportViewModel()
            {
                FromDate = fromDate,
                ToDate = toDate,
                SportTypes = sportTypes,
                BetTypes = betTypes,
                Username = userName,
                UserType = userType,
                PageIndex = pageIndex,
                FromPaging = true
            };

            return this.Transactions(viewModel);
        }

        /// <summary>
        /// Bet list Transaction
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ViewResult Transactions(BetListReportViewModel model)
        {
            var statusIds = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                     (int)Apollo.Age.Enumerations.BetStatus.Void,
                     (int)Apollo.Age.Enumerations.BetStatus.Reject,
                     (int)Apollo.Age.Enumerations.BetStatus.Running,
                     (int)Apollo.Age.Enumerations.BetStatus.Waiting,
                     (int)Apollo.Age.Enumerations.BetStatus.Refund,
                     (int)Apollo.Age.Enumerations.BetStatus.Draw,
                     (int)Apollo.Age.Enumerations.BetStatus.Won,
                     (int)Apollo.Age.Enumerations.BetStatus.Lose);
            var balanceUpDay = _reportService.GetBalanceUpDay();
            int agentId, custId;
            int errorCode;
            model.Username = !string.IsNullOrEmpty(model.Username) ? model.Username.Trim() : CurrentSession.AgentName;
            _betListService.GetUserIdByUserName(CurrentSession.AgentId, model.UserType == 1, model.Username, out agentId, out custId, out errorCode);

            model.SportList = SingbetSportService.GetSport(isOther: false);
            model.BettypeList = _reportService.GetBettype();
            model.BalanceUpDay = balanceUpDay;

            if (errorCode == 0)
            {
                var betListService = new BetListService();
                var criteria = new BetListCriteria
                {
                    AgentId = agentId,
                    LoggedId = CurrentSession.AgentId,
                    CustId = custId,
                    FromDate = this.ResetStartDate(model.FromDate),
                    ToDate = this.ResetEndDate(model.ToDate),
                    StatusIds = statusIds,
                    SportTypes = model.SportTypes,
                    BetTypes = model.BetTypes,
                    BetListPage = BetListPage.Transaction,
                    MoveBetsDay = _reportService.MoveBetsDay()
                };

                //// Cache Key
                var sessionKey = string.Format("TransactionsReport{0}-{1}-{2}", 
                    criteria.LoggedId,
                    criteria.AgentId,
                    criteria.CustId
                    );

                TicketResult mainTickets = null;
                var getFromDB = true;
                if (model.FromPaging == true)
                {
                    var resultFromSession = HttpContext.Session[sessionKey] as TicketResult;
                    if (resultFromSession != null)
                    {
                        mainTickets = resultFromSession.Clone();
                        getFromDB = false;
                    }
                }

                if (getFromDB)
                {
                    mainTickets = betListService.GetTicketResult(criteria);
                    HttpContext.Session[sessionKey] = mainTickets.Clone();
                }

                ViewBag.TotalItems = mainTickets.TicketList.Count();

                mainTickets.TicketList = mainTickets.TicketList.Skip(model.PageSize * model.PageIndex).Take(model.PageSize).ToList() as IList<ITicket>;

                // Set data to view model
                model.TicketList = this.Ticket(mainTickets, BetListPage.Transaction);
            }
            else
            {
                ViewBag.TotalItems = 0;
            }

            return this.View("~/Views/BetList/Transactions.cshtml", model);
        }

        /// <summary>
        /// Cancel ticket betlist
        /// </summary>
        [HttpGet]
        public ViewResult TicketCancel()
        {
            var viewModel = new BetListReportViewModel
                {
                    Username = CurrentSession.AgentName
                };

            return this.TicketCancel(viewModel);
        }

        /// <summary>
        /// Cancel ticket betlist
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ViewResult TicketCancel(BetListReportViewModel model)
        {
            var statusIds = string.Format("{0},{1}", (int)Apollo.Age.Enumerations.BetStatus.Void, (int)Apollo.Age.Enumerations.BetStatus.Reject);
            var balanceUpDay = _reportService.GetBalanceUpDay();
            int agentId, custId;
            int errorCode;

            model.Username = !string.IsNullOrEmpty(model.Username) ? model.Username.Trim() : CurrentSession.AgentName;
            _betListService.GetUserIdByUserName(CurrentSession.AgentId, model.UserType == 1, model.Username, out agentId, out custId, out errorCode);

            model.SportList = SingbetSportService.GetSport(isOther: false);
            model.BettypeList = _reportService.GetBettype();
            model.BalanceUpDay = balanceUpDay;

            if (errorCode == 0)
            {
                var betListService = new BetListService();
                var criteria = new BetListCriteria
                {
                    AgentId = agentId,
                    LoggedId = CurrentSession.AgentId,
                    CustId = custId,
                    FromDate = this.ResetStartDate(model.FromDate),
                    ToDate = this.ResetEndDate(model.ToDate),
                    StatusIds = statusIds,
                    SportTypes = model.SportTypes,
                    BetTypes = model.BetTypes,
                    BetListPage = BetListPage.Cancel
                };

                TicketResult mainTickets;
                mainTickets = betListService.GetTicketResult(criteria);

                // Set data to view model
                model.TicketList = this.Ticket(mainTickets, BetListPage.Cancel);
            }

            return this.View(model);
        }

        /// <summary>
        /// Bet list Outstanding listOutstanding
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="username"></param>
        /// <param name="mainShareHolderId"></param>
        /// <param name="mainShareHolderUsername"></param>
        /// <returns></returns>
        public ViewResult Outstanding(int custId,
            string username = _StringEmpty,
            int mainShareHolderId = 0,
            string mainShareHolderUsername = _StringEmpty,
            string betType = _StringEmpty,
            string sportType = _StringEmpty,
            string sportName = _StringEmpty,
            string betTypeName = _StringEmpty)
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
                BetTypes = betType,
                SportTypes = sportType,
                LoggedId = CurrentSession.AgentId,
                CustId = custId,
                FromDate = ResetStartDate(moveBetsDay),
                ToDate = ResetEndDate(DateTime.Now),
                StatusIds = statusIds,
                BetListPage = BetListPage.Outstanding
            };

            var mainTickets = _betListService.GetTicketResult(criteria);
            var modelAgentDownLine = betListService.GetAgentUpLineOfMember(CurrentSession.AgentId, custId);

            var viewModel = new BetListViewModel
            {
                Title = string.Format("{0}: {1} --{2}: {3}", Lang.lblReport, sportName, Lang.WagerType, betTypeName),
                DownlinePath = modelAgentDownLine,
                TicketList = this.Ticket(mainTickets, BetListPage.Outstanding),
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// Bet list winloss listOutstanding
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
        public ViewResult Winloss(int custId, 
            DateTime fromDate, 
            DateTime toDate, 
            string type = _StringEmpty, 
            string username = _StringEmpty, 
            int mainShareHolderId = 0, 
            string mainShareHolderUsername = _StringEmpty, 
            string betType = _StringEmpty, 
            string sportType = _StringEmpty,
            string sportName = _StringEmpty,
            string betTypeName = _StringEmpty)
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
                BetTypes = betType,
                SportTypes = sportType,
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
                mainTickets = betListService.GetTicketResult(criteria);
            }

            var modelAgentDownLine = betListService.GetAgentUpLineOfMember(CurrentSession.AgentId, custId);

            var viewModel = new BetListViewModel
            {
                Title = string.Format("--{0}: {1} --{2}: {3}",
                    Lang.lblReport,
                    sportName,
                    Lang.WagerType,
                    betTypeName
                ),
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
                title = CacheRef.GetBettype((int)BetTypes.MixParlay);
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
                BetTypes = strBetType,
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

        /// <summary>
        /// Action view betList from Wagers page
        /// </summary>
        /// <param name="loggedId"></param>
        /// <param name="fromDate"></param>
        /// <param name="UGroup"></param>
        /// <param name="isDirectMem"></param>
        /// <param name="liveIndicator"></param>
        /// <param name="leagueId"></param>
        /// <param name="betType"></param>
        /// <param name="sportType"></param>
        /// <param name="betTeam"></param>
        /// <returns></returns>
        public ViewResult Wagers(int loggedId, DateTime fromDate, DateTime? toDate, string uGroup = _StringEmpty, bool isDirectMem = false, bool liveIndicator = false, int leagueId = 0, string betType = _StringEmpty, string sportType = _StringEmpty, string betTeam = _StringEmpty, int matchId = 0)
        {
            // List ticket
            var betListService = new BetListService();
            if (toDate == null)
            {
                toDate = fromDate;
            }
            var criteria = new WagersCriteria
            {
                LoggedId = loggedId > 0 ? loggedId : CurrentSession.AgentId,
                FromDate = fromDate,
                ToDate = toDate,
                UGroup = uGroup,
                IsDirectMem = isDirectMem,
                LiveIndicator = liveIndicator,
                LeagueId = leagueId,
                SportTypes = string.IsNullOrEmpty(sportType) ? "0" : sportType,
                BetTypes = string.IsNullOrEmpty(betType) ? "0" : betType,
                BetTeam = betTeam,
                MatchId = matchId,
                IsSuperView = CurrentSession.IsSuper
            };

            TicketResult mainTickets = betListService.WagersTicket(criteria);

            var viewModel = new BetListViewModel
            {
                TicketList = this.Ticket(mainTickets, BetListPage.Wager),
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// Action MixParlay
        /// </summary>
        /// <param name="transid"></param>
        /// <returns></returns>
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
        /// Combination MixParlay
        /// </summary>
        /// <param name="refNo"></param>
        /// <param name="winlossDate"></param>
        /// <param name="transId"></param>
        /// <param name="betId"></param>
        /// <param name="custId"></param>
        /// <returns></returns>
        public ViewResult CombinationMixParlay(string refNo, DateTime winlossDate, int transId, int betId, int custId)
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
                if (ticket.AgentLevel == 0 && CurrentSession.AgentLevel != 0)
                {
                    ticket.AgentLevel = CurrentSession.AgentLevel;
                }

                var ticketText = new TicketText
                {
                    TransId = ticket.TransId,
                    UserName = ticket.UserName,
                    TransTime = builder.BuildTime(ticket, betlistPage),
                    Discount = builder.BuildDiscount(ticket, betlistPage),
                    GameType = builder.BuildTrans(ticket, ticketHelper, funcGetOddsTypeName, betlistPage),
                    Choice = builder.BuildChoice(ticket, ticketHelper, ticketResult.TicketDataList, false, betlistPage),
                    Stake = ticket.Stake,
                    Status = builder.BuildStatus(ticket, ticketResult.TicketDataList),
                    AgentCommValue = ticket.AComm,
                    AgentComm = builder.BuildAgentComm(ticket),
                    AgentShare = builder.BuildAgentShare(ticket, null)
                };

                string ticketBetStatus = string.IsNullOrEmpty(ticket.Status) ? string.Empty : ticket.Status.ToLower();
                if (ticketBetStatus == Fanex.BetList.Core.Entities.BetStatus.Won
                    || ticketBetStatus == Fanex.BetList.Core.Entities.BetStatus.Draw
                    || ticketBetStatus == Fanex.BetList.Core.Entities.BetStatus.Lose
                    || (ticket.StatusId != null && ticket.StatusId.Value > 110 && ticket.StatusId.Value < 114))
                {
                    ticketText.Result = ticket.Winlost + ticket.PlayerComm;
                    ticketText.AgentWinlossValue = ticket.AWinlost;
                }
                else
                {
                    ticketText.Result = 0;
                    ticketText.AgentWinlossValue = 0;
                }

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
                if (ticket.AgentLevel == 0 && CurrentSession.AgentLevel != 0)
                {
                    ticket.AgentLevel = CurrentSession.AgentLevel;
                }

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
            mainShareHolderId = agentId;
            return new List<Agent>();
        }

        /// <summary>
        /// Time start date
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DateTime ResetStartDate(DateTime dt)
        {
            return dt.AddMilliseconds(-(((dt.Hour * 60 + dt.Minute) * 60 + dt.Second) * 1000 + dt.Millisecond));
        }

        /// <summary>
        /// Time end date
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DateTime ResetEndDate(DateTime dt)
        {
            return ResetStartDate(dt).Add(new TimeSpan(0, 23, 59, 59, 000));
        }

        #endregion Format Start, End date
    }
}