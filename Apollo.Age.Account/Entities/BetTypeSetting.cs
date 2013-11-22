// --------------------------------------------------------------------------
// <copyright file="BetTypeSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.5.7">Created</change>
// </history>

namespace Apollo.Age.Account.Entities
{
    /// <summary>
    /// Bet type setting.
    /// </summary>
    public class BetTypeSetting
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsLive { get; set; }

        public bool IsDead { get; set; }

        public string ResourceKey { get; set; }
    }
}
