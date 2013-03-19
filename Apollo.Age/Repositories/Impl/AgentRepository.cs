// --------------------------------------------------------------------------
// <copyright file="AgentRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
// </history>

namespace Apollo.Age.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Entities;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentRepository : IAgentRepository
    {
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

        public int Count(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Agent> Fetch(ICriteria criteria, Action<Orderable<Agent>> order)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Agent> Fetch(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Agent> Fetch()
        {
            throw new NotImplementedException();
        }

        public Agent Get(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public Agent Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Length(dynamic paramaters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Agent> Query(dynamic parameters)
        {
            throw new NotImplementedException();
        }

        public Agent Single(dynamic parameters)
        {
            throw new NotImplementedException();
        }
    }
}
