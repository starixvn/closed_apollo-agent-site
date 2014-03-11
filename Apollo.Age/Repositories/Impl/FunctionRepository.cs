// --------------------------------------------------------------------------
// <copyright file="FunctionRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.2">Created</change>
// </history>

namespace Apollo.Age.Repositories.Impl
{
    using System.Collections.Generic;
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Interface;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Function repository.
    /// </summary>
    public class FunctionRepository : IFunctionRepository
    {
        /// <summary>
        /// Gets all functions.
        /// </summary>
        /// <param name="criteria">The criteria get functions.</param>
        /// <returns>Return list function.</returns>
        public IEnumerable<Function> GetAll(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    return objectDb.Query<Function, FunctionGroup, Function>(
                        (f, fg) => { f.FunctionGroup = fg; return f; },
                        "FunctionGroupId",
                        criteria.GetSpParams());
                }
            }

            return null;
        }
    }
}
