// --------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
//   <change who="Lewis.Le" date="2013.5.3">Added AuthenticateNapUser</change>
// </history>

namespace Apollo.Age.Services.Interface
{
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;

    public interface IUserService
    {
        User GetUserById(int userId);
        
        User GetUserByUsername(string username);

        /// <summary>
        /// Validates a username and password against credentials stored in NAP database
        /// </summary>
        bool AuthenticateNapUser(AuthenticateNapUserCriteria criteria);
    }
}