// --------------------------------------------------------------------------
// <copyright file="SportSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.5.21">Created</change>
// </history>

namespace Apollo.Age.Account.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Sport setting.
    /// </summary>
    public class SportSetting
    {
        public short Id { get; set; }

        public string Name { get; set; }

        public bool IsMixSportParlay { get; set; }

        public string ResourceKey { get; set; }

        public List<BetTypeSetting> BetTypeList { get; set; }
    }
}
