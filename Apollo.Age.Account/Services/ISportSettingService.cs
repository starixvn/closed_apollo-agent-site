// --------------------------------------------------------------------------
// <copyright file="ISportSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.5.7">Created</change>
// </history>

namespace Apollo.Age.Account.Services
{
    using System.Collections.Generic;
    using Apollo.Age.Account.Entities;

    public interface ISportSettingService
    {
        List<SportSetting> GetAllSportType();

        List<BetTypeSetting> GetBetTypeBySport(short sportId, bool mixSportParlay);
    }
}
