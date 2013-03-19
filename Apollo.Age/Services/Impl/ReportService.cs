// --------------------------------------------------------------------------
// <copyright file="ReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.19">Created</change>
// </history>

namespace Apollo.Age.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Services.Interface;
    using Fanex.NAP.Framework.Data;
    using Apollo.Age.DTO.Reports;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReportService : ReportServiceBase, IReportService
    {
        public List<AgentWinlossDTO> GetAgentWinloss(ICriteria criteria)
        {
            IEnumerable<AgentWinlossDTO> reportData = null;

            try
            {
                if (criteria.IsValid())
                {
                    reportData = this.GetReportData<AgentWinlossDTO>(criteria);
                    // Do not forget create sum dto
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return (ReferenceEquals(reportData, null) ? new List<AgentWinlossDTO>() : reportData.ToList());
        }

        public List<MemberWinlossDTO> GetMemberWilossDetail(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public List<WinLossByBetTypeDTO> GetWinLossByBetType(ICriteria crteria)
        {
            throw new NotImplementedException();
        }
    }
}
