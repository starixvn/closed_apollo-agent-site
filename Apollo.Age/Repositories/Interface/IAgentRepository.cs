// --------------------------------------------------------------------------
// <copyright file="IAgentRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
// </history>


namespace Apollo.Age.Repositories.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Entities;
    using Fanex.NAP.Framework.Data;
    

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAgentRepository : IRepository<Agent>
    {
        bool IsExistValueSearch(ICriteria criteria);
    }
}
