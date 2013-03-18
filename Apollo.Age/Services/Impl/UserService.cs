// -----------------------------------------------------------------------
// <copyright file="UserService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Apollo.Age.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Services.Interface;
    using Fanex.Common.Helper;
    using Apollo.Age.Models;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Repositories.Impl;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository = null)
        {
            _userRepository = userRepository ?? new UserRepository();
        }

        public User GetUserById(int userId)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            throw new NotImplementedException();
        }

        public User GetUserByUsername(string username)
        {
            Check.Argument.IsNotNull(username, "username");
            return new User();
        }
    }
}
