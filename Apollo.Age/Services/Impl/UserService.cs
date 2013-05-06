// --------------------------------------------------------------------------
// <copyright file="UserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="???" date="???">Created</change>
//   <change who="Lewis.Le" date="2013.5.3">Added AuthenticateNapUser</change>
// </history>

namespace Apollo.Age.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Services.Interface;
    using Fanex.Common.Helper;
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Criterias;
    using Fanex.Common;

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
            User user = null;
            Check.Argument.IsNotNull(username, "username");

            try
            {
                user = _userRepository.Get(new UserGetByUsernameCriteria
                     {
                         Username = username
                     });
            }
            catch (Exception ex)
            { 
                //writelog
            }

            return user;
        }

        public bool AuthenticateNapUser(AuthenticateNapUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            criteria.Password = Cryptography.CfsEnCode(criteria.Password);

            return _userRepository.AuthenticateNapUser(criteria);
        }
    }
}