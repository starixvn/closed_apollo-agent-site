// --------------------------------------------------------------------------
// <copyright file="AgentRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
//   <change who="Anders.Le" date="2013.3.26">Apply generic repository</change>
// </history>

namespace Apollo.Age.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Interface;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentRepository : RepositoryBase<Agent>, IAgentRepository
    {
        public override Agent Get(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.QueryEntity<Agent, Agent, Agent>(
                        (f, w) =>
                        {
                            f.Agent = w;
                            return f;
                        },
                        "ParentId",
                        criteria.GetSpParams());
                }
            }
            catch
            {
                throw;
            }
        }

        public bool IsExistValueSearch(ICriteria criteria)
        {
            bool result = false;
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    result = (bool)db.ExecuteScalar(criteria.GetSpParams());
                }
            }
            catch 
            {
                throw;
            }

            return result;
        }

        public ICommandResult Delete(Agent entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Insert(Agent entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Update(Agent entity)
        {
            throw new NotImplementedException();
        }      
    }
}
