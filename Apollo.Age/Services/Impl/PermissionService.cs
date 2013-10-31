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
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;

    /// <summary>
    /// Permission service.
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