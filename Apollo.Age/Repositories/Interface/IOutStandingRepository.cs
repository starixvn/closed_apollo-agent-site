namespace Apollo.Age.Repositories.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Criterias.Reports.OutStandings;
    using Apollo.Age.DTO.Reports;
    using Fanex.NAP.Framework.Data;

    public interface IOutStandingRepository : IRepository<OutStandingReportDTO>
    {
        IEnumerable<OutStandingReportDTO> GetOutStanding(ICriteria criteria);
    }
}