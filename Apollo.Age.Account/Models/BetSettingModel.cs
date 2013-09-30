// --------------------------------------------------------------------------
// <copyright file="BetSettingModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.5.16">Created</change>
// </history>

namespace Apollo.Age.Account.Models
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class BetSettingModel
    {
        public int SportType { get; set; }

        public int BetType { get; set; }

        public decimal MinBet { get; set; }

        public decimal? OldMinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal? OldMaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal? OldMaxPerMatch { get; set; }
    }
}
