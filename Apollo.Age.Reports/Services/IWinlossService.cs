// --------------------------------------------------------------------------
// <copyright file="IWinlossService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.11">Created</change>
// </history>

namespace Apollo.Age.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using Apollo.Age.Reports.Models;

    public interface IWinlossService
    {
        List<MatchEnteredModel> GetMatchEntered();

        List<AgentWinlossModel> GetAgentWinloss(int agentId, DateTime fromDate, DateTime toDate);

        List<MemberWinlossModel> GetMemberWinloss(int agentId, DateTime fromDate, DateTime toDate);
    }
}
