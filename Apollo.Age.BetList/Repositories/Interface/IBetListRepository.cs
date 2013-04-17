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
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Models;

    public interface IBetListRepository
    {
        TicketResult GetDetailAgentComm(BetListCriteria criteria);
    }
}