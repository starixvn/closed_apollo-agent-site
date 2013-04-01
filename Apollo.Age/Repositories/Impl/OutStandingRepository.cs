namespace Apollo.Age.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.DTO.Reports;
    using Apollo.Age.Repositories.Interface;
    using Criterias.Reports.OutStandings;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    public class OutStandingRepository : RepositoryBase<OutStandingReportDTO>, IOutStandingRepository
    {
        public IEnumerable<OutStandingReportDTO> GetOutStanding(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var param = criteria.GetSpParams();
                        var list = db.Query<OutStandingReportDTO>(param);

                        return list;
                    }
                }

                return new List<OutStandingReportDTO>();
            }
            catch
            {
                throw;
            }
        }

        public ICommandResult Delete(OutStandingReportDTO entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Insert(OutStandingReportDTO entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Update(OutStandingReportDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}