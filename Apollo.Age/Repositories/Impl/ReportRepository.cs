namespace Apollo.Age.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.DTO.Reports;
    using Fanex.NAP.Framework.Data;
    using Fanex.Data;
    using Apollo.Age.Entities;

    public class ReportRepository : IReportRepository
    {
        public ICommandResult Delete(AgentWinlossDTO entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Insert(AgentWinlossDTO entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Update(AgentWinlossDTO entity)
        {
            throw new NotImplementedException();
        }

        public int Count(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AgentWinlossDTO> Fetch(ICriteria criteria, Action<Orderable<AgentWinlossDTO>> order)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AgentWinlossDTO> Fetch(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.Query<AgentWinlossDTO, WinlossComm, AgentWinlossDTO>(
                        (f, w) =>
                        {
                            f.Agent = w;
                            return f;
                        },
                        "Winloss",
                        criteria.GetSpParams()).OrderBy(f => f.UserName).ToList<AgentWinlossDTO>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<AgentWinlossDTO> Fetch()
        {
            throw new NotImplementedException();
        }

        public AgentWinlossDTO Get(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public AgentWinlossDTO Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Length(dynamic paramaters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AgentWinlossDTO> Query(dynamic parameters)
        {
            throw new NotImplementedException();
        }

        public AgentWinlossDTO Single(dynamic parameters)
        {
            throw new NotImplementedException();
        }

        
    }
}
