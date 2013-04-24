// --------------------------------------------------------------------------
// <copyright file="IBetListRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.17">Created and Implement</change>
// </history>

namespace Apollo.Age.BetList.Repositories.Interface
{
    using System.Collections;
    using System.Collections.Generic;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Models;
    using Fanex.BetList.Core.Entities;

    public interface IBetListRepository
    {
        IList<ITicket> GetDetailAgentComm(BetListCriteria criteria);

        IList<ITicket> GetMixParlayDetailTicket(BetListCriteria criteria);

        TicketResult GetBetListData(BetListCriteria criteria, string uspKey);

        TicketResult GetWinLossByDateAndMem(BetListCriteria criteria);

        TicketResult GetRunningByBettypeAndMatch(BetListCriteria criteria);

        void GetUserIdByUserName(int loggedId, bool isAgent, string username, out int agentId, out int custId, out int errorCode);
    }
}