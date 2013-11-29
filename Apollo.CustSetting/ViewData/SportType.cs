// --------------------------------------------------------------------------
// <copyright file="SportType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.1">Created</change>
// </history>

namespace Apollo.CustSetting.ViewData
{
    using System.Collections.Generic;

    /// <summary>
    /// Class name.
    /// </summary>
    public class SportType
    {
        public SportType()
        {
            BetTypeGroups = new List<BetTypeGroup>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string ResourceKey { get; set; }

        public bool IsLive { get; set; }

        public bool Collapsed { get; set; }

        public IList<BetTypeGroup> BetTypeGroups { get; set; }
    }
}