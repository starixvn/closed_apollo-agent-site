// --------------------------------------------------------------------------
// <copyright file="IBetListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.17">Created and Implement</change>
// </history>
namespace Apollo.Age.BetList.Services
{
    using System.Collections;
    using System.Collections.Generic;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Models;
    using Fanex.BetList.Core.Entities;

    public interface IBetListService
    {
        IList<ITicket> GetDetailAgentComm(BetListCriteria criteria);

        TicketResult GetRunningByBettype(BetListCriteria criteria);

        TicketResult GetRunningByBettypeAndMatch(BetListCriteria criteria);

        TicketResult GetAllBetsByDateAndMem(BetListCriteria criteria);

        TicketResult GetAllBetsTransactionByDateAndMem(BetListCriteria criteria);

        void GetUserIdByUserName(int loggedId, bool isAgent, string username, out int agentId, out int custId, out int errorCode);
    }
}