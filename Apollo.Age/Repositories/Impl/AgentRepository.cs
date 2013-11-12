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
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Interface;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// This class for retrieve agent.
    /// </summary>
    public class AgentRepository : RepositoryBase<Agent>, IAgentRepository
    {
        /// <summary>
        /// Query agent from criteria.
        /// </summary>
        /// <param name="criteria">Input a criteria.</param>
        /// <returns>Return list of agent.</returns>
        public override Agent Get(ICriteria criteria)
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

        /// <summary>
        /// Delete action from command result.
        /// </summary>
        /// <param name="entity">Input a agent.</param>
        /// <returns>Return result.</returns>
        public ICommandResult Delete(Agent entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert action from command result.
        /// </summary>
        /// <param name="entity">Input a agent.</param>
        /// <returns>Return result.</returns>
        public ICommandResult Insert(Agent entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update action from command result.
        /// </summary>
        /// <param name="entity">Input a agent.</param>
        /// <returns>Return result.</returns>
        public ICommandResult Update(Agent entity)
        {
            throw new NotImplementedException();
        }
    }
}