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
    using System.Collections.Generic;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.ViewModels;
    using NPOI.HSSF.UserModel;

    public interface IWinLossService
    {
        List<MatchEnteredModel> GetMatchEntered(bool isShowDataTest);

        List<AgentWinLossModel> GetAgentWinLoss(AgentWinLossCriteria criteria);

        List<MemberWinLossModel> GetMemberWinLoss(MemberWinLossCriteria criteria);

        HSSFWorkbook ExportAgentWinLoss(AgentWinLossViewModel model);

        HSSFWorkbook ExportMemberWinLoss(MemberWinLossViewModel model);

        string ShortDateFormat(object dt);
    }
}
