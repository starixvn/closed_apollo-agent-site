// --------------------------------------------------------------------------
// <copyright file="IFunctionRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.2">Created</change>
// </history>

namespace Apollo.Age.Repositories.Interface
{
    using System.Collections.Generic;
    using Apollo.Age.Entities;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IFunctionRepository
    {
        /// <summary>
        /// Gets all functions.
        /// </summary>
        /// <param name="criteria">The criteria get functions.</param>
        /// <returns>Return list function.</returns>
        IEnumerable<Function> GetAll(ICriteria criteria);
    }
}
