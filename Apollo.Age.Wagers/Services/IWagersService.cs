// --------------------------------------------------------------------------
// <copyright file="IWagerService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.6.28">Created</change>
// </history>

namespace Apollo.Age.Wagers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IWagersService
    {
        IEnumerable<League> GetLeague(string sportType);
    }
}
