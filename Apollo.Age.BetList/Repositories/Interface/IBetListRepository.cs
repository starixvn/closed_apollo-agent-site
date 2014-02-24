// --------------------------------------------------------------------------
// <copyright file="IBetListRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.17">Created and Implement</change>
//   <change who="Kaden.Trinh" date="2013.05.20">Add more function get agent upline of member</change>
// </history>

namespace Apollo.Age.BetList.Repositories.Interface
{
    using System.Collections.Generic;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.Entities;
    using Apollo.BetList.Core.Entities;

    public interface IBetListRepository
    {
        /// <summary>
        /// Gets the detail agent commission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of tickets.</returns>
        IList<ITicket> GetDetailAgentComm(BetListCriteria criteria);

        /// <summary>
        /// Gets the detail win loss agent commission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of tickets.</returns>
        IList<ITicket> GetDetailWinLossAgentComm(BetListCriteria criteria);

        /// <summary>
        /// Gets the mix parlay detail ticket.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of tickets.</returns>
        IList<ITicket> GetMixParlayDetailTicket(BetListCriteria criteria);

        /// <summary>
        /// Wagers the ticket.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The ticket result.</returns>
        TicketResult WagersTicket(WagersCriteria criteria);

        /// <summary>
        /// Gets the ticket result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The ticket result.</returns>
        TicketResult GetTicketResult(BetListCriteria criteria);

        /// <summary>
        /// Gets the ticket result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="isIncludeOldData">If set to <c>true</c> [is include old data].</param>
        /// <returns>The ticket result.</returns>
        TicketResult GetTicketResult(BetListCriteria criteria, bool isIncludeOldData = false);

        /// <summary>
        /// Gets the running by bet type.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The ticket result.</returns>
        TicketResult GetRunningByBettype(BetListCriteria criteria);

        /// <summary>
        /// Gets the win loss by date and memory.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The ticket result.</returns>
        TicketResult GetWinLossByDateAndMem(BetListCriteria criteria);

        /// <summary>
        /// Gets the running by bet type and match.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The ticket result.</returns>
        TicketResult GetRunningByBettypeAndMatch(BetListCriteria criteria);

        /// <summary>
        /// Gets the combination mix parlay detail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System parlay data.</returns>
        SystemParlayData GetCombinationMixParlayDetail(SystemParlayDetailCriteria criteria);

        /// <summary>
        /// Gets the name of the user identifier by user.
        /// </summary>
        /// <param name="loggedId">The logged identifier.</param>
        /// <param name="isAgent">If set to <c>true</c> [is agent].</param>
        /// <param name="username">The username.</param>
        /// <param name="agentId">The agent identifier.</param>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="errorCode">The error code.</param>
        void GetUserIdByUserName(int loggedId, bool isAgent, string username, out int agentId, out int custId, out int errorCode);

        /// <summary>
        /// Gets the agent up line of member.
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>List of agents.</returns>
        IEnumerable<Agent> GetAgentUpLineOfMember(int parentId, int custId);

        /// <summary>
        /// Gets the account information ticket.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The ticket result.</returns>
        TicketResult GetAccountInfoTicket(AccountInfoBetlistCriteria criteria);

        /// <summary>
        /// Gets the out standing ticket.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The ticket result.</returns>
        TicketResult GetOutStandingTicket(OutstandingCriteria criteria);

        /// <summary>
        /// Gets the cancelled ticket.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The ticket result.</returns>
        TicketResult GetCancelledTicket(CancelledTicketCriteria criteria);

        /// <summary>
        /// Gets the cancelled old ticket.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The ticket result.</returns>
        TicketResult GetCancelledOldTicket(CancelledTicketOldDataCriteria criteria);

        /// <summary>
        /// Gets the transaction ticket.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The ticket result.</returns>
        TicketResult GetTransactionTicket(TransactionCriteria criteria);
    }
}