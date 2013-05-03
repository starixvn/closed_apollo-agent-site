// --------------------------------------------------------------------------
// <copyright file="ISportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.29">Created</change>
// </history>

namespace Apollo.Age.Services.Interface
{
    using System.Collections.Generic;
    using Apollo.Age.Entities;

    public interface ISportService
    {
        List<Sport> GetAllSportType();

        List<BetType> GetBetTypeBySport(short sportId, bool isMixSportParlay);
    }
}
