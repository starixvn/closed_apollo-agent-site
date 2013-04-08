namespace Apollo.Age.Account.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Repositories.Interface;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    public class AgentListRepository : RepositoryBase<AgentListViewModel>, IAgentListRepository
    {
        public IEnumerable<AgentListViewModel> GetData(ref AgentListCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var param = criteria.GetSpParams();
                        var list = db.Query<AgentListViewModel>(param);

                        // get output parameter
                        criteria.TotalPages = Convert.ToInt32(param["totalpages"]);
                        return list;
                    }
                }

                return new List<AgentListViewModel>();
            }
            catch
            {
                throw;
            }
        }

        public ICommandResult Delete(AgentListViewModel entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Insert(AgentListViewModel entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Update(AgentListViewModel entity)
        {
            throw new NotImplementedException();
        }
    }
}