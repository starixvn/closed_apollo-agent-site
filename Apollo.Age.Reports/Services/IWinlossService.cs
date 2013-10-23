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
        /// <summary>
        /// Gets the match entered.
        /// </summary>
        /// <param name="isShowDataTest">Include data test or not.</param>
        /// <returns>List of match entered.</returns>
        List<MatchEnteredModel> GetMatchEntered(bool isShowDataTest);

        /// <summary>
        /// Gets the agent win loss.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of agent win loss.</returns>
        List<AgentWinLossModel> GetAgentWinLoss(AgentWinLossCriteria criteria);

        /// <summary>
        /// Gets the member win loss.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of member win loss.</returns>
        List<MemberWinLossModel> GetMemberWinLoss(MemberWinLossCriteria criteria);

        /// <summary>
        /// Exports the agent win loss.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Agent win loss workbook.</returns>
        HSSFWorkbook ExportAgentWinLoss(AgentWinLossViewModel model);

        /// <summary>
        /// Exports the member win loss.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Member win loss workbook.</returns>
        HSSFWorkbook ExportMemberWinLoss(MemberWinLossViewModel model);

        /// <summary>
        /// Shorts the date format.
        /// </summary>
        /// <param name="dt">The date need to format.</param>
        /// <returns>Date formatted.</returns>
        string ShortDateFormat(object dt);
    }
}