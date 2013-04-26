// --------------------------------------------------------------------------
// <copyright file="BetListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">Created and Implement</change>
//   <change who="Kaden.Trinh" date="2013.04.17">Implement function get detail agentComm for Popup</change>
// </history>

namespace Apollo.Age.BetList.Services
{
    using System.Collections;
    using System.Collections.Generic;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Models;
    using Apollo.Age.BetList.Repositories.Impl;
    using Apollo.Age.BetList.Repositories.Interface;
    using Apollo.Age.Services.Impl;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.DNAide;

    /// <summary>
    /// BetList Service
    /// </summary>
    public class BetListService : QueryService, IBetListService
    {
        private IBetListRepository betListRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BetListService" /> class.
        /// </summary>
        public BetListService()
            : this(null)
        {
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="BetListService" /> class.
        /// </summary>
        /// <param name="betListRepository">BetList Repository</param>
        public BetListService(IBetListRepository betListRepository)
        {
            this.betListRepository = this.betListRepository ?? new BetListRepository();
        }

        /// <summary>
        /// Get detail agent PT and agent commission
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>List Ticket</returns>
        public IList<ITicket> GetDetailAgentComm(BetListCriteria criteria)
        {
            return this.betListRepository.GetDetailAgentComm(criteria);
        }

        /// <summary>
        /// Get detail ticket mix-parlay
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>Ticket data</returns>
        public IList<ITicket> GetMixParlayDetailTicket(BetListCriteria criteria)
        {
            return this.betListRepository.GetMixParlayDetailTicket(criteria);
        }

        /// <summary>
        /// Get bet list running on Total Bets menu
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>Ticket data</returns>
        public TicketResult GetRunningByBettype(BetListCriteria criteria)
        {
            return this.betListRepository.GetBetListData(criteria, "Age_BetList_TotalBets_SelRunningByBettype");
        }

        /// <summary>
        /// Get all bets by date and member
        /// For only agent (Member Outstanding bet list)
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>Ticket data</returns>
        public TicketResult GetAllBetsByDateAndMem(BetListCriteria criteria)
        {
            return this.betListRepository.GetBetListData(criteria, "Age_BetList_SelByDateMemStatus");
        }

        /// <summary>
        /// Get all bets by date and member from Win loss report
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>Ticket data</returns>
        public TicketResult GetWinLossByDateAndMem(BetListCriteria criteria)
        {
            return this.betListRepository.GetWinLossByDateAndMem(criteria);
        }

        /// <summary>
        /// BetList transaction
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>Ticket data</returns>
        public TicketResult GetAllBetsTransactionByDateAndMem(BetListCriteria criteria)
        {
            return this.betListRepository.GetBetListData(criteria, "Age_BetList_SelTransaction");
        }

        /// <summary>
        /// Get bet list by matchId
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>Ticket data</returns>
        public TicketResult GetRunningByBettypeAndMatch(BetListCriteria criteria)
        {
            return this.betListRepository.GetRunningByBettypeAndMatch(criteria);
        }

        /// <summary>
        /// Get Combination Mix Parlay Detail
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>SystemParlay data</returns>
        public SystemParlayData GetCombinationMixParlayDetail(SystemParlayDetailCriteria criteria)
        {
            return this.betListRepository.GetCombinationMixParlayDetail(criteria);
        }

        /// <summary>
        /// Get UserId By Username
        /// </summary>
        /// <param name="loggedId">Logged id</param>
        /// <param name="isAgent">Agent choice</param>
        /// <param name="username">User name</param>
        /// <param name="agentId">Output agent id</param>
        /// <param name="custId">Output customer id</param>
        /// <param name="errorCode">Output error code</param>
        public void GetUserIdByUserName(int loggedId, bool isAgent, string username, out int agentId, out int custId, out int errorCode)
        {
            this.betListRepository.GetUserIdByUserName(loggedId, isAgent, username, out agentId, out custId, out errorCode);
        }
    }
}