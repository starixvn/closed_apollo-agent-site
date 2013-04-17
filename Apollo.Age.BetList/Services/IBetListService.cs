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
    using System;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Models;

    internal interface IBetListService
    {
        TicketResult GetDetailAgentComm(BetListCriteria criteria);
    }
}