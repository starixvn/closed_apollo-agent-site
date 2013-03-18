// --------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
// </history>


namespace Apollo.Age.Services.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IUserService
    {
        User GetUserById(int userId);
        User GetUserByUsername(string username);
    }
}
