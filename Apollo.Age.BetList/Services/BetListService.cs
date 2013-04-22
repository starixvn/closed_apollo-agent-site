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

    /// <summary>
    /// BetList Service
    /// </summary>
    public class BetListService : QueryService, IBetListService
    {
        private IBetListRepository _betListRepository { get; set; }

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
        /// Get detail agent PT and agent Comm
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IList<ITicket> GetDetailAgentComm(BetListCriteria criteria)
        {
            return this._betListRepository.GetDetailAgentComm(criteria);
        }

        /// <summary>
        /// Get betlist running on Total Bets menu
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public TicketResult GetRunningByBettype(BetListCriteria criteria)
        {
            return this._betListRepository.GetBetListData(criteria, "Age_BetList_TotalBets_SelRunningByBettype");
        }

        /// <summary>
        /// Get all bets by date and member
        /// For only agent (Member Outstanding betlist)
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public TicketResult GetAllBetsByDateAndMem(BetListCriteria criteria)
        {
            return this._betListRepository.GetBetListData(criteria, "Age_BetList_SelByDateMemStatus");
        }

        /// <summary>
        /// BetList Trannsaction
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public TicketResult GetAllBetsTransactionByDateAndMem(BetListCriteria criteria)
        {
            return this._betListRepository.GetBetListData(criteria, "Age_BetList_SelTransaction");
        }

        /// <summary>
        /// Get betlist by matchId
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public TicketResult GetRunningByBettypeAndMatch(BetListCriteria criteria)
        {
            return this._betListRepository.GetRunningByBettypeAndMatch(criteria);
        }

        /// <summary>
        /// Get UserId By Username
        /// </summary>
        /// <param name="loggedId"></param>
        /// <param name="isAgent"></param>
        /// <param name="username"></param>
        /// <param name="agentId"></param>
        /// <param name="custId"></param>
        /// <param name="errorCode"></param>
        public void GetUserIdByUserName(int loggedId, bool isAgent, string username, out int agentId, out int custId, out int errorCode)
        {
            this._betListRepository.GetUserIdByUserName(loggedId, isAgent, username, out agentId, out custId, out errorCode);
        }
    }
}