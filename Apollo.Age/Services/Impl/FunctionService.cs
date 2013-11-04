// --------------------------------------------------------------------------
// <copyright file="FunctionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.2">Created</change>
// </history>

namespace Apollo.Age.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using Apollo.Age.Entities;
    using Apollo.Age.PermissionControl.Criterias;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Services.Interface;

    /// <summary>
    /// Function service.
    /// </summary>
    public class FunctionService : ServiceBase, IFunctionService
    {
        private readonly IFunctionRepository _functionRepository;

        public FunctionService()
            : this(null)
        {
        }

        public FunctionService(IFunctionRepository functionRepository)
        {
            _functionRepository = functionRepository ?? new FunctionRepository();
        }

        /// <summary>
        /// Gets all function.
        /// </summary>
        /// <param name="functionGroupId">The function group identifier.</param>
        /// <param name="isActive">The is active.</param>
        /// <returns>Return list functions.</returns>
        public IEnumerable<Function> GetAll(short functionGroupId = 0, bool? isActive = null)
        {
            try
            {
                var criteria = new FunctionGetAllCriteria
                {
                    FunctionGroupId = functionGroupId,
                    IsActive = isActive
                };

                return _functionRepository.GetAll(criteria);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }
    }
}
