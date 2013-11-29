// --------------------------------------------------------------------------
// <copyright file="BetTypeGroup.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.1">Created</change>
// </history>

namespace Apollo.CustSetting.ViewData
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Class name.
    /// </summary>
    public class BetTypeGroup
    {
        public BetTypeGroup()
        {
            BetTypes = new List<BetType>();
        }

        public int? Id { get; set; }

        public IList<BetType> BetTypes { get; set; }
    }
}