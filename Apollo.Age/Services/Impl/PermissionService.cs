// --------------------------------------------------------------------------
// <copyright file="PermissionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.29">Created</change>
// </history>

namespace Apollo.Age.Services.Impl
{
    using System;
    using Apollo.Age.Criterias.Accounts;    
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Fanex.Logging;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Criterias;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class PermissionService
    {
        private static IUserRepository _userRepository;

        static PermissionService()
        {
            _userRepository = new UserRepository();
        }

    }
}