// --------------------------------------------------------------------------
// <copyright file="ReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.19">Created</change>
// </history>

namespace Apollo.Age.Services.Interface
{
    using System.Collections.Generic;
    using Apollo.Age.DTO.Reports;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IReportService
    {
        List<AgentWinlossDTO> GetAgentWinloss(ICriteria criteria);

        List<MemberWinlossDTO> GetMemberWilossDetail(ICriteria criteria);

        List<WinLossByBetTypeDTO> GetWinLossByBetType(ICriteria criteria);
    }
}
