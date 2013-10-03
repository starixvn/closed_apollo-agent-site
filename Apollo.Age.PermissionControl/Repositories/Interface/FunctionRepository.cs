// --------------------------------------------------------------------------
// <copyright file="FunctionRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.2">Created</change>
// </history>

namespace Apollo.Age.PermissionControl.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using Apollo.Age.PermissionControl.Entities;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
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

            try
            {
                if (criteria.IsValid())
                {
                    using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return objectDb.Query<Function, FunctionGroup, Function>(
                            (f, fg) => { f.FunctionGroup = fg; return f; },
                            "FunctionGroupId",
                            criteria.GetSpParams());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }
    }
}
