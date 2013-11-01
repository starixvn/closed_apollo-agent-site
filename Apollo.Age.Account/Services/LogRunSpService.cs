// --------------------------------------------------------------------------
// <copyright file="LogRunSpService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.5.16">Created</change>
// </history>

namespace Apollo.Age.Account.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Services.Impl;
    using Fanex.Data;

    /// <summary>
    /// Execute SP log service.
    /// </summary>
    public class LogRunSpService : QueryService, ILogRunSpService
    {
        /// <summary>
        /// Search execute SP log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of execute SP log model.</returns>
        public IEnumerable<LogRunSpModel> Search(ref GetLogCustSettingCriteria criteria)
        {
            IEnumerable<LogRunSpModel> list = null;

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();

                list = objectDb.Query<LogRunSpModel>(param);

                criteria.TotalRows = (int)param["totalrows"];
            }

            return list;
        }

        public string RevertGroupName(string group)
        {
            return base.RevertCommGroupName(group);
        }
    }
}
