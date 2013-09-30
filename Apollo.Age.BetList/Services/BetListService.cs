// --------------------------------------------------------------------------
// <copyright file="BetListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">Created and Implement</change>
//   <change who="Kaden.Trinh" date="2013.04.17">Implement function get detail agentComm for Popup</change>
//   <change who="Kaden.Trinh" date="2013.05.20">Add more function get agent upline of member</change>
// </history>

namespace Apollo.Age.BetList.Services
{
    using System.Collections.Generic;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Repositories.Impl;
    using Apollo.Age.BetList.Repositories.Interface;
    using Apollo.Age.Entities;
    using Apollo.Age.Services.Impl;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// BetList Service
    /// </summary>
    public class BetListService : QueryService, IBetListService
    {
        private IBetListRepository _betListRepository;

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
            this._betListRepository = this._betListRepository ?? new BetListRepository();
        }

        /// <summary>
        /// Get detail agent PT and agent commission
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>List Ticket</returns>
        public IList<ITicket> GetDetailAgentComm(BetListCriteria criteria)
        {
            return this._betListRepository.GetDetailAgentComm(criteria);
        }

        /// <summary>
        /// Get detail win loss agent PT and agent commission
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>List Ticket</returns>
        public IList<ITicket> GetDetailWinLossAgentComm(BetListCriteria criteria)
        {
            return this._betListRepository.GetDetailWinLossAgentComm(criteria);
        }

        /// <summary>
        /// Get detail ticket mix-parlay
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>Ticket data</returns>
        public IList<ITicket> GetMixParlayDetailTicket(BetListCriteria criteria)
        {
            return this._betListRepository.GetMixParlayDetailTicket(criteria);
        }

        /// <summary>
        /// Get bet list running on Total Bets menu
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>Ticket data</returns>
        public TicketResult GetRunningByBettype(BetListCriteria criteria)
        {
            return this._betListRepository.GetRunningByBettype(criteria);
        }

        /// <summary>
        /// Get all bets by date and member from Win loss listOutstanding
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>Ticket data</returns>
        public TicketResult GetWinLossByDateAndMem(BetListCriteria criteria)
        {
            return this._betListRepository.GetWinLossByDateAndMem(criteria);
        }

        /// <summary>
        /// Get bet list by matchId
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>Ticket data</returns>
        public TicketResult GetRunningByBettypeAndMatch(BetListCriteria criteria)
        {
            return this._betListRepository.GetRunningByBettypeAndMatch(criteria);
        }

        /// <summary>
        /// Get Combination Mix Parlay Detail
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>SystemParlay data</returns>
        public SystemParlayData GetCombinationMixParlayDetail(SystemParlayDetailCriteria criteria)
        {
            return this._betListRepository.GetCombinationMixParlayDetail(criteria);
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
            this._betListRepository.GetUserIdByUserName(loggedId, isAgent, username, out agentId, out custId, out errorCode);
        }

        /// <summary>
        /// Get Agent up line of member
        /// </summary>
        /// <param name="parentId">Logged id</param>
        /// <param name="custId">customer id</param>
        /// <returns></returns>
        public IEnumerable<Agent> GetAgentUpLineOfMember(int parentId, int custId)
        {
            return this._betListRepository.GetAgentUpLineOfMember(parentId, custId);
        }

        /// <summary>
        /// Get data betlist on Wagers page
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public TicketResult WagersTicket(WagersCriteria criteria)
        {
            criteria.UGroup = base.RevertCommGroupName(criteria.UGroup);
            return this._betListRepository.WagersTicket(criteria);
        }

        public TicketResult GetTicketResult(BetListCriteria criteria)
        {
            return this._betListRepository.GetTicketResult(criteria);
        }
    }
}