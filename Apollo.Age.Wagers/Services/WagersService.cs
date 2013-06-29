// --------------------------------------------------------------------------
// <copyright file="WagersService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.6.28">Created</change>
// </history>

namespace Apollo.Age.Wagers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Entities;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Wagers.Criterias;
using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WagersService : QueryService, IWagersService
    {
        public IEnumerable<League> GetLeague(DateTime fromDate, string sportTypes)
        {
            var criteria = new GetLeagueCriteria
            {
                FromDate = fromDate,
                SportTypes = sportTypes
            };

            return this.GetData<League>(criteria);
        }

        public IEnumerable<T> GetWagerData<T>(ICriteria criteria)
        {
            return this.GetData<T>(criteria);
        }
    }
}
